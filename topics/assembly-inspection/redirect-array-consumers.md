# Typed array consumers and redirection boundaries

Replacing a shared array at one field load is insufficient when downstream code expects a more specific array type, reads the field again from generated code or uses a different source for UI. The effective boundary includes field and local signatures, every affected consumer, and ownership of the objects in the replacement view.

## Why the apparent call boundary is incomplete

Decompiled C# can hide the local types and generated methods that constrain a replacement. A helper returning an array of a base class cannot replace a derived-array load if the result is stored in a derived-array local. A generated predicate or continuation can also read the original field outside the parent method that appeared to own the operation.

UI and quantity helpers may follow a third route. For example, a detail panel can inspect slot controllers while consumption reads raw stack objects. Redirecting only one path can make the display and the action disagree.

## Inspection method

1. Record the input assembly's build, backend, SHA-256 and module MVID. Decode its PE metadata and IL without loading or executing it.
2. Inspect field signatures and declared local signatures as well as decompiled source.
3. Trace each `ldfld` through `stloc`, `ldloc`, array element operations such as `ldelem.ref`, calls, branches and address uses. If a replacement changes the array type, remap each affected local and all its uses explicitly or choose another verified adaptation.
4. Follow referenced delegates, closures and compiler-generated bodies. Treat generated names as observations for that exact build, not stable APIs.
5. Inspect UI, quantity and validation helpers separately. Adapt the narrow caller to the same source view instead of changing a global container API.
6. Preserve native ownership. A temporary array of original object references can redirect a read without replacing the saved container. Copying the ingredient objects themselves can separate mutation from the real inventory.
7. Keep one ordered view across action validation, callbacks and indexed consumption. Do not compact the view as entries become empty. Preserve explicit caller-supplied arrays and prevent recursive refresh while native consumption runs.
8. Record exact targets, read counts, local declarations, other consumers and exceptions. Then inspect the transformed IL and validate callbacks, conservation and persistence separately.

## Evidence boundary

This method supports a static consumer inventory for one identified assembly. Direct call records do not exhaust virtual, reflection or third-party callers, and static inspection does not establish a working runtime patch. A project still needs to verify transformed IL, patch wrappers, callbacks, conservation, persistence and gameplay against its own target.

Reinspect whenever the input assembly, compiler-generated bodies, local signatures, patch library or source ownership changes.
