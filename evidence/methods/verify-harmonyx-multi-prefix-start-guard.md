# Verify a HarmonyX start guard with multiple prefixes

## Purpose

Keep an invalid menu choice from entering any startup side effect when several mods patch the same start method. This is a verification recipe, not a claim that every game has the same menu call chain.

## Conditions

Inspect the target application's exact installed assemblies and active loader, including the `0Harmony.dll` actually loaded. Confirm the active profile's Harmony implementation before applying this recipe to a live run.

## Procedure

1. Trace the visible button's pointer method to the method that starts a new game. Record where the menu is disabled and where world mutation begins.
2. Enumerate every prefix on the start method and their actual order. Consult the [HarmonyX class-patch documentation](https://github.com/BepInEx/HarmonyX/wiki/Class-patches) for the loaded fork. In HarmonyX, returning `false` skips the original but does not suppress other prefixes, so a later world-mutating prefix may still run.
3. Put the user-action guard at the earliest narrow caller that can stop the start call before other startup prefixes are entered. Keep the visible button disabled when the choice is invalid, but also guard a direct menu action that bypasses its pointer state. Do not assume a `false` prefix on the shared start method is an atomic cancellation.
4. Test both an invalid and a valid choice with the co-installed patches active. An invalid choice should leave the menu and world unchanged. A valid choice should still reach every expected prefix and the original start method. Repeat after a menu reopen, a config reload, and any relevant mod-order change.

## Uncertainty

A guard on the menu caller may not cover a different mod that invokes the start method directly. The application may also have another confirmation path or a patch on the caller itself. Static IL and documentation establish the possible control flow, while live tests are needed to confirm menu behavior in the exact profile.

## Recheck triggers

Repeat when the HarmonyX version, call chain, button type, prefix set or ordering, co-installed startup mod, or menu lifecycle changes.
