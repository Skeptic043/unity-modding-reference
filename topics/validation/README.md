# Validation

These articles separate what an individual check establishes from the runtime behavior it cannot prove. They also show how to test a native operation's acceptance rules without mutating real state before the action is known to be valid.

- [Compatibility evidence layers](compatibility-evidence-layers.md): keep package, loader, static, synthetic, live and extended acceptance evidence separate, and correlate logs to the same session.
- [Isolated native-consumption simulation](native-consumption-simulation.md): consume detached mutable copies, inspect the residual requirements and recheck the real ordered sources before side effects.
- [Array-consumer inspection](../assembly-inspection/redirect-array-consumers.md): inspect typed locals, generated delegates and alternate readers before validating a redirected source view.
- [Runtime audio failure boundaries](../audio/investigating-runtime-audio.md): test discovery, decoding, clip ownership, scheduling and final output as distinct claims.

Earlier compatibility layers and managed simulations cannot establish Unity startup, real callback behavior, persistence, multiplayer, performance, long-session behavior or gameplay acceptance.

[Unity modding knowledge](../../README.md)
