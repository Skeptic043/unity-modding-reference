# Crest CPU Query integration limits

The NWH Water Assets integration described by its manual requires Crest CPU Queries because the integration assembly references the CPU Queries assembly directly. The inspected Crest CPU Queries documentation also records simulation limits that affect the applicability of a water-driven physics path.

## What the documentation establishes

The NWH manual inspected on 2026-09-15 says its shipped Crest integration requires Crest CPU Queries in editor and client projects. Without that dependency, the integration assembly can fail to import. The documented CPU Queries assembly identity is `WaveHarmonic.Crest.CPUQueries`. The manual targets Crest 5 or later and names different integration assemblies before and after UPM 2.4.0. These are claims about the documented NWH integration, not every possible Crest integration.

The Crest CPU Queries manual inspected on the same date says query collision data is baked from `ShapeFFT` components. Since CPU Queries 1.1.0, multiple baked components can be blended by changing their `Weight` values. The manual also lists these unsupported behaviors:

- changing Wind Speed at runtime
- wave attenuation from depth probes
- Water Level simulation

If visual and query results need to agree, the manual recommends disabling unsupported depth attenuation.

## Integration questions left open by the documentation

Extending this finding to a runtime integration would require evidence of:

- the exact Crest, CPU Queries and NWH package versions
- the installed assembly definitions and integration assembly name
- support for the query features required by that integration
- calm-to-storm blend behavior at fixed locations and times, with query height, normal, depth attenuation and sea-level configuration recorded
- the authoritative query and force path without graphics, if a dedicated host is required

A runtime claim would need a complete versioned harness and its observations. No such tests were performed for this article. Documentation alone does not establish runtime behavior.

## Evidence and limits

This page is based only on official web documentation inspected on 2026-09-15. No package source, installation, Unity build, runtime query or headless process was inspected. Exact installed references, performance, visual and physics agreement during transitions, package compatibility and licensing remain unverified.

The documentation does not establish that CPU conditions are always static, that every Crest integration requires this dependency or that GPU behavior has the same limitations. Changes to package versions or required water features require a new inspection of the current manuals and installed assembly definitions.

## Sources

- [NWH Dynamic Water Physics Water Assets manual](https://nwhcoding.com/DynamicWaterPhysics/manual/WaterAssets.html), inspected 2026-09-15. See the Crest requirements, assembly names and missing-assembly troubleshooting.
- [Crest CPU Queries manual](https://docs.crest.waveharmonic.com/Packages/CPUQueries/Manual.html), inspected 2026-09-15. See changing conditions and limitations.
