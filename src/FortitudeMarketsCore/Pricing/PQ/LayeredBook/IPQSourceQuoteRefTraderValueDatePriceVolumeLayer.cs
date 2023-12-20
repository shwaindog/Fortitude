#region

using FortitudeMarketsApi.Pricing.LayeredBook;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.LayeredBook;

public interface IPQSourceQuoteRefTraderValueDatePriceVolumeLayer : IPQTraderPriceVolumeLayer,
    IPQValueDatePriceVolumeLayer, IPQSourceQuoteRefPriceVolumeLayer,
    IMutableSourceQuoteRefTraderValueDatePriceVolumeLayer
{
    new IPQSourceQuoteRefTraderValueDatePriceVolumeLayer Clone();
}
