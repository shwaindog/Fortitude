#region

using FortitudeCommon.Types;

#endregion

namespace FortitudeCommon.Chronometry.Timers;

internal class ActionTimerCallBackRunInfo : TimerCallBackRunInfo
{
    private WaitCallback? actionAsWaitCallback;
    private Action? backingAction;

    public Action? Action
    {
        get => backingAction;
        set
        {
            backingAction = value;
            actionAsWaitCallback ??= _ => backingAction!();
        }
    }

    public override void CopyFrom(TimerCallBackRunInfo source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (source is ActionTimerCallBackRunInfo actionTimerCallBackRunInfo) Action = actionTimerCallBackRunInfo.Action;

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
            return ThreadPool.QueueUserWorkItem(actionAsWaitCallback!, null);
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
            backingAction!();
            return true;
        }

        return false;
    }
}
