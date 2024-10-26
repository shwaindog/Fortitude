// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;

#endregion

namespace FortitudeMarkets.Indicators.Pricing;

public struct IndicatorPublishInterval
{
    public IndicatorPublishInterval(TimeSpan publishTimeSpan, TimeSpan? latestOffset = null)
    {
        PublishInterval = new DiscreetTimePeriod(publishTimeSpan);
        LatestOffset    = latestOffset != null ? new DiscreetTimePeriod(latestOffset!.Value) : new DiscreetTimePeriod();
    }

    public IndicatorPublishInterval(TimeBoundaryPeriod publishPeriod) => PublishInterval = new DiscreetTimePeriod(publishPeriod);

    public DiscreetTimePeriod LatestOffset    { get; }
    public DiscreetTimePeriod PublishInterval { get; }
}

public struct BatchIndicatorPublishInterval
{
    public BatchIndicatorPublishInterval
    (int batchCount, DiscreetTimePeriod publishInterval, DiscreetTimePeriod coveringPeriod
      , DiscreetTimePeriod? batchEntryOffsetPeriod = null, DiscreetTimePeriod? latestOffset = null)
    {
        BatchCount             = batchCount;
        CoveringPeriod         = coveringPeriod;
        PublishInterval        = publishInterval;
        BatchEntryOffsetPeriod = batchEntryOffsetPeriod ?? coveringPeriod;
        LatestOffset           = latestOffset ?? new DiscreetTimePeriod();
    }

    public DiscreetTimePeriod LatestOffset           { get; }
    public DiscreetTimePeriod BatchEntryOffsetPeriod { get; }
    public DiscreetTimePeriod CoveringPeriod         { get; }

    public int BatchCount { get; }

    public DiscreetTimePeriod PublishInterval { get; }
}
