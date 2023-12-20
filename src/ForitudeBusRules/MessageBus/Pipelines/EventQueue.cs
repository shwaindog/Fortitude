#region

using FortitudeBusRules.MessageBus.Messages;
using FortitudeBusRules.MessageBus.Routing.SelectionStrategies;
using FortitudeBusRules.MessageBus.Tasks;
using FortitudeBusRules.Messaging;
using FortitudeBusRules.Rules;
using FortitudeCommon.EventProcessing.Disruption.Rings.Batching;
using FortitudeCommon.EventProcessing.Disruption.Waiting;
using FortitudeCommon.Extensions;
using FortitudeCommon.Monitoring.Logging;

#endregion

namespace FortitudeBusRules.MessageBus.Pipelines;

public interface IEventQueue : IComparable<IEventQueue>
{
    string Name { get; }
    EventQueueType QueueType { get; }
    int Id { get; }

    bool IsRunning { get; }

    uint RecentlyReceivedMessagesCount { get; }
    void Start();
    void EnqueueMessage(Message msg);

    ValueTask<IDispatchResult> EnqueuePayloadWithStatsAsync<TPayload>(TPayload payload, IRule sender,
        ProcessorRegistry processorRegistry, string? destinationAddress = null
        , MessageType msgType = MessageType.Publish, RuleFilter? ruleFilter = null);

    void EnqueuePayload<TPayload>(TPayload payload, IRule sender,
        string? destinationAddress = null, MessageType msgType = MessageType.Publish, RuleFilter? ruleFilter = null);

    ValueTask<RequestResponse<TResponse>> RequestFromPayloadAsync<TPayload, TResponse>(TPayload payload, IRule sender,
        ProcessorRegistry processorRegistry, string? destinationAddress = null
        , MessageType msgType = MessageType.RequestResponse, RuleFilter? ruleFilter = null);

    ValueTask<IDispatchResult> LaunchRuleAsync(IRule sender, IRule rule, RouteSelectionResult selectionResult);

    ValueTask<IDispatchResult> StopRuleAsync(IRule sender, IRule rule);

    bool IsListeningToAddress(string destinationAddress);
    int RulesListeningToAddress(ISet<IRule> toAddRules, string destinationAddress);

    void RunOn(IRule sender, Action action);

    void Shutdown();
}

