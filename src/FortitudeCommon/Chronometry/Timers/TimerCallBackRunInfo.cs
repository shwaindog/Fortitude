#region

using FortitudeCommon.AsyncProcessing.Tasks;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Types;

#endregion

namespace FortitudeCommon.Chronometry.Timers;

public abstract class TimerCallBackRunInfo : ReusableObject<TimerCallBackRunInfo>, IComparable<TimerCallBackRunInfo>
{
    protected static IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(TimerCallBackRunInfo));

    protected int CurrentInvocations;
    protected ReusableValueTaskSource<DateTime>? NextExecutionComplete;
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

    public ValueTask<DateTime> NextThreadPoolExecutionAsync()
    {
        var reusableDateTimeSource = Recycler?.Borrow<ReusableValueTaskSource<DateTime>>() ??
                                     new ReusableValueTaskSource<DateTime>();
        if (Interlocked.CompareExchange(ref NextExecutionComplete, reusableDateTimeSource, null) !=
            null)
            reusableDateTimeSource.DecrementRefCount();
        return NextExecutionComplete.GenerateValueTask();
    }

    public void Stop()
    {
        MaxNumberOfCalls = CurrentNumberOfCalls;
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

    public override void StateReset()
    {
        MaxNumberOfCalls = 0;
        IsPaused = false;
        CurrentInvocations = 0;
        FirstScheduledTime = DateTime.MaxValue;
        LastRunTime = DateTime.MaxValue;
        LastRunTime = DateTime.MaxValue;
        NextScheduleTime = DateTime.MaxValue;
        IntervalPeriodTimeSpan = Timer.MaxTimerSpan;
        base.StateReset();
    }

    public abstract bool RunCallbackOnThreadPool();

    public abstract bool RunCallbackOnThisThread();

    public override string ToString() =>
        $"{GetType().Name}({nameof(FirstScheduledTime)}: {FirstScheduledTime}, " +
        $"{nameof(CurrentNumberOfCalls)}: {CurrentNumberOfCalls}, {nameof(RegisteredTimer)}: {RegisteredTimer}, " +
        $"{nameof(IsPaused)}: {IsPaused}, {nameof(IsFinished)}: {IsFinished}, {nameof(LastRunTime)}: {LastRunTime}, " +
        $"{nameof(MaxNumberOfCalls)}: {MaxNumberOfCalls}, {nameof(NextScheduleTime)}: {NextScheduleTime}, " +
        $"{nameof(RefCount)}, {RefCount})";
}
