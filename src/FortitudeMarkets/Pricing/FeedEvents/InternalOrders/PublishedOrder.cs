using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;

namespace FortitudeMarkets.Pricing.FeedEvents.InternalOrders;

public class PublishedOrder : ReusableObject<IPublishedOrder>, IMutablePublishedOrder
{
    public PublishedOrder() { }

    public PublishedOrder
    (int orderId, DateTime createdTime, decimal orderDisplayVolume
      , OrderType orderType = OrderType.None, OrderFlags typeFlags = OrderFlags.None
      , OrderLifeCycleState lifeCycleState = OrderLifeCycleState.None, DateTime? updatedTime = null
      , decimal? remainingVolume = null, uint trackingId = 0)
    {
        OrderId              = orderId;
        CreatedTime          = createdTime;
        OrderType            = orderType;
        TypeFlags            = typeFlags;
        OrderLifeCycleState  = lifeCycleState;
        OrderDisplayVolume   = orderDisplayVolume;
        OrderRemainingVolume = remainingVolume ?? orderDisplayVolume;
        UpdateTime           = updatedTime ?? createdTime;
        TrackingId           = trackingId;
    }

    public PublishedOrder(IPublishedOrder toClone)
    {
        OrderId              = toClone.OrderId;
        CreatedTime          = toClone.CreatedTime;
        OrderType            = toClone.OrderType;
        TypeFlags            = toClone.TypeFlags;
        OrderLifeCycleState  = toClone.OrderLifeCycleState;
        OrderDisplayVolume   = toClone.OrderDisplayVolume;
        OrderRemainingVolume = toClone.OrderRemainingVolume;
        UpdateTime           = toClone.UpdateTime;
        TrackingId           = toClone.TrackingId;
    }

    public override IPublishedOrder Clone() =>
        Recycler?.Borrow<PublishedOrder>().CopyFrom(this, CopyMergeFlags.FullReplace) ?? new PublishedOrder(this);

    public int       OrderId   { get; set; }
    public OrderType OrderType { get; set; }

    public OrderFlags TypeFlags  { get; set; }
    public uint       TrackingId { get; set; }

    public OrderLifeCycleState OrderLifeCycleState { get; set; }

    public DateTime CreatedTime        { get; set; }
    public DateTime UpdateTime         { get; set; }
    public decimal  OrderDisplayVolume { get; set; }

    public decimal OrderRemainingVolume { get; set; }
    public virtual bool IsEmpty
    {
        get =>
            OrderId == 0
         && CreatedTime == default
         && OrderType == OrderType.None
         && TypeFlags == OrderFlags.None
         && OrderLifeCycleState == OrderLifeCycleState.None
         && UpdateTime == default
         && OrderDisplayVolume == 0m
         && OrderRemainingVolume == 0m
         && TrackingId == 0;
        set
        {
            if (!value) return;

            OrderId     = 0;
            CreatedTime = default;
            UpdateTime  = default;
            OrderType   = OrderType.None;
            TypeFlags   = OrderFlags.None;
            TrackingId  = 0;

            OrderLifeCycleState = OrderLifeCycleState.None;
            OrderDisplayVolume   = 0m;
            OrderRemainingVolume = 0m;
        }
    }

    public override void StateReset()
    {
        OrderId     = 0;
        CreatedTime = default;
        UpdateTime  = default;
        OrderType   = OrderType.None;
        TypeFlags   = OrderFlags.None;
        TrackingId  = 0;

        OrderLifeCycleState = OrderLifeCycleState.None;
        OrderDisplayVolume   = 0m;
        OrderRemainingVolume = 0m;
    }

    public override PublishedOrder CopyFrom(IPublishedOrder source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        OrderId     = source.OrderId;
        CreatedTime = source.CreatedTime;
        OrderType   = source.OrderType;
        TypeFlags   = source.TypeFlags;
        UpdateTime  = source.UpdateTime;
        TrackingId = source.TrackingId;

        OrderLifeCycleState  = source.OrderLifeCycleState;
        OrderDisplayVolume   = source.OrderDisplayVolume;
        OrderRemainingVolume = source.OrderRemainingVolume;

        return this;
    }


    public virtual bool AreEquivalent(IPublishedOrder? other, bool exactTypes = false)
    {
        if (other == null) return false;
        if (exactTypes && other.GetType() != GetType()) return false;

        var orderIdsSame  = OrderId == other.OrderId;
        var createdSame   = CreatedTime == other.CreatedTime;
        var orderTypeSame = OrderType == other.OrderType;
        var typeFlagsSame = TypeFlags == other.TypeFlags;
        var lifecycleSame = OrderLifeCycleState == other.OrderLifeCycleState;
        var updatedSame   = UpdateTime == other.UpdateTime;
        var volumeSame    = OrderDisplayVolume == other.OrderDisplayVolume;
        var trackingIdSame    = TrackingId == other.TrackingId;

        var remainingVolumeSame = OrderRemainingVolume == other.OrderRemainingVolume;

        return orderIdsSame && orderTypeSame && typeFlagsSame && lifecycleSame && createdSame && updatedSame && volumeSame && remainingVolumeSame && trackingIdSame;
    }

    public virtual IInternalPassiveOrder? ToInternalOrder() =>
        this is InternalOrder internalOrder && TypeFlags.IsInternalOrder() && TypeFlags.HasInternalOrderInfo()
            ? internalOrder
            : null;

    public virtual IExternalCounterPartyInfoOrder? ToExternalCounterPartyInfoOrder() =>
        this is ExternalCounterPartyInfoOrder externalCounterPartyOrder && TypeFlags.IsExternalOrder() && TypeFlags.HasExternalCounterPartyInfo()
            ? externalCounterPartyOrder
            : null;
    
    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IPublishedOrder, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = OrderId;
            hashCode = ((int)OrderType * 397) ^ hashCode;
            hashCode = ((int)TypeFlags * 397) ^ hashCode;
            hashCode = ((int)OrderLifeCycleState * 397) ^ hashCode;
            hashCode = (CreatedTime.GetHashCode() * 397) ^ hashCode;
            hashCode = (UpdateTime.GetHashCode() * 397) ^ hashCode;
            hashCode = (OrderDisplayVolume.GetHashCode() * 397) ^ hashCode;
            hashCode = (OrderRemainingVolume.GetHashCode() * 397) ^ hashCode;
            hashCode = ((int)TrackingId * 397) ^ hashCode;
            return hashCode;
        }
    }

    protected string PublishedOrderToStringMembers => 
        $"{nameof(OrderId)}: {OrderId}, {nameof(OrderType)}: {OrderType}, {nameof(TypeFlags)}: {TypeFlags}, {nameof(CreatedTime)}: {CreatedTime}, " +
        $"{nameof(OrderLifeCycleState)}: {OrderLifeCycleState}, {nameof(UpdateTime)}: {UpdateTime}, {nameof(OrderDisplayVolume)}: {OrderDisplayVolume:N2}, " +
        $"{nameof(OrderRemainingVolume)}: {OrderRemainingVolume:N2}";

    public override string ToString() => $"{GetType().Name}{{{PublishedOrderToStringMembers}}}";
}
