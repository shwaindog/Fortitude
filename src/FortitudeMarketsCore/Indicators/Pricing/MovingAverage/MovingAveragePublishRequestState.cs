// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.Rules;
using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeIO.TimeSeries;
using FortitudeMarketsApi.Indicators.Pricing;
using FortitudeMarketsApi.Pricing;

#endregion

namespace FortitudeMarketsCore.Indicators.Pricing.MovingAverage;

public class MovingAveragePublishRequestState
{
    private readonly List<MovingAverageCalculationState> averagesToCalculate;

    private readonly MovingAverageBatchPublisherParams? batchMovingAverageParams;

    private readonly IListeningRule containingRule;

    private readonly long indicatorSourceTickerId;
    private readonly bool isBatchPublisher;

    private readonly MovingAveragePublisherParams? singleMoveAverageParams;

    public MovingAveragePublishRequestState
    (IListeningRule containingRule, long indicatorSourceTickerId, DateTime earliestQuoteTime, MovingAveragePublisherParams singlePublisherParams
      , TimeSpan ignoreTickGapsGreaterThan)
    {
        this.containingRule          = containingRule;
        this.indicatorSourceTickerId = indicatorSourceTickerId;

        singleMoveAverageParams = singlePublisherParams;

        isBatchPublisher = false;

        averagesToCalculate =
            singlePublisherParams.PeriodsToPublish
                                 .Select(movingAverageOffset =>
                                             CreateMovingAverageState(movingAverageOffset, earliestQuoteTime, ignoreTickGapsGreaterThan)).ToList();
    }

    public MovingAveragePublishRequestState
    (IListeningRule containingRule, long indicatorSourceTickerId, DateTime earliestQuoteTime, MovingAverageBatchPublisherParams batchPublisherParams
      , TimeSpan ignoreTickGapsGreaterThan)
    {
        this.containingRule          = containingRule;
        this.indicatorSourceTickerId = indicatorSourceTickerId;

        batchMovingAverageParams = batchPublisherParams;

        isBatchPublisher = true;

        averagesToCalculate =
            batchPublisherParams
                .BatchPeriodsToPublish.Flatten()
                .Select(offset => CreateMovingAverageState(offset, earliestQuoteTime, ignoreTickGapsGreaterThan)).ToList();
    }

    public DateTime? NextPublishDateTime { get; private set; }

    public TimePeriod PublishInterval =>
        singleMoveAverageParams?.PublishFrequency.PublishInterval ?? batchMovingAverageParams!.Value.PublishFrequency.PublishInterval;


    public DateTime EarliestTime(DateTime asOfTime)
    {
        return averagesToCalculate.Min(macs => macs.EarliestTime(asOfTime));
    }

    public void CalculateMovingAverageAndPublish(IDoublyLinkedList<IBidAskInstant> timeOrderedPairs, DateTime atTime)
    {
        if (isBatchPublisher)
        {
            var toPublish = CalculateBatch(timeOrderedPairs, atTime);
            PublishBatchBidAskInstantChain(toPublish);
        }
        else
        {
            foreach (var movingAverageCalculationState in averagesToCalculate)
            {
                var movingAverageInstant = CalculateSingleMovingAverage(movingAverageCalculationState, timeOrderedPairs, atTime);
                PublishSingleMovingAverage(movingAverageInstant);
            }
        }
        NextPublishDateTime = atTime + PublishInterval.AveragePeriodTimeSpan();
    }

    public void PublishBatchBidAskInstantChain(SameIndicatorBidAskInstantChain bidAskChain) { }

    public void PublishSingleMovingAverage(IIndicatorValidRangeBidAskPeriod validRangeBidAskPeriod) { }

    public MovingAverageCalculationState CreateMovingAverageState
        (MovingAverageOffset movingAverageOffset, DateTime earliestQuoteTime, TimeSpan ignoreTickGapsGreaterThan) =>
        new(movingAverageOffset, earliestQuoteTime, ignoreTickGapsGreaterThan);

    public SameIndicatorBidAskInstantChain CalculateBatch(IDoublyLinkedList<IBidAskInstant> timeOrderedPairs, DateTime atTime)
    {
        var recycler    = containingRule.Context.PooledRecycler;
        var resultChain = recycler.Borrow<SameIndicatorBidAskInstantChain>();
        resultChain.Configure(indicatorSourceTickerId, batchMovingAverageParams!.Value.BatchPeriodsToPublish.CoveringPeriod);
        var batchCount = 0u;
        foreach (var movingAverageState in averagesToCalculate)
        {
            var movingAverageInstant = movingAverageState.Calculate(timeOrderedPairs, atTime, batchCount++);
            resultChain.AddLast(movingAverageInstant.ToBidAskInstant(recycler));
        }
        return resultChain;
    }

    public IIndicatorValidRangeBidAskPeriod CalculateSingleMovingAverage
        (MovingAverageCalculationState movingAverageCalculationState, IDoublyLinkedList<IBidAskInstant> timeOrderedPairs, DateTime atTime)
    {
        var recycler             = containingRule.Context.PooledRecycler;
        var movingAverageInstant = movingAverageCalculationState.Calculate(timeOrderedPairs, atTime, 0);
        var bidAskInstant        = recycler.Borrow<IndicatorValidRangeBidAskPeriod>();
        bidAskInstant.Configure(indicatorSourceTickerId, movingAverageInstant, movingAverageCalculationState.AveragePeriod);
        return bidAskInstant;
    }
}
