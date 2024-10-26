// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory.UnmanagedMemory.MemoryMappedFiles;
using FortitudeIO.TimeSeries.FileSystem.File.Buckets;
using FortitudeIO.TimeSeries.FileSystem.File.Session;
using FortitudeMarkets.Pricing;

#endregion

namespace FortitudeMarkets.Pricing.PQ.TimeSeries.FileSystem.File.Buckets;

public class PriceBucketFactory<TBucket> : BucketFactory<TBucket>
    where TBucket : class, IBucketNavigation<TBucket>, IPriceBucket, IMutableBucket
{
    public PriceBucketFactory
    (IPricingInstrumentId pricingInstrumentId,
        bool isFileRootBucketType = false) : base(isFileRootBucketType) =>
        PricingInstrumentId = pricingInstrumentId;

    private IPricingInstrumentId PricingInstrumentId { get; }

    protected override TBucket NewBucketObject
    (IMutableBucketContainer bucketContainer, long currentFileOffset, bool isWritable
      , ShiftableMemoryMappedFileView? alternativeMappedFileView = null)
    {
        var newBucket = base.NewBucketObject(bucketContainer, currentFileOffset, isWritable, alternativeMappedFileView);
        newBucket.PricingInstrumentId = PricingInstrumentId;
        return newBucket;
    }
}
