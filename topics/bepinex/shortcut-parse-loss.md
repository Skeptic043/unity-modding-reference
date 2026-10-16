# BepInEx 5 shortcut parse failures can replace the saved binding

In BepInEx 5.4.23.5, invalid shortcut text can parse to an empty shortcut and later be saved as an empty value. A change listener cannot reliably recover the original text because the configuration system saves before invoking listeners.

[BepInEx](README.md)

## How the text is lost

The inspected `KeyboardShortcut.Deserialize` splits on spaces and shortcut separators, then calls the case-sensitive `Enum.Parse` overload for each key. It catches `SystemException`, logs a keybind error and returns `Empty`. An invalid value therefore does not have to escape as an exception to the configuration system. `KeyboardShortcut.Serialize` returns an empty string for that empty shortcut. [BepInEx configuration and shortcut implementation](#evidence)

`ConfigEntryBase.SetSerializedValue` assigns the converter's returned value. `ConfigFile.Bind` applies an orphaned serialized value, removes the orphan and saves when `SaveOnConfigSet` is enabled. `Save` serializes the bound value. From that call chain, a failed shortcut parse can replace the original binding text with an empty value during binding or a later save. A later change listener cannot reliably recover the original text because `ConfigFile.OnSettingChanged` saves before invoking listeners. [BepInEx configuration and shortcut implementation](#evidence)

Key names with changed capitalization or embedded spaces need deliberate handling before binding to the loader's shortcut type. A mod-specific raw string entry with separate validation is one possible design. This observation does not prescribe changing the global converter used by other plugins.

## Reproduce and retrieve

Acquire BepInEx 5.4.23.5 independently and locate `BepInEx/core/BepInEx.dll`. With ILSpy's command-line tool available, run the read-only inspection commands after replacing the example input path. Another managed decompiler can expose the same types and methods.

```powershell
ilspycmd -t BepInEx.Configuration.KeyboardShortcut 'C:\path\to\BepInEx\core\BepInEx.dll'
ilspycmd -t BepInEx.Configuration.ConfigEntryBase 'C:\path\to\BepInEx\core\BepInEx.dll'
ilspycmd -t BepInEx.Configuration.ConfigFile 'C:\path\to\BepInEx\core\BepInEx.dll'
```

Inspect the named methods and converter call chain. No proprietary game assembly, game installation change or configuration write is needed. For a mod fix, separately test its production binding with the real loader library in a scratch configuration directory, including bind, save, reload and invalid-input retention.

## Evidence

The evidence is read-only decompilation of BepInEx 5.4.23.5 on Windows. Inspect `BepInEx.Configuration.KeyboardShortcut.Deserialize` and `Serialize`, `ConfigEntryBase.SetSerializedValue`, and `ConfigFile.Bind`, `Save` and `OnSettingChanged`.

The engine, backend and player runtime were not exercised. This does not establish that every reported lost setting has this cause or which application last wrote a user's file. Other releases, configuration editors and input backends may differ, and a running game can overwrite file edits from its in-memory configuration. The article also does not promise that every enum key is supported by every device. Reinspect after changing loader versions, converter registration, shortcut entry type or configuration editor. Test migration before changing an existing section/key identity, and verify actual game input separately from successful parsing.
