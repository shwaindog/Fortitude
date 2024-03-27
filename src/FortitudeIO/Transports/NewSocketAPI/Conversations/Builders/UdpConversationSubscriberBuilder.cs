#region

using FortitudeIO.Conversations;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Transports.NewSocketAPI.Config;
using FortitudeIO.Transports.NewSocketAPI.Construction;
using FortitudeIO.Transports.NewSocketAPI.Controls;
using FortitudeIO.Transports.NewSocketAPI.State;

#endregion

namespace FortitudeIO.Transports.NewSocketAPI.Conversations.Builders;

public class UdpConversationSubscriberBuilder
{
    private ISocketFactoryResolver? socketFactories;

    public ISocketFactoryResolver SocketFactories
    {
        get => socketFactories ??= SocketFactoryResolver.GetRealSocketFactories();
        set => socketFactories = value;
    }

    public ConversationSubscriber Build(ISocketTopicConnectionConfig socketConnectionConfig
        , ISerdesFactory serdesFactory)
    {
        var conversationType = ConversationType.Subscriber;
        var conversationProtocol = SocketConversationProtocol.UdpSubscriber;

        var sockFactories = SocketFactories;

        var socketSessionContext = new SocketSessionContext(conversationType, conversationProtocol,
            socketConnectionConfig.TopicName, socketConnectionConfig, sockFactories
            , serdesFactory);
        socketSessionContext.Name += "Subscriber";

        var initiateControls
            = (IInitiateControls)sockFactories.StreamControlsFactory.ResolveStreamControls(socketSessionContext);

        return new ConversationSubscriber(socketSessionContext, initiateControls);
    }
}
