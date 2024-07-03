// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory.UnmanagedMemory.MemoryMappedFiles;
using FortitudeIO.TimeSeries;
using FortitudeIO.TimeSeries.FileSystem.File.Session;
using FortitudeMarketsApi.Pricing.Summaries;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.TimeSeries.FileSystem.File.Buckets;

public class DailyToHourlyPriceSummarySubBuckets<TEntry> :
    PriceSummarySubBucket<DailyToHourlyPriceSummarySubBuckets<TEntry>, HourlyPriceSummaryDataBucket<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry<TEntry>, IPricePeriodSummary
{
    public DailyToHourlyPriceSummarySubBuckets
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable,
        ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) =>
        IndexCount = 24;

    public override TimeSeriesPeriod TimeSeriesPeriod => TimeSeriesPeriod.OneDay;
}

public class WeeklyToDailyPriceSummarySubBuckets<TEntry> :
    PriceSummarySubBucket<WeeklyToDailyPriceSummarySubBuckets<TEntry>, DailyPriceSummaryDataBucket<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry<TEntry>, IPricePeriodSummary
{
    public WeeklyToDailyPriceSummarySubBuckets
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable,
        ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) =>
        IndexCount = 7;

    public override TimeSeriesPeriod TimeSeriesPeriod => TimeSeriesPeriod.OneWeek;
}

public class WeeklyToFourHourlyPriceSummarySubBuckets<TEntry> :
    PriceSummarySubBucket<WeeklyToFourHourlyPriceSummarySubBuckets<TEntry>, FourHourlyPriceSummaryDataBucket<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry<TEntry>, IPricePeriodSummary
{
    public WeeklyToFourHourlyPriceSummarySubBuckets
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable,
        ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) =>
        IndexCount = 42;

    public override TimeSeriesPeriod TimeSeriesPeriod => TimeSeriesPeriod.OneWeek;
}

public class MonthlyToFourHourlyPriceSummarySubBuckets<TEntry> :
    PriceSummarySubBucket<MonthlyToFourHourlyPriceSummarySubBuckets<TEntry>, FourHourlyPriceSummaryDataBucket<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry<TEntry>, IPricePeriodSummary
{
    public MonthlyToFourHourlyPriceSummarySubBuckets
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable,
        ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) =>
        IndexCount = 186;

    public override TimeSeriesPeriod TimeSeriesPeriod => TimeSeriesPeriod.OneMonth;
}

public class MonthlyToDailyPriceSummarySubBuckets<TEntry> : PriceSummarySubBucket<MonthlyToDailyPriceSummarySubBuckets<TEntry>,
    DailyPriceSummaryDataBucket<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry<TEntry>, IPricePeriodSummary
{
    public MonthlyToDailyPriceSummarySubBuckets
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable,
        ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) =>
        IndexCount = 31;

    public override TimeSeriesPeriod TimeSeriesPeriod => TimeSeriesPeriod.OneMonth;
}

public class MonthlyToWeeklyPriceSummarySubBuckets<TEntry> : PriceSummarySubBucket<MonthlyToWeeklyPriceSummarySubBuckets<TEntry>,
    WeeklyPriceSummaryDataBucket<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry<TEntry>, IPricePeriodSummary
{
    public MonthlyToWeeklyPriceSummarySubBuckets
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable,
        ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) =>
        IndexCount = 4;

    public override TimeSeriesPeriod TimeSeriesPeriod => TimeSeriesPeriod.OneMonth;
}

public class YearlyToWeeklyPriceSummarySubBuckets<TEntry> : PriceSummarySubBucket<YearlyToWeeklyPriceSummarySubBuckets<TEntry>,
    WeeklyPriceSummaryDataBucket<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry<TEntry>, IPricePeriodSummary
{
    public YearlyToWeeklyPriceSummarySubBuckets
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable,
        ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) =>
        IndexCount = 53;

    public override TimeSeriesPeriod TimeSeriesPeriod => TimeSeriesPeriod.OneYear;
}

public class YearlyToMonthlyPriceSummarySubBuckets<TEntry> : PriceSummarySubBucket<YearlyToMonthlyPriceSummarySubBuckets<TEntry>,
    MonthlyPriceSummaryDataBucket<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry<TEntry>, IPricePeriodSummary
{
    public YearlyToMonthlyPriceSummarySubBuckets
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable,
        ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) =>
        IndexCount = 12;

    public override TimeSeriesPeriod TimeSeriesPeriod => TimeSeriesPeriod.OneYear;
}

public class YearlyToDailyPriceSummarySubBuckets<TEntry> : PriceSummarySubBucket<YearlyToDailyPriceSummarySubBuckets<TEntry>,
    DailyPriceSummaryDataBucket<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry<TEntry>, IPricePeriodSummary
{
    public YearlyToDailyPriceSummarySubBuckets
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable,
        ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) =>
        IndexCount = 366;

    public override TimeSeriesPeriod TimeSeriesPeriod => TimeSeriesPeriod.OneYear;
}

public class DecenniallyToWeeklyPriceSummarySubBuckets<TEntry> : PriceSummarySubBucket<DecenniallyToWeeklyPriceSummarySubBuckets<TEntry>,
    WeeklyPriceSummaryDataBucket<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry<TEntry>, IPricePeriodSummary
{
    public DecenniallyToWeeklyPriceSummarySubBuckets
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable,
        ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) =>
        IndexCount = 523;

    public override TimeSeriesPeriod TimeSeriesPeriod => TimeSeriesPeriod.OneDecade;
}

public class DecenniallyToMonthlyPriceSummarySubBuckets<TEntry> : PriceSummarySubBucket<DecenniallyToMonthlyPriceSummarySubBuckets<TEntry>,
    MonthlyPriceSummaryDataBucket<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry<TEntry>, IPricePeriodSummary
{
    public DecenniallyToMonthlyPriceSummarySubBuckets
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable,
        ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) =>
        IndexCount = 120;

    public override TimeSeriesPeriod TimeSeriesPeriod => TimeSeriesPeriod.OneDecade;
}

public class DecenniallyToYearlyPriceSummarySubBuckets<TEntry> : PriceSummarySubBucket<DecenniallyToYearlyPriceSummarySubBuckets<TEntry>,
    YearlyPriceSummaryDataBucket<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry<TEntry>, IPricePeriodSummary
{
    public DecenniallyToYearlyPriceSummarySubBuckets
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable,
        ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) =>
        IndexCount = 10;

    public override TimeSeriesPeriod TimeSeriesPeriod => TimeSeriesPeriod.OneDecade;
}

public class UnlimitedToYearlyPriceSummarySubBuckets<TEntry> : PriceSummarySubBucket<UnlimitedToYearlyPriceSummarySubBuckets<TEntry>,
    YearlyPriceSummaryDataBucket<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry<TEntry>, IPricePeriodSummary
{
    public UnlimitedToYearlyPriceSummarySubBuckets
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable,
        ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) =>
        IndexCount = 100;

    public override TimeSeriesPeriod TimeSeriesPeriod => TimeSeriesPeriod.None;
}

public class UnlimitedToDecenniallyPriceSummarySubBuckets<TEntry> : PriceSummarySubBucket<UnlimitedToDecenniallyPriceSummarySubBuckets<TEntry>
  , DecenniallyPriceSummaryDataBucket<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry<TEntry>, IPricePeriodSummary
{
    public UnlimitedToDecenniallyPriceSummarySubBuckets
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable,
        ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) =>
        IndexCount = 100;

    public override TimeSeriesPeriod TimeSeriesPeriod => TimeSeriesPeriod.None;
}
