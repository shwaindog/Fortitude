#region

#endregion

namespace FortitudeIO.Conversations;

public interface ISubscriberConversation : IConversation
{
    IConversationListener? ConversationListener { get; }
}
