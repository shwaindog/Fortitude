// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text.Json.Serialization;
using FortitudeCommon.Types.Mutable;

#endregion

namespace FortitudeMarkets.Pricing.FeedEvents.LastTraded;

public class LastPaidGivenTrade : LastTrade, IMutableLastPaidGivenTrade
{
    public LastPaidGivenTrade() { }

    public LastPaidGivenTrade
    (uint tradeId = 0, uint batchId = 0, decimal tradePrice = 0m, DateTime? tradeDateTime = null, decimal tradeVolume = 0m
       , uint orderId = 0, bool wasPaid = false, bool wasGiven = false, LastTradedTypeFlags tradeTypeFlags = LastTradedTypeFlags.None
      , LastTradedLifeCycleFlags tradeLifecycleStatus = LastTradedLifeCycleFlags.None
      , DateTime? firstNotifiedTime = null, DateTime? adapterReceivedTime = null, DateTime? updateTime = null) 
        : base(tradeId, batchId, tradePrice, tradeDateTime, tradeTypeFlags, tradeLifecycleStatus, firstNotifiedTime, adapterReceivedTime, updateTime)
    {
        OrderId     = orderId;
        WasPaid     = wasPaid;
        WasGiven    = wasGiven;
        TradeVolume = tradeVolume;
    }

    public LastPaidGivenTrade(ILastTrade toClone) : base(toClone)
    {
        if (toClone is ILastPaidGivenTrade lastPaidGivenTrade)
        {
            OrderId     = lastPaidGivenTrade.OrderId;
            WasPaid     = lastPaidGivenTrade.WasPaid;
            WasGiven    = lastPaidGivenTrade.WasGiven;
            TradeVolume = lastPaidGivenTrade.TradeVolume;
        }
    }

    [JsonIgnore] public override LastTradeType LastTradeType => LastTradeType.PricePaidOrGivenVolume;

    [JsonIgnore]
    public override LastTradedFlags SupportsLastTradedFlags =>
        LastTradedFlags.PaidOrGiven | LastTradedFlags.LastTradedVolume | base.SupportsLastTradedFlags;

    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public uint OrderId { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool WasPaid { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool WasGiven { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public decimal TradeVolume { get; set; }


    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public override bool IsEmpty
    {
        get => base.IsEmpty && OrderId == 0 && WasPaid == false && WasGiven == false && TradeVolume == 0m;
        set => base.IsEmpty = value;
    }

    public override LastPaidGivenTrade ResetWithTracking()
    {
        OrderId     = 0;
        TradeVolume = 0;
        WasPaid     = WasGiven = false;
        base.ResetWithTracking();

        return this;
    }

    public override ILastTrade CopyFrom(ILastTrade source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);
        if (source is ILastPaidGivenTrade lastPaidGivenTrade)
        {
            OrderId = lastPaidGivenTrade.OrderId;
            TradeVolume = lastPaidGivenTrade.TradeVolume;
            WasPaid     = lastPaidGivenTrade.WasPaid;
            WasGiven    = lastPaidGivenTrade.WasGiven;
        }

        return this;
    }

    public override IMutableLastTrade Clone() =>
        (IMutableLastTrade?)Recycler?.Borrow<LastPaidGivenTrade>().CopyFrom(this) ?? new LastPaidGivenTrade(this);

    ILastPaidGivenTrade ILastPaidGivenTrade.Clone() => (ILastPaidGivenTrade)Clone();

    IMutableLastPaidGivenTrade IMutableLastPaidGivenTrade.Clone() => (IMutableLastPaidGivenTrade)Clone();

    public override bool AreEquivalent(ILastTrade? other, bool exactTypes = false)
    {
        if (!(other is ILastPaidGivenTrade lastPaidGivenTrade)) return false;

        var baseSame        = base.AreEquivalent(other, exactTypes);
        var orderIdSame     = OrderId == lastPaidGivenTrade.OrderId;
        var wasPaidSame     = WasPaid == lastPaidGivenTrade.WasPaid;
        var wasGivenSame    = WasGiven == lastPaidGivenTrade.WasGiven;
        var tradeVolumeSame = TradeVolume == lastPaidGivenTrade.TradeVolume;

        var allAreSame = baseSame && orderIdSame && wasPaidSame && wasGivenSame && tradeVolumeSame;

        return allAreSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as ILastPaidGivenTrade, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = base.GetHashCode();
            hashCode = (hashCode * 397) ^ (int)OrderId;
            hashCode = (hashCode * 397) ^ WasPaid.GetHashCode();
            hashCode = (hashCode * 397) ^ WasGiven.GetHashCode();
            hashCode = (hashCode * 397) ^ TradeVolume.GetHashCode();
            return hashCode;
        }
    }

    protected string LastPaidGivenTradeToStringMembers =>
        $"{LastTradeToStringMembers}, {nameof(OrderId)}: {OrderId}, {nameof(WasPaid)}: {WasPaid}, {nameof(WasGiven)}: {WasGiven}, " +
        $"{nameof(TradeVolume)}: {TradeVolume:N2}";
    
    public override string ToString() => $"{nameof(LastPaidGivenTrade)} {{{LastPaidGivenTradeToStringMembers}}}";
}
