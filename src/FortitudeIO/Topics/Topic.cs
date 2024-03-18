#region

using FortitudeIO.Conversations;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Serdes.Binary;
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

    public ConversationType ConversationType { get; }
    public ConversationState ConversationState { get; protected set; }
    public string Name { get; }
    public string InstanceName { get; }
    public abstract void Start();

    public abstract void Stop();

    protected class PublisherConversationPublisher : IConversationPublisher
    {
        private readonly IList<IPublisherTransportTopicConversation> publisherTransportSessions;

        public PublisherConversationPublisher(IList<IPublisherTransportTopicConversation> publisherTransportSessions) =>
            this.publisherTransportSessions = publisherTransportSessions;

        public void RegisterSerializer(uint messageId, IMessageSerializer serializer)
        {
            foreach (var pts in publisherTransportSessions)
                pts.PublisherConversation.ConversationPublisher!.RegisterSerializer(messageId, serializer);
        }

        public void Enqueue(IVersionedMessage message)
        {
            foreach (var pts in publisherTransportSessions)
                pts.PublisherConversation.ConversationPublisher!.Enqueue(message);
        }

        public void Send(IVersionedMessage message)
        {
            foreach (var pts in publisherTransportSessions)
                pts.PublisherConversation.ConversationPublisher!.Send(message);
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
