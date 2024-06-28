// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeIO.TimeSeries;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;

#endregion

namespace FortitudeMarketsCore.Indicators.Pricing.MovingAverage;

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

public static class MovingAveragePublisherParamsExtensions
{
    public static DateTime NextPublishTime(this MovingAveragePublisherParams movingAvg, DateTime lastPublishTime)
    {
        var calcStartTime = lastPublishTime;
        var calcEndTime   = lastPublishTime;
        if (movingAvg.PublishInterval.PriceIndicatorPublishType == PriceIndicatorPublishType.TimeSeriesPeriod)
        {
            calcStartTime = movingAvg.PublishInterval.PublishPeriod!.Value.ContainingPeriodBoundaryStart(lastPublishTime);
            calcEndTime   = movingAvg.PublishInterval.PublishPeriod!.Value.PeriodEnd(calcStartTime);
        }
        else if (movingAvg.PublishInterval.PriceIndicatorPublishType == PriceIndicatorPublishType.SetTimeSpan)
        {
            calcEndTime = lastPublishTime.Add(movingAvg.PublishInterval.PublishTimeSpan!.Value);
        }
        return calcEndTime;
    }
}
