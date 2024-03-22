#region

using FortitudeIO.Conversations;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Serdes.Binary;

#endregion

namespace FortitudeIO.Topics.Factories;

public class ResponderToPublisherAdapter : IPublisherConversation
{
    private readonly ConversationPublisherAdapter conversationPublisherAdapter;
    private readonly IConversationResponder requestResponseResponderConversation;

    public ResponderToPublisherAdapter(IConversationResponder requestResponseResponderConversation)
    {
        this.requestResponseResponderConversation = requestResponseResponderConversation;
        conversationPublisherAdapter = new ConversationPublisherAdapter(requestResponseResponderConversation);
    }

    public int Id { get; } = 0;
    public IConversationSession Session { get; } = null!;

    public ConversationType ConversationType => ConversationType.Publisher;

    public bool IsStarted => requestResponseResponderConversation.IsStarted;

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
    public string Name => requestResponseResponderConversation.Name;

    public void Start()
    {
        requestResponseResponderConversation.Start();
    }

    public void Stop()
    {
        requestResponseResponderConversation.Stop();
    }

    public IConversationPublisher ConversationPublisher => conversationPublisherAdapter;

    private class ConversationPublisherAdapter : IConversationPublisher
    {
        private readonly IConversationResponder requestResponseResponderConversation;
        private IList<IVersionedMessage> queuedMessages = new List<IVersionedMessage>();
        private IList<IVersionedMessage> sendMessages = new List<IVersionedMessage>();


        public ConversationPublisherAdapter(
            IConversationResponder requestResponseResponderConversation) =>
            this.requestResponseResponderConversation = requestResponseResponderConversation;

        public void RegisterSerializer(uint messageId, IMessageSerializer serializer)
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
