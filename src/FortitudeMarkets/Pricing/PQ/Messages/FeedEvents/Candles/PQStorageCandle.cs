﻿// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Storage.TimeSeries;
using FortitudeMarkets.Pricing.FeedEvents.Candles;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;
using static FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Candles.PQStorageCandleFlags;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Candles;

[Flags]
public enum PQStorageCandleFlags : uint
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

public static class PQStorageCandleFlagsExtensions
{
    public static bool HasSnapshotFlag(this PQStorageCandleFlags flags) => (flags & Snapshot) > 0;

    public static int SignMultiplier(this PQStorageCandleFlags flags, PQStorageCandleFlags checkNegative) => (flags & checkNegative) > 0 ? -1 : 1;
}

public interface IPQStorageCandle : IMutableCandle, ITracksChanges, ITrackableReset<IPQStorageCandle>
{
    PQStorageCandleFlags CandleStorageFlags { get; }

    IPQPriceVolumePublicationPrecisionSettings? PrecisionSettings { get; }

    uint DeltaPeriodsFromPrevious   { get; set; }
    uint DeltaStartBidPrice         { get; set; }
    uint DeltaStartAskPrice         { get; set; }
    uint DeltaHighestBidPrice       { get; set; }
    uint DeltaHighestAskPrice       { get; set; }
    uint DeltaLowestBidPrice        { get; set; }
    uint DeltaLowestAskPrice        { get; set; }
    uint DeltaEndBidPrice           { get; set; }
    uint DeltaEndAskPrice           { get; set; }
    uint DeltaTickCount             { get; set; }
    uint DeltaPeriodVolume          { get; set; }
    uint DeltaCandleFlagsUpperBytes { get; set; }
    uint DeltaCandleFlagsLowerBytes { get; set; }
    uint DeltaAverageBidPrice       { get; set; }
    uint DeltaAverageAskPrice       { get; set; }
    byte VolumePricePrecisionScale  { get; }

    void CalculatedScaledDeltas();
    void CalculateAllFromNewDeltas();

    new IPQStorageCandle Clone();
    new IPQStorageCandle ResetWithTracking();
}

public class PQStorageCandle : ReusableObject<ICandle>, IPQStorageCandle, ICloneable<PQStorageCandle>
{
    private   decimal averageAskPrice;
    private   decimal averageBidPrice;
    private   uint    deltaPeriodsFromPrevious;
    private   decimal endAskPrice;
    private   decimal endBidPrice;
    private   decimal highestAskPrice;
    private   decimal highestBidPrice;
    private   decimal lowestAskPrice;
    private   decimal lowestBidPrice;
    protected uint    SequenceId = uint.MaxValue;

    private CandleFlags candleFlags;

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

    private DateTime startTime = DateTime.MinValue;
    private uint     tickCount;

    public PQStorageCandle()
    {
        CandleStorageFlags |= Snapshot;
        CandleFlags        =  CandleFlags.FromStorage;

        if (GetType() == typeof(PQStorageCandle)) SequenceId = 0;
    }

    public PQStorageCandle(IPQStorageCandle toClone)
    {
        PrecisionSettings  = toClone.PrecisionSettings;
        TimeBoundaryPeriod = toClone.TimeBoundaryPeriod;
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
        CandleFlags        = toClone.CandleFlags | CandleFlags.FromStorage;
        AverageBidPrice    = toClone.AverageBidPrice;
        AverageAskPrice    = toClone.AverageBidPrice;

        if (GetType() == typeof(PQStorageCandle)) SequenceId = 0;
    }

    public PQStorageCandleFlags CandleStorageFlags { get; private set; }

