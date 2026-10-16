# Packaging

Practical checks for dependency collection and identity handling when a working project is turned into a distributable application or plugin.

- [Diagnose Qt freezer ICU mismatches from the build environment](qt-freezer-icu-selection.md): compare the symbols imported by Qt with the symbols exported by the native library selected during the freeze.
- [Verify packaged plugin identity with the actual importer](plugin-import-identity.md): distinguish declared plugin identity from optional integration and dependency strings, then exercise the consumer's real discovery path.
- [Declared plugin identity and package ownership](plugin-identity-and-package-ownership.md): the BepInEx declaration, support-file identity and ownership of a shared folder.
- [Profile replacement and shared-cache rollback](profile-replacement-and-cache-recovery.md): source-aware cache artifacts and active-profile restoration after a failed replacement.

These articles establish narrow packaging and discovery behavior under their recorded conditions. They do not establish installation, Unity runtime, gameplay or save compatibility.
