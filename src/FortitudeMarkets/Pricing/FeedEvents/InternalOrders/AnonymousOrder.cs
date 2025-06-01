using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;

namespace FortitudeMarkets.Pricing.FeedEvents.InternalOrders;

public class AnonymousOrder : ReusableObject<IAnonymousOrder>, IMutableAnonymousOrder
{
    private IInternalPassiveOrder?      asPassiveOrderRef;
    private IExternalCounterPartyOrder? asExternalCpRef;


    public AnonymousOrder() { }

    public AnonymousOrder(OrderGenesisFlags genesisFlags)
    {
        EmptyIgnoreGenesisFlags = genesisFlags;
        GenesisFlags             = genesisFlags;
    }

    public AnonymousOrder
    (int orderId = 0, DateTime createdTime = default, decimal orderDisplayVolume = 0m
      , OrderType orderType = OrderType.None, OrderGenesisFlags genesisFlags = OrderGenesisFlags.None
      , OrderLifeCycleState lifeCycleState = OrderLifeCycleState.None, DateTime? updatedTime = null
      , decimal? remainingVolume = null, uint trackingId = 0)
    {
        OrderId              = orderId;
        CreatedTime          = createdTime;
        OrderType            = orderType;
        GenesisFlags         = genesisFlags;
        OrderLifeCycleState  = lifeCycleState;
        OrderDisplayVolume   = orderDisplayVolume;
        OrderRemainingVolume = remainingVolume ?? orderDisplayVolume;
        UpdateTime           = updatedTime ?? createdTime;
        TrackingId           = trackingId;
    }

    public AnonymousOrder(IAnonymousOrder toClone)
    {
        OrderId                 = toClone.OrderId;
        CreatedTime             = toClone.CreatedTime;
        OrderType               = toClone.OrderType;
        GenesisFlags            = toClone.GenesisFlags;
        EmptyIgnoreGenesisFlags = toClone.EmptyIgnoreGenesisFlags;
        OrderLifeCycleState     = toClone.OrderLifeCycleState;
        OrderDisplayVolume      = toClone.OrderDisplayVolume;
        OrderRemainingVolume    = toClone.OrderRemainingVolume;
        UpdateTime              = toClone.UpdateTime;
        TrackingId              = toClone.TrackingId;
        EmptyIgnoreGenesisFlags = toClone.EmptyIgnoreGenesisFlags;

        if (toClone is AnonymousOrder anonClone)
        {
            InternalOrderInfo             = anonClone.InternalOrderInfo?.Clone();
            ExternalCounterPartyOrderInfo = anonClone.ExternalCounterPartyOrderInfo?.Clone();
        }
        else
        {
            if (toClone.InternalOrderInfo != null) InternalOrderInfo             = CreatedPassiveOrderInfoInstance(toClone.InternalOrderInfo);
            if (toClone.ExternalCounterPartyOrderInfo != null) ExternalCounterPartyOrderInfo = CreatedExternalCounterPartyInfo(toClone.ExternalCounterPartyOrderInfo);
        }
    }


    public int       OrderId   { get; set; }
    public OrderType OrderType { get; set; }

    public OrderGenesisFlags GenesisFlags { get; set; }

    public OrderGenesisFlags EmptyIgnoreGenesisFlags { get; set; } = OrderGenesisFlags.None;

    public uint              TrackingId               { get; set; }

    public OrderLifeCycleState OrderLifeCycleState { get; set; }

    public DateTime CreatedTime        { get; set; }
    public DateTime UpdateTime         { get; set; }
    public decimal  OrderDisplayVolume { get; set; }

    public decimal OrderRemainingVolume { get; set; }

    IAdditionalInternalPassiveOrderInfo? IAnonymousOrder.     InternalOrderInfo             => InternalOrderInfo;
    IAdditionalExternalCounterPartyOrderInfo? IAnonymousOrder.ExternalCounterPartyOrderInfo => ExternalCounterPartyOrderInfo;
    public IMutableAdditionalInternalPassiveOrderInfo?        InternalOrderInfo             { get; set; }
    public IMutableAdditionalExternalCounterPartyOrderInfo?   ExternalCounterPartyOrderInfo { get; set; }

