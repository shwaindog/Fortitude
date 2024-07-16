// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Types;

#endregion

namespace FortitudeCommon.Chronometry.Timers;

internal class ValueTaskActionStateTimerCallBackRunInfo<T> : TimerCallBackRunInfo where T : class
{
    private WaitCallback?        actionAsWaitCallback;
    private Func<T?, ValueTask>? backingAction;

    public ValueTaskActionStateTimerCallBackRunInfo() { }

    private ValueTaskActionStateTimerCallBackRunInfo(ValueTaskActionStateTimerCallBackRunInfo<T> toClone)
    {
        // ReSharper disable once VirtualMemberCallInConstructor
        CopyFrom(toClone);
    }

    public Func<T?, ValueTask>? ValueTaskActionState
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
        if (source is ValueTaskActionStateTimerCallBackRunInfo<T> actionTimerCallBackRunInfo)
        {
            ValueTaskActionState = actionTimerCallBackRunInfo.ValueTaskActionState;
            State                = actionTimerCallBackRunInfo.State;
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
        Recycler?.Borrow<ValueTaskActionStateTimerCallBackRunInfo<T>>().CopyFrom(this) ??
        new ValueTaskActionStateTimerCallBackRunInfo<T>(this);
}

internal class ScheduledActualValueTaskActionTimerCallBackRunInfo : ValueTaskActionStateTimerCallBackRunInfo<IScheduleActualTime>
  , ITimerUpdateCallBackRunInfo
{
    private int callCount;

    public ITimerUpdate? TimerUpdate { get; set; }

    public override bool RunCallbackOnThreadPool()
    {
        var scheduleActualState = Recycler?.Borrow<ScheduleActualTime>() ?? new ScheduleActualTime();
        scheduleActualState.Configure(TimerUpdate!, TimerUpdate!.NextScheduleDateTime, TimeContext.UtcNow, callCount++);
        State = scheduleActualState;
        return base.RunCallbackOnThreadPool();
    }

    public override bool RunCallbackOnThisThread()
    {
        var scheduleActualState = Recycler?.Borrow<ScheduleActualTime>() ?? new ScheduleActualTime();
        scheduleActualState.Configure(TimerUpdate!, TimerUpdate!.NextScheduleDateTime, TimeContext.UtcNow, callCount++);
        State = scheduleActualState;
        return base.RunCallbackOnThisThread();
    }

    public override void StateReset()
    {
        TimerUpdate = null;
        base.StateReset();
    }
}

internal class ScheduledActualValueTaskActionStateTimerCallBackRunInfo<T> : ValueTaskActionStateTimerCallBackRunInfo<IScheduleActualTime<T>>,
    ITimerUpdateCallBackRunInfo where T : class
{
    private int callCount;

    public T? SendState { get; set; }

    public ITimerUpdate? TimerUpdate { get; set; }

    public override bool RunCallbackOnThreadPool()
    {
        var scheduleActualState = Recycler?.Borrow<ScheduleActualTime<T>>() ?? new ScheduleActualTime<T>();
        scheduleActualState.Configure(TimerUpdate!, TimerUpdate!.NextScheduleDateTime, TimeContext.UtcNow, SendState, callCount++);
        State = scheduleActualState;
        return base.RunCallbackOnThreadPool();
    }

    public override bool RunCallbackOnThisThread()
    {
        var scheduleActualState = Recycler?.Borrow<ScheduleActualTime<T>>() ?? new ScheduleActualTime<T>();
        scheduleActualState.Configure(TimerUpdate!, TimerUpdate!.NextScheduleDateTime, TimeContext.UtcNow, SendState, callCount++);
        State = scheduleActualState;
        return base.RunCallbackOnThisThread();
    }

    public override void StateReset()
    {
        TimerUpdate = null;
        base.StateReset();
    }
}
