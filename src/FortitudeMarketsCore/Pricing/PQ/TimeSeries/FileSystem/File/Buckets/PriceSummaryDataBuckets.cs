// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory.UnmanagedMemory.MemoryMappedFiles;
using FortitudeIO.TimeSeries;
using FortitudeIO.TimeSeries.FileSystem.File.Session;
using FortitudeMarketsApi.Pricing.Summaries;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.TimeSeries.FileSystem.File.Buckets;

public class HourlyPriceSummaryDataBucket<TEntry> : PQPriceSummaryDataBucket<HourlyPriceSummaryDataBucket<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry<TEntry>, IPricePeriodSummary
{
    public HourlyPriceSummaryDataBucket
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset,
        bool writable, ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) { }

    public override TimeSeriesPeriod TimeSeriesPeriod => TimeSeriesPeriod.OneHour;
}

public class FourHourlyPriceSummaryDataBucket<TEntry> : PQPriceSummaryDataBucket<FourHourlyPriceSummaryDataBucket<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry<TEntry>, IPricePeriodSummary
{
    public FourHourlyPriceSummaryDataBucket
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset,
        bool writable, ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) { }

    public override TimeSeriesPeriod TimeSeriesPeriod => TimeSeriesPeriod.FourHours;
}

public class DailyPriceSummaryDataBucket<TEntry> : PQPriceSummaryDataBucket<DailyPriceSummaryDataBucket<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry<TEntry>, IPricePeriodSummary
{
    public DailyPriceSummaryDataBucket
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset,
        bool writable, ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) { }

    public override TimeSeriesPeriod TimeSeriesPeriod => TimeSeriesPeriod.OneDay;
}

public class WeeklyPriceSummaryDataBucket<TEntry> : PQPriceSummaryDataBucket<WeeklyPriceSummaryDataBucket<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry<TEntry>, IPricePeriodSummary
{
    public WeeklyPriceSummaryDataBucket
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset,
        bool writable, ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) { }

    public override TimeSeriesPeriod TimeSeriesPeriod => TimeSeriesPeriod.OneWeek;
}

public class MonthlyPriceSummaryDataBucket<TEntry> : PQPriceSummaryDataBucket<MonthlyPriceSummaryDataBucket<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry<TEntry>, IPricePeriodSummary
{
    public MonthlyPriceSummaryDataBucket
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset,
        bool writable, ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) { }

    public override TimeSeriesPeriod TimeSeriesPeriod => TimeSeriesPeriod.OneMonth;
}

public class YearlyPriceSummaryDataBucket<TEntry> : PQPriceSummaryDataBucket<YearlyPriceSummaryDataBucket<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry<TEntry>, IPricePeriodSummary
{
    public YearlyPriceSummaryDataBucket
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset,
        bool writable, ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) { }

    public override TimeSeriesPeriod TimeSeriesPeriod => TimeSeriesPeriod.OneYear;
}

public class DecenniallyPriceSummaryDataBucket<TEntry> : PQPriceSummaryDataBucket<DecenniallyPriceSummaryDataBucket<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry<TEntry>, IPricePeriodSummary
{
    public DecenniallyPriceSummaryDataBucket
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset,
        bool writable, ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) { }

    public override TimeSeriesPeriod TimeSeriesPeriod => TimeSeriesPeriod.OneDecade;
}

public class UnlimitedPriceSummaryDataBucket<TEntry> : PQPriceSummaryDataBucket<UnlimitedPriceSummaryDataBucket<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry<TEntry>, IPricePeriodSummary
{
    public UnlimitedPriceSummaryDataBucket
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset,
        bool writable, ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) { }

    public override TimeSeriesPeriod TimeSeriesPeriod => TimeSeriesPeriod.None;
}
