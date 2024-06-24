// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.BusMessaging.Tasks;
using FortitudeCommon.AsyncProcessing.Tasks;
using FortitudeCommon.DataStructures.Memory;

#endregion

namespace FortitudeBusRules.BusMessaging.Pipelines.Execution;

public class OneParamSyncActionPayload<TP> : ReusableValueTaskSource<int>, IInvokeablePayload
{
    private TP firstParameter = default!;

    private Action<TP>? toInvoke;

    public bool IsAsyncInvoke => false;

    public ValueTask InvokeAsync()
    {
        toInvoke?.Invoke(firstParameter);
        TrySetResult(0);
        return ValueTask.CompletedTask;
    }

    public virtual void Invoke()
    {
        try
        {
            toInvoke!(firstParameter);
            TrySetResult(0);
        }
        catch (Exception ex)
        {
            SetException(ex);
        }
    }

    public override int DecrementRefCount()
    {
        if (firstParameter is IRecyclableObject recyclableFirstParam) recyclableFirstParam.DecrementRefCount();
        return base.DecrementRefCount();
    }

    public override int IncrementRefCount()
    {
        if (firstParameter is IRecyclableObject recyclableFirstParam) recyclableFirstParam.IncrementRefCount();
        return base.IncrementRefCount();
    }

    public void Configure(Action<TP> toExecute, TP firstParam)
    {
        toInvoke       = toExecute;
        firstParameter = firstParam;
        if (firstParameter is IRecyclableObject recyclableFirstParam) recyclableFirstParam.IncrementRefCount();
    }
}

public class TwoParamSyncActionPayload<TP, TP2> : ReusableValueTaskSource<int>, IInvokeablePayload
{
    private TP  firstParameter  = default!;
    private TP2 secondParameter = default!;

    private Action<TP, TP2>? toInvoke;

    public bool IsAsyncInvoke => false;

    public ValueTask InvokeAsync()
    {
        toInvoke?.Invoke(firstParameter, secondParameter);
        TrySetResult(0);
        return ValueTask.CompletedTask;
    }

    public virtual void Invoke()
    {
        try
        {
            toInvoke!(firstParameter, secondParameter);
            TrySetResult(0);
        }
        catch (Exception ex)
        {
            SetException(ex);
        }
    }

    public override int DecrementRefCount()
    {
        if (firstParameter is IRecyclableObject recyclableFirstParam) recyclableFirstParam.DecrementRefCount();
        if (secondParameter is IRecyclableObject recyclableSecondParam) recyclableSecondParam.DecrementRefCount();
        return base.DecrementRefCount();
    }

    public override int IncrementRefCount()
    {
        if (firstParameter is IRecyclableObject recyclableFirstParam) recyclableFirstParam.IncrementRefCount();
        if (secondParameter is IRecyclableObject recyclableSecondParam) recyclableSecondParam.IncrementRefCount();
        return base.IncrementRefCount();
    }

    public void Configure(Action<TP, TP2> toExecute, TP firstParam, TP2 secondParam)
    {
        toInvoke       = toExecute;
        firstParameter = firstParam;
        if (firstParameter is IRecyclableObject recyclableFirstParam) recyclableFirstParam.IncrementRefCount();
        secondParameter = secondParam;
        if (secondParameter is IRecyclableObject recyclableSecondParam) recyclableSecondParam.IncrementRefCount();
    }
}

public class ThreeParamSyncActionPayload<TP1, TP2, TP3> : ReusableValueTaskSource<int>, IInvokeablePayload
{
    private TP1 firstParameter  = default!;
    private TP2 secondParameter = default!;
    private TP3 thirdParameter  = default!;

    private Action<TP1, TP2, TP3>? toInvoke;

    public bool IsAsyncInvoke => false;

    public ValueTask InvokeAsync()
    {
        toInvoke?.Invoke(firstParameter, secondParameter, thirdParameter);
        TrySetResult(0);
        return ValueTask.CompletedTask;
    }

    public virtual void Invoke()
    {
        try
        {
            toInvoke!(firstParameter, secondParameter, thirdParameter);
            TrySetResult(0);
        }
        catch (Exception ex)
        {
            SetException(ex);
        }
    }

