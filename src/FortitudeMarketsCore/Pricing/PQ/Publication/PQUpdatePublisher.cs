#region

using FortitudeIO.Conversations;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Transports.NewSocketAPI.Config;
using FortitudeIO.Transports.NewSocketAPI.Construction;
using FortitudeIO.Transports.NewSocketAPI.Controls;
using FortitudeIO.Transports.NewSocketAPI.Conversations;
using FortitudeIO.Transports.NewSocketAPI.Dispatcher;
using FortitudeIO.Transports.NewSocketAPI.State;
using FortitudeMarketsCore.Pricing.PQ.Serialization;
using FortitudeMarketsCore.Pricing.PQ.Subscription;
using SocketsAPI = FortitudeIO.Transports.NewSocketAPI;

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