    public virtual bool IsEmpty
    {
        get =>
            OrderId == 0
         && CreatedTime == default
         && OrderType == OrderType.None
         && (GenesisFlags & ~EmptyIgnoreGenesisFlags) == OrderGenesisFlags.None
         && OrderLifeCycleState == OrderLifeCycleState.None
         && UpdateTime == default
         && OrderDisplayVolume == 0m
         && OrderRemainingVolume == 0m
         && TrackingId == 0;
        set
        {
            if (!value) return;

            ResetWithTracking();
        }
    }

    IMutableAnonymousOrder ITrackableReset<IMutableAnonymousOrder>.ResetWithTracking() => ResetWithTracking();

    public virtual AnonymousOrder ResetWithTracking()
    {
        OrderId      = 0;
        CreatedTime  = default;
        UpdateTime   = default;
        OrderType    = OrderType.None;
        GenesisFlags = EmptyIgnoreGenesisFlags;  // Todo create ResetFieldsWithTracking and move this here
        TrackingId   = 0;

        InternalOrderInfo?.ResetWithTracking();
        ExternalCounterPartyOrderInfo?.ResetWithTracking();

        OrderLifeCycleState  = OrderLifeCycleState.None;
        OrderDisplayVolume   = 0m;
        OrderRemainingVolume = 0m;

        return this;
    }

    public override void StateReset()
    {
        ResetWithTracking();
    }

    IMutableAnonymousOrder ICloneable<IMutableAnonymousOrder>.Clone() => Clone();

    IMutableAnonymousOrder IMutableAnonymousOrder.Clone() => Clone();

    public override AnonymousOrder Clone() =>
        Recycler?.Borrow<AnonymousOrder>().CopyFrom(this, CopyMergeFlags.FullReplace) ?? new AnonymousOrder(this);
    
    IReusableObject<IMutableAnonymousOrder> ITransferState<IReusableObject<IMutableAnonymousOrder>>
        .CopyFrom(IReusableObject<IMutableAnonymousOrder> source, CopyMergeFlags copyMergeFlags) => 
        CopyFrom((IAnonymousOrder)source, copyMergeFlags);

