// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory.UnmanagedMemory.MemoryMappedFiles;
using FortitudeIO.TimeSeries;
using FortitudeIO.TimeSeries.FileSystem.File;
using FortitudeIO.TimeSeries.FileSystem.File.Header;
using FortitudeIO.TimeSeries.FileSystem.File.Session;
using FortitudeMarketsApi.Pricing.TimeSeries;
using FortitudeMarketsCore.Pricing.PQ.TimeSeries.FileSystem.File.Buckets;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.TimeSeries.FileSystem.File;

public class WeeklyDailyHourlyPriceSummaryTimeSeriesFile : PriceSummaryTimeSeriesFile<WeeklyDailyHourlyPriceSummaryTimeSeriesFile,
    DailyToHourlyPriceSummarySubBuckets<IPricePeriodSummary>, IPricePeriodSummary>
{
    public WeeklyDailyHourlyPriceSummaryTimeSeriesFile(PagedMemoryMappedFile pagedMemoryMappedFile, IMutableTimeSeriesFileHeader header)
        : base(pagedMemoryMappedFile, header) { }

    public WeeklyDailyHourlyPriceSummaryTimeSeriesFile(PriceTimeSeriesFileParameters sourceTickerTimeSeriesFileParams)
        : base(sourceTickerTimeSeriesFileParams.SetFilePeriod(TimeSeriesPeriod.OneWeek)
                                               .AssertTimeSeriesEntryType(InstrumentType.PriceSummaryPeriod)
                                               .SetFileFlags(FileFlags.HasSubFileHeader)
                                               .SetInternalIndexSize(7)
                                               .SetInitialFileSize(512 * 1024)) { }

    public override ISessionAppendContext<IPricePeriodSummary, DailyToHourlyPriceSummarySubBuckets<IPricePeriodSummary>> CreateAppendContext() =>
        new PQPricePeriodSummaryAppendContext<IPricePeriodSummary, DailyToHourlyPriceSummarySubBuckets<IPricePeriodSummary>>();

    public static WeeklyDailyHourlyPriceSummaryTimeSeriesFile OpenExistingTimeSeriesFile(FileInfo file) => OpenExistingTimeSeriesFile(file.FullName);
}

public class MonthlyDailyHourlyPriceSummaryTimeSeriesFile : PriceSummaryTimeSeriesFile<MonthlyDailyHourlyPriceSummaryTimeSeriesFile,
    DailyToHourlyPriceSummarySubBuckets<IPricePeriodSummary>, IPricePeriodSummary>
{
    public MonthlyDailyHourlyPriceSummaryTimeSeriesFile(PagedMemoryMappedFile pagedMemoryMappedFile, IMutableTimeSeriesFileHeader header)
        : base(pagedMemoryMappedFile, header) { }

    public MonthlyDailyHourlyPriceSummaryTimeSeriesFile(PriceTimeSeriesFileParameters sourceTickerTimeSeriesFileParams)
        : base(sourceTickerTimeSeriesFileParams.SetFilePeriod(TimeSeriesPeriod.OneMonth)
                                               .AssertTimeSeriesEntryType(InstrumentType.PriceSummaryPeriod)
                                               .SetFileFlags(FileFlags.HasSubFileHeader)
                                               .SetInternalIndexSize(31)
                                               .SetInitialFileSize(512 * 1024)) { }

    public override ISessionAppendContext<IPricePeriodSummary, DailyToHourlyPriceSummarySubBuckets<IPricePeriodSummary>> CreateAppendContext() =>
        new PQPricePeriodSummaryAppendContext<IPricePeriodSummary, DailyToHourlyPriceSummarySubBuckets<IPricePeriodSummary>>();

    public static MonthlyDailyHourlyPriceSummaryTimeSeriesFile OpenExistingTimeSeriesFile(FileInfo file) => OpenExistingTimeSeriesFile(file.FullName);
}

