#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;

#endregion

namespace FortitudeMarketsApi.Pricing.LayeredBook;

// The reason for so many sub-types of IPriceVolume Layer is to reduce Quote memory waste when
// duplicated 1000s of times in a ring.
public interface IPriceVolumeLayer : IReusableObject<IPriceVolumeLayer>, IInterfacesComparable<IPriceVolumeLayer>
{
    LayerType LayerType { get; }
    LayerFlags SupportsLayerFlags { get; }
    decimal Price { get; }
    decimal Volume { get; }
    bool IsEmpty { get; }
}
