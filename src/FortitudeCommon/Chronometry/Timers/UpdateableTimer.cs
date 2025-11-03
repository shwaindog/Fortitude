// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.ComponentModel;
using System.Runtime.CompilerServices;
using FortitudeCommon.AsyncProcessing;
using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Monitoring.Logging;

#endregion

namespace FortitudeCommon.Chronometry.Timers;

public class UpdateableTimer : IUpdateableTimer
{
    public const uint MaxTimerMs = uint.MaxValue - 3_000;

    private static readonly  IFLogger Logger       = FLoggerFactory.Instance.GetLogger(typeof(UpdateableTimer));
    internal static readonly TimeSpan MaxTimerSpan = TimeSpan.FromMilliseconds(MaxTimerMs);

    private static int totalTimers;

    private readonly TimeSpan heartBeatCheck = TimeSpan.FromSeconds(10);

    private readonly List<ITimerCallBackRunInfo> intervalCallBacks = new();

    private readonly string name;

    private readonly List<ITimerCallBackRunInfo> oneOffCallbacks = new();

    private readonly   ISyncLock oneOffSpinLock = new SpinLockLight();
    protected readonly ITimer    OneOffTimer;

    private int  instanceNum;
    private bool isDead;

    private DateTime nextOneOffTimerTickDateTime = DateTime.MinValue;

    private volatile bool pauseAll;

    public UpdateableTimer(string? name = "Unnamed-Timer")
    {
        instanceNum = Interlocked.Increment(ref totalTimers);
        this.name   = name + "-" + instanceNum;
        Recycler    = new Recycler();
        OneOffTimer = CreateOneOffTimer();
    }


    protected ITimerCallBackRunInfo? NextScheduledOneOffTimerCallBackRunInfo
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

    private DateTime NextScheduledOneOffTimerTick => NextScheduledOneOffTimerCallBackRunInfo?.NextScheduleTime ?? TimeContext.UtcNow + MaxTimerSpan;

