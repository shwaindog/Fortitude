#region

using FortitudeCommon.Chronometry.Timers;
using FortitudeCommon.DataStructures.Memory;

#endregion

namespace FortitudeCommon.AsyncProcessing.Tasks;

public abstract class ThreadPoolExecutionContext : RecyclableObject
{
    protected static readonly UpdateableTimer Timer = new();
    protected static readonly Action<ThreadPoolExecutionContext?> DecrementExecutionContextAction = DecrementExecutionRefCount;
    protected readonly WaitCallback AsyncCallback;
    protected readonly WaitCallback ImmediateCallback;
    protected readonly WaitCallback ImmediateCancellableCallback;

    protected ThreadPoolExecutionContext()
    {
        ImmediateCallback = RunImmediateCallback;
        AsyncCallback = RunAsyncCallback;
        ImmediateCancellableCallback = RunImmediateCancellableCallback;
    }

    protected void EnsureRefCountIsAtLeastTwo() // callers should decrement call this after dispatching
    {
        while (RefCount < 2) IncrementRefCount();
    }

    protected abstract void RunImmediateCallback(object? state);
    protected abstract void RunAsyncCallback(object? state);
    protected abstract void RunImmediateCancellableCallback(object? state);

    private static void DecrementExecutionRefCount(ThreadPoolExecutionContext? toDecrement)
    {
        toDecrement?.DecrementRefCount();
    }
}

public abstract class ThreadPoolExecutionContextResultBase<TR> : ThreadPoolExecutionContext
{
    public readonly ReusableValueTaskSource<TR> ReusableValueTaskSource = new()
    {
        AutoRecycleAtRefCountZero = false
    };

    protected BasicCancellationToken? BasicCancellationToken;

    public override void StateReset()
    {
        ReusableValueTaskSource.StateReset();
        BasicCancellationToken?.DecrementRefCount();
        BasicCancellationToken = null;
        base.StateReset();
    }
}

public class ThreadPoolExecutionContextResult<TR> : ThreadPoolExecutionContextResultBase<TR>
    , IAlternativeExecutionContextResult<TR>
{
    private Func<ValueTask<TR>>? asyncMethodToExecute;
    private Func<BasicCancellationToken?, TR>? cancellableMethodToExecute;
    private Func<TR>? nonAsyncMethodToExecute;

    public ValueTask<TR> Execute(Func<TR> methodToExecute)
    {
        EnsureRefCountIsAtLeastTwo();
        nonAsyncMethodToExecute = methodToExecute;
        ThreadPool.QueueUserWorkItem(ImmediateCallback, null);
        return new ValueTask<TR>(ReusableValueTaskSource, ReusableValueTaskSource.Version);
    }

    public ValueTask<TR> Execute(Func<ValueTask<TR>> methodToExecute)
    {
        EnsureRefCountIsAtLeastTwo();
        asyncMethodToExecute = methodToExecute;
        ThreadPool.QueueUserWorkItem(AsyncCallback, null);
        return new ValueTask<TR>(ReusableValueTaskSource, ReusableValueTaskSource.Version);
    }

    public ValueTask<TR> Execute(Func<BasicCancellationToken?, TR> methodToExecute, BasicCancellationToken? firstParam)
    {
        EnsureRefCountIsAtLeastTwo();
        cancellableMethodToExecute = methodToExecute;
        BasicCancellationToken = firstParam;
        ThreadPool.QueueUserWorkItem(ImmediateCancellableCallback, null);
        return new ValueTask<TR>(ReusableValueTaskSource, ReusableValueTaskSource.Version);
    }

    public override void StateReset()
    {
        asyncMethodToExecute = null;
        cancellableMethodToExecute = null;
        nonAsyncMethodToExecute = null;
        base.StateReset();
    }

    protected override void RunImmediateCallback(object? state)
    {
        try
        {
            var result = nonAsyncMethodToExecute!();
            ReusableValueTaskSource.TrySetResult(result);
        }
        catch (Exception ex)
        {
            ReusableValueTaskSource.SetException(ex);
        }

        Timer.RunIn(60_000, this, DecrementExecutionContextAction);
    }

    protected override void RunAsyncCallback(object? state)
    {
        try
        {
            var asyncResult = asyncMethodToExecute!();
            if (asyncResult.IsCompletedSuccessfully)
            {
                ReusableValueTaskSource.TrySetResult(asyncResult.Result);
            }
            else if (asyncResult.IsFaulted)
            {
                // ReSharper disable once UnusedVariable
                var expectThrowsException = asyncResult.Result;
            }
            else
            {
                ReusableValueTaskSource.TrySetResultFromAwaitingTask(asyncResult);
            }
        }
        catch (Exception ex)
        {
            ReusableValueTaskSource.SetException(ex);
        }

        Timer.RunIn(60_000, this, DecrementExecutionContextAction);
    }

    protected override void RunImmediateCancellableCallback(object? state)
    {
        try
        {
            var result = cancellableMethodToExecute!(BasicCancellationToken);
            ReusableValueTaskSource.TrySetResult(result);
        }
        catch (Exception ex)
        {
            ReusableValueTaskSource.SetException(ex);
        }

        Timer.RunIn(60_000, this, DecrementExecutionContextAction);
    }
}

