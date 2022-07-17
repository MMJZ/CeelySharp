# CeelySharp

This repo fleshes out the C# implementations of the monads Maybe, List, and Task.

The async/await capabilities of Maybe and List are implementated, and then for all three monads Linq Query syntax is implemented in terms of identical async/await syntax.

The resulting Linq Query syntax implementation for List is nearly identical (excluding exception handling) to that implicitly provided by `IEnumerable<T>`, but with the following extra requirements:

- lists `await`ed in an async method body must be declared before the body start;
- lists must not be otherwise mutated during method execution; and
- lists must not be awaited more than once in the same method body.

These requirements follow from the restrictive underlying implementation of async/await in c#'s compiler.
