#region

using FortitudeIO.Conversations;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Transports.NewSocketAPI.Config;
using FortitudeIO.Transports.NewSocketAPI.Controls;
using FortitudeIO.Transports.NewSocketAPI.Sockets;

#endregion

namespace FortitudeIO.Transports.NewSocketAPI.Conversations.Builders;

public class TcpConversationRequesterBuilder
{
    private ISocketFactories? socketFactories;

    public ISocketFactories SocketFactories
    {
        get => socketFactories ??= Sockets.SocketFactories.GetRealSocketFactories();
        set => socketFactories = value;
    }

    public ConversationRequester Build(ISocketConnectionConfig socketConnectionConfig, ISerdesFactory serdesFactory)
    {
        var conversationType = ConversationType.Requester;
        var conversationProtocol = SocketConversationProtocol.TcpClient;

        var sockFactories = SocketFactories;

        var socketSessionContext = new SocketSessionContext(conversationType, conversationProtocol,
            socketConnectionConfig.SocketDescription.ToString(), socketConnectionConfig, sockFactories, serdesFactory);
        socketSessionContext.Name += "Requester";

        var initiateControls = new InitiateControls(socketSessionContext);

        return new ConversationRequester(socketSessionContext, initiateControls);
    }
}
