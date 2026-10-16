# Declared plugin identity and package ownership

A BepInEx plugin declaration identifies a loadable entry point. It does not identify every file that must be installed with that plugin, prove that the loader accepted it, or establish ownership of a shared directory.

[Packaging](README.md)

## Declared metadata identity

For BepInEx plugins, the three constructor arguments of `BepInPlugin` are the declared GUID, display name and version. Those compiled metadata arguments provide the direct declaration.

The following values do not substitute for the declaration:

- the DLL filename
- a dependency GUID
- a nearby version number
- a folder name
- a loader-log guess
- a catalog's latest release

Constants used in the source attribute are compiled into its metadata arguments. The dated design reference for this result was BepInEx `5.4.23.3` [`Attributes.cs`](https://github.com/BepInEx/BepInEx/blob/v5.4.23.3/BepInEx/Contract/Attributes.cs). That source explains the attribute shape but does not prove which loader or plugin binary is currently installed.

## Release-bundle ownership

Managed support DLLs and native libraries can be required even when they declare no plugin. Giving them invented plugin identities loses the difference between a loadable entry point and package support.

The reverse problem also occurs. One DLL can declare more than one plugin, and one directory can contain files shared by several plugins. Metadata alone cannot decide whether those files are safe to install, update or remove independently. An unambiguous release bundle, manifest or repository source can provide that ownership unit. Ambiguous ownership does not support an independent install, update or removal decision.

A catalog association, filename cleanup or package move does not by itself justify changing an established plugin GUID. Removal and replacement also depend on disabled copies, shared support libraries, generated configuration and files the manager does not own.

## Evidence limits

A bounded metadata reader can establish the declared GUID, name and version without loading an assembly. It cannot establish dependency resolution, successful loading, package completeness, safe removal, gameplay or compatibility. The actual binaries and release bundle need reinspection after an update, while [compatibility evidence levels](../validation/compatibility-evidence-layers.md) distinguishes any later checks that were reached.
