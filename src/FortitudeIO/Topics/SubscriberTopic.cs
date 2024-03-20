#region

using FortitudeIO.Conversations;

#endregion

namespace FortitudeIO.Topics;

public interface ISubscriberTopic : ITopic, ISubscriberConversation { }

public class SubscriberTopic : Topic, ISubscriberTopic
{
    private readonly ISubscriberConversation subscriberTransportSession;

    public SubscriberTopic(string description, ISubscriberConversation subscriberTransportSession) : base(description
        , ConversationType.Subscriber) =>
        this.subscriberTransportSession = subscriberTransportSession;

    public override bool IsStarted { get; } = false;

    public override void Start()
    {
        subscriberTransportSession?.Start();
    }

    public override void Stop()
    {
        subscriberTransportSession?.Stop();
    }

    public IConversationListener? ConversationListener => subscriberTransportSession.ConversationListener;
}
