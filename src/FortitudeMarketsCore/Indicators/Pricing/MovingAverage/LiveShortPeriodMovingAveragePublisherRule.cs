// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.BusMessaging.Messages.ListeningSubscriptions;
using FortitudeBusRules.BusMessaging.Routing.Channel;
using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;
using FortitudeBusRules.Rules.Common.TimeSeries;
using FortitudeCommon.Chronometry;
using FortitudeCommon.Chronometry.Timers;
using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.DataStructures.Memory;
using FortitudeIO.TimeSeries;
using FortitudeIO.TimeSeries.FileSystem;
using FortitudeMarketsApi.Indicators.Pricing;
using FortitudeMarketsApi.Pricing;
using FortitudeMarketsCore.Indicators.Pricing.Parameters;
using FortitudeMarketsCore.Pricing;
using FortitudeMarketsCore.Pricing.PQ.Converters;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;
using FortitudeMarketsCore.Pricing.PQ.TimeSeries.BusRules;

#endregion

namespace FortitudeMarketsCore.Indicators.Pricing.MovingAverage;

public class LiveShortPeriodMovingAveragePublisherRule : PriceListenerIndicatorRule<PQLevel1Quote>
{
    private readonly CalculateMovingAverageOptions defaultCalculateMovingAverageOptions;

    private readonly TimeSpan hardLimitTimeSpan;

    private readonly IndicatorSourceTickerIdentifier            indicatorSourceTickerId;
    private readonly LiveShortPeriodMovingAveragePublishParams  movingAverageParams;
    private readonly IDoublyLinkedList<IValidRangeBidAskPeriod> periodTopOfBookChain = new DoublyLinkedList<IValidRangeBidAskPeriod>();
    private readonly List<MovingAveragePublishRequestState>     publishRequests;

    private readonly RequestExceedTimeRangeOptions requestExceedsTimeRangeOptions;

    private readonly IDoublyLinkedList<IValidRangeBidAskPeriod> startupCachePrices = new DoublyLinkedList<IValidRangeBidAskPeriod>();

    private readonly TickGapOptions tickGapOptions;

    private TimePeriod broadcastPublishInterval;

    private IChannelLimitedEventFactory<PQLevel1Quote>? historicalQuotesChannel;

    private ITimerUpdate? intervalTimer;

    private TimeSpan liveTicksTimeSpan;

    private int queueTargetSpecificTimeCountDown;

    private InstrumentFileEntryInfo? quotesRepoInfo;

    private ISubscription? recentPeriodRequestSubscription;

