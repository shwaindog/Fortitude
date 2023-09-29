using FortitudeIO.Protocols.Serialization;
using FortitudeIO.Topics.TopicRepository;

namespace FortitudeIO.Topics
{
    public class TopicResolverService
    {
        private TopicRegistry topicRegistry;
        private readonly ITopicFactory topicFactory;
        public TopicResolverService(ITopicFactory topicFactory)
        {
            this.topicFactory = topicFactory;
        }

        PublisherTopic ResolvePublisherTopic(string topicName, string instanceName, 
            ISerdesFactory serdesFactory)
        {
            var existingTopic = topicRegistry.SupplyPublisherTopic(topicName, instanceName);
            if (existingTopic != null)
            {
                return existingTopic;
            }

            var newPublisher = topicFactory.CreatePublisherTopic(topicName, instanceName, serdesFactory);
            topicRegistry.RegisterTopic(newPublisher);
            return newPublisher;
        }
    }
}
