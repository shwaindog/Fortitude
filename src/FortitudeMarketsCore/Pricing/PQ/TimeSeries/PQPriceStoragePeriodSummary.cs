// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.Types;
using FortitudeIO.TimeSeries;
using FortitudeMarketsApi.Pricing.TimeSeries;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using static FortitudeMarketsCore.Pricing.PQ.TimeSeries.PQPriceStorageSummaryFlags;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.TimeSeries;

[Flags]
public enum PQPriceStorageSummaryFlags : uint
{
    None                           = 0x00_00
  , IsNextTimePeriod               = 0x00_01
  , Snapshot                       = 0x00_02
  , PricesStartSameAsLastEndPrices = 0x00_04
  , NegateDeltaStartBidPrice       = 0x00_08
  , NegateDeltaStartAskPrice       = 0x00_10
  , NegateDeltaHighestBidPrice     = 0x00_20
  , NegateDeltaHighestAskPrice     = 0x00_40
  , NegateDeltaLowestBidPrice      = 0x00_80
  , NegateDeltaLowestAskPrice      = 0x01_00
  , NegateDeltaEndBidPrice         = 0x02_00
  , NegateDeltaEndAskPrice         = 0x04_00
  , TickCountSameAsPrevious        = 0x08_00
  , NegateDeltaTickCount           = 0x10_00
  , NegateDeltaPeriodVolume        = 0x20_00
  , NegateDeltaAverageBidPrice     = 0x40_00
  , NegateDeltaAverageAskPrice     = 0x80_00
  , PriceScaleMask                 = 0x0F_00_00
  , VolumeScaleMask                = 0xF0_00_00
  , SnapshotClearMask              = 0x00_FF_FD
  , AllFlags                       = 0xFF_FF_FF
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

    IPQPriceVolumePublicationPrecisionSettings? PrecisionSettings { get; set; }

    uint DeltaPeriodsFromPrevious  { get; set; }
    uint DeltaStartBidPrice        { get; set; }
    uint DeltaStartAskPrice        { get; set; }
    uint DeltaHighestBidPrice      { get; set; }
    uint DeltaHighestAskPrice      { get; set; }
    uint DeltaLowestBidPrice       { get; set; }
    uint DeltaLowestAskPrice       { get; set; }
    uint DeltaEndBidPrice          { get; set; }
    uint DeltaEndAskPrice          { get; set; }
    uint DeltaTickCount            { get; set; }
    uint DeltaPeriodVolume         { get; set; }
    uint DeltaAverageBidPrice      { get; set; }
    uint DeltaAverageAskPrice      { get; set; }
    byte VolumePricePrecisionScale { get; }
}

