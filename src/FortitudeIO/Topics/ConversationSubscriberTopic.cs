#region

using FortitudeIO.Conversations;

#endregion

namespace FortitudeIO.Topics;

public interface IConversationSubscriberTopic : ITopic, IConversationSubscriber { }

public class ConversationSubscriberTopic : Topic, IConversationSubscriberTopic
{
    private readonly IConversationSubscriber conversationSubscriberTransportSession;

    public ConversationSubscriberTopic(string description
        , IConversationSubscriber conversationSubscriberTransportSession) : base(description
        , ConversationType.Subscriber) =>
        this.conversationSubscriberTransportSession = conversationSubscriberTransportSession;

    public override bool IsStarted { get; } = false;

    public override void Start()
    {
        conversationSubscriberTransportSession?.Start();
    }

    public override void Stop()
    {
        conversationSubscriberTransportSession?.Stop();
    }

    public IListener? ConversationListener => conversationSubscriberTransportSession.ConversationListener;
}
