// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory.UnmanagedMemory.MemoryMappedFiles;
using FortitudeIO.TimeSeries;
using FortitudeIO.TimeSeries.FileSystem.File.Session;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.TimeSeries.FileSystem.File.Buckets;

public class HourlyPriceSummaryDataBucket : PQPriceSummaryDataBucket<HourlyPriceSummaryDataBucket>
{
    public HourlyPriceSummaryDataBucket
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset,
        bool writable, ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) { }

    public override TimeSeriesPeriod TimeSeriesPeriod => TimeSeriesPeriod.OneHour;
}

public class FourHourlyPriceSummaryDataBucket : PQPriceSummaryDataBucket<FourHourlyPriceSummaryDataBucket>
{
    public FourHourlyPriceSummaryDataBucket
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset,
        bool writable, ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) { }

    public override TimeSeriesPeriod TimeSeriesPeriod => TimeSeriesPeriod.FourHours;
}

public class DailyPriceSummaryDataBucket : PQPriceSummaryDataBucket<DailyPriceSummaryDataBucket>
{
    public DailyPriceSummaryDataBucket
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset,
        bool writable, ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) { }

    public override TimeSeriesPeriod TimeSeriesPeriod => TimeSeriesPeriod.OneDay;
}

public class WeeklyPriceSummaryDataBucket : PQPriceSummaryDataBucket<WeeklyPriceSummaryDataBucket>
{
    public WeeklyPriceSummaryDataBucket
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset,
        bool writable, ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) { }

    public override TimeSeriesPeriod TimeSeriesPeriod => TimeSeriesPeriod.OneWeek;
}

public class MonthlyPriceSummaryDataBucket : PQPriceSummaryDataBucket<MonthlyPriceSummaryDataBucket>
{
    public MonthlyPriceSummaryDataBucket
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset,
        bool writable, ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) { }

    public override TimeSeriesPeriod TimeSeriesPeriod => TimeSeriesPeriod.OneMonth;
}

public class YearlyPriceSummaryDataBucket : PQPriceSummaryDataBucket<YearlyPriceSummaryDataBucket>
{
    public YearlyPriceSummaryDataBucket
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset,
        bool writable, ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) { }

    public override TimeSeriesPeriod TimeSeriesPeriod => TimeSeriesPeriod.OneYear;
}

public class DecenniallyPriceSummaryDataBucket : PQPriceSummaryDataBucket<DecenniallyPriceSummaryDataBucket>
{
    public DecenniallyPriceSummaryDataBucket
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset,
        bool writable, ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) { }

    public override TimeSeriesPeriod TimeSeriesPeriod => TimeSeriesPeriod.OneDecade;
}

public class UnlimitedPriceSummaryDataBucket : PQPriceSummaryDataBucket<UnlimitedPriceSummaryDataBucket>
{
    public UnlimitedPriceSummaryDataBucket
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset,
        bool writable, ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) { }

    public override TimeSeriesPeriod TimeSeriesPeriod => TimeSeriesPeriod.None;
}
