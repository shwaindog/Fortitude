#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;

#endregion

namespace Fortitude.EventProcessing.BusRules.MessageBus.Tasks;

public interface ITaskPayload : IRecyclableObject
{
    void Invoke();
}

public class TaskPayload : ITaskPayload, IRecyclableObject<TaskPayload>
{
    private int refCount;
    public SendOrPostCallback Callback { get; set; } = null!;
    public object? State { get; set; }

    public void CopyFrom(TaskPayload source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        Callback = source.Callback;
        State = source.State;
    }

    public void CopyFrom(IStoreState source, CopyMergeFlags copyMergeFlags)
    {
        CopyFrom((TaskPayload)source, copyMergeFlags);
    }

    public int RefCount => refCount;
    public bool RecycleOnRefCountZero { get; set; } = true;
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
        if (refCount == 0 || !RecycleOnRefCountZero) Recycler!.Recycle(this);

        return IsInRecycler;
    }

    public void Invoke()
    {
        Callback(State);
    }
}
