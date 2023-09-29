using System.Collections.Generic;
using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsCore.Pricing.PQ.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.DictionaryCompression;

namespace FortitudeMarketsCore.Pricing.PQ.LayeredBook
{
    public interface IPQTraderPriceVolumeLayer : IMutableTraderPriceVolumeLayer, IPQPriceVolumeLayer,
        IEnumerable<IPQTraderLayerInfo>, IPQSupportsStringUpdates<IPriceVolumeLayer>
    {
        new IPQTraderLayerInfo this[int index] { get; set; }
        IPQNameIdLookupGenerator TraderNameIdLookup { get; set; }
        new IPQTraderPriceVolumeLayer Clone();
        new IEnumerator<IPQTraderLayerInfo> GetEnumerator();
    }
}