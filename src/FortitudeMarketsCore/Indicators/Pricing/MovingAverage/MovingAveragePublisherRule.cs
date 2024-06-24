// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.BusMessaging.Routing.Channel;
using FortitudeBusRules.Messages;
using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.DataStructures.Memory;
using FortitudeIO.TimeSeries;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing;
using FortitudeMarketsCore.Pricing;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;

#endregion

namespace FortitudeMarketsCore.Indicators.Pricing.MovingAverage;

public struct MovingAverageParams
{
    public MovingAverageParams(TimeSpan periodTimeSpan, TimeSpan? latestOffsetTimeSpan = null)
    {
        PeriodTimeSpan       = periodTimeSpan;
        LatestOffsetTimeSpan = latestOffsetTimeSpan ?? TimeSpan.Zero;
    }

    public MovingAverageParams(TimeSpan periodTimeSpan, TimeSeriesPeriod? latestPeriodOffset, int latestOffsetNumberOfPeriods = 1)
    {
        PeriodTimeSpan               = periodTimeSpan;
        LatestOffsetTimeSeriesPeriod = latestPeriodOffset ?? FortitudeIO.TimeSeries.TimeSeriesPeriod.None;
        LatestOffsetNumberOfPeriods  = latestOffsetNumberOfPeriods;
    }

    public MovingAverageParams(TimeSeriesPeriod timeSeriesPeriod, TimeSeriesPeriod? latestPeriodOffset, int latestOffsetNumberOfPeriods = 1)
    {
        TimeSeriesPeriod             = timeSeriesPeriod;
        LatestOffsetTimeSeriesPeriod = latestPeriodOffset ?? FortitudeIO.TimeSeries.TimeSeriesPeriod.None;
        LatestOffsetNumberOfPeriods  = latestOffsetNumberOfPeriods;
    }

    public MovingAverageParams(TimeSeriesPeriod timeSeriesPeriod, TimeSpan? latestOffsetTimeSpan = null)
    {
        TimeSeriesPeriod     = timeSeriesPeriod;
        LatestOffsetTimeSpan = latestOffsetTimeSpan ?? TimeSpan.Zero;
    }

    public TimeSpan?         PeriodTimeSpan               { get; }
    public TimeSeriesPeriod? TimeSeriesPeriod             { get; }
    public TimeSpan          LatestOffsetTimeSpan         { get; }
    public TimeSeriesPeriod  LatestOffsetTimeSeriesPeriod { get; }
    public int               LatestOffsetNumberOfPeriods  { get; }
}

public static class MovingAverageParamsExtensions
{
    public static IEnumerable<MovingAverageParams> Flatten(this BatchPricePublishInterval batchInterval)
    {
        var currentTimeSpan    = TimeSpan.Zero;
        var periodsOffset      = 0;
        var offsetsUseTimeSpan = batchInterval.BatchPublishEntrySeparation.PriceIndicatorPublishType == PriceIndicatorPublishType.SetTimeSpan;
        for (var i = 0; i < batchInterval.BatchCount; i++)
        {
            if (batchInterval.EntryRange.PriceIndicatorPublishType == PriceIndicatorPublishType.SetTimeSpan)
            {
                var movingAvgParam = offsetsUseTimeSpan
                    ? new MovingAverageParams(batchInterval.EntryRange.PublishTimeSpan!.Value, currentTimeSpan)
                    : new MovingAverageParams(batchInterval.EntryRange.PublishTimeSpan!.Value, batchInterval.BatchPublishEntrySeparation.PublishPeriod
                                            , periodsOffset++);
                yield return movingAvgParam;
            }
            if (batchInterval.EntryRange.PriceIndicatorPublishType == PriceIndicatorPublishType.TimeSeriesPeriod)
            {
                var movingAvgParam = offsetsUseTimeSpan
                    ? new MovingAverageParams(batchInterval.EntryRange.PublishPeriod!.Value, currentTimeSpan)
                    : new MovingAverageParams(batchInterval.EntryRange.PublishPeriod!.Value, batchInterval.BatchPublishEntrySeparation.PublishPeriod
                                            , periodsOffset++);
                yield return movingAvgParam;
            }
            if (batchInterval.BatchPublishEntrySeparation.PriceIndicatorPublishType == PriceIndicatorPublishType.SetTimeSpan)
                currentTimeSpan += batchInterval.BatchPublishEntrySeparation.PublishTimeSpan!.Value;
        }
    }
}

public struct MovingAveragePublisherParams
{
    public MovingAveragePublisherParams
    (ushort publishSourceId, ISourceTickerIdentifier sourceTickerId, PricePublishInterval publishInterval
      , params MovingAverageParams[] periodsToPublish)
    {
        PublishAsSourceId = publishSourceId;
        SourceTickerId    = sourceTickerId;
        PublishInterval   = publishInterval;
        PeriodsToPublish  = periodsToPublish;
    }

