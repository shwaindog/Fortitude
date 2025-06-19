#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Trading.Counterparties;
using FortitudeMarkets.Trading.Executions;
using FortitudeMarkets.Trading.Orders.Client;
using FortitudeMarkets.Trading.Orders.Products;
using FortitudeMarkets.Trading.Orders.Venues;
using FortitudeMarkets.Trading.ORX.Orders;

#endregion

namespace FortitudeMarkets.Trading.Orders;

public interface IOrder : IReusableObject<IOrder>, IInterfacesComparable<IOrder>
{
    IOrderId         OrderId                { get; set; }
    ushort           TickerId               { get; set; }
    IMutableString?  Ticker                 { get; set; }
    TimeInForce      TimeInForce            { get; set; }
    DateTime         CreationTime           { get; set; }
    DateTime?         SubmitTime             { get; set; }
    DateTime?         DoneTime               { get; set; }
    IParties?        Parties                { get; set; }
    OrderStatus      Status                 { get; set; }
    IOrderPublisher? OrderPublisher         { get; set; }
    IVenueCriteria?  VenueSelectionCriteria { get; set; } // if null adapter defaults
    IVenueOrders?    VenueOrders            { get; set; }
    IExecutions?     Executions             { get; set; }
    IMutableString?  Message                { get; set; }
    bool             IsComplete             { get; set; }
    bool             IsError                { get; }
    ProductType      ProductType            { get; }

    void ApplyAmendment(IOrderAmend amendment);
    bool RequiresAmendment(IOrderAmend amendment);
    void RegisterExecution(IExecution execution);

    OrxOrder AsOrxOrder();
    IOrder AsDomainOrder();
}
