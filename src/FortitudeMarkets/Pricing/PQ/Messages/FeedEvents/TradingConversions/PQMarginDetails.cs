using FortitudeMarkets.Pricing.FeedEvents.TradingConversions;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;

namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.TradingConversions;

public interface IPQMarginDetails : IMutableMarginDetails, IPQSupportsNumberPrecisionFieldUpdates, IPQSupportsStringUpdates
{

}
public class PQMarginDetails
{
}