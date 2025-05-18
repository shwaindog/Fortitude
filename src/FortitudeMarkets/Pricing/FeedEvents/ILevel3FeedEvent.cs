using FortitudeMarkets.Pricing.FeedEvents.Quotes;

namespace FortitudeMarkets.Pricing.FeedEvents;


public interface ILevel3FeedEvent : IPricedQuoteFeedEventUpdate<ILevel3Quote>, ILevel2FeedEvent
{
    new ILevel2Quote Quote { get; set; }

    new ILevel2Quote? ContinuousPriceAdjustedQuote { get; }
}

public interface IMutableLevel3Event : ILevel3FeedEvent, IMutableLevel2FeedEvent
{
    new IMutableLevel2Quote Quote { get; set; }

    new IMutableLevel2Quote? ContinuousPriceAdjustedQuote { get; }
}