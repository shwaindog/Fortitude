// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory.UnmanagedMemory.MemoryMappedFiles;
using FortitudeIO.TimeSeries;
using FortitudeIO.TimeSeries.FileSystem.File.Session;
using FortitudeMarkets.Pricing.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes;

#endregion

namespace FortitudeMarkets.Pricing.PQ.TimeSeries.FileSystem.File.Buckets;

public class HourlyTickInstantDataBucket<TEntry> : PQQuoteDataBucket<TEntry, HourlyTickInstantDataBucket<TEntry>, PQPublishableTickInstant>
    where TEntry : ITimeSeriesEntry, IPublishableTickInstant
{
    public HourlyTickInstantDataBucket
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset,
        bool writable, ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) { }

    public override TimeBoundaryPeriod TimeBoundaryPeriod => TimeBoundaryPeriod.OneHour;
}

public class HourlyLevel1QuoteDataBucket<TEntry> : PQQuoteDataBucket<TEntry, HourlyLevel1QuoteDataBucket<TEntry>, PQPublishableLevel1Quote>
    where TEntry : ITimeSeriesEntry, IPublishableLevel1Quote
{
    public HourlyLevel1QuoteDataBucket
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset,
        bool writable, ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) { }

    public override TimeBoundaryPeriod TimeBoundaryPeriod => TimeBoundaryPeriod.OneHour;
}

public class HourlyLevel2QuoteDataBucket<TEntry> : PQQuoteDataBucket<TEntry, HourlyLevel2QuoteDataBucket<TEntry>, PQPublishableLevel2Quote>
    where TEntry : ITimeSeriesEntry, IPublishableLevel2Quote
{
    public HourlyLevel2QuoteDataBucket
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset,
        bool writable, ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) { }

    public override TimeBoundaryPeriod TimeBoundaryPeriod => TimeBoundaryPeriod.OneHour;
}

public class HourlyLevel3QuoteDataBucket<TEntry> : PQQuoteDataBucket<TEntry, HourlyLevel3QuoteDataBucket<TEntry>, PQPublishableLevel3Quote>
    where TEntry : ITimeSeriesEntry, IPublishableLevel3Quote
{
    public HourlyLevel3QuoteDataBucket
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset,
        bool writable, ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) { }

    public override TimeBoundaryPeriod TimeBoundaryPeriod => TimeBoundaryPeriod.OneHour;
}
