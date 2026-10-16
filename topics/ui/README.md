# UI

These articles separate input permission, action execution, overlay lifetime, layout geometry and displayed labels so one visible result is not mistaken for proof of the whole interaction.

- [Input targeting and action boundaries](input-action-boundaries.md): trace permission, execution and ownership as separate paths.
- [Overlay lifecycle and input ownership](overlay-lifecycle.md): define availability and restore cursor, pause and control state across every close path.
- [World-space menu layout](world-space-menu-layout.md): the shared layout of visuals, controls and colliders, including optional plugins and rotated geometry.
- [Displayed labels and gameplay effects](displayed-labels-and-effects.md): trace the label and the mechanical value independently.
- [Cached UI rendering](../rendering/cached-ui-rendering.md): use a graphics-enabled fixture and inspect pixels rather than treating resource allocation as proof of visible output.

The Sailwind [held-item targeting](https://github.com/Skeptic043/sailwind-technical-reference/blob/main/docs/research/input/held-item-targeting.md) and [native UI cache](https://github.com/Skeptic043/sailwind-technical-reference/blob/main/docs/research/ui/native-assets-cache-and-labels.md) cases provide concrete game-specific applications.

Source inspection and synthetic hierarchies do not establish live projection, hit testing, event propagation or readability.

[Unity modding reference](../../README.md)
