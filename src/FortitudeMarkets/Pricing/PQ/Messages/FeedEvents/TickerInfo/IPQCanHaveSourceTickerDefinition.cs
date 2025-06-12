using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;

namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.TickerInfo;

public interface IPQCanHaveSourceTickerDefinition : IMutableCanHaveSourceTickerDefinition
{
    new IPQSourceTickerInfo? SourceTickerInfo { get; set; }
}