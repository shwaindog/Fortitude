#region

using FortitudeBusRules.MessageBus.Messages;
using FortitudeBusRules.MessageBus.Pipelines.Execution;
using FortitudeBusRules.MessageBus.Routing.SelectionStrategies;
using FortitudeBusRules.MessageBus.Tasks;
using FortitudeBusRules.Messaging;
using FortitudeBusRules.Rules;
using FortitudeCommon.AsyncProcessing.Tasks;
using FortitudeCommon.EventProcessing.Disruption.Rings;
using FortitudeCommon.EventProcessing.Disruption.Rings.PollingRings;
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

    QueueEventTime LatestMessageStartProcessing { get; }
    QueueEventTime LatestMessageFinishedProcessing { get; }

    IEventContext Context { get; }
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

    IAlternativeExecutionContextAction<TP> GetExecutionContextAction<TP>(IRule sender);
    IAlternativeExecutionContextResult<TR> GetExecutionContextResult<TR>(IRule sender);
    IAlternativeExecutionContextResult<TR, TP> GetExecutionContextResult<TR, TP>(IRule sender);

    void RunOn(IRule sender, Action action);

    void Shutdown();
}

public class EventQueue : IEventQueue
{
    public const int MessageCountHistoryEntries = 60;
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(EventQueue));

    private readonly EventContext eventContext;
    private readonly string name;

    private readonly uint[] recentMessageCountReceived = new uint[MessageCountHistoryEntries];
    private readonly IPollingRing<Message> ring;

    private DateTime lastUpDateTime = DateTime.Now;

    public EventQueue(IConfigureEventBus eventBus, EventQueueType queueType, int id, IAsyncValueTaskRingPoller<Message> ringPoller)
    {
        QueueType = queueType;
        Id = id;
        name = ringPoller.Ring.Name;
        ring = ringPoller.Ring;
        eventContext = new EventContext(this, eventBus);
        ringPoller.Recycler = eventContext.PooledRecycler;
        LatestMessageStartProcessing = new QueueEventTime(-1, DateTime.UtcNow);
        LatestMessageFinishedProcessing = new QueueEventTime(-1, DateTime.UtcNow);
        MessagePump = new MessagePump(ringPoller, eventContext);
        MessagePump.MessageStartProcessingTime += SetLatestStarted;
        MessagePump.MessageFinishProcessingTime += SetLatestFinished;
    }

    public MessagePump MessagePump { get; set; }

    public string Name => name;

    public EventQueueType QueueType { get; }

    public IEventContext Context => eventContext;

    public int Id { get; }

    public bool IsRunning => MessagePump.IsRunning;

    public QueueEventTime LatestMessageStartProcessing { get; private set; }
    public QueueEventTime LatestMessageFinishedProcessing { get; private set; }

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
        MessagePump.WakeIfAsleep();
    }

    public ValueTask<IDispatchResult> EnqueuePayloadWithStatsAsync<TPayload>(TPayload payload, IRule sender,
        ProcessorRegistry processorRegistry, string? destinationAddress = null
        , MessageType msgType = MessageType.Publish, RuleFilter? ruleFilter = null)
    {
        IncrementRecentMessageReceived();
        var seqId = ring.Claim();
        var evt = ring[seqId];
        var payLoadContainer = eventContext.PooledRecycler.Borrow<PayLoad<TPayload>>();
        payLoadContainer.Body = payload;
        evt.Type = msgType;
        evt.PayLoad = payLoadContainer;
        evt.DestinationAddress = destinationAddress;
        evt.Response = Message.NoOpCompletionSource;
        evt.Sender = sender;
        evt.SentTime = DateTime.Now;
        // logger.Debug("EnqueuePayloadWithStats processorRegistry: {0}", processorRegistry.ToString());
        evt.ProcessorRegistry = processorRegistry;
        evt.RuleFilter = ruleFilter ?? Message.AppliesToAll;
        ring.Publish(seqId);
        MessagePump.WakeIfAsleep();
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
        var payLoadContainer = eventContext.PooledRecycler.Borrow<PayLoad<TPayload>>();
        payLoadContainer.Body = payload;
        evt.Type = msgType;
        evt.PayLoad = payLoadContainer;
        evt.DestinationAddress = destinationAddress;
        var reusableValueTaskSource
            = eventContext.PooledRecycler.Borrow<ReusableResponseValueTaskSource<TResponse>>();
        reusableValueTaskSource.IncrementRefCount(); // decremented when value is read for valueTask;
        reusableValueTaskSource.DispatchResult = processorRegistry.DispatchResult;
        reusableValueTaskSource.ResponseTimeoutAndRecycleTimer = eventContext.Timer;
        evt.Response = reusableValueTaskSource;
        evt.Sender = sender;
        evt.SentTime = DateTime.Now;
        evt.ProcessorRegistry = processorRegistry;
        evt.RuleFilter = ruleFilter ?? Message.AppliesToAll;
        ring.Publish(seqId);
        MessagePump.WakeIfAsleep();
        return reusableValueTaskSource.GenerateValueTask();
    }

    public ValueTask<IDispatchResult> StopRuleAsync(IRule sender, IRule rule)
    {
        rule.LifeCycleState = RuleLifeCycle.ShuttingDown;
        rule.Context = eventContext;
        var processorRegistry = eventContext.PooledRecycler.Borrow<ProcessorRegistry>();
        processorRegistry.IncrementRefCount(); // decremented when value is read for valueTask;
        processorRegistry.DispatchResult = eventContext.PooledRecycler.Borrow<DispatchResult>();
        processorRegistry.ResponseTimeoutAndRecycleTimer = eventContext.Timer;
        return EnqueuePayloadWithStatsAsync(rule, sender, processorRegistry, null, MessageType.UnloadRule);
    }

    public bool IsListeningToAddress(string destinationAddress) => MessagePump.IsListeningOn(destinationAddress);

    public int RulesListeningToAddress(ISet<IRule> toAddRules, string destinationAddress)
    {
        var listenersAtDestination = MessagePump.ListeningSubscriptionsOn(destinationAddress);
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
        var payLoadContainer = eventContext.PooledRecycler.Borrow<PayLoad<Action>>();
        payLoadContainer.Body = action;
        evt.Type = MessageType.RunActionPayload;
        evt.PayLoad = payLoadContainer;
        evt.DestinationAddress = null;
        evt.Response = Message.NoOpCompletionSource;
        evt.Sender = sender;
        evt.SentTime = DateTime.Now;
        ring.Publish(seqId);
        MessagePump.WakeIfAsleep();
    }

    public void EnqueuePayload<TPayload>(TPayload payload, IRule sender,
        string? destinationAddress = null, MessageType msgType = MessageType.Publish, RuleFilter? ruleFilter = null)
    {
        IncrementRecentMessageReceived();
        var seqId = ring.Claim();
        var evt = ring[seqId];
        var payLoadContainer = eventContext.PooledRecycler.Borrow<PayLoad<TPayload>>();
        payLoadContainer.Body = payload;
        evt.PayLoad = payLoadContainer;
        evt.Type = msgType;
        evt.DestinationAddress = destinationAddress;
        evt.Response = Message.NoOpCompletionSource;
        evt.Sender = sender;
        evt.SentTime = DateTime.Now;
        evt.ProcessorRegistry = null;
        evt.RuleFilter = ruleFilter ?? Message.AppliesToAll;
        ring.Publish(seqId);
        MessagePump.WakeIfAsleep();
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

    public int CompareTo(IEventQueue? other) => (int)RecentlyReceivedMessagesCount - (int)(other?.RecentlyReceivedMessagesCount ?? 0);

    public void Start()
    {
        MessagePump?.Start();
    }

    public void Shutdown()
    {
        MessagePump.Dispose();
    }

    public ValueTask<IDispatchResult> LaunchRuleAsync(IRule sender, IRule rule, RouteSelectionResult selectionResult)
    {
        rule.LifeCycleState = RuleLifeCycle.Starting;
        rule.Context = eventContext;
        var processorRegistry = eventContext.PooledRecycler.Borrow<ProcessorRegistry>();
        processorRegistry.DispatchResult = eventContext.PooledRecycler.Borrow<DispatchResult>();
        processorRegistry.DispatchResult.DeploymentSelectionResult = selectionResult;
        processorRegistry.IncrementRefCount(); // decremented when value is read for valueTask;
        processorRegistry.ResponseTimeoutAndRecycleTimer = eventContext.Timer;
        return EnqueuePayloadWithStatsAsync(rule, sender, processorRegistry, null, MessageType.LoadRule);
    }

    public IAlternativeExecutionContextAction<TP> GetExecutionContextAction<TP>(IRule sender)
    {
        var executionContext = Context.PooledRecycler.Borrow<EventQueueExecutionContextAction<TP>>();
        executionContext.Configure(this, sender);
        return executionContext;
    }

    public IAlternativeExecutionContextResult<TR> GetExecutionContextResult<TR>(IRule sender)
    {
        var executionContext = Context.PooledRecycler.Borrow<EventQueueExecutionContextResult<TR>>();
        executionContext.Configure(this, sender);
        return executionContext;
    }

    public IAlternativeExecutionContextResult<TR, TP> GetExecutionContextResult<TR, TP>(IRule sender)
    {
        var executionContext = Context.PooledRecycler.Borrow<EventQueueExecutionContextResult<TR, TP>>();
        executionContext.Configure(this, sender);
        return executionContext;
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

    private void SetLatestStarted(QueueEventTime queueEventTime)
    {
        LatestMessageStartProcessing = queueEventTime;
    }

    private void SetLatestFinished(QueueEventTime queueEventTime)
    {
        LatestMessageFinishedProcessing = queueEventTime;
    }

    public override string ToString() => $"{GetType().Name}({nameof(name)}: \"{name}\")";
}
