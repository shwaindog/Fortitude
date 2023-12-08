#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;

#endregion

namespace FortitudeCommon.Chronometry.Timers;

public abstract class TimerCallBackRunInfo : ReusableObject<TimerCallBackRunInfo>, IComparable<TimerCallBackRunInfo>
{
    protected int CurrentInvocations;

    internal int QueueCount;

    internal int RunState;
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

    public override bool Recycle()
    {
        if (!IsInRecycler)
        {
            Interlocked.CompareExchange(ref RunState, (int)RunStateEnum.RecycleRequested
                , (int)RunStateEnum.CheckQueueCount);
            Interlocked.CompareExchange(ref RunState, (int)RunStateEnum.RecycleRequested
                , (int)RunStateEnum.Executing);
            var currentState = Interlocked.CompareExchange(ref RunState, (int)RunStateEnum.RecycleRequested
                , (int)RunStateEnum.Queued);

            if (currentState == (int)RunStateEnum.NotRunning)
                Recycler?.Recycle(this);
        }

        return IsInRecycler;
    }

    protected void AsyncPostRunRecycleCheck()
    {
        if (Interlocked.Decrement(ref QueueCount) <= 0)
        {
            if (Interlocked.CompareExchange(ref RunState, (int)RunStateEnum.NotRunning
                    , (int)RunStateEnum.RecycleRequested) == (int)RunStateEnum.RecycleRequested)
            {
                Recycle();
            }
            else
            {
                var lastRunState = Interlocked.CompareExchange(ref RunState, (int)RunStateEnum.CheckQueueCount
                    , (int)RunStateEnum.Executing);
                if (lastRunState == (int)RunStateEnum.Executing && Thread.VolatileRead(ref QueueCount) > 0)
                    Interlocked.CompareExchange(ref RunState, (int)RunStateEnum.Queued
                        , (int)RunStateEnum.CheckQueueCount);
                else if (lastRunState == (int)RunStateEnum.Executing)
                    Interlocked.CompareExchange(ref RunState, (int)RunStateEnum.NotRunning
                        , (int)RunStateEnum.CheckQueueCount);
            }
        }
        else
        {
            Interlocked.CompareExchange(ref RunState, (int)RunStateEnum.Queued
                , (int)RunStateEnum.Executing);
        }
    }

    public override void Reset()
    {
        RunState = (int)RunStateEnum.NotRunning;
        QueueCount = 0;
        CurrentInvocations = 0;
        FirstScheduledTime = DateTime.MaxValue;
        LastRunTime = DateTime.MaxValue;
        LastRunTime = DateTime.MaxValue;
        NextScheduleTime = DateTime.MaxValue;
        IntervalPeriodTimeSpan = Timer.MaxTimerSpan;
        base.Reset();
    }

    public override TimerCallBackRunInfo CopyFrom(TimerCallBackRunInfo source
        , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        CurrentNumberOfCalls = source.CurrentNumberOfCalls;
        FirstScheduledTime = source.FirstScheduledTime;
        LastRunTime = source.LastRunTime;
        NextScheduleTime = source.NextScheduleTime;
        IntervalPeriodTimeSpan = source.IntervalPeriodTimeSpan;
        MaxNumberOfCalls = source.MaxNumberOfCalls;
        IsPaused = source.IsPaused;
        RegisteredTimer = source.RegisteredTimer;
        return this;
    }

    public abstract bool RunCallbackOnThreadPool();

    public abstract bool RunCallbackOnThisThread();

    protected enum RunStateEnum
    {
        NotRunning = 0
        , Queued = 1
        , Executing = 2
        , CheckQueueCount = 3
        , RecycleRequested = 4
    }
}
