using FortitudeIO.Conversations;

namespace FortitudeIO.Transports.NewSocketAPI.Conversations
{
    public interface ISubscriberConversation : IConversation
    {
        IConversationListener ConversationListener { get; }
    }
}