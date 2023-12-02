#region

using Fortitude.EventProcessing.BusRules.Messaging;
using Fortitude.EventProcessing.BusRules.Rules;

#endregion

namespace Fortitude.EventProcessing.BusRules.MessageBus.Messages;

public interface ISubscription
{
    void Unsubscribe();
}

public class MessageListenerUnsubscribe : ISubscription
{
    public MessageListenerUnsubscribe(IListeningRule subscriberRule, string publishAddress, string subscriberId)
    {
        SubscriberRule = subscriberRule;
        PublishAddress = publishAddress;
        SubscriberId = subscriberId;
    }

    public IListeningRule SubscriberRule { get; set; }
    public string PublishAddress { get; set; }
    public string SubscriberId { get; set; }

    public void Unsubscribe() =>
        SubscriberRule.Context.RegisteredOn.EnqueuePayload(this, SubscriberRule, PublishAddress
            , MessageType.ListenerUnsubscribe);
}
