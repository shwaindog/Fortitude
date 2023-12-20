#region

using FortitudeCommon.Types;

#endregion

namespace FortitudeMarketsApi.Pricing.LayeredBook;

public interface ISourceQuoteRefTraderValueDatePriceVolumeLayer : ISourceQuoteRefPriceVolumeLayer,
    IValueDatePriceVolumeLayer, ITraderPriceVolumeLayer, ICloneable<ISourceQuoteRefTraderValueDatePriceVolumeLayer>
{
    new ISourceQuoteRefTraderValueDatePriceVolumeLayer Clone();
}
