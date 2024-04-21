#region

using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;
using FortitudeCommon.DataStructures.Memory;

#endregion

namespace FortitudeBusRules.BusMessaging.Messages.ListeningSubscriptions;

public interface ISubscription : IRecyclableObject
{
    void Unsubscribe();

    ValueTask UnsubscribeAsync();
}

public class MessageListenerUnsubscribe : RecyclableObject, ISubscription
{
    public MessageListenerUnsubscribe() { }

    public MessageListenerUnsubscribe(IListeningRule subscriberRule, string publishAddress, string subscriberId
        , IDispatchResult? dispatchResult = null)
    {
        SubscriberRule = subscriberRule;
        PublishAddress = publishAddress;
        SubscriberId = subscriberId;
        DispatchResult = dispatchResult;
    }

    public IListeningRule SubscriberRule { get; set; } = null!;
    public string PublishAddress { get; set; } = null!;
    public string SubscriberId { get; set; } = null!;
    public IDispatchResult? DispatchResult { get; set; }

    public void Unsubscribe() =>
        SubscriberRule.Context.RegisteredOn.EnqueuePayload(this, SubscriberRule, PublishAddress
            , MessageType.ListenerUnsubscribe);

    public async ValueTask UnsubscribeAsync()
    {
        var processorRegistry = SubscriberRule.Context.PooledRecycler.Borrow<ProcessorRegistry>();
        processorRegistry.DispatchResult = SubscriberRule.Context.PooledRecycler.Borrow<DispatchResult>();
        processorRegistry.IncrementRefCount();
        processorRegistry.DispatchResult.SentTime = DateTime.Now;
        processorRegistry.ResponseTimeoutAndRecycleTimer = SubscriberRule.Context.Timer;
        await SubscriberRule.Context.RegisteredOn.EnqueuePayloadWithStatsAsync(this, SubscriberRule, processorRegistry, PublishAddress
            , MessageType.ListenerUnsubscribe);
    }
}
