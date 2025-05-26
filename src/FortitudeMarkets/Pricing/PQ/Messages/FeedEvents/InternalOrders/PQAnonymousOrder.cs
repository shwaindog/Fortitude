using System;
using System.Text.Json.Serialization;
using FortitudeCommon.DataStructures.Maps.IdMap;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.FeedEvents.InternalOrders;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;

namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.InternalOrders;

[Flags]
public enum PQAnonymousOrderUpdatedFlags : ushort
{
    None                    = 0x00_00
  , OrderIdFlag             = 0x00_01
  , GenesisFlagsFlag        = 0x00_02
  , OrderTypeFlag           = 0x00_04
  , OrderLifecycleStateFlag = 0x00_08
  , CreatedTimeDateFlag     = 0x00_10
  , CreatedTimeSub2MinFlag  = 0x00_20
  , UpdatedTimeDateFlag     = 0x00_40
  , UpdatedTimeSub2MinFlag  = 0x00_80
  , OrderVolumeFlag         = 0x01_00

  , OrderRemainingVolumeFlag = 0x02_00
  , TrackingIdFlag           = 0x04_00
}

public interface IPQAnonymousOrder : IMutableAnonymousOrder, IPQSupportsStringUpdates<IPQAnonymousOrder>
  , IPQSupportsNumberPrecisionFieldUpdates<IPQAnonymousOrder>, ICloneable<IPQAnonymousOrder>, ISupportsPQNameIdLookupGenerator
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    bool IsOrderIdUpdated { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    bool IsGenesisFlagsUpdated { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    bool IsOrderTypeUpdated { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    bool IsOrderLifecycleStateUpdated { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    bool IsCreatedTimeDateUpdated { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    bool IsCreatedTimeSub2MinUpdated { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    bool IsUpdateTimeDateUpdated { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    bool IsUpdateTimeSub2MinUpdated { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    bool IsOrderVolumeUpdated { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    bool IsOrderRemainingVolumeUpdated { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    bool IsTrackingIdUpdated { get; set; }


    new bool HasUpdates { get; set; }

    new IPQAdditionalInternalPassiveOrderInfo?      InternalOrderInfo             { get; set; }
    new IPQAdditionalExternalCounterPartyOrderInfo? ExternalCounterPartyOrderInfo { get; set; }

    new IPQAnonymousOrder Clone();
}

public class PQAnonymousOrder : ReusableObject<IAnonymousOrder>, IPQAnonymousOrder
{
    public const PQFeedFields DefaultOrderStringDictionaryField = PQFeedFields.QuoteLayerStringUpdates;

    private IPQInternalPassiveOrder?      asPassiveOrderRef;
    private IPQExternalCounterPartyOrder? asExternalCpRef;

    protected int NumUpdatesSinceEmpty = -1;

    protected PQAnonymousOrderUpdatedFlags UpdatedFlags;

    private IPQNameIdLookupGenerator nameIdLookup = null!;

    private int       orderId;
    private decimal   orderVolume;
    private decimal   remainingOrderVolume;
    private DateTime  updateTime;
    private DateTime  createdTime;
    private OrderType orderType;
    private uint      trackingId;

    private OrderGenesisFlags   genesisFlags;
    private OrderLifeCycleState orderLifeCycleState;

    private IPQAdditionalInternalPassiveOrderInfo?      additionalInternalPassiveOrderInfo;
    private IPQAdditionalExternalCounterPartyOrderInfo? additionalExternalCounterPartyOrderInfo;

    public PQAnonymousOrder()
    {
        nameIdLookup = new PQNameIdLookupGenerator(DefaultOrderStringDictionaryField);
        if (GetType() == typeof(PQAnonymousOrder)) NumUpdatesSinceEmpty = 0;
    }

    public PQAnonymousOrder
        (OrderGenesisFlags genesisFlags, IPQNameIdLookupGenerator? nameIdLookupGenerator = null)
    {
        nameIdLookup = nameIdLookupGenerator ?? new PQNameIdLookupGenerator(DefaultOrderStringDictionaryField);

        EmptyIgnoreGenesisFlags = genesisFlags;
        GenesisFlags            = genesisFlags;
        IsGenesisFlagsUpdated   = false;
        if (GetType() == typeof(PQAnonymousOrder)) NumUpdatesSinceEmpty = 0;
    }

    public PQAnonymousOrder
    (IPQNameIdLookupGenerator? nameIdLookupGenerator = null, int orderId = 0, DateTime createdTime = default, decimal orderVolume = 0m
      , OrderType orderType = OrderType.None, OrderGenesisFlags genesisFlags = OrderGenesisFlags.None
      , OrderLifeCycleState orderLifeCycleState = OrderLifeCycleState.None
      , DateTime? updatedTime = null, decimal? remainingVolume = null, uint trackingId = 0)
    {
        nameIdLookup = nameIdLookupGenerator ?? new PQNameIdLookupGenerator(DefaultOrderStringDictionaryField);


        OrderId      = orderId;
        OrderType    = orderType;
        GenesisFlags = genesisFlags;
        CreatedTime  = createdTime;
        UpdateTime   = updatedTime ?? createdTime;
        TrackingId   = trackingId;

        OrderLifeCycleState  = orderLifeCycleState;
        OrderDisplayVolume   = orderVolume;
        OrderRemainingVolume = remainingVolume ?? orderVolume;
        if (GetType() == typeof(PQAnonymousOrder)) NumUpdatesSinceEmpty = 0;
    }

    public PQAnonymousOrder(IAnonymousOrder? toClone, IPQNameIdLookupGenerator? nameIdLookupGenerator = null)
    {
        if (toClone is PQAnonymousOrder pqAnonymousOrder)
        {
            nameIdLookup = nameIdLookupGenerator ?? pqAnonymousOrder.NameIdLookup.Clone();
        }
        else
        {
            nameIdLookup = nameIdLookupGenerator ?? new PQNameIdLookupGenerator(DefaultOrderStringDictionaryField);
        }
        if (toClone != null)
        {
            OrderId      = toClone.OrderId;
            OrderType    = toClone.OrderType;
            GenesisFlags = toClone.GenesisFlags;
            CreatedTime  = toClone.CreatedTime;
            UpdateTime   = toClone.UpdateTime;
            TrackingId   = toClone.TrackingId;

            EmptyIgnoreGenesisFlags = toClone.EmptyIgnoreGenesisFlags;
            OrderLifeCycleState     = toClone.OrderLifeCycleState;
            OrderDisplayVolume      = toClone.OrderDisplayVolume;
            OrderRemainingVolume    = toClone.OrderRemainingVolume;


            if (toClone.InternalOrderInfo != null)
            {
                additionalInternalPassiveOrderInfo ??= CreatedPassiveOrderInfoInstance();
                additionalInternalPassiveOrderInfo.CopyFrom(toClone.InternalOrderInfo, CopyMergeFlags.FullReplace | CopyMergeFlags.AsNew);
            }

            if (toClone.ExternalCounterPartyOrderInfo != null)
            {
                additionalExternalCounterPartyOrderInfo ??= CreatedExternalCounterPartyInfo();
                additionalExternalCounterPartyOrderInfo.CopyFrom(toClone.ExternalCounterPartyOrderInfo
                                                               , CopyMergeFlags.FullReplace | CopyMergeFlags.AsNew);
            }
            else if (additionalExternalCounterPartyOrderInfo != null)
            {
                additionalExternalCounterPartyOrderInfo.IsEmpty = true;
            }
            SetFlagsSame(toClone);
        }

        if (GetType() == typeof(PQAnonymousOrder)) NumUpdatesSinceEmpty = 0;
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int OrderId
    {
        get => orderId;
        set
        {
            IsOrderIdUpdated |= orderId != value || NumUpdatesSinceEmpty == 0;
            orderId          =  value;
        }
    }

    public OrderType OrderType
    {
        get => orderType;
        set
        {
            IsOrderTypeUpdated |= orderType != value || NumUpdatesSinceEmpty == 0;
            orderType          =  value;
        }
    }
    public OrderLifeCycleState OrderLifeCycleState
    {
        get => orderLifeCycleState;
        set
        {
            IsOrderLifecycleStateUpdated |= orderLifeCycleState != value || NumUpdatesSinceEmpty == 0;
            orderLifeCycleState          =  value;
        }
    }
    public OrderGenesisFlags GenesisFlags
    {
        get => genesisFlags;
        set
        {
            IsGenesisFlagsUpdated |= genesisFlags != value || NumUpdatesSinceEmpty == 0;
            genesisFlags          =  value;
        }
    }
    public OrderGenesisFlags EmptyIgnoreGenesisFlags { get; set; } = OrderGenesisFlags.None;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTime CreatedTime
    {
        get => createdTime;
        set
        {
            IsCreatedTimeDateUpdated |= createdTime.Get2MinIntervalsFromUnixEpoch() != value.Get2MinIntervalsFromUnixEpoch() ||
                                        NumUpdatesSinceEmpty == 0;
            IsCreatedTimeSub2MinUpdated |= createdTime.GetSub2MinComponent() != value.GetSub2MinComponent() || NumUpdatesSinceEmpty == 0;
            createdTime                 =  value;
        }
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTime UpdateTime
    {
        get => updateTime;
        set
        {
            IsUpdateTimeDateUpdated
                |= updateTime.Get2MinIntervalsFromUnixEpoch() != value.Get2MinIntervalsFromUnixEpoch() || NumUpdatesSinceEmpty == 0;
            IsUpdateTimeSub2MinUpdated |= updateTime.GetSub2MinComponent() != value.GetSub2MinComponent() || NumUpdatesSinceEmpty == 0;
            updateTime                 =  value;
        }
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public decimal OrderDisplayVolume
    {
        get => orderVolume;
        set
        {
            IsOrderVolumeUpdated |= orderVolume != value || NumUpdatesSinceEmpty == 0;

            orderVolume = value;
        }
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public decimal OrderRemainingVolume
    {
        get => remainingOrderVolume;
        set
        {
            IsOrderRemainingVolumeUpdated |= remainingOrderVolume != value || NumUpdatesSinceEmpty == 0;

            remainingOrderVolume = value;
        }
    }

    INameIdLookup? IHasNameIdLookup.NameIdLookup => NameIdLookup;

    [JsonIgnore]
    public IPQNameIdLookupGenerator NameIdLookup
    {
        get => nameIdLookup;
        set
        {
            if (nameIdLookup == value) return;

            nameIdLookup = value;

            if (additionalInternalPassiveOrderInfo != null) additionalInternalPassiveOrderInfo!.NameIdLookup           = value;
            if (additionalExternalCounterPartyOrderInfo != null) additionalExternalCounterPartyOrderInfo!.NameIdLookup = value;
        }
    }

    IAdditionalInternalPassiveOrderInfo? IAnonymousOrder.     InternalOrderInfo             => InternalOrderInfo;
    IAdditionalExternalCounterPartyOrderInfo? IAnonymousOrder.ExternalCounterPartyOrderInfo => ExternalCounterPartyOrderInfo;

    IMutableAdditionalInternalPassiveOrderInfo? IMutableAnonymousOrder.InternalOrderInfo
    {
        get => InternalOrderInfo;
        set => InternalOrderInfo = value as IPQAdditionalInternalPassiveOrderInfo;
    }
    IMutableAdditionalExternalCounterPartyOrderInfo? IMutableAnonymousOrder.ExternalCounterPartyOrderInfo
    {
        get => ExternalCounterPartyOrderInfo;
        set => ExternalCounterPartyOrderInfo = value as IPQAdditionalExternalCounterPartyOrderInfo;
    }

    public IPQAdditionalInternalPassiveOrderInfo? InternalOrderInfo
    {
        get => additionalInternalPassiveOrderInfo;
        set
        {
            if (additionalInternalPassiveOrderInfo == null)
            {
                additionalInternalPassiveOrderInfo = value;
            }
            else
            {
                if (value == null)
                {
                    additionalInternalPassiveOrderInfo.IsEmpty = true;
                }
                else
                {
                    additionalInternalPassiveOrderInfo!.CopyFrom(value, CopyMergeFlags.FullReplace);
                }
            }
            if (additionalInternalPassiveOrderInfo != null)
            {
                additionalInternalPassiveOrderInfo.NameIdLookup = NameIdLookup;
            }
        }
    }

    public IPQAdditionalExternalCounterPartyOrderInfo? ExternalCounterPartyOrderInfo
    {
        get => additionalExternalCounterPartyOrderInfo;
        set
        {
            if (additionalExternalCounterPartyOrderInfo == null)
            {
                additionalExternalCounterPartyOrderInfo = value;
            }
            else
            {
                if (value == null)
                {
                    additionalExternalCounterPartyOrderInfo.IsEmpty = true;
                }
                else
                {
                    additionalExternalCounterPartyOrderInfo!.CopyFrom(value, CopyMergeFlags.FullReplace);
                }
            }
            if (additionalExternalCounterPartyOrderInfo != null)
            {
                additionalExternalCounterPartyOrderInfo.NameIdLookup = NameIdLookup;
            }
        }
    }

    public uint TrackingId
    {
        get => trackingId;
        set
        {
            IsTrackingIdUpdated |= trackingId != value || NumUpdatesSinceEmpty == 0;

            trackingId = value;
        }
    }

    public bool IsOrderIdUpdated
    {
        get => (UpdatedFlags & PQAnonymousOrderUpdatedFlags.OrderIdFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= PQAnonymousOrderUpdatedFlags.OrderIdFlag;

            else if (IsOrderIdUpdated) UpdatedFlags ^= PQAnonymousOrderUpdatedFlags.OrderIdFlag;
        }
    }

    public bool IsGenesisFlagsUpdated
    {
        get => (UpdatedFlags & PQAnonymousOrderUpdatedFlags.GenesisFlagsFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= PQAnonymousOrderUpdatedFlags.GenesisFlagsFlag;

            else if (IsGenesisFlagsUpdated) UpdatedFlags ^= PQAnonymousOrderUpdatedFlags.GenesisFlagsFlag;
        }
    }
    public bool IsOrderTypeUpdated
    {
        get => (UpdatedFlags & PQAnonymousOrderUpdatedFlags.OrderTypeFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= PQAnonymousOrderUpdatedFlags.OrderTypeFlag;

            else if (IsOrderTypeUpdated) UpdatedFlags ^= PQAnonymousOrderUpdatedFlags.OrderTypeFlag;
        }
    }
    public bool IsOrderLifecycleStateUpdated
    {
        get => (UpdatedFlags & PQAnonymousOrderUpdatedFlags.OrderLifecycleStateFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= PQAnonymousOrderUpdatedFlags.OrderLifecycleStateFlag;

            else if (IsOrderLifecycleStateUpdated) UpdatedFlags ^= PQAnonymousOrderUpdatedFlags.OrderLifecycleStateFlag;
        }
    }

    public bool IsCreatedTimeSub2MinUpdated
    {
        get => (UpdatedFlags & PQAnonymousOrderUpdatedFlags.CreatedTimeSub2MinFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= PQAnonymousOrderUpdatedFlags.CreatedTimeSub2MinFlag;

            else if (IsCreatedTimeSub2MinUpdated) UpdatedFlags ^= PQAnonymousOrderUpdatedFlags.CreatedTimeSub2MinFlag;
        }
    }
    public bool IsCreatedTimeDateUpdated
    {
        get => (UpdatedFlags & PQAnonymousOrderUpdatedFlags.CreatedTimeDateFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= PQAnonymousOrderUpdatedFlags.CreatedTimeDateFlag;

            else if (IsCreatedTimeDateUpdated) UpdatedFlags ^= PQAnonymousOrderUpdatedFlags.CreatedTimeDateFlag;
        }
    }


    public bool IsUpdateTimeSub2MinUpdated
    {
        get => (UpdatedFlags & PQAnonymousOrderUpdatedFlags.UpdatedTimeSub2MinFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= PQAnonymousOrderUpdatedFlags.UpdatedTimeSub2MinFlag;

            else if (IsUpdateTimeSub2MinUpdated) UpdatedFlags ^= PQAnonymousOrderUpdatedFlags.UpdatedTimeSub2MinFlag;
        }
    }

    public bool IsUpdateTimeDateUpdated
    {
        get => (UpdatedFlags & PQAnonymousOrderUpdatedFlags.UpdatedTimeDateFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= PQAnonymousOrderUpdatedFlags.UpdatedTimeDateFlag;

            else if (IsUpdateTimeDateUpdated) UpdatedFlags ^= PQAnonymousOrderUpdatedFlags.UpdatedTimeDateFlag;
        }
    }

    public bool IsOrderVolumeUpdated
    {
        get => (UpdatedFlags & PQAnonymousOrderUpdatedFlags.OrderVolumeFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= PQAnonymousOrderUpdatedFlags.OrderVolumeFlag;

            else if (IsOrderVolumeUpdated) UpdatedFlags ^= PQAnonymousOrderUpdatedFlags.OrderVolumeFlag;
        }
    }

    public bool IsOrderRemainingVolumeUpdated
    {
        get => (UpdatedFlags & PQAnonymousOrderUpdatedFlags.OrderRemainingVolumeFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= PQAnonymousOrderUpdatedFlags.OrderRemainingVolumeFlag;

            else if (IsOrderRemainingVolumeUpdated) UpdatedFlags ^= PQAnonymousOrderUpdatedFlags.OrderRemainingVolumeFlag;
        }
    }

    public bool IsTrackingIdUpdated
    {
        get => (UpdatedFlags & PQAnonymousOrderUpdatedFlags.TrackingIdFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= PQAnonymousOrderUpdatedFlags.TrackingIdFlag;

            else if (IsTrackingIdUpdated) UpdatedFlags ^= PQAnonymousOrderUpdatedFlags.TrackingIdFlag;
        }
    }


    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public virtual bool HasUpdates
    {
        get =>
            UpdatedFlags != PQAnonymousOrderUpdatedFlags.None
         || (additionalInternalPassiveOrderInfo?.HasUpdates ?? false)
         || (additionalExternalCounterPartyOrderInfo?.HasUpdates ?? false);
        set
        {
            if (additionalInternalPassiveOrderInfo != null) additionalInternalPassiveOrderInfo.HasUpdates           = value;
            if (additionalExternalCounterPartyOrderInfo != null) additionalExternalCounterPartyOrderInfo.HasUpdates = value;
            if (value)
            {
                UpdatedFlags = UpdatedFlags.AllFlags();
            }
            else
            {
                UpdatedFlags = PQAnonymousOrderUpdatedFlags.None;
            }
        }
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
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
         && TrackingId == 0
         && (additionalInternalPassiveOrderInfo?.IsEmpty ?? true)
         && (additionalExternalCounterPartyOrderInfo?.IsEmpty ?? true);
        set
        {
            if (!value) return;
            OrderId      = 0;
            CreatedTime  = default;
            UpdateTime   = default;
            UpdateTime   = default;
            OrderType    = OrderType.None;
            GenesisFlags = EmptyIgnoreGenesisFlags;
            TrackingId   = 0;

            OrderLifeCycleState  = OrderLifeCycleState.None;
            OrderDisplayVolume   = 0m;
            OrderRemainingVolume = 0m;

            NumUpdatesSinceEmpty = 0;
            if (additionalInternalPassiveOrderInfo != null) additionalInternalPassiveOrderInfo.IsEmpty           = true;
            if (additionalExternalCounterPartyOrderInfo != null) additionalExternalCounterPartyOrderInfo.IsEmpty = true;
        }
    }

    public uint UpdateCount => (uint)NumUpdatesSinceEmpty;

    public virtual void UpdateComplete()
    {
        if (HasUpdates && !IsEmpty) NumUpdatesSinceEmpty++;
        HasUpdates = false;
    }

    IAnonymousOrder ICloneable<IAnonymousOrder>.Clone() => Clone();

    IPQAnonymousOrder ICloneable<IPQAnonymousOrder>.Clone() => Clone();

    IPQAnonymousOrder IPQAnonymousOrder.Clone() => Clone();

    IMutableAnonymousOrder ICloneable<IMutableAnonymousOrder>.Clone() => Clone();

    IMutableAnonymousOrder IMutableAnonymousOrder.Clone() => Clone();

    public override PQAnonymousOrder Clone() => Recycler?.Borrow<PQAnonymousOrder>().CopyFrom(this) ?? new PQAnonymousOrder(this);

    IInternalPassiveOrder? IAnonymousOrder.ToInternalOrder() => ToInternalOrder();

    public virtual IPQInternalPassiveOrder? ToInternalOrder()
    {
        var hasInfo = GenesisFlags.IsInternalOrder() && GenesisFlags.HasInternalOrderInfo() && InternalOrderInfo != null;
        if (!hasInfo) return null;
        if (this is IPQInternalPassiveOrder alreadyInternalPassiveOrder) return alreadyInternalPassiveOrder;

        additionalInternalPassiveOrderInfo ??= CreatedPassiveOrderInfoInstance();
        asPassiveOrderRef                  ??= new PQInternalPassiveOrder(this, additionalInternalPassiveOrderInfo);
        return asPassiveOrderRef;
    }

    IExternalCounterPartyOrder? IAnonymousOrder.ToExternalCounterPartyInfoOrder() => ToExternalCounterPartyInfoOrder();

    public virtual IExternalCounterPartyOrder? ToExternalCounterPartyInfoOrder()
    {
        var hasInfo = GenesisFlags.IsExternalOrder() && GenesisFlags.HasExternalCounterPartyInfo() && ExternalCounterPartyOrderInfo != null;
        if (!hasInfo) return null;
        if (this is IPQExternalCounterPartyOrder alreadyExternalCounterPartyInfoOrder) return alreadyExternalCounterPartyInfoOrder;
        additionalExternalCounterPartyOrderInfo ??= CreatedExternalCounterPartyInfo();
        asExternalCpRef                         ??= new PQExternalCounterPartyOrder(this, additionalExternalCounterPartyOrderInfo);
        return asExternalCpRef;
    }

    public virtual int UpdateField(PQFieldUpdate fieldUpdate)
    {
        switch (fieldUpdate.OrdersSubId)
        {
            case PQOrdersSubFieldKeys.OrderId:
                IsOrderIdUpdated = true; // incase of reset and sending 0;
                OrderId          = (int)fieldUpdate.Payload;
                return 0;
            case PQOrdersSubFieldKeys.OrderType:
                IsOrderTypeUpdated = true; // incase of reset and sending 0;
                OrderType          = (OrderType)fieldUpdate.Payload;
                return 0;
            case PQOrdersSubFieldKeys.OrderGenesisFlags:
                IsGenesisFlagsUpdated = true; // incase of reset and sending 0;
                GenesisFlags          = (OrderGenesisFlags)fieldUpdate.Payload;
                return 0;
            case PQOrdersSubFieldKeys.OrderLifecycleStateFlags:
                IsOrderLifecycleStateUpdated = true; // incase of reset and sending 0;
                OrderLifeCycleState          = (OrderLifeCycleState)fieldUpdate.Payload;
                return 0;
            case PQOrdersSubFieldKeys.OrderCreatedDate:
                IsCreatedTimeDateUpdated = true; // incase of reset and sending 0;
                PQFieldConverters.Update2MinuteIntervalsFromUnixEpoch(ref createdTime, fieldUpdate.Payload);
                if (createdTime == DateTime.UnixEpoch) createdTime = default;
                return 0;
            case PQOrdersSubFieldKeys.OrderCreatedSub2MinTime:
                IsCreatedTimeSub2MinUpdated = true; // incase of reset and sending 0;
                PQFieldConverters.UpdateSub2MinComponent
                    (ref createdTime, fieldUpdate.Flag.AppendScaleFlagsToUintToMakeLong(fieldUpdate.Payload));
                if (createdTime == DateTime.UnixEpoch) createdTime = default;
                return 0;
            case PQOrdersSubFieldKeys.OrderUpdatedDate:
                IsUpdateTimeDateUpdated = true; // incase of reset and sending 0;
                PQFieldConverters.Update2MinuteIntervalsFromUnixEpoch(ref updateTime, fieldUpdate.Payload);
                if (updateTime == DateTime.UnixEpoch) updateTime = default;
                return 0;
            case PQOrdersSubFieldKeys.OrderUpdatedSub2MinTime:
                IsUpdateTimeSub2MinUpdated = true; // incase of reset and sending 0;
                PQFieldConverters.UpdateSub2MinComponent
                    (ref updateTime, fieldUpdate.Flag.AppendScaleFlagsToUintToMakeLong(fieldUpdate.Payload));
                if (updateTime == DateTime.UnixEpoch) updateTime = default;
                return 0;
            case PQOrdersSubFieldKeys.OrderDisplayVolume:
                IsOrderVolumeUpdated = true; // incase of reset and sending 0;
                OrderDisplayVolume   = PQScaling.Unscale(fieldUpdate.Payload, fieldUpdate.Flag);
                return 0;
            case PQOrdersSubFieldKeys.OrderRemainingVolume:
                IsOrderRemainingVolumeUpdated = true; // incase of reset and sending 0;
                OrderRemainingVolume          = PQScaling.Unscale(fieldUpdate.Payload, fieldUpdate.Flag);
                return 0;
            case PQOrdersSubFieldKeys.OrderTrackingId:
                IsTrackingIdUpdated = true; // incase of reset and sending 0;
                TrackingId          = fieldUpdate.Payload;
                return 0;
            default:

                if (fieldUpdate.OrdersSubId is >= PQOrdersSubFieldKeys.OrderId and <= PQOrdersSubFieldKeys.OrderInternalMarginConsumed)
                {
                    additionalInternalPassiveOrderInfo ??= CreatedPassiveOrderInfoInstance();
                    var result = additionalInternalPassiveOrderInfo.UpdateField(fieldUpdate);
                    if (result >= 0) return result;
                }
                if (fieldUpdate.OrdersSubId is >= PQOrdersSubFieldKeys.OrderExternalCounterPartyId
                                           and <= PQOrdersSubFieldKeys.OrderExternalTraderNameId)
                {
                    additionalExternalCounterPartyOrderInfo ??= CreatedExternalCounterPartyInfo();
                    var result = additionalExternalCounterPartyOrderInfo.UpdateField(fieldUpdate);
                    if (result >= 0) return result;
                }

                break;
        }
        return -1;
    }

    protected virtual IPQAdditionalInternalPassiveOrderInfo CreatedPassiveOrderInfoInstance
        (IAdditionalInternalPassiveOrderInfo? optionalCopy = null) =>
        new PQAdditionalInternalPassiveOrderInfo(NameIdLookup);

    protected virtual IPQAdditionalExternalCounterPartyOrderInfo CreatedExternalCounterPartyInfo
        (IAdditionalExternalCounterPartyOrderInfo? optionalCopy = null) =>
        new PQAdditionalExternalCounterPartyInfo(NameIdLookup);

    public virtual IEnumerable<PQFieldUpdate> GetDeltaUpdateFields
    (DateTime snapShotTime, StorageFlags messageFlags
      , IPQPriceVolumePublicationPrecisionSettings? quotePublicationPrecisionSettings = null)
    {
        var updatedOnly = (messageFlags & StorageFlags.Complete) == 0;
        if (!updatedOnly || IsOrderIdUpdated)
            yield return new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, PQOrdersSubFieldKeys.OrderId, (uint)OrderId);
        if (!updatedOnly || IsOrderTypeUpdated)
            yield return new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, PQOrdersSubFieldKeys.OrderType, (uint)OrderType);
        if (!updatedOnly || IsGenesisFlagsUpdated)
            yield return new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, PQOrdersSubFieldKeys.OrderGenesisFlags, (uint)GenesisFlags);
        if (!updatedOnly || IsOrderLifecycleStateUpdated)
            yield return new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, PQOrdersSubFieldKeys.OrderLifecycleStateFlags, (uint)OrderLifeCycleState);

        if (!updatedOnly || IsCreatedTimeDateUpdated)
            yield return new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, PQOrdersSubFieldKeys.OrderCreatedDate
                                         , createdTime.Get2MinIntervalsFromUnixEpoch());
        if (!updatedOnly || IsCreatedTimeSub2MinUpdated)
        {
            var extended = createdTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var value);
            yield return new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, PQOrdersSubFieldKeys.OrderCreatedSub2MinTime, value, extended);
        }
        if (!updatedOnly || IsUpdateTimeDateUpdated)
            yield return new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, PQOrdersSubFieldKeys.OrderUpdatedDate
                                         , updateTime.Get2MinIntervalsFromUnixEpoch());

        if (!updatedOnly || IsUpdateTimeSub2MinUpdated)
        {
            var extended = updateTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var value);
            yield return new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, PQOrdersSubFieldKeys.OrderUpdatedSub2MinTime, value, extended);
        }
        if (!updatedOnly || IsOrderVolumeUpdated)
            yield return new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, PQOrdersSubFieldKeys.OrderDisplayVolume, OrderDisplayVolume,
                                           quotePublicationPrecisionSettings?.VolumeScalingPrecision ?? (PQFieldFlags)6);
        if (!updatedOnly || IsOrderRemainingVolumeUpdated)
            yield return new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, PQOrdersSubFieldKeys.OrderRemainingVolume, OrderRemainingVolume,
                                           quotePublicationPrecisionSettings?.VolumeScalingPrecision ?? (PQFieldFlags)6);
        if (!updatedOnly || IsTrackingIdUpdated)
            yield return new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, PQOrdersSubFieldKeys.OrderTrackingId, TrackingId);

        if (additionalInternalPassiveOrderInfo != null)
        {
            foreach (var internalOrderFields in additionalInternalPassiveOrderInfo.GetDeltaUpdateFields(snapShotTime, messageFlags
                    , quotePublicationPrecisionSettings))
            {
                yield return internalOrderFields;
            }
        }
        if (additionalExternalCounterPartyOrderInfo != null)
        {
            foreach (var externalCpFields in additionalExternalCounterPartyOrderInfo.GetDeltaUpdateFields(snapShotTime, messageFlags
                    , quotePublicationPrecisionSettings))
            {
                yield return externalCpFields;
            }
        }
    }

    public virtual bool UpdateFieldString(PQFieldStringUpdate stringUpdate) => NameIdLookup.UpdateFieldString(stringUpdate);

    public virtual IEnumerable<PQFieldStringUpdate> GetStringUpdates(DateTime snapShotTime, StorageFlags messageFlags) =>
        NameIdLookup.GetStringUpdates(snapShotTime, messageFlags);

    public override void StateReset()
    {
        OrderId      = 0;
        OrderType    = OrderType.None;
        GenesisFlags = EmptyIgnoreGenesisFlags; // Todo move this to ResetFieldsWithTracking
        CreatedTime  = default;
        UpdateTime   = default;
        TrackingId   = 0;
        additionalInternalPassiveOrderInfo?.StateReset();
        additionalExternalCounterPartyOrderInfo?.StateReset();

        OrderDisplayVolume   = 0m;
        OrderRemainingVolume = 0m;
        OrderLifeCycleState  = OrderLifeCycleState.None;
        NumUpdatesSinceEmpty = 0;

        UpdatedFlags = PQAnonymousOrderUpdatedFlags.None;
        base.StateReset();
    }

    public virtual bool AreEquivalent(IAnonymousOrder? other, bool exactTypes = false)
    {
        if (other == null) return false;
        if (exactTypes && other.GetType() != GetType()) return false;
        var orderIdsSame    = OrderId == other.OrderId;
        var createdSame     = CreatedTime == other.CreatedTime;
        var orderTypeSame   = OrderType == other.OrderType;
        var lifecycleSame   = OrderLifeCycleState == other.OrderLifeCycleState;
        var updatedTimeSame = UpdateTime == other.UpdateTime;
        var volumeSame      = OrderDisplayVolume == other.OrderDisplayVolume;
        var trackingIdSame  = TrackingId == other.TrackingId;

        var ignoreFlagsDifferences = IExternalCounterPartyOrder.HasExternalCounterPartyOrderInfoFlags |
                                     IInternalPassiveOrder.HasInternalOrderInfo;
        var ignoreFlagsDiffMask = ~ignoreFlagsDifferences;
        var emptyGenesisSame = exactTypes
            ? EmptyIgnoreGenesisFlags == other.EmptyIgnoreGenesisFlags
            : (EmptyIgnoreGenesisFlags & ignoreFlagsDiffMask) == (other.EmptyIgnoreGenesisFlags & ignoreFlagsDiffMask);
        
        var genesisFlagsSame   = exactTypes
            ? GenesisFlags == other.GenesisFlags
            : (GenesisFlags & ignoreFlagsDiffMask) == (other.GenesisFlags & ignoreFlagsDiffMask);


        var remainingVolumeSame = OrderRemainingVolume == other.OrderRemainingVolume;

        var internalOrderInfoSame =
            additionalInternalPassiveOrderInfo?.AreEquivalent(other.InternalOrderInfo, exactTypes) ??
            !exactTypes || other.InternalOrderInfo is null or { IsEmpty: true };

        var externalCpInfoSame =
            additionalExternalCounterPartyOrderInfo?.AreEquivalent(other.ExternalCounterPartyOrderInfo, exactTypes) ??
            !exactTypes || other.ExternalCounterPartyOrderInfo is null or { IsEmpty: true };

        var updatedSame = true;
        if (exactTypes)
        {
            var pqTraderLayerInfo = (PQAnonymousOrder)other;
            updatedSame = UpdatedFlags == pqTraderLayerInfo.UpdatedFlags;
        }

        var allAreSame = orderIdsSame && createdSame && orderTypeSame && genesisFlagsSame && lifecycleSame && updatedTimeSame
                      && volumeSame && remainingVolumeSame && updatedSame && trackingIdSame && internalOrderInfoSame && externalCpInfoSame &&
                         emptyGenesisSame;

        return allAreSame;
    }

    protected void SetFlagsSame(IAnonymousOrder toCopyFlags)
    {
        if (toCopyFlags is PQAnonymousOrder pqToClone)
        {
            UpdatedFlags = pqToClone.UpdatedFlags;
        }
        else if (toCopyFlags is IPQAnonymousOrder ipqAnonOrder)
        {
            IsOrderIdUpdated              = ipqAnonOrder.IsOrderIdUpdated;
            IsGenesisFlagsUpdated         = ipqAnonOrder.IsGenesisFlagsUpdated;
            IsOrderTypeUpdated            = ipqAnonOrder.IsOrderTypeUpdated;
            IsOrderLifecycleStateUpdated  = ipqAnonOrder.IsOrderLifecycleStateUpdated;
            IsCreatedTimeDateUpdated      = ipqAnonOrder.IsCreatedTimeDateUpdated;
            IsCreatedTimeSub2MinUpdated   = ipqAnonOrder.IsCreatedTimeSub2MinUpdated;
            IsUpdateTimeDateUpdated       = ipqAnonOrder.IsUpdateTimeDateUpdated;
            IsUpdateTimeSub2MinUpdated    = ipqAnonOrder.IsUpdateTimeSub2MinUpdated;
            IsOrderVolumeUpdated          = ipqAnonOrder.IsOrderVolumeUpdated;
            IsOrderRemainingVolumeUpdated = ipqAnonOrder.IsOrderRemainingVolumeUpdated;
            IsTrackingIdUpdated           = ipqAnonOrder.IsTrackingIdUpdated;
        }
    }

    IReusableObject<IAnonymousOrder> ITransferState<IReusableObject<IAnonymousOrder>>.CopyFrom
        (IReusableObject<IAnonymousOrder> source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom((IAnonymousOrder?)source!, copyMergeFlags);

    IAnonymousOrder ITransferState<IAnonymousOrder>.CopyFrom(IAnonymousOrder source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom((IAnonymousOrder?)source!, copyMergeFlags);

    IPQAnonymousOrder ITransferState<IPQAnonymousOrder>.CopyFrom(IPQAnonymousOrder source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom(source, copyMergeFlags);

    IMutableAnonymousOrder ITransferState<IMutableAnonymousOrder>.CopyFrom(IMutableAnonymousOrder source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom(source, copyMergeFlags);

    public override PQAnonymousOrder CopyFrom(IAnonymousOrder? source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (source is null) return this;
        var isFullReplace = copyMergeFlags.HasFullReplace();
        if (source is not IPQAnonymousOrder pqAnonOrder)
        {
            OrderId      = source.OrderId;
            OrderType    = source.OrderType;
            GenesisFlags = source.GenesisFlags;
            CreatedTime  = source.CreatedTime;
            UpdateTime   = source.UpdateTime;
            TrackingId   = source.TrackingId;

            EmptyIgnoreGenesisFlags = source.EmptyIgnoreGenesisFlags;
            OrderLifeCycleState     = source.OrderLifeCycleState;
            OrderDisplayVolume      = source.OrderDisplayVolume;
            OrderRemainingVolume    = source.OrderRemainingVolume;
        }
        else
        {
            EmptyIgnoreGenesisFlags = source.EmptyIgnoreGenesisFlags;

            if (pqAnonOrder.IsOrderIdUpdated || isFullReplace)
            {
                IsOrderIdUpdated = true;

                OrderId = pqAnonOrder.OrderId;
            }
            if (pqAnonOrder.IsOrderTypeUpdated || isFullReplace)
            {
                IsOrderTypeUpdated = true;

                OrderType = pqAnonOrder.OrderType;
            }
            if (pqAnonOrder.IsGenesisFlagsUpdated || isFullReplace)
            {
                IsGenesisFlagsUpdated = true;

                GenesisFlags = pqAnonOrder.GenesisFlags;
            }
            if (pqAnonOrder.IsOrderLifecycleStateUpdated || isFullReplace)
            {
                IsOrderLifecycleStateUpdated = true;

                OrderLifeCycleState = pqAnonOrder.OrderLifeCycleState;
            }
            if (pqAnonOrder.IsCreatedTimeDateUpdated || isFullReplace)
            {
                IsCreatedTimeDateUpdated = true;

                CreatedTime = pqAnonOrder.CreatedTime;
            }
            if (pqAnonOrder.IsCreatedTimeSub2MinUpdated || isFullReplace)
            {
                IsCreatedTimeSub2MinUpdated = true;

                CreatedTime = pqAnonOrder.CreatedTime;
            }
            if (pqAnonOrder.IsUpdateTimeDateUpdated || isFullReplace)
            {
                IsUpdateTimeDateUpdated = true;

                UpdateTime = pqAnonOrder.UpdateTime;
            }
            if (pqAnonOrder.IsUpdateTimeSub2MinUpdated || isFullReplace)
            {
                IsUpdateTimeSub2MinUpdated = true;

                UpdateTime = pqAnonOrder.UpdateTime;
            }
            if (pqAnonOrder.IsOrderVolumeUpdated || isFullReplace)
            {
                IsOrderVolumeUpdated = true;

                OrderDisplayVolume = pqAnonOrder.OrderDisplayVolume;
            }
            if (pqAnonOrder.IsOrderRemainingVolumeUpdated || isFullReplace)
            {
                IsOrderRemainingVolumeUpdated = true;

                OrderRemainingVolume = pqAnonOrder.OrderRemainingVolume;
            }
            if (pqAnonOrder.IsTrackingIdUpdated || isFullReplace)
            {
                IsTrackingIdUpdated = true;

                TrackingId = pqAnonOrder.TrackingId;
            }

            if (isFullReplace) UpdatedFlags = (source as PQAnonymousOrder)?.UpdatedFlags ?? UpdatedFlags;
        }
        if (source.InternalOrderInfo != null)
        {
            additionalInternalPassiveOrderInfo ??= CreatedPassiveOrderInfoInstance();
            additionalInternalPassiveOrderInfo.CopyFrom(source.InternalOrderInfo, copyMergeFlags);
        }
        else if (isFullReplace && additionalInternalPassiveOrderInfo != null)
        {
            additionalInternalPassiveOrderInfo.IsEmpty = true;
        }

        if (source.ExternalCounterPartyOrderInfo != null)
        {
            additionalExternalCounterPartyOrderInfo ??= CreatedExternalCounterPartyInfo();
            additionalExternalCounterPartyOrderInfo.CopyFrom(source.ExternalCounterPartyOrderInfo, copyMergeFlags);
        }
        else if (isFullReplace && additionalExternalCounterPartyOrderInfo != null)
        {
            additionalExternalCounterPartyOrderInfo.IsEmpty = true;
        }
        return this;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent((IAnonymousOrder?)obj, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = OrderId;
            hashCode = ((int)OrderType * 397) ^ hashCode;
            hashCode = ((int)GenesisFlags * 397) ^ hashCode;
            hashCode = ((int)OrderLifeCycleState * 397) ^ hashCode;
            hashCode = ((int)EmptyIgnoreGenesisFlags * 397) ^ hashCode;
            hashCode = (CreatedTime.GetHashCode() * 397) ^ hashCode;
            hashCode = (UpdateTime.GetHashCode() * 397) ^ hashCode;
            hashCode = (OrderDisplayVolume.GetHashCode() * 397) ^ hashCode;
            hashCode = (OrderRemainingVolume.GetHashCode() * 397) ^ hashCode;
            hashCode = ((int)TrackingId * 397) ^ hashCode;
            return hashCode;
        }
    }

    protected string PQPublishedOrderToStringMembers =>
        $"{nameof(OrderId)}: {OrderId}, {nameof(OrderType)}: {OrderType}, {nameof(GenesisFlags)}: {GenesisFlags}, " +
        $"{nameof(EmptyIgnoreGenesisFlags)}: {EmptyIgnoreGenesisFlags}, {nameof(CreatedTime)}: {CreatedTime}, " +
        $"{nameof(OrderLifeCycleState)}: {OrderLifeCycleState}, {nameof(UpdateTime)}: {UpdateTime}, " +
        $"{nameof(OrderDisplayVolume)}: {OrderDisplayVolume:N2}, {nameof(OrderRemainingVolume)}: {OrderRemainingVolume:N2}, {nameof(TrackingId)}: {TrackingId}";

    protected string UpdatedFlagsToString => $"{nameof(UpdatedFlags)}: {UpdatedFlags}";

    public override string ToString() => $"{nameof(PQAnonymousOrder)}{{{PQPublishedOrderToStringMembers}, {UpdatedFlagsToString}}}";
}
