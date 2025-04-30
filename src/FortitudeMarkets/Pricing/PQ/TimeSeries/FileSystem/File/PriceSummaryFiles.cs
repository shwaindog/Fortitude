// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory.UnmanagedMemory.MemoryMappedFiles;
using FortitudeIO.TimeSeries;
using FortitudeIO.TimeSeries.FileSystem.File;
using FortitudeIO.TimeSeries.FileSystem.File.Header;
using FortitudeMarkets.Pricing.PQ.TimeSeries.FileSystem.File.Buckets;
using FortitudeMarkets.Pricing.Summaries;

#endregion

namespace FortitudeMarkets.Pricing.PQ.TimeSeries.FileSystem.File;

public class WeeklyDailyHourlyPriceSummaryTimeSeriesFile<TEntry> :
    PriceSummaryTimeSeriesFile<WeeklyDailyHourlyPriceSummaryTimeSeriesFile<TEntry>, DailyToHourlyPriceSummarySubBuckets<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry, IPricePeriodSummary
{
    public WeeklyDailyHourlyPriceSummaryTimeSeriesFile(PagedMemoryMappedFile pagedMemoryMappedFile, IMutableTimeSeriesFileHeader header)
        : base(pagedMemoryMappedFile, header) { }

    public WeeklyDailyHourlyPriceSummaryTimeSeriesFile(PriceTimeSeriesFileParameters sourceTickerTimeSeriesFileParams)
        : base(sourceTickerTimeSeriesFileParams.SetFilePeriod(TimeBoundaryPeriod.OneWeek)
                                               .AssertTimeSeriesEntryType(InstrumentType.PriceSummaryPeriod)
                                               .SetFileFlags(FileFlags.HasSubFileHeader)
                                               .SetInternalIndexSize(7)
                                               .SetInitialFileSize(512 * 1024)) { }

    public static WeeklyDailyHourlyPriceSummaryTimeSeriesFile<TEntry> OpenExistingTimeSeriesFile
        (FileInfo file) =>
        OpenExistingTimeSeriesFile(file.FullName);
}

public class WeeklyFourHourlyPriceSummaryTimeSeriesFile<TEntry> :
    PriceSummaryTimeSeriesFile<WeeklyFourHourlyPriceSummaryTimeSeriesFile<TEntry>, FourHourlyPriceSummaryDataBucket<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry, IPricePeriodSummary
{
    public WeeklyFourHourlyPriceSummaryTimeSeriesFile(PagedMemoryMappedFile pagedMemoryMappedFile, IMutableTimeSeriesFileHeader header)
        : base(pagedMemoryMappedFile, header) { }

    public WeeklyFourHourlyPriceSummaryTimeSeriesFile(PriceTimeSeriesFileParameters sourceTickerTimeSeriesFileParams)
        : base(sourceTickerTimeSeriesFileParams.SetFilePeriod(TimeBoundaryPeriod.OneWeek)
                                               .AssertTimeSeriesEntryType(InstrumentType.PriceSummaryPeriod)
                                               .SetFileFlags(FileFlags.HasSubFileHeader)
                                               .SetInternalIndexSize(42)
                                               .SetInitialFileSize(512 * 1024)) { }

    public static WeeklyFourHourlyPriceSummaryTimeSeriesFile<TEntry> OpenExistingTimeSeriesFile
        (FileInfo file) =>
        OpenExistingTimeSeriesFile(file.FullName);
}

