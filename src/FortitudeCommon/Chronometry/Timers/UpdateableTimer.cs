#region

using System.ComponentModel;
using System.Runtime.CompilerServices;
using FortitudeCommon.AsyncProcessing;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Monitoring.Logging;

#endregion

namespace FortitudeCommon.Chronometry.Timers;

public class UpdateableTimer : IUpdateableTimer
{
    public const uint MaxTimerMs = uint.MaxValue - 3_000;
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(UpdateableTimer));
    private static int totalTimers;
    private static ITimer? instance;
    private static readonly object SyncLock = new();
    internal static readonly TimeSpan MaxTimerSpan = TimeSpan.FromMilliseconds(MaxTimerMs);
    private readonly TimeSpan heartBeatCheck = TimeSpan.FromSeconds(10);
    private readonly List<TimerCallBackRunInfo> intervalCallBacks = new();
    private readonly string name;
    private readonly List<TimerCallBackRunInfo> oneOffCallbacks = new();
    private readonly ISyncLock oneOffSpinLock = new SpinLockLight();
    private readonly Timer oneOffTimer;
    private readonly IRecycler recycler;
    private int instanceNum;
    private bool isDead;
    private DateTime nextOneOffTimerTickDateTime = DateTime.MinValue;
    private volatile bool pauseAll;

    public UpdateableTimer(string? name = "Unnamed-Timer")
    {
        instanceNum = Interlocked.Increment(ref totalTimers);
        this.name = name + "-" + instanceNum;
        recycler = new Recycler();
        oneOffTimer = new Timer(OneOffTimerTicker, this, Timeout.Infinite, Timeout.Infinite);
    }

    public static ITimer Instance
    {
        get
        {
            if (instance == null)
                lock (SyncLock)
                {
                    instance ??= new UpdateableTimer();
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

    private DateTime NextScheduledOneOffTimerTick => NextScheduledOneOffTimerCallBackRunInfo?.NextScheduleTime ?? DateTime.UtcNow + MaxTimerSpan;

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

    public ITimerUpdate RunIn(TimeSpan waitTimeSpan, Func<ValueTask> callback)
    {
        if (isDead) throw new InvalidAsynchronousStateException("Timer was stopped and should be used again!");
        var now = TimeContext.UtcNow;
        var runAt = now + waitTimeSpan;
        var timerCallBack = GetValueTaskActionOneOffTimerRunInfo(runAt, callback);
        timerCallBack.RegisteredTimer = oneOffTimer;
        RegisterCallback(timerCallBack);
        CheckNextOneOffLaunchTimeStillCorrect(timerCallBack);
        var timerUpdate = recycler.Borrow<OneOffTimerUpdate>();
        timerCallBack.IncrementRefCount();
        timerUpdate.CallBackRunInfo = timerCallBack;
        timerUpdate.UpdateableTimer = this;
        return timerUpdate;
    }

    public ITimerUpdate RunIn<T>(TimeSpan waitTimeSpan, T state, Func<T?, ValueTask> callback) where T : class
    {
        if (isDead) throw new InvalidAsynchronousStateException("Timer was stopped and should be used again!");
        var now = TimeContext.UtcNow;
        var runAt = now + waitTimeSpan;
        var timerCallBack = GetValueTaskActionStateOneOffTimerRunInfo(runAt, callback, state);
        timerCallBack.RegisteredTimer = oneOffTimer;
        RegisterCallback(timerCallBack);
        CheckNextOneOffLaunchTimeStillCorrect(timerCallBack);
        var timerUpdate = recycler.Borrow<OneOffTimerUpdate>();
        timerCallBack.IncrementRefCount();
        timerUpdate.CallBackRunInfo = timerCallBack;
        timerUpdate.UpdateableTimer = this;
        return timerUpdate;
    }

    public ITimerUpdate RunIn(int waitMs, Func<ValueTask> callback) => RunIn(TimeSpan.FromMilliseconds(waitMs), callback);

    public ITimerUpdate RunIn<T>(int waitMs, T state, Func<T?, ValueTask> callback) where T : class =>
        RunIn(TimeSpan.FromMilliseconds(waitMs), state, callback);

    public ITimerUpdate RunEvery(TimeSpan periodTimeSpan, Func<ValueTask> callback)
    {
        if (isDead) throw new InvalidAsynchronousStateException("Timer was stopped and should be used again!");
        var timerCallBack = GetValueTaskActionIntervalTimerRunInfo(periodTimeSpan, callback);
        var intervalTimer = new Timer(IntervalTimerTicker, timerCallBack, periodTimeSpan, periodTimeSpan);
        timerCallBack.RegisteredTimer = intervalTimer;
        intervalCallBacks.Add(timerCallBack);
        var timerUpdate = recycler.Borrow<IntervalTimerUpdate>();
        timerCallBack.IncrementRefCount();
        timerUpdate.CallBackRunInfo = timerCallBack;
        timerUpdate.UpdateableTimer = this;
        return timerUpdate;
    }

    public ITimerUpdate RunEvery<T>(TimeSpan periodTimeSpan, T state, Func<T?, ValueTask> callback) where T : class
    {
        if (isDead) throw new InvalidAsynchronousStateException("Timer was stopped and should be used again!");
        var timerCallBack = GetValueTaskActionStateIntervalTimerRunInfo(periodTimeSpan, callback, state);
        var intervalTimer = new Timer(IntervalTimerTicker, timerCallBack, periodTimeSpan, periodTimeSpan);
        timerCallBack.RegisteredTimer = intervalTimer;
        intervalCallBacks.Add(timerCallBack);
        var timerUpdate = recycler.Borrow<IntervalTimerUpdate>();
        timerCallBack.IncrementRefCount();
        timerUpdate.CallBackRunInfo = timerCallBack;
        timerUpdate.UpdateableTimer = this;
        return timerUpdate;
    }

    public ITimerUpdate RunEvery(int intervalMs, Func<ValueTask> callback) => RunEvery(TimeSpan.FromMilliseconds(intervalMs), callback);

    public ITimerUpdate RunEvery<T>(int intervalMs, T state, Func<T?, ValueTask> callback) where T : class =>
        RunIn(TimeSpan.FromMilliseconds(intervalMs), state, callback);

    public ITimerUpdate RunAt(DateTime futureDateTime, Func<ValueTask> callback)
    {
        if (isDead) throw new InvalidAsynchronousStateException("Timer was stopped and should be used again!");
        var timerCallBack
            = GetValueTaskActionOneOffTimerRunInfo(futureDateTime, callback);
        timerCallBack.RegisteredTimer = oneOffTimer;
        RegisterCallback(timerCallBack);
        CheckNextOneOffLaunchTimeStillCorrect(timerCallBack);
        var timerUpdate = recycler.Borrow<OneOffTimerUpdate>();
        timerCallBack.IncrementRefCount();
        timerUpdate.CallBackRunInfo = timerCallBack;
        timerUpdate.UpdateableTimer = this;
        return timerUpdate;
    }

    public ITimerUpdate RunAt<T>(DateTime futureDateTime, T state, Func<T?, ValueTask> callback) where T : class
    {
        if (isDead) throw new InvalidAsynchronousStateException("Timer was stopped and should be used again!");
        var timerCallBack
            = GetValueTaskActionStateOneOffTimerRunInfo(futureDateTime, callback, state);
        timerCallBack.RegisteredTimer = oneOffTimer;
        RegisterCallback(timerCallBack);
        CheckNextOneOffLaunchTimeStillCorrect(timerCallBack);
        var timerUpdate = recycler.Borrow<OneOffTimerUpdate>();
        timerCallBack.IncrementRefCount();
        timerUpdate.CallBackRunInfo = timerCallBack;
        timerUpdate.UpdateableTimer = this;
        return timerUpdate;
    }


    public ITimerUpdate RunIn(TimeSpan waitTimeSpan, WaitCallback callback) => RunIn(waitTimeSpan, null, callback);

    public ITimerUpdate RunIn(TimeSpan waitTimeSpan, Action callback)
    {
        if (isDead) throw new InvalidAsynchronousStateException("Timer was stopped and should be used again!");
        var now = TimeContext.UtcNow;
        var runAt = now + waitTimeSpan;
        var timerCallBack = GetActionOneOffTimerRunInfo(runAt, callback);
        timerCallBack.RegisteredTimer = oneOffTimer;
        RegisterCallback(timerCallBack);
        CheckNextOneOffLaunchTimeStillCorrect(timerCallBack);
        var timerUpdate = recycler.Borrow<OneOffTimerUpdate>();
        timerCallBack.IncrementRefCount();
        timerUpdate.CallBackRunInfo = timerCallBack;
        timerUpdate.UpdateableTimer = this;
        return timerUpdate;
    }

    public ITimerUpdate RunIn<T>(TimeSpan waitTimeSpan, T state, Action<T?> callback) where T : class
    {
        if (isDead) throw new InvalidAsynchronousStateException("Timer was stopped and should be used again!");
        var now = TimeContext.UtcNow;
        var runAt = now + waitTimeSpan;
        var timerCallBack = GetActionStateOneOffTimerRunInfo(runAt, callback, state);
        timerCallBack.RegisteredTimer = oneOffTimer;
        RegisterCallback(timerCallBack);
        CheckNextOneOffLaunchTimeStillCorrect(timerCallBack);
        var timerUpdate = recycler.Borrow<OneOffTimerUpdate>();
        timerCallBack.IncrementRefCount();
        timerUpdate.CallBackRunInfo = timerCallBack;
        timerUpdate.UpdateableTimer = this;
        return timerUpdate;
    }

    public ITimerUpdate RunIn(TimeSpan waitTimeSpan, object? state, WaitCallback callback)
    {
        if (isDead) throw new InvalidAsynchronousStateException("Timer was stopped and should be used again!");
        var now = TimeContext.UtcNow;
        var runAt = now + waitTimeSpan;
        var timerCallBack
            = GetWaitCallbackOneOffTimerRunInfo(runAt, callback, state);
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

    public ITimerUpdate RunIn<T>(int waitMs, T state, Action<T?> callback) where T : class =>
        RunIn(TimeSpan.FromMilliseconds(waitMs), state, callback);

    public ITimerUpdate RunIn(int waitMs, object? state, WaitCallback callback) => RunIn(TimeSpan.FromMilliseconds(waitMs), state, callback);

    public ITimerUpdate RunEvery(int intervalMs, WaitCallback callback) => RunEvery(TimeSpan.FromMilliseconds(intervalMs), callback);

    public ITimerUpdate RunEvery(int intervalMs, Action callback) => RunEvery(TimeSpan.FromMilliseconds(intervalMs), callback);

    public ITimerUpdate RunEvery<T>(int intervalMs, T state, Action<T?> callback) where T : class =>
        RunEvery(TimeSpan.FromMilliseconds(intervalMs), state, callback);

    public ITimerUpdate RunEvery(int intervalMs, object? state, WaitCallback callback) =>
        RunEvery(TimeSpan.FromMilliseconds(intervalMs), state, callback);

    public ITimerUpdate RunEvery(TimeSpan periodTimeSpan, WaitCallback callback) => RunEvery(periodTimeSpan, null, callback);

    public ITimerUpdate RunEvery(TimeSpan periodTimeSpan, Action callback)
    {
        if (isDead) throw new InvalidAsynchronousStateException("Timer was stopped and should be used again!");
        var timerCallBack
            = GetActionIntervalTimerRunInfo(periodTimeSpan, callback);
        var intervalTimer
            = new Timer(IntervalTimerTicker, timerCallBack, periodTimeSpan, periodTimeSpan);
        timerCallBack.RegisteredTimer = intervalTimer;
        intervalCallBacks.Add(timerCallBack);
        var timerUpdate = recycler.Borrow<IntervalTimerUpdate>();
        timerCallBack.IncrementRefCount();
        timerUpdate.CallBackRunInfo = timerCallBack;
        timerUpdate.UpdateableTimer = this;
        return timerUpdate;
    }

    public ITimerUpdate RunEvery<T>(TimeSpan periodTimeSpan, T state, Action<T?> callback) where T : class
    {
        if (isDead) throw new InvalidAsynchronousStateException("Timer was stopped and should be used again!");
        var timerCallBack
            = GetActionStateIntervalTimerRunInfo(periodTimeSpan, callback, state);
        var intervalTimer
            = new Timer(IntervalTimerTicker, timerCallBack, periodTimeSpan, periodTimeSpan);
        timerCallBack.RegisteredTimer = intervalTimer;
        intervalCallBacks.Add(timerCallBack);
        var timerUpdate = recycler.Borrow<IntervalTimerUpdate>();
        timerCallBack.IncrementRefCount();
        timerUpdate.CallBackRunInfo = timerCallBack;
        timerUpdate.UpdateableTimer = this;
        return timerUpdate;
    }

    public ITimerUpdate RunEvery(TimeSpan periodTimeSpan, object? state, WaitCallback callback)
    {
        if (isDead) throw new InvalidAsynchronousStateException("Timer was stopped and should be used again!");
        var timerCallBack
            = GetWaitCallbackIntervalTimerRunInfo(periodTimeSpan, callback, state);
        var intervalTimer
            = new Timer(IntervalTimerTicker, timerCallBack, periodTimeSpan, periodTimeSpan);
        timerCallBack.RegisteredTimer = intervalTimer;
        intervalCallBacks.Add(timerCallBack);
        var timerUpdate = recycler.Borrow<IntervalTimerUpdate>();
        timerCallBack.IncrementRefCount();
        timerUpdate.CallBackRunInfo = timerCallBack;
        timerUpdate.UpdateableTimer = this;
        return timerUpdate;
    }

    public ITimerUpdate RunAt(DateTime futureDateTime, WaitCallback callback) => RunAt(futureDateTime, null, callback);

    public ITimerUpdate RunAt(DateTime futureDateTime, Action callback)
    {
        if (isDead) throw new InvalidAsynchronousStateException("Timer was stopped and should be used again!");
        var timerCallBack
            = GetActionOneOffTimerRunInfo(futureDateTime, callback);
        timerCallBack.RegisteredTimer = oneOffTimer;
        RegisterCallback(timerCallBack);
        CheckNextOneOffLaunchTimeStillCorrect(timerCallBack);
        var timerUpdate = recycler.Borrow<OneOffTimerUpdate>();
        timerCallBack.IncrementRefCount();
        timerUpdate.CallBackRunInfo = timerCallBack;
        timerUpdate.UpdateableTimer = this;
        return timerUpdate;
    }

    public ITimerUpdate RunAt<T>(DateTime futureDateTime, T state, Action<T?> callback) where T : class
    {
        if (isDead) throw new InvalidAsynchronousStateException("Timer was stopped and should be used again!");
        var timerCallBack
            = GetActionStateOneOffTimerRunInfo(futureDateTime, callback, state);
        timerCallBack.RegisteredTimer = oneOffTimer;
        RegisterCallback(timerCallBack);
        CheckNextOneOffLaunchTimeStillCorrect(timerCallBack);
        var timerUpdate = recycler.Borrow<OneOffTimerUpdate>();
        timerCallBack.IncrementRefCount();
        timerUpdate.CallBackRunInfo = timerCallBack;
        timerUpdate.UpdateableTimer = this;
        return timerUpdate;
    }

    public ITimerUpdate RunAt(DateTime futureDateTime, object? state, WaitCallback callback)
    {
        if (isDead) throw new InvalidAsynchronousStateException("Timer was stopped and should be used again!");
        var timerCallBack
            = GetWaitCallbackOneOffTimerRunInfo(futureDateTime, callback, state);
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

    public TimerCallBackRunInfo? RunNextScheduledOneOffCallbackNow()
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

    public TimerCallBackRunInfo? RunNextScheduledIntervalCallbackNow()
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
            UpdateNextOneOffTimerTick(oneOffCallbacks.Any());
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
        TimeSpan rangeCheckNextTick;
        if (now.Add(nextTick) > now.Add(MaxTimerSpan).Add(TimeSpan.FromSeconds(2)))
        {
            Logger.Warn("Limiting next {0} timer tick as was out of range {1}", enable ? "enabled" : "disabled", nextTick);
            rangeCheckNextTick = now.Add(nextTick) > now.Add(MaxTimerSpan) ? MaxTimerSpan : nextTick;
        }
        else
        {
            rangeCheckNextTick = nextTick;
        }

        oneOffTimer.Change(rangeCheckNextTick, heatBeatCheck);
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

    private TimerCallBackRunInfo GetWaitCallbackIntervalTimerRunInfo(TimeSpan intervalPeriod
        , WaitCallback callback
        , object? state = null)
    {
        var callBackInfo = recycler.Borrow<WaitCallbackTimerCallBackRunInfo>();
        callBackInfo.WaitCallback = callback;
        callBackInfo.State = state;
        return ConfigureIntervalTimerRunInfo(intervalPeriod, callBackInfo);
    }

    private TimerCallBackRunInfo GetActionIntervalTimerRunInfo(TimeSpan intervalPeriod
        , Action callback)
    {
        var callBackInfo = recycler.Borrow<ActionTimerCallBackRunInfo>();
        callBackInfo.Action = callback;
        return ConfigureIntervalTimerRunInfo(intervalPeriod, callBackInfo);
    }

    private TimerCallBackRunInfo GetActionStateIntervalTimerRunInfo<T>(TimeSpan intervalPeriod
        , Action<T?> callback, T? state) where T : class
    {
        var callBackInfo = recycler.Borrow<ActionStateTimerCallBackRunInfo<T>>();
        callBackInfo.Action = callback;
        callBackInfo.State = state;
        return ConfigureIntervalTimerRunInfo(intervalPeriod, callBackInfo);
    }

    private TimerCallBackRunInfo GetValueTaskActionIntervalTimerRunInfo(TimeSpan intervalPeriod
        , Func<ValueTask> callback)
    {
        var callBackInfo = recycler.Borrow<ValueTaskActionTimerCallBackRunInfo>();
        callBackInfo.ValueTaskAction = callback;
        return ConfigureIntervalTimerRunInfo(intervalPeriod, callBackInfo);
    }

    private TimerCallBackRunInfo GetValueTaskActionStateIntervalTimerRunInfo<T>(TimeSpan intervalPeriod
        , Func<T?, ValueTask> callback, T? state) where T : class
    {
        var callBackInfo = recycler.Borrow<ValueTaskActionStateTimerCallBackRunInfo<T>>();
        callBackInfo.ValueTaskActionState = callback;
        callBackInfo.State = state;
        return ConfigureIntervalTimerRunInfo(intervalPeriod, callBackInfo);
    }

    private TimerCallBackRunInfo ConfigureIntervalTimerRunInfo(TimeSpan intervalPeriod
        , TimerCallBackRunInfo callBackInfo)
    {
        callBackInfo.CurrentNumberOfCalls = 0;
        var firstScheduledTime = TimeContext.UtcNow + intervalPeriod;
        callBackInfo.FirstScheduledTime = firstScheduledTime;
        callBackInfo.IsPaused = pauseAll;
        callBackInfo.LastRunTime = DateTime.MinValue;
        callBackInfo.MaxNumberOfCalls = int.MaxValue;
        callBackInfo.NextScheduleTime = firstScheduledTime;
        callBackInfo.IntervalPeriodTimeSpan = intervalPeriod;
        return callBackInfo;
    }

    private TimerCallBackRunInfo GetWaitCallbackOneOffTimerRunInfo(DateTime runAt, WaitCallback callback
        , object? state = null)
    {
        var callBackInfo = recycler.Borrow<WaitCallbackTimerCallBackRunInfo>();
        callBackInfo.WaitCallback = callback;
        callBackInfo.State = state;
        return ConfigureOneOffTimerRunInfo(runAt, callBackInfo);
    }

    private TimerCallBackRunInfo GetActionOneOffTimerRunInfo(DateTime runAt, Action callback)
    {
        var callBackInfo = recycler.Borrow<ActionTimerCallBackRunInfo>();
        callBackInfo.Action = callback;
        return ConfigureOneOffTimerRunInfo(runAt, callBackInfo);
    }

    private TimerCallBackRunInfo GetActionStateOneOffTimerRunInfo<T>(DateTime runAt
        , Action<T?> callback, T? state) where T : class
    {
        var callBackInfo = recycler.Borrow<ActionStateTimerCallBackRunInfo<T>>();
        callBackInfo.Action = callback;
        callBackInfo.State = state;
        return ConfigureOneOffTimerRunInfo(runAt, callBackInfo);
    }

    private TimerCallBackRunInfo GetValueTaskActionOneOffTimerRunInfo(DateTime runAt, Func<ValueTask> callback)
    {
        var callBackInfo = recycler.Borrow<ValueTaskActionTimerCallBackRunInfo>();
        callBackInfo.ValueTaskAction = callback;
        return ConfigureOneOffTimerRunInfo(runAt, callBackInfo);
    }

    private TimerCallBackRunInfo GetValueTaskActionStateOneOffTimerRunInfo<T>(DateTime runAt
        , Func<T?, ValueTask> callback, T? state) where T : class
    {
        var callBackInfo = recycler.Borrow<ValueTaskActionStateTimerCallBackRunInfo<T>>();
        callBackInfo.ValueTaskActionState = callback;
        callBackInfo.State = state;
        return ConfigureOneOffTimerRunInfo(runAt, callBackInfo);
    }

    private TimerCallBackRunInfo ConfigureOneOffTimerRunInfo(DateTime runAt, TimerCallBackRunInfo callBackInfo)
    {
        callBackInfo.CurrentNumberOfCalls = 0;
        callBackInfo.FirstScheduledTime = runAt;
        callBackInfo.IsPaused = pauseAll;
        callBackInfo.LastRunTime = DateTime.MinValue;
        callBackInfo.MaxNumberOfCalls = 1;
        callBackInfo.NextScheduleTime = runAt;
        callBackInfo.IntervalPeriodTimeSpan = MaxTimerSpan;
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

    public override string ToString() => $"Timer({name})";
}
