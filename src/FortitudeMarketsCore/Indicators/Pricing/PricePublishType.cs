// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeIO.TimeSeries;

#endregion

namespace FortitudeMarketsCore.Indicators.Pricing;

public struct PricePublishInterval
{
    public PricePublishInterval() => PriceIndicatorPublishType = PriceIndicatorPublishType.Tick;

    public PricePublishInterval
        (PriceIndicatorPublishType priceIndicatorPublishType, TimeSpan? publishTimeSpan = null, TimeSeriesPeriod? publishPeriod = null)
    {
        PriceIndicatorPublishType = priceIndicatorPublishType;
        PublishTimeSpan           = publishTimeSpan;
        PublishPeriod             = publishPeriod;
    }

    public PriceIndicatorPublishType PriceIndicatorPublishType { get; }
    public TimeSpan?                 PublishTimeSpan           { get; }
    public TimeSeriesPeriod?         PublishPeriod             { get; }
}

public enum PriceIndicatorPublishType
{
    Tick
  , SetTimeSpan
  , TimeSeriesPeriod
}

public struct BatchPricePublishInterval
{
    public BatchPricePublishInterval(int batchCount, PricePublishInterval publishEvery, PricePublishInterval batchPublishEntrySeparation = default)
    {
        BatchCount                  = batchCount;
        PublishEvery                = publishEvery;
        BatchPublishEntrySeparation = batchPublishEntrySeparation;
    }

    private PricePublishInterval BatchPublishEntrySeparation { get; }

    public int BatchCount { get; }

    public PricePublishInterval PublishEvery { get; }
}
