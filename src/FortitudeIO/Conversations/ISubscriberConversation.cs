#region

using FortitudeIO.Conversations;

#endregion

namespace FortitudeIO.Transports.NewSocketAPI.Conversations;

public interface ISubscriberConversation : IConversation
{
    IConversationListener? ConversationListener { get; }
}
