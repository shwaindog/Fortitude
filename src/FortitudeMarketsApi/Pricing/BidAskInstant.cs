// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeMarketsApi.Pricing.Quotes;

#endregion

namespace FortitudeMarketsApi.Pricing;

public interface IBidAskInstant : IBidAskPair, IReusableObject<IBidAskInstant>, IInterfacesComparable<IBidAskInstant>
  , IDoublyLinkedListNode<IBidAskInstant>
{
    DateTime AtTime { get; }
}

public readonly struct BidAskInstantPair // not inheriting from IBidAskInstantPair to prevent accidental boxing unboxing
{
    public BidAskInstantPair() { }

    public BidAskInstantPair(BidAskInstantPair toClone)
    {
        BidPrice = toClone.BidPrice;
        AskPrice = toClone.AskPrice;
        AtTime   = toClone.AtTime;
    }

    public BidAskInstantPair(IBidAskInstant toClone)
    {
        BidPrice = toClone.BidPrice;
        AskPrice = toClone.AskPrice;
        AtTime   = toClone.AtTime;
    }

    public BidAskInstantPair(ILevel1Quote toCapture)
    {
        BidPrice = toCapture.BidPriceTop;
        AskPrice = toCapture.AskPriceTop;
        AtTime   = toCapture.SourceTime;
    }

    public BidAskInstantPair(decimal bidPrice, decimal askPrice, DateTime? atTime = null)
    {
        BidPrice = bidPrice;
        AskPrice = askPrice;
        AtTime   = atTime ?? DateTime.UtcNow;
    }

    public decimal  BidPrice { get; }
    public decimal  AskPrice { get; }
    public DateTime AtTime   { get; }
}

public static class BidAskInstantPairExtensions
{
    public static BidAskInstant ToBidAskInstant(this BidAskInstantPair pair, IRecycler? recycler = null)
    {
        var instant = recycler?.Borrow<BidAskInstant>() ?? new BidAskInstant();
        instant.Configure(pair);
        return instant;
    }

    public static BidAskInstantPair SetBidPrice(this BidAskInstantPair pair, decimal bidPrice) => new(bidPrice, pair.AskPrice, pair.AtTime);

    public static BidAskInstantPair SetAskPrice(this BidAskInstantPair pair, decimal askPrice) => new(pair.BidPrice, askPrice, pair.AtTime);

    public static BidAskInstantPair SetAtTime(this BidAskInstantPair pair, DateTime atTime) => new(pair.BidPrice, pair.AskPrice, atTime);
}

public interface IMutableBidAskInstant : IBidAskInstant
{
    new decimal  BidPrice { get; set; }
    new decimal  AskPrice { get; set; }
    new DateTime AtTime   { get; set; }
}

public class BidAskInstant : ReusableObject<IBidAskInstant>, IMutableBidAskInstant
{
    protected BidAskInstantPair BidAskInstantPairState;

    public BidAskInstant() { }
    public BidAskInstant(BidAskInstantPair bidAskInstantPair) => BidAskInstantPairState = bidAskInstantPair;

    public BidAskInstant(BidAskInstant bidAskInstant) => BidAskInstantPairState = bidAskInstant.BidAskInstantPairState;

    public BidAskInstant(ILevel1Quote toCapture) => BidAskInstantPairState = new BidAskInstantPair(toCapture);

    public BidAskInstant(decimal bidPrice, decimal askPrice, DateTime? atTime = null)
    {
        BidPrice = bidPrice;
        AskPrice = askPrice;
        AtTime   = atTime ?? DateTime.UtcNow;
    }

    public BidAskInstant(IBidAskInstant toClone)
    {
        BidPrice = toClone.BidPrice;
        AskPrice = toClone.AskPrice;
        AtTime   = toClone.AtTime;
    }

    public decimal BidPrice
    {
        get => BidAskInstantPairState.BidPrice;
        set => BidAskInstantPairState = BidAskInstantPairState.SetBidPrice(value);
    }

    public decimal AskPrice
    {
        get => BidAskInstantPairState.AskPrice;
        set => BidAskInstantPairState = BidAskInstantPairState.SetAskPrice(value);
    }

    public DateTime AtTime
    {
        get => BidAskInstantPairState.AtTime;
        set => BidAskInstantPairState = BidAskInstantPairState.SetAtTime(value);
    }

    public IBidAskInstant? Previous { get; set; }
    public IBidAskInstant? Next     { get; set; }

    public virtual bool AreEquivalent(IBidAskInstant? other, bool exactTypes = false)
    {
        if (other == null) return false;
        var bidPriceSame = BidPrice == other.BidPrice;
        var askPriceSame = AskPrice == other.AskPrice;
        var atTimeSame   = AtTime == other.AtTime;

        var allAreSame = bidPriceSame && askPriceSame && atTimeSame;
        return allAreSame;
    }

    public override IBidAskInstant CopyFrom(IBidAskInstant source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        BidPrice = source.BidPrice;
        AskPrice = source.AskPrice;
        AtTime   = source.AtTime;
        return this;
    }

    public override IBidAskInstant Clone() => Recycler?.Borrow<BidAskInstant>().CopyFrom(this) ?? new BidAskInstant(this);

    public void Configure(BidAskInstantPair bidAskInstantPair) => BidAskInstantPairState = bidAskInstantPair;

    public void Configure(BidAskInstant bidAskInstant) => BidAskInstantPairState = bidAskInstant.BidAskInstantPairState;

    public void Configure(ILevel1Quote toCapture) => BidAskInstantPairState = new BidAskInstantPair(toCapture);

    public void Configure(decimal bidPrice, decimal askPrice, DateTime? atTime = null)
    {
        BidPrice = bidPrice;
        AskPrice = askPrice;
        AtTime   = atTime ?? DateTime.UtcNow;
    }

    public virtual IBidAskInstant CopyFrom(ILevel1Quote source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        BidPrice = source.BidPriceTop;
        AskPrice = source.AskPriceTop;
        AtTime   = source.SourceTime;

        return this;
    }

    protected bool Equals(BidAskInstant other) => AreEquivalent(other, true);

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((BidAskInstant)obj);
    }

    // ReSharper disable once NonReadonlyMemberInGetHashCode
    public override int GetHashCode() => BidAskInstantPairState.GetHashCode();

    public override string ToString() =>
        $"{nameof(BidAskInstant)}({nameof(BidPrice)}: {BidPrice}, {nameof(AskPrice)}: {AskPrice}, " +
        $"{nameof(AtTime)}: {AtTime})";


    public static implicit operator BidAskInstantPair(BidAskInstant pricePoint) => pricePoint.BidAskInstantPairState;
}
