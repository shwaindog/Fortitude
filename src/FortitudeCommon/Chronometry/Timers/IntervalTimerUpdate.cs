// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeCommon.Chronometry.Timers;

public class IntervalTimerUpdate : OneOffTimerUpdate
{
    public override bool Cancel()
    {
        CallBackRunInfo!.IsPaused        = true;
        CallBackRunInfo.MaxNumberOfCalls = CallBackRunInfo.CurrentNumberOfCalls;
        var removed = UpdateableTimer.Remove(CallBackRunInfo);
        return removed && CallBackRunInfo.IsFinished;
    }

    public override bool UpdateWaitPeriod(TimeSpan newWaitFromNowTimeSpan)
    {
        CallBackRunInfo!.IntervalPeriodTimeSpan = newWaitFromNowTimeSpan;
        CallBackRunInfo.NextScheduleTime        = TimeContext.UtcNow + newWaitFromNowTimeSpan;
        return CallBackRunInfo.RegisteredTimer.Change(newWaitFromNowTimeSpan, newWaitFromNowTimeSpan) &&
               !CallBackRunInfo.IsFinished;
    }

    public override bool Pause()
    {
        CallBackRunInfo!.IsPaused = true;
        return CallBackRunInfo.RegisteredTimer.Change(Timers.UpdateableTimer.MaxTimerSpan, Timers.UpdateableTimer.MaxTimerSpan) &&
               !CallBackRunInfo.IsFinished;
    }

    public override bool Resume()
    {
        CallBackRunInfo!.IsPaused = false;
        var now        = TimeContext.UtcNow;
        var launchTime = CallBackRunInfo.NextScheduleTime < now ? TimeSpan.Zero : now - CallBackRunInfo.NextScheduleTime;
        return CallBackRunInfo.RegisteredTimer.Change(launchTime, CallBackRunInfo.IntervalPeriodTimeSpan) &&
               !CallBackRunInfo.IsFinished;
    }
}
