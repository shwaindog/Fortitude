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

public class WeeklyDailyHourlyPriceSummaryTimeSeriesFile :
    PriceSummaryTimeSeriesFile<WeeklyDailyHourlyPriceSummaryTimeSeriesFile, DailyToHourlyPriceSummarySubBuckets>
{
    public WeeklyDailyHourlyPriceSummaryTimeSeriesFile(PagedMemoryMappedFile pagedMemoryMappedFile, IMutableTimeSeriesFileHeader header)
        : base(pagedMemoryMappedFile, header) { }

    public WeeklyDailyHourlyPriceSummaryTimeSeriesFile(PriceTimeSeriesFileParameters sourceTickerTimeSeriesFileParams)
        : base(sourceTickerTimeSeriesFileParams.SetFilePeriod(TimeSeriesPeriod.OneWeek)
                                               .AssertTimeSeriesEntryType(InstrumentType.PriceSummaryPeriod)
                                               .SetFileFlags(FileFlags.HasSubFileHeader)
                                               .SetInternalIndexSize(7)
                                               .SetInitialFileSize(512 * 1024)) { }

    public override ISessionAppendContext<IPricePeriodSummary, DailyToHourlyPriceSummarySubBuckets> CreateAppendContext() =>
        new PQPricePeriodSummaryAppendContext<IPricePeriodSummary, DailyToHourlyPriceSummarySubBuckets>();

    public static WeeklyDailyHourlyPriceSummaryTimeSeriesFile OpenExistingTimeSeriesFile(FileInfo file) => OpenExistingTimeSeriesFile(file.FullName);
}

public class WeeklyFourHourlyPriceSummaryTimeSeriesFile :
    PriceSummaryTimeSeriesFile<WeeklyFourHourlyPriceSummaryTimeSeriesFile, FourHourlyPriceSummaryDataBucket>
{
    public WeeklyFourHourlyPriceSummaryTimeSeriesFile(PagedMemoryMappedFile pagedMemoryMappedFile, IMutableTimeSeriesFileHeader header)
        : base(pagedMemoryMappedFile, header) { }

    public WeeklyFourHourlyPriceSummaryTimeSeriesFile(PriceTimeSeriesFileParameters sourceTickerTimeSeriesFileParams)
        : base(sourceTickerTimeSeriesFileParams.SetFilePeriod(TimeSeriesPeriod.OneWeek)
                                               .AssertTimeSeriesEntryType(InstrumentType.PriceSummaryPeriod)
                                               .SetFileFlags(FileFlags.HasSubFileHeader)
                                               .SetInternalIndexSize(42)
                                               .SetInitialFileSize(512 * 1024)) { }

    public override ISessionAppendContext<IPricePeriodSummary, FourHourlyPriceSummaryDataBucket> CreateAppendContext() =>
        new PQPricePeriodSummaryAppendContext<IPricePeriodSummary, FourHourlyPriceSummaryDataBucket>();

    public static WeeklyFourHourlyPriceSummaryTimeSeriesFile OpenExistingTimeSeriesFile(FileInfo file) => OpenExistingTimeSeriesFile(file.FullName);
}

public class MonthlyFourHourlyPriceSummaryTimeSeriesFile :
    PriceSummaryTimeSeriesFile<MonthlyFourHourlyPriceSummaryTimeSeriesFile, FourHourlyPriceSummaryDataBucket>
{
    public MonthlyFourHourlyPriceSummaryTimeSeriesFile(PagedMemoryMappedFile pagedMemoryMappedFile, IMutableTimeSeriesFileHeader header)
        : base(pagedMemoryMappedFile, header) { }

    public MonthlyFourHourlyPriceSummaryTimeSeriesFile(PriceTimeSeriesFileParameters sourceTickerTimeSeriesFileParams)
        : base(sourceTickerTimeSeriesFileParams.SetFilePeriod(TimeSeriesPeriod.OneMonth)
                                               .AssertTimeSeriesEntryType(InstrumentType.PriceSummaryPeriod)
                                               .SetFileFlags(FileFlags.HasSubFileHeader)
                                               .SetInternalIndexSize(186)
                                               .SetInitialFileSize(512 * 1024)) { }

    public override ISessionAppendContext<IPricePeriodSummary, FourHourlyPriceSummaryDataBucket> CreateAppendContext() =>
        new PQPricePeriodSummaryAppendContext<IPricePeriodSummary, FourHourlyPriceSummaryDataBucket>();

    public static MonthlyFourHourlyPriceSummaryTimeSeriesFile OpenExistingTimeSeriesFile(FileInfo file) => OpenExistingTimeSeriesFile(file.FullName);
}

