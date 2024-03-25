#region

using FortitudeIO.Conversations;
using FortitudeIO.Transports.NewSocketAPI.Config;

#endregion

namespace FortitudeIO.Transports.NewSocketAPI.Sockets;

public class ProtocolResolver
{
    public SocketConversationProtocol ResolveProtocol(ConversationType conversationType,
        ISocketTopicConnectionConfig socketConnectionConfig)
    {
        var isUdp = IsUdp(conversationType, socketConnectionConfig);
        if (isUdp && conversationType == ConversationType.Publisher) return SocketConversationProtocol.UdpPublisher;
        if (isUdp && conversationType == ConversationType.Subscriber) return SocketConversationProtocol.UdpSubscriber;

        if (conversationType == ConversationType.Publisher ||
            conversationType == ConversationType.Responder)
            return SocketConversationProtocol.TcpAcceptor;
        return SocketConversationProtocol.TcpClient;
    }

    private bool IsUdp(ConversationType conversationType, ISocketTopicConnectionConfig socketConnectionConfig)
    {
        if (conversationType == ConversationType.Publisher || conversationType == ConversationType.Subscriber)
            return socketConnectionConfig.ConnectionAttributes == SocketConnectionAttributes.Multicast ||
                   (socketConnectionConfig.ConnectionAttributes == SocketConnectionAttributes.Fast &&
                    socketConnectionConfig.ConnectionAttributes != SocketConnectionAttributes.Reliable);
        return false;
    }
}
