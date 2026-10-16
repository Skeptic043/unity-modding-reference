# World-space menu geometry and controls

For a world-space menu shared by multiple mods, the interactive root, backing geometry and colliders form one layout. A screenshot or bounding rectangle alone cannot establish clearance, readable text or clickable controls.

[UI](README.md)

## Actual hierarchy

The effective hierarchy includes direct children, panel children, rotated transform axes, backing meshes, button colliders and pointer targets. A decorative panel may not own the controls drawn over it. Uniform glyph scaling also depends on the native `TextMesh` font, renderer material, character size, font size and style together.

A projected-silhouette comparison covers only the horizontal range where objects overlap, including curls and other protrusions in that range. Usable interior and decorative bounds are separate measurements. Screenshot-derived pixels remain estimates until the revised layout is observed in the game.

## Optional UI and stable layout baselines

Stable optional-UI behavior depends on a defined layout for each plugin state and on when plugin presence becomes known relative to menu construction. Positions applied from a stable baseline avoid accumulating another offset on every update or reopen.

Native click components and the ordinary transforms, headings and actions restored on close remain part of the layout state. Complete validation would cover every category-toggle combination, hidden colliders, tab availability, save, cancel, navigation and return from subpages.

Hiding a decorative object therefore depends on any late-attached input components and action-time lookup. Unity 2019.1's [`GetComponentInChildren`](https://docs.unity3d.com/2019.1/Documentation/ScriptReference/Component.GetComponentInChildren.html) documentation describes the default search as using active `GameObject` instances. An input object that another component still needs cannot be removed solely as decoration. Its keyboard updates and displayed text are separate state that can be stopped while hidden and restored on return.

## Evidence limits

Geometry calculations and simulated hierarchies cannot establish live projection, hit testing, event propagation, final contrast or lifecycle order across installations. Live evidence requires the complete menu in the game at the original display size and at another supported aspect ratio or resolution.
