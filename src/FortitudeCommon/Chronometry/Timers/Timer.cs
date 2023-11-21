#region

using System.ComponentModel;
using System.Runtime.CompilerServices;
using FortitudeCommon.AsyncProcessing;
using FortitudeCommon.DataStructures.Memory;

#endregion

namespace FortitudeCommon.Chronometry.Timers;

public class Timer : IUpdateableTimer
{
    public const uint MaxTimerMs = uint.MaxValue - 1;
    private static ITimer? instance;
    private static readonly object SyncLock = new();
    private static readonly TimeSpan MaxTimerSpan = TimeSpan.FromMilliseconds(MaxTimerMs);
    private readonly TimeSpan heartBeatCheck = TimeSpan.FromSeconds(10);
    private readonly List<TimerCallBackRunInfo> intervalCallBacks = new();
    private readonly List<TimerCallBackRunInfo> oneOffCallbacks = new();
    private readonly ISyncLock oneOffSpinLock = new SpinLockLight();
    private readonly System.Threading.Timer oneOffTimer;
    private readonly IRecycler recycler;
    private bool isDead;
    private DateTime nextOneOffTimerTickDateTime = DateTime.MinValue;
    private volatile bool pauseAll;

    public Timer()
    {
        recycler = new Recycler();
        oneOffTimer = new System.Threading.Timer(OneOffTimerTicker, this, Timeout.Infinite, Timeout.Infinite);
    }

    public static ITimer Instance
    {
        get
        {
            if (instance == null)
                lock (SyncLock)
                {
                    instance ??= new Timer();
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
                return oneOffCallbacks.FirstOrDefault(tcbri => !tcbri.IsPaused);
            }
            finally
            {
                oneOffSpinLock.Release();
            }
        }
    }

    private DateTime NextScheduledOneOffTimerTick =>
        NextScheduledOneOffTimerCallBackRunInfo?.NextScheduleTime ?? DateTime.MaxValue;

    private TimerCallBackRunInfo? FirstOneOffTimerCallBackRunInfo
    {
        get
        {
            TimerCallBackRunInfo? nextScheduledOneOff;
            try
            {
                oneOffSpinLock.Acquire();
                nextScheduledOneOff = oneOffCallbacks.FirstOrDefault();
            }
            finally
            {
                oneOffSpinLock.Release();
            }

            return nextScheduledOneOff;
        }
    }

    private TimerCallBackRunInfo? NextScheduledIntervalTimerCallBackRunInfo
    {
        get
        {
            var nextIntervalTimerLowestDateTime = intervalCallBacks.Min(tcbri => tcbri.NextScheduleTime);
            var nextIntervalTimer
                = intervalCallBacks.FirstOrDefault(tcbri => tcbri.NextScheduleTime == nextIntervalTimerLowestDateTime);
            return nextIntervalTimer;
        }
    }

    public ITimerUpdate RunIn(TimeSpan waitTimeSpan, WaitCallback callback) => RunIn(waitTimeSpan, null, callback);

    public ITimerUpdate RunIn(TimeSpan waitTimeSpan, Action callback) => RunIn(waitTimeSpan, null, _ => callback());

    public ITimerUpdate RunIn<T>(TimeSpan waitTimeSpan, T state, Action<T> callback) where T : class =>
        RunIn(waitTimeSpan, state, (WaitCallback)(stateParam => callback((T)stateParam!)));

    public ITimerUpdate RunIn(TimeSpan waitTimeSpan, object? state, WaitCallback callback)
    {
        if (isDead) throw new InvalidAsynchronousStateException("Timer was stopped and should be used again!");
        var now = TimeContext.UtcNow;
        var runAt = now + waitTimeSpan;
        var timerCallBack
            = GetConfiguredOneOffTimerCallBackRunInfo(runAt, callback, state);
        timerCallBack.RegisteredTimer = oneOffTimer;
        RegisterCallback(timerCallBack);
        CheckNextOneOffLaunchTimeStillCorrect(timerCallBack);
        var timerUpdate = recycler.Borrow<OneOffTimerUpdate>();
        timerCallBack.IncrementRefCount();
        timerUpdate.CallBackRunInfo = timerCallBack;
        timerUpdate.UpdateableTimer = this;
        return timerUpdate;
    }

    public ITimerUpdate RunIn(int waitMs, WaitCallback callback) => RunIn(TimeSpan.FromMilliseconds(waitMs), callback);

    public ITimerUpdate RunIn(int waitMs, Action callback) => RunIn(TimeSpan.FromMilliseconds(waitMs), callback);

    public ITimerUpdate RunIn<T>(int waitMs, T state, Action<T> callback) where T : class =>
        RunIn(TimeSpan.FromMilliseconds(waitMs), state, callback);

    public ITimerUpdate RunIn(int waitMs, object? state, WaitCallback callback) =>
        RunIn(TimeSpan.FromMilliseconds(waitMs), state, callback);

    public ITimerUpdate RunEvery(int intervalMs, WaitCallback callback) =>
        RunEvery(TimeSpan.FromMilliseconds(intervalMs), callback);

