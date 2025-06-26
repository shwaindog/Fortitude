// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeMarkets.Trading.Counterparties;
using FortitudeMarkets.Trading.Executions;
using FortitudeMarkets.Trading.Orders.Products;
using FortitudeMarkets.Trading.Orders.Venues;

namespace FortitudeMarkets.Trading.Orders;

public interface IOrder : IReusableObject<IOrder>, IInterfacesComparable<IOrder>
{
    IVenueCriteria? VenueSelectionCriteria { get; set; } // if null adapter defaults

    IOrderId         OrderId        { get; set; }
    ushort           TickerId       { get; set; }
    string?  Ticker                 { get; set; }
    TimeInForce      TimeInForce    { get; set; }
    DateTime         CreationTime   { get; set; }
    DateTime?        SubmitTime     { get; set; }
    DateTime?        DoneTime       { get; set; }
    IParties        Parties        { get; set; }
    OrderStatus      Status         { get; set; }
    IVenueOrders?    VenueOrders    { get; set; }
    IExecutions?     Executions     { get; set; }
    string?  Message        { get; set; }
    bool             IsComplete     { get; set; }
    bool             IsError        { get; }
    ProductType      ProductType    { get; }

    string OrderToStringMembers { get; }
}
