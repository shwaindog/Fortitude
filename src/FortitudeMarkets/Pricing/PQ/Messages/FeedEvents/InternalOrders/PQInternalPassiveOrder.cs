using FortitudeCommon.DataStructures.Maps.IdMap;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.FeedEvents.InternalOrders;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;

namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.InternalOrders;

public interface IPQInternalPassiveOrder : IPQAnonymousOrder, IPQAdditionalInternalPassiveOrderInfo, IMutableInternalPassiveOrder
  , IInterfacesComparable<IPQInternalPassiveOrder>
{
    new IEnumerable<PQFieldUpdate> GetDeltaUpdateFields
    (DateTime snapShotTime, PQMessageFlags messageFlags,
        IPQPriceVolumePublicationPrecisionSettings? quotePublicationPrecisionSetting = null);

    new int UpdateField(PQFieldUpdate pqFieldUpdate);

    new bool UpdateFieldString(PQFieldStringUpdate stringUpdate);

    new IEnumerable<PQFieldStringUpdate> GetStringUpdates(DateTime snapShotTime, PQMessageFlags messageFlags);

    new bool HasUpdates { get; set; }

    new IPQInternalPassiveOrder ResetWithTracking();

    new IPQInternalPassiveOrder Clone();
}

public class PQInternalPassiveOrder : ReusableObject<IAnonymousOrder>, IPQInternalPassiveOrder
{
    private const OrderGenesisFlags HasInternalOrderFlags = IInternalPassiveOrder.HasInternalOrderInfo;

    private readonly IPQAnonymousOrder owner;

    private IPQAdditionalInternalPassiveOrderInfo addInternalOrderInfo;

    public PQInternalPassiveOrder()
        : this(new PQAnonymousOrder(IInternalPassiveOrder.HasInternalOrderInfo),
               new PQAdditionalInternalPassiveOrderInfo()) { }

    public PQInternalPassiveOrder(IPQNameIdLookupGenerator nameIdLookupGenerator)
        : this(new PQAnonymousOrder
                   (IInternalPassiveOrder.HasInternalOrderInfo, nameIdLookupGenerator), nameIdLookupGenerator) { }

    public PQInternalPassiveOrder(IAnonymousOrder owner, IPQAdditionalInternalPassiveOrderInfo addInternalOrderInfo)
        : this((owner as IPQAnonymousOrder)?.NameIdLookup ?? new PQNameIdLookupGenerator(PQAnonymousOrder.DefaultOrderStringDictionaryField),
               owner, addInternalOrderInfo) { }

    public PQInternalPassiveOrder
        (IPQNameIdLookupGenerator nameIdLookupGenerator, IAnonymousOrder owner, IPQAdditionalInternalPassiveOrderInfo addInternalOrderInfo)
    {
        if (owner is IPQAnonymousOrder pqOwner)
        {
            var originalGenesisFlagsUpdate = pqOwner.IsGenesisFlagsUpdated;
            this.owner                         =  pqOwner;
            this.owner.GenesisFlags            |= HasInternalOrderFlags;
            this.owner.EmptyIgnoreGenesisFlags |= HasInternalOrderFlags;
            this.owner.IsGenesisFlagsUpdated   =  originalGenesisFlagsUpdate;
        }
        else
        {
            this.owner = new PQAnonymousOrder(owner);
            var originalGenesisFlagsUpdate = owner.GenesisFlags.AnyButNone();
            this.owner.GenesisFlags            |= HasInternalOrderFlags;
            this.owner.EmptyIgnoreGenesisFlags |= HasInternalOrderFlags;
            this.owner.IsGenesisFlagsUpdated   =  originalGenesisFlagsUpdate;
        }

        this.owner.NameIdLookup   = nameIdLookupGenerator;
        this.addInternalOrderInfo = this.owner.InternalOrderInfo ?? addInternalOrderInfo;

        if (!ReferenceEquals(this.owner.InternalOrderInfo, this.addInternalOrderInfo))
        {
            if (this.owner.InternalOrderInfo == null)
            {
                this.owner.InternalOrderInfo = this.addInternalOrderInfo;
            }
            else
            {
                this.owner.InternalOrderInfo.CopyFrom(addInternalOrderInfo, CopyMergeFlags.FullReplace | CopyMergeFlags.AsNew);
            }
        }
    }

