# On Unity 2019.1 x86 Mono, a ComImport Media Foundation reader crashed while manual vtable calls completed

A `[ComImport]` Media Foundation source-reader run ended with Windows exit code `-1073741819`, an access violation, after `MFStartup` and reader creation succeeded. A separate manual-vtable spike completed under the same Unity Mono host.

This comparison is a warning about that exact host and pair of spikes. It does not prove that `ComImport` always fails under Unity Mono or that manual vtable calls are always required.

[Interop](README.md)

## What the two spikes showed

The `[ComImport]` spike created an `IMFSourceReader` through `MFCreateSourceReaderFromURL`, then the process crashed before it printed a read result or its caught-error path. The output has no stack trace locating the fault.

The manual-vtable spike used `Marshal.GetDelegateForFunctionPointer` for interface slots and completed these operations:

- `MFStartup`
- `MFTEnumEx`
- `MFCreateSourceReaderFromURL`
- configuring float PCM output
- `IMFSourceReader.ReadSample`
- `IMFSourceReader.SetCurrentPosition`
- `GetPresentationAttribute(MF_PD_DURATION)`
- opening the reader on one thread and reading it on another

The programs also differ in COM initialization and output configuration. Their result does not isolate the invocation mechanism as the only cause of the crash.

## Duration, decoded frames and seek landing

For a synthetic three-second AAC-LC M4A made with FFmpeg, the manual-vtable reader reported:

- `MF_PD_DURATION = 30,000,000`, or 3.000 seconds
- 132,300 frames implied by that duration at 44.1 kHz
- 134,144 decoded frames
- a first output timestamp of 0
- 1,024-frame output packets

The extra decoded frames are consistent with the MP4 edit list not trimming the encoder's 1,024-frame priming from decoded output. That is an interpretation of the frame counts and container behavior, not an inspection of Media Foundation internals.

After `SetCurrentPosition`, the first returned timestamp landed at or before the requested position. One example returned frame 65,536 for a 66,150-frame target.

## Tested environment

- Unity 2019.1.10f1 MonoBleedingEdge `mono.exe`
- x86 standalone Mono host, not a built Unity player
- Mono 4.0.30319.42000
- Unity 2019.1.10f1 `mcs`
- Windows 11 Home 10.0.26200
- FFmpeg 8.1.1 for the synthetic AAC-LC M4A
- inspected September 27, 2026

## What this does not establish

The crash was not reproduced inside an x64 Unity player. The exact marshalling defect was not isolated. Audible output was not tested.

Changes to Unity, Mono, process architecture, Windows, Media Foundation components, COM setup, output configuration or input profile require a new check. HE-AAC and raw ADTS inputs remain separate tests.

## Reproduction

The [manual-vtable spike](../../evidence/audio/Spike.cs) and [ComImport comparison spike](../../evidence/audio/ComSpike.cs) are available with a [fresh-fixture recipe](../../evidence/audio/README.md). The two programs differ in COM setup and output configuration, so a matching rerun would reproduce the comparison without isolating the invocation mechanism as the sole cause.
