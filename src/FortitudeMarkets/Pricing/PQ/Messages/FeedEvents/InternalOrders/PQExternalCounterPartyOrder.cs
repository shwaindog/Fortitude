using System.Diagnostics;
using FortitudeCommon.DataStructures.Maps.IdMap;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.FeedEvents.InternalOrders;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;

namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.InternalOrders;

public interface IPQExternalCounterPartyOrder : IPQAnonymousOrder, IPQAdditionalExternalCounterPartyOrderInfo, IMutableExternalCounterPartyOrder
  , ITransferState<IPQExternalCounterPartyOrder>, IInterfacesComparable<IPQExternalCounterPartyOrder>
{
    new IEnumerable<PQFieldUpdate> GetDeltaUpdateFields
    (DateTime snapShotTime, StorageFlags messageFlags,
        IPQPriceVolumePublicationPrecisionSettings? quotePublicationPrecisionSetting = null);

    new int UpdateField(PQFieldUpdate pqFieldUpdate);

    new bool UpdateFieldString(PQFieldStringUpdate stringUpdate);

    new IEnumerable<PQFieldStringUpdate> GetStringUpdates(DateTime snapShotTime, StorageFlags messageFlags);

    new bool AreEquivalent(IAnonymousOrder other, bool exactTypes = false);

    new bool HasUpdates { get; set; }

    new IPQExternalCounterPartyOrder Clone();
}

public class PQExternalCounterPartyOrder : ReusableObject<IAnonymousOrder>, IPQExternalCounterPartyOrder
{
    private const    OrderGenesisFlags HasCpFlags = IExternalCounterPartyOrder.HasExternalCounterPartyOrderInfoFlags;

    private readonly IPQAnonymousOrder owner;

    private IPQAdditionalExternalCounterPartyOrderInfo addExternalCpOrderInfo;

    public PQExternalCounterPartyOrder() :
        this(new PQAnonymousOrder(IExternalCounterPartyOrder.HasExternalCounterPartyOrderInfoFlags)
           , new PQAdditionalExternalCounterPartyInfo()) { }

    public PQExternalCounterPartyOrder(IPQNameIdLookupGenerator nameIdLookupGenerator)
        : this(new PQAnonymousOrder
                   (genesisFlags: IExternalCounterPartyOrder.HasExternalCounterPartyOrderInfoFlags, nameIdLookupGenerator)
             , nameIdLookupGenerator) { }

    public PQExternalCounterPartyOrder(IAnonymousOrder owner, IPQAdditionalExternalCounterPartyOrderInfo addExternalCpOrderInfo)
    : this((owner as IPQAnonymousOrder)?.NameIdLookup ?? new PQNameIdLookupGenerator(PQAnonymousOrder.DefaultOrderStringDictionaryField),
           owner, addExternalCpOrderInfo)
    {
    }

    public PQExternalCounterPartyOrder
    (IPQNameIdLookupGenerator nameIdLookupGenerator, IAnonymousOrder owner
      , IPQAdditionalExternalCounterPartyOrderInfo addExternalCpOrderInfo)
    {
        if (owner is IPQAnonymousOrder pqOwner)
        {
            var originalGenesisFlagsUpdate = pqOwner.IsGenesisFlagsUpdated;
            this.owner                         =  pqOwner;
            this.owner.GenesisFlags            |= HasCpFlags;
            this.owner.EmptyIgnoreGenesisFlags |= HasCpFlags;
            this.owner.IsGenesisFlagsUpdated   =  originalGenesisFlagsUpdate;
        }
        else
        {
            this.owner = new PQAnonymousOrder(owner);
            var originalGenesisFlagsUpdate = owner.GenesisFlags.AnyButNone();
            this.owner.GenesisFlags            |= HasCpFlags;
            this.owner.EmptyIgnoreGenesisFlags |= HasCpFlags;
            this.owner.IsGenesisFlagsUpdated   =  originalGenesisFlagsUpdate;
        }

        this.owner.NameIdLookup = nameIdLookupGenerator;
        this.addExternalCpOrderInfo
            = this.owner.ExternalCounterPartyOrderInfo ?? addExternalCpOrderInfo;

        if (!ReferenceEquals(this.owner.ExternalCounterPartyOrderInfo, this.addExternalCpOrderInfo))
        {
            if (this.owner.ExternalCounterPartyOrderInfo == null)
            {
                this.owner.ExternalCounterPartyOrderInfo = this.addExternalCpOrderInfo;
            }
            else 
            {
                this.owner.ExternalCounterPartyOrderInfo.CopyFrom(addExternalCpOrderInfo, CopyMergeFlags.FullReplace | CopyMergeFlags.AsNew);
            }
        }
    }