    public MovingAveragePublisherParams
    (ushort publishSourceId, ISourceTickerIdentifier sourceTickerId, PricePublishInterval publishInterval
      , params BatchPricePublishInterval[] batchPeriodsToPublish)
    {
        PublishAsSourceId     = publishSourceId;
        SourceTickerId        = sourceTickerId;
        PublishInterval       = publishInterval;
        BatchPeriodsToPublish = batchPeriodsToPublish;
    }

    public ISourceTickerIdentifier SourceTickerId { get; }

    public ushort PublishAsSourceId { get; }

    public MovingAverageParams[]? PeriodsToPublish { get; }

    public BatchPricePublishInterval[]? BatchPeriodsToPublish { get; }

    public PricePublishInterval PublishInterval { get; }
}

public class MovingAveragePublisherRule : PriceListenerIndicatorRule<PQLevel1Quote>
{
    private readonly MovingAveragePublisherParams       movingAverageParams;
    private readonly IDoublyLinkedList<IBidAskPairNode> periodTopOfBookChain = new DoublyLinkedList<IBidAskPairNode>();

    private readonly IDoublyLinkedList<IBidAskPairNode> startupCachePrices = new DoublyLinkedList<IBidAskPairNode>();

    private List<MovingAverageCalculationState>                averagesToCalculate;
    private ChannelWrappingLimitedEventFactory<PQLevel1Quote>? historicalQuotes;


    private bool retrievingHistoricalPrices = true;

    private uint sequenceNumber = 0;

    public MovingAveragePublisherRule(MovingAveragePublisherParams movingAverageParams)
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
        historicalQuotes =
            new ChannelWrappingLimitedEventFactory<PQLevel1Quote>
                (new InterQueueChannel<PQLevel1Quote>(this, ReceiveHistoricalQuote), new LimitedBlockingRecycler(200));

        // build chained list

        // merge live ticks

        // set publish timer if applicable

