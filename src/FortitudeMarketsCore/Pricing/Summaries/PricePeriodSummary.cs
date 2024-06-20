// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.Types;
using FortitudeIO.TimeSeries;
using FortitudeMarketsApi.Pricing.Summaries;

#endregion

namespace FortitudeMarketsCore.Pricing.Summaries;

public struct Candle
{
    public DateTime         SummaryStartTime;
    public TimeSeriesPeriod SummaryPeriod;

    public DateTime SummaryEndTime;
    public decimal  StartBidPrice;
    public decimal  StartAskPrice;
    public decimal  HighestBidPrice;
    public decimal  HighestAskPrice;
    public decimal  LowestBidPrice;
    public decimal  LowestAskPrice;
    public decimal  EndBidPrice;
    public decimal  EndAskPrice;

    public uint TickCount;
    public long PeriodVolume;
    public long AverageBidPrice;
    public long AverageAskPrice;
}

public class PricePeriodSummary : IMutablePricePeriodSummary
{
    public PricePeriodSummary
    (TimeSeriesPeriod timeSeriesPeriod = TimeSeriesPeriod.None, DateTime? startTime = null, DateTime? endTime = null,
        decimal startBidPrice = 0m, decimal startAskPrice = 0m, decimal highestBidPrice = 0m, decimal highestAskPrice = 0m,
        decimal lowestBidPrice = 0m, decimal lowestAskPrice = 0m, decimal endBidPrice = 0m, decimal endAskPrice = 0m, uint tickCount = 0u,
        long periodVolume = 0L, decimal averageBidPrice = 0m, decimal averageAskPrice = 0m)
    {
        SummaryPeriod    = timeSeriesPeriod;
        SummaryStartTime = startTime ?? DateTimeConstants.UnixEpoch;
        SummaryEndTime   = endTime ?? DateTimeConstants.UnixEpoch;
        StartBidPrice    = startBidPrice;
        StartAskPrice    = startAskPrice;
        HighestBidPrice  = highestBidPrice;
        HighestAskPrice  = highestAskPrice;
        LowestBidPrice   = lowestBidPrice;
        LowestAskPrice   = lowestAskPrice;
        EndBidPrice      = endBidPrice;
        EndAskPrice      = endAskPrice;
        TickCount        = tickCount;
        PeriodVolume     = periodVolume;
        AverageBidPrice  = averageBidPrice;
        AverageAskPrice  = averageAskPrice;
    }

    public PricePeriodSummary(IPricePeriodSummary toClone)
    {
        SummaryPeriod    = toClone.SummaryPeriod;
        SummaryStartTime = toClone.SummaryStartTime;
        SummaryEndTime   = toClone.SummaryEndTime;
        StartBidPrice    = toClone.StartBidPrice;
        StartAskPrice    = toClone.StartAskPrice;
        HighestBidPrice  = toClone.HighestBidPrice;
        HighestAskPrice  = toClone.HighestAskPrice;
        LowestBidPrice   = toClone.LowestBidPrice;
        LowestAskPrice   = toClone.LowestAskPrice;
        EndBidPrice      = toClone.EndBidPrice;
        EndAskPrice      = toClone.EndAskPrice;
        TickCount        = toClone.TickCount;
        PeriodVolume     = toClone.PeriodVolume;
        AverageBidPrice  = toClone.AverageBidPrice;
        AverageAskPrice  = toClone.AverageAskPrice;
    }

    public TimeSeriesPeriod SummaryPeriod { get; set; }
    public bool IsEmpty
    {
        get
        {
            var pricesAreAllZero = StartBidPrice == decimal.Zero && StartAskPrice == decimal.Zero && HighestBidPrice == decimal.Zero &&
                                   HighestAskPrice == decimal.Zero && LowestBidPrice == decimal.Zero && LowestAskPrice == decimal.Zero &&
                                   EndBidPrice == decimal.Zero && EndAskPrice == decimal.Zero &&
                                   AverageBidPrice == decimal.Zero && AverageAskPrice == decimal.Zero;
            var tickCountAndVolumeZero = TickCount == 0 && PeriodVolume == 0;
            var summaryPeriodNone      = SummaryPeriod == TimeSeriesPeriod.None;
            var startEndTimeUnixEpoch  = SummaryStartTime == DateTimeConstants.UnixEpoch && SummaryEndTime == DateTimeConstants.UnixEpoch;
            return pricesAreAllZero && tickCountAndVolumeZero && summaryPeriodNone && startEndTimeUnixEpoch;
        }
        set
        {
            if (!value) return;
            StartBidPrice    = StartAskPrice  = HighestBidPrice = HighestAskPrice = AverageBidPrice = decimal.Zero;
            LowestBidPrice   = LowestAskPrice = EndBidPrice     = EndAskPrice     = AverageAskPrice = decimal.Zero;
            TickCount        = 0;
            PeriodVolume     = 0;
            SummaryPeriod    = TimeSeriesPeriod.None;
            SummaryStartTime = SummaryEndTime = DateTimeConstants.UnixEpoch;
        }
    }

    public DateTime SummaryStartTime { get; set; }
    public DateTime SummaryEndTime   { get; set; }
    public decimal  StartBidPrice    { get; set; }
    public decimal  StartAskPrice    { get; set; }
    public decimal  HighestBidPrice  { get; set; }
    public decimal  HighestAskPrice  { get; set; }
    public decimal  LowestBidPrice   { get; set; }
    public decimal  LowestAskPrice   { get; set; }
    public decimal  EndBidPrice      { get; set; }
    public decimal  EndAskPrice      { get; set; }
    public uint     TickCount        { get; set; }
    public long     PeriodVolume     { get; set; }
    public decimal  AverageBidPrice  { get; set; }
    public decimal  AverageAskPrice  { get; set; }

