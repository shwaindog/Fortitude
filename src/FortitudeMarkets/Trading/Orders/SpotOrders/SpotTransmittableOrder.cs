#region

using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeMarkets.Trading.Counterparties;
using FortitudeMarkets.Trading.Executions;
using FortitudeMarkets.Trading.Orders.Client;
using FortitudeMarkets.Trading.Orders.Products;
using FortitudeMarkets.Trading.Orders.Venues;
using FortitudeMarkets.Trading.ORX.Orders;
using FortitudeMarkets.Trading.ORX.Orders.SpotOrders;

#endregion

namespace FortitudeMarkets.Trading.Orders.SpotOrders;

public class SpotTransmittableOrder : TransmittableOrder, ISpotTransmittableOrder
{
    public SpotTransmittableOrder() { }

    public SpotTransmittableOrder(ISpotTransmittableOrder toClone) : base(toClone)
    {
        Side  = toClone.Side;
        Price = toClone.Price;
        Size  = toClone.Size;
        Type  = toClone.Type;

        DisplaySize   = toClone.DisplaySize;
        ExecutedPrice = toClone.ExecutedPrice;
        ExecutedSize  = toClone.ExecutedSize;
        SizeAtRisk    = toClone.SizeAtRisk;

        AllowedPriceSlippage  = toClone.AllowedPriceSlippage;
        AllowedVolumeSlippage = toClone.AllowedVolumeSlippage;
        FillExpectation       = toClone.FillExpectation;
        QuoteInformation      = toClone.QuoteInformation;
    }

    public SpotTransmittableOrder
    (IOrderId orderId, ushort tickerId, uint accountId, OrderSide side, decimal price, decimal size
      , OrderType type, DateTime? creationTime = null, OrderStatus status = OrderStatus.PendingNew
      , TimeInForce timeInForce = TimeInForce.ImmediateOrCancel, IVenueCriteria? venueSelectionCriteria = null, decimal displaySize = 0m
      , decimal allowedPriceSlippage = 0m, decimal allowedVolumeSlippage = 0m, decimal executedPrice = 0m, decimal executedSize = 0m
      , decimal sizeAtRisk = 0m, FillExpectation fillExpectation = FillExpectation.Complete, IVenuePriceQuoteId? quoteInformation = null
      , DateTime? submitTime = null, DateTime? doneTime = null, IVenueOrders? venueOrders = null, IExecutions? executions = null
      , bool isComplete = false, string? tickerName = null, IMutableString? message = null, DateTime? lastUpdateTime = null)
        : this(orderId, tickerId, new Parties(accountId), side, price, size, type, creationTime, status, timeInForce
             , venueSelectionCriteria, displaySize, allowedPriceSlippage, allowedVolumeSlippage, executedPrice, executedSize
             , sizeAtRisk, fillExpectation, quoteInformation, submitTime, doneTime, venueOrders, executions, isComplete
             , (MutableString?)tickerName, (MutableString?)message, lastUpdateTime) { }

    public SpotTransmittableOrder
    (IOrderId orderId, ushort tickerId, IParties parties, OrderSide side, decimal price, decimal size, OrderType type
      , DateTime? creationTime = null, OrderStatus status = OrderStatus.PendingNew, TimeInForce timeInForce = TimeInForce.ImmediateOrCancel
      , IVenueCriteria? venueSelectionCriteria = null, decimal displaySize = 0m, decimal allowedPriceSlippage = 0m
      , decimal allowedVolumeSlippage = 0m, decimal executedPrice = 0m, decimal executedSize = 0m
      , decimal sizeAtRisk = 0m, FillExpectation fillExpectation = FillExpectation.Complete
      , IVenuePriceQuoteId? quoteInformation = null, DateTime? submitTime = null, DateTime? doneTime = null
      , IVenueOrders? venueOrders = null, IExecutions? executions = null
      , bool isComplete = false, IMutableString? tickerName = null, IMutableString? message = null, DateTime? lastUpdateTime = null)
        : base(new SpotOrder(), orderId, tickerId, parties, creationTime, status, timeInForce, venueSelectionCriteria, submitTime, doneTime
             , venueOrders, executions, isComplete, tickerName, message, lastUpdateTime)
    {
        Side  = side;
        Price = price;
        Size  = size;
        Type  = type;

        DisplaySize   = displaySize;
        ExecutedPrice = executedPrice;
        ExecutedSize  = executedSize;
        SizeAtRisk    = sizeAtRisk;

        AllowedPriceSlippage  = allowedPriceSlippage;
        AllowedVolumeSlippage = allowedVolumeSlippage;
        FillExpectation       = fillExpectation;
        QuoteInformation      = quoteInformation;
    }