        // calculate and publish
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
        throw new NotImplementedException();
        retrievingHistoricalPrices = false;
    }

    protected override void ReceiveQuoteHandler(IBusMessage<PQLevel1Quote> priceQuoteMessage)
    {
        var l1Quote = priceQuoteMessage.Payload.Body();
        var bidAsk  = l1Quote.ToBidAsk(Context.PooledRecycler);
        if (retrievingHistoricalPrices)
            startupCachePrices.AddLast(bidAsk);
        else
            periodTopOfBookChain.AddLast(bidAsk);
        if (!retrievingHistoricalPrices && movingAverageParams.PublishInterval.PriceIndicatorPublishType == PriceIndicatorPublishType.Tick)
            CalculateAndRepublish(bidAsk.AtTime);
    }

    private void CalculateAndRepublish(DateTime publishTime)
    {
        foreach (var movingAvgCalc in averagesToCalculate)
        {
            var movingAverage = movingAvgCalc.Calculate(periodTopOfBookChain, publishTime, sequenceNumber);
        }
        sequenceNumber++;
    }


    private class MovingAverageCalculationState
    {
        private readonly MovingAverageParams movingAvg;
        private readonly ushort              publishAsSource;
        private          IBidAskPairNode?    lastCalcEarliest;
        private          IBidAskPairNode?    lastCalcLatest;

        private DateTime? lastCalcTimeEarliest;
        private DateTime? lastCalcTimeLatest;

        private BidAskPair runningTotalTimeWeightedInMs;

        public MovingAverageCalculationState(MovingAverageParams movingAvg, ushort publishAsSource)
        {
            this.movingAvg       = movingAvg;
            this.publishAsSource = publishAsSource;
        }

        public BidAskPair Calculate(IDoublyLinkedList<IBidAskPairNode> timeOrderedPairs, DateTime atTime, uint batchNumber)
        {
            var calcStartTime = atTime;
            var calcEndTime   = atTime;
            if (movingAvg.TimeSeriesPeriod != null)
            {
                calcStartTime = movingAvg.TimeSeriesPeriod.Value.ContainingPeriodBoundaryStart(atTime);
                for (var i = 0; i <= movingAvg.LatestOffsetNumberOfPeriods; i++)
                    calcStartTime = movingAvg.LatestOffsetTimeSeriesPeriod.PreviousPeriodStart(calcStartTime);
                calcEndTime = movingAvg.TimeSeriesPeriod.Value.PeriodEnd(calcStartTime);
            }
            else if (movingAvg.PeriodTimeSpan != null)
            {
                calcStartTime = atTime.Subtract(movingAvg.PeriodTimeSpan!.Value);
                for (var i = 0; i < movingAvg.LatestOffsetNumberOfPeriods; i++)
                    calcStartTime = calcStartTime.Subtract(movingAvg.LatestOffsetTimeSpan);
                calcEndTime = calcStartTime.Add(movingAvg.PeriodTimeSpan!.Value);
            }
            var startDeltaFrom = lastCalcTimeEarliest ?? calcStartTime;
            var endDeltaFrom   = lastCalcTimeLatest ?? calcEndTime;

            var justCalcDeltaPeriods = true;
            var currentNode          = lastCalcEarliest;
            var nextNode             = currentNode?.Next;
            if (currentNode == null)
            {
                runningTotalTimeWeightedInMs = new BidAskPair();

                justCalcDeltaPeriods = false;

                currentNode = timeOrderedPairs.Head;
                nextNode    = currentNode?.Next;
                while (nextNode != null && nextNode.AtTime < calcStartTime)
                {
                    currentNode = nextNode;
                    nextNode    = nextNode.Next;
                }
                nextNode = currentNode?.Next;
            }
            while (currentNode != null && currentNode.AtTime < calcEndTime)
            {
                if (currentNode.AtTime <= calcStartTime && (nextNode?.AtTime ?? calcEndTime) > calcStartTime)
                {
                    lastCalcEarliest = currentNode;
                }
                if (justCalcDeltaPeriods
                 && (currentNode.AtTime < startDeltaFrom && (nextNode?.AtTime ?? calcEndTime) > startDeltaFrom)
                 || (currentNode.AtTime > startDeltaFrom && currentNode.AtTime < calcStartTime))
                {
                    var sliceStartTicks = Math.Max(currentNode.AtTime.Ticks, startDeltaFrom.Ticks);
                    var sliceEndTicks   = Math.Min(nextNode?.AtTime.Ticks ?? calcEndTime.Ticks, calcEndTime.Ticks);

                    var sliceMs = (decimal)TimeSpan.FromTicks(sliceEndTicks - sliceStartTicks).TotalMilliseconds;

                    var bidTimeWeightedCurrentPrice = currentNode.BidPrice * sliceMs;
                    var askTimeWeightedCurrentPrice = currentNode.AskPrice * sliceMs;

                    runningTotalTimeWeightedInMs.BidPrice -= bidTimeWeightedCurrentPrice;
                    runningTotalTimeWeightedInMs.AskPrice -= askTimeWeightedCurrentPrice;
                }
                else if (justCalcDeltaPeriods
                      && (currentNode.AtTime < endDeltaFrom && (nextNode?.AtTime ?? calcEndTime) > endDeltaFrom)
                      || (currentNode.AtTime > endDeltaFrom && currentNode.AtTime < calcEndTime))
                {
                    var sliceStartTicks = Math.Min(nextNode?.AtTime.Ticks ?? calcEndTime.Ticks, endDeltaFrom.Ticks);
                    var sliceEndTicks   = Math.Max(sliceStartTicks, Math.Min(nextNode?.AtTime.Ticks ?? calcEndTime.Ticks, calcEndTime.Ticks));

                    var sliceMs = (decimal)TimeSpan.FromTicks(sliceEndTicks - sliceStartTicks).TotalMilliseconds;

                    var bidTimeWeightedCurrentPrice = currentNode.BidPrice * sliceMs;
                    var askTimeWeightedCurrentPrice = currentNode.AskPrice * sliceMs;

                    runningTotalTimeWeightedInMs.BidPrice += bidTimeWeightedCurrentPrice;
                    runningTotalTimeWeightedInMs.AskPrice += askTimeWeightedCurrentPrice;
                }
                else if (!justCalcDeltaPeriods
                      && ((currentNode.AtTime < calcStartTime && (nextNode?.AtTime ?? calcEndTime) > calcStartTime)
                       || (currentNode.AtTime > calcStartTime && currentNode.AtTime < calcEndTime)))
                {
                    var sliceStartTicks = Math.Max(currentNode.AtTime.Ticks, calcStartTime.Ticks);
                    var sliceEndTicks   = Math.Min(nextNode?.AtTime.Ticks ?? calcEndTime.Ticks, calcEndTime.Ticks);

                    var sliceMs = (decimal)TimeSpan.FromTicks(sliceEndTicks - sliceStartTicks).TotalMilliseconds;

                    var bidTimeWeightedCurrentPrice = currentNode.BidPrice * sliceMs;
                    var askTimeWeightedCurrentPrice = currentNode.AskPrice * sliceMs;

                    runningTotalTimeWeightedInMs.BidPrice += bidTimeWeightedCurrentPrice;
                    runningTotalTimeWeightedInMs.AskPrice += askTimeWeightedCurrentPrice;
                }
                else if (justCalcDeltaPeriods && currentNode.AtTime > calcStartTime && currentNode.AtTime < (lastCalcLatest?.AtTime ?? calcStartTime))
                {
                    currentNode = lastCalcLatest!;
                    nextNode    = currentNode.Next;
                    continue;
                }
                else if (currentNode.AtTime <= calcEndTime && (nextNode?.AtTime ?? calcEndTime) >= calcEndTime)
                {
                    lastCalcLatest = currentNode;
                }

                if (currentNode.AtTime > calcEndTime) break;

                currentNode = nextNode;
                nextNode    = currentNode?.Next;
            }
            var periodTotalMs = (decimal)(calcEndTime - calcStartTime).TotalMilliseconds;
            lastCalcTimeEarliest = calcStartTime;
            lastCalcTimeLatest   = calcEndTime;
            return new BidAskPair
                (runningTotalTimeWeightedInMs.BidPrice / periodTotalMs
               , runningTotalTimeWeightedInMs.AskPrice / periodTotalMs
               , calcEndTime, publishAsSource, batchNumber);
        }
    }
}
