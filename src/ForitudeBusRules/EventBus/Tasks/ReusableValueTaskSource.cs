#region

using System.Threading.Tasks.Sources;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;

#endregion

namespace Fortitude.EventProcessing.BusRules.EventBus.Tasks;

public interface IMessageResponseSource : IValueTaskSource, IRecyclableObject<IMessageResponseSource> { }

// credit here to Microsoft.AspNetCore.Server.Kestrel.Core.Internal.ManualResetValueTaskSource
// copy taken from Microsoft.AspNetCore.Server.Kestrel.Core.Internal.ManualResetValueTaskSource in Microsoft.AspNetCore.Server.IIS
public class ReusableValueTaskSource<T> : IValueTaskSource<T>, IMessageResponseSource
    , IStoreState<ReusableValueTaskSource<T>>
{
    private ManualResetValueTaskSourceCore<T> core; // mutable struct; do not make this readonly
    private int refCount = 0;


    public bool RunContinuationsAsynchronously
    {
        get => core.RunContinuationsAsynchronously;
        set => core.RunContinuationsAsynchronously = value;
    }

    public short Version => core.Version;

    public void CopyFrom(IStoreState source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        CopyFrom((ReusableValueTaskSource<T>)source, copyMergeFlags);
    }

    public int RefCount => refCount;
    public bool RecycleOnRefCountZero { get; set; }
    public bool AutoRecycledByProducer { get; set; }
    public bool IsInRecycler { get; set; }
    public IRecycler? Recycler { get; set; }

    public int DecrementRefCount()
    {
        if (Interlocked.Decrement(ref refCount) == 0 && RecycleOnRefCountZero) Recycle();
        return refCount;
    }

    public int IncrementRefCount() => Interlocked.Increment(ref refCount);

    public bool Recycle()
    {
        if (!AutoRecycledByProducer && !IsInRecycler && (refCount == 0 || !RecycleOnRefCountZero))
        {
            Reset();
            Recycler!.Recycle(this);
        }

        return IsInRecycler;
    }

    public void CopyFrom(IMessageResponseSource source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        CopyFrom((ReusableValueTaskSource<T>)source, copyMergeFlags);
    }

    void IValueTaskSource.GetResult(short token) => core.GetResult(token);

    public void CopyFrom(ReusableValueTaskSource<T> source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        core = source.core;
    }

    public T GetResult(short token) => core.GetResult(token);
    public ValueTaskSourceStatus GetStatus(short token) => core.GetStatus(token);

    public void OnCompleted(Action<object?> continuation, object? state, short token
        , ValueTaskSourceOnCompletedFlags flags) =>
        core.OnCompleted(continuation, state, token, flags);

    public virtual void Reset() => core.Reset();

    public void SetResult(T result) => core.SetResult(result);
    public void SetException(Exception error) => core.SetException(error);

    public ValueTaskSourceStatus GetStatus() => core.GetStatus(core.Version);

    public void TrySetResult(T result)
    {
        if (core.GetStatus(core.Version) == ValueTaskSourceStatus.Pending) core.SetResult(result);
    }

    public override string ToString() => $"{GetType()}{{ {nameof(refCount)}: {refCount} }}";
}
