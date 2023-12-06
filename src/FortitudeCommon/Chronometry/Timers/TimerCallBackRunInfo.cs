#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;

#endregion

namespace FortitudeCommon.Chronometry.Timers;

public abstract class TimerCallBackRunInfo : ReusableObject<TimerCallBackRunInfo>, IComparable<TimerCallBackRunInfo>
    , IRecyclableObject
{
    protected int CurrentInvocations;
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

    public override void Reset()
    {
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
}
