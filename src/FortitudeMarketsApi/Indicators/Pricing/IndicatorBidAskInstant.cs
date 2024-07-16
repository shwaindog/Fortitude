// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeIO.TimeSeries;
using FortitudeMarketsApi.Pricing;
using FortitudeMarketsApi.Pricing.Quotes;

#endregion

namespace FortitudeMarketsApi.Indicators.Pricing;

public interface IIndicatorBidAskInstantPair : IBidAskInstantPair, IReusableObject<IIndicatorBidAskInstantPair>
  , IInterfacesComparable<IIndicatorBidAskInstantPair>
{
    long IndicatorSourceTickerId { get; }

    TimePeriod CoveringPeriod { get; }

    new IIndicatorBidAskInstantPair Clone();
}

public readonly struct IndicatorBidAskInstantPair // not inheriting from IBidAskInstantPair to prevent accidental boxing unboxing
{
    public IndicatorBidAskInstantPair() { }

    public IndicatorBidAskInstantPair(long indicatorSourceTickerId, TimePeriod coveringPeriod, BidAskInstantPair toClone)
    {
        IndicatorSourceTickerId = indicatorSourceTickerId;

        CoveringPeriod = coveringPeriod;

        BidPrice = toClone.BidPrice;
        AskPrice = toClone.AskPrice;
        AtTime   = toClone.AtTime;
    }

    public IndicatorBidAskInstantPair(IIndicatorBidAskInstantPair toClone)
    {
        IndicatorSourceTickerId = toClone.IndicatorSourceTickerId;

        CoveringPeriod = toClone.CoveringPeriod;

        BidPrice = toClone.BidPrice;
        AskPrice = toClone.AskPrice;
        AtTime   = toClone.AtTime;
    }

    public IndicatorBidAskInstantPair(long indicatorSourceTickerId, ILevel1Quote toCapture)
    {
        IndicatorSourceTickerId = indicatorSourceTickerId;

        CoveringPeriod = new TimePeriod(TimeSeriesPeriod.Tick);

        BidPrice = toCapture.BidPriceTop;
        AskPrice = toCapture.AskPriceTop;
        AtTime   = toCapture.SourceTime;
    }

    public IndicatorBidAskInstantPair
        (long indicatorSourceTickerId, decimal bidPrice, decimal askPrice, TimePeriod coveringPeriod, DateTime? atTime = null)
    {
        IndicatorSourceTickerId = indicatorSourceTickerId;

        CoveringPeriod = coveringPeriod;

        BidPrice = bidPrice;
        AskPrice = askPrice;
        AtTime   = atTime ?? DateTime.UtcNow;
    }

    public IndicatorBidAskInstantPair
        (long indicatorSourceTickerId, BidAskInstantPair bidAskInstantPair, TimePeriod coveringPeriod)
    {
        IndicatorSourceTickerId = indicatorSourceTickerId;

        CoveringPeriod = coveringPeriod;

        BidPrice = bidAskInstantPair.BidPrice;
        AskPrice = bidAskInstantPair.AskPrice;
        AtTime   = bidAskInstantPair.AtTime;
    }

    public long IndicatorSourceTickerId { get; }

    public TimePeriod CoveringPeriod { get; }

    public decimal  BidPrice { get; }
    public decimal  AskPrice { get; }
    public DateTime AtTime   { get; }


    public static implicit operator BidAskInstantPair(IndicatorBidAskInstantPair indicatorBidAskInstant) =>
        new(indicatorBidAskInstant.BidPrice, indicatorBidAskInstant.AskPrice, indicatorBidAskInstant.AtTime);
}

public static class BidAskInstantPairExtensions
{
    public static IndicatorBidAskInstantPair ToIndicatorBidAskInstantPair
        (this BidAskInstantPair pair, long indicatorSourceTickerId, TimePeriod coveringPeriod) =>
        new(indicatorSourceTickerId, pair, coveringPeriod);

    public static IndicatorBidAskInstant ToIndicatorBidAskInstant(this IndicatorBidAskInstantPair pair, IRecycler? recycler = null)
    {
        var instant = recycler?.Borrow<IndicatorBidAskInstant>() ?? new IndicatorBidAskInstant();
        instant.Configure(pair);
        return instant;
    }

    public static IndicatorBidAskInstant ToIndicatorBidAskInstant
        (this BidAskInstantPair pair, long indicatorSourceTickerId, TimePeriod coveringPeriod, IRecycler? recycler = null)
    {
        var instant = recycler?.Borrow<IndicatorBidAskInstant>() ?? new IndicatorBidAskInstant();
        instant.Configure(indicatorSourceTickerId, pair, coveringPeriod);
        return instant;
    }

