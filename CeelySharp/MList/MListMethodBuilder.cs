using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;

namespace CeelySharp.MList;

public class MListMethodBuilder<TResult>
{
    private readonly Stack<IMList> _awaiterStack = new();
    private readonly MList<TResult> _ret = new();
    private IAsyncStateMachine? _asyncStateMachine;

    public static MListMethodBuilder<TResult> Create() => new();

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
        ref TAwaiter awaiter, ref TStateMachine stateMachine)
        where TAwaiter : ICriticalNotifyCompletion
        where TStateMachine : IAsyncStateMachine
    {
        var mList = (IMList)awaiter;
        if (mList.AwaitState == -1)
        {
            if (_awaiterStack.Any())
            {
                _awaiterStack.Peek().AwaitState = 0;
            }

            mList.ResetEnumerator();
            mList.AwaitState = 1;
            _awaiterStack.Push(mList);

            if (mList.HasMoveNext())
            {
                stateMachine.MoveNext();
                return;
            }
        }
        
        _awaiterStack.Pop();
        mList.AwaitState = -1;
        if (_awaiterStack.Any())
        {
            _awaiterStack.Peek().AwaitState = 1;
            stateMachine.GetType().GetField("<>1__state")?.SetValue(stateMachine, -1);
            stateMachine.MoveNext();
        }
    }

    public MList<TResult> Task => _ret;

    public void SetResult(TResult result)
    {
        _ret.Add(result);
        if (_awaiterStack.Any())
        {
            _asyncStateMachine?.MoveNext();
        }
    }

    public void SetException(Exception exception) => ExceptionDispatchInfo.Capture(exception).Throw();
}
