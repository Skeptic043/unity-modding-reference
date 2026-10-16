# Authoritative pose publication before support queries

After an actor is reset onto moving support, collision and support queries can observe different `Rigidbody`, `Transform` and child-station poses. Final support can hide a failed first contact followed by recovery.

[Physics](README.md)

## Query pose

The query pose depends on the authoritative pose writer, interpolation mode and reset order. `Rigidbody.position`, `Rigidbody.rotation`, the support `Transform` and the child station can differ immediately before and after reset, particularly when publication or `Physics.SyncTransforms` occurs after a child-coordinate read or contact query.

A focused fixture can assign the kinematic body's pose, publish it to the support `Transform`, then read the child station and query contact. That ordering must match the implementation under test rather than being treated as a universal Unity requirement.

## First contact and continuous support

Record both initial pose snapshots, the first support result, the first rejected ground or clearance query and the first detachment. Require initial and continuous support for the intended movement path. A final supported state is insufficient because the actor can fail placement and recover later.

Compare pose differences before changing collision dimensions or tolerances. In a new reproduction, run a focused support path first, then a complete movement fixture so a narrow diagnostic pass is not mistaken for whole-system acceptance.

## Evidence limits

A focused fixture does not establish behavior for dynamic platforms, another interpolation mode, other Unity versions, airborne attachment, networking or human comfort. Changes to the engine, pose writer, reset order, interpolation, station geometry or query shape require a new check.
