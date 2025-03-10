// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.BusMessaging.Messages.ListeningSubscriptions;
using FortitudeBusRules.BusMessaging.Routing.Channel;
using FortitudeBusRules.BusMessaging.Routing.Response;
using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;
using FortitudeCommon.Chronometry;
using FortitudeCommon.Chronometry.Timers;
using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Extensions;
using FortitudeMarkets.Indicators.Pricing.Parameters;
using FortitudeMarkets.Pricing;
using FortitudeMarkets.Pricing.PQ.Converters;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes;
using FortitudeMarkets.Pricing.PQ.TimeSeries.BusRules;

#endregion

namespace FortitudeMarkets.Indicators.Pricing.MovingAverage.TimeWeighted;

public interface ITimeWeightedMovingAveragePublisherRule : IListeningRule
{
    bool RemovePublishRequest(MovingAverageTimeWeightedPublishRequestState toRemove);
}

public class LiveShortPeriodMovingAveragePublisherRule : PriceListenerIndicatorRule<PQLevel1Quote>, ITimeWeightedMovingAveragePublisherRule
{
    private readonly CalculateMovingAverageOptions defaultCalculateMovingAverageOptions;

    private readonly TimeSpan hardLimitTimeSpan;

    private readonly PricingIndicatorId indicatorSourceTickerId;

    private readonly LiveShortPeriodMovingAveragePublishParams  movingAverageParams;
    private readonly IDoublyLinkedList<IValidRangeBidAskPeriod> periodTopOfBookChain = new DoublyLinkedList<IValidRangeBidAskPeriod>();

    private readonly List<MovingAverageTimeWeightedPublishRequestState> publishRequests;

    private readonly RequestExceedTimeRangeOptions requestExceedsTimeRangeOptions;

    private readonly TickGapOptions tickGapOptions;

    private DiscreetTimePeriod broadcastPublishInterval;

    private ITimerUpdate? calculateAveragesIntervalTimer;

    private TimeSpan currentTicksValidTimeSpan = TimeSpan.Zero;

    private IChannelLimitedEventFactory<PQLevel1Quote>? historicalQuotesChannel;

    private TimeSpan liveTicksTimeSpan;

    private TaskCompletionSource<int> quotesRetrieved = null!;

    private ISubscription? recentPeriodRequestSubscription;

    private bool retrievingHistoricalPrices = true;

    private IDoublyLinkedList<IValidRangeBidAskPeriod>? startupCachePrices = new DoublyLinkedList<IValidRangeBidAskPeriod>();

    private ITimerUpdate? trimExpiredTicksIntervalTimer;

    public LiveShortPeriodMovingAveragePublisherRule(LiveShortPeriodMovingAveragePublishParams movingAverageParams)
        : base(movingAverageParams.IndicatorSourceTickerIdentifier
             , $"MovingAveragePublisherRule_{movingAverageParams.IndicatorSourceTickerIdentifier.Ticker}_" +
               $"{movingAverageParams.IndicatorSourceTickerIdentifier.Source}")
    {
        this.movingAverageParams             = movingAverageParams;
        requestExceedsTimeRangeOptions       = movingAverageParams.TimeRangeExtensionOptions;
        tickGapOptions                       = movingAverageParams.MarkTipGapOptions;
        defaultCalculateMovingAverageOptions = movingAverageParams.DefaultCalculateMovingAverageOptions;

        indicatorSourceTickerId = movingAverageParams.IndicatorSourceTickerIdentifier;

        hardLimitTimeSpan = movingAverageParams.LimitMaxTicksInitialToHardTimeSpanRange.UpperLimit ?? TimeSpan.MaxValue;
        liveTicksTimeSpan = movingAverageParams.LimitMaxTicksInitialToHardTimeSpanRange.LowerLimit ?? hardLimitTimeSpan;

        if (movingAverageParams.InitialPeriodsToPublish != null)
            if (!AcceptRequestedTicksPeriod(movingAverageParams.InitialPeriodsToPublish.Value.PeriodsToPublish))
                throw new ArgumentException("Requested period exceeds default or hard limit limit or TimeRangeExtensionOptions " +
                                            "is not set to GrowToHardLimitTimeRange or GrowUnlimited");
        publishRequests = new List<MovingAverageTimeWeightedPublishRequestState>();
    }