public class MonthlyDailyHourlyPriceSummaryTimeSeriesFile :
    PriceSummaryTimeSeriesFile<MonthlyDailyHourlyPriceSummaryTimeSeriesFile, DailyToHourlyPriceSummarySubBuckets>
{
    public MonthlyDailyHourlyPriceSummaryTimeSeriesFile(PagedMemoryMappedFile pagedMemoryMappedFile, IMutableTimeSeriesFileHeader header)
        : base(pagedMemoryMappedFile, header) { }

    public MonthlyDailyHourlyPriceSummaryTimeSeriesFile(PriceTimeSeriesFileParameters sourceTickerTimeSeriesFileParams)
        : base(sourceTickerTimeSeriesFileParams.SetFilePeriod(TimeSeriesPeriod.OneMonth)
                                               .AssertTimeSeriesEntryType(InstrumentType.PriceSummaryPeriod)
                                               .SetFileFlags(FileFlags.HasSubFileHeader)
                                               .SetInternalIndexSize(31)
                                               .SetInitialFileSize(512 * 1024)) { }

    public override ISessionAppendContext<IPricePeriodSummary, DailyToHourlyPriceSummarySubBuckets> CreateAppendContext() =>
        new PQPricePeriodSummaryAppendContext<IPricePeriodSummary, DailyToHourlyPriceSummarySubBuckets>();

    public static MonthlyDailyHourlyPriceSummaryTimeSeriesFile OpenExistingTimeSeriesFile(FileInfo file) => OpenExistingTimeSeriesFile(file.FullName);
}

public class MonthlyWeeklyFourHourlyPriceSummaryTimeSeriesFile :
    PriceSummaryTimeSeriesFile<MonthlyWeeklyFourHourlyPriceSummaryTimeSeriesFile, WeeklyToFourHourlyPriceSummarySubBuckets>
{
    public MonthlyWeeklyFourHourlyPriceSummaryTimeSeriesFile(PagedMemoryMappedFile pagedMemoryMappedFile, IMutableTimeSeriesFileHeader header)
        : base(pagedMemoryMappedFile, header) { }

    public MonthlyWeeklyFourHourlyPriceSummaryTimeSeriesFile(PriceTimeSeriesFileParameters sourceTickerTimeSeriesFileParams)
        : base(sourceTickerTimeSeriesFileParams.SetFilePeriod(TimeSeriesPeriod.OneWeek)
                                               .AssertTimeSeriesEntryType(InstrumentType.PriceSummaryPeriod)
                                               .SetFileFlags(FileFlags.HasSubFileHeader)
                                               .SetInternalIndexSize(4)
                                               .SetInitialFileSize(512 * 1024)) { }

    public override ISessionAppendContext<IPricePeriodSummary, WeeklyToFourHourlyPriceSummarySubBuckets> CreateAppendContext() =>
        new PQPricePeriodSummaryAppendContext<IPricePeriodSummary, WeeklyToFourHourlyPriceSummarySubBuckets>();

    public static MonthlyWeeklyFourHourlyPriceSummaryTimeSeriesFile OpenExistingTimeSeriesFile
        (FileInfo file) =>
        OpenExistingTimeSeriesFile(file.FullName);
}

public class MonthlyWeeklyDailyPriceSummaryTimeSeriesFile :
    PriceSummaryTimeSeriesFile<MonthlyWeeklyDailyPriceSummaryTimeSeriesFile, WeeklyToDailyPriceSummarySubBuckets>
{
    public MonthlyWeeklyDailyPriceSummaryTimeSeriesFile(PagedMemoryMappedFile pagedMemoryMappedFile, IMutableTimeSeriesFileHeader header)
        : base(pagedMemoryMappedFile, header) { }

    public MonthlyWeeklyDailyPriceSummaryTimeSeriesFile(PriceTimeSeriesFileParameters sourceTickerTimeSeriesFileParams)
        : base(sourceTickerTimeSeriesFileParams.SetFilePeriod(TimeSeriesPeriod.OneMonth)
                                               .AssertTimeSeriesEntryType(InstrumentType.PriceSummaryPeriod)
                                               .SetFileFlags(FileFlags.HasSubFileHeader)
                                               .SetInternalIndexSize(4)
                                               .SetInitialFileSize(512 * 1024)) { }

    public override ISessionAppendContext<IPricePeriodSummary, WeeklyToDailyPriceSummarySubBuckets> CreateAppendContext() =>
        new PQPricePeriodSummaryAppendContext<IPricePeriodSummary, WeeklyToDailyPriceSummarySubBuckets>();

    public static MonthlyWeeklyDailyPriceSummaryTimeSeriesFile OpenExistingTimeSeriesFile(FileInfo file) => OpenExistingTimeSeriesFile(file.FullName);
}

