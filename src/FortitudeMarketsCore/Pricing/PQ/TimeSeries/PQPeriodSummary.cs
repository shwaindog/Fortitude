// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.Chronometry;
using FortitudeCommon.Types;
using FortitudeIO.TimeSeries;
using FortitudeMarketsApi.Pricing.TimeSeries;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.Serdes.Serialization;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.TimeSeries;

public interface IPQPricePeriodSummary : IMutablePricePeriodSummary, IPQSupportsFieldUpdates<IPricePeriodSummary>
{
    bool IsSummaryPeriodUpdated          { get; set; }
    bool IsStartTimeDateUpdated          { get; set; }
    bool IsStartTimeSubHourUpdated       { get; set; }
    bool IsEndTimeDateUpdated            { get; set; }
    bool IsEndTimeSubHourUpdated         { get; set; }
    bool IsStartBidPriceUpdated          { get; set; }
    bool IsStartAskPriceUpdated          { get; set; }
    bool IsHighestBidPriceUpdated        { get; set; }
    bool IsHighestAskPriceUpdated        { get; set; }
    bool IsLowestBidPriceUpdated         { get; set; }
    bool IsLowestAskPriceUpdated         { get; set; }
    bool IsEndBidPriceUpdated            { get; set; }
    bool IsEndAskPriceUpdated            { get; set; }
    bool IsTickCountUpdated              { get; set; }
    bool IsPeriodVolumeLowerBytesUpdated { get; set; }
    bool IsPeriodVolumeUpperBytesUpdated { get; set; }
    bool IsAverageBidPriceUpdated        { get; set; }
    bool IsAverageAskPriceUpdated        { get; set; }
}

