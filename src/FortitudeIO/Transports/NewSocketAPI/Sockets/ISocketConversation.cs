#region

using FortitudeIO.Conversations;

#endregion

namespace FortitudeIO.Transports.NewSocketAPI.Sockets;

public interface ISocketConversation : IConversation
{
    IConversationListener? ConversationListener { get; }
    IConversationPublisher? ConversationPublisher { get; }
    event Action? Connected;
    event Action? Disconnected;
    event Action<SocketSessionState>? StateChanged;
    event Action<ISocketConnection>? SocketConnected;
    event Action? Disconnecting;
}
