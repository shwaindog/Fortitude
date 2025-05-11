// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeIO.TimeSeries;
using FortitudeIO.TimeSeries.FileSystem.File.Buckets;
using FortitudeIO.TimeSeries.FileSystem.File.Session;
using FortitudeMarkets.Pricing;
using FortitudeMarkets.Pricing.Quotes;
using FortitudeMarkets.Pricing.Summaries;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes;
using FortitudeMarkets.Pricing.PQ.Summaries;

#endregion

namespace FortitudeMarkets.Pricing.PQ.TimeSeries.FileSystem.File;

public interface IPQQuoteAppendContext<TEntry, out TSerializeType> : IAppendContext<TEntry>
    where TEntry : ITimeSeriesEntry, IPublishableTickInstant
    where TSerializeType : PQPublishableTickInstant, new()
{
    TSerializeType SerializeEntry { get; }
}

public class PQQuoteAppendContext<TEntry, TBucket, TSerializeType> : AppendContext<TEntry, TBucket>, IPQQuoteAppendContext<TEntry, TSerializeType>
    where TEntry : ITimeSeriesEntry, IPublishableTickInstant
    where TBucket : class, IBucketNavigation<TBucket>, IMutableBucket<TEntry>
    where TSerializeType : PQPublishableTickInstant, new()
{
    public TSerializeType SerializeEntry { get; } = new();
}

public interface IPQPricePeriodSummaryAppendContext<TEntry> : IAppendContext<TEntry>
    where TEntry : ITimeSeriesEntry, IPricePeriodSummary
{
    IPQPriceStoragePeriodSummary SerializeEntry { get; }
}

public class PQPricePeriodSummaryAppendContext<TEntry, TBucket> : AppendContext<TEntry, TBucket>, IPQPricePeriodSummaryAppendContext<TEntry>
    where TEntry : ITimeSeriesEntry, IPricePeriodSummary
    where TBucket : class, IBucketNavigation<TBucket>, IMutableBucket<TEntry>
{
    public PQPricePeriodSummaryAppendContext
        (IPricingInstrumentId pricingInstrumentId) =>
        SerializeEntry = new PQPriceStoragePeriodSummary();

    public IPQPriceStoragePeriodSummary SerializeEntry { get; }
}
