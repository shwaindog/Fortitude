#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;

#endregion

namespace FortitudeCommon.Chronometry.Timers;

public class OneOffTimerUpdate : IThreadPoolTimerUpdate
{
    protected internal TimerCallBackRunInfo CallBackRunInfo = null!;
    private int refCount;
    private IUpdateableTimer? updateableTimer;

    internal IUpdateableTimer UpdateableTimer
    {
        get => updateableTimer!;
        set => updateableTimer = value;
    }

    public ITimer RegisteredTimer => updateableTimer!;

    public bool IsFinished => CallBackRunInfo.IsFinished;
    public bool IsPaused => CallBackRunInfo.IsPaused;

    public DateTime NextScheduleDateTime => CallBackRunInfo.NextScheduleTime;

    public void CopyFrom(ITimerUpdate source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        updateableTimer = (IUpdateableTimer)source.RegisteredTimer;
        CallBackRunInfo = ((OneOffTimerUpdate)source).CallBackRunInfo;
    }

    public void CopyFrom(IStoreState source, CopyMergeFlags copyMergeFlags)
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
        CallBackRunInfo.MaxNumberOfCalls = CallBackRunInfo.CurrentNumberOfCalls;
        var removed = UpdateableTimer.Remove(CallBackRunInfo);
        UpdateableTimer.CheckNextOneOffLaunchTimeStillCorrect(CallBackRunInfo);
        return removed && CallBackRunInfo.IsFinished;
    }

    public bool ExecuteNowOnThreadPool()
    {
        var result = CallBackRunInfo.RunCallbackOnThreadPool();
        if (result && CallBackRunInfo.IsFinished)
        {
            UpdateableTimer.Remove(CallBackRunInfo);
            UpdateableTimer.CheckNextOneOffLaunchTimeStillCorrect(CallBackRunInfo);
        }

        return result;
    }

    public bool ExecuteNowOnThisThread()
    {
        var result = CallBackRunInfo.RunCallbackOnThisThread();
        if (result && CallBackRunInfo.IsFinished)
        {
            UpdateableTimer.Remove(CallBackRunInfo);
            UpdateableTimer.CheckNextOneOffLaunchTimeStillCorrect(CallBackRunInfo);
        }
        else
        {
            UpdateableTimer.CheckNextOneOffLaunchTimeStillCorrect(CallBackRunInfo);
        }

        return result;
    }

    public bool UpdateWaitPeriod(int newWaitFromNowMs) => UpdateWaitPeriod(TimeSpan.FromMilliseconds(newWaitFromNowMs));

    public virtual bool UpdateWaitPeriod(TimeSpan newWaitFromNowTimeSpan)
    {
        var originalPause = CallBackRunInfo.IsPaused;
        CallBackRunInfo.IsPaused = true;
        CallBackRunInfo.NextScheduleTime = TimeContext.UtcNow + newWaitFromNowTimeSpan;
        CallBackRunInfo.IsPaused = originalPause;
        UpdateableTimer.CheckNextOneOffLaunchTimeStillCorrect(CallBackRunInfo);
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
        UpdateableTimer.CheckNextOneOffLaunchTimeStillCorrect(CallBackRunInfo);
        return !CallBackRunInfo.IsFinished;
    }
}
