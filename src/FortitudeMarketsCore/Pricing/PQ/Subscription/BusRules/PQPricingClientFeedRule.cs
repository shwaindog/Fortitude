﻿#region

using FortitudeBusRules.BusMessaging.Messages.ListeningSubscriptions;
using FortitudeBusRules.BusMessaging.Pipelines.Groups;
using FortitudeBusRules.BusMessaging.Routing.SelectionStrategies;
using FortitudeBusRules.Connectivity.Network.Serdes.Deserialization;
using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeMarketsApi.Configuration.ClientServerConfig;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsCore.Pricing.PQ.Subscription.BusRules.BusMessages;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Subscription.BusRules;

public class PQPricingClientFeedRule : Rule
{
    private readonly string feedAddress;
    private readonly string feedAvailableTickersUpdateAddress;
    private readonly string feedFeedStatusUpdatePublishAddress;
    private readonly string feedName;
    private readonly string feedStatusRequestAddress;
    private readonly string feedTickersHealthRequestAddress;
    private readonly IMarketConnectionConfig marketConnectionConfig;
    private DateTime feedStartTime;

    private ISubscription? feedStatusRequestListenerSubscription;
    private ISubscription? feedTickersListenerSubscription;

    private List<ISourceTickerQuoteInfo> latestReceivedSourceTickerQuoteInfos = new();

    private PQPricingClientRequesterRule? pqClientSnapshotRequesterRule;
    private PQPricingClientUpdatesSubscriberRule? pqClientUpdateSubscriberRule;

    private PricingFeedStatus pricingFeedStatus;

    public PQPricingClientFeedRule(IMarketConnectionConfig marketConnectionConfig) : base("PQClientSourceFeedRule" + marketConnectionConfig.Name)
    {
        pricingFeedStatus = new PricingFeedStatus();
        this.marketConnectionConfig = marketConnectionConfig;
        feedName = marketConnectionConfig.Name;
        feedAddress = feedName.FeedAddress();
        feedStatusRequestAddress = feedName.FeedStatusRequestAddress();
        feedTickersHealthRequestAddress = feedName.FeedTickerHealthRequestAddress();
        feedAvailableTickersUpdateAddress = feedName.FeedAvailableTickersUpdateAddress();
        feedFeedStatusUpdatePublishAddress = feedName.FeedStatusUpdateAddress();
    }

    public PricingFeedStatus PricingFeedStatus
    {
        get => pricingFeedStatus;
        set
        {
            if (pricingFeedStatus == value) return;
            pricingFeedStatus = value;
            Context.MessageBus.Publish(this, feedFeedStatusUpdatePublishAddress,
                new SourceFeedUpdate(pricingFeedStatus, feedName, feedAddress), new DispatchOptions(RoutingFlags.DefaultPublish));
        }
    }

    public override async ValueTask StartAsync()
    {
        feedStartTime = DateTime.UtcNow;
        PricingFeedStatus = PricingFeedStatus.Starting;
        feedStatusRequestListenerSubscription
            = await Context.MessageBus.RegisterRequestListenerAsync<PricingFeedStatusRequest, PricingFeedStatusResponse>
                (this, feedStatusRequestAddress, ReceivedFeedStatusRequestHandler);
        feedTickersListenerSubscription = await Context.MessageBus.RegisterListenerAsync<FeedSourceTickerInfoUpdate>(
            this, feedAvailableTickersUpdateAddress, ReceivedFeedAvailableTickersUpdate);
        Context.MessageBus.Publish(this, feedAddress,
            new SourceFeedUpdate(PricingFeedStatus.Starting, feedName, feedAddress), new DispatchOptions(RoutingFlags.DefaultPublish));
        var socketDispatcherResolver = Context.MessageBus.BusIOResolver.GetDispatcherResolver(QueueSelectionStrategy.FirstInSet);
        var sharedFeedDeserializationRepository
            = new BusRulesMessageDeserializationRepository(new MessageDeserializationRepository($"{feedName}SharedSnapshotAndUpdateRepo"
                , Context.PooledRecycler));
        pqClientSnapshotRequesterRule = new PQPricingClientRequesterRule(feedName, marketConnectionConfig
            , sharedFeedDeserializationRepository, socketDispatcherResolver);
        pqClientUpdateSubscriberRule = new PQPricingClientUpdatesSubscriberRule(feedName, marketConnectionConfig
            , sharedFeedDeserializationRepository, socketDispatcherResolver);
        var launchSnapshotClient = Context.MessageBus.DeployRuleAsync(this, pqClientSnapshotRequesterRule, new DeploymentOptions());
        var launchUpdateClient = Context.MessageBus.DeployRuleAsync(this, pqClientUpdateSubscriberRule, new DeploymentOptions());
        await launchSnapshotClient;
        await launchUpdateClient;
    }