    public CandleFlags CandleFlags
    {
        get => candleFlags;
        set
        {
            var upperCurrent  = (uint)candleFlags >> 16;
            var newUpperValue = (uint)value >> 16;
            var deltaUpper =
                CandleStorageFlags.SignMultiplier(NegateDeltaSummaryFlagsUpperByte) * DeltaCandleFlagsUpperBytes
              + (int)newUpperValue - (int)upperCurrent;
            if (deltaUpper < 0)
                CandleStorageFlags |= NegateDeltaSummaryFlagsUpperByte;
            else
                CandleStorageFlags &= AllFlags & ~NegateDeltaSummaryFlagsUpperByte;
            DeltaCandleFlagsUpperBytes = (uint)Math.Abs(deltaUpper);
            var lowerCurrent  = (ushort)candleFlags;
            var newLowerValue = (ushort)(value | CandleFlags.FromStorage);
            var deltaLower =
                CandleStorageFlags.SignMultiplier(NegateDeltaSummaryFlagsLowerByte) * DeltaCandleFlagsLowerBytes
              + newLowerValue - lowerCurrent;
            if (deltaLower < 0)
                CandleStorageFlags |= NegateDeltaSummaryFlagsLowerByte;
            else
                CandleStorageFlags &= AllFlags & ~NegateDeltaSummaryFlagsLowerByte;
            DeltaCandleFlagsLowerBytes = (uint)Math.Abs(deltaLower);
            if (deltaUpper > 0 || deltaLower > 0)
                CandleStorageFlags |= HasSummaryFlagsChanges;
            else
                CandleStorageFlags &= AllFlags & ~HasSummaryFlagsChanges;
            candleFlags = value | CandleFlags.FromStorage;
        }
    }

    public BoundedTimeRange ToBoundedTimeRange(DateTime? maxDateTime = null) => new(PeriodStartTime, PeriodEndTime.Min(maxDateTime));

    public bool IsWhollyBoundedBy
        (ITimeBoundaryPeriodRange parentRange) =>
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
            if (value == 1) CandleStorageFlags |= IsNextTimePeriod;
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
            ? (byte)(((byte)(PrecisionSettings.VolumeScalingPrecision & PQFieldFlags.DecimalScaleBits) << 4)
                   | (byte)(PrecisionSettings.PriceScalingPrecision & PQFieldFlags.DecimalScaleBits))
            : (byte)0x88;

    public uint DeltaTickCount       { get; set; }
    public uint DeltaPeriodVolume    { get; set; }
    public uint DeltaAverageBidPrice { get; set; }
    public uint DeltaAverageAskPrice { get; set; }

    public uint DeltaCandleFlagsUpperBytes { get; set; }
    public uint DeltaCandleFlagsLowerBytes { get; set; }