public class YearlyMonthlyFourHourlyPriceSummaryTimeSeriesFile :
    PriceSummaryTimeSeriesFile<YearlyMonthlyFourHourlyPriceSummaryTimeSeriesFile, MonthlyToFourHourlyPriceSummarySubBuckets>
{
    public YearlyMonthlyFourHourlyPriceSummaryTimeSeriesFile(PagedMemoryMappedFile pagedMemoryMappedFile, IMutableTimeSeriesFileHeader header)
        : base(pagedMemoryMappedFile, header) { }

    public YearlyMonthlyFourHourlyPriceSummaryTimeSeriesFile(PriceTimeSeriesFileParameters sourceTickerTimeSeriesFileParams)
        : base(sourceTickerTimeSeriesFileParams.SetFilePeriod(TimeSeriesPeriod.OneYear)
                                               .AssertTimeSeriesEntryType(InstrumentType.PriceSummaryPeriod)
                                               .SetFileFlags(FileFlags.HasSubFileHeader)
                                               .SetInternalIndexSize(12)
                                               .SetInitialFileSize(512 * 1024)) { }

    public override ISessionAppendContext<IPricePeriodSummary, MonthlyToFourHourlyPriceSummarySubBuckets> CreateAppendContext() =>
        new PQPricePeriodSummaryAppendContext<IPricePeriodSummary, MonthlyToFourHourlyPriceSummarySubBuckets>();

    public static YearlyMonthlyFourHourlyPriceSummaryTimeSeriesFile OpenExistingTimeSeriesFile
        (FileInfo file) =>
        OpenExistingTimeSeriesFile(file.FullName);
}

public class YearlyWeeklyDailyPriceSummaryTimeSeriesFile :
    PriceSummaryTimeSeriesFile<YearlyWeeklyDailyPriceSummaryTimeSeriesFile, WeeklyToDailyPriceSummarySubBuckets>
{
    public YearlyWeeklyDailyPriceSummaryTimeSeriesFile(PagedMemoryMappedFile pagedMemoryMappedFile, IMutableTimeSeriesFileHeader header)
        : base(pagedMemoryMappedFile, header) { }

    public YearlyWeeklyDailyPriceSummaryTimeSeriesFile(PriceTimeSeriesFileParameters sourceTickerTimeSeriesFileParams)
        : base(sourceTickerTimeSeriesFileParams.SetFilePeriod(TimeSeriesPeriod.OneYear)
                                               .AssertTimeSeriesEntryType(InstrumentType.PriceSummaryPeriod)
                                               .SetFileFlags(FileFlags.HasSubFileHeader)
                                               .SetInternalIndexSize(53)
                                               .SetInitialFileSize(512 * 1024)) { }

    public override ISessionAppendContext<IPricePeriodSummary, WeeklyToDailyPriceSummarySubBuckets> CreateAppendContext() =>
        new PQPricePeriodSummaryAppendContext<IPricePeriodSummary, WeeklyToDailyPriceSummarySubBuckets>();

    public static YearlyWeeklyDailyPriceSummaryTimeSeriesFile OpenExistingTimeSeriesFile(FileInfo file) => OpenExistingTimeSeriesFile(file.FullName);
}

