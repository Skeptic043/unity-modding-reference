"""Synthetic bounded PE fixtures; no game files or assembly execution."""
import contextlib
import hashlib
import importlib.util
import io
import json
from pathlib import Path
import struct
import tempfile
import unittest

HARNESS = Path(__file__).resolve().parents[1] / "tools/inspect_managed_pe.py"
SPEC = importlib.util.spec_from_file_location("managed_pe_probe", HARNESS)
probe = importlib.util.module_from_spec(SPEC)
SPEC.loader.exec_module(probe)


def fixture(plus=False, managed=True):
    """Build every consumed header and a file-backed CLI metadata root."""
    data = bytearray(0x600)
    data[:2] = b"MZ"
    struct.pack_into("<I", data, 0x3C, 0x80)
    data[0x80:0x84] = b"PE\0\0"
    struct.pack_into("<HH", data, 0x84, 0x8664 if plus else 0x14C, 1)
    optional = 0x98
    optional_size = 240 if plus else 224
    struct.pack_into("<H", data, 0x94, optional_size)
    struct.pack_into("<H", data, optional, 0x20B if plus else 0x10B)
    struct.pack_into("<I", data, optional + 60, 0x200)
    struct.pack_into("<I", data, optional + (108 if plus else 92), 16)
    directory = optional + (112 if plus else 96) + 14 * 8
    section = optional + optional_size
    data[section:section + 8] = b".text\0\0\0"
    struct.pack_into("<IIII", data, section + 8, 0x400, 0x2000, 0x400, 0x200)
    if managed:
        struct.pack_into("<II", data, directory, 0x2000, 72)
        struct.pack_into("<IHHII", data, 0x200, 72, 2, 5, 0x2100, 0x40)
        data[0x300:0x304] = b"BSJB"
        struct.pack_into("<HHII", data, 0x304, 1, 1, 0, 12)
        data[0x310:0x31C] = b"v4.0.30319\0\0"
        # Empty stream inventory: parser intentionally does not inspect streams.
        struct.pack_into("<HH", data, 0x31C, 0, 0)
    return data


