# Inspect managed PE metadata without loading an assembly

Use [inspect_managed_pe.py](../../tools/inspect_managed_pe.py) to read bounded PE and CLI metadata without loading or executing the input assembly. It reports structural identity and a metadata producer string. It does not identify the runtime a player uses or establish compatibility.

[Assembly inspection](README.md)

## Run the inspector

Python 3.10 or newer and its standard library are sufficient. From the repository checkout, substitute an input file you are allowed to inspect:

```powershell
python -B tools/inspect_managed_pe.py --build 'operator-supplied-build' 'C:\path\to\Assembly-CSharp.dll'
python -B -m unittest discover -s tests -p test_inspect_managed_pe.py -v
```

Multiple input paths are accepted. Save stdout as a JSON artifact with the research for the inspected target, and retain stderr on failure. Record the exact command, logical input paths, OS and Python versions, independently inspected distribution or build identity, and this repository's full Git revision. `--build` records only an operator-supplied label. Do not distribute proprietary inputs.

## What the report means

The report includes each file's basename, size, SHA256, PE architecture, CLI presence and metadata version when present. It also records the inspector hash and execution environment. A native PE with no CLI directory is a valid result.

Exit 0 means inspection succeeded. Malformed, truncated or unreadable inputs return 1 with a readable `inspection failed:` error on stderr and no partial JSON, even when an earlier input was valid. Missing arguments and invalid options return 2 through `argparse`. Diagnostics identify input basenames without exposing installation paths.

The parser accepts ordinary PE32 and PE32+ files up to 512 MiB with file-backed headers and sections. A present CLI header must be at least 72 bytes, and a NUL-terminated UTF-8 metadata version field must be no more than 256 bytes. The parser rejects unmapped or ambiguous RVAs.

The inspector does not validate metadata tables, signatures, IL or executability. The metadata version is a producer string. It does not establish the runtime selected by a player, Unity version, Mono or IL2CPP selection, loader compatibility, method behavior or live gameplay correctness.

## Tests and evidence limits

The [synthetic tests](../../tests/test_inspect_managed_pe.py) construct PE32 and PE32+ managed and native fixtures locally. They cover truncation, malformed signatures, headers and version fields, invalid or ambiguous mappings, input preservation, output identity and atomic failure. They require no game files.

When changing the command or its launch instructions, also invoke the executable as a subprocess with no arguments, a missing input, malformed input and valid fixtures. Imported-function tests alone do not establish operator launch behavior. Check exit status and stderr, not merely whether an output file exists.

The parser follows the [Microsoft PE Format](https://learn.microsoft.com/en-us/windows/win32/debug/pe-format) sections File Headers, Optional Header Data Directories and Section Table, and [ECMA-335, sixth edition, June 2012](https://ecma-international.org/wp-content/uploads/ECMA-335_6th_edition_june_2012.pdf), II.24.2.1 on printed page 271 and PDF page 297, and II.25.3.3 on printed page 283 and PDF page 309.

Changed inspector bytes, Python, OS or intended PE layouts warrant reinspection. Changed input hashes require a new target-specific record. Runtime questions need separate live evidence.
