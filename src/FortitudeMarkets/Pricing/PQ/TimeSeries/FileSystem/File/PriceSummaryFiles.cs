// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory.UnmanagedMemory.MemoryMappedFiles;
using FortitudeIO.TimeSeries;
using FortitudeIO.TimeSeries.FileSystem.File;
using FortitudeIO.TimeSeries.FileSystem.File.Header;
using FortitudeMarkets.Pricing.FeedEvents.Candles;
using FortitudeMarkets.Pricing.PQ.TimeSeries.FileSystem.File.Buckets;

#endregion

namespace FortitudeMarkets.Pricing.PQ.TimeSeries.FileSystem.File;

public class WeeklyDailyHourlyCandleTimeSeriesFile<TEntry> :
    CandleTimeSeriesFile<WeeklyDailyHourlyCandleTimeSeriesFile<TEntry>, DailyToHourlyCandleSubBuckets<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry, ICandle
{
    public WeeklyDailyHourlyCandleTimeSeriesFile(PagedMemoryMappedFile pagedMemoryMappedFile, IMutableTimeSeriesFileHeader header)
        : base(pagedMemoryMappedFile, header) { }

    public WeeklyDailyHourlyCandleTimeSeriesFile(PriceTimeSeriesFileParameters sourceTickerTimeSeriesFileParams)
        : base(sourceTickerTimeSeriesFileParams.SetFilePeriod(TimeBoundaryPeriod.OneWeek)
                                               .AssertTimeSeriesEntryType(InstrumentType.Candle)
                                               .SetFileFlags(FileFlags.HasSubFileHeader)
                                               .SetInternalIndexSize(7)
                                               .SetInitialFileSize(512 * 1024)) { }

    public static WeeklyDailyHourlyCandleTimeSeriesFile<TEntry> OpenExistingTimeSeriesFile
        (FileInfo file) =>
        OpenExistingTimeSeriesFile(file.FullName);
}

public class WeeklyFourHourlyCandleTimeSeriesFile<TEntry> :
    CandleTimeSeriesFile<WeeklyFourHourlyCandleTimeSeriesFile<TEntry>, FourHourlyCandleDataBucket<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry, ICandle
{
    public WeeklyFourHourlyCandleTimeSeriesFile(PagedMemoryMappedFile pagedMemoryMappedFile, IMutableTimeSeriesFileHeader header)
        : base(pagedMemoryMappedFile, header) { }

    public WeeklyFourHourlyCandleTimeSeriesFile(PriceTimeSeriesFileParameters sourceTickerTimeSeriesFileParams)
        : base(sourceTickerTimeSeriesFileParams.SetFilePeriod(TimeBoundaryPeriod.OneWeek)
                                               .AssertTimeSeriesEntryType(InstrumentType.Candle)
                                               .SetFileFlags(FileFlags.HasSubFileHeader)
                                               .SetInternalIndexSize(42)
                                               .SetInitialFileSize(512 * 1024)) { }

    public static WeeklyFourHourlyCandleTimeSeriesFile<TEntry> OpenExistingTimeSeriesFile
        (FileInfo file) =>
        OpenExistingTimeSeriesFile(file.FullName);
}

public class MonthlyFourHourlyCandleTimeSeriesFile<TEntry> :
    CandleTimeSeriesFile<MonthlyFourHourlyCandleTimeSeriesFile<TEntry>, FourHourlyCandleDataBucket<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry, ICandle
{
    public MonthlyFourHourlyCandleTimeSeriesFile(PagedMemoryMappedFile pagedMemoryMappedFile, IMutableTimeSeriesFileHeader header)
        : base(pagedMemoryMappedFile, header) { }

    public MonthlyFourHourlyCandleTimeSeriesFile(PriceTimeSeriesFileParameters sourceTickerTimeSeriesFileParams)
        : base(sourceTickerTimeSeriesFileParams.SetFilePeriod(TimeBoundaryPeriod.OneMonth)
                                               .AssertTimeSeriesEntryType(InstrumentType.Candle)
                                               .SetFileFlags(FileFlags.HasSubFileHeader)
                                               .SetInternalIndexSize(186)
                                               .SetInitialFileSize(512 * 1024)) { }

    public static MonthlyFourHourlyCandleTimeSeriesFile<TEntry> OpenExistingTimeSeriesFile
        (FileInfo file) =>
        OpenExistingTimeSeriesFile(file.FullName);
}

