# BepInEx

Articles about BepInEx configuration identity, live changes, reload behavior and common editor interactions. Each article identifies the exact loader or Configuration Manager version that was inspected.

- [Config identity and section ordering](config-identity-and-ordering.md): why section and key names define a setting's saved identity, while binding order does not control section order in the inspected implementation.
- [Runtime config changes and `Reload()`](runtime-config-and-reload.md): how setting events, automatic saves and reload ordering affect live configuration.
- [Shortcut parse failures](shortcut-parse-loss.md): how invalid shortcut text can become an empty saved binding.
- [Configuration Manager range sliders](configuration-manager-range-sliders.md): what range metadata provides, what it does not provide and when loader clamping hides invalid inputs.

[Unity modding knowledge](../../README.md)
