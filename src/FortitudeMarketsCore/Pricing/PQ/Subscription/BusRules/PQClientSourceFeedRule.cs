#region

using FortitudeBusRules.MessageBus.Routing.SelectionStrategies;
using FortitudeBusRules.Messaging;
using FortitudeBusRules.Rules;
using FortitudeMarketsApi.Configuration.ClientServerConfig;
using static FortitudeMarketsCore.Pricing.PQ.Subscription.BusRules.PricingSubscriptionConstants;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Subscription.BusRules;

public struct SourceTickerSubscriptionRequest
{
    public string Ticker { get; set; }
}

public struct SourceTickerSubscriptionResponse
{
    public bool Succeeded { get; }

    public string Message { get; }
}

public class PQClientSourceFeedRule : Rule
{
    public const string SourceFeedUpdatesAddress = $"{PricingSubscriptionBasePath}.Feed.";
    public const string TickerSubAddressPostFix = $".Ticker.*";

    private readonly string feedAddress;
    private readonly string feedName;
    private readonly string tickerSubAddress;
    private IMarketConnectionConfig marketConnectionConfig;
    private SourceFeedStatus snapshotFeedStatus;

    private SourceFeedStatus updateFeedStatus;


    public PQClientSourceFeedRule(IMarketConnectionConfig marketConnectionConfig) : base("PQClientSourceFeedRule" + marketConnectionConfig.Name)
    {
        this.marketConnectionConfig = marketConnectionConfig;
        feedName = marketConnectionConfig.Name;
        feedAddress = SourceFeedUpdatesAddress + feedName;
        tickerSubAddress = feedAddress + TickerSubAddressPostFix;
    }

    public override ValueTask StartAsync()
    {
        Context.EventBus.RegisterRequestListener<SourceTickerSubscriptionRequest, SourceTickerSubscriptionResponse>(this, tickerSubAddress
            , ReceivedTickerSubscriptionRequest);
        Context.EventBus.PublishAsync(this, feedAddress, new SourceFeedUpdate(SourceFeedStatus.Starting, feedName, feedAddress)
            , new DispatchOptions(RoutingFlags.DefaultPublish));
        return base.StartAsync();
    }

    public SourceTickerSubscriptionResponse ReceivedTickerSubscriptionRequest(
        IRespondingMessage<SourceTickerSubscriptionRequest, SourceTickerSubscriptionResponse> messageRequest) =>
        new();
}
