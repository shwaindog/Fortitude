// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing;
using FortitudeMarkets.Pricing.FeedEvents.Quotes;

#endregion

namespace FortitudeMarkets.Indicators.Pricing;

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

    public IndicatorValidRangeBidAskPeriodValue
    (long indicatorSourceTickerId, BidAskInstantPair toClone, DateTime validTo, DiscreetTimePeriod? coveringPeriod = null
      , DateTime? validFrom = null)
    {
        IndicatorSourceTickerId = indicatorSourceTickerId;

        CoveringPeriod = coveringPeriod ?? new DiscreetTimePeriod(TimeBoundaryPeriod.Tick);

        BidPrice = toClone.BidPrice;
        AskPrice = toClone.AskPrice;
        AtTime   = toClone.AtTime;

        ValidTo   = validTo.Max(AtTime);
        ValidFrom = AtTime.Max(validFrom).Min(ValidTo);
    }

    public IndicatorValidRangeBidAskPeriodValue
    (PricingIndicatorId indicatorSourceTickerId, BidAskInstantPair toClone, DateTime validTo
      , DiscreetTimePeriod? coveringPeriod = null, DateTime? validFrom = null)
        : this(indicatorSourceTickerId.IndicatorSourceTickerId, toClone, validTo, coveringPeriod, validFrom) { }

    public IndicatorValidRangeBidAskPeriodValue(IIndicatorValidRangeBidAskPeriod toClone)
    {
        IndicatorSourceTickerId = toClone.IndicatorSourceTickerId;

        CoveringPeriod = toClone.CoveringPeriod;

        BidPrice = toClone.BidPrice;
        AskPrice = toClone.AskPrice;
        AtTime   = toClone.AtTime;

        ValidTo   = toClone.ValidTo.Max(AtTime);
        ValidFrom = AtTime.Max(toClone.ValidFrom).Min(ValidTo);
    }

    public IndicatorValidRangeBidAskPeriodValue(long indicatorSourceTickerId, IPublishableLevel1Quote toCapture)
    {
        IndicatorSourceTickerId = indicatorSourceTickerId;

        CoveringPeriod = new DiscreetTimePeriod(TimeBoundaryPeriod.Tick);

        BidPrice = toCapture.BidPriceTop;
        AskPrice = toCapture.AskPriceTop;
        AtTime   = toCapture.SourceTime;

        ValidTo   = toCapture.ValidTo.Max(AtTime);
        ValidFrom = AtTime.Max(toCapture.ValidFrom).Min(ValidTo);
    }

    public IndicatorValidRangeBidAskPeriodValue
        (PricingIndicatorId indicatorSourceTickerId, IPublishableLevel1Quote toCapture)
        : this(indicatorSourceTickerId.IndicatorSourceTickerId, toCapture) { }

    public IndicatorValidRangeBidAskPeriodValue
    (long indicatorSourceTickerId, decimal bidPrice, decimal askPrice, DateTime validTo, DateTime? atTime = null
      , DiscreetTimePeriod? coveringPeriod = null, DateTime? validFrom = null)
    {
        IndicatorSourceTickerId = indicatorSourceTickerId;

        CoveringPeriod = coveringPeriod ?? new DiscreetTimePeriod(TimeBoundaryPeriod.Tick);

        BidPrice = bidPrice;
        AskPrice = askPrice;
        AtTime   = atTime ?? DateTime.UtcNow;

        ValidTo   = validTo.Max(AtTime);
        ValidFrom = AtTime.Max(validFrom).Min(ValidTo);
    }

    public IndicatorValidRangeBidAskPeriodValue
    (PricingIndicatorId indicatorSourceTickerId, decimal bidPrice, decimal askPrice, DateTime validTo
      , DateTime? atTime = null, DiscreetTimePeriod? coveringPeriod = null
      , DateTime? validFrom = null)
        : this(indicatorSourceTickerId.IndicatorSourceTickerId, bidPrice, askPrice, validTo, atTime, coveringPeriod, validFrom) { }

    public IndicatorValidRangeBidAskPeriodValue
    (long indicatorSourceTickerId, IBidAskInstant bidAskInstantPair, DateTime validTo, DiscreetTimePeriod? coveringPeriod = null
      , DateTime? validFrom = null)
    {
        IndicatorSourceTickerId = indicatorSourceTickerId;

        CoveringPeriod = coveringPeriod ?? new DiscreetTimePeriod(TimeBoundaryPeriod.Tick);
        ;

        BidPrice = bidAskInstantPair.BidPrice;
        AskPrice = bidAskInstantPair.AskPrice;
        AtTime   = bidAskInstantPair.AtTime;

        ValidTo   = validTo.Max(AtTime);
        ValidFrom = AtTime.Max(validFrom).Min(ValidTo);
    }

    public IndicatorValidRangeBidAskPeriodValue
    (PricingIndicatorId indicatorSourceTickerId, IBidAskInstant bidAskInstantPair, DateTime validTo
      , DiscreetTimePeriod? coveringPeriod = null, DateTime? validFrom = null)
        : this(indicatorSourceTickerId.IndicatorSourceTickerId, bidAskInstantPair, validTo, coveringPeriod, validFrom) { }

    public IndicatorValidRangeBidAskPeriodValue
        (long indicatorSourceTickerId, ValidRangeBidAskPeriodValue validRangeBidAskPeriodValue)
    {
        IndicatorSourceTickerId = indicatorSourceTickerId;

        CoveringPeriod = validRangeBidAskPeriodValue.CoveringPeriod;

        BidPrice = validRangeBidAskPeriodValue.BidPrice;
        AskPrice = validRangeBidAskPeriodValue.AskPrice;
        AtTime   = validRangeBidAskPeriodValue.AtTime;

        ValidTo   = validRangeBidAskPeriodValue.ValidTo.Max(AtTime);
        ValidFrom = AtTime.Max(validRangeBidAskPeriodValue.ValidFrom).Min(ValidTo);
    }

    public IndicatorValidRangeBidAskPeriodValue
        (PricingIndicatorId indicatorSourceTickerId, ValidRangeBidAskPeriodValue validRangeBidAskPeriodValue)
        : this(indicatorSourceTickerId.IndicatorSourceTickerId, validRangeBidAskPeriodValue) { }

    public IndicatorValidRangeBidAskPeriodValue
        (long indicatorSourceTickerId, IValidRangeBidAskPeriod validRangeBidAskPeriod)
    {
        IndicatorSourceTickerId = indicatorSourceTickerId;

        CoveringPeriod = validRangeBidAskPeriod.CoveringPeriod;

        BidPrice = validRangeBidAskPeriod.BidPrice;
        AskPrice = validRangeBidAskPeriod.AskPrice;
        AtTime   = validRangeBidAskPeriod.AtTime;

        ValidTo   = validRangeBidAskPeriod.ValidTo.Max(AtTime);
        ValidFrom = AtTime.Max(validRangeBidAskPeriod.ValidFrom).Min(ValidTo);
    }

    public IndicatorValidRangeBidAskPeriodValue
        (PricingIndicatorId indicatorSourceTickerId, IValidRangeBidAskPeriod validRangeBidAskPeriod)
        : this(indicatorSourceTickerId.IndicatorSourceTickerId, validRangeBidAskPeriod) { }

    public long IndicatorSourceTickerId { get; }

    public DiscreetTimePeriod CoveringPeriod { get; }

    public decimal  BidPrice  { get; }
    public decimal  AskPrice  { get; }
    public DateTime AtTime    { get; }
    public DateTime ValidFrom { get; }
    public DateTime ValidTo   { get; }


    public static implicit operator ValidRangeBidAskPeriodValue(IndicatorValidRangeBidAskPeriodValue indicatorValidRangeBidAskPeriodValue) =>
        new(indicatorValidRangeBidAskPeriodValue.BidPrice, indicatorValidRangeBidAskPeriodValue.AskPrice
          , indicatorValidRangeBidAskPeriodValue.AtTime);
}

