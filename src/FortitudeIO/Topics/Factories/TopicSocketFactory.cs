#region

using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Topics.Config.ConnectionConfig;
using FortitudeIO.Topics.TopicTransports;
using FortitudeIO.Transports.NewSocketAPI.Config;
using FortitudeIO.Transports.NewSocketAPI.Conversations.Builders;
using FortitudeIO.Transports.NewSocketAPI.Sockets;

#endregion

namespace FortitudeIO.Topics.Factories;

public class TopicSocketFactory : ITopicTransportFactory
{
    private readonly TcpConversationResponderBuilder tcpConversationResponderBuilder = new();
    private readonly UdpConversationPublisherBuilder udpConversationPublisherBuilder = new();

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
            var udpPublisher = udpConversationPublisherBuilder.Build(scc, serdesFactory);
            return new PublisherTransportTopicConversation(scc, udpPublisher);
        }

        var tcpResponder = tcpConversationResponderBuilder.Build(scc, serdesFactory);
        var wrappedResponderAsPublisher = new ResponderToPublisherAdapter(tcpResponder);
        return new PublisherTransportTopicConversation(scc, wrappedResponderAsPublisher);
    }
}
