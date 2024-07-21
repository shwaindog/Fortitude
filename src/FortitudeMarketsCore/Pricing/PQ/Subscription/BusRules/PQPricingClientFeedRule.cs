// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.BusMessaging.Pipelines.Groups;
using FortitudeBusRules.BusMessaging.Routing.SelectionStrategies;
using FortitudeBusRules.Connectivity.Network.Serdes.Deserialization;
using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;
using FortitudeCommon.Monitoring.Logging;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeMarketsApi.Configuration.ClientServerConfig;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Subscription.BusRules.BusMessages;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Subscription.BusRules;

public class PQPricingClientFeedRule : Rule
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(PQPricingClientFeedRule));

    private readonly string feedAddress;
    private readonly string feedAvailableTickersUpdateAddress;
    private readonly string feedFeedStatusUpdatePublishAddress;
    private readonly string feedName;
    private readonly string feedStatusRequestAddress;
    private readonly string feedTickersHealthRequestAddress;

    private readonly IMarketConnectionConfig marketConnectionConfig;

    private DateTime feedStartTime;

    private List<ISourceTickerInfo> latestReceivedSourceTickerInfos = new();

    private PQPricingClientRequesterRule?         pqClientSnapshotRequesterRule;
    private PQPricingClientUpdatesSubscriberRule? pqClientUpdateSubscriberRule;

    private PricingFeedStatus pricingFeedStatus;

    public PQPricingClientFeedRule(IMarketConnectionConfig marketConnectionConfig) : base("PQClientSourceFeedRule" + marketConnectionConfig.Name)
    {
        pricingFeedStatus = new PricingFeedStatus();
        feedName          = marketConnectionConfig.Name;
        feedAddress       = feedName.FeedAddress();

        feedStatusRequestAddress    = feedName.FeedStatusRequestAddress();
        this.marketConnectionConfig = marketConnectionConfig;

        feedTickersHealthRequestAddress    = feedName.FeedTickerHealthRequestAddress();
        feedAvailableTickersUpdateAddress  = feedName.FeedAvailableTickersUpdateAddress();
        feedFeedStatusUpdatePublishAddress = feedName.FeedStatusUpdateAddress();
    }

    public PricingFeedStatus PricingFeedStatus
    {
        get => pricingFeedStatus;
        set
        {
            if (pricingFeedStatus == value) return;
            pricingFeedStatus = value;
            this.Publish(feedFeedStatusUpdatePublishAddress,
                         new SourceFeedUpdate(pricingFeedStatus, feedName, feedAddress), new DispatchOptions(RoutingFlags.DefaultPublish));
        }
    }

    public override async ValueTask StartAsync()
    {
        feedStartTime = DateTime.UtcNow;

        PricingFeedStatus = PricingFeedStatus.Starting;
        await this.RegisterRequestListenerAsync<PricingFeedStatusRequest, PricingFeedStatusResponse>
            (feedStatusRequestAddress, ReceivedFeedStatusRequestHandler);
        await this.RegisterListenerAsync<FeedSourceTickerInfoUpdate>
            (feedAvailableTickersUpdateAddress, ReceivedFeedAvailableTickersUpdate);
        this.Publish(feedAddress,
                     new SourceFeedUpdate(PricingFeedStatus.Starting, feedName, feedAddress), new DispatchOptions(RoutingFlags.DefaultPublish));
        var socketDispatcherResolver = Context.MessageBus.BusIOResolver.GetDispatcherResolver(QueueSelectionStrategy.FirstInSet);
        var sharedFeedDeserializationRepository
            = new BusRulesMessageDeserializationRepository
                (new MessageDeserializationRepository($"{feedName}SharedSnapshotAndUpdateRepo", Context.PooledRecycler));
        pqClientSnapshotRequesterRule = new PQPricingClientRequesterRule
            (feedName, marketConnectionConfig, sharedFeedDeserializationRepository, socketDispatcherResolver);
        pqClientUpdateSubscriberRule = new PQPricingClientUpdatesSubscriberRule
            (feedName, marketConnectionConfig, sharedFeedDeserializationRepository, socketDispatcherResolver);

        var launchSnapshotClient = this.DeployRuleAsync(pqClientSnapshotRequesterRule, new DeploymentOptions());
        var launchUpdateClient   = this.DeployRuleAsync(pqClientUpdateSubscriberRule, new DeploymentOptions());
        await launchSnapshotClient;
        await launchUpdateClient;
        Logger.Info("Launched pricing client {0}", feedName);
    }

    public override async ValueTask StopAsync()
    {
        if (pqClientUpdateSubscriberRule != null) await Context.MessageBus.UndeployRuleAsync(this, pqClientUpdateSubscriberRule);
        if (pqClientSnapshotRequesterRule != null) await Context.MessageBus.UndeployRuleAsync(this, pqClientSnapshotRequesterRule);
    }

    private void ReceivedFeedAvailableTickersUpdate(IBusMessage<FeedSourceTickerInfoUpdate> sourceTickerInfosMessage)
    {
        PricingFeedStatus = PricingFeedStatus.AvailableForSubscription;
        var tickersUpdate = sourceTickerInfosMessage.Payload.Body();
        latestReceivedSourceTickerInfos = tickersUpdate!.SourceTickerInfos.ToList();
    }

    private async ValueTask<PricingFeedStatusResponse> ReceivedFeedStatusRequestHandler
    (
        IBusRespondingMessage<PricingFeedStatusRequest, PricingFeedStatusResponse> busRequestMessage)
    {
        PricingFeedStatusResponse? pricingFeedStatusResponse;
        if (pqClientUpdateSubscriberRule?.PqPricingClientFeedSyncMonitorRule.LifeCycleState == RuleLifeCycle.Started)
        {
            var tickerHealthResponse = await this.RequestAsync<PricingFeedStatusRequest, PricingFeedStatusResponse?>
                (feedTickersHealthRequestAddress, busRequestMessage.Payload.Body(), new DispatchOptions(RoutingFlags.TargetSpecific
                   , targetRule: pqClientUpdateSubscriberRule.PqPricingClientFeedSyncMonitorRule, timeoutMs: 3_000));
            pricingFeedStatusResponse = tickerHealthResponse;
            pricingFeedStatusResponse?.IncrementRefCount();
            pricingFeedStatusResponse ??= Context.PooledRecycler.Borrow<PricingFeedStatusResponse>();

            PricingFeedStatus = pricingFeedStatusResponse.HealthySubscribedTickers.Any() ? PricingFeedStatus.Ticking
                : pricingFeedStatusResponse.UnhealthySubscribedTickers.Any() ? PricingFeedStatus.NothingTicking
                : latestReceivedSourceTickerInfos.Any() ? PricingFeedStatus.AvailableForSubscription
                : PricingFeedStatus.Starting;
        }
        else
        {
            PricingFeedStatus = PricingFeedStatus.Starting;

            pricingFeedStatusResponse = Context.PooledRecycler.Borrow<PricingFeedStatusResponse>();
        }

        pricingFeedStatusResponse.FeedStatus = pricingFeedStatus;

        pricingFeedStatusResponse.AvailableTickersTickers = latestReceivedSourceTickerInfos;

        pricingFeedStatusResponse.StartTime = feedStartTime;

        return pricingFeedStatusResponse;
    }
}
