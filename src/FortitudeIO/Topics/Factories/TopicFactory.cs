#region

using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Topics.Config;
using FortitudeIO.Topics.TopicRepository;

#endregion

namespace FortitudeIO.Topics.Factories;

public class TopicFactory : ITopicFactory
{
    public PublisherTopic? CreatePublisherTopic(string topicName, string instanceName, ISerdesFactory serdesFactory)
    {
        var configForTopic = topicConfigRepository!.GetConfigForTopic(topicName);

        var instanceConfig = configForTopic!.TopicConnectionConfigs.FirstOrDefault(
            cc => cc.InstanceName.Equals(instanceName, StringComparison.InvariantCultureIgnoreCase));
        if (instanceConfig != null) { }

        return null;
    }
#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value
    private ITopicConfigRepository? topicConfigRepository;
#pragma warning disable 0169
    private ITopicRegistry? topicRegistry;
    private ITopicTransportFactory? transportSelectorTopicTransportFactory;
#pragma warning restore 0169
#pragma warning restore CS0649 // Field is never assigned to, and will always have its default value
}
