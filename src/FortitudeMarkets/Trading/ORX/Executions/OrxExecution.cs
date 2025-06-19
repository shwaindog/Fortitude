#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Protocols.ORX.Serdes;
using FortitudeMarkets.Trading.Counterparties;
using FortitudeMarkets.Trading.Executions;
using FortitudeMarkets.Trading.Orders;
using FortitudeMarkets.Trading.Orders.Venues;
using FortitudeMarkets.Trading.ORX.CounterParties;
using FortitudeMarkets.Trading.ORX.Orders;
using FortitudeMarkets.Trading.ORX.Orders.Venues;

#endregion

namespace FortitudeMarkets.Trading.ORX.Executions;

public class OrxExecution : ReusableObject<IExecution>, IExecution
{
    public OrxExecution()
    {
        ExecutionId = new OrxExecutionId();
        Venue = new OrxVenue();
        VenueOrderId = new OrxVenueOrderId();
        OrderId = new OrxOrderId();
        CounterPartyPortfolio = new OrxPartyPortfolio();
    }

    public OrxExecution(IExecution toClone)
    {
        ExecutionId = new OrxExecutionId(toClone.ExecutionId);
        Venue = new OrxVenue(toClone.Venue);
        VenueOrderId = new OrxVenueOrderId(toClone.VenueOrderId);
        OrderId = new OrxOrderId(toClone.OrderId);
        ExecutionTime = toClone.ExecutionTime;
        Price = toClone.Price;
        Quantity = toClone.Quantity;
        CounterPartyPortfolio = new OrxPartyPortfolio(toClone.CounterPartyPortfolio);
        ValueDate = toClone.ValueDate;
        Type = toClone.Type;
        ExecutionStageType = toClone.ExecutionStageType;
    }


    public OrxExecution(OrxExecutionId executionId, OrxVenue venue, OrxVenueOrderId venueOrderId,
        OrxOrderId orderId, DateTime executionTime, decimal price, decimal quantity, OrxPartyPortfolio counterPartyPortfolio,
        DateTime valueDate, ExecutionType type, ExecutionStageType executionStageType)
    {
        ExecutionId = executionId;
        Venue = venue;
        VenueOrderId = venueOrderId;
        OrderId = orderId;
        ExecutionTime = executionTime;
        Price = price;
        Quantity = quantity;
        CounterPartyPortfolio = counterPartyPortfolio;
        ValueDate = valueDate;
        Type = type;
        ExecutionStageType = executionStageType;
    }

    [OrxMandatoryField(0)] public OrxExecutionId ExecutionId { get; set; }

    [OrxMandatoryField(1)] public OrxVenue Venue { get; set; }

    [OrxMandatoryField(2)] public OrxVenueOrderId VenueOrderId { get; set; }

    [OrxMandatoryField(3)] public OrxOrderId OrderId { get; set; }

    [OrxOptionalField(9)] public OrxPartyPortfolio CounterPartyPortfolio { get; set; }

    IExecutionId IExecution.ExecutionId
    {
        get => ExecutionId;
        set => ExecutionId = (OrxExecutionId)value;
    }

    IVenue IExecution.Venue
    {
        get => Venue;
        set => Venue = (OrxVenue)value;
    }

    IOrderId IExecution.OrderId
    {
        get => OrderId;
        set => OrderId = (OrxOrderId)value;
    }

    IVenueOrderId IExecution.VenueOrderId
    {
        get => VenueOrderId;
        set => VenueOrderId = (OrxVenueOrderId)value;
    }

    [OrxMandatoryField(4)] public DateTime ExecutionTime { get; set; } = DateTimeConstants.UnixEpoch;

    [OrxMandatoryField(5)] public decimal Price { get; set; }

    [OrxMandatoryField(6)] public decimal Quantity { get; set; }

    [OrxMandatoryField(7)] public decimal CumulativeQuantity { get; set; }

    [OrxMandatoryField(8)] public decimal CumulativeVwapPrice { get; set; }

    IPartyPortfolio IExecution.CounterPartyPortfolio
    {
        get => CounterPartyPortfolio;
        set => CounterPartyPortfolio = (OrxPartyPortfolio)value;
    }

    [OrxOptionalField(10)] public DateTime ValueDate { get; set; } = DateTimeConstants.UnixEpoch;

    [OrxOptionalField(11)] public ExecutionType Type { get; set; }

