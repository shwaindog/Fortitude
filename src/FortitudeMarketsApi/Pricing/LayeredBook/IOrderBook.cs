#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;

#endregion

namespace FortitudeMarketsApi.Pricing.LayeredBook;

public enum BookSide
{
    Unknown
    , BidBook
    , AskBook
}

public interface IOrderBook : IEnumerable<IPriceVolumeLayer>, IReusableObject<IOrderBook>,
    IInterfacesComparable<IOrderBook>
{
    LayerType LayersOfType { get; }
    LayerFlags LayersSupportsLayerFlags { get; }
    int Capacity { get; }
    int Count { get; }
    BookSide BookSide { get; }
    IPriceVolumeLayer? this[int level] { get; }
}
