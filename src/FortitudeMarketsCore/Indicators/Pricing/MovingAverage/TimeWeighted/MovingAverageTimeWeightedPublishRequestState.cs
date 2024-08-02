// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.BusMessaging.Routing.Channel;
using FortitudeBusRules.BusMessaging.Routing.Response;
using FortitudeBusRules.Rules;
using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeMarketsApi.Indicators.Pricing;
using FortitudeMarketsApi.Pricing;
using FortitudeMarketsCore.Indicators.Pricing.Parameters;

#endregion

namespace FortitudeMarketsCore.Indicators.Pricing.MovingAverage.TimeWeighted;

public class MovingAverageTimeWeightedPublishRequestState
{
    private readonly List<MovingAverageCalculationState> averagesToCalculate;

    private readonly MovingAverageBatchPublisherParams? batchMovingAverageParams;

    private readonly ITimeWeightedMovingAveragePublisherRule containingRule;

    private readonly PricingIndicatorId indicatorSourceTickerId;

    private readonly bool isBatchPublisher;

    private readonly MovingAveragePublisherParams? singleMoveAverageParams;

    public MovingAverageTimeWeightedPublishRequestState
    (ITimeWeightedMovingAveragePublisherRule containingRule, PricingIndicatorId indicatorSourceTickerId
      , MovingAveragePublisherParams singlePublisherParams
      , CalculateMovingAverageOptions calculateMovingAverageOptions)
    {
        this.containingRule          = containingRule;
        this.indicatorSourceTickerId = indicatorSourceTickerId;

        singleMoveAverageParams = singlePublisherParams;

        isBatchPublisher = false;

        averagesToCalculate =
            singlePublisherParams
                .PeriodsToPublish
                .Select(movingAverageOffset => CreateMovingAverageState(movingAverageOffset, calculateMovingAverageOptions))
                .ToList();
    }

    public MovingAverageTimeWeightedPublishRequestState
    (ITimeWeightedMovingAveragePublisherRule containingRule
      , PricingIndicatorId indicatorSourceTickerId
      , MovingAverageBatchPublisherParams batchPublisherParams
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

    public DiscreetTimePeriod PublishInterval =>
        singleMoveAverageParams?.PublishFrequency.PublishInterval ?? batchMovingAverageParams!.Value.PublishFrequency.PublishInterval;


    public DateTime EarliestTime(DateTime asOfTime)
    {
        return averagesToCalculate.Min(macs => macs.EarliestTime(asOfTime));
    }

    public async ValueTask CalculateMovingAverageAndPublish(IDoublyLinkedList<IValidRangeBidAskPeriod> timeOrderedPairs, DateTime atTime)
    {
        NextPublishDateTime = atTime + PublishInterval.AveragePeriodTimeSpan();
        if (isBatchPublisher)
        {
            var toPublish = CalculateBatch(timeOrderedPairs, atTime);
            await PublishBatchBidAskInstantChain(toPublish);
        }
        else
        {
            foreach (var movingAverageCalculationState in averagesToCalculate)
            {
                var movingAverageInstant = CalculateSingleMovingAverage(movingAverageCalculationState, timeOrderedPairs, atTime);
                await PublishSingleMovingAverage(movingAverageInstant, movingAverageCalculationState.AveragePeriod);
            }
        }
    }

    public async ValueTask PublishBatchBidAskInstantChain(SameIndicatorValidRangeBidAskPeriodChain bidAskChain)
    {
        var publishParams  = batchMovingAverageParams!.Value;
        var publishChannel = publishParams.PublishChannelRequest.PublishChannel;
        var getMore        = await publishChannel.Publish(containingRule, bidAskChain);
        if (!getMore) containingRule.RemovePublishRequest(this);
    }

    public async ValueTask PublishSingleMovingAverage(IndicatorValidRangeBidAskPeriodValue validRangeBidAskPeriod, DiscreetTimePeriod averagePeriod)
    {
        var publishParams = singleMoveAverageParams!.Value.PublishParams;
        var responseType  = publishParams.ResponsePublishMethod;
        if (responseType is ResponsePublishMethod.ListenerDefaultBroadcastAddress or ResponsePublishMethod.AlternativeBroadcastAddress)
        {
            await containingRule.PublishAsync
                (string.Format(publishParams.AlternativePublishAddress!, averagePeriod.ShortName()), validRangeBidAskPeriod);
        }
        else
        {
            var publishChannel = (IChannel<IndicatorValidRangeBidAskPeriodValue>)publishParams.ChannelRequest!.Channel;
            var getMore        = await publishChannel.Publish(containingRule, validRangeBidAskPeriod);
            if (!getMore) containingRule.RemovePublishRequest(this);
        }
    }

    public MovingAverageCalculationState CreateMovingAverageState
        (MovingAverageOffset movingAverageOffset, CalculateMovingAverageOptions calculateMovingAverageOptions) =>
        new(movingAverageOffset, calculateMovingAverageOptions);

    public SameIndicatorValidRangeBidAskPeriodChain CalculateBatch(IDoublyLinkedList<IValidRangeBidAskPeriod> timeOrderedPairs, DateTime atTime)
    {
        var recycler    = containingRule.Context.PooledRecycler;
        var resultChain = recycler.Borrow<SameIndicatorValidRangeBidAskPeriodChain>();
        resultChain.Configure(indicatorSourceTickerId.IndicatorSourceTickerId, batchMovingAverageParams!.Value.BatchPeriodsToPublish.CoveringPeriod);
        var batchCount = 0u;
        foreach (var movingAverageState in averagesToCalculate)
        {
            var movingAverageInstant = movingAverageState.Calculate(timeOrderedPairs, atTime, batchCount++);
            resultChain.AddLast(movingAverageInstant.ToValidRangeBidAskPeriod(recycler));
        }
        return resultChain;
    }

    public IndicatorValidRangeBidAskPeriodValue CalculateSingleMovingAverage
        (MovingAverageCalculationState movingAverageCalculationState, IDoublyLinkedList<IValidRangeBidAskPeriod> timeOrderedPairs, DateTime atTime)
    {
        var movingAveragePeriod = movingAverageCalculationState.Calculate(timeOrderedPairs, atTime, 0);
        var bidAskInstant       = new IndicatorValidRangeBidAskPeriodValue(indicatorSourceTickerId, movingAveragePeriod);
        return bidAskInstant;
    }
}
