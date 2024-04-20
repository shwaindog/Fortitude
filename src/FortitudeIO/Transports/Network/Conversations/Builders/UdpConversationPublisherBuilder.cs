#region

using FortitudeIO.Conversations;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Transports.Network.Config;
using FortitudeIO.Transports.Network.Construction;
using FortitudeIO.Transports.Network.State;

#endregion

namespace FortitudeIO.Transports.Network.Conversations.Builders;

public class UdpConversationPublisherBuilder
{
    private ISocketFactoryResolver? socketFactories;

    public ISocketFactoryResolver SocketFactories
    {
        get => socketFactories ??= SocketFactoryResolver.GetRealSocketFactories();
        set => socketFactories = value;
    }

    public ConversationPublisher Build(INetworkTopicConnectionConfig networkConnectionConfig
        , IMessageSerdesRepositoryFactory serdesFactory)
    {
        var conversationType = ConversationType.Publisher;
        var conversationProtocol = SocketConversationProtocol.UdpPublisher;

        var sockFactories = SocketFactories;

        var socketSessionContext = new SocketSessionContext(networkConnectionConfig.TopicName + "Publisher", conversationType, conversationProtocol,
            networkConnectionConfig, sockFactories, serdesFactory);

        var streamControls = sockFactories.StreamControlsFactory.ResolveStreamControls(socketSessionContext);

        return new ConversationPublisher(socketSessionContext, streamControls);
    }
}
