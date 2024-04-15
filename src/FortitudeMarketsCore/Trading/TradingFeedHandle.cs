#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Monitoring.Logging;
using FortitudeMarketsApi.Trading;
using FortitudeMarketsApi.Trading.Executions;
using FortitudeMarketsApi.Trading.Orders;
using FortitudeMarketsApi.Trading.Orders.Client;
using FortitudeMarketsApi.Trading.Orders.Products;
using FortitudeMarketsApi.Trading.Orders.Server;
using FortitudeMarketsApi.Trading.Orders.Venues;
using FortitudeMarketsCore.Trading.Orders;
using FortitudeMarketsCore.Trading.Orders.Server;
using FortitudeMarketsCore.Trading.ORX.Orders;
using FortitudeMarketsCore.Trading.ORX.Orders.Server;

#endregion

namespace FortitudeMarketsCore.Trading;

public class TradingFeedHandle : ITradingFeed
{
    // ReSharper disable once UnusedMember.Local
    private static IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(TradingFeedHandle));

    private IRecycler? recycler;
    public IOrder? LastReceivedOrder { get; private set; }

    public string? User { get; set; }

    public IRecycler Recycler
    {
        get => recycler ?? new Recycler();
        set => recycler = value;
    }

    public void Dispose()
    {
        Closed?.Invoke();
    }

    public void CancelOrder(IOrder order)
    {
        var updatedOrder = Recycler.Borrow<Order>().CopyFrom(order);
        updatedOrder.Status = OrderStatus.Dead;
        OrderUpdate?.Invoke(new OrderUpdate(updatedOrder, OrderUpdateEventType.OrderCancelled, TimeContext.UtcNow));
    }

    public void SuspendOrder(IOrder order)
    {
        var updatedOrder = Recycler.Borrow<Order>().CopyFrom(order);
        updatedOrder.Status = OrderStatus.Frozen;
        OrderUpdate?.Invoke(new OrderUpdate(updatedOrder, OrderUpdateEventType.OrderPaused, TimeContext.UtcNow));
    }

    public void ResumeOrder(IOrder order)
    {
        var updatedOrder = Recycler.Borrow<Order>().CopyFrom(order);
        updatedOrder.Status = OrderStatus.Active;
        OrderUpdate?.Invoke(new OrderUpdate(updatedOrder, OrderUpdateEventType.OrderResumed, TimeContext.UtcNow));
    }

    public bool IsAvailable { get; set; }
    public event Action<string, bool>? FeedStatusUpdate;
    public event Action<IOrderUpdate>? OrderUpdate;
    public event Action<IOrderAmendResponse>? OrderAmendResponse;
    public event Action<IExecutionUpdate>? Execution;
    public event Action<IVenueOrderUpdate>? VenueOrderUpdated;
    public event Action? Closed;

    public TimeInForce SupportedTimeInForce { get; set; }
    public VenueFeatures SupportedVenueFeatures { get; set; }

    public void SubmitOrderRequest(IOrderSubmitRequest submitRequest)
    {
        submitRequest.OrderDetails!.Status = OrderStatus.Active;
        var orderUpdate = Recycler.Borrow<OrderUpdate>();
        orderUpdate.Order = submitRequest.OrderDetails;
        orderUpdate.OrderUpdateType = OrderUpdateEventType.OrderAcknowledged;
        orderUpdate.AdapterUpdateTime = TimeContext.UtcNow;
        orderUpdate.ClientReceivedTime = DateTime.MinValue;
        OnOrderUpdate(orderUpdate);
        LastReceivedOrder = submitRequest.OrderDetails.Clone();
    }

    public void AmendOrderRequest(IOrder order, IOrderAmend amendOrderRequest)
    {
        order.Product!.ApplyAmendment(amendOrderRequest);
        var amendResponse = Recycler.Borrow<OrxOrderAmendResponse>();
        amendResponse.Order = (OrxOrder)order;
        amendResponse.OrderUpdateType = OrderUpdateEventType.OrderAmended;
        amendResponse.AdapterUpdateTime = TimeContext.UtcNow;
        amendResponse.AmendType = AmendType.Amended;
        amendResponse.OldOrderId = null;
        OnOrderAmendResponse(amendResponse);
        LastReceivedOrder = order.Clone();
    }

    public virtual void OnVenueOrder(IVenueOrderUpdate venueOrderUpdate)
    {
        VenueOrderUpdated?.Invoke(venueOrderUpdate);
    }

    public virtual void OnExecution(IExecutionUpdate executionUpdate)
    {
        Execution?.Invoke(executionUpdate);
    }

    public virtual void OnOrderAmendResponse(IOrderAmendResponse orderAmendResponse)
    {
        OrderAmendResponse?.Invoke(orderAmendResponse);
    }

    public virtual void OnOrderUpdate(IOrderUpdate orderUpdate)
    {
        OrderUpdate?.Invoke(orderUpdate);
    }

    public virtual void OnStatusUpdate(string arg1, bool arg2)
    {
        FeedStatusUpdate?.Invoke(arg1, arg2);
    }
}
