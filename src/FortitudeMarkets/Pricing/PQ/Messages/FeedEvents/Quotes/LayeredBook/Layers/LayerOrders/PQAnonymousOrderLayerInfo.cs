// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text.Json.Serialization;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.FeedEvents.InternalOrders;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.Layers.LayerOrders;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;
using FortitudeMarkets.Trading.Orders;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook.Layers.LayerOrders;

[Flags]
public enum OrderLayerInfoUpdatedFlags : ushort
{
    None                    = 0x00_00
  , OrderIdFlag             = 0x00_01
  , TypeFlagsFlag           = 0x00_02
  , OrderTypeFlag           = 0x00_04
  , OrderLifecycleStateFlag = 0x00_08
  , OrderLayerFlagsFlag     = 0x00_10
  , CreatedTimeDateFlag     = 0x00_20
  , CreatedTimeSub2MinFlag  = 0x00_40
  , UpdatedTimeDateFlag     = 0x00_80
  , UpdatedTimeSub2MinFlag  = 0x01_00
  , OrderVolumeFlag         = 0x02_00

  , OrderRemainingVolumeFlag = 0x04_00
  , TrackingIdFlag           = 0x08_00

  , ExternalTraderIdUpdatedFlag     = 0x10_00
  , ExternalTraderNameIdUpdatedFlag = 0x20_00

  , ExternalCounterPartyIdUpdatedFlag     = 0x40_00
  , ExternalCounterPartyNameIdUpdatedFlag = 0x80_00
}

