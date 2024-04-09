#region

using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsApi.Pricing.Quotes.SourceTickerInfo;

#endregion

namespace FortitudeMarketsCore.Pricing.Quotes.LayeredBook.LayerSelector;

public interface ILayerFlagsSelector<T, Tu> where T : class where Tu : ISourceTickerQuoteInfo
{
    T FindForLayerFlags(Tu sourceTickerQuoteInfo);
    IPriceVolumeLayer? ConvertToExpectedImplementation(IPriceVolumeLayer? priceVolumeLayer, bool clone = false);
}
