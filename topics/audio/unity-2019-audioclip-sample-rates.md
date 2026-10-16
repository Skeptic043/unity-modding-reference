# Unity 2019.1 AudioClip playback advanced from 44.1 to 192 kHz

In the tested Unity 2019.1 editor, clips created at 44.1, 88.2, 96, 176.4 and 192 kHz played, remained non-virtual and advanced `AudioSource.timeSamples` at their nominal rate. This held for both PCM-reader streamed clips and clips filled with `AudioClip.SetData`.

[Audio](README.md)

## What was measured

The probe ran each clip for about 1.5 seconds and compared `timeSamples` advancement with DSP time and real time. The measured rates stayed within DSP-buffer quantization of nominal. At 176.4 kHz, for example, the probe observed 173,915 and 176,400 samples per second in its measurement windows.

The exact tested rates were:

- 44,100 Hz
- 88,200 Hz
- 96,000 Hz
- 176,400 Hz
- 192,000 Hz

## Tested environment

- Unity 2019.1.10f1 editor, revision `f007ed779b7a`
- Unity's bundled MonoBleedingEdge runtime
- Windows 11
- Batchmode editor
- 48,000 Hz stereo output
- DSP buffer 1024 x 4
- Listener volume zero

The measurement was inspected on September 26, 2026.

## What this does not establish

The probe measured cursor advancement, not audible quality or resampling quality. It did not test a standalone player, other output-device rates, rates above 192 kHz, IL2CPP or a different Unity version.

## Evidence boundary

The matching original rate-phase harness is not available in this repository. Treat these measurements as bounded historical observations and rerun the rate, cursor, playback and virtualization checks before relying on them in another environment.
