#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeMarkets.Trading.Orders;
using FortitudeMarkets.Trading.Orders.Client;
using FortitudeMarkets.Trading.Orders.Products;
using FortitudeMarkets.Trading.Orders.SpotOrders;

#endregion

namespace FortitudeMarkets.Trading;

public interface ITradingFeed : ITradingFeedListener
{
    IRecycler Recycler { get; set; }
    bool IsAvailable { get; }
    TimeInForce SupportedTimeInForce { get; }
    VenueFeatures SupportedVenueFeatures { get; }
    void CancelOrder(ITransmittableOrder order);
    void SuspendOrder(ITransmittableOrder order);
    void ResumeOrder(ITransmittableOrder order);
    void SubmitOrderRequest(IOrderSubmitRequest submitRequest);
    void AmendOrderRequest(ITransmittableOrder order, IOrderAmend amendOrderRequest);
}
