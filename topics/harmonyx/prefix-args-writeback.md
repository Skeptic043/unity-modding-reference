# HarmonyX prefix `__args` writes do not change the original argument in the inspected build

In HarmonyX 2.9.0.0 from BepInEx 5.4.23.5, a prefix could read `__args`, but assigning a new value into that array did not change the argument received by the original method. A named `ref` parameter did.

[HarmonyX](README.md)

## What the probe observed

A `HarmonyMethod` prefix that read `__args[0]` saw the caller's value, 1, and assigned `__args[0] = 7`. The original method still received 1. A prefix on an identical method that declared `ref int value` and assigned 9 changed the value received by the original to 9. The [expected results](../../evidence/harmonyx-prefix-args-20261005/results.txt) record both cases.

For this HarmonyX build, changing an element in `__args` in a prefix did not write back to the original method's arguments. Argument writeback in the probe instead used a `ref` parameter with the original parameter's name in a prefix for that specific signature. A prefix shared across methods with different signatures can delegate to a common helper that takes the value by reference.

## Evidence and applicability

The [probe harness](../../evidence/harmonyx-prefix-args-20261005/) contains `Program.cs`, `Probe.csproj`, `Run.ps1 -HarmonyPath <BepInEx/core/0Harmony.dll>` and its expected output. It needs a .NET SDK that builds `net472` and the `0Harmony.dll` from the [official BepInEx 5.4.23.5 release](https://github.com/BepInEx/BepInEx/releases/tag/v5.4.23.5), package `BepInEx_win_x64_5.4.23.5.zip`. The loader library is not retained here.

The probe ran static methods with one `int` parameter on a desktop .NET Framework 4.x CLR, not on Unity Mono. Instance methods, reference-type arguments and other HarmonyX versions were not checked. Write-back behavior likely depends on how the wrapper generator copies the argument array back, so a later HarmonyX release could differ. Rerun the probe before relying on either behavior with another version or method signature.
