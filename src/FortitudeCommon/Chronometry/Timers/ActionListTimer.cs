// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.AsyncProcessing;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Types;

#endregion

namespace FortitudeCommon.Chronometry.Timers;

public interface ITimerCallbackPayload : IRecyclableObject
{
    bool      IsAsyncInvoke();
    ValueTask InvokeAsync();
    void      Invoke();
}

public class TimerCallbackPayload<T> : ReusableObject<TimerCallbackPayload<T>>, ITimerCallbackPayload where T : class
{
    public TimerCallbackPayload() { }

    private TimerCallbackPayload(TimerCallbackPayload<T> toClone)
    {
        // ReSharper disable once VirtualMemberCallInConstructor
        CopyFrom(toClone);
    }

    public Action?     Action      { get; set; }
    public Action<T?>? ActionState { get; set; }

    public Func<ValueTask>?     ValueTaskAction      { get; set; }
    public Func<T?, ValueTask>? ValueTaskActionState { get; set; }

    public T? State { get; set; }

    public bool IsAsyncInvoke() => ValueTaskAction != null || ValueTaskActionState != null;

    public async ValueTask InvokeAsync()
    {
        if (ValueTaskAction != null && ValueTaskActionState != null)
            throw new InvalidDataException("Only expect ValueTaskAction or ValueTaskState to be set");
        if (ValueTaskAction != null)
            await ValueTaskAction();
        else if (ValueTaskActionState != null)
            await ValueTaskActionState(State);
        else
            throw new InvalidDataException("Expected either Action or ActionState to be set");
    }

    public void Invoke()
    {
        if (Action != null && ActionState != null) throw new InvalidDataException("Only expect Action or ActionState to be set");
        if (Action != null)
            Action();
        else if (ActionState != null)
            ActionState(State);
        else
            throw new InvalidDataException("Expected either Action or ActionState to be set");
    }

    public override void StateReset()
    {
        ValueTaskAction      = null;
        ValueTaskActionState = null;

        Action      = null;
        ActionState = null;
        State       = null;
        base.StateReset();
    }

    public override TimerCallbackPayload<T> CopyFrom
    (TimerCallbackPayload<T> source
      , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        ValueTaskAction      = source.ValueTaskAction;
        ValueTaskActionState = source.ValueTaskActionState;

        Action      = source.Action;
        ActionState = source.ActionState;
        State       = source.State;
        return this;
    }

    public override TimerCallbackPayload<T> Clone() =>
        Recycler?.Borrow<TimerCallbackPayload<T>>().CopyFrom(this) ?? new TimerCallbackPayload<T>(this);
}

public interface IScheduledActualTimerCallbackPayload : ITimerCallbackPayload
{
    IScheduleActualTime ScheduleActualTime { get; }
}

