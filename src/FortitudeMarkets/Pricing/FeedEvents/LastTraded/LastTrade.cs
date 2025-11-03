// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text.Json.Serialization;
using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;

#endregion

// ReSharper disable DoNotCallOverridableMethodsInConstructor

namespace FortitudeMarkets.Pricing.FeedEvents.LastTraded;

public class LastTrade : ReusableObject<IMutableLastTrade>, IMutableLastTrade
{
    public LastTrade() => TradeTime = default;

    public LastTrade
    (uint tradeId = 0, uint batchId = 0, decimal tradePrice = 0m, DateTime? tradeDateTime = null
      , LastTradedTypeFlags tradeTypeFlags = LastTradedTypeFlags.None, LastTradedLifeCycleFlags tradeLifecycleStatus = LastTradedLifeCycleFlags.None
      , DateTime? firstNotifiedTime = null, DateTime? adapterReceivedTime = null, DateTime? updateTime = null)
    {
        TradeId        = tradeId;
        BatchId        = batchId;
        TradeTime      = tradeDateTime ?? default;
        TradePrice     = tradePrice;
        TradeTypeFlags = tradeTypeFlags;
        UpdateTime     = updateTime ?? default;

        TradeLifeCycleStatus = tradeLifecycleStatus;
        FirstNotifiedTime    = firstNotifiedTime ?? default;
        AdapterReceivedTime  = adapterReceivedTime ?? default;
    }

    public LastTrade(ILastTrade toClone)
    {
        TradeId    = toClone.TradeId;
        BatchId    = toClone.BatchId;
        TradeTime  = toClone.TradeTime;
        TradePrice = toClone.TradePrice;
        UpdateTime = toClone.UpdateTime;

        TradeTypeFlags       = toClone.TradeTypeFlags;
        FirstNotifiedTime    = toClone.FirstNotifiedTime;
        TradeLifeCycleStatus = toClone.TradeLifeCycleStatus;
        AdapterReceivedTime  = toClone.AdapterReceivedTime;
    }

    [JsonIgnore] public virtual LastTradeType   LastTradeType           => LastTradeType.Price;
    [JsonIgnore] public virtual LastTradedFlags SupportsLastTradedFlags => LastTradedFlags.LastTradedPrice | LastTradedFlags.LastTradedTime;


    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTime TradeTime { get; set; }


    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public virtual decimal TradePrice { get; set; }

    public LastTradedTypeFlags      TradeTypeFlags       { get; set; }
    public LastTradedLifeCycleFlags TradeLifeCycleStatus { get; set; }

    public uint     TradeId             { get; set; }
    public uint     BatchId             { get; set; }
    public DateTime FirstNotifiedTime   { get; set; }
    public DateTime AdapterReceivedTime { get; set; }
    public DateTime UpdateTime          { get; set; }


    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public virtual bool IsEmpty
    {
        get =>
            TradeId == 0
         && BatchId == 0
         && TradeTime == default
         && TradePrice == 0m
         && TradeTypeFlags == LastTradedTypeFlags.None
         && FirstNotifiedTime == default
         && AdapterReceivedTime == default
         && UpdateTime == default;
        set
        {
            if (!value) return;
            ResetWithTracking();
        }
    }

    IMutableLastTrade ITrackableReset<IMutableLastTrade>.ResetWithTracking() => ResetWithTracking();

    public virtual LastTrade ResetWithTracking()
    {
        TradeId    = 0;
        BatchId    = 0;
        TradeTime  = default;
        UpdateTime = default;
        TradePrice = 0m;

        TradeTypeFlags       = LastTradedTypeFlags.None;
        TradeLifeCycleStatus = LastTradedLifeCycleFlags.None;
        FirstNotifiedTime    = default;
        AdapterReceivedTime  = default;
        return this;
    }

    public override void StateReset()
    {
        ResetWithTracking();
        base.StateReset();
    }

    public override LastTrade CopyFrom(IMutableLastTrade source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        return CopyFrom(source, copyMergeFlags);
    }

