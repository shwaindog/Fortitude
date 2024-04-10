#region

using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.LayeredBook;

#endregion

namespace FortitudeMarketsCore.Pricing.Quotes.LayeredBook.LayerSelector;

public interface ILayerFlagsSelector<T, Tu> where T : class where Tu : ISourceTickerQuoteInfo
{
    T FindForLayerFlags(Tu sourceTickerQuoteInfo);
    IPriceVolumeLayer? ConvertToExpectedImplementation(IPriceVolumeLayer? priceVolumeLayer, bool clone = false);
}
