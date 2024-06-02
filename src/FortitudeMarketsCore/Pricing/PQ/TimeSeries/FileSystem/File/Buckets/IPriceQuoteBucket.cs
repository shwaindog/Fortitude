// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeIO.TimeSeries;
using FortitudeIO.TimeSeries.FileSystem.File.Buckets;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.Quotes;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.TimeSeries.FileSystem.File.Buckets;

public interface IPriceQuoteBucket
{
    ISourceTickerQuoteInfo SourceTickerQuoteInfo { get; set; }
}

public interface IPriceQuoteBucket<TEntry> : IPriceQuoteBucket, IMutableBucket<TEntry>
    where TEntry : ITimeSeriesEntry<TEntry>, ILevel0Quote { }