public class YearlyMonthlyWeeklyPriceSummaryTimeSeriesFile :
    PriceSummaryTimeSeriesFile<YearlyMonthlyWeeklyPriceSummaryTimeSeriesFile, MonthlyToWeeklyPriceSummarySubBuckets>
{
    public YearlyMonthlyWeeklyPriceSummaryTimeSeriesFile(PagedMemoryMappedFile pagedMemoryMappedFile, IMutableTimeSeriesFileHeader header)
        : base(pagedMemoryMappedFile, header) { }

    public YearlyMonthlyWeeklyPriceSummaryTimeSeriesFile(PriceTimeSeriesFileParameters sourceTickerTimeSeriesFileParams)
        : base(sourceTickerTimeSeriesFileParams.SetFilePeriod(TimeSeriesPeriod.OneYear)
                                               .AssertTimeSeriesEntryType(InstrumentType.PriceSummaryPeriod)
                                               .SetFileFlags(FileFlags.HasSubFileHeader)
                                               .SetInternalIndexSize(12)
                                               .SetInitialFileSize(512 * 1024)) { }

    public override ISessionAppendContext<IPricePeriodSummary, MonthlyToWeeklyPriceSummarySubBuckets> CreateAppendContext() =>
        new PQPricePeriodSummaryAppendContext<IPricePeriodSummary, MonthlyToWeeklyPriceSummarySubBuckets>();

    public static YearlyMonthlyWeeklyPriceSummaryTimeSeriesFile OpenExistingTimeSeriesFile(FileInfo file) =>
        OpenExistingTimeSeriesFile(file.FullName);
}

public class DecenniallyMonthlyWeeklyPriceSummaryTimeSeriesFile :
    PriceSummaryTimeSeriesFile<DecenniallyMonthlyWeeklyPriceSummaryTimeSeriesFile, MonthlyToWeeklyPriceSummarySubBuckets>
{
    public DecenniallyMonthlyWeeklyPriceSummaryTimeSeriesFile(PagedMemoryMappedFile pagedMemoryMappedFile, IMutableTimeSeriesFileHeader header)
        : base(pagedMemoryMappedFile, header) { }

    public DecenniallyMonthlyWeeklyPriceSummaryTimeSeriesFile(PriceTimeSeriesFileParameters sourceTickerTimeSeriesFileParams)
        : base(sourceTickerTimeSeriesFileParams.SetFilePeriod(TimeSeriesPeriod.OneDecade)
                                               .AssertTimeSeriesEntryType(InstrumentType.PriceSummaryPeriod)
                                               .SetFileFlags(FileFlags.HasSubFileHeader)
                                               .SetInternalIndexSize(120)
                                               .SetInitialFileSize(512 * 1024)) { }

    public override ISessionAppendContext<IPricePeriodSummary, MonthlyToWeeklyPriceSummarySubBuckets> CreateAppendContext() =>
        new PQPricePeriodSummaryAppendContext<IPricePeriodSummary, MonthlyToWeeklyPriceSummarySubBuckets>();

    public static DecenniallyMonthlyWeeklyPriceSummaryTimeSeriesFile OpenExistingTimeSeriesFile(FileInfo file) =>
        OpenExistingTimeSeriesFile(file.FullName);
}

public class DecenniallyYearlyMonthlyPriceSummaryTimeSeriesFile :
    PriceSummaryTimeSeriesFile<DecenniallyYearlyMonthlyPriceSummaryTimeSeriesFile, YearlyToMonthlyPriceSummarySubBuckets>
{
    public DecenniallyYearlyMonthlyPriceSummaryTimeSeriesFile(PagedMemoryMappedFile pagedMemoryMappedFile, IMutableTimeSeriesFileHeader header)
        : base(pagedMemoryMappedFile, header) { }

    public DecenniallyYearlyMonthlyPriceSummaryTimeSeriesFile(PriceTimeSeriesFileParameters sourceTickerTimeSeriesFileParams)
        : base(sourceTickerTimeSeriesFileParams.SetFilePeriod(TimeSeriesPeriod.OneDecade)
                                               .AssertTimeSeriesEntryType(InstrumentType.PriceSummaryPeriod)
                                               .SetFileFlags(FileFlags.HasSubFileHeader)
                                               .SetInternalIndexSize(10)
                                               .SetInitialFileSize(512 * 1024)) { }

    public override ISessionAppendContext<IPricePeriodSummary, YearlyToMonthlyPriceSummarySubBuckets> CreateAppendContext() =>
        new PQPricePeriodSummaryAppendContext<IPricePeriodSummary, YearlyToMonthlyPriceSummarySubBuckets>();

    public static DecenniallyYearlyMonthlyPriceSummaryTimeSeriesFile OpenExistingTimeSeriesFile(FileInfo file) =>
        OpenExistingTimeSeriesFile(file.FullName);
}

