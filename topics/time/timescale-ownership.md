# Time-scale ownership and numeric equality

If native code writes the same `Time.timeScale` value a mod selected, equality cannot identify the current owner. Ownership requires the native and mod writers plus the lifecycle transitions around acquisition, release and restoration.

[Time](README.md)

## Ownership transitions

The ownership path includes every native and mod writer to `Time.timeScale`, related `Time.fixedDeltaTime` behavior, and the lifecycle markers around each write. Transition preparation can occur before the actual takeover.

Selected state, observed global value and ownership are separate facts. A menu, sleep or transition flag can be useful evidence for a known path, but the flag alone does not prove that the corresponding write has happened.

Review acquisition and every release path, including key release, configuration changes, errors, disable and destruction. Clearing mod ownership must not restore a default over native ownership, even when native and mod values match.

## Matching and different values

Exercise matching values, different values, marker combinations, blocked acquisition and release followed by reacquisition. Verify that native takeover remains authoritative and that a stale mod operation cannot restore an earlier value after the transition changes.

Static call ordering and managed fixtures can establish known branches and ownership rules. They do not establish Unity callback timing, physics stability, save behavior, another plugin's writes or gameplay acceptance. Changes to native writers, lifecycle markers, permitted speeds, restoration or cancellation paths require a new check.

The [Sailwind time-scale and sleep case](https://github.com/Skeptic043/sailwind-technical-reference/blob/main/docs/research/time/time-scale-and-sleep-ownership.md) applies this ownership distinction to one game's sleep, pause and transition paths. Its markers do not establish ownership in another game.
