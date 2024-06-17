// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory.UnmanagedMemory.MemoryMappedFiles;
using FortitudeIO.TimeSeries;
using FortitudeIO.TimeSeries.FileSystem.File.Session;
using FortitudeMarketsApi.Pricing.TimeSeries;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.TimeSeries.FileSystem.File.Buckets;

public class DailyToHourlyPriceSummarySubBuckets<TEntry> : PriceSummarySubBucket<TEntry, DailyToHourlyPriceSummarySubBuckets<TEntry>,
    HourlyPriceSummaryDataBucket<TEntry>>
    where TEntry : ITimeSeriesEntry<TEntry>, IPricePeriodSummary
{
    public DailyToHourlyPriceSummarySubBuckets
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable,
        ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) =>
        IndexCount = 24;

    public override TimeSeriesPeriod TimeSeriesPeriod => TimeSeriesPeriod.OneDay;
}

public class WeeklyToDailyPriceSummarySubBuckets<TEntry> : PriceSummarySubBucket<TEntry, WeeklyToDailyPriceSummarySubBuckets<TEntry>,
    DailyPriceSummaryDataBucket<TEntry>>
    where TEntry : ITimeSeriesEntry<TEntry>, IPricePeriodSummary
{
    public WeeklyToDailyPriceSummarySubBuckets
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable,
        ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) =>
        IndexCount = 7;

    public override TimeSeriesPeriod TimeSeriesPeriod => TimeSeriesPeriod.OneWeek;
}

public class MonthlyToDailyPriceSummarySubBuckets<TEntry> : PriceSummarySubBucket<TEntry, MonthlyToDailyPriceSummarySubBuckets<TEntry>,
    DailyPriceSummaryDataBucket<TEntry>>
    where TEntry : ITimeSeriesEntry<TEntry>, IPricePeriodSummary
{
    public MonthlyToDailyPriceSummarySubBuckets
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable,
        ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) =>
        IndexCount = 31;

    public override TimeSeriesPeriod TimeSeriesPeriod => TimeSeriesPeriod.OneMonth;
}

public class MonthlyToWeeklyPriceSummarySubBuckets<TEntry> : PriceSummarySubBucket<TEntry, MonthlyToWeeklyPriceSummarySubBuckets<TEntry>,
    WeeklyPriceSummaryDataBucket<TEntry>>
    where TEntry : ITimeSeriesEntry<TEntry>, IPricePeriodSummary
{
    public MonthlyToWeeklyPriceSummarySubBuckets
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable,
        ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) =>
        IndexCount = 4;

    public override TimeSeriesPeriod TimeSeriesPeriod => TimeSeriesPeriod.OneMonth;
}

public class YearlyToWeeklyPriceSummarySubBuckets<TEntry> : PriceSummarySubBucket<TEntry, YearlyToWeeklyPriceSummarySubBuckets<TEntry>,
    WeeklyPriceSummaryDataBucket<TEntry>>
    where TEntry : ITimeSeriesEntry<TEntry>, IPricePeriodSummary
{
    public YearlyToWeeklyPriceSummarySubBuckets
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable,
        ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) =>
        IndexCount = 53;

    public override TimeSeriesPeriod TimeSeriesPeriod => TimeSeriesPeriod.OneYear;
}

public class YearlyToMonthlyPriceSummarySubBuckets<TEntry> : PriceSummarySubBucket<TEntry, YearlyToMonthlyPriceSummarySubBuckets<TEntry>,
    MonthlyPriceSummaryDataBucket<TEntry>>
    where TEntry : ITimeSeriesEntry<TEntry>, IPricePeriodSummary
{
    public YearlyToMonthlyPriceSummarySubBuckets
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable,
        ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) =>
        IndexCount = 12;

    public override TimeSeriesPeriod TimeSeriesPeriod => TimeSeriesPeriod.OneYear;
}

public class YearlyToDailyPriceSummarySubBuckets<TEntry> : PriceSummarySubBucket<TEntry, YearlyToDailyPriceSummarySubBuckets<TEntry>,
    DailyPriceSummaryDataBucket<TEntry>>
    where TEntry : ITimeSeriesEntry<TEntry>, IPricePeriodSummary
{
    public YearlyToDailyPriceSummarySubBuckets
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable,
        ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) =>
        IndexCount = 366;

    public override TimeSeriesPeriod TimeSeriesPeriod => TimeSeriesPeriod.OneYear;
}

public class DecenniallyToWeeklyPriceSummarySubBuckets<TEntry> : PriceSummarySubBucket<TEntry, DecenniallyToWeeklyPriceSummarySubBuckets<TEntry>,
    WeeklyPriceSummaryDataBucket<TEntry>>
    where TEntry : ITimeSeriesEntry<TEntry>, IPricePeriodSummary
{
    public DecenniallyToWeeklyPriceSummarySubBuckets
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable,
        ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) =>
        IndexCount = 523;

    public override TimeSeriesPeriod TimeSeriesPeriod => TimeSeriesPeriod.OneDecade;
}

public class DecenniallyToMonthlyPriceSummarySubBuckets<TEntry> : PriceSummarySubBucket<TEntry, DecenniallyToMonthlyPriceSummarySubBuckets<TEntry>,
    MonthlyPriceSummaryDataBucket<TEntry>>
    where TEntry : ITimeSeriesEntry<TEntry>, IPricePeriodSummary
{
    public DecenniallyToMonthlyPriceSummarySubBuckets
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable,
        ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) =>
        IndexCount = 120;

    public override TimeSeriesPeriod TimeSeriesPeriod => TimeSeriesPeriod.OneDecade;
}

public class DecenniallyToYearlyPriceSummarySubBuckets<TEntry> : PriceSummarySubBucket<TEntry, DecenniallyToYearlyPriceSummarySubBuckets<TEntry>,
    YearlyPriceSummaryDataBucket<TEntry>>
    where TEntry : ITimeSeriesEntry<TEntry>, IPricePeriodSummary
{
    public DecenniallyToYearlyPriceSummarySubBuckets
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable,
        ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) =>
        IndexCount = 10;

    public override TimeSeriesPeriod TimeSeriesPeriod => TimeSeriesPeriod.OneDecade;
}

public class UnlimitedToYearlyPriceSummarySubBuckets<TEntry> : PriceSummarySubBucket<TEntry, UnlimitedToYearlyPriceSummarySubBuckets<TEntry>,
    YearlyPriceSummaryDataBucket<TEntry>>
    where TEntry : ITimeSeriesEntry<TEntry>, IPricePeriodSummary
{
    public UnlimitedToYearlyPriceSummarySubBuckets
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable,
        ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) =>
        IndexCount = 100;

    public override TimeSeriesPeriod TimeSeriesPeriod => TimeSeriesPeriod.None;
}

public class UnlimitedToDecenniallyPriceSummarySubBuckets<TEntry> : PriceSummarySubBucket<TEntry, UnlimitedToDecenniallyPriceSummarySubBuckets<TEntry>
   ,
    YearlyPriceSummaryDataBucket<TEntry>>
    where TEntry : ITimeSeriesEntry<TEntry>, IPricePeriodSummary
{
    public UnlimitedToDecenniallyPriceSummarySubBuckets
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable,
        ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) =>
        IndexCount = 100;

    public override TimeSeriesPeriod TimeSeriesPeriod => TimeSeriesPeriod.None;
}
