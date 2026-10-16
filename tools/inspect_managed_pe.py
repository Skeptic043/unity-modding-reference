#!/usr/bin/env python3
"""Read raw PE/CLI structure without loading or executing the input.

Python 3.10+ standard library only. This is a deliberately bounded inspection
harness, not a PE verifier, assembly resolver or runtime detector. Inputs must be
ordinary PE32/PE32+ files <=512 MiB with file-backed headers/sections and, when
present, a CLI header of at least 72 bytes and a metadata version field <=256
bytes. It does not validate metadata tables, signatures, IL or executability.
The metadata runtime version is a raw producer string: it does not establish
the selected runtime/backend, Unity version or whether a game loads the file.
"""

import argparse
import hashlib
import json
import platform
import struct
import sys
from datetime import datetime, timezone
from pathlib import Path

MAX_FILE_BYTES = 512 * 1024 * 1024
MAX_VERSION_BYTES = 256


class InspectionError(ValueError):
    """Input does not fit the supported, bounded structural format."""


def inspect_bytes(data):
    """Return only observed structural fields; reject missing/out-of-file data."""
    def require(offset, size, label):
        if offset < 0 or size < 0 or offset + size > len(data):
            raise InspectionError(f"truncated or out-of-file {label}")

    def u16(offset):
        require(offset, 2, "16-bit field")
        return struct.unpack_from("<H", data, offset)[0]

    def u32(offset):
        require(offset, 4, "32-bit field")
        return struct.unpack_from("<I", data, offset)[0]

    require(0, 64, "DOS header")
    if data[:2] != b"MZ":
        raise InspectionError("missing MZ signature")
    pe = u32(0x3C)
    if pe < 64:
        raise InspectionError("PE header overlaps DOS header")
    require(pe, 24, "PE/COFF header")
    if data[pe:pe + 4] != b"PE\0\0":
        raise InspectionError("missing PE signature")
    machine, section_count = u16(pe + 4), u16(pe + 6)
    optional_size = u16(pe + 20)
    optional = pe + 24
    require(optional, optional_size, "optional header")
    if optional_size < 2:
        raise InspectionError("missing optional header magic")
    magic = u16(optional)
    if magic == 0x10B:
        pe_format, directory_offset, count_offset = "PE32", 96, 92
    elif magic == 0x20B:
        pe_format, directory_offset, count_offset = "PE32+", 112, 108
    else:
        raise InspectionError(f"unsupported optional header magic 0x{magic:04x}")
    if optional_size < directory_offset:
        raise InspectionError("optional header is too short for data directories")
    directory_count = u32(optional + count_offset)
    if directory_count > (optional_size - directory_offset) // 8:
        raise InspectionError("data directory count exceeds optional header")
    section_table = optional + optional_size
    require(section_table, section_count * 40, "section table")
    headers_size = u32(optional + 60)
    if headers_size < section_table + section_count * 40 or headers_size > len(data):
        raise InspectionError("invalid SizeOfHeaders")
    sections = []
    for index in range(section_count):
        section = section_table + index * 40
        virtual_size, rva = u32(section + 8), u32(section + 12)
        raw_size, raw_offset = u32(section + 16), u32(section + 20)
        if raw_size:
            require(raw_offset, raw_size, "section raw data")
            if raw_offset < headers_size:
                raise InspectionError("section raw data overlaps headers")
        sections.append((rva, virtual_size, raw_offset, raw_size))

    def map_rva(rva, size, label):
        if not rva or size <= 0 or rva + size > 0x100000000:
            raise InspectionError(f"invalid {label} RVA/size")
        matches = []
        if rva < headers_size and rva + size <= headers_size:
            matches.append(rva)
        for start, virtual_size, raw_offset, raw_size in sections:
            delta = rva - start
            # All bytes must exist on disk. Virtual zero-fill is not evidence.
            if delta >= 0 and delta + size <= raw_size:
                matches.append(raw_offset + delta)
        if len(matches) != 1:
            raise InspectionError(f"unmapped or ambiguous {label} RVA")
        require(matches[0], size, label)
        return matches[0]

    architecture = {0x14C: "x86", 0x8664: "x86-64", 0x1C0: "ARM",
                    0x1C4: "ARMv7", 0xAA64: "ARM64"}.get(machine, "unknown")
    result = {"pe_format": pe_format, "machine": f"0x{machine:04x}",
              "pe_architecture": architecture, "cli_header_rva": None,
              "metadata_runtime_version": None, "cli_status": "absent"}
    if directory_count <= 14:
        return result
    cli_directory = optional + directory_offset + 14 * 8
    cli_rva, cli_size = u32(cli_directory), u32(cli_directory + 4)
    if not cli_rva and not cli_size:
        return result
    if cli_size < 72:
        raise InspectionError("CLI directory is shorter than the 72-byte header")
    cli = map_rva(cli_rva, cli_size, "CLI header")
    header_size = u32(cli)
    if header_size < 72 or header_size > cli_size:
        raise InspectionError("invalid CLI header cb size")
    metadata_rva, metadata_size = u32(cli + 8), u32(cli + 12)
    if metadata_size < 16:
        raise InspectionError("metadata root is shorter than its fixed header")
    metadata = map_rva(metadata_rva, metadata_size, "metadata root")
    if data[metadata:metadata + 4] != b"BSJB":
        raise InspectionError("missing BSJB metadata signature")
    version_length = u32(metadata + 12)
    if not 0 < version_length <= MAX_VERSION_BYTES:
        raise InspectionError("metadata version length must be 1..256 bytes")
    if 16 + version_length > metadata_size:
        raise InspectionError("metadata version exceeds metadata root size")
    raw_version = data[metadata + 16:metadata + 16 + version_length]
    terminator = raw_version.find(b"\0")
    if terminator < 0:
        raise InspectionError("metadata version is not NUL-terminated")
    try:
        version = raw_version[:terminator].decode("utf-8")
    except UnicodeDecodeError as exc:
        raise InspectionError("metadata version is not valid UTF-8") from exc
    result.update(cli_status="present", cli_header_rva=cli_rva,
                  metadata_rva=metadata_rva, metadata_size=metadata_size,
                  metadata_runtime_version=version,
                  metadata_version_field_hex=raw_version.hex())
    return result


