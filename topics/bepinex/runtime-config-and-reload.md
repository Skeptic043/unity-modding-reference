# BepInEx 5 runtime config changes and `Reload()`

If a mod copies configuration into its own settings objects once at startup, it ignores Configuration Manager edits for the rest of the session. A later write from those stale objects also reverts the edit on disk. In the tested implementation, live in-game edits required rereading state after `ConfigFile.SettingChanged` while distinguishing events raised by the mod's own saves.

[BepInEx](README.md)

## What happens when a setting changes

- `ConfigEntry<T>.Value` clamps the value and returns if nothing changed. Otherwise it stores the value and calls `ConfigFile.OnSettingChanged` synchronously. `ConfigEntryBase.BoxedValue` sets `Value`.
- When `ConfigFile.SaveOnConfigSet` is true, `OnSettingChanged` calls `Save()` before invoking every `SettingChanged` handler. Each handler runs in its own try/catch, and exceptions are logged rather than thrown. The changed value is therefore already on disk when a handler runs.
- A mod's own multi-entry save raises one `SettingChanged` event per changed entry, part-way through the save. A handler that rereads everything sees a mix of old and new values. In the tested implementation, a guard distinguished the mod's own writes before one reread afterwards when needed.

## What `Reload()` does

`ConfigFile.Reload()` reads the whole file under the I/O lock before applying anything, so a missing or locked file throws before any entry changes. It then calls `SetSerializedValue` on each bound key, which goes through the `Value` setter. Unknown keys become orphaned entries.

`Reload()` raises `SettingChanged` once for each changed entry, then raises `ConfigReloaded`. With `SaveOnConfigSet` on, every changed entry rewrites the whole file during reload. In the tested handling, `SaveOnConfigSet` was turned off around `Reload()`, restored in `finally`, and followed by one reread. Leaving it on rewrites the hand-edited file part-way through reload and exposes partial states that can trigger invalid fallback behavior.

A bound key missing from the file keeps its in-memory value. A value that cannot be parsed is logged and the old value stays.

## Evidence and applicability

The inspected library was BepInEx 5.4.23.5. Sources were decompiled `ConfigEntry.cs` members `Value` and `BoxedValue`, and `ConfigFile.cs` members `OnSettingChanged`, `Reload` and `Save`, together with [BepInEx ConfigFile.cs at v5.4.23.5](https://github.com/BepInEx/BepInEx/blob/57f1fb859bd4d0264cd2a59074d0e96c6a492a33/BepInEx/Configuration/ConfigFile.cs).

Managed checks against the real `ConfigFile` ran outside Unity. They assigned entries the way Configuration Manager does and drove `Reload()` from hand-edited files. The checks covered in-place updates, later saves, guarded rereads and preservation of the hand-edited file during reload. Deliberately broken variants without the guard or reread failed.

This was not run in a game or through the Configuration Manager UI. That Configuration Manager applies values through `BoxedValue` on Unity's main thread, possibly on every keystroke in a text field, is an inference that was not re-inspected. If a save fails because the file is locked, memory and disk disagree. A later `Reload()` then restores the older disk value. Other BepInEx versions were not checked.

A BepInEx upgrade, or a change in Configuration Manager's use of `Value` or `BoxedValue`, requires reinspection of `ConfigEntry.Value`, `ConfigFile.OnSettingChanged` and `ConfigFile.Reload`.

Related: [config identity and ordering](config-identity-and-ordering.md) and [Configuration Manager range sliders](configuration-manager-range-sliders.md).
