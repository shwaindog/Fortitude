#region

using System.Text;
using FortitudeMarketsApi.Trading.Executions;
using FortitudeMarketsApi.Trading.Orders;
using FortitudeMarketsApi.Trading.Orders.Client;
using FortitudeMarketsApi.Trading.Orders.Products.General;
using FortitudeMarketsApi.Trading.Orders.Venues;

#endregion

namespace FortitudeMarketsApi.Monitoring.Logging;

public class NewOrderLog
{
    private readonly decimal displaySize;
    private readonly ISpotOrder order;
    private readonly decimal price;
    private readonly decimal size;

    public NewOrderLog(ISpotOrder order)
    {
        this.order = order;
        displaySize = order.DisplaySize;
        price = order.Price;
        size = order.Size;
    }

    public override string ToString()
    {
        var sb = new StringBuilder(256)
            .Append("New order: ")
            .Append(order.Order.VenueSelectionCriteria).Append(';')
            .Append(order.Order.OrderId.ClientOrderId).Append(';')
            .Append(order.Order.Parties).Append(';')
            .Append(order.Side).Append(';')
            .Append(order.Ticker).Append(';')
            .Append(price).Append(';')
            .Append(size);
        if (displaySize > 0) sb.Append("(").Append(displaySize).Append(")");
        return sb.ToString();
    }
}

public class AbortedOrderLog
{
    private readonly ISpotOrder order;

    public AbortedOrderLog(ISpotOrder order) => this.order = order;

    public override string ToString() =>
        new StringBuilder(256)
            .Append("Aborted order: ")
            .Append(order.Order.VenueSelectionCriteria).Append(';')
            .Append(order.Order.OrderId.ClientOrderId).Append(';')
            .Append(order.Order.Parties).Append(';')
            .Append(order.Side).Append(';')
            .Append(order.Ticker).Append(';')
            .Append(order.Price).Append(';')
            .Append(order.Size).Append(';')
            .Append(order.Message)
            .ToString();
}

public class OrderUpdateLog
{
    private readonly string id;
    private readonly string? message;
    private readonly decimal price;
    private readonly decimal size;
    private readonly OrderStatus status;
    private readonly IVenueCriteria venueCriteria;

    public OrderUpdateLog(ISpotOrder order)
    {
        id = order.Order.OrderId.VenueClientOrderId.ToString()!;
        venueCriteria = order.Order.VenueSelectionCriteria;
        status = order.Order.Status;
        size = order.Size;
        price = order.Price;
        message = order.Message?.ToString();
    }

    public override string ToString()
    {
        var sb = new StringBuilder(128)
            .Append("Order (ID=").Append(id)
            .Append(" ExchangeID=").Append(venueCriteria)
            .Append(") updated: ").Append(status)
            .Append(' ').Append(size).Append('@').Append(price);
        if (!string.IsNullOrEmpty(message)) sb.Append(" [").Append(message).Append(']');
        return sb.ToString();
    }
}

public class OrderAmendLog
{
    private readonly IVenueCriteria exchangeId;
    private readonly decimal executedPrice;
    private readonly decimal executedSize;
    private readonly string id;
    private readonly string? message;
    private readonly decimal price;
    private readonly decimal size;
    private readonly OrderStatus status;

    public OrderAmendLog(ISpotOrder order)
    {
        id = order.Order.OrderId.VenueClientOrderId.ToString()!;
        exchangeId = order.Order.VenueSelectionCriteria;
        status = order.Order.Status;
        size = order.Size;
        executedSize = order.ExecutedSize;
        price = order.Price;
        executedPrice = order.ExecutedPrice;
        message = order.Message?.ToString();
    }

    public override string ToString()
    {
        var sb = new StringBuilder(128)
            .Append("Order (ID=").Append(id)
            .Append(" ExchangeID=").Append(exchangeId)
            .Append(") amended: ").Append(status)
            .Append(' ').Append(size).Append('@').Append(price)
            .Append(" Filled ").Append(executedSize).Append('@').Append(executedPrice);
        if (!string.IsNullOrEmpty(message)) sb.Append(" [").Append(message).Append(']');
        return sb.ToString();
    }
}

public class ExecutionLog
{
    private readonly IExecution execution;

    public ExecutionLog(IExecution execution) => this.execution = execution;

    public override string ToString() =>
        new StringBuilder(128)
            .Append("Execution received for order (ID=").Append(execution.ExecutionId)
            .Append("): ")
            .Append(execution.Venue).Append(';')
            .Append(execution.CounterParty).Append(';')
            .Append(execution.Price).Append(';')
            .Append(execution.Quantity)
            .ToString();
}

public class CancelOrderLog
{
    private readonly string id;

    public CancelOrderLog(IOrder order) => id = order.OrderId.VenueClientOrderId.ToString()!;

    public override string ToString() =>
        new StringBuilder(128)
            .Append("Cancel order (ID=").Append(id).Append(")")
            .ToString();
}

public class SuspendOrderLog
{
    private readonly string id;

    public SuspendOrderLog(IOrder order) => id = order.OrderId.VenueClientOrderId.ToString()!;

    public override string ToString() =>
        new StringBuilder(128)
            .Append("Suspend order (ID=").Append(id).Append(")")
            .ToString();
}

public class ResumeOrderLog
{
    private readonly string id;

    public ResumeOrderLog(IOrder order) => id = order.OrderId.VenueClientOrderId.ToString()!;

    public override string ToString() =>
        new StringBuilder(128)
            .Append("Resume order (ID=").Append(id).Append(")")
            .ToString();
}

public class AmendOrderLog
{
    private readonly IOrder order;
    private readonly IOrderAmend orderRequest;

    public AmendOrderLog(IOrder order, IOrderAmend orderRequest)
    {
        this.order = order;
        this.orderRequest = orderRequest;
    }

    public override string ToString() =>
        new StringBuilder(128)
            .Append("Amend order (ID=").Append(order.OrderId.ClientOrderId).Append("): ")
            .Append("NewPrice=").Append(orderRequest.NewPrice)
            .Append("NewSize=").Append(orderRequest.NewQuantity)
            .ToString();
}