    public static IndicatorBidAskInstantPair SetBidPrice
        (this IndicatorBidAskInstantPair pair, decimal bidPrice) =>
        new(pair.IndicatorSourceTickerId, bidPrice, pair.AskPrice, pair.CoveringPeriod, pair.AtTime);

    public static IndicatorBidAskInstantPair SetAskPrice
        (this IndicatorBidAskInstantPair pair, decimal askPrice) =>
        new(pair.IndicatorSourceTickerId, pair.BidPrice, askPrice, pair.CoveringPeriod, pair.AtTime);

    public static IndicatorBidAskInstantPair SetCoveringPeriod
        (this IndicatorBidAskInstantPair pair, TimePeriod coveringPeriod) =>
        new(pair.IndicatorSourceTickerId, pair.BidPrice, pair.AskPrice, coveringPeriod, pair.AtTime);

    public static IndicatorBidAskInstantPair SetAtTime
        (this IndicatorBidAskInstantPair pair, DateTime atTime) =>
        new(pair.IndicatorSourceTickerId, pair.BidPrice, pair.AskPrice, pair.CoveringPeriod, atTime);

    public static IndicatorBidAskInstantPair SetIndicatorSourceTickerId
        (this IndicatorBidAskInstantPair pair, long indicatorSourceTickerId) =>
        new(indicatorSourceTickerId, pair.BidPrice, pair.AskPrice, pair.CoveringPeriod, pair.AtTime);
}

public interface IIndicatorBidAskInstant : IIndicatorBidAskInstantPair, IBidAskInstant, IDoublyLinkedListNode<IIndicatorBidAskInstant>
{
    new long IndicatorSourceTickerId { get; set; }

    new IIndicatorBidAskInstant? Previous { get; set; }
    new IIndicatorBidAskInstant? Next     { get; set; }
}

public class IndicatorBidAskInstant : BidAskInstant, IIndicatorBidAskInstant
{
    public IndicatorBidAskInstant() { }

    public IndicatorBidAskInstant(IndicatorBidAskInstantPair indicatorBidAskInstantPair) : base(indicatorBidAskInstantPair)
    {
        IndicatorSourceTickerId = indicatorBidAskInstantPair.IndicatorSourceTickerId;

        CoveringPeriod = indicatorBidAskInstantPair.CoveringPeriod;
    }

    public IndicatorBidAskInstant(IndicatorBidAskInstant indicatorBidAskInstant) : base(indicatorBidAskInstant)
    {
        IndicatorSourceTickerId = indicatorBidAskInstant.IndicatorSourceTickerId;

        CoveringPeriod = indicatorBidAskInstant.CoveringPeriod;
    }

    public IndicatorBidAskInstant(long indicatorSourceTickerId, ILevel1Quote toCapture) : base(toCapture)
    {
        IndicatorSourceTickerId = indicatorSourceTickerId;

        CoveringPeriod = new TimePeriod(TimeSeriesPeriod.Tick);
    }

    public IndicatorBidAskInstant
        (long indicatorSourceTickerId, decimal bidPrice, decimal askPrice, TimePeriod coveringPeriod, DateTime? atTime = null)
        : base(bidPrice, askPrice, atTime)
    {
        IndicatorSourceTickerId = indicatorSourceTickerId;

        CoveringPeriod = coveringPeriod;
    }

    public IndicatorBidAskInstant
        (long indicatorSourceTickerId, BidAskInstantPair bidAskInstantPair, TimePeriod coveringPeriod)
    {
        BidAskInstantPairState = bidAskInstantPair;

        IndicatorSourceTickerId = indicatorSourceTickerId;

        CoveringPeriod = coveringPeriod;
    }

    public long IndicatorSourceTickerId { get; set; }

    public TimePeriod CoveringPeriod { get; set; }

    public new IIndicatorBidAskInstant? Previous
    {
        get => base.Previous as IIndicatorBidAskInstant;
        set => base.Previous = value;
    }

    public new IIndicatorBidAskInstant? Next
    {
        get => base.Next as IIndicatorBidAskInstant;
        set => base.Next = value;
    }

    public override bool AreEquivalent(IBidAskInstantPair? other, bool exactTypes = false)
    {
        if (other == null) return false;
        var indicatorBidAskInstant = other as IIndicatorBidAskInstant;
        if (indicatorBidAskInstant == null && exactTypes) return false;
        var baseIsSame = base.AreEquivalent(other, exactTypes);

        var idSame             = IndicatorSourceTickerId == indicatorBidAskInstant?.IndicatorSourceTickerId;
        var coveringPeriodSame = Equals(CoveringPeriod, indicatorBidAskInstant?.CoveringPeriod);

        var allAreSame = baseIsSame && idSame && coveringPeriodSame;
        return allAreSame;
    }

