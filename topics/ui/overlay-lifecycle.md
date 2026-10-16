# Overlay lifetime and action prerequisites

An overlay's availability and ownership can differ from the actions displayed inside it. A player-object prerequisite can otherwise decide whether a read-only window exists, while adopting a native menu's pause or input ownership can add behavior unrelated to the overlay itself.

[UI](README.md)

## Availability states

The availability states include the title screen, loading, sleep, absence of a player or primary world object and an in-progress operation. The effective lifecycle also includes hotkey entry, early returns, the open predicate, next-frame auto-close, tabs and transitions.

Inspect every read and write of the state that keeps the overlay open. Keep action prerequisites separate from window visibility so null-safe labels and catalogs can remain available when gameplay objects are missing.

## Production lifecycle

Drive the real open, update and close path. A passing helper cannot detect an early return in its caller. Test native transitions both before opening and while the overlay is already open.

Verify cursor restoration, mouse, scroll, keyboard and controller click-through, pause ownership and interruption cleanup. A loading overlay must not stop the load from completing. Delayed callbacks must not reopen a closed overlay or attach work to a new scene owner.

## Evidence limits

Source inspection and managed lifecycle tests cannot establish Unity event propagation, focus, projection or live input behavior. Recheck on a fresh boot, scene return, reopen, interruption and every supported input route.
