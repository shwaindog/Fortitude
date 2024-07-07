// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types;
using FortitudeIO.TimeSeries;
using FortitudeMarketsApi.Pricing;
using FortitudeMarketsApi.Pricing.Summaries;

#endregion

namespace FortitudeMarketsCore.Pricing.Summaries;

public struct Candle
{
    public TimeSeriesPeriod        SummaryPeriod;
    public PricePeriodSummaryFlags PeriodSummaryFlags;

    public DateTime SummaryStartTime;
    public DateTime SummaryEndTime;

    public BidAskPair StartBidAsk;
    public BidAskPair HighestBidAsk;
    public BidAskPair LowestBidAsk;
    public BidAskPair EndBidAsk;
    public BidAskPair AverageBidAsk;

    public uint TickCount;
    public long PeriodVolume;
}

public class PricePeriodSummary : ReusableObject<IPricePeriodSummary>, IMutablePricePeriodSummary, ITimeSeriesEntry<PricePeriodSummary>
  , IDoublyLinkedListNode<PricePeriodSummary>
{
    public PricePeriodSummary()
    {
        PeriodStartTime = DateTimeConstants.UnixEpoch;
        PeriodEndTime   = DateTimeConstants.UnixEpoch;
    }

    public PricePeriodSummary(TimeSeriesPeriod timeSeriesPeriod, DateTime startTime)
    {
        TimeSeriesPeriod = timeSeriesPeriod;
        PeriodStartTime  = startTime;
        PeriodEndTime    = timeSeriesPeriod.PeriodEnd(startTime);
    }

    public PricePeriodSummary
    (TimeSeriesPeriod timeSeriesPeriod = TimeSeriesPeriod.None, DateTime? startTime = null, DateTime? endTime = null,
        decimal startBidPrice = 0m, decimal startAskPrice = 0m, decimal highestBidPrice = 0m, decimal highestAskPrice = 0m,
        decimal lowestBidPrice = 0m, decimal lowestAskPrice = 0m, decimal endBidPrice = 0m, decimal endAskPrice = 0m, uint tickCount = 0u,
        long periodVolume = 0L, decimal averageBidPrice = 0m, decimal averageAskPrice = 0m
      , PricePeriodSummaryFlags periodSummaryFlags = PricePeriodSummaryFlags.None)
    {
        TimeSeriesPeriod   = timeSeriesPeriod;
        PeriodStartTime    = startTime ?? DateTimeConstants.UnixEpoch;
        PeriodEndTime      = endTime ?? DateTimeConstants.UnixEpoch;
        StartBidPrice      = startBidPrice;
        StartAskPrice      = startAskPrice;
        HighestBidPrice    = highestBidPrice;
        HighestAskPrice    = highestAskPrice;
        LowestBidPrice     = lowestBidPrice;
        LowestAskPrice     = lowestAskPrice;
        EndBidPrice        = endBidPrice;
        EndAskPrice        = endAskPrice;
        TickCount          = tickCount;
        PeriodVolume       = periodVolume;
        PeriodSummaryFlags = periodSummaryFlags;
        AverageBidPrice    = averageBidPrice;
        AverageAskPrice    = averageAskPrice;
    }

    public PricePeriodSummary(IPricePeriodSummary toClone)
    {
        TimeSeriesPeriod   = toClone.TimeSeriesPeriod;
        PeriodStartTime    = toClone.PeriodStartTime;
        PeriodEndTime      = toClone.PeriodEndTime;
        StartBidPrice      = toClone.StartBidAsk.BidPrice;
        StartAskPrice      = toClone.StartBidAsk.AskPrice;
        HighestBidPrice    = toClone.HighestBidAsk.BidPrice;
        HighestAskPrice    = toClone.HighestBidAsk.AskPrice;
        LowestBidPrice     = toClone.LowestBidAsk.BidPrice;
        LowestAskPrice     = toClone.LowestBidAsk.AskPrice;
        EndBidPrice        = toClone.EndBidAsk.BidPrice;
        EndAskPrice        = toClone.EndBidAsk.AskPrice;
        TickCount          = toClone.TickCount;
        PeriodVolume       = toClone.PeriodVolume;
        PeriodSummaryFlags = toClone.PeriodSummaryFlags;
        AverageBidPrice    = toClone.AverageBidAsk.BidPrice;
        AverageAskPrice    = toClone.AverageBidAsk.AskPrice;
    }

    public PricePeriodSummary? Previous { get; set; }
    public PricePeriodSummary? Next     { get; set; }

    public bool IsEmpty
    {
        get
        {
            var pricesAreAllZero = StartBidPrice == decimal.Zero && StartAskPrice == decimal.Zero && HighestBidPrice == decimal.Zero &&
                                   HighestAskPrice == decimal.Zero && LowestBidPrice == decimal.Zero && LowestAskPrice == decimal.Zero &&
                                   EndBidPrice == decimal.Zero && EndAskPrice == decimal.Zero &&
                                   AverageBidPrice == decimal.Zero && AverageAskPrice == decimal.Zero;
            var tickCountAndVolumeZero = TickCount == 0 && PeriodVolume == 0;
            var summaryPeriodNone      = TimeSeriesPeriod == TimeSeriesPeriod.None;
            var summaryFlagsNone       = PeriodSummaryFlags == PricePeriodSummaryFlags.None;
            var startEndTimeUnixEpoch  = PeriodStartTime == DateTimeConstants.UnixEpoch && PeriodEndTime == DateTimeConstants.UnixEpoch;
            return pricesAreAllZero && tickCountAndVolumeZero && summaryPeriodNone && startEndTimeUnixEpoch && summaryFlagsNone;
        }
        set
        {
            if (!value) return;
            StartBidPrice      = StartAskPrice  = HighestBidPrice = HighestAskPrice = AverageBidPrice = decimal.Zero;
            LowestBidPrice     = LowestAskPrice = EndBidPrice     = EndAskPrice     = AverageAskPrice = decimal.Zero;
            TickCount          = 0;
            PeriodVolume       = 0;
            TimeSeriesPeriod   = TimeSeriesPeriod.None;
            PeriodSummaryFlags = PricePeriodSummaryFlags.None;
            PeriodStartTime    = PeriodEndTime = DateTimeConstants.UnixEpoch;
        }
    }

    public double ContributingCompletePercentage(BoundedTimeRange timeRange, IRecycler recycler)
    {
        const ushort checkBitMask = 0x1;

        var missingTickPeriods      = PeriodSummaryFlags.MissingTickFlags();
        var currentRangeMissing     = (missingTickPeriods & checkBitMask) > 0;
        var totalCompletePercentage = 0.0;
        foreach (var subRange in this.To16SubTimeRanges(recycler))
        {
            if (subRange.IntersectsWith(timeRange) && !currentRangeMissing)
                totalCompletePercentage += subRange.ContributingPercentageOfTimeRange(timeRange);
            missingTickPeriods  >>= 1;
            currentRangeMissing =   (missingTickPeriods & checkBitMask) > 0;
        }
        return totalCompletePercentage;
    }

    public PricePeriodSummaryFlags PeriodSummaryFlags { get; set; }
    public TimeSeriesPeriod        TimeSeriesPeriod   { get; set; }

    public DateTime PeriodStartTime { get; set; }
    public DateTime PeriodEndTime   { get; set; }

    public BidAskPair StartBidAsk   => new(StartBidPrice, StartAskPrice);
    public BidAskPair HighestBidAsk => new(HighestBidPrice, HighestAskPrice);
    public BidAskPair LowestBidAsk  => new(LowestBidPrice, LowestAskPrice);
    public BidAskPair EndBidAsk     => new(EndBidPrice, EndAskPrice);
    public BidAskPair AverageBidAsk => new(AverageBidPrice, AverageAskPrice);

    public decimal StartBidPrice   { get; set; }
    public decimal StartAskPrice   { get; set; }
    public decimal HighestBidPrice { get; set; }
    public decimal HighestAskPrice { get; set; }
    public decimal LowestBidPrice  { get; set; }
    public decimal LowestAskPrice  { get; set; }
    public decimal EndBidPrice     { get; set; }
    public decimal EndAskPrice     { get; set; }
    public uint    TickCount       { get; set; }
    public long    PeriodVolume    { get; set; }
    public decimal AverageBidPrice { get; set; }
    public decimal AverageAskPrice { get; set; }

    public BoundedTimeRange ToBoundedTimeRange(DateTime? maxDateTime = null) => new(PeriodStartTime, PeriodEndTime.Min(maxDateTime));

    public bool IsWhollyBoundedBy
        (ITimeSeriesPeriodRange parentRange) =>
        parentRange.PeriodStartTime <= PeriodStartTime && parentRange.PeriodEnd() >= PeriodEndTime;

    DateTime ITimeSeriesEntry<IPricePeriodSummary>.StorageTime(IStorageTimeResolver<IPricePeriodSummary>? resolver) => PeriodEndTime;

    IPricePeriodSummary IStoreState<IPricePeriodSummary>.CopyFrom(IPricePeriodSummary source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom((IMutablePricePeriodSummary)source, copyMergeFlags);

    IReusableObject<IPricePeriodSummary> IStoreState<IReusableObject<IPricePeriodSummary>>.CopyFrom
        (IReusableObject<IPricePeriodSummary> source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom((IMutablePricePeriodSummary)source, copyMergeFlags);

    IPricePeriodSummary? IDoublyLinkedListNode<IPricePeriodSummary>.Previous { get; set; }
    IPricePeriodSummary? IDoublyLinkedListNode<IPricePeriodSummary>.Next     { get; set; }

    object ICloneable.Clone() => Clone();

    IPricePeriodSummary ICloneable<IPricePeriodSummary>.Clone() => Clone();

    IMutablePricePeriodSummary IMutablePricePeriodSummary.Clone() => Clone();

    public bool AreEquivalent(IPricePeriodSummary? other, bool exactTypes = false)
    {
        if (other == null) return false;
        if (exactTypes && other.GetType() != GetType()) return false;
        var timeFrameSame          = TimeSeriesPeriod == other.TimeSeriesPeriod;
        var startTimeSame          = PeriodStartTime.Equals(other.PeriodStartTime);
        var endTimeSame            = PeriodEndTime.Equals(other.PeriodEndTime);
        var startBidPriceSame      = StartBidPrice == other.StartBidAsk.BidPrice;
        var startAskPriceSame      = StartAskPrice == other.StartBidAsk.AskPrice;
        var highestBidPriceSame    = HighestBidPrice == other.HighestBidAsk.BidPrice;
        var highestAskPriceSame    = HighestAskPrice == other.HighestBidAsk.AskPrice;
        var lowestBidPriceSame     = LowestBidPrice == other.LowestBidAsk.BidPrice;
        var lowestAskPriceSame     = LowestAskPrice == other.LowestBidAsk.AskPrice;
        var endBidPriceSame        = EndBidPrice == other.EndBidAsk.BidPrice;
        var endAskPriceSame        = EndAskPrice == other.EndBidAsk.AskPrice;
        var tickCountSame          = TickCount == other.TickCount;
        var periodVolumeSame       = PeriodVolume == other.PeriodVolume;
        var periodSummaryFlagsSame = PeriodSummaryFlags == other.PeriodSummaryFlags;
        var averageBidPriceSame    = AverageBidPrice == other.AverageBidAsk.BidPrice;
        var averageAskPriceSame    = AverageAskPrice == other.AverageBidAsk.AskPrice;

        var allAreSame = timeFrameSame && startTimeSame && endTimeSame && startBidPriceSame && startAskPriceSame
                      && highestBidPriceSame && highestAskPriceSame && lowestBidPriceSame && lowestAskPriceSame && endBidPriceSame
                      && endAskPriceSame && tickCountSame && periodVolumeSame && periodSummaryFlagsSame && averageBidPriceSame
                      && averageAskPriceSame;
        return allAreSame;
    }

    public override void StateReset()
    {
        ((IPricePeriodSummary)this).Next     = null;
        ((IPricePeriodSummary)this).Previous = null;

        Next    = Previous = null;
        IsEmpty = true;
        base.StateReset();
    }

    public DateTime StorageTime(IStorageTimeResolver<PricePeriodSummary>? resolver = null) => PeriodEndTime;

    public override PricePeriodSummary Clone() =>
        Recycler?.Borrow<PricePeriodSummary>().CopyFrom(this) as PricePeriodSummary ?? new PricePeriodSummary(this);

    public void Configure
    (TimeSeriesPeriod timeSeriesPeriod = TimeSeriesPeriod.None, DateTime? startTime = null, DateTime? endTime = null,
        decimal startBidPrice = 0m, decimal startAskPrice = 0m, decimal highestBidPrice = 0m, decimal highestAskPrice = 0m,
        decimal lowestBidPrice = 0m, decimal lowestAskPrice = 0m, decimal endBidPrice = 0m, decimal endAskPrice = 0m, uint tickCount = 0u,
        long periodVolume = 0L, decimal averageBidPrice = 0m, decimal averageAskPrice = 0m
      , PricePeriodSummaryFlags periodSummaryFlags = PricePeriodSummaryFlags.None)
    {
        TimeSeriesPeriod   = timeSeriesPeriod;
        PeriodStartTime    = startTime ?? DateTimeConstants.UnixEpoch;
        PeriodEndTime      = endTime ?? DateTimeConstants.UnixEpoch;
        StartBidPrice      = startBidPrice;
        StartAskPrice      = startAskPrice;
        HighestBidPrice    = highestBidPrice;
        HighestAskPrice    = highestAskPrice;
        LowestBidPrice     = lowestBidPrice;
        LowestAskPrice     = lowestAskPrice;
        EndBidPrice        = endBidPrice;
        EndAskPrice        = endAskPrice;
        TickCount          = tickCount;
        PeriodVolume       = periodVolume;
        PeriodSummaryFlags = periodSummaryFlags;
        AverageBidPrice    = averageBidPrice;
        AverageAskPrice    = averageAskPrice;
    }

    public void Configure(IPricePeriodSummary toClone)
    {
        TimeSeriesPeriod   = toClone.TimeSeriesPeriod;
        PeriodStartTime    = toClone.PeriodStartTime;
        PeriodEndTime      = toClone.PeriodEndTime;
        StartBidPrice      = toClone.StartBidAsk.BidPrice;
        StartAskPrice      = toClone.StartBidAsk.AskPrice;
        HighestBidPrice    = toClone.HighestBidAsk.BidPrice;
        HighestAskPrice    = toClone.HighestBidAsk.AskPrice;
        LowestBidPrice     = toClone.LowestBidAsk.BidPrice;
        LowestAskPrice     = toClone.LowestBidAsk.AskPrice;
        EndBidPrice        = toClone.EndBidAsk.BidPrice;
        EndAskPrice        = toClone.EndBidAsk.AskPrice;
        TickCount          = toClone.TickCount;
        PeriodVolume       = toClone.PeriodVolume;
        PeriodSummaryFlags = toClone.PeriodSummaryFlags;
        AverageBidPrice    = toClone.AverageBidAsk.BidPrice;
        AverageAskPrice    = toClone.AverageBidAsk.AskPrice;
    }

    public override IPricePeriodSummary CopyFrom(IPricePeriodSummary source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        TimeSeriesPeriod   = source.TimeSeriesPeriod;
        PeriodStartTime    = source.PeriodStartTime;
        PeriodEndTime      = source.PeriodEndTime;
        StartBidPrice      = source.StartBidAsk.BidPrice;
        StartAskPrice      = source.StartBidAsk.AskPrice;
        HighestBidPrice    = source.HighestBidAsk.BidPrice;
        HighestAskPrice    = source.HighestBidAsk.AskPrice;
        LowestBidPrice     = source.LowestBidAsk.BidPrice;
        LowestAskPrice     = source.LowestBidAsk.AskPrice;
        EndBidPrice        = source.EndBidAsk.BidPrice;
        EndAskPrice        = source.EndBidAsk.AskPrice;
        TickCount          = source.TickCount;
        PeriodVolume       = source.PeriodVolume;
        PeriodSummaryFlags = source.PeriodSummaryFlags;
        AverageBidPrice    = source.AverageBidAsk.BidPrice;
        AverageAskPrice    = source.AverageBidAsk.AskPrice;
        return this;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent((IPricePeriodSummary?)obj, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = (int)TimeSeriesPeriod;
            hashCode = (hashCode * 397) ^ PeriodStartTime.GetHashCode();
            hashCode = (hashCode * 397) ^ PeriodEndTime.GetHashCode();
            hashCode = (hashCode * 397) ^ StartBidPrice.GetHashCode();
            hashCode = (hashCode * 397) ^ StartAskPrice.GetHashCode();
            hashCode = (hashCode * 397) ^ HighestBidPrice.GetHashCode();
            hashCode = (hashCode * 397) ^ HighestAskPrice.GetHashCode();
            hashCode = (hashCode * 397) ^ EndBidPrice.GetHashCode();
            hashCode = (hashCode * 397) ^ EndAskPrice.GetHashCode();
            hashCode = (hashCode * 397) ^ (int)TickCount;
            hashCode = (hashCode * 397) ^ PeriodVolume.GetHashCode();
            hashCode = (hashCode * 397) ^ PeriodSummaryFlags.GetHashCode();
            hashCode = (hashCode * 397) ^ AverageBidPrice.GetHashCode();
            hashCode = (hashCode * 397) ^ AverageAskPrice.GetHashCode();
            return hashCode;
        }
    }

    public override string ToString() =>
        $"{nameof(PricePeriodSummary)} ({nameof(TimeSeriesPeriod)}: {TimeSeriesPeriod}, {nameof(PeriodStartTime)}: {PeriodStartTime:O}, " +
        $"{nameof(PeriodEndTime)}: {PeriodEndTime:O}, {nameof(StartBidPrice)}: {StartBidPrice:N5}, " +
        $"{nameof(StartAskPrice)}: {StartAskPrice:N5}, {nameof(HighestBidPrice)}: {HighestBidPrice:N5}, " +
        $"{nameof(HighestAskPrice)}: {HighestAskPrice:N5}, {nameof(LowestBidPrice)}: {LowestBidPrice:N5}, " +
        $"{nameof(LowestAskPrice)}: {LowestAskPrice:N5}, {nameof(EndBidPrice)}: {EndBidPrice:N5}, " +
        $"{nameof(EndAskPrice)}: {EndAskPrice:N5}, {nameof(TickCount)}: {TickCount}, {nameof(PeriodVolume)}: {PeriodVolume:N2}, " +
        $"{nameof(PeriodSummaryFlags)}: {PeriodSummaryFlags}, {nameof(AverageBidPrice)}: {AverageBidPrice}, {nameof(AverageAskPrice)}: {AverageAskPrice})";
}
