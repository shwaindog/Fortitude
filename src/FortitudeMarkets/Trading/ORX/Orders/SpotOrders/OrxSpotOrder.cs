#region

using System.Text;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeCommon.Types.Mutable.Strings;
using FortitudeIO.Protocols.ORX.Serdes;
using FortitudeMarkets.Trading.Counterparties;
using FortitudeMarkets.Trading.Executions;
using FortitudeMarkets.Trading.Orders;
using FortitudeMarkets.Trading.Orders.Client;
using FortitudeMarkets.Trading.Orders.Products;
using FortitudeMarkets.Trading.Orders.SpotOrders;
using FortitudeMarkets.Trading.Orders.Venues;
using FortitudeMarkets.Trading.ORX.CounterParties;
using FortitudeMarkets.Trading.ORX.Executions;
using FortitudeMarkets.Trading.ORX.Orders.Venues;

#endregion

namespace FortitudeMarkets.Trading.ORX.Orders.SpotOrders;

public class OrxSpotOrder : OrxOrder, ISpotTransmittableOrder
{
    // ReSharper disable once UnusedMember.Global
    public OrxSpotOrder() { }

    public OrxSpotOrder(ISpotTransmittableOrder toClone) : base(toClone)
    {
        Type  = toClone.Type;
        Side  = toClone.Side;
        Price = toClone.Price;
        Size  = toClone.Size;

        DisplaySize   = toClone.DisplaySize;
        ExecutedPrice = toClone.ExecutedPrice;
        ExecutedSize  = toClone.ExecutedSize;
        SizeAtRisk    = toClone.SizeAtRisk;

        AllowedPriceSlippage  = toClone.AllowedPriceSlippage;
        AllowedVolumeSlippage = toClone.AllowedVolumeSlippage;
        FillExpectation       = toClone.FillExpectation;
        QuoteInformation      = toClone.QuoteInformation != null ? new OrxVenuePriceQuoteId(toClone.QuoteInformation) : null;
    }

    public OrxSpotOrder
    (IOrderId orderId, ushort tickerId, uint accountId, OrderSide side, decimal price, decimal size
      , OrderType type, DateTime? creationTime = null, OrderStatus status = OrderStatus.New
      , TimeInForce timeInForce = TimeInForce.ImmediateOrCancel, IVenueCriteria? venueSelectionCriteria = null, decimal displaySize = 0m
      , decimal allowedPriceSlippage = 0m, decimal allowedVolumeSlippage = 0m, decimal executedPrice = 0m, decimal executedSize = 0m
      , decimal sizeAtRisk = 0m, FillExpectation fillExpectation = FillExpectation.Complete, IVenuePriceQuoteId? quoteInformation = null
      , DateTime? submitTime = null, DateTime? doneTime = null, IVenueOrders? venueOrders = null, IExecutions? executions = null
      , bool isComplete = false, string? tickerName = null, string? message = null, DateTime? lastUpdateTime = null)
        : this(new OrxOrderId(orderId), tickerId, new OrxParties(accountId), side, price, size, type, creationTime, status, timeInForce
             , venueSelectionCriteria.ToOrxVenueCriteria(), displaySize, allowedPriceSlippage, allowedVolumeSlippage, executedPrice, executedSize
             , sizeAtRisk, fillExpectation, quoteInformation.ToOrxPriceQuoteId(), submitTime, doneTime, venueOrders.ToOrxVenueOrders()
             , executions.ToOrxExecutions(), isComplete, tickerName, message, lastUpdateTime) { }

