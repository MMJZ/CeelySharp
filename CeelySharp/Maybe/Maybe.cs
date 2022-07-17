using System.Runtime.CompilerServices;
using Void = CeelySharp.Base.Void;

namespace CeelySharp.Maybe;

public static class Maybe
{
    private static readonly Maybe<Void> MEmpty = new None<Void>();
    private static readonly Maybe<Void> MPure = Unit(new Void());
    public static Maybe<Void> Guard(bool predicate) =>
        predicate ? MPure : MEmpty;

    public static async Maybe<T> Unit<T>(T t) => t;
}

[AsyncMethodBuilder(typeof(MaybeMethodBuilder<>))]
public abstract record Maybe<T> : ICriticalNotifyCompletion
{
    public Maybe<T> GetAwaiter() => this;
    public abstract bool IsCompleted { get; }
    public abstract T GetResult();
    public void UnsafeOnCompleted(Action action) => throw new NotImplementedException();
    public void OnCompleted(Action action) => throw new NotImplementedException();
}

public record Some<T>(T Item) : Maybe<T>
{
    public override bool IsCompleted => true;
    public override T GetResult() => Item;
}

public record None<T> : Maybe<T>
{
    public override bool IsCompleted => false;
    public override T GetResult() => throw new NotImplementedException();
}
