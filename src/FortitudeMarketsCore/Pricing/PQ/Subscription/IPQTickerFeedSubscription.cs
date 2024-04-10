#region

using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.Quotes;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Subscription;

public interface IPQTickerFeedSubscription
{
    string Source { get; }
    string Ticker { get; }
    IPricingServerConfig Feed { get; }
    void Unsubscribe();
}

public interface IPQTickerFeedSubscriptionQuoteStream<out T> : IPQTickerFeedSubscription, IObservable<T>
    where T : class, ILevel0Quote { }
