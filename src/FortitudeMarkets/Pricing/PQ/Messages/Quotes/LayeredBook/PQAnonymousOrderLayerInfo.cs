// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text.Json.Serialization;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;
using FortitudeMarkets.Pricing.Quotes.LayeredBook;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Messages.Quotes.LayeredBook;

[Flags]
public enum OrderLayerInfoFlags : ushort
{
    None                   = 0x00_00
  , OrderIdFlag            = 0x00_01
  , OrderFlagsFlag         = 0x00_02
  , CreatedTimeDateFlag    = 0x00_04
  , CreatedTimeSubHourFlag = 0x00_08
  , UpdatedTimeDateFlag    = 0x00_10
  , UpdatedTimeSubHourFlag = 0x00_20
  , OrderVolumeFlag        = 0x00_40

  , OrderRemainingVolumeFlag = 0x00_80
  , TraderNameIdUpdatedFlag  = 0x01_00

  , CounterPartyNameIdUpdatedFlag = 0x02_00
}

public interface IPQAnonymousOrderLayerInfo : IMutableAnonymousOrderLayerInfo, IPQSupportsFieldUpdates<IAnonymousOrderLayerInfo>
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    bool IsOrderIdUpdated { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    bool IsOrderFlagsUpdated { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    bool IsCreatedTimeDateUpdated { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    bool IsCreatedTimeSub2MinUpdated { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    bool IsUpdatedTimeDateUpdated { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    bool IsUpdatedTimeSub2MinUpdated { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    bool IsOrderVolumeUpdated { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    bool IsOrderRemainingVolumeUpdated { get; set; }

    new IPQAnonymousOrderLayerInfo Clone();
}

public class PQAnonymousOrderLayerInfo : ReusableObject<IAnonymousOrderLayerInfo>, IPQAnonymousOrderLayerInfo
{
    private   DateTime        createdTime;
    protected int             NumUpdatesSinceEmpty = -1;
    private   LayerOrderFlags orderFlags;

    private int     orderId;
    private decimal orderVolume;
    private decimal remainingOrderVolume;

    protected OrderLayerInfoFlags UpdatedFlags;

    private DateTime updatedTime;

    public PQAnonymousOrderLayerInfo()
    {
        if (GetType() == typeof(PQAnonymousOrderLayerInfo)) NumUpdatesSinceEmpty = 0;
    }

    public PQAnonymousOrderLayerInfo
    (int orderId = 0, LayerOrderFlags orderFlags = LayerOrderFlags.None, DateTime createdTime = default, decimal orderVolume = 0m
      , DateTime? updatedTime = null
      , decimal? remainingVolume = null)
    {
        OrderId     = orderId;
        OrderFlags  = orderFlags;
        CreatedTime = createdTime;
        UpdatedTime = updatedTime ?? createdTime;
        OrderVolume = orderVolume;

        OrderRemainingVolume = remainingVolume ?? orderVolume;
        if (GetType() == typeof(PQAnonymousOrderLayerInfo)) NumUpdatesSinceEmpty = 0;
    }

    public PQAnonymousOrderLayerInfo(IAnonymousOrderLayerInfo toClone)
    {
        OrderId     = toClone.OrderId;
        OrderFlags  = toClone.OrderFlags;
        CreatedTime = toClone.CreatedTime;
        UpdatedTime = toClone.UpdatedTime;
        OrderVolume = toClone.OrderVolume;

        OrderRemainingVolume = toClone.OrderRemainingVolume;

        SetFlagsSame(toClone);
        if (GetType() == typeof(PQAnonymousOrderLayerInfo)) NumUpdatesSinceEmpty = 0;
    }

    protected string PQAnonymousOrderLayerInfoToStringMembers =>
        $"{nameof(OrderId)}: {OrderId}, {nameof(OrderFlags)}: {OrderFlags}, " +
        $"{nameof(CreatedTime)}: {CreatedTime}, {nameof(UpdatedTime)}: {UpdatedTime}, {nameof(OrderVolume)}: {OrderVolume:N2}, " +
        $"{nameof(OrderRemainingVolume)}: {OrderRemainingVolume:N2}";

    protected string UpdatedFlagsToString => $"{nameof(UpdatedFlags)}: {UpdatedFlags}";

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

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public LayerOrderFlags OrderFlags
    {
        get => orderFlags;
        set
        {
            IsOrderFlagsUpdated |= orderFlags != value || NumUpdatesSinceEmpty == 0;
            orderFlags          =  value;
        }
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTime CreatedTime
    {
        get => createdTime;
        set
        {
            IsCreatedTimeDateUpdated    |= createdTime.Get2MinIntervalsFromUnixEpoch() != value.Get2MinIntervalsFromUnixEpoch() || NumUpdatesSinceEmpty == 0;
            IsCreatedTimeSub2MinUpdated |= createdTime.GetSub2MinComponent() != value.GetSub2MinComponent() || NumUpdatesSinceEmpty == 0;
            createdTime                 =  value;
        }
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTime UpdatedTime
    {
        get => updatedTime;
        set
        {
            IsUpdatedTimeDateUpdated    |= updatedTime.Get2MinIntervalsFromUnixEpoch() != value.Get2MinIntervalsFromUnixEpoch() || NumUpdatesSinceEmpty == 0;
            IsUpdatedTimeSub2MinUpdated |= updatedTime.GetSub2MinComponent() != value.GetSub2MinComponent() || NumUpdatesSinceEmpty == 0;
            updatedTime                 =  value;
        }
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public decimal OrderVolume
    {
        get => orderVolume;
        set
        {
            IsOrderVolumeUpdated |= orderVolume != value || NumUpdatesSinceEmpty == 0;
            orderVolume          =  value;
        }
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public decimal OrderRemainingVolume
    {
        get => remainingOrderVolume;
        set
        {
            IsOrderRemainingVolumeUpdated |= remainingOrderVolume != value || NumUpdatesSinceEmpty == 0;
            remainingOrderVolume          =  value;
        }
    }

    public bool IsOrderIdUpdated
    {
        get => (UpdatedFlags & OrderLayerInfoFlags.OrderIdFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= OrderLayerInfoFlags.OrderIdFlag;

            else if (IsOrderIdUpdated) UpdatedFlags ^= OrderLayerInfoFlags.OrderIdFlag;
        }
    }


    public bool IsOrderFlagsUpdated
    {
        get => (UpdatedFlags & OrderLayerInfoFlags.OrderFlagsFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= OrderLayerInfoFlags.OrderFlagsFlag;

            else if (IsOrderFlagsUpdated) UpdatedFlags ^= OrderLayerInfoFlags.OrderFlagsFlag;
        }
    }


    public bool IsCreatedTimeSub2MinUpdated
    {
        get => (UpdatedFlags & OrderLayerInfoFlags.CreatedTimeSubHourFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= OrderLayerInfoFlags.CreatedTimeSubHourFlag;

            else if (IsCreatedTimeSub2MinUpdated) UpdatedFlags ^= OrderLayerInfoFlags.CreatedTimeSubHourFlag;
        }
    }
    public bool IsCreatedTimeDateUpdated
    {
        get => (UpdatedFlags & OrderLayerInfoFlags.CreatedTimeDateFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= OrderLayerInfoFlags.CreatedTimeDateFlag;

            else if (IsCreatedTimeDateUpdated) UpdatedFlags ^= OrderLayerInfoFlags.CreatedTimeDateFlag;
        }
    }


    public bool IsUpdatedTimeSub2MinUpdated
    {
        get => (UpdatedFlags & OrderLayerInfoFlags.UpdatedTimeSubHourFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= OrderLayerInfoFlags.UpdatedTimeSubHourFlag;

            else if (IsUpdatedTimeSub2MinUpdated) UpdatedFlags ^= OrderLayerInfoFlags.UpdatedTimeSubHourFlag;
        }
    }

    public bool IsUpdatedTimeDateUpdated
    {
        get => (UpdatedFlags & OrderLayerInfoFlags.UpdatedTimeDateFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= OrderLayerInfoFlags.UpdatedTimeDateFlag;

            else if (IsUpdatedTimeDateUpdated) UpdatedFlags ^= OrderLayerInfoFlags.UpdatedTimeDateFlag;
        }
    }


    public bool IsOrderVolumeUpdated
    {
        get => (UpdatedFlags & OrderLayerInfoFlags.OrderVolumeFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= OrderLayerInfoFlags.OrderVolumeFlag;

            else if (IsOrderVolumeUpdated) UpdatedFlags ^= OrderLayerInfoFlags.OrderVolumeFlag;
        }
    }


    public bool IsOrderRemainingVolumeUpdated
    {
        get => (UpdatedFlags & OrderLayerInfoFlags.OrderRemainingVolumeFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= OrderLayerInfoFlags.OrderRemainingVolumeFlag;

            else if (IsOrderRemainingVolumeUpdated) UpdatedFlags ^= OrderLayerInfoFlags.OrderRemainingVolumeFlag;
        }
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public virtual bool HasUpdates
    {
        get => UpdatedFlags != OrderLayerInfoFlags.None;
        set
        {
            if (value)
            {
                UpdatedFlags = UpdatedFlags.AllFlags();
                return;
            }
            else
            {
                UpdatedFlags = OrderLayerInfoFlags.None;
            }
        }
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public virtual bool IsEmpty
    {
        get =>
            OrderId == 0 && OrderFlags == LayerOrderFlags.None && CreatedTime == default && UpdatedTime == default && OrderVolume == 0m &&
            OrderRemainingVolume == 0m;
        set
        {
            if (!value) return;
            OrderId     = 0;
            OrderFlags  = LayerOrderFlags.None;
            CreatedTime = default;
            UpdatedTime = default;
            UpdatedTime = default;
            OrderVolume = 0m;

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

    public virtual int UpdateField(PQFieldUpdate fieldUpdate)
    {
        switch (fieldUpdate.Id)
        {
            case PQQuoteFields.OrderId:
                IsOrderIdUpdated = true; // incase of reset and sending 0;
                OrderId          = (int)fieldUpdate.Payload;
                return 0;
            case PQQuoteFields.OrderFlags:
                IsOrderFlagsUpdated = true; // incase of reset and sending 0;
                OrderFlags          = (LayerOrderFlags)fieldUpdate.Payload;
                return 0;
            case PQQuoteFields.OrderCreatedDate:
                IsCreatedTimeDateUpdated = true; // incase of reset and sending 0;
                PQFieldConverters.Update2MinuteIntervalsFromUnixEpoch(ref createdTime, fieldUpdate.Payload);
                if (createdTime == DateTime.UnixEpoch) createdTime = default;
                return 0;
            case PQQuoteFields.OrderCreatedSub2MinTime:
                IsCreatedTimeSub2MinUpdated = true; // incase of reset and sending 0;
                PQFieldConverters.UpdateSub2MinComponent
                    (ref createdTime, fieldUpdate.Flag.AppendScaleFlagsToUintToMakeLong(fieldUpdate.Payload));
                if (createdTime == DateTime.UnixEpoch) createdTime = default;
                return 0;
            case PQQuoteFields.OrderUpdatedDate:
                IsUpdatedTimeDateUpdated = true; // incase of reset and sending 0;
                PQFieldConverters.Update2MinuteIntervalsFromUnixEpoch(ref updatedTime, fieldUpdate.Payload);
                if (updatedTime == DateTime.UnixEpoch) updatedTime = default;
                return 0;
            case PQQuoteFields.OrderUpdatedSub2MinTime:
                IsUpdatedTimeSub2MinUpdated = true; // incase of reset and sending 0;
                PQFieldConverters.UpdateSub2MinComponent
                    (ref updatedTime, fieldUpdate.Flag.AppendScaleFlagsToUintToMakeLong(fieldUpdate.Payload));
                if (updatedTime == DateTime.UnixEpoch) updatedTime = default;
                return 0;
            case PQQuoteFields.OrderVolume:
                IsOrderVolumeUpdated = true; // incase of reset and sending 0;
                OrderVolume          = PQScaling.Unscale(fieldUpdate.Payload, fieldUpdate.Flag);
                return 0;
            case PQQuoteFields.OrderRemainingVolume:
                IsOrderRemainingVolumeUpdated = true; // incase of reset and sending 0;
                OrderRemainingVolume          = PQScaling.Unscale(fieldUpdate.Payload, fieldUpdate.Flag);
                return 0;
        }
        return -1;
    }

    public virtual IEnumerable<PQFieldUpdate> GetDeltaUpdateFields
    (DateTime snapShotTime, StorageFlags messageFlags
      , IPQPriceVolumePublicationPrecisionSettings? quotePublicationPrecisionSettings = null)
    {
        var updatedOnly = (messageFlags & StorageFlags.Complete) == 0;
        if (!updatedOnly || IsOrderIdUpdated) yield return new PQFieldUpdate(PQQuoteFields.OrderId, (uint)OrderId);
        if (!updatedOnly || IsOrderFlagsUpdated) yield return new PQFieldUpdate(PQQuoteFields.OrderFlags, (uint)OrderFlags);

        if (!updatedOnly || IsCreatedTimeDateUpdated)
            yield return new PQFieldUpdate(PQQuoteFields.OrderCreatedDate, createdTime.Get2MinIntervalsFromUnixEpoch());
        if (!updatedOnly || IsCreatedTimeSub2MinUpdated)
        {
            var extended = createdTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var value);
            yield return new PQFieldUpdate(PQQuoteFields.OrderCreatedSub2MinTime, value, extended);
        }
        if (!updatedOnly || IsUpdatedTimeDateUpdated)
            yield return new PQFieldUpdate(PQQuoteFields.OrderUpdatedDate, updatedTime.Get2MinIntervalsFromUnixEpoch());

        if (!updatedOnly || IsUpdatedTimeSub2MinUpdated)
        {
            var extended = updatedTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var value);
            yield return new PQFieldUpdate(PQQuoteFields.OrderUpdatedSub2MinTime, value, extended);
        }
        if (!updatedOnly || IsOrderVolumeUpdated)
            yield return new PQFieldUpdate(PQQuoteFields.OrderVolume, OrderVolume,
                                           quotePublicationPrecisionSettings?.VolumeScalingPrecision ?? (PQFieldFlags)6);
        if (!updatedOnly || IsOrderRemainingVolumeUpdated)
            yield return new PQFieldUpdate(PQQuoteFields.OrderRemainingVolume, OrderRemainingVolume,
                                           quotePublicationPrecisionSettings?.VolumeScalingPrecision ?? (PQFieldFlags)6);
    }

    public override void StateReset()
    {
        OrderId     = 0;
        OrderFlags  = LayerOrderFlags.None;
        CreatedTime = default;
        UpdatedTime = default;
        OrderVolume = 0m;

        OrderRemainingVolume = 0m;
        UpdatedFlags         = OrderLayerInfoFlags.None;
        NumUpdatesSinceEmpty = 0;
        base.StateReset();
    }

    object ICloneable.Clone() => Clone();

    IPQAnonymousOrderLayerInfo IPQAnonymousOrderLayerInfo.Clone() => Clone();

    IMutableAnonymousOrderLayerInfo IMutableAnonymousOrderLayerInfo.Clone() => Clone();

    public bool AreEquivalent(IMutableAnonymousOrderLayerInfo? other, bool exactTypes = false) =>
        AreEquivalent((IAnonymousOrderLayerInfo?)other, exactTypes);

    public virtual bool AreEquivalent(IAnonymousOrderLayerInfo? other, bool exactTypes = false)
    {
        if (other == null) return false;
        if (exactTypes && other.GetType() != GetType()) return false;
        var orderIdsSame    = OrderId == other.OrderId;
        var orderFlagsSame  = OrderFlags == other.OrderFlags;
        var createdSame     = CreatedTime == other.CreatedTime;
        var updatedTimeSame = UpdatedTime == other.UpdatedTime;
        var volumeSame      = OrderVolume == other.OrderVolume;

        var remainingVolumeSame = OrderRemainingVolume == other.OrderRemainingVolume;

        var updatedSame = true;
        if (exactTypes)
        {
            var pqTraderLayerInfo = (PQAnonymousOrderLayerInfo)other;
            updatedSame = UpdatedFlags == pqTraderLayerInfo.UpdatedFlags;
        }

        return orderIdsSame && orderFlagsSame && createdSame && updatedTimeSame && volumeSame && remainingVolumeSame && updatedSame;
    }

    protected void SetFlagsSame(IAnonymousOrderLayerInfo toCopyFlags)
    {
        if (toCopyFlags is PQAnonymousOrderLayerInfo pqToClone) UpdatedFlags = pqToClone.UpdatedFlags;
    }

    public override PQAnonymousOrderLayerInfo CopyFrom(IAnonymousOrderLayerInfo? source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (source is null) return this;
        if (source is not IPQAnonymousOrderLayerInfo pqAnonOrderLyrInfo)
        {
            OrderId     = source.OrderId;
            OrderFlags  = source.OrderFlags;
            CreatedTime = source.CreatedTime;
            UpdatedTime = source.UpdatedTime;
            OrderVolume = source.OrderVolume;

            OrderRemainingVolume = source.OrderRemainingVolume;
        }
        else
        {
            var isFullReplace = copyMergeFlags.HasFullReplace();

            if (pqAnonOrderLyrInfo.IsOrderIdUpdated || isFullReplace) OrderId       = pqAnonOrderLyrInfo.OrderId;
            if (pqAnonOrderLyrInfo.IsOrderFlagsUpdated || isFullReplace) OrderFlags = pqAnonOrderLyrInfo.OrderFlags;
            if (pqAnonOrderLyrInfo.IsCreatedTimeDateUpdated || pqAnonOrderLyrInfo.IsCreatedTimeSub2MinUpdated || isFullReplace)
                CreatedTime = pqAnonOrderLyrInfo.CreatedTime;
            if (pqAnonOrderLyrInfo.IsUpdatedTimeDateUpdated || pqAnonOrderLyrInfo.IsUpdatedTimeSub2MinUpdated || isFullReplace)
                UpdatedTime = pqAnonOrderLyrInfo.UpdatedTime;
            if (pqAnonOrderLyrInfo.IsOrderVolumeUpdated || isFullReplace) OrderVolume = pqAnonOrderLyrInfo.OrderVolume;

            if (pqAnonOrderLyrInfo.IsOrderRemainingVolumeUpdated || isFullReplace) OrderRemainingVolume = pqAnonOrderLyrInfo.OrderRemainingVolume;

            if (isFullReplace) UpdatedFlags = (source as PQAnonymousOrderLayerInfo)?.UpdatedFlags ?? UpdatedFlags;
        }

        return this;
    }

    public override PQAnonymousOrderLayerInfo Clone() =>
        Recycler?.Borrow<PQAnonymousOrderLayerInfo>().CopyFrom(this) ?? new PQAnonymousOrderLayerInfo(this);

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent((IAnonymousOrderLayerInfo?)obj, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = OrderId;
            hashCode = ((int)OrderFlags * 397) ^ hashCode;
            hashCode = (CreatedTime.GetHashCode() * 397) ^ hashCode;
            hashCode = (UpdatedTime.GetHashCode() * 397) ^ hashCode;
            hashCode = (OrderVolume.GetHashCode() * 397) ^ hashCode;
            hashCode = (OrderRemainingVolume.GetHashCode() * 397) ^ hashCode;
            return hashCode;
        }
    }

    public override string ToString() => $"{nameof(PQAnonymousOrderLayerInfo)}{{{PQAnonymousOrderLayerInfoToStringMembers}, {UpdatedFlagsToString}}}";
}
