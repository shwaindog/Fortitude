#region

using FortitudeBusRules.BusMessaging;
using FortitudeBusRules.Connectivity.Network;
using FortitudeCommon.Monitoring.Logging;
using FortitudeIO.Conversations;
using FortitudeIO.Transports.Network.Config;
using FortitudeIO.Transports.Network.Construction;
using FortitudeIO.Transports.Network.Controls;
using FortitudeIO.Transports.Network.Conversations;
using FortitudeIO.Transports.Network.Dispatcher;
using FortitudeIO.Transports.Network.State;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes;
using FortitudeMarkets.Pricing.PQ.Serdes;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Publication.BusRules;

public class PQPricingServerSnapshotResponder : ConversationResponder
{
    private static ISocketFactoryResolver? socketFactories;
    private readonly string feedName;
    private readonly IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(PQPricingServerSnapshotResponder));

    public PQPricingServerSnapshotResponder(ISocketSessionContext socketSessionContext, IAcceptorControls acceptorControls, string feedName)
        : base(socketSessionContext, acceptorControls) =>
        this.feedName = feedName;

    public static ISocketFactoryResolver SocketFactories
    {
        get => socketFactories ??= SocketFactoryResolver.GetRealSocketFactories();
        set => socketFactories = value;
    }

    public static PQPricingServerSnapshotResponder BuildTcpResponder(string feedName, INetworkTopicConnectionConfig networkConnectionConfig,
        ISocketDispatcherResolver socketDispatcherResolver, IMessageBus messageBus)
    {
        var conversationType = ConversationType.Responder;
        var conversationProtocol = SocketConversationProtocol.TcpAcceptor;

        var socFactories = SocketFactories;

        var serdesFactory = new PQServerSerdesRepositoryFactory(PQMessageFlags.Snapshot);

        var socketSessionContext = new BusSocketSessionContext(networkConnectionConfig.TopicName + "Responder", conversationType, conversationProtocol
            , networkConnectionConfig, socFactories, serdesFactory, socketDispatcherResolver);

        var acceptorControls = new BusTcpAcceptorControls(socketSessionContext, messageBus);

        return new PQPricingServerSnapshotResponder(socketSessionContext, acceptorControls, feedName);
    }
}
