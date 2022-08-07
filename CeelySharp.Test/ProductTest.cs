using System;
using System.Threading.Tasks;
using CeelySharp.Base;
using CeelySharp.ListMonad;
using CeelySharp.MaybeMonad;
using CeelySharp.TaskMonad;
using NUnit.Framework;
using Void = CeelySharp.Base.Void;

namespace CeelySharp.Test;

public class ProductTest
{
    [Test]
    public void ListProduct()
    {
        var a = new MList<int> { 1, 2, 3 };
        var b = new MList<int> { 4, 5, 6 };

        CollectionAssert.AreEqual(new[] { (1, 4), (1, 5), (1, 6), (2, 4), (2, 5), (2, 6), (3, 4), (3, 5), (3, 6) },
            Product(a, b, _ => true));

        CollectionAssert.AreEqual(new[] { (2, 4), (2, 5), (2, 6), (3, 4), (3, 5), (3, 6) },
            Product(a, b, x => x > 1));

        CollectionAssert.AreEqual(new[] { (1, 4), (1, 5), (1, 6), (3, 4), (3, 5), (3, 6) },
            Product(a, b, x => x != 2));

        CollectionAssert.IsEmpty(Product(a, new MList<int>(), _ => true));

        CollectionAssert.IsEmpty(Product(a, b, x => x > 3));
    }

    [Test]
    public void MaybeProduct()
    {
        Assert.AreEqual(new Some<(int, int)>((1, 2)),
            Product(new Some<int>(1), new Some<int>(2), x => x == 1));

        Assert.AreEqual(new None<(int, int)>(),
            Product(new Some<int>(1), new None<int>(), _ => true));

        Assert.AreEqual(new None<(int, int)>(),
            Product(new Some<int>(1), new Some<int>(2), x => x != 1));
    }

    [Test]
    public void PassPredicateTaskProduct()
    {
        Assert.AreEqual((1, 2),
            Product(Task.FromResult(1), Task.FromResult(2), x => x == 1).Result);
    }

    [Explicit /* ;) */, Test]
    public void FailPredicateTaskTest()
    {
        Assert.AreEqual(new Void(),
            Product(Task.FromResult(1), Task.FromResult(2), x => x != 1).Result);
    }

    private async MList<(T, TU)> Product<T, TU>(MList<T> ts, MList<TU> us, Predicate<T> guard)
    {
        var t = await ts;
        var u = await us;
        await MList.Guard(guard(t));
        return (t, u);
    }

    private async Maybe<(T, TU)> Product<T, TU>(Maybe<T> ts, Maybe<TU> us, Predicate<T> guard)
    {
        var t = await ts;
        var u = await us;
        await Maybe.Guard(guard(t));
        return (t, u);
    }

    private async Task<(T, TU)> Product<T, TU>(Task<T> ts, Task<TU> us, Predicate<T> guard)
    {
        var t = await ts;
        var u = await us;
        await MTask.Guard(guard(t));
        return (t, u);
    }
}
