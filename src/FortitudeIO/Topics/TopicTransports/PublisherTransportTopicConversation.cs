#region

using FortitudeIO.Conversations;
using FortitudeIO.Topics.Config.ConnectionConfig;

#endregion

namespace FortitudeIO.Topics.TopicTransports;

public interface IPublisherTransportTopicConversation : ITransportTopicConversation
{
    IConversationPublisher PublisherConversation { get; }
}

public class PublisherTransportTopicConversation : TransportTopicConversation, IPublisherTransportTopicConversation
{
    public PublisherTransportTopicConversation(ITopicEndpointInfo endpoint,
        IConversationPublisher publisherConversation) : base(endpoint) =>
        PublisherConversation = publisherConversation;

    public PublisherTransportTopicConversation(ITopicConnectionConfig connectionConfig,
        IConversationPublisher publisherConversation) : base(connectionConfig) =>
        PublisherConversation = publisherConversation;

    public IConversationPublisher PublisherConversation { get; }
}
