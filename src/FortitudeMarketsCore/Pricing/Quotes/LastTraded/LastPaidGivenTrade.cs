// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Types;
using FortitudeMarketsApi.Pricing.LastTraded;

#endregion

namespace FortitudeMarketsCore.Pricing.Quotes.LastTraded;

public class LastPaidGivenTrade : LastTrade, IMutableLastPaidGivenTrade
{
    public LastPaidGivenTrade() { }

    public LastPaidGivenTrade(decimal tradePrice = 0m, DateTime? tradeDateTime = null, decimal tradeVolume = 0m,
        bool wasPaid = false, bool wasGiven = false) : base(tradePrice, tradeDateTime)
    {
        WasPaid     = wasPaid;
        WasGiven    = wasGiven;
        TradeVolume = tradeVolume;
    }

    public LastPaidGivenTrade(ILastTrade toClone) : base(toClone)
    {
        if (toClone is ILastPaidGivenTrade lastPaidGivenTrade)
        {
            WasPaid     = lastPaidGivenTrade.WasPaid;
            WasGiven    = lastPaidGivenTrade.WasGiven;
            TradeVolume = lastPaidGivenTrade.TradeVolume;
        }
    }

    public override LastTradeType LastTradeType => LastTradeType.PricePaidOrGivenVolume;

    public override LastTradedFlags SupportsLastTradedFlags =>
        LastTradedFlags.PaidOrGiven | LastTradedFlags.LastTradedVolume | base.SupportsLastTradedFlags;

    public bool    WasPaid     { get; set; }
    public bool    WasGiven    { get; set; }
    public decimal TradeVolume { get; set; }
    public override bool IsEmpty
    {
        get => base.IsEmpty && WasPaid == false && WasGiven == false && TradeVolume == 0m;
        set
        {
            if (!value) return;
            WasPaid  = false;
            WasGiven = false;

            TradeVolume  = 0m;
            base.IsEmpty = true;
        }
    }

    public override void StateReset()
    {
        TradeVolume = 0;
        WasPaid     = WasGiven = false;
        base.StateReset();
    }

    public override ILastTrade CopyFrom(ILastTrade source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);
        if (source is ILastPaidGivenTrade lastPaidGivenTrade)
        {
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
        var wasPaidSame     = WasPaid == lastPaidGivenTrade.WasPaid;
        var wasGivenSame    = WasGiven == lastPaidGivenTrade.WasGiven;
        var tradeVolumeSame = TradeVolume == lastPaidGivenTrade.TradeVolume;

        return baseSame && wasPaidSame && wasGivenSame && tradeVolumeSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as ILastPaidGivenTrade, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = base.GetHashCode();
            hashCode = (hashCode * 397) ^ WasPaid.GetHashCode();
            hashCode = (hashCode * 397) ^ WasGiven.GetHashCode();
            hashCode = (hashCode * 397) ^ TradeVolume.GetHashCode();
            return hashCode;
        }
    }

    public override string ToString() =>
        $"LastPaidGivenTrade {{ {nameof(TradePrice)}: {TradePrice:N5}, {nameof(TradeTime)}: " +
        $"{TradeTime:O}, {nameof(WasPaid)}: {WasPaid}, {nameof(WasGiven)}: {WasGiven}, " +
        $"{nameof(TradeVolume)}: {TradeVolume:N2} }}";
}
