#region

using FortitudeBusRules.Messaging;
using FortitudeBusRules.Rules;

#endregion

namespace FortitudeBusRules.MessageBus.Messages.ListeningSubscriptions;

public interface ISubscription
{
    void Unsubscribe();
}

public class MessageListenerUnsubscribe(IListeningRule subscriberRule, string publishAddress, string subscriberId)
    : ISubscription
{
    public IListeningRule SubscriberRule { get; set; } = subscriberRule;
    public string PublishAddress { get; set; } = publishAddress;
    public string SubscriberId { get; set; } = subscriberId;

    public void Unsubscribe() =>
        SubscriberRule.Context.RegisteredOn.EnqueuePayload(this, SubscriberRule, PublishAddress
            , MessageType.ListenerUnsubscribe);
}
