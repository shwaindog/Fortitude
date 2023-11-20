#region

using System.Runtime.CompilerServices;
using FortitudeCommon.AsyncProcessing;
using FortitudeCommon.DataStructures.Memory;

#endregion

namespace FortitudeCommon.Chronometry.Timers;

public class Timer : ITimer
{
    private static ITimer? instance;
    private static object syncLock = new();

    private readonly TimeSpan heartBeatCheck = TimeSpan.FromSeconds(10);
    private readonly List<TimerCallBackRunInfo> intervalCallBacks = new();
    private readonly List<TimerCallBackRunInfo> oneOffCallbacks = new();
    private readonly ISyncLock oneOffSpinLock = new SpinLockLight();
    private readonly System.Threading.Timer oneOffTimer;
    private readonly IRecycler recycler;
    private DateTime nextOneOffTimerTickDateTime = DateTime.MinValue;
    private volatile bool pauseAll;

    public Timer()
    {
        recycler = new Recycler();
        oneOffTimer = new System.Threading.Timer(OneOffTimeTicker, this, Timeout.Infinite, Timeout.Infinite);
    }

    public static ITimer Instance
    {
        get
        {
            if (instance == null)
                lock (syncLock)
                {
                    if (instance == null) instance = new Timer();
                }

            return instance;
        }
        set => instance = value;
    }

    private TimerCallBackRunInfo? NextScheduledOneOffTimerCallBackRunInfo
    {
        get
        {
            oneOffSpinLock.Acquire();
            try
            {
                return oneOffCallbacks.FirstOrDefault();
            }
            finally
            {
                oneOffSpinLock.Release();
            }
        }
    }

    private DateTime NextScheduledOneOffTimerTick =>
        NextScheduledOneOffTimerCallBackRunInfo?.NextScheduleTime ?? DateTime.MaxValue;

    public ITimerUpdate RunIn(TimeSpan waitTimeSpan, WaitCallback callback)
    {
        var now = TimeContext.UtcNow;
        var runAt = now + waitTimeSpan;
        var timerCallBack = GetConfiguredOneOffTimerCallBackRunInfo(runAt, callback);
        timerCallBack.RegisteredTimer = oneOffTimer;
        RegisterCallback(timerCallBack);
        CheckNextOneOffLaunchTimeStillCorrect(timerCallBack);
        var timerUpdate = recycler.Borrow<OneOffTimerUpdate>();
        timerUpdate.CallBackRunInfo = timerCallBack;
        timerUpdate.RegisteredTimer = this;
        return timerUpdate;
    }

    public ITimerUpdate RunIn(TimeSpan waitTimeSpan, Action callback)
    {
        var now = TimeContext.UtcNow;
        var runAt = now + waitTimeSpan;
        var timerCallBack = GetConfiguredOneOffTimerCallBackRunInfo(runAt, _ => callback());
        timerCallBack.RegisteredTimer = oneOffTimer;
        RegisterCallback(timerCallBack);
        CheckNextOneOffLaunchTimeStillCorrect(timerCallBack);
        var timerUpdate = recycler.Borrow<OneOffTimerUpdate>();
        timerUpdate.CallBackRunInfo = timerCallBack;
        timerUpdate.RegisteredTimer = this;
        return timerUpdate;
    }

    public ITimerUpdate RunIn<T>(TimeSpan waitTimeSpan, T state, Action<T> callback) where T : class
    {
        var now = TimeContext.UtcNow;
        var runAt = now + waitTimeSpan;
        var timerCallBack
            = GetConfiguredOneOffTimerCallBackRunInfo(runAt, stateParam => callback((T)stateParam!), state);
        timerCallBack.RegisteredTimer = oneOffTimer;
        RegisterCallback(timerCallBack);
        CheckNextOneOffLaunchTimeStillCorrect(timerCallBack);
        var timerUpdate = recycler.Borrow<OneOffTimerUpdate>();
        timerUpdate.CallBackRunInfo = timerCallBack;
        timerUpdate.RegisteredTimer = this;
        return timerUpdate;
    }

    public ITimerUpdate RunIn(int waitMs, WaitCallback callback) => RunIn(TimeSpan.FromMilliseconds(waitMs), callback);

    public ITimerUpdate RunIn(int waitMs, Action callback) => RunIn(TimeSpan.FromMilliseconds(waitMs), callback);

