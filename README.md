# CeelySharp

This repo fleshes out the C# implementations of the monads `Maybe`, `List`, and `Task`.

The async/await capabilities of `Maybe` and `List` are implemented, and then Linq Query syntax is implemented in terms of identical async/await syntax for all three monads.

### Linq Query Syntax Equivalence

The resulting Linq Query syntax implementation for `List<T>` is nearly identical (excluding exception handling) to that implicitly provided by `IEnumerable<T>`, but with the following extra requirements:

- lists awaited in an async method body must be declared before the body start;
- lists must not be otherwise mutated during method execution; and
- lists must not be awaited more than once in the same method body.

The resulting Linq Query syntax implementation for `Maybe` is identical to what would be provided if `Maybe<T>` implemented `IEnumerable<T>` with:

- `Some<T>(T item).GetEnumerator() => new [] { item }.GetEnumerator()`; and
- `None<T>().GetEnumerator() => Array.Empty<T>.GetEnumerator()`.

These requirements follow from the restrictive underlying implementation of async/await in C#'s compiler.
