using FortitudeMarkets.Pricing.FeedEvents.MarketEvents;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;

namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.MarketEvents;


public interface IPQMarketNewsPanel : IMutableMarketNewsPanel, IPQSupportsNumberPrecisionFieldUpdates, IPQSupportsStringUpdates
{

}

public class PQMarketNewsPanel
{
}