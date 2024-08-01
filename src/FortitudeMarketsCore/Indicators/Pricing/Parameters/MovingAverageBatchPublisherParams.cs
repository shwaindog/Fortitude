// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.BusMessaging.Routing.Channel;
using FortitudeCommon.Chronometry;
using FortitudeIO.TimeSeries;
using FortitudeMarketsApi.Indicators.Pricing;
using FortitudeMarketsApi.Pricing;

#endregion

namespace FortitudeMarketsCore.Indicators.Pricing.Parameters;

public struct MovingAverageBatchPublisherParams
{
    public MovingAverageBatchPublisherParams
    (SourceTickerIdentifier sourceTickerIdentifier, IndicatorPublishInterval publishFrequency
      , BatchIndicatorPublishInterval batchPeriodsToPublish, ChannelPublishRequest<SameIndicatorValidRangeBidAskPeriodChain> publishChannelRequest)
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

    public ChannelPublishRequest<SameIndicatorValidRangeBidAskPeriodChain> PublishChannelRequest { get; }
}

public static class MovingAveragePublisherParamsExtensions
{
    internal static int AllRequestIds;

    public static DateTime NextPublishTime(this MovingAveragePublisherParams movingAvg, DateTime lastPublishTime)
    {
        var calcStartTime = lastPublishTime;
        var calcEndTime   = lastPublishTime;
        if (movingAvg.PublishFrequency.PublishInterval.IsTimeBoundaryPeriod())
        {
            calcStartTime = movingAvg.PublishFrequency.PublishInterval.Period.ContainingPeriodBoundaryStart(lastPublishTime);
            calcEndTime   = movingAvg.PublishFrequency.PublishInterval.Period.PeriodEnd(calcStartTime);
        }
        else if (movingAvg.PublishFrequency.PublishInterval.IsTimeBoundaryPeriod())
        {
            calcEndTime = lastPublishTime.Add(movingAvg.PublishFrequency.PublishInterval.AveragePeriodTimeSpan());
        }
        return calcEndTime;
    }
}
