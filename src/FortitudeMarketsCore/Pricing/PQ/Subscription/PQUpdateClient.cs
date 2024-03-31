#region

using FortitudeCommon.DataStructures.Maps;
using FortitudeIO.Conversations;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Transports.Network.Config;
using FortitudeIO.Transports.Network.Construction;
using FortitudeIO.Transports.Network.Controls;
using FortitudeIO.Transports.Network.Conversations;
using FortitudeIO.Transports.Network.Dispatcher;
using FortitudeIO.Transports.Network.State;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Subscription;

public sealed class PQUpdateClient : ConversationSubscriber, IPQUpdateClient
{
    private static ISocketFactoryResolver? socketFactories;
    private readonly PQClientMessageStreamDecoder messageStreamDecoder;

    public PQUpdateClient(ISocketSessionContext socketSessionContext,
        IInitiateControls initiateControls)
        : base(socketSessionContext, initiateControls)
    {
        messageStreamDecoder
            = new PQClientMessageStreamDecoder(new ConcurrentMap<uint, IMessageDeserializer>(), PQFeedType.Snapshot);
        socketSessionContext.SerdesFactory.StreamDecoderFactory
            = new SocketStreamDecoderFactory(deserializers => messageStreamDecoder);
    }

    public static ISocketFactoryResolver SocketFactories
    {
        get =>
            socketFactories
                ??= SocketFactoryResolver.GetRealSocketFactories();
        set => socketFactories = value;
    }

    public IMessageStreamDecoder MessageStreamDecoder => messageStreamDecoder;

    public static PQUpdateClient BuildUdpSubscriber(INetworkTopicConnectionConfig networkConnectionConfig
        , ISocketDispatcherResolver? socketDispatcherResolver = null)
    {
        var conversationType = ConversationType.Subscriber;
        var conversationProtocol = SocketConversationProtocol.UdpSubscriber;

        var sockFactories = SocketFactories;

        var serdesFactory = new SerdesFactory();

        var socketSessionContext = new SocketSessionContext(conversationType, conversationProtocol,
            networkConnectionConfig.TopicName, networkConnectionConfig, sockFactories
            , serdesFactory, socketDispatcherResolver);
        socketSessionContext.Name += "Subscriber";

        var initControls
            = (IInitiateControls)sockFactories.StreamControlsFactory.ResolveStreamControls(socketSessionContext);

        return new PQUpdateClient(socketSessionContext, initControls);
    }
}
