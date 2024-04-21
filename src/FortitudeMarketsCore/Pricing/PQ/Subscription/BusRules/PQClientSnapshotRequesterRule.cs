#region

using FortitudeBusRules.BusMessaging.Messages.ListeningSubscriptions;
using FortitudeBusRules.BusMessaging.Pipelines;
using FortitudeBusRules.BusMessaging.Pipelines.Groups;
using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;
using FortitudeCommon.Monitoring.Logging;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Transports.Network.Config;
using FortitudeIO.Transports.Network.Dispatcher;
using FortitudeMarketsApi.Configuration.ClientServerConfig;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsCore.Pricing.PQ.Subscription.BusRules.BusMessages;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Subscription.BusRules;

public class PQClientSnapshotRequesterRule(string feedName, IMarketConnectionConfig marketConnectionConfig,
        IMessageDeserializationRepository sharedUpdateAndSnapshotDeserializationRepo, ISocketDispatcherResolver dispatcherResolver)
    : Rule("PQClient_" + feedName + "_PQClientSnapshotRule")
{
    private readonly string feedAvailableTickersUpdateAddress = feedName.FeedAvailableTickersUpdateAddress();
    private readonly IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(PQClientSnapshotRequesterRule));

    private readonly INetworkTopicConnectionConfig snapshotClientTopicConnectionConfig
        = marketConnectionConfig.PricingServerConfig!.SnapshotConnectionConfig;

    private List<ISourceTickerQuoteInfo> lastReceivedSourceTickerQuoteInfos = new();
    private ISubscription? listenForRequestFeedAvailableTickersRefreshAddress;

    private PQBusRulesSnapshotClient? snapshotClient;

    public override async ValueTask StartAsync()
    {
        await LaunchSnapshotClient();
    }

    private async ValueTask LaunchSnapshotClient()
    {
        listenForRequestFeedAvailableTickersRefreshAddress = await Context.MessageBus.RegisterListenerAsync<string>(this
            , feedName.RequestToPublishRefreshedFeedAvailableTickersAddress(), CheckAndPublishChangedFeedTickersTemplateListener);
        await AttemptSnapshotClientConnect();
    }

    private async ValueTask AttemptSnapshotClientConnect()
    {
        try
        {
            snapshotClient ??= PQBusRulesSnapshotClient.BuildTcpRequester(marketConnectionConfig, dispatcherResolver, feedName, this
                , sharedUpdateAndSnapshotDeserializationRepo);
            var workerQueueConnect = Context.GetEventQueues(MessageQueueType.Worker)
                .SelectEventQueue(QueueSelectionStrategy.EarliestCompleted).GetExecutionContextResult<bool, TimeSpan>(this);
            var connected = await snapshotClient.StartAsync(10_000, workerQueueConnect);
            if (connected)
            {
                var checkSourceTickerQuoteInfos = await RequestFeedServerSourceTickerQuoteInfos();
                if (checkSourceTickerQuoteInfos.Any())
                {
                    await PublishUpdatedSourceTickerQuotInfos(checkSourceTickerQuoteInfos);
                    return;
                }
            }

            var nextAttemptTime = snapshotClientTopicConnectionConfig.ReconnectConfig.NextReconnectIntervalMs;
            logger.Warn("Warning did not connect to PQSnapshot Client will wait {0}ms before trying again", nextAttemptTime);
            Context.Timer.RunIn((int)nextAttemptTime, AttemptSnapshotClientConnect);
        }
        catch (Exception ex)
        {
            var nextAttemptTime = snapshotClientTopicConnectionConfig.ReconnectConfig.NextReconnectIntervalMs;
            logger.Warn("Warning caught exception and did not connect to PQSnapshot Client will wait {0}ms before trying again. Got {1}"
                , nextAttemptTime, ex);
            Context.Timer.RunIn((int)nextAttemptTime, AttemptSnapshotClientConnect);
        }
    }

    private async ValueTask CheckAndPublishChangedFeedTickersTemplateListener(IBusMessage<string> feedTickersMessage)
    {
        var sourceTickerQuoteInfos = await RequestFeedServerSourceTickerQuoteInfos();
        await PublishUpdatedSourceTickerQuotInfos(sourceTickerQuoteInfos);
    }

    private async ValueTask PublishUpdatedSourceTickerQuotInfos(List<ISourceTickerQuoteInfo> toPublish)
    {
        if (!toPublish.Any() || toPublish.SequenceEqual(lastReceivedSourceTickerQuoteInfos)) return;
        lastReceivedSourceTickerQuoteInfos = toPublish;
        var publishSourceTickerInfos = Context.PooledRecycler.Borrow<FeedSourceTickerInfoUpdate>();
        publishSourceTickerInfos.FeedName = feedName;
        publishSourceTickerInfos.FeedSourceTickerQuoteInfos = toPublish;
        await Context.MessageBus.PublishAsync(this, feedAvailableTickersUpdateAddress, publishSourceTickerInfos, new DispatchOptions());
    }

    private async ValueTask<List<ISourceTickerQuoteInfo>> RequestFeedServerSourceTickerQuoteInfos()
    {
        if (snapshotClient == null || !snapshotClient.IsStarted)
        {
            logger.Warn(
                "Can not request source ticker infos as the snapshot client has not been started. Or was not able to connect.  Returning last successful source ticker infos retrieved.");
            return lastReceivedSourceTickerQuoteInfos;
        }

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

    public override async ValueTask StopAsync()
    {
        if (listenForRequestFeedAvailableTickersRefreshAddress != null) await listenForRequestFeedAvailableTickersRefreshAddress.UnsubscribeAsync();
        snapshotClient?.Stop();
    }
}
