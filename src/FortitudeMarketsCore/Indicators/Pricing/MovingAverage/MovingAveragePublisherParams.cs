// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeIO.TimeSeries;
using FortitudeMarketsApi.Pricing;

#endregion

namespace FortitudeMarketsCore.Indicators.Pricing.MovingAverage;

public struct SharedTicksMovingAveragePublishParams
{
    public SharedTicksMovingAveragePublishParams
    (SourceTickerIdentifier sourceTickerIdentifier, IndicatorPublishInterval publishInterval
      , params MovingAverageOffset[] initialPeriodsToPublish)
    {
        InitialPeriodsToPublish = initialPeriodsToPublish;
        PublishInterval         = publishInterval;
        SourceTickerIdentifier  = sourceTickerIdentifier;
    }

    public SourceTickerIdentifier SourceTickerIdentifier { get; }

    public IndicatorPublishInterval PublishInterval { get; }

    public MovingAverageOffset[]? InitialPeriodsToPublish { get; }
}

public struct MovingAveragePublisherParams
{
    public MovingAveragePublisherParams
    (SourceTickerIdentifier sourceTickerIdentifier, IndicatorPublishInterval publishFrequency
      , params MovingAverageOffset[] periodsToPublish)
    {
        SourceTickerIdentifier = sourceTickerIdentifier;
        PublishFrequency       = publishFrequency;
        PeriodsToPublish       = periodsToPublish;
    }

    public MovingAveragePublisherParams
    (SourceTickerIdentifier sourceTickerIdentifier, IndicatorPublishInterval publishFrequency
      , params BatchIndicatorPublishInterval[] batchPeriodsToPublish)
    {
        SourceTickerIdentifier = sourceTickerIdentifier;
        PublishFrequency       = publishFrequency;
        BatchPeriodsToPublish  = batchPeriodsToPublish;
    }

    public SourceTickerIdentifier SourceTickerIdentifier { get; }

    public MovingAverageOffset[]? PeriodsToPublish { get; }

    public BatchIndicatorPublishInterval[]? BatchPeriodsToPublish { get; }

    public IndicatorPublishInterval PublishFrequency { get; }
}

public static class MovingAveragePublisherParamsExtensions
{
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
