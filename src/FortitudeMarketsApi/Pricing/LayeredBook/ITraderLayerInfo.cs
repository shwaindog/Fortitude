#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;

#endregion

namespace FortitudeMarketsApi.Pricing.LayeredBook;

public interface ITraderLayerInfo : IReusableObject<ITraderLayerInfo>, IInterfacesComparable<ITraderLayerInfo>
{
    string? TraderName { get; }
    decimal TraderVolume { get; }
    bool IsEmpty { get; }
}
