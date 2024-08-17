// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.BusMessaging.Messages;
using FortitudeBusRules.BusMessaging.Messages.ListeningSubscriptions;
using FortitudeBusRules.BusMessaging.Tasks;
using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;
using FortitudeCommon.AsyncProcessing.Tasks;
using FortitudeCommon.Chronometry;
using FortitudeCommon.Chronometry.Timers;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Monitoring.Logging;

#endregion

namespace FortitudeBusRules.BusMessaging.Pipelines.Processing;

public class AsyncBusMessageProcessor : ReusableValueTaskSource<int>
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(AsyncBusMessageProcessor));

    private readonly Action callCheckCompletedAction;
    private readonly Action callFinaliseLoadNewRule;
    private readonly Action callFinaliseStartNewRule;
    private readonly Action callSetProcessingComplete;

    private readonly ReusableValueTaskSource<int> finalizeLoadNewRule = new()
    {
        AutoRecycleAtRefCountZero = false
    };

    private BusMessageValue busMessage;

    private ValueTask       currentAwaitingTask;
    private IListeningRule? newRule;
    private ValueTask       newRuleTask;

    private IProcessorRegistry? processorRegistry;
    private QueueMessageRing    queueMessageRing = null!;

    public AsyncBusMessageProcessor()
    {
        callCheckCompletedAction  = CheckCompleted;
        callSetProcessingComplete = SetProcessingComplete;
        callFinaliseStartNewRule  = FinalizeStartRule;
        callFinaliseLoadNewRule   = FinalizeLoadRule;
    }

    public override IRecycler? Recycler
    {
        get => base.Recycler;
        set
        {
            base.Recycler                = value;
            finalizeLoadNewRule.Recycler = value;
        }
    }

    public ValueTask Start(BusMessageValue busMsg, QueueMessageRing messageRing)
    {
        busMessage       = busMsg;
        queueMessageRing = messageRing;
        ProcessMessageSynchronousMessages();
        SetCurrentAwaitingTask(ProcessMessageAsyncSynchronousMessages(), callCheckCompletedAction);
        return ToValueTask();
    }

    private void SetProcessingComplete()
    {
        busMessage.DecrementCargoRefCounts();
        TrySetResult(1);
    }

    private void CheckCompleted()
    {
        CheckDaemonCompleted();
        SetCurrentAwaitingTask(CheckRuleLivingCountAboveZero(), callSetProcessingComplete);
    }

    private void SetCurrentAwaitingTask(ValueTask newTask, Action continuationAction)
    {
        if (currentAwaitingTask.IsCompleted)
            try
            {
                currentAwaitingTask.GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Logger.Warn("AsyncBusMessageProcessor caught {0}", ex);
            }
        currentAwaitingTask = newTask;
        currentAwaitingTask.ContinueWith(continuationAction);
    }

    private ValueTask CheckRuleLivingCountAboveZero()
    {
        IListeningRule? lastRequestedRemove = null;
        for (var i = 0; i < queueMessageRing.LivingRules.Count; i++)
        {
            if (i < 0) // async await below may alter the living rules list
                i = 0;
            if (queueMessageRing.LivingRules.Count == 0 ||
                i >= queueMessageRing.LivingRules.Count) // async await below may alter the living rules list
                break;
            if (lastRequestedRemove == queueMessageRing.LivingRules[i])
            {
                i++;
                if (i >= queueMessageRing.LivingRules.Count) break;
            }

            var checkRule = queueMessageRing.LivingRules[i];

            if (checkRule.LifeCycleState != RuleLifeCycle.Started || !checkRule.ShouldBeStopped()) continue;
            try
            {
                lastRequestedRemove = checkRule;
                i--;
                return UnloadRuleAndDependents(checkRule);
            }
            catch (Exception ex)
            {
                Logger.Warn("Caught exception stopping rule {0}, Got {1}", checkRule.FriendlyName, ex);
            }
        }
        return ValueTask.CompletedTask;
    }

    private void CheckDaemonCompleted()
    {
        for (var i = 0; i < queueMessageRing.DaemonExecutions.Count; i++)
        {
            var checkHasFinished = queueMessageRing.DaemonExecutions[i];
            if (checkHasFinished.StartTask.IsCompleted)
            {
                try
                {
                    checkHasFinished.StartTask.GetAwaiter().GetResult();
                    checkHasFinished.Rule.DecrementLifeTimeCount();
                }
                catch (Exception ex)
                {
                    Logger.Warn("Daemon rule completed with exception.  Got {0} ", ex);
                }
                queueMessageRing.DaemonExecutions.RemoveAt(i--);
            }
        }
    }

    private void ProcessMessageSynchronousMessages()
    { // Logger.Debug("Received bus message {0} on {1}", data, ringPoller.Ring.Name);
        switch (busMessage.Type)
        {
            case MessageType.RunActionPayload:
            {
                RunAction(busMessage);
                break;
            }
            case MessageType.Publish:
            case MessageType.RequestResponse:
            {
                CheckListenersForSubscription(busMessage);
                break;
            }
        }
    }

    private ValueTask ProcessMessageAsyncSynchronousMessages()
    { // Logger.Debug("Received bus message {0} on {1}", data, ringPoller.Ring.Name);
        switch (busMessage!.Type)
        {
            case MessageType.LoadRule:   return LoadNewRule(busMessage);
            case MessageType.UnloadRule: return UnloadExistingRule(busMessage);
            case MessageType.RunAsyncActionPayload:
            {
                return RunAsyncAction(busMessage);
            }
            case MessageType.InvokeablePayload:
            {
                return RunInvokeablePayload(busMessage);
            }
            case MessageType.TimerPayload:
            {
                return RunTimerCallback(busMessage);
            }
            case MessageType.ListenerSubscribe:
            {
                return SubscribeToAddress(busMessage);
            }
            case MessageType.ListenerUnsubscribe:
            {
                return UnsubscribeFromAddress(busMessage);
            }
            case MessageType.AddListenSubscribeInterceptor:
            {
                return AddInterceptor(busMessage);
            }
            case MessageType.RemoveListenSubscribeInterceptor:
            {
                return RemoveInterceptor(busMessage);
            }
            default: return ValueTask.CompletedTask;
        }
    }

    private ValueTask RemoveInterceptor(BusMessageValue data)
    {
        var unsubscribePayload = (IListenSubscribeInterceptor)data.Payload.BodyObj(PayloadRequestType.QueueReceive)!;
        return queueMessageRing.ListenerRegistry.RemoveSubscribeInterceptor(unsubscribePayload);
    }

    private ValueTask AddInterceptor(BusMessageValue data)
    {
        var subscribePayload = (IListenSubscribeInterceptor)data.Payload.BodyObj(PayloadRequestType.QueueReceive)!;
        return queueMessageRing.ListenerRegistry.AddSubscribeInterceptor(subscribePayload);
    }

    private ValueTask UnsubscribeFromAddress(BusMessageValue data)
    {
        var unsubscribePayload = ((Payload<MessageListenerSubscription>)data.Payload).Body(PayloadRequestType.QueueReceive)!;
        return queueMessageRing.ListenerRegistry.RemoveListenerFromWatchList(unsubscribePayload);
    }

    private ValueTask SubscribeToAddress(BusMessageValue data) => queueMessageRing.ListenerRegistry.AddListenerToWatchList(data);

    private ValueTask RunTimerCallback(BusMessageValue data)
    {
        try
        {
            var timerCallbackPayload = (ITimerCallbackPayload)data.Payload.BodyObj(PayloadRequestType.QueueReceive)!;
            if (timerCallbackPayload is IScheduledActualTimerCallbackPayload scheduleActual)
                scheduleActual.ScheduleActualTime.ReceivedAt = TimeContext.UtcNow;
            if (!timerCallbackPayload.IsAsyncInvoke())
            {
                // ReSharper disable once MethodHasAsyncOverload
                timerCallbackPayload.Invoke();
                return ValueTask.CompletedTask;
            }
            return timerCallbackPayload.InvokeAsync();
        }
        catch (Exception ex)
        {
            Logger.Warn("Caught exception running TimerPayload on {1}.  Got {0}", queueMessageRing.Name, ex);
        }
        return ValueTask.CompletedTask;
    }

    private ValueTask RunInvokeablePayload(BusMessageValue data)
    {
        try
        {
            var invokable = (IInvokeablePayload)data.Payload.BodyObj(PayloadRequestType.QueueReceive)!;
            if (!invokable.IsAsyncInvoke)
            {
                // ReSharper disable once MethodHasAsyncOverload
                invokable!.Invoke();
                return ValueTask.CompletedTask;
            }
            else
            {
                return invokable.InvokeAsync();
            }
        }
        catch (Exception ex)
        {
            Logger.Warn("Caught exception running InvokablePayload on {1}.  Got {0}", queueMessageRing.Name, ex);
        }
        return ValueTask.CompletedTask;
    }

    private ValueTask RunAsyncAction(BusMessageValue data)
    {
        try
        {
            var actionBody = ((Payload<Func<ValueTask>>)data.Payload).Body(PayloadRequestType.QueueReceive);
            return actionBody();
        }
        catch (Exception ex)
        {
            Logger.Warn("Caught exception running Func<ValueTask> on {1}.  Got {0}", queueMessageRing.Name, ex);
        }
        return ValueTask.CompletedTask;
    }

    private void RunAction(BusMessageValue data)
    {
        try
        {
            var actionBody = ((Payload<Action>)data.Payload).Body(PayloadRequestType.QueueReceive);
            actionBody();
        }
        catch (Exception ex)
        {
            Logger.Warn("Caught exception running Action on {1}.  Got {0}", queueMessageRing.Name, ex);
        }
    }

    private void CheckListenersForSubscription(BusMessageValue data)
    {
        foreach (var matcherListener in queueMessageRing.ListenerRegistry.MatchingSubscriptions(data.DestinationAddress!))
            try
            {
                matcherListener.Handler(data);
            }
            catch (Exception ex)
            {
                Logger.Warn("Caught exception processing message {0} on rule handler {1}.  Got {2}"
                          , data, matcherListener.SubscriberRule.FriendlyName, ex);
                data.ProcessorRegistry?.RegisterFinish(matcherListener.SubscriberRule);
                data.Response?.SetException(ex);
            }
    }

    private async ValueTask UnloadExistingRule(BusMessageValue data)
    {
        var toShutdown = (IListeningRule)data.Payload.BodyObj(PayloadRequestType.QueueReceive)!;
        try
        {
            if (queueMessageRing.LivingRules.Contains(toShutdown) && toShutdown.LifeCycleState == RuleLifeCycle.Started)
            {
                await UnloadRuleAndDependents(toShutdown);
                data.ProcessorRegistry!.ProcessingComplete();
            }
        }
        catch (Exception ex)
        {
            Logger.Warn("Problem starting rule: {0}.  Caught {1}", toShutdown.FriendlyName, ex);
        }
    }

    private async ValueTask UnloadRuleAndDependents(IListeningRule toShutdown)
    {
        if (toShutdown.LifeCycleState != RuleLifeCycle.Started) return;
        toShutdown.LifeCycleState = RuleLifeCycle.ShutDownRequested;
        await UndeployChildren(toShutdown);
        try
        {
            foreach (var ruleRegisteredDisposables in toShutdown.OnStopResourceCleanup()) await ruleRegisteredDisposables.Dispose();
            await queueMessageRing.ListenerRegistry.UnsubscribeAllListenersForRule(toShutdown);
            await toShutdown.MessageBusStopAsync();
        }
        catch (Exception ex)
        {
            Logger.Warn("Caught exception stopping rule of {0}.  Caught {1}", toShutdown.FriendlyName, ex);
        }

        queueMessageRing.LivingRules.Remove(toShutdown);
        toShutdown.LifeCycleState = RuleLifeCycle.Stopped;
    }

    private static async ValueTask UndeployChildren(IRule parentRule)
    {
        if (parentRule.ChildRules.Any())
            foreach (var child in parentRule.ChildRules)
                if (child.LifeCycleState == RuleLifeCycle.Started)
                    try
                    {
                        await child.Context.RegisteredOn.StopRuleAsync(parentRule, child);
                    }
                    catch (Exception ex)
                    {
                        Logger.Warn("Problem undeploying child rule of {0}.  Child {1}.  Caught {2}"
                                  , parentRule.FriendlyName, child.FriendlyName, ex);
                    }
    }

    private ValueTask LoadNewRule(BusMessageValue data)
    {
        var newLaunchRule = ((Payload<LaunchRulePayload>)data.Payload).Body(PayloadRequestType.QueueReceive)!;
        return newLaunchRule.DeploymentOptions.RunAsDaemon ? LoadDaemonRule(data) : LoadChildRule(data);
    }

    private ValueTask LoadChildRule(BusMessageValue data)
    {
        var newLaunchRule = ((Payload<LaunchRulePayload>)data.Payload).Body(PayloadRequestType.QueueReceive)!;
        newRule           = newLaunchRule.Rule;
        processorRegistry = data.ProcessorRegistry;
        processorRegistry?.RegisterStart(newRule);
        try
        {
            newRule.IncrementLifeTimeCount();
            queueMessageRing.LivingRules.Add(newRule);
            newRuleTask = newRule.StartAsync();
            newRuleTask.ContinueWith(callFinaliseStartNewRule);
            return finalizeLoadNewRule.ToValueTask();
        }
        catch (Exception ex)
        {
            Logger.Warn("Problem starting rule: {0}.  Caught {1}", newRule.FriendlyName, ex);
            processorRegistry?.SetException(ex);
            newRuleTask = UnloadRuleAndDependents(newRule);
            newRuleTask.ContinueWith(callFinaliseLoadNewRule);
            finalizeLoadNewRule.ToValueTask();
        }
        FinalizeLoadRule();
        return ValueTask.CompletedTask;
    }

    private ValueTask LoadDaemonRule(BusMessageValue data)
    {
        var newLaunchRule = ((Payload<LaunchRulePayload>)data.Payload).Body(PayloadRequestType.QueueReceive)!;
        newRule           = newLaunchRule.Rule;
        processorRegistry = data.ProcessorRegistry;
        processorRegistry?.RegisterStart(newRule);
        try
        {
            newRule.IncrementLifeTimeCount();
            queueMessageRing.LivingRules.Add(newRule);
            queueMessageRing.DaemonExecutions.Add(new DaemonRuleStart(newRule, newRule.StartAsync()));
            newRule.LifeCycleState = RuleLifeCycle.Started;
        }
        catch (Exception ex)
        {
            Logger.Warn("Problem starting rule: {0}.  Caught {1}", newRule.FriendlyName, ex);
            processorRegistry?.SetException(ex);
            newRuleTask = UnloadRuleAndDependents(newRule);
            newRuleTask.ContinueWith(callFinaliseLoadNewRule);
            finalizeLoadNewRule.ToValueTask();
        }
        FinalizeLoadRule();
        return ValueTask.CompletedTask;
    }

    private void FinalizeStartRule()
    {
        newRule!.LifeCycleState = RuleLifeCycle.Started;
        newRule.DecrementLifeTimeCount();
        FinalizeLoadRule();
    }

    private void FinalizeLoadRule()
    {
        processorRegistry?.RegisterFinish(newRule!);
        processorRegistry?.ProcessingComplete();
        processorRegistry = null!;
        newRule           = null!;
        finalizeLoadNewRule.TrySetResult(1);
    }

    public override void StateReset()
    {
        finalizeLoadNewRule.StateReset();
        processorRegistry?.DecrementRefCount();
        processorRegistry   = null;
        newRule             = null!;
        currentAwaitingTask = ValueTask.CompletedTask;
        newRuleTask         = ValueTask.CompletedTask;
        busMessage          = default;
        queueMessageRing    = null!;
        base.StateReset();
    }
}