    public ITimerUpdate RunIn<T>(int waitMs, T state, Action<T> callback) where T : class =>
        RunIn(TimeSpan.FromMilliseconds(waitMs), state, callback);

    public ITimerUpdate RunEvery(int intervalMs, WaitCallback callback) =>
        RunEvery(TimeSpan.FromMilliseconds(intervalMs), callback);

    public ITimerUpdate RunEvery(int intervalMs, Action callback) =>
        RunEvery(TimeSpan.FromMilliseconds(intervalMs), callback);

    public ITimerUpdate RunEvery<T>(int intervalMs, T state, Action<T> callback) where T : class =>
        RunEvery(TimeSpan.FromMilliseconds(intervalMs), state, callback);

    public ITimerUpdate RunEvery(TimeSpan periodTimeSpan, WaitCallback callback)
    {
        var timerCallBack = GetConfiguredRepeatedTimerCallBackRunInfo(periodTimeSpan, callback);
        var repeatedTimer
            = new System.Threading.Timer(RepeatedTimeTicker, timerCallBack, periodTimeSpan, periodTimeSpan);
        timerCallBack.RegisteredTimer = repeatedTimer;
        intervalCallBacks.Add(timerCallBack);
        var timerUpdate = recycler.Borrow<IntervalTimerUpdate>();
        timerUpdate.CallBackRunInfo = timerCallBack;
        timerUpdate.RegisteredTimer = this;
        return timerUpdate;
    }

    public ITimerUpdate RunEvery(TimeSpan periodTimeSpan, Action callback)
    {
        var timerCallBack = GetConfiguredRepeatedTimerCallBackRunInfo(periodTimeSpan, _ => callback());
        var repeatedTimer
            = new System.Threading.Timer(RepeatedTimeTicker, timerCallBack, periodTimeSpan, periodTimeSpan);
        timerCallBack.RegisteredTimer = repeatedTimer;
        intervalCallBacks.Add(timerCallBack);
        var timerUpdate = recycler.Borrow<IntervalTimerUpdate>();
        timerUpdate.CallBackRunInfo = timerCallBack;
        timerUpdate.RegisteredTimer = this;
        return timerUpdate;
    }

    public ITimerUpdate RunEvery<T>(TimeSpan periodTimeSpan, T state, Action<T> callback) where T : class
    {
        var timerCallBack
            = GetConfiguredRepeatedTimerCallBackRunInfo(periodTimeSpan, stateParam => callback((T)stateParam!)
                , state);
        var repeatedTimer
            = new System.Threading.Timer(RepeatedTimeTicker, timerCallBack, periodTimeSpan, periodTimeSpan);
        timerCallBack.RegisteredTimer = repeatedTimer;
        intervalCallBacks.Add(timerCallBack);
        var timerUpdate = recycler.Borrow<IntervalTimerUpdate>();
        timerUpdate.CallBackRunInfo = timerCallBack;
        timerUpdate.RegisteredTimer = this;
        return timerUpdate;
    }

    public ITimerUpdate RunAt(DateTime futureDateTime, WaitCallback callback)
    {
        var timerCallBack = GetConfiguredOneOffTimerCallBackRunInfo(futureDateTime, callback);
        timerCallBack.RegisteredTimer = oneOffTimer;
        RegisterCallback(timerCallBack);
        CheckNextOneOffLaunchTimeStillCorrect(timerCallBack);
        var timerUpdate = recycler.Borrow<OneOffTimerUpdate>();
        timerUpdate.CallBackRunInfo = timerCallBack;
        timerUpdate.RegisteredTimer = this;
        return timerUpdate;
    }

    public ITimerUpdate RunAt(DateTime futureDateTime, Action callback)
    {
        var timerCallBack = GetConfiguredOneOffTimerCallBackRunInfo(futureDateTime, _ => callback());
        timerCallBack.RegisteredTimer = oneOffTimer;
        RegisterCallback(timerCallBack);
        CheckNextOneOffLaunchTimeStillCorrect(timerCallBack);
        var timerUpdate = recycler.Borrow<OneOffTimerUpdate>();
        timerUpdate.CallBackRunInfo = timerCallBack;
        timerUpdate.RegisteredTimer = this;
        return timerUpdate;
    }

