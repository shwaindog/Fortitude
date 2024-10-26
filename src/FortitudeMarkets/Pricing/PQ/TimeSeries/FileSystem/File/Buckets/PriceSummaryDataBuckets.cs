// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory.UnmanagedMemory.MemoryMappedFiles;
using FortitudeIO.TimeSeries;
using FortitudeIO.TimeSeries.FileSystem.File.Session;
using FortitudeMarkets.Pricing.Summaries;

#endregion

namespace FortitudeMarkets.Pricing.PQ.TimeSeries.FileSystem.File.Buckets;

public class HourlyPriceSummaryDataBucket<TEntry> : PQPriceSummaryDataBucket<HourlyPriceSummaryDataBucket<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry, IPricePeriodSummary
{
    public HourlyPriceSummaryDataBucket
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset,
        bool writable, ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) { }

    public override TimeBoundaryPeriod TimeBoundaryPeriod => TimeBoundaryPeriod.OneHour;
}

public class FourHourlyPriceSummaryDataBucket<TEntry> : PQPriceSummaryDataBucket<FourHourlyPriceSummaryDataBucket<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry, IPricePeriodSummary
{
    public FourHourlyPriceSummaryDataBucket
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset,
        bool writable, ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) { }

    public override TimeBoundaryPeriod TimeBoundaryPeriod => TimeBoundaryPeriod.FourHours;
}

public class DailyPriceSummaryDataBucket<TEntry> : PQPriceSummaryDataBucket<DailyPriceSummaryDataBucket<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry, IPricePeriodSummary
{
    public DailyPriceSummaryDataBucket
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset,
        bool writable, ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) { }

    public override TimeBoundaryPeriod TimeBoundaryPeriod => TimeBoundaryPeriod.OneDay;
}

public class WeeklyPriceSummaryDataBucket<TEntry> : PQPriceSummaryDataBucket<WeeklyPriceSummaryDataBucket<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry, IPricePeriodSummary
{
    public WeeklyPriceSummaryDataBucket
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset,
        bool writable, ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) { }

    public override TimeBoundaryPeriod TimeBoundaryPeriod => TimeBoundaryPeriod.OneWeek;
}

public class MonthlyPriceSummaryDataBucket<TEntry> : PQPriceSummaryDataBucket<MonthlyPriceSummaryDataBucket<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry, IPricePeriodSummary
{
    public MonthlyPriceSummaryDataBucket
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset,
        bool writable, ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) { }

    public override TimeBoundaryPeriod TimeBoundaryPeriod => TimeBoundaryPeriod.OneMonth;
}

public class YearlyPriceSummaryDataBucket<TEntry> : PQPriceSummaryDataBucket<YearlyPriceSummaryDataBucket<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry, IPricePeriodSummary
{
    public YearlyPriceSummaryDataBucket
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset,
        bool writable, ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) { }

    public override TimeBoundaryPeriod TimeBoundaryPeriod => TimeBoundaryPeriod.OneYear;
}

public class DecenniallyPriceSummaryDataBucket<TEntry> : PQPriceSummaryDataBucket<DecenniallyPriceSummaryDataBucket<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry, IPricePeriodSummary
{
    public DecenniallyPriceSummaryDataBucket
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset,
        bool writable, ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) { }

    public override TimeBoundaryPeriod TimeBoundaryPeriod => TimeBoundaryPeriod.OneDecade;
}

public class UnlimitedPriceSummaryDataBucket<TEntry> : PQPriceSummaryDataBucket<UnlimitedPriceSummaryDataBucket<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry, IPricePeriodSummary
{
    public UnlimitedPriceSummaryDataBucket
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset,
        bool writable, ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) { }

    public override TimeBoundaryPeriod TimeBoundaryPeriod => TimeBoundaryPeriod.None;
}
