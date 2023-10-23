using FortitudeMarketsApi.Pricing.LayeredBook;

namespace FortitudeMarketsCore.Pricing.PQ.LayeredBook
{
    public interface IPQValueDatePriceVolumeLayer : IMutableValueDatePriceVolumeLayer, IPQPriceVolumeLayer
    {
        bool IsValueDateUpdated { get; set; }
        new IPQValueDatePriceVolumeLayer Clone();
    }
}