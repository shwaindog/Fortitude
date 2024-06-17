// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory.UnmanagedMemory.MemoryMappedFiles;
using FortitudeIO.TimeSeries;
using FortitudeIO.TimeSeries.FileSystem.File.Session;
using FortitudeMarketsApi.Pricing.Quotes;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.TimeSeries.FileSystem.File.Buckets;

public class DailyToHourlyLevel0QuoteSubBuckets<TEntry> : PriceQuoteSubBucket<TEntry, DailyToHourlyLevel0QuoteSubBuckets<TEntry>,
    HourlyLevel0QuoteDataBucket<TEntry>>
    where TEntry : ITimeSeriesEntry<TEntry>, ILevel0Quote
{
    public DailyToHourlyLevel0QuoteSubBuckets
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable,
        ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) =>
        IndexCount = 24;

    public override TimeSeriesPeriod TimeSeriesPeriod => TimeSeriesPeriod.OneDay;
}

public class DailyToHourlyLevel1QuoteSubBuckets<TEntry> : PriceQuoteSubBucket<TEntry, DailyToHourlyLevel1QuoteSubBuckets<TEntry>,
    HourlyLevel1QuoteDataBucket<TEntry>>
    where TEntry : ITimeSeriesEntry<TEntry>, ILevel1Quote
{
    public DailyToHourlyLevel1QuoteSubBuckets
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable,
        ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) =>
        IndexCount = 24;

    public override TimeSeriesPeriod TimeSeriesPeriod => TimeSeriesPeriod.OneDay;
}

public class DailyToHourlyLevel2QuoteSubBuckets<TEntry> : PriceQuoteSubBucket<TEntry, DailyToHourlyLevel2QuoteSubBuckets<TEntry>,
    HourlyLevel2QuoteDataBucket<TEntry>>
    where TEntry : ITimeSeriesEntry<TEntry>, ILevel2Quote
{
    public DailyToHourlyLevel2QuoteSubBuckets
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable,
        ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) =>
        IndexCount = 24;

    public override TimeSeriesPeriod TimeSeriesPeriod => TimeSeriesPeriod.OneDay;
}

public class DailyToHourlyLevel3QuoteSubBuckets<TEntry> : PriceQuoteSubBucket<TEntry, DailyToHourlyLevel3QuoteSubBuckets<TEntry>,
    HourlyLevel3QuoteDataBucket<TEntry>>
    where TEntry : ITimeSeriesEntry<TEntry>, ILevel3Quote
{
    public DailyToHourlyLevel3QuoteSubBuckets
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable,
        ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) =>
        IndexCount = 24;

    public override TimeSeriesPeriod TimeSeriesPeriod => TimeSeriesPeriod.OneDay;
}