public class MonthlyWeeklyDailyPriceSummaryTimeSeriesFile : PriceSummaryTimeSeriesFile<MonthlyWeeklyDailyPriceSummaryTimeSeriesFile,
    WeeklyToDailyPriceSummarySubBuckets<IPricePeriodSummary>, IPricePeriodSummary>
{
    public MonthlyWeeklyDailyPriceSummaryTimeSeriesFile(PagedMemoryMappedFile pagedMemoryMappedFile, IMutableTimeSeriesFileHeader header)
        : base(pagedMemoryMappedFile, header) { }

    public MonthlyWeeklyDailyPriceSummaryTimeSeriesFile(PriceTimeSeriesFileParameters sourceTickerTimeSeriesFileParams)
        : base(sourceTickerTimeSeriesFileParams.SetFilePeriod(TimeSeriesPeriod.OneMonth)
                                               .AssertTimeSeriesEntryType(InstrumentType.PriceSummaryPeriod)
                                               .SetFileFlags(FileFlags.HasSubFileHeader)
                                               .SetInternalIndexSize(4)
                                               .SetInitialFileSize(512 * 1024)) { }

    public override ISessionAppendContext<IPricePeriodSummary, WeeklyToDailyPriceSummarySubBuckets<IPricePeriodSummary>> CreateAppendContext() =>
        new PQPricePeriodSummaryAppendContext<IPricePeriodSummary, WeeklyToDailyPriceSummarySubBuckets<IPricePeriodSummary>>();

    public static MonthlyWeeklyDailyPriceSummaryTimeSeriesFile OpenExistingTimeSeriesFile(FileInfo file) => OpenExistingTimeSeriesFile(file.FullName);
}

public class YearlyWeeklyDailyPriceSummaryTimeSeriesFile : PriceSummaryTimeSeriesFile<YearlyWeeklyDailyPriceSummaryTimeSeriesFile,
    WeeklyToDailyPriceSummarySubBuckets<IPricePeriodSummary>, IPricePeriodSummary>
{
    public YearlyWeeklyDailyPriceSummaryTimeSeriesFile(PagedMemoryMappedFile pagedMemoryMappedFile, IMutableTimeSeriesFileHeader header)
        : base(pagedMemoryMappedFile, header) { }

    public YearlyWeeklyDailyPriceSummaryTimeSeriesFile(PriceTimeSeriesFileParameters sourceTickerTimeSeriesFileParams)
        : base(sourceTickerTimeSeriesFileParams.SetFilePeriod(TimeSeriesPeriod.OneYear)
                                               .AssertTimeSeriesEntryType(InstrumentType.PriceSummaryPeriod)
                                               .SetFileFlags(FileFlags.HasSubFileHeader)
                                               .SetInternalIndexSize(53)
                                               .SetInitialFileSize(512 * 1024)) { }

    public override ISessionAppendContext<IPricePeriodSummary, WeeklyToDailyPriceSummarySubBuckets<IPricePeriodSummary>> CreateAppendContext() =>
        new PQPricePeriodSummaryAppendContext<IPricePeriodSummary, WeeklyToDailyPriceSummarySubBuckets<IPricePeriodSummary>>();

    public static YearlyWeeklyDailyPriceSummaryTimeSeriesFile OpenExistingTimeSeriesFile(FileInfo file) => OpenExistingTimeSeriesFile(file.FullName);
}

