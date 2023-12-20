#region

using FortitudeCommon.Types;

#endregion

namespace FortitudeMarketsApi.Pricing.LayeredBook;

public interface IMutableSourceQuoteRefTraderValueDatePriceVolumeLayer : ISourceQuoteRefTraderValueDatePriceVolumeLayer,
    IMutableSourceQuoteRefPriceVolumeLayer, IMutableValueDatePriceVolumeLayer, IMutableTraderPriceVolumeLayer,
    ICloneable<IMutableSourceQuoteRefTraderValueDatePriceVolumeLayer>
{
    new IMutableSourceQuoteRefTraderValueDatePriceVolumeLayer Clone();
}
