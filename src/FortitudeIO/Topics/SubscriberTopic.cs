using FortitudeIO.Transports;
using FortitudeIO.Transports.NewSocketAPI.Conversations;

namespace FortitudeIO.Topics
{
    public interface ISubscriberTopic : ITopic, ISubscriberConversation
    {
    }

    public class SubscriberTopic : Topic, ISubscriberTopic
    {
        public SubscriberTopic(string description) : base(description, ConversationType.Subscriber)
        {
        }

        private ISubscriberConversation subscriberTransportSession;

        public override void Start()
        {
            subscriberTransportSession?.Start();
        }

        public override void Stop()
        {
            subscriberTransportSession?.Stop();
        }

        public IConversationListener ConversationListener => subscriberTransportSession.ConversationListener;
    }
}
