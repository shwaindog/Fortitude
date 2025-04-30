// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.BusMessaging.Routing.SelectionStrategies;
using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;
using FortitudeMarkets.Configuration.ClientServerConfig;
using FortitudeMarkets.Pricing.PQ.Subscription.BusRules.BusMessages;
using static FortitudeMarkets.Pricing.PQ.Subscription.BusRules.PricingClientSubscriptionConstants;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Subscription.BusRules;

public readonly struct PQClientUpdate(PricingFeedStatus pricingFeedStatus, string feedName)
{
    public PricingFeedStatus PricingFeedStatus { get; } = pricingFeedStatus;

    public string FeedName { get; } = feedName;
}

public readonly struct SourceFeedUpdate(PricingFeedStatus pricingFeedStatus, string feedName, string feedAddress)
{
    public PricingFeedStatus PricingFeedStatus { get; } = pricingFeedStatus;

    public string FeedAddress { get; } = feedAddress;
    public string FeedName    { get; } = feedName;
}

public class PQPricingClientRule : Rule
{
    private IMarketsConfig marketsConfig;

    private IPQPriceConnectionRuleFactory priceConnectionRuleFactory;

    public PQPricingClientRule
    (IMarketsConfig marketsConfig,
        IPQPriceConnectionRuleFactory priceConnectionRuleFactory) : base("PQClientRule")
    {
        this.marketsConfig = marketsConfig;

        this.priceConnectionRuleFactory = priceConnectionRuleFactory;
    }

    public override async ValueTask StartAsync()
    {
        this.RegisterListener<SourceFeedUpdate>(AllFeedStatusUpdates + "*", ReceivedFeedUpdate);
        foreach (var marketConnConfig in marketsConfig.Markets.Values)
            await this.DeployChildRuleAsync(new PQPricingClientFeedRule(marketConnConfig)
                                          , new DeploymentOptions(RoutingFlags.DefaultDeploy));
    }

    public void ReceivedFeedUpdate(IBusMessage<SourceFeedUpdate> busFeedUpdateMessage)
    {
        var sourceFeedUpdate = busFeedUpdateMessage.Payload.Body();
        var feedStatus       = sourceFeedUpdate.PricingFeedStatus;
        if (feedStatus is PricingFeedStatus.Starting or PricingFeedStatus.Stopping or PricingFeedStatus.NothingTicking)
        {
            var feedName = sourceFeedUpdate.FeedName;
            this.PublishAsync(AllFeedStatusUpdates, new PQClientUpdate(feedStatus, feedName)
                            , new DispatchOptions(RoutingFlags.DefaultPublish));
        }
    }
}

public interface IPQPriceConnectionRuleFactory { }
