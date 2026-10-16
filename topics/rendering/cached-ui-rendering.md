# Cached UI rendering evidence

A created render texture or successful render call does not prove that a cached UI image contains the intended pixels. Graphics-backed evidence requires the production renderer, the source asset's relevant import constraints and inspection of the captured image.

[Rendering](README.md)

## Representative fixture

A representative fixture should use independently acquired assets read-only and keep extracted proprietary inputs private. Its evidence identity includes game-file hashes, editor version, graphics device and backend, and the exact production source.

Representative conditions include the native readability constraints. If the source mesh is non-readable, a readable fixture removes the condition that matters. An offline decoder reading vertices does not establish that the game's runtime permits CPU access through `Mesh.isReadable`.

The actual rendering path includes its camera, render texture and material handling. A simplified drawing sample is evidence for a different path rather than equivalent production evidence.

## Pixel and ownership evidence

Check the saved image's center opacity, transparent edges, silhouette and text area. Exercise the minimum, default and maximum supported sizes. Record the native font, material, character size, font size, style and glyph measurements when text fit matters.

Verify cleanup of owned render textures, materials, command buffers and temporary cameras. When comparing an old renderer with a replacement, run the reported failing path first in a fresh process or graphics context. A successful new camera render can change graphics state used by a later immediate draw.

## Fixture evidence and game acceptance

A nonblank fixture can detect an empty cache or a broken isolated render path. It cannot establish the desired style, readability, in-game lifecycle, global render state or compatibility across display settings and coinstalled plugins.

If both implementations work in isolation, retain that negative result and leave the original in-game cause unresolved. Recheck the actual modded game before claiming that a reported rendering failure is reproduced or fixed.

The [Sailwind native UI asset and cache case](https://github.com/Skeptic043/sailwind-technical-reference/blob/main/docs/research/ui/native-assets-cache-and-labels.md) applies this fixture boundary to one game's cached menu rendering. Its asset identities and visual expectations do not transfer automatically to another project.
