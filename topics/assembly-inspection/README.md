# Assembly inspection

Articles and tools for examining managed assemblies without treating metadata as runtime proof.

- [Inspect managed PE metadata without loading an assembly](managed-pe-metadata.md): a bounded read-only inspector for architecture, CLI headers, metadata producer strings and file identity.
- [Raw IL hashes change after unrelated edits](raw-il-hashes.md): why metadata tokens make `GetILAsByteArray()` hashes brittle as compatibility guards.
- [Typed array consumers and redirection boundaries](redirect-array-consumers.md): field and local signatures, generated delegates, alternate consumers and source ownership around a redirected read.

[Unity modding reference](../../README.md)
