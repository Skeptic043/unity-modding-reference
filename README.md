# Unity modding reference

Research findings and reproducible methods for Unity mod developers. Each article identifies the observed behavior or method, its practical implications, and the versions and limitations that bound the result.

## Browse by question

The subject pages remain the canonical homes for their articles. These broader routes group them by the kind of problem a reader is trying to solve, and some findings naturally cross more than one route.

### Runtime integration

- [APIs](topics/apis/README.md): side-effect-free state queries, scoped ownership handles and optional integration boundaries.
- [BepInEx](topics/bepinex/README.md): configuration identity, live changes, reload behavior, shortcut parsing and Configuration Manager ranges.
- [Dependencies](topics/dependencies/README.md): repeated failures caused by one unresolved managed dependency.
- [HarmonyX](topics/harmonyx/README.md): target preparation, transpiler input, prefix arguments, startup guards, temporary state and failed-install cleanup.
- [Interop](topics/interop/README.md): native API behavior under specific Unity, Mono, architecture and OS combinations.
- [Lifecycle](topics/lifecycle/README.md): object readiness and separate visual, physics, save and joint ownership.
- [Persistence](topics/persistence/README.md): durable instance identity and safe deferred save capture.
- [Time](topics/time/README.md): ownership when native code and a mod select the same time scale.

### Interaction and presentation

- [Input](topics/input/README.md): targeting permission, action dispatch and ownership changes traced as separate paths.
- [UI](topics/ui/README.md): input and action boundaries, overlay lifetime, world-space layout and displayed-label investigation.
- [Audio](topics/audio/README.md): runtime audio failure boundaries, measured Unity 2019.1 behavior and playback ownership.
- [Rendering](topics/rendering/README.md): HDRP exposure and batch capture, mesh and material reuse, and graphics-backed UI validation.

### Physics and world interaction

- [Physics](topics/physics/README.md): moving-support pose verification and operating envelopes derived from force and failure paths.
- [Placement](topics/placement/README.md): displacement behavior when a clearance search can absorb a translated origin.
- [Water](topics/water/README.md): Crest CPU query limits and source-bounded HDRP water exclusion and spectrum-unit findings.

### Inspection, validation and release

- [Assembly inspection](topics/assembly-inspection/README.md): managed PE metadata, resilient method-change checks and typed-consumer audits before array redirection.
- [Validation](topics/validation/README.md): compatibility evidence levels and bounded simulations of native consuming paths before real side effects.
- [Tools](topics/tools/README.md): read-only binary inspection helpers and the current Unity CLI and Pipeline route.
- [Packaging](topics/packaging/README.md): frozen Qt dependency diagnosis, plugin and bundle identity, and profile replacement rollback.

## Game-specific research

For game-specific mechanics, see the [Sailwind technical reference](https://github.com/Skeptic043/sailwind-technical-reference).

## Evidence scope

A result from source inspection or a desktop fixture does not automatically establish behavior in Unity gameplay. Each article identifies the tested environment and the limits that matter when applying it elsewhere.

See [Contributing to the public reference](CONTRIBUTING.md) for repository ownership, evidence, privacy, attribution and review expectations.

## Licensing

Original written explanations use CC BY 4.0. Facts and discoveries are outside that copyright claim. See the [license scope](LICENSE.md).