public class ScheduledActualTimerStateCallbackPayload<T> : ReusableObject<ScheduledActualTimerStateCallbackPayload<T>>
  , IScheduledActualTimerCallbackPayload, ICaptureTimesState
    where T : class
{
    private int callCount;
    public ScheduledActualTimerStateCallbackPayload() { }

    private ScheduledActualTimerStateCallbackPayload(ScheduledActualTimerStateCallbackPayload<T> toClone)
    {
        // ReSharper disable once VirtualMemberCallInConstructor
        CopyFrom(toClone);
    }

    public Action<IScheduleActualTime<T>?>? Action      { get; set; }
    public Action<IScheduleActualTime<T>?>? ActionState { get; set; }

    public Func<IScheduleActualTime<T>?, ValueTask>? ValueTaskAction      { get; set; }
    public Func<IScheduleActualTime<T>?, ValueTask>? ValueTaskActionState { get; set; }

    public T? SendState { get; set; }

    public IScheduleActualTime<T>? State { get; set; }

    public ITimerUpdate? TimerUpdate { get; set; }

    public void CaptureTriggerAndScheduleTime()
    {
        var scheduleActualState = Recycler?.Borrow<ScheduleActualTime<T>>() ?? new ScheduleActualTime<T>();
        scheduleActualState.Configure(TimerUpdate!, TimerUpdate!.NextScheduleDateTime, TimeContext.UtcNow, callCount++);
        if (SendState != null) scheduleActualState.State = SendState;
        State = scheduleActualState;
    }

    public IScheduleActualTime ScheduleActualTime => State!;

    public bool IsAsyncInvoke() => ValueTaskAction != null || ValueTaskActionState != null;

    public async ValueTask InvokeAsync()
    {
        if (ValueTaskAction != null && ValueTaskActionState != null)
            throw new InvalidDataException("Only expect ValueTaskAction or ValueTaskState to be set");
        if (ValueTaskAction != null)
        {
            await ValueTaskAction(State);
        }
        else if (ValueTaskActionState != null)
        {
            if (State != null) State.State = SendState;
            await ValueTaskActionState(State);
        }
        else
        {
            throw new InvalidDataException("Expected either Action or ActionState to be set");
        }
    }

    public void Invoke()
    {
        if (Action != null && ActionState != null) throw new InvalidDataException("Only expect Action or ActionState to be set");
        if (Action != null)
        {
            Action(State);
        }
        else if (ActionState != null)
        {
            if (State != null) State.State = SendState;
            ActionState(State);
        }
        else
        {
            throw new InvalidDataException("Expected either Action or ActionState to be set");
        }
    }

    public override void StateReset()
    {
        ValueTaskAction      = null;
        ValueTaskActionState = null;

        callCount   = 0;
        Action      = null;
        ActionState = null;
        State       = null;
        base.StateReset();
    }

    public override ScheduledActualTimerStateCallbackPayload<T> CopyFrom
    (ScheduledActualTimerStateCallbackPayload<T> source
      , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        ValueTaskAction      = source.ValueTaskAction;
        ValueTaskActionState = source.ValueTaskActionState;

        callCount   = source.callCount;
        Action      = source.Action;
        ActionState = source.ActionState;
        State       = source.State;
        return this;
    }

    public override ScheduledActualTimerStateCallbackPayload<T> Clone() =>
        Recycler?.Borrow<ScheduledActualTimerStateCallbackPayload<T>>().CopyFrom(this) ?? new ScheduledActualTimerStateCallbackPayload<T>(this);
}

public class ScheduledActualTimerStateCallbackPayload : ReusableObject<ScheduledActualTimerStateCallbackPayload>, IScheduledActualTimerCallbackPayload
  , ICaptureTimesState
{
    private int callCount;
    public ScheduledActualTimerStateCallbackPayload() { }

    private ScheduledActualTimerStateCallbackPayload(ScheduledActualTimerStateCallbackPayload toClone)
    {
        // ReSharper disable once VirtualMemberCallInConstructor
        CopyFrom(toClone);
    }

    public Action<IScheduleActualTime?>? Action      { get; set; }
    public Action<IScheduleActualTime?>? ActionState { get; set; }

    public Func<IScheduleActualTime?, ValueTask>? ValueTaskAction      { get; set; }
    public Func<IScheduleActualTime?, ValueTask>? ValueTaskActionState { get; set; }

    public IScheduleActualTime? State { get; set; }

    public ITimerUpdate? TimerUpdate { get; set; }

    public void CaptureTriggerAndScheduleTime()
    {
        var scheduleActualState = Recycler?.Borrow<ScheduleActualTime>() ?? new ScheduleActualTime();
        scheduleActualState.Configure(TimerUpdate!, TimerUpdate!.NextScheduleDateTime, TimeContext.UtcNow, callCount++);
        State = scheduleActualState;
    }

    public IScheduleActualTime ScheduleActualTime => State!;

    public bool IsAsyncInvoke() => ValueTaskAction != null || ValueTaskActionState != null;

    public async ValueTask InvokeAsync()
    {
        if (ValueTaskAction != null && ValueTaskActionState != null)
            throw new InvalidDataException("Only expect ValueTaskAction or ValueTaskState to be set");
        if (ValueTaskAction != null)
            await ValueTaskAction(State);
        else if (ValueTaskActionState != null)
            await ValueTaskActionState(State);
        else
            throw new InvalidDataException("Expected either Action or ActionState to be set");
    }

    public void Invoke()
    {
        if (Action != null && ActionState != null) throw new InvalidDataException("Only expect Action or ActionState to be set");
        if (Action != null)
            Action(State);
        else if (ActionState != null)
            ActionState(State);
        else
            throw new InvalidDataException("Expected either Action or ActionState to be set");
    }

    public override void StateReset()
    {
        ValueTaskAction      = null;
        ValueTaskActionState = null;

        callCount   = 0;
        Action      = null;
        ActionState = null;
        State       = null;
        base.StateReset();
    }

    public override ScheduledActualTimerStateCallbackPayload CopyFrom
    (ScheduledActualTimerStateCallbackPayload source
      , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        ValueTaskAction      = source.ValueTaskAction;
        ValueTaskActionState = source.ValueTaskActionState;

        callCount   = source.callCount;
        Action      = source.Action;
        ActionState = source.ActionState;
        State       = source.State;
        return this;
    }

    public override ScheduledActualTimerStateCallbackPayload Clone() =>
        Recycler?.Borrow<ScheduledActualTimerStateCallbackPayload>().CopyFrom(this) ?? new ScheduledActualTimerStateCallbackPayload(this);
}

