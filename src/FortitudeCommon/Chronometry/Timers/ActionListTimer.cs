﻿#region

using FortitudeCommon.AsyncProcessing;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;

#endregion

namespace FortitudeCommon.Chronometry.Timers;

public interface ITimerCallbackPayload : IRecyclableObject
{
    void Invoke();
}

public class TimerCallbackPayload<T> : ReusableObject<TimerCallbackPayload<T>>, ITimerCallbackPayload where T : class
{
    public TimerCallbackPayload() { }

    private TimerCallbackPayload(TimerCallbackPayload<T> toClone)
    {
        // ReSharper disable once VirtualMemberCallInConstructor
        CopyFrom(toClone);
    }

    public Action? Action { get; set; }
    public Action<T?>? ActionState { get; set; }
    public T? State { get; set; }

    public void Invoke()
    {
        if (Action != null && ActionState != null)
            throw new InvalidDataException("Only expect Action or ActionState to be set");
        if (Action != null)
            Action();
        else if (ActionState != null)
            ActionState(State);
        else
            throw new InvalidDataException("Expected either Action or ActionState to be set");
    }

    public override void StateReset()
    {
        Action = null;
        ActionState = null;
        State = null;
        base.StateReset();
    }

    public override TimerCallbackPayload<T> CopyFrom(TimerCallbackPayload<T> source
        , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        Action = source.Action;
        ActionState = source.ActionState;
        State = source.State;
        return this;
    }

    public override TimerCallbackPayload<T> Clone() =>
        Recycler?.Borrow<TimerCallbackPayload<T>>().CopyFrom(this) ?? new TimerCallbackPayload<T>(this);
}

public class ActionTimer : IActionTimer
{
    private readonly IUpdateableTimer backingTimer;
    public readonly IRecycler Recycler;

    public ActionTimer(IUpdateableTimer backingTimer, IRecycler recycler)
    {
        this.backingTimer = backingTimer;
        Recycler = recycler;
    }

    public WaitCallback IntervalWaitCallback { get; set; } = null!;
    public WaitCallback OneOffWaitCallback { get; set; } = null!;

    public ITimerUpdate RunIn(TimeSpan waitTimeSpan, Action callback)
    {
        var actionPayload = Recycler.Borrow<TimerCallbackPayload<object>>();
        actionPayload.ActionState = null;
        actionPayload.State = null;
        actionPayload.Action = callback;
        return backingTimer.RunIn(waitTimeSpan, actionPayload, OneOffWaitCallback);
    }

    public ITimerUpdate RunIn<T>(TimeSpan waitTimeSpan, T state, Action<T?> callback) where T : class
    {
        var actionPayload = Recycler.Borrow<TimerCallbackPayload<T>>();
        actionPayload.ActionState = callback;
        actionPayload.State = state;
        actionPayload.Action = null;
        return backingTimer.RunIn(waitTimeSpan, actionPayload, OneOffWaitCallback);
    }

    public ITimerUpdate RunIn(int waitMs, Action callback)
    {
        var actionPayload = Recycler.Borrow<TimerCallbackPayload<object>>();
        actionPayload.ActionState = null;
        actionPayload.State = null;
        actionPayload.Action = callback;
        return backingTimer.RunIn(waitMs, actionPayload, OneOffWaitCallback);
    }

    public ITimerUpdate RunIn<T>(int waitMs, T state, Action<T?> callback) where T : class
    {
        var actionPayload = Recycler.Borrow<TimerCallbackPayload<T>>();
        actionPayload.ActionState = callback;
        actionPayload.State = state;
        actionPayload.Action = null;
        return backingTimer.RunIn(waitMs, actionPayload, OneOffWaitCallback);
    }

    public ITimerUpdate RunEvery(int intervalMs, Action callback)
    {
        var actionPayload = Recycler.Borrow<TimerCallbackPayload<object>>();
        actionPayload.ActionState = null;
        actionPayload.State = null;
        actionPayload.Action = callback;
        return backingTimer.RunIn(intervalMs, actionPayload, IntervalWaitCallback);
    }

    public ITimerUpdate RunEvery<T>(int intervalMs, T state, Action<T?> callback) where T : class
    {
        var actionPayload = Recycler.Borrow<TimerCallbackPayload<T>>();
        actionPayload.ActionState = callback;
        actionPayload.State = state;
        actionPayload.Action = null;
        return backingTimer.RunIn(intervalMs, actionPayload, IntervalWaitCallback);
    }

    public ITimerUpdate RunEvery(TimeSpan periodTimeSpan, Action callback)
    {
        var actionPayload = Recycler.Borrow<TimerCallbackPayload<object>>();
        actionPayload.ActionState = null;
        actionPayload.State = null;
        actionPayload.Action = callback;
        return backingTimer.RunIn(periodTimeSpan, actionPayload, IntervalWaitCallback);
    }

    public ITimerUpdate RunEvery<T>(TimeSpan periodTimeSpan, T state, Action<T?> callback) where T : class
    {
        var actionPayload = Recycler.Borrow<TimerCallbackPayload<T>>();
        actionPayload.ActionState = callback;
        actionPayload.State = state;
        actionPayload.Action = null;
        return backingTimer.RunIn(periodTimeSpan, actionPayload, IntervalWaitCallback);
    }

    public ITimerUpdate RunAt(DateTime futureDateTime, Action callback)
    {
        var actionPayload = Recycler.Borrow<TimerCallbackPayload<object>>();
        actionPayload.ActionState = null;
        actionPayload.State = null;
        actionPayload.Action = callback;
        return backingTimer.RunAt(futureDateTime, actionPayload, OneOffWaitCallback);
    }

    public ITimerUpdate RunAt<T>(DateTime futureDateTime, T state, Action<T?> callback) where T : class
    {
        var actionPayload = Recycler.Borrow<TimerCallbackPayload<T>>();
        actionPayload.ActionState = callback;
        actionPayload.State = state;
        actionPayload.Action = null;
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

public class ActionListTimer : ActionTimer
{
    private List<ITimerCallbackPayload> processingCallbacks = new();
    private ISyncLock syncLock = new SpinLockLight();
    private List<ITimerCallbackPayload> unprocessedCallbacks = new();

    public ActionListTimer(IUpdateableTimer backingTimer, IRecycler recycler)
        : base(backingTimer, recycler)
    {
        OneOffWaitCallback = OneOffTimerAddUnprocessedAction;
        IntervalWaitCallback = IntervalTimerAddUnprocessedAction;
    }

    public void OneOffTimerAddUnprocessedAction(object? state)
    {
        if (state is ITimerCallbackPayload timerCallbackPayload)
        {
            syncLock.Acquire();
            try
            {
                unprocessedCallbacks.Add(timerCallbackPayload);
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
                unprocessedCallbacks.Add(timerCallbackPayload);
            }
            finally
            {
                syncLock.Release();
            }
        }
    }

    public void GetTimerActionsToExecute(List<ITimerCallbackPayload> toPopulate)
    {
        toPopulate.Clear();
        syncLock.Acquire();
        try
        {
            (unprocessedCallbacks, processingCallbacks) = (processingCallbacks, unprocessedCallbacks);
            unprocessedCallbacks.Clear();
            toPopulate.AddRange(processingCallbacks);
        }
        finally
        {
            syncLock.Release();
        }
    }
}