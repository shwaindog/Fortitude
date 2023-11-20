#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;

#endregion

namespace FortitudeCommon.Chronometry.Timers;

public class OneOffTimerUpdate : ITimerUpdate
{
    protected internal TimerCallBackRunInfo CallBackRunInfo = null!;
    private int refCount = 0;

    public Timer RegisteredTimer { get; set; } = null!;

    public void CopyFrom(ITimerUpdate source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        RegisteredTimer = source.RegisteredTimer;
        CallBackRunInfo = ((OneOffTimerUpdate)source).CallBackRunInfo;
    }

    public void CopyFrom(IStoreState source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        CopyFrom((ITimerUpdate)source, copyMergeFlags);
    }

    public int RefCount => refCount;
    public bool RecycleOnRefCountZero { get; set; } = true;
    public bool AutoRecycledByProducer { get; set; }
    public bool IsInRecycler { get; set; }
    public IRecycler? Recycler { get; set; }

    public int DecrementRefCount()
    {
        if (Interlocked.Decrement(ref refCount) == 0 && RecycleOnRefCountZero) Recycle();
        return refCount;
    }

    public int IncrementRefCount() => Interlocked.Increment(ref refCount);

    public bool Recycle()
    {
        if (refCount <= 0 || !RecycleOnRefCountZero)
        {
            CallBackRunInfo.DecrementRefCount();
            Recycler!.Recycle(this);
        }

        return IsInRecycler;
    }


    public virtual bool Cancel()
    {
        CallBackRunInfo.IsPaused = true;
        var removed = RegisteredTimer.Remove(CallBackRunInfo);
        RegisteredTimer.CheckNextOneOffLaunchTimeStillCorrect(CallBackRunInfo);
        return removed && !CallBackRunInfo.IsFinished;
    }

    public bool ExecuteNowOnThreadPool()
    {
        var result = CallBackRunInfo.RunCallbackOnThreadPool();
        if (result && CallBackRunInfo.IsFinished)
        {
            RegisteredTimer.Remove(CallBackRunInfo);
            RegisteredTimer.CheckNextOneOffLaunchTimeStillCorrect(CallBackRunInfo);
        }

        return result;
    }

    public bool ExecuteNowOnThisThread()
    {
        var result = CallBackRunInfo.RunCallbackOnThisThread();
        if (result && CallBackRunInfo.IsFinished)
        {
            RegisteredTimer.Remove(CallBackRunInfo);
            RegisteredTimer.CheckNextOneOffLaunchTimeStillCorrect(CallBackRunInfo);
        }
        else
        {
            RegisteredTimer.CheckNextOneOffLaunchTimeStillCorrect(CallBackRunInfo);
        }

        return result;
    }

    public bool UpdateWaitPeriod(int newWaitFromNowMs) => UpdateWaitPeriod(TimeSpan.FromMilliseconds(newWaitFromNowMs));

    public virtual bool UpdateWaitPeriod(TimeSpan newWaitFromNowTimeSpan)
    {
        CallBackRunInfo.IsPaused = true;
        CallBackRunInfo.NextScheduleTime = TimeContext.UtcNow + newWaitFromNowTimeSpan;
        CallBackRunInfo.IsPaused = false;
        RegisteredTimer.CheckNextOneOffLaunchTimeStillCorrect(CallBackRunInfo);
        return !CallBackRunInfo.IsFinished;
    }

    public virtual bool Pause()
    {
        CallBackRunInfo.IsPaused = true;
        return !CallBackRunInfo.IsFinished;
    }

    public virtual bool Resume()
    {
        CallBackRunInfo.IsPaused = false;
        return !CallBackRunInfo.IsFinished;
    }
}
