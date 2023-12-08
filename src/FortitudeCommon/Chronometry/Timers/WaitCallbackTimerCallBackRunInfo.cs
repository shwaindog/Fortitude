#region

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
            cleanUpWaitCallback ??= state =>
            {
                Interlocked.CompareExchange(ref RunState, (int)RunStateEnum.Executing
                    , (int)RunStateEnum.Queued);
                waitCallback!(state);
                AsyncPostRunRecycleCheck();
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
                return ThreadPool.QueueUserWorkItem(cleanUpWaitCallback!, State);
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
            WaitCallback!(State);
            return true;
        }

        return false;
    }

    public override void Reset()
    {
        WaitCallback = null;
        State = null;
        base.Reset();
    }

    public override TimerCallBackRunInfo Clone() =>
        Recycler?.Borrow<WaitCallbackTimerCallBackRunInfo>().CopyFrom(this) ??
        new WaitCallbackTimerCallBackRunInfo(this);
}