    [OrxOptionalField(12)] public ExecutionStageType ExecutionStageType { get; set; }

    public override IExecution Clone() => Recycler?.Borrow<OrxExecution>().CopyFrom(this) ?? new OrxExecution(this);

    public override IExecution CopyFrom(IExecution execution, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        ExecutionId = execution.ExecutionId.CopyOrClone(ExecutionId)!;
        Venue = execution.Venue.CopyOrClone(Venue)!;
        VenueOrderId = execution.VenueOrderId.CopyOrClone(VenueOrderId)!;
        OrderId = execution.OrderId.CopyOrClone(OrderId)!;
        Price = execution.Price;
        Quantity = execution.Quantity;
        CounterPartyPortfolio = execution.CounterPartyPortfolio.CopyOrClone(CounterPartyPortfolio)!;
        ValueDate = execution.ValueDate;
        Type = execution.Type;
        ExecutionStageType = execution.ExecutionStageType;
        return this;
    }

    protected bool Equals(OrxExecution other)
    {
        var executionIdSame = Equals(ExecutionId, other.ExecutionId);
        var venueSame = Equals(Venue, other.Venue);
        var venueOrderIdSame = Equals(VenueOrderId, other.VenueOrderId);
        var orderIdSame = Equals(OrderId, other.OrderId);
        var executionTimeSame = Equals(ExecutionTime, other.ExecutionTime);
        var priceSame = Price == other.Price;
        var quantitySame = Quantity == other.Quantity;
        var cumlativeQuantitySame = CumulativeQuantity == other.CumulativeQuantity;
        var cumlativeVwapPriceSame = CumulativeVwapPrice == other.CumulativeVwapPrice;
        var counterPartySame = Equals(CounterPartyPortfolio, other.CounterPartyPortfolio);
        var valueDateSame = ValueDate.Equals(other.ValueDate);
        var typeSame = Type == other.Type;
        var executionStageTypeSame = ExecutionStageType == other.ExecutionStageType;

        return executionIdSame && venueSame && venueOrderIdSame && orderIdSame && executionTimeSame &&
               priceSame && quantitySame && cumlativeQuantitySame && cumlativeVwapPriceSame &&
               counterPartySame && valueDateSame && typeSame && executionStageTypeSame;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((OrxExecution)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = ExecutionId != null ? ExecutionId.GetHashCode() : 0;
            hashCode = (hashCode * 397) ^ (Venue != null ? Venue.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (VenueOrderId != null ? VenueOrderId.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (OrderId != null ? OrderId.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (ExecutionTime != default ? ExecutionTime.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ Price.GetHashCode();
            hashCode = (hashCode * 397) ^ Quantity.GetHashCode();
            hashCode = (hashCode * 397) ^ CumulativeQuantity.GetHashCode();
            hashCode = (hashCode * 397) ^ CumulativeVwapPrice.GetHashCode();
            hashCode = (hashCode * 397) ^ (CounterPartyPortfolio != null ? CounterPartyPortfolio.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ ValueDate.GetHashCode();
            hashCode = (hashCode * 397) ^ (int)Type;
            hashCode = (hashCode * 397) ^ (int)ExecutionStageType;
            return hashCode;
        }
    }

    public override string ToString() => 
        $"{nameof(ExecutionId)}: {ExecutionId}, {nameof(Venue)}: {Venue}, {nameof(VenueOrderId)}: {VenueOrderId}, {nameof(OrderId)}: {OrderId}, " +
        $"{nameof(CounterPartyPortfolio)}: {CounterPartyPortfolio}, {nameof(ExecutionId)}: {ExecutionId}, {nameof(Venue)}: {Venue}, " +
        $"{nameof(OrderId)}: {OrderId}, {nameof(VenueOrderId)}: {VenueOrderId}, {nameof(ExecutionTime)}: {ExecutionTime}, {nameof(Price)}: {Price}, " +
        $"{nameof(Quantity)}: {Quantity}, {nameof(CumulativeQuantity)}: {CumulativeQuantity}, {nameof(CumulativeVwapPrice)}: {CumulativeVwapPrice}, " +
        $"{nameof(CounterPartyPortfolio)}: {CounterPartyPortfolio}, {nameof(ValueDate)}: {ValueDate}, {nameof(Type)}: {Type}, " +
        $"{nameof(ExecutionStageType)}: {ExecutionStageType}";
}