    public IReusableObject<ILastTrade> CopyFrom(IReusableObject<ILastTrade> source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default) =>
        CopyFrom((ILastTrade)source, copyMergeFlags);

    ILastTrade ITransferState<ILastTrade>.CopyFrom(ILastTrade source, CopyMergeFlags copyMergeFlags) => CopyFrom(source, copyMergeFlags);

    public virtual LastTrade CopyFrom(ILastTrade source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        TradeId    = source.TradeId;
        BatchId    = source.BatchId;
        TradeTime  = source.TradeTime;
        TradePrice = source.TradePrice;
        UpdateTime = source.UpdateTime;

        TradeTypeFlags       = source.TradeTypeFlags;
        FirstNotifiedTime    = source.FirstNotifiedTime;
        TradeLifeCycleStatus = source.TradeLifeCycleStatus;
        AdapterReceivedTime  = source.AdapterReceivedTime;
        return this;
    }

    public override IMutableLastTrade Clone() => (LastTrade?)Recycler?.Borrow<LastTrade>().CopyFrom(this) ?? new LastTrade(this);

    object ICloneable.Clone() => Clone();

    ILastTrade ICloneable<ILastTrade>.Clone() => Clone();

    public virtual bool AreEquivalent(ILastTrade? other, bool exactTypes = false)
    {
        if (other == null) return false;
        if (exactTypes && other.GetType() != GetType()) return false;

        var tradeIdSame    = TradeId == other.TradeId;
        var batchIdSame    = BatchId == other.BatchId;
        var tradeDateSame  = TradeTime == other.TradeTime;
        var tradePriceSame = TradePrice == other.TradePrice;
        var updateTimeSame = UpdateTime == other.UpdateTime;

        var tradeTypeSame    = TradeTypeFlags == other.TradeTypeFlags;
        var firstNotTimeSame = FirstNotifiedTime == other.FirstNotifiedTime;
        var lifecycleSame    = TradeLifeCycleStatus == other.TradeLifeCycleStatus;
        var adapterRecvSame  = AdapterReceivedTime == other.AdapterReceivedTime;

        var allAreSame = tradeIdSame && batchIdSame && tradeDateSame && tradePriceSame && updateTimeSame
                      && tradeTypeSame && firstNotTimeSame && lifecycleSame && adapterRecvSame;

        return allAreSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent((ILastTrade?)obj, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = (int)TradeId;
            hashCode = ((int)BatchId * 397) ^ hashCode;
            hashCode = (TradePrice.GetHashCode() * 397) ^ hashCode;
            hashCode = (TradeTime.GetHashCode() * 397) ^ hashCode;
            hashCode = ((int)TradeLifeCycleStatus * 397) ^ hashCode;
            hashCode = ((int)TradeTypeFlags * 397) ^ hashCode;
            hashCode = (UpdateTime.GetHashCode() * 397) ^ hashCode;
            hashCode = (FirstNotifiedTime.GetHashCode() * 397) ^ hashCode;
            hashCode = (AdapterReceivedTime.GetHashCode() * 397) ^ hashCode;
            return hashCode;
        }
    }

    protected string LastTradeToStringMembers =>
        $"{nameof(TradeId)}: {TradeId}, {nameof(BatchId)}: {BatchId}, {nameof(TradePrice)}: {TradePrice:N5}, {nameof(TradeTime)}: {TradeTime:O}, " +
        $"{nameof(TradeTypeFlags)}: {TradeTypeFlags}, {nameof(TradeLifeCycleStatus)}: {TradeLifeCycleStatus}, {nameof(FirstNotifiedTime)}: {FirstNotifiedTime:O}, " +
        $"{nameof(AdapterReceivedTime)}: {AdapterReceivedTime:O}, {nameof(UpdateTime)}: {UpdateTime:O}";


    public override string ToString() => $"{nameof(LastTradeType)} {{{LastTradeToStringMembers}}}";
}
