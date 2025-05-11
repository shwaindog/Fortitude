// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory.UnmanagedMemory.MemoryMappedFiles;
using FortitudeIO.TimeSeries;
using FortitudeIO.TimeSeries.FileSystem.File.Session;
using FortitudeMarkets.Pricing.Quotes;

#endregion

namespace FortitudeMarkets.Pricing.PQ.TimeSeries.FileSystem.File.Buckets;

public class DailyToHourlyTickInstantSubBuckets<TEntry> : PriceQuoteSubBucket<TEntry, DailyToHourlyTickInstantSubBuckets<TEntry>,
    HourlyTickInstantDataBucket<TEntry>>
    where TEntry : ITimeSeriesEntry, IPublishableTickInstant
{
    public DailyToHourlyTickInstantSubBuckets
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable,
        ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) =>
        IndexCount = 24;

    public override TimeBoundaryPeriod TimeBoundaryPeriod => TimeBoundaryPeriod.OneDay;
}

public class DailyToHourlyLevel1QuoteSubBuckets<TEntry> : PriceQuoteSubBucket<TEntry, DailyToHourlyLevel1QuoteSubBuckets<TEntry>,
    HourlyLevel1QuoteDataBucket<TEntry>>
    where TEntry : ITimeSeriesEntry, IPublishableLevel1Quote
{
    public DailyToHourlyLevel1QuoteSubBuckets
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable,
        ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) =>
        IndexCount = 24;

    public override TimeBoundaryPeriod TimeBoundaryPeriod => TimeBoundaryPeriod.OneDay;
}

public class DailyToHourlyLevel2QuoteSubBuckets<TEntry> : PriceQuoteSubBucket<TEntry, DailyToHourlyLevel2QuoteSubBuckets<TEntry>,
    HourlyLevel2QuoteDataBucket<TEntry>>
    where TEntry : ITimeSeriesEntry, IPublishableLevel2Quote
{
    public DailyToHourlyLevel2QuoteSubBuckets
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable,
        ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) =>
        IndexCount = 24;

    public override TimeBoundaryPeriod TimeBoundaryPeriod => TimeBoundaryPeriod.OneDay;
}

public class DailyToHourlyLevel3QuoteSubBuckets<TEntry> : PriceQuoteSubBucket<TEntry, DailyToHourlyLevel3QuoteSubBuckets<TEntry>,
    HourlyLevel3QuoteDataBucket<TEntry>>
    where TEntry : ITimeSeriesEntry, IPublishableLevel3Quote
{
    public DailyToHourlyLevel3QuoteSubBuckets
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable,
        ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) =>
        IndexCount = 24;

    public override TimeBoundaryPeriod TimeBoundaryPeriod => TimeBoundaryPeriod.OneDay;
}