    public PQExternalCounterPartyOrder(IAnonymousOrder? toClone, IPQNameIdLookupGenerator? nameIdLookupGenerator = null)
    {
        if (toClone is PQExternalCounterPartyOrder pqExternalCounterPartyOrder)
        {
            owner = new PQAnonymousOrder(pqExternalCounterPartyOrder.owner, nameIdLookupGenerator);
            var originalGenesisFlagsUpdate = pqExternalCounterPartyOrder.IsGenesisFlagsUpdated;
            owner.GenesisFlags            |= HasCpFlags;
            owner.EmptyIgnoreGenesisFlags |= HasCpFlags;
            owner.IsGenesisFlagsUpdated   =  originalGenesisFlagsUpdate;
            addExternalCpOrderInfo = owner.ExternalCounterPartyOrderInfo ??
                                     new PQAdditionalExternalCounterPartyInfo(toClone.ExternalCounterPartyOrderInfo, NameIdLookup);
        } else if (toClone is PQAnonymousOrder pqAnonymous)
        {
            owner = new PQAnonymousOrder(pqAnonymous, nameIdLookupGenerator);
            var originalGenesisFlagsUpdate = pqAnonymous.IsGenesisFlagsUpdated;
            owner.GenesisFlags            |= HasCpFlags;
            owner.EmptyIgnoreGenesisFlags |= HasCpFlags;
            owner.IsGenesisFlagsUpdated   =  originalGenesisFlagsUpdate;
            addExternalCpOrderInfo = owner.ExternalCounterPartyOrderInfo ??
                                     new PQAdditionalExternalCounterPartyInfo(toClone.ExternalCounterPartyOrderInfo, NameIdLookup);
        }
        else
        {
            owner = new PQAnonymousOrder(toClone, nameIdLookupGenerator);
            var originalGenesisFlagsUpdate = !(toClone?.IsEmpty ?? true) && (toClone?.GenesisFlags.AnyButNone() ?? false);
            owner.GenesisFlags |= IExternalCounterPartyOrder.HasExternalCounterPartyOrderInfoFlags;
            owner.EmptyIgnoreGenesisFlags |= IExternalCounterPartyOrder.HasExternalCounterPartyOrderInfoFlags;
            owner.IsGenesisFlagsUpdated   =  originalGenesisFlagsUpdate;
            addExternalCpOrderInfo = owner.ExternalCounterPartyOrderInfo ??
                                     new PQAdditionalExternalCounterPartyInfo(toClone?.ExternalCounterPartyOrderInfo, NameIdLookup);
        }

        if (!ReferenceEquals(owner.ExternalCounterPartyOrderInfo, addExternalCpOrderInfo))
        {
            if (owner.ExternalCounterPartyOrderInfo == null)
            {
                owner.ExternalCounterPartyOrderInfo = addExternalCpOrderInfo;
            }
            else if (toClone?.ExternalCounterPartyOrderInfo != null)
            {
                owner.ExternalCounterPartyOrderInfo.CopyFrom(toClone.ExternalCounterPartyOrderInfo
                                                           , CopyMergeFlags.FullReplace | CopyMergeFlags.AsNew);
            }
        }
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

    public int ExternalCounterPartyId
    {
        get => addExternalCpOrderInfo.ExternalCounterPartyId;
        set => addExternalCpOrderInfo.ExternalCounterPartyId = value;
    }

    public string? ExternalCounterPartyName
    {
        get => addExternalCpOrderInfo.ExternalCounterPartyName;
        set => addExternalCpOrderInfo.ExternalCounterPartyName = value;
    }

    public int ExternalTraderId
    {
        get => addExternalCpOrderInfo.ExternalTraderId;
        set => addExternalCpOrderInfo.ExternalTraderId = value;
    }

    public string? ExternalTraderName
    {
        get => addExternalCpOrderInfo.ExternalTraderName;
        set => addExternalCpOrderInfo.ExternalTraderName = value;
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

    public int ExternalCounterPartyNameId
    {
        get => addExternalCpOrderInfo.ExternalCounterPartyNameId;
        set => addExternalCpOrderInfo.ExternalCounterPartyNameId = value;
    }

    public int ExternalTraderNameId
    {
        get => addExternalCpOrderInfo.ExternalTraderNameId;
        set => addExternalCpOrderInfo.ExternalTraderNameId = value;
    }

    public bool IsExternalCounterPartyNameUpdated
    {
        get => addExternalCpOrderInfo.IsExternalCounterPartyNameUpdated;
        set => addExternalCpOrderInfo.IsExternalCounterPartyNameUpdated = value;
    }

    public bool IsExternalTraderNameUpdated
    {
        get => addExternalCpOrderInfo.IsExternalTraderNameUpdated;
        set => addExternalCpOrderInfo.IsExternalTraderNameUpdated = value;
    }

    public bool IsExternalCounterPartyIdUpdated
    {
        get => addExternalCpOrderInfo.IsExternalCounterPartyIdUpdated;
        set => addExternalCpOrderInfo.IsExternalCounterPartyIdUpdated = value;
    }

    public bool IsExternalTraderIdUpdated
    {
        get => addExternalCpOrderInfo.IsExternalTraderIdUpdated;
        set => addExternalCpOrderInfo.IsExternalTraderIdUpdated = value;
    }

    public uint UpdateCount => owner.UpdateCount;

    public void UpdateComplete()
    {
        owner.UpdateComplete();
    }

    public bool HasUpdates
    {
        get => owner.HasUpdates;
        set => owner.HasUpdates = value;
    }

    INameIdLookup IHasNameIdLookup.NameIdLookup => NameIdLookup;

    public IPQNameIdLookupGenerator NameIdLookup
    {
        get => owner.NameIdLookup;
        set => owner.NameIdLookup = value;
    }

    IAdditionalInternalPassiveOrderInfo? IAnonymousOrder.InternalOrderInfo => owner.InternalOrderInfo;

    IAdditionalExternalCounterPartyOrderInfo IAnonymousOrder.ExternalCounterPartyOrderInfo => this;

    IMutableAdditionalInternalPassiveOrderInfo? IMutableAnonymousOrder.InternalOrderInfo
    {
        get => InternalOrderInfo;
        set => InternalOrderInfo = (IPQAdditionalInternalPassiveOrderInfo?)value;
    }

    public IPQAdditionalInternalPassiveOrderInfo? InternalOrderInfo
    {
        get => owner.InternalOrderInfo;
        set => owner.InternalOrderInfo = value;
    }

    IMutableAdditionalExternalCounterPartyOrderInfo? IMutableAnonymousOrder.ExternalCounterPartyOrderInfo
    {
        get => ExternalCounterPartyOrderInfo;
        set => ExternalCounterPartyOrderInfo = (IPQAdditionalExternalCounterPartyOrderInfo?)value;
    }

    public IPQAdditionalExternalCounterPartyOrderInfo? ExternalCounterPartyOrderInfo
    {
        get => owner.ExternalCounterPartyOrderInfo;
        set
        {
            if (ReferenceEquals(value, addExternalCpOrderInfo)) return;
            if (value != null)
            {
                owner.ExternalCounterPartyOrderInfo = value;
                addExternalCpOrderInfo              = value;
            }
            else
            {
                addExternalCpOrderInfo.IsEmpty = true;
            }
        }
    }

    public bool IsEmpty
    {
        get => owner.IsEmpty && addExternalCpOrderInfo.IsEmpty;
        set
        {
            owner.IsEmpty                  = value;
            addExternalCpOrderInfo.IsEmpty = value;
        }
    }

    public override void StateReset()
    {
        owner.StateReset();
    }

    public IInternalPassiveOrder? ToInternalOrder() => owner.ToInternalOrder();

    IExternalCounterPartyOrder IAnonymousOrder.ToExternalCounterPartyInfoOrder() => ToExternalCounterPartyInfoOrder();

    public IPQExternalCounterPartyOrder ToExternalCounterPartyInfoOrder() => this;

    int IPQSupportsNumberPrecisionFieldUpdates<IPQAnonymousOrder>.UpdateField(PQFieldUpdate fieldUpdate) => owner.UpdateField(fieldUpdate);

    public virtual bool UpdateFieldString(PQFieldStringUpdate stringUpdate) => owner.UpdateFieldString(stringUpdate);

    public virtual IEnumerable<PQFieldStringUpdate> GetStringUpdates
        (DateTime snapShotTime, StorageFlags messageFlags) =>
        owner.GetStringUpdates(snapShotTime, messageFlags);

    IEnumerable<PQFieldUpdate> IPQSupportsNumberPrecisionFieldUpdates<IPQAdditionalExternalCounterPartyOrderInfo>.GetDeltaUpdateFields
        (DateTime snapShotTime, StorageFlags messageFlags, IPQPriceVolumePublicationPrecisionSettings? quotePublicationPrecisionSettings) =>
        GetDeltaUpdateFields(snapShotTime, messageFlags, quotePublicationPrecisionSettings);

    IEnumerable<PQFieldUpdate> IPQSupportsNumberPrecisionFieldUpdates<IPQAnonymousOrder>.GetDeltaUpdateFields
        (DateTime snapShotTime, StorageFlags messageFlags, IPQPriceVolumePublicationPrecisionSettings? quotePublicationPrecisionSettings) =>
        owner.GetDeltaUpdateFields(snapShotTime, messageFlags, quotePublicationPrecisionSettings);

    public virtual IEnumerable<PQFieldUpdate> GetDeltaUpdateFields
        (DateTime snapShotTime, StorageFlags messageFlags, IPQPriceVolumePublicationPrecisionSettings? quotePublicationPrecisionSetting) =>
        owner.GetDeltaUpdateFields(snapShotTime, messageFlags, quotePublicationPrecisionSetting);

    int IPQSupportsNumberPrecisionFieldUpdates<IPQAdditionalExternalCounterPartyOrderInfo>.UpdateField(PQFieldUpdate fieldUpdate) =>
        UpdateField(fieldUpdate);

    public virtual int UpdateField(PQFieldUpdate pqFieldUpdate) => owner.UpdateField(pqFieldUpdate);

    IAnonymousOrder ICloneable<IAnonymousOrder>.Clone() => Clone();

    IAdditionalExternalCounterPartyOrderInfo ICloneable<IAdditionalExternalCounterPartyOrderInfo>.Clone() => Clone();

    IMutableAdditionalExternalCounterPartyOrderInfo ICloneable<IMutableAdditionalExternalCounterPartyOrderInfo>.Clone() => Clone();

    IMutableAdditionalExternalCounterPartyOrderInfo IMutableAdditionalExternalCounterPartyOrderInfo.Clone() => Clone();

    IMutableAnonymousOrder ICloneable<IMutableAnonymousOrder>.Clone() => Clone();

    IMutableAnonymousOrder IMutableAnonymousOrder.Clone() => Clone();

    IPQAnonymousOrder ICloneable<IPQAnonymousOrder>.Clone() => Clone();

    IPQAnonymousOrder IPQAnonymousOrder.Clone() => Clone();

    IPQAdditionalExternalCounterPartyOrderInfo IPQAdditionalExternalCounterPartyOrderInfo.Clone() => Clone();

    IPQExternalCounterPartyOrder IPQExternalCounterPartyOrder.Clone() => Clone();

    IExternalCounterPartyOrder ICloneable<IExternalCounterPartyOrder>.Clone() => Clone();

    IExternalCounterPartyOrder IExternalCounterPartyOrder.Clone() => Clone();

    IMutableExternalCounterPartyOrder ICloneable<IMutableExternalCounterPartyOrder>.Clone() => Clone();

    IMutableExternalCounterPartyOrder IMutableExternalCounterPartyOrder.Clone() => Clone();

    public override PQExternalCounterPartyOrder Clone() =>
        Recycler?.Borrow<PQExternalCounterPartyOrder>().CopyFrom(this, CopyMergeFlags.FullReplace) ??
        new PQExternalCounterPartyOrder(owner.Clone(), addExternalCpOrderInfo.Clone());


    IReusableObject<IAdditionalExternalCounterPartyOrderInfo> ITransferState<IReusableObject<IAdditionalExternalCounterPartyOrderInfo>>.CopyFrom
        (IReusableObject<IAdditionalExternalCounterPartyOrderInfo> source, CopyMergeFlags copyMergeFlags) =>
        TryCopyAdditionalCounterPartyOrderInfo(source as IAdditionalExternalCounterPartyOrderInfo, copyMergeFlags);

    IAdditionalExternalCounterPartyOrderInfo ITransferState<IAdditionalExternalCounterPartyOrderInfo>.CopyFrom
        (IAdditionalExternalCounterPartyOrderInfo source, CopyMergeFlags copyMergeFlags) =>
        TryCopyAdditionalCounterPartyOrderInfo(source, copyMergeFlags);

    IPQAdditionalExternalCounterPartyOrderInfo ITransferState<IPQAdditionalExternalCounterPartyOrderInfo>.CopyFrom
        (IPQAdditionalExternalCounterPartyOrderInfo source, CopyMergeFlags copyMergeFlags) =>
        TryCopyAdditionalCounterPartyOrderInfo(source, copyMergeFlags);

    IMutableAdditionalExternalCounterPartyOrderInfo ITransferState<IMutableAdditionalExternalCounterPartyOrderInfo>.CopyFrom
        (IMutableAdditionalExternalCounterPartyOrderInfo source, CopyMergeFlags copyMergeFlags) =>
        TryCopyAdditionalCounterPartyOrderInfo(source, copyMergeFlags);

    protected virtual PQExternalCounterPartyOrder TryCopyAdditionalCounterPartyOrderInfo
        (IAdditionalExternalCounterPartyOrderInfo? source, CopyMergeFlags copyMergeFlags)
    {
        if (source is IAnonymousOrder anonymousOrder)
        {
            return CopyFrom(anonymousOrder, copyMergeFlags);
        }
        if (source != null) addExternalCpOrderInfo.CopyFrom(source, copyMergeFlags);
        return this;
    }

    IPQAnonymousOrder ITransferState<IPQAnonymousOrder>.CopyFrom(IPQAnonymousOrder source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom(source, copyMergeFlags);

    IMutableAnonymousOrder ITransferState<IMutableAnonymousOrder>.CopyFrom
        (IMutableAnonymousOrder source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom(source, copyMergeFlags);

    IPQExternalCounterPartyOrder ITransferState<IPQExternalCounterPartyOrder>.CopyFrom
        (IPQExternalCounterPartyOrder source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom(source, copyMergeFlags);

    IMutableExternalCounterPartyOrder ITransferState<IMutableExternalCounterPartyOrder>.CopyFrom
        (IMutableExternalCounterPartyOrder source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom(source, copyMergeFlags);

    public override PQExternalCounterPartyOrder CopyFrom(IAnonymousOrder source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        owner.CopyFrom(source, copyMergeFlags);
        return this;
    }

    bool IInterfacesComparable<IAdditionalExternalCounterPartyOrderInfo>.AreEquivalent
        (IAdditionalExternalCounterPartyOrderInfo? other, bool exactTypes) =>
        AreEquivalent(other as IAnonymousOrder, exactTypes);

    bool IInterfacesComparable<IPQExternalCounterPartyOrder>.AreEquivalent(IPQExternalCounterPartyOrder? other, bool exactTypes) =>
        AreEquivalent(other, exactTypes);


    public bool AreEquivalent(IAnonymousOrder? other, bool exactTypes = false)
    {
        if (other is not IExternalCounterPartyOrder ipqExtCpOrder) return false;
        var anonOrderSame     = true;
        var addExtCpOrderSame = true;
        if (ipqExtCpOrder is PQExternalCounterPartyOrder pqExternalCounterPartyOrder)
        {
            anonOrderSame     = owner.AreEquivalent(pqExternalCounterPartyOrder.owner, exactTypes);
            addExtCpOrderSame = addExternalCpOrderInfo.AreEquivalent(pqExternalCounterPartyOrder.addExternalCpOrderInfo, exactTypes);
        }
        else
        {
            if (exactTypes) return false;
            anonOrderSame     = owner.AreEquivalent(other, false);
            addExtCpOrderSame = addExternalCpOrderInfo.AreEquivalent(ipqExtCpOrder, false);
        }

        var allAreSame = anonOrderSame && addExtCpOrderSame;

        return allAreSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IAnonymousOrder, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = owner.GetHashCode();
            hashCode = (addExternalCpOrderInfo.GetHashCode() * 397) ^ hashCode;
            return hashCode;
        }
    }

    protected string PQExternalCounterPartyOrderToStringMembers =>
        $"{nameof(OrderId)}: {OrderId}, {nameof(OrderType)}: {OrderType}, {nameof(GenesisFlags)}: {GenesisFlags}, " +
        $"{nameof(EmptyIgnoreGenesisFlags)}: {EmptyIgnoreGenesisFlags}, {nameof(CreatedTime)}: {CreatedTime}, " +
        $"{nameof(OrderLifeCycleState)}: {OrderLifeCycleState}, {nameof(UpdateTime)}: {UpdateTime}, {nameof(OrderDisplayVolume)}: {OrderDisplayVolume:N2}, " +
        $"{nameof(OrderRemainingVolume)}: {OrderRemainingVolume:N2}, {nameof(TrackingId)}: {TrackingId}, " +
        $"{nameof(ExternalCounterPartyId)}: {ExternalCounterPartyId}, {nameof(ExternalCounterPartyName)}: {ExternalCounterPartyName}, " +
        $"{nameof(ExternalTraderId)}: {ExternalTraderId}, {nameof(ExternalTraderName)}: {ExternalTraderName}";

    public override string ToString() => $"{GetType().Name}{{{PQExternalCounterPartyOrderToStringMembers}}}";
}
