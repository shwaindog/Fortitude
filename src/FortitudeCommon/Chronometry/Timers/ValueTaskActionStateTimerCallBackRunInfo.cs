#region

using FortitudeCommon.Types;

#endregion

namespace FortitudeCommon.Chronometry.Timers;

internal class ValueTaskActionStateTimerCallBackRunInfo<T> : TimerCallBackRunInfo where T : class
{
    private WaitCallback? actionAsWaitCallback;
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

    public override TimerCallBackRunInfo CopyFrom(TimerCallBackRunInfo source
        , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (source is ValueTaskActionStateTimerCallBackRunInfo<T> actionTimerCallBackRunInfo)
        {
            ValueTaskActionState = actionTimerCallBackRunInfo.ValueTaskActionState;
            State = actionTimerCallBackRunInfo.State;
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

    public override TimerCallBackRunInfo Clone() =>
        Recycler?.Borrow<ValueTaskActionStateTimerCallBackRunInfo<T>>().CopyFrom(this) ??
        new ValueTaskActionStateTimerCallBackRunInfo<T>(this);
}
