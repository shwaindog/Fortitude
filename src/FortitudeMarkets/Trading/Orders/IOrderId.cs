#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;

#endregion

namespace FortitudeMarkets.Trading.Orders;

public interface IOrderId : IReusableObject<IOrderId>
{
    long ClientOrderId { get; set; }
    IMutableString? VenueClientOrderId { get; set; }
    IMutableString? TrackingId { get; set; }
    long AdapterOrderId { get; set; }
    IMutableString? VenueAdapterOrderId { get; set; }
    IOrderId? ParentOrderId { get; set; }
}
