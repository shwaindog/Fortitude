// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeIO.TimeSeries;
using FortitudeIO.TimeSeries.FileSystem.File.Appending;
using FortitudeIO.TimeSeries.FileSystem.File.Buckets;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.TimeSeries.FileSystem.File;

public interface IPQAppendContext<TEntry, out TSerializeType> : IAppendContext<TEntry>
    where TEntry : ITimeSeriesEntry<TEntry>, ILevel0Quote
    where TSerializeType : PQLevel0Quote, new()
{
    TSerializeType SerializeEntry { get; }
}

public class PQAppendContext<TEntry, TBucket, TSerializeType> : AppendContext<TEntry, TBucket>, IPQAppendContext<TEntry, TSerializeType>
    where TEntry : ITimeSeriesEntry<TEntry>, ILevel0Quote
    where TBucket : class, IBucketNavigation<TBucket>, IMutableBucket<TEntry>
    where TSerializeType : PQLevel0Quote, new()
{
    public TSerializeType SerializeEntry { get; } = new();
}
