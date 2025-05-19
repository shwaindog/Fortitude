using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FortitudeMarkets.Pricing.FeedEvents.TradingConversions;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;

namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.TradingConversions;

public interface IPQPnLConversions : IMutablePnLConversions, IPQSupportsNumberPrecisionFieldUpdates<IPnLConversions>, IPQSupportsStringUpdates<IPnLConversions>
{

}
public class PQPnLConversions
{
}