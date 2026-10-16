# HDRP spectrum wind-speed units

In HDRP 17.6, the ocean spectrum's `largeWindSpeed` value is converted from kilometres per hour to metres per second. A value of `23` represents 23 km/h, approximately 6.4 m/s, not 23 m/s.

The conversion makes the unit relevant to tuning code, configuration and diagnostic labels.

## Source observation

In the inspected `WaterSurface.Simulation.cs`, `EvaluateSpectrumParams` multiplies `largeWindSpeed` by `WaterConsts.k_KilometerPerHourToMeterPerSecond` before assigning the spectrum band's wind speed.

The inspected file came from Unity 6000.6.0f1 with HDRP 17.6.0:

- `Runtime/Water/WaterSurface/WaterSurface.Simulation.cs`

## Evidence and limits

This is installed source inspection only. It does not establish a particular wave height, believable storm, render and query agreement or vessel response. Patch size, amplitude multipliers, band weighting, viewing distance and the consuming physics model still need evaluation together. A surface normal at one point is not a validated orientation model for a large hull.

Reinspect `EvaluateSpectrumParams` when the HDRP version or spectrum setup changes. Compare runtime appearance and vessel response over time before accepting new tuning.