public class YearlyMonthlyWeeklyPriceSummaryTimeSeriesFile : PriceSummaryTimeSeriesFile<YearlyMonthlyWeeklyPriceSummaryTimeSeriesFile,
    MonthlyToWeeklyPriceSummarySubBuckets<IPricePeriodSummary>, IPricePeriodSummary>
{
    public YearlyMonthlyWeeklyPriceSummaryTimeSeriesFile(PagedMemoryMappedFile pagedMemoryMappedFile, IMutableTimeSeriesFileHeader header)
        : base(pagedMemoryMappedFile, header) { }

    public YearlyMonthlyWeeklyPriceSummaryTimeSeriesFile(PriceTimeSeriesFileParameters sourceTickerTimeSeriesFileParams)
        : base(sourceTickerTimeSeriesFileParams.SetFilePeriod(TimeSeriesPeriod.OneYear)
                                               .AssertTimeSeriesEntryType(InstrumentType.PriceSummaryPeriod)
                                               .SetFileFlags(FileFlags.HasSubFileHeader)
                                               .SetInternalIndexSize(12)
                                               .SetInitialFileSize(512 * 1024)) { }

    public override ISessionAppendContext<IPricePeriodSummary, MonthlyToWeeklyPriceSummarySubBuckets<IPricePeriodSummary>> CreateAppendContext() =>
        new PQPricePeriodSummaryAppendContext<IPricePeriodSummary, MonthlyToWeeklyPriceSummarySubBuckets<IPricePeriodSummary>>();

    public static YearlyMonthlyWeeklyPriceSummaryTimeSeriesFile OpenExistingTimeSeriesFile(FileInfo file) =>
        OpenExistingTimeSeriesFile(file.FullName);
}

public class DecenniallyMonthlyWeeklyPriceSummaryTimeSeriesFile : PriceSummaryTimeSeriesFile<DecenniallyMonthlyWeeklyPriceSummaryTimeSeriesFile,
    MonthlyToWeeklyPriceSummarySubBuckets<IPricePeriodSummary>, IPricePeriodSummary>
{
    public DecenniallyMonthlyWeeklyPriceSummaryTimeSeriesFile(PagedMemoryMappedFile pagedMemoryMappedFile, IMutableTimeSeriesFileHeader header)
        : base(pagedMemoryMappedFile, header) { }

    public DecenniallyMonthlyWeeklyPriceSummaryTimeSeriesFile(PriceTimeSeriesFileParameters sourceTickerTimeSeriesFileParams)
        : base(sourceTickerTimeSeriesFileParams.SetFilePeriod(TimeSeriesPeriod.OneDecade)
                                               .AssertTimeSeriesEntryType(InstrumentType.PriceSummaryPeriod)
                                               .SetFileFlags(FileFlags.HasSubFileHeader)
                                               .SetInternalIndexSize(120)
                                               .SetInitialFileSize(512 * 1024)) { }

    public override ISessionAppendContext<IPricePeriodSummary, MonthlyToWeeklyPriceSummarySubBuckets<IPricePeriodSummary>> CreateAppendContext() =>
        new PQPricePeriodSummaryAppendContext<IPricePeriodSummary, MonthlyToWeeklyPriceSummarySubBuckets<IPricePeriodSummary>>();

    public static DecenniallyMonthlyWeeklyPriceSummaryTimeSeriesFile OpenExistingTimeSeriesFile(FileInfo file) =>
        OpenExistingTimeSeriesFile(file.FullName);
}

public class DecenniallyYearlyMonthlyPriceSummaryTimeSeriesFile : PriceSummaryTimeSeriesFile<DecenniallyYearlyMonthlyPriceSummaryTimeSeriesFile,
    YearlyToMonthlyPriceSummarySubBuckets<IPricePeriodSummary>, IPricePeriodSummary>
{
    public DecenniallyYearlyMonthlyPriceSummaryTimeSeriesFile(PagedMemoryMappedFile pagedMemoryMappedFile, IMutableTimeSeriesFileHeader header)
        : base(pagedMemoryMappedFile, header) { }

    public DecenniallyYearlyMonthlyPriceSummaryTimeSeriesFile(PriceTimeSeriesFileParameters sourceTickerTimeSeriesFileParams)
        : base(sourceTickerTimeSeriesFileParams.SetFilePeriod(TimeSeriesPeriod.OneDecade)
                                               .AssertTimeSeriesEntryType(InstrumentType.PriceSummaryPeriod)
                                               .SetFileFlags(FileFlags.HasSubFileHeader)
                                               .SetInternalIndexSize(10)
                                               .SetInitialFileSize(512 * 1024)) { }

    public override ISessionAppendContext<IPricePeriodSummary, YearlyToMonthlyPriceSummarySubBuckets<IPricePeriodSummary>> CreateAppendContext() =>
        new PQPricePeriodSummaryAppendContext<IPricePeriodSummary, YearlyToMonthlyPriceSummarySubBuckets<IPricePeriodSummary>>();

    public static DecenniallyYearlyMonthlyPriceSummaryTimeSeriesFile OpenExistingTimeSeriesFile(FileInfo file) =>
        OpenExistingTimeSeriesFile(file.FullName);
}

