#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeMarkets.Trading.Orders;
using FortitudeMarkets.Trading.Orders.Client;
using FortitudeMarkets.Trading.Orders.Products;

#endregion

namespace FortitudeMarkets.Trading;

public interface ITradingFeed : ITradingFeedListener
{
    IRecycler Recycler { get; set; }
    bool IsAvailable { get; }
    TimeInForce SupportedTimeInForce { get; }
    VenueFeatures SupportedVenueFeatures { get; }
    void CancelOrder(IOrder order);
    void SuspendOrder(IOrder order);
    void ResumeOrder(IOrder order);
    void SubmitOrderRequest(IOrderSubmitRequest submitRequest);
    void AmendOrderRequest(IOrder order, IOrderAmend amendOrderRequest);
}
