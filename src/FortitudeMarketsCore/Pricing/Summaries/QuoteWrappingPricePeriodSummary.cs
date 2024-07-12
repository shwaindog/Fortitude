// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types;
using FortitudeIO.TimeSeries;
using FortitudeMarketsApi.Pricing;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsApi.Pricing.Summaries;

#endregion

namespace FortitudeMarketsCore.Pricing.Summaries;

public class QuoteWrappingPricePeriodSummary : ReusableObject<IPricePeriodSummary>, IPricePeriodSummary, ICloneable<QuoteWrappingPricePeriodSummary>
{
    private ILevel1Quote? level1Quote;

    public QuoteWrappingPricePeriodSummary() { }

    public QuoteWrappingPricePeriodSummary(ILevel1Quote level1Quote) => this.level1Quote = level1Quote;
    public QuoteWrappingPricePeriodSummary(QuoteWrappingPricePeriodSummary toClone) => level1Quote = toClone.level1Quote;

    public override QuoteWrappingPricePeriodSummary Clone() =>
        Recycler?.Borrow<QuoteWrappingPricePeriodSummary>().CopyFrom(this) as QuoteWrappingPricePeriodSummary ??
        new QuoteWrappingPricePeriodSummary(this);

    public override IPricePeriodSummary CopyFrom
        (IPricePeriodSummary source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (source is QuoteWrappingPricePeriodSummary quoteWrappingPricePeriodSummary) level1Quote = quoteWrappingPricePeriodSummary.level1Quote;
        return this;
    }

    IPricePeriodSummary ICloneable<IPricePeriodSummary>.Clone() => Clone();

    public bool AreEquivalent(IPricePeriodSummary? other, bool exactTypes = false)
    {
        if (other == null) return false;
        if (exactTypes && other.GetType() != GetType()) return false;
        var timeFrameSame          = TimeSeriesPeriod == other.TimeSeriesPeriod;
        var startTimeSame          = PeriodStartTime.Equals(other.PeriodStartTime);
        var endTimeSame            = PeriodEndTime.Equals(other.PeriodEndTime);
        var startBidAskSame        = Equals(StartBidAsk, other.StartBidAsk);
        var highestBidAskSame      = Equals(HighestBidAsk, other.HighestBidAsk);
        var lowestBidAskSame       = Equals(LowestBidAsk, other.LowestBidAsk);
        var endBidAskSame          = Equals(EndBidAsk, other.EndBidAsk);
        var tickCountSame          = TickCount == other.TickCount;
        var periodVolumeSame       = PeriodVolume == other.PeriodVolume;
        var periodSummaryFlagsSame = PeriodSummaryFlags == other.PeriodSummaryFlags;
        var averageBidAskSame      = Equals(AverageBidAsk, other.AverageBidAsk);

        var allAreSame = timeFrameSame && startTimeSame && endTimeSame && startBidAskSame && highestBidAskSame
                      && lowestBidAskSame && endBidAskSame && tickCountSame && periodVolumeSame && averageBidAskSame
                      && periodSummaryFlagsSame;
        return allAreSame;
    }

    public DateTime         PeriodStartTime  => level1Quote?.SourceTime ?? DateTimeConstants.UnixEpoch;
    public TimeSeriesPeriod TimeSeriesPeriod => TimeSeriesPeriod.Tick;
    public DateTime         PeriodEndTime    => Next?.PeriodStartTime ?? (level1Quote?.SourceTime ?? DateTimeConstants.UnixEpoch);

    public DateTime StorageTime(IStorageTimeResolver<IPricePeriodSummary>? resolver = null) => PeriodEndTime;

    public PricePeriodSummaryFlags PeriodSummaryFlags { get; set; }

    public IPricePeriodSummary? Previous { get; set; }
    public IPricePeriodSummary? Next     { get; set; }

    public bool IsEmpty
    {
        get => level1Quote == null;
        set
        {
            if (!value) return;
            level1Quote = null;
        }
    }
    public BidAskPair StartBidAsk   => level1Quote?.BidAskTop ?? new BidAskPair();
    public BidAskPair HighestBidAsk => level1Quote?.BidAskTop ?? new BidAskPair();
    public BidAskPair LowestBidAsk  => level1Quote?.BidAskTop ?? new BidAskPair();
    public BidAskPair EndBidAsk     => level1Quote?.BidAskTop ?? new BidAskPair();
    public uint       TickCount     => level1Quote != null ? (uint)1 : 0;
    public long       PeriodVolume  { get; set; }
    public BidAskPair AverageBidAsk => level1Quote?.BidAskTop ?? new BidAskPair();

    public BoundedTimeRange ToBoundedTimeRange(DateTime? maxDateTime = null) =>
        new(PeriodStartTime, Next?.PeriodStartTime.Min(maxDateTime) ?? maxDateTime ?? (level1Quote?.SourceTime ?? DateTimeConstants.UnixEpoch));

    public bool IsWhollyBoundedBy
        (ITimeSeriesPeriodRange parentRange) =>
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

    public IPricePeriodSummary CopyFrom(ILevel1Quote source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        level1Quote = source;
        return this;
    }

    public void Configure(ILevel1Quote level1)
    {
        level1.IncrementRefCount();
        level1Quote = level1;
    }

    public static IPricePeriodSummary Wrap(ILevel1Quote level1Quote, IRecycler? recycler = null) =>
        recycler?.Borrow<QuoteWrappingPricePeriodSummary>().CopyFrom(level1Quote) ?? new QuoteWrappingPricePeriodSummary(level1Quote);

    public override string ToString() => $"{nameof(QuoteWrappingPricePeriodSummary)}({nameof(level1Quote)}: {level1Quote})";
}

public static class QuoteWrappingPricePeriodSummaryExtensions
{
    public static QuoteWrappingPricePeriodSummary ToSummary(this ILevel1Quote level1Quote) => new(level1Quote);
}