public interface IPQAnonymousOrderLayerInfo : IMutableAnonymousOrderLayerInfo, IPQSupportsNumberPrecisionFieldUpdates<IAnonymousOrderLayerInfo>
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    bool IsOrderIdUpdated { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    bool IsTypeFlagsUpdated { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    bool IsOrderTypeUpdated { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    bool IsOrderLifecycleStateUpdated { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    bool IsOrderLayerFlagsUpdated { get; set; }

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

    new IPQAnonymousOrderLayerInfo Clone();
}

public class PQAnonymousOrderLayerInfo : ReusableObject<IAnonymousOrderLayerInfo>, IPQAnonymousOrderLayerInfo
{
    protected int NumUpdatesSinceEmpty = -1;

    protected OrderLayerInfoUpdatedFlags UpdatedFlags;

    private int        orderId;
    private decimal    orderVolume;
    private decimal    remainingOrderVolume;
    private DateTime   updateTime;
    private DateTime   createdTime;
    private OrderType  orderType;
    private OrderFlags typeFlags;
    private uint       trackingId;

    private LayerOrderFlags     orderFlags;
    private OrderLifeCycleState orderLifeCycleState;

    public PQAnonymousOrderLayerInfo()
    {
        if (GetType() == typeof(PQAnonymousOrderLayerInfo)) NumUpdatesSinceEmpty = 0;
    }

    public PQAnonymousOrderLayerInfo
    (int orderId = 0, DateTime createdTime = default, decimal orderVolume = 0m, LayerOrderFlags orderFlags = LayerOrderFlags.None
      , OrderType orderType = OrderType.None, OrderFlags typeFlags = OrderFlags.None, OrderLifeCycleState orderLifeCycleState = OrderLifeCycleState.None
      , DateTime? updatedTime = null, decimal? remainingVolume = null, uint trackingId = 0)
    {
        OrderId         = orderId;
        OrderType       = orderType;
        TypeFlags       = typeFlags;
        OrderLayerFlags = orderFlags;
        CreatedTime     = createdTime;
        UpdateTime      = updatedTime ?? createdTime;
        TrackingId      = trackingId;

        OrderLifeCycleState  = orderLifeCycleState;
        OrderDisplayVolume   = orderVolume;
        OrderRemainingVolume = remainingVolume ?? orderVolume;
        if (GetType() == typeof(PQAnonymousOrderLayerInfo)) NumUpdatesSinceEmpty = 0;
    }

    public PQAnonymousOrderLayerInfo(IAnonymousOrderLayerInfo toClone)
    {
        OrderId         = toClone.OrderId;
        OrderType       = toClone.OrderType;
        TypeFlags       = toClone.TypeFlags;
        OrderLayerFlags = toClone.OrderLayerFlags;
        CreatedTime     = toClone.CreatedTime;
        UpdateTime      = toClone.UpdateTime;
        TrackingId      = toClone.TrackingId;

        OrderLifeCycleState  = toClone.OrderLifeCycleState;
        OrderDisplayVolume   = toClone.OrderDisplayVolume;
        OrderRemainingVolume = toClone.OrderRemainingVolume;

        SetFlagsSame(toClone);
        if (GetType() == typeof(PQAnonymousOrderLayerInfo)) NumUpdatesSinceEmpty = 0;
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
    public OrderFlags TypeFlags
    {
        get => typeFlags;
        set
        {
            IsTypeFlagsUpdated |= typeFlags != value || NumUpdatesSinceEmpty == 0;
            typeFlags          =  value;
        }
    }


    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public LayerOrderFlags OrderLayerFlags
    {
        get => orderFlags;
        set
        {
            IsOrderLayerFlagsUpdated |= orderFlags != value || NumUpdatesSinceEmpty == 0;
            orderFlags               =  value;
        }
    }

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
        get => (UpdatedFlags & OrderLayerInfoUpdatedFlags.OrderIdFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= OrderLayerInfoUpdatedFlags.OrderIdFlag;

            else if (IsOrderIdUpdated) UpdatedFlags ^= OrderLayerInfoUpdatedFlags.OrderIdFlag;
        }
    }

    public bool IsTypeFlagsUpdated
    {
        get => (UpdatedFlags & OrderLayerInfoUpdatedFlags.TypeFlagsFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= OrderLayerInfoUpdatedFlags.TypeFlagsFlag;

            else if (IsTypeFlagsUpdated) UpdatedFlags ^= OrderLayerInfoUpdatedFlags.TypeFlagsFlag;
        }
    }
    public bool IsOrderTypeUpdated
    {
        get => (UpdatedFlags & OrderLayerInfoUpdatedFlags.OrderTypeFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= OrderLayerInfoUpdatedFlags.OrderTypeFlag;

            else if (IsOrderTypeUpdated) UpdatedFlags ^= OrderLayerInfoUpdatedFlags.OrderTypeFlag;
        }
    }
    public bool IsOrderLifecycleStateUpdated
    {
        get => (UpdatedFlags & OrderLayerInfoUpdatedFlags.OrderLifecycleStateFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= OrderLayerInfoUpdatedFlags.OrderLifecycleStateFlag;

            else if (IsOrderLifecycleStateUpdated) UpdatedFlags ^= OrderLayerInfoUpdatedFlags.OrderLifecycleStateFlag;
        }
    }


    public bool IsOrderLayerFlagsUpdated
    {
        get => (UpdatedFlags & OrderLayerInfoUpdatedFlags.OrderLayerFlagsFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= OrderLayerInfoUpdatedFlags.OrderLayerFlagsFlag;

            else if (IsOrderLayerFlagsUpdated) UpdatedFlags ^= OrderLayerInfoUpdatedFlags.OrderLayerFlagsFlag;
        }
    }


    public bool IsCreatedTimeSub2MinUpdated
    {
        get => (UpdatedFlags & OrderLayerInfoUpdatedFlags.CreatedTimeSub2MinFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= OrderLayerInfoUpdatedFlags.CreatedTimeSub2MinFlag;

            else if (IsCreatedTimeSub2MinUpdated) UpdatedFlags ^= OrderLayerInfoUpdatedFlags.CreatedTimeSub2MinFlag;
        }
    }
    public bool IsCreatedTimeDateUpdated
    {
        get => (UpdatedFlags & OrderLayerInfoUpdatedFlags.CreatedTimeDateFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= OrderLayerInfoUpdatedFlags.CreatedTimeDateFlag;

            else if (IsCreatedTimeDateUpdated) UpdatedFlags ^= OrderLayerInfoUpdatedFlags.CreatedTimeDateFlag;
        }
    }


    public bool IsUpdateTimeSub2MinUpdated
    {
        get => (UpdatedFlags & OrderLayerInfoUpdatedFlags.UpdatedTimeSub2MinFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= OrderLayerInfoUpdatedFlags.UpdatedTimeSub2MinFlag;

            else if (IsUpdateTimeSub2MinUpdated) UpdatedFlags ^= OrderLayerInfoUpdatedFlags.UpdatedTimeSub2MinFlag;
        }
    }

    public bool IsUpdateTimeDateUpdated
    {
        get => (UpdatedFlags & OrderLayerInfoUpdatedFlags.UpdatedTimeDateFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= OrderLayerInfoUpdatedFlags.UpdatedTimeDateFlag;

            else if (IsUpdateTimeDateUpdated) UpdatedFlags ^= OrderLayerInfoUpdatedFlags.UpdatedTimeDateFlag;
        }
    }


    public bool IsOrderVolumeUpdated
    {
        get => (UpdatedFlags & OrderLayerInfoUpdatedFlags.OrderVolumeFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= OrderLayerInfoUpdatedFlags.OrderVolumeFlag;

            else if (IsOrderVolumeUpdated) UpdatedFlags ^= OrderLayerInfoUpdatedFlags.OrderVolumeFlag;
        }
    }


    public bool IsOrderRemainingVolumeUpdated
    {
        get => (UpdatedFlags & OrderLayerInfoUpdatedFlags.OrderRemainingVolumeFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= OrderLayerInfoUpdatedFlags.OrderRemainingVolumeFlag;

            else if (IsOrderRemainingVolumeUpdated) UpdatedFlags ^= OrderLayerInfoUpdatedFlags.OrderRemainingVolumeFlag;
        }
    }

    public bool IsTrackingIdUpdated
    {
        get => (UpdatedFlags & OrderLayerInfoUpdatedFlags.TrackingIdFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= OrderLayerInfoUpdatedFlags.TrackingIdFlag;

            else if (IsTrackingIdUpdated) UpdatedFlags ^= OrderLayerInfoUpdatedFlags.TrackingIdFlag;
        }
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public virtual bool HasUpdates
    {
        get => UpdatedFlags != OrderLayerInfoUpdatedFlags.None;
        set
        {
            if (value)
            {
                UpdatedFlags = UpdatedFlags.AllFlags();
            }
            else
            {
                UpdatedFlags = OrderLayerInfoUpdatedFlags.None;
            }
        }
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public virtual bool IsEmpty
    {
        get =>
            OrderId == 0
         && OrderLayerFlags == LayerOrderFlags.None
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
            OrderId         = 0;
            OrderLayerFlags = LayerOrderFlags.None;
            CreatedTime     = default;
            UpdateTime      = default;
            UpdateTime      = default;
            OrderType       = OrderType.None;
            TypeFlags       = OrderFlags.None;
            TrackingId      = 0;

            OrderLifeCycleState  = OrderLifeCycleState.None;
            OrderDisplayVolume   = 0m;
            OrderRemainingVolume = 0m;

            NumUpdatesSinceEmpty = 0;
        }
    }

    public uint UpdateCount => (uint)NumUpdatesSinceEmpty;

    public virtual void UpdateComplete()
    {
        if (HasUpdates && !IsEmpty) NumUpdatesSinceEmpty++;
        HasUpdates = false;
    }


    IInternalPassiveOrderLayerInfo? IAnonymousOrderLayerInfo.ToInternalOrder() => ToInternalOrder();

    IMutableInternalPassiveOrderLayerInfo? IMutableAnonymousOrderLayerInfo.ToInternalOrder() => ToInternalOrder();

    IInternalPassiveOrder? IPublishedOrder.ToInternalOrder() => ToInternalOrder();

    public virtual IMutableInternalPassiveOrderLayerInfo? ToInternalOrder() =>
        this is PQInternalPassiveOrderLayerInfo internalOrder && TypeFlags.IsInternalOrder() && TypeFlags.HasInternalOrderInfo()
            ? internalOrder
            : null;


    IExternalCounterPartyOrderLayerInfo? IAnonymousOrderLayerInfo.ToExternalCounterPartyInfoOrder() => ToExternalCounterPartyInfoOrder();

    IMutableExternalCounterPartyOrderLayerInfo? IMutableAnonymousOrderLayerInfo.ToExternalCounterPartyInfoOrder() =>
        ToExternalCounterPartyInfoOrder();

    IExternalCounterPartyInfoOrder? IPublishedOrder.ToExternalCounterPartyInfoOrder() => ToExternalCounterPartyInfoOrder();

    public virtual IMutableExternalCounterPartyOrderLayerInfo? ToExternalCounterPartyInfoOrder() =>
        this is PQCounterPartyOrderLayerInfo externalCounterPartyOrder && TypeFlags.IsExternalOrder() && TypeFlags.HasExternalCounterPartyInfo()
            ? externalCounterPartyOrder
            : null;

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
            case PQOrdersSubFieldKeys.OrderFlags:
                IsTypeFlagsUpdated = true; // incase of reset and sending 0;
                TypeFlags          = (OrderFlags)fieldUpdate.Payload;
                return 0;
            case PQOrdersSubFieldKeys.OrderLayerFlags:
                IsOrderLayerFlagsUpdated = true; // incase of reset and sending 0;
                OrderLayerFlags          = (LayerOrderFlags)fieldUpdate.Payload;
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
        }
        return -1;
    }

    public virtual IEnumerable<PQFieldUpdate> GetDeltaUpdateFields
    (DateTime snapShotTime, StorageFlags messageFlags
      , IPQPriceVolumePublicationPrecisionSettings? quotePublicationPrecisionSettings = null)
    {
        var updatedOnly = (messageFlags & StorageFlags.Complete) == 0;
        if (!updatedOnly || IsOrderIdUpdated)
            yield return new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, PQOrdersSubFieldKeys.OrderId, (uint)OrderId);
        if (!updatedOnly || IsOrderTypeUpdated)
            yield return new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, PQOrdersSubFieldKeys.OrderType, (uint)OrderType);
        if (!updatedOnly || IsTypeFlagsUpdated)
            yield return new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, PQOrdersSubFieldKeys.OrderFlags, (uint)TypeFlags);
        if (!updatedOnly || IsOrderLifecycleStateUpdated)
            yield return new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, PQOrdersSubFieldKeys.OrderLifecycleStateFlags, (uint)OrderLifeCycleState);
        if (!updatedOnly || IsOrderLayerFlagsUpdated)
            yield return new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, PQOrdersSubFieldKeys.OrderLayerFlags, (uint)OrderLayerFlags);

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
    }

    public override void StateReset()
    {
        OrderId         = 0;
        OrderLayerFlags = LayerOrderFlags.None;
        OrderType       = OrderType.None;
        TypeFlags       = OrderFlags.None;
        CreatedTime     = default;
        UpdateTime      = default;
        TrackingId      = 0;

        OrderDisplayVolume   = 0m;
        OrderRemainingVolume = 0m;
        OrderLifeCycleState  = OrderLifeCycleState.None;
        NumUpdatesSinceEmpty = 0;

        UpdatedFlags = OrderLayerInfoUpdatedFlags.None;
        base.StateReset();
    }

    object ICloneable.Clone() => Clone();

    IPQAnonymousOrderLayerInfo IPQAnonymousOrderLayerInfo.Clone() => Clone();

    IMutableAnonymousOrderLayerInfo IMutableAnonymousOrderLayerInfo.Clone() => Clone();

    public bool AreEquivalent(IMutableAnonymousOrderLayerInfo? other, bool exactTypes = false) =>
        AreEquivalent((IAnonymousOrderLayerInfo?)other, exactTypes);

    bool IInterfacesComparable<IPublishedOrder>.AreEquivalent
        (IPublishedOrder? other, bool exactTypes) =>
        AreEquivalent((IAnonymousOrderLayerInfo?)other, exactTypes);

    public virtual bool AreEquivalent(IAnonymousOrderLayerInfo? other, bool exactTypes = false)
    {
        if (other == null) return false;
        if (exactTypes && other.GetType() != GetType()) return false;
        var orderIdsSame        = OrderId == other.OrderId;
        var orderLayerFlagsSame = OrderLayerFlags == other.OrderLayerFlags;
        var createdSame         = CreatedTime == other.CreatedTime;
        var orderTypeSame       = OrderType == other.OrderType;
        var typeFlagsSame       = TypeFlags == other.TypeFlags;
        var lifecycleSame       = OrderLifeCycleState == other.OrderLifeCycleState;
        var updatedTimeSame     = UpdateTime == other.UpdateTime;
        var volumeSame          = OrderDisplayVolume == other.OrderDisplayVolume;
        var trackingIdSame      = TrackingId == other.TrackingId;

        var remainingVolumeSame = OrderRemainingVolume == other.OrderRemainingVolume;

        var updatedSame = true;
        if (exactTypes)
        {
            var pqTraderLayerInfo = (PQAnonymousOrderLayerInfo)other;
            updatedSame = UpdatedFlags == pqTraderLayerInfo.UpdatedFlags;
        }

        return orderIdsSame && orderLayerFlagsSame && createdSame && orderTypeSame && typeFlagsSame && lifecycleSame && updatedTimeSame
            && volumeSame && remainingVolumeSame && updatedSame && trackingIdSame;
    }

    protected void SetFlagsSame(IAnonymousOrderLayerInfo toCopyFlags)
    {
        if (toCopyFlags is PQAnonymousOrderLayerInfo pqToClone) UpdatedFlags = pqToClone.UpdatedFlags;
    }

    IReusableObject<IPublishedOrder> ITransferState<IReusableObject<IPublishedOrder>>.CopyFrom
        (IReusableObject<IPublishedOrder> source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom((IAnonymousOrderLayerInfo?)source!, copyMergeFlags);

    IPublishedOrder ITransferState<IPublishedOrder>.CopyFrom(IPublishedOrder source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom((IAnonymousOrderLayerInfo?)source!, copyMergeFlags);

    public override PQAnonymousOrderLayerInfo CopyFrom(IAnonymousOrderLayerInfo? source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (source is null) return this;
        if (source is not IPQAnonymousOrderLayerInfo pqAnonOrderLyrInfo)
        {
            OrderId         = source.OrderId;
            OrderType       = source.OrderType;
            TypeFlags       = source.TypeFlags;
            OrderLayerFlags = source.OrderLayerFlags;
            CreatedTime     = source.CreatedTime;
            UpdateTime      = source.UpdateTime;
            TrackingId      = source.TrackingId;

            OrderLifeCycleState  = source.OrderLifeCycleState;
            OrderDisplayVolume   = source.OrderDisplayVolume;
            OrderRemainingVolume = source.OrderRemainingVolume;
        }
        else
        {
            var isFullReplace = copyMergeFlags.HasFullReplace();

            if (pqAnonOrderLyrInfo.IsOrderIdUpdated || isFullReplace)
            {
                IsOrderIdUpdated = true;

                OrderId = pqAnonOrderLyrInfo.OrderId;
            }
            if (pqAnonOrderLyrInfo.IsOrderTypeUpdated || isFullReplace)
            {
                IsOrderTypeUpdated = true;

                OrderType = pqAnonOrderLyrInfo.OrderType;
            }
            if (pqAnonOrderLyrInfo.IsTypeFlagsUpdated || isFullReplace)
            {
                IsTypeFlagsUpdated = true;

                TypeFlags = pqAnonOrderLyrInfo.TypeFlags;
            }
            if (pqAnonOrderLyrInfo.IsOrderLifecycleStateUpdated || isFullReplace)
            {
                IsOrderLifecycleStateUpdated = true;

                OrderLifeCycleState = pqAnonOrderLyrInfo.OrderLifeCycleState;
            }
            if (pqAnonOrderLyrInfo.IsOrderLayerFlagsUpdated || isFullReplace)
            {
                IsOrderLayerFlagsUpdated = true;

                OrderLayerFlags = pqAnonOrderLyrInfo.OrderLayerFlags;
            }
            if (pqAnonOrderLyrInfo.IsCreatedTimeDateUpdated || isFullReplace)
            {
                IsCreatedTimeDateUpdated = true;

                CreatedTime = pqAnonOrderLyrInfo.CreatedTime;
            }
            if (pqAnonOrderLyrInfo.IsCreatedTimeSub2MinUpdated || isFullReplace)
            {
                IsCreatedTimeSub2MinUpdated = true;

                CreatedTime = pqAnonOrderLyrInfo.CreatedTime;
            }
            if (pqAnonOrderLyrInfo.IsUpdateTimeDateUpdated || isFullReplace)
            {
                IsUpdateTimeDateUpdated = true;

                UpdateTime = pqAnonOrderLyrInfo.UpdateTime;
            }
            if (pqAnonOrderLyrInfo.IsUpdateTimeSub2MinUpdated || isFullReplace)
            {
                IsUpdateTimeSub2MinUpdated = true;

                UpdateTime = pqAnonOrderLyrInfo.UpdateTime;
            }
            if (pqAnonOrderLyrInfo.IsOrderVolumeUpdated || isFullReplace)
            {
                IsOrderVolumeUpdated = true;

                OrderDisplayVolume = ((IPublishedOrder)pqAnonOrderLyrInfo).OrderDisplayVolume;
            }

            if (pqAnonOrderLyrInfo.IsOrderRemainingVolumeUpdated || isFullReplace)
            {
                IsOrderRemainingVolumeUpdated = true;

                OrderRemainingVolume = pqAnonOrderLyrInfo.OrderRemainingVolume;
            }

            if (pqAnonOrderLyrInfo.IsTrackingIdUpdated || isFullReplace)
            {
                IsTrackingIdUpdated = true;

                TrackingId = pqAnonOrderLyrInfo.TrackingId;
            }

            if (isFullReplace) UpdatedFlags = (source as PQAnonymousOrderLayerInfo)?.UpdatedFlags ?? UpdatedFlags;
        }

        return this;
    }

    IPublishedOrder ICloneable<IPublishedOrder>.Clone() => Clone();

    public override PQAnonymousOrderLayerInfo Clone() =>
        Recycler?.Borrow<PQAnonymousOrderLayerInfo>().CopyFrom(this) ?? new PQAnonymousOrderLayerInfo(this);

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent((IAnonymousOrderLayerInfo?)obj, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = OrderId;
            hashCode = ((int)OrderType * 397) ^ hashCode;
            hashCode = ((int)TypeFlags * 397) ^ hashCode;
            hashCode = ((int)OrderLifeCycleState * 397) ^ hashCode;
            hashCode = ((int)OrderLayerFlags * 397) ^ hashCode;
            hashCode = (CreatedTime.GetHashCode() * 397) ^ hashCode;
            hashCode = (UpdateTime.GetHashCode() * 397) ^ hashCode;
            hashCode = (OrderDisplayVolume.GetHashCode() * 397) ^ hashCode;
            hashCode = (OrderRemainingVolume.GetHashCode() * 397) ^ hashCode;
            hashCode = ((int)TrackingId * 397) ^ hashCode;
            return hashCode;
        }
    }

    protected string PQAnonymousOrderLayerInfoToStringMembers =>
        $"{nameof(OrderId)}: {OrderId}, {nameof(OrderType)}: {OrderType}, {nameof(TypeFlags)}: {TypeFlags} , {nameof(OrderLayerFlags)}: {OrderLayerFlags}, " +
        $"{nameof(CreatedTime)}: {CreatedTime}, {nameof(OrderLifeCycleState)}: {OrderLifeCycleState}, {nameof(UpdateTime)}: {UpdateTime}, " +
        $"{nameof(OrderDisplayVolume)}: {OrderDisplayVolume:N2}, {nameof(OrderRemainingVolume)}: {OrderRemainingVolume:N2}, {nameof(TrackingId)}: {TrackingId}";

    protected string UpdatedFlagsToString => $"{nameof(UpdatedFlags)}: {UpdatedFlags}";

    public override string ToString() => $"{nameof(PQAnonymousOrderLayerInfo)}{{{PQAnonymousOrderLayerInfoToStringMembers}, {UpdatedFlagsToString}}}";
}