    public ITimerUpdate RunAt<T>(DateTime futureDateTime, T state, Action<T> callback) where T : class
    {
        var timerCallBack
            = GetConfiguredOneOffTimerCallBackRunInfo(futureDateTime, stateParam => callback((T)stateParam!)
                , state);
        timerCallBack.RegisteredTimer = oneOffTimer;
        RegisterCallback(timerCallBack);
        CheckNextOneOffLaunchTimeStillCorrect(timerCallBack);
        var timerUpdate = recycler.Borrow<OneOffTimerUpdate>();
        timerUpdate.CallBackRunInfo = timerCallBack;
        timerUpdate.RegisteredTimer = this;
        return timerUpdate;
    }

    public void PauseAllTimers()
    {
        pauseAll = true;
        UpdateNextOneOffTimerTick(false);
        foreach (var intvTimerKvp in intervalCallBacks)
            intvTimerKvp.RegisteredTimer.Change(TimeSpan.MaxValue, TimeSpan.MaxValue);
    }

    public void ResumeAllTimers()
    {
        pauseAll = false;
        var now = TimeContext.UtcNow;
        UpdateNextOneOffTimerTick(true);
        foreach (var intvTimerKvp in intervalCallBacks)
        {
            var nextTimerTickTime = intvTimerKvp.NextScheduleTime;
            var nextTickTimeSpan = nextTimerTickTime < now ? TimeSpan.Zero : nextTimerTickTime - now;
            intvTimerKvp.RegisteredTimer.Change(nextTickTimeSpan, intvTimerKvp.RepeatPeriodTimeSpan);
        }
    }

    public TimerCallBackRunInfo? RunNextScheduledOneOffCallbackNowOnThreadPool()
    {
        var nextScheduledOneOff = NextScheduledOneOffTimerCallBackRunInfo;
        if (nextScheduledOneOff != null)
        {
            nextScheduledOneOff.RunCallbackOnThreadPool();
            CheckNextOneOffLaunchTimeStillCorrect(nextScheduledOneOff);
        }

        return nextScheduledOneOff;
    }

    public TimerCallBackRunInfo? RunNextScheduledOneOffCallbackNowOnThisThread()
    {
        var nextScheduledOneOff = NextScheduledOneOffTimerCallBackRunInfo;
        if (nextScheduledOneOff != null)
        {
            nextScheduledOneOff.RunCallbackOnThisThread();
            CheckNextOneOffLaunchTimeStillCorrect(nextScheduledOneOff);
        }

        return nextScheduledOneOff;
    }

    public TimerCallBackRunInfo? RunNextScheduledIntervalCallbackNowOnThreadPool()
    {
        if (intervalCallBacks.Any())
        {
            var nextRepeatedTimerLowestDateTime = intervalCallBacks.Min(tcbri => tcbri.NextScheduleTime);
            var nextRepeatedTimer
                = intervalCallBacks.FirstOrDefault(tcbri => tcbri.NextScheduleTime == nextRepeatedTimerLowestDateTime);
            nextRepeatedTimer?.RunCallbackOnThreadPool();
            return nextRepeatedTimer;
        }

        return null;
    }

    public TimerCallBackRunInfo? RunNextScheduledIntervalCallbackNowThisThread()
    {
        if (intervalCallBacks.Any())
        {
            var nextRepeatedTimerLowestDateTime = intervalCallBacks.Min(tcbri => tcbri.NextScheduleTime);
            var nextRepeatedTimer
                = intervalCallBacks.FirstOrDefault(tcbri => tcbri.NextScheduleTime == nextRepeatedTimerLowestDateTime);
            nextRepeatedTimer?.RunCallbackOnThisThread();
            return nextRepeatedTimer;
        }

        return null;
    }

    private void UpdateNextOneOffTimerTick(bool enable)
    {
        var heatBeatCheck = !enable ? TimeSpan.MaxValue : heartBeatCheck;
        nextOneOffTimerTickDateTime = !enable ? DateTime.MaxValue : NextScheduledOneOffTimerTick;
        var now = TimeContext.UtcNow;
        var nextTick = nextOneOffTimerTickDateTime > now ? nextOneOffTimerTickDateTime - now : TimeSpan.Zero;
        oneOffTimer.Change(nextTick, heatBeatCheck);
    }

    private void RegisterCallback(TimerCallBackRunInfo timerCallBack)
    {
        try
        {
            oneOffSpinLock.Acquire();
            oneOffCallbacks.Add(timerCallBack);
        }
        finally
        {
            oneOffSpinLock.Release();
        }
    }

