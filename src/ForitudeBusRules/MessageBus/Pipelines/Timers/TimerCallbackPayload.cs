#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;

#endregion

namespace Fortitude.EventProcessing.BusRules.MessageBus.Pipelines.Timers;

public interface ITimerCallbackPayload : IRecyclableObject
{
    void Invoke();
}

public class TimerCallbackPayload<T> : ITimerCallbackPayload, IRecyclableObject<TimerCallbackPayload<T>> where T : class
{
    private int refCount;
    public Action? Action { get; set; }
    public Action<T?>? ActionState { get; set; }
    public T? State { get; set; }

    public void CopyFrom(TimerCallbackPayload<T> source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        Action = source.Action;
        ActionState = source.ActionState;
        State = source.State;
    }

    public void CopyFrom(IStoreState source, CopyMergeFlags copyMergeFlags)
    {
        CopyFrom((TimerCallbackPayload<T>)source, copyMergeFlags);
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
        if (Action != null && ActionState != null)
            throw new InvalidDataException("Only expect Action or ActionState to be set");
        if (Action != null)
            Action();
        else if (ActionState != null)
            ActionState(State);
        else
            throw new InvalidDataException("Expected either Action or ActionState to be set");
    }
}
