using FortitudeIO.Protocols.Serialization;
using FortitudeIO.Topics.Config.ConnectionConfig;
using FortitudeIO.Topics.TopicTransports;
using FortitudeIO.Transports.NewSocketAPI.Conversations.Builders;
using FortitudeIO.Transports.NewSocketAPI.SocketFactory;

namespace FortitudeIO.Topics.TopicRepository
{
    public class TopicSocketFactory : ITopicTransportFactory
    {
        private UDPPublisherBuilder udpPublisherBuilder = new UDPPublisherBuilder();
        private TCPRequestResponseResponderBuilder tcpRequestResponseResponderBuilder = new TCPRequestResponseResponderBuilder();

        public ISubscriberTransportTopicConversation CreateSubscriberTransportTopic( 
            ISerdesFactory serdesFactory, ITopicEndpointInfo topicEndpointInfo)
        {
            return null;
        }

        public IPublisherTransportTopicConversation CreatePublisherTransportTopic(ISerdesFactory serdesFactory, ITopicConnectionConfig topicEndpointInfo)
        {
            var scc = topicEndpointInfo as ISocketConnectionConfig;
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
}