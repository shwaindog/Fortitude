// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Threading.Tasks.Sources;
using FortitudeBusRules.Messages;
using FortitudeCommon.AsyncProcessing.Tasks;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;

#endregion

namespace FortitudeBusRules.BusMessaging.Tasks;

public class NoMessageResponseSource : IAsyncResponseSource
{
    public void GetResult(short token)
    {
        throw new NotImplementedException("No value should be passed into this instance");
    }

    public ValueTaskSourceStatus GetStatus(short token) => throw new NotImplementedException("No request should be be made on this instance");

    public void OnCompleted
    (Action<object?> continuation, object? state, short token
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

    public int  RefCount                  => 0;
    public bool AutoRecycleAtRefCountZero { get; set; }
    public bool IsInRecycler              { get; set; }

    public IRecycler? Recycler { get; set; }

    public int DecrementRefCount() => 0;

    public int IncrementRefCount() => 0;

    public bool Recycle() => false;

    public void StateReset() { }

    public void CopyFrom(ITransferState source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default) { }

    public void CopyFrom(IAsyncResponseSource source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default) { }
}

public class NoMessageResponseSource<T> : NoMessageResponseSource, IResponseValueTaskSource<T>
{
    public Task<T> AsTask => throw new NotImplementedException("No request should be be made on this instance");

    public short Version => throw new NotImplementedException("No request should be be made on this instance");

    public IDispatchResult? DispatchResult { get; set; }

    public void TrySetResult(T result)
    {
        throw new NotImplementedException("No request should be be made on this instance");
    }

    public void SetResult(T result)
    {
        throw new NotImplementedException("No request should be be made on this instance");
    }
}