public interface ISingleEntryActionTimer : IActionTimer
{
    WaitCallback IntervalWaitCallback { get; set; }
    WaitCallback OneOffWaitCallback   { get; set; }
}

public class ActionTimer : ISingleEntryActionTimer
{
    private readonly IUpdateableTimer backingTimer;
    public readonly  IRecycler        Recycler;

    public ActionTimer(IUpdateableTimer backingTimer, IRecycler recycler)
    {
        this.backingTimer = backingTimer;
        Recycler          = recycler;
    }

    public WaitCallback IntervalWaitCallback { get; set; } = null!;
    public WaitCallback OneOffWaitCallback   { get; set; } = null!;


    public ITimerUpdate RunIn(TimeSpan waitTimeSpan, Func<ValueTask> callback)
    {
        var actionPayload = Recycler.Borrow<TimerCallbackPayload<object>>();
        actionPayload.ValueTaskAction = callback;
        return backingTimer.RunIn(waitTimeSpan, actionPayload, OneOffWaitCallback);
    }

    public ITimerUpdate RunIn<T>(TimeSpan waitTimeSpan, T state, Func<T?, ValueTask> callback) where T : class
    {
        var actionPayload = Recycler.Borrow<TimerCallbackPayload<T>>();
        actionPayload.ValueTaskActionState = callback;
        actionPayload.State                = state;
        return backingTimer.RunIn(waitTimeSpan, actionPayload, OneOffWaitCallback);
    }

    public ITimerUpdate RunIn(TimeSpan waitTimeSpan, Func<IScheduleActualTime?, ValueTask> callback)
    {
        var actionPayload = Recycler.Borrow<ScheduledActualTimerStateCallbackPayload>();
        actionPayload.ValueTaskActionState = callback;
        return backingTimer.RunIn(waitTimeSpan, actionPayload, OneOffWaitCallback);
    }

    public ITimerUpdate RunIn<T>(TimeSpan waitTimeSpan, T state, Func<IScheduleActualTime<T>?, ValueTask> callback) where T : class
    {
        var actionPayload = Recycler.Borrow<ScheduledActualTimerStateCallbackPayload<T>>();
        actionPayload.ValueTaskActionState = callback;
        actionPayload.SendState            = state;
        return backingTimer.RunIn(waitTimeSpan, actionPayload, OneOffWaitCallback);
    }

    public ITimerUpdate RunIn(int waitMs, Func<IScheduleActualTime?, ValueTask> callback) => RunIn(TimeSpan.FromMilliseconds(waitMs), callback);

    public ITimerUpdate RunIn<T>
        (int waitMs, T state, Func<IScheduleActualTime<T>?, ValueTask> callback) where T : class =>
        RunIn(TimeSpan.FromMilliseconds(waitMs), state, callback);

    public ITimerUpdate RunIn(int waitMs, Func<ValueTask> callback) => RunIn(TimeSpan.FromMilliseconds(waitMs), callback);

    public ITimerUpdate RunIn<T>
        (int waitMs, T state, Func<T?, ValueTask> callback) where T : class =>
        RunIn(TimeSpan.FromMilliseconds(waitMs), state, callback);


    public ITimerUpdate RunEvery(TimeSpan periodTimeSpan, Func<ValueTask> callback)
    {
        var actionPayload = Recycler.Borrow<TimerCallbackPayload<object>>();
        actionPayload.ValueTaskAction = callback;
        return backingTimer.RunEvery(periodTimeSpan, actionPayload, IntervalWaitCallback);
    }

