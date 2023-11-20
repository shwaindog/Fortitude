#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;

#endregion

namespace FortitudeCommon.Chronometry.Timers;

public class TimerCallBackRunInfo : IComparable<TimerCallBackRunInfo>, IRecyclableObject<TimerCallBackRunInfo>
{
    private int currentNumberOfCalls;
    private int refCount;

    public WaitCallback? Callback { get; set; }
    public DateTime FirstScheduledTime { get; set; }
    public DateTime? LastRunTime { get; set; }
    public DateTime NextScheduleTime { get; set; }
    public TimeSpan RepeatPeriodTimeSpan { get; set; }

    public int CurrentNumberOfCalls
    {
        get => currentNumberOfCalls;
        set => currentNumberOfCalls = value;
    }

    public int MaxNumberOfCalls { get; set; }
    public object? State { get; set; }
    public bool IsPaused { get; set; }
    public bool IsFinished => CurrentNumberOfCalls < MaxNumberOfCalls;
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

    public void CopyFrom(TimerCallBackRunInfo source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        Callback = source.Callback;
        FirstScheduledTime = source.FirstScheduledTime;
        LastRunTime = source.LastRunTime;
        NextScheduleTime = source.NextScheduleTime;
        RepeatPeriodTimeSpan = source.RepeatPeriodTimeSpan;
        CurrentNumberOfCalls = source.CurrentNumberOfCalls;
        MaxNumberOfCalls = source.MaxNumberOfCalls;
        State = source.State;
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
        if (refCount <= 0 && !RecycleOnRefCountZero) Recycler!.Recycle(this);

        return IsInRecycler;
    }

    public bool RunCallbackOnThreadPool()
    {
        if (!IsFinished)
        {
            Interlocked.Increment(ref currentNumberOfCalls);
            if (!IsFinished)
                NextScheduleTime += RepeatPeriodTimeSpan;
            else
                NextScheduleTime = DateTime.MaxValue;

            LastRunTime = TimeContext.UtcNow;
            return ThreadPool.QueueUserWorkItem(Callback!, State);
        }

        return false;
    }

    public bool RunCallbackOnThisThread()
    {
        if (!IsFinished)
        {
            Interlocked.Increment(ref currentNumberOfCalls);
            if (!IsFinished)
                NextScheduleTime += RepeatPeriodTimeSpan;
            else
                NextScheduleTime = DateTime.MaxValue;

            LastRunTime = TimeContext.UtcNow;
            Callback!(State);
            return true;
        }

        return false;
    }
}
