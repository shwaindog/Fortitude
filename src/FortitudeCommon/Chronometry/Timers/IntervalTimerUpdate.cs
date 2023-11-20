namespace FortitudeCommon.Chronometry.Timers;

public class IntervalTimerUpdate : OneOffTimerUpdate
{
    public override bool Cancel()
    {
        CallBackRunInfo.IsPaused = true;
        var removed = RegisteredTimer.Remove(CallBackRunInfo);
        return removed && !CallBackRunInfo.IsFinished;
    }

    public override bool UpdateWaitPeriod(TimeSpan newWaitFromNowTimeSpan) =>
        CallBackRunInfo.RegisteredTimer.Change(newWaitFromNowTimeSpan, newWaitFromNowTimeSpan);

    public override bool Pause()
    {
        CallBackRunInfo.IsPaused = true;
        return CallBackRunInfo.RegisteredTimer.Change(TimeSpan.MaxValue, TimeSpan.MaxValue);
    }

    public override bool Resume()
    {
        CallBackRunInfo.IsPaused = false;
        var now = TimeContext.UtcNow;
        var launchTime = CallBackRunInfo.NextScheduleTime < now ?
            TimeSpan.Zero :
            now - CallBackRunInfo.NextScheduleTime;
        return CallBackRunInfo.RegisteredTimer.Change(launchTime, CallBackRunInfo.RepeatPeriodTimeSpan);
    }
}
