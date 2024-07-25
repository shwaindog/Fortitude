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

public interface IIndicatorValidRangeBidAskPeriod : IValidRangeBidAskPeriod, IReusableObject<IIndicatorValidRangeBidAskPeriod>
  , IInterfacesComparable<IIndicatorValidRangeBidAskPeriod>, IDoublyLinkedListNode<IIndicatorValidRangeBidAskPeriod>
{
    long IndicatorSourceTickerId { get; }

    new IIndicatorValidRangeBidAskPeriod? Next     { get; set; }
    new IIndicatorValidRangeBidAskPeriod? Previous { get; set; }

    new IIndicatorValidRangeBidAskPeriod Clone();
}

public readonly struct IndicatorValidRangeBidAskPeriodValue // not inheriting from IBidAskInstantPair to prevent accidental boxing unboxing
{
    public IndicatorValidRangeBidAskPeriodValue() { }

    public IndicatorValidRangeBidAskPeriodValue(long indicatorSourceTickerId, TimePeriod coveringPeriod, BidAskInstantPair toClone)
    {
        IndicatorSourceTickerId = indicatorSourceTickerId;

        CoveringPeriod = coveringPeriod;

        BidPrice = toClone.BidPrice;
        AskPrice = toClone.AskPrice;
        AtTime   = toClone.AtTime;
    }

    public IndicatorValidRangeBidAskPeriodValue(IIndicatorValidRangeBidAskPeriod toClone)
    {
        IndicatorSourceTickerId = toClone.IndicatorSourceTickerId;

        CoveringPeriod = toClone.CoveringPeriod;

        BidPrice = toClone.BidPrice;
        AskPrice = toClone.AskPrice;
        AtTime   = toClone.AtTime;
    }

    public IndicatorValidRangeBidAskPeriodValue(long indicatorSourceTickerId, ILevel1Quote toCapture)
    {
        IndicatorSourceTickerId = indicatorSourceTickerId;

        CoveringPeriod = new TimePeriod(TimeSeriesPeriod.Tick);

        BidPrice = toCapture.BidPriceTop;
        AskPrice = toCapture.AskPriceTop;
        AtTime   = toCapture.SourceTime;
    }

    public IndicatorValidRangeBidAskPeriodValue
        (long indicatorSourceTickerId, decimal bidPrice, decimal askPrice, TimePeriod coveringPeriod, DateTime? atTime = null)
    {
        IndicatorSourceTickerId = indicatorSourceTickerId;

        CoveringPeriod = coveringPeriod;

        BidPrice = bidPrice;
        AskPrice = askPrice;
        AtTime   = atTime ?? DateTime.UtcNow;
    }

    public IndicatorValidRangeBidAskPeriodValue
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


    public static implicit operator ValidRangeBidAskPeriodValue(IndicatorValidRangeBidAskPeriodValue indicatorValidRangeBidAskPeriodValue) =>
        new(indicatorValidRangeBidAskPeriodValue.BidPrice, indicatorValidRangeBidAskPeriodValue.AskPrice
          , indicatorValidRangeBidAskPeriodValue.AtTime);
}

public static class IndicatorValidRangeBidAskPeriodValueExtensions
{
    public static IndicatorValidRangeBidAskPeriodValue ToIndicatorBidAskInstantPair
        (this BidAskInstantPair pair, long indicatorSourceTickerId, TimePeriod coveringPeriod) =>
        new(indicatorSourceTickerId, pair, coveringPeriod);

    public static IndicatorValidRangeBidAskPeriod ToIndicatorBidAskInstant(this IndicatorValidRangeBidAskPeriodValue pair, IRecycler? recycler = null)
    {
        var instant = recycler?.Borrow<IndicatorValidRangeBidAskPeriod>() ?? new IndicatorValidRangeBidAskPeriod();
        instant.Configure(pair);
        return instant;
    }

    public static IndicatorValidRangeBidAskPeriod ToIndicatorBidAskInstant
        (this BidAskInstantPair pair, long indicatorSourceTickerId, TimePeriod coveringPeriod, IRecycler? recycler = null)
    {
        var instant = recycler?.Borrow<IndicatorValidRangeBidAskPeriod>() ?? new IndicatorValidRangeBidAskPeriod();
        instant.Configure(indicatorSourceTickerId, pair, coveringPeriod);
        return instant;
    }