public class PQPriceStoragePeriodSummary : IPQPriceStoragePeriodSummary
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
    private long    periodVolume;

    private IPQPriceVolumePublicationPrecisionSettings? precisionSettings = null!;

    private decimal  startAskPrice;
    private decimal  startBidPrice;
    private DateTime startTime = DateTimeConstants.UnixEpoch;
    private uint     tickCount;

    public PQPriceStoragePeriodSummary() { }

    public PQPriceStoragePeriodSummary(IPQPriceVolumePublicationPrecisionSettings precisionSettings)
    {
        PrecisionSettings   =  precisionSettings;
        SummaryStorageFlags |= Snapshot;
    }

    public PQPriceStoragePeriodSummary(IPQPriceStoragePeriodSummary toClone)
    {
        precisionSettings = toClone.PrecisionSettings;
        SummaryPeriod     = toClone.SummaryPeriod;
        SummaryStartTime  = toClone.SummaryStartTime;
        SummaryEndTime    = toClone.SummaryEndTime;
        StartBidPrice     = toClone.StartBidPrice;
        StartAskPrice     = toClone.StartAskPrice;
        HighestBidPrice   = toClone.HighestBidPrice;
        HighestAskPrice   = toClone.HighestAskPrice;
        LowestBidPrice    = toClone.LowestBidPrice;
        LowestAskPrice    = toClone.LowestAskPrice;
        EndBidPrice       = toClone.EndBidPrice;
        EndAskPrice       = toClone.EndAskPrice;
        TickCount         = toClone.TickCount;
        PeriodVolume      = toClone.PeriodVolume;
        AverageBidPrice   = toClone.AverageBidPrice;
        AverageAskPrice   = toClone.AverageBidPrice;
    }

    public PQPriceStorageSummaryFlags SummaryStorageFlags { get; private set; }


    public uint DeltaPeriodsFromPrevious
    {
        get => deltaPeriodsFromPrevious;
        set
        {
            if (value == 1)
                SummaryStorageFlags |= IsNextTimePeriod;
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

    public IPQPriceVolumePublicationPrecisionSettings? PrecisionSettings
    {
        get => precisionSettings;
        set
        {
            precisionSettings   =  value;
            SummaryStorageFlags &= AllFlags & ~PriceScaleMask & ~VolumeScaleMask;
            SummaryStorageFlags |= (PQPriceStorageSummaryFlags)((uint)VolumePricePrecisionScale << 16);
        }
    }

    public byte VolumePricePrecisionScale =>
        PrecisionSettings != null
            ? (byte)(((PrecisionSettings.VolumeScalingPrecision & 0xF) << 4)
                   | (PrecisionSettings.PriceScalingPrecision & 0xF))
            : (byte)0x88;

    public uint DeltaTickCount       { get; set; }
    public uint DeltaPeriodVolume    { get; set; }
    public uint DeltaAverageBidPrice { get; set; }
    public uint DeltaAverageAskPrice { get; set; }

    public TimeSeriesPeriod SummaryPeriod { get; set; }

    public bool IsEmpty
    {
        get
        {
            var pricesAreAllZero =
                StartBidPrice == decimal.Zero && StartAskPrice == decimal.Zero && HighestBidPrice == decimal.Zero &&
                HighestAskPrice == decimal.Zero && LowestBidPrice == decimal.Zero && LowestAskPrice == decimal.Zero &&
                EndBidPrice == decimal.Zero && EndAskPrice == decimal.Zero &&
                AverageBidPrice == decimal.Zero && AverageAskPrice == decimal.Zero;
            var tickCountAndVolumeZero = TickCount == 0 && PeriodVolume == 0;
            var summaryPeriodNone      = SummaryPeriod == TimeSeriesPeriod.None;
            var startEndTimeUnixEpoch = SummaryStartTime == DateTimeConstants.UnixEpoch
                                     && SummaryEndTime == DateTimeConstants.UnixEpoch;
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

    public DateTime SummaryStartTime
    {
        get => startTime;
        set
        {
            if (SummaryEndTime > DateTimeConstants.UnixEpoch)
            {
                var  currentStartTime   = SummaryEndTime;
                var  haveFoundStartTime = currentStartTime == value;
                uint countPeriod        = 1;
                while (!haveFoundStartTime)
                {
                    currentStartTime = SummaryPeriod.PeriodEnd(currentStartTime);
                    countPeriod++;
                    haveFoundStartTime = currentStartTime == value;
                }
                DeltaPeriodsFromPrevious = countPeriod;
            }
            startTime      = value;
            SummaryEndTime = SummaryPeriod.PeriodEnd(startTime);
        }
    }

    public DateTime SummaryEndTime { get; set; } = DateTimeConstants.UnixEpoch;

    public decimal AverageBidPrice
    {
        get => averageBidPrice;
        set
        {
            var current  = PQScaling.Scale(averageBidPrice, (byte)(PrecisionSettings.PriceScalingPrecision & 0x1F));
            var newValue = PQScaling.Scale(value, (byte)(PrecisionSettings.PriceScalingPrecision & 0x1F));
            var delta =
                SummaryStorageFlags.SignMultiplier(NegateDeltaAverageBidPrice) * DeltaAverageBidPrice
              + (int)newValue - (int)current;
            if (delta < 0)
                SummaryStorageFlags |= NegateDeltaAverageBidPrice;
            else
                SummaryStorageFlags &= AllFlags & ~NegateDeltaAverageBidPrice;
            DeltaAverageBidPrice = (uint)Math.Abs(delta);
            averageBidPrice      = value;
        }
    }
    public decimal AverageAskPrice
    {
        get => averageAskPrice;
        set
        {
            var current  = PQScaling.Scale(averageAskPrice, (byte)(PrecisionSettings.PriceScalingPrecision & 0x1F));
            var newValue = PQScaling.Scale(value, (byte)(PrecisionSettings.PriceScalingPrecision & 0x1F));
            var delta =
                SummaryStorageFlags.SignMultiplier(NegateDeltaAverageAskPrice) * DeltaAverageAskPrice
              + (int)newValue - (int)current;
            if (delta < 0)
                SummaryStorageFlags |= NegateDeltaAverageAskPrice;
            else
                SummaryStorageFlags &= AllFlags & ~NegateDeltaAverageAskPrice;
            DeltaAverageAskPrice = (uint)Math.Abs(delta);
            averageAskPrice      = value;
        }
    }

    public decimal StartBidPrice
    {
        get => startBidPrice;
        set
        {
            var current  = PQScaling.Scale(endBidPrice, (byte)(PrecisionSettings.PriceScalingPrecision & 0x1F));
            var newValue = PQScaling.Scale(value, (byte)(PrecisionSettings.PriceScalingPrecision & 0x1F));
            var delta =
                SummaryStorageFlags.SignMultiplier(NegateDeltaStartBidPrice) * DeltaStartBidPrice
              + (int)newValue - (int)current;
            if (delta < 0)
            {
                SummaryStorageFlags &= AllFlags & ~PricesStartSameAsLastEndPrices;
                SummaryStorageFlags |= NegateDeltaStartAskPrice;
            }
            else if (delta > 0)
            {
                SummaryStorageFlags &= AllFlags & ~NegateDeltaStartAskPrice & ~PricesStartSameAsLastEndPrices;
            }
            else
            {
                SummaryStorageFlags |= PricesStartSameAsLastEndPrices;
            }
            DeltaStartBidPrice = (uint)Math.Abs(delta);
            startBidPrice      = value;
        }
    }

    public decimal StartAskPrice
    {
        get => startAskPrice;
        set
        {
            var current  = PQScaling.Scale(endAskPrice, (byte)(PrecisionSettings.PriceScalingPrecision & 0x1F));
            var newValue = PQScaling.Scale(value, (byte)(PrecisionSettings.PriceScalingPrecision & 0x1F));
            var delta =
                SummaryStorageFlags.SignMultiplier(NegateDeltaStartAskPrice) * DeltaStartAskPrice
              + (int)newValue - (int)current;
            if (delta < 0)
            {
                SummaryStorageFlags &= AllFlags & ~PricesStartSameAsLastEndPrices;
                SummaryStorageFlags |= NegateDeltaStartAskPrice;
            }
            else if (delta > 0)
            {
                SummaryStorageFlags &= AllFlags & ~NegateDeltaStartAskPrice & ~PricesStartSameAsLastEndPrices;
            }
            else
            {
                SummaryStorageFlags |= PricesStartSameAsLastEndPrices;
            }
            DeltaStartAskPrice = (uint)Math.Abs(delta);
            startAskPrice      = value;
        }
    }

    public decimal HighestBidPrice
    {
        get => highestBidPrice;
        set
        {
            var current  = PQScaling.Scale(highestBidPrice, (byte)(PrecisionSettings.PriceScalingPrecision & 0x1F));
            var newValue = PQScaling.Scale(value, (byte)(PrecisionSettings.PriceScalingPrecision & 0x1F));
            var delta =
                SummaryStorageFlags.SignMultiplier(NegateDeltaHighestBidPrice) * DeltaHighestBidPrice
              + (int)newValue - (int)current;
            if (delta < 0)
                SummaryStorageFlags |= NegateDeltaHighestBidPrice;
            else
                SummaryStorageFlags &= AllFlags & ~NegateDeltaHighestBidPrice;
            DeltaHighestBidPrice = (uint)Math.Abs(delta);
            highestBidPrice      = value;
        }
    }

    public decimal HighestAskPrice
    {
        get => highestAskPrice;
        set
        {
            var current  = PQScaling.Scale(highestAskPrice, (byte)(PrecisionSettings.PriceScalingPrecision & 0x1F));
            var newValue = PQScaling.Scale(value, (byte)(PrecisionSettings.PriceScalingPrecision & 0x1F));
            var delta =
                SummaryStorageFlags.SignMultiplier(NegateDeltaHighestAskPrice) * DeltaHighestAskPrice
              + (int)newValue - (int)current;
            if (delta < 0)
                SummaryStorageFlags |= NegateDeltaHighestAskPrice;
            else
                SummaryStorageFlags &= AllFlags & ~NegateDeltaHighestAskPrice;
            DeltaHighestAskPrice = (uint)Math.Abs(delta);
            highestAskPrice      = value;
        }
    }

    public decimal LowestBidPrice
    {
        get => lowestBidPrice;
        set
        {
            var current  = PQScaling.Scale(lowestBidPrice, (byte)(PrecisionSettings.PriceScalingPrecision & 0x1F));
            var newValue = PQScaling.Scale(value, (byte)(PrecisionSettings.PriceScalingPrecision & 0x1F));
            var delta =
                SummaryStorageFlags.SignMultiplier(NegateDeltaLowestBidPrice) * DeltaLowestBidPrice
              + (int)newValue - (int)current;
            if (delta < 0)
                SummaryStorageFlags |= NegateDeltaLowestBidPrice;
            else
                SummaryStorageFlags &= AllFlags & ~NegateDeltaLowestBidPrice;
            DeltaLowestBidPrice = (uint)Math.Abs(delta);
            lowestBidPrice      = value;
        }
    }

    public decimal LowestAskPrice
    {
        get => lowestAskPrice;
        set
        {
            var current  = PQScaling.Scale(lowestAskPrice, (byte)(PrecisionSettings.PriceScalingPrecision & 0x1F));
            var newValue = PQScaling.Scale(value, (byte)(PrecisionSettings.PriceScalingPrecision & 0x1F));
            var delta =
                SummaryStorageFlags.SignMultiplier(NegateDeltaLowestAskPrice) * DeltaLowestAskPrice
              + (int)newValue - (int)current;
            if (delta < 0)
                SummaryStorageFlags |= NegateDeltaLowestAskPrice;
            else
                SummaryStorageFlags &= AllFlags & ~NegateDeltaLowestAskPrice;
            DeltaLowestBidPrice = (uint)Math.Abs(delta);
            lowestAskPrice      = value;
        }
    }

    public decimal EndBidPrice
    {
        get => endBidPrice;
        set
        {
            var current  = PQScaling.Scale(endBidPrice, (byte)(PrecisionSettings.PriceScalingPrecision & 0x1F));
            var newValue = PQScaling.Scale(value, (byte)(PrecisionSettings.PriceScalingPrecision & 0x1F));
            var delta =
                SummaryStorageFlags.SignMultiplier(NegateDeltaEndBidPrice) * DeltaEndBidPrice
              + (int)newValue - (int)current;
            if (delta < 0)
                SummaryStorageFlags |= NegateDeltaEndBidPrice;
            else
                SummaryStorageFlags &= AllFlags & ~NegateDeltaEndBidPrice;
            DeltaEndBidPrice = (uint)Math.Abs(delta);
            endBidPrice      = value;
        }
    }

    public decimal EndAskPrice
    {
        get => endAskPrice;
        set
        {
            var current  = PQScaling.Scale(endAskPrice, (byte)(PrecisionSettings.PriceScalingPrecision & 0x1F));
            var newValue = PQScaling.Scale(value, (byte)(PrecisionSettings.PriceScalingPrecision & 0x1F));
            var delta =
                SummaryStorageFlags.SignMultiplier(NegateDeltaEndAskPrice) * DeltaEndAskPrice
              + (int)newValue - (int)current;
            if (delta < 0)
                SummaryStorageFlags |= NegateDeltaEndAskPrice;
            else
                SummaryStorageFlags &= AllFlags & ~NegateDeltaEndAskPrice;
            DeltaEndAskPrice = (uint)Math.Abs(delta);
            endAskPrice      = value;
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
            var current  = PQScaling.Scale(periodVolume, (byte)(PrecisionSettings.VolumeScalingPrecision & 0x1F));
            var newValue = PQScaling.Scale(value, (byte)(PrecisionSettings.VolumeScalingPrecision & 0x1F));
            var delta =
                SummaryStorageFlags.SignMultiplier(NegateDeltaPeriodVolume) * DeltaPeriodVolume
              + (int)newValue - (int)current;
            if (delta < 0)
                SummaryStorageFlags |= NegateDeltaPeriodVolume;
            else
                SummaryStorageFlags &= AllFlags & ~NegateDeltaPeriodVolume;
            DeltaPeriodVolume = (uint)Math.Abs(delta);
            periodVolume      = value;
        }
    }

    public bool HasUpdates
    {
        get => SummaryStorageFlags > 0;
        set
        {
            if (!value) return;
            SummaryStorageFlags = None;
            SummaryStorageFlags = (PQPriceStorageSummaryFlags)((uint)VolumePricePrecisionScale << 16);

            DeltaStartBidPrice  = DeltaStartAskPrice  = DeltaHighestBidPrice = DeltaHighestAskPrice = 0;
            DeltaLowestBidPrice = DeltaLowestAskPrice = DeltaEndBidPrice     = DeltaEndAskPrice     = 0;
            DeltaTickCount      = DeltaPeriodVolume   = DeltaAverageBidPrice = DeltaAverageAskPrice = 0;
        }
    }


    public IPQPriceStoragePeriodSummary CopyFrom
        (IPQPriceStoragePeriodSummary ps, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        SummaryPeriod    = ps.SummaryPeriod;
        SummaryStartTime = ps.SummaryStartTime;
        StartBidPrice    = ps.StartBidPrice;
        StartAskPrice    = ps.StartAskPrice;
        HighestBidPrice  = ps.HighestBidPrice;
        HighestAskPrice  = ps.HighestAskPrice;
        LowestBidPrice   = ps.LowestBidPrice;
        LowestAskPrice   = ps.LowestAskPrice;
        EndBidPrice      = ps.EndBidPrice;
        EndAskPrice      = ps.EndAskPrice;
        TickCount        = ps.TickCount;
        PeriodVolume     = ps.PeriodVolume;
        AverageBidPrice  = ps.AverageBidPrice;
        AverageAskPrice  = ps.AverageAskPrice;

        return this;
    }

    public IPricePeriodSummary CopyFrom(IPricePeriodSummary ps, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        SummaryPeriod    = ps.SummaryPeriod;
        SummaryStartTime = ps.SummaryStartTime;
        StartBidPrice    = ps.StartBidPrice;
        StartAskPrice    = ps.StartAskPrice;
        HighestBidPrice  = ps.HighestBidPrice;
        HighestAskPrice  = ps.HighestAskPrice;
        LowestBidPrice   = ps.LowestBidPrice;
        LowestAskPrice   = ps.LowestAskPrice;
        EndBidPrice      = ps.EndBidPrice;
        EndAskPrice      = ps.EndAskPrice;
        TickCount        = ps.TickCount;
        PeriodVolume     = ps.PeriodVolume;
        AverageBidPrice  = ps.AverageBidPrice;
        AverageAskPrice  = ps.AverageAskPrice;

        return this;
    }

    public IStoreState CopyFrom(IStoreState source, CopyMergeFlags copyMergeFlags) => CopyFrom((IPricePeriodSummary)source, copyMergeFlags);

    public IMutablePricePeriodSummary Clone() => new PQPricePeriodSummary(this);

    object ICloneable.Clone() => Clone();

    IPricePeriodSummary ICloneable<IPricePeriodSummary>.Clone() => Clone();

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
        var averageBidSame      = AverageBidPrice == other.AverageBidPrice;
        var averageAskSame      = AverageAskPrice == other.AverageAskPrice;

        var updateFlagsSame = true;
        if (exactTypes)
        {
            var otherPQ = (PQPriceStoragePeriodSummary)other;
            updateFlagsSame = SummaryStorageFlags == otherPQ.SummaryStorageFlags;
        }

        var allAreSame = timeFrameSame && startTimeSame && endTimeSame && startBidPriceSame && startAskPriceSame
                      && highestBidPriceSame && highestAskPriceSame && lowestBidPriceSame && lowestAskPriceSame
                      && endBidPriceSame && endAskPriceSame && tickCountSame && periodVolumeSame && updateFlagsSame
                      && averageBidSame && averageAskSame;
        return allAreSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent((IPricePeriodSummary?)obj, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = SummaryPeriod.GetHashCode();
            hashCode = (hashCode * 397) ^ startTime.GetHashCode();
            hashCode = (hashCode * 397) ^ SummaryEndTime.GetHashCode();
            hashCode = (hashCode * 397) ^ startBidPrice.GetHashCode();
            hashCode = (hashCode * 397) ^ startAskPrice.GetHashCode();
            hashCode = (hashCode * 397) ^ highestBidPrice.GetHashCode();
            hashCode = (hashCode * 397) ^ highestAskPrice.GetHashCode();
            hashCode = (hashCode * 397) ^ lowestBidPrice.GetHashCode();
            hashCode = (hashCode * 397) ^ lowestAskPrice.GetHashCode();
            hashCode = (hashCode * 397) ^ endBidPrice.GetHashCode();
            hashCode = (hashCode * 397) ^ endAskPrice.GetHashCode();
            hashCode = (hashCode * 397) ^ (int)tickCount;
            hashCode = (hashCode * 397) ^ periodVolume.GetHashCode();
            hashCode = (hashCode * 397) ^ averageBidPrice.GetHashCode();
            hashCode = (hashCode * 397) ^ averageAskPrice.GetHashCode();
            return hashCode;
        }
    }

    public override string ToString() =>
        $"PQPriceStoragePeriodSummary {{ {nameof(SummaryPeriod)}: {SummaryPeriod}, {nameof(SummaryStartTime)}: {SummaryStartTime}, " +
        $"{nameof(SummaryEndTime)}: {SummaryEndTime}, {nameof(StartBidPrice)}: {StartBidPrice}, {nameof(StartAskPrice)}:" +
        $" {StartAskPrice}, {nameof(HighestBidPrice)}: {HighestBidPrice}, {nameof(HighestAskPrice)}: " +
        $"{HighestAskPrice}, {nameof(LowestBidPrice)}: {LowestBidPrice}, {nameof(LowestAskPrice)}: " +
        $"{LowestAskPrice}, {nameof(EndBidPrice)}: {EndBidPrice}, {nameof(EndAskPrice)}: {EndAskPrice}, " +
        $"{nameof(TickCount)}: {TickCount}, {nameof(PeriodVolume)}: {PeriodVolume}, {nameof(AverageBidPrice)}: {AverageBidPrice}, " +
        $"{nameof(AverageAskPrice)}: {AverageAskPrice} }}";
}
