using FortitudeCommon.Types;

namespace FortitudeMarketsApi.Pricing.LayeredBook
{
    public interface IMutableSourceQuoteRefTraderValueDatePriceVolumeLayer : ISourceQuoteRefTraderValueDatePriceVolumeLayer,
        IMutableSourceQuoteRefPriceVolumeLayer, IMutableValueDatePriceVolumeLayer, IMutableTraderPriceVolumeLayer,  
        ICloneable<IMutableSourceQuoteRefTraderValueDatePriceVolumeLayer>
    {
        new IMutableSourceQuoteRefTraderValueDatePriceVolumeLayer Clone();
    }
}