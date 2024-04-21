#region

using FortitudeBusRules.BusMessaging.Pipelines;
using FortitudeBusRules.BusMessaging.Pipelines.Groups;
using FortitudeBusRules.Rules;
using FortitudeCommon.Monitoring.Logging;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Transports.Network.Config;
using FortitudeIO.Transports.Network.Dispatcher;
using FortitudeMarketsApi.Configuration.ClientServerConfig;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Subscription.BusRules;

public class PQClientUpdateSubscriberRule(string feedName, IMarketConnectionConfig marketConnectionConfig,
        IMessageDeserializationRepository sharedUpdateAndSnapshotDeserializationRepo, ISocketDispatcherResolver dispatcherResolver)
    : Rule("PQClient_" + feedName + "_PQClientSnapshotRule")
{
    private readonly IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(PQClientSnapshotRequesterRule));

    private readonly INetworkTopicConnectionConfig updateClientTopicConnectionConfig
        = marketConnectionConfig.PricingServerConfig!.UpdateConnectionConfig;

    private PQBusRulesUpdateClient? snapshotClient;

    public override async ValueTask StartAsync()
    {
        await LaunchUpdateClient();
    }

    private async ValueTask LaunchUpdateClient()
    {
        snapshotClient = PQBusRulesUpdateClient.BuildUdpSubscriber(updateClientTopicConnectionConfig, dispatcherResolver
            , sharedUpdateAndSnapshotDeserializationRepo);
        var workerQueueConnect = Context.GetEventQueues(MessageQueueType.Worker)
            .SelectEventQueue(QueueSelectionStrategy.EarliestCompleted).GetExecutionContextResult<bool, TimeSpan>(this);
        var connected = await snapshotClient.StartAsync(10_000, workerQueueConnect);
        if (connected)
        {
            logger.Info("Successfully subscribed to multicast pricing updates for {0}", feedName);
            return;
        }

        logger.Error(
            "Warning did not connect to PQUpdate Client typically this means there is a misconfiguration between your configuration and environment and will not auto resolve.");
    }
}