    public TimeBoundaryPeriod TimeBoundaryPeriod { get; set; }

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
            var candlePeriodNone          = TimeBoundaryPeriod == TimeBoundaryPeriod.Tick;
            var summaryFlagsNoneOrStorage = CandleFlags is CandleFlags.FromStorage or CandleFlags.None;
            var startEndTimeUnixEpoch = PeriodStartTime == DateTime.MinValue
                                     && PeriodEndTime == DateTime.MinValue;
            return pricesAreAllZero && tickCountAndVolumeZero && candlePeriodNone && startEndTimeUnixEpoch && summaryFlagsNoneOrStorage;
        }
        set
        {
            if (!value) return;
            StartBidPrice  = StartAskPrice  = HighestBidPrice = HighestAskPrice = AverageBidPrice = decimal.Zero;
            LowestBidPrice = LowestAskPrice = EndBidPrice     = EndAskPrice     = AverageAskPrice = decimal.Zero;

            previousHighestBidPrice = previousHighestAskPrice = previousLowestBidPrice  = previousLowestAskPrice  = -1m;
            previousEndBidPrice     = previousEndAskPrice     = previousAverageBidPrice = previousAverageAskPrice = -1m;
            previousPeriodVolume    = -1L;

            TickCount          = 0;
            PeriodVolume       = 0;
            TimeBoundaryPeriod = TimeBoundaryPeriod.Tick;
            CandleFlags        = CandleFlags.FromStorage;
            PeriodStartTime    = PeriodEndTime = DateTime.MinValue;
            CandleStorageFlags = None;

            SequenceId = 0;
        }
    }

    public DateTime PeriodStartTime
    {
        get => startTime;
        set
        {
            if (PeriodEndTime > DateTime.MinValue)
            {
                var  currentStartTime   = PeriodEndTime;
                var  haveFoundStartTime = currentStartTime == value;
                uint countPeriod        = 1;
                while (!haveFoundStartTime)
                {
                    currentStartTime = TimeBoundaryPeriod.PeriodEnd(currentStartTime);
                    countPeriod++;
                    haveFoundStartTime = currentStartTime >= value;
                }
                DeltaPeriodsFromPrevious = countPeriod;
            }
            startTime     = value;
            PeriodEndTime = TimeBoundaryPeriod.PeriodEnd(startTime);
        }
    }

    public DateTime PeriodEndTime { get; set; } = DateTime.MinValue;


    public DateTime StorageTime(IStorageTimeResolver? resolver)
    {
        if (resolver is IStorageTimeResolver<ICandle> candleResolver) return candleResolver.ResolveStorageTime(this);
        return PeriodEndTime;
    }

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
                CandleStorageFlags.SignMultiplier(NegateDeltaTickCount) * DeltaTickCount
              + (int)newValue - (int)current;
            if (delta < 0)
            {
                CandleStorageFlags &= AllFlags & ~TickCountSameAsPrevious;
                CandleStorageFlags |= NegateDeltaTickCount;
            }
            else if (delta > 0)
            {
                CandleStorageFlags &= AllFlags & ~NegateDeltaTickCount & ~TickCountSameAsPrevious;
            }
            else
            {
                CandleStorageFlags |= TickCountSameAsPrevious;
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
        get => CandleStorageFlags > 0;
        set
        {
            if (value) return;
            CandleStorageFlags = None;

            previousHighestBidPrice = previousHighestAskPrice = previousLowestBidPrice  = previousLowestAskPrice  = -1m;
            previousEndBidPrice     = previousEndAskPrice     = previousAverageBidPrice = previousAverageAskPrice = -1m;
            previousPeriodVolume    = -1L;

            DeltaStartBidPrice         = DeltaStartAskPrice         = DeltaHighestBidPrice = DeltaHighestAskPrice = 0;
            DeltaLowestBidPrice        = DeltaLowestAskPrice        = DeltaEndBidPrice     = DeltaEndAskPrice     = 0;
            DeltaTickCount             = DeltaPeriodVolume          = DeltaAverageBidPrice = DeltaAverageAskPrice = 0;
            DeltaCandleFlagsLowerBytes = DeltaCandleFlagsUpperBytes = 0;
        }
    }

    public ICandle? Previous { get; set; }

    public ICandle? Next { get; set; }

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

    public uint UpdateSequenceId => SequenceId;

    public void UpdateStarted(uint updateSequenceId)
    {
        SequenceId = updateSequenceId;
    }

    public void UpdateComplete(uint updateSequenceId = 0)
    {
        if (HasUpdates && !IsEmpty) SequenceId++;
        HasUpdates = false;
    }

    IMutableCandle ITrackableReset<IMutableCandle>.ResetWithTracking() => ResetWithTracking();

    IPQStorageCandle ITrackableReset<IPQStorageCandle>.ResetWithTracking() => ResetWithTracking();

    IPQStorageCandle IPQStorageCandle.ResetWithTracking() => ResetWithTracking();

    public PQStorageCandle ResetWithTracking()
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
        TickCount       = 0;
        PeriodVolume    = 0;


        DeltaStartBidPrice         = DeltaStartAskPrice         = DeltaHighestBidPrice = DeltaHighestAskPrice = 0;
        DeltaLowestBidPrice        = DeltaLowestAskPrice        = DeltaEndBidPrice     = DeltaEndAskPrice     = 0;
        DeltaTickCount             = DeltaPeriodVolume          = DeltaAverageBidPrice = DeltaAverageAskPrice = 0;
        DeltaCandleFlagsLowerBytes = DeltaCandleFlagsUpperBytes = 0;

        return this;
    }

    public override void StateReset()
    {
        Next       = Previous = null;
        IsEmpty    = true;
        HasUpdates = false;

        SequenceId = 0;
        base.StateReset();
    }

    IPQStorageCandle IPQStorageCandle.Clone() => Clone();

    object ICloneable.Clone() => Clone();

    ICandle ICloneable<ICandle>.Clone() => Clone();

    public override PQStorageCandle Clone() => Recycler?.Borrow<PQStorageCandle>().CopyFrom(this) as PQStorageCandle ?? new PQStorageCandle(this);

    ITransferState ITransferState.CopyFrom
        (ITransferState source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom((ICandle)source, copyMergeFlags);

    IMutableCandle IMutableCandle.Clone() => Recycler?.Borrow<PQStorageCandle>().CopyFrom(this) as IMutableCandle ?? new PQStorageCandle(this);

    IReusableObject<ICandle> ITransferState<IReusableObject<ICandle>>.CopyFrom
        (IReusableObject<ICandle> source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom((IMutableCandle)source, copyMergeFlags);

    public IPQStorageCandle CopyFrom
        (IPQStorageCandle ps, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default) =>
        (IPQStorageCandle)CopyFrom((ICandle)ps, copyMergeFlags);


    public override ICandle CopyFrom(ICandle ps, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (copyMergeFlags.HasFullReplace()) IsEmpty = true;
        TimeBoundaryPeriod = ps.TimeBoundaryPeriod;
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
        CandleFlags        = ps.CandleFlags | CandleFlags.FromStorage;
        AverageBidPrice    = ps.AverageBidAsk.BidPrice;
        AverageAskPrice    = ps.AverageBidAsk.AskPrice;

        if (copyMergeFlags.HasFullReplace())
            CandleStorageFlags |= Snapshot;
        else
            CandleStorageFlags &= SnapshotClearMask;

        return this;
    }

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
        var candleFlagsSame     = CandleFlags == (other.CandleFlags | CandleFlags.FromStorage);
        var averageBidSame      = AverageBidPrice == other.AverageBidAsk.BidPrice;
        var averageAskSame      = AverageAskPrice == other.AverageBidAsk.AskPrice;

        var updateFlagsSame = true;
        if (exactTypes)
        {
            var otherPQ = (PQStorageCandle)other;
            updateFlagsSame = CandleStorageFlags == otherPQ.CandleStorageFlags;
        }

        var allAreSame = timeFrameSame && startTimeSame && endTimeSame && startBidPriceSame && startAskPriceSame
                      && highestBidPriceSame && highestAskPriceSame && lowestBidPriceSame && lowestAskPriceSame
                      && endBidPriceSame && endAskPriceSame && tickCountSame && periodVolumeSame && candleFlagsSame
                      && updateFlagsSame && averageBidSame && averageAskSame;
        return allAreSame;
    }

    public void CalculateAllFromNewDeltas()
    {
        DeltaStartBidPrice         = DeltaStartAskPrice         = DeltaHighestBidPrice = DeltaHighestAskPrice = 0;
        DeltaLowestBidPrice        = DeltaLowestAskPrice        = DeltaEndBidPrice     = DeltaEndAskPrice     = 0;
        DeltaTickCount             = DeltaPeriodVolume          = DeltaAverageBidPrice = DeltaAverageAskPrice = 0;
        DeltaCandleFlagsLowerBytes = DeltaCandleFlagsUpperBytes = 0;

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
        if (DeltaStartAskPrice + DeltaStartBidPrice == 0) CandleStorageFlags |= PricesStartSameAsLastEndPrices;
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

    private uint GetPriceDecimalDeltaAndFlags(decimal newValue, decimal oldValue, PQStorageCandleFlags negateFlag, uint previousDelta)
    {
        var oldScaled = PQScaling.Scale(oldValue, PrecisionSettings!.PriceScalingPrecision);
        var newScaled = PQScaling.Scale(newValue, PrecisionSettings!.PriceScalingPrecision);
        var delta =
            CandleStorageFlags.SignMultiplier(negateFlag) * previousDelta + (int)newScaled - (int)oldScaled;
        if (delta < 0)
            CandleStorageFlags |= negateFlag;
        else
            CandleStorageFlags &= AllFlags & ~negateFlag;
        return (uint)Math.Abs(delta);
    }

    private uint GetVolumeLongDeltaAndFlags(long newValue, long oldValue, PQStorageCandleFlags negateFlag, uint previousDelta)
    {
        var oldScaled = PQScaling.Scale(oldValue, PrecisionSettings!.VolumeScalingPrecision);
        var newScaled = PQScaling.Scale(newValue, PrecisionSettings!.VolumeScalingPrecision);
        var delta =
            CandleStorageFlags.SignMultiplier(negateFlag) * previousDelta + (int)newScaled - (int)oldScaled;
        if (delta < 0)
            CandleStorageFlags |= negateFlag;
        else
            CandleStorageFlags &= AllFlags & ~negateFlag;
        return (uint)Math.Abs(delta);
    }

    private IPQPriceVolumePublicationPrecisionSettings GetProposedVolumeScaleFactor()
    {
        var priceScale = (PrecisionSettings?.PriceScalingPrecision ?? PQFieldFlags.DecimalScaleBits) & PQFieldFlags.DecimalScaleBits;
        if (PrecisionSettings == null || DeltaStartBidPrice + DeltaStartAskPrice + DeltaHighestBidPrice
          + DeltaHighestAskPrice + DeltaLowestBidPrice + DeltaLowestAskPrice + DeltaEndBidPrice + DeltaEndAskPrice <= 0)
        {
            priceScale = StartBidPrice > 0
                ? (PQFieldFlags)Math.Min((byte)priceScale, (byte)PQScaling.FindPriceScaleFactor(StartBidPrice))
                : priceScale;
            priceScale = StartAskPrice > 0
                ? (PQFieldFlags)Math.Min((byte)priceScale, (byte)PQScaling.FindPriceScaleFactor(StartAskPrice))
                : priceScale;
            priceScale = HighestBidPrice > 0
                ? (PQFieldFlags)Math.Min((byte)priceScale, (byte)PQScaling.FindPriceScaleFactor(HighestBidPrice))
                : priceScale;
            priceScale = HighestAskPrice > 0
                ? (PQFieldFlags)Math.Min((byte)priceScale, (byte)PQScaling.FindPriceScaleFactor(HighestAskPrice))
                : priceScale;
            priceScale = LowestBidPrice > 0
                ? (PQFieldFlags)Math.Min((byte)priceScale, (byte)PQScaling.FindPriceScaleFactor(LowestBidPrice))
                : priceScale;
            priceScale = LowestAskPrice > 0
                ? (PQFieldFlags)Math.Min((byte)priceScale, (byte)PQScaling.FindPriceScaleFactor(LowestAskPrice))
                : priceScale;
            priceScale = EndBidPrice > 0 ? (PQFieldFlags)Math.Min((byte)priceScale, (byte)PQScaling.FindPriceScaleFactor(EndBidPrice)) : priceScale;
            priceScale = EndAskPrice > 0 ? (PQFieldFlags)Math.Min((byte)priceScale, (byte)PQScaling.FindPriceScaleFactor(EndAskPrice)) : priceScale;
            priceScale = previousHighestBidPrice > 0
                ? (PQFieldFlags)Math.Min((byte)priceScale, (byte)PQScaling.FindPriceScaleFactor(previousHighestBidPrice))
                : priceScale;
            priceScale = previousHighestAskPrice > 0
                ? (PQFieldFlags)Math.Min((byte)priceScale, (byte)PQScaling.FindPriceScaleFactor(previousHighestAskPrice))
                : priceScale;
            priceScale = previousLowestBidPrice > 0
                ? (PQFieldFlags)Math.Min((byte)priceScale, (byte)PQScaling.FindPriceScaleFactor(previousLowestBidPrice))
                : priceScale;
            priceScale = previousLowestAskPrice > 0
                ? (PQFieldFlags)Math.Min((byte)priceScale, (byte)PQScaling.FindPriceScaleFactor(previousLowestAskPrice))
                : priceScale;
            priceScale = previousEndBidPrice > 0
                ? (PQFieldFlags)Math.Min((byte)priceScale, (byte)PQScaling.FindPriceScaleFactor(previousEndBidPrice))
                : priceScale;
            priceScale = previousEndAskPrice > 0
                ? (PQFieldFlags)Math.Min((byte)priceScale, (byte)PQScaling.FindPriceScaleFactor(previousEndAskPrice))
                : priceScale;
            // don't use AverageBidAsk as it may have many fractional decimals places
        }

        var volumeScale = (PrecisionSettings?.VolumeScalingPrecision ?? PQFieldFlags.DecimalScaleBits) & PQFieldFlags.DecimalScaleBits;

        if (PrecisionSettings == null || DeltaPeriodVolume <= 0)
        {
            volumeScale = previousPeriodVolume > 0
                ? (PQFieldFlags)Math.Min((byte)volumeScale, (byte)PQScaling.FindVolumeScaleFactor(previousPeriodVolume))
                : volumeScale;
            volumeScale = periodVolume > 0
                ? (PQFieldFlags)Math.Min((byte)volumeScale, (byte)PQScaling.FindVolumeScaleFactor(periodVolume))
                : volumeScale;
        }

        if (PrecisionSettings == null || (PrecisionSettings.PriceScalingPrecision & PQFieldFlags.DecimalScaleBits) != priceScale
                                      || (PrecisionSettings.VolumeScalingPrecision & PQFieldFlags.DecimalScaleBits) != volumeScale)
            PrecisionSettings = new PQPriceVolumePublicationPrecisionSettings(priceScale, volumeScale);

        CandleStorageFlags &= AllFlags & ~PriceVolumeScaleMask;
        CandleStorageFlags |= (PQStorageCandleFlags)((uint)VolumePricePrecisionScale << 24);
        return PrecisionSettings;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent((ICandle?)obj, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = TimeBoundaryPeriod.GetHashCode();
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
            hashCode = (hashCode * 397) ^ CandleFlags.GetHashCode();
            hashCode = (hashCode * 397) ^ averageBidPrice.GetHashCode();
            hashCode = (hashCode * 397) ^ averageAskPrice.GetHashCode();
            return hashCode;
        }
    }

    public override string ToString() =>
        $"{nameof(PQStorageCandle)} {{ {nameof(TimeBoundaryPeriod)}: {TimeBoundaryPeriod}, {nameof(PeriodStartTime)}: {PeriodStartTime}, " +
        $"{nameof(PeriodEndTime)}: {PeriodEndTime}, {nameof(StartBidPrice)}: {StartBidPrice}, {nameof(StartAskPrice)}:" +
        $" {StartAskPrice}, {nameof(HighestBidPrice)}: {HighestBidPrice}, {nameof(HighestAskPrice)}: " +
        $"{HighestAskPrice}, {nameof(LowestBidPrice)}: {LowestBidPrice}, {nameof(LowestAskPrice)}: " +
        $"{LowestAskPrice}, {nameof(EndBidPrice)}: {EndBidPrice}, {nameof(EndAskPrice)}: {EndAskPrice}, " +
        $"{nameof(TickCount)}: {TickCount}, {nameof(PeriodVolume)}: {PeriodVolume}, {nameof(CandleFlags)}: {CandleFlags}, " +
        $"{nameof(AverageBidPrice)}: {AverageBidPrice}, {nameof(AverageAskPrice)}: {AverageAskPrice} }}";
}
