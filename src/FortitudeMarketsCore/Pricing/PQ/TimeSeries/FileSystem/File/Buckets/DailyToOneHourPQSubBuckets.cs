// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory.UnmanagedMemory.MemoryMappedFiles;
using FortitudeIO.TimeSeries;
using FortitudeIO.TimeSeries.FileSystem.File;
using FortitudeMarketsApi.Pricing.Quotes;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.TimeSeries.FileSystem.File.Buckets;

public class DailyToOneHourPQLevel0QuoteSubBuckets<TEntry> : PriceQuoteSubBucket<TEntry, DailyToOneHourPQLevel0QuoteSubBuckets<TEntry>,
    OneHourPQLevel0QuoteDataBucket<TEntry>>
    where TEntry : ITimeSeriesEntry<TEntry>, ILevel0Quote
{
    public DailyToOneHourPQLevel0QuoteSubBuckets(IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable,
        ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) { }

    public override TimeSeriesPeriod TimeSeriesPeriod => TimeSeriesPeriod.OneDay;
}

public class DailyToOneHourPQLevel1QuoteSubBuckets<TEntry> : PriceQuoteSubBucket<TEntry, DailyToOneHourPQLevel1QuoteSubBuckets<TEntry>,
    OneHourPQLevel1QuoteDataBucket<TEntry>>
    where TEntry : ITimeSeriesEntry<TEntry>, ILevel1Quote
{
    public DailyToOneHourPQLevel1QuoteSubBuckets(IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable,
        ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) { }

    public override TimeSeriesPeriod TimeSeriesPeriod => TimeSeriesPeriod.OneDay;
}

public class DailyToOneHourPQLevel2QuoteSubBuckets<TEntry> : PriceQuoteSubBucket<TEntry, DailyToOneHourPQLevel2QuoteSubBuckets<TEntry>,
    OneHourPQLevel2QuoteDataBucket<TEntry>>
    where TEntry : ITimeSeriesEntry<TEntry>, ILevel2Quote
{
    public DailyToOneHourPQLevel2QuoteSubBuckets(IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable,
        ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) { }

    public override TimeSeriesPeriod TimeSeriesPeriod => TimeSeriesPeriod.OneDay;
}

public class DailyToOneHourPQLevel3QuoteSubBuckets<TEntry> : PriceQuoteSubBucket<TEntry, DailyToOneHourPQLevel3QuoteSubBuckets<TEntry>,
    OneHourPQLevel3QuoteDataBucket<TEntry>>
    where TEntry : ITimeSeriesEntry<TEntry>, ILevel3Quote
{
    public DailyToOneHourPQLevel3QuoteSubBuckets(IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable,
        ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) { }

    public override TimeSeriesPeriod TimeSeriesPeriod => TimeSeriesPeriod.OneDay;
}
