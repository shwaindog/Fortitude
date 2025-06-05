using FortitudeMarkets.Pricing.FeedEvents.MarketEvents;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;

namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.MarketEvents;


public interface IPQPublishedMarketEvents : IMutablePublishedMarketEvents, IPQSupportsNumberPrecisionFieldUpdates, IPQSupportsStringUpdates
{

}

public class PQPublishedMarketEvents
{
}