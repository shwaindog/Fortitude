// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Types;
using FortitudeMarkets.Trading.Counterparties;
using FortitudeMarkets.Trading.Executions;
using FortitudeMarkets.Trading.Orders.Products;
using FortitudeMarkets.Trading.Orders.Venues;

namespace FortitudeMarkets.Trading.Orders;

public interface IOrder : IReusableObject<IOrder>, IInterfacesComparable<IOrder>
{
    IVenueCriteria? VenueSelectionCriteria { get; } // if null adapter defaults

    IOrderId      OrderId        { get; }
    ushort        TickerId       { get; }
    string?       Ticker         { get; }
    TimeInForce   TimeInForce    { get; }
    DateTime      CreationTime   { get; }
    DateTime      LastUpdateTime { get; }
    DateTime?     SubmitTime     { get; }
    DateTime?     DoneTime       { get; }
    IParties      Parties        { get; }
    OrderStatus   Status         { get; }
    IVenueOrders? VenueOrders    { get; }
    IExecutions?  Executions     { get; }
    string?       Message        { get; }
    bool          IsComplete     { get; }
    bool          IsError        { get; }
    ProductType   ProductType    { get; }

    string OrderToStringMembers { get; }
}

public interface IMutableOrder : IOrder, ICloneable<IMutableOrder>
{
    new IVenueCriteria? VenueSelectionCriteria { get; set; } // if null adapter defaults

    new IOrderId      OrderId        { get; set; }
    new ushort        TickerId       { get; set; }
    new string?       Ticker         { get; set; }
    new TimeInForce   TimeInForce    { get; set; }
    new DateTime      CreationTime   { get; set; }
    new DateTime      LastUpdateTime { get; set; }
    new DateTime?     SubmitTime     { get; set; }
    new DateTime?     DoneTime       { get; set; }
    new IParties      Parties        { get; set; }
    new OrderStatus   Status         { get; set; }
    new IVenueOrders? VenueOrders    { get; set; }
    new IExecutions?  Executions     { get; set; }
    new string?       Message        { get; set; }
    new bool          IsComplete     { get; set; }

    new IMutableOrder Clone();
}
