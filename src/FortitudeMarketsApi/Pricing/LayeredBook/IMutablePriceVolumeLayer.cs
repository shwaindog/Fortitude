#region

using FortitudeCommon.Types;

#endregion

namespace FortitudeMarketsApi.Pricing.LayeredBook;

public interface IMutablePriceVolumeLayer : IPriceVolumeLayer, IStoreState<IPriceVolumeLayer>
{
    new decimal Price { get; set; }
    new decimal Volume { get; set; }
}
