# Optional integration boundaries at load and runtime

An integration is optional only when the consumer can load before the provider is available and can continue ordinary operation when the provider is absent or unusable. Provider-absent, old, disabled, malformed and ready states expose different parts of that boundary.

[APIs](README.md)

This is an engineering pattern for an integration such as a consumer mod that can use another mod's service when available but must still load and retain ordinary behavior without it. It is not a report of native Unity behavior or proof that a particular provider is compatible.

## Discovery and readiness

Loader discovery, dependency metadata and type availability answer different questions. Optional provider types may need to remain behind a boundary that the consumer can load without resolving, while contract and readiness checks happen before use.

Absent and unavailable providers need an established fallback. In the pattern described here, the consumer retains its ordinary behavior without leaving partially registered callbacks, handles or UI state.

Teardown can occur in either direction: the consumer can disable before the provider, the provider can disappear first, and a scene or session transition can invalidate both. Cleanup is limited to registrations owned by the consumer and tolerates a provider that is already unavailable.

## Evidence limits

Static reference inspection can reveal direct type dependencies and metadata. Loader fixtures can exercise selected version and absence cases. Neither establishes startup order, disable order, scene behavior or compatibility in a Unity player. A change to the loader, provider contract or consumer assembly requires a new check of all three.

[Compatibility evidence levels](../validation/compatibility-evidence-layers.md) keeps provider discovery, synthetic contract tests and live integration evidence distinct.
