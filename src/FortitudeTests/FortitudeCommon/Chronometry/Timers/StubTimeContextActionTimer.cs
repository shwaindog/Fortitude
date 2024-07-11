// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.Chronometry.Timers;

#endregion

namespace FortitudeTests.FortitudeCommon.Chronometry.Timers;

public class StubTimerContextProvider : ITimerProvider
{
    public ValueTask DisposeAsync()
    {
        Dispose();
        return ValueTask.CompletedTask;
    }

    public void Dispose()
    {
        TimerContext.Provider = new RealTimerProvider();
    }

    public IUpdateableTimer CreateUpdateableTimer(string? name = "Unnamed-Timer") => new StubTimeContextTimer(name);
}

public class StubTimer : ITimer, IStubTimeProgressListener
{
    private readonly StubTimeContext       stubTimeContext;
    private readonly StubTimeContextTimer  stubTimeContextTimer;
    private readonly TimerCallback         timerCallBack;
    private readonly TimerCallBackRunInfo? timerCallBackRunInfo;

    private bool hasSelectedTimeBefore;
    private bool intervalTimerWasPaused;

    private bool     isDisposed;
    private DateTime nextScheduledTime;
    private TimeSpan periodTimeSpan;

    public StubTimer(StubTimeContextTimer stubTimeContextTimer, TimerCallback timerCallBack, DateTime nextScheduledTime)
    {
        stubTimeContext = TimeContext.Provider as StubTimeContext ??
                          throw new Exception("Expected StubTimeContext to be the current TimeContext.Provider");
        this.nextScheduledTime    = nextScheduledTime;
        this.stubTimeContextTimer = stubTimeContextTimer;
        this.timerCallBack        = timerCallBack;
        IsIntervalTimer           = false;
    }

    public StubTimer
        (StubTimeContextTimer stubTimeContextTimer, TimerCallback timerCallBack, TimerCallBackRunInfo timerCallBackRunInfo, TimeSpan periodTimeSpan)
    {
        stubTimeContext = TimeContext.Provider as StubTimeContext ??
                          throw new Exception("Expected StubTimeContext to be the current TimeContext.Provider");
        nextScheduledTime         = TimeContext.UtcNow + periodTimeSpan;
        this.stubTimeContextTimer = stubTimeContextTimer;
        this.timerCallBackRunInfo = timerCallBackRunInfo;
        this.timerCallBack        = timerCallBack;
        this.periodTimeSpan       = periodTimeSpan;
        IsIntervalTimer           = true;
    }

    public bool IsIntervalTimer { get; set; }

    public DateTime? GetNextScheduleEvent(DateTime fromDateTime, DateTime toDateTime)
    {
        if (!IsIntervalTimer)
        {
            var nextOneOffTime = stubTimeContextTimer.NextOneOffTimeCallBackTime() ?? DateTime.MaxValue;
            hasSelectedTimeBefore = nextOneOffTime == nextScheduledTime && hasSelectedTimeBefore;
            nextScheduledTime     = nextOneOffTime;
        }
        return (nextScheduledTime >= fromDateTime && nextScheduledTime <= toDateTime)
            || (!hasSelectedTimeBefore && nextScheduledTime < fromDateTime)
            ? nextScheduledTime
            : null;
    }

    public void ProgressTime(DateTime fromDateTime, DateTime toDateTime)
    {
        var isScheduledInRange = (nextScheduledTime >= fromDateTime && nextScheduledTime <= toDateTime) ||
                                 (!hasSelectedTimeBefore && nextScheduledTime < fromDateTime);
        hasSelectedTimeBefore = true;
        if (IsIntervalTimer)
        {
            if (isScheduledInRange)
                if (!timerCallBackRunInfo!.IsFinished && !timerCallBackRunInfo.IsPaused)
                {
                    timerCallBack(timerCallBackRunInfo);
                    var nextIntervalTime = timerCallBackRunInfo.NextScheduleTime;
                    hasSelectedTimeBefore = nextIntervalTime == nextScheduledTime && hasSelectedTimeBefore;
                    nextScheduledTime     = nextIntervalTime;
                }
            if (timerCallBackRunInfo!.IsFinished)
            {
                Dispose();
            }
            else
            {
                if (intervalTimerWasPaused && !timerCallBackRunInfo!.IsPaused) hasSelectedTimeBefore = false;
                intervalTimerWasPaused = timerCallBackRunInfo!.IsPaused;
            }
        }
        else if (isScheduledInRange)
        {
            timerCallBack(stubTimeContextTimer);
        }
    }

    public void Dispose()
    {
        if (isDisposed) return;
        isDisposed = true;
        stubTimeContext.RemoveStubTimeProgressListener(this);
        timerCallBackRunInfo?.DecrementRefCount();
    }

    public ValueTask DisposeAsync()
    {
        Dispose();
        return ValueTask.CompletedTask;
    }

    public bool Change(TimeSpan dueTime, TimeSpan period)
    {
        if (timerCallBackRunInfo?.IsFinished ?? false) return false;
        nextScheduledTime = TimeContext.UtcNow + dueTime;
        if (timerCallBackRunInfo != null) timerCallBackRunInfo.NextScheduleTime = TimeContext.UtcNow + dueTime;
        periodTimeSpan = period;
        return true;
    }
}

public class StubTimeContextTimer : UpdateableTimer
{
    public StubTimeContextTimer(string? name = "Unnamed-Timer") : base(name) { }

    protected override ITimer CreateOneOffTimer()
    {
        var stubTimeContext = TimeContext.Provider as StubTimeContext ??
                              throw new Exception("Expected StubTimeContext to be the current TimeContext.Provider");
        var stubTimer = new StubTimer(this, OneOffTimerTicker, DateTime.MaxValue);
        stubTimeContext.AddStubTimeProgressListener(stubTimer);
        return stubTimer;
    }

    public DateTime? NextOneOffTimeCallBackTime() => NextScheduledOneOffTimerCallBackRunInfo?.NextScheduleTime;

    protected override ITimer CreateIntervalTimer(TimerCallBackRunInfo timerCallBack, TimeSpan periodTimeSpan)
    {
        var stubTimeContext = TimeContext.Provider as StubTimeContext ??
                              throw new Exception("Expected StubTimeContext to be the current TimeContext.Provider");
        var stubTimer = new StubTimer(this, IntervalTimerTicker, timerCallBack, periodTimeSpan);
        stubTimeContext.AddStubTimeProgressListener(stubTimer);
        return stubTimer;
    }
}
