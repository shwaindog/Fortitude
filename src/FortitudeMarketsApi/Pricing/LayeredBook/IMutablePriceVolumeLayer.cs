using FortitudeCommon.Types;

namespace FortitudeMarketsApi.Pricing.LayeredBook
{
    public interface IMutablePriceVolumeLayer : IPriceVolumeLayer, IStoreState<IPriceVolumeLayer>
    {
        new decimal Price { get; set; }
        new decimal Volume { get; set; }
        void Reset();
    }
}