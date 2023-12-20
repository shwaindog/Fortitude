#region

using FortitudeMarketsApi.Pricing.Quotes;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Subscription;

public interface IPQTickerFeedSubscriptionQuoteStream<out T> : IPQTickerFeedSubscription, IObservable<T>
    where T : class, ILevel0Quote { }
