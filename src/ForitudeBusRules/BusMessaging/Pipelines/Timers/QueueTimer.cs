#region

using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;
using FortitudeCommon.Chronometry.Timers;

#endregion

namespace FortitudeBusRules.BusMessaging.Pipelines.Timers;

public class QueueTimer : Rule, IActionTimer
{
    private readonly ActionTimer backingTimer;

    public QueueTimer(IUpdateableTimer backingTimer, IQueueContext context)
    {
        this.backingTimer = new ActionTimer(backingTimer, context.PooledRecycler);
        this.backingTimer.OneOffWaitCallback = OneOffTimerEnqueueAsMessage;
        this.backingTimer.IntervalWaitCallback = IntervalTimerEnqueueAsMessage;
        Context = context;
        Id = "QueueTimer_" + Context.RegisteredOn.Name;
        FriendlyName = Id;
        ParentRule = this;
    }

    public ITimerUpdate RunIn(TimeSpan waitTimeSpan, Func<ValueTask> callback) => backingTimer.RunIn(waitTimeSpan, callback);

    public ITimerUpdate RunIn<T>(TimeSpan waitTimeSpan, T state, Func<T?, ValueTask> callback) where T : class =>
        backingTimer.RunIn(waitTimeSpan, state, callback);

    public ITimerUpdate RunIn(int waitMs, Func<ValueTask> callback) => backingTimer.RunIn(waitMs, callback);

    public ITimerUpdate RunIn<T>(int waitMs, T state, Func<T?, ValueTask> callback) where T : class => backingTimer.RunIn(waitMs, state, callback);

    public ITimerUpdate RunEvery(int intervalMs, Func<ValueTask> callback) => backingTimer.RunEvery(intervalMs, callback);

    public ITimerUpdate RunEvery<T>(int intervalMs, T state, Func<T?, ValueTask> callback) where T : class =>
        backingTimer.RunEvery(intervalMs, state, callback);

    public ITimerUpdate RunEvery(TimeSpan periodTimeSpan, Func<ValueTask> callback) => backingTimer.RunEvery(periodTimeSpan, callback);

    public ITimerUpdate RunEvery<T>(TimeSpan periodTimeSpan, T state, Func<T?, ValueTask> callback) where T : class =>
        backingTimer.RunEvery(periodTimeSpan, state, callback);

    public ITimerUpdate RunAt(DateTime futureDateTime, Func<ValueTask> callback) => backingTimer.RunAt(futureDateTime, callback);

    public ITimerUpdate RunAt<T>(DateTime futureDateTime, T state, Func<T?, ValueTask> callback) where T : class =>
        backingTimer.RunAt(futureDateTime, state, callback);

    public ITimerUpdate RunIn(TimeSpan waitTimeSpan, Action callback) => backingTimer.RunIn(waitTimeSpan, callback);

    public ITimerUpdate RunIn<T>(TimeSpan waitTimeSpan, T state, Action<T?> callback) where T : class =>
        backingTimer.RunIn(waitTimeSpan, state, callback);

    public ITimerUpdate RunIn(int waitMs, Action callback) => backingTimer.RunIn(waitMs, callback);

    public ITimerUpdate RunIn<T>(int waitMs, T state, Action<T?> callback) where T : class => backingTimer.RunIn(waitMs, state, callback);

    public ITimerUpdate RunEvery(int intervalMs, Action callback) => backingTimer.RunEvery(intervalMs, callback);

    public ITimerUpdate RunEvery<T>(int intervalMs, T state, Action<T?> callback) where T : class =>
        backingTimer.RunEvery(intervalMs, state, callback);

    public ITimerUpdate RunEvery(TimeSpan periodTimeSpan, Action callback) => backingTimer.RunEvery(periodTimeSpan, callback);

    public ITimerUpdate RunEvery<T>(TimeSpan periodTimeSpan, T state, Action<T?> callback) where T : class =>
        backingTimer.RunEvery(periodTimeSpan, state, callback);

    public ITimerUpdate RunAt(DateTime futureDateTime, Action callback) => backingTimer.RunAt(futureDateTime, callback);

    public ITimerUpdate RunAt<T>(DateTime futureDateTime, T state, Action<T?> callback) where T : class =>
        backingTimer.RunAt(futureDateTime, state, callback);

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
            Context.RegisteredOn.EnqueuePayloadBody(timerCallbackPayload, this, null, MessageType.TimerPayload);
    }

    public void IntervalTimerEnqueueAsMessage(object? state)
    {
        if (state is ITimerCallbackPayload timerCallbackPayload)
        {
            timerCallbackPayload.IncrementRefCount();
            Context.RegisteredOn.EnqueuePayloadBody(timerCallbackPayload, this, null, MessageType.TimerPayload);
        }
    }
}