public static class IndicatorValidRangeBidAskPeriodValueExtensions
{
    public static IndicatorValidRangeBidAskPeriodValue ToIndicatorBidAskInstantPair
    (this BidAskInstantPair pair, long indicatorSourceTickerId, DateTime validTo, DiscreetTimePeriod? coveringPeriod = null
      , DateTime? validFrom = null) =>
        new(indicatorSourceTickerId, pair, validTo, coveringPeriod, validFrom);

    public static IndicatorValidRangeBidAskPeriod ToIndicatorBidAskInstant(this IndicatorValidRangeBidAskPeriodValue pair, IRecycler? recycler = null)
    {
        var instant = recycler?.Borrow<IndicatorValidRangeBidAskPeriod>() ?? new IndicatorValidRangeBidAskPeriod();
        instant.Configure(pair);
        return instant;
    }

    public static IndicatorValidRangeBidAskPeriod ToIndicatorBidAskInstant
        (this BidAskInstantPair pair, long indicatorSourceTickerId, DiscreetTimePeriod coveringPeriod, IRecycler? recycler = null)
    {
        var instant = recycler?.Borrow<IndicatorValidRangeBidAskPeriod>() ?? new IndicatorValidRangeBidAskPeriod();
        instant.Configure(indicatorSourceTickerId, pair, coveringPeriod);
        return instant;
    }

