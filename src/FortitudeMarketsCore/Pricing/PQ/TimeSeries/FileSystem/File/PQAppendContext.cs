// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeIO.TimeSeries;
using FortitudeIO.TimeSeries.FileSystem.File.Buckets;
using FortitudeIO.TimeSeries.FileSystem.File.Session;
using FortitudeMarketsApi.Pricing;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsApi.Pricing.Summaries;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Summaries;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.TimeSeries.FileSystem.File;

public interface IPQQuoteAppendContext<TEntry, out TSerializeType> : IAppendContext<TEntry>
    where TEntry : ITimeSeriesEntry, ITickInstant
    where TSerializeType : PQTickInstant, new()
{
    TSerializeType SerializeEntry { get; }
}

public class PQQuoteAppendContext<TEntry, TBucket, TSerializeType> : AppendContext<TEntry, TBucket>, IPQQuoteAppendContext<TEntry, TSerializeType>
    where TEntry : ITimeSeriesEntry, ITickInstant
    where TBucket : class, IBucketNavigation<TBucket>, IMutableBucket<TEntry>
    where TSerializeType : PQTickInstant, new()
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
