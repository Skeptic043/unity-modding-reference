# Unity 2019.1 did not use one callback pattern for streamed clip positions

For an `AudioClip.Create(..., stream: true, reader, setPosition)` clip, Unity 2019.1 called the position callback during creation, scheduling, active or paused seeks, and looping. It did not call the callback for every operation, and looping callbacks arrived from a thread other than the main thread.

[Audio](README.md)

## Observed calls

The editor probe recorded these main-thread callbacks:

- `AudioClip.Create` called the callback with position 0. The later looping-clip creation called it twice.
- `AudioSource.PlayScheduled` called the callback once with a position of 44,100 after that position had been written. A later `PlayScheduled` without a new write called it twice.
- Writing `AudioSource.timeSamples` while playing or paused called the callback with the written position.

It recorded no callback for clip assignment, clip clearing, a `timeSamples` write while stopped, `Pause`, `UnPause`, or `Stop`.

A looping 0.5-second streamed clip played for 1.3 seconds called the position callback three times with position 0 from a thread other than the main thread during that run.

## Practical consequence

The evidence does not support correctness that depends on one exact callback count or on every callback running on the main thread. Reader state therefore has to account for the observed audio-thread loop callbacks, while the position callback requests a position for that clip's reader rather than establishing authority over shared decode state.

## Tested environment

- Unity 2019.1.10f1 editor, revision `f007ed779b7a`
- Unity's bundled MonoBleedingEdge runtime
- Windows 11
- Batchmode editor with 48,000 Hz stereo output
- DSP buffer 1024 x 4
- Listener volume zero

Calls were counted during each operation and for 0.3 seconds afterward. The looping observation used 1.3 seconds. The capture was inspected on September 27, 2026.

## What this does not establish

The capture is editor-only. Callback counts and threading can differ in a standalone player, another Unity version, another backend or another audio configuration.

## Evidence boundary

The matching original callback harness is not available in this repository. Treat the operation counts, positions and thread classifications as bounded historical observations, and rerun them before depending on the callback pattern in another environment.