    public static IndicatorValidRangeBidAskPeriodValue SetBidPrice
        (this IndicatorValidRangeBidAskPeriodValue pair, decimal bidPrice) =>
        new(pair.IndicatorSourceTickerId, bidPrice, pair.AskPrice, pair.CoveringPeriod, pair.AtTime);

    public static IndicatorValidRangeBidAskPeriodValue SetAskPrice
        (this IndicatorValidRangeBidAskPeriodValue pair, decimal askPrice) =>
        new(pair.IndicatorSourceTickerId, pair.BidPrice, askPrice, pair.CoveringPeriod, pair.AtTime);

    public static IndicatorValidRangeBidAskPeriodValue SetCoveringPeriod
        (this IndicatorValidRangeBidAskPeriodValue pair, TimePeriod coveringPeriod) =>
        new(pair.IndicatorSourceTickerId, pair.BidPrice, pair.AskPrice, coveringPeriod, pair.AtTime);

    public static IndicatorValidRangeBidAskPeriodValue SetAtTime
        (this IndicatorValidRangeBidAskPeriodValue pair, DateTime atTime) =>
        new(pair.IndicatorSourceTickerId, pair.BidPrice, pair.AskPrice, pair.CoveringPeriod, atTime);

    public static IndicatorValidRangeBidAskPeriodValue SetIndicatorSourceTickerId
        (this IndicatorValidRangeBidAskPeriodValue pair, long indicatorSourceTickerId) =>
        new(indicatorSourceTickerId, pair.BidPrice, pair.AskPrice, pair.CoveringPeriod, pair.AtTime);
}

public class IndicatorValidRangeBidAskPeriod : ValidRangeBidAskPeriod, IIndicatorValidRangeBidAskPeriod
{
    public IndicatorValidRangeBidAskPeriod() { }

    public IndicatorValidRangeBidAskPeriod
        (IndicatorValidRangeBidAskPeriodValue indicatorValidRangeBidAskPeriodValue) : base(indicatorValidRangeBidAskPeriodValue)
    {
        IndicatorSourceTickerId = indicatorValidRangeBidAskPeriodValue.IndicatorSourceTickerId;

        CoveringPeriod = indicatorValidRangeBidAskPeriodValue.CoveringPeriod;
    }

    public IndicatorValidRangeBidAskPeriod(IndicatorValidRangeBidAskPeriod indicatorValidRangeBidAskPeriod) : base(indicatorValidRangeBidAskPeriod)
    {
        IndicatorSourceTickerId = indicatorValidRangeBidAskPeriod.IndicatorSourceTickerId;

        CoveringPeriod = indicatorValidRangeBidAskPeriod.CoveringPeriod;
    }

    public IndicatorValidRangeBidAskPeriod
        (long indicatorSourceTickerId, ValidRangeBidAskPeriodValue validRangeBidAskPeriodValue, TimePeriod coveringPeriod) :
        base(validRangeBidAskPeriodValue) =>
        IndicatorSourceTickerId = indicatorSourceTickerId;

    public IndicatorValidRangeBidAskPeriod
        (long indicatorSourceTickerId, ILevel1Quote toCapture) : base(toCapture) =>
        IndicatorSourceTickerId = indicatorSourceTickerId;

    public IndicatorValidRangeBidAskPeriod
    (long indicatorSourceTickerId, decimal bidPrice, decimal askPrice, DateTime validToTime, DateTime? atTime = null
      , DateTime? validFromTime = null, TimePeriod? coveringPeriod = null)
        : base(bidPrice, askPrice, validToTime, atTime, validFromTime, coveringPeriod) =>
        IndicatorSourceTickerId = indicatorSourceTickerId;

    public long IndicatorSourceTickerId { get; set; }

    public new IIndicatorValidRangeBidAskPeriod? Previous
    {
        get => base.Previous as IIndicatorValidRangeBidAskPeriod;
        set => ((IValidRangeBidAskPeriod)this).Previous = value;
    }

    public new IIndicatorValidRangeBidAskPeriod? Next
    {
        get => base.Next as IIndicatorValidRangeBidAskPeriod;
        set => ((IValidRangeBidAskPeriod)this).Next = value;
    }