    public override int DecrementRefCount()
    {
        if (firstParameter is IRecyclableObject recyclableFirstParam) recyclableFirstParam.DecrementRefCount();
        if (secondParameter is IRecyclableObject recyclableSecondParam) recyclableSecondParam.DecrementRefCount();
        return base.DecrementRefCount();
    }

    public override int IncrementRefCount()
    {
        if (firstParameter is IRecyclableObject recyclableFirstParam) recyclableFirstParam.IncrementRefCount();
        if (secondParameter is IRecyclableObject recyclableSecondParam) recyclableSecondParam.IncrementRefCount();
        return base.IncrementRefCount();
    }

    public void Configure(Action<TP1, TP2, TP3> toExecute, TP1 firstParam, TP2 secondParam, TP3 thirdParam)
    {
        toInvoke       = toExecute;
        firstParameter = firstParam;
        if (firstParameter is IRecyclableObject recyclableFirstParam) recyclableFirstParam.IncrementRefCount();
        secondParameter = secondParam;
        if (secondParameter is IRecyclableObject recyclableSecondParam) recyclableSecondParam.IncrementRefCount();
        thirdParameter = thirdParam;
        if (thirdParameter is IRecyclableObject recyclableThirdParam) recyclableThirdParam.IncrementRefCount();
    }
}

public class NoParamsSyncResultPayload<TR> : ReusableValueTaskSource<TR>, IInvokeablePayload
{
    private Func<TR>? toInvoke;

    public bool IsAsyncInvoke => false;

    public ValueTask InvokeAsync()
    {
        if (toInvoke != null)
        {
            var result = toInvoke.Invoke();
            TrySetResult(result);
        }

        return ValueTask.CompletedTask;
    }

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

    public bool IsAsyncInvoke => true;

    public async ValueTask InvokeAsync()
    {
        if (toInvoke != null)
        {
            var result = await toInvoke.Invoke();
            TrySetResult(result);
        }
    }

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

    public bool IsAsyncInvoke => false;

    public ValueTask InvokeAsync()
    {
        if (toInvoke != null)
        {
            var result = toInvoke.Invoke(firstParameter);
            TrySetResult(result);
        }

        return ValueTask.CompletedTask;
    }

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

    public override int DecrementRefCount()
    {
        if (firstParameter is IRecyclableObject recyclableFirstParam) recyclableFirstParam.DecrementRefCount();
        return base.DecrementRefCount();
    }

    public override int IncrementRefCount()
    {
        if (firstParameter is IRecyclableObject recyclableFirstParam) recyclableFirstParam.IncrementRefCount();
        return base.IncrementRefCount();
    }

    public void Configure(Func<TP, TR> toExecute, TP firstParam)
    {
        toInvoke       = toExecute;
        firstParameter = firstParam;
        if (firstParameter is IRecyclableObject recyclableFirstParam) recyclableFirstParam.IncrementRefCount();
    }
}

public class OneParamAsyncActionPayload<TP> : ReusableValueTaskSource<int>, IInvokeablePayload
{
    private TP firstParameter = default!;

    private Func<TP, ValueTask>? toInvoke;

    public bool IsAsyncInvoke => true;

    public async ValueTask InvokeAsync()
    {
        if (toInvoke != null)
        {
            await toInvoke.Invoke(firstParameter);
            TrySetResult(0);
        }
    }

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

    public override int DecrementRefCount()
    {
        if (firstParameter is IRecyclableObject recyclableFirstParam) recyclableFirstParam.DecrementRefCount();
        return base.DecrementRefCount();
    }

    public override int IncrementRefCount()
    {
        if (firstParameter is IRecyclableObject recyclableFirstParam) recyclableFirstParam.IncrementRefCount();
        return base.IncrementRefCount();
    }

    public void Configure(Func<TP, ValueTask> toExecute, TP firstParam)
    {
        toInvoke       = toExecute;
        firstParameter = firstParam;
        if (firstParameter is IRecyclableObject recyclableFirstParam) recyclableFirstParam.IncrementRefCount();
    }
}

public class OneParamAsyncResultPayload<TR, TP> : ReusableValueTaskSource<TR>, IInvokeablePayload
{
    private TP firstParameter = default!;

    private Func<TP, ValueTask<TR>>? toInvoke;

    public bool IsAsyncInvoke => true;

