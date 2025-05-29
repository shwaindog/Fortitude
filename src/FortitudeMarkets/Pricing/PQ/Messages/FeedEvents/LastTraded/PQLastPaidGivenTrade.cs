// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text.Json.Serialization;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.FeedEvents.LastTraded;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.LastTraded;

public interface IPQLastPaidGivenTrade : IPQLastTrade, IMutableLastPaidGivenTrade, ITrackableReset<IPQLastPaidGivenTrade>, ICloneable<IPQLastPaidGivenTrade>
{
    bool IsOrderIdUpdated { get; set; }
    bool IsWasPaidUpdated      { get; set; }
    bool IsWasGivenUpdated     { get; set; }
    bool IsTradeVolumeUpdated  { get; set; }

    new IPQLastPaidGivenTrade Clone();

    new IPQLastPaidGivenTrade ResetWithTracking();
}

[Flags]
public enum LastTradeBooleanFlags : byte
{
    None     = 0x00
  , WasGiven = 0x01
  , WasPaid  = 0x02
}

public class PQLastPaidGivenTrade : PQLastTrade, IPQLastPaidGivenTrade
{
    protected LastTradeBooleanFlags LastTradeBooleanFlags;

    private decimal tradeVolume;
    private uint    orderId;

    public PQLastPaidGivenTrade()
    {
        if (GetType() == typeof(PQLastPaidGivenTrade)) SequenceId = 0;
    }

    public PQLastPaidGivenTrade
    (uint tradeId = 0, uint batchId = 0, decimal tradePrice = 0m, DateTime? tradeDateTime = null, decimal tradeVolume = 0m
       , uint orderId = 0, bool wasPaid = false, bool wasGiven = false, LastTradedTypeFlags tradeTypeFlags = LastTradedTypeFlags.None
      , LastTradedLifeCycleFlags tradeLifecycleStatus = LastTradedLifeCycleFlags.None
      , DateTime? firstNotifiedTime = null, DateTime? adapterReceivedTime = null, DateTime? updateTime = null) 
        : base(tradeId, batchId, tradePrice, tradeDateTime, tradeTypeFlags, tradeLifecycleStatus, firstNotifiedTime, adapterReceivedTime, updateTime)
    {
        OrderId     = orderId;
        TradeVolume = tradeVolume;
        WasPaid     = wasPaid;
        WasGiven    = wasGiven;

        if (GetType() == typeof(PQLastPaidGivenTrade)) SequenceId = 0;
    }

    public PQLastPaidGivenTrade(ILastTrade toClone) : base(toClone)
    {
        if (toClone is ILastPaidGivenTrade lastPaidGivenTrade)
        {
            OrderId = lastPaidGivenTrade.OrderId;
            TradeVolume = lastPaidGivenTrade.TradeVolume;
            WasPaid     = lastPaidGivenTrade.WasPaid;
            WasGiven    = lastPaidGivenTrade.WasGiven;
        }

        SetFlagsSame(toClone);

        if (GetType() == typeof(PQLastPaidGivenTrade)) SequenceId = 0;
    }

    [JsonIgnore] public override LastTradeType LastTradeType => LastTradeType.PricePaidOrGivenVolume;

    [JsonIgnore]
    public override LastTradedFlags SupportsLastTradedFlags =>
        LastTradedFlags.PaidOrGiven | LastTradedFlags.LastTradedVolume | base.SupportsLastTradedFlags;


    public uint OrderId
    {
        get => orderId;
        set
        {
            IsOrderIdUpdated |= orderId != value || SequenceId == 0;
            orderId          =  value;
        }
    }

    [JsonIgnore]
    public bool IsOrderIdUpdated
    {
        get => (UpdatedFlags & LastTradeUpdated.TradeOrderIdUpdated) > 0;
        set
        {
            if (value)
                UpdatedFlags |= LastTradeUpdated.TradeOrderIdUpdated;

            else if (IsOrderIdUpdated) UpdatedFlags ^= LastTradeUpdated.TradeOrderIdUpdated;
        }
    }


    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool WasPaid
    {
        get => (LastTradeBooleanFlags & LastTradeBooleanFlags.WasPaid) > 0;
        set
        {
            IsWasPaidUpdated |= WasPaid != value || SequenceId == 0;
            if (value)
                LastTradeBooleanFlags |= LastTradeBooleanFlags.WasPaid;

            else if (WasPaid) LastTradeBooleanFlags ^= LastTradeBooleanFlags.WasPaid;
        }
    }

