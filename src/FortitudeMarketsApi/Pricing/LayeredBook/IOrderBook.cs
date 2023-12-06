#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;

#endregion

namespace FortitudeMarketsApi.Pricing.LayeredBook;

public interface IOrderBook : IEnumerable<IPriceVolumeLayer>, IReusableObject<IOrderBook>,
    IInterfacesComparable<IOrderBook>
{
    int Capacity { get; }
    int Count { get; }
    IPriceVolumeLayer? this[int level] { get; }
}
