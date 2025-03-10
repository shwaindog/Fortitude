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

public class DailyToHourlyPriceSummarySubBuckets<TEntry> :
    PriceSummarySubBucket<DailyToHourlyPriceSummarySubBuckets<TEntry>, HourlyPriceSummaryDataBucket<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry, IPricePeriodSummary
{
    public DailyToHourlyPriceSummarySubBuckets
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable,
        ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) =>
        IndexCount = 24;

    public override TimeBoundaryPeriod TimeBoundaryPeriod => TimeBoundaryPeriod.OneDay;
}

public class WeeklyToDailyPriceSummarySubBuckets<TEntry> :
    PriceSummarySubBucket<WeeklyToDailyPriceSummarySubBuckets<TEntry>, DailyPriceSummaryDataBucket<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry, IPricePeriodSummary
{
    public WeeklyToDailyPriceSummarySubBuckets
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable,
        ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) =>
        IndexCount = 7;

    public override TimeBoundaryPeriod TimeBoundaryPeriod => TimeBoundaryPeriod.OneWeek;
}

public class WeeklyToFourHourlyPriceSummarySubBuckets<TEntry> :
    PriceSummarySubBucket<WeeklyToFourHourlyPriceSummarySubBuckets<TEntry>, FourHourlyPriceSummaryDataBucket<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry, IPricePeriodSummary
{
    public WeeklyToFourHourlyPriceSummarySubBuckets
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable,
        ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) =>
        IndexCount = 42;

    public override TimeBoundaryPeriod TimeBoundaryPeriod => TimeBoundaryPeriod.OneWeek;
}

public class MonthlyToFourHourlyPriceSummarySubBuckets<TEntry> :
    PriceSummarySubBucket<MonthlyToFourHourlyPriceSummarySubBuckets<TEntry>, FourHourlyPriceSummaryDataBucket<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry, IPricePeriodSummary
{
    public MonthlyToFourHourlyPriceSummarySubBuckets
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable,
        ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) =>
        IndexCount = 186;

    public override TimeBoundaryPeriod TimeBoundaryPeriod => TimeBoundaryPeriod.OneMonth;
}

public class MonthlyToDailyPriceSummarySubBuckets<TEntry> : PriceSummarySubBucket<MonthlyToDailyPriceSummarySubBuckets<TEntry>,
    DailyPriceSummaryDataBucket<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry, IPricePeriodSummary
{
    public MonthlyToDailyPriceSummarySubBuckets
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable,
        ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) =>
        IndexCount = 31;

    public override TimeBoundaryPeriod TimeBoundaryPeriod => TimeBoundaryPeriod.OneMonth;
}

public class MonthlyToWeeklyPriceSummarySubBuckets<TEntry> : PriceSummarySubBucket<MonthlyToWeeklyPriceSummarySubBuckets<TEntry>,
    WeeklyPriceSummaryDataBucket<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry, IPricePeriodSummary
{
    public MonthlyToWeeklyPriceSummarySubBuckets
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable,
        ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) =>
        IndexCount = 4;

    public override TimeBoundaryPeriod TimeBoundaryPeriod => TimeBoundaryPeriod.OneMonth;
}

public class YearlyToWeeklyPriceSummarySubBuckets<TEntry> : PriceSummarySubBucket<YearlyToWeeklyPriceSummarySubBuckets<TEntry>,
    WeeklyPriceSummaryDataBucket<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry, IPricePeriodSummary
{
    public YearlyToWeeklyPriceSummarySubBuckets
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable,
        ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) =>
        IndexCount = 53;

    public override TimeBoundaryPeriod TimeBoundaryPeriod => TimeBoundaryPeriod.OneYear;
}

public class YearlyToMonthlyPriceSummarySubBuckets<TEntry> : PriceSummarySubBucket<YearlyToMonthlyPriceSummarySubBuckets<TEntry>,
    MonthlyPriceSummaryDataBucket<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry, IPricePeriodSummary
{
    public YearlyToMonthlyPriceSummarySubBuckets
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable,
        ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) =>
        IndexCount = 12;

    public override TimeBoundaryPeriod TimeBoundaryPeriod => TimeBoundaryPeriod.OneYear;
}

public class YearlyToDailyPriceSummarySubBuckets<TEntry> : PriceSummarySubBucket<YearlyToDailyPriceSummarySubBuckets<TEntry>,
    DailyPriceSummaryDataBucket<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry, IPricePeriodSummary
{
    public YearlyToDailyPriceSummarySubBuckets
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable,
        ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) =>
        IndexCount = 366;

    public override TimeBoundaryPeriod TimeBoundaryPeriod => TimeBoundaryPeriod.OneYear;
}

public class DecenniallyToWeeklyPriceSummarySubBuckets<TEntry> : PriceSummarySubBucket<DecenniallyToWeeklyPriceSummarySubBuckets<TEntry>,
    WeeklyPriceSummaryDataBucket<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry, IPricePeriodSummary
{
    public DecenniallyToWeeklyPriceSummarySubBuckets
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable,
        ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) =>
        IndexCount = 523;

    public override TimeBoundaryPeriod TimeBoundaryPeriod => TimeBoundaryPeriod.OneDecade;
}

public class DecenniallyToMonthlyPriceSummarySubBuckets<TEntry> : PriceSummarySubBucket<DecenniallyToMonthlyPriceSummarySubBuckets<TEntry>,
    MonthlyPriceSummaryDataBucket<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry, IPricePeriodSummary
{
    public DecenniallyToMonthlyPriceSummarySubBuckets
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable,
        ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) =>
        IndexCount = 120;

    public override TimeBoundaryPeriod TimeBoundaryPeriod => TimeBoundaryPeriod.OneDecade;
}

public class DecenniallyToYearlyPriceSummarySubBuckets<TEntry> : PriceSummarySubBucket<DecenniallyToYearlyPriceSummarySubBuckets<TEntry>,
    YearlyPriceSummaryDataBucket<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry, IPricePeriodSummary
{
    public DecenniallyToYearlyPriceSummarySubBuckets
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable,
        ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) =>
        IndexCount = 10;

    public override TimeBoundaryPeriod TimeBoundaryPeriod => TimeBoundaryPeriod.OneDecade;
}

public class UnlimitedToYearlyPriceSummarySubBuckets<TEntry> : PriceSummarySubBucket<UnlimitedToYearlyPriceSummarySubBuckets<TEntry>,
    YearlyPriceSummaryDataBucket<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry, IPricePeriodSummary
{
    public UnlimitedToYearlyPriceSummarySubBuckets
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable,
        ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) =>
        IndexCount = 100;

    public override TimeBoundaryPeriod TimeBoundaryPeriod => TimeBoundaryPeriod.Tick;
}

public class UnlimitedToDecenniallyPriceSummarySubBuckets<TEntry> : PriceSummarySubBucket<UnlimitedToDecenniallyPriceSummarySubBuckets<TEntry>
  , DecenniallyPriceSummaryDataBucket<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry, IPricePeriodSummary
{
    public UnlimitedToDecenniallyPriceSummarySubBuckets
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable,
        ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) =>
        IndexCount = 100;

    public override TimeBoundaryPeriod TimeBoundaryPeriod => TimeBoundaryPeriod.Tick;
}
