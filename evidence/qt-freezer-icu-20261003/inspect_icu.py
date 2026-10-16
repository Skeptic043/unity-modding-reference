"""Read PE imports/exports without loading DLLs; requires pefile 2024.8.26."""
import argparse
import hashlib
import json
from pathlib import Path
import pefile

parser = argparse.ArgumentParser()
parser.add_argument('qt_core', type=Path)
parser.add_argument('icu_candidates', type=Path, nargs='+')
args = parser.parse_args()
pefile.MAX_SYMBOL_EXPORT_COUNT = 100000
qt = pefile.PE(str(args.qt_core))
imports = sorted({symbol.name.decode() for entry in qt.DIRECTORY_ENTRY_IMPORT
                  for symbol in entry.imports if symbol.name
                  and symbol.name.startswith((b'ucnv_', b'u_'))})
result = {'qt_sha256': hashlib.sha256(args.qt_core.read_bytes()).hexdigest(),
          'icu_imports': imports, 'candidates': []}
for candidate in args.icu_candidates:
    binary = pefile.PE(str(candidate))
    exports = {symbol.name.decode() for symbol in binary.DIRECTORY_ENTRY_EXPORT.symbols if symbol.name}
    result['candidates'].append({
        'name': candidate.name,
        'sha256': hashlib.sha256(candidate.read_bytes()).hexdigest(),
        'missing_imports': [name for name in imports if name not in exports],
        'ucnv_open_exports': sorted(name for name in exports if name.startswith('ucnv_open')),
    })
print(json.dumps(result, indent=2))