    public ITimerUpdate RunEvery<T>(TimeSpan periodTimeSpan, T state, Func<T?, ValueTask> callback) where T : class
    {
        var actionPayload = Recycler.Borrow<TimerCallbackPayload<T>>();
        actionPayload.ValueTaskActionState = callback;
        actionPayload.State                = state;
        return backingTimer.RunEvery(periodTimeSpan, actionPayload, IntervalWaitCallback);
    }

    public ITimerUpdate RunEvery(TimeSpan periodTimeSpan, Func<IScheduleActualTime?, ValueTask> callback)
    {
        var actionPayload = Recycler.Borrow<ScheduledActualTimerStateCallbackPayload>();
        actionPayload.ValueTaskAction = callback;
        return backingTimer.RunEvery(periodTimeSpan, actionPayload, IntervalWaitCallback);
    }

    public ITimerUpdate RunEvery<T>(TimeSpan periodTimeSpan, T state, Func<IScheduleActualTime<T>?, ValueTask> callback) where T : class
    {
        var actionPayload = Recycler.Borrow<ScheduledActualTimerStateCallbackPayload<T>>();
        actionPayload.ValueTaskActionState = callback;
        actionPayload.SendState            = state;
        return backingTimer.RunEvery(periodTimeSpan, actionPayload, IntervalWaitCallback);
    }

    public ITimerUpdate RunEvery
        (int intervalMs, Func<IScheduleActualTime?, ValueTask> callback) =>
        RunEvery(TimeSpan.FromMilliseconds(intervalMs), callback);

    public ITimerUpdate RunEvery<T>
        (int intervalMs, T state, Func<IScheduleActualTime<T>?, ValueTask> callback) where T : class =>
        RunEvery(TimeSpan.FromMilliseconds(intervalMs), state, callback);

    public ITimerUpdate RunEvery(int intervalMs, Func<ValueTask> callback) => RunEvery(TimeSpan.FromMilliseconds(intervalMs), callback);

    public ITimerUpdate RunEvery<T>
        (int intervalMs, T state, Func<T?, ValueTask> callback) where T : class =>
        RunEvery(TimeSpan.FromMilliseconds(intervalMs), state, callback);

    public ITimerUpdate RunAt(DateTime futureDateTime, Func<ValueTask> callback)
    {
        var actionPayload = Recycler.Borrow<TimerCallbackPayload<object>>();
        actionPayload.ValueTaskAction = callback;
        return backingTimer.RunAt(futureDateTime, actionPayload, OneOffWaitCallback);
    }

    public ITimerUpdate RunAt<T>(DateTime futureDateTime, T state, Func<T?, ValueTask> callback) where T : class
    {
        var actionPayload = Recycler.Borrow<TimerCallbackPayload<T>>();
        actionPayload.ValueTaskActionState = callback;
        actionPayload.State                = state;
        return backingTimer.RunAt(futureDateTime, actionPayload, OneOffWaitCallback);
    }

    public ITimerUpdate RunAt(DateTime futureDateTime, Func<IScheduleActualTime?, ValueTask> callback)
    {
        var actionPayload = Recycler.Borrow<ScheduledActualTimerStateCallbackPayload>();
        actionPayload.ValueTaskAction = callback;
        return backingTimer.RunAt(futureDateTime, actionPayload, OneOffWaitCallback);
    }

    public ITimerUpdate RunAt<T>(DateTime futureDateTime, T state, Func<IScheduleActualTime<T>?, ValueTask> callback) where T : class
    {
        var actionPayload = Recycler.Borrow<ScheduledActualTimerStateCallbackPayload<T>>();
        actionPayload.ValueTaskActionState = callback;
        actionPayload.SendState            = state;
        return backingTimer.RunAt(futureDateTime, actionPayload, OneOffWaitCallback);
    }

    public ITimerUpdate RunIn(TimeSpan waitTimeSpan, Action callback)
    {
        var actionPayload = Recycler.Borrow<TimerCallbackPayload<object>>();
        actionPayload.Action = callback;
        return backingTimer.RunIn(waitTimeSpan, actionPayload, OneOffWaitCallback);
    }

    public ITimerUpdate RunIn<T>(TimeSpan waitTimeSpan, T state, Action<T?> callback) where T : class
    {
        var actionPayload = Recycler.Borrow<TimerCallbackPayload<T>>();
        actionPayload.ActionState = callback;
        actionPayload.State       = state;
        return backingTimer.RunIn(waitTimeSpan, actionPayload, OneOffWaitCallback);
    }

