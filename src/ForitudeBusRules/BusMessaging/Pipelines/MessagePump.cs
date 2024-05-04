#region

using FortitudeBusRules.BusMessaging.Messages;
using FortitudeBusRules.BusMessaging.Messages.ListeningSubscriptions;
using FortitudeBusRules.BusMessaging.Tasks;
using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;
using FortitudeCommon.AsyncProcessing.Tasks;
using FortitudeCommon.Chronometry.Timers;
using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.EventProcessing.Disruption.Rings;
using FortitudeCommon.EventProcessing.Disruption.Rings.PollingRings;
using FortitudeCommon.Monitoring.Logging;

#endregion


namespace FortitudeBusRules.BusMessaging.Pipelines;

public interface IMessagePump
{
    bool IsListeningOn(string address);

    int CopyLivingRulesTo(IAutoRecycleEnumerable<IRule> toCopyTo);
    IEnumerable<IMessageListenerRegistration> ListeningSubscriptionsOn(string address);

    event Action<QueueEventTime> MessageStartProcessingTime;
    event Action<QueueEventTime> MessageFinishProcessingTime;
}

public class MessagePump : IMessagePump
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(MessagePump));

    private readonly ListenerRegistry listenerRegistry = new();

    private readonly List<IListeningRule> livingRules = new();
    private readonly IAsyncValueTaskRingPoller<BusMessage> ringPoller;

    public MessagePump(IAsyncValueTaskRingPoller<BusMessage> ringPoller, QueueContext eventContext)
    {
        this.ringPoller = ringPoller;
        this.ringPoller.ProcessEvent = Processor;
        EventContext = eventContext;
    }

    public SyncContextTaskScheduler RingPollerScheduler { get; private set; } = null!;
    public QueueContext EventContext { get; set; }

    public bool IsRunning => ringPoller.IsRunning;

    public int UsageCount => ringPoller.UsageCount;

    public event Action<QueueEventTime>? MessageStartProcessingTime
    {
        add => ringPoller.QueueEntryStart += value;
        remove => ringPoller.QueueEntryStart -= value;
    }

    public event Action<QueueEventTime>? MessageFinishProcessingTime
    {
        add => ringPoller.QueueEntryStart += value;
        remove => ringPoller.QueueEntryStart -= value;
    }

    public bool IsListeningOn(string address) => listenerRegistry.IsListeningOn(address);

    public int CopyLivingRulesTo(IAutoRecycleEnumerable<IRule> toCopyTo)
    {
        var i = 0;
        try
        {
            for (; i < livingRules.Count; i++) toCopyTo.Add(livingRules[i]);
        }
        catch (IndexOutOfRangeException)
        {
            // swallow
        }

        return i;
    }

    public IEnumerable<IMessageListenerRegistration> ListeningSubscriptionsOn(string address) => listenerRegistry.MatchingSubscriptions(address);

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        EventContext.QueueTimer.Dispose();
        ringPoller.Dispose();
    }

    public void WakeIfAsleep()
    {
        ringPoller.WakeIfAsleep();
    }

    public void Start(Action? threadStartInitialize = null)
    {
        ringPoller.Start(InitializeInPollingThread);
    }

    public void Stop()
    {
        EventContext.QueueTimer.Dispose();
        ringPoller.Stop();
    }

    public async ValueTask<long> Processor(long sequence, BusMessage data)
    {
        try
        {
            // Logger.Debug("Received bus message {0} on {1}", data, ringPoller.Ring.Name);
            switch (data.Type)
            {
                case MessageType.LoadRule:
                    await LoadNewRule(data);
                    break;
                case MessageType.UnloadRule:
                    await UnloadExistingRule(data);
                    break;
                case MessageType.RunActionPayload:
                {
                    try
                    {
                        var actionBody = ((Payload<Action>)data.Payload).Body(PayloadRequestType.QueueReceive);
                        actionBody();
                    }
                    catch (Exception ex)
                    {
                        Logger.Warn("Caught exception running Action on {1}.  Got {0}", ringPoller.Ring.Name, ex);
                    }

                    break;
                }
                case MessageType.QueueParamsExecutionPayload:
                {
                    try
                    {
                        var invokable = (IInvokeablePayload)data.Payload.BodyObj(PayloadRequestType.QueueReceive)!;
                        if (invokable.IsAsyncInvoke)
                            await invokable.InvokeAsync();
                        else
                            // ReSharper disable once MethodHasAsyncOverload
                            invokable!.Invoke();
                    }
                    catch (Exception ex)
                    {
                        Logger.Warn("Caught exception running InvokablePayload on {1}.  Got {0}", ringPoller.Ring.Name, ex);
                    }

                    break;
                }
                case MessageType.TimerPayload:
                {
                    try
                    {
                        var timerCallbackPayload = (ITimerCallbackPayload)data.Payload.BodyObj(PayloadRequestType.QueueReceive)!;
                        if (timerCallbackPayload.IsAsyncInvoke())
                            await timerCallbackPayload.InvokeAsync();
                        else
                            // ReSharper disable once MethodHasAsyncOverload
                            timerCallbackPayload.Invoke();
                    }
                    catch (Exception ex)
                    {
                        Logger.Warn("Caught exception running TimerPayload on {1}.  Got {0}", ringPoller.Ring.Name, ex);
                    }

                    break;
                }
                case MessageType.ListenerSubscribe:
                {
                    var subscribePayload = (IMessageListenerRegistration)data.Payload.BodyObj(PayloadRequestType.QueueReceive)!;
                    var processorRegistry = data.ProcessorRegistry;
                    processorRegistry?.RegisterStart(subscribePayload.SubscriberRule);
                    await listenerRegistry.AddListenerToWatchList(subscribePayload);
                    processorRegistry?.RegisterFinish(subscribePayload.SubscriberRule);
                    break;
                }
                case MessageType.ListenerUnsubscribe:
                {
                    var unsubscribePayload = ((Payload<MessageListenerSubscription>)data.Payload).Body(PayloadRequestType.QueueReceive)!;
                    await listenerRegistry.RemoveListenerFromWatchList(unsubscribePayload);
                    break;
                }
                case MessageType.AddListenSubscribeInterceptor:
                {
                    var subscribePayload = (IListenSubscribeInterceptor)data.Payload.BodyObj(PayloadRequestType.QueueReceive)!;
                    await listenerRegistry.AddSubscribeInterceptor(subscribePayload);
                    break;
                }
                case MessageType.RemoveListenSubscribeInterceptor:
                {
                    var unsubscribePayload = (IListenSubscribeInterceptor)data.Payload.BodyObj(PayloadRequestType.QueueReceive)!;
                    await listenerRegistry.RemoveSubscribeInterceptor(unsubscribePayload);
                    break;
                }
                default:
                {
                    CheckListenersForSubscription(data);
                    break;
                }
            }

            for (var i = 0; i < livingRules.Count; i++)
            {
                if (i < 0) // async await below may alter the living rules list
                    i = 0;
                if (livingRules.Count == 0 || i >= livingRules.Count) // async await below may alter the living rules list
                    break;
                var checkRule = livingRules[i];

                if (checkRule.LifeCycleState != RuleLifeCycle.Started || !checkRule.ShouldBeStopped()) continue;
                try
                {
                    i--;
                    await UnloadRuleAndDependents(checkRule);
                }
                catch (Exception ex)
                {
                    Logger.Warn("Caught exception stopping rule {0}, Got {1}", checkRule.FriendlyName, ex);
                }
            }

            data.DecrementCargoRefCounts();
        }
        catch (Exception e)
        {
            Logger.Warn("MessagePump id: {0} caught the following exception. ", e);
        }

        return sequence;
    }

    private void CheckListenersForSubscription(BusMessage data)
    {
        foreach (var matcherListener in listenerRegistry.MatchingSubscriptions(data.DestinationAddress!))
            try
            {
                matcherListener.Handler(data);
            }
            catch (Exception ex)
            {
                Logger.Warn("Caught exception processing message {0} on rule handler {1}.  Got {2}", data, matcherListener.SubscriberRule.FriendlyName
                    , ex);
                data.ProcessorRegistry?.RegisterFinish(matcherListener.SubscriberRule);
                data.Response?.SetException(ex);
            }
    }

    public void InitializeInPollingThread()
    {
        QueueContext.CurrentThreadQueueContext = EventContext;
        RingPollerScheduler = new SyncContextTaskScheduler();
    }

    private async ValueTask UnloadExistingRule(BusMessage data)
    {
        var toShutdown = (IListeningRule)data.Payload.BodyObj(PayloadRequestType.QueueReceive)!;
        try
        {
            if (livingRules.Contains(toShutdown) && toShutdown.LifeCycleState == RuleLifeCycle.Started)
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
            await listenerRegistry.UnsubscribeAllListenersForRule(toShutdown);
            await toShutdown.MessageBusStopAsync();
        }
        catch (Exception ex)
        {
            Logger.Warn("Caught exception stopping rule of {0}.  Caught {1}", toShutdown.FriendlyName, ex);
        }

        livingRules.Remove(toShutdown);
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
                        Logger.Warn("Problem undeploying child rule of {0}.  Child {1}.  Caught {2}", parentRule.FriendlyName, child.FriendlyName
                            , ex);
                    }
    }

    private async ValueTask LoadNewRule(BusMessage data)
    {
        var newRule = (IListeningRule)data.Payload.BodyObj(PayloadRequestType.QueueReceive)!;
        var processorRegistry = data.ProcessorRegistry!;
        processorRegistry.RegisterStart(newRule);
        try
        {
            await newRule.StartAsync();
            livingRules.Add(newRule);
            newRule.LifeCycleState = RuleLifeCycle.Started;
            processorRegistry.RegisterFinish(newRule);
            data.ProcessorRegistry!.ProcessingComplete();
        }
        catch (Exception ex)
        {
            Logger.Warn("Problem starting rule: {0}.  Caught {1}", newRule.FriendlyName, ex);
            await UnloadRuleAndDependents(newRule);
            data.ProcessorRegistry!.SetException(ex);
        }
    }
}
