// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.BusMessaging.Pipelines;
using FortitudeBusRules.BusMessaging.Pipelines.Groups;
using FortitudeBusRules.BusMessaging.Routing.SelectionStrategies;
using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;
using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.Monitoring.Logging;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Transports.Network.Config;
using FortitudeIO.Transports.Network.Dispatcher;
using FortitudeMarketsApi.Configuration.ClientServerConfig;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Converters;
using FortitudeMarketsCore.Pricing.PQ.Subscription.BusRules.BusMessages;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Subscription.BusRules;

public class PQPricingClientRequesterRule
    (string feedName, IMarketConnectionConfig marketConnectionConfig,
        IMessageDeserializationRepository sharedFeedDeserializationRepo, ISocketDispatcherResolver dispatcherResolver)
    : Rule("PQClient_" + feedName + "_PQClientSnapshotRule")
{
    private readonly string feedAvailableTickersUpdateAddress = feedName.FeedAvailableTickersUpdateAddress();

    private readonly IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(PQPricingClientRequesterRule));

    private readonly INetworkTopicConnectionConfig snapshotClientTopicConnectionConfig =
        marketConnectionConfig.PricingServerConfig!.SnapshotConnectionConfig;

    private int connectionTimeout;

    private List<ISourceTickerInfo> lastReceivedSourceTickerInfos = new();

    private IRule? remoteMessageBusTopicPublicationAmenderRule;

    private PQPricingClientSnapshotConversationRequester? snapshotClient;

    public override async ValueTask StartAsync()
    {
        connectionTimeout = (int)snapshotClientTopicConnectionConfig.ConnectionTimeoutMs;
        await LaunchSnapshotClient();
    }

    public override ValueTask StopAsync()
    {
        snapshotClient?.Stop();
        return base.StopAsync();
    }

    private async ValueTask LaunchSnapshotClient()
    {
        await this.RegisterListenerAsync<string>(feedName.FeedAvailableTickersRequestAddress(), CheckAndPublishChangedFeedTickersHandler);
        snapshotClient ??= PQPricingClientSnapshotConversationRequester.BuildTcpRequester
            (marketConnectionConfig, dispatcherResolver, feedName, this, sharedFeedDeserializationRepo);
        await AttemptSnapshotClientStart();
    }

    private async ValueTask SnapshotIdsRequestHandler(IBusMessage<FeedSourceTickerInfoUpdate> snapshotIdsMessage)
    {
        var sourceTickerInfos = snapshotIdsMessage.Payload.Body().SourceTickerInfos;
        logger.Info("Received request to publish snapshot ids [{0}]", sourceTickerInfos.JoinToString());
        var workerQueueConnect =
            Context.GetEventQueues
                       (MessageQueueType.Worker).SelectEventQueue(QueueSelectionStrategy.EarliestCompleted)
                   .GetExecutionContextResult<bool, TimeSpan>(this);
        var succeeded = await (snapshotClient?.RequestSnapshots(sourceTickerInfos, connectionTimeout, workerQueueConnect)
                            ?? new ValueTask<bool>(false));
        if (!succeeded)
            logger.Warn("{0} did not successfully request snapshots for [{1}]", feedName
                      , string.Join(", ", sourceTickerInfos.Select(stqi => stqi.Ticker)));
        else
            logger.Info("{0} requested snapshots for [{1}]", feedName
                      , string.Join(", ", sourceTickerInfos.Select(stqi => stqi.Ticker)));
    }

    private async ValueTask AttemptSnapshotClientStart()
    {
        try
        {
            var startedSuccessfully = snapshotClient!.IsStarted;

            if (!startedSuccessfully) startedSuccessfully = await CallSnapshotClientStartOnWorkerQueue();
            if (startedSuccessfully)
            {
                await this.RegisterListenerAsync<FeedSourceTickerInfoUpdate>(feedName.FeedTickersSnapshotRequestAddress(), SnapshotIdsRequestHandler);
                await AttemptGetFeedAvailableTickersLaunchTopicAmender();
                return;
            }

            var nextAttemptTime = snapshotClientTopicConnectionConfig.ReconnectConfig.NextReconnectIntervalMs;
            logger.Warn("Warning did not connect to PQSnapshot Client will wait {0}ms before trying again", nextAttemptTime);
            Timer.RunIn((int)nextAttemptTime, AttemptSnapshotClientStart);
        }
        catch (Exception ex)
        {
            var nextAttemptTime = snapshotClientTopicConnectionConfig.ReconnectConfig.NextReconnectIntervalMs;
            logger.Warn("Warning caught exception and did not connect to PQSnapshot Client will wait {0}ms before trying again. Got {1}"
                      , nextAttemptTime, ex);
            Timer.RunIn((int)nextAttemptTime, AttemptSnapshotClientStart);
        }
    }

    private async ValueTask AttemptGetFeedAvailableTickersLaunchTopicAmender()
    {
        remoteMessageBusTopicPublicationAmenderRule
            = Context.MessageBus.RulesMatching(r => r.FriendlyName == feedName.FeedAmendTickerPublicationRuleName()).FirstOrDefault();
        if (remoteMessageBusTopicPublicationAmenderRule is { LifeCycleState: RuleLifeCycle.Started }) return;
        var checkSourceTickerInfos = await RequestFeedServerSourceTickerInfos();
        if (checkSourceTickerInfos.Any())
        {
            remoteMessageBusTopicPublicationAmenderRule = new PQPricingClientBusTopicPublicationAmenderRule(
             feedName, checkSourceTickerInfos, snapshotClient!.SocketSessionContext, marketConnectionConfig.PricingServerConfig!
           , new PQtoPQPriceConverterRepository(), sharedFeedDeserializationRepo.Name);
            var deployedSocketListenerQueue = snapshotClient.NetworkInboundMessageQueue;
            var dispatchResult = await Context.MessageBus.DeployChildRuleAsync
                (this, remoteMessageBusTopicPublicationAmenderRule,
                 new DeploymentOptions(RoutingFlags.TargetSpecific, MessageQueueType.NetworkInbound, 1, deployedSocketListenerQueue!.Name));

            // logger.Info("Have deployed PQPricingClientBusTopicPublicationAmenderRule on {0} with dispatchResults {1}"
            //     , deployedSocketListenerQueue.Name, dispatchResult);
            await PublishUpdatedSourceTickerQuotInfos(checkSourceTickerInfos);
            return;
        }

        var nextRequestAttemptTime = snapshotClientTopicConnectionConfig.ReconnectConfig.NextReconnectIntervalMs;
        logger.Warn("Warning did not retrieve any SourceTickerInfos for {0} will wait {1}ms before trying again", feedName
                  , nextRequestAttemptTime);
        Timer.RunIn((int)nextRequestAttemptTime, AttemptGetFeedAvailableTickersLaunchTopicAmender);
    }

    private async ValueTask CheckAndPublishChangedFeedTickersHandler(IBusMessage<string> feedTickersMessage)
    {
        var sourceTickerInfos = await RequestFeedServerSourceTickerInfos();
        await PublishUpdatedSourceTickerQuotInfos(sourceTickerInfos);
    }

    private async ValueTask PublishUpdatedSourceTickerQuotInfos(List<ISourceTickerInfo> toPublish)
    {
        if (!toPublish.Any() || toPublish.SequenceEqual(lastReceivedSourceTickerInfos)) return;
        lastReceivedSourceTickerInfos = toPublish.ToList();
        var publishSourceTickerInfos = Context.PooledRecycler.Borrow<FeedSourceTickerInfoUpdate>();
        publishSourceTickerInfos.FeedName = feedName;
        publishSourceTickerInfos.SourceTickerInfos.AddRange(lastReceivedSourceTickerInfos);
        await this.PublishAsync(feedAvailableTickersUpdateAddress, publishSourceTickerInfos, new DispatchOptions());
    }

    private async ValueTask<List<ISourceTickerInfo>> RequestFeedServerSourceTickerInfos()
    {
        if (!snapshotClient!.IsStarted) await CallSnapshotClientStartOnWorkerQueue();

        var lastPQSourceTickerInfoResponse = await snapshotClient.RequestSourceTickerInfoListAsync();
        if (lastPQSourceTickerInfoResponse == null)
        {
            logger.Warn
                ("Got no response when requesting source ticker information from PQSnapshot server for feed name {0}, on connection {1}"
               , feedName, snapshotClientTopicConnectionConfig);
            return lastReceivedSourceTickerInfos;
        }

        return lastPQSourceTickerInfoResponse.SourceTickerInfos;
    }

    private async ValueTask<bool> CallSnapshotClientStartOnWorkerQueue()
    {
        var workerQueueConnect =
            Context.GetEventQueues(MessageQueueType.Worker)
                   .SelectEventQueue(QueueSelectionStrategy.EarliestCompleted).GetExecutionContextResult<bool, TimeSpan>(this);
        return await snapshotClient!.StartAsync(connectionTimeout, workerQueueConnect);
    }
}