    public override async ValueTask StopAsync()
    {
        // ReSharper disable MethodHasAsyncOverload
        feedStatusRequestListenerSubscription?.Unsubscribe();
        feedTickersListenerSubscription?.Unsubscribe();
        // ReSharper restore MethodHasAsyncOverload
        if (pqClientUpdateSubscriberRule != null) await Context.MessageBus.UndeployRuleAsync(this, pqClientUpdateSubscriberRule);
        if (pqClientSnapshotRequesterRule != null) await Context.MessageBus.UndeployRuleAsync(this, pqClientSnapshotRequesterRule);
    }

    private void ReceivedFeedAvailableTickersUpdate(IBusMessage<FeedSourceTickerInfoUpdate> sourceTickerQuoteInfosMessage)
    {
        PricingFeedStatus = PricingFeedStatus.AvailableForSubscription;
        var tickersUpdate = sourceTickerQuoteInfosMessage.Payload.Body();
        latestReceivedSourceTickerQuoteInfos = tickersUpdate!.SourceTickerQuoteInfos.ToList();
    }

    private async ValueTask<PricingFeedStatusResponse> ReceivedFeedStatusRequestHandler(
        IBusRespondingMessage<PricingFeedStatusRequest, PricingFeedStatusResponse> busRequestMessage)
    {
        PricingFeedStatusResponse? pricingFeedStatusResponse;
        if (pqClientUpdateSubscriberRule?.PqPricingClientFeedSyncMonitorRule.LifeCycleState == RuleLifeCycle.Started)
        {
            var tickerHealthResponse = await Context.MessageBus.RequestAsync<PricingFeedStatusRequest, PricingFeedStatusResponse?>(
                this, feedTickersHealthRequestAddress, busRequestMessage.Payload.Body(), new DispatchOptions(RoutingFlags.TargetSpecific,
                    targetRule: pqClientUpdateSubscriberRule.PqPricingClientFeedSyncMonitorRule, timeoutMs: 3_000));
            pricingFeedStatusResponse = tickerHealthResponse.Response;
            pricingFeedStatusResponse?.IncrementRefCount();
            pricingFeedStatusResponse ??= Context.PooledRecycler.Borrow<PricingFeedStatusResponse>();

            PricingFeedStatus = pricingFeedStatusResponse.HealthySubscribedTickers.Any() ? PricingFeedStatus.Ticking
                : pricingFeedStatusResponse.UnhealthySubscribedTickers.Any() ? PricingFeedStatus.NothingTicking
                : latestReceivedSourceTickerQuoteInfos.Any() ? PricingFeedStatus.AvailableForSubscription
                : PricingFeedStatus.Starting;
        }
        else
        {
            PricingFeedStatus = PricingFeedStatus.Starting;
            pricingFeedStatusResponse = Context.PooledRecycler.Borrow<PricingFeedStatusResponse>();
        }

        pricingFeedStatusResponse.FeedStatus = pricingFeedStatus;
        pricingFeedStatusResponse.AvailableTickersTickers = latestReceivedSourceTickerQuoteInfos;
        pricingFeedStatusResponse.StartTime = feedStartTime;

        return pricingFeedStatusResponse;
    }
}