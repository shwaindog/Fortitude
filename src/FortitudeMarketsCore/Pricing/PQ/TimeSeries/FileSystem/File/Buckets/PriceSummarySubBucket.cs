// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory.UnmanagedMemory.MemoryMappedFiles;
using FortitudeIO.TimeSeries;
using FortitudeIO.TimeSeries.FileSystem.File.Buckets;
using FortitudeIO.TimeSeries.FileSystem.File.Session;
using FortitudeMarketsApi.Pricing;
using FortitudeMarketsApi.Pricing.Summaries;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.TimeSeries.FileSystem.File.Buckets;

public abstract class PriceSummarySubBucket<TBucket, TSubBucket, TEntry> : SubBucketOnlyBucket<TEntry, TBucket, TSubBucket>, IPriceBucket
    where TBucket : class, IBucketNavigation<TBucket>, IMutableBucket<TEntry>, IPriceBucket
    where TSubBucket : class, IBucketNavigation<TSubBucket>, IMutableBucket<TEntry>, IPriceBucket
    where TEntry : ITimeSeriesEntry<TEntry>, IPricePeriodSummary
{
    protected PriceSummarySubBucket
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable,
        ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) { }

    protected override IBucketFactory<TSubBucket> SubBucketFactory => SubBucketFac ??= new PriceBucketFactory<TSubBucket>(PricingInstrumentId);


    public IPricingInstrumentId PricingInstrumentId { get; set; } = null!;
}
