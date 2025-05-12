// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text.Json.Serialization;
using FortitudeCommon.Types.Mutable;

#endregion

namespace FortitudeMarkets.Pricing.FeedEvents.LastTraded;

public class LastTraderPaidGivenTrade : LastPaidGivenTrade, IMutableLastTraderPaidGivenTrade
{
    public LastTraderPaidGivenTrade() { }

    public LastTraderPaidGivenTrade
    (decimal tradePrice = 0m, DateTime? tradeDateTime = null,
        decimal tradeVolume = 0m, bool wasPaid = false, bool wasGiven = false, string? traderName = null)
        : base(tradePrice, tradeDateTime, tradeVolume, wasPaid, wasGiven) =>
        TraderName = traderName;

    public LastTraderPaidGivenTrade(ILastTrade toClone) : base(toClone)
    {
        if (toClone is ILastTraderPaidGivenTrade lastTraderPaidGivenTrade) TraderName = lastTraderPaidGivenTrade.TraderName;
    }

    [JsonIgnore] public override LastTradeType   LastTradeType           => LastTradeType.PriceLastTraderPaidOrGivenVolume;
    [JsonIgnore] public override LastTradedFlags SupportsLastTradedFlags => LastTradedFlags.TraderName | base.SupportsLastTradedFlags;


    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? TraderName { get; set; }


    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public override bool IsEmpty
    {
        get => base.IsEmpty && TraderName == null;
        set
        {
            if (!value) return;
            TraderName = null;

            base.IsEmpty = true;
        }
    }

    public override void StateReset()
    {
        TraderName = null;
        base.StateReset();
    }

    public override ILastTrade CopyFrom(ILastTrade source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);

        if (source is ILastTraderPaidGivenTrade lastTraderPaidGivenTrade) TraderName = lastTraderPaidGivenTrade.TraderName;
        return this;
    }

    public override IMutableLastTrade Clone() =>
        (IMutableLastTrade?)Recycler?.Borrow<LastTraderPaidGivenTrade>().CopyFrom(this) ??
        new LastTraderPaidGivenTrade(this);

    ILastTraderPaidGivenTrade ILastTraderPaidGivenTrade.Clone() => (ILastTraderPaidGivenTrade)Clone();

    IMutableLastTraderPaidGivenTrade IMutableLastTraderPaidGivenTrade.Clone() => (IMutableLastTraderPaidGivenTrade)Clone();

    public override bool AreEquivalent(ILastTrade? other, bool exactTypes = false)
    {
        if (!(other is ILastTraderPaidGivenTrade lastTraderPaidGivenTrade)) return false;

        var baseSame       = base.AreEquivalent(other, exactTypes);
        var traderNameSame = TraderName == lastTraderPaidGivenTrade.TraderName;

        return baseSame && traderNameSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as ILastTrade, true);

    public override int GetHashCode()
    {
        unchecked
        {
            return (base.GetHashCode() * 397) ^ (TraderName?.GetHashCode() ?? 0);
        }
    }

    public override string ToString() =>
        $"LastTraderPaidGivenTrade {{ {nameof(TradePrice)}: {TradePrice:N5}, " +
        $"{nameof(TradeTime)}: {TradeTime:O}, {nameof(WasPaid)}: {WasPaid}, " +
        $"{nameof(WasGiven)}: {WasGiven}, {nameof(TradeVolume)}: {TradeVolume:N2}, " +
        $"{nameof(TraderName)}: {TraderName} }}]";
}
