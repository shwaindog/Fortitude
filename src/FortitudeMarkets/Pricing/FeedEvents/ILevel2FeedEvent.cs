using FortitudeMarkets.Pricing.FeedEvents.Quotes;

namespace FortitudeMarkets.Pricing.FeedEvents;


public interface ILevel2FeedEvent : IPricedFeedEventUpdate<ILevel2Quote>, ILevel1FeedEvent
{
    new ILevel2Quote Quote { get; set; }

    new ILevel2Quote? ContinuousPriceAdjustedQuote { get; }
}

public interface IMutableLevel2FeedEvent : ILevel2FeedEvent, IMutableLevel1FeedEvent
{
    new IMutableLevel2Quote Quote { get; set; }

    new IMutableLevel2Quote? ContinuousPriceAdjustedQuote { get; }
}