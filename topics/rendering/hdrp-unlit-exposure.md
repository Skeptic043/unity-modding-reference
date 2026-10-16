# HDRP 17.6 unlit base color and emission use different exposure paths

In the inspected HDRP 17.6 forward unlit pass, base color and emission do not receive the same exposure multiplier. Compensation copied from a lit material or from the other unlit color contribution therefore does not automatically apply to the active shader.

[Rendering](README.md)

## What the shader does

The fragment output passed this blend input to `ApplyBlendMode`:

- base color multiplied by `GetDeExposureMultiplier()`
- emissive color multiplied by `GetCurrentExposureMultiplier()`

The observation came from `ShaderPassForwardUnlit.hlsl` in Unity 6000.6.0f1 with HDRP 17.6.0. The expression was around line 165 in the forward unlit fragment output before optional transparent fog handling.

## Applicability

Applicability depends on the material's actual shader, pass and enabled variants. Lit and unlit test surfaces under the same camera and exposure settings can separate the base-color contribution from emission before either receives authored compensation.

A source match establishes the shader calculation, not the displayed brightness. Blend mode, alpha, color space, post effects, fog and the selected material properties still affect the final frame.

## Recheck boundary

Reinspect after changing Unity, HDRP, the shader pass, material type or exposure policy. Retain actual captures before choosing compensation values. This source inspection did not render a scene or establish perceptual equivalence to another shader graph.
