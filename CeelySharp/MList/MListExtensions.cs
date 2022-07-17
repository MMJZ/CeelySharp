namespace CeelySharp.MList;

public static class MListExtensions
{
    public static async MList<V> SelectMany<T, U, V>(
        this MList<T> source, Func<T, MList<U>> selector, Func<T, U, V> resultSelector)
    {
        var t = await source;
        return resultSelector(t, await selector(t));
    }
    
    public static async MList<U> Select<T, U>(
        this MList<T> source, Func<T, U> selector)
    {
        return selector(await source);
    }

    public static async MList<T> Where<T>(
        this MList<T> source, Func<T, bool> predicate)
    {
        var t = await source;
        await MList.Guard(predicate(t));
        return t;
    }

    public static async MList<V> Join<T, U, K, V>(
        this MList<T> source, MList<U> inner,
        Func<T, K> outerKeySelector, Func<U, K> innerKeySelector,
        Func<T, U, V> resultSelector)
    {
        var s = await source;
        var i = await inner;
        await MList.Guard(!EqualityComparer<K>.Default
            .Equals(outerKeySelector(s), innerKeySelector(i)));
        return resultSelector(s, i);
    }

    public static async MList<V> GroupJoin<T, U, K, V>(
        this MList<T> source, MList<U> inner,
        Func<T, K> outerKeySelector, Func<U, K> innerKeySelector,
        Func<T, MList<U>, V> resultSelector)
    {
        var t = await source;
        return resultSelector(t,
            inner.Where(u => EqualityComparer<K>.Default.Equals(
                outerKeySelector(t), innerKeySelector(u))));
    }
}