    public override async ValueTask StartAsync()
    {
        startupCachePrices = Context.PooledRecycler.Borrow<DoublyLinkedList<IValidRangeBidAskPeriod>>();
        // subscribe to live prices and cache ticks
        await base.StartAsync();


        // request historical data from latest to valid period of liveTicksTimeSpan
        if (movingAverageParams.InitialPeriodsToPublish != null)
        {
            var initialPeriodsToPublish = movingAverageParams.InitialPeriodsToPublish!.Value;
            var pubParams               = initialPeriodsToPublish.PublishParams;
            if (pubParams.ResponsePublishMethod == ResponsePublishMethod.ListenerDefaultBroadcastAddress)
            {
                pubParams = new ResponsePublishParams(((SourceTickerIdentifier)indicatorSourceTickerId)
                                                      .MovingAverageTimeWeightedLiveShortPeriodPublishTemplate());
                initialPeriodsToPublish = new MovingAveragePublisherParams
                    (initialPeriodsToPublish.PublishFrequency, pubParams, initialPeriodsToPublish.CalculateMovingAverageOptions
                   , initialPeriodsToPublish.PeriodsToPublish);
            }
            publishRequests.Add
                (new MovingAverageTimeWeightedPublishRequestState
                    (this, indicatorSourceTickerId, initialPeriodsToPublish
                   , initialPeriodsToPublish.CalculateMovingAverageOptions ?? defaultCalculateMovingAverageOptions));
            await CheckRequestHistoricalTicksToCoverRequestPeriods();
        }
        retrievingHistoricalPrices = false;

        recentPeriodRequestSubscription = await this.RegisterRequestListenerAsync<MovingAveragePublisherParams, bool>
            (MovingAverageConstants.MovingAverageTimeWeightedLiveShortPeriodRequest(indicatorSourceTickerId), ReceivePublishRequestHandler);
        if (movingAverageParams.InitialPeriodsToPublish != null)
        {
            broadcastPublishInterval = movingAverageParams.InitialPeriodsToPublish!.Value.PublishFrequency.PublishInterval;
            if (broadcastPublishInterval.IsUncommonTimeSpan())
                calculateAveragesIntervalTimer = Timer.RunEvery(broadcastPublishInterval.TimeSpan, CalculateAndPublishMovingAverages);
            if (broadcastPublishInterval.IsTimeBoundaryPeriod())
                calculateAveragesIntervalTimer = Timer.RunAt
                    (broadcastPublishInterval.Period.PeriodEnd(TimeContext.UtcNow), StartTimeSeriesPeriodInterval);
        }

        trimExpiredTicksIntervalTimer = Timer.RunEvery(broadcastPublishInterval.AveragePeriodTimeSpan(), TrimExpiredTicks);
    }

    public override async ValueTask StopAsync()
    {
        calculateAveragesIntervalTimer?.Cancel();
        trimExpiredTicksIntervalTimer?.Cancel();
        await recentPeriodRequestSubscription.NullSafeUnsubscribe();

        await base.StopAsync();
    }

    public bool RemovePublishRequest(MovingAverageTimeWeightedPublishRequestState toRemove) => publishRequests.Remove(toRemove);

    private async ValueTask<bool> ReceivePublishRequestHandler(IBusMessage<MovingAveragePublisherParams> publishRequestMsg)
    {
        var movingAvgPubReq = publishRequestMsg.Payload.Body();
        var acceptRequest   = AcceptRequestedTicksPeriod(movingAvgPubReq.PeriodsToPublish);
        {
            publishRequests.Add
                (new MovingAverageTimeWeightedPublishRequestState
                    (this, indicatorSourceTickerId, movingAvgPubReq
                   , movingAvgPubReq.CalculateMovingAverageOptions ?? defaultCalculateMovingAverageOptions));
            await CheckRequestHistoricalTicksToCoverRequestPeriods();
        }
        return acceptRequest;
    }

    public async ValueTask CheckRequestHistoricalTicksToCoverRequestPeriods()
    {
        var now = TimeContext.UtcNow;

        var movingAverageRequiredTicksTime = publishRequests.Select(map => map.EarliestTime(now)).Min();

        var earliestTick = periodTopOfBookChain.Head?.AtTime;
        if (earliestTick == null || earliestTick > movingAverageRequiredTicksTime.Add(TimeSpan.FromSeconds(10)))
        {
            historicalQuotesChannel = this.CreateChannelFactory<PQLevel1Quote>
                (ReceiveHistoricalQuote, new LimitedBlockingRecycler(200, Context.PooledRecycler));
            var channelRequest = historicalQuotesChannel.ToChannelPublishRequest(-1, 200);
            var request = channelRequest.ToHistoricalQuotesRequest
                (indicatorSourceTickerId, null, true);
            quotesRetrieved = new TaskCompletionSource<int>();
            var shouldWait = await this.RequestAsync<HistoricalQuotesRequest<PQLevel1Quote>, bool>(request.RequestAddress, request);
            if (shouldWait) await quotesRetrieved.Task;
        }
    }

