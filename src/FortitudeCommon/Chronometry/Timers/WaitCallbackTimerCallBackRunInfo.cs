﻿#region

using FortitudeCommon.Types;

#endregion

namespace FortitudeCommon.Chronometry.Timers;

internal class WaitCallbackTimerCallBackRunInfo : TimerCallBackRunInfo
{
    private WaitCallback? cleanUpWaitCallback;
    private WaitCallback? waitCallback;
    public WaitCallbackTimerCallBackRunInfo() { }

    private WaitCallbackTimerCallBackRunInfo(WaitCallbackTimerCallBackRunInfo toCLone)
    {
        // ReSharper disable once VirtualMemberCallInConstructor
        CopyFrom(toCLone);
    }

    public WaitCallback? WaitCallback
    {
        get => waitCallback;
        set
        {
            waitCallback = value;
            cleanUpWaitCallback ??= objState =>
            {
                var reusuableDateTimeSource = NextExecutionComplete;
                try
                {
                    waitCallback!(objState);
                    reusuableDateTimeSource?.TrySetResult(DateTime.Now);
                }
                catch (Exception e)
                {
                    Logger.Warn("Timer Call '{0}' with state {1} back caught exception {2}", ToString(), objState, e);
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

    public object? State { get; set; }

    public override TimerCallBackRunInfo CopyFrom(TimerCallBackRunInfo source
        , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (source is WaitCallbackTimerCallBackRunInfo waitCallbackTimerCallBackRunInfo)
        {
            WaitCallback = waitCallbackTimerCallBackRunInfo.WaitCallback;
            State = waitCallbackTimerCallBackRunInfo.State;
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
                return ThreadPool.QueueUserWorkItem(cleanUpWaitCallback!, State);
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
            WaitCallback!(State);
            return true;
        }

        return false;
    }

    public override void StateReset()
    {
        WaitCallback = null;
        State = null;
        base.StateReset();
    }

    public override TimerCallBackRunInfo Clone() =>
        Recycler?.Borrow<WaitCallbackTimerCallBackRunInfo>().CopyFrom(this) ??
        new WaitCallbackTimerCallBackRunInfo(this);
}