public class ThreadPoolExecutionContextResult<TR, TP> : ThreadPoolExecutionContextResult<TR>
    , IAlternativeExecutionContextResult<TR, TP>
{
    private Func<TP, ValueTask<TR>>? asyncMethodToExecute;
    private Func<TP, BasicCancellationToken?, TR>? cancellableMethodToExecute;
    private TP? firstParam;

    private Func<TP, TR>? nonAsyncMethodToExecute;

    public ValueTask<TR> Execute(Func<TP, TR> methodToExecute, TP firstParameter)
    {
        EnsureRefCountIsAtLeastTwo();
        nonAsyncMethodToExecute = methodToExecute;
        firstParam = firstParameter;
        ThreadPool.QueueUserWorkItem(ImmediateCallback, null);
        return new ValueTask<TR>(ReusableValueTaskSource, ReusableValueTaskSource.Version);
    }

    public ValueTask<TR> Execute(Func<TP, ValueTask<TR>> methodToExecute, TP firstParameter)
    {
        EnsureRefCountIsAtLeastTwo();
        asyncMethodToExecute = methodToExecute;
        firstParam = firstParameter;
        ThreadPool.QueueUserWorkItem(AsyncCallback, null);
        return new ValueTask<TR>(ReusableValueTaskSource, ReusableValueTaskSource.Version);
    }

    public ValueTask<TR> Execute(Func<TP, BasicCancellationToken?, TR> methodToExecute, TP firstParameter, BasicCancellationToken? secondParameter)
    {
        EnsureRefCountIsAtLeastTwo();
        cancellableMethodToExecute = methodToExecute;
        firstParam = firstParameter;
        BasicCancellationToken = secondParameter;
        ThreadPool.QueueUserWorkItem(ImmediateCancellableCallback, null);
        return new ValueTask<TR>(ReusableValueTaskSource, ReusableValueTaskSource.Version);
    }

    public override void StateReset()
    {
        asyncMethodToExecute = null;
        cancellableMethodToExecute = null;
        nonAsyncMethodToExecute = null;
        firstParam = default;
        base.StateReset();
    }

    protected override void RunImmediateCallback(object? state)
    {
        try
        {
            var result = nonAsyncMethodToExecute!(firstParam!);
            ReusableValueTaskSource.TrySetResult(result);
        }
        catch (Exception ex)
        {
            ReusableValueTaskSource.SetException(ex);
        }

        Timer.RunIn(60_000, this, DecrementExecutionContextAction);
    }

    protected override void RunAsyncCallback(object? state)
    {
        try
        {
            var asyncResult = asyncMethodToExecute!(firstParam!);
            if (asyncResult.IsCompletedSuccessfully)
            {
                ReusableValueTaskSource.TrySetResult(asyncResult.Result);
            }
            else if (asyncResult.IsFaulted)
            {
                // ReSharper disable once UnusedVariable
                var expectThrowsException = asyncResult.Result;
            }
            else
            {
                ReusableValueTaskSource.TrySetResultFromAwaitingTask(asyncResult);
            }
        }
        catch (Exception ex)
        {
            ReusableValueTaskSource.SetException(ex);
        }

        Timer.RunIn(60_000, this, DecrementExecutionContextAction);
    }

    protected override void RunImmediateCancellableCallback(object? state)
    {
        try
        {
            var result = cancellableMethodToExecute!(firstParam!, BasicCancellationToken);
            ReusableValueTaskSource.TrySetResult(result);
        }
        catch (Exception ex)
        {
            ReusableValueTaskSource.SetException(ex);
        }

        Timer.RunIn(60_000, this, DecrementExecutionContextAction);
    }
}