    public IMutableSpotOrder AsSpotOrder => (IMutableSpotOrder)WrappedOrder;

    public override ProductType ProductType => ProductType.Spot;

    public OrderSide Side
    {
        get => AsSpotOrder.Side;
        set => AsSpotOrder.Side = value;
    }

    public decimal Price
    {
        get => AsSpotOrder.Price;
        set => AsSpotOrder.Price = value;
    }

    public decimal Size
    {
        get => AsSpotOrder.Size;
        set => AsSpotOrder.Size = value;
    }

    public OrderType Type
    {
        get => AsSpotOrder.Type;
        set => AsSpotOrder.Type = value;
    }

    public decimal DisplaySize
    {
        get => AsSpotOrder.DisplaySize;
        set => AsSpotOrder.DisplaySize = value;
    }

    public decimal ExecutedPrice
    {
        get => AsSpotOrder.ExecutedPrice;
        set => AsSpotOrder.ExecutedPrice = value;
    }

    public decimal ExecutedSize
    {
        get => AsSpotOrder.ExecutedSize;
        set => AsSpotOrder.ExecutedSize = value;
    }

    public decimal SizeAtRisk
    {
        get => AsSpotOrder.SizeAtRisk;
        set => AsSpotOrder.SizeAtRisk = value;
    }

    public decimal AllowedPriceSlippage
    {
        get => AsSpotOrder.AllowedPriceSlippage;
        set => AsSpotOrder.AllowedPriceSlippage = value;
    }

    public decimal AllowedVolumeSlippage
    {
        get => AsSpotOrder.AllowedVolumeSlippage;
        set => AsSpotOrder.AllowedVolumeSlippage = value;
    }

    public FillExpectation FillExpectation
    {
        get => AsSpotOrder.FillExpectation;
        set => AsSpotOrder.FillExpectation = value;
    }

    public IVenuePriceQuoteId? QuoteInformation
    {
        get => AsSpotOrder.QuoteInformation;
        set => AsSpotOrder.QuoteInformation = value;
    }

    public override ITransmittableOrder AsTransmittableOrder => this;

    public override OrxOrder AsOrxOrder => new OrxSpotOrder(this);

    public override void RegisterExecution(IExecution execution)
    {
        ExecutedPrice = (ExecutedPrice * ExecutedSize + execution.Price * execution.Quantity) /
                        (ExecutedSize + execution.Quantity);
        ExecutedSize += execution.Quantity;
    }

    public override void ApplyAmendment(IOrderAmend amendment)
    {
        Price = amendment.NewPrice;
        Size  = amendment.NewQuantity;
        Side  = amendment.NewSide;
    }

    public override bool RequiresAmendment(IOrderAmend amendment) =>
        amendment.NewPrice != Price ||
        amendment.NewQuantity != Size ||
        amendment.NewSide != Side;

    ISpotOrder ICloneable<ISpotOrder>.Clone() => Clone();

    ISpotOrder ISpotOrder.Clone() => Clone();

    IMutableSpotOrder ICloneable<IMutableSpotOrder>.Clone() => Clone();

    IMutableSpotOrder IMutableSpotOrder.Clone() => Clone();

    ISpotTransmittableOrder ICloneable<ISpotTransmittableOrder>.Clone() => Clone();

    ISpotTransmittableOrder ISpotTransmittableOrder.Clone() => Clone();

    public override SpotTransmittableOrder Clone() => new(this);

    public override SpotTransmittableOrder CopyFrom(ITransmittableOrder source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);

        return this;
    }

    public void SetError(string msg, long sizeAtRisk)
    {
        SetError((MutableString)msg, sizeAtRisk);
    }

    public void SetError(IMutableString msg, long sizeAtRisk)
    {
        MutableMessage = msg;
        SizeAtRisk     = sizeAtRisk;
        Status         = OrderStatus.Dead;
    }

    public override string OrderToStringMembers => AsSpotOrder.OrderToStringMembers;

    public override string ToString() => $"{nameof(SpotTransmittableOrder)}{{{OrderToStringMembers}}}";
}
