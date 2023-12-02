#region

using Fortitude.EventProcessing.BusRules.MessageBus.Messages;
using Fortitude.EventProcessing.BusRules.MessageBus.Pipelines.Timers;
using Fortitude.EventProcessing.BusRules.MessageBus.Tasks;
using Fortitude.EventProcessing.BusRules.Messaging;
using Fortitude.EventProcessing.BusRules.Rules;
using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.EventProcessing.Disruption.Rings.Batching;
using FortitudeCommon.Monitoring.Logging;

#endregion


namespace Fortitude.EventProcessing.BusRules.MessageBus.Pipelines;

public class MessagePump : RingPoller<Message>
{
    public const string RunActionAddress = "RunActionAddress";
    public const string RunTaskAddress = "RunTaskAddress";
    private static IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(MessagePump));

    private static readonly Action<Task, object?> RuleStated = RuleStartedCallback;
    private static readonly Action<Task, object?> RuleStopped = RuleStoppedCallback;
    private readonly List<string> destinationAddresses = new();

    public IMap<string, List<IMessageListenerSubscription>> listeners
        = new ConcurrentMap<string, List<IMessageListenerSubscription>>();

    private List<IListeningRule> livingRules = new();

    private MessagePumpSyncContext syncContext = null!;

    public MessagePump(PollingRing<Message> ring, uint timeoutMs, int id)
        : base(ring, timeoutMs) { }


    public MessagePumpTaskScheduler RingPollerScheduler { get; private set; } = null!;

    protected override void InitializeInPollingThread()
    {
        syncContext = new MessagePumpSyncContext();
        SynchronizationContext.SetSynchronizationContext(syncContext);
        RingPollerScheduler = new MessagePumpTaskScheduler();
    }

    protected override void CurrentPollingIterationComplete()
    {
        syncContext.RunQueuedTasks();
    }

    private static void RuleStartedCallback(Task launchTask, object? state)
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

    private static void RuleStoppedCallback(Task launchTask, object? state)
    {
        if (state is IProcessorRegistry processorRegistry)
        {
            processorRegistry.RulePayLoad!.LifeCycleState = RuleLifeCycle.Stopped;
            processorRegistry.RegisterFinish(processorRegistry.RulePayLoad);
            processorRegistry.ProcessingComplete();
        }
    }

    protected override void Processor(long sequence, long batchSize, Message data, bool startOfBatch,
        bool endOfBatch)
    {
        try
        {
            if (data.Type == MessageType.LoadRule)
            {
                LoadNewRule(data);
            }
            else if (data.Type == MessageType.UnloadRule)
            {
                UnloadExistingRule(data);
            }
            else if (data.Type == MessageType.RunActionPayload)
            {
                var actionBody = ((PayLoad<Action>)data.PayLoad!).Body!;
                actionBody();
            }
            else if (data.Type == MessageType.TimerPayload)
            {
                var timerCallbackPayload = (ITimerCallbackPayload)data.PayLoad!.BodyObj!;
                timerCallbackPayload.Invoke();
            }
            else if (data.Type == MessageType.ListenerSubscribe)
            {
                var subscribePayLoad = (IMessageListenerSubscription)data.PayLoad!.BodyObj!;
                subscribePayLoad.SubscriberRule.IncrementLifeTimeCount();
                var listeningAddress = subscribePayLoad.PublishAddress;
                if (!listeners.TryGetValue(subscribePayLoad.PublishAddress, out var ruleListeners))
                {
                    ruleListeners = new List<IMessageListenerSubscription>();
                    listeners.Add(subscribePayLoad.PublishAddress, ruleListeners);
                }

                ruleListeners!.Add(subscribePayLoad);
            }
            else if (data.Type == MessageType.ListenerUnsubscribe)
            {
                var unsubscribePayLoad = ((PayLoad<MessageListenerUnsubscribe>)data.PayLoad!).Body!;
                var listeningAddress = unsubscribePayLoad.PublishAddress;
                if (listeners.TryGetValue(unsubscribePayLoad.PublishAddress, out var ruleListeners))
                {
                    for (var i = 0; i < ruleListeners!.Count; i++)
                    {
                        var ruleListener = ruleListeners![i];
                        if (ruleListener.SubscriberId == unsubscribePayLoad.SubscriberId) ruleListeners.RemoveAt(i);
                    }

                    if (ruleListeners.Count == 0) listeners.Remove(unsubscribePayLoad.PublishAddress);
                }

                unsubscribePayLoad.SubscriberRule.DecrementLifeTimeCount();
            }
            else if (listeners.TryGetValue(data.DestinationAddress!, out var ruleListeners))
            {
                foreach (var ruleListener in ruleListeners!) ruleListener.Handler(data);
            }

            for (var i = 0; i < livingRules.Count; i++)
            {
                var checkRule = livingRules[i];
                if (checkRule.ShouldBeStopped()) checkRule.Stop();
                livingRules.RemoveAt(i--);
            }

            data.DecrementCargoRefCounts();
        }
        catch (Exception e)
        {
            logger.Warn("MessagePump id: {0} caught the following exception. ", e);
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
                var stopTask = toShutdown.Stop();
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
                    stopTask.ContinueWith(RuleStopped, data.ProcessorRegistry);
                }
            }
        }
        catch (Exception ex)
        {
            logger.Warn("Problem starting rule: {0}.  Caught {1}", toShutdown.FriendlyName, ex);
        }
    }

    private void UndeployChildren(IRule parentRule)
    {
        var isWaitingForChildToShuttdown = false;
        if (parentRule.ChildRules.Any())
            foreach (var child in parentRule.ChildRules)
                if (child.LifeCycleState == RuleLifeCycle.Started)
                {
                    if (!isWaitingForChildToShuttdown) parentRule.LifeCycleState = RuleLifeCycle.AwaitingChildShutdown;

                    child.Context.RegisteredOn.StopRule(parentRule, child);
                }
    }

    private void UnsubscribeAllListenersForRule(IRule removeListeners)
    {
        destinationAddresses.Clear();
        foreach (var listenKvp in listeners)
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
            listeners.Remove(emptyDestinationAddress);
        }
    }

    private void LoadNewRule(Message data)
    {
        var newRule = (IListeningRule)data.PayLoad!.BodyObj!;
        try
        {
            var processorRegistry = data.ProcessorRegistry!;
            processorRegistry.RegisterStart(newRule);
            var started = newRule.Start();
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
                started.ContinueWith(RuleStated, processorRegistry);
            }
        }
        catch (Exception ex)
        {
            logger.Warn("Problem starting rule: {0}.  Caught {1}", newRule.FriendlyName, ex);
        }
    }
}
