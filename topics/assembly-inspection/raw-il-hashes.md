# Raw IL body hashes change when unrelated metadata tokens move

A raw `MethodBody.GetILAsByteArray()` hash is not a stable general game-compatibility guard. Unrelated edits elsewhere in the assembly can renumber metadata operands and change the method's bytes without changing its source or resolved instruction sequence.

[Assembly inspection](README.md)

## What the reproduction changed

The [reproduction](../../evidence/il-body-hash-tokens-20261004/Run.ps1) compiles the same `Probe.Target.Check` source twice. The second build only adds a field, a method and a string literal to an earlier, unrelated class. The two raw IL byte arrays and their SHA256 hashes differ. Decoding the opcodes and resolving each operand with `Module.ResolveMember` or `Module.ResolveString` produces identical sequences in both builds. The [expected result](../../evidence/il-body-hash-tokens-20261004/results.txt) records both forms.

All three token kinds used by the method shifted:

| Operand | First build | Second build |
| --- | --- | --- |
| Field definition | `0x04000001` | `0x04000002` |
| `Console.WriteLine` member reference | `0x0a00000c` | `0x0a00000b` |
| User string | `0x70000001` | `0x7000000d` |

Field and method definition tokens are row numbers in metadata tables. Member references and the user-string heap are assigned during emission. An edit elsewhere in the assembly can therefore change raw IL bytes without changing what the inspected method resolves to.

## Guard applicability

A mod that compares a method's raw IL hash with a stored value can reject a later game build even when the targeted method is untouched. Comparing opcodes with resolved operand names survives this token renumbering, but it still is not a universal semantic comparison. Compiler or rewriter changes can legitimately alter the instruction sequence.

A narrower guard can compare the specific instructions and members on which the patch depends. Missing requirements can leave the method unpatched without throwing. A throwing transpiler can also run when another mod later patches the same method, which can make that mod appear responsible for the failure.

## Evidence and applicability

The reproduction builds deterministic Release assemblies targeting .NET Framework 4.7.2, then compares raw method bytes with decoded instructions and resolved operands. The [expected result](../../evidence/il-body-hash-tokens-20261004/results.txt) records the changed tokens and matching normalized sequence.

This tested one synthetic assembly and one compiler. Token assignment is an emitter detail, not a promise that every update changes a token. Unity was not involved. The bytes came from compiled metadata, so the finding concerns a shipped managed assembly rather than runtime execution or gameplay.

The harness builds `lib/` with and without `EXTRA`, then runs `compare/` on both outputs in a temporary directory and removes it. It requires a .NET SDK that can build net472. A different compiler, obfuscator, IL rewriter or metadata emitter, or a guard that hashes something other than raw IL bytes, requires a new check.
