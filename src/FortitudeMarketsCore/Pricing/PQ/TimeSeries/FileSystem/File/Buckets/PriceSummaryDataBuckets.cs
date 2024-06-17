// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory.UnmanagedMemory.MemoryMappedFiles;
using FortitudeIO.TimeSeries;
using FortitudeIO.TimeSeries.FileSystem.File.Session;
using FortitudeMarketsApi.Pricing.TimeSeries;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.TimeSeries.FileSystem.File.Buckets;

public class HourlyPriceSummaryDataBucket<TEntry> : PQPriceSummaryDataBucket<TEntry, HourlyPriceSummaryDataBucket<TEntry>>
    where TEntry : ITimeSeriesEntry<TEntry>, IPricePeriodSummary
{
    public HourlyPriceSummaryDataBucket
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset,
        bool writable, ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) { }

    public override TimeSeriesPeriod TimeSeriesPeriod => TimeSeriesPeriod.OneHour;
}

public class DailyPriceSummaryDataBucket<TEntry> : PQPriceSummaryDataBucket<TEntry, DailyPriceSummaryDataBucket<TEntry>>
    where TEntry : ITimeSeriesEntry<TEntry>, IPricePeriodSummary
{
    public DailyPriceSummaryDataBucket
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset,
        bool writable, ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) { }

    public override TimeSeriesPeriod TimeSeriesPeriod => TimeSeriesPeriod.OneDay;
}

public class WeeklyPriceSummaryDataBucket<TEntry> : PQPriceSummaryDataBucket<TEntry, WeeklyPriceSummaryDataBucket<TEntry>>
    where TEntry : ITimeSeriesEntry<TEntry>, IPricePeriodSummary
{
    public WeeklyPriceSummaryDataBucket
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset,
        bool writable, ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) { }

    public override TimeSeriesPeriod TimeSeriesPeriod => TimeSeriesPeriod.OneWeek;
}

public class MonthlyPriceSummaryDataBucket<TEntry> : PQPriceSummaryDataBucket<TEntry, MonthlyPriceSummaryDataBucket<TEntry>>
    where TEntry : ITimeSeriesEntry<TEntry>, IPricePeriodSummary
{
    public MonthlyPriceSummaryDataBucket
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset,
        bool writable, ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) { }

    public override TimeSeriesPeriod TimeSeriesPeriod => TimeSeriesPeriod.OneMonth;
}

public class YearlyPriceSummaryDataBucket<TEntry> : PQPriceSummaryDataBucket<TEntry, YearlyPriceSummaryDataBucket<TEntry>>
    where TEntry : ITimeSeriesEntry<TEntry>, IPricePeriodSummary
{
    public YearlyPriceSummaryDataBucket
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset,
        bool writable, ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) { }

    public override TimeSeriesPeriod TimeSeriesPeriod => TimeSeriesPeriod.OneMonth;
}

public class DecenniallyPriceSummaryDataBucket<TEntry> : PQPriceSummaryDataBucket<TEntry, DecenniallyPriceSummaryDataBucket<TEntry>>
    where TEntry : ITimeSeriesEntry<TEntry>, IPricePeriodSummary
{
    public DecenniallyPriceSummaryDataBucket
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset,
        bool writable, ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) { }

    public override TimeSeriesPeriod TimeSeriesPeriod => TimeSeriesPeriod.OneDecade;
}
