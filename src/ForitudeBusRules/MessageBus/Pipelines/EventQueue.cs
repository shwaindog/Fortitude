#region

using Fortitude.EventProcessing.BusRules.MessageBus.Messages;
using Fortitude.EventProcessing.BusRules.MessageBus.Tasks;
using Fortitude.EventProcessing.BusRules.Messaging;
using Fortitude.EventProcessing.BusRules.Rules;
using FortitudeCommon.EventProcessing.Disruption.Rings.Batching;
using FortitudeCommon.EventProcessing.Disruption.Waiting;
using FortitudeCommon.Extensions;
using FortitudeCommon.Monitoring.Logging;

#endregion

namespace Fortitude.EventProcessing.BusRules.MessageBus.Pipelines;

public interface IEventQueue
{
    string Name { get; }
    void Start();
    void EnqueueMessage(Message msg);

    ValueTask<IDispatchResult> EnqueuePayloadWithStats<TPayload>(TPayload payload, IRule sender,
        ProcessorRegistry processorRegistry, string? destinationAddress = null
        , MessageType msgType = MessageType.Publish);

    void EnqueuePayload<TPayload>(TPayload payload, IRule sender,
        string? destinationAddress = null, MessageType msgType = MessageType.Publish);

    ValueTask<RequestResponse<TResponse>> RequestFromPayload<TPayload, TResponse>(TPayload payload, IRule sender,
        ProcessorRegistry processorRegistry, string? destinationAddress = null
        , MessageType msgType = MessageType.RequestResponse);

    ValueTask<IDispatchResult> LaunchRule(IRule sender, IRule rule);

    ValueTask<IDispatchResult> StopRule(IRule sender, IRule rule);

    bool IsListeningToAddress(string destinationAddress);

    void RunOn(IRule sender, Action action);

    uint NumOfMessagesReceivedRecently();

    void Shutdown();
}

public enum EventQueueType
{
    Event
    , Worker
    , IOInbound
    , IOOutbound
    , Custom
}

public class EventQueue : IEventQueue
{
    public const int MessageCountHistoryEntries = 60;
    private static IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(EventQueue));

    private readonly EventContext eventContext;
    private readonly int id;
    private readonly MessagePump messagePump;
    private readonly string name;

    private readonly uint[] recentMessageCountReceived = new uint[MessageCountHistoryEntries];
    private readonly PollingRing<Message> ring;
    private readonly EventQueueType type;
    private DateTime lastUpDateTime = DateTime.Now;

    public EventQueue(IEventBus eventBus, EventQueueType type, int id, int size)
    {
        this.type = type;
        this.id = id;
        name = $"{type}-{id}";
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
        ring.Publish(seqId);
        messagePump.WakeIfAsleep();
    }

    public ValueTask<IDispatchResult> EnqueuePayloadWithStats<TPayload>(TPayload payload, IRule sender,
        ProcessorRegistry processorRegistry, string? destinationAddress = null
        , MessageType msgType = MessageType.Publish)
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
        ring.Publish(seqId);
        messagePump.WakeIfAsleep();
        return processorRegistry.GenerateValueTask();
    }

    public ValueTask<RequestResponse<TResponse>> RequestFromPayload<TPayload, TResponse>(TPayload payload, IRule sender,
        ProcessorRegistry processorRegistry, string? destinationAddress = null
        , MessageType msgType = MessageType.RequestResponse)
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
        ring.Publish(seqId);
        messagePump.WakeIfAsleep();
        return reusableValueTaskSource.GenerateValueTask();
    }

    public ValueTask<IDispatchResult> LaunchRule(IRule sender, IRule rule)
    {
        rule.LifeCycleState = RuleLifeCycle.Starting;
        rule.Context = eventContext;
        var processorRegistry = eventContext.PooledRecycler.Borrow<ProcessorRegistry>();
        processorRegistry.DispatchResult = eventContext.PooledRecycler.Borrow<DispatchResult>();
        processorRegistry.IncrementRefCount(); // decremented when value is read for valueTask;
        processorRegistry.RecycleTimer = eventContext.Timer;
        return EnqueuePayloadWithStats(rule, sender, processorRegistry, null, MessageType.LoadRule);
    }

    public ValueTask<IDispatchResult> StopRule(IRule sender, IRule rule)
    {
        rule.LifeCycleState = RuleLifeCycle.ShuttingDown;
        rule.Context = eventContext;
        var processorRegistry = eventContext.PooledRecycler.Borrow<ProcessorRegistry>();
        processorRegistry.IncrementRefCount(); // decremented when value is read for valueTask;
        processorRegistry.DispatchResult = eventContext.PooledRecycler.Borrow<DispatchResult>();
        processorRegistry.RecycleTimer = eventContext.Timer;
        return EnqueuePayloadWithStats(rule, sender, processorRegistry, null, MessageType.UnloadRule);
    }

    public bool IsListeningToAddress(string destinationAddress) =>
        messagePump.Listeners.ContainsKey(destinationAddress);

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
        string? destinationAddress = null, MessageType msgType = MessageType.Publish)
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
        ring.Publish(seqId);
        messagePump.WakeIfAsleep();
    }

    public uint NumOfMessagesReceivedRecently()
    {
        uint total = 0;
        for (var i = 0; i < MessageCountHistoryEntries; i++) total += recentMessageCountReceived[i];
        return total;
    }

    public void Start()
    {
        messagePump.StartPolling();
    }

    public void Shutdown()
    {
        messagePump.Dispose();
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
