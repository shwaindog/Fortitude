#region

using System.Threading.Tasks.Sources;
using FortitudeBusRules.Messages;
using FortitudeCommon.AsyncProcessing.Tasks;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;

#endregion

namespace FortitudeBusRules.BusMessaging.Tasks;

public class NoMessageResponseSource : IAsyncResponseSource
{
    public bool AutoRecycledByProducer { get; set; }

    public void GetResult(short token)
    {
        throw new NotImplementedException("No value should be passed into this instance");
    }

    public ValueTaskSourceStatus GetStatus(short token) => throw new NotImplementedException("No request should be be made on this instance");

    public void OnCompleted(Action<object?> continuation, object? state, short token
        , ValueTaskSourceOnCompletedFlags flags)
    {
        throw new NotImplementedException("No request should be be made on this instance");
    }

    public bool IsCompleted => false;

    public void SetException(Exception error)
    {
        throw new NotImplementedException("No request should be be made on this instance");
    }

    public Type ResponseType => typeof(object);

    public int RefCount => 0;
    public bool AutoRecycleAtRefCountZero { get; set; }
    public bool IsInRecycler { get; set; }
    public IRecycler? Recycler { get; set; }
    public int DecrementRefCount() => 0;

    public int IncrementRefCount() => 0;

    public bool Recycle() => false;

    public void StateReset() { }

    public void CopyFrom(IStoreState source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default) { }

    public void CopyFrom(IAsyncResponseSource source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default) { }
}

public class NoMessageResponseSource<T> : NoMessageResponseSource, IResponseValueTaskSource<T>
{
    public Task<RequestResponse<T>> AsTask => throw new NotImplementedException("No request should be be made on this instance");

    public short Version => throw new NotImplementedException("No request should be be made on this instance");

    public ValueTask<RequestResponse<T>>? AwaitingValueTask
    {
        get => throw new NotImplementedException("No request should be be made on this instance");
        set => throw new NotImplementedException("No request should be be made on this instance");
    }

    public void TrySetResultFromAwaitingTask(ValueTask<RequestResponse<T>> awaitingValueTask)
    {
        throw new NotImplementedException("No request should be be made on this instance");
    }

    public void TrySetResultFromAwaitingTask(Task<RequestResponse<T>> awaitingTask)
    {
        throw new NotImplementedException("No request should be be made on this instance");
    }

    public void TrySetResult(RequestResponse<T> result)
    {
        throw new NotImplementedException("No request should be be made on this instance");
    }

    public void SetResult(RequestResponse<T> result)
    {
        throw new NotImplementedException("No request should be be made on this instance");
    }

    public void TrySetResult(T result)
    {
        throw new NotImplementedException("No request should be be made on this instance");
    }

    public void TrySetResultFromAwaitingTask(ValueTask<T> awaitingValueTask)
    {
        throw new NotImplementedException("No request should be be made on this instance");
    }

    public void TrySetResultFromAwaitingTask(Task<T> awaitingTask)
    {
        throw new NotImplementedException("No request should be be made on this instance");
    }

    public void SetResult(T result)
    {
        throw new NotImplementedException("No request should be be made on this instance");
    }

    public ValueTask<RequestResponse<T>> GenerateValueTask() => throw new NotImplementedException("No request should be be made on this instance");
}