def inspect_file(path):
    """Read an owned input; output its basename, size and content fingerprint."""
    try:
        if path.stat().st_size > MAX_FILE_BYTES:
            raise InspectionError("input exceeds 512 MiB harness limit")
        with path.open("rb") as handle:
            data = handle.read(MAX_FILE_BYTES + 1)
        if len(data) > MAX_FILE_BYTES:
            raise InspectionError("input exceeds 512 MiB harness limit")
        fields = inspect_bytes(data)
    except (OSError, InspectionError) as exc:
        # Avoid leaking installation/user paths even in routine error output.
        detail = exc.strerror if isinstance(exc, OSError) else str(exc)
        raise InspectionError(f"{path.name}: {detail}") from exc
    return {"filename": path.name, "size_bytes": len(data),
            "sha256": hashlib.sha256(data).hexdigest(), **fields}


def main(argv=None):
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("files", nargs="+", type=Path, help="owned PE inputs (read only)")
    parser.add_argument("--build", help="operator-supplied build label, not detected")
    args = parser.parse_args(argv)
    try:
        files = [inspect_file(path) for path in args.files]
        harness_hash = hashlib.sha256(Path(__file__).read_bytes()).hexdigest()
    except (InspectionError, OSError) as exc:
        print(f"inspection failed: {exc}", file=sys.stderr)
        return 1
    output = {"schema_version": 1,
              "observed_at_utc": datetime.now(timezone.utc).isoformat(),
              "python_version": platform.python_version(), "os": platform.platform(),
              "harness_sha256": harness_hash, "operator_build_label": args.build,
              "files": files}
    print(json.dumps(output, indent=2, ensure_ascii=True))
    return 0


if __name__ == "__main__":
    sys.exit(main())
