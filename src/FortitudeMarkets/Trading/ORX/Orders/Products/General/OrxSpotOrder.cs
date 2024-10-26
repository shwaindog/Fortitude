#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Protocols.ORX.Serdes;
using FortitudeMarkets.Trading.Executions;
using FortitudeMarkets.Trading.Orders.Client;
using FortitudeMarkets.Trading.Orders.Products;
using FortitudeMarkets.Trading.Orders.Products.General;
using FortitudeMarkets.Trading.Orders.Venues;
using FortitudeMarkets.Trading.ORX.Orders.Venues;

#endregion

namespace FortitudeMarkets.Trading.ORX.Orders.Products.General;

public class OrxSpotOrder : OrxProductOrder, ISpotOrder
{
    // ReSharper disable once UnusedMember.Global
    public OrxSpotOrder() { }

    public OrxSpotOrder(ISpotOrder toClone) : base(toClone)
    {
        Type = toClone.Type;
        QuoteInformation = toClone.QuoteInformation != null ? new OrxVenuePriceQuoteId(toClone.QuoteInformation) : null;
        Side = toClone.Side;
        Ticker = toClone.Ticker != null ? new MutableString(toClone.Ticker) : null;
        Price = toClone.Price;
        Size = toClone.Size;
        DisplaySize = toClone.DisplaySize;
        ExecutedPrice = toClone.ExecutedPrice;
        ExecutedSize = toClone.ExecutedSize;
        SizeAtRisk = toClone.SizeAtRisk;
        AllowedPriceSlippage = toClone.AllowedPriceSlippage;
        AllowedVolumeSlippage = toClone.AllowedVolumeSlippage;
        FillExpectation = toClone.FillExpectation;
    }

    public OrxSpotOrder(string ticker, OrderSide side, decimal price, decimal size, OrderType type,
        decimal displaySize = 0m, decimal allowedPriceSlippage = 0m, decimal allowedVolumeSlippage = 0m,
        FillExpectation fillExpectation = FillExpectation.Complete, OrxVenuePriceQuoteId? quoteInformation = null,
        decimal executedPrice = 0m, decimal executedSize = 0m)
        : this((MutableString)ticker, side, price, size, type, displaySize, allowedPriceSlippage, allowedVolumeSlippage,
            fillExpectation, quoteInformation, executedPrice, executedSize) { }

    public OrxSpotOrder(MutableString ticker, OrderSide side, decimal price, decimal size, OrderType type,
        decimal displaySize = 0m, decimal allowedPriceSlippage = 0m, decimal allowedVolumeSlippage = 0m,
        FillExpectation fillExpectation = FillExpectation.Complete, OrxVenuePriceQuoteId? quoteInformation = null,
        decimal executedPrice = 0m, decimal executedSize = 0m)
    {
        Ticker = ticker;
        Side = side;
        Price = price;
        Size = size;
        Type = type;
        DisplaySize = displaySize;
        AllowedPriceSlippage = allowedPriceSlippage;
        AllowedVolumeSlippage = allowedVolumeSlippage;
        FillExpectation = fillExpectation;
        QuoteInformation = quoteInformation;
        ExecutedPrice = executedPrice;
        ExecutedSize = executedSize;
    }

    [OrxMandatoryField(10)] public MutableString? Ticker { get; set; }

    [OrxOptionalField(22)] public OrxVenuePriceQuoteId? QuoteInformation { get; set; }

    public override ProductType ProductType => ProductType.Spot;

    IMutableString? ISpotOrder.Ticker
    {
        get => Ticker;
        set => Ticker = value as MutableString;
    }

    [OrxMandatoryField(11)] public OrderSide Side { get; set; }

    [OrxMandatoryField(12)] public decimal Price { get; set; }

    [OrxMandatoryField(13)] public decimal Size { get; set; }

    [OrxMandatoryField(14)] public OrderType Type { get; set; }

    [OrxOptionalField(15)] public decimal DisplaySize { get; set; }

    [OrxOptionalField(16)] public decimal ExecutedPrice { get; set; }

    [OrxOptionalField(17)] public decimal ExecutedSize { get; set; }

    [OrxOptionalField(18)] public decimal SizeAtRisk { get; set; }

    [OrxOptionalField(19)] public decimal AllowedPriceSlippage { get; set; }

    [OrxOptionalField(20)] public decimal AllowedVolumeSlippage { get; set; }

    [OrxOptionalField(21)] public FillExpectation FillExpectation { get; set; }

    IVenuePriceQuoteId? ISpotOrder.QuoteInformation
    {
        get => QuoteInformation;
        set => QuoteInformation = value as OrxVenuePriceQuoteId;
    }

    public override bool IsError => Message != null || Size < ExecutedSize;

    public override void ApplyAmendment(IOrderAmend amendment)
    {
        Price = amendment.NewPrice;
        Size = amendment.NewQuantity;
        Side = amendment.NewSide;
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

    public override IProductOrder Clone() => Recycler?.Borrow<OrxSpotOrder>().CopyFrom(this) ?? new OrxSpotOrder(this);

    public override IProductOrder CopyFrom(IProductOrder source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);
        if (source is ISpotOrder spotOrder)
        {
            Side = spotOrder.Side;
            Ticker = spotOrder.Ticker.SyncOrRecycle(Ticker);
            Price = spotOrder.Price;
            Size = spotOrder.Size;
            DisplaySize = spotOrder.DisplaySize;
            Type = spotOrder.Type;
            ExecutedPrice = spotOrder.ExecutedPrice;
            ExecutedSize = spotOrder.ExecutedSize;
            SizeAtRisk = spotOrder.SizeAtRisk;
            AllowedPriceSlippage = spotOrder.AllowedPriceSlippage;
            AllowedVolumeSlippage = spotOrder.AllowedVolumeSlippage;
            FillExpectation = spotOrder.FillExpectation;
            QuoteInformation = spotOrder.QuoteInformation.SyncOrRecycle(QuoteInformation);
        }

        return this;
    }

    protected bool Equals(OrxSpotOrder other)
    {
        var tickersSame = Equals(Ticker, other.Ticker);
        var sideSame = Side == other.Side;
        var priceSame = Price == other.Price;
        var sizeSame = Size == other.Size;
        var typeSame = Equals(Type, other.Type);
        var displaySizeSame = DisplaySize == other.DisplaySize;
        var executedPriceSame = ExecutedPrice == other.ExecutedPrice;
        var executedSizeSame = ExecutedSize == other.ExecutedSize;
        var sizeAtRiskSame = SizeAtRisk == other.SizeAtRisk;
        var allowedPriceSlippageSame = AllowedPriceSlippage == other.AllowedPriceSlippage;
        var allowedVolumeSlippageSame = AllowedVolumeSlippage == other.AllowedVolumeSlippage;
        var fillExpectationSame = FillExpectation == other.FillExpectation;
        var quoteInfoSame = Equals(QuoteInformation, other.QuoteInformation);

        return tickersSame && sideSame && priceSame && sizeSame && typeSame && displaySizeSame && executedPriceSame
               && executedSizeSame && sizeAtRiskSame && allowedPriceSlippageSame && allowedVolumeSlippageSame &&
               fillExpectationSame && quoteInfoSame;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((OrxSpotOrder)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = Ticker != null ? Ticker.GetHashCode() : 0;
            hashCode = (hashCode * 397) ^ (int)Side;
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
}
