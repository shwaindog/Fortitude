﻿#region

using FortitudeBusRules.MessageBus.Routing.SelectionStrategies;
using FortitudeBusRules.Messaging;
using FortitudeBusRules.Rules;
using FortitudeMarketsApi.Configuration.ClientServerConfig;
using static FortitudeMarketsCore.Pricing.PQ.Subscription.BusRules.PricingSubscriptionConstants;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Subscription.BusRules;

public enum SourceFeedStatus
{
    NotStarted
    , Starting
    , SnapshotConnectionFailed
    , UpdatesStopped
    , Connected
    , Connecting
    , Streaming
    , Disconnected
    , RuleStopping
}

public enum SourceTickerStatus
{
    NotStarted
    , Healthy
    , Stale
    , OutOfSync
    , SnapshotRequested
    , UpdatesStopped
    , MarketClosed
}

public readonly struct PQClientUpdate(SourceFeedStatus sourceFeedStatus, string feedName)
{
    public SourceFeedStatus SourceFeedStatus { get; } = sourceFeedStatus;
    public string FeedName { get; } = feedName;
}

public readonly struct SourceFeedUpdate(SourceFeedStatus sourceFeedStatus, string feedName, string feedAddress)
{
    public SourceFeedStatus SourceFeedStatus { get; } = sourceFeedStatus;
    public string FeedAddress { get; } = feedAddress;
    public string FeedName { get; } = feedName;
}

public class PQClientRule : Rule
{
    public const string PQClientUpdateAddress = $"{PricingSubscriptionBasePath}.ClientUpdate";

    private IMarketConnectionConfigRepository marketConnectionConfigRepository;
    private IPQPriceConnectionRuleFactory priceConnectionRuleFactory;

    public PQClientRule(IMarketConnectionConfigRepository marketConnectionConfigRepository,
        IPQPriceConnectionRuleFactory priceConnectionRuleFactory) : base("PQClientRule")
    {
        this.marketConnectionConfigRepository = marketConnectionConfigRepository;
        this.priceConnectionRuleFactory = priceConnectionRuleFactory;
    }

    public override async ValueTask StartAsync()
    {
        Context.EventBus.RegisterListener<SourceFeedUpdate>(this, PQClientSourceFeedRule.SourceFeedUpdatesAddress + "*", ReceivedFeedUpdate);
        foreach (var marketConnConfig in marketConnectionConfigRepository.AllMarketConnectionConfigs)
            await Context.EventBus.DeployRuleAsync(this, new PQClientSourceFeedRule(marketConnConfig)
                , new DeploymentOptions(RoutingFlags.DefaultDeploy));
    }

    public void ReceivedFeedUpdate(IMessage<SourceFeedUpdate> feedUpdateMessage)
    {
        var feedStatus = feedUpdateMessage.PayLoad.Body.SourceFeedStatus;
        if (feedStatus is SourceFeedStatus.Connected or SourceFeedStatus.Disconnected or SourceFeedStatus.Streaming)
        {
            var feedName = feedUpdateMessage.PayLoad.Body.FeedName;
            Context.EventBus.PublishAsync(this, PQClientUpdateAddress, new PQClientUpdate(feedStatus, feedName)
                , new DispatchOptions(RoutingFlags.DefaultPublish));
        }
    }
}

public interface IPQPriceConnectionRuleFactory { }