public class MonthlyFourHourlyPriceSummaryTimeSeriesFile<TEntry> :
    PriceSummaryTimeSeriesFile<MonthlyFourHourlyPriceSummaryTimeSeriesFile<TEntry>, FourHourlyPriceSummaryDataBucket<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry, IPricePeriodSummary
{
    public MonthlyFourHourlyPriceSummaryTimeSeriesFile(PagedMemoryMappedFile pagedMemoryMappedFile, IMutableTimeSeriesFileHeader header)
        : base(pagedMemoryMappedFile, header) { }

    public MonthlyFourHourlyPriceSummaryTimeSeriesFile(PriceTimeSeriesFileParameters sourceTickerTimeSeriesFileParams)
        : base(sourceTickerTimeSeriesFileParams.SetFilePeriod(TimeBoundaryPeriod.OneMonth)
                                               .AssertTimeSeriesEntryType(InstrumentType.PriceSummaryPeriod)
                                               .SetFileFlags(FileFlags.HasSubFileHeader)
                                               .SetInternalIndexSize(186)
                                               .SetInitialFileSize(512 * 1024)) { }

    public static MonthlyFourHourlyPriceSummaryTimeSeriesFile<TEntry> OpenExistingTimeSeriesFile
        (FileInfo file) =>
        OpenExistingTimeSeriesFile(file.FullName);
}

public class MonthlyDailyHourlyPriceSummaryTimeSeriesFile<TEntry> :
    PriceSummaryTimeSeriesFile<MonthlyDailyHourlyPriceSummaryTimeSeriesFile<TEntry>, DailyToHourlyPriceSummarySubBuckets<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry, IPricePeriodSummary
{
    public MonthlyDailyHourlyPriceSummaryTimeSeriesFile(PagedMemoryMappedFile pagedMemoryMappedFile, IMutableTimeSeriesFileHeader header)
        : base(pagedMemoryMappedFile, header) { }

    public MonthlyDailyHourlyPriceSummaryTimeSeriesFile(PriceTimeSeriesFileParameters sourceTickerTimeSeriesFileParams)
        : base(sourceTickerTimeSeriesFileParams.SetFilePeriod(TimeBoundaryPeriod.OneMonth)
                                               .AssertTimeSeriesEntryType(InstrumentType.PriceSummaryPeriod)
                                               .SetFileFlags(FileFlags.HasSubFileHeader)
                                               .SetInternalIndexSize(31)
                                               .SetInitialFileSize(512 * 1024)) { }

    public static MonthlyDailyHourlyPriceSummaryTimeSeriesFile<TEntry> OpenExistingTimeSeriesFile
        (FileInfo file) =>
        OpenExistingTimeSeriesFile(file.FullName);
}

public class MonthlyWeeklyFourHourlyPriceSummaryTimeSeriesFile<TEntry> :
    PriceSummaryTimeSeriesFile<MonthlyWeeklyFourHourlyPriceSummaryTimeSeriesFile<TEntry>, WeeklyToFourHourlyPriceSummarySubBuckets<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry, IPricePeriodSummary
{
    public MonthlyWeeklyFourHourlyPriceSummaryTimeSeriesFile(PagedMemoryMappedFile pagedMemoryMappedFile, IMutableTimeSeriesFileHeader header)
        : base(pagedMemoryMappedFile, header) { }

    public MonthlyWeeklyFourHourlyPriceSummaryTimeSeriesFile(PriceTimeSeriesFileParameters sourceTickerTimeSeriesFileParams)
        : base(sourceTickerTimeSeriesFileParams.SetFilePeriod(TimeBoundaryPeriod.OneMonth)
                                               .AssertTimeSeriesEntryType(InstrumentType.PriceSummaryPeriod)
                                               .SetFileFlags(FileFlags.HasSubFileHeader)
                                               .SetInternalIndexSize(4)
                                               .SetInitialFileSize(512 * 1024)) { }

    public static MonthlyWeeklyFourHourlyPriceSummaryTimeSeriesFile<TEntry> OpenExistingTimeSeriesFile
        (FileInfo file) =>
        OpenExistingTimeSeriesFile(file.FullName);
}

