#region

using FortitudeMarketsApi.Pricing.LayeredBook;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.LayeredBook;

public interface IPQValueDatePriceVolumeLayer : IMutableValueDatePriceVolumeLayer, IPQPriceVolumeLayer
{
    bool IsValueDateUpdated { get; set; }
    new IPQValueDatePriceVolumeLayer Clone();
}
