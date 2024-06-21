// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.Messages;
using FortitudeIO.TimeSeries;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
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

public struct MovingAverageIndicatorParams
{
    public MovingAverageIndicatorParams
        (ISourceTickerIdentifier sourceTickerId, PricePublishInterval publishInterval, params MovingAverageParams[] periodsToPublish)
    {
        SourceTickerId   = sourceTickerId;
        PublishInterval  = publishInterval;
        PeriodsToPublish = periodsToPublish;
    }

    public MovingAverageIndicatorParams
        (ISourceTickerIdentifier sourceTickerId, PricePublishInterval publishInterval, params BatchPricePublishInterval[] batchPeriodsToPublish)
    {
        SourceTickerId        = sourceTickerId;
        PublishInterval       = publishInterval;
        BatchPeriodsToPublish = batchPeriodsToPublish;
    }

    public ISourceTickerIdentifier SourceTickerId { get; }

    public MovingAverageParams[]? PeriodsToPublish { get; }

    public BatchPricePublishInterval[]? BatchPeriodsToPublish { get; }

    public PricePublishInterval PublishInterval { get; }
}

public class MovingAveragePublisherRule : PriceListenerIndicatorRule<PQLevel1Quote>
{
    public MovingAveragePublisherRule(MovingAverageIndicatorParams movingAverageParams)
        : base(movingAverageParams.SourceTickerId
             , $"MovingAveragePublisherRule_{movingAverageParams.SourceTickerId.Ticker}_{movingAverageParams.SourceTickerId.Source}") { }


    public override async ValueTask StartAsync()
    {
        // subscribe to live prices and cache ticks

        // request historical data to latest

        // build chained list

        // merge live ticks

        // set publish timer if applicable

        // calculate and publish
        await base.StartAsync();
    }

    protected override void ReceiveQuoteHandler(IBusMessage<PQLevel1Quote> priceQuoteMessage)
    {
        base.ReceiveQuoteHandler(priceQuoteMessage);
    }
}
