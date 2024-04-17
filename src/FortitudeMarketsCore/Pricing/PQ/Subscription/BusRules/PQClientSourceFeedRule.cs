#region

using FortitudeBusRules.BusMessaging.Routing.SelectionStrategies;
using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;
using FortitudeMarketsApi.Configuration.ClientServerConfig;

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
    private readonly string defaultAllTickerSubAddress;

    private readonly string feedAddress;
    private readonly string feedName;
    private IMarketConnectionConfig marketConnectionConfig;
    private SourceFeedStatus snapshotFeedStatus;

    private SourceFeedStatus updateFeedStatus;


    public PQClientSourceFeedRule(IMarketConnectionConfig marketConnectionConfig) : base("PQClientSourceFeedRule" + marketConnectionConfig.Name)
    {
        this.marketConnectionConfig = marketConnectionConfig;
        feedName = marketConnectionConfig.Name;
        feedAddress = feedName.FeedAddress();
        defaultAllTickerSubAddress = feedName.DefaultAllTickersPublishAddress();
    }

    public override ValueTask StartAsync()
    {
        Context.MessageBus.RegisterRequestListener<SourceTickerSubscriptionRequest, SourceTickerSubscriptionResponse>(this, defaultAllTickerSubAddress
            , ReceivedTickerSubscriptionRequest);
        Context.MessageBus.PublishAsync(this, feedAddress, new SourceFeedUpdate(SourceFeedStatus.Starting, feedName, feedAddress)
            , new DispatchOptions(RoutingFlags.DefaultPublish));
        return base.StartAsync();
    }

    public SourceTickerSubscriptionResponse ReceivedTickerSubscriptionRequest(
        IBusRespondingMessage<SourceTickerSubscriptionRequest, SourceTickerSubscriptionResponse> busRequestMessage) =>
        new();
}
