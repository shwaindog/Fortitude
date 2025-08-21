// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory.Buffers.ByteBuffers.UnmanagedMemory.MemoryMappedFiles;
using FortitudeIO.Storage.TimeSeries;
using FortitudeIO.Storage.TimeSeries.FileSystem.File.Buckets;
using FortitudeIO.Storage.TimeSeries.FileSystem.File.Session;
using FortitudeMarkets.Pricing;
using FortitudeMarkets.Pricing.FeedEvents.Quotes;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Storage.TimeSeries.FileSystem.File.Buckets;

public abstract class PriceQuoteSubBucket<TEntry, TBucket, TSubBucket> : SubBucketOnlyBucket<TEntry, TBucket, TSubBucket>, IPriceBucket
    where TEntry : ITimeSeriesEntry, IPublishableTickInstant
    where TBucket : class, IBucketNavigation<TBucket>, IMutableBucket<TEntry>, IPriceBucket
    where TSubBucket : class, IBucketNavigation<TSubBucket>, IMutableBucket<TEntry>, IPriceBucket
{
    protected PriceQuoteSubBucket
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable,
        ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) { }

    protected override IBucketFactory<TSubBucket> SubBucketFactory => SubBucketFac ??= new PriceBucketFactory<TSubBucket>(PricingInstrumentId);


    public IPricingInstrumentId PricingInstrumentId { get; set; } = null!;
}
