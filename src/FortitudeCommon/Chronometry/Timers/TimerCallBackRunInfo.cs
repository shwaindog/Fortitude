// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.AsyncProcessing.Tasks;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Types;

#endregion

namespace FortitudeCommon.Chronometry.Timers;

public interface ITimerCallBackRunInfo : IReusableObject<ITimerCallBackRunInfo>, IComparable<ITimerCallBackRunInfo>
{
    DateTime  FirstScheduledTime     { get; }
    DateTime? LastRunTime            { get; set; }
    DateTime  NextScheduleTime       { get; set; }
    TimeSpan  IntervalPeriodTimeSpan { get; set; }
    int       CurrentNumberOfCalls   { get; set; }
    int       MaxNumberOfCalls       { get; set; }

    bool IsPaused   { get; set; }
    bool IsFinished { get; }

    ITimer RegisteredTimer { get; set; }

    ValueTask<DateTime> NextThreadPoolExecutionAsync();

    void Stop();
    bool RunCallbackOnThreadPool();
    bool RunCallbackOnThisThread();
}

public interface ICaptureTimesState
{
    ITimerUpdate? TimerUpdate { get; set; }
    void          CaptureTriggerAndScheduleTime();
}

public interface ITimerUpdateCallBackRunInfo : ITimerCallBackRunInfo, ICaptureTimesState { }

public abstract class TimerCallBackRunInfo : ReusableObject<ITimerCallBackRunInfo>, ITimerCallBackRunInfo
{
    protected static IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(TimerCallBackRunInfo));

    protected int CurrentInvocations;

    protected ReusableValueTaskSource<DateTime>? NextExecutionComplete;

    public DateTime  FirstScheduledTime     { get; set; }
    public DateTime? LastRunTime            { get; set; }
    public DateTime  NextScheduleTime       { get; set; }
    public TimeSpan  IntervalPeriodTimeSpan { get; set; }

    public int CurrentNumberOfCalls
    {
        get => CurrentInvocations;
        set => CurrentInvocations = value;
    }

    public int  MaxNumberOfCalls { get; set; }
    public bool IsPaused         { get; set; }

    public bool IsFinished => CurrentNumberOfCalls >= MaxNumberOfCalls;

    public ITimer RegisteredTimer { get; set; } = null!;

    public int CompareTo(ITimerCallBackRunInfo? other)
    {
        var tickDiff = NextScheduleTime.Ticks - (other?.NextScheduleTime.Ticks ?? 0);
        return tickDiff < 0L ? -1 : tickDiff > 0L ? 1 : 0;
    }

    public ValueTask<DateTime> NextThreadPoolExecutionAsync()
    {
        var reusableDateTimeSource = Recycler?.Borrow<ReusableValueTaskSource<DateTime>>() ?? new ReusableValueTaskSource<DateTime>();
        if (Interlocked.CompareExchange(ref NextExecutionComplete, reusableDateTimeSource, null) != null) reusableDateTimeSource.DecrementRefCount();
        return NextExecutionComplete.GenerateValueTask();
    }

    public void Stop()
    {
        MaxNumberOfCalls = CurrentNumberOfCalls;
    }

    public override ITimerCallBackRunInfo CopyFrom
    (ITimerCallBackRunInfo source
      , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        CurrentNumberOfCalls   = source.CurrentNumberOfCalls;
        FirstScheduledTime     = source.FirstScheduledTime;
        LastRunTime            = source.LastRunTime;
        NextScheduleTime       = source.NextScheduleTime;
        IntervalPeriodTimeSpan = source.IntervalPeriodTimeSpan;

        MaxNumberOfCalls = source.MaxNumberOfCalls;
        IsPaused         = source.IsPaused;
        RegisteredTimer  = source.RegisteredTimer;

        return this;
    }

    public override void StateReset()
    {
        MaxNumberOfCalls   = 0;
        IsPaused           = false;
        CurrentInvocations = 0;
        FirstScheduledTime = DateTime.MaxValue;
        LastRunTime        = DateTime.MaxValue;
        LastRunTime        = DateTime.MaxValue;
        NextScheduleTime   = DateTime.UtcNow + UpdateableTimer.MaxTimerSpan;

        IntervalPeriodTimeSpan = UpdateableTimer.MaxTimerSpan;
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