    public bool AreEquivalent(IIndicatorBidAskInstantPair? other, bool exactTypes = false)
    {
        if (other == null) return false;
        var baseIsSame = base.AreEquivalent(other, exactTypes);

        var idSame             = IndicatorSourceTickerId == other.IndicatorSourceTickerId;
        var coveringPeriodSame = Equals(CoveringPeriod, other.CoveringPeriod);

        var allAreSame = baseIsSame && idSame && coveringPeriodSame;
        return allAreSame;
    }

    public IReusableObject<IIndicatorBidAskInstantPair> CopyFrom
        (IReusableObject<IIndicatorBidAskInstantPair> source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default) =>
        CopyFrom((IBidAskInstantPair)source, copyMergeFlags);

    public IIndicatorBidAskInstantPair CopyFrom(IIndicatorBidAskInstantPair source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default) =>
        CopyFrom((IBidAskInstantPair)source, copyMergeFlags);

    IIndicatorBidAskInstantPair ICloneable<IIndicatorBidAskInstantPair>.Clone() => Clone();

    public override IIndicatorBidAskInstantPair Clone() =>
        Recycler?.Borrow<IndicatorBidAskInstant>().CopyFrom(this) ?? new IndicatorBidAskInstant(this);

    public void Configure(IndicatorBidAskInstantPair indicatorBidAskInstantPair)
    {
        base.Configure(indicatorBidAskInstantPair);

        IndicatorSourceTickerId = indicatorBidAskInstantPair.IndicatorSourceTickerId;

        CoveringPeriod = indicatorBidAskInstantPair.CoveringPeriod;
    }

    public void Configure(IndicatorBidAskInstant indicatorBidAskInstant)
    {
        base.Configure(indicatorBidAskInstant);

        IndicatorSourceTickerId = indicatorBidAskInstant.IndicatorSourceTickerId;

        CoveringPeriod = indicatorBidAskInstant.CoveringPeriod;
    }

    public void Configure(long indicatorSourceTickerId, ILevel1Quote toCapture)
    {
        base.Configure(toCapture);

        IndicatorSourceTickerId = indicatorSourceTickerId;

        CoveringPeriod = new TimePeriod(TimeSeriesPeriod.Tick);
    }

    public void Configure(long indicatorSourceTickerId, decimal bidPrice, decimal askPrice, TimePeriod coveringPeriod, DateTime? atTime = null)
    {
        base.Configure(bidPrice, askPrice, atTime);

        IndicatorSourceTickerId = indicatorSourceTickerId;

        CoveringPeriod = coveringPeriod;
    }

    public void Configure(long indicatorSourceTickerId, BidAskInstantPair bidAskInstantPair, TimePeriod coveringPeriod)
    {
        BidAskInstantPairState = bidAskInstantPair;

        IndicatorSourceTickerId = indicatorSourceTickerId;

        CoveringPeriod = coveringPeriod;
    }

    public override IIndicatorBidAskInstant CopyFrom(IBidAskInstantPair source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);

        if (source is IIndicatorBidAskInstant indicatorBidAskInstant)
        {
            IndicatorSourceTickerId = indicatorBidAskInstant.IndicatorSourceTickerId;
            CoveringPeriod          = indicatorBidAskInstant.CoveringPeriod;
        }
        return this;
    }

    protected bool Equals(IndicatorBidAskInstant other) => AreEquivalent(other, true);

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((IndicatorBidAskInstant)obj);
    }

    // ReSharper disable NonReadonlyMemberInGetHashCode
    public override int GetHashCode() => BidAskInstantPairState.GetHashCode() ^ IndicatorSourceTickerId.GetHashCode();
    // ReSharper restore NonReadonlyMemberInGetHashCode

    public override string ToString() =>
        $"{nameof(BidAskInstant)}({nameof(IndicatorSourceTickerId)}: {IndicatorSourceTickerId}, {nameof(BidPrice)}: {BidPrice}, " +
        $"{nameof(AskPrice)}: {AskPrice}, {nameof(AtTime)}: {AtTime})";


    public static implicit operator IndicatorBidAskInstantPair(IndicatorBidAskInstant pricePoint) =>
        new(pricePoint.IndicatorSourceTickerId, pricePoint.CoveringPeriod, pricePoint.BidAskInstantPairState);
}
