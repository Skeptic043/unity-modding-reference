# HarmonyX

Articles about HarmonyX target preparation, transpiler input, prefix behavior and cleanup. Each article keeps the relevant HarmonyX, loader and runtime versions beside the behavior it explains.

- [Prepare before target resolution](prepare-before-target-resolution.md): how a class `Prepare` guard can skip an optional target lookup in the inspected build.
- [Transpiler branch normalization](transpiler-branch-normalization.md): why exact short-branch opcode checks can reject legitimate transpiler input.
- [Prefix `__args` write-back](prefix-args-writeback.md): why writing an argument array did not change the original call while a named `ref` parameter did.
- [Startup guard scope](startup-guard-scope.md): place an invalid-choice guard before every shared startup side effect and verify the installed prefix ordering.
- [Temporary field restoration](temporary-field-restoration.md): capture an original value per invocation and restore it on normal, skipped and exceptional exits.
- [Clean up every attempted target after a patch failure](failed-install-cleanup.md): why a failed target can still retain owner records, and how to remove one mod's records without removing competing patches.

[Unity modding knowledge](../../README.md)
