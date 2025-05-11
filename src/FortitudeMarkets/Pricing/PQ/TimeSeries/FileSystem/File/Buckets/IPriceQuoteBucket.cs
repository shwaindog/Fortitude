// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeIO.TimeSeries;
using FortitudeIO.TimeSeries.FileSystem.File.Buckets;
using FortitudeMarkets.Pricing;
using FortitudeMarkets.Pricing.Quotes;
using FortitudeMarkets.Pricing.Summaries;

#endregion

namespace FortitudeMarkets.Pricing.PQ.TimeSeries.FileSystem.File.Buckets;

public interface IPriceBucket
{
    IPricingInstrumentId PricingInstrumentId { get; set; }
}

public interface IPriceQuoteBucket<TEntry> : IPriceBucket, IMutableBucket<TEntry>
    where TEntry : ITimeSeriesEntry, IPublishableTickInstant { }

public interface IPricePeriodSummaryBucket<TEntry> : IPriceBucket, IMutableBucket<TEntry>
    where TEntry : ITimeSeriesEntry, IPricePeriodSummary { }
