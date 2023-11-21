namespace FortitudeCommon.Chronometry.Timers;

public class IntervalTimerUpdate : OneOffTimerUpdate
{
    public override bool Cancel()
    {
        CallBackRunInfo.IsPaused = true;
        CallBackRunInfo.MaxNumberOfCalls = CallBackRunInfo.CurrentNumberOfCalls;
        var removed = UpdateableTimer.Remove(CallBackRunInfo);
        return removed && CallBackRunInfo.IsFinished;
    }

    public override bool UpdateWaitPeriod(TimeSpan newWaitFromNowTimeSpan)
    {
        CallBackRunInfo.IntervalPeriodTimeSpan = newWaitFromNowTimeSpan;
        CallBackRunInfo.NextScheduleTime = TimeContext.UtcNow + newWaitFromNowTimeSpan;
        return CallBackRunInfo.RegisteredTimer.Change(newWaitFromNowTimeSpan, newWaitFromNowTimeSpan) &&
               !CallBackRunInfo.IsFinished;
    }

    public override bool Pause()
    {
        CallBackRunInfo.IsPaused = true;
        return CallBackRunInfo.RegisteredTimer.Change(Timer.MaxTimerMs, Timer.MaxTimerMs) &&
               !CallBackRunInfo.IsFinished;
    }

    public override bool Resume()
    {
        CallBackRunInfo.IsPaused = false;
        var now = TimeContext.UtcNow;
        var launchTime = CallBackRunInfo.NextScheduleTime < now ?
            TimeSpan.Zero :
            now - CallBackRunInfo.NextScheduleTime;
        return CallBackRunInfo.RegisteredTimer.Change(launchTime, CallBackRunInfo.IntervalPeriodTimeSpan) &&
               !CallBackRunInfo.IsFinished;
    }
}
