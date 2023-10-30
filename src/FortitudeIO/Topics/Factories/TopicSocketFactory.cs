#region

using FortitudeIO.Protocols.Serialization;
using FortitudeIO.Topics.Config.ConnectionConfig;
using FortitudeIO.Topics.Factories;
using FortitudeIO.Topics.TopicTransports;
using FortitudeIO.Transports.NewSocketAPI.Conversations.Builders;
using FortitudeIO.Transports.NewSocketAPI.SocketFactory;

#endregion

namespace FortitudeIO.Topics.TopicRepository;

public class TopicSocketFactory : ITopicTransportFactory
{
    private readonly TCPRequestResponseResponderBuilder tcpRequestResponseResponderBuilder = new();
    private readonly UDPPublisherBuilder udpPublisherBuilder = new();

    public ISubscriberTransportTopicConversation? CreateSubscriberTransportTopic(
        ISerdesFactory serdesFactory, ITopicEndpointInfo topicEndpointInfo) =>
        null;

    public IPublisherTransportTopicConversation CreatePublisherTransportTopic(ISerdesFactory serdesFactory
        , ITopicConnectionConfig topicEndpointInfo)
    {
        var scc = (ISocketConnectionConfig)topicEndpointInfo;
        if ((scc.ConnectionAttributes & SocketConnectionAttributes.Multicast) > 0 &&
            (scc.ConnectionAttributes & SocketConnectionAttributes.Reliable) == 0)
        {
            var udpPublisher = udpPublisherBuilder.Build(scc, serdesFactory);
            return new PublisherTransportTopicConversation(scc, udpPublisher);
        }

        var tcpResponder = tcpRequestResponseResponderBuilder.Build(scc, serdesFactory);
        var wrappedResponderAsPublisher = new ResponderToPublisherAdapter(tcpResponder);
        return new PublisherTransportTopicConversation(scc, wrappedResponderAsPublisher);
    }
}