    public PQInternalPassiveOrder(IAnonymousOrder? toClone, IPQNameIdLookupGenerator? nameIdLookupGenerator = null)
    {
        if (toClone is PQInternalPassiveOrder pqInternalPassiveOrder)
        {
            owner = pqInternalPassiveOrder.owner.Clone();
            if (nameIdLookupGenerator != null)
            {
                NameIdLookup = nameIdLookupGenerator;
            }
            var originalGenesisFlagsUpdate = pqInternalPassiveOrder.IsGenesisFlagsUpdated;
            owner.GenesisFlags            |= HasInternalOrderFlags;
            owner.EmptyIgnoreGenesisFlags |= HasInternalOrderFlags;
            owner.IsGenesisFlagsUpdated   =  originalGenesisFlagsUpdate;
            addInternalOrderInfo = pqInternalPassiveOrder.InternalOrderInfo ??
                                   new PQAdditionalInternalPassiveOrderInfo(toClone.InternalOrderInfo, NameIdLookup);
        }
        else if (toClone is PQAnonymousOrder pqAnonOrder)
        {
            owner = pqAnonOrder.Clone();
            if (nameIdLookupGenerator != null)
            {
                NameIdLookup = nameIdLookupGenerator;
            }
            var originalGenesisFlagsUpdate = pqAnonOrder.IsGenesisFlagsUpdated;
            owner.GenesisFlags |= HasInternalOrderFlags;
            owner.EmptyIgnoreGenesisFlags |= HasInternalOrderFlags;
            owner.IsGenesisFlagsUpdated = originalGenesisFlagsUpdate;
            addInternalOrderInfo = pqAnonOrder.InternalOrderInfo ?? new PQAdditionalInternalPassiveOrderInfo(toClone.InternalOrderInfo, NameIdLookup);
        }
        else
        {
            owner = new PQAnonymousOrder(toClone);
            if (nameIdLookupGenerator != null)
            {
                NameIdLookup = nameIdLookupGenerator;
            }
            var originalGenesisFlagsUpdate = !(toClone?.IsEmpty ?? true) && (toClone.GenesisFlags.AnyButNone());
            owner.GenesisFlags |= HasInternalOrderFlags;
            owner.EmptyIgnoreGenesisFlags |= HasInternalOrderFlags;
            owner.IsGenesisFlagsUpdated = originalGenesisFlagsUpdate;
            addInternalOrderInfo = owner.InternalOrderInfo ?? new PQAdditionalInternalPassiveOrderInfo(toClone?.InternalOrderInfo, NameIdLookup);
        }

        if (!ReferenceEquals(owner.InternalOrderInfo, addInternalOrderInfo))
        {
            if (owner.InternalOrderInfo == null)
            {
                owner.InternalOrderInfo = addInternalOrderInfo;
            }
            else if (toClone?.InternalOrderInfo != null)
            {
                owner.InternalOrderInfo.CopyFrom(toClone.InternalOrderInfo, CopyMergeFlags.FullReplace | CopyMergeFlags.AsNew);
            }
        }
    }

    public PQAnonymousOrderUpdatedFlags AnonymousOrderUpdatedFlags
    {
        get => owner.AnonymousOrderUpdatedFlags;
        set => owner.AnonymousOrderUpdatedFlags = value;
    }
    
    public  PQAdditionalInternalPassiveOrderInfoUpdatedFlags InternalPassiveOrderInfoUpdatedFlags
    {
        get => addInternalOrderInfo.InternalPassiveOrderInfoUpdatedFlags;
        set => addInternalOrderInfo.InternalPassiveOrderInfoUpdatedFlags = value;
    }

    public int OrderId
    {
        get => owner.OrderId;
        set => owner.OrderId = value;
    }

    public OrderType OrderType
    {
        get => owner.OrderType;
        set => owner.OrderType = value;
    }

    public OrderLifeCycleState OrderLifeCycleState
    {
        get => owner.OrderLifeCycleState;
        set => owner.OrderLifeCycleState = value;
    }

