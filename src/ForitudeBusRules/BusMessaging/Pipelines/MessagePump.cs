#region

using FortitudeBusRules.BusMessaging.Messages;
using FortitudeBusRules.BusMessaging.Messages.ListeningSubscriptions;
using FortitudeBusRules.BusMessaging.Tasks;
using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;
using FortitudeCommon.AsyncProcessing.Tasks;
using FortitudeCommon.Chronometry.Timers;
using FortitudeCommon.EventProcessing.Disruption.Rings;
using FortitudeCommon.EventProcessing.Disruption.Rings.PollingRings;
using FortitudeCommon.Monitoring.Logging;

#endregion


namespace FortitudeBusRules.BusMessaging.Pipelines;

public interface IMessagePump
{
    bool IsListeningOn(string address);
    IEnumerable<IMessageListenerSubscription> ListeningSubscriptionsOn(string address);

    event Action<QueueEventTime> MessageStartProcessingTime;
    event Action<QueueEventTime> MessageFinishProcessingTime;
}

public class MessagePump : IMessagePump
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(MessagePump));

    private static readonly Action<ValueTask, object?> RuleStarted = RuleStartedCallback;
    private static readonly Action<ValueTask, object?> RuleStopped = RuleStoppedCallback;

    private readonly ListenerRegistry listenerRegistry = new();

    private readonly List<IListeningRule> livingRules = new();
    private readonly IAsyncValueTaskRingPoller<BusMessage> ringPoller;

    public MessagePump(IAsyncValueTaskRingPoller<BusMessage> ringPoller, QueueContext? eventContext = null)
    {
        this.ringPoller = ringPoller;
        this.ringPoller.ProcessEvent = Processor;
        EventContext = eventContext;
    }

    public SyncContextTaskScheduler RingPollerScheduler { get; private set; } = null!;
    public QueueContext? EventContext { get; set; }

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

    public IEnumerable<IMessageListenerSubscription> ListeningSubscriptionsOn(string address) => listenerRegistry.MatchingSubscriptions(address);

    public void Dispose()
    {
        GC.SuppressFinalize(this);
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
        ringPoller.Stop();
    }

    public async ValueTask<long> Processor(long sequence, BusMessage data)
    {
        try
        {
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
                        var actionBody = ((PayLoad<Action>)data.PayLoad!).Body!;
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
                        var actionBody = (IInvokeablePayload)data.PayLoad!.BodyObj!;
                        actionBody?.Invoke();
                    }
                    catch (Exception ex)
                    {
                        Logger.Warn("Caught exception running InvokeablePayload on {1}.  Got {0}", ringPoller.Ring.Name, ex);
                    }

                    break;
                }
                case MessageType.TimerPayload:
                {
                    try
                    {
                        var timerCallbackPayload = (ITimerCallbackPayload)data.PayLoad!.BodyObj!;
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
                    var subscribePayLoad = (IMessageListenerSubscription)data.PayLoad!.BodyObj!;
                    var processorRegistry = data.ProcessorRegistry;
                    processorRegistry?.RegisterStart(subscribePayLoad.SubscriberRule);
                    listenerRegistry.AddListenerToWatchList(subscribePayLoad);
                    processorRegistry?.RegisterFinish(subscribePayLoad.SubscriberRule);
                    break;
                }
                case MessageType.ListenerUnsubscribe:
                {
                    var unsubscribePayLoad = ((PayLoad<MessageListenerUnsubscribe>)data.PayLoad!).Body!;
                    listenerRegistry.RemoveListenerFromWatchList(unsubscribePayLoad);
                    break;
                }
                case MessageType.AddListenSubscribeInterceptor:
                {
                    var subscribePayLoad = (IListenSubscribeInterceptor)data.PayLoad!.BodyObj!;
                    listenerRegistry.AddSubscribeInterceptor(subscribePayLoad);
                    break;
                }
                case MessageType.RemoveListenSubscribeInterceptor:
                {
                    var unsubscribePayLoad = (IListenSubscribeInterceptor)data.PayLoad!.BodyObj!;
                    listenerRegistry.RemoveSubscribeInterceptor(unsubscribePayLoad);
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
                var checkRule = livingRules[i];
                if (checkRule.ShouldBeStopped()) await checkRule.StopAsync();
                livingRules.RemoveAt(i--);
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
        RingPollerScheduler = new SyncContextTaskScheduler();
    }

    private static void RuleStartedCallback(ValueTask launchTask, object? state)
    {
        if (state is IProcessorRegistry processorRegistry)
        {
            processorRegistry.RulePayLoad!.LifeCycleState = launchTask.IsCompletedSuccessfully ?
                RuleLifeCycle.Started :
                RuleLifeCycle.ShuttingDown;
            processorRegistry.RegisterFinish(processorRegistry.RulePayLoad);
            processorRegistry.ProcessingComplete();
        }
    }

    private static void RuleStoppedCallback(ValueTask launchTask, object? state)
    {
        if (state is IProcessorRegistry processorRegistry)
        {
            processorRegistry.RulePayLoad!.LifeCycleState = RuleLifeCycle.Stopped;
            processorRegistry.RegisterFinish(processorRegistry.RulePayLoad);
            processorRegistry.ProcessingComplete();
        }
    }

    private async ValueTask UnloadExistingRule(BusMessage data)
    {
        var toShutdown = (IListeningRule)data.PayLoad!.BodyObj!;
        try
        {
            if (livingRules.Contains(toShutdown) && toShutdown.LifeCycleState == RuleLifeCycle.Started)
            {
                await ForceUnloadRule(data, toShutdown);
                data.ProcessorRegistry!.ProcessingComplete();
            }
        }
        catch (Exception ex)
        {
            Logger.Warn("Problem starting rule: {0}.  Caught {1}", toShutdown.FriendlyName, ex);
        }
    }

    private async Task ForceUnloadRule(BusMessage data, IListeningRule toShutdown)
    {
        await UndeployChildren(toShutdown);
        listenerRegistry.UnsubscribeAllListenersForRule(toShutdown);
        try
        {
            await toShutdown.StopAsync();
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
        var newRule = (IListeningRule)data.PayLoad!.BodyObj!;
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
            await ForceUnloadRule(data, newRule);
            data.ProcessorRegistry!.SetException(ex);
        }
    }
}
