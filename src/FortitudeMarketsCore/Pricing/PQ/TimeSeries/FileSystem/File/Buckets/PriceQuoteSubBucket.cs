// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory.UnmanagedMemory.MemoryMappedFiles;
using FortitudeIO.TimeSeries;
using FortitudeIO.TimeSeries.FileSystem.File;
using FortitudeIO.TimeSeries.FileSystem.File.Buckets;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.Quotes;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.TimeSeries.FileSystem.File.Buckets;

public abstract class PriceQuoteSubBucket<TEntry, TBucket, TSubBucket> : SubBucketOnlyBucket<TEntry, TBucket, TSubBucket>, IPriceQuoteBucket
    where TEntry : ITimeSeriesEntry<TEntry>, ILevel0Quote
    where TBucket : class, IBucketNavigation<TBucket>, IMutableBucket<TEntry>, IPriceQuoteBucket
    where TSubBucket : class, IBucketNavigation<TSubBucket>, IMutableBucket<TEntry>, IPriceQuoteBucket
{
    protected PriceQuoteSubBucket(IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable,
        ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) { }

    protected override IBucketFactory<TSubBucket> SubBucketFactory => SubBucketFac ??= new PriceQuoteBucketFactory<TSubBucket>(SourceTickerQuoteInfo);


    public ISourceTickerQuoteInfo SourceTickerQuoteInfo { get; set; } = null!;
}
