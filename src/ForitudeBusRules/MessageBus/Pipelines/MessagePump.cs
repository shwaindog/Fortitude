#region

using FortitudeBusRules.MessageBus.Messages;
using FortitudeBusRules.MessageBus.Pipelines.Timers;
using FortitudeBusRules.MessageBus.Tasks;
using FortitudeBusRules.Messaging;
using FortitudeBusRules.Rules;
using FortitudeCommon.AsyncProcessing.Tasks;
using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.EventProcessing.Disruption.Rings.Batching;
using FortitudeCommon.Monitoring.Logging;

#endregion


namespace FortitudeBusRules.MessageBus.Pipelines;

public interface IMessagePump : IPollSink<Message>, IRingPoller { }

public class MessagePump : IMessagePump
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(MessagePump));

    private static readonly Action<ValueTask, object?> RuleStarted = RuleStartedCallback;
    private static readonly Action<ValueTask, object?> RuleStopped = RuleStoppedCallback;
    private readonly List<string> destinationAddresses = [];

    private readonly List<IListeningRule> livingRules = [];
    private readonly IRingPollerSink<Message> ringPoller;

    public IMap<string, List<IMessageListenerSubscription>> Listeners
        = new ConcurrentMap<string, List<IMessageListenerSubscription>>();

    private MessagePumpSyncContext syncContext = null!;

    public MessagePump(IRingPollerSink<Message> ringPoller, EventContext? eventContext = null)
    {
        this.ringPoller = ringPoller;
        this.ringPoller.PollSink = this;
        EventContext = eventContext;
    }

    public MessagePumpTaskScheduler RingPollerScheduler { get; private set; } = null!;
    public EventContext? EventContext { get; set; }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        ringPoller.Dispose();
    }

    public bool IsRunning => ringPoller.IsRunning;

    public int UsageCount => ringPoller.UsageCount;

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

    public void Processor(long sequence, long batchSize, Message data, bool startOfBatch,
        bool endOfBatch)
    {
        try
        {
            switch (data.Type)
            {
                case MessageType.LoadRule:
                    LoadNewRule(data);
                    break;
                case MessageType.UnloadRule:
                    UnloadExistingRule(data);
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
                case MessageType.TaskAction:
                {
                    var timerCallbackPayload = (ITaskPayload)data.PayLoad!.BodyObj!;
                    timerCallbackPayload.Invoke();
                    break;
                }
                case MessageType.ListenerSubscribe:
                {
                    var subscribePayLoad = (IMessageListenerSubscription)data.PayLoad!.BodyObj!;
                    subscribePayLoad.SubscriberRule.IncrementLifeTimeCount();
                    var listeningAddress = subscribePayLoad.PublishAddress;
                    if (!Listeners.TryGetValue(listeningAddress, out var ruleListeners))
                    {
                        ruleListeners = [];
                        Listeners.Add(listeningAddress, ruleListeners);
                    }

                    ruleListeners!.Add(subscribePayLoad);
                    break;
                }
                case MessageType.ListenerUnsubscribe:
                {
                    var unsubscribePayLoad = ((PayLoad<MessageListenerUnsubscribe>)data.PayLoad!).Body!;
                    var listeningAddress = unsubscribePayLoad.PublishAddress;
                    if (Listeners.TryGetValue(listeningAddress, out var ruleListeners))
                    {
                        for (var i = 0; i < ruleListeners!.Count; i++)
                        {
                            var ruleListener = ruleListeners[i];
                            if (ruleListener.SubscriberId == unsubscribePayLoad.SubscriberId) ruleListeners.RemoveAt(i);
                        }

                        if (ruleListeners.Count == 0) Listeners.Remove(listeningAddress);
                    }

                    unsubscribePayLoad.SubscriberRule.DecrementLifeTimeCount();
                    break;
                }
                default:
                {
                    if (Listeners.TryGetValue(data.DestinationAddress!, out var ruleListeners))
                        foreach (var ruleListener in ruleListeners!)
                            ruleListener.Handler(data);

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
    }

    public void InitializeInPollingThread()
    {
        syncContext = new MessagePumpSyncContext(EventContext!);
        SynchronizationContext.SetSynchronizationContext(syncContext);
        RingPollerScheduler = new MessagePumpTaskScheduler();
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

    private void UnloadExistingRule(Message data)
    {
        var toShutdown = (IListeningRule)data.PayLoad!.BodyObj!;
        try
        {
            if (livingRules.Contains(toShutdown) && toShutdown.LifeCycleState == RuleLifeCycle.Started)
            {
                UndeployChildren(toShutdown);
                UnsubscribeAllListenersForRule(toShutdown);
                var stopTask = toShutdown.StopAsync();
                livingRules.Remove(toShutdown);

                if (stopTask.IsCompleted)
                {
                    toShutdown.LifeCycleState = RuleLifeCycle.Stopped;
                    data.ProcessorRegistry!.ProcessingComplete();
                }
                else
                {
                    data.ProcessorRegistry!.IncrementRefCount();
                    data.ProcessorRegistry.RulePayLoad = toShutdown;
                    var _ = stopTask.ContinueWith(RuleStopped, data.ProcessorRegistry);
                }
            }
        }
        catch (Exception ex)
        {
            Logger.Warn("Problem starting rule: {0}.  Caught {1}", toShutdown.FriendlyName, ex);
        }
    }

    private static void UndeployChildren(IRule parentRule)
    {
        if (parentRule.ChildRules.Any())
            foreach (var child in parentRule.ChildRules)
                if (child.LifeCycleState == RuleLifeCycle.Started)
                {
                    var _ = child.Context.RegisteredOn.StopRuleAsync(parentRule, child);
                }
    }

    private void UnsubscribeAllListenersForRule(IRule removeListeners)
    {
        destinationAddresses.Clear();
        foreach (var listenKvp in Listeners)
        {
            for (var i = 0; i < listenKvp.Value.Count; i++)
            {
                var currentListener = listenKvp.Value[i];
                if (currentListener.SubscriberRule == removeListeners) listenKvp.Value.RemoveAt(i--);
            }

            if (listenKvp.Value.Count <= 0) destinationAddresses.Add(listenKvp.Key);
        }

        for (var i = 0; i < destinationAddresses.Count; i++)
        {
            var emptyDestinationAddress = destinationAddresses[i];
            Listeners.Remove(emptyDestinationAddress);
        }
    }

    private void LoadNewRule(Message data)
    {
        var newRule = (IListeningRule)data.PayLoad!.BodyObj!;
        try
        {
            var processorRegistry = data.ProcessorRegistry!;
            processorRegistry.RegisterStart(newRule);
            var started = newRule.StartAsync();
            livingRules.Add(newRule);
            if (started.IsCompleted)
            {
                newRule.LifeCycleState = RuleLifeCycle.Started;
                processorRegistry.RegisterFinish(newRule);
                data.ProcessorRegistry!.ProcessingComplete();
            }
            else
            {
                processorRegistry.RegisterAwaiting(newRule);
                processorRegistry.RulePayLoad = newRule;
                var _ = started.ContinueWith(RuleStarted, processorRegistry);
            }
        }
        catch (Exception ex)
        {
            Logger.Warn("Problem starting rule: {0}.  Caught {1}", newRule.FriendlyName, ex);
        }
    }
}
