# Persistence

These articles separate durable identity and save-capture timing from the live objects and callbacks that happen to exist in one frame.

- [Instance identity and restoration](instance-identity-and-restoration.md): bind extension records to a verified durable identity and distinguish fresh acquisition from loaded restoration.
- [Save continuations and cancellation](save-continuations-and-cancellation.md): follow a save through suspension and actual capture so stale work cannot write after a newer operation takes ownership.

The Sailwind [item identity](https://github.com/Skeptic043/sailwind-technical-reference/blob/main/docs/research/persistence/item-identity-and-migration.md) and [save-capture](https://github.com/Skeptic043/sailwind-technical-reference/blob/main/docs/research/persistence/save-capture-and-coroutine-boundaries.md) cases provide game-specific applications.

Static ordering and desktop fixtures do not establish save/reload behavior in a Unity player. Live acceptance belongs on disposable copied saves.

[Unity modding reference](../../README.md)
