// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Types.Mutable;

#endregion

namespace FortitudeCommon.Chronometry.Timers;

internal class ValueTaskActionTimerCallBackRunInfo : TimerCallBackRunInfo
{
    private WaitCallback?    actionAsWaitCallback;
    private Func<ValueTask>? backingAction;

    public ValueTaskActionTimerCallBackRunInfo() { }

    private ValueTaskActionTimerCallBackRunInfo(ValueTaskActionTimerCallBackRunInfo toClone)
    {
        // ReSharper disable once VirtualMemberCallInConstructor
        CopyFrom(toClone);
    }

    public Func<ValueTask>? ValueTaskAction
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

    public override ITimerCallBackRunInfo CopyFrom
        (ITimerCallBackRunInfo source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (source is ValueTaskActionTimerCallBackRunInfo valueTaskTimerCallBackRunInfo)
            ValueTaskAction = valueTaskTimerCallBackRunInfo.ValueTaskAction;

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

    public override ITimerCallBackRunInfo Clone() =>
        Recycler?.Borrow<ValueTaskActionTimerCallBackRunInfo>().CopyFrom(this) ?? new ValueTaskActionTimerCallBackRunInfo(this);
}
