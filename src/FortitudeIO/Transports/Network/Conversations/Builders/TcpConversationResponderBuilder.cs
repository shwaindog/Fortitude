#region

using FortitudeIO.Conversations;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Transports.Network.Config;
using FortitudeIO.Transports.Network.Construction;
using FortitudeIO.Transports.Network.Controls;
using FortitudeIO.Transports.Network.State;

#endregion

namespace FortitudeIO.Transports.Network.Conversations.Builders;

public class TcpConversationResponderBuilder
{
    private ISocketFactoryResolver? socketFactories;

    public ISocketFactoryResolver SocketFactories
    {
        get => socketFactories ??= SocketFactoryResolver.GetRealSocketFactories();
        set => socketFactories = value;
    }

    public ConversationResponder Build(INetworkTopicConnectionConfig networkConnectionConfig
        , IMessageSerdesRepositoryFactory serdesFactory)
    {
        var conversationType = ConversationType.Responder;
        var conversationProtocol = SocketConversationProtocol.TcpAcceptor;

        var sockFactories = SocketFactories;

        var socketSessionContext = new SocketSessionContext(conversationType, conversationProtocol,
            networkConnectionConfig.TopicName, networkConnectionConfig, sockFactories, serdesFactory);
        socketSessionContext.Name += "Responder";


        var acceptorControls
            = (IAcceptorControls)sockFactories.StreamControlsFactory.ResolveStreamControls(socketSessionContext);

        return new ConversationResponder(socketSessionContext, acceptorControls);
    }
}
