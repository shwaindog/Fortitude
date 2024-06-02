// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory.UnmanagedMemory.MemoryMappedFiles;
using FortitudeIO.TimeSeries;
using FortitudeIO.TimeSeries.FileSystem.File;
using FortitudeIO.TimeSeries.FileSystem.File.Buckets;
using FortitudeMarketsApi.Pricing.Quotes;

#endregion

namespace FortitudeTests.FortitudeIO.TimeSeries.FileSystem.File;

public class TestLevel1DailyQuoteStructBucket :
    SubBucketOnlyBucket<Level1QuoteStruct, TestLevel1DailyQuoteStructBucket, TestLevel1HourlyQuoteStructBucket>
{
    public TestLevel1DailyQuoteStructBucket(IMutableBucketContainer bucketContainer, long bucketFileCursorOffset,
        bool writable, ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) =>
        IndexCount = 24;

    public override TimeSeriesPeriod TimeSeriesPeriod => TimeSeriesPeriod.OneDay;
}