    private ITimerCallBackRunInfo? FirstOneOffTimerCallBackRunInfo
    {
        get
        {
            ITimerCallBackRunInfo? nextScheduledOneOff;
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

    private ITimerCallBackRunInfo? NextScheduledIntervalTimerCallBackRunInfo
    {
        get
        {
            var nextIntervalTimerLowestDateTime = intervalCallBacks.Min(tcbri => tcbri.NextScheduleTime);
            var nextIntervalTimer
                = intervalCallBacks.FirstOrDefault(tcbri => tcbri.NextScheduleTime == nextIntervalTimerLowestDateTime);
            return nextIntervalTimer;
        }
    }

    public IRecycler? Recycler { get; set; }

    public ITimerUpdate RunIn(TimeSpan waitTimeSpan, Func<ValueTask> callback)
    {
        if (isDead) throw new InvalidAsynchronousStateException("Timer was stopped and should be used again!");
        var now           = TimeContext.UtcNow;
        var runAt         = now + waitTimeSpan;
        var timerCallBack = GetValueTaskActionOneOffTimerRunInfo(runAt, callback);
        timerCallBack.RegisteredTimer = OneOffTimer;
        RegisterCallback(timerCallBack);
        CheckNextOneOffLaunchTimeStillCorrect(timerCallBack);
        var timerUpdate = Recycler!.Borrow<OneOffTimerUpdate>();
        timerCallBack.IncrementRefCount();
        timerUpdate.CallBackRunInfo = timerCallBack;
        timerUpdate.UpdateableTimer = this;
        return timerUpdate;
    }

    public ITimerUpdate RunIn<T>(TimeSpan waitTimeSpan, T state, Func<T?, ValueTask> callback) where T : class
    {
        if (isDead) throw new InvalidAsynchronousStateException("Timer was stopped and should be used again!");
        var now           = TimeContext.UtcNow;
        var runAt         = now + waitTimeSpan;
        var timerCallBack = GetValueTaskActionStateOneOffTimerRunInfo(runAt, callback, state);
        timerCallBack.RegisteredTimer = OneOffTimer;
        RegisterCallback(timerCallBack);
        CheckNextOneOffLaunchTimeStillCorrect(timerCallBack);
        var timerUpdate = Recycler!.Borrow<OneOffTimerUpdate>();
        timerCallBack.IncrementRefCount();
        timerUpdate.CallBackRunInfo = timerCallBack;
        timerUpdate.UpdateableTimer = this;
        return timerUpdate;
    }

    public ITimerUpdate RunIn(TimeSpan waitTimeSpan, Func<IScheduleActualTime?, ValueTask> callback)
    {
        if (isDead) throw new InvalidAsynchronousStateException("Timer was stopped and should be used again!");
        var now           = TimeContext.UtcNow;
        var runAt         = now + waitTimeSpan;
        var timerCallBack = GetScheduledActualValueTaskActionOneOffTimerRunInfo(runAt, callback);
        timerCallBack.RegisteredTimer = OneOffTimer;
        RegisterCallback(timerCallBack);
        CheckNextOneOffLaunchTimeStillCorrect(timerCallBack);
        var timerUpdate = Recycler!.Borrow<OneOffTimerUpdate>();
        timerCallBack.IncrementRefCount();
        timerUpdate.CallBackRunInfo = timerCallBack;
        timerUpdate.UpdateableTimer = this;
        return timerUpdate;
    }

    public ITimerUpdate RunIn<T>(TimeSpan waitTimeSpan, T state, Func<IScheduleActualTime<T>?, ValueTask> callback) where T : class
    {
        if (isDead) throw new InvalidAsynchronousStateException("Timer was stopped and should be used again!");
        var now           = TimeContext.UtcNow;
        var runAt         = now + waitTimeSpan;
        var timerCallBack = GetScheduledActualValueTaskActionStateOneOffTimerRunInfo(runAt, callback, state);
        timerCallBack.RegisteredTimer = OneOffTimer;
        RegisterCallback(timerCallBack);
        CheckNextOneOffLaunchTimeStillCorrect(timerCallBack);
        var timerUpdate = Recycler!.Borrow<OneOffTimerUpdate>();
        timerCallBack.IncrementRefCount();
        timerUpdate.CallBackRunInfo = timerCallBack;
        timerUpdate.UpdateableTimer = this;
        return timerUpdate;
    }

    public ITimerUpdate RunIn(int waitMs, Func<IScheduleActualTime?, ValueTask> callback) => RunIn(TimeSpan.FromMilliseconds(waitMs), callback);

    public ITimerUpdate RunIn<T>
        (int waitMs, T state, Func<IScheduleActualTime<T>?, ValueTask> callback) where T : class =>
        RunIn(TimeSpan.FromMilliseconds(waitMs), state, callback);

    public ITimerUpdate RunIn(int waitMs, Func<ValueTask> callback) => RunIn(TimeSpan.FromMilliseconds(waitMs), callback);

    public ITimerUpdate RunIn<T>(int waitMs, T state, Func<T?, ValueTask> callback) where T : class =>
        RunIn(TimeSpan.FromMilliseconds(waitMs), state, callback);

    public ITimerUpdate RunEvery(TimeSpan periodTimeSpan, Func<ValueTask> callback)
    {
        if (isDead) throw new InvalidAsynchronousStateException("Timer was stopped and should be used again!");
        var timerCallBack = GetValueTaskActionIntervalTimerRunInfo(periodTimeSpan, callback);
        var intervalTimer = CreateIntervalTimer(timerCallBack, periodTimeSpan);
        timerCallBack.RegisteredTimer = intervalTimer;
        intervalCallBacks.Add(timerCallBack);
        var timerUpdate = Recycler!.Borrow<IntervalTimerUpdate>();
        timerCallBack.IncrementRefCount();
        timerUpdate.CallBackRunInfo = timerCallBack;
        timerUpdate.UpdateableTimer = this;
        return timerUpdate;
    }

    public ITimerUpdate RunEvery<T>(TimeSpan periodTimeSpan, T state, Func<T?, ValueTask> callback) where T : class
    {
        if (isDead) throw new InvalidAsynchronousStateException("Timer was stopped and should be used again!");
        var timerCallBack = GetValueTaskActionStateIntervalTimerRunInfo(periodTimeSpan, callback, state);
        var intervalTimer = CreateIntervalTimer(timerCallBack, periodTimeSpan);
        timerCallBack.RegisteredTimer = intervalTimer;
        intervalCallBacks.Add(timerCallBack);
        var timerUpdate = Recycler!.Borrow<IntervalTimerUpdate>();
        timerCallBack.IncrementRefCount();
        timerUpdate.CallBackRunInfo = timerCallBack;
        timerUpdate.UpdateableTimer = this;
        return timerUpdate;
    }


    public ITimerUpdate RunEvery(TimeSpan periodTimeSpan, Func<IScheduleActualTime?, ValueTask> callback)
    {
        if (isDead) throw new InvalidAsynchronousStateException("Timer was stopped and should be used again!");
        var timerCallBack = GetScheduledActualValueTaskValueTaskActionIntervalTimerRunInfo(periodTimeSpan, callback);
        var intervalTimer = CreateIntervalTimer(timerCallBack, periodTimeSpan);
        timerCallBack.RegisteredTimer = intervalTimer;
        intervalCallBacks.Add(timerCallBack);
        var timerUpdate = Recycler!.Borrow<IntervalTimerUpdate>();
        timerCallBack.IncrementRefCount();
        timerUpdate.CallBackRunInfo = timerCallBack;
        timerUpdate.UpdateableTimer = this;
        return timerUpdate;
    }

    public ITimerUpdate RunEvery<T>(TimeSpan periodTimeSpan, T state, Func<IScheduleActualTime<T>?, ValueTask> callback) where T : class
    {
        if (isDead) throw new InvalidAsynchronousStateException("Timer was stopped and should be used again!");
        var timerCallBack = GetScheduledActualValueTaskActionStateIntervalTimerRunInfo(periodTimeSpan, callback, state);
        var intervalTimer = CreateIntervalTimer(timerCallBack, periodTimeSpan);
        timerCallBack.RegisteredTimer = intervalTimer;
        intervalCallBacks.Add(timerCallBack);
        var timerUpdate = Recycler!.Borrow<IntervalTimerUpdate>();
        timerCallBack.IncrementRefCount();
        timerUpdate.CallBackRunInfo = timerCallBack;
        timerUpdate.UpdateableTimer = this;
        return timerUpdate;
    }

    public ITimerUpdate RunEvery
        (int intervalMs, Func<IScheduleActualTime?, ValueTask> callback) =>
        RunEvery(TimeSpan.FromMilliseconds(intervalMs), callback);

    public ITimerUpdate RunEvery<T>
        (int intervalMs, T state, Func<IScheduleActualTime<T>?, ValueTask> callback) where T : class =>
        RunEvery(TimeSpan.FromMilliseconds(intervalMs), state, callback);


    public ITimerUpdate RunEvery(int intervalMs, Func<ValueTask> callback) => RunEvery(TimeSpan.FromMilliseconds(intervalMs), callback);

    public ITimerUpdate RunEvery<T>(int intervalMs, T state, Func<T?, ValueTask> callback) where T : class =>
        RunIn(TimeSpan.FromMilliseconds(intervalMs), state, callback);

    public ITimerUpdate RunAt(DateTime futureDateTime, Func<ValueTask> callback)
    {
        if (isDead) throw new InvalidAsynchronousStateException("Timer was stopped and should be used again!");
        var timerCallBack
            = GetValueTaskActionOneOffTimerRunInfo(futureDateTime, callback);
        timerCallBack.RegisteredTimer = OneOffTimer;
        RegisterCallback(timerCallBack);
        CheckNextOneOffLaunchTimeStillCorrect(timerCallBack);
        var timerUpdate = Recycler!.Borrow<OneOffTimerUpdate>();
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
        timerCallBack.RegisteredTimer = OneOffTimer;
        RegisterCallback(timerCallBack);
        CheckNextOneOffLaunchTimeStillCorrect(timerCallBack);
        var timerUpdate = Recycler!.Borrow<OneOffTimerUpdate>();
        timerCallBack.IncrementRefCount();
        timerUpdate.CallBackRunInfo = timerCallBack;
        timerUpdate.UpdateableTimer = this;
        return timerUpdate;
    }

    public ITimerUpdate RunAt(DateTime futureDateTime, Func<IScheduleActualTime?, ValueTask> callback)
    {
        if (isDead) throw new InvalidAsynchronousStateException("Timer was stopped and should be used again!");
        var timerCallBack
            = GetScheduledActualValueTaskActionOneOffTimerRunInfo(futureDateTime, callback);
        timerCallBack.RegisteredTimer = OneOffTimer;
        RegisterCallback(timerCallBack);
        CheckNextOneOffLaunchTimeStillCorrect(timerCallBack);
        var timerUpdate = Recycler!.Borrow<OneOffTimerUpdate>();
        timerCallBack.IncrementRefCount();
        timerUpdate.CallBackRunInfo = timerCallBack;
        timerUpdate.UpdateableTimer = this;
        return timerUpdate;
    }

    public ITimerUpdate RunAt<T>(DateTime futureDateTime, T state, Func<IScheduleActualTime<T>?, ValueTask> callback) where T : class
    {
        if (isDead) throw new InvalidAsynchronousStateException("Timer was stopped and should be used again!");
        var timerCallBack
            = GetScheduledActualValueTaskActionStateOneOffTimerRunInfo(futureDateTime, callback, state);
        timerCallBack.RegisteredTimer = OneOffTimer;
        RegisterCallback(timerCallBack);
        CheckNextOneOffLaunchTimeStillCorrect(timerCallBack);
        var timerUpdate = Recycler!.Borrow<OneOffTimerUpdate>();
        timerCallBack.IncrementRefCount();
        timerUpdate.CallBackRunInfo = timerCallBack;
        timerUpdate.UpdateableTimer = this;
        return timerUpdate;
    }


    public ITimerUpdate RunIn(TimeSpan waitTimeSpan, WaitCallback callback) => RunIn(waitTimeSpan, null, callback);

    public ITimerUpdate RunIn(TimeSpan waitTimeSpan, Action callback)
    {
        if (isDead) throw new InvalidAsynchronousStateException("Timer was stopped and should be used again!");
        var now           = TimeContext.UtcNow;
        var runAt         = now + waitTimeSpan;
        var timerCallBack = GetActionOneOffTimerRunInfo(runAt, callback);
        timerCallBack.RegisteredTimer = OneOffTimer;
        RegisterCallback(timerCallBack);
        CheckNextOneOffLaunchTimeStillCorrect(timerCallBack);
        var timerUpdate = Recycler!.Borrow<OneOffTimerUpdate>();
        timerCallBack.IncrementRefCount();
        timerUpdate.CallBackRunInfo = timerCallBack;
        timerUpdate.UpdateableTimer = this;
        return timerUpdate;
    }

    public ITimerUpdate RunIn<T>(TimeSpan waitTimeSpan, T state, Action<T?> callback) where T : class
    {
        if (isDead) throw new InvalidAsynchronousStateException("Timer was stopped and should be used again!");
        var now           = TimeContext.UtcNow;
        var runAt         = now + waitTimeSpan;
        var timerCallBack = GetActionStateOneOffTimerRunInfo(runAt, callback, state);
        timerCallBack.RegisteredTimer = OneOffTimer;
        RegisterCallback(timerCallBack);
        CheckNextOneOffLaunchTimeStillCorrect(timerCallBack);
        var timerUpdate = Recycler!.Borrow<OneOffTimerUpdate>();
        timerCallBack.IncrementRefCount();
        timerUpdate.CallBackRunInfo = timerCallBack;
        timerUpdate.UpdateableTimer = this;
        return timerUpdate;
    }

    public ITimerUpdate RunIn(TimeSpan waitTimeSpan, object? state, WaitCallback callback)
    {
        if (isDead) throw new InvalidAsynchronousStateException("Timer was stopped and should be used again!");
        var now   = TimeContext.UtcNow;
        var runAt = now + waitTimeSpan;
        var timerCallBack
            = GetWaitCallbackOneOffTimerRunInfo(runAt, callback, state);
        timerCallBack.RegisteredTimer = OneOffTimer;
        RegisterCallback(timerCallBack);
        CheckNextOneOffLaunchTimeStillCorrect(timerCallBack);
        var timerUpdate = Recycler!.Borrow<OneOffTimerUpdate>();
        timerCallBack.IncrementRefCount();
        timerUpdate.CallBackRunInfo = timerCallBack;
        timerUpdate.UpdateableTimer = this;
        return timerUpdate;
    }

    public ITimerUpdate RunIn(TimeSpan waitTimeSpan, Action<IScheduleActualTime?> callback)
    {
        if (isDead) throw new InvalidAsynchronousStateException("Timer was stopped and should be used again!");
        var now   = TimeContext.UtcNow;
        var runAt = now + waitTimeSpan;
        var timerCallBack
            = GetScheduledActualActionOneOffTimerRunInfo(runAt, callback);
        timerCallBack.RegisteredTimer = OneOffTimer;
        RegisterCallback(timerCallBack);
        CheckNextOneOffLaunchTimeStillCorrect(timerCallBack);
        var timerUpdate = Recycler!.Borrow<OneOffTimerUpdate>();
        timerCallBack.IncrementRefCount();
        timerUpdate.CallBackRunInfo = timerCallBack;
        timerUpdate.UpdateableTimer = this;
        return timerUpdate;
    }

    public ITimerUpdate RunIn<T>(TimeSpan waitTimeSpan, T state, Action<IScheduleActualTime<T>?> callback) where T : class
    {
        if (isDead) throw new InvalidAsynchronousStateException("Timer was stopped and should be used again!");
        var now   = TimeContext.UtcNow;
        var runAt = now + waitTimeSpan;
        var timerCallBack
            = GetScheduledActualActionStateOneOffTimerRunInfo(runAt, callback, state);
        timerCallBack.RegisteredTimer = OneOffTimer;
        RegisterCallback(timerCallBack);
        CheckNextOneOffLaunchTimeStillCorrect(timerCallBack);
        var timerUpdate = Recycler!.Borrow<OneOffTimerUpdate>();
        timerCallBack.IncrementRefCount();
        timerUpdate.CallBackRunInfo = timerCallBack;
        timerUpdate.UpdateableTimer = this;
        return timerUpdate;
    }

    public ITimerUpdate RunIn(int waitMs, Action<IScheduleActualTime?> callback) => RunIn(TimeSpan.FromMilliseconds(waitMs), callback);

    public ITimerUpdate RunIn<T>
        (int waitMs, T state, Action<IScheduleActualTime<T>?> callback) where T : class =>
        RunIn(TimeSpan.FromMilliseconds(waitMs), state, callback);

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
            = CreateIntervalTimer(timerCallBack, periodTimeSpan);
        timerCallBack.RegisteredTimer = intervalTimer;
        intervalCallBacks.Add(timerCallBack);
        var timerUpdate = Recycler!.Borrow<IntervalTimerUpdate>();
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
            = CreateIntervalTimer(timerCallBack, periodTimeSpan);
        timerCallBack.RegisteredTimer = intervalTimer;
        intervalCallBacks.Add(timerCallBack);
        var timerUpdate = Recycler!.Borrow<IntervalTimerUpdate>();
        timerCallBack.IncrementRefCount();
        timerUpdate.CallBackRunInfo = timerCallBack;
        timerUpdate.UpdateableTimer = this;
        return timerUpdate;
    }

    public ITimerUpdate RunEvery(TimeSpan periodTimeSpan, Action<IScheduleActualTime?> callback)
    {
        if (isDead) throw new InvalidAsynchronousStateException("Timer was stopped and should be used again!");
        var timerCallBack
            = GetScheduledActualActionIntervalTimerRunInfo(periodTimeSpan, callback);
        var intervalTimer
            = CreateIntervalTimer(timerCallBack, periodTimeSpan);
        timerCallBack.RegisteredTimer = intervalTimer;
        intervalCallBacks.Add(timerCallBack);
        var timerUpdate = Recycler!.Borrow<IntervalTimerUpdate>();
        timerCallBack.IncrementRefCount();
        timerUpdate.CallBackRunInfo = timerCallBack;
        timerUpdate.UpdateableTimer = this;
        return timerUpdate;
    }

    public ITimerUpdate RunEvery<T>(TimeSpan periodTimeSpan, T state, Action<IScheduleActualTime<T>?> callback) where T : class
    {
        if (isDead) throw new InvalidAsynchronousStateException("Timer was stopped and should be used again!");
        var timerCallBack
            = GetScheduledActualActionStateIntervalTimerRunInfo(periodTimeSpan, callback, state);
        var intervalTimer
            = CreateIntervalTimer(timerCallBack, periodTimeSpan);
        timerCallBack.RegisteredTimer = intervalTimer;
        intervalCallBacks.Add(timerCallBack);
        var timerUpdate = Recycler!.Borrow<IntervalTimerUpdate>();
        timerCallBack.IncrementRefCount();
        timerUpdate.CallBackRunInfo = timerCallBack;
        timerUpdate.UpdateableTimer = this;
        return timerUpdate;
    }


    public ITimerUpdate RunEvery(int intervalMs, Action<IScheduleActualTime?> callback) => RunEvery(TimeSpan.FromMilliseconds(intervalMs), callback);

    public ITimerUpdate RunEvery<T>(int intervalMs, T state, Action<IScheduleActualTime<T>?> callback) where T : class =>
        RunEvery(TimeSpan.FromMilliseconds(intervalMs), state, callback);

    public ITimerUpdate RunEvery(TimeSpan periodTimeSpan, object? state, WaitCallback callback)
    {
        if (isDead) throw new InvalidAsynchronousStateException("Timer was stopped and should be used again!");
        var timerCallBack
            = GetWaitCallbackIntervalTimerRunInfo(periodTimeSpan, callback, state);
        var intervalTimer
            = CreateIntervalTimer(timerCallBack, periodTimeSpan);
        timerCallBack.RegisteredTimer = intervalTimer;
        intervalCallBacks.Add(timerCallBack);
        var timerUpdate = Recycler!.Borrow<IntervalTimerUpdate>();
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
        timerCallBack.RegisteredTimer = OneOffTimer;
        RegisterCallback(timerCallBack);
        CheckNextOneOffLaunchTimeStillCorrect(timerCallBack);
        var timerUpdate = Recycler!.Borrow<OneOffTimerUpdate>();
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
        timerCallBack.RegisteredTimer = OneOffTimer;
        RegisterCallback(timerCallBack);
        CheckNextOneOffLaunchTimeStillCorrect(timerCallBack);
        var timerUpdate = Recycler!.Borrow<OneOffTimerUpdate>();
        timerCallBack.IncrementRefCount();
        timerUpdate.CallBackRunInfo = timerCallBack;
        timerUpdate.UpdateableTimer = this;
        return timerUpdate;
    }

    public ITimerUpdate RunAt(DateTime futureDateTime, Action<IScheduleActualTime?> callback)
    {
        if (isDead) throw new InvalidAsynchronousStateException("Timer was stopped and should be used again!");
        var timerCallBack
            = GetScheduledActualActionOneOffTimerRunInfo(futureDateTime, callback);
        timerCallBack.RegisteredTimer = OneOffTimer;
        RegisterCallback(timerCallBack);
        CheckNextOneOffLaunchTimeStillCorrect(timerCallBack);
        var timerUpdate = Recycler!.Borrow<OneOffTimerUpdate>();
        timerCallBack.IncrementRefCount();
        timerUpdate.CallBackRunInfo = timerCallBack;
        timerUpdate.UpdateableTimer = this;
        return timerUpdate;
    }

    public ITimerUpdate RunAt<T>(DateTime futureDateTime, T state, Action<IScheduleActualTime<T>?> callback) where T : class
    {
        if (isDead) throw new InvalidAsynchronousStateException("Timer was stopped and should be used again!");
        var timerCallBack
            = GetScheduledActualActionStateOneOffTimerRunInfo(futureDateTime, callback, state);
        timerCallBack.RegisteredTimer = OneOffTimer;
        RegisterCallback(timerCallBack);
        CheckNextOneOffLaunchTimeStillCorrect(timerCallBack);
        var timerUpdate = Recycler!.Borrow<OneOffTimerUpdate>();
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
        timerCallBack.RegisteredTimer = OneOffTimer;
        RegisterCallback(timerCallBack);
        CheckNextOneOffLaunchTimeStillCorrect(timerCallBack);
        var timerUpdate = Recycler!.Borrow<OneOffTimerUpdate>();
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
            var nextTickTimeSpan  = nextTimerTickTime < now ? TimeSpan.Zero : nextTimerTickTime - now;
            intvlCallback.RegisteredTimer.Change(nextTickTimeSpan, intvlCallback.IntervalPeriodTimeSpan);
        }
    }