public class UnlimitedDecenniallyYearlyPriceSummaryTimeSeriesFile : PriceSummaryTimeSeriesFile<UnlimitedDecenniallyYearlyPriceSummaryTimeSeriesFile,
    DecenniallyToYearlyPriceSummarySubBuckets<IPricePeriodSummary>, IPricePeriodSummary>
{
    public UnlimitedDecenniallyYearlyPriceSummaryTimeSeriesFile(PagedMemoryMappedFile pagedMemoryMappedFile, IMutableTimeSeriesFileHeader header)
        : base(pagedMemoryMappedFile, header) { }

    public UnlimitedDecenniallyYearlyPriceSummaryTimeSeriesFile(PriceTimeSeriesFileParameters sourceTickerTimeSeriesFileParams)
        : base(sourceTickerTimeSeriesFileParams.SetFilePeriod(TimeSeriesPeriod.None)
                                               .AssertTimeSeriesEntryType(InstrumentType.PriceSummaryPeriod)
                                               .SetFileFlags(FileFlags.HasSubFileHeader)
                                               .SetInternalIndexSize(10)
                                               .SetInitialFileSize(512 * 1024)) { }

    public override ISessionAppendContext<IPricePeriodSummary, DecenniallyToYearlyPriceSummarySubBuckets<IPricePeriodSummary>>
        CreateAppendContext() =>
        new PQPricePeriodSummaryAppendContext<IPricePeriodSummary, DecenniallyToYearlyPriceSummarySubBuckets<IPricePeriodSummary>>();

    public static UnlimitedDecenniallyYearlyPriceSummaryTimeSeriesFile OpenExistingTimeSeriesFile(FileInfo file) =>
        OpenExistingTimeSeriesFile(file.FullName);
}

public class UnlimitedUnlimitedDecenniallyPriceSummaryTimeSeriesFile : PriceSummaryTimeSeriesFile<
    UnlimitedUnlimitedDecenniallyPriceSummaryTimeSeriesFile,
    UnlimitedToDecenniallyPriceSummarySubBuckets<IPricePeriodSummary>, IPricePeriodSummary>
{
    public UnlimitedUnlimitedDecenniallyPriceSummaryTimeSeriesFile(PagedMemoryMappedFile pagedMemoryMappedFile, IMutableTimeSeriesFileHeader header)
        : base(pagedMemoryMappedFile, header) { }

    public UnlimitedUnlimitedDecenniallyPriceSummaryTimeSeriesFile(PriceTimeSeriesFileParameters sourceTickerTimeSeriesFileParams)
        : base(sourceTickerTimeSeriesFileParams.SetFilePeriod(TimeSeriesPeriod.None)
                                               .AssertTimeSeriesEntryType(InstrumentType.PriceSummaryPeriod)
                                               .SetFileFlags(FileFlags.HasSubFileHeader)
                                               .SetInternalIndexSize(1)
                                               .SetInitialFileSize(512 * 1024)) { }

    public override ISessionAppendContext<IPricePeriodSummary, UnlimitedToDecenniallyPriceSummarySubBuckets<IPricePeriodSummary>>
        CreateAppendContext() =>
        new PQPricePeriodSummaryAppendContext<IPricePeriodSummary, UnlimitedToDecenniallyPriceSummarySubBuckets<IPricePeriodSummary>>();

    public static UnlimitedUnlimitedDecenniallyPriceSummaryTimeSeriesFile OpenExistingTimeSeriesFile(FileInfo file) =>
        OpenExistingTimeSeriesFile(file.FullName);
}