    [JsonIgnore]
    public bool IsWasPaidUpdated
    {
        get => (UpdatedFlags & LastTradeUpdated.TradeWasPaidUpdated) > 0;
        set
        {
            if (value)
                UpdatedFlags |= LastTradeUpdated.TradeWasPaidUpdated;

            else if (IsWasPaidUpdated) UpdatedFlags ^= LastTradeUpdated.TradeWasPaidUpdated;
        }
    }


    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool WasGiven
    {
        get => (LastTradeBooleanFlags & LastTradeBooleanFlags.WasGiven) > 0;
        set
        {
            IsWasGivenUpdated |= WasGiven != value || SequenceId == 0;
            if (value)
                LastTradeBooleanFlags |= LastTradeBooleanFlags.WasGiven;

            else if (WasGiven) LastTradeBooleanFlags ^= LastTradeBooleanFlags.WasGiven;
        }
    }

    [JsonIgnore]
    public bool IsWasGivenUpdated
    {
        get => (UpdatedFlags & LastTradeUpdated.TradeWasGivenUpdated) > 0;
        set
        {
            if (value)
                UpdatedFlags |= LastTradeUpdated.TradeWasGivenUpdated;

            else if (IsWasGivenUpdated) UpdatedFlags ^= LastTradeUpdated.TradeWasGivenUpdated;
        }
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public decimal TradeVolume
    {
        get => tradeVolume;
        set
        {
            IsTradeVolumeUpdated |= tradeVolume != value || SequenceId == 0;
            tradeVolume          =  value;
        }
    }

    [JsonIgnore]
    public bool IsTradeVolumeUpdated
    {
        get => (UpdatedFlags & LastTradeUpdated.TradeVolumeUpdated) > 0;
        set
        {
            if (value)
                UpdatedFlags |= LastTradeUpdated.TradeVolumeUpdated;

            else if (IsTradeVolumeUpdated) UpdatedFlags ^= LastTradeUpdated.TradeVolumeUpdated;
        }
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public override bool IsEmpty
    {
        get => base.IsEmpty && OrderId == 0 && WasPaid == false && WasGiven == false && TradeVolume == 0m;
        set => base.IsEmpty = value;
    }

    IMutableLastTrade ITrackableReset<IMutableLastTrade>.ResetWithTracking() => ResetWithTracking();

    IPQLastTrade ITrackableReset<IPQLastTrade>.ResetWithTracking() => ResetWithTracking();


    IPQLastPaidGivenTrade ITrackableReset<IPQLastPaidGivenTrade>.ResetWithTracking() => ResetWithTracking();

    IPQLastPaidGivenTrade IPQLastPaidGivenTrade.ResetWithTracking() => ResetWithTracking();

    public override PQLastPaidGivenTrade ResetWithTracking()
    {
        OrderId     = 0;
        WasGiven    = WasPaid = false;
        TradeVolume = 0m;

        base.ResetWithTracking();
        return this;
    }

    public override IEnumerable<PQFieldUpdate> GetDeltaUpdateFields
    (DateTime snapShotTime, StorageFlags messageFlags,
        IPQPriceVolumePublicationPrecisionSettings? quotePublicationPrecisionSetting = null)
    {
        var updatedOnly = (messageFlags & StorageFlags.Complete) == 0;
        foreach (var deltaUpdateField in base.GetDeltaUpdateFields(snapShotTime, messageFlags,
                                                                   quotePublicationPrecisionSetting))
            yield return deltaUpdateField;
        if (!updatedOnly || IsOrderIdUpdated)
            yield return new PQFieldUpdate(PQFeedFields.LastTradedTickTrades, PQTradingSubFieldKeys.LastTradedOrderId, OrderId);
        if (!updatedOnly || IsBooleanFlagsChanged())
            yield return new PQFieldUpdate(PQFeedFields.LastTradedTickTrades, PQTradingSubFieldKeys.LastTradedBooleanFlags
                                         , (uint)LastTradeBooleanFlags);
        if (!updatedOnly || IsTradeVolumeUpdated)
            yield return new PQFieldUpdate(PQFeedFields.LastTradedTickTrades, PQTradingSubFieldKeys.LastTradedTradeVolume, TradeVolume,
                                           quotePublicationPrecisionSetting?.VolumeScalingPrecision ?? (PQFieldFlags)6);
    }

    public override int UpdateField(PQFieldUpdate pqFieldUpdate)
    {
        // assume the parent has already forwarded this through to the correct last trade
        switch (pqFieldUpdate.TradingSubId)
        {
            case PQTradingSubFieldKeys.LastTradedOrderId:
                IsOrderIdUpdated = true;
                OrderId          = pqFieldUpdate.Payload;
                return 0;
            case PQTradingSubFieldKeys.LastTradedBooleanFlags:
                var boolValues = pqFieldUpdate.Payload;
                WasGiven = ((LastTradeBooleanFlags)boolValues & LastTradeBooleanFlags.WasGiven) == LastTradeBooleanFlags.WasGiven;
                WasPaid  = ((LastTradeBooleanFlags)boolValues & LastTradeBooleanFlags.WasPaid) == LastTradeBooleanFlags.WasPaid;
                return 0;
            case PQTradingSubFieldKeys.LastTradedTradeVolume:
                IsTradeVolumeUpdated = true;

                TradeVolume          = PQScaling.Unscale(pqFieldUpdate.Payload, pqFieldUpdate.Flag);
                return 0;
        }

        return base.UpdateField(pqFieldUpdate);
    }

    ILastPaidGivenTrade ILastPaidGivenTrade.Clone() => Clone();

    IMutableLastPaidGivenTrade IMutableLastPaidGivenTrade.Clone() => Clone();

    IPQLastPaidGivenTrade IPQLastPaidGivenTrade.Clone() => Clone();

    IPQLastPaidGivenTrade ICloneable<IPQLastPaidGivenTrade>.Clone() => Clone();

    public override PQLastPaidGivenTrade Clone() => 
        Recycler?.Borrow<PQLastPaidGivenTrade>().CopyFrom(this, CopyMergeFlags.FullReplace) ?? new PQLastPaidGivenTrade(this);

    public override PQLastPaidGivenTrade CopyFrom(ILastTrade source, CopyMergeFlags copyMergeFlags)
    {
        base.CopyFrom(source, copyMergeFlags);
        if (source is not ILastPaidGivenTrade lpgt) return this;
        var pqlpgt = source as IPQLastPaidGivenTrade;
        if (pqlpgt == null)
        {
            OrderId     = lpgt.OrderId;
            TradeVolume = lpgt.TradeVolume;
            WasGiven    = lpgt.WasGiven;
            WasPaid     = lpgt.WasPaid;
        }
        else 
        {
            var isFullReplace = copyMergeFlags.HasFullReplace();

            if (pqlpgt.IsOrderIdUpdated || isFullReplace)
            {
                IsOrderIdUpdated = true;

                OrderId      = pqlpgt.OrderId;
            }
            if (pqlpgt.IsTradeVolumeUpdated || isFullReplace)
            {
                IsTradeVolumeUpdated = true;

                TradeVolume = pqlpgt.TradeVolume;
            }
            if (pqlpgt.IsWasPaidUpdated || isFullReplace)
            {
                IsWasPaidUpdated = true;

                WasPaid         = pqlpgt.WasPaid;
            }
            if (pqlpgt.IsWasGivenUpdated || isFullReplace)
            {
                IsWasGivenUpdated = true;

                WasGiven       = pqlpgt.WasGiven;
            }

            if(isFullReplace) SetFlagsSame(pqlpgt);
        }

        return this;
    }

    public override bool AreEquivalent(ILastTrade? other, bool exactTypes = false)
    {
        if (other is not ILastPaidGivenTrade pqLastPaidGivenTrader) return false;

        var baseSame = base.AreEquivalent(other, exactTypes);
        
        var orderIdSame      = OrderId == pqLastPaidGivenTrader.OrderId;
        var traderVolumeSame = tradeVolume == pqLastPaidGivenTrader.TradeVolume;

        var wasPaidSame  = WasPaid == pqLastPaidGivenTrader.WasPaid;
        var wasGivenSame = WasGiven == pqLastPaidGivenTrader.WasGiven;
        var allAreSame = baseSame  && orderIdSame && traderVolumeSame && wasPaidSame && wasGivenSame;

        return allAreSame;
    }

    protected virtual bool IsBooleanFlagsChanged() => IsWasGivenUpdated || IsWasPaidUpdated;

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as ILastTrade, true);

    public override int GetHashCode()
    {
        unchecked
        {
            return (base.GetHashCode() * 397) ^ tradeVolume.GetHashCode();
        }
    }
    
    protected string PQLastPaidGivenTradeToStringMembers =>
        $"{PQLastTradeToStringMembers}, {nameof(OrderId)}: {OrderId}, {nameof(WasPaid)}: {WasPaid}, " +
        $"{nameof(WasGiven)}: {WasGiven}, {nameof(TradeVolume)}: {TradeVolume:N2}";

    public override string ToString() => $"{GetType().Name}({PQLastPaidGivenTradeToStringMembers}, {UpdatedFlagsToString})";
}
