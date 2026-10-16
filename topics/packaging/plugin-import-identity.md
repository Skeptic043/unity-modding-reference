# Verify packaged plugin identity with the actual importer

Do not assume that a plugin importer identifies a DLL from its declared `BepInPlugin` metadata. Inspect the candidate DLL and the importer's real discovery code, then exercise that same discovery path with the filename, hints and catalog used by the reported import. Keep the installed mod set unchanged while isolating the identity decision.

This catches consumers that rank arbitrary GUID-like strings and accidentally select an optional integration or dependency string instead of the declared plugin identity.

## Procedure

1. Preserve a failing artifact, then hash the delivered DLL and the exact importer executable.
2. Inspect the DLL's `BepInPlugin` attribute directly.
3. Distinguish the plugin GUID from optional integration strings and `BepInDependency` attributes. Decode dependency flags before calling another plugin required.
4. Inspect the importer's logs and identity-selection implementation.
5. Exercise the importer's own discovery function against before and after artifacts with the same filenames, hints and catalog.
6. Keep discovery tests separate from import, installation and network refresh operations.
7. If a narrow workaround changes an optional lookup string, verify its exact runtime value and confirm that plugin and configuration identity remain unchanged.

Do not rename a released plugin merely to satisfy a consumer heuristic. Existing GUIDs can participate in configuration paths, dependency declarations, Harmony ownership and persisted references.

## Evidence to retain

Record the complete discovery harness, importer identity, relevant catalog context, artifact hashes, output and limits. Record the importer version or hash because a result from one implementation does not establish how another importer chooses identity. No consumer harness is included in this public reference, so readers must reproduce the check against their own importer.

A successful discovery result does not establish installation, Unity runtime, gameplay or save compatibility. Repeat the check when the importer, catalog, filename or candidate metadata changes.