    public OrderGenesisFlags GenesisFlags
    {
        get => owner.GenesisFlags;
        set => owner.GenesisFlags = value;
    }

    public OrderGenesisFlags EmptyIgnoreGenesisFlags
    {
        get => owner.EmptyIgnoreGenesisFlags;
        set => owner.EmptyIgnoreGenesisFlags = value;
    }

    public uint TrackingId
    {
        get => owner.TrackingId;
        set => owner.TrackingId = value;
    }

    public DateTime CreatedTime
    {
        get => owner.CreatedTime;
        set => owner.CreatedTime = value;
    }

    public DateTime UpdateTime
    {
        get => owner.UpdateTime;
        set => owner.UpdateTime = value;
    }

    public decimal OrderDisplayVolume
    {
        get => owner.OrderDisplayVolume;
        set => owner.OrderDisplayVolume = value;
    }

    public decimal OrderRemainingVolume
    {
        get => owner.OrderRemainingVolume;
        set => owner.OrderRemainingVolume = value;
    }

    public uint OrderSequenceId
    {
        get => addInternalOrderInfo.OrderSequenceId;
        set => addInternalOrderInfo.OrderSequenceId = value;
    }

    public uint ParentOrderId
    {
        get => addInternalOrderInfo.ParentOrderId;
        set => addInternalOrderInfo.ParentOrderId = value;
    }

    public uint ClosingOrderId
    {
        get => addInternalOrderInfo.ClosingOrderId;
        set => addInternalOrderInfo.ClosingOrderId = value;
    }

    public decimal ClosingOrderPrice
    {
        get => addInternalOrderInfo.ClosingOrderPrice;
        set => addInternalOrderInfo.ClosingOrderPrice = value;
    }
    public DateTime DecisionCreatedTime
    {
        get => addInternalOrderInfo.DecisionCreatedTime;
        set => addInternalOrderInfo.DecisionCreatedTime = value;
    }
    public DateTime DecisionAmendTime
    {
        get => addInternalOrderInfo.DecisionAmendTime;
        set => addInternalOrderInfo.DecisionAmendTime = value;
    }
    public uint DivisionId
    {
        get => addInternalOrderInfo.DivisionId;
        set => addInternalOrderInfo.DivisionId = value;
    }

    public int DivisionNameId
    {
        get => addInternalOrderInfo.DivisionNameId;
        set => addInternalOrderInfo.DivisionNameId = value;
    }

    public string? DivisionName
    {
        get => addInternalOrderInfo.DivisionName;
        set => addInternalOrderInfo.DivisionName = value;
    }
    public uint DeskId
    {
        get => addInternalOrderInfo.DeskId;
        set => addInternalOrderInfo.DeskId = value;
    }

    public int DeskNameId
    {
        get => addInternalOrderInfo.DeskNameId;
        set => addInternalOrderInfo.DeskNameId = value;
    }

    public string? DeskName
    {
        get => addInternalOrderInfo.DeskName;
        set => addInternalOrderInfo.DeskName = value;
    }

    public uint StrategyId
    {
        get => addInternalOrderInfo.StrategyId;
        set => addInternalOrderInfo.StrategyId = value;
    }

    public int StrategyNameId
    {
        get => addInternalOrderInfo.StrategyNameId;
        set => addInternalOrderInfo.StrategyNameId = value;
    }

    public string? StrategyName
    {
        get => addInternalOrderInfo.StrategyName;
        set => addInternalOrderInfo.StrategyName = value;
    }

    public uint StrategyDecisionId
    {
        get => addInternalOrderInfo.StrategyDecisionId;
        set => addInternalOrderInfo.StrategyDecisionId = value;
    }

    public int StrategyDecisionNameId
    {
        get => addInternalOrderInfo.StrategyDecisionNameId;
        set => addInternalOrderInfo.StrategyDecisionNameId = value;
    }

    public string? StrategyDecisionName
    {
        get => addInternalOrderInfo.StrategyDecisionName;
        set => addInternalOrderInfo.StrategyDecisionName = value;
    }

    public uint PortfolioId
    {
        get => addInternalOrderInfo.PortfolioId;
        set => addInternalOrderInfo.PortfolioId = value;
    }

