// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory.UnmanagedMemory.MemoryMappedFiles;
using FortitudeIO.TimeSeries;
using FortitudeIO.TimeSeries.FileSystem.File.Session;
using FortitudeMarkets.Pricing.FeedEvents.Candles;

#endregion

namespace FortitudeMarkets.Pricing.PQ.TimeSeries.FileSystem.File.Buckets;

public class HourlyCandleDataBucket<TEntry> : PQCandleDataBucket<HourlyCandleDataBucket<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry, ICandle
{
    public HourlyCandleDataBucket
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset,
        bool writable, ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) { }

    public override TimeBoundaryPeriod TimeBoundaryPeriod => TimeBoundaryPeriod.OneHour;
}

public class FourHourlyCandleDataBucket<TEntry> : PQCandleDataBucket<FourHourlyCandleDataBucket<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry, ICandle
{
    public FourHourlyCandleDataBucket
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset,
        bool writable, ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) { }

    public override TimeBoundaryPeriod TimeBoundaryPeriod => TimeBoundaryPeriod.FourHours;
}

public class DailyCandleDataBucket<TEntry> : PQCandleDataBucket<DailyCandleDataBucket<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry, ICandle
{
    public DailyCandleDataBucket
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset,
        bool writable, ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) { }

    public override TimeBoundaryPeriod TimeBoundaryPeriod => TimeBoundaryPeriod.OneDay;
}

public class WeeklyCandleDataBucket<TEntry> : PQCandleDataBucket<WeeklyCandleDataBucket<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry, ICandle
{
    public WeeklyCandleDataBucket
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset,
        bool writable, ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) { }

    public override TimeBoundaryPeriod TimeBoundaryPeriod => TimeBoundaryPeriod.OneWeek;
}

public class MonthlyCandleDataBucket<TEntry> : PQCandleDataBucket<MonthlyCandleDataBucket<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry, ICandle
{
    public MonthlyCandleDataBucket
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset,
        bool writable, ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) { }

    public override TimeBoundaryPeriod TimeBoundaryPeriod => TimeBoundaryPeriod.OneMonth;
}

public class YearlyCandleDataBucket<TEntry> : PQCandleDataBucket<YearlyCandleDataBucket<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry, ICandle
{
    public YearlyCandleDataBucket
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset,
        bool writable, ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) { }

    public override TimeBoundaryPeriod TimeBoundaryPeriod => TimeBoundaryPeriod.OneYear;
}

public class DecenniallyCandleDataBucket<TEntry> : PQCandleDataBucket<DecenniallyCandleDataBucket<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry, ICandle
{
    public DecenniallyCandleDataBucket
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset,
        bool writable, ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) { }

    public override TimeBoundaryPeriod TimeBoundaryPeriod => TimeBoundaryPeriod.OneDecade;
}

public class UnlimitedCandleDataBucket<TEntry> : PQCandleDataBucket<UnlimitedCandleDataBucket<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry, ICandle
{
    public UnlimitedCandleDataBucket
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset,
        bool writable, ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) { }

    public override TimeBoundaryPeriod TimeBoundaryPeriod => TimeBoundaryPeriod.Tick;
}
