#region

using FortitudeIO.Conversations;
using FortitudeIO.Protocols.Serialization;
using FortitudeIO.Transports.NewSocketAPI.Config;
using FortitudeIO.Transports.NewSocketAPI.Controls;
using FortitudeIO.Transports.NewSocketAPI.Sockets;

#endregion

namespace FortitudeIO.Transports.NewSocketAPI.Conversations.Builders;

public class TCPRequestResponseResponderBuilder
{
    private ISocketFactories? socketFactories;

    public ISocketFactories SocketFactories
    {
        get => socketFactories ??= Sockets.SocketFactories.GetRealSocketFactories();
        set => socketFactories = value;
    }

    public RequestResponseResponder Build(ISocketConnectionConfig socketConnectionConfig, ISerdesFactory serdesFactory)
    {
        var conversationType = ConversationType.RequestResponseResponder;
        var conversationProtocol = SocketConversationProtocol.TCPAcceptor;

        var socFactories = SocketFactories;

        var socketSessionContext = new SocketSessionContext(conversationType, conversationProtocol,
            socketConnectionConfig.SocketDescription.ToString(), socketConnectionConfig, socFactories, serdesFactory);
        socketSessionContext.ConversationDescription += "Responder";

        var tcpAcceptorControls = new TCPAcceptorControls(socketSessionContext);

        return new RequestResponseResponder(socketSessionContext, tcpAcceptorControls);
    }
}
