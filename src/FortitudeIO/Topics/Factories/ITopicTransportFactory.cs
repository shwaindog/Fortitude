#region

using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Topics.Config.ConnectionConfig;
using FortitudeIO.Topics.TopicTransports;

#endregion

namespace FortitudeIO.Topics.Factories;

public interface ITopicTransportFactory
{
    ISubscriberTransportTopicConversation? CreateSubscriberTransportTopic(
        IMessageSerdesRepositoryFactory serdesFactory, ITopicEndpointInfo topicEndpointInfo);

    IPublisherTransportTopicConversation? CreatePublisherTransportTopic(
        IMessageSerdesRepositoryFactory serdesFactory, ITopicConnectionConfig topicEndpointInfo);
}

public class TopicTransportFactory : ITopicTransportFactory
{
    private readonly ITopicTransportFactory sockeTopicTransportFactory;

    public TopicTransportFactory(ITopicTransportFactory sockeTopicTransportFactory) => this.sockeTopicTransportFactory = sockeTopicTransportFactory;

    public ISubscriberTransportTopicConversation CreateSubscriberTransportTopic(IMessageSerdesRepositoryFactory serdesFactory,
        ITopicEndpointInfo topicEndpointInfo) =>
        throw new NotImplementedException();

    public IPublisherTransportTopicConversation? CreatePublisherTransportTopic(IMessageSerdesRepositoryFactory serdesFactory,
        ITopicConnectionConfig topicEndpointInfo)
    {
        if (topicEndpointInfo.TransportType == TransportType.Sockets)
            return sockeTopicTransportFactory.CreatePublisherTransportTopic(serdesFactory,
                topicEndpointInfo);

        return null;
    }
}
