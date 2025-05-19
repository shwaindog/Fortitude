using FortitudeMarkets.Pricing.FeedEvents.LastTraded;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;

namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.LastTraded;

public interface IPQRecentlyTradedHistory : IMutableRecentlyTradedHistory, IPQSupportsNumberPrecisionFieldUpdates<IRecentlyTradedHistory>
  , IPQSupportsStringUpdates<IRecentlyTradedHistory>
{

}


public class PQRecentlyTradedHistory
{
}