// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;
using FortitudeCommon.Chronometry.Timers;
using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;

#endregion

namespace FortitudeBusRules.BusMessaging.Pipelines.Timers;

public interface IQueueTimer : IActionTimer, IAsyncValueTaskDisposable, IStringBearer
{
    IRuleTimer CreateRuleTimer(IRule owningRule);
}

public class QueueTimer : Rule, IQueueTimer
{
    private static readonly NoOpTimerUpdate NoOpTimerUpdate = new();

    private readonly ISingleEntryActionTimer    actionTimer;
    private readonly ReusableList<ITimerUpdate> registeredRuleTimers = new();
    private readonly IUpdateableTimer           updateableTimer;

    private bool isClosing;

    private IRecycler? recycler;

    public QueueTimer(IUpdateableTimer updateableTimer, IQueueContext context)
    {
        this.updateableTimer = updateableTimer;
        actionTimer = new ActionTimer(updateableTimer, context.PooledRecycler)
        {
            OneOffWaitCallback = OneOffTimerEnqueueAsMessage, IntervalWaitCallback = IntervalTimerEnqueueAsMessage
        };
        Context      = context;
        Id           = "QueueTimer_" + Context.RegisteredOn.Name;
        FriendlyName = Id;
        ParentRule   = this;
    }

    public IRuleTimer CreateRuleTimer(IRule owningRule) =>
        new RuleTimer(owningRule, new ActionTimer(updateableTimer, owningRule.Context.PooledRecycler))
        {
            Recycler = recycler
        };

    public IRecycler? Recycler
    {
        get => recycler;
        set
        {
            recycler = value;

            registeredRuleTimers.Recycler = value;
        }
    }

    public ValueTask DisposeAwaitValueTask { get; set; }

    public async ValueTask Dispose()
    {
        isClosing = true;
        actionTimer.StopAllTimers();
        foreach (var registeredRuleTimer in registeredRuleTimers) await registeredRuleTimer.Dispose();
    }

    public ValueTask DisposeAsync() => Dispose();

    public virtual ITimerUpdate RunIn(TimeSpan waitTimeSpan, Func<ValueTask> callback) =>
        isClosing ? NoOpTimerUpdate : registeredRuleTimers.AddReturn(actionTimer.RunIn(waitTimeSpan, callback));

    public virtual ITimerUpdate RunIn<T>(TimeSpan waitTimeSpan, T state, Func<T?, ValueTask> callback) where T : class =>
        isClosing ? NoOpTimerUpdate : registeredRuleTimers.AddReturn(actionTimer.RunIn(waitTimeSpan, state, callback));

    public ITimerUpdate RunIn(TimeSpan waitTimeSpan, Func<IScheduleActualTime?, ValueTask> callback) =>
        isClosing ? NoOpTimerUpdate : registeredRuleTimers.AddReturn(actionTimer.RunIn(waitTimeSpan, callback));

    public ITimerUpdate RunIn<T>(TimeSpan waitTimeSpan, T state, Func<IScheduleActualTime<T>?, ValueTask> callback) where T : class =>
        isClosing ? NoOpTimerUpdate : registeredRuleTimers.AddReturn(actionTimer.RunIn(waitTimeSpan, state, callback));

    public ITimerUpdate RunIn(int waitMs, Func<IScheduleActualTime?, ValueTask> callback) =>
        isClosing ? NoOpTimerUpdate : registeredRuleTimers.AddReturn(actionTimer.RunIn(waitMs, callback));

    public ITimerUpdate RunIn<T>(int waitMs, T state, Func<IScheduleActualTime<T>?, ValueTask> callback) where T : class =>
        isClosing ? NoOpTimerUpdate : registeredRuleTimers.AddReturn(actionTimer.RunIn(waitMs, state, callback));

    public virtual ITimerUpdate RunIn(int waitMs, Func<ValueTask> callback) =>
        isClosing ? NoOpTimerUpdate : registeredRuleTimers.AddReturn(actionTimer.RunIn(waitMs, callback));

    public virtual ITimerUpdate RunIn<T>(int waitMs, T state, Func<T?, ValueTask> callback) where T : class =>
        isClosing ? NoOpTimerUpdate : registeredRuleTimers.AddReturn(actionTimer.RunIn(waitMs, state, callback));

    public virtual ITimerUpdate RunEvery(int intervalMs, Func<ValueTask> callback) =>
        isClosing ? NoOpTimerUpdate : registeredRuleTimers.AddReturn(actionTimer.RunEvery(intervalMs, callback));

    public virtual ITimerUpdate RunEvery<T>(int intervalMs, T state, Func<T?, ValueTask> callback) where T : class =>
        isClosing ? NoOpTimerUpdate : registeredRuleTimers.AddReturn(actionTimer.RunEvery(intervalMs, state, callback));

