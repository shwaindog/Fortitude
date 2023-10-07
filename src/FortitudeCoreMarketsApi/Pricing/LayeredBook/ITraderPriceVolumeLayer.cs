#region

using FortitudeCommon.Types;

#endregion

namespace FortitudeMarketsApi.Pricing.LayeredBook;

public interface ITraderPriceVolumeLayer : IPriceVolumeLayer, IEnumerable<ITraderLayerInfo>,
    ICloneable<ITraderPriceVolumeLayer>
{
    int Count { get; }
    bool IsTraderCountOnly { get; }
    ITraderLayerInfo? this[int i] { get; }
    new ITraderPriceVolumeLayer Clone();
}
