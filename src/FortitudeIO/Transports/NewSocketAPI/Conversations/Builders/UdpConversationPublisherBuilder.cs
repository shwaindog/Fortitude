#region

using FortitudeIO.Conversations;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Transports.NewSocketAPI.Config;
using FortitudeIO.Transports.NewSocketAPI.Controls;
using FortitudeIO.Transports.NewSocketAPI.Sockets;

#endregion

namespace FortitudeIO.Transports.NewSocketAPI.Conversations.Builders;

public class UdpConversationPublisherBuilder
{
    private ISocketFactories? socketFactories;

    public ISocketFactories SocketFactories
    {
        get => socketFactories ??= Sockets.SocketFactories.GetRealSocketFactories();
        set => socketFactories = value;
    }

    public ConversationPublisher Build(ISocketConnectionConfig socketConnectionConfig, ISerdesFactory serdesFactory)
    {
        var conversationType = ConversationType.Publisher;
        var conversationProtocol = SocketConversationProtocol.UdpPublisher;

        var socketFactories = SocketFactories;

        var socketSessionContext = new SocketSessionContext(conversationType, conversationProtocol,
            socketConnectionConfig.SocketDescription.ToString(), socketConnectionConfig, socketFactories
            , serdesFactory);
        socketSessionContext.Name += "Publisher";

        var initiateControls = new InitiateControls(socketSessionContext);

        return new ConversationPublisher(socketSessionContext, initiateControls);
    }
}
