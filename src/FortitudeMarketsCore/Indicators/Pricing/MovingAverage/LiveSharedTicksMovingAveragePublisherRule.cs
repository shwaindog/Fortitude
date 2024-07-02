// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.BusMessaging.Routing.Channel;
using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;
using FortitudeCommon.Chronometry;
using FortitudeCommon.Chronometry.Timers;
using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.DataStructures.Memory;
using FortitudeIO.TimeSeries;
using FortitudeMarketsApi.Pricing;
using FortitudeMarketsCore.Pricing;
using FortitudeMarketsCore.Pricing.PQ.Converters;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;
using FortitudeMarketsCore.Pricing.PQ.TimeSeries.BusRules;

#endregion

namespace FortitudeMarketsCore.Indicators.Pricing.MovingAverage;

public class LiveSharedTicksMovingAveragePublisherRule : PriceListenerIndicatorRule<PQLevel1Quote>
{
    private readonly MovingAveragePublisherParams      movingAverageParams;
    private readonly IDoublyLinkedList<IBidAskInstant> periodTopOfBookChain = new DoublyLinkedList<IBidAskInstant>();

    private readonly IDoublyLinkedList<IBidAskInstant> startupCachePrices = new DoublyLinkedList<IBidAskInstant>();

    private List<MovingAverageCalculationState>         averagesToCalculate;
    private IChannelLimitedEventFactory<PQLevel1Quote>? historicalQuotesChannel;

    private ITimerUpdate? intervalTimer;

    private DateTime? lastPublishDateTime;
    private int       queueTargetSpecificTimeCountDown;

    private bool retrievingHistoricalPrices = true;

    private uint sequenceNumber;

    public LiveSharedTicksMovingAveragePublisherRule(MovingAveragePublisherParams movingAverageParams)
        : base(movingAverageParams.SourceTickerId
             , $"MovingAveragePublisherRule_{movingAverageParams.SourceTickerId.Ticker}_{movingAverageParams.SourceTickerId.Source}")
    {
        this.movingAverageParams = movingAverageParams;
        averagesToCalculate = (movingAverageParams.PeriodsToPublish ?? Enumerable.Empty<MovingAverageParams>())
                              .Concat(movingAverageParams.BatchPeriodsToPublish?
                                          .SelectMany(bppi => bppi.Flatten()) ?? Enumerable.Empty<MovingAverageParams>())
                              .Select(map => new MovingAverageCalculationState(map, movingAverageParams.PublishAsSourceId))
                              .ToList();
    }

    public override async ValueTask StartAsync()
    {
        // subscribe to live prices and cache ticks
        await base.StartAsync();

        // request historical data to latest
        historicalQuotesChannel = this.CreateChannelFactory<PQLevel1Quote>(ReceiveHistoricalQuote, new LimitedBlockingRecycler(200));
        var channelRequest = historicalQuotesChannel.ToChannelPublishRequest(-1, 50);
        var now            = DateTime.UtcNow;
        var earliestTime   = averagesToCalculate.Select(map => map.EarliestTime(now)).Min();
        var request        = channelRequest.ToHistoricalQuotesRequest(movingAverageParams.SourceTickerId, new UnboundedTimeRange(earliestTime, null));

        var retrieving = await this.RequestAsync<HistoricalQuotesRequest<PQLevel1Quote>, bool>(request.RequestAddress, request);
        if (!retrieving) retrievingHistoricalPrices = false;
        if (movingAverageParams.PublishInterval.PriceIndicatorPublishType == PriceIndicatorPublishType.SetTimeSpan)
            intervalTimer = Timer.RunEvery(movingAverageParams.PublishInterval.PublishTimeSpan!.Value, CalculateAndPublishMovingAverages);
        if (movingAverageParams.PublishInterval.PriceIndicatorPublishType == PriceIndicatorPublishType.TimeSeriesPeriod)
            Timer.RunAt(movingAverageParams.PublishInterval.PublishPeriod!.Value.PeriodEnd(DateTime.UtcNow), StartTimeSeriesPeriodInterval);
    }

    public void StartTimeSeriesPeriodInterval()
    {
        intervalTimer = Timer.RunEvery(movingAverageParams.PublishInterval.PublishPeriod!.Value.AveragePeriodTimeSpan()
                                     , CalculateAndPublishMovingAverages);
        CalculateAndPublishMovingAverages();
    }

    public void CalculateAndPublishMovingAverages()
    {
        if (!retrievingHistoricalPrices)
        {
            var nextPublishTime = lastPublishDateTime == null ? movingAverageParams.NextPublishTime(lastPublishDateTime!.Value) : DateTime.UtcNow;
            if (DateTime.UtcNow > nextPublishTime)
            {
                CalculateAndRepublish(nextPublishTime);
                lastPublishDateTime              = nextPublishTime;
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
        periodTopOfBookChain.AddLast(channelEvent.Event.ToBidAsk(Context.PooledRecycler));
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
        var bidAsk  = l1Quote.ToBidAsk(Context.PooledRecycler);
        if (retrievingHistoricalPrices)
            startupCachePrices.AddLast(bidAsk);
        else
            periodTopOfBookChain.AddLast(bidAsk);
        if (!retrievingHistoricalPrices && movingAverageParams.PublishInterval.PriceIndicatorPublishType == PriceIndicatorPublishType.Tick)
            CalculateAndRepublish(bidAsk.AtTime);
        return ValueTask.CompletedTask;
    }

    private void CalculateAndRepublish(DateTime publishTime)
    {
        foreach (var movingAvgCalc in averagesToCalculate)
        {
            var movingAverage = movingAvgCalc.Calculate(periodTopOfBookChain, publishTime, sequenceNumber);
        }
        sequenceNumber++;
    }
}