public class MonthlyDailyHourlyCandleTimeSeriesFile<TEntry> :
    CandleTimeSeriesFile<MonthlyDailyHourlyCandleTimeSeriesFile<TEntry>, DailyToHourlyCandleSubBuckets<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry, ICandle
{
    public MonthlyDailyHourlyCandleTimeSeriesFile(PagedMemoryMappedFile pagedMemoryMappedFile, IMutableTimeSeriesFileHeader header)
        : base(pagedMemoryMappedFile, header) { }

    public MonthlyDailyHourlyCandleTimeSeriesFile(PriceTimeSeriesFileParameters sourceTickerTimeSeriesFileParams)
        : base(sourceTickerTimeSeriesFileParams.SetFilePeriod(TimeBoundaryPeriod.OneMonth)
                                               .AssertTimeSeriesEntryType(InstrumentType.Candle)
                                               .SetFileFlags(FileFlags.HasSubFileHeader)
                                               .SetInternalIndexSize(31)
                                               .SetInitialFileSize(512 * 1024)) { }

    public static MonthlyDailyHourlyCandleTimeSeriesFile<TEntry> OpenExistingTimeSeriesFile
        (FileInfo file) =>
        OpenExistingTimeSeriesFile(file.FullName);
}

public class MonthlyWeeklyFourHourlyCandleTimeSeriesFile<TEntry> :
    CandleTimeSeriesFile<MonthlyWeeklyFourHourlyCandleTimeSeriesFile<TEntry>, WeeklyToFourHourlyCandleSubBuckets<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry, ICandle
{
    public MonthlyWeeklyFourHourlyCandleTimeSeriesFile(PagedMemoryMappedFile pagedMemoryMappedFile, IMutableTimeSeriesFileHeader header)
        : base(pagedMemoryMappedFile, header) { }

    public MonthlyWeeklyFourHourlyCandleTimeSeriesFile(PriceTimeSeriesFileParameters sourceTickerTimeSeriesFileParams)
        : base(sourceTickerTimeSeriesFileParams.SetFilePeriod(TimeBoundaryPeriod.OneMonth)
                                               .AssertTimeSeriesEntryType(InstrumentType.Candle)
                                               .SetFileFlags(FileFlags.HasSubFileHeader)
                                               .SetInternalIndexSize(4)
                                               .SetInitialFileSize(512 * 1024)) { }

    public static MonthlyWeeklyFourHourlyCandleTimeSeriesFile<TEntry> OpenExistingTimeSeriesFile
        (FileInfo file) =>
        OpenExistingTimeSeriesFile(file.FullName);
}

public class MonthlyWeeklyDailyCandleTimeSeriesFile<TEntry> :
    CandleTimeSeriesFile<MonthlyWeeklyDailyCandleTimeSeriesFile<TEntry>, WeeklyToDailyCandleSubBuckets<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry, ICandle
{
    public MonthlyWeeklyDailyCandleTimeSeriesFile(PagedMemoryMappedFile pagedMemoryMappedFile, IMutableTimeSeriesFileHeader header)
        : base(pagedMemoryMappedFile, header) { }

    public MonthlyWeeklyDailyCandleTimeSeriesFile(PriceTimeSeriesFileParameters sourceTickerTimeSeriesFileParams)
        : base(sourceTickerTimeSeriesFileParams.SetFilePeriod(TimeBoundaryPeriod.OneMonth)
                                               .AssertTimeSeriesEntryType(InstrumentType.Candle)
                                               .SetFileFlags(FileFlags.HasSubFileHeader)
                                               .SetInternalIndexSize(4)
                                               .SetInitialFileSize(512 * 1024)) { }

    public static MonthlyWeeklyDailyCandleTimeSeriesFile<TEntry> OpenExistingTimeSeriesFile
        (FileInfo file) =>
        OpenExistingTimeSeriesFile(file.FullName);
}

public class YearlyMonthlyFourHourlyCandleTimeSeriesFile<TEntry> :
    CandleTimeSeriesFile<YearlyMonthlyFourHourlyCandleTimeSeriesFile<TEntry>, MonthlyToFourHourlyCandleSubBuckets<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry, ICandle
{
    public YearlyMonthlyFourHourlyCandleTimeSeriesFile(PagedMemoryMappedFile pagedMemoryMappedFile, IMutableTimeSeriesFileHeader header)
        : base(pagedMemoryMappedFile, header) { }

    public YearlyMonthlyFourHourlyCandleTimeSeriesFile(PriceTimeSeriesFileParameters sourceTickerTimeSeriesFileParams)
        : base(sourceTickerTimeSeriesFileParams.SetFilePeriod(TimeBoundaryPeriod.OneYear)
                                               .AssertTimeSeriesEntryType(InstrumentType.Candle)
                                               .SetFileFlags(FileFlags.HasSubFileHeader)
                                               .SetInternalIndexSize(12)
                                               .SetInitialFileSize(512 * 1024)) { }

    public static YearlyMonthlyFourHourlyCandleTimeSeriesFile<TEntry> OpenExistingTimeSeriesFile
        (FileInfo file) =>
        OpenExistingTimeSeriesFile(file.FullName);
}