    public ITimerUpdate RunIn(TimeSpan waitTimeSpan, Action<IScheduleActualTime?> callback)
    {
        var actionPayload = Recycler.Borrow<ScheduledActualTimerStateCallbackPayload>();
        actionPayload.ActionState = callback;
        return backingTimer.RunIn(waitTimeSpan, actionPayload, OneOffWaitCallback);
    }

    public ITimerUpdate RunIn<T>(TimeSpan waitTimeSpan, T state, Action<IScheduleActualTime<T>?> callback) where T : class
    {
        var actionPayload = Recycler.Borrow<ScheduledActualTimerStateCallbackPayload<T>>();
        actionPayload.ActionState = callback;
        actionPayload.SendState   = state;
        return backingTimer.RunIn(waitTimeSpan, actionPayload, OneOffWaitCallback);
    }

    public ITimerUpdate RunIn(int waitMs, Action<IScheduleActualTime?> callback) => RunIn(TimeSpan.FromMilliseconds(waitMs), callback);

    public ITimerUpdate RunIn<T>
        (int waitMs, T state, Action<IScheduleActualTime<T>?> callback) where T : class =>
        RunIn(TimeSpan.FromMilliseconds(waitMs), state, callback);

    public ITimerUpdate RunIn(int waitMs, Action callback) => RunIn(TimeSpan.FromMilliseconds(waitMs), callback);

    public ITimerUpdate RunIn<T>
        (int waitMs, T state, Action<T?> callback) where T : class =>
        RunIn(TimeSpan.FromMilliseconds(waitMs), state, callback);


    public ITimerUpdate RunEvery(TimeSpan periodTimeSpan, Action callback)
    {
        var actionPayload = Recycler.Borrow<TimerCallbackPayload<object>>();
        actionPayload.Action = callback;
        return backingTimer.RunEvery(periodTimeSpan, actionPayload, IntervalWaitCallback);
    }

    public ITimerUpdate RunEvery<T>(TimeSpan periodTimeSpan, T state, Action<T?> callback) where T : class
    {
        var actionPayload = Recycler.Borrow<TimerCallbackPayload<T>>();
        actionPayload.ActionState = callback;
        actionPayload.State       = state;
        return backingTimer.RunEvery(periodTimeSpan, actionPayload, IntervalWaitCallback);
    }

    public ITimerUpdate RunEvery(TimeSpan periodTimeSpan, Action<IScheduleActualTime?> callback)
    {
        var actionPayload = Recycler.Borrow<ScheduledActualTimerStateCallbackPayload>();
        actionPayload.ActionState = callback;
        return backingTimer.RunEvery(periodTimeSpan, actionPayload, IntervalWaitCallback);
    }

    public ITimerUpdate RunEvery<T>(TimeSpan periodTimeSpan, T state, Action<IScheduleActualTime<T>?> callback) where T : class
    {
        var actionPayload = Recycler.Borrow<ScheduledActualTimerStateCallbackPayload<T>>();
        actionPayload.ActionState = callback;
        actionPayload.SendState   = state;
        return backingTimer.RunEvery(periodTimeSpan, actionPayload, IntervalWaitCallback);
    }

    public ITimerUpdate RunEvery(int intervalMs, Action<IScheduleActualTime?> callback) => RunEvery(TimeSpan.FromMilliseconds(intervalMs), callback);

    public ITimerUpdate RunEvery<T>
        (int intervalMs, T state, Action<IScheduleActualTime<T>?> callback) where T : class =>
        RunEvery(TimeSpan.FromMilliseconds(intervalMs), state, callback);

    public ITimerUpdate RunEvery(int intervalMs, Action callback) => RunEvery(TimeSpan.FromMilliseconds(intervalMs), callback);

    public ITimerUpdate RunEvery<T>
        (int intervalMs, T state, Action<T?> callback) where T : class =>
        RunEvery(TimeSpan.FromMilliseconds(intervalMs), state, callback);

    public ITimerUpdate RunAt(DateTime futureDateTime, Action callback)
    {
        var actionPayload = Recycler.Borrow<TimerCallbackPayload<object>>();
        actionPayload.Action = callback;
        return backingTimer.RunAt(futureDateTime, actionPayload, OneOffWaitCallback);
    }

