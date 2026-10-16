# Startup guards and shared side effects

An invalid-choice guard prevents all startup side effects only when it covers every entry into the shared start path. Disabling one pointer target is insufficient when another menu action or caller can still invoke the same method.

[HarmonyX](README.md)

## Startup entry paths

The relevant entry path runs from the visible button through its pointer method, menu action and shared start method, including the points where the menu closes and world mutation begins. Prefix order is part of that path.

The [HarmonyX class-patch documentation](https://github.com/BepInEx/HarmonyX/wiki/Class-patches#prefix) states that a false prefix skips the original method while the other prefixes still run.

This is not a claim about every Harmony fork or the currently loaded DLL. Inspect the active profile and generated wrapper before relying on prefix ordering.

## Guarded and ordinary starts

The verification recipe includes direct actions that bypass pointer disabled state. It expects invalid choices to leave the menu and world unchanged, while valid choices reach every expected prefix and the original, including after menu reopen, configuration reload and mod-order changes.

A caller guard may not cover another mod calling the shared start method directly, another confirmation path or a patch on the caller. Record input hashes, game and loader versions, source offsets and the complete harness for the tested game.

The [complete verification recipe](../../evidence/methods/verify-harmonyx-multi-prefix-start-guard.md) separates the static control-flow inspection from the live profile checks.
