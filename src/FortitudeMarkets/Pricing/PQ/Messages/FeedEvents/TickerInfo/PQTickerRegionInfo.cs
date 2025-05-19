using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;

namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.TickerInfo;

public interface IPQTickerRegionInfo : IMutableTickerRegionInfo, IPQSupportsNumberPrecisionFieldUpdates<ITickerRegionInfo>, IPQSupportsStringUpdates<ITickerRegionInfo>
{

}
public class PQTickerRegionInfo
{
}