    public virtual ITimerUpdate RunEvery(TimeSpan periodTimeSpan, Func<ValueTask> callback) =>
        isClosing ? NoOpTimerUpdate : registeredRuleTimers.AddReturn(actionTimer.RunEvery(periodTimeSpan, callback));

    public virtual ITimerUpdate RunEvery<T>(TimeSpan periodTimeSpan, T state, Func<T?, ValueTask> callback) where T : class =>
        isClosing ? NoOpTimerUpdate : registeredRuleTimers.AddReturn(actionTimer.RunEvery(periodTimeSpan, state, callback));

    public ITimerUpdate RunEvery(int intervalMs, Func<IScheduleActualTime?, ValueTask> callback) =>
        isClosing ? NoOpTimerUpdate : registeredRuleTimers.AddReturn(actionTimer.RunEvery(intervalMs, callback));

    public ITimerUpdate RunEvery<T>(int intervalMs, T state, Func<IScheduleActualTime<T>?, ValueTask> callback) where T : class =>
        isClosing ? NoOpTimerUpdate : registeredRuleTimers.AddReturn(actionTimer.RunEvery(intervalMs, state, callback));

    public ITimerUpdate RunEvery(TimeSpan periodTimeSpan, Func<IScheduleActualTime?, ValueTask> callback) =>
        isClosing ? NoOpTimerUpdate : registeredRuleTimers.AddReturn(actionTimer.RunEvery(periodTimeSpan, callback));

    public ITimerUpdate RunEvery<T>(TimeSpan periodTimeSpan, T state, Func<IScheduleActualTime<T>?, ValueTask> callback) where T : class =>
        isClosing ? NoOpTimerUpdate : registeredRuleTimers.AddReturn(actionTimer.RunEvery(periodTimeSpan, state, callback));

    public virtual ITimerUpdate RunAt(DateTime futureDateTime, Func<ValueTask> callback) =>
        isClosing ? NoOpTimerUpdate : registeredRuleTimers.AddReturn(actionTimer.RunAt(futureDateTime, callback));

    public virtual ITimerUpdate RunAt<T>(DateTime futureDateTime, T state, Func<T?, ValueTask> callback) where T : class =>
        isClosing ? NoOpTimerUpdate : registeredRuleTimers.AddReturn(actionTimer.RunAt(futureDateTime, state, callback));

    public ITimerUpdate RunAt(DateTime futureDateTime, Func<IScheduleActualTime?, ValueTask> callback) =>
        isClosing ? NoOpTimerUpdate : registeredRuleTimers.AddReturn(actionTimer.RunAt(futureDateTime, callback));

    public ITimerUpdate RunAt<T>(DateTime futureDateTime, T state, Func<IScheduleActualTime<T>?, ValueTask> callback) where T : class =>
        isClosing ? NoOpTimerUpdate : registeredRuleTimers.AddReturn(actionTimer.RunAt(futureDateTime, state, callback));

    public virtual ITimerUpdate RunIn(TimeSpan waitTimeSpan, Action callback) =>
        isClosing ? NoOpTimerUpdate : registeredRuleTimers.AddReturn(actionTimer.RunIn(waitTimeSpan, callback));

    public virtual ITimerUpdate RunIn<T>(TimeSpan waitTimeSpan, T state, Action<T?> callback) where T : class =>
        isClosing ? NoOpTimerUpdate : registeredRuleTimers.AddReturn(actionTimer.RunIn(waitTimeSpan, state, callback));

    public ITimerUpdate RunIn(TimeSpan waitTimeSpan, Action<IScheduleActualTime?> callback) =>
        isClosing ? NoOpTimerUpdate : registeredRuleTimers.AddReturn(actionTimer.RunIn(waitTimeSpan, callback));

    public ITimerUpdate RunIn<T>(TimeSpan waitTimeSpan, T state, Action<IScheduleActualTime<T>?> callback) where T : class =>
        isClosing ? NoOpTimerUpdate : registeredRuleTimers.AddReturn(actionTimer.RunIn(waitTimeSpan, state, callback));

    public ITimerUpdate RunIn(int waitMs, Action<IScheduleActualTime?> callback) =>
        isClosing ? NoOpTimerUpdate : registeredRuleTimers.AddReturn(actionTimer.RunIn(waitMs, callback));

    public ITimerUpdate RunIn<T>(int waitMs, T state, Action<IScheduleActualTime<T>?> callback) where T : class =>
        isClosing ? NoOpTimerUpdate : registeredRuleTimers.AddReturn(actionTimer.RunIn(waitMs, state, callback));

    public virtual ITimerUpdate RunIn(int waitMs, Action callback) =>
        isClosing ? NoOpTimerUpdate : registeredRuleTimers.AddReturn(actionTimer.RunIn(waitMs, callback));

    public virtual ITimerUpdate RunIn<T>(int waitMs, T state, Action<T?> callback) where T : class =>
        isClosing ? NoOpTimerUpdate : registeredRuleTimers.AddReturn(actionTimer.RunIn(waitMs, state, callback));

