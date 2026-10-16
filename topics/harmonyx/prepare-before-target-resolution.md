# HarmonyX class `Prepare` precedes target resolution in the inspected build

In HarmonyX 2.9.0.0, a class `Prepare` guard can skip target resolution before an optional target is looked up. This is useful when a target may disappear in a later host build, but the ordering is specific to the inspected HarmonyX binary.

[HarmonyX](README.md)

## What the inspected control flow does

`PatchClassProcessor.Patch` calls the class `HarmonyPrepare` hook before `PatchClassProcessor.GetBulkMethods`. If the hook returns false, the method returns an empty patch list before reaching `GetBulkMethods`. `GetBulkMethods` is where this implementation invokes `HarmonyTargetMethod`.

For an optional patch whose target may disappear in a later host build, a class `Prepare` guard can therefore skip target resolution in this exact HarmonyX build. This is an inference from the inspected control flow, not a compatibility promise for other Harmony releases.

## Verification recipe

Acquire the HarmonyX and Mono.Cecil binaries independently. Run the [read-only inspector](../../tools/inspect_harmony_patch_prepare.ps1) with their paths:

```powershell
pwsh -File tools/inspect_harmony_patch_prepare.ps1 -HarmonyDll 'C:\path\to\BepInEx\core\0Harmony.dll' -CecilDll 'C:\path\to\BepInEx\core\Mono.Cecil.dll'
```

Confirm the reported HarmonyX assembly version, then read the `Patch` branch from `HarmonyPrepare` to its false return and the later call to `GetBulkMethods`. Read the `HarmonyTargetMethod` invocation inside `GetBulkMethods`. Repeat on the actual loaded binary after a loader update.

A project that uses this guard still needs a build against the new host assemblies and a live startup check. The IL inspection only establishes this library's patch-processing order.

## Evidence

The finding comes from static IL inspection of HarmonyX 2.9.0.0 with Mono.Cecil 0.10.4.0. Inspect `HarmonyLib.PatchClassProcessor.Patch` and `GetBulkMethods` with the supplied read-only tool.

The inspection did not execute Harmony patching or test a future host build. It does not establish runtime detouring, target signature stability, behavior of other Harmony forks or safety of other patches in the same plugin. Reinspect after the loaded HarmonyX binary, patch-processing order, optional target method or host build changes.
