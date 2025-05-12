// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.TimeSeries;
using FortitudeMarkets.Pricing.FeedEvents.Candles;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Candles;

public interface IPQCandle : IMutableCandle, IPQSupportsFieldUpdates<ICandle>
  , IDoublyLinkedListNode<IPQCandle>
{
    [JsonIgnore] bool IsCandlePeriodUpdated    { get; set; }
    [JsonIgnore] bool IsStartTimeDateUpdated    { get; set; }
    [JsonIgnore] bool IsStartTimeSub2MinUpdated { get; set; }
    [JsonIgnore] bool IsEndTimeDateUpdated      { get; set; }
    [JsonIgnore] bool IsEndTimeSub2MinUpdated   { get; set; }
    [JsonIgnore] bool IsStartBidPriceUpdated    { get; set; }
    [JsonIgnore] bool IsStartAskPriceUpdated    { get; set; }
    [JsonIgnore] bool IsHighestBidPriceUpdated  { get; set; }
    [JsonIgnore] bool IsHighestAskPriceUpdated  { get; set; }
    [JsonIgnore] bool IsLowestBidPriceUpdated   { get; set; }
    [JsonIgnore] bool IsLowestAskPriceUpdated   { get; set; }
    [JsonIgnore] bool IsEndBidPriceUpdated      { get; set; }
    [JsonIgnore] bool IsEndAskPriceUpdated      { get; set; }
    [JsonIgnore] bool IsTickCountUpdated        { get; set; }

    [JsonIgnore] bool IsPeriodVolumeUpdated            { get; set; }
    [JsonIgnore] bool IsCandleFlagsUpdated { get; set; }
    [JsonIgnore] bool IsAverageBidPriceUpdated         { get; set; }
    [JsonIgnore] bool IsAverageAskPriceUpdated         { get; set; }

    [JsonIgnore] new IPQCandle? Previous { get; set; }
    [JsonIgnore] new IPQCandle? Next     { get; set; }

    new IPQCandle Clone();
}

public class PQCandle : ReusableObject<ICandle>, IPQCandle, ICloneable<PQCandle>
{
    private   decimal  averageAskPrice;
    private   decimal  averageBidPrice;
    private   decimal  endAskPrice;
    private   decimal  endBidPrice;
    private   DateTime endTime = DateTime.MinValue;
    private   decimal  highestAskPrice;
    private   decimal  highestBidPrice;
    private   decimal  lowestAskPrice;
    private   decimal  lowestBidPrice;
    protected uint     NumUpdatesSinceEmpty = uint.MaxValue;

    private CandleFlags candleFlags;

    private long     periodVolume;
    private decimal  startAskPrice;
    private decimal  startBidPrice;
    private DateTime startTime = DateTime.MinValue;
    private uint     tickCount;

    private TimeBoundaryPeriod        timeBoundaryPeriod;
    private CandleUpdatedFlags updatedFlags;

    public PQCandle()
    {
        if (GetType() == typeof(PQCandle)) NumUpdatesSinceEmpty = 0;
    }

    public PQCandle(ICandle toClone)
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
        CandleFlags = toClone.CandleFlags;
        AverageBidPrice    = toClone.AverageBidAsk.BidPrice;
        AverageAskPrice    = toClone.AverageBidAsk.AskPrice;

