# Use an explicit render request for HDRP batch capture

For a bounded HDRP capture in a batch player, keep graphics enabled, submit a render request for the target camera and wait for that camera's completed render callback before reading the render texture. Process completion and a created texture do not prove that the intended content reached the image.

[Rendering](README.md)

## Capture sequence

1. Build a small diagnostic scene with an identified target camera and known visible fixture.
2. Launch the standalone player with `-batchmode` but without `-nographics`.
3. Create the target `RenderTexture` and submit `RenderPipeline.StandardRequest` through `RenderPipeline.SubmitRenderRequest`.
4. Observe `RenderPipelineManager.endContextRendering` and count only callbacks that include the target camera.
5. Read and save the texture only after the expected target-camera render has completed.
6. Bound the process with a timeout and record the player exit, renderer, graphics API, GPU, frame count, capture dimensions and artifact hashes.
7. Inspect the actual image for the intended geometry, materials and silhouette. Pixel variation alone cannot identify the visible subject.

## Preserve the whole experiment

Keep the runtime component, editor scene setup, build steps, sanitized run reports and rendered image together. Verify that the input project did not change during the run. An edited callback or scene is a new experiment and needs its own identity.

Raw Unity licensing logs can contain account information, so keep them private and retain only the sanitized facts needed to support the result.

## Evidence limits

This method does not establish rendering with `-nographics`, automatic camera scheduling in batch mode, interactive input, a performance target or compatibility with another HDRP version, graphics API or GPU. Repeat the complete capture after any of those inputs or the renderer initialization path changes.
