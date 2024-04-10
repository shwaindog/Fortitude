#region

using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Subscription;

public abstract class PQTickerFeedSubscription : IPQTickerFeedSubscription
{
    private readonly ISourceTickerQuoteInfo sourceTickerQuoteInfo;

    protected PQTickerFeedSubscription(IPricingServerConfig feedServerConfig,
        ISourceTickerQuoteInfo sourceTickerQuoteInfo)
    {
        Feed = feedServerConfig;
        this.sourceTickerQuoteInfo = sourceTickerQuoteInfo;
    }

    public string Source => sourceTickerQuoteInfo.Source;
    public string Ticker => sourceTickerQuoteInfo.Ticker;
    public IPricingServerConfig Feed { get; }

    public abstract void Unsubscribe();

    public override int GetHashCode()
    {
        var hash = 13;
        hash = hash * 7 + Source.GetHashCode();
        hash = hash * 7 + Ticker.GetHashCode();
        return hash;
    }

    public override bool Equals(object? obj) => obj is PQTickerFeedSubscription sub && Source == sub.Source && Ticker == sub.Ticker;

    public override string ToString() => $"PQTickerFeedSubscription {Source}-{Ticker}";
}