    public int PortfolioNameId
    {
        get => addInternalOrderInfo.PortfolioNameId;
        set => addInternalOrderInfo.PortfolioNameId = value;
    }

    public string? PortfolioName
    {
        get => addInternalOrderInfo.PortfolioName;
        set => addInternalOrderInfo.PortfolioName = value;
    }

    public uint InternalTraderId
    {
        get => addInternalOrderInfo.InternalTraderId;
        set => addInternalOrderInfo.InternalTraderId = value;
    }

    public int InternalTraderNameId
    {
        get => addInternalOrderInfo.InternalTraderNameId;
        set => addInternalOrderInfo.InternalTraderNameId = value;
    }

    public string? InternalTraderName
    {
        get => addInternalOrderInfo.InternalTraderName;
        set => addInternalOrderInfo.InternalTraderName = value;
    }
    public decimal MarginConsumed
    {
        get => addInternalOrderInfo.MarginConsumed;
        set => addInternalOrderInfo.MarginConsumed = value;
    }

    public bool IsOrderIdUpdated
    {
        get => owner.IsOrderIdUpdated;
        set => owner.IsOrderIdUpdated = value;
    }
    public bool IsGenesisFlagsUpdated
    {
        get => owner.IsGenesisFlagsUpdated;
        set => owner.IsGenesisFlagsUpdated = value;
    }
    public bool IsOrderTypeUpdated
    {
        get => owner.IsOrderTypeUpdated;
        set => owner.IsOrderTypeUpdated = value;
    }
    public bool IsOrderLifecycleStateUpdated
    {
        get => owner.IsOrderLifecycleStateUpdated;
        set => owner.IsOrderLifecycleStateUpdated = value;
    }
    public bool IsCreatedTimeDateUpdated
    {
        get => owner.IsCreatedTimeDateUpdated;
        set => owner.IsCreatedTimeDateUpdated = value;
    }
    public bool IsCreatedTimeSub2MinUpdated
    {
        get => owner.IsCreatedTimeSub2MinUpdated;
        set => owner.IsCreatedTimeSub2MinUpdated = value;
    }
    public bool IsUpdateTimeDateUpdated
    {
        get => owner.IsUpdateTimeDateUpdated;
        set => owner.IsUpdateTimeDateUpdated = value;
    }
    public bool IsUpdateTimeSub2MinUpdated
    {
        get => owner.IsUpdateTimeSub2MinUpdated;
        set => owner.IsUpdateTimeSub2MinUpdated = value;
    }
    public bool IsOrderVolumeUpdated
    {
        get => owner.IsOrderVolumeUpdated;
        set => owner.IsOrderVolumeUpdated = value;
    }
    public bool IsOrderRemainingVolumeUpdated
    {
        get => owner.IsOrderRemainingVolumeUpdated;
        set => owner.IsOrderRemainingVolumeUpdated = value;
    }
    public bool IsTrackingIdUpdated
    {
        get => owner.IsTrackingIdUpdated;
        set => owner.IsTrackingIdUpdated = value;
    }
    public bool IsOrderSequenceIdUpdated
    {
        get => addInternalOrderInfo.IsOrderSequenceIdUpdated;
        set => addInternalOrderInfo.IsOrderSequenceIdUpdated = value;
    }
    public bool IsParentOrderIdUpdated
    {
        get => addInternalOrderInfo.IsParentOrderIdUpdated;
        set => addInternalOrderInfo.IsParentOrderIdUpdated = value;
    }
    public bool IsClosingOrderIdUpdated
    {
        get => addInternalOrderInfo.IsClosingOrderIdUpdated;
        set => addInternalOrderInfo.IsClosingOrderIdUpdated = value;
    }
    public bool IsClosingOrderPriceUpdated
    {
        get => addInternalOrderInfo.IsClosingOrderPriceUpdated;
        set => addInternalOrderInfo.IsClosingOrderPriceUpdated = value;
    }
    public bool IsDivisionIdUpdated
    {
        get => addInternalOrderInfo.IsDivisionIdUpdated;
        set => addInternalOrderInfo.IsDivisionIdUpdated = value;
    }
    public bool IsDivisionNameUpdated
    {
        get => addInternalOrderInfo.IsDivisionNameUpdated;
        set => addInternalOrderInfo.IsDivisionNameUpdated = value;
    }
    public bool IsDeskIdUpdated
    {
        get => addInternalOrderInfo.IsDeskIdUpdated;
        set => addInternalOrderInfo.IsDeskIdUpdated = value;
    }
    public bool IsDeskNameUpdated
    {
        get => addInternalOrderInfo.IsDeskNameUpdated;
        set => addInternalOrderInfo.IsDeskNameUpdated = value;
    }
    public bool IsStrategyIdUpdated
    {
        get => addInternalOrderInfo.IsStrategyIdUpdated;
        set => addInternalOrderInfo.IsStrategyIdUpdated = value;
    }
    public bool IsStrategyNameUpdated
    {
        get => addInternalOrderInfo.IsStrategyNameUpdated;
        set => addInternalOrderInfo.IsStrategyNameUpdated = value;
    }
    public bool IsStrategyDecisionIdUpdated
    {
        get => addInternalOrderInfo.IsStrategyDecisionIdUpdated;
        set => addInternalOrderInfo.IsStrategyDecisionIdUpdated = value;
    }
    public bool IsStrategyDecisionNameUpdated
    {
        get => addInternalOrderInfo.IsStrategyDecisionNameUpdated;
        set => addInternalOrderInfo.IsStrategyDecisionNameUpdated = value;
    }
    public bool IsPortfolioIdUpdated
    {
        get => addInternalOrderInfo.IsPortfolioIdUpdated;
        set => addInternalOrderInfo.IsPortfolioIdUpdated = value;
    }
    public bool IsPortfolioNameUpdated
    {
        get => addInternalOrderInfo.IsPortfolioNameUpdated;
        set => addInternalOrderInfo.IsPortfolioNameUpdated = value;
    }
    public bool IsInternalTraderIdUpdated
    {
        get => addInternalOrderInfo.IsInternalTraderIdUpdated;
        set => addInternalOrderInfo.IsInternalTraderIdUpdated = value;
    }
    public bool IsInternalTraderNameUpdated
    {
        get => addInternalOrderInfo.IsInternalTraderNameUpdated;
        set => addInternalOrderInfo.IsInternalTraderNameUpdated = value;
    }
    public bool IsMarginConsumedUpdated
    {
        get => addInternalOrderInfo.IsMarginConsumedUpdated;
        set => addInternalOrderInfo.IsMarginConsumedUpdated = value;
    }
    public bool IsDecisionCreatedDateUpdated
    {
        get => addInternalOrderInfo.IsDecisionCreatedDateUpdated;
        set => addInternalOrderInfo.IsDecisionCreatedDateUpdated = value;
    }
    public bool IsDecisionCreatedSub2MinTimeUpdated
    {
        get => addInternalOrderInfo.IsDecisionCreatedSub2MinTimeUpdated;
        set => addInternalOrderInfo.IsDecisionCreatedSub2MinTimeUpdated = value;
    }
    public bool IsDecisionAmendDateUpdated
    {
        get => addInternalOrderInfo.IsDecisionAmendDateUpdated;
        set => addInternalOrderInfo.IsDecisionAmendDateUpdated = value;
    }
    public bool IsDecisionAmendSub2MinTimeUpdated
    {
        get => addInternalOrderInfo.IsDecisionAmendSub2MinTimeUpdated;
        set => addInternalOrderInfo.IsDecisionAmendSub2MinTimeUpdated = value;
    }