    private TimerCallBackRunInfo GetConfiguredRepeatedTimerCallBackRunInfo(TimeSpan repeatPeriod, WaitCallback callback
        , object? state = null)
    {
        var callBackInfo = recycler.Borrow<TimerCallBackRunInfo>();
        callBackInfo.Callback = callback;
        callBackInfo.CurrentNumberOfCalls = 0;
        var firstScheduledTime = TimeContext.UtcNow + repeatPeriod;
        callBackInfo.FirstScheduledTime = firstScheduledTime;
        callBackInfo.IsPaused = pauseAll;
        callBackInfo.LastRunTime = DateTime.MinValue;
        callBackInfo.MaxNumberOfCalls = int.MaxValue;
        callBackInfo.NextScheduleTime = firstScheduledTime;
        callBackInfo.RepeatPeriodTimeSpan = repeatPeriod;
        callBackInfo.State = state;
        return callBackInfo;
    }

    private TimerCallBackRunInfo GetConfiguredOneOffTimerCallBackRunInfo(DateTime runAt, WaitCallback callback
        , object? state = null)
    {
        var callBackInfo = recycler.Borrow<TimerCallBackRunInfo>();
        callBackInfo.Callback = callback;
        callBackInfo.CurrentNumberOfCalls = 0;
        callBackInfo.FirstScheduledTime = runAt;
        callBackInfo.IsPaused = pauseAll;
        callBackInfo.LastRunTime = DateTime.MinValue;
        callBackInfo.MaxNumberOfCalls = 1;
        callBackInfo.NextScheduleTime = runAt;
        callBackInfo.RepeatPeriodTimeSpan = TimeSpan.MaxValue;
        callBackInfo.State = state;
        return callBackInfo;
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    private void OneOffTimeTicker(object? stateInfo)
    {
        var now = TimeContext.UtcNow;
        try
        {
            oneOffSpinLock.Acquire();
            for (var i = 0; i < oneOffCallbacks.Count; i++)
            {
                var currentCallback = oneOffCallbacks[i]!;
                if (!currentCallback.IsPaused && currentCallback.NextScheduleTime < now)
                {
                    currentCallback.RunCallbackOnThreadPool();
                    oneOffCallbacks.RemoveAt(i--);
                    currentCallback.DecrementRefCount();
                }
            }
        }
        finally
        {
            oneOffSpinLock.Release();
        }

        if (oneOffCallbacks.Any()) CheckNextOneOffLaunchTimeStillCorrect(NextScheduledOneOffTimerCallBackRunInfo!);
    }

    private void RepeatedTimeTicker(object? stateInfo)
    {
        if (stateInfo is TimerCallBackRunInfo { IsPaused: false } repeatedTimer)
            repeatedTimer.RunCallbackOnThisThread();
    }

    internal bool Remove(TimerCallBackRunInfo callBackRunInfo)
    {
        foreach (var timerCallBackRunInfo in oneOffCallbacks)
            if (timerCallBackRunInfo == callBackRunInfo)
            {
                oneOffCallbacks.Remove(timerCallBackRunInfo);
                timerCallBackRunInfo.DecrementRefCount();
                CheckNextOneOffLaunchTimeStillCorrect(timerCallBackRunInfo);
                return true;
            }

        foreach (var timerCallBackRunInfo in intervalCallBacks)
            if (timerCallBackRunInfo == callBackRunInfo)
            {
                timerCallBackRunInfo.RegisteredTimer.Dispose();
                intervalCallBacks.Remove(timerCallBackRunInfo);
                timerCallBackRunInfo.DecrementRefCount();
                return true;
            }

        return false;
    }

    public void CheckNextOneOffLaunchTimeStillCorrect(TimerCallBackRunInfo changed)
    {
        if (pauseAll) return;
        var indxBeforeSort = oneOffCallbacks.IndexOf(changed);
        SortOneOfTimerTicks();

        if (indxBeforeSort == 0 || nextOneOffTimerTickDateTime >= changed.NextScheduleTime)
            UpdateNextOneOffTimerTick(true);
    }

    private void SortOneOfTimerTicks()
    {
        try
        {
            oneOffSpinLock.Acquire();
            oneOffCallbacks.Sort();
        }
        finally
        {
            oneOffSpinLock.Release();
        }
    }
}
