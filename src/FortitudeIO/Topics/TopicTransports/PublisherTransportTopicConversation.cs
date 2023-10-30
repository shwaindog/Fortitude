#region

using FortitudeIO.Conversations;
using FortitudeIO.Topics.Config.ConnectionConfig;

#endregion

namespace FortitudeIO.Topics.TopicTransports;

public interface IPublisherTransportTopicConversation : ITransportTopicConversation
{
    IPublisherConversation PublisherConversation { get; }
}

public class PublisherTransportTopicConversation : TransportTopicConversation, IPublisherTransportTopicConversation
{
    public PublisherTransportTopicConversation(ITopicEndpointInfo endpoint,
        IPublisherConversation publisherConversation) : base(endpoint) =>
        PublisherConversation = publisherConversation;

    public PublisherTransportTopicConversation(ITopicConnectionConfig connectionConfig,
        IPublisherConversation publisherConversation) : base(connectionConfig) =>
        PublisherConversation = publisherConversation;

    public IPublisherConversation PublisherConversation { get; }
}