        if (GetType() == typeof(PQCandle)) NumUpdatesSinceEmpty = 0;
    }

    public override PQCandle Clone() =>
        Recycler?.Borrow<PQCandle>().CopyFrom(this) as PQCandle ?? new PQCandle(this);


    public TimeBoundaryPeriod TimeBoundaryPeriod
    {
        get => timeBoundaryPeriod;
        set
        {
            IsCandlePeriodUpdated = timeBoundaryPeriod != value || NumUpdatesSinceEmpty == 0;
            timeBoundaryPeriod     = value;
        }
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool IsEmpty
    {
        get
        {
            var pricesAreAllZero = StartBidPrice == decimal.Zero && StartAskPrice == decimal.Zero && HighestBidPrice == decimal.Zero &&
                                   HighestAskPrice == decimal.Zero && LowestBidPrice == decimal.Zero && LowestAskPrice == decimal.Zero &&
                                   EndBidPrice == decimal.Zero && EndAskPrice == decimal.Zero &&
                                   AverageBidPrice == decimal.Zero && AverageAskPrice == decimal.Zero;
            var tickCountAndVolumeZero = TickCount == 0 && PeriodVolume == 0;
            var candlePeriodNone      = TimeBoundaryPeriod == TimeBoundaryPeriod.Tick;
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
            CandleFlags = CandleFlags.None;
            PeriodStartTime    = PeriodEndTime = DateTime.MinValue;

            NumUpdatesSinceEmpty = 0;
        }
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTime PeriodStartTime
    {
        get => startTime;
        set
        {
            if (startTime == value) return;
            IsStartTimeDateUpdated    |= startTime.Get2MinIntervalsFromUnixEpoch() != value.Get2MinIntervalsFromUnixEpoch();
            IsStartTimeSub2MinUpdated |= startTime.GetSub2MinComponent() != value.GetSub2MinComponent();
            startTime                 =  value;
        }
    }

    public BoundedTimeRange ToBoundedTimeRange(DateTime? maxDateTime = null) => new(PeriodStartTime, PeriodEndTime.Min(maxDateTime));

    public bool IsWhollyBoundedBy
        (ITimeBoundaryPeriodRange parentRange) =>
        parentRange.PeriodStartTime <= PeriodStartTime && parentRange.PeriodEnd() >= PeriodEndTime;

    public CandleFlags CandleFlags
    {
        get => candleFlags;
        set
        {
            IsCandleFlagsUpdated = candleFlags != value || NumUpdatesSinceEmpty == 0;
            candleFlags               = value;
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

    [JsonIgnore]
    public bool IsCandlePeriodUpdated
    {
        get => (updatedFlags & CandleUpdatedFlags.CandlePeriodFlag) > 0;
        set
        {
            if (value)
                updatedFlags |= CandleUpdatedFlags.CandlePeriodFlag;

            else if (IsCandlePeriodUpdated) updatedFlags ^= CandleUpdatedFlags.CandlePeriodFlag;
        }
    }

    [JsonIgnore]
    public bool IsStartTimeDateUpdated
    {
        get => (updatedFlags & CandleUpdatedFlags.StartTimeDateFlag) > 0;
        set
        {
            if (value)
                updatedFlags |= CandleUpdatedFlags.StartTimeDateFlag;

            else if (IsStartTimeDateUpdated) updatedFlags ^= CandleUpdatedFlags.StartTimeDateFlag;
        }
    }

    [JsonIgnore]
    public bool IsStartTimeSub2MinUpdated
    {
        get => (updatedFlags & CandleUpdatedFlags.StartTimeSub2MinFlag) > 0;
        set
        {
            if (value)
                updatedFlags |= CandleUpdatedFlags.StartTimeSub2MinFlag;

            else if (IsStartTimeSub2MinUpdated) updatedFlags ^= CandleUpdatedFlags.StartTimeSub2MinFlag;
        }
    }

    public DateTime PeriodEndTime
    {
        get => endTime;
        set
        {
            IsEndTimeDateUpdated    |= endTime.Get2MinIntervalsFromUnixEpoch() != value.Get2MinIntervalsFromUnixEpoch() || NumUpdatesSinceEmpty == 0;
            IsEndTimeSub2MinUpdated |= endTime.GetSub2MinComponent() != value.GetSub2MinComponent() || NumUpdatesSinceEmpty == 0;
            endTime                 =  value;
        }
    }

    public DateTime StorageTime(IStorageTimeResolver? resolver)
    {
        if (resolver is IStorageTimeResolver<ICandle> candleResolver) return candleResolver.ResolveStorageTime(this);
        return PeriodEndTime;
    }

    [JsonIgnore]
    public bool IsEndTimeDateUpdated
    {
        get => (updatedFlags & CandleUpdatedFlags.EndTimeDateFlag) > 0;
        set
        {
            if (value)
                updatedFlags |= CandleUpdatedFlags.EndTimeDateFlag;

            else if (IsEndTimeDateUpdated) updatedFlags ^= CandleUpdatedFlags.EndTimeDateFlag;
        }
    }

    [JsonIgnore]
    public bool IsEndTimeSub2MinUpdated
    {
        get => (updatedFlags & CandleUpdatedFlags.EndTimeSub2MinFlag) > 0;
        set
        {
            if (value)
                updatedFlags |= CandleUpdatedFlags.EndTimeSub2MinFlag;

            else if (IsEndTimeSub2MinUpdated) updatedFlags ^= CandleUpdatedFlags.EndTimeSub2MinFlag;
        }
    }
    public BidAskPair StartBidAsk   => new(StartBidPrice, StartAskPrice);
    public BidAskPair HighestBidAsk => new(HighestBidPrice, HighestAskPrice);
    public BidAskPair LowestBidAsk  => new(LowestBidPrice, LowestAskPrice);
    public BidAskPair EndBidAsk     => new(EndBidPrice, EndAskPrice);
    public BidAskPair AverageBidAsk => new(AverageBidPrice, AverageAskPrice);

    [JsonIgnore]
    public decimal AverageBidPrice
    {
        get => averageBidPrice;
        set
        {
            IsAverageBidPriceUpdated = averageBidPrice != value || NumUpdatesSinceEmpty == 0;
            averageBidPrice          = value;
        }
    }
    [JsonIgnore]
    public decimal AverageAskPrice
    {
        get => averageAskPrice;
        set
        {
            IsAverageAskPriceUpdated = averageAskPrice != value || NumUpdatesSinceEmpty == 0;

            averageAskPrice = value;
        }
    }

    [JsonIgnore]
    public decimal StartBidPrice
    {
        get => startBidPrice;
        set
        {
            IsStartBidPriceUpdated = startBidPrice != value || NumUpdatesSinceEmpty == 0;
            startBidPrice          = value;
        }
    }

    [JsonIgnore]
    public bool IsStartBidPriceUpdated
    {
        get => (updatedFlags & CandleUpdatedFlags.StartBidPriceFlag) > 0;
        set
        {
            if (value)
                updatedFlags |= CandleUpdatedFlags.StartBidPriceFlag;

            else if (IsStartBidPriceUpdated) updatedFlags ^= CandleUpdatedFlags.StartBidPriceFlag;
        }
    }

    [JsonIgnore]
    public decimal StartAskPrice
    {
        get => startAskPrice;
        set
        {
            IsStartAskPriceUpdated = startAskPrice != value || NumUpdatesSinceEmpty == 0;
            startAskPrice          = value;
        }
    }

    [JsonIgnore]
    public bool IsStartAskPriceUpdated
    {
        get => (updatedFlags & CandleUpdatedFlags.StartAskPriceFlag) > 0;
        set
        {
            if (value)
                updatedFlags |= CandleUpdatedFlags.StartAskPriceFlag;

            else if (IsStartAskPriceUpdated) updatedFlags ^= CandleUpdatedFlags.StartAskPriceFlag;
        }
    }

    [JsonIgnore]
    public decimal HighestBidPrice
    {
        get => highestBidPrice;
        set
        {
            IsHighestBidPriceUpdated = highestBidPrice != value || NumUpdatesSinceEmpty == 0;
            highestBidPrice          = value;
        }
    }

    [JsonIgnore]
    public bool IsHighestBidPriceUpdated
    {
        get => (updatedFlags & CandleUpdatedFlags.HighestBidPriceFlag) > 0;
        set
        {
            if (value)
                updatedFlags |= CandleUpdatedFlags.HighestBidPriceFlag;

            else if (IsHighestBidPriceUpdated) updatedFlags ^= CandleUpdatedFlags.HighestBidPriceFlag;
        }
    }

    [JsonIgnore]
    public decimal HighestAskPrice
    {
        get => highestAskPrice;
        set
        {
            IsHighestAskPriceUpdated = highestAskPrice != value || NumUpdatesSinceEmpty == 0;
            highestAskPrice          = value;
        }
    }

    [JsonIgnore]
    public bool IsHighestAskPriceUpdated
    {
        get => (updatedFlags & CandleUpdatedFlags.HighestAskPriceFlag) > 0;
        set
        {
            if (value)
                updatedFlags |= CandleUpdatedFlags.HighestAskPriceFlag;

            else if (IsHighestAskPriceUpdated) updatedFlags ^= CandleUpdatedFlags.HighestAskPriceFlag;
        }
    }

    [JsonIgnore]
    public decimal LowestBidPrice
    {
        get => lowestBidPrice;
        set
        {
            IsLowestBidPriceUpdated = lowestBidPrice != value || NumUpdatesSinceEmpty == 0;
            lowestBidPrice          = value;
        }
    }

    [JsonIgnore]
    public bool IsLowestBidPriceUpdated
    {
        get => (updatedFlags & CandleUpdatedFlags.LowestBidPriceFlag) > 0;
        set
        {
            if (value)
                updatedFlags |= CandleUpdatedFlags.LowestBidPriceFlag;

            else if (IsLowestBidPriceUpdated) updatedFlags ^= CandleUpdatedFlags.LowestBidPriceFlag;
        }
    }

    [JsonIgnore]
    public decimal LowestAskPrice
    {
        get => lowestAskPrice;
        set
        {
            IsLowestAskPriceUpdated = lowestAskPrice != value || NumUpdatesSinceEmpty == 0;
            lowestAskPrice          = value;
        }
    }

    [JsonIgnore]
    public bool IsLowestAskPriceUpdated
    {
        get => (updatedFlags & CandleUpdatedFlags.LowestAskPriceFlag) > 0;
        set
        {
            if (value)
                updatedFlags |= CandleUpdatedFlags.LowestAskPriceFlag;

            else if (IsLowestAskPriceUpdated) updatedFlags ^= CandleUpdatedFlags.LowestAskPriceFlag;
        }
    }

    [JsonIgnore]
    public decimal EndBidPrice
    {
        get => endBidPrice;
        set
        {
            IsEndBidPriceUpdated = endBidPrice != value || NumUpdatesSinceEmpty == 0;
            endBidPrice          = value;
        }
    }

    [JsonIgnore]
    public bool IsEndBidPriceUpdated
    {
        get => (updatedFlags & CandleUpdatedFlags.EndBidPriceFlag) > 0;
        set
        {
            if (value)
                updatedFlags |= CandleUpdatedFlags.EndBidPriceFlag;

            else if (IsEndBidPriceUpdated) updatedFlags ^= CandleUpdatedFlags.EndBidPriceFlag;
        }
    }

    [JsonIgnore]
    public decimal EndAskPrice
    {
        get => endAskPrice;
        set
        {
            IsEndAskPriceUpdated = endAskPrice != value || NumUpdatesSinceEmpty == 0;
            endAskPrice          = value;
        }
    }

    [JsonIgnore]
    public bool IsEndAskPriceUpdated
    {
        get => (updatedFlags & CandleUpdatedFlags.EndAskPriceFlag) > 0;
        set
        {
            if (value)
                updatedFlags |= CandleUpdatedFlags.EndAskPriceFlag;

            else if (IsEndAskPriceUpdated) updatedFlags ^= CandleUpdatedFlags.EndAskPriceFlag;
        }
    }

    [JsonIgnore]
    public bool IsAverageBidPriceUpdated
    {
        get => (updatedFlags & CandleUpdatedFlags.AverageBidPriceFlag) > 0;
        set
        {
            if (value)
                updatedFlags |= CandleUpdatedFlags.AverageBidPriceFlag;

            else if (IsEndAskPriceUpdated) updatedFlags ^= CandleUpdatedFlags.AverageBidPriceFlag;
        }
    }

    [JsonIgnore]
    public bool IsAverageAskPriceUpdated
    {
        get => (updatedFlags & CandleUpdatedFlags.AverageAskPriceFlag) > 0;
        set
        {
            if (value)
                updatedFlags |= CandleUpdatedFlags.AverageAskPriceFlag;

            else if (IsEndAskPriceUpdated) updatedFlags ^= CandleUpdatedFlags.AverageAskPriceFlag;
        }
    }

    public uint TickCount
    {
        get => tickCount;
        set
        {
            IsTickCountUpdated = tickCount != value || NumUpdatesSinceEmpty == 0;
            tickCount          = value;
        }
    }

    [JsonIgnore]
    public bool IsTickCountUpdated
    {
        get => (updatedFlags & CandleUpdatedFlags.TickCountFlag) > 0;
        set
        {
            if (value)
                updatedFlags |= CandleUpdatedFlags.TickCountFlag;

            else if (IsTickCountUpdated) updatedFlags ^= CandleUpdatedFlags.TickCountFlag;
        }
    }

    public long PeriodVolume
    {
        get => periodVolume;
        set
        {
            IsPeriodVolumeUpdated = periodVolume != value || NumUpdatesSinceEmpty == 0;
            periodVolume          = value;
        }
    }

    [JsonIgnore]
    public bool IsPeriodVolumeUpdated
    {
        get => (updatedFlags & CandleUpdatedFlags.PeriodVolumeFlag) > 0;
        set
        {
            if (value)
                updatedFlags |= CandleUpdatedFlags.PeriodVolumeFlag;

            else if (IsPeriodVolumeUpdated) updatedFlags ^= CandleUpdatedFlags.PeriodVolumeFlag;
        }
    }

    [JsonIgnore]
    public bool IsCandleFlagsUpdated
    {
        get => (updatedFlags & CandleUpdatedFlags.CandleFlagsFlag) > 0;
        set
        {
            if (value)
                updatedFlags |= CandleUpdatedFlags.CandleFlagsFlag;

            else if (IsCandleFlagsUpdated) updatedFlags ^= CandleUpdatedFlags.CandleFlagsFlag;
        }
    }

    [JsonIgnore]
    public IPQCandle? Previous
    {
        get => ((ICandle)this).Previous as IPQCandle;
        set => ((ICandle)this).Previous = value;
    }

    [JsonIgnore]
    public IPQCandle? Next
    {
        get => ((ICandle)this).Next as IPQCandle;
        set => ((ICandle)this).Next = value;
    }

    [JsonIgnore] ICandle? IDoublyLinkedListNode<ICandle>.Previous { get; set; }

    [JsonIgnore] ICandle? IDoublyLinkedListNode<ICandle>.Next { get; set; }

    [JsonIgnore]
    public bool HasUpdates
    {
        get => updatedFlags > 0;
        set => updatedFlags = value ? updatedFlags.AllFlags() : 0;
    }

    public uint UpdateCount => NumUpdatesSinceEmpty;

    public virtual void UpdateComplete()
    {
        if (HasUpdates && !IsEmpty) NumUpdatesSinceEmpty++;
        HasUpdates = false;
    }

    [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
    public IEnumerable<PQFieldUpdate> GetDeltaUpdateFields
    (DateTime snapShotTime, StorageFlags messageFlags,
        IPQPriceVolumePublicationPrecisionSettings? quotePublicationPrecisionSettings = null)
    {
        var updatedOnly = (messageFlags & StorageFlags.Update) > 0;
        if (!updatedOnly || IsCandlePeriodUpdated) yield return new PQFieldUpdate(PQFeedFields.PriceCandleStick, PQPricingSubFieldKeys.CandlePeriod, (uint)timeBoundaryPeriod);
        if (!updatedOnly || IsStartTimeDateUpdated)
            yield return new PQFieldUpdate(PQFeedFields.PriceCandleStick, PQPricingSubFieldKeys.CandleStartDateTime, startTime.Get2MinIntervalsFromUnixEpoch());
        if (!updatedOnly || IsStartTimeSub2MinUpdated)
        {
            var fifthByte = startTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var lowerFourBytes);
            yield return new PQFieldUpdate(PQFeedFields.PriceCandleStick, PQPricingSubFieldKeys.CandleStartSub2MinTime, lowerFourBytes, fifthByte);
        }

        if (!updatedOnly || IsEndTimeDateUpdated) yield return new PQFieldUpdate(PQFeedFields.PriceCandleStick, PQPricingSubFieldKeys.CandleEndDateTime, endTime.Get2MinIntervalsFromUnixEpoch());
        if (!updatedOnly || IsEndTimeSub2MinUpdated)
        {
            var fifthByte = endTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var lowerFourBytes);
            yield return new PQFieldUpdate(PQFeedFields.PriceCandleStick, PQPricingSubFieldKeys.CandleEndSub2MinTime, lowerFourBytes, fifthByte);
        }

        if (!updatedOnly || IsStartBidPriceUpdated)
            yield return new PQFieldUpdate(PQFeedFields.PriceCandleStick, PQPricingSubFieldKeys.CandleStartPrice, StartBidPrice,
                                           quotePublicationPrecisionSettings!.PriceScalingPrecision);
        if (!updatedOnly || IsStartAskPriceUpdated)
            yield return new PQFieldUpdate(PQFeedFields.PriceCandleStick, PQDepthKey.AskSide, PQPricingSubFieldKeys.CandleStartPrice,  StartAskPrice
                                         , quotePublicationPrecisionSettings!.PriceScalingPrecision);
        if (!updatedOnly || IsHighestBidPriceUpdated)
            yield return new PQFieldUpdate(PQFeedFields.PriceCandleStick, PQPricingSubFieldKeys.CandleHighestPrice, HighestBidPrice,
                                           quotePublicationPrecisionSettings!.PriceScalingPrecision);
        if (!updatedOnly || IsHighestAskPriceUpdated)
            yield return new PQFieldUpdate(PQFeedFields.PriceCandleStick, PQDepthKey.AskSide, PQPricingSubFieldKeys.CandleHighestPrice, HighestAskPrice
                                         , quotePublicationPrecisionSettings!.PriceScalingPrecision);
        if (!updatedOnly || IsLowestBidPriceUpdated)
            yield return new PQFieldUpdate(PQFeedFields.PriceCandleStick, PQPricingSubFieldKeys.CandleLowestPrice, LowestBidPrice,
                                           quotePublicationPrecisionSettings!.PriceScalingPrecision);
        if (!updatedOnly || IsLowestAskPriceUpdated)
            yield return new PQFieldUpdate(PQFeedFields.PriceCandleStick, PQDepthKey.AskSide, PQPricingSubFieldKeys.CandleLowestPrice, LowestAskPrice
                                         , quotePublicationPrecisionSettings!.PriceScalingPrecision);
        if (!updatedOnly || IsEndBidPriceUpdated)
            yield return new PQFieldUpdate(PQFeedFields.PriceCandleStick, PQPricingSubFieldKeys.CandleEndPrice, EndBidPrice,
                                           quotePublicationPrecisionSettings!.PriceScalingPrecision);
        if (!updatedOnly || IsEndAskPriceUpdated)
            yield return new PQFieldUpdate(PQFeedFields.PriceCandleStick, PQDepthKey.AskSide, PQPricingSubFieldKeys.CandleEndPrice, EndAskPrice
                                         , quotePublicationPrecisionSettings!.PriceScalingPrecision);
        if (!updatedOnly || IsTickCountUpdated) yield return new PQFieldUpdate(PQFeedFields.PriceCandleStick, PQPricingSubFieldKeys.CandleTickCount, TickCount);
        if (!updatedOnly || IsPeriodVolumeUpdated) yield return new PQFieldUpdate(PQFeedFields.PriceCandleStick, PQPricingSubFieldKeys.CandleVolume, PeriodVolume);

        if (!updatedOnly || IsCandleFlagsUpdated)
        {
            var flagsUint = (uint)CandleFlags;
            yield return new PQFieldUpdate(PQFeedFields.PriceCandleStick, PQPricingSubFieldKeys.CandleSummaryFlags, flagsUint);
        }

        if (!updatedOnly || IsAverageBidPriceUpdated)
            yield return new PQFieldUpdate(PQFeedFields.PriceCandleStick, PQPricingSubFieldKeys.CandleAveragePrice, AverageBidPrice,
                                           quotePublicationPrecisionSettings!.PriceScalingPrecision);
        if (!updatedOnly || IsAverageAskPriceUpdated)
            yield return new PQFieldUpdate(PQFeedFields.PriceCandleStick, PQDepthKey.AskSide, PQPricingSubFieldKeys.CandleAveragePrice,  AverageAskPrice
                                         , quotePublicationPrecisionSettings!.PriceScalingPrecision);
    }

    public int UpdateField(PQFieldUpdate pqFieldUpdate)
    {
        switch (pqFieldUpdate.PricingSubId)
        {
            case PQPricingSubFieldKeys.CandlePeriod:
                TimeBoundaryPeriod     = (TimeBoundaryPeriod)pqFieldUpdate.Payload;
                IsCandlePeriodUpdated = true;
                return 0;
            case PQPricingSubFieldKeys.CandleStartDateTime:
                IsStartTimeDateUpdated = true;
                PQFieldConverters.Update2MinuteIntervalsFromUnixEpoch(ref startTime, pqFieldUpdate.Payload);
                if(startTime == DateTime.UnixEpoch) startTime = DateTime.MinValue;
                return 0;
            case PQPricingSubFieldKeys.CandleStartSub2MinTime:
                IsStartTimeSub2MinUpdated = true;
                PQFieldConverters.UpdateSub2MinComponent
                    (ref startTime, pqFieldUpdate.Flag.AppendScaleFlagsToUintToMakeLong(pqFieldUpdate.Payload));
                if(startTime == DateTime.UnixEpoch) startTime = DateTime.MinValue;
                return 0;
            case PQPricingSubFieldKeys.CandleEndDateTime:
                IsEndTimeDateUpdated = true;
                PQFieldConverters.Update2MinuteIntervalsFromUnixEpoch(ref endTime, pqFieldUpdate.Payload);
                if(endTime == DateTime.UnixEpoch) endTime = DateTime.MinValue;
                return 0;
            case PQPricingSubFieldKeys.CandleEndSub2MinTime:
                IsEndTimeSub2MinUpdated = true;
                PQFieldConverters.UpdateSub2MinComponent
                    (ref endTime, pqFieldUpdate.Flag.AppendScaleFlagsToUintToMakeLong(pqFieldUpdate.Payload));
                if(endTime == DateTime.UnixEpoch) endTime = DateTime.MinValue;
                return 0;
            case PQPricingSubFieldKeys.CandleStartPrice:
                if (pqFieldUpdate.IsBid())
                {
                    StartBidPrice          = PQScaling.Unscale(pqFieldUpdate.Payload, pqFieldUpdate.Flag);
                    IsStartBidPriceUpdated = true;
                }
                else
                {
                    StartAskPrice = PQScaling.Unscale(pqFieldUpdate.Payload, pqFieldUpdate.Flag);
                    IsStartAskPriceUpdated = true;
                }
                return 0;
            case PQPricingSubFieldKeys.CandleHighestPrice:
                if (pqFieldUpdate.IsBid())
                {
                    HighestBidPrice = PQScaling.Unscale(pqFieldUpdate.Payload, pqFieldUpdate.Flag);
                    IsHighestBidPriceUpdated = true;
                }
                else
                {
                    HighestAskPrice          = PQScaling.Unscale(pqFieldUpdate.Payload, pqFieldUpdate.Flag);
                    IsHighestAskPriceUpdated = true;
                }
                return 0;
            case PQPricingSubFieldKeys.CandleLowestPrice:
                if (pqFieldUpdate.IsBid())
                {
                    LowestBidPrice          = PQScaling.Unscale(pqFieldUpdate.Payload, pqFieldUpdate.Flag);
                    IsLowestBidPriceUpdated = true;
                }
                else
                {
                    LowestAskPrice = PQScaling.Unscale(pqFieldUpdate.Payload, pqFieldUpdate.Flag);
                    IsLowestAskPriceUpdated = true;
                }
                return 0;
            case PQPricingSubFieldKeys.CandleEndPrice:
                if (pqFieldUpdate.IsBid())
                {
                    EndBidPrice = PQScaling.Unscale(pqFieldUpdate.Payload, pqFieldUpdate.Flag);
                    IsEndBidPriceUpdated = true;
                }
                else
                {
                    EndAskPrice          = PQScaling.Unscale(pqFieldUpdate.Payload, pqFieldUpdate.Flag);
                    IsEndAskPriceUpdated = true;
                }
                return 0;
            case PQPricingSubFieldKeys.CandleTickCount:
                TickCount          = pqFieldUpdate.Payload;
                IsTickCountUpdated = true;
                return 0;
            case PQPricingSubFieldKeys.CandleVolume:
                PeriodVolume          = pqFieldUpdate.ReadPayloadAndAuxiliaryAs48BitScaledLong();
                IsPeriodVolumeUpdated = true;
                return 0;
            case PQPricingSubFieldKeys.CandleSummaryFlags:
                CandleFlags               = (CandleFlags)pqFieldUpdate.Payload;
                IsCandleFlagsUpdated = true;
                return 0;
            case PQPricingSubFieldKeys.CandleAveragePrice:
                if (pqFieldUpdate.IsBid())
                {
                    AverageBidPrice = PQScaling.Unscale(pqFieldUpdate.Payload, pqFieldUpdate.Flag);
                    IsAverageBidPriceUpdated = true;
                }
                else
                {
                    AverageAskPrice          = PQScaling.Unscale(pqFieldUpdate.Payload, pqFieldUpdate.Flag);
                    IsAverageAskPriceUpdated = true;
                }
                return 0;
            default: return -1;
        }
    }

    IReusableObject<ICandle> ITransferState<IReusableObject<ICandle>>.CopyFrom
        (IReusableObject<ICandle> source, CopyMergeFlags copyMergeFlags) =>
        (IPQCandle)CopyFrom((ICandle)source, copyMergeFlags);

    public override ICandle CopyFrom(ICandle ps, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (!(ps is IPQCandle pqPs))
        {
            TimeBoundaryPeriod = ps.TimeBoundaryPeriod;
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
            CandleFlags = ps.CandleFlags;
            AverageBidPrice    = ps.AverageBidAsk.BidPrice;
            AverageAskPrice    = ps.AverageBidAsk.AskPrice;
        }
        else
        {
            // between types only copy the changed parts not everything.
            if (pqPs.IsCandlePeriodUpdated)
            {
                TimeBoundaryPeriod     = pqPs.TimeBoundaryPeriod;
                IsCandlePeriodUpdated = true;
            }
            if (pqPs.IsStartTimeDateUpdated)
            {
                PQFieldConverters.Update2MinuteIntervalsFromUnixEpoch(ref startTime,
                                                                      pqPs.PeriodStartTime.Get2MinIntervalsFromUnixEpoch());
                
                IsStartTimeDateUpdated = true;
                if (startTime == DateTime.UnixEpoch) startTime = DateTime.MinValue;
            }
            if (pqPs.IsStartTimeSub2MinUpdated)
            {
                PQFieldConverters.UpdateSub2MinComponent(ref startTime,
                                                         pqPs.PeriodStartTime.GetSub2MinComponent());
                
                IsStartTimeSub2MinUpdated = true;
                if (startTime == DateTime.UnixEpoch) startTime = DateTime.MinValue;
            }
            if (pqPs.IsEndTimeDateUpdated)
            {
                PQFieldConverters.Update2MinuteIntervalsFromUnixEpoch(ref endTime,
                                                                      pqPs.PeriodEndTime.Get2MinIntervalsFromUnixEpoch());
                
                IsEndTimeDateUpdated = true;
                if (endTime == DateTime.UnixEpoch) endTime = DateTime.MinValue;
            }
            if (pqPs.IsEndTimeSub2MinUpdated)
            {
                PQFieldConverters.UpdateSub2MinComponent(ref endTime,
                                                         pqPs.PeriodEndTime.GetSub2MinComponent());
                IsEndTimeSub2MinUpdated = true;
                if (endTime == DateTime.UnixEpoch) endTime = DateTime.MinValue;
            }

            if (pqPs.IsStartBidPriceUpdated)
            {
                StartBidPrice          = pqPs.StartBidPrice;
                IsStartBidPriceUpdated = true;
            }
            if (pqPs.IsStartAskPriceUpdated)
            {
                StartAskPrice          = pqPs.StartAskPrice;
                IsStartAskPriceUpdated = true;
            }
            if (pqPs.IsHighestBidPriceUpdated)
            {
                HighestBidPrice = pqPs.HighestBidPrice;
                IsHighestBidPriceUpdated = true;
            }
            if (pqPs.IsHighestAskPriceUpdated)
            {
                HighestAskPrice = pqPs.HighestAskPrice;
                IsHighestAskPriceUpdated = true;
            }
            if (pqPs.IsLowestBidPriceUpdated)
            {
                LowestBidPrice          = pqPs.LowestBidPrice;
                IsLowestBidPriceUpdated = true;
            }
            if (pqPs.IsLowestAskPriceUpdated)
            {
                LowestAskPrice          = pqPs.LowestAskPrice;
                IsLowestAskPriceUpdated = true;
            }
            if (pqPs.IsEndBidPriceUpdated)
            {
                EndBidPrice         = pqPs.EndBidPrice;
                IsEndBidPriceUpdated = true;
            }
            if (pqPs.IsEndAskPriceUpdated)
            {
                EndAskPrice         = pqPs.EndAskPrice;
                IsEndAskPriceUpdated = true;
            }
            if (pqPs.IsTickCountUpdated)
            {
                TickCount             = pqPs.TickCount;
                IsTickCountUpdated = true;
            }
            if (pqPs.IsPeriodVolumeUpdated)
            {
                PeriodVolume       = pqPs.PeriodVolume;
                IsPeriodVolumeUpdated = true;
            }
            if (pqPs.IsCandleFlagsUpdated)
            {
                CandleFlags = pqPs.CandleFlags;
                IsCandleFlagsUpdated = true;
            }
            if (pqPs.IsAverageBidPriceUpdated)
            {
                AverageBidPrice = pqPs.AverageBidPrice;
                IsAverageBidPriceUpdated = true;
            }
            if (pqPs.IsAverageAskPriceUpdated)
            {
                AverageAskPrice = pqPs.AverageAskPrice;
                IsAverageAskPriceUpdated = true;
            }

            if (pqPs is PQCandle pqCandle) updatedFlags = pqCandle.updatedFlags;
        }

        return this;
    }

    ITransferState ITransferState.CopyFrom
        (ITransferState source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom((ICandle)source, copyMergeFlags);

    IMutableCandle IMutableCandle.Clone() => Clone();

    IPQCandle IPQCandle.Clone() => Clone();

    object ICloneable.Clone() => Clone();

    ICandle ICloneable<ICandle>.Clone() => Clone();

    public override void StateReset()
    {
        Next       = Previous = null;
        IsEmpty    = true;
        HasUpdates = false;

        NumUpdatesSinceEmpty = 0;
        base.StateReset();
    }

    public bool AreEquivalent(ICandle? other, bool exactTypes = false)
    {
        if (other == null) return false;
        if (exactTypes && other.GetType() != GetType()) return false;

        var timeFrameSame          = TimeBoundaryPeriod == other.TimeBoundaryPeriod;
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
        var candleFlagsSame = CandleFlags == other.CandleFlags;
        var averageBidSame         = AverageBidPrice == other.AverageBidAsk.BidPrice;
        var averageAskSame         = AverageAskPrice == other.AverageBidAsk.AskPrice;

        var updateFlagsSame = true;
        if (exactTypes)
        {
            var otherPQ = (PQCandle)other;
            updateFlagsSame = updatedFlags == otherPQ.updatedFlags;
        }

        var allAreSame = timeFrameSame && startTimeSame && endTimeSame && startBidPriceSame && startAskPriceSame
                      && highestBidPriceSame && highestAskPriceSame && lowestBidPriceSame && lowestAskPriceSame
                      && endBidPriceSame && endAskPriceSame && tickCountSame && periodVolumeSame && candleFlagsSame
                      && updateFlagsSame && averageBidSame && averageAskSame;
        return allAreSame;
    }

    public DateTime StorageTime(IStorageTimeResolver<PQCandle>? resolver = null) => PeriodEndTime;

    public IPQCandle CopyFrom
        (IPQCandle ps, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default) =>
        (IPQCandle)CopyFrom((ICandle)ps, copyMergeFlags);

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent((ICandle?)obj, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = timeBoundaryPeriod.GetHashCode();
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
            hashCode = (hashCode * 397) ^ candleFlags.GetHashCode();
            hashCode = (hashCode * 397) ^ averageBidPrice.GetHashCode();
            hashCode = (hashCode * 397) ^ averageAskPrice.GetHashCode();
            return hashCode;
        }
    }

    public override string ToString() =>
        $"{nameof(PQCandle)} {{ {nameof(TimeBoundaryPeriod)}: {TimeBoundaryPeriod}, {nameof(PeriodStartTime)}: {PeriodStartTime}, " +
        $"{nameof(PeriodEndTime)}: {PeriodEndTime}, {nameof(StartBidPrice)}: {StartBidPrice}, {nameof(StartAskPrice)}:" +
        $" {StartAskPrice}, {nameof(HighestBidPrice)}: {HighestBidPrice}, {nameof(HighestAskPrice)}: " +
        $"{HighestAskPrice}, {nameof(LowestBidPrice)}: {LowestBidPrice}, {nameof(LowestAskPrice)}: " +
        $"{LowestAskPrice}, {nameof(EndBidPrice)}: {EndBidPrice}, {nameof(EndAskPrice)}: {EndAskPrice}, " +
        $"{nameof(TickCount)}: {TickCount}, {nameof(PeriodVolume)}: {PeriodVolume}, {nameof(CandleFlags)}: {CandleFlags}, " +
        $"{nameof(AverageBidPrice)}: {AverageBidPrice}, {nameof(AverageAskPrice)}: {AverageAskPrice} }}";
}
