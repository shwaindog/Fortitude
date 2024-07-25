// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types;
using FortitudeIO.TimeSeries;
using FortitudeMarketsApi.Pricing;
using FortitudeMarketsApi.Pricing.Summaries;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.Serdes.Serialization;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Summaries;

public interface IPQPricePeriodSummary : IMutablePricePeriodSummary, IPQSupportsFieldUpdates<IPricePeriodSummary>
  , IDoublyLinkedListNode<IPQPricePeriodSummary>
{
    bool IsSummaryPeriodUpdated    { get; set; }
    bool IsStartTimeDateUpdated    { get; set; }
    bool IsStartTimeSubHourUpdated { get; set; }
    bool IsEndTimeDateUpdated      { get; set; }
    bool IsEndTimeSubHourUpdated   { get; set; }
    bool IsStartBidPriceUpdated    { get; set; }
    bool IsStartAskPriceUpdated    { get; set; }
    bool IsHighestBidPriceUpdated  { get; set; }
    bool IsHighestAskPriceUpdated  { get; set; }
    bool IsLowestBidPriceUpdated   { get; set; }
    bool IsLowestAskPriceUpdated   { get; set; }
    bool IsEndBidPriceUpdated      { get; set; }
    bool IsEndAskPriceUpdated      { get; set; }
    bool IsTickCountUpdated        { get; set; }

    bool IsPeriodVolumeUpdated            { get; set; }
    bool IsPricePeriodSummaryFlagsUpdated { get; set; }
    bool IsAverageBidPriceUpdated         { get; set; }
    bool IsAverageAskPriceUpdated         { get; set; }

    new IPQPricePeriodSummary? Previous { get; set; }
    new IPQPricePeriodSummary? Next     { get; set; }

    new IPQPricePeriodSummary Clone();
}

