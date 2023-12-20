#region

using FortitudeCommon.Types;

#endregion

namespace FortitudeMarketsApi.Pricing.LayeredBook;

public interface IValueDatePriceVolumeLayer : IPriceVolumeLayer, ICloneable<IValueDatePriceVolumeLayer>
{
    DateTime ValueDate { get; }
    new IValueDatePriceVolumeLayer Clone();
}