public class MonthlyWeeklyDailyPriceSummaryTimeSeriesFile<TEntry> :
    PriceSummaryTimeSeriesFile<MonthlyWeeklyDailyPriceSummaryTimeSeriesFile<TEntry>, WeeklyToDailyPriceSummarySubBuckets<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry, IPricePeriodSummary
{
    public MonthlyWeeklyDailyPriceSummaryTimeSeriesFile(PagedMemoryMappedFile pagedMemoryMappedFile, IMutableTimeSeriesFileHeader header)
        : base(pagedMemoryMappedFile, header) { }

    public MonthlyWeeklyDailyPriceSummaryTimeSeriesFile(PriceTimeSeriesFileParameters sourceTickerTimeSeriesFileParams)
        : base(sourceTickerTimeSeriesFileParams.SetFilePeriod(TimeBoundaryPeriod.OneMonth)
                                               .AssertTimeSeriesEntryType(InstrumentType.PriceSummaryPeriod)
                                               .SetFileFlags(FileFlags.HasSubFileHeader)
                                               .SetInternalIndexSize(4)
                                               .SetInitialFileSize(512 * 1024)) { }

    public static MonthlyWeeklyDailyPriceSummaryTimeSeriesFile<TEntry> OpenExistingTimeSeriesFile
        (FileInfo file) =>
        OpenExistingTimeSeriesFile(file.FullName);
}

public class YearlyMonthlyFourHourlyPriceSummaryTimeSeriesFile<TEntry> :
    PriceSummaryTimeSeriesFile<YearlyMonthlyFourHourlyPriceSummaryTimeSeriesFile<TEntry>, MonthlyToFourHourlyPriceSummarySubBuckets<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry, IPricePeriodSummary
{
    public YearlyMonthlyFourHourlyPriceSummaryTimeSeriesFile(PagedMemoryMappedFile pagedMemoryMappedFile, IMutableTimeSeriesFileHeader header)
        : base(pagedMemoryMappedFile, header) { }

    public YearlyMonthlyFourHourlyPriceSummaryTimeSeriesFile(PriceTimeSeriesFileParameters sourceTickerTimeSeriesFileParams)
        : base(sourceTickerTimeSeriesFileParams.SetFilePeriod(TimeBoundaryPeriod.OneYear)
                                               .AssertTimeSeriesEntryType(InstrumentType.PriceSummaryPeriod)
                                               .SetFileFlags(FileFlags.HasSubFileHeader)
                                               .SetInternalIndexSize(12)
                                               .SetInitialFileSize(512 * 1024)) { }

    public static YearlyMonthlyFourHourlyPriceSummaryTimeSeriesFile<TEntry> OpenExistingTimeSeriesFile
        (FileInfo file) =>
        OpenExistingTimeSeriesFile(file.FullName);
}

public class YearlyWeeklyDailyPriceSummaryTimeSeriesFile<TEntry> :
    PriceSummaryTimeSeriesFile<YearlyWeeklyDailyPriceSummaryTimeSeriesFile<TEntry>, WeeklyToDailyPriceSummarySubBuckets<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry, IPricePeriodSummary
{
    public YearlyWeeklyDailyPriceSummaryTimeSeriesFile(PagedMemoryMappedFile pagedMemoryMappedFile, IMutableTimeSeriesFileHeader header)
        : base(pagedMemoryMappedFile, header) { }

    public YearlyWeeklyDailyPriceSummaryTimeSeriesFile(PriceTimeSeriesFileParameters sourceTickerTimeSeriesFileParams)
        : base(sourceTickerTimeSeriesFileParams.SetFilePeriod(TimeBoundaryPeriod.OneYear)
                                               .AssertTimeSeriesEntryType(InstrumentType.PriceSummaryPeriod)
                                               .SetFileFlags(FileFlags.HasSubFileHeader)
                                               .SetInternalIndexSize(53)
                                               .SetInitialFileSize(512 * 1024)) { }

    public static YearlyWeeklyDailyPriceSummaryTimeSeriesFile<TEntry> OpenExistingTimeSeriesFile
        (FileInfo file) =>
        OpenExistingTimeSeriesFile(file.FullName);
}

