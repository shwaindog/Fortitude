using FortitudeMarkets.Pricing.FeedEvents.Limits;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;

namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Limits;

public interface IPQPublishedLimits : IMutablePublishedLimits, IPQSupportsNumberPrecisionFieldUpdates, IPQSupportsStringUpdates
{

}


public class PQPublishedLimits
{
}