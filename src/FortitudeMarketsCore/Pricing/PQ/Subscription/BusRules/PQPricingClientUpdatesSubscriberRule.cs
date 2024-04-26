#region

using FortitudeBusRules.BusMessaging.Pipelines;
using FortitudeBusRules.BusMessaging.Pipelines.Groups;
using FortitudeBusRules.BusMessaging.Routing.SelectionStrategies;
using FortitudeBusRules.Rules;
using FortitudeCommon.Monitoring.Logging;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Transports.Network.Config;
using FortitudeIO.Transports.Network.Dispatcher;
using FortitudeMarketsApi.Configuration.ClientServerConfig;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Subscription.BusRules;

public class PQPricingClientUpdatesSubscriberRule : Rule
{
    private readonly ISocketDispatcherResolver dispatcherResolver1;
    private readonly string feedName;
    private readonly IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(PQPricingClientRequesterRule));
    private readonly IMessageDeserializationRepository sharedFeedDeserializationRepo;

    private readonly INetworkTopicConnectionConfig updateClientTopicConnectionConfig;
    public PQPricingClientFeedSyncMonitorRule PqPricingClientFeedSyncMonitorRule;

    private PQPricingClientUpdatesConversationSubscriber? updateClient;

    public PQPricingClientUpdatesSubscriberRule(string feedName, IMarketConnectionConfig marketConnectionConfig,
        IMessageDeserializationRepository sharedFeedDeserializationRepo, ISocketDispatcherResolver dispatcherResolver) : base("PQClient_" + feedName +
        "_PQClientSnapshotRule")
    {
        this.feedName = feedName;
        this.sharedFeedDeserializationRepo = sharedFeedDeserializationRepo;
        dispatcherResolver1 = dispatcherResolver;
        updateClientTopicConnectionConfig = marketConnectionConfig.PricingServerConfig!.UpdateConnectionConfig;
        PqPricingClientFeedSyncMonitorRule = new PQPricingClientFeedSyncMonitorRule(feedName, sharedFeedDeserializationRepo);
    }

    public override async ValueTask StartAsync()
    {
        await LaunchUpdateClient();
    }

    public override async ValueTask StopAsync()
    {
        await Context.MessageBus.UndeployRuleAsync(this, PqPricingClientFeedSyncMonitorRule);
    }

    private async ValueTask LaunchUpdateClient()
    {
        updateClient ??= PQPricingClientUpdatesConversationSubscriber.BuildUdpSubscriber(updateClientTopicConnectionConfig, dispatcherResolver1
            , sharedFeedDeserializationRepo);
        var workerQueueConnect = Context.GetEventQueues(MessageQueueType.Worker)
            .SelectEventQueue(QueueSelectionStrategy.EarliestCompleted).GetExecutionContextResult<bool, TimeSpan>(this);
        var connected = await updateClient.StartAsync(10_000, workerQueueConnect);
        if (connected)
        {
            logger.Info("Successfully subscribed to multicast pricing updates for {0}", feedName);
            await LaunchTickerSyncMonitorOnUpdateFeedListener(updateClient);
            return;
        }

        logger.Error(
            "Warning did not connect to PQUpdate Client typically this means there is a misconfiguration between your configuration and environment and will not auto resolve.");
    }

    private async ValueTask LaunchTickerSyncMonitorOnUpdateFeedListener(
        PQPricingClientUpdatesConversationSubscriber startedUpdateClientUpdatesConversationSubscriber)
    {
        var deployedUpdatesSocketListenerQueue
            = Context.MessageBus.BusIOResolver.GetInboundQueueOnSocketListener(startedUpdateClientUpdatesConversationSubscriber.SocketSessionContext
                .SocketDispatcher.Listener!);
        PqPricingClientFeedSyncMonitorRule = new PQPricingClientFeedSyncMonitorRule(feedName, sharedFeedDeserializationRepo);
        await Context.MessageBus.DeployRuleAsync(this, PqPricingClientFeedSyncMonitorRule
            , new DeploymentOptions(RoutingFlags.TargetSpecific, MessageQueueType.IOInbound, 1, deployedUpdatesSocketListenerQueue!.Name));
    }
}
