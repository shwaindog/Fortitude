namespace FortitudeIO.Conversations;

public interface IConversationSubscriber : IConversation
{
    IListener? ConversationListener { get; }
}