    public ITimerUpdate RunEvery(int intervalMs, Action callback) =>
        RunEvery(TimeSpan.FromMilliseconds(intervalMs), callback);

    public ITimerUpdate RunEvery<T>(int intervalMs, T state, Action<T> callback) where T : class =>
        RunEvery(TimeSpan.FromMilliseconds(intervalMs), state, callback);

    public ITimerUpdate RunEvery(int intervalMs, object? state, WaitCallback callback) =>
        RunEvery(TimeSpan.FromMilliseconds(intervalMs), state, callback);

    public ITimerUpdate RunEvery(TimeSpan periodTimeSpan, WaitCallback callback) =>
        RunEvery(periodTimeSpan, null, callback);

    public ITimerUpdate RunEvery(TimeSpan periodTimeSpan, Action callback) =>
        RunEvery(periodTimeSpan, null, _ => callback());

    public ITimerUpdate RunEvery<T>(TimeSpan periodTimeSpan, T state, Action<T> callback) where T : class =>
        RunEvery(periodTimeSpan, state, (WaitCallback)(stateParam => callback((T)stateParam!)));

    public ITimerUpdate RunEvery(TimeSpan periodTimeSpan, object? state, WaitCallback callback)
    {
        if (isDead) throw new InvalidAsynchronousStateException("Timer was stopped and should be used again!");
        var timerCallBack
            = GetConfiguredIntervalTimerCallBackRunInfo(periodTimeSpan, callback, state);
        var intervalTimer
            = new System.Threading.Timer(IntervalTimerTicker, timerCallBack, periodTimeSpan, periodTimeSpan);
        timerCallBack.RegisteredTimer = intervalTimer;
        intervalCallBacks.Add(timerCallBack);
        var timerUpdate = recycler.Borrow<IntervalTimerUpdate>();
        timerCallBack.IncrementRefCount();
        timerUpdate.CallBackRunInfo = timerCallBack;
        timerUpdate.UpdateableTimer = this;
        return timerUpdate;
    }

    public ITimerUpdate RunAt(DateTime futureDateTime, WaitCallback callback) => RunAt(futureDateTime, null, callback);

    public ITimerUpdate RunAt(DateTime futureDateTime, Action callback) => RunAt(futureDateTime, null, _ => callback());

    public ITimerUpdate RunAt<T>(DateTime futureDateTime, T state, Action<T> callback) where T : class =>
        RunAt(futureDateTime, state, (WaitCallback)(stateParam => callback((T)stateParam!)));

    public ITimerUpdate RunAt(DateTime futureDateTime, object? state, WaitCallback callback)
    {
        if (isDead) throw new InvalidAsynchronousStateException("Timer was stopped and should be used again!");
        var timerCallBack
            = GetConfiguredOneOffTimerCallBackRunInfo(futureDateTime, callback, state);
        timerCallBack.RegisteredTimer = oneOffTimer;
        RegisterCallback(timerCallBack);
        CheckNextOneOffLaunchTimeStillCorrect(timerCallBack);
        var timerUpdate = recycler.Borrow<OneOffTimerUpdate>();
        timerCallBack.IncrementRefCount();
        timerUpdate.CallBackRunInfo = timerCallBack;
        timerUpdate.UpdateableTimer = this;
        return timerUpdate;
    }

    public void PauseAllTimers()
    {
        pauseAll = true;
        UpdateNextOneOffTimerTick(false);
        foreach (var oneOffCallback in oneOffCallbacks) oneOffCallback.IsPaused = true;
        foreach (var intervalCallback in intervalCallBacks)
        {
            intervalCallback.RegisteredTimer.Change(MaxTimerSpan, MaxTimerSpan);
            intervalCallback.IsPaused = true;
        }
    }

    public void ResumeAllTimers()
    {
        if (isDead) throw new InvalidAsynchronousStateException("Timer was stopped and should be used again!");
        pauseAll = false;

        foreach (var oneOffCallback in oneOffCallbacks) oneOffCallback.IsPaused = false;
        UpdateNextOneOffTimerTick(true);

        var now = TimeContext.UtcNow;
        foreach (var intvlCallback in intervalCallBacks)
        {
            intvlCallback.IsPaused = false;
            var nextTimerTickTime = intvlCallback.NextScheduleTime;
            var nextTickTimeSpan = nextTimerTickTime < now ? TimeSpan.Zero : nextTimerTickTime - now;
            intvlCallback.RegisteredTimer.Change(nextTickTimeSpan, intvlCallback.IntervalPeriodTimeSpan);
        }
    }

    public void StopAllTimers()
    {
        pauseAll = false;
        isDead = true;
        oneOffTimer.Dispose();
        foreach (var oneOffCallback in oneOffCallbacks) oneOffCallback.DecrementRefCount();
        oneOffCallbacks.Clear();
        foreach (var intvlCallback in intervalCallBacks)
        {
            intvlCallback.RegisteredTimer.Dispose();
            intvlCallback.DecrementRefCount();
        }

        intervalCallBacks.Clear();
    }

