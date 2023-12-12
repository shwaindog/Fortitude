#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;

#endregion

namespace FortitudeCommon.Chronometry.Timers;

public class OneOffTimerUpdate : ReusableObject<ITimerUpdate>, IThreadPoolTimerUpdate
{
    private TimerCallBackRunInfo? callBackRunInfo;

    private IUpdateableTimer? updateableTimer;

    public OneOffTimerUpdate() { }

    private OneOffTimerUpdate(OneOffTimerUpdate toClone)
    {
        // ReSharper disable once VirtualMemberCallInConstructor
        CopyFrom(toClone);
    }

    protected internal TimerCallBackRunInfo? CallBackRunInfo
    {
        get => callBackRunInfo;
        set
        {
            if (value == callBackRunInfo) return;
            if (value != null) value.IncrementRefCount();
            callBackRunInfo = value;
        }
    }

    internal IUpdateableTimer UpdateableTimer
    {
        get => updateableTimer!;
        set => updateableTimer = value;
    }

    public ITimer RegisteredTimer => updateableTimer!;

    public bool IsFinished => CallBackRunInfo!.IsFinished;
    public bool IsPaused => CallBackRunInfo!.IsPaused;

    public DateTime NextScheduleDateTime => CallBackRunInfo!.NextScheduleTime;


    public override int DecrementRefCount()
    {
        CallBackRunInfo?.DecrementRefCount();
        return base.DecrementRefCount();
    }

    public override int IncrementRefCount()
    {
        CallBackRunInfo?.IncrementRefCount();
        return base.IncrementRefCount();
    }

    public virtual bool Cancel()
    {
        CallBackRunInfo!.IsPaused = true;
        CallBackRunInfo.MaxNumberOfCalls = CallBackRunInfo.CurrentNumberOfCalls;
        var removed = UpdateableTimer.Remove(CallBackRunInfo);
        UpdateableTimer.CheckNextOneOffLaunchTimeStillCorrect(CallBackRunInfo);
        return removed && CallBackRunInfo.IsFinished;
    }

    public bool ExecuteNowOnThreadPool()
    {
        var result = CallBackRunInfo!.RunCallbackOnThreadPool();
        if (result && CallBackRunInfo.IsFinished)
        {
            UpdateableTimer.Remove(CallBackRunInfo);
            UpdateableTimer.CheckNextOneOffLaunchTimeStillCorrect(CallBackRunInfo);
        }

        return result;
    }

    public bool ExecuteNowOnThisThread()
    {
        var result = CallBackRunInfo!.RunCallbackOnThisThread();
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
        var originalPause = CallBackRunInfo!.IsPaused;
        CallBackRunInfo.IsPaused = true;
        CallBackRunInfo.NextScheduleTime = TimeContext.UtcNow + newWaitFromNowTimeSpan;
        CallBackRunInfo.IsPaused = originalPause;
        UpdateableTimer.CheckNextOneOffLaunchTimeStillCorrect(CallBackRunInfo);
        return !CallBackRunInfo.IsFinished;
    }

    public virtual bool Pause()
    {
        CallBackRunInfo!.IsPaused = true;
        return !CallBackRunInfo.IsFinished;
    }

    public virtual bool Resume()
    {
        CallBackRunInfo!.IsPaused = false;
        UpdateableTimer.CheckNextOneOffLaunchTimeStillCorrect(CallBackRunInfo);
        return !CallBackRunInfo.IsFinished;
    }

    public override ITimerUpdate CopyFrom(ITimerUpdate source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (ReferenceEquals(this, source)) return this;
        updateableTimer = (IUpdateableTimer)source.RegisteredTimer;
        CallBackRunInfo = ((OneOffTimerUpdate)source).CallBackRunInfo;
        return this;
    }

    public override void StateReset()
    {
        CallBackRunInfo?.DecrementRefCount();
        CallBackRunInfo = null;
        updateableTimer = null;
        base.StateReset();
    }

    public override ITimerUpdate Clone() =>
        Recycler?.Borrow<OneOffTimerUpdate>().CopyFrom(this) ?? new OneOffTimerUpdate(this);
}
