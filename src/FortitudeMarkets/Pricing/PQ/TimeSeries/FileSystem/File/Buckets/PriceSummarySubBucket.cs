// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory.UnmanagedMemory.MemoryMappedFiles;
using FortitudeIO.TimeSeries;
using FortitudeIO.TimeSeries.FileSystem.File.Buckets;
using FortitudeIO.TimeSeries.FileSystem.File.Session;
using FortitudeMarkets.Pricing;
using FortitudeMarkets.Pricing.Summaries;

#endregion

namespace FortitudeMarkets.Pricing.PQ.TimeSeries.FileSystem.File.Buckets;

public abstract class PriceSummarySubBucket<TBucket, TSubBucket, TEntry> : SubBucketOnlyBucket<TEntry, TBucket, TSubBucket>, IPriceBucket
    where TBucket : class, IBucketNavigation<TBucket>, IMutableBucket<TEntry>, IPriceBucket
    where TSubBucket : class, IBucketNavigation<TSubBucket>, IMutableBucket<TEntry>, IPriceBucket
    where TEntry : ITimeSeriesEntry, IPricePeriodSummary
{
    protected PriceSummarySubBucket
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable,
        ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) { }

    protected override IBucketFactory<TSubBucket> SubBucketFactory => SubBucketFac ??= new PriceBucketFactory<TSubBucket>(PricingInstrumentId);


    public IPricingInstrumentId PricingInstrumentId { get; set; } = null!;
}
