#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;

#endregion

namespace Fortitude.EventProcessing.BusRules.MessageBus.Tasks;

internal class TaskExecuteAction : IRecyclableObject<TaskExecuteAction>
{
    private int refCount;
    public SendOrPostCallback TaskAction { get; set; } = null!;
    public object? State { get; set; }

    public void CopyFrom(TaskExecuteAction source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        TaskAction = source.TaskAction;
        State = source.State;
    }

    public void CopyFrom(IStoreState source, CopyMergeFlags copyMergeFlags)
    {
        CopyFrom((TaskExecuteAction)source, copyMergeFlags);
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
        TaskAction(State);
    }
}
