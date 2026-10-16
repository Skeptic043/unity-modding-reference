# HarmonyX normalizes branch encodings before transpilers

HarmonyX 2.9.0.0 converts short branch opcodes to their long forms before invoking transpilers. A guard that requires an exact raw short-form opcode can therefore reject legitimate transpiler input even when the condition and destination have not changed.

[HarmonyX](README.md)

## What normalization changes

In the inspected library, `ILManipulator.ApplyTranspilers` prepares instructions and calls `NormalizeInstructions` before invoking a registered transpiler. It normalizes the returned instructions again afterward. [HarmonyX ILManipulator implementation](#evidence)

`NormalizeInstructions` uses `ShortToLongMap` to replace short branch opcodes with corresponding long forms, including `OpCodes.Brfalse_S` to `OpCodes.Brfalse`. The inspected method changes the opcode while retaining the operand. The rejection risk is an inference from this transformation and an exact-opcode guard.

Accepting equivalent branch encodings must still validate the condition, target label and relevant members. Different conditions or destinations are not formatting differences.

## Verification recipe

Acquire the target loader library separately and confirm its reported HarmonyX version. With ILSpy's command-line tool, inspect the named type. Another managed decompiler can expose the same methods.

```powershell
ilspycmd -t HarmonyLib.Internal.Patching.ILManipulator 'C:\path\to\BepInEx\core\0Harmony.dll'
```

Read `ApplyTranspilers`, `NormalizeInstructions` and `ShortToLongMap`. For an integration check, feed the inspected method body through the actual library's instruction reader and transpiler invocation path. A hand-built Cecil-to-`CodeInstruction` conversion alone may omit this adapter step. Keep every fixture and helper with the consuming mod's tests.

## Evidence

This is static inspection of `0Harmony.dll` assembly version 2.9.0.0. Inspect `HarmonyLib.Internal.Patching.ILManipulator.ApplyTranspilers`, `NormalizeInstructions` and `ShortToLongMap`.

No game or editor was launched. The finding does not establish the behavior of every Harmony or HarmonyX release, Mono runtime or another mod's patches. An in-memory instruction transformation does not prove that detouring or plugin startup works inside Unity. Reinspect when the library version or instruction adapter changes, and test normalized input plus changed-condition and changed-target rejection in the consuming mod.