public class YearlyMonthlyWeeklyPriceSummaryTimeSeriesFile<TEntry> :
    PriceSummaryTimeSeriesFile<YearlyMonthlyWeeklyPriceSummaryTimeSeriesFile<TEntry>, MonthlyToWeeklyPriceSummarySubBuckets<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry, IPricePeriodSummary
{
    public YearlyMonthlyWeeklyPriceSummaryTimeSeriesFile(PagedMemoryMappedFile pagedMemoryMappedFile, IMutableTimeSeriesFileHeader header)
        : base(pagedMemoryMappedFile, header) { }

    public YearlyMonthlyWeeklyPriceSummaryTimeSeriesFile(PriceTimeSeriesFileParameters sourceTickerTimeSeriesFileParams)
        : base(sourceTickerTimeSeriesFileParams.SetFilePeriod(TimeBoundaryPeriod.OneYear)
                                               .AssertTimeSeriesEntryType(InstrumentType.PriceSummaryPeriod)
                                               .SetFileFlags(FileFlags.HasSubFileHeader)
                                               .SetInternalIndexSize(12)
                                               .SetInitialFileSize(512 * 1024)) { }

    public static YearlyMonthlyWeeklyPriceSummaryTimeSeriesFile<TEntry> OpenExistingTimeSeriesFile(FileInfo file) =>
        OpenExistingTimeSeriesFile(file.FullName);
}

public class DecenniallyMonthlyWeeklyPriceSummaryTimeSeriesFile<TEntry> :
    PriceSummaryTimeSeriesFile<DecenniallyMonthlyWeeklyPriceSummaryTimeSeriesFile<TEntry>, MonthlyToWeeklyPriceSummarySubBuckets<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry, IPricePeriodSummary
{
    public DecenniallyMonthlyWeeklyPriceSummaryTimeSeriesFile(PagedMemoryMappedFile pagedMemoryMappedFile, IMutableTimeSeriesFileHeader header)
        : base(pagedMemoryMappedFile, header) { }

    public DecenniallyMonthlyWeeklyPriceSummaryTimeSeriesFile(PriceTimeSeriesFileParameters sourceTickerTimeSeriesFileParams)
        : base(sourceTickerTimeSeriesFileParams.SetFilePeriod(TimeBoundaryPeriod.OneDecade)
                                               .AssertTimeSeriesEntryType(InstrumentType.PriceSummaryPeriod)
                                               .SetFileFlags(FileFlags.HasSubFileHeader)
                                               .SetInternalIndexSize(120)
                                               .SetInitialFileSize(512 * 1024)) { }

    public static DecenniallyMonthlyWeeklyPriceSummaryTimeSeriesFile<TEntry> OpenExistingTimeSeriesFile(FileInfo file) =>
        OpenExistingTimeSeriesFile(file.FullName);
}

public class DecenniallyYearlyMonthlyPriceSummaryTimeSeriesFile<TEntry> :
    PriceSummaryTimeSeriesFile<DecenniallyYearlyMonthlyPriceSummaryTimeSeriesFile<TEntry>, YearlyToMonthlyPriceSummarySubBuckets<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry, IPricePeriodSummary
{
    public DecenniallyYearlyMonthlyPriceSummaryTimeSeriesFile(PagedMemoryMappedFile pagedMemoryMappedFile, IMutableTimeSeriesFileHeader header)
        : base(pagedMemoryMappedFile, header) { }

    public DecenniallyYearlyMonthlyPriceSummaryTimeSeriesFile(PriceTimeSeriesFileParameters sourceTickerTimeSeriesFileParams)
        : base(sourceTickerTimeSeriesFileParams.SetFilePeriod(TimeBoundaryPeriod.OneDecade)
                                               .AssertTimeSeriesEntryType(InstrumentType.PriceSummaryPeriod)
                                               .SetFileFlags(FileFlags.HasSubFileHeader)
                                               .SetInternalIndexSize(10)
                                               .SetInitialFileSize(512 * 1024)) { }

    public static DecenniallyYearlyMonthlyPriceSummaryTimeSeriesFile<TEntry> OpenExistingTimeSeriesFile(FileInfo file) =>
        OpenExistingTimeSeriesFile(file.FullName);
}

