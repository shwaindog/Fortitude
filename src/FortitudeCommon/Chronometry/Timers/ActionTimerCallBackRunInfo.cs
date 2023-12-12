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
                var reusuableDateTimeSource = NextExecutionComplete;
                try
                {
                    backingAction!();
                    reusuableDateTimeSource?.TrySetResult(DateTime.Now);
                }
                catch (Exception e)
                {
                    Logger.Warn("Timer Call '{0}' back caught exception {1}", this, e);
                    reusuableDateTimeSource?.SetException(e);
                }
                finally
                {
                    if (reusuableDateTimeSource != null)
                    {
                        reusuableDateTimeSource.DecrementRefCount();
                        Interlocked.CompareExchange(ref NextExecutionComplete, null, reusuableDateTimeSource);
                    }

                    DecrementRefCount();
                }
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
            if (IncrementRefCount() > 1) // 1 means it was already at Zero
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
                DecrementRefCount();
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

    public override void StateReset()
    {
        backingAction = null;
        base.StateReset();
    }

    public override TimerCallBackRunInfo Clone() =>
        Recycler?.Borrow<ActionTimerCallBackRunInfo>().CopyFrom(this) ?? new ActionTimerCallBackRunInfo(this);
}
