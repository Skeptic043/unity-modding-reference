# Audio

These articles separate measured Unity audio behavior from assumptions about codecs, memory, scheduling and audible output. The recorded measurements use Unity 2019.1.10f1 in a Windows editor unless a page says otherwise.

- [Runtime-loaded Vorbis used PCM-sized memory and could stall one editor frame](unity-2019-runtime-vorbis-loading.md): `compressed = true` did not reduce the reported memory in the tested non-streaming path, and one retained Vorbis input produced a roughly 470 to 480 ms completion-frame stall.
- [AudioClip playback advanced from 44.1 to 192 kHz](unity-2019-audioclip-sample-rates.md): streamed reader clips and `SetData` clips advanced at their nominal rates in the tested editor.
- [When Unity calls a streamed clip's position callback](unity-2019-streamed-clip-position-callback.md): creation, scheduling, seeking and looping did not use one callback count or one thread.
- [Streamed clips pre-read before playback](unity-2019-streamed-clip-preread.md): the tested clip read 0.8 seconds at creation and again after scheduling moved its position.
- [Runtime audio failure boundaries](investigating-runtime-audio.md): separate discovery, decoding, main-thread completion, clip memory, ownership, scheduling, output and synchronization.
- [One playback timeline for multiple outputs](single-playback-timeline.md): keep decoding and queue progression with one source while eligible speakers join its current position.

For the related Windows native API result, see [Media Foundation on Unity Mono](../interop/media-foundation-unity-mono.md).

[Unity modding reference](../../README.md)
