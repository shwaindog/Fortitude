#region

using FortitudeIO.Transports;

#endregion

namespace FortitudeIO.Conversations;

public interface IRequestResponseRequesterConversation : IConversation
{
    IConversationListener? ConversationListener { get; }
    IConversationPublisher? ConversationPublisher { get; }
}
