#region

using FortitudeIO.Conversations;
using FortitudeIO.Topics.TopicTransports;
using FortitudeIO.Transports;

#endregion

namespace FortitudeIO.Topics;

public interface IPublisherTopic : ITopic, IPublisherConversation
{
    TopicTransportMap<IPublisherTransportTopicConversation> PublisherTransports { get; }
}

public class PublisherTopic : Topic, IPublisherTopic
{
    public PublisherTopic(string description
        , TopicTransportMap<IPublisherTransportTopicConversation> initialConnections)
        : base(description, ConversationType.Publisher) =>
        PublisherTransports = initialConnections;

    public TopicTransportMap<IPublisherTransportTopicConversation> PublisherTransports { get; }

    public override void Start()
    {
        throw new NotImplementedException();
    }

    public override void Stop()
    {
        throw new NotImplementedException();
    }

    public IConversationPublisher? ConversationPublisher { get; set; }
}
