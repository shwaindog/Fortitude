// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.MemoryPools.Buffers.ByteBuffers.UnmanagedMemory.MemoryMappedFiles;
using FortitudeIO.Storage.TimeSeries;
using FortitudeIO.Storage.TimeSeries.FileSystem.File.Session;
using FortitudeMarkets.Pricing.FeedEvents.Candles;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Storage.TimeSeries.FileSystem.File.Buckets;

public class DailyToHourlyCandleSubBuckets<TEntry> :
    CandleSubBucket<DailyToHourlyCandleSubBuckets<TEntry>, HourlyCandleDataBucket<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry, ICandle
{
    public DailyToHourlyCandleSubBuckets
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable,
        ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) =>
        IndexCount = 24;

    public override TimeBoundaryPeriod TimeBoundaryPeriod => TimeBoundaryPeriod.OneDay;
}

public class WeeklyToDailyCandleSubBuckets<TEntry> :
    CandleSubBucket<WeeklyToDailyCandleSubBuckets<TEntry>, DailyCandleDataBucket<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry, ICandle
{
    public WeeklyToDailyCandleSubBuckets
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable,
        ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) =>
        IndexCount = 7;

    public override TimeBoundaryPeriod TimeBoundaryPeriod => TimeBoundaryPeriod.OneWeek;
}

public class WeeklyToFourHourlyCandleSubBuckets<TEntry> :
    CandleSubBucket<WeeklyToFourHourlyCandleSubBuckets<TEntry>, FourHourlyCandleDataBucket<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry, ICandle
{
    public WeeklyToFourHourlyCandleSubBuckets
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable,
        ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) =>
        IndexCount = 42;

    public override TimeBoundaryPeriod TimeBoundaryPeriod => TimeBoundaryPeriod.OneWeek;
}

public class MonthlyToFourHourlyCandleSubBuckets<TEntry> :
    CandleSubBucket<MonthlyToFourHourlyCandleSubBuckets<TEntry>, FourHourlyCandleDataBucket<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry, ICandle
{
    public MonthlyToFourHourlyCandleSubBuckets
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable,
        ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) =>
        IndexCount = 186;

    public override TimeBoundaryPeriod TimeBoundaryPeriod => TimeBoundaryPeriod.OneMonth;
}

public class MonthlyToDailyCandleSubBuckets<TEntry> : CandleSubBucket<MonthlyToDailyCandleSubBuckets<TEntry>,
    DailyCandleDataBucket<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry, ICandle
{
    public MonthlyToDailyCandleSubBuckets
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable,
        ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) =>
        IndexCount = 31;

    public override TimeBoundaryPeriod TimeBoundaryPeriod => TimeBoundaryPeriod.OneMonth;
}

public class MonthlyToWeeklyCandleSubBuckets<TEntry> : CandleSubBucket<MonthlyToWeeklyCandleSubBuckets<TEntry>,
    WeeklyCandleDataBucket<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry, ICandle
{
    public MonthlyToWeeklyCandleSubBuckets
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable,
        ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) =>
        IndexCount = 4;

    public override TimeBoundaryPeriod TimeBoundaryPeriod => TimeBoundaryPeriod.OneMonth;
}

public class YearlyToWeeklyCandleSubBuckets<TEntry> : CandleSubBucket<YearlyToWeeklyCandleSubBuckets<TEntry>,
    WeeklyCandleDataBucket<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry, ICandle
{
    public YearlyToWeeklyCandleSubBuckets
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable,
        ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) =>
        IndexCount = 53;

    public override TimeBoundaryPeriod TimeBoundaryPeriod => TimeBoundaryPeriod.OneYear;
}

public class YearlyToMonthlyCandleSubBuckets<TEntry> : CandleSubBucket<YearlyToMonthlyCandleSubBuckets<TEntry>,
    MonthlyCandleDataBucket<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry, ICandle
{
    public YearlyToMonthlyCandleSubBuckets
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable,
        ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) =>
        IndexCount = 12;

    public override TimeBoundaryPeriod TimeBoundaryPeriod => TimeBoundaryPeriod.OneYear;
}

public class YearlyToDailyCandleSubBuckets<TEntry> : CandleSubBucket<YearlyToDailyCandleSubBuckets<TEntry>,
    DailyCandleDataBucket<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry, ICandle
{
    public YearlyToDailyCandleSubBuckets
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable,
        ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) =>
        IndexCount = 366;

    public override TimeBoundaryPeriod TimeBoundaryPeriod => TimeBoundaryPeriod.OneYear;
}

public class DecenniallyToWeeklyCandleSubBuckets<TEntry> : CandleSubBucket<DecenniallyToWeeklyCandleSubBuckets<TEntry>,
    WeeklyCandleDataBucket<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry, ICandle
{
    public DecenniallyToWeeklyCandleSubBuckets
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable,
        ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) =>
        IndexCount = 523;

    public override TimeBoundaryPeriod TimeBoundaryPeriod => TimeBoundaryPeriod.OneDecade;
}

public class DecenniallyToMonthlyCandleSubBuckets<TEntry> : CandleSubBucket<DecenniallyToMonthlyCandleSubBuckets<TEntry>,
    MonthlyCandleDataBucket<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry, ICandle
{
    public DecenniallyToMonthlyCandleSubBuckets
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable,
        ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) =>
        IndexCount = 120;

    public override TimeBoundaryPeriod TimeBoundaryPeriod => TimeBoundaryPeriod.OneDecade;
}

public class DecenniallyToYearlyCandleSubBuckets<TEntry> : CandleSubBucket<DecenniallyToYearlyCandleSubBuckets<TEntry>,
    YearlyCandleDataBucket<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry, ICandle
{
    public DecenniallyToYearlyCandleSubBuckets
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable,
        ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) =>
        IndexCount = 10;

    public override TimeBoundaryPeriod TimeBoundaryPeriod => TimeBoundaryPeriod.OneDecade;
}

public class UnlimitedToYearlyCandleSubBuckets<TEntry> : CandleSubBucket<UnlimitedToYearlyCandleSubBuckets<TEntry>,
    YearlyCandleDataBucket<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry, ICandle
{
    public UnlimitedToYearlyCandleSubBuckets
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable,
        ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) =>
        IndexCount = 100;

    public override TimeBoundaryPeriod TimeBoundaryPeriod => TimeBoundaryPeriod.Tick;
}

public class UnlimitedToDecenniallyCandleSubBuckets<TEntry> : CandleSubBucket<UnlimitedToDecenniallyCandleSubBuckets<TEntry>
  , DecenniallyCandleDataBucket<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry, ICandle
{
    public UnlimitedToDecenniallyCandleSubBuckets
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable,
        ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) =>
        IndexCount = 100;

    public override TimeBoundaryPeriod TimeBoundaryPeriod => TimeBoundaryPeriod.Tick;
}
