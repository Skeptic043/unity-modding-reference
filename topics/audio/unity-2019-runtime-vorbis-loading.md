# Runtime-loaded Vorbis used PCM-sized memory and could stall one Unity 2019.1 editor frame

In the tested non-streaming load path, setting `DownloadHandlerAudioClip.compressed = true` did not reduce the reported runtime memory for Ogg Vorbis. A separate run attributed roughly 470 to 480 ms of one editor frame to the owning code's completion-time `Tick` around `DownloadHandlerAudioClip.GetContent`.

[Audio](README.md)

## Memory measurements

A 14-minute, 44.1 kHz stereo Vorbis file was 2,399,601 bytes on disk and reported 148,322,828 runtime bytes with `compressed` both on and off. Sixteen-bit stereo PCM for the same duration is 148,176,000 bytes. Both requests took about 0.8 seconds to load.

A 20-minute, 44.1 kHz stereo Vorbis clip reported 211,833,957 runtime bytes, about 0.1 percent above its 16-bit PCM size, and loaded in about one second.

The reported sizes are consistent with decoded PCM-sized storage in this path. That is an inference from the measurements, not an inspection of Unity's native allocation internals.

## Completion-frame timing

A 4 minute 13 second, 44.1 kHz stereo Vorbis file produced a maximum measured `Tick` time of about 478 ms in both the cold and warm passes. The stopwatch wrapped the owning implementation's per-frame `Tick`, which calls `DownloadHandlerAudioClip.GetContent` after the request completes.

The MP3, FLAC and M4A rows ranged from 0.1 to 30.9 ms for their maximum `Tick` times, with the longer decode cases mostly around 25 ms. Those rows do not retain input identities or durations, and there are two different MP3 rows per pass. They are useful context, not a controlled equal-duration codec comparison.

The timing output identifies the expensive completion frame. It does not isolate whether decoding occurred inside `GetContent` or in a native completion step on that same frame.

## Tested environment

- Unity 2019.1.10f1 editor, revision `f007ed779b7a`
- Unity's bundled MonoBleedingEdge runtime
- Windows 11
- Batchmode editor with 48,000 Hz stereo output
- DSP buffer 1024 x 4
- Listener volume zero
- Local `file://` requests through `UnityWebRequestMultimedia.GetAudioClip`
- `streamAudio = false`
- FFmpeg 8.1.1 for the synthetic 14-minute and 20-minute tones

The measurements were inspected on September 26 and 27, 2026. The original 4 minute 13 second timing input was read from an HDD but was not retained, so that row remains a bounded historical observation rather than a reproducible controlled codec comparison.

## What this does not establish

Only the editor was measured. The captures do not establish standalone-player behavior, audible playback quality, allocation internals, `streamAudio = true`, background decoding in another API or the behavior of a newer Unity version.

## Evidence boundary

The original probe phases are not available in this repository, and the codec-comparison inputs were not all identified. Treat the measured sizes and timings as bounded historical observations rather than a reproducible current benchmark.