public class PQPricePeriodSummary : ReusableObject<IPricePeriodSummary>, IPQPricePeriodSummary, ITimeSeriesEntry<PQPricePeriodSummary>
  , ICloneable<PQPricePeriodSummary>
{
    private decimal  averageAskPrice;
    private decimal  averageBidPrice;
    private decimal  endAskPrice;
    private decimal  endBidPrice;
    private DateTime endTime = DateTimeConstants.UnixEpoch;
    private decimal  highestAskPrice;
    private decimal  highestBidPrice;
    private decimal  lowestAskPrice;
    private decimal  lowestBidPrice;

    private PricePeriodSummaryFlags periodSummaryFlags;

    private long     periodVolume;
    private decimal  startAskPrice;
    private decimal  startBidPrice;
    private DateTime startTime = DateTimeConstants.UnixEpoch;
    private uint     tickCount;

    private TimeSeriesPeriod          timeSeriesPeriod;
    private PeriodSummaryUpdatedFlags updatedFlags;

    public PQPricePeriodSummary() { }

    public PQPricePeriodSummary(IPricePeriodSummary toClone)
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

    public override PQPricePeriodSummary Clone() =>
        Recycler?.Borrow<PQPricePeriodSummary>().CopyFrom(this) as PQPricePeriodSummary ?? new PQPricePeriodSummary(this);

    public TimeSeriesPeriod TimeSeriesPeriod
    {
        get => timeSeriesPeriod;
        set
        {
            if (value == timeSeriesPeriod) return;
            IsSummaryPeriodUpdated = true;
            timeSeriesPeriod       = value;
        }
    }

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

    public DateTime PeriodStartTime
    {
        get => startTime;
        set
        {
            if (startTime == value) return;
            IsStartTimeDateUpdated    |= startTime.GetHoursFromUnixEpoch() != value.GetHoursFromUnixEpoch();
            IsStartTimeSubHourUpdated |= startTime.GetSubHourComponent() != value.GetSubHourComponent();
            startTime                 =  value;
        }
    }

    public BoundedTimeRange ToBoundedTimeRange(DateTime? maxDateTime = null) => new(PeriodStartTime, PeriodEndTime.Min(maxDateTime));

    public bool IsWhollyBoundedBy
        (ITimeSeriesPeriodRange parentRange) =>
        parentRange.PeriodStartTime <= PeriodStartTime && parentRange.PeriodEnd() >= PeriodEndTime;

    public PricePeriodSummaryFlags PeriodSummaryFlags
    {
        get => periodSummaryFlags;
        set
        {
            if (value == periodSummaryFlags) return;

            IsPricePeriodSummaryFlagsUpdated = true;

            periodSummaryFlags = value;
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

    public bool IsSummaryPeriodUpdated
    {
        get => (updatedFlags & PeriodSummaryUpdatedFlags.SummaryPeriodFlag) > 0;
        set
        {
            if (value)
                updatedFlags |= PeriodSummaryUpdatedFlags.SummaryPeriodFlag;

            else if (IsSummaryPeriodUpdated) updatedFlags ^= PeriodSummaryUpdatedFlags.SummaryPeriodFlag;
        }
    }

    public bool IsStartTimeDateUpdated
    {
        get => (updatedFlags & PeriodSummaryUpdatedFlags.StartTimeDateFlag) > 0;
        set
        {
            if (value)
                updatedFlags |= PeriodSummaryUpdatedFlags.StartTimeDateFlag;

            else if (IsStartTimeDateUpdated) updatedFlags ^= PeriodSummaryUpdatedFlags.StartTimeDateFlag;
        }
    }

    public bool IsStartTimeSubHourUpdated
    {
        get => (updatedFlags & PeriodSummaryUpdatedFlags.StartTimeSubHourFlag) > 0;
        set
        {
            if (value)
                updatedFlags |= PeriodSummaryUpdatedFlags.StartTimeSubHourFlag;

            else if (IsStartTimeSubHourUpdated) updatedFlags ^= PeriodSummaryUpdatedFlags.StartTimeSubHourFlag;
        }
    }

    public DateTime PeriodEndTime
    {
        get => endTime;
        set
        {
            if (endTime == value) return;
            IsEndTimeDateUpdated    |= endTime.GetHoursFromUnixEpoch() != value.GetHoursFromUnixEpoch();
            IsEndTimeSubHourUpdated |= endTime.GetSubHourComponent() != value.GetSubHourComponent();
            endTime                 =  value;
        }
    }

    DateTime ITimeSeriesEntry<IPricePeriodSummary>.StorageTime(IStorageTimeResolver<IPricePeriodSummary>? resolver) => PeriodEndTime;

    public bool IsEndTimeDateUpdated
    {
        get => (updatedFlags & PeriodSummaryUpdatedFlags.EndTimeDateFlag) > 0;
        set
        {
            if (value)
                updatedFlags |= PeriodSummaryUpdatedFlags.EndTimeDateFlag;

            else if (IsEndTimeDateUpdated) updatedFlags ^= PeriodSummaryUpdatedFlags.EndTimeDateFlag;
        }
    }

    public bool IsEndTimeSubHourUpdated
    {
        get => (updatedFlags & PeriodSummaryUpdatedFlags.EndTimeSubHourFlag) > 0;
        set
        {
            if (value)
                updatedFlags |= PeriodSummaryUpdatedFlags.EndTimeSubHourFlag;

            else if (IsEndTimeSubHourUpdated) updatedFlags ^= PeriodSummaryUpdatedFlags.EndTimeSubHourFlag;
        }
    }
    public BidAskPair StartBidAsk   => new(StartBidPrice, StartAskPrice);
    public BidAskPair HighestBidAsk => new(HighestBidPrice, HighestAskPrice);
    public BidAskPair LowestBidAsk  => new(LowestBidPrice, LowestAskPrice);
    public BidAskPair EndBidAsk     => new(EndBidPrice, EndAskPrice);
    public BidAskPair AverageBidAsk => new(AverageBidPrice, AverageAskPrice);

    public decimal AverageBidPrice
    {
        get => averageBidPrice;
        set
        {
            if (averageBidPrice == value) return;

            IsAverageBidPriceUpdated = true;
            averageBidPrice          = value;
        }
    }
    public decimal AverageAskPrice
    {
        get => averageAskPrice;
        set
        {
            if (averageAskPrice == value) return;

            IsAverageAskPriceUpdated = true;

            averageAskPrice = value;
        }
    }

    public decimal StartBidPrice
    {
        get => startBidPrice;
        set
        {
            if (startBidPrice == value) return;
            IsStartBidPriceUpdated = true;
            startBidPrice          = value;
        }
    }

    public bool IsStartBidPriceUpdated
    {
        get => (updatedFlags & PeriodSummaryUpdatedFlags.StartBidPriceFlag) > 0;
        set
        {
            if (value)
                updatedFlags |= PeriodSummaryUpdatedFlags.StartBidPriceFlag;

            else if (IsStartBidPriceUpdated) updatedFlags ^= PeriodSummaryUpdatedFlags.StartBidPriceFlag;
        }
    }

    public decimal StartAskPrice
    {
        get => startAskPrice;
        set
        {
            if (startAskPrice == value) return;
            IsStartAskPriceUpdated = true;
            startAskPrice          = value;
        }
    }

    public bool IsStartAskPriceUpdated
    {
        get => (updatedFlags & PeriodSummaryUpdatedFlags.StartAskPriceFlag) > 0;
        set
        {
            if (value)
                updatedFlags |= PeriodSummaryUpdatedFlags.StartAskPriceFlag;

            else if (IsStartAskPriceUpdated) updatedFlags ^= PeriodSummaryUpdatedFlags.StartAskPriceFlag;
        }
    }

    public decimal HighestBidPrice
    {
        get => highestBidPrice;
        set
        {
            if (highestBidPrice == value) return;
            IsHighestBidPriceUpdated = true;
            highestBidPrice          = value;
        }
    }

    public bool IsHighestBidPriceUpdated
    {
        get => (updatedFlags & PeriodSummaryUpdatedFlags.HighestBidPriceFlag) > 0;
        set
        {
            if (value)
                updatedFlags |= PeriodSummaryUpdatedFlags.HighestBidPriceFlag;

            else if (IsHighestBidPriceUpdated) updatedFlags ^= PeriodSummaryUpdatedFlags.HighestBidPriceFlag;
        }
    }

    public decimal HighestAskPrice
    {
        get => highestAskPrice;
        set
        {
            if (highestAskPrice == value) return;
            IsHighestAskPriceUpdated = true;
            highestAskPrice          = value;
        }
    }

    public bool IsHighestAskPriceUpdated
    {
        get => (updatedFlags & PeriodSummaryUpdatedFlags.HighestAskPriceFlag) > 0;
        set
        {
            if (value)
                updatedFlags |= PeriodSummaryUpdatedFlags.HighestAskPriceFlag;

            else if (IsHighestAskPriceUpdated) updatedFlags ^= PeriodSummaryUpdatedFlags.HighestAskPriceFlag;
        }
    }

    public decimal LowestBidPrice
    {
        get => lowestBidPrice;
        set
        {
            if (lowestBidPrice == value) return;
            IsLowestBidPriceUpdated = true;
            lowestBidPrice          = value;
        }
    }

    public bool IsLowestBidPriceUpdated
    {
        get => (updatedFlags & PeriodSummaryUpdatedFlags.LowestBidPriceFlag) > 0;
        set
        {
            if (value)
                updatedFlags |= PeriodSummaryUpdatedFlags.LowestBidPriceFlag;

            else if (IsLowestBidPriceUpdated) updatedFlags ^= PeriodSummaryUpdatedFlags.LowestBidPriceFlag;
        }
    }

    public decimal LowestAskPrice
    {
        get => lowestAskPrice;
        set
        {
            if (lowestAskPrice == value) return;
            IsLowestAskPriceUpdated = true;
            lowestAskPrice          = value;
        }
    }

    public bool IsLowestAskPriceUpdated
    {
        get => (updatedFlags & PeriodSummaryUpdatedFlags.LowestAskPriceFlag) > 0;
        set
        {
            if (value)
                updatedFlags |= PeriodSummaryUpdatedFlags.LowestAskPriceFlag;

            else if (IsLowestAskPriceUpdated) updatedFlags ^= PeriodSummaryUpdatedFlags.LowestAskPriceFlag;
        }
    }

    public decimal EndBidPrice
    {
        get => endBidPrice;
        set
        {
            if (endBidPrice == value) return;
            IsEndBidPriceUpdated = true;
            endBidPrice          = value;
        }
    }

    public bool IsEndBidPriceUpdated
    {
        get => (updatedFlags & PeriodSummaryUpdatedFlags.EndBidPriceFlag) > 0;
        set
        {
            if (value)
                updatedFlags |= PeriodSummaryUpdatedFlags.EndBidPriceFlag;

            else if (IsEndBidPriceUpdated) updatedFlags ^= PeriodSummaryUpdatedFlags.EndBidPriceFlag;
        }
    }

    public decimal EndAskPrice
    {
        get => endAskPrice;
        set
        {
            if (endAskPrice == value) return;
            IsEndAskPriceUpdated = true;
            endAskPrice          = value;
        }
    }

    public bool IsEndAskPriceUpdated
    {
        get => (updatedFlags & PeriodSummaryUpdatedFlags.EndAskPriceFlag) > 0;
        set
        {
            if (value)
                updatedFlags |= PeriodSummaryUpdatedFlags.EndAskPriceFlag;

            else if (IsEndAskPriceUpdated) updatedFlags ^= PeriodSummaryUpdatedFlags.EndAskPriceFlag;
        }
    }

    public bool IsAverageBidPriceUpdated
    {
        get => (updatedFlags & PeriodSummaryUpdatedFlags.AverageBidPriceFlag) > 0;
        set
        {
            if (value)
                updatedFlags |= PeriodSummaryUpdatedFlags.AverageBidPriceFlag;

            else if (IsEndAskPriceUpdated) updatedFlags ^= PeriodSummaryUpdatedFlags.AverageBidPriceFlag;
        }
    }

    public bool IsAverageAskPriceUpdated
    {
        get => (updatedFlags & PeriodSummaryUpdatedFlags.AverageAskPriceFlag) > 0;
        set
        {
            if (value)
                updatedFlags |= PeriodSummaryUpdatedFlags.AverageAskPriceFlag;

            else if (IsEndAskPriceUpdated) updatedFlags ^= PeriodSummaryUpdatedFlags.AverageAskPriceFlag;
        }
    }

    public uint TickCount
    {
        get => tickCount;
        set
        {
            if (tickCount == value) return;
            IsTickCountUpdated = true;
            tickCount          = value;
        }
    }

    public bool IsTickCountUpdated
    {
        get => (updatedFlags & PeriodSummaryUpdatedFlags.TickCountFlag) > 0;
        set
        {
            if (value)
                updatedFlags |= PeriodSummaryUpdatedFlags.TickCountFlag;

            else if (IsTickCountUpdated) updatedFlags ^= PeriodSummaryUpdatedFlags.TickCountFlag;
        }
    }

    public long PeriodVolume
    {
        get => periodVolume;
        set
        {
            if (periodVolume == value) return;
            IsPeriodVolumeUpdated = true;

            periodVolume = value;
        }
    }

    public bool IsPeriodVolumeUpdated
    {
        get => (updatedFlags & PeriodSummaryUpdatedFlags.PeriodVolumeFlag) > 0;
        set
        {
            if (value)
                updatedFlags |= PeriodSummaryUpdatedFlags.PeriodVolumeFlag;

            else if (IsPeriodVolumeUpdated) updatedFlags ^= PeriodSummaryUpdatedFlags.PeriodVolumeFlag;
        }
    }

    public bool IsPricePeriodSummaryFlagsUpdated
    {
        get => (updatedFlags & PeriodSummaryUpdatedFlags.PricePeriodSummaryFlagsFlag) > 0;
        set
        {
            if (value)
                updatedFlags |= PeriodSummaryUpdatedFlags.PricePeriodSummaryFlagsFlag;

            else if (IsPricePeriodSummaryFlagsUpdated) updatedFlags ^= PeriodSummaryUpdatedFlags.PricePeriodSummaryFlagsFlag;
        }
    }

    public IPQPricePeriodSummary? Previous
    {
        get => ((IPricePeriodSummary)this).Previous as IPQPricePeriodSummary;
        set => ((IPricePeriodSummary)this).Previous = value;
    }

    public IPQPricePeriodSummary? Next
    {
        get => ((IPricePeriodSummary)this).Next as IPQPricePeriodSummary;
        set => ((IPricePeriodSummary)this).Next = value;
    }

    IPricePeriodSummary? IDoublyLinkedListNode<IPricePeriodSummary>.Previous { get; set; }

    IPricePeriodSummary? IDoublyLinkedListNode<IPricePeriodSummary>.Next { get; set; }

    public bool HasUpdates
    {
        get => updatedFlags > 0;
        set => updatedFlags = value ? updatedFlags.AllFlags() : 0;
    }

    [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
    public IEnumerable<PQFieldUpdate> GetDeltaUpdateFields
    (DateTime snapShotTime, StorageFlags messageFlags,
        IPQPriceVolumePublicationPrecisionSettings? quotePublicationPrecisionSettings = null)
    {
        var updatedOnly = (messageFlags & StorageFlags.Update) > 0;
        if (!updatedOnly || IsSummaryPeriodUpdated) yield return new PQFieldUpdate(PQFieldKeys.SummaryPeriod, (uint)timeSeriesPeriod);
        if (!updatedOnly || IsStartTimeDateUpdated)
            yield return new PQFieldUpdate(PQFieldKeys.PeriodStartDateTime, startTime.GetHoursFromUnixEpoch());
        if (!updatedOnly || IsStartTimeSubHourUpdated)
        {
            var fifthByte = startTime.GetSubHourComponent().BreakLongToByteAndUint(out var lowerFourBytes);
            yield return new PQFieldUpdate(PQFieldKeys.PeriodStartSubHourTime, lowerFourBytes, fifthByte);
        }

        if (!updatedOnly || IsEndTimeDateUpdated) yield return new PQFieldUpdate(PQFieldKeys.PeriodEndDateTime, endTime.GetHoursFromUnixEpoch());
        if (!updatedOnly || IsEndTimeSubHourUpdated)
        {
            var fifthByte = endTime.GetSubHourComponent().BreakLongToByteAndUint(out var lowerFourBytes);
            yield return new PQFieldUpdate(PQFieldKeys.PeriodEndSubHourTime, lowerFourBytes, fifthByte);
        }

        if (!updatedOnly || IsStartBidPriceUpdated)
            yield return new PQFieldUpdate(PQFieldKeys.PeriodStartPrice, StartBidPrice,
                                           quotePublicationPrecisionSettings!.PriceScalingPrecision);
        if (!updatedOnly || IsStartAskPriceUpdated)
            yield return new PQFieldUpdate(PQFieldKeys.PeriodStartPrice, StartAskPrice,
                                           (byte)(PQFieldFlags.IsAskSideFlag | quotePublicationPrecisionSettings!.PriceScalingPrecision));
        if (!updatedOnly || IsHighestBidPriceUpdated)
            yield return new PQFieldUpdate(PQFieldKeys.PeriodHighestPrice, HighestBidPrice,
                                           quotePublicationPrecisionSettings!.PriceScalingPrecision);
        if (!updatedOnly || IsHighestAskPriceUpdated)
            yield return new PQFieldUpdate(PQFieldKeys.PeriodHighestPrice, HighestAskPrice,
                                           (byte)(PQFieldFlags.IsAskSideFlag | quotePublicationPrecisionSettings!.PriceScalingPrecision));
        if (!updatedOnly || IsLowestBidPriceUpdated)
            yield return new PQFieldUpdate(PQFieldKeys.PeriodLowestPrice, LowestBidPrice,
                                           quotePublicationPrecisionSettings!.PriceScalingPrecision);
        if (!updatedOnly || IsLowestAskPriceUpdated)
            yield return new PQFieldUpdate(PQFieldKeys.PeriodLowestPrice, LowestAskPrice,
                                           (byte)(PQFieldFlags.IsAskSideFlag | quotePublicationPrecisionSettings!.PriceScalingPrecision));
        if (!updatedOnly || IsEndBidPriceUpdated)
            yield return new PQFieldUpdate(PQFieldKeys.PeriodEndPrice, EndBidPrice,
                                           quotePublicationPrecisionSettings!.PriceScalingPrecision);
        if (!updatedOnly || IsEndAskPriceUpdated)
            yield return new PQFieldUpdate(PQFieldKeys.PeriodEndPrice, EndAskPrice,
                                           (byte)(PQFieldFlags.IsAskSideFlag | quotePublicationPrecisionSettings!.PriceScalingPrecision));
        if (!updatedOnly || IsTickCountUpdated) yield return new PQFieldUpdate(PQFieldKeys.PeriodTickCount, TickCount);
        if (!updatedOnly || IsPeriodVolumeUpdated)
            yield return new PQFieldUpdate(PQFieldKeys.PeriodVolume, (decimal)PeriodVolume,
                                           quotePublicationPrecisionSettings!.VolumeScalingPrecision);

        if (!updatedOnly || IsPricePeriodSummaryFlagsUpdated)
        {
            var flagsUint = (uint)PeriodSummaryFlags;
            yield return new PQFieldUpdate(PQFieldKeys.PeriodSummaryFlags, flagsUint);
        }

        if (!updatedOnly || IsAverageBidPriceUpdated)
            yield return new PQFieldUpdate(PQFieldKeys.PeriodAveragePrice, AverageBidPrice,
                                           quotePublicationPrecisionSettings!.PriceScalingPrecision);
        if (!updatedOnly || IsAverageAskPriceUpdated)
            yield return new PQFieldUpdate(PQFieldKeys.PeriodAveragePrice, AverageAskPrice,
                                           (byte)(PQFieldFlags.IsAskSideFlag | quotePublicationPrecisionSettings!.PriceScalingPrecision));
    }

    public int UpdateField(PQFieldUpdate pqFieldUpdate)
    {
        switch (pqFieldUpdate.Id)
        {
            case PQFieldKeys.SummaryPeriod:
                TimeSeriesPeriod = (TimeSeriesPeriod)pqFieldUpdate.Value;
                return 0;
            case PQFieldKeys.PeriodStartDateTime:
                IsStartTimeDateUpdated = true;
                PQFieldConverters.UpdateHoursFromUnixEpoch(ref startTime, pqFieldUpdate.Value);
                return 0;
            case PQFieldKeys.PeriodStartSubHourTime:
                IsStartTimeSubHourUpdated = true;
                PQFieldConverters.UpdateSubHourComponent(ref startTime,
                                                         pqFieldUpdate.Flag.AppendUintToMakeLong(pqFieldUpdate.Value));
                return 0;
            case PQFieldKeys.PeriodEndDateTime:
                IsEndTimeDateUpdated = true;
                PQFieldConverters.UpdateHoursFromUnixEpoch(ref endTime, pqFieldUpdate.Value);
                return 0;
            case PQFieldKeys.PeriodEndSubHourTime:
                IsEndTimeSubHourUpdated = true;
                PQFieldConverters.UpdateSubHourComponent(ref endTime,
                                                         pqFieldUpdate.Flag.AppendUintToMakeLong(pqFieldUpdate.Value));
                return 0;
            case PQFieldKeys.PeriodStartPrice:
                if (pqFieldUpdate.IsBid())
                    StartBidPrice = PQScaling.Unscale(pqFieldUpdate.Value, pqFieldUpdate.Flag);
                else
                    StartAskPrice = PQScaling.Unscale(pqFieldUpdate.Value, pqFieldUpdate.Flag);
                return 0;
            case PQFieldKeys.PeriodHighestPrice:
                if (pqFieldUpdate.IsBid())
                    HighestBidPrice = PQScaling.Unscale(pqFieldUpdate.Value, pqFieldUpdate.Flag);
                else
                    HighestAskPrice = PQScaling.Unscale(pqFieldUpdate.Value, pqFieldUpdate.Flag);
                return 0;
            case PQFieldKeys.PeriodLowestPrice:
                if (pqFieldUpdate.IsBid())
                    LowestBidPrice = PQScaling.Unscale(pqFieldUpdate.Value, pqFieldUpdate.Flag);
                else
                    LowestAskPrice = PQScaling.Unscale(pqFieldUpdate.Value, pqFieldUpdate.Flag);
                return 0;
            case PQFieldKeys.PeriodEndPrice:
                if (pqFieldUpdate.IsBid())
                    EndBidPrice = PQScaling.Unscale(pqFieldUpdate.Value, pqFieldUpdate.Flag);
                else
                    EndAskPrice = PQScaling.Unscale(pqFieldUpdate.Value, pqFieldUpdate.Flag);
                return 0;
            case PQFieldKeys.PeriodTickCount:
                TickCount = pqFieldUpdate.Value;
                return 0;
            case PQFieldKeys.PeriodVolume:
                PeriodVolume = (long)PQScaling.Unscale(pqFieldUpdate.Value, pqFieldUpdate.Flag);
                return 0;
            case PQFieldKeys.PeriodSummaryFlags:
                PeriodSummaryFlags = (PricePeriodSummaryFlags)pqFieldUpdate.Value;
                return 0;
            case PQFieldKeys.PeriodAveragePrice:
                if (pqFieldUpdate.IsBid())
                    AverageBidPrice = PQScaling.Unscale(pqFieldUpdate.Value, pqFieldUpdate.Flag);
                else
                    AverageAskPrice = PQScaling.Unscale(pqFieldUpdate.Value, pqFieldUpdate.Flag);
                return 0;
            default: return -1;
        }
    }

    IReusableObject<IPricePeriodSummary> IStoreState<IReusableObject<IPricePeriodSummary>>.CopyFrom
        (IReusableObject<IPricePeriodSummary> source, CopyMergeFlags copyMergeFlags) =>
        (IPQPricePeriodSummary)CopyFrom((IPricePeriodSummary)source, copyMergeFlags);

    public override IPricePeriodSummary CopyFrom(IPricePeriodSummary ps, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (!(ps is IPQPricePeriodSummary pqPs))
        {
            TimeSeriesPeriod   = ps.TimeSeriesPeriod;
            PeriodStartTime    = ps.PeriodStartTime;
            PeriodEndTime      = ps.PeriodEndTime;
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
            PeriodSummaryFlags = ps.PeriodSummaryFlags;
            AverageBidPrice    = ps.AverageBidAsk.BidPrice;
            AverageAskPrice    = ps.AverageBidAsk.AskPrice;
        }
        else
        {
            // between types only copy the changed parts not everything.
            if (pqPs.IsSummaryPeriodUpdated) TimeSeriesPeriod = pqPs.TimeSeriesPeriod;
            if (pqPs.IsStartTimeDateUpdated)
                PQFieldConverters.UpdateHoursFromUnixEpoch(ref startTime,
                                                           pqPs.PeriodStartTime.GetHoursFromUnixEpoch());
            if (pqPs.IsStartTimeSubHourUpdated)
                PQFieldConverters.UpdateSubHourComponent(ref startTime,
                                                         pqPs.PeriodStartTime.GetSubHourComponent());
            if (pqPs.IsEndTimeDateUpdated)
                PQFieldConverters.UpdateHoursFromUnixEpoch(ref endTime,
                                                           pqPs.PeriodEndTime.GetHoursFromUnixEpoch());
            if (pqPs.IsEndTimeSubHourUpdated)
                PQFieldConverters.UpdateSubHourComponent(ref endTime,
                                                         pqPs.PeriodEndTime.GetSubHourComponent());

            if (pqPs.IsStartBidPriceUpdated) StartBidPrice     = pqPs.StartBidPrice;
            if (pqPs.IsStartAskPriceUpdated) StartAskPrice     = pqPs.StartAskPrice;
            if (pqPs.IsHighestBidPriceUpdated) HighestBidPrice = pqPs.HighestBidPrice;
            if (pqPs.IsHighestAskPriceUpdated) HighestAskPrice = pqPs.HighestAskPrice;
            if (pqPs.IsLowestBidPriceUpdated) LowestBidPrice   = pqPs.LowestBidPrice;
            if (pqPs.IsLowestAskPriceUpdated) LowestAskPrice   = pqPs.LowestAskPrice;
            if (pqPs.IsEndBidPriceUpdated) EndBidPrice         = pqPs.EndBidPrice;
            if (pqPs.IsEndAskPriceUpdated) EndAskPrice         = pqPs.EndAskPrice;
            if (pqPs.IsTickCountUpdated) TickCount             = pqPs.TickCount;
            if (pqPs.IsPeriodVolumeUpdated) PeriodVolume       = pqPs.PeriodVolume;

            if (pqPs.IsPricePeriodSummaryFlagsUpdated) PeriodSummaryFlags = pqPs.PeriodSummaryFlags;

            if (pqPs.IsAverageBidPriceUpdated) AverageBidPrice = pqPs.AverageBidPrice;
            if (pqPs.IsAverageAskPriceUpdated) AverageAskPrice = pqPs.AverageAskPrice;

            if (pqPs is PQPricePeriodSummary pqPeriodSummary) updatedFlags = pqPeriodSummary.updatedFlags;
        }

        return this;
    }

    IStoreState IStoreState.CopyFrom(IStoreState source, CopyMergeFlags copyMergeFlags) => CopyFrom((IPricePeriodSummary)source, copyMergeFlags);

    IMutablePricePeriodSummary IMutablePricePeriodSummary.Clone() => Clone();

    IPQPricePeriodSummary IPQPricePeriodSummary.Clone() => Clone();

    object ICloneable.Clone() => Clone();

    IPricePeriodSummary ICloneable<IPricePeriodSummary>.Clone() => Clone();

    public override void StateReset()
    {
        Next       = Previous = null;
        IsEmpty    = true;
        HasUpdates = false;
        base.StateReset();
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
        var periodSummaryFlagsSame = PeriodSummaryFlags == other.PeriodSummaryFlags;
        var averageBidSame         = AverageBidPrice == other.AverageBidAsk.BidPrice;
        var averageAskSame         = AverageAskPrice == other.AverageBidAsk.AskPrice;

        var updateFlagsSame = true;
        if (exactTypes)
        {
            var otherPQ = (PQPricePeriodSummary)other;
            updateFlagsSame = updatedFlags == otherPQ.updatedFlags;
        }

        var allAreSame = timeFrameSame && startTimeSame && endTimeSame && startBidPriceSame && startAskPriceSame
                      && highestBidPriceSame && highestAskPriceSame && lowestBidPriceSame && lowestAskPriceSame
                      && endBidPriceSame && endAskPriceSame && tickCountSame && periodVolumeSame && periodSummaryFlagsSame
                      && updateFlagsSame && averageBidSame && averageAskSame;
        if (!allAreSame) Debugger.Break();
        return allAreSame;
    }

    public DateTime StorageTime(IStorageTimeResolver<PQPricePeriodSummary>? resolver = null) => PeriodEndTime;

    public IPQPricePeriodSummary CopyFrom
        (IPQPricePeriodSummary ps, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default) =>
        (IPQPricePeriodSummary)CopyFrom((IPricePeriodSummary)ps, copyMergeFlags);

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent((IPricePeriodSummary?)obj, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = timeSeriesPeriod.GetHashCode();
            hashCode = (hashCode * 397) ^ startTime.GetHashCode();
            hashCode = (hashCode * 397) ^ endTime.GetHashCode();
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
            hashCode = (hashCode * 397) ^ periodSummaryFlags.GetHashCode();
            hashCode = (hashCode * 397) ^ averageBidPrice.GetHashCode();
            hashCode = (hashCode * 397) ^ averageAskPrice.GetHashCode();
            return hashCode;
        }
    }

    public override string ToString() =>
        $"PQPeriodSummary {{ {nameof(TimeSeriesPeriod)}: {TimeSeriesPeriod}, {nameof(PeriodStartTime)}: {PeriodStartTime}, " +
        $"{nameof(PeriodEndTime)}: {PeriodEndTime}, {nameof(StartBidPrice)}: {StartBidPrice}, {nameof(StartAskPrice)}:" +
        $" {StartAskPrice}, {nameof(HighestBidPrice)}: {HighestBidPrice}, {nameof(HighestAskPrice)}: " +
        $"{HighestAskPrice}, {nameof(LowestBidPrice)}: {LowestBidPrice}, {nameof(LowestAskPrice)}: " +
        $"{LowestAskPrice}, {nameof(EndBidPrice)}: {EndBidPrice}, {nameof(EndAskPrice)}: {EndAskPrice}, " +
        $"{nameof(TickCount)}: {TickCount}, {nameof(PeriodVolume)}: {PeriodVolume}, {nameof(PeriodSummaryFlags)}: {PeriodSummaryFlags}, " +
        $"{nameof(AverageBidPrice)}: {AverageBidPrice}, {nameof(AverageAskPrice)}: {AverageAskPrice} }}";
}