    IMutableAnonymousOrder ITransferState<IMutableAnonymousOrder>.CopyFrom(IMutableAnonymousOrder source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom(source, copyMergeFlags);

    public override AnonymousOrder CopyFrom(IAnonymousOrder source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        OrderId      = source.OrderId;
        CreatedTime  = source.CreatedTime;
        OrderType    = source.OrderType;
        GenesisFlags = source.GenesisFlags;
        UpdateTime   = source.UpdateTime;
        TrackingId   = source.TrackingId;

        EmptyIgnoreGenesisFlags = source.EmptyIgnoreGenesisFlags;
        OrderLifeCycleState  = source.OrderLifeCycleState;
        OrderDisplayVolume   = source.OrderDisplayVolume;
        OrderRemainingVolume = source.OrderRemainingVolume;

        
        var isFullReplace = copyMergeFlags.HasFullReplace();
        if (source.InternalOrderInfo != null)
        {
            InternalOrderInfo ??= CreatedPassiveOrderInfoInstance();
            InternalOrderInfo.CopyFrom(source.InternalOrderInfo, copyMergeFlags);
        }
        else if (isFullReplace && InternalOrderInfo != null)
        {
            InternalOrderInfo.IsEmpty = true;
        }

        if (source.ExternalCounterPartyOrderInfo != null)
        {
            ExternalCounterPartyOrderInfo ??= CreatedExternalCounterPartyInfo();
            ExternalCounterPartyOrderInfo.CopyFrom(source.ExternalCounterPartyOrderInfo, copyMergeFlags);
        }
        else if (isFullReplace && ExternalCounterPartyOrderInfo != null)
        {
            ExternalCounterPartyOrderInfo.IsEmpty = true;
        }

        return this;
    }


    public virtual bool AreEquivalent(IAnonymousOrder? other, bool exactTypes = false)
    {
        if (other == null) return false;
        if (exactTypes && other.GetType() != GetType()) return false;

        var orderIdsSame   = OrderId == other.OrderId;
        var createdSame    = CreatedTime == other.CreatedTime;
        var orderTypeSame  = OrderType == other.OrderType;
        var lifecycleSame  = OrderLifeCycleState == other.OrderLifeCycleState;
        var updatedSame    = UpdateTime == other.UpdateTime;
        var volumeSame     = OrderDisplayVolume == other.OrderDisplayVolume;
        var trackingIdSame = TrackingId == other.TrackingId;

        
        var ignoreFlagsDifferences = IExternalCounterPartyOrder.HasExternalCounterPartyOrderInfoFlags |
                                     IInternalPassiveOrder.HasInternalOrderInfo;
        var ignoreFlagsDiffMask = ~ignoreFlagsDifferences;
        var emptyGenesisSame = exactTypes
            ? EmptyIgnoreGenesisFlags == other.EmptyIgnoreGenesisFlags
            : (EmptyIgnoreGenesisFlags & ignoreFlagsDiffMask) == (other.EmptyIgnoreGenesisFlags & ignoreFlagsDiffMask);
        var genesisFlagsSame    = exactTypes
            ? GenesisFlags == other.GenesisFlags
            : (GenesisFlags & ignoreFlagsDiffMask) == (other.GenesisFlags & ignoreFlagsDiffMask);


        var remainingVolumeSame = OrderRemainingVolume == other.OrderRemainingVolume;

        return orderIdsSame && orderTypeSame && genesisFlagsSame && lifecycleSame && createdSame && updatedSame && volumeSame && remainingVolumeSame &&
               trackingIdSame && emptyGenesisSame;
    }

    public virtual IInternalPassiveOrder? ToInternalOrder()
    {
        var hasInfo = GenesisFlags.HasInternalOrderInfo() && InternalOrderInfo != null;
        if (!hasInfo) return null;
        if (this is IInternalPassiveOrder alreadyInternalPassiveOrder) return alreadyInternalPassiveOrder;
        asPassiveOrderRef ??= new InternalPassiveOrder(this, InternalOrderInfo);
        return asPassiveOrderRef;
    }

    public virtual IExternalCounterPartyOrder? ToExternalCounterPartyInfoOrder()
    {
        var hasInfo = GenesisFlags.IsExternalOrder() && GenesisFlags.HasExternalCounterPartyInfo() && ExternalCounterPartyOrderInfo != null;
        if (!hasInfo) return null;
        if (this is IExternalCounterPartyOrder alreadyExternalCounterPartyInfoOrder) return alreadyExternalCounterPartyInfoOrder;
        asExternalCpRef ??= new ExternalCounterPartyOrder(this, ExternalCounterPartyOrderInfo);
        return asExternalCpRef;
    }

    protected virtual IMutableAdditionalInternalPassiveOrderInfo CreatedPassiveOrderInfoInstance(IAdditionalInternalPassiveOrderInfo? optionalCopy = null) =>
        new AdditionalInternalPassiveOrderInfo(optionalCopy);

    protected virtual IMutableAdditionalExternalCounterPartyOrderInfo CreatedExternalCounterPartyInfo(IAdditionalExternalCounterPartyOrderInfo? optionalCopy = null) =>
        new AdditionalExternalCounterPartyInfo(optionalCopy);

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IAnonymousOrder, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = OrderId;
            hashCode = ((int)OrderType * 397) ^ hashCode;
            hashCode = ((int)GenesisFlags * 397) ^ hashCode;
            hashCode = ((int)EmptyIgnoreGenesisFlags * 397) ^ hashCode;
            hashCode = ((int)OrderLifeCycleState * 397) ^ hashCode;
            hashCode = (CreatedTime.GetHashCode() * 397) ^ hashCode;
            hashCode = (UpdateTime.GetHashCode() * 397) ^ hashCode;
            hashCode = (OrderDisplayVolume.GetHashCode() * 397) ^ hashCode;
            hashCode = (OrderRemainingVolume.GetHashCode() * 397) ^ hashCode;
            hashCode = ((int)TrackingId * 397) ^ hashCode;
            return hashCode;
        }
    }

    protected string AnonymousOrderToStringMembers =>
        $"{nameof(OrderId)}: {OrderId}, {nameof(OrderType)}: {OrderType}, {nameof(GenesisFlags)}: {GenesisFlags}, " +
        $"{nameof(EmptyIgnoreGenesisFlags)}: {EmptyIgnoreGenesisFlags}, {nameof(CreatedTime)}: {CreatedTime}, " +
        $"{nameof(OrderLifeCycleState)}: {OrderLifeCycleState}, {nameof(UpdateTime)}: {UpdateTime}, {nameof(OrderDisplayVolume)}: {OrderDisplayVolume:N2}, " +
        $"{nameof(OrderRemainingVolume)}: {OrderRemainingVolume:N2}, {nameof(TrackingId)}: {TrackingId}";

    public override string ToString() => $"{GetType().Name}{{{AnonymousOrderToStringMembers}}}";
}