public class YearlyWeeklyDailyCandleTimeSeriesFile<TEntry> :
    CandleTimeSeriesFile<YearlyWeeklyDailyCandleTimeSeriesFile<TEntry>, WeeklyToDailyCandleSubBuckets<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry, ICandle
{
    public YearlyWeeklyDailyCandleTimeSeriesFile(PagedMemoryMappedFile pagedMemoryMappedFile, IMutableTimeSeriesFileHeader header)
        : base(pagedMemoryMappedFile, header) { }

    public YearlyWeeklyDailyCandleTimeSeriesFile(PriceTimeSeriesFileParameters sourceTickerTimeSeriesFileParams)
        : base(sourceTickerTimeSeriesFileParams.SetFilePeriod(TimeBoundaryPeriod.OneYear)
                                               .AssertTimeSeriesEntryType(InstrumentType.Candle)
                                               .SetFileFlags(FileFlags.HasSubFileHeader)
                                               .SetInternalIndexSize(53)
                                               .SetInitialFileSize(512 * 1024)) { }

    public static YearlyWeeklyDailyCandleTimeSeriesFile<TEntry> OpenExistingTimeSeriesFile
        (FileInfo file) =>
        OpenExistingTimeSeriesFile(file.FullName);
}

public class YearlyMonthlyWeeklyCandleTimeSeriesFile<TEntry> :
    CandleTimeSeriesFile<YearlyMonthlyWeeklyCandleTimeSeriesFile<TEntry>, MonthlyToWeeklyCandleSubBuckets<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry, ICandle
{
    public YearlyMonthlyWeeklyCandleTimeSeriesFile(PagedMemoryMappedFile pagedMemoryMappedFile, IMutableTimeSeriesFileHeader header)
        : base(pagedMemoryMappedFile, header) { }

    public YearlyMonthlyWeeklyCandleTimeSeriesFile(PriceTimeSeriesFileParameters sourceTickerTimeSeriesFileParams)
        : base(sourceTickerTimeSeriesFileParams.SetFilePeriod(TimeBoundaryPeriod.OneYear)
                                               .AssertTimeSeriesEntryType(InstrumentType.Candle)
                                               .SetFileFlags(FileFlags.HasSubFileHeader)
                                               .SetInternalIndexSize(12)
                                               .SetInitialFileSize(512 * 1024)) { }

    public static YearlyMonthlyWeeklyCandleTimeSeriesFile<TEntry> OpenExistingTimeSeriesFile(FileInfo file) =>
        OpenExistingTimeSeriesFile(file.FullName);
}

public class DecenniallyMonthlyWeeklyCandleTimeSeriesFile<TEntry> :
    CandleTimeSeriesFile<DecenniallyMonthlyWeeklyCandleTimeSeriesFile<TEntry>, MonthlyToWeeklyCandleSubBuckets<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry, ICandle
{
    public DecenniallyMonthlyWeeklyCandleTimeSeriesFile(PagedMemoryMappedFile pagedMemoryMappedFile, IMutableTimeSeriesFileHeader header)
        : base(pagedMemoryMappedFile, header) { }

    public DecenniallyMonthlyWeeklyCandleTimeSeriesFile(PriceTimeSeriesFileParameters sourceTickerTimeSeriesFileParams)
        : base(sourceTickerTimeSeriesFileParams.SetFilePeriod(TimeBoundaryPeriod.OneDecade)
                                               .AssertTimeSeriesEntryType(InstrumentType.Candle)
                                               .SetFileFlags(FileFlags.HasSubFileHeader)
                                               .SetInternalIndexSize(120)
                                               .SetInitialFileSize(512 * 1024)) { }

    public static DecenniallyMonthlyWeeklyCandleTimeSeriesFile<TEntry> OpenExistingTimeSeriesFile(FileInfo file) =>
        OpenExistingTimeSeriesFile(file.FullName);
}

