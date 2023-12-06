#region

using FortitudeCommon.Types;

#endregion

namespace FortitudeCommon.Chronometry.Timers;

internal class ActionStateTimerCallBackRunInfo<T> : TimerCallBackRunInfo where T : class
{
    private WaitCallback? actionAsWaitCallback;
    private Action<T?>? backingAction;

    public ActionStateTimerCallBackRunInfo() { }

    private ActionStateTimerCallBackRunInfo(ActionStateTimerCallBackRunInfo<T> toClone)
    {
        // ReSharper disable once VirtualMemberCallInConstructor
        CopyFrom(toClone);
    }

    public Action<T?>? Action
    {
        get => backingAction;
        set
        {
            backingAction = value;
            actionAsWaitCallback ??= _ => backingAction!(State);
        }
    }

    public T? State { get; set; }

    public override TimerCallBackRunInfo CopyFrom(TimerCallBackRunInfo source
        , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (source is ActionStateTimerCallBackRunInfo<T> actionTimerCallBackRunInfo)
        {
            Action = actionTimerCallBackRunInfo.Action;
            State = actionTimerCallBackRunInfo.State;
        }

        return base.CopyFrom(source, copyMergeFlags);
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
            return ThreadPool.QueueUserWorkItem(actionAsWaitCallback!, State);
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
            backingAction!(State);
            return true;
        }

        return false;
    }

    public override TimerCallBackRunInfo Clone() =>
        Recycler?.Borrow<ActionStateTimerCallBackRunInfo<T>>().CopyFrom(this) ??
        new ActionStateTimerCallBackRunInfo<T>(this);
}
