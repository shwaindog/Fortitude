#region

using FortitudeIO.Conversations;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Transports.Network.Config;
using FortitudeIO.Transports.Network.Construction;
using FortitudeIO.Transports.Network.Controls;
using FortitudeIO.Transports.Network.State;

#endregion

namespace FortitudeIO.Transports.Network.Conversations.Builders;

public class TcpConversationRequesterBuilder
{
    private ISocketFactoryResolver? socketFactories;

    public ISocketFactoryResolver SocketFactories
    {
        get => socketFactories ??= SocketFactoryResolver.GetRealSocketFactories();
        set => socketFactories = value;
    }

    public ConversationRequester Build(INetworkTopicConnectionConfig networkConnectionConfig
        , IMessageSerdesRepositoryFactory serdesFactory)
    {
        var conversationType = ConversationType.Requester;
        var conversationProtocol = SocketConversationProtocol.TcpClient;

        var sockFactories = SocketFactories;

        var socketSessionContext = new SocketSessionContext(conversationType, conversationProtocol,
            networkConnectionConfig.TopicName, networkConnectionConfig, sockFactories, serdesFactory);
        socketSessionContext.Name += "Requester";

        var initiateControls
            = (IInitiateControls)sockFactories.StreamControlsFactory.ResolveStreamControls(socketSessionContext);

        return new ConversationRequester(socketSessionContext, initiateControls);
    }
}