public class PQPricePeriodSummary : IPQPricePeriodSummary
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
        AverageAskPrice  = toClone.AverageBidPrice;
    }

    public TimeSeriesPeriod SummaryPeriod
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

    public DateTime SummaryStartTime
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

    public DateTime SummaryEndTime
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
            IsPeriodVolumeLowerBytesUpdated |= unchecked((uint)periodVolume != (uint)value);
            IsPeriodVolumeUpperBytesUpdated |= periodVolume >> 32 != value >> 32;
            periodVolume                    =  value;
        }
    }

    public bool IsPeriodVolumeLowerBytesUpdated
    {
        get => (updatedFlags & PeriodSummaryUpdatedFlags.PeriodVolumeLowerBytesFlag) > 0;
        set
        {
            if (value)
                updatedFlags |= PeriodSummaryUpdatedFlags.PeriodVolumeLowerBytesFlag;
            else if (IsPeriodVolumeLowerBytesUpdated)
                updatedFlags ^= PeriodSummaryUpdatedFlags.PeriodVolumeLowerBytesFlag;
        }
    }

    public bool IsPeriodVolumeUpperBytesUpdated
    {
        get => (updatedFlags & PeriodSummaryUpdatedFlags.PeriodVolumeUpperBytesFlag) > 0;
        set
        {
            if (value)
                updatedFlags |= PeriodSummaryUpdatedFlags.PeriodVolumeUpperBytesFlag;
            else if (IsPeriodVolumeUpperBytesUpdated)
                updatedFlags ^= PeriodSummaryUpdatedFlags.PeriodVolumeUpperBytesFlag;
        }
    }

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
        if (!updatedOnly || IsSummaryPeriodUpdated)
            yield return new PQFieldUpdate(PQFieldKeys.SummaryPeriod, (uint)timeSeriesPeriod);
        if (!updatedOnly || IsStartTimeDateUpdated)
            yield return new PQFieldUpdate(PQFieldKeys.PeriodStartDateTime, startTime.GetHoursFromUnixEpoch());
        if (!updatedOnly || IsStartTimeSubHourUpdated)
        {
            var fifthByte = startTime.GetSubHourComponent().BreakLongToByteAndUint(out var lowerFourBytes);
            yield return new PQFieldUpdate(PQFieldKeys.PeriodStartSubHourTime, lowerFourBytes, fifthByte);
        }

        if (!updatedOnly || IsEndTimeDateUpdated)
            yield return new PQFieldUpdate(PQFieldKeys.PeriodEndDateTime, endTime.GetHoursFromUnixEpoch());
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
        if (!updatedOnly || IsPeriodVolumeLowerBytesUpdated)
        {
            var lowerBytes = (uint)PeriodVolume;
            yield return new PQFieldUpdate(PQFieldKeys.PeriodVolumeLowerBytes, lowerBytes);
        }

        if (!updatedOnly || IsPeriodVolumeUpperBytesUpdated)
        {
            var upperBytes = (uint)(PeriodVolume >> 32);
            yield return new PQFieldUpdate(PQFieldKeys.PeriodVolumeUpperBytes, upperBytes);
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
                SummaryPeriod = (TimeSeriesPeriod)pqFieldUpdate.Value;
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
            case PQFieldKeys.PeriodVolumeLowerBytes:
                PeriodVolume = (PeriodVolume & unchecked((long)0xFFFF_FFFF_0000_0000)) | pqFieldUpdate.Value;
                return 0;
            case PQFieldKeys.PeriodVolumeUpperBytes:
                PeriodVolume = (PeriodVolume & 0x0000_0000_FFFF_FFFF) | ((long)pqFieldUpdate.Value << 32);
                return 0;
            case PQFieldKeys.PeriodAveragePrice:
                if (pqFieldUpdate.IsBid())
                    AverageBidPrice = PQScaling.Unscale(pqFieldUpdate.Value, pqFieldUpdate.Flag);
                else
                    AverageAskPrice = PQScaling.Unscale(pqFieldUpdate.Value, pqFieldUpdate.Flag);
                return 0;
            default:
                return -1;
        }
    }

    public IPricePeriodSummary CopyFrom(IPricePeriodSummary ps, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (!(ps is IPQPricePeriodSummary pqPs))
        {
            SummaryPeriod    = ps.SummaryPeriod;
            SummaryStartTime = ps.SummaryStartTime;
            SummaryEndTime   = ps.SummaryEndTime;
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
        }
        else
        {
            // between types only copy the changed parts not everything.
            if (pqPs.IsSummaryPeriodUpdated)
                SummaryPeriod = pqPs.SummaryPeriod;
            if (pqPs.IsStartTimeDateUpdated)
                PQFieldConverters.UpdateHoursFromUnixEpoch(ref startTime,
                                                           pqPs.SummaryStartTime.GetHoursFromUnixEpoch());
            if (pqPs.IsStartTimeSubHourUpdated)
                PQFieldConverters.UpdateSubHourComponent(ref startTime,
                                                         pqPs.SummaryStartTime.GetSubHourComponent());
            if (pqPs.IsEndTimeDateUpdated)
                PQFieldConverters.UpdateHoursFromUnixEpoch(ref endTime,
                                                           pqPs.SummaryEndTime.GetHoursFromUnixEpoch());
            if (pqPs.IsEndTimeSubHourUpdated)
                PQFieldConverters.UpdateSubHourComponent(ref endTime,
                                                         pqPs.SummaryEndTime.GetSubHourComponent());
            if (pqPs.IsStartBidPriceUpdated) StartBidPrice     = pqPs.StartBidPrice;
            if (pqPs.IsStartAskPriceUpdated) StartAskPrice     = pqPs.StartAskPrice;
            if (pqPs.IsHighestBidPriceUpdated) HighestBidPrice = pqPs.HighestBidPrice;
            if (pqPs.IsHighestAskPriceUpdated) HighestAskPrice = pqPs.HighestAskPrice;
            if (pqPs.IsLowestBidPriceUpdated) LowestBidPrice   = pqPs.LowestBidPrice;
            if (pqPs.IsLowestAskPriceUpdated) LowestAskPrice   = pqPs.LowestAskPrice;
            if (pqPs.IsEndBidPriceUpdated) EndBidPrice         = pqPs.EndBidPrice;
            if (pqPs.IsEndAskPriceUpdated) EndAskPrice         = pqPs.EndAskPrice;
            if (pqPs.IsTickCountUpdated) TickCount             = pqPs.TickCount;
            if (pqPs.IsPeriodVolumeLowerBytesUpdated || pqPs.IsPeriodVolumeUpperBytesUpdated)
                PeriodVolume = pqPs.PeriodVolume;
            if (pqPs.IsAverageBidPriceUpdated) AverageBidPrice = pqPs.AverageBidPrice;
            if (pqPs.IsAverageAskPriceUpdated) AverageAskPrice = pqPs.AverageAskPrice;

            if (pqPs is PQPricePeriodSummary pqPeriodSummary) updatedFlags = pqPeriodSummary.updatedFlags;
        }

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
            var otherPQ = (PQPricePeriodSummary)other;
            updateFlagsSame = updatedFlags == otherPQ.updatedFlags;
        }

        var allAreSame = timeFrameSame && startTimeSame && endTimeSame && startBidPriceSame && startAskPriceSame
                      && highestBidPriceSame && highestAskPriceSame && lowestBidPriceSame && lowestAskPriceSame
                      && endBidPriceSame && endAskPriceSame && tickCountSame && periodVolumeSame && updateFlagsSame
                      && averageBidSame && averageAskSame;
        if (!allAreSame) Debugger.Break();
        return allAreSame;
    }

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
            hashCode = (hashCode * 397) ^ averageBidPrice.GetHashCode();
            hashCode = (hashCode * 397) ^ averageAskPrice.GetHashCode();
            return hashCode;
        }
    }

    public override string ToString() =>
        $"PQPeriodSummary {{ {nameof(SummaryPeriod)}: {SummaryPeriod}, {nameof(SummaryStartTime)}: {SummaryStartTime}, " +
        $"{nameof(SummaryEndTime)}: {SummaryEndTime}, {nameof(StartBidPrice)}: {StartBidPrice}, {nameof(StartAskPrice)}:" +
        $" {StartAskPrice}, {nameof(HighestBidPrice)}: {HighestBidPrice}, {nameof(HighestAskPrice)}: " +
        $"{HighestAskPrice}, {nameof(LowestBidPrice)}: {LowestBidPrice}, {nameof(LowestAskPrice)}: " +
        $"{LowestAskPrice}, {nameof(EndBidPrice)}: {EndBidPrice}, {nameof(EndAskPrice)}: {EndAskPrice}, " +
        $"{nameof(TickCount)}: {TickCount}, {nameof(PeriodVolume)}: {PeriodVolume}, {nameof(AverageBidPrice)}: {AverageBidPrice}, " +
        $"{nameof(AverageAskPrice)}: {AverageAskPrice} }}";
}
