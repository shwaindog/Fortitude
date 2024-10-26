namespace FortitudeMarkets.Trading.Orders.Server;

public interface IOrderStatusUpdate : ITradingMessage
{
    IOrderId OrderId { get; set; }
    OrderStatus NewOrderStatus { get; set; }
    OrderUpdateEventType EventType { get; set; }
}
