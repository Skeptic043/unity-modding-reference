# Verify temporary rate changes in a shared frame callback

## Purpose

Before scaling a consumption field around a native callback, determine whether the same callback handles visual state or depletion. This is a verification plan, not a general compatibility finding.

## Conditions

Use independently owned managed game and loader assemblies as read-only inputs. Record hashes, game build, tool versions, exact signatures and relevant IL offsets with the result. Keep game binaries and decompiled dumps out of packages.

## Procedure

1. Inspect the callback and rate field with Mono.Cecil or a managed decompiler. Confirm static/instance shape, parameters, return type, field type and consumption formula.
2. Trace other callback responsibilities and potential reentrancy before deciding how narrowly to patch it.
3. Check normal and exceptional restoration. Model a finalizer invoked without a captured value and require a capture marker before restoring. Preserve the original exception. Verify actual prefix skipping and finalizer order against the installed Harmony implementation before making interoperability claims.
4. Exercise zero, identity, faster/slower, nonfinite and extreme accepted inputs using production scaling/restoration code. Verify repeated calls do not compound the multiplier. Retain the complete harness and small result artifact with its exact limits.
5. In the game, compare consumption over equal simulation intervals, test depletion and refilling, and check the callback's other responsibilities. Repeat save/reload and the intended coinstalled patches. Record these separately from build and synthetic results.

## Expected observations

Accept bounded results: inspected arithmetic supports the scaling, capture/restoration behaves as exercised, and named live scenarios pass on the recorded profile. Member-existence checks and compilation do not establish unchanged callback semantics.

## Uncertainty

Direct invocation does not execute Harmony's generated wrapper. Static inspection cannot prove runtime ordering, visuals, reentrancy safety or persistence behavior. Single-precision resolution and rounded UI displays can obscure small changes.

## Recheck triggers

Repeat relevant inspection and live checks after game/loader, target symbol, patch code, accepted range or coinstalled patch changes.
