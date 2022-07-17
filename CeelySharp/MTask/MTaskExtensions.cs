using CeelySharp.MList;

namespace CeelySharp.MTask;

public static class MTaskExtensions
{
    public static async Task<V> SelectMany<T, U, V>(
        this Task<T> source, Func<T, Task<U>> selector, Func<T, U, V> resultSelector)
    {
        var t = await source;
        return resultSelector(t, await selector(t));
    }
    
    public static async Task<U> Select<T, U>(
        this Task<T> source, Func<T, U> selector)
    {
        return selector(await source);
    }

    public static async Task<T> Where<T>(
        this Task<T> source, Func<T, bool> predicate)
    {
        var t = await source;
        await MTask.Guard(predicate(t));
        return t;
    }

    public static async Task<V> Join<T, U, K, V>(
        this Task<T> source, MList<U> inner,
        Func<T, K> outerKeySelector, Func<U, K> innerKeySelector,
        Func<T, U, V> resultSelector)
    {
        var s = await source;
        var i = await inner;
        await MTask.Guard(EqualityComparer<K>.Default
            .Equals(outerKeySelector(s), innerKeySelector(i)));
        return resultSelector(s, i);
    }

    public static async Task<V> GroupJoin<T, U, K, V>(
        this Task<T> source, Task<U> inner,
        Func<T, K> outerKeySelector, Func<U, K> innerKeySelector,
        Func<T, Task<U>, V> resultSelector)
    {
        var t = await source;
        return resultSelector(t,
            inner.Where(u => EqualityComparer<K>.Default.Equals(
                outerKeySelector(t), innerKeySelector(u))));
    }
}
