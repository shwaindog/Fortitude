// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeIO.TimeSeries;

#endregion

namespace FortitudeMarketsCore.Indicators.Pricing;

public struct IndicatorPublishInterval
{
    public IndicatorPublishInterval(TimeSpan publishTimeSpan, TimeSpan? latestOffset = null)
    {
        PublishInterval = new TimePeriod(publishTimeSpan);
        LatestOffset    = latestOffset != null ? new TimePeriod(latestOffset) : new TimePeriod();
    }

    public IndicatorPublishInterval(TimeSeriesPeriod publishPeriod) => PublishInterval = new TimePeriod(publishPeriod);

    public TimePeriod LatestOffset    { get; }
    public TimePeriod PublishInterval { get; }
}

public struct BatchIndicatorPublishInterval
{
    public BatchIndicatorPublishInterval
    (int batchCount, TimePeriod publishInterval, TimePeriod coveringPeriod
      , TimePeriod? batchEntryOffsetPeriod = null, TimePeriod? latestOffset = null)
    {
        BatchCount             = batchCount;
        CoveringPeriod         = coveringPeriod;
        PublishInterval        = publishInterval;
        BatchEntryOffsetPeriod = batchEntryOffsetPeriod ?? coveringPeriod;
        LatestOffset           = latestOffset ?? new TimePeriod();
    }

    public TimePeriod LatestOffset           { get; }
    public TimePeriod BatchEntryOffsetPeriod { get; }
    public TimePeriod CoveringPeriod         { get; }

    public int BatchCount { get; }

    public TimePeriod PublishInterval { get; }
}
