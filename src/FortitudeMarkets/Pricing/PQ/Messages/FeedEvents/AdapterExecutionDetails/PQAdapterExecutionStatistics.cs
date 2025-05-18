using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FortitudeMarkets.Pricing.FeedEvents.AdapterExecutionDetails;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;

namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.AdapterExecutionDetails;

public interface IPQAdapterExecutionStatistics : IMutableAdapterExecutionStatistics, IPQSupportsNumberPrecisionFieldUpdates<IAdapterExecutionStatistics>, IPQSupportsStringUpdates<IAdapterExecutionStatistics>
{

}
public class PQAdapterExecutionStatistics
{
}