#region

using FortitudeIO.Conversations;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Transports.Network.Config;
using FortitudeIO.Transports.Network.Construction;
using FortitudeIO.Transports.Network.Controls;
using FortitudeIO.Transports.Network.Conversations;
using FortitudeIO.Transports.Network.Dispatcher;
using FortitudeIO.Transports.Network.State;
using FortitudeMarketsCore.Pricing.PQ.Serialization;
using FortitudeMarketsCore.Pricing.PQ.Subscription;
using SocketsAPI = FortitudeIO.Transports.Network;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Publication;

public sealed class PQUpdatePublisher : ConversationPublisher, IPQUpdateServer
{
    private static readonly PQServerSerializationRepository UpdateSerializationRepository = new(PQFeedType.Update);
    private static ISocketFactoryResolver? socketFactories;

    public PQUpdatePublisher(ISocketSessionContext socketSessionContext,
        IInitiateControls initiateControls)
        : base(socketSessionContext, initiateControls) =>
        socketSessionContext.SerdesFactory.StreamEncoderFactory = UpdateSerializationRepository;

    public static ISocketFactoryResolver SocketFactories
    {
        get =>
            socketFactories
                ??= SocketFactoryResolver.GetRealSocketFactories();
        set => socketFactories = value;
    }

    public static PQUpdatePublisher BuildUdpMulticastPublisher(INetworkTopicConnectionConfig networkConnectionConfig,
        ISocketDispatcherResolver? socketDispatcherResolver = null)
    {
        var conversationType = ConversationType.Publisher;
        var conversationProtocol = SocketConversationProtocol.UdpPublisher;

        var socFactories = SocketFactories;

        var serdesFactory = new SerdesFactory();

        var socketSessionContext = new SocketSessionContext(conversationType, conversationProtocol,
            networkConnectionConfig.TopicName, networkConnectionConfig, socFactories, serdesFactory
            , socketDispatcherResolver);
        socketSessionContext.Name += "Publisher";

        var initiateControls = new InitiateControls(socketSessionContext);

        return new PQUpdatePublisher(socketSessionContext, initiateControls);
    }
}
