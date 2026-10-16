# Diagnose Qt freezer ICU mismatches from the build environment

A successful freeze does not prove that the collected native libraries are compatible. In the recorded failure, PyInstaller selected an incompatible `icuuc.dll` from the build process `PATH`. The executable then failed while importing QtCore even though the application worked from source.

Inspect the freezer's dependency table and compare imported and exported symbols before changing application code.

## Bounded case study

The recorded environment was Windows 11 build 26200 x64 with Python 3.14.3, PySide6 6.11.2, PyInstaller 6.22.3 and `pyinstaller-hooks-contrib` 2026.8. The frozen executable failed on startup with:

```text
ImportError: DLL load failed while importing QtCore: The specified procedure could not be found.
```

The build's `Analysis-00.toc` selected `icuuc.dll` from a Poppler installation on the build process `PATH`. That library exports version-suffixed functions such as `ucnv_open_78`, while the inspected Qt6Core imports the unsuffixed `ucnv_open`. The inspected Windows System32 ICU satisfied all 18 ICU imports requested by that Qt6Core.

The symbol comparison found that the selected Poppler ICU was missing all 18 ICU imports requested by Qt6Core, while the inspected Windows System32 ICU provided them.

## Diagnose the selected dependency

1. Reproduce the freeze in a disposable virtual environment with exact dependency versions.
2. Inspect the freezer's analysis table for the actual source path of each collected ICU library.
3. Run the supplied [read-only PE harness](../../evidence/qt-freezer-icu-20261003/inspect_icu.py), using `pefile==2024.8.26`, against Qt6Core and each candidate ICU library.

```powershell
python inspect_icu.py path/to/Qt6Core.dll path/to/candidate/icuuc.dll
```

The harness reads imports and exports without loading the DLLs. It reports file hashes, requested ICU symbols, missing symbols for each candidate and exported `ucnv_open` variants. The DLLs themselves are not included.

4. Rebuild with the process environment limited to the project's virtual environment and required Windows system directories.
5. Test the freshly extracted executable, not only the source tree or archive creation.

Apply the same diagnostic method to another application through its own build command.

## Result and limits

A clean process-local build omitted the incompatible bundled ICU. The resulting windowed executable opened, responded and closed with exit code 0. A freshly extracted archive passed the same smoke check. Source tests were separate because they did not exercise frozen DLL loading.

This establishes one build-environment failure and its local correction. It does not establish compatibility with older Windows versions or every Qt application. The harness compares only ICU-related imports and is not a complete Windows loader emulator. No Unity player, BepInEx plugin, gameplay or save behavior was tested. A hidden native window did not provide a useful title assertion on this host, so the successful smoke check opened the UI normally and closed it without starting a game.

Recheck after changing Python, Qt, freezer hooks, Windows or the build process environment.
