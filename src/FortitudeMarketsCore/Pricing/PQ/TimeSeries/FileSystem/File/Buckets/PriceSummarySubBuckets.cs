// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory.UnmanagedMemory.MemoryMappedFiles;
using FortitudeIO.TimeSeries;
using FortitudeIO.TimeSeries.FileSystem.File.Session;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.TimeSeries.FileSystem.File.Buckets;

public class DailyToHourlyPriceSummarySubBuckets :
    PriceSummarySubBucket<DailyToHourlyPriceSummarySubBuckets, HourlyPriceSummaryDataBucket>
{
    public DailyToHourlyPriceSummarySubBuckets
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable,
        ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) =>
        IndexCount = 24;

    public override TimeSeriesPeriod TimeSeriesPeriod => TimeSeriesPeriod.OneDay;
}

public class WeeklyToDailyPriceSummarySubBuckets :
    PriceSummarySubBucket<WeeklyToDailyPriceSummarySubBuckets, DailyPriceSummaryDataBucket>
{
    public WeeklyToDailyPriceSummarySubBuckets
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable,
        ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) =>
        IndexCount = 7;

    public override TimeSeriesPeriod TimeSeriesPeriod => TimeSeriesPeriod.OneWeek;
}

public class WeeklyToFourHourlyPriceSummarySubBuckets :
    PriceSummarySubBucket<WeeklyToFourHourlyPriceSummarySubBuckets, FourHourlyPriceSummaryDataBucket>
{
    public WeeklyToFourHourlyPriceSummarySubBuckets
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable,
        ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) =>
        IndexCount = 42;

    public override TimeSeriesPeriod TimeSeriesPeriod => TimeSeriesPeriod.OneWeek;
}

public class MonthlyToFourHourlyPriceSummarySubBuckets :
    PriceSummarySubBucket<MonthlyToFourHourlyPriceSummarySubBuckets, FourHourlyPriceSummaryDataBucket>
{
    public MonthlyToFourHourlyPriceSummarySubBuckets
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable,
        ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) =>
        IndexCount = 186;

    public override TimeSeriesPeriod TimeSeriesPeriod => TimeSeriesPeriod.OneMonth;
}

public class MonthlyToDailyPriceSummarySubBuckets : PriceSummarySubBucket<MonthlyToDailyPriceSummarySubBuckets,
    DailyPriceSummaryDataBucket>
{
    public MonthlyToDailyPriceSummarySubBuckets
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable,
        ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) =>
        IndexCount = 31;

    public override TimeSeriesPeriod TimeSeriesPeriod => TimeSeriesPeriod.OneMonth;
}

public class MonthlyToWeeklyPriceSummarySubBuckets : PriceSummarySubBucket<MonthlyToWeeklyPriceSummarySubBuckets,
    WeeklyPriceSummaryDataBucket>
{
    public MonthlyToWeeklyPriceSummarySubBuckets
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable,
        ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) =>
        IndexCount = 4;

    public override TimeSeriesPeriod TimeSeriesPeriod => TimeSeriesPeriod.OneMonth;
}

public class YearlyToWeeklyPriceSummarySubBuckets : PriceSummarySubBucket<YearlyToWeeklyPriceSummarySubBuckets,
    WeeklyPriceSummaryDataBucket>
{
    public YearlyToWeeklyPriceSummarySubBuckets
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable,
        ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) =>
        IndexCount = 53;

    public override TimeSeriesPeriod TimeSeriesPeriod => TimeSeriesPeriod.OneYear;
}

public class YearlyToMonthlyPriceSummarySubBuckets : PriceSummarySubBucket<YearlyToMonthlyPriceSummarySubBuckets,
    MonthlyPriceSummaryDataBucket>
{
    public YearlyToMonthlyPriceSummarySubBuckets
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable,
        ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) =>
        IndexCount = 12;

    public override TimeSeriesPeriod TimeSeriesPeriod => TimeSeriesPeriod.OneYear;
}

public class YearlyToDailyPriceSummarySubBuckets : PriceSummarySubBucket<YearlyToDailyPriceSummarySubBuckets,
    DailyPriceSummaryDataBucket>
{
    public YearlyToDailyPriceSummarySubBuckets
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable,
        ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) =>
        IndexCount = 366;

    public override TimeSeriesPeriod TimeSeriesPeriod => TimeSeriesPeriod.OneYear;
}

public class DecenniallyToWeeklyPriceSummarySubBuckets : PriceSummarySubBucket<DecenniallyToWeeklyPriceSummarySubBuckets,
    WeeklyPriceSummaryDataBucket>
{
    public DecenniallyToWeeklyPriceSummarySubBuckets
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable,
        ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) =>
        IndexCount = 523;

    public override TimeSeriesPeriod TimeSeriesPeriod => TimeSeriesPeriod.OneDecade;
}

public class DecenniallyToMonthlyPriceSummarySubBuckets : PriceSummarySubBucket<DecenniallyToMonthlyPriceSummarySubBuckets,
    MonthlyPriceSummaryDataBucket>
{
    public DecenniallyToMonthlyPriceSummarySubBuckets
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable,
        ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) =>
        IndexCount = 120;

    public override TimeSeriesPeriod TimeSeriesPeriod => TimeSeriesPeriod.OneDecade;
}

public class DecenniallyToYearlyPriceSummarySubBuckets : PriceSummarySubBucket<DecenniallyToYearlyPriceSummarySubBuckets,
    YearlyPriceSummaryDataBucket>
{
    public DecenniallyToYearlyPriceSummarySubBuckets
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable,
        ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) =>
        IndexCount = 10;

    public override TimeSeriesPeriod TimeSeriesPeriod => TimeSeriesPeriod.OneDecade;
}

public class UnlimitedToYearlyPriceSummarySubBuckets : PriceSummarySubBucket<UnlimitedToYearlyPriceSummarySubBuckets,
    YearlyPriceSummaryDataBucket>
{
    public UnlimitedToYearlyPriceSummarySubBuckets
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable,
        ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) =>
        IndexCount = 100;

    public override TimeSeriesPeriod TimeSeriesPeriod => TimeSeriesPeriod.None;
}

public class UnlimitedToDecenniallyPriceSummarySubBuckets : PriceSummarySubBucket<UnlimitedToDecenniallyPriceSummarySubBuckets
  , DecenniallyPriceSummaryDataBucket>
{
    public UnlimitedToDecenniallyPriceSummarySubBuckets
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable,
        ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) =>
        IndexCount = 100;

    public override TimeSeriesPeriod TimeSeriesPeriod => TimeSeriesPeriod.None;
}