    INameIdLookup IHasNameIdLookup.NameIdLookup => NameIdLookup;

    public IPQNameIdLookupGenerator NameIdLookup
    {
        get => owner.NameIdLookup;
        set => owner.NameIdLookup = value;
    }

    public uint UpdateSequenceId => owner.UpdateSequenceId;

    public void UpdateStarted(uint updateSequenceId) { }

    public void UpdateComplete(uint updateSequenceId = 0)
    {
        owner.UpdateComplete(updateSequenceId);
    }

    public bool HasUpdates
    {
        get => owner.HasUpdates;
        set => owner.HasUpdates = value;
    }


    public bool IsEmpty
    {
        get => owner.IsEmpty && addInternalOrderInfo.IsEmpty;
        set
        {
            owner.IsEmpty                = value;
            addInternalOrderInfo.IsEmpty = value;
        }
    }

    IMutableAnonymousOrder ITrackableReset<IMutableAnonymousOrder>.ResetWithTracking() => ResetWithTracking();

    IPQAnonymousOrder ITrackableReset<IPQAnonymousOrder>.ResetWithTracking() => ResetWithTracking();

    IPQAnonymousOrder IPQAnonymousOrder.ResetWithTracking() => ResetWithTracking();

    IMutableAdditionalInternalPassiveOrderInfo ITrackableReset<IMutableAdditionalInternalPassiveOrderInfo>.ResetWithTracking() => ResetWithTracking();

