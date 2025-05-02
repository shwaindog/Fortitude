// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Types.Mutable;

#endregion

namespace FortitudeCommon.Chronometry.Timers;

internal class ActionStateTimerCallBackRunInfo<T> : TimerCallBackRunInfo where T : class
{
    private WaitCallback? actionAsWaitCallback;
    private Action<T?>?   backingAction;

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
            actionAsWaitCallback ??= objState =>
            {
                var reusuableDateTimeSource = NextExecutionComplete;
                try
                {
                    backingAction!((T?)objState);
                    reusuableDateTimeSource?.TrySetResult(DateTime.Now);
                }
                catch (Exception e)
                {
                    Logger.Warn("Timer Call '{0}' with state {1} back caught exception {2}", this, objState, e);
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

    public T? State { get; set; }

    public override ITimerCallBackRunInfo CopyFrom
    (ITimerCallBackRunInfo source
      , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (source is ActionStateTimerCallBackRunInfo<T> actionTimerCallBackRunInfo)
        {
            Action = actionTimerCallBackRunInfo.Action;
            State  = actionTimerCallBackRunInfo.State;
        }

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
                return ThreadPool.QueueUserWorkItem(actionAsWaitCallback!, State);
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
            backingAction!(State);
            return true;
        }

        return false;
    }

    public override ITimerCallBackRunInfo Clone() =>
        Recycler?.Borrow<ActionStateTimerCallBackRunInfo<T>>().CopyFrom(this) ??
        new ActionStateTimerCallBackRunInfo<T>(this);
}

internal class ScheduledActualActionStateTimerCallBackRunInfo : ActionStateTimerCallBackRunInfo<IScheduleActualTime>, ITimerUpdateCallBackRunInfo
{
    private int callCount;

    public ITimerUpdate? TimerUpdate { get; set; }

    public override bool RunCallbackOnThreadPool()
    {
        CaptureTriggerAndScheduleTime();
        return base.RunCallbackOnThreadPool();
    }

    public override bool RunCallbackOnThisThread()
    {
        CaptureTriggerAndScheduleTime();
        return base.RunCallbackOnThisThread();
    }

    public void CaptureTriggerAndScheduleTime()
    {
        var scheduleActualState = Recycler?.Borrow<ScheduleActualTime>() ?? new ScheduleActualTime();
        scheduleActualState.Configure(TimerUpdate!, TimerUpdate!.NextScheduleDateTime, TimeContext.UtcNow, callCount++);
        State = scheduleActualState;
    }

    public override void StateReset()
    {
        TimerUpdate = null;
        base.StateReset();
    }
}

internal class ScheduledActualActionStateTimerCallBackRunInfo<T> : ActionStateTimerCallBackRunInfo<IScheduleActualTime<T>>
  , ITimerUpdateCallBackRunInfo where T : class
{
    private int callCount;

    public T? SendState { get; set; }

    public ITimerUpdate? TimerUpdate { get; set; }

    public override bool RunCallbackOnThreadPool()
    {
        CaptureTriggerAndScheduleTime();
        return base.RunCallbackOnThreadPool();
    }

    public override bool RunCallbackOnThisThread()
    {
        CaptureTriggerAndScheduleTime();
        return base.RunCallbackOnThisThread();
    }

    public void CaptureTriggerAndScheduleTime()
    {
        var scheduleActualState = Recycler?.Borrow<ScheduleActualTime<T>>() ?? new ScheduleActualTime<T>();
        scheduleActualState.Configure(TimerUpdate!, TimerUpdate!.NextScheduleDateTime, TimeContext.UtcNow, SendState, callCount++);
        State = scheduleActualState;
    }

    public override void StateReset()
    {
        TimerUpdate = null;
        base.StateReset();
    }
}
