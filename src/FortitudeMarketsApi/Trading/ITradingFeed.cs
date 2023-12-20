#region

using FortitudeMarketsApi.Trading.Orders;
using FortitudeMarketsApi.Trading.Orders.Client;
using FortitudeMarketsApi.Trading.Orders.Products;

#endregion

namespace FortitudeMarketsApi.Trading;

public interface ITradingFeed : ITradingFeedListener
{
    bool IsAvailable { get; }
    TimeInForce SupportedTimeInForce { get; }
    VenueFeatures SupportedVenueFeatures { get; }
    void CancelOrder(IOrder order);
    void SuspendOrder(IOrder order);
    void ResumeOrder(IOrder order);
    void SubmitOrderRequest(IOrderSubmitRequest submitRequest);
    void AmendOrderRequest(IOrder order, IOrderAmend amendOrderRequest);
}