public class EventQueue : IEventQueue
{
    public const int MessageCountHistoryEntries = 60;
    private static IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(EventQueue));

    private readonly EventContext eventContext;
    private readonly MessagePump messagePump;
    private readonly string name;

    private readonly uint[] recentMessageCountReceived = new uint[MessageCountHistoryEntries];
    private readonly PollingRing<Message> ring;
    private DateTime lastUpDateTime = DateTime.Now;

    public EventQueue(IEventBus eventBus, EventQueueType queueType, int id, int size)
    {
        QueueType = queueType;
        Id = id;
        name = $"{queueType}-{id}";
        ring = new PollingRing<Message>(
            $"EventQueue-{name}",
            size,
            () => new Message(),
            ClaimStrategyType.MultiProducers,
            false);
        eventContext = new EventContext(this, eventBus);
        messagePump = new MessagePump(eventContext, ring, 30, id);
    }

    public string Name => name;

    public EventQueueType QueueType { get; }

    public int Id { get; }

    public bool IsRunning => messagePump.IsRunning;

    public void EnqueueMessage(Message msg)
    {
        IncrementRecentMessageReceived();
        var seqId = ring.Claim();
        var evt = ring[seqId];
        evt.Type = msg.Type;
        evt.PayLoad = msg.PayLoad;
        evt.DestinationAddress = msg.DestinationAddress;
        evt.Response = msg.Response;
        evt.Sender = msg.Sender;
        evt.SentTime = msg.SentTime;
        evt.ProcessorRegistry = msg.ProcessorRegistry;
        evt.RuleFilter = msg.RuleFilter;
        ring.Publish(seqId);
        messagePump.WakeIfAsleep();
    }

    public ValueTask<IDispatchResult> EnqueuePayloadWithStatsAsync<TPayload>(TPayload payload, IRule sender,
        ProcessorRegistry processorRegistry, string? destinationAddress = null
        , MessageType msgType = MessageType.Publish, RuleFilter? ruleFilter = null)
    {
        IncrementRecentMessageReceived();
        var seqId = ring.Claim();
        var evt = ring[seqId];
        evt.Type = msgType;
        evt.PayLoad = new PayLoad<TPayload>(payload);
        evt.DestinationAddress = destinationAddress;
        evt.Response = Message.NoOpCompletionSource;
        evt.Sender = sender;
        evt.SentTime = DateTime.Now;
        // logger.Debug("EnqueuePayloadWithStats processorRegistry: {0}", processorRegistry.ToString());
        evt.ProcessorRegistry = processorRegistry;
        evt.RuleFilter = ruleFilter ?? Message.AppliesToAll;
        ring.Publish(seqId);
        messagePump.WakeIfAsleep();
        return processorRegistry.GenerateValueTask();
    }

    public ValueTask<RequestResponse<TResponse>> RequestFromPayloadAsync<TPayload, TResponse>(TPayload payload
        , IRule sender,
        ProcessorRegistry processorRegistry, string? destinationAddress = null
        , MessageType msgType = MessageType.RequestResponse, RuleFilter? ruleFilter = null)
    {
        IncrementRecentMessageReceived();
        var seqId = ring.Claim();
        var evt = ring[seqId];
        evt.Type = msgType;
        evt.PayLoad = new PayLoad<TPayload>(payload);
        evt.DestinationAddress = destinationAddress;
        var reusableValueTaskSource
            = eventContext.PooledRecycler.Borrow<ReusableResponseValueTaskSource<TResponse>>();
        reusableValueTaskSource.IncrementRefCount(); // decremented when value is read for valueTask;
        reusableValueTaskSource.DispatchResult = processorRegistry.DispatchResult;
        reusableValueTaskSource.RecycleTimer = eventContext.Timer;
        evt.Response = reusableValueTaskSource;
        evt.Sender = sender;
        evt.SentTime = DateTime.Now;
        evt.ProcessorRegistry = processorRegistry;
        evt.RuleFilter = ruleFilter ?? Message.AppliesToAll;
        ring.Publish(seqId);
        messagePump.WakeIfAsleep();
        return reusableValueTaskSource.GenerateValueTask();
    }

    public ValueTask<IDispatchResult> StopRuleAsync(IRule sender, IRule rule)
    {
        rule.LifeCycleState = RuleLifeCycle.ShuttingDown;
        rule.Context = eventContext;
        var processorRegistry = eventContext.PooledRecycler.Borrow<ProcessorRegistry>();
        processorRegistry.IncrementRefCount(); // decremented when value is read for valueTask;
        processorRegistry.DispatchResult = eventContext.PooledRecycler.Borrow<DispatchResult>();
        processorRegistry.RecycleTimer = eventContext.Timer;
        return EnqueuePayloadWithStatsAsync(rule, sender, processorRegistry, null, MessageType.UnloadRule);
    }

    public bool IsListeningToAddress(string destinationAddress) =>
        messagePump.Listeners.ContainsKey(destinationAddress);

    public int RulesListeningToAddress(ISet<IRule> toAddRules, string destinationAddress)
    {
        var listenersAtDestination = messagePump.Listeners[destinationAddress];
        var count = 0;
        if (listenersAtDestination == null) return count;
        foreach (var subscription in listenersAtDestination)
            if (subscription.SubscriberRule.LifeCycleState is RuleLifeCycle.Started)
            {
                count++;
                toAddRules.Add(subscription.SubscriberRule);
            }

        return count;
    }

    public void RunOn(IRule sender, Action action)
    {
        IncrementRecentMessageReceived();
        var seqId = ring.Claim();
        var evt = ring[seqId];
        evt.Type = MessageType.RunActionPayload;
        evt.PayLoad = new PayLoad<Action>(action);
        evt.DestinationAddress = null;
        evt.Response = Message.NoOpCompletionSource;
        evt.Sender = sender;
        evt.SentTime = DateTime.Now;
        ring.Publish(seqId);
        messagePump.WakeIfAsleep();
    }

    public void EnqueuePayload<TPayload>(TPayload payload, IRule sender,
        string? destinationAddress = null, MessageType msgType = MessageType.Publish, RuleFilter? ruleFilter = null)
    {
        IncrementRecentMessageReceived();
        var seqId = ring.Claim();
        var evt = ring[seqId];
        evt.PayLoad = new PayLoad<TPayload>(payload);
        evt.Type = msgType;
        evt.DestinationAddress = destinationAddress;
        evt.Response = Message.NoOpCompletionSource;
        evt.Sender = sender;
        evt.SentTime = DateTime.Now;
        evt.ProcessorRegistry = null;
        evt.RuleFilter = ruleFilter ?? Message.AppliesToAll;
        ring.Publish(seqId);
        messagePump.WakeIfAsleep();
    }

    public uint RecentlyReceivedMessagesCount
    {
        get
        {
            uint total = 0;
            var currentTimeToSecond = DateTime.Now.TruncToSecond();
            if (currentTimeToSecond != lastUpDateTime)
            {
                var timeSpanBetweenLastMessage = currentTimeToSecond - lastUpDateTime;
                if (timeSpanBetweenLastMessage > TimeSpan.FromSeconds(60))
                {
                    for (var i = 0; i < MessageCountHistoryEntries; i++) recentMessageCountReceived[i] = 0;
                }
                else
                {
                    var secondsSinceLastMessage = (int)timeSpanBetweenLastMessage.TotalSeconds;
                    for (var i = lastUpDateTime.Second + 1; i < secondsSinceLastMessage; i++)
                        recentMessageCountReceived[i % MessageCountHistoryEntries] = 0;
                }

                lastUpDateTime = currentTimeToSecond;
            }

            for (var i = 0; i < MessageCountHistoryEntries; i++) total += recentMessageCountReceived[i];
            return total;
        }
    }

    public int CompareTo(IEventQueue? other) =>
        (int)RecentlyReceivedMessagesCount - (int)(other?.RecentlyReceivedMessagesCount ?? 0);

    public void Start()
    {
        messagePump.StartPolling();
    }

    public void Shutdown()
    {
        messagePump.Dispose();
    }

    public ValueTask<IDispatchResult> LaunchRuleAsync(IRule sender, IRule rule, RouteSelectionResult selectionResult)
    {
        rule.LifeCycleState = RuleLifeCycle.Starting;
        rule.Context = eventContext;
        var processorRegistry = eventContext.PooledRecycler.Borrow<ProcessorRegistry>();
        processorRegistry.DispatchResult = eventContext.PooledRecycler.Borrow<DispatchResult>();
        processorRegistry.DispatchResult.DeploymentSelectionResult = selectionResult;
        processorRegistry.IncrementRefCount(); // decremented when value is read for valueTask;
        processorRegistry.RecycleTimer = eventContext.Timer;
        return EnqueuePayloadWithStatsAsync(rule, sender, processorRegistry, null, MessageType.LoadRule);
    }

    private void IncrementRecentMessageReceived()
    {
        var currentTimeToSecond = DateTime.Now.TruncToSecond();
        if (currentTimeToSecond != lastUpDateTime)
        {
            var timeSpanBetweenLastMessage = currentTimeToSecond - lastUpDateTime;
            if (timeSpanBetweenLastMessage > TimeSpan.FromSeconds(60))
            {
                for (var i = 0; i < MessageCountHistoryEntries; i++) recentMessageCountReceived[i] = 0;
            }
            else
            {
                var secondsSinceLastMessage = (int)timeSpanBetweenLastMessage.TotalSeconds;
                for (var i = lastUpDateTime.Second + 1; i < secondsSinceLastMessage; i++)
                    recentMessageCountReceived[i % MessageCountHistoryEntries] = 0;
            }

            lastUpDateTime = currentTimeToSecond;
        }

        recentMessageCountReceived[currentTimeToSecond.Second] += 1;
    }

    public override string ToString() => $"{GetType().Name}({nameof(name)}: \"{name}\")";
}
