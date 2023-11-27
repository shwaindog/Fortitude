#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;

#endregion

namespace FortitudeCommon.Chronometry.Timers;

public abstract class TimerCallBackRunInfo : IComparable<TimerCallBackRunInfo>, IRecyclableObject<TimerCallBackRunInfo>
{
    protected int CurrentInvocations;
    private int refCount;
    public DateTime FirstScheduledTime { get; set; }
    public DateTime? LastRunTime { get; set; }
    public DateTime NextScheduleTime { get; set; }
    public TimeSpan IntervalPeriodTimeSpan { get; set; }

    public int CurrentNumberOfCalls
    {
        get => CurrentInvocations;
        set => CurrentInvocations = value;
    }

    public int MaxNumberOfCalls { get; set; }
    public bool IsPaused { get; set; }
    public bool IsFinished => CurrentNumberOfCalls >= MaxNumberOfCalls;
    public System.Threading.Timer RegisteredTimer { get; set; } = null!;

    public int CompareTo(TimerCallBackRunInfo? other)
    {
        var tickDiff = NextScheduleTime.Ticks - (other?.NextScheduleTime.Ticks ?? 0);
        return tickDiff < 0L ? -1 : tickDiff > 0L ? 1 : 0;
    }

    public int RefCount => refCount;
    public bool RecycleOnRefCountZero { get; set; } = true;
    public bool AutoRecycledByProducer { get; set; }
    public bool IsInRecycler { get; set; }
    public IRecycler? Recycler { get; set; }

    public virtual void CopyFrom(TimerCallBackRunInfo source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        FirstScheduledTime = source.FirstScheduledTime;
        LastRunTime = source.LastRunTime;
        IsPaused = source.IsPaused;
        NextScheduleTime = source.NextScheduleTime;
        IntervalPeriodTimeSpan = source.IntervalPeriodTimeSpan;
        CurrentNumberOfCalls = source.CurrentNumberOfCalls;
        MaxNumberOfCalls = source.MaxNumberOfCalls;
        RegisteredTimer = source.RegisteredTimer;
    }

    public void CopyFrom(IStoreState source, CopyMergeFlags copyMergeFlags)
    {
        CopyFrom((TimerCallBackRunInfo)source, copyMergeFlags);
    }

    public int DecrementRefCount()
    {
        if (Interlocked.Decrement(ref refCount) <= 0 && RecycleOnRefCountZero) Recycle();
        return refCount;
    }

    public int IncrementRefCount() => Interlocked.Increment(ref refCount);

    public bool Recycle()
    {
        if (refCount <= 0 || !RecycleOnRefCountZero) Recycler!.Recycle(this);

        return IsInRecycler;
    }

    public abstract bool RunCallbackOnThreadPool();

    public abstract bool RunCallbackOnThisThread();
}
