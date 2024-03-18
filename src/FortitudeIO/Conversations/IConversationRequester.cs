namespace FortitudeIO.Conversations;

public interface IConversationRequester : IConversation
{
    IConversationListener? ConversationListener { get; }
    IConversationPublisher? ConversationPublisher { get; }
}
