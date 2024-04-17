#region

using FortitudeBusRules.BusMessaging.Tasks;
using FortitudeCommon.AsyncProcessing.Tasks;

#endregion

namespace FortitudeBusRules.BusMessaging.Pipelines.Execution;

public class OneParamSyncActionPayload<TP> : ReusableValueTaskSource<int>, IInvokeablePayload
{
    private TP firstParameter = default!;
    private Action<TP>? toInvoke;

    public virtual void Invoke()
    {
        try
        {
            toInvoke!(firstParameter);
            SetResult(0);
        }
        catch (Exception ex)
        {
            SetException(ex);
        }
    }

    public void Configure(Action<TP> toExecute, TP firstParam)
    {
        toInvoke = toExecute;
        firstParameter = firstParam;
    }
}

public class TwoParamSyncActionPayload<TP, TP2> : ReusableValueTaskSource<int>, IInvokeablePayload
{
    private TP firstParameter = default!;
    private TP2 secondParameter = default!;
    private Action<TP, TP2>? toInvoke;

    public virtual void Invoke()
    {
        try
        {
            toInvoke!(firstParameter, secondParameter);
            SetResult(0);
        }
        catch (Exception ex)
        {
            SetException(ex);
        }
    }

    public void Configure(Action<TP, TP2> toExecute, TP firstParam, TP2 secondParam)
    {
        toInvoke = toExecute;
        firstParameter = firstParam;
        secondParameter = secondParam;
    }
}

public class NoParamsSyncResultPayload<TR> : ReusableValueTaskSource<TR>, IInvokeablePayload
{
    private Func<TR>? toInvoke;

    public virtual void Invoke()
    {
        try
        {
            var result = toInvoke!();
            TrySetResult(result);
        }
        catch (Exception ex)
        {
            SetException(ex);
        }
    }

    public void Configure(Func<TR> toExecute)
    {
        toInvoke = toExecute;
    }
}

public class NoParamsAsyncResultPayload<TR> : ReusableValueTaskSource<TR>, IInvokeablePayload
{
    private Func<ValueTask<TR>>? toInvoke;

    public virtual void Invoke()
    {
        try
        {
            var asyncResult = toInvoke!();
            if (asyncResult.IsCompletedSuccessfully)
            {
                TrySetResult(asyncResult.Result);
            }
            else if (asyncResult.IsFaulted)
            {
                // ReSharper disable once UnusedVariable
                var expectThrowsException = asyncResult.Result;
            }
            else
            {
                TrySetResultFromAwaitingTask(asyncResult);
            }
        }
        catch (Exception ex)
        {
            SetException(ex);
        }
    }

    public void Configure(Func<ValueTask<TR>> toExecute)
    {
        toInvoke = toExecute;
    }
}

public class OneParamSyncResultPayload<TR, TP> : ReusableValueTaskSource<TR>, IInvokeablePayload
{
    private TP firstParameter = default!;
    private Func<TP, TR>? toInvoke;

    public virtual void Invoke()
    {
        try
        {
            var result = toInvoke!(firstParameter);
            TrySetResult(result);
        }
        catch (Exception ex)
        {
            SetException(ex);
        }
    }

    public void Configure(Func<TP, TR> toExecute, TP firstParam)
    {
        toInvoke = toExecute;
        firstParameter = firstParam;
    }
}

public class OneParamAsyncResultPayload<TR, TP> : ReusableValueTaskSource<TR>, IInvokeablePayload
{
    private TP firstParameter = default!;
    private Func<TP, ValueTask<TR>>? toInvoke;

    public virtual void Invoke()
    {
        try
        {
            var asyncResult = toInvoke!(firstParameter);
            if (asyncResult.IsCompletedSuccessfully)
            {
                TrySetResult(asyncResult.Result);
            }
            else if (asyncResult.IsFaulted)
            {
                // ReSharper disable once UnusedVariable
                var expectThrowsException = asyncResult.Result;
            }
            else
            {
                TrySetResultFromAwaitingTask(asyncResult);
            }
        }
        catch (Exception ex)
        {
            SetException(ex);
        }
    }

