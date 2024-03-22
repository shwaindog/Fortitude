#region

using FortitudeIO.Conversations;
using FortitudeIO.Protocols;
using FortitudeIO.Topics.TopicTransports;

#endregion

namespace FortitudeIO.Topics;

public interface ITopic : IConversation
{
    string InstanceName { get; }
}

public abstract class Topic : ITopic
{
    private const string DefaultInstanceName = "DEFAULT";

    protected Topic(string name, ConversationType conversationType, string instanceName = DefaultInstanceName)
    {
        Name = name + (instanceName != DefaultInstanceName ? "_" + instanceName : "");
        ConversationType = conversationType;
        InstanceName = instanceName;
    }

    public int Id { get; } = 0;
    public IConversationSession Session { get; } = null!;

    public ConversationType ConversationType { get; }
    public ConversationState ConversationState { get; protected set; }
    public string Name { get; }
    public string InstanceName { get; }

    public abstract bool IsStarted { get; }

    public abstract void Start();

    public abstract void Stop();

    protected class PublisherConversationPublisher : IConversationPublisher
    {
        private readonly IList<IPublisherTransportTopicConversation> publisherTransportSessions;

        public PublisherConversationPublisher(IList<IPublisherTransportTopicConversation> publisherTransportSessions) =>
            this.publisherTransportSessions = publisherTransportSessions;

        public void Send(IVersionedMessage message)
        {
            foreach (var pts in publisherTransportSessions)
                pts.PublisherConversation.ConversationPublisher!.Send(message);
        }

        public ConversationType ConversationType { get; } = ConversationType.Publisher;
        public ConversationState ConversationState { get; }
        public string Name { get; } = "";
        public event Action<string, int>? Error;
        public event Action? Started;
        public event Action? Stopped;

        public void Start()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        public int Id { get; }
        public bool IsStarted { get; }
        public IConversationSession Session { get; } = null!;
        public IPublisher? ConversationPublisher { get; }


        public void Enqueue(IVersionedMessage message)
        {
            foreach (var pts in publisherTransportSessions)
                pts.PublisherConversation.ConversationPublisher!.Enqueue(message);
        }

        public bool SendEnqueued()
        {
            var allSucceeded = true;
            foreach (var pts in publisherTransportSessions)
                allSucceeded &= pts.PublisherConversation.ConversationPublisher!.SendEnqueued();
            return allSucceeded;
        }
    }

#pragma warning disable 67
    public event Action<string, int>? Error;
    public event Action? Started;
    public event Action? Stopped;
#pragma warning restore 67
}
