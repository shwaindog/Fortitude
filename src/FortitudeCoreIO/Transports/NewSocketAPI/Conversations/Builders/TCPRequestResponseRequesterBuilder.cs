#region

using FortitudeIO.Protocols.Serialization;
using FortitudeIO.Transports.NewSocketAPI.Controls;
using FortitudeIO.Transports.NewSocketAPI.SocketFactory;
using FortitudeIO.Transports.NewSocketAPI.Sockets;

#endregion

namespace FortitudeIO.Transports.NewSocketAPI.Conversations.Builders;

public class TCPRequestResponseRequesterBuilder
{
    private ISocketFactories? socketFactories;

    public ISocketFactories SocketFactories
    {
        get => socketFactories ??= Sockets.SocketFactories.GetRealSocketFactories();
        set => socketFactories = value;
    }

    public RequestResponseRequester Build(ISocketConnectionConfig socketConnectionConfig, ISerdesFactory serdesFactory)
    {
        var conversationType = ConversationType.RequestResponseRequester;
        var conversationProtocol = SocketConversationProtocol.TCPClient;

        var sockFactories = SocketFactories;

        var socketSessionContext = new SocketSessionContext(conversationType, conversationProtocol,
            socketConnectionConfig.SocketDescription.ToString(), socketConnectionConfig, sockFactories, serdesFactory);
        socketSessionContext.ConversationDescription += "Requester";

        var initiateControls = new InitiateControls(socketSessionContext);

        return new RequestResponseRequester(socketSessionContext, initiateControls);
    }
}
