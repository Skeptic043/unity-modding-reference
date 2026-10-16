# Runtime audio failure boundaries

Runtime audio is a chain of separate boundaries: file discovery, decoding, main-thread completion, clip ownership, scheduling, output and synchronization. A success at one boundary proves little about the next. A decoder can return valid PCM while clip creation stalls a frame, an `AudioSource` can report playback while the final mix is silent, and two outputs can expose matching cursors while playing different buffered content.

[Audio](README.md)

## Discovery and decoding

File enumeration establishes that an input was found, while decoding establishes that one decoder accepted its bytes. Record the engine version, scripting backend, runtime, operating system, process architecture, audio device settings and decoder version, then reproduce the format with an owned file and a short synthetic fixture. Preserve the underlying error so corrupt data, unsupported codecs, missing dependencies and timeouts do not collapse into one generic failure.

Native codec interop is another boundary rather than a shortcut around this work. Test the exact runtime and architecture, and compare call paths, cross-thread reads, seek landing, reported duration and decoded frame count. A reproducible report should include the complete spike, input-generation command and output.

## Completion and clip memory

An asynchronous decode or web request can still do expensive main-thread work when it completes. Measure enumeration, request or decoder setup, completion, PCM upload and time to playing separately. Compare cold and warm runs, and prepare the next track while the current one is playing when the design permits it.

Compare runtime clip memory with decoded PCM size instead of assuming that a compression flag changes the resident data. Count retained clips and temporary upload buffers. For a streaming path, verify that memory stays bounded as duration grows, then test independent cursors, underruns and seeks. Keep decoder and disk work out of audio callbacks.

## Ownership and cancellation

Clip creation does not decide whether its result still has a valid owner. Exercise cancellation, stale completions, replacement, pause, owner destruction and device reset. Delayed work must carry enough identity to avoid attaching a clip to a replacement owner or reviving a track that has already been released.

Simulation pause, listener pause and the DSP clock are separate controls. A system that treats them as one state can resume at the wrong point, advance while the game appears paused or strand work that was scheduled against DSP time.

## Scheduling, output and synchronization

Scheduling establishes when Unity should begin consuming a clip. It does not establish that the device, mixer and listener produced audible sound. Measure time to playing and scheduled-start behavior separately from final output. A captured digital signal can establish timing or content alignment, while an audible test answers a different question about the device, mix and listener experience.

For synchronized outputs, compare content as well as cursor values. Native pre-read and mixer buffers can make matching cursors misleading. Test scheduled starts, late joins, long pauses, loops and output loss in the actual target rather than inferring synchronization from `time` or `timeSamples` alone.

## Evidence boundaries

The [Unity 2019.1 editor observations](README.md) are bounded examples of these boundaries. They do not establish the same behavior in a standalone player, a newer engine, another backend or another device.

Game-specific acoustic geometry, playlists, native-library adapters and startup policy belong with the game that uses them. Recheck these boundaries when the engine, decoder, device, input format or asynchronous ownership changes.