    public DateTime StorageTime(IStorageTimeResolver<IPricePeriodSummary>? resolver = null) => SummaryEndTime;

    public IPricePeriodSummary CopyFrom(IPricePeriodSummary source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        SummaryPeriod    = source.SummaryPeriod;
        SummaryStartTime = source.SummaryStartTime;
        SummaryEndTime   = source.SummaryEndTime;
        StartBidPrice    = source.StartBidPrice;
        StartAskPrice    = source.StartAskPrice;
        HighestBidPrice  = source.HighestBidPrice;
        HighestAskPrice  = source.HighestAskPrice;
        LowestBidPrice   = source.LowestBidPrice;
        LowestAskPrice   = source.LowestAskPrice;
        EndBidPrice      = source.EndBidPrice;
        EndAskPrice      = source.EndAskPrice;
        TickCount        = source.TickCount;
        PeriodVolume     = source.PeriodVolume;
        AverageBidPrice  = source.AverageBidPrice;
        AverageAskPrice  = source.AverageAskPrice;
        return this;
    }

    public IStoreState CopyFrom(IStoreState source, CopyMergeFlags copyMergeFlags) => CopyFrom((IPricePeriodSummary)source, copyMergeFlags);

    object ICloneable.Clone() => Clone();

    IPricePeriodSummary ICloneable<IPricePeriodSummary>.Clone() => Clone();

    public IMutablePricePeriodSummary Clone() => new PricePeriodSummary(this);

    public bool AreEquivalent(IPricePeriodSummary? other, bool exactTypes = false)
    {
        if (other == null) return false;
        if (exactTypes && other.GetType() != GetType()) return false;
        var timeFrameSame       = SummaryPeriod == other.SummaryPeriod;
        var startTimeSame       = SummaryStartTime.Equals(other.SummaryStartTime);
        var endTimeSame         = SummaryEndTime.Equals(other.SummaryEndTime);
        var startBidPriceSame   = StartBidPrice == other.StartBidPrice;
        var startAskPriceSame   = StartAskPrice == other.StartAskPrice;
        var highestBidPriceSame = HighestBidPrice == other.HighestBidPrice;
        var highestAskPriceSame = HighestAskPrice == other.HighestAskPrice;
        var lowestBidPriceSame  = LowestBidPrice == other.LowestBidPrice;
        var lowestAskPriceSame  = LowestAskPrice == other.LowestAskPrice;
        var endBidPriceSame     = EndBidPrice == other.EndBidPrice;
        var endAskPriceSame     = EndAskPrice == other.EndAskPrice;
        var tickCountSame       = TickCount == other.TickCount;
        var periodVolumeSame    = PeriodVolume == other.PeriodVolume;
        var averageBidPriceSame = AverageBidPrice == other.AverageBidPrice;
        var averageAskPriceSame = AverageAskPrice == other.AverageAskPrice;

        var allAreSame = timeFrameSame && startTimeSame && endTimeSame && startBidPriceSame && startAskPriceSame
                      && highestBidPriceSame && highestAskPriceSame && lowestBidPriceSame && lowestAskPriceSame && endBidPriceSame
                      && endAskPriceSame && tickCountSame && periodVolumeSame && averageBidPriceSame && averageAskPriceSame;
        return allAreSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent((IPricePeriodSummary?)obj, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = (int)SummaryPeriod;
            hashCode = (hashCode * 397) ^ SummaryStartTime.GetHashCode();
            hashCode = (hashCode * 397) ^ SummaryEndTime.GetHashCode();
            hashCode = (hashCode * 397) ^ StartBidPrice.GetHashCode();
            hashCode = (hashCode * 397) ^ StartAskPrice.GetHashCode();
            hashCode = (hashCode * 397) ^ HighestBidPrice.GetHashCode();
            hashCode = (hashCode * 397) ^ HighestAskPrice.GetHashCode();
            hashCode = (hashCode * 397) ^ EndBidPrice.GetHashCode();
            hashCode = (hashCode * 397) ^ EndAskPrice.GetHashCode();
            hashCode = (hashCode * 397) ^ (int)TickCount;
            hashCode = (hashCode * 397) ^ PeriodVolume.GetHashCode();
            hashCode = (hashCode * 397) ^ AverageBidPrice.GetHashCode();
            hashCode = (hashCode * 397) ^ AverageAskPrice.GetHashCode();
            return hashCode;
        }
    }

    public override string ToString() =>
        $"{nameof(PricePeriodSummary)} ({nameof(SummaryPeriod)}: {SummaryPeriod}, {nameof(SummaryStartTime)}: {SummaryStartTime:O}, " +
        $"{nameof(SummaryEndTime)}: {SummaryEndTime:O}, {nameof(StartBidPrice)}: {StartBidPrice:N5}, " +
        $"{nameof(StartAskPrice)}: {StartAskPrice:N5}, {nameof(HighestBidPrice)}: {HighestBidPrice:N5}, " +
        $"{nameof(HighestAskPrice)}: {HighestAskPrice:N5}, {nameof(LowestBidPrice)}: {LowestBidPrice:N5}, " +
        $"{nameof(LowestAskPrice)}: {LowestAskPrice:N5}, {nameof(EndBidPrice)}: {EndBidPrice:N5}, " +
        $"{nameof(EndAskPrice)}: {EndAskPrice:N5}, {nameof(TickCount)}: {TickCount}, {nameof(PeriodVolume)}: {PeriodVolume:N2}, " +
        $"{nameof(AverageBidPrice)}: {AverageBidPrice}, {nameof(AverageAskPrice)}: {AverageAskPrice})";
}
