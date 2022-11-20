using System;
using System.Threading.Tasks;
using CeelySharp.ListMonad;
using CeelySharp.MaybeMonad;
using CeelySharp.TaskMonad;
using NUnit.Framework;
using Void = CeelySharp.Base.Void;

namespace CeelySharp.Test;

[TestFixture]
public class ProductTest
{
    private readonly record struct ListTestCase((int, int)[] Expected, MList<int> Tm, MList<int> Um,
        Predicate<int> Predicate);
    private readonly record struct MaybeTestCase(Maybe<(int, int)> Expected, Maybe<int> Tm, Maybe<int> Um,
        Predicate<int> Predicate);

    [Test]
    public void ListProduct()
    {
        var a = new MList<int> { 1, 2, 3 };
        var b = new MList<int> { 4, 5, 6 };
        var cases = new ListTestCase[]
        {
            new(new[] { (1, 4), (1, 5), (1, 6), (2, 4), (2, 5), (2, 6), (3, 4), (3, 5), (3, 6) },
                a, b, _ => true),
            new(new[] { (2, 4), (2, 5), (2, 6), (3, 4), (3, 5), (3, 6) },
                a, b, x => x > 1),
            new(new[] { (1, 4), (1, 5), (1, 6), (3, 4), (3, 5), (3, 6) },
                a, b, x => x != 2),
            new(Array.Empty<(int, int)>(),
                a, new MList<int>(), _ => true),
            new(Array.Empty<(int, int)>(),
                a, b, x => false)
        };

        foreach (var (expected, tm, um, predicate) in cases)
        {
            CollectionAssert.AreEqual(expected, Product(tm, um, predicate));
            CollectionAssert.AreEqual(expected, ProductAsync(tm, um, predicate));
        }
    }

    [Test]
    public void MaybeProduct()
    {
        var a = new Some<int>(1);
        var b = new Some<int>(2);
        var cases = new MaybeTestCase[]
        {
            new(new Some<(int, int)>((1, 2)),
                a, b, _ => true),
            new(new None<(int, int)>(),
                a, new None<int>(), _ => true),
            new(new None<(int, int)>(),
                a, b, x => x != 1)
        };

        foreach (var (expected, tm, um, predicate) in cases)
        {
            Assert.AreEqual(expected, Product(tm, um, predicate));
            Assert.AreEqual(expected, ProductAsync(tm, um, predicate));
        }
    }

    [Test]
    public void PassPredicateTaskProduct()
    {
        Assert.AreEqual((1, 2),
            Product(Task.FromResult(1), Task.FromResult(2), x => x == 1).Result);
        Assert.AreEqual((1, 2),
            ProductAsync(Task.FromResult(1), Task.FromResult(2), x => x == 1).Result);
    }

    [Explicit /* ;) */, Test]
    public void FailPredicateTaskTest()
    {
        Assert.AreEqual(new Void(),
            Product(Task.FromResult(1), Task.FromResult(2), x => x != 1).Result);
        Assert.AreEqual(new Void(),
            ProductAsync(Task.FromResult(1), Task.FromResult(2), x => x != 1).Result);
    }

    private static async MList<(T, TU)> ProductAsync<T, TU>(MList<T> tm, MList<TU> um, Predicate<T> guard)
    {
        var t = await tm;
        var u = await um;
        await MList.Guard(guard(t));
        return (t, u);
    }

    private static MList<(T, TU)> Product<T, TU>(MList<T> tm, MList<TU> um, Predicate<T> guard) =>
        from t in tm
        from u in um
        where guard(t)
        select (t, u);

    private static async Maybe<(T, TU)> ProductAsync<T, TU>(Maybe<T> tm, Maybe<TU> um, Predicate<T> guard)
    {
        var t = await tm;
        var u = await um;
        await Maybe.Guard(guard(t));
        return (t, u);
    }

    private static Maybe<(T, TU)> Product<T, TU>(Maybe<T> tm, Maybe<TU> um, Predicate<T> guard) =>
        from t in tm
        from u in um
        where guard(t)
        select (t, u);

    private static async Task<(T, TU)> ProductAsync<T, TU>(Task<T> tm, Task<TU> um, Predicate<T> guard)
    {
        var t = await tm;
        var u = await um;
        await MTask.Guard(guard(t));
        return (t, u);
    }

    private static Task<(T, TU)> Product<T, TU>(Task<T> tm, Task<TU> um, Predicate<T> guard) =>
        from t in tm
        from u in um
        where guard(t)
        select (t, u);
}
