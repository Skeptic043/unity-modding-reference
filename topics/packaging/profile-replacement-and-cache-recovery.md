# Profile replacement and shared-cache rollback

Restoring an active profile after a failed replacement is not enough when the manager also changed a shared artifact cache. A later profile can replay rejected cached bytes even though the profile that failed now looks unchanged.

[Packaging](README.md)

## Profile and cache transaction boundary

The bounded replacement transaction includes:

1. Normalize incoming files in disposable staging before moving active files.
2. Retain the original profile manifest bytes and the planned replacement manifest.
3. Back up every active profile path owned by the operation.
4. Include the exact source-aware cache artifact in the same rollback boundary.
5. Restore the prior artifact on failure, or remove the newly created artifact when none existed before.
6. Keep recovery evidence and block reuse of that exact artifact while rollback remains incomplete.

Equal plugin GUIDs and versions from different repositories can still map to different cache keys. Resolve the effective repository and source key before checking recovery state. A guard on an older plain key does not protect a later source-associated key.

## Exact restoration and separate cleanup state

Rollback restores original folder spelling, file bytes and manifest bytes. Disabled replacements remain disabled instead of being copied into active plugins. Shared or untracked folders require explicit ownership before the replacement can remove them.

Commit success and cleanup success are different states. A replacement can become usable even when marker or temporary-directory cleanup later fails. That cleanup failure does not establish a full rollback, and its recovery evidence remains relevant.

These checks do not establish power-loss or process-termination safety, junction behavior, real profiles, network behavior or safety while a managed game is loaded. Recheck transaction boundaries after cache-key, manifest, staging, replacement or cleanup behavior changes.
