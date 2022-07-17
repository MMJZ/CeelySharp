namespace CeelySharp.Maybe;

public static class MListExtensions
{
    public static async Maybe<V> SelectMany<T, U, V>(
        this Maybe<T> source, Func<T, Maybe<U>> selector, Func<T, U, V> resultSelector)
    {
        var t = await source;
        return resultSelector(t, await selector(t));
    }
    
    public static async Maybe<U> Select<T, U>(
        this Maybe<T> source, Func<T, U> selector) =>
        selector(await source);

    public static async Maybe<T> Where<T>(
        this Maybe<T> source, Func<T, bool> predicate)
    {
        var t = await source;
        await Maybe.Guard(predicate(t));
        return t;
    }

    public static async Maybe<V> Join<T, U, K, V>(
        this Maybe<T> source, Maybe<U> inner,
        Func<T, K> outerKeySelector, Func<U, K> innerKeySelector,
        Func<T, U, V> resultSelector)
    {
        var s = await source;
        var i = await inner;
        if (!EqualityComparer<K>.Default.Equals(
                outerKeySelector(s), innerKeySelector(i)))
            throw new OperationCanceledException();
        return resultSelector(s, i);
    }

    public static async Maybe<V> GroupJoin<T, U, K, V>(
        this Maybe<T> source, Maybe<U> inner,
        Func<T, K> outerKeySelector, Func<U, K> innerKeySelector,
        Func<T, Maybe<U>, V> resultSelector)
    {
        var t = await source;
        return resultSelector(t,
            inner.Where(u => EqualityComparer<K>.Default.Equals(
                outerKeySelector(t), innerKeySelector(u))));
    }
}
