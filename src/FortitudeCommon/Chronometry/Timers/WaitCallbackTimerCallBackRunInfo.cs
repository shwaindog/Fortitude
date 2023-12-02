#region

using FortitudeCommon.Types;

#endregion

namespace FortitudeCommon.Chronometry.Timers;

internal class WaitCallbackTimerCallBackRunInfo : TimerCallBackRunInfo
{
    public WaitCallback? WaitCallback { get; set; }
    public object? State { get; set; }

    public override void CopyFrom(TimerCallBackRunInfo source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (source is WaitCallbackTimerCallBackRunInfo waitCallbackTimerCallBackRunInfo)
        {
            WaitCallback = waitCallbackTimerCallBackRunInfo.WaitCallback;
            State = waitCallbackTimerCallBackRunInfo.State;
        }

        base.CopyFrom(source, copyMergeFlags);
    }

    public override bool RunCallbackOnThreadPool()
    {
        if (!IsFinished)
        {
            Interlocked.Increment(ref CurrentInvocations);
            if (!IsFinished)
                NextScheduleTime += IntervalPeriodTimeSpan;
            else
                NextScheduleTime = DateTime.MaxValue;

            LastRunTime = TimeContext.UtcNow;
            return ThreadPool.QueueUserWorkItem(WaitCallback!, State);
        }

        return false;
    }

    public override bool RunCallbackOnThisThread()
    {
        if (!IsFinished)
        {
            Interlocked.Increment(ref CurrentInvocations);
            if (!IsFinished)
                NextScheduleTime += IntervalPeriodTimeSpan;
            else
                NextScheduleTime = DateTime.MaxValue;

            LastRunTime = TimeContext.UtcNow;
            WaitCallback!(State);
            return true;
        }

        return false;
    }
}
