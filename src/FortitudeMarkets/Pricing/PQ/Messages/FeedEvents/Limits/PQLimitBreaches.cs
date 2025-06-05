using FortitudeMarkets.Pricing.FeedEvents.Limits;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;

namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Limits;

public interface IPQLimitBreaches : IMutableLimitBreaches, IPQSupportsNumberPrecisionFieldUpdates, IPQSupportsStringUpdates
{

}

public class PQLimitBreaches
{
}