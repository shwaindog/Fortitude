using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.Types;
using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsApi.Pricing.Quotes.SourceTickerInfo;
using FortitudeMarketsCore.Pricing.PQ.DeltaUpdates;

namespace FortitudeMarketsCore.Pricing.PQ.LayeredBook
{
    public interface IPQPriceVolumeLayer : IMutablePriceVolumeLayer, IPQSupportsFieldUpdates<IPriceVolumeLayer>,
        IRelatedItem<ISourceTickerQuoteInfo>, IRelatedItem<IPQPriceVolumeLayer>
    {
        bool IsPriceUpdated { get; set; }
        bool IsVolumeUpdated { get; set; }
        new IPQPriceVolumeLayer Clone();
    }
}