class ProbeTests(unittest.TestCase):
    def test_managed_formats(self):
        for plus in (False, True):
            with self.subTest(plus=plus):
                result = probe.inspect_bytes(fixture(plus))
                self.assertEqual(result["pe_format"], "PE32+" if plus else "PE32")
                self.assertEqual(result["pe_architecture"], "x86-64" if plus else "x86")
                self.assertEqual(result["cli_status"], "present")
                self.assertEqual(result["cli_header_rva"], 0x2000)
                self.assertEqual(result["metadata_runtime_version"], "v4.0.30319")

    def test_native_no_cli(self):
        for plus in (False, True):
            with self.subTest(plus=plus):
                result = probe.inspect_bytes(fixture(plus=plus, managed=False))
                self.assertEqual(result["cli_status"], "absent")
                self.assertIsNone(result["metadata_runtime_version"])
                self.assertIsNone(result["cli_header_rva"])

    def test_no_com_directory(self):
        data = fixture(managed=False)
        struct.pack_into("<I", data, 0x98 + 92, 14)
        self.assertEqual(probe.inspect_bytes(data)["cli_status"], "absent")

    def test_truncation(self):
        for length in (0, 2, 63, 0x90, 0x100, 0x190, 0x250, 0x5FF):
            with self.subTest(length=length), self.assertRaises(probe.InspectionError):
                probe.inspect_bytes(fixture()[:length])

    def test_bad_signatures(self):
        for offset in (0, 0x80, 0x300):
            data = fixture()
            data[offset] = 0
            with self.subTest(offset=offset), self.assertRaises(probe.InspectionError):
                probe.inspect_bytes(data)

    def test_invalid_headers_and_offsets(self):
        patches = [(0x3C, 0x10), (0x3C, 0xFFFFFFF0),
                   (0x98 + 60, 0x100), (0x98 + 60, 0x10000),
                   (0x98 + 92, 1000), (0x98 + 96 + 14 * 8, 0),
                   (0x98 + 96 + 14 * 8, 0xFFFFFFF0),
                   (0x98 + 96 + 14 * 8 + 4, 0), (0x200, 71),
                   (0x200, 73), (0x208, 0x5000), (0x20C, 15),
                   (0x20C, 0x10000), (0x178 + 20, 0x100)]
        for offset, value in patches:
            data = fixture()
            struct.pack_into("<I", data, offset, value)
            with self.subTest(offset=offset, value=value), self.assertRaises(probe.InspectionError):
                probe.inspect_bytes(data)

    def test_unsupported_or_short_optional_header(self):
        for offset, value in ((0x98, 0x999), (0x94, 0), (0x94, 95)):
            data = fixture()
            struct.pack_into("<H", data, offset, value)
            with self.subTest(offset=offset, value=value), self.assertRaises(probe.InspectionError):
                probe.inspect_bytes(data)

    def test_version_length_bounds(self):
        for length in (0, 49, 257, 0xFFFFFFFF):
            data = fixture()
            struct.pack_into("<I", data, 0x30C, length)
            with self.subTest(length=length), self.assertRaises(probe.InspectionError):
                probe.inspect_bytes(data)

    def test_invalid_version_encoding_and_termination(self):
        for version in (b"x" * 12, b"\xff\0" + b"\0" * 10):
            data = fixture()
            data[0x310:0x31C] = version
            with self.subTest(version=version), self.assertRaises(probe.InspectionError):
                probe.inspect_bytes(data)

    def test_virtual_only_data_rejected(self):
        data = fixture()
        struct.pack_into("<I", data, 0x178 + 16, 72)
        with self.assertRaisesRegex(probe.InspectionError, "unmapped"):
            probe.inspect_bytes(data)

    def test_ambiguous_rva_rejected(self):
        data = fixture()
        struct.pack_into("<H", data, 0x86, 2)
        data[0x1A0:0x1C8] = data[0x178:0x1A0]
        with self.assertRaisesRegex(probe.InspectionError, "ambiguous"):
            probe.inspect_bytes(data)

    def test_unknown_machine_retains_numeric_identity(self):
        data = fixture()
        struct.pack_into("<H", data, 0x84, 0xFFFF)
        result = probe.inspect_bytes(data)
        self.assertEqual(result["pe_architecture"], "unknown")
        self.assertEqual(result["machine"], "0xffff")

    def test_cli_success_multiple_inputs_and_identity(self):
        with tempfile.TemporaryDirectory() as directory:
            files = [Path(directory) / "managed.dll", Path(directory) / "native.exe"]
            for path, managed in zip(files, (True, False)):
                path.write_bytes(fixture(managed=managed))
            before = [path.read_bytes() for path in files]
            stdout = io.StringIO()
            with contextlib.redirect_stdout(stdout):
                self.assertEqual(probe.main([str(p) for p in files] + ["--build", "operator-label"]), 0)
            result = json.loads(stdout.getvalue())
            self.assertNotIn(directory, stdout.getvalue())
            self.assertEqual(result["operator_build_label"], "operator-label")
            self.assertEqual(result["harness_sha256"], hashlib.sha256(HARNESS.read_bytes()).hexdigest())
            self.assertTrue(result["observed_at_utc"].endswith("+00:00"))
            for path, content, entry in zip(files, before, result["files"]):
                self.assertEqual(entry["filename"], path.name)
                self.assertEqual(entry["size_bytes"], len(content))
                self.assertEqual(entry["sha256"], hashlib.sha256(content).hexdigest())
                self.assertEqual(path.read_bytes(), content)

    def test_cli_failure_is_atomic(self):
        with tempfile.TemporaryDirectory() as directory:
            valid = Path(directory) / "valid.dll"
            valid.write_bytes(fixture())
            invalid = Path(directory) / "invalid.dll"
            invalid.write_bytes(b"invalid")
            for path in (invalid, Path(directory) / "missing.dll"):
                stdout, stderr = io.StringIO(), io.StringIO()
                with contextlib.redirect_stdout(stdout), contextlib.redirect_stderr(stderr):
                    self.assertEqual(probe.main([str(valid), str(path)]), 1)
                self.assertEqual(stdout.getvalue(), "")
                self.assertIn("inspection failed:", stderr.getvalue())
                self.assertIn(path.name, stderr.getvalue())
                self.assertNotIn(directory, stderr.getvalue())

    def test_cli_argument_errors(self):
        for args in ([], ["--unknown"], ["--build"]):
            with self.subTest(args=args), contextlib.redirect_stderr(io.StringIO()):
                with self.assertRaises(SystemExit) as result:
                    probe.main(args)
                self.assertEqual(result.exception.code, 2)


if __name__ == "__main__":
    unittest.main()
