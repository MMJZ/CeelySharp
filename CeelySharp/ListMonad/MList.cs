using System.Collections;
using System.Runtime.CompilerServices;
using Void = CeelySharp.Base.Void;

namespace CeelySharp.ListMonad;

public static class MList
{
    private static readonly MList<Void> MEmpty = new();
    private static readonly MList<Void> MPure = Unit(new Void());
    public static MList<Void> Guard(bool predicate) =>
        predicate ? MPure : MEmpty;

    public static async MList<T> Unit<T>(T t) => t;
}

public enum AwaitState
{
    NoAwait = -1,
    /// <summary>
    /// Currently awaited, but not advancing when returning the current value
    /// </summary>
    SecondaryAwait = 0,
    /// <summary>
    /// Currently awaited, and advancing when returning the current value
    /// </summary>
    PrimaryAwait = 1
}

public interface IMList : ICriticalNotifyCompletion
{
    void ResetEnumerator();
    AwaitState AwaitState { get; set; }
    bool IsCompleted { get; }
}

[AsyncMethodBuilder(typeof(MListMethodBuilder<>))]
public record MList<T> : IReadOnlyCollection<T>, IMList
{
    private IEnumerator<T>? _awaitedEnumerator;
    private readonly IList<T> _items = new List<T>();
    public AwaitState AwaitState { get; set; } = AwaitState.NoAwait;
    public IEnumerator<T> GetEnumerator() => _items.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => _items.GetEnumerator();
    public void Add(T t) => _items.Add(t);
    public int Count => _items.Count;
    public MList<T> GetAwaiter() => this;

    public bool IsCompleted => AwaitState switch
    {
        AwaitState.NoAwait => false,
        AwaitState.SecondaryAwait => true,
        AwaitState.PrimaryAwait => _awaitedEnumerator!.MoveNext(),
        _ => throw new ArgumentOutOfRangeException()
    };

    public T GetResult() => _awaitedEnumerator!.Current;
    public void ResetEnumerator() => _awaitedEnumerator = GetEnumerator();
    public void UnsafeOnCompleted(Action action) => throw new NotImplementedException();
    public void OnCompleted(Action action) => throw new NotImplementedException();
}
