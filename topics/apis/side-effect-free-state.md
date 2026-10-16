# Side-effect-free API state queries

A status query that changes provider state cannot be polled without changing the behavior being observed. Separating state advancement from observation permits an immutable snapshot whose capture timing and meaning are explicit.

[APIs](README.md)

This is an engineering pattern for a consumer that polls whether an operation is available, blocked or owned. A UI indicator or optional integration should be able to read that state without consuming permission, clearing a pending reason or advancing the provider.

## Query-path behavior

The complete helper path determines whether a proposed getter is observational. A method that returns a reason or availability value can also clear permissions, consume pending work or refresh internal ownership. In a side-effect-free contract, polling frequency does not change behavior.

Related fields captured together after the provider finishes its mutation form one consistent observation. Snapshot age and readiness remain distinguishable from an ordinary available state, avoiding several getters that can observe different moments or trigger different transitions.

For APIs that publish notifications, mutation completes before consumers are invoked. Exception isolation, reentrancy, ordering and subscription cleanup are part of that contract. Events add no value when command results and explicit snapshots already cover the consumer need.

## Evidence limits

Source inspection and managed tests can establish that known query paths are pure and snapshots are internally consistent. They do not establish Unity callback order, thread safety outside the tested contract, UI visibility or fault recovery in a running game. Changes to ownership, snapshot timing or notification behavior require a new check.
