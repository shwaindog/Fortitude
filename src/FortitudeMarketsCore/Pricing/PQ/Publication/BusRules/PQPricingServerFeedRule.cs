// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.BusMessaging.Pipelines;
using FortitudeBusRules.BusMessaging.Pipelines.Groups;
using FortitudeBusRules.BusMessaging.Routing.SelectionStrategies;
using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;
using FortitudeMarketsApi.Configuration.ClientServerConfig;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Publication.BusRules.BusMessages;
using FortitudeMarketsCore.Pricing.PQ.Subscription.BusRules.BusMessages;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Publication.BusRules;

public class PQPricingServerFeedRule : Rule
{
    private readonly string allAvailableTickersRequestAddress;
    private readonly string feedName;

    private readonly IMarketConnectionConfig marketConnectionConfig;

    private readonly string publishTickerAddress;

    private List<ISourceTickerInfo>             allPricingConfiguredTickerInfos = null!;
    private PQPricingServerClientResponderRule? clientResponderRule;
    private PQPricingServerQuotePublisherRule?  updatePublisherRule;

    public PQPricingServerFeedRule(IMarketConnectionConfig marketConnectionConfig)
        : base(marketConnectionConfig.Name + "_" + nameof(PQPricingServerFeedRule))
    {
        this.marketConnectionConfig       = marketConnectionConfig;
        feedName                          = marketConnectionConfig.Name;
        publishTickerAddress              = feedName.FeedTickerPublishAddress();
        allAvailableTickersRequestAddress = feedName.FeedPricingConfiguredTickersRequestAddress();
    }

    public override async ValueTask StartAsync()
    {
        var outboundQueueDeploy = Context.GetEventQueues(MessageQueueType.NetworkOutbound)
                                         .SelectEventQueue(QueueSelectionStrategy.LeastBusy);
        updatePublisherRule = new PQPricingServerQuotePublisherRule(feedName, marketConnectionConfig.PricingServerConfig!);
        await this.DeployChildRuleAsync
            (updatePublisherRule, new DeploymentOptions(RoutingFlags.TargetSpecific, MessageQueueType.NetworkOutbound, 1, outboundQueueDeploy.Name));
        var inboundQueueDeploy = Context.GetEventQueues(MessageQueueType.NetworkInbound)
                                        .SelectEventQueue(QueueSelectionStrategy.EarliestCompleted);
        clientResponderRule = new PQPricingServerClientResponderRule(feedName, marketConnectionConfig.PricingServerConfig!);
        await this.DeployChildRuleAsync
            (clientResponderRule, new DeploymentOptions(RoutingFlags.TargetSpecific, MessageQueueType.NetworkInbound, 1, inboundQueueDeploy.Name));
        await RegisterEachConfiguredTicker();
        await this.RegisterRequestListenerAsync<string, FeedSourceTickerInfoUpdate>
            (allAvailableTickersRequestAddress, ReceivedAllAvailableTickersRequest);
    }

    private FeedSourceTickerInfoUpdate ReceivedAllAvailableTickersRequest(IBusRespondingMessage<string, FeedSourceTickerInfoUpdate> arg)
    {
        var response = Context.PooledRecycler.Borrow<FeedSourceTickerInfoUpdate>();
        response.SourceTickerInfos = allPricingConfiguredTickerInfos;

        response.FeedName = feedName;
        return response;
    }

    public async ValueTask RegisterEachConfiguredTicker()
    {
        allPricingConfiguredTickerInfos = marketConnectionConfig.PricingEnabledSourceTickerInfos.ToList();
        foreach (var sourceTickerInfo in allPricingConfiguredTickerInfos)
        {
            var emptyQuoteEvent = Context.PooledRecycler.Borrow<PublishQuoteEvent>();
            emptyQuoteEvent.PublishQuote = sourceTickerInfo.PublishedTypePQInstance();
            await Context.MessageBus.PublishAsync
                (this, publishTickerAddress, emptyQuoteEvent, new DispatchOptions(RoutingFlags.TargetSpecific, targetRule: updatePublisherRule));
        }
    }
}