    public void StopAllTimers()
    {
        pauseAll = false;
        isDead   = true;
        OneOffTimer.Dispose();
        foreach (var oneOffCallback in oneOffCallbacks) oneOffCallback.DecrementRefCount();
        oneOffCallbacks.Clear();
        foreach (var intvlCallback in intervalCallBacks)
        {
            intvlCallback.RegisteredTimer.Dispose();
            intvlCallback.DecrementRefCount();
        }

        intervalCallBacks.Clear();
    }

    public ITimerCallBackRunInfo? RunNextScheduledOneOffCallbackNowOnThreadPool()
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

    public ITimerCallBackRunInfo? RunNextScheduledOneOffCallbackNow()
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

    public ITimerCallBackRunInfo? RunNextScheduledIntervalCallbackNowOnThreadPool()
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

    public ITimerCallBackRunInfo? RunNextScheduledIntervalCallbackNow()
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

    public void CheckNextOneOffLaunchTimeStillCorrect(ITimerCallBackRunInfo changed)
    {
        if (isDead) throw new InvalidAsynchronousStateException("Timer was stopped and should be used again!");
        if (pauseAll) return;
        var indxBeforeSort = oneOffCallbacks.IndexOf(changed);
        SortOneOfTimerTicks();

        if (indxBeforeSort == 0 || nextOneOffTimerTickDateTime >= changed.NextScheduleTime) UpdateNextOneOffTimerTick(oneOffCallbacks.Any());
    }

