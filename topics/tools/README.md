# Tools

Read-only inspection helpers and documented Unity tool routes. Each entry answers a bounded question whose evidence limits are described in the linked article.

## Managed PE metadata

[`inspect_managed_pe.py`](../../tools/inspect_managed_pe.py) reads managed assembly identity, references and selected plugin attributes without loading the assembly. See [Inspect managed PE metadata without loading the assembly](../assembly-inspection/managed-pe-metadata.md) for supported inputs and limits. Its [synthetic tests](../../tests/test_inspect_managed_pe.py) cover the maintained parser behavior.

## HarmonyX patch-processing IL

[`inspect_harmony_patch_prepare.ps1`](../../tools/inspect_harmony_patch_prepare.ps1) extracts the narrow HarmonyX patch-processing IL path used to determine when `Prepare` runs relative to target resolution. See [Run `Prepare` before target resolution](../harmonyx/prepare-before-target-resolution.md) for the exact HarmonyX version, expected symbols and interpretation.

## Windows DLL imports and exports

[`inspect_icu.py`](../../evidence/qt-freezer-icu-20261003/inspect_icu.py) compares ICU-related imports from Qt6Core with exports from candidate ICU libraries without loading them. See [Diagnose Qt freezer ICU mismatches from the build environment](../packaging/qt-freezer-icu-selection.md) for the recorded Windows, Python, Qt, PyInstaller and `pefile` versions.

## Unity CLI and Pipeline

[Unity CLI terminal automation and Pipeline editor control](unity-cli-and-pipeline.md) separates the experimental standalone CLI from the package required to control a running Unity Editor. The article records the operating-system and Editor prerequisites in Unity's official documentation as inspected on 2026-10-11.

These tools and documented routes do not prove that an assembly loads, a Harmony patch succeeds, the Windows loader accepts every dependency, Editor automation succeeds or a game behaves correctly. Keep runtime, gameplay, save and performance acceptance separate.
