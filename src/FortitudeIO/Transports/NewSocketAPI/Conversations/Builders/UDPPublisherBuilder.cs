#region

using FortitudeIO.Protocols.Serialization;
using FortitudeIO.Transports.NewSocketAPI.Controls;
using FortitudeIO.Transports.NewSocketAPI.SocketFactory;
using FortitudeIO.Transports.NewSocketAPI.Sockets;

#endregion

namespace FortitudeIO.Transports.NewSocketAPI.Conversations.Builders;

public class UDPPublisherBuilder
{
    private ISocketFactories? socketFactories;

    public ISocketFactories SocketFactories
    {
        get => socketFactories ??= Sockets.SocketFactories.GetRealSocketFactories();
        set => socketFactories = value;
    }

    public PublisherConversation Build(ISocketConnectionConfig socketConnectionConfig, ISerdesFactory serdesFactory)
    {
        var conversationType = ConversationType.Publisher;
        var conversationProtocol = SocketConversationProtocol.UDPPublisher;

        var socketFactories = SocketFactories;

        var socketSessionContext = new SocketSessionContext(conversationType, conversationProtocol,
            socketConnectionConfig.SocketDescription.ToString(), socketConnectionConfig, socketFactories
            , serdesFactory);
        socketSessionContext.ConversationDescription += "Publisher";

        var initiateControls = new InitiateControls(socketSessionContext);

        return new PublisherConversation(socketSessionContext, initiateControls);
    }
}
