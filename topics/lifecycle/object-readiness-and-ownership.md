# Object readiness and ownership before activation

A component's presence or a hierarchy name does not prove that native setup is complete. Readiness depends on the first consumer and on the visual, physics, save and joint owners that consumer relies on.

[Lifecycle](README.md)

This is an inspection method for factory, spawn and restoration paths. It does not define a universal Unity lifecycle order.

## First-consumer timing

The relevant path can include asset loading, component addition, activation, native registration and load callbacks. References read before setup completes include inactive alternatives, serialized fields and dependency objects, while branches, exception handlers and later initialization can change the first effective consumer.

Inspect exact lifecycle and ownership members, including `Awake`, `Start`, `Object.Instantiate`, `Transform.parent`, `Rigidbody`, `Joint.connectedBody` and deferred `IEnumerator.MoveNext` bodies where they participate in the path.

A correction made after the first dependent callback is already too late for that callback. Independent clones, renamed clones, inactive staging, inventory insertion and later retrieval can also enter through different lifecycle paths than the factory case.

## Distinct ownership relationships

Visual parent, physics parent, save parent, joint owner and connected body are separate relationships. Actual reference assignments and cached owners provide stronger evidence than hierarchy names, null joints or public fields.

For serialized template identity, runtime registration, per-instance identity and restoration, use [Instance identity and restoration](../persistence/instance-identity-and-restoration.md).

## Lifecycle evidence

Exercise missing references, foreign objects with similar hierarchies, unsupported inputs, repeated preparation and exceptional cleanup. Compare reference assignments, relative poses, contents and joints before and after activation or relocation.

Source inspection and managed fixtures do not establish callback order in a running player, scene readiness, physical stability, rendering or save restoration. Changes to native callbacks, assets, registries, loaders or supported attachments require a new check.

The Sailwind [item spawn-initialization case](https://github.com/Skeptic043/sailwind-technical-reference/blob/main/docs/research/items/spawn-initialization.md) provides a concrete application with its own game-specific limits.
