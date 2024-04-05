#region

using FortitudeBusRules.Rules;
using FortitudeCommon.Serdes;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Serdes.Binary;

#endregion

namespace FortitudeBusRules.Connectivity.Network.Serdes.Deserialization;

public class EventContextSubscriptionCount
{
    public EventContextSubscriptionCount(IEventContext eventContext)
    {
        EventContext = eventContext;
        SubscriberCount = 1;
    }

    public IEventContext EventContext { get; }
    public int SubscriberCount { get; set; }
}

public interface IEventPublishingDeserializer : IMessageDeserializer
{
    int IncrementPublishToContext(IEventContext eventContext);
    int DecrementPublishToContext(IEventContext eventContext);
}

public class EventPublishingDeserializer<TOrig, TConv> : IEventPublishingDeserializer
    where TOrig : class, IVersionedMessage, new() where TConv : class
{
    private readonly IConverter<TOrig, TConv>? converter;
    private readonly string publishAddress;
    private INotifyingMessageDeserializer<TOrig> backingMessageDeserializer;

    private List<EventContextSubscriptionCount> subscribingContexts = new();

    public EventPublishingDeserializer(string publishAddress, INotifyingMessageDeserializer<TOrig> backingMessageDeserializer
        , IConverter<TOrig, TConv>? converter = null)
    {
        this.publishAddress = publishAddress;
        this.backingMessageDeserializer = backingMessageDeserializer;
        this.converter = converter;
        backingMessageDeserializer.MessageDeserialized += BackingMessageDeserializer_MessageDeserialized;
    }

    public IRule RegistratorRule { get; set; } = new Rule();

    public MarshalType MarshalType => backingMessageDeserializer.MarshalType;

    object? IMessageDeserializer.Deserialize(IBufferContext bufferContext) => Deserialize(bufferContext);

    public int IncrementPublishToContext(IEventContext eventContext)
    {
        var foundContextSubscriber = subscribingContexts.FirstOrDefault(ecsc => ecsc.EventContext == eventContext);
        if (foundContextSubscriber != null)
        {
            foundContextSubscriber.SubscriberCount++;
        }
        else
        {
            foundContextSubscriber = new EventContextSubscriptionCount(eventContext);
            subscribingContexts.Add(new EventContextSubscriptionCount(eventContext));
        }

        return foundContextSubscriber.SubscriberCount;
    }

    public int DecrementPublishToContext(IEventContext eventContext)
    {
        var foundContextSubscriber = subscribingContexts.FirstOrDefault(ecsc => ecsc.EventContext == eventContext);
        if (foundContextSubscriber != null)
        {
            foundContextSubscriber.SubscriberCount--;
            if (foundContextSubscriber.SubscriberCount <= 0) subscribingContexts.Remove(foundContextSubscriber);
        }

        return foundContextSubscriber?.SubscriberCount ?? 0;
    }

    private void BackingMessageDeserializer_MessageDeserialized(TOrig origDeserialized, IBufferContext arg2)
    {
        foreach (var eventContextSubscription in subscribingContexts)
            if (converter != null)
            {
                var publishObj = converter.Convert(origDeserialized, eventContextSubscription.EventContext.PooledRecycler);
                eventContextSubscription.EventContext.RegisteredOn.EnqueuePayload(publishObj, RegistratorRule, publishAddress);
            }
    }

    public TOrig? Deserialize(ISerdeContext readContext) => backingMessageDeserializer.Deserialize(readContext);

    public TOrig? Deserialize(IBufferContext bufferContext) => backingMessageDeserializer.Deserialize(bufferContext);
}
