#region

using FortitudeBusRules.BusMessaging.Routing.SelectionStrategies;
using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;
using FortitudeMarketsApi.Configuration.ClientServerConfig;
using FortitudeMarketsCore.Pricing.PQ.Subscription.BusRules.BusMessages;
using static FortitudeMarketsCore.Pricing.PQ.Subscription.BusRules.PricingClientSubscriptionConstants;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Subscription.BusRules;

public readonly struct PQClientUpdate(PricingFeedStatus pricingFeedStatus, string feedName)
{
    public PricingFeedStatus PricingFeedStatus { get; } = pricingFeedStatus;
    public string FeedName { get; } = feedName;
}

public readonly struct SourceFeedUpdate(PricingFeedStatus pricingFeedStatus, string feedName, string feedAddress)
{
    public PricingFeedStatus PricingFeedStatus { get; } = pricingFeedStatus;
    public string FeedAddress { get; } = feedAddress;
    public string FeedName { get; } = feedName;
}

public class PQPricingClientRule : Rule
{
    private IMarketsConfig marketsConfig;
    private IPQPriceConnectionRuleFactory priceConnectionRuleFactory;

    public PQPricingClientRule(IMarketsConfig marketsConfig,
        IPQPriceConnectionRuleFactory priceConnectionRuleFactory) : base("PQClientRule")
    {
        this.marketsConfig = marketsConfig;
        this.priceConnectionRuleFactory = priceConnectionRuleFactory;
    }

    public override async ValueTask StartAsync()
    {
        Context.MessageBus.RegisterListener<SourceFeedUpdate>(this, AllFeedStatusUpdates + "*", ReceivedFeedUpdate);
        foreach (var marketConnConfig in marketsConfig.Markets)
            await Context.MessageBus.DeployRuleAsync(this, new PQPricingClientFeedRule(marketConnConfig)
                , new DeploymentOptions(RoutingFlags.DefaultDeploy));
    }

    public void ReceivedFeedUpdate(IBusMessage<SourceFeedUpdate> busFeedUpdateMessage)
    {
        var sourceFeedUpdate = busFeedUpdateMessage.Payload.Body();
        var feedStatus = sourceFeedUpdate.PricingFeedStatus;
        if (feedStatus is PricingFeedStatus.Starting or PricingFeedStatus.Stopping or PricingFeedStatus.NothingTicking)
        {
            var feedName = sourceFeedUpdate.FeedName;
            Context.MessageBus.PublishAsync(this, AllFeedStatusUpdates, new PQClientUpdate(feedStatus, feedName)
                , new DispatchOptions(RoutingFlags.DefaultPublish));
        }
    }
}

public interface IPQPriceConnectionRuleFactory { }
