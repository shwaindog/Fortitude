﻿#region

using Fortitude.EventProcessing.BusRules.Messaging;

#endregion

namespace Fortitude.EventProcessing.BusRules.MessageBus.Tasks;

public interface IResponseValueTaskSource<T> : IReusableMessageResponseSource<RequestResponse<T>>
{
    void TrySetResult(T result);
    void TrySetResultFromAwaitingTask(ValueTask<T> awaitingValueTask);
    void TrySetResultFromAwaitingTask(Task<T> awaitingTask);
    void SetResult(T result);
}

public class ReusableResponseValueTaskSource<T> : ReusableValueTaskSource<RequestResponse<T>>
    , IResponseValueTaskSource<T>
{
    public IDispatchResult? DispatchResult { get; set; }

    public void TrySetResult(T result)
    {
        TrySetResult(new RequestResponse<T>(DispatchResult, result));
    }

    public void TrySetResultFromAwaitingTask(ValueTask<T> awaitingValueTask)
    {
        if (awaitingValueTask.IsCompleted)
            try
            {
                TrySetResult(awaitingValueTask.Result);
            }
            catch (AggregateException ae)
            {
                SetException(ae.InnerException!);
            }
            catch (Exception e)
            {
                SetException(e);
            }
        else
            throw new ArgumentException("Expected awaitingValueTask to be completed");
    }

    public void TrySetResultFromAwaitingTask(Task<T> awaitingTask)
    {
        if (awaitingTask.IsCompleted)
            try
            {
                TrySetResult(awaitingTask.Result);
            }
            catch (Exception e)
            {
                SetException(e);
            }
        else
            throw new ArgumentException("Expected awaitingTask to be completed");
    }

    public void SetResult(T result) => SetResult(new RequestResponse<T>(DispatchResult, result));

    public override void Reset()
    {
        DispatchResult = null;
        base.Reset();
    }
}