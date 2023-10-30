#region

using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsCore.Pricing.LayeredBook.LayerSelector;
using FortitudeMarketsCore.Pricing.PQ.Quotes.SourceTickerInfo;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.LayeredBook.LayerSelector;

public interface
    IPQOrderBookLayerFactorySelector : ILayerFlagsSelector<IOrderBookLayerFactory, IPQSourceTickerQuoteInfo>
{
    bool TypeCanWholeyContain(Type copySourceType, Type copyDestinationType);
    IPQPriceVolumeLayer? SelectPriceVolumeLayer(IPQPriceVolumeLayer? original, IPriceVolumeLayer? desired);
}