public class UnlimitedDecenniallyYearlyPriceSummaryTimeSeriesFile<TEntry> :
    PriceSummaryTimeSeriesFile<UnlimitedDecenniallyYearlyPriceSummaryTimeSeriesFile<TEntry>, DecenniallyToYearlyPriceSummarySubBuckets<TEntry>,
        TEntry>
    where TEntry : ITimeSeriesEntry, IPricePeriodSummary
{
    public UnlimitedDecenniallyYearlyPriceSummaryTimeSeriesFile(PagedMemoryMappedFile pagedMemoryMappedFile, IMutableTimeSeriesFileHeader header)
        : base(pagedMemoryMappedFile, header) { }

    public UnlimitedDecenniallyYearlyPriceSummaryTimeSeriesFile(PriceTimeSeriesFileParameters sourceTickerTimeSeriesFileParams)
        : base(sourceTickerTimeSeriesFileParams.SetFilePeriod(TimeBoundaryPeriod.Tick)
                                               .AssertTimeSeriesEntryType(InstrumentType.PriceSummaryPeriod)
                                               .SetFileFlags(FileFlags.HasSubFileHeader)
                                               .SetInternalIndexSize(10)
                                               .SetInitialFileSize(512 * 1024)) { }

    public static UnlimitedDecenniallyYearlyPriceSummaryTimeSeriesFile<TEntry> OpenExistingTimeSeriesFile(FileInfo file) =>
        OpenExistingTimeSeriesFile(file.FullName);
}

public class UnlimitedDecenniallyPriceSummaryTimeSeriesFile<TEntry> :
    PriceSummaryTimeSeriesFile<UnlimitedDecenniallyPriceSummaryTimeSeriesFile<TEntry>, DecenniallyPriceSummaryDataBucket<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry, IPricePeriodSummary
{
    public UnlimitedDecenniallyPriceSummaryTimeSeriesFile(PagedMemoryMappedFile pagedMemoryMappedFile, IMutableTimeSeriesFileHeader header)
        : base(pagedMemoryMappedFile, header) { }

    public UnlimitedDecenniallyPriceSummaryTimeSeriesFile(PriceTimeSeriesFileParameters sourceTickerTimeSeriesFileParams)
        : base(sourceTickerTimeSeriesFileParams.SetFilePeriod(TimeBoundaryPeriod.Tick)
                                               .AssertTimeSeriesEntryType(InstrumentType.PriceSummaryPeriod)
                                               .SetFileFlags(FileFlags.HasSubFileHeader)
                                               .SetInternalIndexSize(50)
                                               .SetInitialFileSize(512 * 1024)) { }

    public static UnlimitedDecenniallyPriceSummaryTimeSeriesFile<TEntry> OpenExistingTimeSeriesFile(FileInfo file) =>
        OpenExistingTimeSeriesFile(file.FullName);
}

public class UnlimitedPriceSummaryTimeSeriesFile<TEntry> :
    PriceSummaryTimeSeriesFile<UnlimitedPriceSummaryTimeSeriesFile<TEntry>, UnlimitedPriceSummaryDataBucket<TEntry>, TEntry>
    where TEntry : ITimeSeriesEntry, IPricePeriodSummary
{
    public UnlimitedPriceSummaryTimeSeriesFile(PagedMemoryMappedFile pagedMemoryMappedFile, IMutableTimeSeriesFileHeader header)
        : base(pagedMemoryMappedFile, header) { }

    public UnlimitedPriceSummaryTimeSeriesFile(PriceTimeSeriesFileParameters sourceTickerTimeSeriesFileParams)
        : base(sourceTickerTimeSeriesFileParams.SetFilePeriod(TimeBoundaryPeriod.Tick)
                                               .AssertTimeSeriesEntryType(InstrumentType.PriceSummaryPeriod)
                                               .SetFileFlags(FileFlags.HasSubFileHeader)
                                               .SetInternalIndexSize(1)
                                               .SetInitialFileSize(512 * 1024)) { }

    public static UnlimitedPriceSummaryTimeSeriesFile<TEntry> OpenExistingTimeSeriesFile(FileInfo file) => OpenExistingTimeSeriesFile(file.FullName);
}
