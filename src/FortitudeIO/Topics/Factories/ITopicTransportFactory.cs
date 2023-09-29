using FortitudeIO.Protocols.Serialization;
using FortitudeIO.Topics.Config.ConnectionConfig;
using FortitudeIO.Topics.TopicTransports;

namespace FortitudeIO.Topics.TopicRepository
{
    public interface ITopicTransportFactory
    {
        ISubscriberTransportTopicConversation CreateSubscriberTransportTopic( 
            ISerdesFactory serdesFactory, ITopicEndpointInfo topicEndpointInfo);

        IPublisherTransportTopicConversation CreatePublisherTransportTopic( 
            ISerdesFactory serdesFactory, ITopicConnectionConfig topicEndpointInfo);
    }

    public class TopicTransportFactory : ITopicTransportFactory
    {
        private ITopicTransportFactory sockeTopicTransportFactory;

        public TopicTransportFactory(ITopicTransportFactory sockeTopicTransportFactory)
        {
            this.sockeTopicTransportFactory = sockeTopicTransportFactory;
        }

        public ISubscriberTransportTopicConversation CreateSubscriberTransportTopic(ISerdesFactory serdesFactory,
            ITopicEndpointInfo topicEndpointInfo)
        {
            throw new System.NotImplementedException();
        }

        public IPublisherTransportTopicConversation CreatePublisherTransportTopic(ISerdesFactory serdesFactory,
            ITopicConnectionConfig topicEndpointInfo)
        {
            if (topicEndpointInfo.TransportType == TransportType.Sockets)
            {
                return sockeTopicTransportFactory.CreatePublisherTransportTopic(serdesFactory,
                    topicEndpointInfo);
            }

            return null;
        }
    }
}