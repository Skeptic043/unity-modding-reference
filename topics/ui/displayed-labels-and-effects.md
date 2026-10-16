# Displayed labels and gameplay effects

A visible quality category does not establish the mechanical value a player receives. The label calculation and gameplay effect are separate paths whose relationship can be established only after both are identified.

[UI](README.md)

## Inspection sequence

1. Find the method that produces the visible label. Record each comparison, boundary and whether the label uses a raw or transformed value.
2. Follow the action's actual value through every argument into the gameplay method. Record whether it applies on entry, during updates or on completion.
3. Distinguish a queued amount or target from the current stat. Inspect the convergence path, update cadence and clamps.
4. Search independently for the suspected recovery or effect stat and every write to its rate. Do not infer a multiplier from a category name.
5. Inspect the serialized prefab or component. A C# field initializer does not establish the shipped asset default.
6. Check save values, difficulty settings and runtime patches as separate overrides.
7. Report the mechanical dimension that differs, its units and when it applies. Keep unrelated safety, ownership and event flags separate from quality.

## Evidence limits

Source and asset inspection can establish calculations and serialized values for an identified build. They do not establish the player's saved overrides, runtime patches or live meter behavior. Do not publish proprietary asset dumps. Retain only narrow values, hashes and reproduction steps.

Recheck after the game build, label thresholds, quality assets, save overrides or effect updates change.
