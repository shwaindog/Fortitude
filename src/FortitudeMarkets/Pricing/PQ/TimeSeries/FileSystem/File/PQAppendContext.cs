// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeIO.TimeSeries;
using FortitudeIO.TimeSeries.FileSystem.File.Buckets;
using FortitudeIO.TimeSeries.FileSystem.File.Session;
using FortitudeMarkets.Pricing.FeedEvents.Candles;
using FortitudeMarkets.Pricing.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Candles;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;

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

public interface IPQCandleAppendContext<TEntry> : IAppendContext<TEntry>
    where TEntry : ITimeSeriesEntry, ICandle
{
    IPQStorageCandle SerializeEntry { get; }
}

public class PQCandleAppendContext<TEntry, TBucket> : AppendContext<TEntry, TBucket>, IPQCandleAppendContext<TEntry>
    where TEntry : ITimeSeriesEntry, ICandle
    where TBucket : class, IBucketNavigation<TBucket>, IMutableBucket<TEntry>
{
    public PQCandleAppendContext(IPricingInstrumentId pricingInstrumentId) =>
        SerializeEntry = new PQStorageCandle();

    public IPQStorageCandle SerializeEntry { get; }
}