    private bool retrievingHistoricalPrices = true;

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
        publishRequests = new List<MovingAveragePublishRequestState>();
    }

    public override async ValueTask StartAsync()
    {
        // subscribe to live prices and cache ticks
        await base.StartAsync();

        // request historical data to latest
        await GetRepositoryQuoteInfo();
        if (movingAverageParams.InitialPeriodsToPublish != null)
        {
            publishRequests.Add
                (new MovingAveragePublishRequestState
                    (this, indicatorSourceTickerId.IndicatorSourceTickerId, movingAverageParams.InitialPeriodsToPublish!.Value
                   , movingAverageParams.InitialPeriodsToPublish.Value.CalculateMovingAverageOptions ?? defaultCalculateMovingAverageOptions));
            retrievingHistoricalPrices = await CheckRequestHistoricalTicksToCoverRequestPeriods();
        }
        else
        {
            retrievingHistoricalPrices = false;
        }

        recentPeriodRequestSubscription = await this.RegisterRequestListenerAsync<MovingAveragePublisherParams, bool>
            (MovingAverageConstants.MovingAverageLiveShortPeriodRequest(indicatorSourceTickerId), ReceivePublishRequestHandler);
        if (movingAverageParams.InitialPeriodsToPublish != null)
        {
            broadcastPublishInterval = movingAverageParams.InitialPeriodsToPublish!.Value.PublishFrequency.PublishInterval;
            if (broadcastPublishInterval.IsTimeSpan())
                intervalTimer = Timer.RunEvery(broadcastPublishInterval.TimeSpan, CalculateAndPublishMovingAverages);
            if (broadcastPublishInterval.IsTimeSeriesPeriod())
                intervalTimer = Timer.RunAt
                    (broadcastPublishInterval.TimeSeriesPeriod.PeriodEnd(TimeContext.UtcNow), StartTimeSeriesPeriodInterval);
        }
    }

    public override async ValueTask StopAsync()
    {
        intervalTimer?.Cancel();
        await recentPeriodRequestSubscription.NullSafeUnsubscribe();

        await base.StopAsync();
    }

    private async ValueTask<bool> ReceivePublishRequestHandler(IBusMessage<MovingAveragePublisherParams> publishRequestMsg)
    {
        var movingAvgPubReq = publishRequestMsg.Payload.Body();
        var acceptRequest   = AcceptRequestedTicksPeriod(movingAvgPubReq.PeriodsToPublish);
        {
            publishRequests.Add
                (new MovingAveragePublishRequestState
                    (this, indicatorSourceTickerId.IndicatorSourceTickerId, movingAvgPubReq
                   , movingAvgPubReq.CalculateMovingAverageOptions ?? defaultCalculateMovingAverageOptions));
            await CheckRequestHistoricalTicksToCoverRequestPeriods();
        }
        return acceptRequest;
    }

    private async ValueTask GetRepositoryQuoteInfo()
    {
        var now       = TimeContext.UtcNow;
        var weekStart = TimeSeriesPeriod.OneWeek.ContainingPeriodBoundaryStart(now);
        var quotesRangeRequest = new TimeSeriesRepositoryInstrumentFileEntryInfoRequest
            (indicatorSourceTickerId.Ticker, indicatorSourceTickerId.Source, InstrumentType.Price, TimeSeriesPeriod.Tick
           , new UnboundedTimeRange(weekStart, null));
        var candidateInstrumentFileInfo = await this.RequestAsync<TimeSeriesRepositoryInstrumentFileEntryInfoRequest, List<InstrumentFileEntryInfo>>
            (TimeSeriesRepositoryConstants.TimeSeriesInstrumentEntryFileInfoRequestResponse, quotesRangeRequest);
        if (candidateInstrumentFileInfo.Count > 1)
            throw new Exception($"Received More than one instrument for {indicatorSourceTickerId.Ticker} type from the repository for quotes");
        if (candidateInstrumentFileInfo.Count == 1) quotesRepoInfo = candidateInstrumentFileInfo[0];
    }

    public async ValueTask<bool> CheckRequestHistoricalTicksToCoverRequestPeriods()
    {
        var now = TimeContext.UtcNow;

        var movingAverageRequiredTicksTime = publishRequests.Select(map => map.EarliestTime(now)).Min();

        var earliestTick = periodTopOfBookChain.Head?.AtTime;
        if (earliestTick == null || earliestTick > movingAverageRequiredTicksTime.Add(TimeSpan.FromSeconds(10)))
        {
            historicalQuotesChannel = this.CreateChannelFactory<PQLevel1Quote>(ReceiveHistoricalQuote, new LimitedBlockingRecycler(200));
            var channelRequest = historicalQuotesChannel.ToChannelPublishRequest(-1, 50);
            var request = channelRequest.ToHistoricalQuotesRequest
                (indicatorSourceTickerId, new UnboundedTimeRange(movingAverageRequiredTicksTime, earliestTick));

            return await this.RequestAsync<HistoricalQuotesRequest<PQLevel1Quote>, bool>(request.RequestAddress, request);
        }
        return false;
    }

    private bool AcceptRequestedTicksPeriod(MovingAverageOffset[] movingAveragePeriods)
    {
        foreach (var averageOffset in movingAveragePeriods)
        {
            var requiredSpan = TimeSpanFromCurrent(averageOffset);
            if (requiredSpan < liveTicksTimeSpan)
                switch (requestExceedsTimeRangeOptions)
                {
                    case RequestExceedTimeRangeOptions.GrowUnlimited:
                        liveTicksTimeSpan = requiredSpan;
                        break;
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

    public void StartTimeSeriesPeriodInterval()
    {
        intervalTimer = Timer.RunEvery
            (broadcastPublishInterval.TimeSeriesPeriod.AveragePeriodTimeSpan(), CalculateAndPublishMovingAverages);
        CalculateAndPublishMovingAverages();
    }

    public void CalculateAndPublishMovingAverages()
    {
        if (!retrievingHistoricalPrices)
            foreach (var request in publishRequests)
            {
                var nextPublishTime = request.NextPublishDateTime ?? TimeContext.UtcNow;
                if (TimeContext.UtcNow > nextPublishTime)
                {
                    request.CalculateMovingAverageAndPublish(periodTopOfBookChain, nextPublishTime);
                    queueTargetSpecificTimeCountDown = 1;
                }
                else if (queueTargetSpecificTimeCountDown-- <= 0)
                {
                    queueTargetSpecificTimeCountDown = ushort.MaxValue;
                    Timer.RunAt(nextPublishTime, CalculateAndPublishMovingAverages);
                }
            }
    }

    private bool ReceiveHistoricalQuote(ChannelEvent<PQLevel1Quote> channelEvent)
    {
        if (channelEvent.IsLastEvent)
        {
            MergeCachedLivePricesWithHistorical();
            return false;
        }
        var bidAsk = channelEvent.Event.ToValidRangeBidAskPeriod(Context.PooledRecycler);
        periodTopOfBookChain.AddLast(bidAsk);
        CheckGapsAndTickQuality(bidAsk);
        return true;
    }

    private void MergeCachedLivePricesWithHistorical()
    {
        var latestQuote   = periodTopOfBookChain.Tail;
        var currentCached = startupCachePrices.Head;
        while (currentCached != null)
        {
            if (latestQuote == null || latestQuote.AtTime < currentCached.AtTime)
            {
                periodTopOfBookChain.AddLast(currentCached);
                latestQuote = currentCached;
            }
            currentCached = currentCached.Next;
        }
        retrievingHistoricalPrices = false;
    }

    protected override ValueTask ReceiveQuoteHandler(IBusMessage<PQLevel1Quote> priceQuoteMessage)
    {
        var l1Quote = priceQuoteMessage.Payload.Body();
        var bidAsk  = l1Quote.ToValidRangeBidAskPeriod(Context.PooledRecycler);
        if (retrievingHistoricalPrices)
            startupCachePrices.AddLast(bidAsk);
        else
            periodTopOfBookChain.AddLast(bidAsk);
        CheckGapsAndTickQuality(bidAsk);
        foreach (var request in publishRequests)
            if (!retrievingHistoricalPrices && request.PublishInterval.IsTickPeriod())
                request.CalculateMovingAverageAndPublish(periodTopOfBookChain, TimeContext.UtcNow);
        return ValueTask.CompletedTask;
    }

    private void CheckGapsAndTickQuality(ValidRangeBidAskPeriod bidAsk)
    {
        if (bidAsk.Previous != null && bidAsk.Previous.HasValidPeriod())
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
        }
        if (bidAsk.BidPrice == 0 || bidAsk.AskPrice == 0)
        {
            if (bidAsk.Previous != null && bidAsk.Previous.BidPrice != 0 && bidAsk.Previous.AskPrice != 0)
                bidAsk.UsePreviousValues = true;
            else
                bidAsk.UseNextValues = true;
        }
    }
}
