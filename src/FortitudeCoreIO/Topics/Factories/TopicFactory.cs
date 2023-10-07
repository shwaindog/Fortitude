#region

using FortitudeIO.Protocols.Serialization;
using FortitudeIO.Topics.Config;
using FortitudeIO.Topics.TopicRepository;

#endregion

namespace FortitudeIO.Topics.Factories;

public class TopicFactory : ITopicFactory
{
    private ITopicConfigRepository? topicConfigRepository;
    private ITopicRegistry? topicRegistry;
    private ITopicTransportFactory? transportSelectorTopicTransportFactory;

    public PublisherTopic? CreatePublisherTopic(string topicName, string instanceName, ISerdesFactory serdesFactory)
    {
        var configForTopic = topicConfigRepository!.GetConfigForTopic(topicName);

        var instanceConfig = configForTopic!.TopicConnectionConfigs.FirstOrDefault(
            cc => cc.InstanceName.Equals(instanceName, StringComparison.InvariantCultureIgnoreCase));
        if (instanceConfig != null) { }

        return null;
    }
}