    public bool Remove(ITimerCallBackRunInfo callBackRunInfo)
    {
        if (isDead) throw new InvalidAsynchronousStateException("Timer was stopped and should be used again!");
        for (var i = 0; i < oneOffCallbacks.Count; i++)
        {
            var timerCallBackRunInfo = oneOffCallbacks[i];
            if (timerCallBackRunInfo == callBackRunInfo)
            {
                oneOffCallbacks.Remove(timerCallBackRunInfo);
                timerCallBackRunInfo.DecrementRefCount();
                CheckNextOneOffLaunchTimeStillCorrect(timerCallBackRunInfo);
                return true;
            }
        }

        for (var i = 0; i < intervalCallBacks.Count; i++)
        {
            var timerCallBackRunInfo = intervalCallBacks[i];
            if (timerCallBackRunInfo == callBackRunInfo)
            {
                intervalCallBacks.RemoveAt(i);
                timerCallBackRunInfo.RegisteredTimer.Dispose();
                timerCallBackRunInfo.DecrementRefCount();
                return true;
            }
        }

        return false;
    }

    protected virtual ITimer CreateOneOffTimer() => new Timer(OneOffTimerTicker, this, Timeout.Infinite, Timeout.Infinite);

    protected virtual ITimer CreateIntervalTimer(TimerCallBackRunInfo timerCallBack, TimeSpan periodTimeSpan) =>
        new Timer(IntervalTimerTicker, timerCallBack, periodTimeSpan, periodTimeSpan);

