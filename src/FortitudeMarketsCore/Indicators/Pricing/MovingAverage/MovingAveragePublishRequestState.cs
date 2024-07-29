// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.Rules;
using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeIO.TimeSeries;
using FortitudeMarketsApi.Indicators.Pricing;
using FortitudeMarketsApi.Pricing;
using FortitudeMarketsCore.Indicators.Pricing.Parameters;

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
    (IListeningRule containingRule, long indicatorSourceTickerId, MovingAveragePublisherParams singlePublisherParams
      , CalculateMovingAverageOptions calculateMovingAverageOptions)
    {
        this.containingRule          = containingRule;
        this.indicatorSourceTickerId = indicatorSourceTickerId;

        singleMoveAverageParams = singlePublisherParams;

        isBatchPublisher = false;

        averagesToCalculate =
            singlePublisherParams.PeriodsToPublish
                                 .Select(movingAverageOffset =>
                                             CreateMovingAverageState(movingAverageOffset, calculateMovingAverageOptions))
                                 .ToList();
    }

    public MovingAveragePublishRequestState
    (IListeningRule containingRule, long indicatorSourceTickerId, MovingAverageBatchPublisherParams batchPublisherParams
      , CalculateMovingAverageOptions calculateMovingAverageOptions)
    {
        this.containingRule          = containingRule;
        this.indicatorSourceTickerId = indicatorSourceTickerId;

        batchMovingAverageParams = batchPublisherParams;

        isBatchPublisher = true;

        averagesToCalculate =
            batchPublisherParams
                .BatchPeriodsToPublish.Flatten()
                .Select(offset => CreateMovingAverageState(offset, calculateMovingAverageOptions)).ToList();
    }

    public DateTime? NextPublishDateTime { get; private set; }

    public TimePeriod PublishInterval =>
        singleMoveAverageParams?.PublishFrequency.PublishInterval ?? batchMovingAverageParams!.Value.PublishFrequency.PublishInterval;


    public DateTime EarliestTime(DateTime asOfTime)
    {
        return averagesToCalculate.Min(macs => macs.EarliestTime(asOfTime));
    }

    public void CalculateMovingAverageAndPublish(IDoublyLinkedList<IValidRangeBidAskPeriod> timeOrderedPairs, DateTime atTime)
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

    public void PublishBatchBidAskInstantChain(SameIndicatorValidRangeBidAskPeriodChain bidAskChain) { }

    public void PublishSingleMovingAverage(IIndicatorValidRangeBidAskPeriod validRangeBidAskPeriod) { }

    public MovingAverageCalculationState CreateMovingAverageState
        (MovingAverageOffset movingAverageOffset, CalculateMovingAverageOptions calculateMovingAverageOptions) =>
        new(movingAverageOffset, calculateMovingAverageOptions);

    public SameIndicatorValidRangeBidAskPeriodChain CalculateBatch(IDoublyLinkedList<IValidRangeBidAskPeriod> timeOrderedPairs, DateTime atTime)
    {
        var recycler    = containingRule.Context.PooledRecycler;
        var resultChain = recycler.Borrow<SameIndicatorValidRangeBidAskPeriodChain>();
        resultChain.Configure(indicatorSourceTickerId, batchMovingAverageParams!.Value.BatchPeriodsToPublish.CoveringPeriod);
        var batchCount = 0u;
        foreach (var movingAverageState in averagesToCalculate)
        {
            var movingAverageInstant = movingAverageState.Calculate(timeOrderedPairs, atTime, batchCount++);
            resultChain.AddLast(movingAverageInstant.ToValidRangeBidAskPeriod(recycler));
        }
        return resultChain;
    }

    public IIndicatorValidRangeBidAskPeriod CalculateSingleMovingAverage
        (MovingAverageCalculationState movingAverageCalculationState, IDoublyLinkedList<IValidRangeBidAskPeriod> timeOrderedPairs, DateTime atTime)
    {
        var recycler             = containingRule.Context.PooledRecycler;
        var movingAverageInstant = movingAverageCalculationState.Calculate(timeOrderedPairs, atTime, 0);
        var bidAskInstant        = recycler.Borrow<IndicatorValidRangeBidAskPeriod>();
        bidAskInstant.Configure(indicatorSourceTickerId, movingAverageInstant, movingAverageCalculationState.AveragePeriod);
        return bidAskInstant;
    }
}