    public void Configure(Func<TP, ValueTask<TR>> toExecute, TP firstParam)
    {
        toInvoke = toExecute;
        firstParameter = firstParam;
    }
}

public class TwoParamsSyncResultPayload<TR, TP, TP2> : ReusableValueTaskSource<TR>, IInvokeablePayload
{
    private TP firstParameter = default!;
    private TP2 secondParameter = default!;
    private Func<TP, TP2, TR>? toInvoke;

    public virtual void Invoke()
    {
        try
        {
            var result = toInvoke!(firstParameter, secondParameter);
            TrySetResult(result);
        }
        catch (Exception ex)
        {
            SetException(ex);
        }
    }

    public void Configure(Func<TP, TP2, TR> toExecute, TP firstParam, TP2 secondParam)
    {
        toInvoke = toExecute;
        firstParameter = firstParam;
        secondParameter = secondParam;
    }
}

public class TwoParamsAsyncResultPayload<TR, TP, TP2> : ReusableValueTaskSource<TR>, IInvokeablePayload
{
    private TP firstParameter = default!;
    private TP2 secondParameter = default!;
    private Func<TP, TP2, ValueTask<TR>>? toInvoke;

    public virtual void Invoke()
    {
        try
        {
            var asyncResult = toInvoke!(firstParameter, secondParameter);
            if (asyncResult.IsCompletedSuccessfully)
            {
                TrySetResult(asyncResult.Result);
            }
            else if (asyncResult.IsFaulted)
            {
                // ReSharper disable once UnusedVariable
                var expectThrowsException = asyncResult.Result;
            }
            else
            {
                TrySetResultFromAwaitingTask(asyncResult);
            }
        }
        catch (Exception ex)
        {
            SetException(ex);
        }
    }

    public void Configure(Func<TP, TP2, ValueTask<TR>> toExecute, TP firstParam, TP2 secondParam)
    {
        toInvoke = toExecute;
        firstParameter = firstParam;
        secondParameter = secondParam;
    }
}

public class ThreeParamsSyncResultPayload<TR, TP, TP2, TP3> : ReusableValueTaskSource<TR>, IInvokeablePayload
{
    private TP firstParameter = default!;
    private TP2 secondParameter = default!;
    private TP3 thirdParameter = default!;
    private Func<TP, TP2, TP3, TR>? toInvoke;

    public virtual void Invoke()
    {
        try
        {
            var result = toInvoke!(firstParameter, secondParameter, thirdParameter);
            TrySetResult(result);
        }
        catch (Exception ex)
        {
            SetException(ex);
        }
    }

    public void Configure(Func<TP, TP2, TP3, TR> toExecute, TP firstParam, TP2 secondParam, TP3 thirdParam)
    {
        toInvoke = toExecute;
        firstParameter = firstParam;
        secondParameter = secondParam;
        thirdParameter = thirdParam;
    }
}

public class ThreeParamsAsyncResultPayload<TR, TP, TP2, TP3> : ReusableValueTaskSource<TR>, IInvokeablePayload
{
    private TP firstParameter = default!;
    private TP2 secondParameter = default!;
    private TP3 thirdParameter = default!;
    private Func<TP, TP2, TP3, ValueTask<TR>>? toInvoke;

    public virtual void Invoke()
    {
        try
        {
            var asyncResult = toInvoke!(firstParameter, secondParameter, thirdParameter);
            if (asyncResult.IsCompletedSuccessfully)
            {
                TrySetResult(asyncResult.Result);
            }
            else if (asyncResult.IsFaulted)
            {
                // ReSharper disable once UnusedVariable
                var expectThrowsException = asyncResult.Result;
            }
            else
            {
                TrySetResultFromAwaitingTask(asyncResult);
            }
        }
        catch (Exception ex)
        {
            SetException(ex);
        }
    }

    public void Configure(Func<TP, TP2, TP3, ValueTask<TR>> toExecute, TP firstParam, TP2 secondParam, TP3 thirdParam)
    {
        toInvoke = toExecute;
        firstParameter = firstParam;
        secondParameter = secondParam;
        thirdParameter = thirdParam;
    }
}