    public virtual ITimerUpdate RunEvery(TimeSpan periodTimeSpan, Action callback) =>
        isClosing ? NoOpTimerUpdate : registeredRuleTimers.AddReturn(actionTimer.RunEvery(periodTimeSpan, callback));

    public virtual ITimerUpdate RunEvery<T>(TimeSpan periodTimeSpan, T state, Action<T?> callback) where T : class =>
        isClosing ? NoOpTimerUpdate : registeredRuleTimers.AddReturn(actionTimer.RunEvery(periodTimeSpan, state, callback));

    public ITimerUpdate RunEvery(int intervalMs, Action<IScheduleActualTime?> callback) =>
        isClosing ? NoOpTimerUpdate : registeredRuleTimers.AddReturn(actionTimer.RunEvery(intervalMs, callback));

    public ITimerUpdate RunEvery<T>(int intervalMs, T state, Action<IScheduleActualTime<T>?> callback) where T : class =>
        isClosing ? NoOpTimerUpdate : registeredRuleTimers.AddReturn(actionTimer.RunEvery(intervalMs, state, callback));

    public ITimerUpdate RunEvery(TimeSpan periodTimeSpan, Action<IScheduleActualTime?> callback) =>
        isClosing ? NoOpTimerUpdate : registeredRuleTimers.AddReturn(actionTimer.RunEvery(periodTimeSpan, callback));

    public ITimerUpdate RunEvery<T>(TimeSpan periodTimeSpan, T state, Action<IScheduleActualTime<T>?> callback) where T : class =>
        isClosing ? NoOpTimerUpdate : registeredRuleTimers.AddReturn(actionTimer.RunEvery(periodTimeSpan, state, callback));

    public virtual ITimerUpdate RunEvery(int intervalMs, Action callback) =>
        isClosing ? NoOpTimerUpdate : registeredRuleTimers.AddReturn(actionTimer.RunEvery(intervalMs, callback));

    public ITimerUpdate RunEvery<T>(int intervalMs, T state, Action<T?> callback) where T : class =>
        isClosing ? NoOpTimerUpdate : registeredRuleTimers.AddReturn(actionTimer.RunEvery(intervalMs, state, callback));

    public virtual ITimerUpdate RunAt(DateTime futureDateTime, Action callback) =>
        isClosing ? NoOpTimerUpdate : registeredRuleTimers.AddReturn(actionTimer.RunAt(futureDateTime, callback));

    public virtual ITimerUpdate RunAt<T>(DateTime futureDateTime, T state, Action<T?> callback) where T : class =>
        isClosing ? NoOpTimerUpdate : registeredRuleTimers.AddReturn(actionTimer.RunAt(futureDateTime, state, callback));

    public ITimerUpdate RunAt(DateTime futureDateTime, Action<IScheduleActualTime?> callback) =>
        isClosing ? NoOpTimerUpdate : registeredRuleTimers.AddReturn(actionTimer.RunAt(futureDateTime, callback));

    public ITimerUpdate RunAt<T>(DateTime futureDateTime, T state, Action<IScheduleActualTime<T>?> callback) where T : class =>
        isClosing ? NoOpTimerUpdate : registeredRuleTimers.AddReturn(actionTimer.RunAt(futureDateTime, state, callback));

    public virtual void PauseAllTimers()
    {
        foreach (var registeredRuleTimer in registeredRuleTimers) registeredRuleTimer.Pause();
    }

    public virtual void ResumeAllTimers()
    {
        foreach (var registeredRuleTimer in registeredRuleTimers) registeredRuleTimer.Resume();
    }

    public virtual void StopAllTimers()
    {
        foreach (var registeredRuleTimer in registeredRuleTimers) registeredRuleTimer.Cancel();
    }

    public override void Stop()
    {
        foreach (var registeredRuleTimer in registeredRuleTimers) registeredRuleTimer.Cancel();
    }

    public void OneOffTimerEnqueueAsMessage(object? state)
    {
        if (state is ITimerCallbackPayload timerCallbackPayload)
            Context.RegisteredOn.EnqueuePayloadBody(timerCallbackPayload, this, MessageType.TimerPayload);
    }

    public void IntervalTimerEnqueueAsMessage(object? state)
    {
        if (state is ITimerCallbackPayload timerCallbackPayload)
        {
            timerCallbackPayload.IncrementRefCount();
            Context.RegisteredOn.EnqueuePayloadBody(timerCallbackPayload, this, MessageType.TimerPayload);
        }
    }

    public override StateExtractStringRange RevealState(ITheOneString stsa) => 
        stsa.StartComplexType(this)
            .AddBaseStyledToStringFields(this)
            .Field.AlwaysAdd(nameof(isClosing), isClosing)
            .Complete();
    
}
