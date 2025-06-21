#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;

#endregion

namespace FortitudeMarkets.Trading.Orders;

public interface IOrderId : IReusableObject<IOrderId>, IInterfacesComparable<IOrderId>
{
    uint ClientOrderId { get; set; }

    uint? TrackingId { get; set; }

    uint? AdapterOrderId { get; set; }

    long? OrderBookingId { get; set; }

    IOrderId? ParentOrderId { get; set; }
}
