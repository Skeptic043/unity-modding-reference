# Deferred save capture and stale continuations

A check at the public save entry does not cover work that resumes later. Ownership, cancellation and cleanup therefore depend on the request's path through every yield or continuation to the point where data is actually captured.

[Persistence](README.md)

## Save-operation lifetime

The complete operation includes newly requested saves, already suspended saves and every caller that can resume them. Its relevant boundaries are the end of preflight, temporary-state changes, native capture and extension serialization.

An operation identity or generation distinguishes resumed work from a newer request. Rechecking that identity before resumed mutation or capture prevents an older continuation from writing or cleaning up state owned by a replacement operation after a scene change, disable or cancellation.

The operation contract includes timeout, cancellation and cleanup behavior for exceptions before and after temporary mutation. Cleanup releases only resources owned by that operation and remains safe when invoked more than once.

## Persistence-boundary evidence

Acceptance would cover overlapping requests, interruption at each suspension point, an exception before mutation, an exception after mutation and old work resuming after replacement. Native and extension capture ordering remain separate observations because adjacent public callbacks are not established as atomic.

Managed continuation tests can establish generation and cleanup behavior. They do not establish Unity scene timing, native serialization completeness, disk durability or successful save/reload. Live acceptance therefore belongs on disposable copied saves and includes switching save slots in one process.
