# Operating limits from force and failure paths

A physics coefficient, displayed weight or visible clearance is not an operating limit by itself. A calculated envelope depends on runtime mass, active force contributors, the actual failure path and the assumptions used by the model.

[Physics](README.md)

## Runtime inputs

Displayed values can differ from the numbers used by the body-mass calculation, and displayed units do not follow automatically from Unity conventions. Serialized multipliers can also differ from code initializers.

Mass and force contributors can change after startup and restoration. Raw prefab activation does not establish post-start activation, while selected options, ancestor state, provider-created objects, cached values and performance guards can determine whether a force or failure check is active.

## Force-law behavior

The force-law interpretation depends on whether a term supplies fixed force or mass-independent acceleration. Lift proportional to immersion without a clamp has no finite pure-buoyancy ceiling, while a conditional acceleration can remove a stationary load ceiling without proving practical stability or motion.

Each modeled configuration's mass corresponds to its enabled force contributors. A runtime mode can suppress supplemental support without disabling the body's ordinary force path, producing different laws for the same authored object.

## Failure paths and bounded envelopes

A bounded envelope depends on sample points transformed through native initialization and parent scale, offsets interpreted in their actual coordinate units and conditional activation identified separately. Visual intersection, displayed clearance, first failure input, positive accumulation and the final failed state are distinct observations.

The envelope depends on prescribed orientation, local clearance and the other stated assumptions. Endpoint samples do not necessarily bound an entire angle interval because a sample entering or leaving the active region can create an interior minimum within the piecewise force boundaries.

## Evidence limits

The motivating method was inspected on 2026-10-06 in a Unity 2019.1.10f1 Windows Mono game. The Sailwind [boat mass and load-limit case](https://github.com/Skeptic043/sailwind-technical-reference/blob/main/docs/research/physics/boat-mass-and-load-limits.md) and [buoyancy and spawn-height case](https://github.com/Skeptic043/sailwind-technical-reference/blob/main/docs/research/physics/buoyancy-and-spawn-height.md) retain the vessel-specific values, mechanics and applications.

A static model does not establish equilibrium, dynamic stability, weather survival, useful speed or gameplay acceptance. A screenshot can constrain displayed mass and condition at one instant, but it cannot prove the model's current input identity or sustained behavior. Changes to force mode, clamps, point initialization, parent scaling, mass bookkeeping, serialized settings, conditional failure samples or provider code require a new check.