    private bool AcceptRequestedTicksPeriod(MovingAverageOffset[] movingAveragePeriods)
    {
        foreach (var averageOffset in movingAveragePeriods)
        {
            var requiredSpan = TimeSpanFromCurrent(averageOffset);
            if (liveTicksTimeSpan < requiredSpan)
                switch (requestExceedsTimeRangeOptions)
                {
                    case RequestExceedTimeRangeOptions.GrowUnlimited: liveTicksTimeSpan = requiredSpan; break;
                    case RequestExceedTimeRangeOptions.GrowToHardLimitTimeRange when requiredSpan <= hardLimitTimeSpan:
                        liveTicksTimeSpan = requiredSpan;
                        break;
                    case RequestExceedTimeRangeOptions.GrowToHardLimitTimeRange
                        when requiredSpan > hardLimitTimeSpan:
                        return false;
                    default: return false;
                }
        }
        return true;
    }

    private TimeSpan TimeSpanFromCurrent(MovingAverageOffset movingAveragePeriods)
    {
        var now = TimeContext.UtcNow;

        var earliestTime = movingAveragePeriods.WallClockPeriodTimeRange(now).FromTime;
        return now - earliestTime;
    }

    public async ValueTask StartTimeSeriesPeriodInterval()
    {
        calculateAveragesIntervalTimer = Timer.RunEvery
            (broadcastPublishInterval.Period.AveragePeriodTimeSpan(), CalculateAndPublishMovingAverages);
        await CalculateAndPublishMovingAverages();
    }

    public void TrimExpiredTicks(IScheduleActualTime? scheduleActualTime)
    {
        if (!retrievingHistoricalPrices)
        {
            var now         = scheduleActualTime?.ScheduleTime ?? TimeContext.UtcNow;
            var currentTick = periodTopOfBookChain.Tail; // go backwards from current time
            if (currentTick == null) return;
            var remainingValidTime = liveTicksTimeSpan;
            var toDeduct           = currentTick.ValidTo.Min(now) - currentTick.ValidFrom.Min(now);
            while (currentTick != null)
            {
                var prevTick = currentTick.Previous;
                if (remainingValidTime > TimeSpan.Zero)
                {
                    remainingValidTime -= toDeduct;
                    if (prevTick != null) toDeduct = prevTick.ValidTo - prevTick.ValidFrom;
                }
                else
                {
                    periodTopOfBookChain.Remove(currentTick);
                    currentTick.DecrementRefCount();
                }
                currentTick = prevTick;
            }
        }
    }

    public async ValueTask CalculateAndPublishMovingAverages(IScheduleActualTime? scheduleActualTime = null)
    {
        if (!retrievingHistoricalPrices)
            foreach (var request in publishRequests)
            {
                var now             = scheduleActualTime?.ScheduleTime ?? TimeContext.UtcNow;
                var nextPublishTime = request.NextPublishDateTime ?? now;
                if ((scheduleActualTime?.ScheduleTime ?? now) < nextPublishTime) continue;
                while (now >= nextPublishTime)
                {
                    await request.CalculateMovingAverageAndPublish(periodTopOfBookChain, nextPublishTime);
                    nextPublishTime = request.NextPublishDateTime ?? TimeContext.UtcNow;
                }
            }
    }

    private bool ReceiveHistoricalQuote(ChannelEvent<PQLevel1Quote> channelEvent)
    {
        if (channelEvent.IsLastEvent)
        {
            MergeCachedLivePricesWithHistorical();
            quotesRetrieved.SetResult(0);
            return false;
        }
        var bidAsk = channelEvent.Event.ToValidRangeBidAskPeriod(Context.PooledRecycler);
        periodTopOfBookChain.AddFirst(bidAsk); // results are in reverse chronological order
        CheckGapsWithNextAndMarkTickQuality(bidAsk);
        return currentTicksValidTimeSpan < liveTicksTimeSpan;
    }

    private void MergeCachedLivePricesWithHistorical()
    {
        var latestQuote   = periodTopOfBookChain.Tail;
        var currentCached = startupCachePrices?.Head;
        while (currentCached != null)
        {
            var next = currentCached.Next;
            if (latestQuote == null || latestQuote.AtTime < currentCached.AtTime)
            {
                startupCachePrices!.Remove(currentCached);
                periodTopOfBookChain.AddLast(currentCached);
                latestQuote = currentCached;
            }
            currentCached = next;
        }
        startupCachePrices?.DecrementRefCount();
        retrievingHistoricalPrices = false;
    }

