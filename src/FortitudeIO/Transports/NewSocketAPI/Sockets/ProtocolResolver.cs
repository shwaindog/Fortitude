#region

using FortitudeIO.Conversations;
using FortitudeIO.Transports.NewSocketAPI.Config;

#endregion

namespace FortitudeIO.Transports.NewSocketAPI.Sockets;

public class ProtocolResolver
{
    public SocketConversationProtocol ResolveProtocol(ConversationType conversationType,
        ISocketConnectionConfig socketConnectionConfig)
    {
        var isUdp = IsUdp(conversationType, socketConnectionConfig);
        if (isUdp && conversationType == ConversationType.Publisher) return SocketConversationProtocol.UDPPublisher;
        if (isUdp && conversationType == ConversationType.Subscriber) return SocketConversationProtocol.UDPSubscriber;

        if (conversationType == ConversationType.Publisher ||
            conversationType == ConversationType.RequestResponseResponder)
            return SocketConversationProtocol.TCPAcceptor;
        return SocketConversationProtocol.TCPClient;
    }

    private bool IsUdp(ConversationType conversationType, ISocketConnectionConfig socketConnectionConfig)
    {
        if (conversationType == ConversationType.Publisher || conversationType == ConversationType.Subscriber)
            return socketConnectionConfig.ConnectionAttributes == SocketConnectionAttributes.Multicast ||
                   (socketConnectionConfig.ConnectionAttributes == SocketConnectionAttributes.Fast &&
                    socketConnectionConfig.ConnectionAttributes != SocketConnectionAttributes.Reliable);
        return false;
    }
}
