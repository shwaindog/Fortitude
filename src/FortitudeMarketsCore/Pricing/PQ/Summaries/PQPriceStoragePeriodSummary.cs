// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types;
using FortitudeIO.TimeSeries;
using FortitudeMarketsApi.Pricing;
using FortitudeMarketsApi.Pricing.Summaries;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using static FortitudeMarketsCore.Pricing.PQ.Summaries.PQPriceStorageSummaryFlags;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Summaries;

[Flags]
public enum PQPriceStorageSummaryFlags : uint
{
    None                             = 0x00_00
  , IsNextTimePeriod                 = 0x00_01
  , Snapshot                         = 0x00_02
  , PricesStartSameAsLastEndPrices   = 0x00_04
  , HasSummaryFlagsChanges           = 0x00_08
  , NegateDeltaSummaryFlagsUpperByte = 0x00_10
  , NegateDeltaSummaryFlagsLowerByte = 0x00_20
  , NegateDeltaStartBidPrice         = 0x00_40
  , NegateDeltaStartAskPrice         = 0x00_80
  , NegateDeltaHighestBidPrice       = 0x01_00
  , NegateDeltaHighestAskPrice       = 0x02_00
  , NegateDeltaLowestBidPrice        = 0x04_00
  , NegateDeltaLowestAskPrice        = 0x08_00
  , NegateDeltaEndBidPrice           = 0x10_00
  , NegateDeltaEndAskPrice           = 0x20_00
  , TickCountSameAsPrevious          = 0x40_00
  , NegateDeltaTickCount             = 0x80_00
  , NegateDeltaPeriodVolume          = 0x01_00_00
  , NegateDeltaAverageBidPrice       = 0x02_00_00
  , NegateDeltaAverageAskPrice       = 0x04_00_00
  , PriceScaleMask                   = 0x0F_00_00_00
  , VolumeScaleMask                  = 0xF0_00_00_00
  , PriceVolumeScaleMask             = 0xFF_00_00_00
  , SnapshotClearMask                = 0xFF_07_FF_FD
  , SetSnapshotMask                  = 0xFF_00_00_02
  , AllFlags                         = 0xFF_07_FF_FF
}

public static class PQPriceStorageSummaryFlagsExtensions
{
    public static bool HasSnapshotFlag(this PQPriceStorageSummaryFlags flags) => (flags & Snapshot) > 0;

    public static int SignMultiplier(this PQPriceStorageSummaryFlags flags, PQPriceStorageSummaryFlags checkNegative) =>
        (flags & checkNegative) > 0 ? -1 : 1;
}

public interface IPQPriceStoragePeriodSummary : IMutablePricePeriodSummary, ITracksChanges<IPQPriceStoragePeriodSummary>
{
    PQPriceStorageSummaryFlags SummaryStorageFlags { get; }

    IPQPriceVolumePublicationPrecisionSettings? PrecisionSettings { get; }

    uint DeltaPeriodsFromPrevious    { get; set; }
    uint DeltaStartBidPrice          { get; set; }
    uint DeltaStartAskPrice          { get; set; }
    uint DeltaHighestBidPrice        { get; set; }
    uint DeltaHighestAskPrice        { get; set; }
    uint DeltaLowestBidPrice         { get; set; }
    uint DeltaLowestAskPrice         { get; set; }
    uint DeltaEndBidPrice            { get; set; }
    uint DeltaEndAskPrice            { get; set; }
    uint DeltaTickCount              { get; set; }
    uint DeltaPeriodVolume           { get; set; }
    uint DeltaSummaryFlagsUpperBytes { get; set; }
    uint DeltaSummaryFlagsLowerBytes { get; set; }
    uint DeltaAverageBidPrice        { get; set; }
    uint DeltaAverageAskPrice        { get; set; }
    byte VolumePricePrecisionScale   { get; }

    void CalculatedScaledDeltas();
    void CalculateAllFromNewDeltas();

    new IPQPriceStoragePeriodSummary Clone();
}

