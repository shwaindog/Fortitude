#region

using FortitudeIO.Conversations;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Transports.NewSocketAPI.Config;
using FortitudeIO.Transports.NewSocketAPI.Construction;
using FortitudeIO.Transports.NewSocketAPI.Controls;
using FortitudeIO.Transports.NewSocketAPI.State;

#endregion

namespace FortitudeIO.Transports.NewSocketAPI.Conversations.Builders;

public class TcpConversationResponderBuilder
{
    private ISocketFactoryResolver? socketFactories;

    public ISocketFactoryResolver SocketFactories
    {
        get => socketFactories ??= SocketFactoryResolver.GetRealSocketFactories();
        set => socketFactories = value;
    }

    public ConversationResponder Build(ISocketTopicConnectionConfig socketConnectionConfig
        , ISerdesFactory serdesFactory)
    {
        var conversationType = ConversationType.Responder;
        var conversationProtocol = SocketConversationProtocol.TcpAcceptor;

        var sockFactories = SocketFactories;

        var socketSessionContext = new SocketSessionContext(conversationType, conversationProtocol,
            socketConnectionConfig.TopicName, socketConnectionConfig, sockFactories, serdesFactory);
        socketSessionContext.Name += "Responder";


        var acceptorControls
            = (IAcceptorControls)sockFactories.StreamControlsFactory.ResolveStreamControls(socketSessionContext);

        return new ConversationResponder(socketSessionContext, acceptorControls);
    }
}
