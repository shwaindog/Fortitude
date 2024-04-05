#region

using FortitudeIO.Protocols.Serdes.Binary;

#endregion

namespace FortitudeIO.Topics.Factories;

public interface ITopicFactory
{
    PublisherTopic? CreatePublisherTopic(string topicName, string instanceName, IMessageSerdesRepositoryFactory serdesFactory);
}
