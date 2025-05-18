using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FortitudeMarkets.Pricing.FeedEvents.InternalOrders;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;

namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.InternalOrders;

public interface IPQPublishedInternalOrders : IMutablePublishedInternalOrders, IPQSupportsNumberPrecisionFieldUpdates<IPublishedInternalOrders>, IPQSupportsStringUpdates<IPublishedInternalOrders>
{

}

public class PQPublishedInternalOrders
{
}