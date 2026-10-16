# Compatibility evidence levels

A clean loader entry does not prove that a feature works, and a matching patch target does not prove live compatibility. Package, loader, static, synthetic and gameplay results support different conclusions rather than one undifferentiated compatible or broken label.

[Validation](README.md)

## Evidence levels

1. Package evidence checks that the distributed archive contains every intended plugin, support library, configuration file and asset with the expected hashes.
2. Loader evidence checks declared identities, dependency resolution and the startup log from the intended profile.
3. Static evidence checks that referenced types, methods, fields and version guards still match the inspected game and mod binaries.
4. Managed or synthetic evidence exercises a bounded policy or operation without claiming Unity callbacks, scene objects or native runtime behavior.
5. Live feature evidence exercises the relevant path in the game under the intended build, loader and mod combination.
6. Persistence evidence covers save, reload, cache transfer and any migration that the feature depends on.
7. Multiplayer evidence covers authority, replication and the supported host and client arrangements.
8. Performance evidence measures the relevant load, frame, allocation or latency behavior under named conditions.
9. Long-session evidence checks behavior that can change after extended play, repeated transitions or accumulated state.

Passing an earlier layer does not imply a later one. A complete package can fail during loading. A clean load can coexist with an unreachable feature. A synthetic operation can succeed while its Unity callback never runs. Persistence, multiplayer, performance and long-session behavior need their own acceptance whenever the feature depends on them.

## Logs from the same launch

Different log files can refer to different launches, profiles or executables. Before combining them as one result, compare timestamps, process or session markers, build and loader identities, the selected profile, and the feature action that produced the relevant lines.

Review logs locally before sharing them because paths, user names, server addresses and other private values can be present. Keep only the lines needed to support the conclusion.

## Scope of compatibility conclusions

A compatibility conclusion can name the input, the successful check and the remaining gate. For example:

- The declared GUID and version were read from this DLL, but runtime loading was not tested.
- The installed method still contains the expected field access, but coinstalled patch order was not exercised.
- The complete package passed a synthetic dependency probe, but gameplay and save/reload remain untested.
- The feature worked in one dated build and profile, but another loader, platform or mod set needs its own check.

Deprecation, a changed version number or lack of reported problems is not by itself evidence of breakage or compatibility. A game, engine, backend, loader, provider, package, dependency or coinstalled-mod change requires a new check when it can affect the layer supporting the claim.

The [Sailwind Radio package-dependency case](https://github.com/Skeptic043/sailwind-technical-reference/blob/main/docs/research/compatibility/radio-package-dependency-case.md) applies this method to one missing managed dependency. Its summary and limits apply to that case only.
