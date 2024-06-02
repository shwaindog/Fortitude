// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory.UnmanagedMemory.MemoryMappedFiles;
using FortitudeIO.TimeSeries;
using FortitudeIO.TimeSeries.FileSystem.File;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.TimeSeries.FileSystem.File.Buckets;

public class OneHourPQLevel0QuoteDataBucket<TEntry> : PQDataBucket<TEntry, OneHourPQLevel0QuoteDataBucket<TEntry>, PQLevel0Quote>
    where TEntry : ITimeSeriesEntry<TEntry>, ILevel0Quote
{
    public OneHourPQLevel0QuoteDataBucket(IMutableBucketContainer bucketContainer, long bucketFileCursorOffset,
        bool writable, ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) { }

    public override TimeSeriesPeriod TimeSeriesPeriod => TimeSeriesPeriod.OneHour;
}

public class OneHourPQLevel1QuoteDataBucket<TEntry> : PQDataBucket<TEntry, OneHourPQLevel1QuoteDataBucket<TEntry>, PQLevel1Quote>
    where TEntry : ITimeSeriesEntry<TEntry>, ILevel1Quote
{
    public OneHourPQLevel1QuoteDataBucket(IMutableBucketContainer bucketContainer, long bucketFileCursorOffset,
        bool writable, ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) { }

    public override TimeSeriesPeriod TimeSeriesPeriod => TimeSeriesPeriod.OneHour;
}

public class OneHourPQLevel2QuoteDataBucket<TEntry> : PQDataBucket<TEntry, OneHourPQLevel2QuoteDataBucket<TEntry>, PQLevel2Quote>
    where TEntry : ITimeSeriesEntry<TEntry>, ILevel2Quote
{
    public OneHourPQLevel2QuoteDataBucket(IMutableBucketContainer bucketContainer, long bucketFileCursorOffset,
        bool writable, ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) { }

    public override TimeSeriesPeriod TimeSeriesPeriod => TimeSeriesPeriod.OneHour;
}

public class OneHourPQLevel3QuoteDataBucket<TEntry> : PQDataBucket<TEntry, OneHourPQLevel3QuoteDataBucket<TEntry>, PQLevel3Quote>
    where TEntry : ITimeSeriesEntry<TEntry>, ILevel3Quote
{
    public OneHourPQLevel3QuoteDataBucket(IMutableBucketContainer bucketContainer, long bucketFileCursorOffset,
        bool writable, ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) { }

    public override TimeSeriesPeriod TimeSeriesPeriod => TimeSeriesPeriod.OneHour;
}
