# Persistent instance identity and restoration

Prefab names, hierarchy paths and convenient live indexes do not establish durable identity on their own. Template identity, runtime registration and per-instance identity answer different questions within the native lifecycle.

[Persistence](README.md)

## Native identity contract

The identity contract includes native allocation, registration and load bounds, plus every place that can hold the same instance while it is absent from the live scene, such as an inventory record, unload cache or deferred-spawn table. Acquisition may already assign an instance identity, while zero or unassigned values can remain distinct from the native registered flag.

For persisted preferences, the current spawn index and durable key remain separate. Migration is supported only when the saved instance ID, expected template or provider, and matching extension record agree, including duplicate-name, provider-removal and numeric-ID reuse cases.

## Fresh acquisition and restoration

A newly acquired instance may need initial extension state, while a loaded instance may still be waiting for the game or provider to deliver its saved extension record. Passive binding therefore depends on a lifecycle point that distinguishes those cases, so an early match cannot initialize over data that has not been restored yet.

The relevant ordering question is whether native capture or caching occurs before the extension save callback. If it does, extension binding may need to happen before native capture as well as before interaction and extension serialization. Acceptance would include acquisition followed immediately by capture or caching without relying on an update tick.

Extension records may need to remain available while an object is cached or unloaded because absence from the live hierarchy does not establish deletion. Cache transfer, deferred spawning, save-slot changes and missing providers expose that distinction.

## Evidence limits

Source inspection can establish the observed allocation and callback order for an identified build. Fixtures can exercise identity collisions and missing records. Neither proves restoration in a Unity player, safe migration of an existing save or compatibility with another provider. Live acceptance on disposable copied saves covers fresh acquisition, save, reload, cache transfer, provider removal and provider return.

The [Sailwind item identity case](https://github.com/Skeptic043/sailwind-technical-reference/blob/main/docs/research/persistence/item-identity-and-migration.md) and [save-capture case](https://github.com/Skeptic043/sailwind-technical-reference/blob/main/docs/research/persistence/save-capture-and-coroutine-boundaries.md) show how these questions map to one game's native item and save lifecycle. Their symbols and ordering are not universal Unity behavior.
