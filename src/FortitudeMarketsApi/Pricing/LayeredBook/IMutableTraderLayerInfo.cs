#region

using FortitudeCommon.Types;

#endregion

namespace FortitudeMarketsApi.Pricing.LayeredBook;

public interface IMutableTraderLayerInfo : ITraderLayerInfo, IStoreState<ITraderLayerInfo>
{
    new string? TraderName { get; set; }
    new decimal TraderVolume { get; set; }
    void Reset();
    new IMutableTraderLayerInfo Clone();
}
