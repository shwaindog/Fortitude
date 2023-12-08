#region

using FortitudeCommon.Types;

#endregion

namespace FortitudeCommon.Chronometry.Timers;

internal class ActionTimerCallBackRunInfo : TimerCallBackRunInfo
{
    private WaitCallback? actionAsWaitCallback;
    private Action? backingAction;

    public ActionTimerCallBackRunInfo() { }

    private ActionTimerCallBackRunInfo(ActionTimerCallBackRunInfo toClone)
    {
        // ReSharper disable once VirtualMemberCallInConstructor
        CopyFrom(toClone);
    }

    public Action? Action
    {
        get => backingAction;
        set
        {
            backingAction = value;
            actionAsWaitCallback ??= _ =>
            {
                Interlocked.CompareExchange(ref RunState, (int)RunStateEnum.Executing
                    , (int)RunStateEnum.Queued);
                backingAction!();
                AsyncPostRunRecycleCheck();
            };
        }
    }

    public override TimerCallBackRunInfo CopyFrom(TimerCallBackRunInfo source
        , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (source is ActionTimerCallBackRunInfo actionTimerCallBackRunInfo) Action = actionTimerCallBackRunInfo.Action;

        return base.CopyFrom(source, copyMergeFlags);
    }

    public override bool RunCallbackOnThreadPool()
    {
        if (!IsFinished)
            if (Interlocked.Increment(ref QueueCount) > 0 && Interlocked.CompareExchange(ref RunState
                    , (int)RunStateEnum.Queued, (int)RunStateEnum.NotRunning) !=
                (int)RunStateEnum.RecycleRequested)
            {
                Interlocked.Increment(ref CurrentInvocations);
                if (!IsFinished)
                    NextScheduleTime += IntervalPeriodTimeSpan;
                else
                    NextScheduleTime = DateTime.MaxValue;

                LastRunTime = TimeContext.UtcNow;
                return ThreadPool.QueueUserWorkItem(actionAsWaitCallback!, null);
            }
            else
            {
                AsyncPostRunRecycleCheck();
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

    public override void Reset()
    {
        backingAction = null;
        base.Reset();
    }

    public override TimerCallBackRunInfo Clone() =>
        Recycler?.Borrow<ActionTimerCallBackRunInfo>().CopyFrom(this) ?? new ActionTimerCallBackRunInfo(this);
}