    public OrxSpotOrder
    (OrxOrderId orderId, ushort tickerId, OrxParties parties, OrderSide side, decimal price, decimal size, OrderType type
      , DateTime? creationTime = null, OrderStatus status = OrderStatus.New, TimeInForce timeInForce = TimeInForce.ImmediateOrCancel
      , OrxVenueCriteria? venueSelectionCriteria = null, decimal displaySize = 0m, decimal allowedPriceSlippage = 0m
      , decimal allowedVolumeSlippage = 0m, decimal executedPrice = 0m, decimal executedSize = 0m
      , decimal sizeAtRisk = 0m, FillExpectation fillExpectation = FillExpectation.Complete
      , OrxVenuePriceQuoteId? quoteInformation = null, DateTime? submitTime = null, DateTime? doneTime = null
      , OrxVenueOrders? venueOrders = null, OrxExecutions? executions = null
      , bool isComplete = false, MutableString? tickerName = null, MutableString? message = null, DateTime? lastUpdateTime = null)
        : base(orderId, tickerId, parties, creationTime, status, timeInForce, venueSelectionCriteria, submitTime, doneTime
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


    public override ProductType ProductType => ProductType.Spot;

    public override IMutableOrder AsOrder =>
        new SpotOrder(new OrderId(OrderId), TickerId, new Parties(Parties), Side, Price, Size, Type, CreationTime, Status, TimeInForce
                    , VenueSelectionCriteria.ToVenueCriteria(), DisplaySize, AllowedPriceSlippage, AllowedVolumeSlippage, ExecutedPrice
                    , ExecutedSize, SizeAtRisk, FillExpectation, QuoteInformation.ToVenuePriceQuoteId(), SubmitTime, DoneTime,
                      VenueOrders.ToVenueOrders(), Executions.ToExecutions(), IsError, IsComplete, ((IMutableOrder)this).Ticker
                    , ((IMutableOrder)this).Message);

    [OrxMandatoryField(20)] public OrderSide Side { get; set; }

    [OrxMandatoryField(21)] public decimal Price { get; set; }

    [OrxMandatoryField(22)] public decimal Size { get; set; }

    [OrxMandatoryField(23)] public OrderType Type { get; set; }

    [OrxOptionalField(20)] public decimal DisplaySize { get; set; }

    [OrxOptionalField(21)] public decimal ExecutedPrice { get; set; }

    [OrxOptionalField(22)] public decimal ExecutedSize { get; set; }

    [OrxOptionalField(23)] public decimal SizeAtRisk { get; set; }

    [OrxOptionalField(24)] public decimal AllowedPriceSlippage { get; set; }

    [OrxOptionalField(25)] public decimal AllowedVolumeSlippage { get; set; }

    [OrxOptionalField(26)] public FillExpectation FillExpectation { get; set; }

    [OrxOptionalField(27)] public OrxVenuePriceQuoteId? QuoteInformation { get; set; }

    IVenuePriceQuoteId? ISpotOrder.QuoteInformation => QuoteInformation;

    IVenuePriceQuoteId? IMutableSpotOrder.QuoteInformation
    {
        get => QuoteInformation;
        set => QuoteInformation = value as OrxVenuePriceQuoteId;
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

    public override void RegisterExecution(IExecution execution)
    {
        ExecutedPrice = (ExecutedPrice * ExecutedSize + execution.Price * execution.Quantity) /
                        (ExecutedSize + execution.Quantity);
        ExecutedSize += execution.Quantity;
    }

    public IMutableSpotOrder AsSpotOrder => new SpotOrder(this);

    public override ISpotTransmittableOrder AsTransmittableOrder => new SpotTransmittableOrder(this);

    public override OrxSpotOrder AsOrxOrder => this;

    ISpotOrder ICloneable<ISpotOrder>.Clone() => Clone();

    ISpotOrder ISpotOrder.Clone() => Clone();

    IMutableSpotOrder ICloneable<IMutableSpotOrder>.Clone() => Clone();

    IMutableSpotOrder IMutableSpotOrder.Clone() => Clone();

    ISpotTransmittableOrder ICloneable<ISpotTransmittableOrder>.Clone() => Clone();

    ISpotTransmittableOrder ISpotTransmittableOrder.Clone() => Clone();


    public override OrxSpotOrder Clone() => Recycler?.Borrow<OrxSpotOrder>().CopyFrom(this) ?? new OrxSpotOrder(this);

    public override OrxSpotOrder CopyFrom(IOrder source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);
        if (source is IMutableSpotOrder spotOrder)
        {
            Side  = spotOrder.Side;
            Price = spotOrder.Price;
            Size  = spotOrder.Size;
            Type  = spotOrder.Type;

            DisplaySize   = spotOrder.DisplaySize;
            ExecutedPrice = spotOrder.ExecutedPrice;
            ExecutedSize  = spotOrder.ExecutedSize;
            SizeAtRisk    = spotOrder.SizeAtRisk;

            AllowedPriceSlippage  = spotOrder.AllowedPriceSlippage;
            AllowedVolumeSlippage = spotOrder.AllowedVolumeSlippage;
            FillExpectation       = spotOrder.FillExpectation;
            QuoteInformation      = spotOrder.QuoteInformation.SyncOrRecycle(QuoteInformation);
        }

        return this;
    }

    public override bool AreEquivalent(ITransmittableOrder? source, bool exactTypes = false)
    {
        if (source is not IMutableSpotOrder spotOrder) return false;

        var baseSame = base.AreEquivalent(source, exactTypes);

        var sideSame  = Side == spotOrder.Side;
        var priceSame = Price == spotOrder.Price;
        var sizeSame  = Size == spotOrder.Size;
        var typeSame  = Type == spotOrder.Type;

        var displaySizeSame   = DisplaySize == spotOrder.DisplaySize;
        var executedPriceSame = ExecutedPrice == spotOrder.ExecutedPrice;
        var executedSizeSame  = ExecutedSize == spotOrder.ExecutedSize;
        var sizeAtRiskSame    = SizeAtRisk == spotOrder.SizeAtRisk;

        var allowedPriceSlippageSame  = AllowedPriceSlippage == spotOrder.AllowedPriceSlippage;
        var allowedVolumeSlippageSame = AllowedVolumeSlippage == spotOrder.AllowedVolumeSlippage;
        var fillExpectationSame       = FillExpectation == spotOrder.FillExpectation;
        var quoteInfoSame             = Equals(QuoteInformation, spotOrder.QuoteInformation);

        return baseSame && sideSame && priceSame && sizeSame && typeSame && displaySizeSame && executedPriceSame
            && executedSizeSame && sizeAtRiskSame && allowedPriceSlippageSame && allowedVolumeSlippageSame &&
               fillExpectationSame && quoteInfoSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as ITransmittableOrder, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = (int)Side;
            hashCode = (hashCode * 397) ^ Price.GetHashCode();
            hashCode = (hashCode * 397) ^ Size.GetHashCode();
            hashCode = (hashCode * 397) ^ Type.GetHashCode();
            hashCode = (hashCode * 397) ^ DisplaySize.GetHashCode();
            hashCode = (hashCode * 397) ^ ExecutedPrice.GetHashCode();
            hashCode = (hashCode * 397) ^ ExecutedSize.GetHashCode();
            hashCode = (hashCode * 397) ^ SizeAtRisk.GetHashCode();
            hashCode = (hashCode * 397) ^ AllowedPriceSlippage.GetHashCode();
            hashCode = (hashCode * 397) ^ AllowedVolumeSlippage.GetHashCode();
            hashCode = (hashCode * 397) ^ (int)FillExpectation;
            hashCode = (hashCode * 397) ^ (QuoteInformation != null ? QuoteInformation.GetHashCode() : 0);
            return hashCode;
        }
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
            if (QuoteInformation != null) sb.Append(", ").Append(nameof(QuoteInformation)).Append(": ").Append(QuoteInformation);
            sb.Append(", ").Append(nameof(FillExpectation)).Append(": ").Append(FillExpectation);

            return sb.ToString();
        }
    }

    public override string ToString() => $"{nameof(OrxSpotOrder)}{{{SpotOrderToStringMembers}}}";
}
