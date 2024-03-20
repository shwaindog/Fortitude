#region

using FortitudeIO.Conversations;
using FortitudeIO.Topics.Config.ConnectionConfig;

#endregion

namespace FortitudeIO.Topics.TopicTransports;

public interface ISubscriberTransportTopicConversation : ITransportTopicConversation
{
    IConversationSubscriber ConversationSubscriber { get; }
}

public class SubscriberTransportTopicConversation : TransportTopicConversation, ISubscriberTransportTopicConversation
{
    public SubscriberTransportTopicConversation(ITopicEndpointInfo endpoint,
        IConversationSubscriber conversationSubscriber) : base(endpoint) =>
        ConversationSubscriber = conversationSubscriber;

    public SubscriberTransportTopicConversation(ITopicConnectionConfig connectionConfig,
        IConversationSubscriber conversationSubscriber) : base(connectionConfig) =>
        ConversationSubscriber = conversationSubscriber;

    public IConversationSubscriber ConversationSubscriber { get; }
}
