#region

using FortitudeIO.Protocols.Serialization;

#endregion

namespace FortitudeIO.Topics.Factories;

public interface ITopicFactory
{
    PublisherTopic? CreatePublisherTopic(string topicName, string instanceName, ISerdesFactory serdesFactory);
}
