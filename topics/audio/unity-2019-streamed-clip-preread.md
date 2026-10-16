# Unity 2019.1 pre-read 0.8 seconds from a streamed clip before playback

For a 44.1 kHz streamed clip, Unity called the PCM reader for 35,280 frames immediately after creation. It read the same amount again after `PlayScheduled` moved the clip position to sample 44,100.

[Audio](README.md)

## Observed pre-read

Each pre-read consisted of 17 calls for 2,048 frames and one call for 464 frames. At 44.1 kHz, the 35,280-frame total is 0.8 seconds.

This means a streaming implementation must be ready to supply data before audible playback begins. The capture does not establish that every Unity version, clip rate or device uses the same pre-read size.

## Measured multi-output alignment

The same run measured decoded content from one built-in output and three speaker outputs in 14 labeled scenarios. They covered:

- starting all outputs at frame 0
- speakers leaving and joining
- starting from a saved nonzero position
- pause and resume
- power cycling
- track changes with and without a pre-opened stream
- a deliberately displaced cursor and drift repair
- a natively stopped speaker and repair
- a one-second loop
- a slow-decoder simulation

The reported steady-window content offsets were 0.0 samples in all 14 scenarios. The deliberately displaced speaker recorded one jump during its repair scenario. Other outputs reported zero jumps and gaps.

The harness accepted a one-DSP-buffer tolerance and formatted offsets to one decimal place. These results show agreement within that measurement, not mathematically exact synchronization or audible equivalence.

## Tested environment

- Unity 2019.1.10f1 editor, revision `f007ed779b7a`
- Unity's bundled MonoBleedingEdge runtime
- Windows 11
- Batchmode editor with 48,000 Hz output
- DSP buffer 1024 x 4
- Position-encoding test signal measured through `OnAudioFilterRead`
- Speaker filters bypassed on measured outputs
- Final mix silenced

The callback-size row directly covers a 44.1 kHz clip. The synchronization fixture used 48 kHz. The output was inspected on September 28, 2026.

## What this does not establish

The capture does not test a standalone player, audible output, other device rates or another Unity version. A prior note reported a 48 kHz pre-read size, but no separate callback-size row for that rate is retained. Readiness for the opening buffer and engine-owned reader positioning are design inferences that still need testing in the target player.

## Evidence boundary

The original Unity fixture is not available in this repository. Treat the callback sizes and 14 scenario results as bounded historical observations, and reproduce the pre-read and alignment checks before relying on them in another player or audio configuration.
