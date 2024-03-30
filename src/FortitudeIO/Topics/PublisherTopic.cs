#region

using FortitudeIO.Conversations;
using FortitudeIO.Protocols;
using FortitudeIO.Topics.TopicTransports;

#endregion

namespace FortitudeIO.Topics;

public interface IPublisherTopic : ITopic, IConversationPublisher
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


    public override bool IsStarted { get; } = false;

    public override void Start()
    {
        throw new NotImplementedException();
    }

    public override void Stop()
    {
        throw new NotImplementedException();
    }

    public override void OnSessionFailure(string reason) => throw new NotImplementedException();

    public void Send(IVersionedMessage versionedMessage)
    {
        throw new NotImplementedException();
    }

    public IStreamPublisher? StreamPublisher { get; set; }
}
