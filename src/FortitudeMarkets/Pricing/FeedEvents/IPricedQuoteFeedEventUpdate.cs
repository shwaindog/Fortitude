using FortitudeMarkets.Pricing.FeedEvents.Quotes;

namespace FortitudeMarkets.Pricing.FeedEvents;

public interface IPricedQuoteFeedEventUpdate<out T> where T : ILevel1Quote
{
    T  Quote                        { get; }
    T? ContinuousPriceAdjustedQuote { get; }
}
