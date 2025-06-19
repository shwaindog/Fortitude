#region

using System.Text;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Trading.Counterparties;
using FortitudeMarkets.Trading.Executions;
using FortitudeMarkets.Trading.Orders.Client;
using FortitudeMarkets.Trading.Orders.Venues;
using FortitudeMarkets.Trading.ORX.Orders;
using FortitudeMarkets.Trading.ORX.Orders.Products.General;

#endregion

namespace FortitudeMarkets.Trading.Orders.Products.General;

public class SpotOrder : Order, ISpotOrder
{
    public SpotOrder() { }

    public SpotOrder(ISpotOrder toClone) : base(toClone)
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

    public SpotOrder
    (IOrderId orderId, ushort tickerId, uint accountId, OrderSide side, decimal price, decimal size
      , OrderType type, DateTime? creationTime = null, OrderStatus status = OrderStatus.PendingNew
      , TimeInForce timeInForce = TimeInForce.ImmediateOrCancel, IVenueCriteria? venueSelectionCriteria = null, decimal displaySize = 0m
      , decimal allowedPriceSlippage = 0m, decimal allowedVolumeSlippage = 0m, decimal executedPrice = 0m, decimal executedSize = 0m
      , decimal sizeAtRisk = 0m, FillExpectation fillExpectation = FillExpectation.Complete, IVenuePriceQuoteId? quoteInformation = null
      , DateTime? submitTime = null, DateTime? doneTime = null, IVenueOrders? venueOrders = null, IExecutions? executions = null
      , string? tickerName = null, IMutableString? message = null, bool isError = false, bool isComplete = false)
        : this(orderId, tickerId, new Parties(accountId), side, price, size, type, creationTime, status, timeInForce
             , venueSelectionCriteria, displaySize, allowedPriceSlippage, allowedVolumeSlippage, executedPrice, executedSize
             , sizeAtRisk, fillExpectation, quoteInformation, submitTime, doneTime, venueOrders, executions, (MutableString?)tickerName
             , message, isError, isComplete) { }

    public SpotOrder
    (IOrderId orderId, ushort tickerId, IParties parties, OrderSide side, decimal price, decimal size, OrderType type
      , DateTime? creationTime = null, OrderStatus status = OrderStatus.PendingNew, TimeInForce timeInForce = TimeInForce.ImmediateOrCancel
      , IVenueCriteria? venueSelectionCriteria = null, decimal displaySize = 0m, decimal allowedPriceSlippage = 0m
      , decimal allowedVolumeSlippage = 0m, decimal executedPrice = 0m, decimal executedSize = 0m
      , decimal sizeAtRisk = 0m, FillExpectation fillExpectation = FillExpectation.Complete
      , IVenuePriceQuoteId? quoteInformation = null, DateTime? submitTime = null, DateTime? doneTime = null
      , IVenueOrders? venueOrders = null, IExecutions? executions = null, IMutableString? tickerName = null, IMutableString? message = null
      , bool isError = false, bool isComplete = false)
        : base(orderId, tickerId, parties, creationTime, status, timeInForce, venueSelectionCriteria, submitTime, doneTime
             , venueOrders, executions, tickerName, message, isError, isComplete)
    {
        Side   = side;
        Price  = price;
        Size   = size;
        Type   = type;

        DisplaySize   = displaySize;
        ExecutedPrice = executedPrice;
        ExecutedSize  = executedSize;
        SizeAtRisk    = sizeAtRisk;

        AllowedPriceSlippage  = allowedPriceSlippage;
        AllowedVolumeSlippage = allowedVolumeSlippage;
        FillExpectation       = fillExpectation;
        QuoteInformation      = quoteInformation;
    }

    public override ProductType ProductType => ProductType.Spot;

    public OrderSide Side  { get; set; }
    public decimal   Price { get; set; }
    public decimal   Size  { get; set; }
    public OrderType Type  { get; set; }

    public decimal DisplaySize   { get; set; }
    public decimal ExecutedPrice { get; set; }
    public decimal ExecutedSize  { get; set; }
    public decimal SizeAtRisk    { get; set; }

    public decimal AllowedPriceSlippage  { get; set; }
    public decimal AllowedVolumeSlippage { get; set; }

    public FillExpectation     FillExpectation  { get; set; }
    public IVenuePriceQuoteId? QuoteInformation { get; set; }

    public override IOrder AsDomainOrder() => this;

    public override OrxOrder AsOrxOrder() => new OrxSpotOrder(this);

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


    public override SpotOrder Clone() => new(this);

    public override SpotOrder CopyFrom(IOrder source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);
        if (source is ISpotOrder spotOrder)
        {
            Side                  = spotOrder.Side;
            Price                 = spotOrder.Price;
            Size                  = spotOrder.Size;
            DisplaySize           = spotOrder.DisplaySize;
            Type                  = spotOrder.Type;
            ExecutedPrice         = spotOrder.ExecutedPrice;
            ExecutedSize          = spotOrder.ExecutedSize;
            SizeAtRisk            = spotOrder.SizeAtRisk;
            AllowedPriceSlippage  = spotOrder.AllowedPriceSlippage;
            AllowedVolumeSlippage = spotOrder.AllowedVolumeSlippage;
            FillExpectation       = spotOrder.FillExpectation;
            QuoteInformation      = spotOrder.QuoteInformation;
        }

        return this;
    }

    public void SetError(string msg, long sizeAtRisk)
    {
        SetError((MutableString)msg!, sizeAtRisk);
    }

    public void SetError(IMutableString msg, long sizeAtRisk)
    {
        Message    = msg;
        SizeAtRisk = sizeAtRisk;
        Status     = OrderStatus.Dead;
    }

    protected string SpotOrderToStringMembers
    {
        get
        {
            var sb = new StringBuilder();
            sb.Append(OrderToStringMembers);
            sb.Append(", ").Append(nameof(Side)).Append(": ").Append(Side);
            sb.Append(", ").Append(nameof(Price)).Append(": ").Append(Price);
            sb.Append(", ").Append(nameof(Size)).Append(": ").Append(Size);
            if (DisplaySize != 0m && DisplaySize != Size) sb.Append(", ").Append(nameof(DisplaySize)).Append(": ").Append(DisplaySize);
            sb.Append(", ").Append(nameof(Type)).Append(": ").Append(Type).Append(", ");
            if (ExecutedPrice != 0m) sb.Append(", ").Append(nameof(ExecutedPrice)).Append(": ").Append(ExecutedPrice);
            if (ExecutedSize != 0m) sb.Append(", ").Append(nameof(ExecutedSize)).Append(": ").Append(ExecutedSize);
            if (SizeAtRisk != 0m) sb.Append(", ").Append(nameof(SizeAtRisk)).Append(": ").Append(SizeAtRisk);
            if (AllowedPriceSlippage != 0m) sb.Append(", ").Append(nameof(AllowedPriceSlippage)).Append(": ").Append(AllowedPriceSlippage);
            if (AllowedVolumeSlippage != 0m) sb.Append(", ").Append(nameof(AllowedVolumeSlippage)).Append(": ").Append(AllowedVolumeSlippage);
            sb.Append(", ").Append(nameof(FillExpectation)).Append(": ").Append(FillExpectation);
            sb.Append(", ").Append(nameof(QuoteInformation)).Append(": ").Append(QuoteInformation);

            return sb.ToString();
        }
    }

    public override string ToString() => $"{nameof(OrxSpotOrder)}{{{SpotOrderToStringMembers}}}";
}
