# Scoped ownership handles for shared restrictions

When several consumers can restrict or reserve shared behavior, a distinct identity for each acquisition prevents one release from removing another caller's claim. The pattern does not automatically resume an earlier action when the last handle is released.

[APIs](README.md)

This is an engineering pattern for shared behavior such as several mods independently blocking fast-forward, reserving a camera mode or suppressing one service. It is not a built-in Unity ownership model or evidence that a specific API implements the pattern.

## Ownership and release

The described contract makes disposal idempotent and rejects stale handles after provider or session generations change. Active restrictions are combined according to the published policy while each identity remains distinct, so one consumer cannot globally unblock another.

Explicit disposal, owner disable or destruction, configuration disable, scene unload and session reset are separate release conditions. Weak owner references can clean up collected owners, but they do not detect a live faulted consumer or a handle that was lost while its owner remained alive. A timeout can remove a claim that is still needed, which prevents it from providing universal cleanup.

Release and activation are distinct transitions. Removing the final restriction exposes the new available state without recreating a previous input or operation that may no longer be valid.

## Evidence limits

Managed tests can establish identity isolation, idempotent release, generation invalidation and aggregate state. They do not establish Unity destruction timing, scene ordering, recovery after arbitrary third-party faults or whether resuming shared behavior is safe. Test the actual provider lifecycle before publishing a contract.
