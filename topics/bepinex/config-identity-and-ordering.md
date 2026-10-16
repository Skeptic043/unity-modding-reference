# BepInEx 5 config identity and section ordering

In BepInEx 5.4.23.5, a setting's section and key form its saved identity. Changing either value needs deliberate migration if existing user values should carry over. Reordering calls to `Bind` does not select a different section order in the inspected implementation.

[BepInEx](README.md)

## How identity and ordering work

`ConfigFile.Save` combines bound and orphaned entries, groups them by section and sorts those section groups by name. Reordering calls to `Bind` therefore does not control the saved section order in this implementation. [BepInEx ConfigFile implementation](https://github.com/BepInEx/BepInEx/blob/57f1fb859bd4d0264cd2a59074d0e96c6a492a33/BepInEx/Configuration/ConfigFile.cs)

`ConfigFile.Bind` identifies settings through `ConfigDefinition`. Its section/key overload constructs that identity from both values, and a matching serialized value from `OrphanedEntries` is applied when binding. With `SaveOnConfigSet` enabled, the bind path then saves the updated configuration. Renaming a section or key therefore needs deliberate migration if earlier values should carry over. This migration requirement is an inference from the inspected identity and orphan-restoration path. [BepInEx ConfigFile implementation](https://github.com/BepInEx/BepInEx/blob/57f1fb859bd4d0264cd2a59074d0e96c6a492a33/BepInEx/Configuration/ConfigFile.cs)

For a migration, test the production binding against a retained old-config fixture. Exercise save and reload with the target library, preserving custom values and unknown entries.

## Evidence and applicability

This is source inspection from September 13, 2026 of release tag `v5.4.23.5`, revision `57f1fb859bd4d0264cd2a59074d0e96c6a492a33`. Inspect `ConfigFile.Save`, both `ConfigFile.Bind` overloads and `ConfigDefinition` in `BepInEx/Configuration/ConfigFile.cs`.

No game, editor or loader lifecycle was executed for these statements. The OS and engine are not relevant to this source-reading observation. The observation does not establish behavior in other versions, runtimes or config-manager UIs, and it does not establish stable within-section enumeration order across runtimes. Reinspect the target loader when upgrading BepInEx or renaming config identities. A mod's grouping and migration policy remains its own decision.