    public static IndicatorValidRangeBidAskPeriodValue SetBidPrice
        (this IndicatorValidRangeBidAskPeriodValue pair, decimal bidPrice) =>
        new(pair.IndicatorSourceTickerId, bidPrice, pair.AskPrice, pair.ValidTo, pair.AtTime, pair.CoveringPeriod, pair.ValidFrom);

    public static IndicatorValidRangeBidAskPeriodValue SetAskPrice
        (this IndicatorValidRangeBidAskPeriodValue pair, decimal askPrice) =>
        new(pair.IndicatorSourceTickerId, pair.BidPrice, askPrice, pair.ValidTo, pair.AtTime, pair.CoveringPeriod, pair.ValidFrom);

    public static IndicatorValidRangeBidAskPeriodValue SetValidTo
        (this IndicatorValidRangeBidAskPeriodValue pair, DateTime validTo) =>
        new(pair.IndicatorSourceTickerId, pair.BidPrice, pair.AskPrice, validTo, pair.AtTime, pair.CoveringPeriod, pair.ValidFrom);

    public static IndicatorValidRangeBidAskPeriodValue SetCoveringPeriod
        (this IndicatorValidRangeBidAskPeriodValue pair, DiscreetTimePeriod coveringPeriod) =>
        new(pair.IndicatorSourceTickerId, pair.BidPrice, pair.AskPrice, pair.ValidTo, pair.AtTime, coveringPeriod, pair.ValidFrom);

    public static IndicatorValidRangeBidAskPeriodValue SetAtTime
        (this IndicatorValidRangeBidAskPeriodValue pair, DateTime atTime) =>
        new(pair.IndicatorSourceTickerId, pair.BidPrice, pair.AskPrice, atTime + (pair.ValidTo - pair.AtTime), atTime, pair.CoveringPeriod
          , atTime + (pair.ValidFrom - pair.AtTime));

    public static IndicatorValidRangeBidAskPeriodValue SetValidFrom
        (this IndicatorValidRangeBidAskPeriodValue pair, DateTime validFrom) =>
        new(pair.IndicatorSourceTickerId, pair.BidPrice, pair.AskPrice, pair.ValidTo, pair.AtTime, pair.CoveringPeriod, validFrom);

    public static IndicatorValidRangeBidAskPeriodValue SetIndicatorSourceTickerId
        (this IndicatorValidRangeBidAskPeriodValue pair, long indicatorSourceTickerId) =>
        new(indicatorSourceTickerId, pair.BidPrice, pair.AskPrice, pair.ValidTo, pair.AtTime, pair.CoveringPeriod);
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
        (long indicatorSourceTickerId, IPublishableLevel1Quote toCapture) : base(toCapture) =>
        IndicatorSourceTickerId = indicatorSourceTickerId;

    public IndicatorValidRangeBidAskPeriod
    (long indicatorSourceTickerId, decimal bidPrice, decimal askPrice, DateTime validToTime, DateTime? atTime = null
      , DiscreetTimePeriod? coveringPeriod = null
      , DateTime? validFromTime = null)
        : base(bidPrice, askPrice, validToTime, atTime, validFromTime, coveringPeriod) =>
        IndicatorSourceTickerId = indicatorSourceTickerId;

    public IndicatorValidRangeBidAskPeriod
        (long indicatorSourceTickerId, IValidRangeBidAskPeriod validRangeBidAskPeriod) : base(validRangeBidAskPeriod) =>
        IndicatorSourceTickerId = indicatorSourceTickerId;

    public IndicatorValidRangeBidAskPeriod
        (long indicatorSourceTickerId, ValidRangeBidAskPeriodValue validRangeBidAskPeriod) : base(validRangeBidAskPeriod) =>
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

    public void Configure(long indicatorSourceTickerId, IPublishableLevel1Quote toCapture)
    {
        base.Configure(toCapture);

        IndicatorSourceTickerId = indicatorSourceTickerId;

        CoveringPeriod = new DiscreetTimePeriod(TimeBoundaryPeriod.Tick);
    }

    public void Configure
        (long indicatorSourceTickerId, decimal bidPrice, decimal askPrice, DiscreetTimePeriod coveringPeriod, DateTime? atTime = null)
    {
        base.Configure(bidPrice, askPrice, atTime);

        IndicatorSourceTickerId = indicatorSourceTickerId;

        CoveringPeriod = coveringPeriod;
    }

    public void Configure(long indicatorSourceTickerId, BidAskInstantPair bidAskInstantPair, DiscreetTimePeriod coveringPeriod)
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
        new(pricePoint.IndicatorSourceTickerId, pricePoint);
}
