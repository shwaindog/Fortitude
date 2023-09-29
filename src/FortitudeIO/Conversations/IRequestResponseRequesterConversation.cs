using FortitudeIO.Conversations;

namespace FortitudeIO.Transports.NewSocketAPI.Conversations
{
    public interface IRequestResponseRequesterConversation : IConversation
    {
        IConversationListener ConversationListener { get; }
        IConversationPublisher ConversationPublisher { get; }
    }
}