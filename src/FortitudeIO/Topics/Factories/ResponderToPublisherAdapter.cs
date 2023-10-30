#region

using FortitudeIO.Conversations;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Serialization;
using FortitudeIO.Transports;
using FortitudeIO.Transports.NewSocketAPI.Conversations;

#endregion

namespace FortitudeIO.Topics.TopicRepository;

public class ResponderToPublisherAdapter : IPublisherConversation
{
    private readonly ConversationPulbisherAdapter conversationPulbisherAdapter;
    private readonly IRequestResponseResponderConversation requestResponseResponderConversation;

    public ResponderToPublisherAdapter(IRequestResponseResponderConversation requestResponseResponderConversation)
    {
        this.requestResponseResponderConversation = requestResponseResponderConversation;
        conversationPulbisherAdapter = new ConversationPulbisherAdapter(requestResponseResponderConversation);
    }

    public ConversationType ConversationType => ConversationType.Publisher;

    event Action<string, int>? IConversationState.Error
    {
        add => requestResponseResponderConversation.Error += value;
        remove => requestResponseResponderConversation.Error -= value;
    }

    event Action? IConversationState.Started
    {
        add => requestResponseResponderConversation.Started += value;
        remove => requestResponseResponderConversation.Started -= value;
    }

    event Action? IConversationState.Stopped
    {
        add => requestResponseResponderConversation.Stopped += value;
        remove => requestResponseResponderConversation.Stopped -= value;
    }

    public ConversationState ConversationState => requestResponseResponderConversation.ConversationState;
    public string ConversationDescription => requestResponseResponderConversation.ConversationDescription;

    public void Start()
    {
        requestResponseResponderConversation.Start();
    }

    public void Stop()
    {
        requestResponseResponderConversation.Stop();
    }

    public IConversationPublisher ConversationPublisher => conversationPulbisherAdapter;

    private class ConversationPulbisherAdapter : IConversationPublisher
    {
        private IList<IVersionedMessage> queuedMessages = new List<IVersionedMessage>();
        private readonly IRequestResponseResponderConversation requestResponseResponderConversation;
        private IList<IVersionedMessage> sendMessages = new List<IVersionedMessage>();


        public ConversationPulbisherAdapter(
            IRequestResponseResponderConversation requestResponseResponderConversation) =>
            this.requestResponseResponderConversation = requestResponseResponderConversation;

        public void RegisterSerializer(uint messageId, IBinarySerializer serializer)
        {
            requestResponseResponderConversation.RegisterSerializer(messageId, serializer);
        }

        public void Enqueue(IVersionedMessage message)
        {
            queuedMessages.Add(message);
        }

        public void Send(IVersionedMessage message)
        {
            requestResponseResponderConversation.Broadcast(message);
        }

        public bool SendEnqueued()
        {
            queuedMessages = Interlocked.Exchange(ref sendMessages, queuedMessages);
            foreach (var versionedMessage in sendMessages)
                requestResponseResponderConversation.Broadcast(versionedMessage);
            sendMessages.Clear();
            return true;
        }
    }
}
