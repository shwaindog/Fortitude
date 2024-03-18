#region

using FortitudeIO.Conversations;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Transports.NewSocketAPI.Config;
using FortitudeIO.Transports.NewSocketAPI.Controls;
using FortitudeIO.Transports.NewSocketAPI.Sockets;

#endregion

namespace FortitudeIO.Transports.NewSocketAPI.Conversations.Builders;

public class TcpConversationResponderBuilder
{
    private ISocketFactories? socketFactories;

    public ISocketFactories SocketFactories
    {
        get => socketFactories ??= Sockets.SocketFactories.GetRealSocketFactories();
        set => socketFactories = value;
    }

    public ConversationResponder Build(ISocketConnectionConfig socketConnectionConfig, ISerdesFactory serdesFactory)
    {
        var conversationType = ConversationType.Responder;
        var conversationProtocol = SocketConversationProtocol.TcpAcceptor;

        var socFactories = SocketFactories;

        var socketSessionContext = new SocketSessionContext(conversationType, conversationProtocol,
            socketConnectionConfig.SocketDescription.ToString(), socketConnectionConfig, socFactories, serdesFactory);
        socketSessionContext.Name += "Responder";

        var tcpAcceptorControls = new TcpAcceptorControls(socketSessionContext);

        return new ConversationResponder(socketSessionContext, tcpAcceptorControls);
    }
}
