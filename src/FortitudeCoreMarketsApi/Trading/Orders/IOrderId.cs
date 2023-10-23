#region

using FortitudeCommon.Types.Mutable;

#endregion

namespace FortitudeMarketsApi.Trading.Orders;

public interface IOrderId
{
    long ClientOrderId { get; set; }
    IMutableString? VenueClientOrderId { get; set; }
    IMutableString? TrackingId { get; set; }
    long AdapterOrderId { get; set; }
    IMutableString? VenueAdapterOrderId { get; set; }
    IOrderId? ParentOrderId { get; set; }
    IOrderId Clone();
}
