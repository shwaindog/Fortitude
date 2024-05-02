#region

using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;
using FortitudeCommon.Chronometry.Timers;
using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.Types;

#endregion

namespace FortitudeBusRules.BusMessaging.Pipelines.Timers;

public interface IRuleTimer : IActionTimer, IAsyncValueTaskDisposable { }

public class RuleTimer : IRuleTimer
{
    private static readonly NoOpTimerUpdate NoOpTimerUpdate = new();
    private readonly ActionTimer backingTimer;
    private readonly IRule owningRule;
    private readonly List<ITimerUpdate> registeredRuleTimers = new();
    private bool isClosing;

    public RuleTimer(IRule owningRule, ActionTimer backingTimer)
    {
        this.owningRule = owningRule;
        this.backingTimer = backingTimer;
        this.backingTimer.OneOffWaitCallback = OneOffTimerEnqueueAsMessage;
        this.backingTimer.IntervalWaitCallback = IntervalTimerEnqueueAsMessage;
    }

    public ITimerUpdate RunIn(TimeSpan waitTimeSpan, Func<ValueTask> callback) =>
        isClosing ? NoOpTimerUpdate : registeredRuleTimers.AddReturn(backingTimer.RunIn(waitTimeSpan, callback));

    public ITimerUpdate RunIn<T>(TimeSpan waitTimeSpan, T state, Func<T?, ValueTask> callback) where T : class =>
        isClosing ? NoOpTimerUpdate : registeredRuleTimers.AddReturn(backingTimer.RunIn(waitTimeSpan, state, callback));

    public ITimerUpdate RunIn(int waitMs, Func<ValueTask> callback) =>
        isClosing ? NoOpTimerUpdate : registeredRuleTimers.AddReturn(backingTimer.RunIn(waitMs, callback));

    public ITimerUpdate RunIn<T>(int waitMs, T state, Func<T?, ValueTask> callback) where T : class =>
        isClosing ? NoOpTimerUpdate : registeredRuleTimers.AddReturn(backingTimer.RunIn(waitMs, state, callback));

    public ITimerUpdate RunEvery(int intervalMs, Func<ValueTask> callback) =>
        isClosing ? NoOpTimerUpdate : registeredRuleTimers.AddReturn(backingTimer.RunEvery(intervalMs, callback));

    public ITimerUpdate RunEvery<T>(int intervalMs, T state, Func<T?, ValueTask> callback) where T : class =>
        isClosing ? NoOpTimerUpdate : registeredRuleTimers.AddReturn(backingTimer.RunEvery(intervalMs, state, callback));

    public ITimerUpdate RunEvery(TimeSpan periodTimeSpan, Func<ValueTask> callback) =>
        isClosing ? NoOpTimerUpdate : registeredRuleTimers.AddReturn(backingTimer.RunEvery(periodTimeSpan, callback));

    public ITimerUpdate RunEvery<T>(TimeSpan periodTimeSpan, T state, Func<T?, ValueTask> callback) where T : class =>
        isClosing ? NoOpTimerUpdate : registeredRuleTimers.AddReturn(backingTimer.RunEvery(periodTimeSpan, state, callback));

    public ITimerUpdate RunAt(DateTime futureDateTime, Func<ValueTask> callback) =>
        isClosing ? NoOpTimerUpdate : registeredRuleTimers.AddReturn(backingTimer.RunAt(futureDateTime, callback));

    public ITimerUpdate RunAt<T>(DateTime futureDateTime, T state, Func<T?, ValueTask> callback) where T : class =>
        isClosing ? NoOpTimerUpdate : registeredRuleTimers.AddReturn(backingTimer.RunAt(futureDateTime, state, callback));

    public ITimerUpdate RunIn(TimeSpan waitTimeSpan, Action callback) =>
        isClosing ? NoOpTimerUpdate : registeredRuleTimers.AddReturn(backingTimer.RunIn(waitTimeSpan, callback));

    public ITimerUpdate RunIn<T>(TimeSpan waitTimeSpan, T state, Action<T?> callback) where T : class =>
        isClosing ? NoOpTimerUpdate : registeredRuleTimers.AddReturn(backingTimer.RunIn(waitTimeSpan, state, callback));

    public ITimerUpdate RunIn(int waitMs, Action callback) =>
        isClosing ? NoOpTimerUpdate : registeredRuleTimers.AddReturn(backingTimer.RunIn(waitMs, callback));

    public ITimerUpdate RunIn<T>(int waitMs, T state, Action<T?> callback) where T : class =>
        isClosing ? NoOpTimerUpdate : registeredRuleTimers.AddReturn(backingTimer.RunIn(waitMs, state, callback));

    public ITimerUpdate RunEvery(int intervalMs, Action callback) =>
        isClosing ? NoOpTimerUpdate : registeredRuleTimers.AddReturn(backingTimer.RunEvery(intervalMs, callback));

    public ITimerUpdate RunEvery<T>(int intervalMs, T state, Action<T?> callback) where T : class =>
        isClosing ? NoOpTimerUpdate : registeredRuleTimers.AddReturn(backingTimer.RunEvery(intervalMs, state, callback));

    public ITimerUpdate RunEvery(TimeSpan periodTimeSpan, Action callback) =>
        isClosing ? NoOpTimerUpdate : registeredRuleTimers.AddReturn(backingTimer.RunEvery(periodTimeSpan, callback));

    public ITimerUpdate RunEvery<T>(TimeSpan periodTimeSpan, T state, Action<T?> callback) where T : class =>
        isClosing ? NoOpTimerUpdate : registeredRuleTimers.AddReturn(backingTimer.RunEvery(periodTimeSpan, state, callback));

    public ITimerUpdate RunAt(DateTime futureDateTime, Action callback) =>
        isClosing ? NoOpTimerUpdate : registeredRuleTimers.AddReturn(backingTimer.RunAt(futureDateTime, callback));

    public ITimerUpdate RunAt<T>(DateTime futureDateTime, T state, Action<T?> callback) where T : class =>
        isClosing ? NoOpTimerUpdate : registeredRuleTimers.AddReturn(backingTimer.RunAt(futureDateTime, state, callback));

    public void PauseAllTimers()
    {
        foreach (var registeredRuleTimer in registeredRuleTimers) registeredRuleTimer.Pause();
    }

    public void ResumeAllTimers()
    {
        foreach (var registeredRuleTimer in registeredRuleTimers) registeredRuleTimer.Resume();
    }

    public void StopAllTimers()
    {
        foreach (var registeredRuleTimer in registeredRuleTimers) registeredRuleTimer.Cancel();
    }

    public ValueTask DisposeAwaitValueTask { get; set; }

    public async ValueTask Dispose()
    {
        isClosing = true;
        foreach (var registeredRuleTimer in registeredRuleTimers) await registeredRuleTimer.Dispose();
    }

    public void OneOffTimerEnqueueAsMessage(object? state)
    {
        if (state is ITimerCallbackPayload timerCallbackPayload)
            owningRule.Context.RegisteredOn.EnqueuePayloadBody(timerCallbackPayload, owningRule, MessageType.TimerPayload);
    }

    public void IntervalTimerEnqueueAsMessage(object? state)
    {
        if (state is ITimerCallbackPayload timerCallbackPayload)
        {
            timerCallbackPayload.IncrementRefCount();
            owningRule.Context.RegisteredOn.EnqueuePayloadBody(timerCallbackPayload, owningRule, MessageType.TimerPayload);
        }
    }
}