    IMutableInternalPassiveOrder ITrackableReset<IMutableInternalPassiveOrder>.ResetWithTracking() => ResetWithTracking();

    IMutableInternalPassiveOrder IMutableInternalPassiveOrder.ResetWithTracking() => ResetWithTracking();

    IPQInternalPassiveOrder IPQInternalPassiveOrder.ResetWithTracking() => ResetWithTracking();

    public PQInternalPassiveOrder ResetWithTracking()
    {
        owner.ResetWithTracking();

        return this;
    }

    public override void StateReset()
    {
        owner.StateReset();
    }

    IMutableAdditionalInternalPassiveOrderInfo? IMutableAnonymousOrder.InternalOrderInfo
    {
        get => InternalOrderInfo;
        set => InternalOrderInfo = (IPQAdditionalInternalPassiveOrderInfo?)value;
    }

    IAdditionalInternalPassiveOrderInfo IAnonymousOrder.InternalOrderInfo => this;

    IMutableAdditionalExternalCounterPartyOrderInfo? IMutableAnonymousOrder.ExternalCounterPartyOrderInfo
    {
        get => ExternalCounterPartyOrderInfo;
        set => ExternalCounterPartyOrderInfo = (IPQAdditionalExternalCounterPartyOrderInfo?)value;
    }

    IAdditionalExternalCounterPartyOrderInfo? IAnonymousOrder.ExternalCounterPartyOrderInfo => owner.ExternalCounterPartyOrderInfo;

    public IPQAdditionalInternalPassiveOrderInfo? InternalOrderInfo
    {
        get => owner.InternalOrderInfo;
        set
        {
            if (ReferenceEquals(value, addInternalOrderInfo)) return;
            if (value != null)
            {
                owner.InternalOrderInfo = value;
                addInternalOrderInfo    = value;
            }
            else
            {
                addInternalOrderInfo.IsEmpty = true;
            }
        }
    }
    public IPQAdditionalExternalCounterPartyOrderInfo? ExternalCounterPartyOrderInfo
    {
        get => owner.ExternalCounterPartyOrderInfo;
        set => owner.ExternalCounterPartyOrderInfo = value;
    }

    public IInternalPassiveOrder ToInternalOrder() => this;

    public IExternalCounterPartyOrder? ToExternalCounterPartyInfoOrder() => owner.ToExternalCounterPartyInfoOrder();

    public virtual bool UpdateFieldString(PQFieldStringUpdate stringUpdate) => owner.UpdateFieldString(stringUpdate);

    public virtual IEnumerable<PQFieldStringUpdate> GetStringUpdates
        (DateTime snapShotTime, PQMessageFlags messageFlags) =>
        owner.GetStringUpdates(snapShotTime, messageFlags);

    public virtual IEnumerable<PQFieldUpdate> GetDeltaUpdateFields
        (DateTime snapShotTime, PQMessageFlags messageFlags, IPQPriceVolumePublicationPrecisionSettings? quotePublicationPrecisionSetting) =>
        owner.GetDeltaUpdateFields(snapShotTime, messageFlags, quotePublicationPrecisionSetting);


    public virtual int UpdateField(PQFieldUpdate pqFieldUpdate) => owner.UpdateField(pqFieldUpdate);


    IInternalPassiveOrder ICloneable<IInternalPassiveOrder>.Clone() => Clone();

    IInternalPassiveOrder IInternalPassiveOrder.Clone() => Clone();

    IMutableAdditionalInternalPassiveOrderInfo ICloneable<IMutableAdditionalInternalPassiveOrderInfo>.Clone() => Clone();

