using System.Collections;
using System.Runtime.CompilerServices;
using Void = CeelySharp.Base.Void;

namespace CeelySharp.MList;

public static class MList
{
    private static readonly MList<Void> MEmpty = new();
    private static readonly MList<Void> MPure = new() { new Void() };
    public static MList<Void> Guard(bool predicate) =>
        predicate ? MPure : MEmpty;
}

public interface IMList : ICriticalNotifyCompletion
{
    void ResetEnumerator();
    int AwaitState { get; set; }
    bool HasMoveNext();
}

[AsyncMethodBuilder(typeof(MListMethodBuilder<>))]
public record MList<T> : IReadOnlyCollection<T>, IMList
{
    private IEnumerator<T>? _awaitedEnumerator;
    private readonly IList<T> _items = new List<T>();
    public int AwaitState { get; set; } = -1;
    public IEnumerator<T> GetEnumerator() => _items.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => _items.GetEnumerator();
    public void Add(T t) => _items.Add(t);
    public int Count => _items.Count;
    public MList<T> GetAwaiter() => this;

    public bool IsCompleted => AwaitState switch
    {
        -1 => false,
        0 => true,
        _ => _awaitedEnumerator!.MoveNext()
    };

    public T GetResult() => _awaitedEnumerator!.Current;
    public void ResetEnumerator() => _awaitedEnumerator = GetEnumerator();
    public bool HasMoveNext() => _awaitedEnumerator!.MoveNext();
    public void UnsafeOnCompleted(Action action) => throw new NotImplementedException();
    public void OnCompleted(Action action) => throw new NotImplementedException();
}
