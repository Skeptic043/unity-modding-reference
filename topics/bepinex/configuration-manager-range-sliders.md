# Standard Configuration Manager range sliders and discrete steps

The official Configuration Manager uses numeric range metadata to draw a normal slider, but a float range does not define discrete steps by itself. BepInEx also clamps values before ordinary change handlers see them, which can hide some nonfinite inputs behind valid-looking endpoints.

[BepInEx](README.md)

## What the standard slider provides

The official manager discovers a setting's acceptable numeric range and draws a horizontal slider. Its inspected `SettingFieldDrawer.DrawRangeField` converts the continuous slider result to the setting type and writes it through the setting entry. The adjacent numeric text input clamps to the endpoints. A float range of 1 to 3 therefore provides a normal numeric slider without a custom drawer or direct manager reference, but does not itself specify half-unit steps. [Manager range drawer](#manager-range-drawer). The [official manager documentation](https://github.com/BepInEx/BepInEx.ConfigurationManager#how-to-make-my-setting-into-a-slider) describes range metadata as the slider trigger.

In the inspected BepInEx library, `AcceptableValueRange<T>.Clamp` checks only the minimum and maximum. `ConfigEntryBase.ClampValue<T>` applies that constraint before the entry stores the value. For float entries, the inspected code's `Single.CompareTo` comparisons can map NaN and negative infinity to the lower endpoint, and positive infinity to the upper endpoint, before an ordinary setting-change handler observes the value. A handler alone cannot reliably distinguish these sanitized endpoints from intentional user choices. [Loader range clamp](#loader-range-clamp).

For a mod that requires discrete steps, normalization belongs in its configuration and value policy. Retain range metadata for the standard manager and test both the production binding and effective gameplay value. Check first binding, setting-change reentry, saved-value roundtrip, config reload, ties, arbitrary typed numbers, out-of-range inputs and nonfinite inputs.

This is a method recommendation, not a claim that every manager renders snapping identically or that a particular mod passed these checks.

## Conditions and uncertainty

The observation comes from static decompilation on October 3, 2026 of BepInEx 5.4.23.5 and official BepInEx.ConfigurationManager 19.0 Mono on Windows. No rendered game or Configuration Manager session was exercised. Whether the UI refreshes a snapped thumb or text value in the same frame needs live verification. Loader variants, manager forks and IL2CPP builds are not covered. Match the manager package to the exact loader and backend before applying this observation elsewhere.

## Evidence and reproduction

Acquire the official [BepInEx 5.4.23.5 Windows x64 release](https://github.com/BepInEx/BepInEx/releases/tag/v5.4.23.5) and [Configuration Manager 19.0 BepInEx 5 release](https://github.com/BepInEx/BepInEx.ConfigurationManager/releases/tag/v19.0). Extract them to an owned dependency folder without modifying a game installation. Both findings come from static decompilation on Windows. The commands below use ILSpy's command-line tool, but another managed decompiler can expose the same types and methods. No rendered manager session was exercised.

### Manager range drawer

In Configuration Manager 19.0 Mono, inspect `SettingFieldDrawer.DrawSettingValue` and `DrawRangeField` for range discovery, slider conversion and numeric input clamping. Substitute the acquired input path:

```powershell
ilspycmd -t ConfigurationManager.SettingFieldDrawer 'C:\path\to\BepInEx\plugins\ConfigurationManager\ConfigurationManager.dll'
```

### Loader range clamp

In BepInEx 5.4.23.5, inspect `AcceptableValueRange<T>.Clamp` and `ConfigEntryBase.ClampValue<T>` for endpoint comparisons and clamping before storage. Substitute the acquired input path:

```powershell
ilspycmd -t 'BepInEx.Configuration.AcceptableValueRange`1' 'C:\path\to\BepInEx\core\BepInEx.dll'
ilspycmd -t BepInEx.Configuration.ConfigEntryBase 'C:\path\to\BepInEx\core\BepInEx.dll'
```

No game code or manager implementation is redistributed here. Reinspect when the loader or manager version, setting type, numeric endpoints or normalization policy changes.