    private void UpdateNextOneOffTimerTick(bool enable)
    {
        var heatBeatCheck = !enable ? MaxTimerSpan : heartBeatCheck;
        var now           = TimeContext.UtcNow;
        nextOneOffTimerTickDateTime = !enable ? now + MaxTimerSpan : NextScheduledOneOffTimerTick;
        var      nextTick = nextOneOffTimerTickDateTime > now ? nextOneOffTimerTickDateTime - now : TimeSpan.Zero;
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

        OneOffTimer.Change(rangeCheckNextTick, heatBeatCheck);
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

    private TimerCallBackRunInfo GetWaitCallbackIntervalTimerRunInfo
        (TimeSpan intervalPeriod, WaitCallback callback, object? state = null)
    {
        var callBackInfo = Recycler!.Borrow<WaitCallbackTimerCallBackRunInfo>();
        callBackInfo.WaitCallback = callback;
        callBackInfo.State        = state;
        return ConfigureIntervalTimerRunInfo(intervalPeriod, callBackInfo);
    }

    private TimerCallBackRunInfo GetActionIntervalTimerRunInfo
        (TimeSpan intervalPeriod, Action callback)
    {
        var callBackInfo = Recycler!.Borrow<ActionTimerCallBackRunInfo>();
        callBackInfo.Action = callback;
        return ConfigureIntervalTimerRunInfo(intervalPeriod, callBackInfo);
    }

    private TimerCallBackRunInfo GetScheduledActualActionIntervalTimerRunInfo
        (TimeSpan intervalPeriod, Action<IScheduleActualTime?> callback)
    {
        var callBackInfo = Recycler!.Borrow<ScheduledActualActionStateTimerCallBackRunInfo>();
        callBackInfo.Action = callback;
        return ConfigureIntervalTimerRunInfo(intervalPeriod, callBackInfo);
    }

    private TimerCallBackRunInfo GetActionStateIntervalTimerRunInfo<T>
        (TimeSpan intervalPeriod, Action<T?> callback, T? state) where T : class
    {
        var callBackInfo = Recycler!.Borrow<ActionStateTimerCallBackRunInfo<T>>();
        callBackInfo.Action = callback;
        callBackInfo.State  = state;
        return ConfigureIntervalTimerRunInfo(intervalPeriod, callBackInfo);
    }

    private TimerCallBackRunInfo GetScheduledActualActionStateIntervalTimerRunInfo<T>
        (TimeSpan intervalPeriod, Action<IScheduleActualTime<T>?> callback, T? state) where T : class
    {
        var callBackInfo = Recycler!.Borrow<ScheduledActualActionStateTimerCallBackRunInfo<T>>();
        callBackInfo.Action    = callback;
        callBackInfo.SendState = state;
        return ConfigureIntervalTimerRunInfo(intervalPeriod, callBackInfo);
    }

    private TimerCallBackRunInfo GetValueTaskActionIntervalTimerRunInfo
        (TimeSpan intervalPeriod, Func<ValueTask> callback)
    {
        var callBackInfo = Recycler!.Borrow<ValueTaskActionTimerCallBackRunInfo>();
        callBackInfo.ValueTaskAction = callback;
        return ConfigureIntervalTimerRunInfo(intervalPeriod, callBackInfo);
    }

    private TimerCallBackRunInfo GetScheduledActualValueTaskValueTaskActionIntervalTimerRunInfo
        (TimeSpan intervalPeriod, Func<IScheduleActualTime?, ValueTask> callback)
    {
        var callBackInfo = Recycler!.Borrow<ScheduledActualValueTaskActionTimerCallBackRunInfo>();
        callBackInfo.ValueTaskActionState = callback;
        return ConfigureIntervalTimerRunInfo(intervalPeriod, callBackInfo);
    }

    private TimerCallBackRunInfo GetValueTaskActionStateIntervalTimerRunInfo<T>
        (TimeSpan intervalPeriod, Func<T?, ValueTask> callback, T? state) where T : class
    {
        var callBackInfo = Recycler!.Borrow<ValueTaskActionStateTimerCallBackRunInfo<T>>();
        callBackInfo.ValueTaskActionState = callback;
        callBackInfo.State                = state;
        return ConfigureIntervalTimerRunInfo(intervalPeriod, callBackInfo);
    }

    private TimerCallBackRunInfo GetScheduledActualValueTaskActionStateIntervalTimerRunInfo<T>
        (TimeSpan intervalPeriod, Func<IScheduleActualTime<T>?, ValueTask> callback, T? state) where T : class
    {
        var callBackInfo = Recycler!.Borrow<ScheduledActualValueTaskActionStateTimerCallBackRunInfo<T>>();
        callBackInfo.ValueTaskActionState = callback;
        callBackInfo.SendState            = state;
        return ConfigureIntervalTimerRunInfo(intervalPeriod, callBackInfo);
    }

    private TimerCallBackRunInfo ConfigureIntervalTimerRunInfo(TimeSpan intervalPeriod, TimerCallBackRunInfo callBackInfo)
    {
        callBackInfo.CurrentNumberOfCalls = 0;
        var firstScheduledTime = TimeContext.UtcNow + intervalPeriod;
        callBackInfo.FirstScheduledTime     = firstScheduledTime;
        callBackInfo.IsPaused               = pauseAll;
        callBackInfo.LastRunTime            = DateTime.MinValue;
        callBackInfo.MaxNumberOfCalls       = int.MaxValue;
        callBackInfo.NextScheduleTime       = firstScheduledTime;
        callBackInfo.IntervalPeriodTimeSpan = intervalPeriod;
        return callBackInfo;
    }

    private TimerCallBackRunInfo GetWaitCallbackOneOffTimerRunInfo
        (DateTime runAt, WaitCallback callback, object? state = null)
    {
        var callBackInfo = Recycler!.Borrow<WaitCallbackTimerCallBackRunInfo>();
        callBackInfo.WaitCallback = callback;
        callBackInfo.State        = state;
        return ConfigureOneOffTimerRunInfo(runAt, callBackInfo);
    }

    private TimerCallBackRunInfo GetActionOneOffTimerRunInfo(DateTime runAt, Action callback)
    {
        var callBackInfo = Recycler!.Borrow<ActionTimerCallBackRunInfo>();
        callBackInfo.Action = callback;
        return ConfigureOneOffTimerRunInfo(runAt, callBackInfo);
    }

    private TimerCallBackRunInfo GetScheduledActualActionOneOffTimerRunInfo(DateTime runAt, Action<IScheduleActualTime?> callback)
    {
        var callBackInfo = Recycler!.Borrow<ScheduledActualActionStateTimerCallBackRunInfo<IScheduleActualTime>>();
        callBackInfo.Action = callback;
        return ConfigureOneOffTimerRunInfo(runAt, callBackInfo);
    }

    private TimerCallBackRunInfo GetActionStateOneOffTimerRunInfo<T>
        (DateTime runAt, Action<T?> callback, T? state) where T : class
    {
        var callBackInfo = Recycler!.Borrow<ActionStateTimerCallBackRunInfo<T>>();
        callBackInfo.Action = callback;
        callBackInfo.State  = state;
        return ConfigureOneOffTimerRunInfo(runAt, callBackInfo);
    }

    private TimerCallBackRunInfo GetScheduledActualActionStateOneOffTimerRunInfo<T>
        (DateTime runAt, Action<IScheduleActualTime<T>?> callback, T? state) where T : class
    {
        var callBackInfo = Recycler!.Borrow<ScheduledActualActionStateTimerCallBackRunInfo<T>>();
        callBackInfo.Action    = callback;
        callBackInfo.SendState = state;
        return ConfigureOneOffTimerRunInfo(runAt, callBackInfo);
    }

    private TimerCallBackRunInfo GetValueTaskActionOneOffTimerRunInfo(DateTime runAt, Func<ValueTask> callback)
    {
        var callBackInfo = Recycler!.Borrow<ValueTaskActionTimerCallBackRunInfo>();
        callBackInfo.ValueTaskAction = callback;
        return ConfigureOneOffTimerRunInfo(runAt, callBackInfo);
    }

    private TimerCallBackRunInfo GetValueTaskActionStateOneOffTimerRunInfo<T>
        (DateTime runAt, Func<T?, ValueTask> callback, T? state) where T : class
    {
        var callBackInfo = Recycler!.Borrow<ValueTaskActionStateTimerCallBackRunInfo<T>>();
        callBackInfo.ValueTaskActionState = callback;
        callBackInfo.State                = state;
        return ConfigureOneOffTimerRunInfo(runAt, callBackInfo);
    }

    private TimerCallBackRunInfo GetScheduledActualValueTaskActionStateOneOffTimerRunInfo<T>
        (DateTime runAt, Func<IScheduleActualTime<T>?, ValueTask> callback, T? state) where T : class
    {
        var callBackInfo = Recycler!.Borrow<ScheduledActualValueTaskActionStateTimerCallBackRunInfo<T>>();
        callBackInfo.ValueTaskActionState = callback;
        callBackInfo.SendState            = state;
        return ConfigureOneOffTimerRunInfo(runAt, callBackInfo);
    }

    private TimerCallBackRunInfo GetScheduledActualValueTaskActionOneOffTimerRunInfo
        (DateTime runAt, Func<IScheduleActualTime?, ValueTask> callback)
    {
        var callBackInfo = Recycler!.Borrow<ScheduledActualValueTaskActionStateTimerCallBackRunInfo<IScheduleActualTime>>();
        callBackInfo.ValueTaskActionState = callback;
        return ConfigureOneOffTimerRunInfo(runAt, callBackInfo);
    }

    private TimerCallBackRunInfo ConfigureOneOffTimerRunInfo(DateTime runAt, TimerCallBackRunInfo callBackInfo)
    {
        callBackInfo.CurrentNumberOfCalls   = 0;
        callBackInfo.FirstScheduledTime     = runAt;
        callBackInfo.IsPaused               = pauseAll;
        callBackInfo.LastRunTime            = DateTime.MinValue;
        callBackInfo.MaxNumberOfCalls       = 1;
        callBackInfo.NextScheduleTime       = runAt;
        callBackInfo.IntervalPeriodTimeSpan = MaxTimerSpan;
        return callBackInfo;
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    protected void OneOffTimerTicker(object? stateInfo)
    {
        if (pauseAll) return;
        var now = TimeContext.UtcNow;
        try
        {
            oneOffSpinLock.Acquire();
            for (var i = 0; i < oneOffCallbacks.Count; i++)
            {
                var currentCallback = oneOffCallbacks[i];
                if (!currentCallback.IsPaused && currentCallback.NextScheduleTime <= now)
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
            if (nextScheduledOneOffTimerCallBackRunInfo != null) CheckNextOneOffLaunchTimeStillCorrect(nextScheduledOneOffTimerCallBackRunInfo);
        }
    }

    protected void IntervalTimerTicker(object? stateInfo)
    {
        if (pauseAll) return;
        if (stateInfo is TimerCallBackRunInfo { IsPaused: false } intervalTimer) intervalTimer.RunCallbackOnThisThread();
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
