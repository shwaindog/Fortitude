#region

using FortitudeBusRules.MessageBus.Messages;
using FortitudeBusRules.MessageBus.Messages.ListeningSubscriptions;
using FortitudeBusRules.Messaging;
using FortitudeBusRules.Rules;
using FortitudeCommon.AsyncProcessing.Tasks;
using FortitudeCommon.Chronometry.Timers;
using FortitudeCommon.EventProcessing.Disruption.Rings.PollingRings;
using FortitudeCommon.Monitoring.Logging;

#endregion


namespace FortitudeBusRules.MessageBus.Pipelines;

public interface IMessagePump
{
    bool IsListeningOn(string address);
    IEnumerable<IMessageListenerSubscription> ListeningSubscriptionsOn(string address);
}

public class MessagePump : IMessagePump
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(MessagePump));

    private static readonly Action<ValueTask, object?> RuleStarted = RuleStartedCallback;
    private static readonly Action<ValueTask, object?> RuleStopped = RuleStoppedCallback;

    private readonly ListenerRegistry listenerRegistry = new();

    private readonly List<IListeningRule> livingRules = new();
    private readonly IAsyncValueTaskRingPoller<Message> ringPoller;

    public MessagePump(IAsyncValueTaskRingPoller<Message> ringPoller, EventContext? eventContext = null)
    {
        this.ringPoller = ringPoller;
        this.ringPoller.ProcessEvent = Processor;
        EventContext = eventContext;
    }

    public SyncContextTaskScheduler RingPollerScheduler { get; private set; } = null!;
    public EventContext? EventContext { get; set; }

    public bool IsRunning => ringPoller.IsRunning;

    public int UsageCount => ringPoller.UsageCount;

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

    public async ValueTask<long> Processor(long sequence, Message data)
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
                    var actionBody = ((PayLoad<Action>)data.PayLoad!).Body!;
                    actionBody();
                    break;
                }
                case MessageType.TimerPayload:
                {
                    var timerCallbackPayload = (ITimerCallbackPayload)data.PayLoad!.BodyObj!;
                    timerCallbackPayload.Invoke();
                    break;
                }
                case MessageType.ListenerSubscribe:
                {
                    var subscribePayLoad = (IMessageListenerSubscription)data.PayLoad!.BodyObj!;
                    listenerRegistry.AddListenerToWatchList(subscribePayLoad);
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
                if (checkRule.ShouldBeStopped()) checkRule.StopAsync();
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

    private void CheckListenersForSubscription(Message data)
    {
        foreach (var matcherListener in listenerRegistry.MatchingSubscriptions(data.DestinationAddress!)) matcherListener.Handler(data);
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

    private async ValueTask UnloadExistingRule(Message data)
    {
        var toShutdown = (IListeningRule)data.PayLoad!.BodyObj!;
        try
        {
            if (livingRules.Contains(toShutdown) && toShutdown.LifeCycleState == RuleLifeCycle.Started)
            {
                await UndeployChildren(toShutdown);
                listenerRegistry.UnsubscribeAllListenersForRule(toShutdown);
                await toShutdown.StopAsync();
                livingRules.Remove(toShutdown);
                toShutdown.LifeCycleState = RuleLifeCycle.Stopped;
                data.ProcessorRegistry!.ProcessingComplete();
            }
        }
        catch (Exception ex)
        {
            Logger.Warn("Problem starting rule: {0}.  Caught {1}", toShutdown.FriendlyName, ex);
        }
    }

    private static async ValueTask UndeployChildren(IRule parentRule)
    {
        if (parentRule.ChildRules.Any())
            foreach (var child in parentRule.ChildRules)
                if (child.LifeCycleState == RuleLifeCycle.Started)
                    await child.Context.RegisteredOn.StopRuleAsync(parentRule, child);
    }

    private async ValueTask LoadNewRule(Message data)
    {
        var newRule = (IListeningRule)data.PayLoad!.BodyObj!;
        try
        {
            var processorRegistry = data.ProcessorRegistry!;
            processorRegistry.RegisterStart(newRule);
            await newRule.StartAsync();
            livingRules.Add(newRule);
            newRule.LifeCycleState = RuleLifeCycle.Started;
            processorRegistry.RegisterFinish(newRule);
            data.ProcessorRegistry!.ProcessingComplete();
        }
        catch (Exception ex)
        {
            Logger.Warn("Problem starting rule: {0}.  Caught {1}", newRule.FriendlyName, ex);
        }
    }
}
