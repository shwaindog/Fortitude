#region

using FortitudeIO.Conversations;
using FortitudeIO.Topics.Config.ConnectionConfig;

#endregion

namespace FortitudeIO.Topics.TopicTransports;

public interface ISubscriberTransportTopicConversation : ITransportTopicConversation
{
    ISubscriberConversation SubscriberConversation { get; }
}

public class SubscriberTransportTopicConversation : TransportTopicConversation, ISubscriberTransportTopicConversation
{
    public SubscriberTransportTopicConversation(ITopicEndpointInfo endpoint,
        ISubscriberConversation subscriberConversation) : base(endpoint) =>
        SubscriberConversation = subscriberConversation;

    public SubscriberTransportTopicConversation(ITopicConnectionConfig connectionConfig,
        ISubscriberConversation subscriberConversation) : base(connectionConfig) =>
        SubscriberConversation = subscriberConversation;

    public ISubscriberConversation SubscriberConversation { get; }
}
