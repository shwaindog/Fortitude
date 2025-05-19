using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FortitudeMarkets.Pricing.FeedEvents.Limits;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;

namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Limits;

public interface IPQPublishedLimits : IMutablePublishedLimits, IPQSupportsNumberPrecisionFieldUpdates<IPublishedLimits>, IPQSupportsStringUpdates<IPublishedLimits>
{

}


public class PQPublishedLimits
{
}