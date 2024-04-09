#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeMarketsApi.Pricing.LastTraded;

#endregion

// ReSharper disable DoNotCallOverridableMethodsInConstructor

namespace FortitudeMarketsCore.Pricing.Quotes.LastTraded;

public class LastTrade : ReusableObject<ILastTrade>, IMutableLastTrade
{
    public LastTrade() => TradeTime = DateTimeConstants.UnixEpoch;

    public LastTrade(decimal tradePrice = 0m, DateTime? tradeDateTime = null)
    {
        TradeTime = tradeDateTime ?? DateTimeConstants.UnixEpoch;
        TradePrice = tradePrice;
    }

    public LastTrade(ILastTrade toClone)
    {
        TradeTime = toClone.TradeTime;
        TradePrice = toClone.TradePrice;
    }

    public DateTime TradeTime { get; set; }

    public virtual decimal TradePrice { get; set; }

    public virtual bool IsEmpty => TradeTime == DateTimeConstants.UnixEpoch && TradePrice == 0m;

    public override void StateReset()
    {
        TradeTime = DateTimeConstants.UnixEpoch;
        TradePrice = 0m;
        base.StateReset();
    }

    public override ILastTrade CopyFrom(ILastTrade source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        TradeTime = source.TradeTime;
        TradePrice = source.TradePrice;
        return this;
    }

    public override IMutableLastTrade Clone() =>
        (LastTrade?)Recycler?.Borrow<LastTrade>().CopyFrom(this) ?? new LastTrade(this);

    object ICloneable.Clone() => Clone();

    ILastTrade ICloneable<ILastTrade>.Clone() => Clone();

    public virtual bool AreEquivalent(ILastTrade? other, bool exactTypes = false)
    {
        if (other == null) return false;
        if (exactTypes && other.GetType() != GetType()) return false;

        var tradeDateSame = TradeTime.Equals(other.TradeTime);
        var tradePriceSame = TradePrice == other.TradePrice;

        return tradeDateSame && tradePriceSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent((ILastTrade?)obj, true);

    public override int GetHashCode()
    {
        unchecked
        {
            return TradeTime.GetHashCode() * 397 ^ TradePrice.GetHashCode();
        }
    }

    public override string ToString() =>
        $"LastTrade {{ {nameof(TradePrice)}: {TradePrice:N5}, {nameof(TradeTime)}: " +
        $"{TradeTime:O} }}";
}
