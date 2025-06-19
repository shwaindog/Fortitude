// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Trading.Counterparties;
using FortitudeMarkets.Trading.Orders;
using FortitudeMarkets.Trading.Orders.Venues;

#endregion

namespace FortitudeMarkets.Trading.Executions;

public class Execution : ReusableObject<IExecution>, IExecution
{
    public Execution()
    {
        ExecutionId  = null!;
        Venue        = null!;
        VenueOrderId = null!;
        OrderId      = null!;
        CounterPartyPortfolio = null!;
        CounterPartyPortfolio = null!;
    }

    public Execution(IExecution toClone)
    {
        ExecutionId         = toClone.ExecutionId;
        Venue               = toClone.Venue;
        VenueOrderId        = toClone.VenueOrderId;
        OrderId             = toClone.OrderId;
        ExecutionTime       = toClone.ExecutionTime;
        Price               = toClone.Price;
        Quantity            = toClone.Quantity;
        CumulativeQuantity  = toClone.CumulativeQuantity;
        CumulativeVwapPrice = toClone.CumulativeVwapPrice;
        CounterPartyPortfolio        = toClone.CounterPartyPortfolio;
        ValueDate           = toClone.ValueDate;
        Type                = toClone.Type;
        ExecutionStageType  = toClone.ExecutionStageType;
    }

    public Execution
    (IExecutionId executionId, IVenue venue, IVenueOrderId venueOrderId, IOrderId orderId,
        DateTime executionTime, decimal price, decimal quantity, decimal cumlativeQuantity,
        decimal cumlativeVwapPrice, IPartyPortfolio counterPartyPortfolio, DateTime valueDate, ExecutionType type
      , ExecutionStageType stageType)
    {
        ExecutionId         = executionId;
        Venue               = venue;
        VenueOrderId        = venueOrderId;
        OrderId             = orderId;
        ExecutionTime       = executionTime;
        Price               = price;
        Quantity            = quantity;
        CumulativeQuantity  = cumlativeQuantity;
        CumulativeVwapPrice = cumlativeVwapPrice;
        CounterPartyPortfolio        = counterPartyPortfolio;
        ValueDate           = valueDate;
        Type                = type;
        ExecutionStageType  = stageType;
    }

    public IExecutionId       ExecutionId         { get; set; }
    public IVenue             Venue               { get; set; }
    public IVenueOrderId      VenueOrderId        { get; set; }
    public IOrderId           OrderId             { get; set; }
    public DateTime           ExecutionTime       { get; set; }
    public decimal            Price               { get; set; }
    public decimal            Quantity            { get; set; }
    public decimal            CumulativeQuantity  { get; set; }
    public decimal            CumulativeVwapPrice { get; set; }
    public IPartyPortfolio             CounterPartyPortfolio        { get; set; }
    public DateTime           ValueDate           { get; set; }
    public ExecutionType      Type                { get; set; }
    public ExecutionStageType ExecutionStageType  { get; set; }

    public override IExecution CopyFrom(IExecution source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        ExecutionId = source.ExecutionId.SyncOrRecycle(ExecutionId as ExecutionId)!;
        Venue       = source.Venue.SyncOrRecycle(Venue as Venue)!;
        return this;
    }

    public override IExecution Clone() => Recycler?.Borrow<Execution>().CopyFrom(this) ?? new Execution(this);
}
