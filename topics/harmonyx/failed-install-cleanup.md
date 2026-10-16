# HarmonyX failed-install cleanup

A failed multi-target HarmonyX installation can leave owner records on the target that failed as well as on targets that succeeded. Cleanup limited to the successful targets therefore does not cover the complete attempted set.

This finding is based on HarmonyX assembly 2.9.0.0 from BepInEx 5.4.23.5 in a Windows desktop .NET Framework fixture. It does not establish the behavior of every Harmony implementation or Unity runtime.

[HarmonyX](README.md)

## Why successful targets are not enough

One relevant failure shape is a successful patch on target A followed by a competing transpiler causing validation of target B's transformed body to fail. Even though target B did not install successfully, it can still list the installing mod's owner in its patch records.

This leaves a gap in cleanup strategies that inspect only successfully patched methods. The failed attempt belongs in the set of targets to clean up too.

## Target- and owner-scoped cleanup

The passing cleanup path retained the exact set of targets attempted during installation. After failure, it visited each target and removed the installing mod's records with `Unpatch(method, HarmonyPatchType.All, ownerId)`.

The `ownerId` matters because other mods may also patch the same method, and cleanup does not own their records. Establishing cleanup requires `Harmony.GetPatchInfo` for each attempted target as well as the general patched-method inventory. A cleanup failure leaves the method's state unresolved.

For example, if installation succeeds on A and fails on B, cleanup must account for both A and B. Checking only A cannot reveal whether B still contains the failed owner's records.

## What the evidence establishes

A desktop regression installed a competing transpiler, exercised a failed installation, and checked that explicit per-target cleanup removed the installing owner's records while retaining the competitor's patch.

The fixture generated wrappers for inspected game methods and executed managed surrogate wrappers. It did not execute the game wrappers or Unity gameplay.

Arbitrary competing patches can still prevent cleanup. A different runtime or Harmony version needs its own verification.
