# Lifecycle

These articles focus on the boundaries where Unity objects become usable and on the separate owners that control their visual, physics, save and joint relationships.

- [Object readiness and ownership](object-readiness-and-ownership.md): find the first consumer and map visual, physics, save and joint ownership separately.
- [Runtime audio failure boundaries](../audio/investigating-runtime-audio.md): keep asynchronous completion, clip ownership, cancellation and DSP-time scheduling separate.

For durable identity and restoration, use [Persistence](../persistence/README.md). Placement geometry and transaction validation belong to their own subjects.

Sailwind's [spawn-initialization case](https://github.com/Skeptic043/sailwind-technical-reference/blob/main/docs/research/items/spawn-initialization.md) applies the inspection method to a concrete game path.

Static inspection and managed fixtures can narrow lifecycle paths. They do not establish scene readiness, physical stability, save restoration, performance or gameplay acceptance.

[Unity modding reference](../../README.md)