    IMutableAdditionalInternalPassiveOrderInfo IMutableAdditionalInternalPassiveOrderInfo.Clone() => Clone();

    IMutableInternalPassiveOrder ICloneable<IMutableInternalPassiveOrder>.Clone() => Clone();

    IMutableInternalPassiveOrder IMutableInternalPassiveOrder.Clone() => Clone();

    IAdditionalInternalPassiveOrderInfo ICloneable<IAdditionalInternalPassiveOrderInfo>.Clone() => Clone();

    IMutableAnonymousOrder ICloneable<IMutableAnonymousOrder>.Clone() => Clone();

    IMutableAnonymousOrder IMutableAnonymousOrder.Clone() => Clone();

    IPQAnonymousOrder ICloneable<IPQAnonymousOrder>.Clone() => Clone();

    IPQAnonymousOrder IPQAnonymousOrder.Clone() => Clone();

    IPQAdditionalInternalPassiveOrderInfo ICloneable<IPQAdditionalInternalPassiveOrderInfo>.Clone() => Clone();

    IPQAdditionalInternalPassiveOrderInfo IPQAdditionalInternalPassiveOrderInfo.Clone() => Clone();

    IPQInternalPassiveOrder IPQInternalPassiveOrder.Clone() => Clone();

    public override PQInternalPassiveOrder Clone() =>
        Recycler?.Borrow<PQInternalPassiveOrder>().CopyFrom(this, CopyMergeFlags.FullReplace) ??
        new PQInternalPassiveOrder(owner.Clone(), addInternalOrderInfo.Clone());

    IMutableAdditionalInternalPassiveOrderInfo ITransferState<IMutableAdditionalInternalPassiveOrderInfo>.CopyFrom
        (IMutableAdditionalInternalPassiveOrderInfo source, CopyMergeFlags copyMergeFlags) =>
        TryCopyAdditionalInternalPassiveOrderInfo(source, copyMergeFlags);

    IReusableObject<IAdditionalInternalPassiveOrderInfo> ITransferState<IReusableObject<IAdditionalInternalPassiveOrderInfo>>.CopyFrom
        (IReusableObject<IAdditionalInternalPassiveOrderInfo> source, CopyMergeFlags copyMergeFlags) =>
        TryCopyAdditionalInternalPassiveOrderInfo(source as IAdditionalInternalPassiveOrderInfo, copyMergeFlags);


    IAdditionalInternalPassiveOrderInfo ITransferState<IAdditionalInternalPassiveOrderInfo>.CopyFrom
        (IAdditionalInternalPassiveOrderInfo source, CopyMergeFlags copyMergeFlags) =>
        TryCopyAdditionalInternalPassiveOrderInfo(source, copyMergeFlags);

    protected virtual PQInternalPassiveOrder TryCopyAdditionalInternalPassiveOrderInfo
        (IAdditionalInternalPassiveOrderInfo? source, CopyMergeFlags copyMergeFlags)
    {
        if (source is IAnonymousOrder anonymousOrder)
        {
            return CopyFrom(anonymousOrder, copyMergeFlags);
        }
        if (source != null) addInternalOrderInfo.CopyFrom(source, copyMergeFlags);
        return this;
    }

