#region

using Fortitude.EventProcessing.BusRules.Messaging;
using Fortitude.EventProcessing.BusRules.Rules;
using FortitudeCommon.Chronometry.Timers;
using FortitudeCommon.DataStructures.Memory;

#endregion

namespace Fortitude.EventProcessing.BusRules.MessageBus.Pipelines.Timers;

public class QueueTimer : Rule, IActionTimer
{
    private readonly IUpdateableTimer backingTimer;
    private readonly WaitCallback intervalWaitCallback;
    private readonly WaitCallback oneOffWaitCallback;
    private readonly IRecycler recycler;

    public QueueTimer(IUpdateableTimer backingTimer, IEventContext context)
    {
        this.backingTimer = backingTimer;
        Context = context;
        recycler = context.PooledRecycler;
        oneOffWaitCallback = OneOffTimerEnqueueAsMessage;
        intervalWaitCallback = IntervalTimerEnqueueAsMessage;
        Id = "QueueTimer_" + Context.RegisteredOn.Name;
        FriendlyName = Id;
        ParentRule = this;
    }

    public ITimerUpdate RunIn(TimeSpan waitTimeSpan, Action callback)
    {
        var actionPayload = recycler.Borrow<TimerCallbackPayload<object>>();
        actionPayload.ActionState = null;
        actionPayload.State = null;
        actionPayload.Action = callback;
        return backingTimer.RunIn(waitTimeSpan, actionPayload, oneOffWaitCallback);
    }

    public ITimerUpdate RunIn<T>(TimeSpan waitTimeSpan, T state, Action<T?> callback) where T : class
    {
        var actionPayload = recycler.Borrow<TimerCallbackPayload<T>>();
        actionPayload.ActionState = callback;
        actionPayload.State = state;
        actionPayload.Action = null;
        return backingTimer.RunIn(waitTimeSpan, actionPayload, oneOffWaitCallback);
    }

    public ITimerUpdate RunIn(int waitMs, Action callback)
    {
        var actionPayload = recycler.Borrow<TimerCallbackPayload<object>>();
        actionPayload.ActionState = null;
        actionPayload.State = null;
        actionPayload.Action = callback;
        return backingTimer.RunIn(waitMs, actionPayload, oneOffWaitCallback);
    }

    public ITimerUpdate RunIn<T>(int waitMs, T state, Action<T?> callback) where T : class
    {
        var actionPayload = recycler.Borrow<TimerCallbackPayload<T>>();
        actionPayload.ActionState = callback;
        actionPayload.State = state;
        actionPayload.Action = null;
        return backingTimer.RunIn(waitMs, actionPayload, oneOffWaitCallback);
    }

    public ITimerUpdate RunEvery(int intervalMs, Action callback)
    {
        var actionPayload = recycler.Borrow<TimerCallbackPayload<object>>();
        actionPayload.ActionState = null;
        actionPayload.State = null;
        actionPayload.Action = callback;
        return backingTimer.RunIn(intervalMs, actionPayload, intervalWaitCallback);
    }

    public ITimerUpdate RunEvery<T>(int intervalMs, T state, Action<T?> callback) where T : class
    {
        var actionPayload = recycler.Borrow<TimerCallbackPayload<T>>();
        actionPayload.ActionState = callback;
        actionPayload.State = state;
        actionPayload.Action = null;
        return backingTimer.RunIn(intervalMs, actionPayload, intervalWaitCallback);
    }

    public ITimerUpdate RunEvery(TimeSpan periodTimeSpan, Action callback)
    {
        var actionPayload = recycler.Borrow<TimerCallbackPayload<object>>();
        actionPayload.ActionState = null;
        actionPayload.State = null;
        actionPayload.Action = callback;
        return backingTimer.RunIn(periodTimeSpan, actionPayload, intervalWaitCallback);
    }

    public ITimerUpdate RunEvery<T>(TimeSpan periodTimeSpan, T state, Action<T?> callback) where T : class
    {
        var actionPayload = recycler.Borrow<TimerCallbackPayload<T>>();
        actionPayload.ActionState = callback;
        actionPayload.State = state;
        actionPayload.Action = null;
        return backingTimer.RunIn(periodTimeSpan, actionPayload, intervalWaitCallback);
    }

    public ITimerUpdate RunAt(DateTime futureDateTime, Action callback)
    {
        var actionPayload = recycler.Borrow<TimerCallbackPayload<object>>();
        actionPayload.ActionState = null;
        actionPayload.State = null;
        actionPayload.Action = callback;
        return backingTimer.RunAt(futureDateTime, actionPayload, oneOffWaitCallback);
    }

    public ITimerUpdate RunAt<T>(DateTime futureDateTime, T state, Action<T?> callback) where T : class
    {
        var actionPayload = recycler.Borrow<TimerCallbackPayload<T>>();
        actionPayload.ActionState = callback;
        actionPayload.State = state;
        actionPayload.Action = null;
        return backingTimer.RunAt(futureDateTime, actionPayload, oneOffWaitCallback);
    }

    public void PauseAllTimers()
    {
        backingTimer.PauseAllTimers();
    }

    public void ResumeAllTimers()
    {
        backingTimer.ResumeAllTimers();
    }

    public void StopAllTimers()
    {
        backingTimer.StopAllTimers();
    }

    public override void Stop()
    {
        backingTimer.StopAllTimers();
        base.Stop();
    }

    public void OneOffTimerEnqueueAsMessage(object? state)
    {
        if (state is ITimerCallbackPayload timerCallbackPayload)
            Context.RegisteredOn.EnqueuePayload(timerCallbackPayload, this, null, MessageType.RunActionPayload);
    }

    public void IntervalTimerEnqueueAsMessage(object? state)
    {
        if (state is ITimerCallbackPayload timerCallbackPayload)
        {
            timerCallbackPayload.IncrementRefCount();
            Context.RegisteredOn.EnqueuePayload(timerCallbackPayload, this, null, MessageType.RunActionPayload);
        }
    }
}