public class PQPriceStoragePeriodSummary : ReusableObject<IPricePeriodSummary>, IPQPriceStoragePeriodSummary
  , ITimeSeriesEntry<PQPriceStoragePeriodSummary>, ICloneable<PQPriceStoragePeriodSummary>
{
    private decimal averageAskPrice;
    private decimal averageBidPrice;
    private uint    deltaPeriodsFromPrevious;
    private decimal endAskPrice;
    private decimal endBidPrice;
    private decimal highestAskPrice;
    private decimal highestBidPrice;
    private decimal lowestAskPrice;
    private decimal lowestBidPrice;

    private PricePeriodSummaryFlags periodSummaryFlags;

    private long periodVolume;

    private decimal previousAverageAskPrice;
    private decimal previousAverageBidPrice;
    private decimal previousEndAskPrice;
    private decimal previousEndBidPrice;
    private decimal previousHighestAskPrice;
    private decimal previousHighestBidPrice;
    private decimal previousLowestAskPrice;
    private decimal previousLowestBidPrice;
    private long    previousPeriodVolume;

    private DateTime startTime = DateTimeConstants.UnixEpoch;
    private uint     tickCount;

    public PQPriceStoragePeriodSummary()
    {
        SummaryStorageFlags |= Snapshot;
        PeriodSummaryFlags  =  PricePeriodSummaryFlags.FromStorage;
    }

    public PQPriceStoragePeriodSummary(IPQPriceStoragePeriodSummary toClone)
    {
        PrecisionSettings  = toClone.PrecisionSettings;
        TimeSeriesPeriod   = toClone.TimeSeriesPeriod;
        PeriodStartTime    = toClone.PeriodStartTime;
        PeriodEndTime      = toClone.PeriodEndTime;
        StartBidPrice      = toClone.StartBidPrice;
        StartAskPrice      = toClone.StartAskPrice;
        HighestBidPrice    = toClone.HighestBidPrice;
        HighestAskPrice    = toClone.HighestAskPrice;
        LowestBidPrice     = toClone.LowestBidPrice;
        LowestAskPrice     = toClone.LowestAskPrice;
        EndBidPrice        = toClone.EndBidPrice;
        EndAskPrice        = toClone.EndAskPrice;
        TickCount          = toClone.TickCount;
        PeriodVolume       = toClone.PeriodVolume;
        PeriodSummaryFlags = toClone.PeriodSummaryFlags | PricePeriodSummaryFlags.FromStorage;
        AverageBidPrice    = toClone.AverageBidPrice;
        AverageAskPrice    = toClone.AverageBidPrice;
    }

    public override PQPriceStoragePeriodSummary Clone() =>
        Recycler?.Borrow<PQPriceStoragePeriodSummary>().CopyFrom(this) as PQPriceStoragePeriodSummary ?? new PQPriceStoragePeriodSummary(this);

    public PQPriceStorageSummaryFlags SummaryStorageFlags { get; private set; }

    public PricePeriodSummaryFlags PeriodSummaryFlags
    {
        get => periodSummaryFlags;
        set
        {
            var upperCurrent  = (uint)periodSummaryFlags >> 16;
            var newUpperValue = (uint)value >> 16;
            var deltaUpper =
                SummaryStorageFlags.SignMultiplier(NegateDeltaSummaryFlagsUpperByte) * DeltaSummaryFlagsUpperBytes
              + (int)newUpperValue - (int)upperCurrent;
            if (deltaUpper < 0)
                SummaryStorageFlags |= NegateDeltaSummaryFlagsUpperByte;
            else
                SummaryStorageFlags &= AllFlags & ~NegateDeltaSummaryFlagsUpperByte;
            DeltaSummaryFlagsUpperBytes = (uint)Math.Abs(deltaUpper);
            var lowerCurrent  = (ushort)periodSummaryFlags;
            var newLowerValue = (ushort)(value | PricePeriodSummaryFlags.FromStorage);
            var deltaLower =
                SummaryStorageFlags.SignMultiplier(NegateDeltaSummaryFlagsLowerByte) * DeltaSummaryFlagsLowerBytes
              + newLowerValue - lowerCurrent;
            if (deltaLower < 0)
                SummaryStorageFlags |= NegateDeltaSummaryFlagsLowerByte;
            else
                SummaryStorageFlags &= AllFlags & ~NegateDeltaSummaryFlagsLowerByte;
            DeltaSummaryFlagsLowerBytes = (uint)Math.Abs(deltaLower);
            if (deltaUpper > 0 || deltaLower > 0)
                SummaryStorageFlags |= HasSummaryFlagsChanges;
            else
                SummaryStorageFlags &= AllFlags & ~HasSummaryFlagsChanges;
            periodSummaryFlags = value | PricePeriodSummaryFlags.FromStorage;
        }
    }

    public BoundedTimeRange ToBoundedTimeRange(DateTime? maxDateTime = null) => new(PeriodStartTime, PeriodEndTime.Min(maxDateTime));

    public bool IsWhollyBoundedBy
        (ITimeSeriesPeriodRange parentRange) =>
        parentRange.PeriodStartTime <= PeriodStartTime && parentRange.PeriodEnd() >= PeriodEndTime;

    public BidAskPair StartBidAsk   => new(StartBidPrice, StartAskPrice);
    public BidAskPair HighestBidAsk => new(HighestBidPrice, HighestAskPrice);
    public BidAskPair LowestBidAsk  => new(LowestBidPrice, LowestAskPrice);
    public BidAskPair EndBidAsk     => new(EndBidPrice, EndAskPrice);
    public BidAskPair AverageBidAsk => new(AverageBidPrice, AverageAskPrice);

    public uint DeltaPeriodsFromPrevious
    {
        get => deltaPeriodsFromPrevious;
        set
        {
            if (value == 1) SummaryStorageFlags |= IsNextTimePeriod;
            deltaPeriodsFromPrevious = value;
        }
    }

    public uint DeltaStartBidPrice   { get; set; }
    public uint DeltaStartAskPrice   { get; set; }
    public uint DeltaHighestBidPrice { get; set; }
    public uint DeltaHighestAskPrice { get; set; }
    public uint DeltaLowestBidPrice  { get; set; }
    public uint DeltaLowestAskPrice  { get; set; }
    public uint DeltaEndBidPrice     { get; set; }
    public uint DeltaEndAskPrice     { get; set; }

    public IPQPriceVolumePublicationPrecisionSettings? PrecisionSettings { get; private set; }

    public byte VolumePricePrecisionScale =>
        PrecisionSettings != null
            ? (byte)(((PrecisionSettings.VolumeScalingPrecision & 0xF) << 4)
                   | (PrecisionSettings.PriceScalingPrecision & 0xF))
            : (byte)0x88;

    public uint DeltaTickCount       { get; set; }
    public uint DeltaPeriodVolume    { get; set; }
    public uint DeltaAverageBidPrice { get; set; }
    public uint DeltaAverageAskPrice { get; set; }

    public uint DeltaSummaryFlagsUpperBytes { get; set; }
    public uint DeltaSummaryFlagsLowerBytes { get; set; }

    public TimeSeriesPeriod TimeSeriesPeriod { get; set; }

    public bool IsEmpty
    {
        get
        {
            var pricesAreAllZero =
                StartBidPrice == decimal.Zero && StartAskPrice == decimal.Zero && HighestBidPrice == decimal.Zero &&
                HighestAskPrice == decimal.Zero && LowestBidPrice == decimal.Zero && LowestAskPrice == decimal.Zero &&
                EndBidPrice == decimal.Zero && EndAskPrice == decimal.Zero &&
                AverageBidPrice == decimal.Zero && AverageAskPrice == decimal.Zero;
            var tickCountAndVolumeZero    = TickCount == 0 && PeriodVolume == 0;
            var summaryPeriodNone         = TimeSeriesPeriod == TimeSeriesPeriod.None;
            var summaryFlagsNoneOrStorage = PeriodSummaryFlags is PricePeriodSummaryFlags.FromStorage or PricePeriodSummaryFlags.None;
            var startEndTimeUnixEpoch = PeriodStartTime == DateTimeConstants.UnixEpoch
                                     && PeriodEndTime == DateTimeConstants.UnixEpoch;
            return pricesAreAllZero && tickCountAndVolumeZero && summaryPeriodNone && startEndTimeUnixEpoch && summaryFlagsNoneOrStorage;
        }
        set
        {
            if (!value) return;
            StartBidPrice  = StartAskPrice  = HighestBidPrice = HighestAskPrice = AverageBidPrice = decimal.Zero;
            LowestBidPrice = LowestAskPrice = EndBidPrice     = EndAskPrice     = AverageAskPrice = decimal.Zero;

            previousHighestBidPrice = previousHighestAskPrice = previousLowestBidPrice  = previousLowestAskPrice  = -1m;
            previousEndBidPrice     = previousEndAskPrice     = previousAverageBidPrice = previousAverageAskPrice = -1m;
            previousPeriodVolume    = -1L;

            TickCount           = 0;
            PeriodVolume        = 0;
            TimeSeriesPeriod    = TimeSeriesPeriod.None;
            PeriodSummaryFlags  = PricePeriodSummaryFlags.FromStorage;
            PeriodStartTime     = PeriodEndTime = DateTimeConstants.UnixEpoch;
            SummaryStorageFlags = None;
        }
    }

    public DateTime PeriodStartTime
    {
        get => startTime;
        set
        {
            if (PeriodEndTime > DateTimeConstants.UnixEpoch)
            {
                var  currentStartTime   = PeriodEndTime;
                var  haveFoundStartTime = currentStartTime == value;
                uint countPeriod        = 1;
                while (!haveFoundStartTime)
                {
                    currentStartTime = TimeSeriesPeriod.PeriodEnd(currentStartTime);
                    countPeriod++;
                    haveFoundStartTime = currentStartTime >= value;
                }
                DeltaPeriodsFromPrevious = countPeriod;
            }
            startTime     = value;
            PeriodEndTime = TimeSeriesPeriod.PeriodEnd(startTime);
        }
    }

    public DateTime PeriodEndTime { get; set; } = DateTimeConstants.UnixEpoch;

    DateTime ITimeSeriesEntry<IPricePeriodSummary>.StorageTime(IStorageTimeResolver<IPricePeriodSummary>? resolver) => PeriodEndTime;

    public decimal AverageBidPrice
    {
        get => averageBidPrice;
        set
        {
            if (previousAverageBidPrice < 0) previousAverageBidPrice = averageBidPrice;
            averageBidPrice = value;
        }
    }
    public decimal AverageAskPrice
    {
        get => averageAskPrice;
        set
        {
            if (previousAverageAskPrice < 0) previousAverageAskPrice = averageAskPrice;
            averageAskPrice = value;
        }
    }

    public decimal StartBidPrice { get; set; }

    public decimal StartAskPrice { get; set; }

    public decimal HighestBidPrice
    {
        get => highestBidPrice;
        set
        {
            if (previousHighestBidPrice < 0) previousHighestBidPrice = highestBidPrice;
            highestBidPrice = value;
        }
    }

    public decimal HighestAskPrice
    {
        get => highestAskPrice;
        set
        {
            if (previousHighestAskPrice < 0) previousHighestAskPrice = highestAskPrice;
            highestAskPrice = value;
        }
    }

    public decimal LowestBidPrice
    {
        get => lowestBidPrice;
        set
        {
            if (previousLowestBidPrice < 0) previousLowestBidPrice = lowestBidPrice;
            lowestBidPrice = value;
        }
    }

    public decimal LowestAskPrice
    {
        get => lowestAskPrice;
        set
        {
            if (previousLowestAskPrice < 0) previousLowestAskPrice = lowestAskPrice;
            lowestAskPrice = value;
        }
    }

    public decimal EndBidPrice
    {
        get => endBidPrice;
        set
        {
            if (previousEndBidPrice < 0) previousEndBidPrice = endBidPrice;
            endBidPrice = value;
        }
    }

    public decimal EndAskPrice
    {
        get => endAskPrice;
        set
        {
            if (previousEndAskPrice < 0) previousEndAskPrice = endAskPrice;
            endAskPrice = value;
        }
    }

    public uint TickCount
    {
        get => tickCount;
        set
        {
            var current  = tickCount;
            var newValue = value;
            var delta =
                SummaryStorageFlags.SignMultiplier(NegateDeltaTickCount) * DeltaTickCount
              + (int)newValue - (int)current;
            if (delta < 0)
            {
                SummaryStorageFlags &= AllFlags & ~TickCountSameAsPrevious;
                SummaryStorageFlags |= NegateDeltaTickCount;
            }
            else if (delta > 0)
            {
                SummaryStorageFlags &= AllFlags & ~NegateDeltaTickCount & ~TickCountSameAsPrevious;
            }
            else
            {
                SummaryStorageFlags |= TickCountSameAsPrevious;
            }
            DeltaTickCount = (uint)Math.Abs(delta);
            tickCount      = value;
        }
    }

    public long PeriodVolume
    {
        get => periodVolume;
        set
        {
            if (previousPeriodVolume < 0) previousPeriodVolume = periodVolume;
            periodVolume = value;
        }
    }

    public bool HasUpdates
    {
        get => SummaryStorageFlags > 0;
        set
        {
            if (value) return;
            SummaryStorageFlags = None;

            previousHighestBidPrice = previousHighestAskPrice = previousLowestBidPrice  = previousLowestAskPrice  = -1m;
            previousEndBidPrice     = previousEndAskPrice     = previousAverageBidPrice = previousAverageAskPrice = -1m;
            previousPeriodVolume    = -1L;

            DeltaStartBidPrice          = DeltaStartAskPrice          = DeltaHighestBidPrice = DeltaHighestAskPrice = 0;
            DeltaLowestBidPrice         = DeltaLowestAskPrice         = DeltaEndBidPrice     = DeltaEndAskPrice     = 0;
            DeltaTickCount              = DeltaPeriodVolume           = DeltaAverageBidPrice = DeltaAverageAskPrice = 0;
            DeltaSummaryFlagsLowerBytes = DeltaSummaryFlagsUpperBytes = 0;
        }
    }

    public IPricePeriodSummary? Previous { get; set; }
    public IPricePeriodSummary? Next     { get; set; }


    public IPQPriceStoragePeriodSummary CopyFrom
        (IPQPriceStoragePeriodSummary ps, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default) =>
        (IPQPriceStoragePeriodSummary)CopyFrom((IPricePeriodSummary)ps, copyMergeFlags);


    public override IPricePeriodSummary CopyFrom(IPricePeriodSummary ps, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (copyMergeFlags.HasFullReplace()) IsEmpty = true;
        TimeSeriesPeriod   = ps.TimeSeriesPeriod;
        PeriodStartTime    = ps.PeriodStartTime;
        StartBidPrice      = ps.StartBidAsk.BidPrice;
        StartAskPrice      = ps.StartBidAsk.AskPrice;
        HighestBidPrice    = ps.HighestBidAsk.BidPrice;
        HighestAskPrice    = ps.HighestBidAsk.AskPrice;
        LowestBidPrice     = ps.LowestBidAsk.BidPrice;
        LowestAskPrice     = ps.LowestBidAsk.AskPrice;
        EndBidPrice        = ps.EndBidAsk.BidPrice;
        EndAskPrice        = ps.EndBidAsk.AskPrice;
        TickCount          = ps.TickCount;
        PeriodVolume       = ps.PeriodVolume;
        PeriodSummaryFlags = ps.PeriodSummaryFlags | PricePeriodSummaryFlags.FromStorage;
        AverageBidPrice    = ps.AverageBidAsk.BidPrice;
        AverageAskPrice    = ps.AverageBidAsk.AskPrice;

        if (copyMergeFlags.HasFullReplace())
            SummaryStorageFlags |= Snapshot;
        else
            SummaryStorageFlags &= SnapshotClearMask;

        return this;
    }

    public override void StateReset()
    {
        Next       = Previous = null;
        IsEmpty    = true;
        HasUpdates = false;
        base.StateReset();
    }

    IStoreState IStoreState.CopyFrom(IStoreState source, CopyMergeFlags copyMergeFlags) => CopyFrom((IPricePeriodSummary)source, copyMergeFlags);

    IMutablePricePeriodSummary IMutablePricePeriodSummary.Clone() =>
        Recycler?.Borrow<PQPriceStoragePeriodSummary>().CopyFrom(this) as IMutablePricePeriodSummary ?? new PQPriceStoragePeriodSummary(this);

    IReusableObject<IPricePeriodSummary> IStoreState<IReusableObject<IPricePeriodSummary>>.CopyFrom
        (IReusableObject<IPricePeriodSummary> source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom((IMutablePricePeriodSummary)source, copyMergeFlags);

    IPQPriceStoragePeriodSummary IPQPriceStoragePeriodSummary.Clone() => Clone();

    object ICloneable.Clone() => Clone();

    IPricePeriodSummary ICloneable<IPricePeriodSummary>.Clone() => Clone();

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
        var periodSummaryFlagsSame = PeriodSummaryFlags == (other.PeriodSummaryFlags | PricePeriodSummaryFlags.FromStorage);
        var averageBidSame         = AverageBidPrice == other.AverageBidAsk.BidPrice;
        var averageAskSame         = AverageAskPrice == other.AverageBidAsk.AskPrice;

        var updateFlagsSame = true;
        if (exactTypes)
        {
            var otherPQ = (PQPriceStoragePeriodSummary)other;
            updateFlagsSame = SummaryStorageFlags == otherPQ.SummaryStorageFlags;
        }

        var allAreSame = timeFrameSame && startTimeSame && endTimeSame && startBidPriceSame && startAskPriceSame
                      && highestBidPriceSame && highestAskPriceSame && lowestBidPriceSame && lowestAskPriceSame
                      && endBidPriceSame && endAskPriceSame && tickCountSame && periodVolumeSame && periodSummaryFlagsSame
                      && updateFlagsSame && averageBidSame && averageAskSame;
        return allAreSame;
    }

    public void CalculateAllFromNewDeltas()
    {
        DeltaStartBidPrice          = DeltaStartAskPrice          = DeltaHighestBidPrice = DeltaHighestAskPrice = 0;
        DeltaLowestBidPrice         = DeltaLowestAskPrice         = DeltaEndBidPrice     = DeltaEndAskPrice     = 0;
        DeltaTickCount              = DeltaPeriodVolume           = DeltaAverageBidPrice = DeltaAverageAskPrice = 0;
        DeltaSummaryFlagsLowerBytes = DeltaSummaryFlagsUpperBytes = 0;

        previousHighestBidPrice = previousHighestAskPrice = previousLowestBidPrice  = previousLowestAskPrice  = 0m;
        previousEndBidPrice     = previousEndAskPrice     = previousAverageBidPrice = previousAverageAskPrice = 0m;
        previousPeriodVolume    = 0;

        CalculatedScaledDeltas();
        DeltaTickCount = TickCount;
    }

    public void CalculatedScaledDeltas()
    {
        PrecisionSettings  = GetProposedVolumeScaleFactor();
        DeltaStartBidPrice = GetPriceDecimalDeltaAndFlags(StartBidPrice, previousEndBidPrice, NegateDeltaStartBidPrice, DeltaStartBidPrice);
        DeltaStartAskPrice = GetPriceDecimalDeltaAndFlags(StartAskPrice, previousEndAskPrice, NegateDeltaStartAskPrice, DeltaStartAskPrice);
        if (DeltaStartAskPrice + DeltaStartBidPrice == 0) SummaryStorageFlags |= PricesStartSameAsLastEndPrices;
        DeltaHighestBidPrice
            = GetPriceDecimalDeltaAndFlags(highestBidPrice, previousHighestBidPrice, NegateDeltaHighestBidPrice, DeltaHighestBidPrice);
        DeltaHighestAskPrice
            = GetPriceDecimalDeltaAndFlags(highestAskPrice, previousHighestAskPrice, NegateDeltaHighestAskPrice, DeltaHighestAskPrice);
        DeltaLowestBidPrice = GetPriceDecimalDeltaAndFlags(lowestBidPrice, previousLowestBidPrice, NegateDeltaLowestBidPrice, DeltaLowestBidPrice);
        DeltaLowestAskPrice = GetPriceDecimalDeltaAndFlags(lowestAskPrice, previousLowestAskPrice, NegateDeltaLowestAskPrice, DeltaLowestAskPrice);
        DeltaEndBidPrice    = GetPriceDecimalDeltaAndFlags(endBidPrice, previousEndBidPrice, NegateDeltaEndBidPrice, DeltaEndBidPrice);
        DeltaEndAskPrice    = GetPriceDecimalDeltaAndFlags(endAskPrice, previousEndAskPrice, NegateDeltaEndAskPrice, DeltaEndAskPrice);
        DeltaAverageBidPrice
            = GetPriceDecimalDeltaAndFlags(averageBidPrice, previousAverageBidPrice, NegateDeltaAverageBidPrice, DeltaAverageBidPrice);
        DeltaAverageAskPrice
            = GetPriceDecimalDeltaAndFlags(averageAskPrice, previousAverageAskPrice, NegateDeltaAverageAskPrice, DeltaAverageAskPrice);
        DeltaPeriodVolume = GetVolumeLongDeltaAndFlags(periodVolume, previousPeriodVolume, NegateDeltaPeriodVolume, DeltaPeriodVolume);
    }

    public DateTime StorageTime(IStorageTimeResolver<PQPriceStoragePeriodSummary>? resolver = null) => PeriodEndTime;

    private uint GetPriceDecimalDeltaAndFlags(decimal newValue, decimal oldValue, PQPriceStorageSummaryFlags negateFlag, uint previousDelta)
    {
        var oldScaled = PQScaling.Scale(oldValue, (byte)(PrecisionSettings!.PriceScalingPrecision & 0x1F));
        var newScaled = PQScaling.Scale(newValue, (byte)(PrecisionSettings!.PriceScalingPrecision & 0x1F));
        var delta =
            SummaryStorageFlags.SignMultiplier(negateFlag) * previousDelta + (int)newScaled - (int)oldScaled;
        if (delta < 0)
            SummaryStorageFlags |= negateFlag;
        else
            SummaryStorageFlags &= AllFlags & ~negateFlag;
        return (uint)Math.Abs(delta);
    }

    private uint GetVolumeLongDeltaAndFlags(long newValue, long oldValue, PQPriceStorageSummaryFlags negateFlag, uint previousDelta)
    {
        var oldScaled = PQScaling.Scale(oldValue, (byte)(PrecisionSettings!.VolumeScalingPrecision & 0x1F));
        var newScaled = PQScaling.Scale(newValue, (byte)(PrecisionSettings!.VolumeScalingPrecision & 0x1F));
        var delta =
            SummaryStorageFlags.SignMultiplier(negateFlag) * previousDelta + (int)newScaled - (int)oldScaled;
        if (delta < 0)
            SummaryStorageFlags |= negateFlag;
        else
            SummaryStorageFlags &= AllFlags & ~negateFlag;
        return (uint)Math.Abs(delta);
    }

    private IPQPriceVolumePublicationPrecisionSettings GetProposedVolumeScaleFactor()
    {
        var priceScale = (byte)((PrecisionSettings?.PriceScalingPrecision ?? 15) & 0x0F);
        if (PrecisionSettings == null || DeltaStartBidPrice + DeltaStartAskPrice + DeltaHighestBidPrice
          + DeltaHighestAskPrice + DeltaLowestBidPrice + DeltaLowestAskPrice + DeltaEndBidPrice + DeltaEndAskPrice <= 0)
        {
            priceScale = StartBidPrice > 0 ? Math.Min(priceScale, PQScaling.FindPriceScaleFactor(StartBidPrice)) : priceScale;
            priceScale = StartAskPrice > 0 ? Math.Min(priceScale, PQScaling.FindPriceScaleFactor(StartAskPrice)) : priceScale;
            priceScale = HighestBidPrice > 0 ? Math.Min(priceScale, PQScaling.FindPriceScaleFactor(HighestBidPrice)) : priceScale;
            priceScale = HighestAskPrice > 0 ? Math.Min(priceScale, PQScaling.FindPriceScaleFactor(HighestAskPrice)) : priceScale;
            priceScale = LowestBidPrice > 0 ? Math.Min(priceScale, PQScaling.FindPriceScaleFactor(LowestBidPrice)) : priceScale;
            priceScale = LowestAskPrice > 0 ? Math.Min(priceScale, PQScaling.FindPriceScaleFactor(LowestAskPrice)) : priceScale;
            priceScale = EndBidPrice > 0 ? Math.Min(priceScale, PQScaling.FindPriceScaleFactor(EndBidPrice)) : priceScale;
            priceScale = EndAskPrice > 0 ? Math.Min(priceScale, PQScaling.FindPriceScaleFactor(EndAskPrice)) : priceScale;
            priceScale = previousHighestBidPrice > 0 ? Math.Min(priceScale, PQScaling.FindPriceScaleFactor(previousHighestBidPrice)) : priceScale;
            priceScale = previousHighestAskPrice > 0 ? Math.Min(priceScale, PQScaling.FindPriceScaleFactor(previousHighestAskPrice)) : priceScale;
            priceScale = previousLowestBidPrice > 0 ? Math.Min(priceScale, PQScaling.FindPriceScaleFactor(previousLowestBidPrice)) : priceScale;
            priceScale = previousLowestAskPrice > 0 ? Math.Min(priceScale, PQScaling.FindPriceScaleFactor(previousLowestAskPrice)) : priceScale;
            priceScale = previousEndBidPrice > 0 ? Math.Min(priceScale, PQScaling.FindPriceScaleFactor(previousEndBidPrice)) : priceScale;
            priceScale = previousEndAskPrice > 0 ? Math.Min(priceScale, PQScaling.FindPriceScaleFactor(previousEndAskPrice)) : priceScale;
            // don't use AverageBidAsk as it may have many fractional decimals places
        }

        var volumeScale = (byte)((PrecisionSettings?.VolumeScalingPrecision ?? 15) & 0x0F);

        if (PrecisionSettings == null || DeltaPeriodVolume <= 0)
        {
            volumeScale = previousPeriodVolume > 0 ? Math.Min(volumeScale, PQScaling.FindVolumeScaleFactor(previousPeriodVolume)) : volumeScale;
            volumeScale = periodVolume > 0 ? Math.Min(volumeScale, PQScaling.FindVolumeScaleFactor(periodVolume)) : volumeScale;
        }

        if (PrecisionSettings == null || (PrecisionSettings.PriceScalingPrecision & 0x0F) != priceScale
                                      || (PrecisionSettings.VolumeScalingPrecision & 0x0F) != volumeScale)
            PrecisionSettings = new PQPriceVolumePublicationPrecisionSettings(priceScale, volumeScale);

        SummaryStorageFlags &= AllFlags & ~PriceVolumeScaleMask;
        SummaryStorageFlags |= (PQPriceStorageSummaryFlags)((uint)VolumePricePrecisionScale << 24);
        return PrecisionSettings;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent((IPricePeriodSummary?)obj, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = TimeSeriesPeriod.GetHashCode();
            hashCode = (hashCode * 397) ^ startTime.GetHashCode();
            hashCode = (hashCode * 397) ^ PeriodEndTime.GetHashCode();
            hashCode = (hashCode * 397) ^ StartBidPrice.GetHashCode();
            hashCode = (hashCode * 397) ^ StartAskPrice.GetHashCode();
            hashCode = (hashCode * 397) ^ highestBidPrice.GetHashCode();
            hashCode = (hashCode * 397) ^ highestAskPrice.GetHashCode();
            hashCode = (hashCode * 397) ^ lowestBidPrice.GetHashCode();
            hashCode = (hashCode * 397) ^ lowestAskPrice.GetHashCode();
            hashCode = (hashCode * 397) ^ endBidPrice.GetHashCode();
            hashCode = (hashCode * 397) ^ endAskPrice.GetHashCode();
            hashCode = (hashCode * 397) ^ (int)tickCount;
            hashCode = (hashCode * 397) ^ periodVolume.GetHashCode();
            hashCode = (hashCode * 397) ^ PeriodSummaryFlags.GetHashCode();
            hashCode = (hashCode * 397) ^ averageBidPrice.GetHashCode();
            hashCode = (hashCode * 397) ^ averageAskPrice.GetHashCode();
            return hashCode;
        }
    }

    public override string ToString() =>
        $"PQPriceStoragePeriodSummary {{ {nameof(TimeSeriesPeriod)}: {TimeSeriesPeriod}, {nameof(PeriodStartTime)}: {PeriodStartTime}, " +
        $"{nameof(PeriodEndTime)}: {PeriodEndTime}, {nameof(StartBidPrice)}: {StartBidPrice}, {nameof(StartAskPrice)}:" +
        $" {StartAskPrice}, {nameof(HighestBidPrice)}: {HighestBidPrice}, {nameof(HighestAskPrice)}: " +
        $"{HighestAskPrice}, {nameof(LowestBidPrice)}: {LowestBidPrice}, {nameof(LowestAskPrice)}: " +
        $"{LowestAskPrice}, {nameof(EndBidPrice)}: {EndBidPrice}, {nameof(EndAskPrice)}: {EndAskPrice}, " +
        $"{nameof(TickCount)}: {TickCount}, {nameof(PeriodVolume)}: {PeriodVolume}, {nameof(PeriodSummaryFlags)}: {PeriodSummaryFlags}, " +
        $"{nameof(AverageBidPrice)}: {AverageBidPrice}, {nameof(AverageAskPrice)}: {AverageAskPrice} }}";
}
