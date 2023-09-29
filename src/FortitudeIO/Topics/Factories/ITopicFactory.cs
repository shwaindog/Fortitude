using FortitudeIO.Protocols.Serialization;

namespace FortitudeIO.Topics.TopicRepository
{
    public interface ITopicFactory
    {
        PublisherTopic CreatePublisherTopic(string topicName, string instanceName, ISerdesFactory serdesFactory);
    }
}