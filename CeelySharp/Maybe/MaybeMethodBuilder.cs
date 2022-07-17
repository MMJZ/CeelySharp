using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;

namespace CeelySharp.Maybe;

public class MaybeMethodBuilder<TResult>
{
    private bool _retSet;
    private TResult _ret = default!;
    private IAsyncStateMachine? _asyncStateMachine;

    public static MaybeMethodBuilder<TResult> Create() => new();

    public void Start<TStateMachine>(ref TStateMachine stateMachine) where TStateMachine : IAsyncStateMachine
    {
        _asyncStateMachine = stateMachine;
        stateMachine.MoveNext();
    }

    public void SetStateMachine(IAsyncStateMachine _) => throw new NotImplementedException();

    public void AwaitOnCompleted<TAwaiter, TStateMachine>(
        ref TAwaiter _, ref TStateMachine __)
        where TAwaiter : INotifyCompletion
        where TStateMachine : IAsyncStateMachine => throw new NotImplementedException();

    public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(
        ref TAwaiter _, ref TStateMachine __)
        where TAwaiter : ICriticalNotifyCompletion
        where TStateMachine : IAsyncStateMachine
    {
    }

    public Maybe<TResult> Task => _retSet ? Maybe.Some(_ret) : Maybe.None<TResult>();

    public void SetResult(TResult result)
    {
        _retSet = true;
        _ret = result;
    }

    public void SetException(Exception exception) => ExceptionDispatchInfo.Capture(exception).Throw();
}