    protected override async ValueTask ReceiveQuoteHandler(PQLevel1Quote l1Quote)
    {
        var bidAsk = l1Quote.ToValidRangeBidAskPeriod(Context.PooledRecycler);
        if (retrievingHistoricalPrices)
            startupCachePrices?.AddLast(bidAsk);
        else
            periodTopOfBookChain.AddLast(bidAsk);
        CheckGapsWithPreviousAndMarkTickQuality(bidAsk);
        foreach (var request in publishRequests)
            if (!retrievingHistoricalPrices && request.PublishInterval.IsTickPeriod())
                await request.CalculateMovingAverageAndPublish(periodTopOfBookChain, TimeContext.UtcNow);
    }

    private void CheckGapsWithPreviousAndMarkTickQuality(ValidRangeBidAskPeriod bidAsk)
    {
        if (bidAsk.Previous != null)
        {
            var timeBetween = bidAsk.AtTime - bidAsk.Previous.AtTime;
            if (timeBetween > tickGapOptions.DataGapTimeSpan && timeBetween < tickGapOptions.MarketDayCloseTimeSpan)
            {
                bidAsk.SweepStartOfDataGap = false;
                bidAsk.SweepEndOfDataGap   = true;

                bidAsk.Previous.SweepStartOfDataGap = true;
            }
            else if (timeBetween >= tickGapOptions.MarketDayCloseTimeSpan && timeBetween < tickGapOptions.MarketWeekCloseTimeSpan)
            {
                bidAsk.SweepEndOfMarketDay   = false;
                bidAsk.SweepStartOfMarketDay = true;

                bidAsk.Previous.SweepEndOfMarketDay = true;
            }
            else if (timeBetween >= tickGapOptions.MarketWeekCloseTimeSpan)
            {
                bidAsk.SweepEndOfMarketDay    = false;
                bidAsk.SweepEndOfMarketWeek   = false;
                bidAsk.SweepStartOfMarketDay  = true;
                bidAsk.SweepStartOfMarketWeek = true;

                bidAsk.Previous.SweepEndOfMarketWeek = true;
                bidAsk.Previous.SweepEndOfMarketDay  = true;
            }
            else if (bidAsk.Previous.HasValidPeriod())
            {
                currentTicksValidTimeSpan += bidAsk.Previous.ValidTo.Min(bidAsk.AtTime) - bidAsk.Previous.ValidFrom.Max(bidAsk.Previous.AtTime);
            }
        }
        if (bidAsk.BidPrice != 0 && bidAsk.AskPrice != 0) return;
        if (bidAsk.Previous != null && bidAsk.Previous.BidPrice != 0 && bidAsk.Previous.AskPrice != 0)
            bidAsk.UsePreviousValues = true;
        else
            bidAsk.UseNextValues = true;
    }

    private void CheckGapsWithNextAndMarkTickQuality(ValidRangeBidAskPeriod bidAsk)
    {
        if (bidAsk.Next != null)
        {
            var timeBetween = bidAsk.Next.AtTime - bidAsk.AtTime;
            if (timeBetween > tickGapOptions.DataGapTimeSpan && timeBetween < tickGapOptions.MarketDayCloseTimeSpan)
            {
                bidAsk.SweepEndOfDataGap   = false;
                bidAsk.SweepStartOfDataGap = true;

                bidAsk.Next.SweepEndOfDataGap = true;
            }
            else if (timeBetween >= tickGapOptions.MarketDayCloseTimeSpan && timeBetween < tickGapOptions.MarketWeekCloseTimeSpan)
            {
                bidAsk.SweepStartOfMarketDay = false;
                bidAsk.SweepEndOfMarketDay   = true;

                bidAsk.Next.SweepStartOfMarketDay = true;
            }
            else if (timeBetween >= tickGapOptions.MarketWeekCloseTimeSpan)
            {
                bidAsk.SweepStartOfMarketDay  = false;
                bidAsk.SweepStartOfMarketWeek = false;
                bidAsk.SweepEndOfMarketDay    = true;
                bidAsk.SweepEndOfMarketWeek   = true;

                bidAsk.Next.SweepStartOfMarketDay  = true;
                bidAsk.Next.SweepStartOfMarketWeek = true;
            }
            else if (bidAsk.HasValidPeriod())
            {
                currentTicksValidTimeSpan += bidAsk.ValidTo.Min(bidAsk.Next.AtTime) - bidAsk.ValidFrom.Max(bidAsk.AtTime);
            }
        }
        if (bidAsk.BidPrice != 0 && bidAsk.AskPrice != 0) return;
        if (bidAsk.Next != null && bidAsk.Next.BidPrice != 0 && bidAsk.Next.AskPrice != 0)
            bidAsk.UseNextValues = true;
        else
            bidAsk.UsePreviousValues = true;
    }
}
