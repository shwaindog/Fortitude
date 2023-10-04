#region

using FortitudeIO.Protocols.Serialization;
using FortitudeIO.Topics.Factories;
using FortitudeIO.Topics.TopicRepository;

#endregion

namespace FortitudeIO.Topics;

public class TopicResolverService
{
    private readonly ITopicFactory topicFactory;
    private readonly TopicRegistry topicRegistry;

    public TopicResolverService(ITopicFactory topicFactory, TopicRegistry topicRegistry)
    {
        this.topicRegistry = topicRegistry;
        this.topicFactory = topicFactory;
    }

    private PublisherTopic ResolvePublisherTopic(string topicName, string instanceName,
        ISerdesFactory serdesFactory)
    {
        var existingTopic = topicRegistry.SupplyPublisherTopic(topicName, instanceName);
        if (existingTopic != null) return existingTopic;

        var newPublisher = topicFactory.CreatePublisherTopic(topicName, instanceName, serdesFactory)!;
        topicRegistry.RegisterTopic(newPublisher);
        return newPublisher;
    }
}
