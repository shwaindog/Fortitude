#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Trading.Counterparties;
using FortitudeMarkets.Trading.Executions;
using FortitudeMarkets.Trading.Orders.Products;
using FortitudeMarkets.Trading.Orders.Venues;

#endregion

namespace FortitudeMarkets.Trading.Orders;

public interface IOrder : IReusableObject<IOrder>
{
    IOrderId OrderId { get; set; }
    TimeInForce TimeInForce { get; set; }
    DateTime CreationTime { get; set; }
    DateTime SubmitTime { get; set; }
    DateTime DoneTime { get; set; }
    IParties? Parties { get; set; }
    OrderStatus Status { get; set; }
    IOrderPublisher? OrderPublisher { get; set; }
    IProductOrder? Product { get; set; }
    IVenueCriteria? VenueSelectionCriteria { get; set; }
    IVenueOrders? VenueOrders { get; set; }
    IExecutions? Executions { get; set; }
    IMutableString Message { get; set; }
}