public class UnlimitedDecenniallyYearlyPriceSummaryTimeSeriesFile :
    PriceSummaryTimeSeriesFile<UnlimitedDecenniallyYearlyPriceSummaryTimeSeriesFile, DecenniallyToYearlyPriceSummarySubBuckets>
{
    public UnlimitedDecenniallyYearlyPriceSummaryTimeSeriesFile(PagedMemoryMappedFile pagedMemoryMappedFile, IMutableTimeSeriesFileHeader header)
        : base(pagedMemoryMappedFile, header) { }

    public UnlimitedDecenniallyYearlyPriceSummaryTimeSeriesFile(PriceTimeSeriesFileParameters sourceTickerTimeSeriesFileParams)
        : base(sourceTickerTimeSeriesFileParams.SetFilePeriod(TimeSeriesPeriod.None)
                                               .AssertTimeSeriesEntryType(InstrumentType.PriceSummaryPeriod)
                                               .SetFileFlags(FileFlags.HasSubFileHeader)
                                               .SetInternalIndexSize(10)
                                               .SetInitialFileSize(512 * 1024)) { }

    public override ISessionAppendContext<IPricePeriodSummary, DecenniallyToYearlyPriceSummarySubBuckets>
        CreateAppendContext() =>
        new PQPricePeriodSummaryAppendContext<IPricePeriodSummary, DecenniallyToYearlyPriceSummarySubBuckets>();

    public static UnlimitedDecenniallyYearlyPriceSummaryTimeSeriesFile OpenExistingTimeSeriesFile(FileInfo file) =>
        OpenExistingTimeSeriesFile(file.FullName);
}

public class UnlimitedDecenniallyPriceSummaryTimeSeriesFile :
    PriceSummaryTimeSeriesFile<UnlimitedDecenniallyPriceSummaryTimeSeriesFile, DecenniallyPriceSummaryDataBucket>
{
    public UnlimitedDecenniallyPriceSummaryTimeSeriesFile(PagedMemoryMappedFile pagedMemoryMappedFile, IMutableTimeSeriesFileHeader header)
        : base(pagedMemoryMappedFile, header) { }

    public UnlimitedDecenniallyPriceSummaryTimeSeriesFile(PriceTimeSeriesFileParameters sourceTickerTimeSeriesFileParams)
        : base(sourceTickerTimeSeriesFileParams.SetFilePeriod(TimeSeriesPeriod.None)
                                               .AssertTimeSeriesEntryType(InstrumentType.PriceSummaryPeriod)
                                               .SetFileFlags(FileFlags.HasSubFileHeader)
                                               .SetInternalIndexSize(1)
                                               .SetInitialFileSize(512 * 1024)) { }

    public override ISessionAppendContext<IPricePeriodSummary, DecenniallyPriceSummaryDataBucket>
        CreateAppendContext() =>
        new PQPricePeriodSummaryAppendContext<IPricePeriodSummary, DecenniallyPriceSummaryDataBucket>();

    public static UnlimitedDecenniallyPriceSummaryTimeSeriesFile OpenExistingTimeSeriesFile(FileInfo file) =>
        OpenExistingTimeSeriesFile(file.FullName);
}

public class UnlimitedPriceSummaryTimeSeriesFile :
    PriceSummaryTimeSeriesFile<UnlimitedPriceSummaryTimeSeriesFile, UnlimitedPriceSummaryDataBucket>
{
    public UnlimitedPriceSummaryTimeSeriesFile(PagedMemoryMappedFile pagedMemoryMappedFile, IMutableTimeSeriesFileHeader header)
        : base(pagedMemoryMappedFile, header) { }

    public UnlimitedPriceSummaryTimeSeriesFile(PriceTimeSeriesFileParameters sourceTickerTimeSeriesFileParams)
        : base(sourceTickerTimeSeriesFileParams.SetFilePeriod(TimeSeriesPeriod.None)
                                               .AssertTimeSeriesEntryType(InstrumentType.PriceSummaryPeriod)
                                               .SetFileFlags(FileFlags.HasSubFileHeader)
                                               .SetInternalIndexSize(1)
                                               .SetInitialFileSize(512 * 1024)) { }

    public override ISessionAppendContext<IPricePeriodSummary, UnlimitedPriceSummaryDataBucket>
        CreateAppendContext() =>
        new PQPricePeriodSummaryAppendContext<IPricePeriodSummary, UnlimitedPriceSummaryDataBucket>();

    public static UnlimitedPriceSummaryTimeSeriesFile OpenExistingTimeSeriesFile(FileInfo file) => OpenExistingTimeSeriesFile(file.FullName);
}