    public ITimerUpdate RunAt<T>(DateTime futureDateTime, T state, Action<T?> callback) where T : class
    {
        var actionPayload = Recycler.Borrow<TimerCallbackPayload<T>>();
        actionPayload.ActionState = callback;
        actionPayload.State       = state;
        return backingTimer.RunAt(futureDateTime, actionPayload, OneOffWaitCallback);
    }

    public ITimerUpdate RunAt(DateTime futureDateTime, Action<IScheduleActualTime?> callback)
    {
        var actionPayload = Recycler.Borrow<ScheduledActualTimerStateCallbackPayload>();
        actionPayload.ActionState = callback;
        return backingTimer.RunAt(futureDateTime, actionPayload, OneOffWaitCallback);
    }

    public ITimerUpdate RunAt<T>(DateTime futureDateTime, T state, Action<IScheduleActualTime<T>?> callback) where T : class
    {
        var actionPayload = Recycler.Borrow<ScheduledActualTimerStateCallbackPayload<T>>();
        actionPayload.ActionState = callback;
        actionPayload.SendState   = state;
        return backingTimer.RunAt(futureDateTime, actionPayload, OneOffWaitCallback);
    }

    public void PauseAllTimers()
    {
        backingTimer.PauseAllTimers();
    }

    public void ResumeAllTimers()
    {
        backingTimer.ResumeAllTimers();
    }

    public virtual void StopAllTimers()
    {
        backingTimer.StopAllTimers();
    }
}

public interface IProcessListConsumer : IDisposable
{
    bool ShouldProcess { get; }

    List<ITimerCallbackPayload> TimerPayloads { get; }
}

public class ActionListTimer : ActionTimer
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(ActionListTimer));

    private readonly ShouldSkipDisposable shouldSkip = new();

    private ShouldProcessDisposable processingCallbacks;
    private ISyncLock               syncLock = new SpinLockLight();
    private ShouldProcessDisposable unprocessedCallbacks;

    public ActionListTimer(IUpdateableTimer backingTimer, IRecycler recycler)
        : base(backingTimer, recycler)
    {
        OneOffWaitCallback   = OneOffTimerAddUnprocessedAction;
        IntervalWaitCallback = IntervalTimerAddUnprocessedAction;
        processingCallbacks  = recycler.Borrow<ShouldProcessDisposable>();
        unprocessedCallbacks = recycler.Borrow<ShouldProcessDisposable>();
    }

    public void OneOffTimerAddUnprocessedAction(object? state)
    {
        if (state is ITimerCallbackPayload timerCallbackPayload)
        {
            syncLock.Acquire();
            try
            {
                unprocessedCallbacks.TimerPayloads.Add(timerCallbackPayload);
            }
            finally
            {
                syncLock.Release();
            }
        }
    }

    public void IntervalTimerAddUnprocessedAction(object? state)
    {
        if (state is ITimerCallbackPayload timerCallbackPayload)
        {
            syncLock.Acquire();
            try
            {
                unprocessedCallbacks.TimerPayloads.Add(timerCallbackPayload);
            }
            finally
            {
                syncLock.Release();
            }
        }
    }

    public IProcessListConsumer GetTimerActionsToExecute()
    {
        syncLock.Acquire();
        try
        {
            processingCallbacks = Recycler.Borrow<ShouldProcessDisposable>();
            if (@unprocessedCallbacks.TimerPayloads.Any()) return shouldSkip;
            (unprocessedCallbacks, processingCallbacks) = (processingCallbacks, unprocessedCallbacks);
            unprocessedCallbacks                        = Recycler.Borrow<ShouldProcessDisposable>();
        }
        finally
        {
            syncLock.Release();
        }

        return processingCallbacks;
    }

    public class ShouldSkipDisposable : IProcessListConsumer
    {
        public bool ShouldProcess => false;

        public List<ITimerCallbackPayload> TimerPayloads => null!;

        public void Dispose() { }
    }

    public class ShouldProcessDisposable : RecyclableObject, IProcessListConsumer
    {
        public bool ShouldProcess => TimerPayloads.Any();

        public List<ITimerCallbackPayload> TimerPayloads { get; set; } = new();

        public void Dispose()
        {
            DecrementRefCount();
        }

        public override void StateReset()
        {
            TimerPayloads.Clear();
        }
    }
}
