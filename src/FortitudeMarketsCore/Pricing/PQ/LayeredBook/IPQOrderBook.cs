using System.Collections.Generic;
using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.Types;
using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsCore.Pricing.PQ.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.Quotes.SourceTickerInfo;

namespace FortitudeMarketsCore.Pricing.PQ.LayeredBook
{
    public interface IPQOrderBook : IMutableOrderBook, IPQSupportsFieldUpdates<IOrderBook>, 
        IPQSupportsStringUpdates<IOrderBook>, IEnumerable<IPQPriceVolumeLayer>, ICloneable<IPQOrderBook>,
        IRelatedItem<IPQSourceTickerQuoteInfo>, IRelatedItem<IPQPriceVolumeLayer>

    {
        new IPQPriceVolumeLayer this[int level] { get; set; }
        IList<IPQPriceVolumeLayer> AllLayers { get; set; }
        new bool HasUpdates { get; set; }
        new IPQOrderBook Clone();
        new IEnumerator<IPQPriceVolumeLayer> GetEnumerator();
    }
}
