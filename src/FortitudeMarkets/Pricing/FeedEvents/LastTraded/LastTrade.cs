// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text.Json.Serialization;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;

#endregion

// ReSharper disable DoNotCallOverridableMethodsInConstructor

namespace FortitudeMarkets.Pricing.FeedEvents.LastTraded;

public class LastTrade : ReusableObject<ILastTrade>, IMutableLastTrade
{
    public LastTrade() => TradeTime = default;

    public LastTrade(decimal tradePrice = 0m, DateTime? tradeDateTime = null)
    {
        TradeTime  = tradeDateTime ?? default;
        TradePrice = tradePrice;
    }

    public LastTrade(ILastTrade toClone)
    {
        TradeTime  = toClone.TradeTime;
        TradePrice = toClone.TradePrice;
    }

    [JsonIgnore] public virtual LastTradeType   LastTradeType           => LastTradeType.Price;
    [JsonIgnore] public virtual LastTradedFlags SupportsLastTradedFlags => LastTradedFlags.LastTradedPrice | LastTradedFlags.LastTradedTime;


    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTime TradeTime { get; set; }


    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public virtual decimal TradePrice { get; set; }


    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public virtual bool IsEmpty
    {
        get => TradeTime == default && TradePrice == 0m;
        set
        {
            if (!value) return;
            TradeTime  = default;
            TradePrice = 0m;
        }
    }

    IMutableLastTrade ITrackableReset<IMutableLastTrade>.ResetWithTracking() => ResetWithTracking();

    public virtual LastTrade ResetWithTracking()
    {
        TradeTime  = default;
        TradePrice = 0m;

        return this;
    }

    public override void StateReset()
    {
        TradeTime  = default;
        TradePrice = 0m;
        base.StateReset();
    }

    public override ILastTrade CopyFrom(ILastTrade source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        TradeTime  = source.TradeTime;
        TradePrice = source.TradePrice;
        return this;
    }

    public override IMutableLastTrade Clone() => (LastTrade?)Recycler?.Borrow<LastTrade>().CopyFrom(this) ?? new LastTrade(this);

    object ICloneable.Clone() => Clone();

    ILastTrade ICloneable<ILastTrade>.Clone() => Clone();

    public virtual bool AreEquivalent(ILastTrade? other, bool exactTypes = false)
    {
        if (other == null) return false;
        if (exactTypes && other.GetType() != GetType()) return false;

        var tradeDateSame  = TradeTime.Equals(other.TradeTime);
        var tradePriceSame = TradePrice == other.TradePrice;

        return tradeDateSame && tradePriceSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent((ILastTrade?)obj, true);

    public override int GetHashCode()
    {
        unchecked
        {
            return (TradeTime.GetHashCode() * 397) ^ TradePrice.GetHashCode();
        }
    }

    public override string ToString() =>
        $"LastTrade {{ {nameof(TradePrice)}: {TradePrice:N5}, {nameof(TradeTime)}: " +
        $"{TradeTime:O} }}";
}
