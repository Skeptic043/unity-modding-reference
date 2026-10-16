# Targeting, action execution and ownership

A component can implement an action without granting permission to target it. Target selection, action dispatch and the resulting ownership change are separate paths, so an ID, name or activation-method change can affect only part of the interaction.

[UI](README.md)

## Complete interaction path

1. Identify the pointer or selection system and record every targeting gate.
2. Follow inherited and virtual item methods, activation overloads and the exact click or drop timing.
3. Record which object is targeted, which component receives the action and which object becomes owned, held or registered afterward.
4. Check whether the target can become stale between physics, update and render frames.
5. Compare old and new templates and coinstalled patches before assigning causality to an identity change.

Test the failing action with held and unheld objects, unrelated targets and native action buttons. Preserve ordinary behavior for objects outside the intended set.

## Evidence limits

Managed call traces can establish gates and dispatch order. They do not prove pointer projection, collider reachability, frame timing or a player's ability to complete the action in the running game. Recheck after targeting, native input, template identity or coinstalled patches change.

The [Sailwind held-item targeting case](https://github.com/Skeptic043/sailwind-technical-reference/blob/main/docs/research/input/held-item-targeting.md) shows how a lamp-hook interaction separated targeting permission from the later item action. That result is bounded to its inspected game path.
