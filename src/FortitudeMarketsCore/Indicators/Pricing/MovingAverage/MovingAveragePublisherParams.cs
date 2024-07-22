// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.BusMessaging.Routing.Channel;
using FortitudeBusRules.BusMessaging.Routing.Response;
using FortitudeCommon.Chronometry;
using FortitudeIO.TimeSeries;
using FortitudeMarketsApi.Indicators.Pricing;
using FortitudeMarketsApi.Pricing;

#endregion

namespace FortitudeMarketsCore.Indicators.Pricing.MovingAverage;

public enum RequestExceedTimeRangeOptions
{
    GrowToHardLimitTimeRange
  , GrowUnlimited
  , RejectRequest
}

public struct LiveShortPeriodMovingAveragePublishParams
{
    public LiveShortPeriodMovingAveragePublishParams
    (IndicatorSourceTickerIdentifier indicatorSourceTickerIdentifier, UnboundedTimeSpan? initialTickTimeRange = null
      , MovingAveragePublisherParams? initialPeriodsToPublish = null
      , RequestExceedTimeRangeOptions requestExceedTimeRangeOptions = RequestExceedTimeRangeOptions.RejectRequest
      , TimeSpan? ignoreGapsTimeSpan = null)
    {
        IndicatorSourceTickerIdentifier = indicatorSourceTickerIdentifier;
        IgnoreGapsTimeSpan              = ignoreGapsTimeSpan ?? TimeSpan.FromHours(2);
        TimeRangeExtensionOptions       = requestExceedTimeRangeOptions;
        InitialToHardLimitTimeSpan      = initialTickTimeRange ?? new UnboundedTimeSpan(TimeSpan.FromMinutes(31), TimeSpan.FromMinutes(31));
        if (initialPeriodsToPublish == null && InitialToHardLimitTimeSpan.LowerLimit != null)
        {
            var defaultPublishPeriods = new List<MovingAverageOffset>();
            foreach (var timeSeriesPeriod in InitialToHardLimitTimeSpan.LowerLimit.Value.TimeSeriesPeriodsSmallerThan())
            {
                var periodMovingAverage = new MovingAverageOffset(new TimePeriod(timeSeriesPeriod));
                defaultPublishPeriods.Add(periodMovingAverage);
            }
            if (defaultPublishPeriods.Any())
            {
                var periodsToPublishParams = new MovingAveragePublisherParams
                    (indicatorSourceTickerIdentifier, new IndicatorPublishInterval(TimeSeriesPeriod.OneSecond), null
                   , defaultPublishPeriods.ToArray());
                InitialPeriodsToPublish = periodsToPublishParams;
            }
        }
        else
        {
            InitialPeriodsToPublish = initialPeriodsToPublish;
        }
    }

    public IndicatorSourceTickerIdentifier IndicatorSourceTickerIdentifier { get; }

    public MovingAveragePublisherParams? InitialPeriodsToPublish { get; }

    public UnboundedTimeSpan InitialToHardLimitTimeSpan { get; }

    public TimeSpan IgnoreGapsTimeSpan { get; } // assume market closed if gap exceeds this

    public RequestExceedTimeRangeOptions TimeRangeExtensionOptions { get; }
}

public struct MovingAveragePublisherParams
{
    public MovingAveragePublisherParams
    (SourceTickerIdentifier sourceTickerIdentifier, IndicatorPublishInterval publishFrequency, ResponsePublishParams? publishParams = null
      , params MovingAverageOffset[] periodsToPublish)
    {
        SourceTickerIdentifier = sourceTickerIdentifier;
        PublishFrequency       = publishFrequency;
        PeriodsToPublish       = periodsToPublish;
        PublishParams          = publishParams ?? new ResponsePublishParams();

        RequestId = Interlocked.Increment(ref MovingAveragePublisherParamsExtensions.AllRequestIds);
    }

    public int RequestId { get; }

    public SourceTickerIdentifier SourceTickerIdentifier { get; }

    public MovingAverageOffset[] PeriodsToPublish { get; }

    public IndicatorPublishInterval PublishFrequency { get; }

    public ResponsePublishParams PublishParams { get; }
}

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
