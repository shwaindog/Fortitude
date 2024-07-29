// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.BusMessaging.Routing.Channel;
using FortitudeIO.TimeSeries;
using FortitudeMarketsApi.Indicators.Pricing;
using FortitudeMarketsApi.Pricing;

#endregion

namespace FortitudeMarketsCore.Indicators.Pricing.Parameters;

public struct MovingAverageBatchPublisherParams
{
    public MovingAverageBatchPublisherParams
    (SourceTickerIdentifier sourceTickerIdentifier, IndicatorPublishInterval publishFrequency
      , BatchIndicatorPublishInterval batchPeriodsToPublish, ChannelPublishRequest<SameIndicatorBidAskInstantChain> publishChannelRequest)
    {
        SourceTickerIdentifier = sourceTickerIdentifier;
        PublishFrequency       = publishFrequency;
        BatchPeriodsToPublish  = batchPeriodsToPublish;
        PublishChannelRequest  = publishChannelRequest;

        RequestId = Interlocked.Increment(ref MovingAveragePublisherParamsExtensions.AllRequestIds);
    }

    public int RequestId { get; }

    public SourceTickerIdentifier SourceTickerIdentifier { get; }

    public BatchIndicatorPublishInterval BatchPeriodsToPublish { get; }

    public IndicatorPublishInterval PublishFrequency { get; }

    private ChannelPublishRequest<SameIndicatorBidAskInstantChain> PublishChannelRequest { get; }
}

public static class MovingAveragePublisherParamsExtensions
{
    internal static int AllRequestIds;

    public static DateTime NextPublishTime(this MovingAveragePublisherParams movingAvg, DateTime lastPublishTime)
    {
        var calcStartTime = lastPublishTime;
        var calcEndTime   = lastPublishTime;
        if (movingAvg.PublishFrequency.PublishInterval.IsTimeSeriesPeriod())
        {
            calcStartTime = movingAvg.PublishFrequency.PublishInterval.TimeSeriesPeriod.ContainingPeriodBoundaryStart(lastPublishTime);
            calcEndTime   = movingAvg.PublishFrequency.PublishInterval.TimeSeriesPeriod.PeriodEnd(calcStartTime);
        }
        else if (movingAvg.PublishFrequency.PublishInterval.IsTimeSeriesPeriod())
        {
            calcEndTime = lastPublishTime.Add(movingAvg.PublishFrequency.PublishInterval.AveragePeriodTimeSpan());
        }
        return calcEndTime;
    }
}
