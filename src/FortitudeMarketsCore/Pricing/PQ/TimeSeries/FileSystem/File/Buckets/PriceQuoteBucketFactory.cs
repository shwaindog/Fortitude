// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory.UnmanagedMemory.MemoryMappedFiles;
using FortitudeIO.TimeSeries.FileSystem.File.Buckets;
using FortitudeIO.TimeSeries.FileSystem.File.Session;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.TimeSeries.FileSystem.File.Buckets;

public class PriceQuoteBucketFactory<TBucket> : BucketFactory<TBucket>
    where TBucket : class, IBucketNavigation<TBucket>, IPriceQuoteBucket, IMutableBucket
{
    public PriceQuoteBucketFactory(ISourceTickerQuoteInfo sourceTickerQuoteInfo,
        bool isFileRootBucketType = false) : base(isFileRootBucketType) =>
        SourceTickerQuoteInfo = sourceTickerQuoteInfo;

    private ISourceTickerQuoteInfo SourceTickerQuoteInfo { get; }

    protected override TBucket NewBucketObject(IMutableBucketContainer bucketContainer, long currentFileOffset, bool isWritable
      , ShiftableMemoryMappedFileView? alternativeMappedFileView = null)
    {
        var newBucket = base.NewBucketObject(bucketContainer, currentFileOffset, isWritable, alternativeMappedFileView);
        newBucket.SourceTickerQuoteInfo = SourceTickerQuoteInfo;
        return newBucket;
    }
}
