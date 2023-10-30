using FortitudeCommon.Types;

namespace FortitudeMarketsApi.Pricing.LayeredBook
{
    // The reason for so many sub-types of IPriceVolume Layer is to reduce Quote memory waste when
    // duplicated 1000s of times in a ring.
    public interface IPriceVolumeLayer: ICloneable<IPriceVolumeLayer>, IInterfacesComparable<IPriceVolumeLayer>
    {
        decimal Price { get; }
        decimal Volume { get; }
        bool IsEmpty { get; }
    }
}