    public async ValueTask InvokeAsync()
    {
        if (toInvoke != null)
        {
            var result = await toInvoke.Invoke(firstParameter);
            TrySetResult(result);
        }
    }

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

    public override int DecrementRefCount()
    {
        if (firstParameter is IRecyclableObject recyclableFirstParam) recyclableFirstParam.DecrementRefCount();
        return base.DecrementRefCount();
    }

    public override int IncrementRefCount()
    {
        if (firstParameter is IRecyclableObject recyclableFirstParam) recyclableFirstParam.IncrementRefCount();
        return base.IncrementRefCount();
    }

    public void Configure(Func<TP, ValueTask<TR>> toExecute, TP firstParam)
    {
        toInvoke       = toExecute;
        firstParameter = firstParam;
        if (firstParameter is IRecyclableObject recyclableFirstParam) recyclableFirstParam.IncrementRefCount();
    }
}

public class TwoParamsSyncResultPayload<TR, TP, TP2> : ReusableValueTaskSource<TR>, IInvokeablePayload
{
    private TP  firstParameter  = default!;
    private TP2 secondParameter = default!;

    private Func<TP, TP2, TR>? toInvoke;

    public bool IsAsyncInvoke => false;

    public ValueTask InvokeAsync()
    {
        if (toInvoke != null)
        {
            var result = toInvoke.Invoke(firstParameter, secondParameter);
            TrySetResult(result);
        }

        return ValueTask.CompletedTask;
    }

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

    public override int DecrementRefCount()
    {
        if (firstParameter is IRecyclableObject recyclableFirstParam) recyclableFirstParam.DecrementRefCount();
        if (secondParameter is IRecyclableObject recyclableSecondParam) recyclableSecondParam.DecrementRefCount();
        return base.DecrementRefCount();
    }

    public override int IncrementRefCount()
    {
        if (firstParameter is IRecyclableObject recyclableFirstParam) recyclableFirstParam.IncrementRefCount();
        if (secondParameter is IRecyclableObject recyclableSecondParam) recyclableSecondParam.IncrementRefCount();
        return base.IncrementRefCount();
    }

    public void Configure(Func<TP, TP2, TR> toExecute, TP firstParam, TP2 secondParam)
    {
        toInvoke       = toExecute;
        firstParameter = firstParam;
        if (firstParameter is IRecyclableObject recyclableFirstParam) recyclableFirstParam.IncrementRefCount();
        secondParameter = secondParam;
        if (secondParameter is IRecyclableObject recyclableSecondParam) recyclableSecondParam.IncrementRefCount();
    }
}

public class TwoParamsAsyncResultPayload<TR, TP, TP2> : ReusableValueTaskSource<TR>, IInvokeablePayload
{
    private TP  firstParameter  = default!;
    private TP2 secondParameter = default!;

    private Func<TP, TP2, ValueTask<TR>>? toInvoke;

    public bool IsAsyncInvoke => true;

    public async ValueTask InvokeAsync()
    {
        if (toInvoke != null)
        {
            var result = await toInvoke.Invoke(firstParameter, secondParameter);
            TrySetResult(result);
        }
    }

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

    public override int DecrementRefCount()
    {
        if (firstParameter is IRecyclableObject recyclableFirstParam) recyclableFirstParam.DecrementRefCount();
        if (secondParameter is IRecyclableObject recyclableSecondParam) recyclableSecondParam.DecrementRefCount();
        return base.DecrementRefCount();
    }

    public override int IncrementRefCount()
    {
        if (firstParameter is IRecyclableObject recyclableFirstParam) recyclableFirstParam.IncrementRefCount();
        if (secondParameter is IRecyclableObject recyclableSecondParam) recyclableSecondParam.IncrementRefCount();
        return base.IncrementRefCount();
    }

    public void Configure(Func<TP, TP2, ValueTask<TR>> toExecute, TP firstParam, TP2 secondParam)
    {
        toInvoke       = toExecute;
        firstParameter = firstParam;
        if (firstParameter is IRecyclableObject recyclableFirstParam) recyclableFirstParam.IncrementRefCount();
        secondParameter = secondParam;
        if (secondParameter is IRecyclableObject recyclableSecondParam) recyclableSecondParam.IncrementRefCount();
    }
}

