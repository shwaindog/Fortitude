using FortitudeMarketsApi.Trading.Orders;
using FortitudeMarketsApi.Trading.Orders.Client;
using FortitudeMarketsApi.Trading.Orders.Products;

namespace FortitudeMarketsApi.Trading
{
    public interface ITradingFeed : ITradingFeedListener
    {
        void CancelOrder(IOrder order);
        void SuspendOrder(IOrder order);
        void ResumeOrder(IOrder order);
        bool IsAvailable { get; }
        TimeInForce SupportedTimeInForce { get; }
        VenueFeatures SupportedVenueFeatures { get; }
        void SubmitOrderRequest(IOrderSubmitRequest submitRequest);
        void AmendOrderRequest(IOrder order, IOrderAmend amendOrderRequest);
    }
}
