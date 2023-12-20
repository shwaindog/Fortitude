#region

using FortitudeIO.Conversations;

#endregion

namespace FortitudeIO.Transports.NewSocketAPI.Sockets;

public interface ISocketConversation : IConversationState
{
    IConversationListener? ConversationListener { get; }
    IConversationPublisher? ConversationPublisher { get; }
    event Action? Connected;
    event Action? Disconnected;
}
