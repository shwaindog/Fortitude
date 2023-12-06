#region

using System.Threading.Tasks.Sources;
using FortitudeCommon.AsyncProcessing.Tasks;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;

#endregion

namespace Fortitude.EventProcessing.BusRules.MessageBus.Tasks;

public class NoMessageResponseSource : IAsyncResponseSource
{
    public bool AutoRecycledByProducer { get; set; }

    public void GetResult(short token)
    {
        throw new NotImplementedException("No value should be passed into this instance");
    }

    public ValueTaskSourceStatus GetStatus(short token) =>
        throw new NotImplementedException("No request should be be made on this instance");

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

    public int RefCount => 0;
    public bool AutoRecycleAtRefCountZero { get; set; }
    public bool IsInRecycler { get; set; }
    public IRecycler? Recycler { get; set; }
    public int DecrementRefCount() => 0;

    public int IncrementRefCount() => 0;

    public bool Recycle() => false;

    public void CopyFrom(IStoreState source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default) { }

    public void CopyFrom(IAsyncResponseSource source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default) { }
}
