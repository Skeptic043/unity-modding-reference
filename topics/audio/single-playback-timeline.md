# Shared playback timelines across multiple outputs

When several speakers represent one playing source, a shared-timeline design gives one component ownership of decoding, position and queue progression. Additional outputs join that timeline instead of starting another decoder or advancing an independent queue.

[Audio](README.md)

This is an engineering pattern, not native Unity behavior. It must be implemented and validated by the audio system that adopts it.

## Playback, eligibility and sound

The source timeline owns the active track, decoder state, scheduled position and queue transitions. An eligible output shares the decoded clip or stream and joins at the source's current position.

Each output can still own its volume, filtering, spatial blend and frequency character. Shared playback does not require every speaker to sound the same, and different sound does not require separate playback state.

The design separates three decisions:

- which source owns playback and queue progression
- which outputs are eligible to join that source
- how each eligible output sounds at the listener

Joining, leaving or changing an output route does not by itself restart the track. Carried, cached or temporarily unavailable outputs require defined eligibility if stale ownership would otherwise create a second timeline.

## Lifecycle and arbitration

Pause, loading, device reset, owner destruction and simulation-speed changes require explicit timeline behavior because a real-time audio clock does not decide those policies by itself. In the described design, delayed decode or scheduling work cannot attach to a replacement owner after the original timeline has been cancelled.

If only one source may be active, define how a new source displaces the old one and whether removing the winner reactivates anything. Automatic fallback transfers ownership, so it should be a deliberate and tested rule rather than a side effect.

## Evidence limits

Managed tests can establish arbitration, cancellation and state transitions. Editor fixtures can exercise scheduling and cursor behavior under a named Unity and audio-device configuration. Neither establishes audible synchronization, spatial results, device recovery, platform decoder behavior, save/reload or long-session stability in a game.
