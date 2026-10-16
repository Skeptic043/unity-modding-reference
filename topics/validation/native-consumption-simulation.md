# Native consumption simulation

An availability query and a consumption method can apply different ownership, charge, group or container rules. A preview can therefore approve an action that the real removal path rejects, or count the same stack twice across overlapping requirements.

[Validation](README.md)

## Native consuming paths on detached state

When the complete removal path can be bounded, detached mutable inputs allow that same consuming mode to run without changing real inventory. The native order is preserved, so an ingredient removed for an early requirement is unavailable to later requirements.

The inspected simulation includes these steps:

1. Enumerate stack mutations, recipe mutation, shared dictionaries, metadata setters, output insertion, refresh and callbacks. Include generated delegates and nested helpers.
2. Detach all mutable stack and recipe data while preserving only verified read-only definitions and Unity identities. A helper called `Copy` or `Clone` does not establish deep isolation. Check nested liquid, charge, skin and quality objects.
3. Redirect action-owned mutable state to the private simulation state. Suppress external insertion and UI effects only within that scope.
4. Run consuming mode on the detached inputs.
5. Inspect the native requirements' final residual values. Reject positive unpaid quantities, nonfinite amounts and unit truncation that would mark a fractional cost paid after removing zero units.
6. Recheck the ordered real sources by reference identity, then compare content and recipe or quantity state after callbacks and immediately before payment or output begins.

## Native policy and failure behavior

Output-container handling follows the native rule. A minimum usable-container requirement can coexist with intentionally retained unbottled liquid, while a total-capacity requirement can reject behavior the native action allows.

In the inspected simulation, every mandatory adaptation is asserted and the extension is disabled as one unit if any required redirect or suppression is missing. A validation failure exits the outer action. Skipping a void removal method can let its caller continue producing outputs.

The inspected simulation restores nested scopes on every exit and preserves native exceptions. A nested read disposes only the scope it created, while refresh and registry mutation remain outside consuming scopes.

## Applicability conditions

This approach is applicable only when the complete mutation and callback surface can be inspected, all mutable inputs can be detached, external side effects can be suppressed within the simulation, and the real ordered sources can be checked again immediately before commitment.

Custom callbacks during real consumption can change later sources after preflight. An extension cannot promise to undo arbitrary third-party side effects. Changes to input matching, metadata cloning, callbacks, output containers, patches or the loader require a new check.

## Evidence limits

Normalized Harmony transformations, mandatory-hook failures, branch destinations and typed locals are separate checks from synthetic provider and detached-state fixtures. Neither establishes Unity startup, native gameplay conservation, callbacks during real payment, save/reload or performance.
