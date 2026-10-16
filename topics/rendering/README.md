# Rendering

These articles focus on proving what Unity actually rendered and on preserving the mesh, material and shader assumptions that make a visual result meaningful.

- [HDRP unlit base color and emission use different exposure paths](hdrp-unlit-exposure.md): inspect the active shader pass before applying exposure compensation.
- [Graphics-enabled HDRP batch capture](graphics-enabled-batch-capture.md): submit an explicit render request, wait for the target camera callback and inspect the captured pixels.
- [Mesh and material reuse](mesh-material-reuse.md): verify UVs, material roles, dependencies and object lifetime before reusing a native visual.
- [Cached UI rendering](cached-ui-rendering.md): exercise the production renderer with real graphics and inspect the result instead of treating resource allocation as proof of visible content.

The [Sailwind native UI cache case](https://github.com/Skeptic043/sailwind-technical-reference/blob/main/docs/research/ui/native-assets-cache-and-labels.md) keeps the game-specific asset and menu context.

Each article separates source inspection, isolated graphics fixtures and actual in-game appearance.

[Unity modding reference](../../README.md)
