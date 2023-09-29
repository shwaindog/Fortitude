using FortitudeIO.Conversations;
using FortitudeIO.Topics.Config.ConnectionConfig;

namespace FortitudeIO.Topics.TopicTransports
{
    public interface ITransportTopicConversation
    {
        ITopicEndpointInfo Endpoint { get; }
        ITopicConnectionConfig ConnectionConfig { get; }
    }

    public class TransportTopicConversation : ITransportTopicConversation
    {
        public TransportTopicConversation(ITopicConnectionConfig topicConnectionConfig)
        {
            TopicConnectionConfig = topicConnectionConfig;
        }

        public TransportTopicConversation(ITopicEndpointInfo endpoint)
        {
            Endpoint = endpoint;
        }

        public ITopicEndpointInfo Endpoint { get; }
        public ITopicConnectionConfig TopicConnectionConfig { get; }
        public ITopicConnectionConfig ConnectionConfig { get; }
    }
}