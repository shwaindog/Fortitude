#region

using Fortitude.EventProcessing.BusRules.MessageBus.Messages;
using Fortitude.EventProcessing.BusRules.MessageBus.Tasks;
using Fortitude.EventProcessing.BusRules.Messaging;
using Fortitude.EventProcessing.BusRules.Rules;
using FortitudeCommon.EventProcessing.Disruption.Rings.Batching;
using FortitudeCommon.EventProcessing.Disruption.Waiting;

#endregion

namespace Fortitude.EventProcessing.BusRules.MessageBus;

public interface IEventQueue
{
    string Name { get; }
    void EnqueueMessage(Message msg);

    ValueTask<IDispatchResult> EnqueuePayloadWithStats<TPayload>(TPayload payload, IRule sender,
        ProcessorRegistry processorRegistry, string? destinationAddress = null
        , MessageType msgType = MessageType.Publish);

    void EnqueuePayload<TPayload>(TPayload payload, IRule sender,
        string? destinationAddress = null, MessageType msgType = MessageType.Publish);

    ValueTask<RequestResponse<TResponse>> RequestFromPayload<TPayload, TResponse>(TPayload payload, IRule sender,
        ProcessorRegistry processorRegistry, string? destinationAddress = null
        , MessageType msgType = MessageType.Publish);

    ValueTask<IDispatchResult> LaunchRule(IRule sender, IRule rule);

    bool IsListeningToAddress(string destinationAddress);

    void RunOn(IRule sender, Action action);
}

public enum EventQueueType
{
    Event
    , Worker
}

public class EventQueue : IEventQueue
{
    private readonly EventContext eventContext;
    private readonly int id;
    private readonly MessagePump messagePump;
    private readonly PollingRing<Message> ring;
    private readonly EventQueueType type;


    public EventQueue(EventBus eventBus, EventQueueType type, int id, int size)
    {
        this.type = type;
        this.id = id;
        ring = new PollingRing<Message>(
            $"EventQueue-{type}-{id}",
            size,
            () => new Message(),
            ClaimStrategyType.MultiProducers,
            false);
        eventContext = new EventContext(this, eventBus);
        messagePump = new MessagePump(ring, 30, id);
        messagePump.StartPolling();
    }

    public string Name => $"{type}-{id}";

    public void EnqueueMessage(Message msg)
    {
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
        var seqId = ring.Claim();
        var evt = ring[seqId];
        evt.Type = msgType;
        evt.PayLoad = new PayLoad<TPayload>(payload);
        evt.DestinationAddress = destinationAddress;
        evt.Response = Message.NoOpCompletionSource;
        evt.Sender = sender;
        evt.SentTime = DateTime.Now;
        var responseResult = new ValueTask<IDispatchResult>(processorRegistry, (short)(processorRegistry.Version + 1));
        evt.ProcessorRegistry = processorRegistry;
        ring.Publish(seqId);
        messagePump.WakeIfAsleep();
        return responseResult;
    }

    public ValueTask<RequestResponse<TResponse>> RequestFromPayload<TPayload, TResponse>(TPayload payload, IRule sender,
        ProcessorRegistry processorRegistry, string? destinationAddress = null
        , MessageType msgType = MessageType.Publish)
    {
        var seqId = ring.Claim();
        var evt = ring[seqId];
        evt.Type = msgType;
        evt.PayLoad = new PayLoad<TPayload>(payload);
        evt.DestinationAddress = destinationAddress;
        var reusableValueTaskSource
            = eventContext.PooledRecycler.Borrow<ReusableResponseValueTaskSource<TResponse>>();
        reusableValueTaskSource.DispatchResult = processorRegistry.DispatchResult;
        var response = new ValueTask<RequestResponse<TResponse>>(reusableValueTaskSource
            , (short)(reusableValueTaskSource.Version + 1));
        evt.Response = reusableValueTaskSource;
        evt.Sender = sender;
        evt.SentTime = DateTime.Now;
        evt.ProcessorRegistry = processorRegistry;
        ring.Publish(seqId);
        messagePump.WakeIfAsleep();
        return response;
    }

    public ValueTask<IDispatchResult> LaunchRule(IRule sender, IRule rule)
    {
        var processorRegistry = eventContext.PooledRecycler.Borrow<ProcessorRegistry>();
        processorRegistry.DispatchResult = eventContext.PooledRecycler.Borrow<DispatchResult>();
        return EnqueuePayloadWithStats(rule, sender, processorRegistry, null, MessageType.LoadRule);
    }

    public bool IsListeningToAddress(string destinationAddress) =>
        messagePump.listeners.ContainsKey(destinationAddress);

    public void RunOn(IRule sender, Action action)
    {
        var seqId = ring.Claim();
        var evt = ring[seqId];
        evt.Type = MessageType.RunActionPayload;
        evt.PayLoad = new PayLoad<Action>(action);
        evt.DestinationAddress = MessagePump.RunActionAddress;
        evt.Response = Message.NoOpCompletionSource;
        evt.Sender = sender;
        evt.SentTime = DateTime.Now;
        ring.Publish(seqId);
        messagePump.WakeIfAsleep();
    }

    public void EnqueuePayload<TPayload>(TPayload payload, IRule sender,
        string? destinationAddress = null, MessageType msgType = MessageType.Publish)
    {
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
}
