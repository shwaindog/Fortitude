using System.Collections.Generic;
using FortitudeCommon.Types;

namespace FortitudeMarketsApi.Pricing.LayeredBook
{
    public interface IOrderBook : IEnumerable<IPriceVolumeLayer>, ICloneable<IOrderBook>,
                    IInterfacesComparable<IOrderBook>
    {
        int Capacity { get; }
        int Count { get; }
        IPriceVolumeLayer this[int level] { get; }
    }
}