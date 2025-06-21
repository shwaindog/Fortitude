#region

using System.Text;
using FortitudeMarkets.Trading.Executions;
using FortitudeMarkets.Trading.Orders;
using FortitudeMarkets.Trading.Orders.Client;
using FortitudeMarkets.Trading.Orders.Products;
using FortitudeMarkets.Trading.Orders.SpotOrders;
using FortitudeMarkets.Trading.Orders.Venues;

#endregion

namespace FortitudeMarkets.Monitoring.Logging;

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
            .Append(order.VenueSelectionCriteria).Append(';')
            .Append(order.OrderId.ClientOrderId).Append(';')
            .Append(order.Parties).Append(';')
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
            .Append(order.VenueSelectionCriteria).Append(';')
            .Append(order.OrderId.ClientOrderId).Append(';')
            .Append(order.Parties).Append(';')
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
    private readonly IVenueCriteria? venueCriteria;

    public OrderUpdateLog(ISpotOrder order)
    {
        id = order.OrderId.ClientOrderId.ToString();
        venueCriteria = order.VenueSelectionCriteria;
        status = order.Status;
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
    private readonly IVenueCriteria? exchangeId;
    private readonly decimal executedPrice;
    private readonly decimal executedSize;
    private readonly string id;
    private readonly string? message;
    private readonly decimal price;
    private readonly decimal size;
    private readonly OrderStatus status;

    public OrderAmendLog(ISpotOrder order)
    {
        id = order.OrderId.ClientOrderId.ToString();
        exchangeId = order.VenueSelectionCriteria;
        status = order.Status;
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
            .Append(execution.CounterPartyPortfolio).Append(';')
            .Append(execution.Price).Append(';')
            .Append(execution.Quantity)
            .ToString();
}

public class CancelOrderLog
{
    private readonly string id;

    public CancelOrderLog(ITransmittableOrder order) => id = order.OrderId.ToString()!;

    public override string ToString() =>
        new StringBuilder(128)
            .Append("Cancel order (ID=").Append(id).Append(")")
            .ToString();
}

public class SuspendOrderLog
{
    private readonly string id;

    public SuspendOrderLog(ITransmittableOrder order) => id = order.OrderId.ToString()!;

    public override string ToString() =>
        new StringBuilder(128)
            .Append("Suspend order (ID=").Append(id).Append(")")
            .ToString();
}

public class ResumeOrderLog
{
    private readonly string id;

    public ResumeOrderLog(ITransmittableOrder order) => id = order.OrderId.ToString()!;

    public override string ToString() =>
        new StringBuilder(128)
            .Append("Resume order (ID=").Append(id).Append(")")
            .ToString();
}

public class AmendOrderLog
{
    private readonly ITransmittableOrder order;
    private readonly IOrderAmend orderRequest;

    public AmendOrderLog(ITransmittableOrder order, IOrderAmend orderRequest)
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
