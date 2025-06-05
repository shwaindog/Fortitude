using FortitudeMarkets.Pricing.FeedEvents.AdapterExecutionDetails;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;

namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.AdapterExecutionDetails;

public interface IPQAdapterExecutionStatistics : IMutableAdapterExecutionStatistics, IPQSupportsNumberPrecisionFieldUpdates, IPQSupportsStringUpdates
{

}
public class PQAdapterExecutionStatistics
{
}