public class DecenniallyYearlyMonthlyCandleTimeSeriesFile<TEntry> :
    CandleTimeSeriesFile<DecenniallyYearlyMonthlyCandleTimeSeriesFile<TEntry>, YearlyToMonthlyCandleSubBuckets<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry, ICandle
{
    public DecenniallyYearlyMonthlyCandleTimeSeriesFile(PagedMemoryMappedFile pagedMemoryMappedFile, IMutableTimeSeriesFileHeader header)
        : base(pagedMemoryMappedFile, header) { }

    public DecenniallyYearlyMonthlyCandleTimeSeriesFile(PriceTimeSeriesFileParameters sourceTickerTimeSeriesFileParams)
        : base(sourceTickerTimeSeriesFileParams.SetFilePeriod(TimeBoundaryPeriod.OneDecade)
                                               .AssertTimeSeriesEntryType(InstrumentType.Candle)
                                               .SetFileFlags(FileFlags.HasSubFileHeader)
                                               .SetInternalIndexSize(10)
                                               .SetInitialFileSize(512 * 1024)) { }

    public static DecenniallyYearlyMonthlyCandleTimeSeriesFile<TEntry> OpenExistingTimeSeriesFile(FileInfo file) =>
        OpenExistingTimeSeriesFile(file.FullName);
}

public class UnlimitedDecenniallyYearlyCandleTimeSeriesFile<TEntry> :
    CandleTimeSeriesFile<UnlimitedDecenniallyYearlyCandleTimeSeriesFile<TEntry>, DecenniallyToYearlyCandleSubBuckets<TEntry>,
        TEntry>
    where TEntry : ITimeSeriesEntry, ICandle
{
    public UnlimitedDecenniallyYearlyCandleTimeSeriesFile(PagedMemoryMappedFile pagedMemoryMappedFile, IMutableTimeSeriesFileHeader header)
        : base(pagedMemoryMappedFile, header) { }

    public UnlimitedDecenniallyYearlyCandleTimeSeriesFile(PriceTimeSeriesFileParameters sourceTickerTimeSeriesFileParams)
        : base(sourceTickerTimeSeriesFileParams.SetFilePeriod(TimeBoundaryPeriod.Tick)
                                               .AssertTimeSeriesEntryType(InstrumentType.Candle)
                                               .SetFileFlags(FileFlags.HasSubFileHeader)
                                               .SetInternalIndexSize(10)
                                               .SetInitialFileSize(512 * 1024)) { }

    public static UnlimitedDecenniallyYearlyCandleTimeSeriesFile<TEntry> OpenExistingTimeSeriesFile(FileInfo file) =>
        OpenExistingTimeSeriesFile(file.FullName);
}

public class UnlimitedDecenniallyCandleTimeSeriesFile<TEntry> :
    CandleTimeSeriesFile<UnlimitedDecenniallyCandleTimeSeriesFile<TEntry>, DecenniallyCandleDataBucket<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry, ICandle
{
    public UnlimitedDecenniallyCandleTimeSeriesFile(PagedMemoryMappedFile pagedMemoryMappedFile, IMutableTimeSeriesFileHeader header)
        : base(pagedMemoryMappedFile, header) { }

    public UnlimitedDecenniallyCandleTimeSeriesFile(PriceTimeSeriesFileParameters sourceTickerTimeSeriesFileParams)
        : base(sourceTickerTimeSeriesFileParams.SetFilePeriod(TimeBoundaryPeriod.Tick)
                                               .AssertTimeSeriesEntryType(InstrumentType.Candle)
                                               .SetFileFlags(FileFlags.HasSubFileHeader)
                                               .SetInternalIndexSize(50)
                                               .SetInitialFileSize(512 * 1024)) { }

    public static UnlimitedDecenniallyCandleTimeSeriesFile<TEntry> OpenExistingTimeSeriesFile(FileInfo file) =>
        OpenExistingTimeSeriesFile(file.FullName);
}

public class UnlimitedCandleTimeSeriesFile<TEntry> :
    CandleTimeSeriesFile<UnlimitedCandleTimeSeriesFile<TEntry>, UnlimitedCandleDataBucket<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry, ICandle
{
    public UnlimitedCandleTimeSeriesFile(PagedMemoryMappedFile pagedMemoryMappedFile, IMutableTimeSeriesFileHeader header)
        : base(pagedMemoryMappedFile, header) { }

    public UnlimitedCandleTimeSeriesFile(PriceTimeSeriesFileParameters sourceTickerTimeSeriesFileParams)
        : base(sourceTickerTimeSeriesFileParams.SetFilePeriod(TimeBoundaryPeriod.Tick)
                                               .AssertTimeSeriesEntryType(InstrumentType.Candle)
                                               .SetFileFlags(FileFlags.HasSubFileHeader)
                                               .SetInternalIndexSize(1)
                                               .SetInitialFileSize(512 * 1024)) { }

    public static UnlimitedCandleTimeSeriesFile<TEntry> OpenExistingTimeSeriesFile(FileInfo file) => OpenExistingTimeSeriesFile(file.FullName);
}
