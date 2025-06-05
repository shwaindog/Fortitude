using FortitudeMarkets.Pricing.FeedEvents.TradingConversions;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;

namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.TradingConversions;

public interface IPQPnLConversions : IMutablePnLConversions, IPQSupportsNumberPrecisionFieldUpdates, IPQSupportsStringUpdates
{

}
public class PQPnLConversions
{
}