    IMutableAnonymousOrder ITransferState<IMutableAnonymousOrder>.CopyFrom(IMutableAnonymousOrder source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom(source, copyMergeFlags);

    IPQAnonymousOrder ITransferState<IPQAnonymousOrder>.CopyFrom(IPQAnonymousOrder source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom(source, copyMergeFlags);

    IReusableObject<IPQAnonymousOrder> ITransferState<IReusableObject<IPQAnonymousOrder>>.CopyFrom
        (IReusableObject<IPQAnonymousOrder> source, CopyMergeFlags copyMergeFlags) => CopyFrom((IAnonymousOrder)source, copyMergeFlags);

    IReusableObject<IMutableAnonymousOrder> ITransferState<IReusableObject<IMutableAnonymousOrder>>.CopyFrom
        (IReusableObject<IMutableAnonymousOrder> source, CopyMergeFlags copyMergeFlags) => CopyFrom((IAnonymousOrder)source, copyMergeFlags);

    public override PQInternalPassiveOrder CopyFrom(IAnonymousOrder source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        owner.CopyFrom(source, copyMergeFlags);
        return this;
    }

    bool IInterfacesComparable<IAdditionalInternalPassiveOrderInfo>.AreEquivalent(IAdditionalInternalPassiveOrderInfo? other, bool exactTypes) =>
        AreEquivalent(other as IAnonymousOrder, exactTypes);

    bool IInterfacesComparable<IPQInternalPassiveOrder>.AreEquivalent(IPQInternalPassiveOrder? other, bool exactTypes) =>
        AreEquivalent(other, exactTypes);

    public bool AreEquivalent(IAnonymousOrder? other, bool exactTypes = false)
    {
        if (other is not IInternalPassiveOrder ipqPassiveOrder) return false;

        bool anonOrderSame;
        bool  addPassiveOrderSame;
        if (ipqPassiveOrder is PQInternalPassiveOrder pqInternalPassive)
        {
            anonOrderSame       = owner.AreEquivalent(pqInternalPassive.owner, exactTypes);
            addPassiveOrderSame = addInternalOrderInfo.AreEquivalent(pqInternalPassive.addInternalOrderInfo, exactTypes);
        }
        else
        {
            if (exactTypes) return false;
            addPassiveOrderSame = addInternalOrderInfo.AreEquivalent(ipqPassiveOrder);
            anonOrderSame       = owner.AreEquivalent(other);
        }

        var allAreSame = anonOrderSame && addPassiveOrderSame;

        return allAreSame;
    }

    public void SetFlagsSame(IAdditionalInternalPassiveOrderInfo? toCopyFlags)
    {
        addInternalOrderInfo.SetFlagsSame(toCopyFlags);
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IAnonymousOrder, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = owner.GetHashCode();
            hashCode = (addInternalOrderInfo.GetHashCode() * 397) ^ hashCode;
            return hashCode;
        }
    }

    protected string PQInternalPassiveOrderToStringMembers =>
        $"{nameof(OrderId)}: {OrderId}, {nameof(OrderType)}: {OrderType}, {nameof(GenesisFlags)}: {GenesisFlags}, " +
        $"{nameof(EmptyIgnoreGenesisFlags)}: {EmptyIgnoreGenesisFlags}, {nameof(CreatedTime)}: {CreatedTime}, " +
        $"{nameof(OrderLifeCycleState)}: {OrderLifeCycleState}, {nameof(UpdateTime)}: {UpdateTime}, {nameof(OrderDisplayVolume)}: {OrderDisplayVolume:N2}, " +
        $"{nameof(OrderRemainingVolume)}: {OrderRemainingVolume:N2}, {nameof(TrackingId)}: {TrackingId}, " +
        $"{nameof(OrderSequenceId)}: {OrderSequenceId}, {nameof(ParentOrderId)}: {ParentOrderId}, {nameof(ClosingOrderId)}: {ClosingOrderId}, " +
        $"{nameof(ClosingOrderPrice)}: {ClosingOrderPrice}, {nameof(DecisionCreatedTime)}: {DecisionCreatedTime}, " +
        $"{nameof(DecisionAmendTime)}: {DecisionAmendTime}, {nameof(DivisionId)}: {DivisionId}, {nameof(DivisionName)}: {DivisionName}, " +
        $"{nameof(DeskId)}: {DeskId}, {nameof(DeskName)}: {DeskName}, {nameof(StrategyId)}: {StrategyId}, {nameof(StrategyName)}: {StrategyName}, " +
        $"{nameof(StrategyDecisionId)}: {StrategyDecisionId}, {nameof(StrategyDecisionName)}: {StrategyDecisionName}, {nameof(PortfolioId)}: {PortfolioId}, " +
        $"{nameof(PortfolioName)}: {PortfolioName}, {nameof(InternalTraderId)}: {InternalTraderId}, {nameof(InternalTraderName)}: {InternalTraderName}, " +
        $"{nameof(MarginConsumed)}: {MarginConsumed}";

    public override string ToString() => $"{GetType().Name}{{{PQInternalPassiveOrderToStringMembers}}}";
}