    public override bool AreEquivalent(IBidAskInstant? other, bool exactTypes = false)
    {
        if (other == null) return false;
        var indicatorBidAskInstant = other as IIndicatorValidRangeBidAskPeriod;
        if (indicatorBidAskInstant == null && exactTypes) return false;
        var baseIsSame = base.AreEquivalent(other, exactTypes);

        var idSame             = IndicatorSourceTickerId == indicatorBidAskInstant?.IndicatorSourceTickerId;
        var coveringPeriodSame = Equals(CoveringPeriod, indicatorBidAskInstant?.CoveringPeriod);

        var allAreSame = baseIsSame && idSame && coveringPeriodSame;
        return allAreSame;
    }

    public bool AreEquivalent(IIndicatorValidRangeBidAskPeriod? other, bool exactTypes = false)
    {
        if (other == null) return false;
        var baseIsSame = base.AreEquivalent(other, exactTypes);

        var idSame             = IndicatorSourceTickerId == other.IndicatorSourceTickerId;
        var coveringPeriodSame = Equals(CoveringPeriod, other.CoveringPeriod);

        var allAreSame = baseIsSame && idSame && coveringPeriodSame;
        return allAreSame;
    }

    public IReusableObject<IIndicatorValidRangeBidAskPeriod> CopyFrom
        (IReusableObject<IIndicatorValidRangeBidAskPeriod> source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default) =>
        CopyFrom((IBidAskInstant)source, copyMergeFlags);

    public IIndicatorValidRangeBidAskPeriod CopyFrom
        (IIndicatorValidRangeBidAskPeriod source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default) =>
        CopyFrom((IBidAskInstant)source, copyMergeFlags);

    IIndicatorValidRangeBidAskPeriod ICloneable<IIndicatorValidRangeBidAskPeriod>.Clone() => Clone();

    IValidRangeBidAskPeriod IValidRangeBidAskPeriod.Clone() => Clone();

    public new IIndicatorValidRangeBidAskPeriod Clone() =>
        Recycler?.Borrow<IndicatorValidRangeBidAskPeriod>().CopyFrom(this) ?? new IndicatorValidRangeBidAskPeriod(this);

    public void Configure(IndicatorValidRangeBidAskPeriodValue indicatorValidRangeBidAskPeriodValue)
    {
        base.Configure(indicatorValidRangeBidAskPeriodValue);

        IndicatorSourceTickerId = indicatorValidRangeBidAskPeriodValue.IndicatorSourceTickerId;

        CoveringPeriod = indicatorValidRangeBidAskPeriodValue.CoveringPeriod;
    }

    public void Configure(IIndicatorValidRangeBidAskPeriod indicatorBidAskInstant)
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

    public override IIndicatorValidRangeBidAskPeriod CopyFrom(IBidAskInstant source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);

        if (source is IIndicatorValidRangeBidAskPeriod indicatorBidAskInstant)
        {
            IndicatorSourceTickerId = indicatorBidAskInstant.IndicatorSourceTickerId;
            CoveringPeriod          = indicatorBidAskInstant.CoveringPeriod;
        }
        return this;
    }

    protected bool Equals(IIndicatorValidRangeBidAskPeriod other) => AreEquivalent(other, true);

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((IIndicatorValidRangeBidAskPeriod)obj);
    }

    // ReSharper disable NonReadonlyMemberInGetHashCode
    public override int GetHashCode() => BidAskInstantPairState.GetHashCode() ^ IndicatorSourceTickerId.GetHashCode();
    // ReSharper restore NonReadonlyMemberInGetHashCode

    public override string ToString() =>
        $"{nameof(BidAskInstant)}({nameof(IndicatorSourceTickerId)}: {IndicatorSourceTickerId}, {nameof(BidPrice)}: {BidPrice}, " +
        $"{nameof(AskPrice)}: {AskPrice}, {nameof(AtTime)}: {AtTime})";


    public static implicit operator IndicatorValidRangeBidAskPeriodValue(IndicatorValidRangeBidAskPeriod pricePoint) =>
        new(pricePoint.IndicatorSourceTickerId, pricePoint.CoveringPeriod, pricePoint.BidAskInstantPairState);
}