    public TimerCallBackRunInfo? RunNextScheduledOneOffCallbackNowOnThreadPool()
    {
        if (isDead) throw new InvalidAsynchronousStateException("Timer was stopped and should be used again!");
        var nextScheduledOneOff = FirstOneOffTimerCallBackRunInfo;

        if (nextScheduledOneOff != null)
        {
            nextScheduledOneOff.RunCallbackOnThreadPool();
            CheckNextOneOffLaunchTimeStillCorrect(nextScheduledOneOff);
        }

        return nextScheduledOneOff;
    }

    public TimerCallBackRunInfo? RunNextScheduledOneOffCallbackNowOnThisThread()
    {
        if (isDead) throw new InvalidAsynchronousStateException("Timer was stopped and should be used again!");
        var nextScheduledOneOff = FirstOneOffTimerCallBackRunInfo;

        if (nextScheduledOneOff != null)
        {
            nextScheduledOneOff.RunCallbackOnThisThread();
            CheckNextOneOffLaunchTimeStillCorrect(nextScheduledOneOff);
        }

        return nextScheduledOneOff;
    }

    public TimerCallBackRunInfo? RunNextScheduledIntervalCallbackNowOnThreadPool()
    {
        if (isDead) throw new InvalidAsynchronousStateException("Timer was stopped and should be used again!");
        if (intervalCallBacks.Any())
        {
            var nextIntervalTimer = NextScheduledIntervalTimerCallBackRunInfo;
            nextIntervalTimer?.RunCallbackOnThreadPool();
            return nextIntervalTimer;
        }

        return null;
    }

    public TimerCallBackRunInfo? RunNextScheduledIntervalCallbackNowThisThread()
    {
        if (isDead) throw new InvalidAsynchronousStateException("Timer was stopped and should be used again!");
        if (intervalCallBacks.Any())
        {
            var nextIntervalTimer = NextScheduledIntervalTimerCallBackRunInfo;
            nextIntervalTimer?.RunCallbackOnThisThread();
            return nextIntervalTimer;
        }

        return null;
    }

    public void CheckNextOneOffLaunchTimeStillCorrect(TimerCallBackRunInfo changed)
    {
        if (isDead) throw new InvalidAsynchronousStateException("Timer was stopped and should be used again!");
        if (pauseAll) return;
        var indxBeforeSort = oneOffCallbacks.IndexOf(changed);
        SortOneOfTimerTicks();

        if (indxBeforeSort == 0 || nextOneOffTimerTickDateTime >= changed.NextScheduleTime)
            UpdateNextOneOffTimerTick(true);
    }

    public bool Remove(TimerCallBackRunInfo callBackRunInfo)
    {
        if (isDead) throw new InvalidAsynchronousStateException("Timer was stopped and should be used again!");
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

    private void UpdateNextOneOffTimerTick(bool enable)
    {
        var heatBeatCheck = !enable ? MaxTimerSpan : heartBeatCheck;
        var now = TimeContext.UtcNow;
        nextOneOffTimerTickDateTime = !enable ? now + MaxTimerSpan : NextScheduledOneOffTimerTick;
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

    private TimerCallBackRunInfo GetConfiguredIntervalTimerCallBackRunInfo(TimeSpan intervalPeriod
        , WaitCallback callback
        , object? state = null)
    {
        var callBackInfo = recycler.Borrow<TimerCallBackRunInfo>();
        callBackInfo.Callback = callback;
        callBackInfo.CurrentNumberOfCalls = 0;
        var firstScheduledTime = TimeContext.UtcNow + intervalPeriod;
        callBackInfo.FirstScheduledTime = firstScheduledTime;
        callBackInfo.IsPaused = pauseAll;
        callBackInfo.LastRunTime = DateTime.MinValue;
        callBackInfo.MaxNumberOfCalls = int.MaxValue;
        callBackInfo.NextScheduleTime = firstScheduledTime;
        callBackInfo.IntervalPeriodTimeSpan = intervalPeriod;
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
        callBackInfo.IntervalPeriodTimeSpan = MaxTimerSpan;
        callBackInfo.State = state;
        return callBackInfo;
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    private void OneOffTimerTicker(object? stateInfo)
    {
        if (pauseAll) return;
        var now = TimeContext.UtcNow;
        try
        {
            oneOffSpinLock.Acquire();
            for (var i = 0; i < oneOffCallbacks.Count; i++)
            {
                var currentCallback = oneOffCallbacks[i];
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

        if (oneOffCallbacks.Any())
        {
            var nextScheduledOneOffTimerCallBackRunInfo = NextScheduledOneOffTimerCallBackRunInfo;
            if (nextScheduledOneOffTimerCallBackRunInfo != null)
                CheckNextOneOffLaunchTimeStillCorrect(nextScheduledOneOffTimerCallBackRunInfo);
        }
    }

    private void IntervalTimerTicker(object? stateInfo)
    {
        if (pauseAll) return;
        if (stateInfo is TimerCallBackRunInfo { IsPaused: false } intervalTimer)
            intervalTimer.RunCallbackOnThisThread();
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