public class ThreeParamsSyncResultPayload<TR, TP, TP2, TP3> : ReusableValueTaskSource<TR>, IInvokeablePayload
{
    private TP  firstParameter  = default!;
    private TP2 secondParameter = default!;
    private TP3 thirdParameter  = default!;

    private Func<TP, TP2, TP3, TR>? toInvoke;

    public bool IsAsyncInvoke => false;

    public ValueTask InvokeAsync()
    {
        if (toInvoke != null)
        {
            var result = toInvoke.Invoke(firstParameter, secondParameter, thirdParameter);
            TrySetResult(result);
        }

        return ValueTask.CompletedTask;
    }

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

    public override int DecrementRefCount()
    {
        if (firstParameter is IRecyclableObject recyclableFirstParam) recyclableFirstParam.DecrementRefCount();
        if (secondParameter is IRecyclableObject recyclableSecondParam) recyclableSecondParam.DecrementRefCount();
        if (thirdParameter is IRecyclableObject recyclableThirdParam) recyclableThirdParam.DecrementRefCount();
        return base.DecrementRefCount();
    }

    public override int IncrementRefCount()
    {
        if (firstParameter is IRecyclableObject recyclableFirstParam) recyclableFirstParam.IncrementRefCount();
        if (secondParameter is IRecyclableObject recyclableSecondParam) recyclableSecondParam.IncrementRefCount();
        if (thirdParameter is IRecyclableObject recyclableThirdParam) recyclableThirdParam.IncrementRefCount();
        return base.IncrementRefCount();
    }

    public void Configure(Func<TP, TP2, TP3, TR> toExecute, TP firstParam, TP2 secondParam, TP3 thirdParam)
    {
        toInvoke       = toExecute;
        firstParameter = firstParam;
        if (firstParameter is IRecyclableObject recyclableFirstParam) recyclableFirstParam.IncrementRefCount();
        secondParameter = secondParam;
        if (secondParameter is IRecyclableObject recyclableSecondParam) recyclableSecondParam.IncrementRefCount();
        thirdParameter = thirdParam;
        if (thirdParameter is IRecyclableObject recyclableThirdParam) recyclableThirdParam.IncrementRefCount();
    }
}

public class ThreeParamsAsyncResultPayload<TR, TP, TP2, TP3> : ReusableValueTaskSource<TR>, IInvokeablePayload
{
    private TP  firstParameter  = default!;
    private TP2 secondParameter = default!;
    private TP3 thirdParameter  = default!;

    private Func<TP, TP2, TP3, ValueTask<TR>>? toInvoke;

    public bool IsAsyncInvoke => true;

    public async ValueTask InvokeAsync()
    {
        if (toInvoke != null)
        {
            var result = await toInvoke.Invoke(firstParameter, secondParameter, thirdParameter);
            TrySetResult(result);
        }
    }

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

    public override int DecrementRefCount()
    {
        if (firstParameter is IRecyclableObject recyclableFirstParam) recyclableFirstParam.DecrementRefCount();
        if (secondParameter is IRecyclableObject recyclableSecondParam) recyclableSecondParam.DecrementRefCount();
        if (thirdParameter is IRecyclableObject recyclableThirdParam) recyclableThirdParam.DecrementRefCount();
        return base.DecrementRefCount();
    }

    public override int IncrementRefCount()
    {
        if (firstParameter is IRecyclableObject recyclableFirstParam) recyclableFirstParam.IncrementRefCount();
        if (secondParameter is IRecyclableObject recyclableSecondParam) recyclableSecondParam.IncrementRefCount();
        if (thirdParameter is IRecyclableObject recyclableThirdParam) recyclableThirdParam.IncrementRefCount();
        return base.IncrementRefCount();
    }

    public void Configure(Func<TP, TP2, TP3, ValueTask<TR>> toExecute, TP firstParam, TP2 secondParam, TP3 thirdParam)
    {
        toInvoke       = toExecute;
        firstParameter = firstParam;
        if (firstParameter is IRecyclableObject recyclableFirstParam) recyclableFirstParam.IncrementRefCount();
        secondParameter = secondParam;
        if (secondParameter is IRecyclableObject recyclableSecondParam) recyclableSecondParam.IncrementRefCount();
        thirdParameter = thirdParam;
        if (thirdParameter is IRecyclableObject recyclableThirdParam) recyclableThirdParam.IncrementRefCount();
    }
}
