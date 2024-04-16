#region

using FortitudeIO.Conversations;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Transports.Network.Config;
using FortitudeIO.Transports.Network.Construction;
using FortitudeIO.Transports.Network.State;

#endregion

namespace FortitudeIO.Transports.Network.Conversations.Builders;

public class UdpConversationSubscriberBuilder
{
    private ISocketFactoryResolver? socketFactories;

    public ISocketFactoryResolver SocketFactories
    {
        get => socketFactories ??= SocketFactoryResolver.GetRealSocketFactories();
        set => socketFactories = value;
    }

    public ConversationSubscriber Build(INetworkTopicConnectionConfig networkConnectionConfig
        , IMessageSerdesRepositoryFactory serdesFactory)
    {
        var conversationType = ConversationType.Subscriber;
        var conversationProtocol = SocketConversationProtocol.UdpSubscriber;

        var sockFactories = SocketFactories;

        var socketSessionContext = new SocketSessionContext(conversationType, conversationProtocol,
            networkConnectionConfig.TopicName, networkConnectionConfig, sockFactories
            , serdesFactory);
        socketSessionContext.Name += "Subscriber";

        var streamControls = sockFactories.StreamControlsFactory.ResolveStreamControls(socketSessionContext);

        return new ConversationSubscriber(socketSessionContext, streamControls);
    }
}
