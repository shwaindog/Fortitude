namespace FortitudeIO.Conversations;

public interface IConversationSubscriber : IConversation
{
    IConversationListener? ConversationListener { get; }
}
