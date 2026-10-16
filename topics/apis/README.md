# APIs

These articles cover three boundaries that make optional mod APIs easier to reason about: observation without mutation, independently owned resource handles and integration that remains optional during loading as well as execution.

- [Side-effect-free state](side-effect-free-state.md): polling a status query must not advance or clear the state being observed.
- [Scoped ownership handles](scoped-ownership-handles.md): each caller releases only its own claim, including across disable, scene and session boundaries.
- [Optional integration boundaries](optional-integration-boundaries.md): test absent, old, disabled and ready providers without assuming discovery means readiness.

These are engineering and contract-review patterns, not native Unity behavior, documentation for a released API or proof of compatibility with a particular provider.

[Unity modding knowledge](../../README.md)
