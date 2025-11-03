// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeIO.Storage.TimeSeries;
using FortitudeMarkets.Pricing.FeedEvents.Quotes;

#endregion

namespace FortitudeMarkets.Pricing.FeedEvents.Candles;

public class QuoteWrappingCandle : ReusableObject<ICandle>, ICandle, ICloneable<QuoteWrappingCandle>
{
    private IPublishableLevel1Quote? level1Quote;

    public QuoteWrappingCandle() { }

    public QuoteWrappingCandle(IPublishableLevel1Quote level1Quote) => this.level1Quote = level1Quote;
    public QuoteWrappingCandle(QuoteWrappingCandle toClone) => level1Quote = toClone.level1Quote;

    public override QuoteWrappingCandle Clone() =>
        Recycler?.Borrow<QuoteWrappingCandle>().CopyFrom(this) as QuoteWrappingCandle ??
        new QuoteWrappingCandle(this);


    public bool AreEquivalent(ICandle? other, bool exactTypes = false)
    {
        if (other == null) return false;
        if (exactTypes && other.GetType() != GetType()) return false;
        var timeFrameSame     = TimeBoundaryPeriod == other.TimeBoundaryPeriod;
        var startTimeSame     = PeriodStartTime.Equals(other.PeriodStartTime);
        var endTimeSame       = PeriodEndTime.Equals(other.PeriodEndTime);
        var startBidAskSame   = Equals(StartBidAsk, other.StartBidAsk);
        var highestBidAskSame = Equals(HighestBidAsk, other.HighestBidAsk);
        var lowestBidAskSame  = Equals(LowestBidAsk, other.LowestBidAsk);
        var endBidAskSame     = Equals(EndBidAsk, other.EndBidAsk);
        var tickCountSame     = TickCount == other.TickCount;
        var periodVolumeSame  = PeriodVolume == other.PeriodVolume;
        var candleFlagsSame   = CandleFlags == other.CandleFlags;
        var averageBidAskSame = Equals(AverageBidAsk, other.AverageBidAsk);

        var allAreSame = timeFrameSame && startTimeSame && endTimeSame && startBidAskSame && highestBidAskSame
                      && lowestBidAskSame && endBidAskSame && tickCountSame && periodVolumeSame && averageBidAskSame
                      && candleFlagsSame;
        return allAreSame;
    }

    public DateTime PeriodStartTime => level1Quote?.SourceTime ?? DateTime.MinValue;
    public TimeBoundaryPeriod TimeBoundaryPeriod => TimeBoundaryPeriod.Tick;
    public DateTime PeriodEndTime => Next?.PeriodStartTime ?? (level1Quote?.SourceTime ?? DateTime.MinValue);

    public DateTime StorageTime(IStorageTimeResolver? resolver)
    {
        if (resolver is IStorageTimeResolver<ICandle> candleResolver) return candleResolver.ResolveStorageTime(this);
        return PeriodEndTime;
    }

    public CandleFlags CandleFlags { get; set; }

    public ICandle? Previous { get; set; }

    public ICandle? Next { get; set; }

    public bool IsEmpty
    {
        get => level1Quote == null;
        set
        {
            if (!value) return;
            level1Quote = null;
        }
    }
    public BidAskPair StartBidAsk => level1Quote?.BidAskTop ?? new BidAskPair();
    public BidAskPair HighestBidAsk => level1Quote?.BidAskTop ?? new BidAskPair();
    public BidAskPair LowestBidAsk => level1Quote?.BidAskTop ?? new BidAskPair();
    public BidAskPair EndBidAsk => level1Quote?.BidAskTop ?? new BidAskPair();

    public uint TickCount => level1Quote != null ? (uint)1 : 0;
    public long PeriodVolume { get; set; }

    public BidAskPair AverageBidAsk => level1Quote?.BidAskTop ?? new BidAskPair();

    public BoundedTimeRange ToBoundedTimeRange(DateTime? maxDateTime = null) =>
        new(PeriodStartTime, Next?.PeriodStartTime.Min(maxDateTime) ?? maxDateTime ?? (level1Quote?.SourceTime ?? DateTime.MinValue));

    public bool IsWhollyBoundedBy
        (ITimeBoundaryPeriodRange parentRange) =>
        parentRange.PeriodStartTime <= PeriodStartTime && parentRange.PeriodEnd() >= PeriodStartTime;

    public double ContributingCompletePercentage(BoundedTimeRange timeRange, IRecycler recycler)
    {
        if (level1Quote == null) return 0;

        return ToBoundedTimeRange(timeRange.ToTime).ContributingPercentageOfTimeRange(timeRange);
    }

    public override void StateReset()
    {
        Next = Previous = null;
        level1Quote?.DecrementRefCount();
        level1Quote = null;
        base.StateReset();
    }

    public override ICandle CopyFrom
        (ICandle source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (source is QuoteWrappingCandle quoteWrappingCandle) level1Quote = quoteWrappingCandle.level1Quote;
        return this;
    }

    ICandle ICloneable<ICandle>.Clone() => Clone();

    public ICandle CopyFrom(IPublishableLevel1Quote source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        level1Quote = source;
        return this;
    }

    public void Configure(IPublishableLevel1Quote level1)
    {
        level1.IncrementRefCount();
        level1Quote = level1;
    }

    public static ICandle Wrap(IPublishableLevel1Quote level1Quote, IRecycler? recycler = null) =>
        recycler?.Borrow<QuoteWrappingCandle>().CopyFrom(level1Quote) ?? new QuoteWrappingCandle(level1Quote);

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
            .Field.AlwaysReveal(nameof(level1Quote), level1Quote)
            .Field.AlwaysAdd(nameof(TimeBoundaryPeriod), TimeBoundaryPeriod)
            .Field.AlwaysAdd(nameof(PeriodStartTime), PeriodStartTime)
            .Field.AlwaysAdd(nameof(PeriodEndTime), PeriodEndTime)
            .Field.AlwaysReveal(nameof(StartBidAsk), StartBidAsk, BidAskPair.Styler)
            .Field.AlwaysReveal(nameof(HighestBidAsk), HighestBidAsk, BidAskPair.Styler)
            .Field.AlwaysReveal(nameof(LowestBidAsk), LowestBidAsk, BidAskPair.Styler)
            .Field.AlwaysReveal(nameof(EndBidAsk), EndBidAsk, BidAskPair.Styler)
            .Field.AlwaysAdd(nameof(TickCount), TickCount)
            .Field.AlwaysAdd(nameof(PeriodVolume), PeriodVolume)
            .Field.AlwaysAdd(nameof(CandleFlags), CandleFlags)
            .Field.AlwaysReveal(nameof(AverageBidAsk), AverageBidAsk, BidAskPair.Styler)
            .Complete();

    public override string ToString() => $"{nameof(QuoteWrappingCandle)}({nameof(level1Quote)}: {level1Quote})";
}

public static class QuoteWrappingCandleExtensions
{
    public static QuoteWrappingCandle ToSummary(this IPublishableLevel1Quote level1Quote) => new(level1Quote);
}
