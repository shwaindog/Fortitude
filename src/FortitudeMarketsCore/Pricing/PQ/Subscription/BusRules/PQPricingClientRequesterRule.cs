#region

using FortitudeBusRules.BusMessaging.Messages.ListeningSubscriptions;
using FortitudeBusRules.BusMessaging.Pipelines;
using FortitudeBusRules.BusMessaging.Pipelines.Groups;
using FortitudeBusRules.BusMessaging.Routing.SelectionStrategies;
using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;
using FortitudeCommon.Monitoring.Logging;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Transports.Network.Config;
using FortitudeIO.Transports.Network.Dispatcher;
using FortitudeMarketsApi.Configuration.ClientServerConfig;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsCore.Pricing.PQ.Converters;
using FortitudeMarketsCore.Pricing.PQ.Subscription.BusRules.BusMessages;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Subscription.BusRules;

public class PQPricingClientRequesterRule(string feedName, IMarketConnectionConfig marketConnectionConfig,
        IMessageDeserializationRepository sharedFeedDeserializationRepo, ISocketDispatcherResolver dispatcherResolver)
    : Rule("PQClient_" + feedName + "_PQClientSnapshotRule")
{
    private readonly string feedAvailableTickersUpdateAddress = feedName.FeedAvailableTickersUpdateAddress();
    private readonly IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(PQPricingClientRequesterRule));

    private readonly INetworkTopicConnectionConfig snapshotClientTopicConnectionConfig
        = marketConnectionConfig.PricingServerConfig!.SnapshotConnectionConfig;

    private int connectionTimeout;

    private List<ISourceTickerQuoteInfo> lastReceivedSourceTickerQuoteInfos = new();
    private ISubscription? listenForRequestFeedAvailableTickersRefreshAddress;
    private ISubscription? listenForRequestSnapshotIdsAddress;
    private IRule? remoteMessageBusTopicPublicationAmenderRule;

    private PQPricingClientSnapshotConversationRequester? snapshotClient;

    public override async ValueTask StartAsync()
    {
        connectionTimeout = (int)snapshotClientTopicConnectionConfig.ConnectionTimeoutMs;
        await LaunchSnapshotClient();
    }

    public override async ValueTask StopAsync()
    {
        if (listenForRequestSnapshotIdsAddress != null) await listenForRequestSnapshotIdsAddress.UnsubscribeAsync();
        if (listenForRequestFeedAvailableTickersRefreshAddress != null) await listenForRequestFeedAvailableTickersRefreshAddress.UnsubscribeAsync();
        snapshotClient?.Stop();
    }

    private async ValueTask LaunchSnapshotClient()
    {
        listenForRequestFeedAvailableTickersRefreshAddress = await Context.MessageBus.RegisterListenerAsync<string>(this
            , feedName.FeedAvailableTickersRequestAddress(), CheckAndPublishChangedFeedTickersHandler);
        snapshotClient ??= PQPricingClientSnapshotConversationRequester.BuildTcpRequester(marketConnectionConfig, dispatcherResolver, feedName
            , this
            , sharedFeedDeserializationRepo);
        await AttemptSnapshotClientStart();
        listenForRequestSnapshotIdsAddress = await Context.MessageBus.RegisterListenerAsync<FeedSourceTickerInfoUpdate>(this
            , feedName.FeedTickersSnapshotRequestAddress(), SnapshotIdsRequestHandler);
    }

    private async ValueTask SnapshotIdsRequestHandler(IBusMessage<FeedSourceTickerInfoUpdate> snapshotIdsMessage)
    {
        var sourceTickerQuoteInfos = snapshotIdsMessage.Payload.Body()!.SourceTickerQuoteInfos;
        var workerQueueConnect = Context.GetEventQueues(MessageQueueType.Worker)
            .SelectEventQueue(QueueSelectionStrategy.EarliestCompleted).GetExecutionContextResult<bool, TimeSpan>(this);
        var succeeded = await (snapshotClient?.RequestSnapshots(sourceTickerQuoteInfos, connectionTimeout, workerQueueConnect)
                               ?? new ValueTask<bool>(false));
        if (!succeeded)
            logger.Warn("{0} did not successfully request snapshots for [{1}]", feedName,
                string.Join(", ", sourceTickerQuoteInfos.Select(stqi => stqi.Ticker)));
        else
            logger.Info("{0} requested snapshots for [{1}]", feedName,
                string.Join(", ", sourceTickerQuoteInfos.Select(stqi => stqi.Ticker)));
    }

    private async ValueTask AttemptSnapshotClientStart()
    {
        try
        {
            var startedSuccessfully = snapshotClient!.IsStarted;
            if (!startedSuccessfully) startedSuccessfully = await CallSnapshotClientStartOnWorkerQueue();
            if (startedSuccessfully)
            {
                await AttemptGetFeedAvailableTickersLaunchTopicAmender();
                return;
            }

            var nextAttemptTime = snapshotClientTopicConnectionConfig.ReconnectConfig.NextReconnectIntervalMs;
            logger.Warn("Warning did not connect to PQSnapshot Client will wait {0}ms before trying again", nextAttemptTime);
            Context.Timer.RunIn((int)nextAttemptTime, AttemptSnapshotClientStart);
        }
        catch (Exception ex)
        {
            var nextAttemptTime = snapshotClientTopicConnectionConfig.ReconnectConfig.NextReconnectIntervalMs;
            logger.Warn("Warning caught exception and did not connect to PQSnapshot Client will wait {0}ms before trying again. Got {1}"
                , nextAttemptTime, ex);
            Context.Timer.RunIn((int)nextAttemptTime, AttemptSnapshotClientStart);
        }
    }

    private async ValueTask AttemptGetFeedAvailableTickersLaunchTopicAmender()
    {
        remoteMessageBusTopicPublicationAmenderRule
            = Context.MessageBus.RulesMatching(r => r.FriendlyName == feedName.FeedAmendTickerPublicationRuleName()).FirstOrDefault();
        if (remoteMessageBusTopicPublicationAmenderRule is { LifeCycleState: RuleLifeCycle.Started }) return;
        var checkSourceTickerQuoteInfos = await RequestFeedServerSourceTickerQuoteInfos();
        if (checkSourceTickerQuoteInfos.Any())
        {
            remoteMessageBusTopicPublicationAmenderRule = new PQPricingClientBusTopicPublicationAmenderRule(
                feedName, checkSourceTickerQuoteInfos, snapshotClient!.SocketSessionContext, marketConnectionConfig.PricingServerConfig!
                , new PQtoPQPriceConverterRepository(), sharedFeedDeserializationRepo.Name);
            var deployedSocketListenerQueue = snapshotClient.IOInboundMessageQueue;
            var dispatchResult = await Context.MessageBus.DeployRuleAsync(this, remoteMessageBusTopicPublicationAmenderRule
                , new DeploymentOptions(RoutingFlags.TargetSpecific, MessageQueueType.IOInbound, 1, deployedSocketListenerQueue!.Name));

            logger.Info("Have deployed PQPricingClientBusTopicPublicationAmenderRule on {0} with dispatchResults {1}"
                , deployedSocketListenerQueue.Name, dispatchResult);
            await PublishUpdatedSourceTickerQuotInfos(checkSourceTickerQuoteInfos);
            return;
        }

        var nextRequestAttemptTime = snapshotClientTopicConnectionConfig.ReconnectConfig.NextReconnectIntervalMs;
        logger.Warn("Warning did not retrieve any SourceTickerQuoteInfos for {0} will wait {1}ms before trying again", feedName
            , nextRequestAttemptTime);
        Context.Timer.RunIn((int)nextRequestAttemptTime, AttemptGetFeedAvailableTickersLaunchTopicAmender);
    }

    private async ValueTask CheckAndPublishChangedFeedTickersHandler(IBusMessage<string> feedTickersMessage)
    {
        var sourceTickerQuoteInfos = await RequestFeedServerSourceTickerQuoteInfos();
        await PublishUpdatedSourceTickerQuotInfos(sourceTickerQuoteInfos);
    }

    private async ValueTask PublishUpdatedSourceTickerQuotInfos(List<ISourceTickerQuoteInfo> toPublish)
    {
        if (!toPublish.Any() || toPublish.SequenceEqual(lastReceivedSourceTickerQuoteInfos)) return;
        lastReceivedSourceTickerQuoteInfos = toPublish.ToList();
        var publishSourceTickerInfos = Context.PooledRecycler.Borrow<FeedSourceTickerInfoUpdate>();
        publishSourceTickerInfos.FeedName = feedName;
        publishSourceTickerInfos.SourceTickerQuoteInfos.AddRange(lastReceivedSourceTickerQuoteInfos);
        await Context.MessageBus.PublishAsync(this, feedAvailableTickersUpdateAddress, publishSourceTickerInfos, new DispatchOptions());
    }

    private async ValueTask<List<ISourceTickerQuoteInfo>> RequestFeedServerSourceTickerQuoteInfos()
    {
        if (!snapshotClient!.IsStarted) await CallSnapshotClientStartOnWorkerQueue();

        var lastPQSourceTickerInfoResponse = await snapshotClient.RequestSourceTickerQuoteInfoListAsync();
        if (lastPQSourceTickerInfoResponse == null)
        {
            logger.Warn(
                "Got no response when requesting source ticker information from PQSnapshot server for feed name {0}, on connection {1}",
                feedName, snapshotClientTopicConnectionConfig);
            return lastReceivedSourceTickerQuoteInfos;
        }

        return lastPQSourceTickerInfoResponse.SourceTickerQuoteInfos;
    }

    private async ValueTask<bool> CallSnapshotClientStartOnWorkerQueue()
    {
        var workerQueueConnect = Context.GetEventQueues(MessageQueueType.Worker)
            .SelectEventQueue(QueueSelectionStrategy.EarliestCompleted).GetExecutionContextResult<bool, TimeSpan>(this);
        return await snapshotClient!.StartAsync(connectionTimeout, workerQueueConnect);
    }
}
