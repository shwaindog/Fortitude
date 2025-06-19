// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text.Json.Serialization;
using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Storage.TimeSeries;

#endregion

namespace FortitudeMarkets.Pricing.FeedEvents.Candles;

public struct CandleValue
{
    public TimeBoundaryPeriod CandlePeriod;
    public CandleFlags        CandleFlags;

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

public class Candle : ReusableObject<ICandle>, IMutableCandle, IDoublyLinkedListNode<Candle>
  , ICloneable<Candle>
{
    public Candle()
    {
        PeriodStartTime = DateTime.MinValue;
        PeriodEndTime   = DateTime.MinValue;
    }

    public Candle(TimeBoundaryPeriod timeBoundaryPeriod, DateTime startTime)
    {
        TimeBoundaryPeriod = timeBoundaryPeriod;
        PeriodStartTime    = startTime;
        PeriodEndTime      = timeBoundaryPeriod.PeriodEnd(startTime);
    }

    public Candle
    (TimeBoundaryPeriod timeBoundaryPeriod = TimeBoundaryPeriod.Tick, DateTime? startTime = null, DateTime? endTime = null,
        decimal startBidPrice = 0m, decimal startAskPrice = 0m, decimal highestBidPrice = 0m, decimal highestAskPrice = 0m,
        decimal lowestBidPrice = 0m, decimal lowestAskPrice = 0m, decimal endBidPrice = 0m, decimal endAskPrice = 0m, uint tickCount = 0u,
        long periodVolume = 0L, decimal averageBidPrice = 0m, decimal averageAskPrice = 0m
      , CandleFlags candleFlags = CandleFlags.None)
    {
        TimeBoundaryPeriod = timeBoundaryPeriod;
        PeriodStartTime    = startTime ?? DateTime.MinValue;
        PeriodEndTime      = endTime ?? DateTime.MinValue;
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
        CandleFlags        = candleFlags;
        AverageBidPrice    = averageBidPrice;
        AverageAskPrice    = averageAskPrice;
    }

    public Candle(ICandle toClone)
    {
        TimeBoundaryPeriod = toClone.TimeBoundaryPeriod;
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
        CandleFlags        = toClone.CandleFlags;
        AverageBidPrice    = toClone.AverageBidAsk.BidPrice;
        AverageAskPrice    = toClone.AverageBidAsk.AskPrice;
    }

    public override Candle Clone() => Recycler?.Borrow<Candle>().CopyFrom(this) as Candle ?? new Candle(this);

    [JsonIgnore]
    public Candle? Previous
    {
        get => ((ICandle)this).Previous as Candle;
        set => ((ICandle)this).Previous = value;
    }

    [JsonIgnore]
    public Candle? Next
    {
        get => ((ICandle)this).Next as Candle;
        set => ((ICandle)this).Next = value;
    }

    [JsonIgnore] ICandle? IDoublyLinkedListNode<ICandle>.Previous { get; set; }
    [JsonIgnore] ICandle? IDoublyLinkedListNode<ICandle>.Next     { get; set; }

    public bool IsEmpty
    {
        get
        {
            var pricesAreAllZero = StartBidPrice == decimal.Zero && StartAskPrice == decimal.Zero && HighestBidPrice == decimal.Zero &&
                                   HighestAskPrice == decimal.Zero && LowestBidPrice == decimal.Zero && LowestAskPrice == decimal.Zero &&
                                   EndBidPrice == decimal.Zero && EndAskPrice == decimal.Zero &&
                                   AverageBidPrice == decimal.Zero && AverageAskPrice == decimal.Zero;
            var tickCountAndVolumeZero = TickCount == 0 && PeriodVolume == 0;
            var candlePeriodNone       = TimeBoundaryPeriod == TimeBoundaryPeriod.Tick;
            var summaryFlagsNone       = CandleFlags == CandleFlags.None;
            var startEndTimeUnixEpoch  = PeriodStartTime == DateTime.MinValue && PeriodEndTime == DateTime.MinValue;
            return pricesAreAllZero && tickCountAndVolumeZero && candlePeriodNone && startEndTimeUnixEpoch && summaryFlagsNone;
        }
        set
        {
            if (!value) return;
            StartBidPrice      = StartAskPrice  = HighestBidPrice = HighestAskPrice = AverageBidPrice = decimal.Zero;
            LowestBidPrice     = LowestAskPrice = EndBidPrice     = EndAskPrice     = AverageAskPrice = decimal.Zero;
            TickCount          = 0;
            PeriodVolume       = 0;
            TimeBoundaryPeriod = TimeBoundaryPeriod.Tick;
            CandleFlags        = CandleFlags.None;
            PeriodStartTime    = PeriodEndTime = DateTime.MinValue;
        }
    }

    public double ContributingCompletePercentage(BoundedTimeRange timeRange, IRecycler recycler)
    {
        const ushort checkBitMask = 0x1;

        var missingTickPeriods      = CandleFlags.MissingTickFlags();
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

    public CandleFlags        CandleFlags        { get; set; }
    public TimeBoundaryPeriod TimeBoundaryPeriod { get; set; }

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
        (ITimeBoundaryPeriodRange parentRange) =>
        parentRange.PeriodStartTime <= PeriodStartTime && parentRange.PeriodEnd() >= PeriodEndTime;


    public DateTime StorageTime(IStorageTimeResolver? resolver)
    {
        if (resolver is IStorageTimeResolver<ICandle> candleResolver) return candleResolver.ResolveStorageTime(this);
        return PeriodEndTime;
    }

    ICandle ITransferState<ICandle>.CopyFrom(ICandle source, CopyMergeFlags copyMergeFlags) => CopyFrom((IMutableCandle)source, copyMergeFlags);

    IReusableObject<ICandle> ITransferState<IReusableObject<ICandle>>.CopyFrom
        (IReusableObject<ICandle> source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom((IMutableCandle)source, copyMergeFlags);

    object ICloneable.Clone() => Clone();

    ICandle ICloneable<ICandle>.Clone() => Clone();

    IMutableCandle IMutableCandle.Clone() => Clone();

    public bool AreEquivalent(ICandle? other, bool exactTypes = false)
    {
        if (other == null) return false;
        if (exactTypes && other.GetType() != GetType()) return false;
        var timeFrameSame       = TimeBoundaryPeriod == other.TimeBoundaryPeriod;
        var startTimeSame       = PeriodStartTime.Equals(other.PeriodStartTime);
        var endTimeSame         = PeriodEndTime.Equals(other.PeriodEndTime);
        var startBidPriceSame   = StartBidPrice == other.StartBidAsk.BidPrice;
        var startAskPriceSame   = StartAskPrice == other.StartBidAsk.AskPrice;
        var highestBidPriceSame = HighestBidPrice == other.HighestBidAsk.BidPrice;
        var highestAskPriceSame = HighestAskPrice == other.HighestBidAsk.AskPrice;
        var lowestBidPriceSame  = LowestBidPrice == other.LowestBidAsk.BidPrice;
        var lowestAskPriceSame  = LowestAskPrice == other.LowestBidAsk.AskPrice;
        var endBidPriceSame     = EndBidPrice == other.EndBidAsk.BidPrice;
        var endAskPriceSame     = EndAskPrice == other.EndBidAsk.AskPrice;
        var tickCountSame       = TickCount == other.TickCount;
        var periodVolumeSame    = PeriodVolume == other.PeriodVolume;
        var candleFlagsSame     = CandleFlags == other.CandleFlags;
        var averageBidPriceSame = AverageBidPrice == other.AverageBidAsk.BidPrice;
        var averageAskPriceSame = AverageAskPrice == other.AverageBidAsk.AskPrice;

        var allAreSame = timeFrameSame && startTimeSame && endTimeSame && startBidPriceSame && startAskPriceSame
                      && highestBidPriceSame && highestAskPriceSame && lowestBidPriceSame && lowestAskPriceSame && endBidPriceSame
                      && endAskPriceSame && tickCountSame && periodVolumeSame && candleFlagsSame && averageBidPriceSame
                      && averageAskPriceSame;
        return allAreSame;
    }

    IMutableCandle ITrackableReset<IMutableCandle>.ResetWithTracking() => ResetWithTracking();

    public Candle ResetWithTracking()
    {
        CandleFlags        = CandleFlags.None;
        TimeBoundaryPeriod = TimeBoundaryPeriod.Tick;
        PeriodStartTime    = DateTime.MinValue;
        PeriodEndTime      = DateTime.MinValue;

        StartBidPrice   = 0m;
        StartAskPrice   = 0m;
        HighestBidPrice = 0m;
        HighestAskPrice = 0m;
        LowestBidPrice  = 0m;
        LowestAskPrice  = 0m;
        EndBidPrice     = 0m;
        EndAskPrice     = 0m;
        AverageBidPrice = 0m;
        AverageAskPrice = 0m;
        TickCount    = 0;
        PeriodVolume = 0;

        return this;
    }

    public override void StateReset()
    {
        Next    = Previous = null;
        IsEmpty = true;
        base.StateReset();
    }

    public void Configure
    (TimeBoundaryPeriod timeBoundaryPeriod = TimeBoundaryPeriod.Tick, DateTime? startTime = null, DateTime? endTime = null,
        decimal startBidPrice = 0m, decimal startAskPrice = 0m, decimal highestBidPrice = 0m, decimal highestAskPrice = 0m,
        decimal lowestBidPrice = 0m, decimal lowestAskPrice = 0m, decimal endBidPrice = 0m, decimal endAskPrice = 0m, uint tickCount = 0u,
        long periodVolume = 0L, decimal averageBidPrice = 0m, decimal averageAskPrice = 0m
      , CandleFlags candleFlags = CandleFlags.None)
    {
        TimeBoundaryPeriod = timeBoundaryPeriod;
        PeriodStartTime    = startTime ?? DateTime.MinValue;
        PeriodEndTime      = endTime ?? DateTime.MinValue;
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
        CandleFlags        = candleFlags;
        AverageBidPrice    = averageBidPrice;
        AverageAskPrice    = averageAskPrice;
    }

    public void Configure(ICandle toClone)
    {
        TimeBoundaryPeriod = toClone.TimeBoundaryPeriod;
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
        CandleFlags        = toClone.CandleFlags;
        AverageBidPrice    = toClone.AverageBidAsk.BidPrice;
        AverageAskPrice    = toClone.AverageBidAsk.AskPrice;
    }

    public override ICandle CopyFrom(ICandle source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        TimeBoundaryPeriod = source.TimeBoundaryPeriod;
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
        CandleFlags        = source.CandleFlags;
        AverageBidPrice    = source.AverageBidAsk.BidPrice;
        AverageAskPrice    = source.AverageBidAsk.AskPrice;
        return this;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent((ICandle?)obj, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = (int)TimeBoundaryPeriod;
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
            hashCode = (hashCode * 397) ^ CandleFlags.GetHashCode();
            hashCode = (hashCode * 397) ^ AverageBidPrice.GetHashCode();
            hashCode = (hashCode * 397) ^ AverageAskPrice.GetHashCode();
            return hashCode;
        }
    }

    public override string ToString() =>
        $"{nameof(Candle)} ({nameof(TimeBoundaryPeriod)}: {TimeBoundaryPeriod}, {nameof(PeriodStartTime)}: {PeriodStartTime:O}, " +
        $"{nameof(PeriodEndTime)}: {PeriodEndTime:O}, {nameof(StartBidPrice)}: {StartBidPrice:N5}, " +
        $"{nameof(StartAskPrice)}: {StartAskPrice:N5}, {nameof(HighestBidPrice)}: {HighestBidPrice:N5}, " +
        $"{nameof(HighestAskPrice)}: {HighestAskPrice:N5}, {nameof(LowestBidPrice)}: {LowestBidPrice:N5}, " +
        $"{nameof(LowestAskPrice)}: {LowestAskPrice:N5}, {nameof(EndBidPrice)}: {EndBidPrice:N5}, " +
        $"{nameof(EndAskPrice)}: {EndAskPrice:N5}, {nameof(TickCount)}: {TickCount}, {nameof(PeriodVolume)}: {PeriodVolume:N2}, " +
        $"{nameof(CandleFlags)}: {CandleFlags}, {nameof(AverageBidPrice)}: {AverageBidPrice}, {nameof(AverageAskPrice)}: {AverageAskPrice})";
}
