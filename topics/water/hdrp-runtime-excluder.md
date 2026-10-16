# HDRP runtime water-excluder construction

Adding `WaterExcluder` and calling `SetExclusionMesh` is not equivalent to the editor menu setup in HDRP 17.6. The inspected method assigns a mesh to an existing internal renderer. The editor command separately creates the renderer child, assigns the package exclusion material and then sets the mesh.

Runtime construction therefore requires the complete setup rather than the component and mesh assignment alone.

## Inspected source path

In Unity 6000.6.0f1 with HDRP 17.6.0:

1. `WaterSurfaceMenuItems.CreateWaterExcluder` creates a child with `MeshFilter` and `MeshRenderer`.
2. The menu assigns the package water-exclusion material.
3. The menu then supplies the mesh to `WaterExcluder`.
4. `WaterExcluder.SetExclusionMesh` uses the component's existing internal renderer rather than constructing that setup itself.

A present component or assigned mesh therefore does not prove that exclusion is operating. The renderer, material and required HDRP frame settings are additional parts of the observed setup.

## View limitation

The bundled documentation describes water exclusion as a screen-pixel effect for the water surface. It limits exclusion to above-surface views and says that an underwater view of the surface is not excluded. It also requires the relevant asset and camera frame settings.

The inspected behavior supports an above-surface rendering tool rather than a general dry volume. Occupied interiors, underwater post effects, surface visibility through windows and multiple observing cameras remain separate questions.

## Evidence and limits

This is source and bundled-document inspection only. No runtime scene, rendered frame or gameplay acceptance is claimed. A renderer and material configuration does not establish that a camera executed the intended stencil pass.

The inspected files were installed with Unity 6000.6.0f1 and HDRP 17.6.0:

- `Runtime/Water/WaterExcluder.cs`
- `Editor/Water/WaterSurfaceMenuItems.cs`
- `Documentation~/water-exclude-part-of-the-water-surface.md`

Reinspect those files when the Unity or HDRP version changes. For a runtime implementation, retain a complete versioned scene and build harness, then inspect exterior and interior frames during motion, including views below the surface.
