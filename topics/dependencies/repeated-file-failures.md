# Repeated file failures can come from one missing managed dependency

When many files fail through the same consumer, one unresolved shared dependency can make every compatible input fail separately. Matching exception and assembly identities distinguish that pattern from several independent bad inputs.

[Dependencies](README.md)

## Dependency diagnosis

A bounded comparison uses a known-valid synthetic input with the actual consumer binary in fresh processes on the relevant host. The only intended difference is whether the pinned companion assembly is present beside the consumer.

The failing case is identified by the exact exception and dependency version. The successful case exercises the consumer operation itself rather than only the process exit code, and normal assembly resolution remains observable when the fixture does not explicitly preload the companion.

A loader message proves failed resolution, not physical absence from a particular installation. A claim that a file is missing therefore depends on inspection of the active profile or direct user evidence. A package check covers the complete runtime inventory through a fresh extraction and input hashes, including companion libraries and their licenses.

## Game-specific evidence ownership

In one October 2026 Sailwind Radio case, repeated decode warnings shared the same unresolved NLayer assembly identity. A fresh-process synthetic comparison reproduced the decoder failure without the companion assembly and opened the same valid input when the pinned dependency was present. A later user retry with the complete bundle no longer showed the recorded missing-decoder loop in the selected logs.

The [Sailwind Radio package-dependency case](https://github.com/Skeptic043/sailwind-technical-reference/blob/main/docs/research/compatibility/radio-package-dependency-case.md) summarizes the recorded warnings, later retry and evidence limits. The case supports one candidate and dependency bundle rather than a general claim about Radio, NLayer or MP3 files.

## Evidence and applicability

The method can establish that a pinned dependency changes the tested consumer operation under the named host. It does not establish live feature behavior, long inputs, other formats, another platform, a cure for every symptom or exhaustive use of the consumer.

Personal logs and user inputs do not belong in the general article. The finding needs rechecking after the host, dependency identity, resolution rules, package layout or exception category changes. [Compatibility evidence levels](../validation/compatibility-evidence-layers.md) distinguishes any later runtime checks that were completed.
