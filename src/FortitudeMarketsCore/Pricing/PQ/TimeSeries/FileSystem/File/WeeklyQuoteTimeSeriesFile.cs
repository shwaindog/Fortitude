// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory.UnmanagedMemory.MemoryMappedFiles;
using FortitudeIO.TimeSeries;
using FortitudeIO.TimeSeries.FileSystem.File;
using FortitudeIO.TimeSeries.FileSystem.File.Header;
using FortitudeIO.TimeSeries.FileSystem.File.Session;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;
using FortitudeMarketsCore.Pricing.PQ.TimeSeries.FileSystem.File.Buckets;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.TimeSeries.FileSystem.File;

public class WeeklyLevel0QuoteTimeSeriesFile : PriceQuoteTimeSeriesFile<WeeklyLevel0QuoteTimeSeriesFile,
    DailyToHourlyLevel0QuoteSubBuckets<ILevel0Quote>, ILevel0Quote>
{
    public WeeklyLevel0QuoteTimeSeriesFile(PagedMemoryMappedFile pagedMemoryMappedFile, IMutableTimeSeriesFileHeader header)
        : base(pagedMemoryMappedFile, header) { }

    public WeeklyLevel0QuoteTimeSeriesFile(PriceTimeSeriesFileParameters sourceTickerTimeSeriesFileParams)
        : base(sourceTickerTimeSeriesFileParams.SetFilePeriod(TimeSeriesPeriod.OneWeek)
                                               .AssertTimeSeriesEntryType(InstrumentType.Price)
                                               .SetFileFlags(FileFlags.HasSubFileHeader)
                                               .SetInternalIndexSize(7)
                                               .SetInitialFileSize(512 * 1024)) { }

    public override ISessionAppendContext<ILevel0Quote, DailyToHourlyLevel0QuoteSubBuckets<ILevel0Quote>> CreateAppendContext() =>
        new PQQuoteAppendContext<ILevel0Quote, DailyToHourlyLevel0QuoteSubBuckets<ILevel0Quote>, PQLevel0Quote>();

    public static WeeklyLevel0QuoteTimeSeriesFile OpenExistingTimeSeriesFile(FileInfo file) => OpenExistingTimeSeriesFile(file.FullName);
}

public class WeeklyLevel1QuoteTimeSeriesFile : PriceQuoteTimeSeriesFile<WeeklyLevel1QuoteTimeSeriesFile,
    DailyToHourlyLevel1QuoteSubBuckets<ILevel1Quote>, ILevel1Quote>
{
    public WeeklyLevel1QuoteTimeSeriesFile(PagedMemoryMappedFile pagedMemoryMappedFile, IMutableTimeSeriesFileHeader header)
        : base(pagedMemoryMappedFile, header) { }

    public WeeklyLevel1QuoteTimeSeriesFile(PriceTimeSeriesFileParameters sourceTickerTimeSeriesFileParams)
        : base(sourceTickerTimeSeriesFileParams.SetFilePeriod(TimeSeriesPeriod.OneWeek)
                                               .AssertTimeSeriesEntryType(InstrumentType.Price)
                                               .SetFileFlags(FileFlags.HasSubFileHeader)
                                               .SetInternalIndexSize(7)
                                               .SetInitialFileSize(512 * 1024)) { }

    public override ISessionAppendContext<ILevel1Quote, DailyToHourlyLevel1QuoteSubBuckets<ILevel1Quote>> CreateAppendContext() =>
        new PQQuoteAppendContext<ILevel1Quote, DailyToHourlyLevel1QuoteSubBuckets<ILevel1Quote>, PQLevel1Quote>();


    public static WeeklyLevel1QuoteTimeSeriesFile OpenExistingTimeSeriesFile(FileInfo file) => OpenExistingTimeSeriesFile(file.FullName);
}

public class WeeklyLevel2QuoteTimeSeriesFile : PriceQuoteTimeSeriesFile<WeeklyLevel2QuoteTimeSeriesFile,
    DailyToHourlyLevel2QuoteSubBuckets<ILevel2Quote>, ILevel2Quote>
{
    public WeeklyLevel2QuoteTimeSeriesFile(PagedMemoryMappedFile pagedMemoryMappedFile, IMutableTimeSeriesFileHeader header)
        : base(pagedMemoryMappedFile, header) { }

    public WeeklyLevel2QuoteTimeSeriesFile(PriceTimeSeriesFileParameters sourceTickerTimeSeriesFileParams)
        : base(sourceTickerTimeSeriesFileParams.SetFilePeriod(TimeSeriesPeriod.OneWeek)
                                               .AssertTimeSeriesEntryType(InstrumentType.Price)
                                               .SetFileFlags(FileFlags.HasSubFileHeader)
                                               .SetInternalIndexSize(7)
                                               .SetInitialFileSize(512 * 1024)) { }

    public override ISessionAppendContext<ILevel2Quote, DailyToHourlyLevel2QuoteSubBuckets<ILevel2Quote>> CreateAppendContext() =>
        new PQQuoteAppendContext<ILevel2Quote, DailyToHourlyLevel2QuoteSubBuckets<ILevel2Quote>, PQLevel2Quote>();

    public static WeeklyLevel2QuoteTimeSeriesFile OpenExistingTimeSeriesFile(FileInfo file) => OpenExistingTimeSeriesFile(file.FullName);
}

public class WeeklyLevel3QuoteTimeSeriesFile : PriceQuoteTimeSeriesFile<WeeklyLevel3QuoteTimeSeriesFile,
    DailyToHourlyLevel3QuoteSubBuckets<ILevel3Quote>, ILevel3Quote>
{
    public WeeklyLevel3QuoteTimeSeriesFile(PagedMemoryMappedFile pagedMemoryMappedFile, IMutableTimeSeriesFileHeader header)
        : base(pagedMemoryMappedFile, header) { }

    public WeeklyLevel3QuoteTimeSeriesFile(PriceTimeSeriesFileParameters sourceTickerTimeSeriesFileParams)
        : base(sourceTickerTimeSeriesFileParams.SetFilePeriod(TimeSeriesPeriod.OneWeek)
                                               .AssertTimeSeriesEntryType(InstrumentType.Price)
                                               .SetFileFlags(FileFlags.HasSubFileHeader)
                                               .SetInternalIndexSize(7)
                                               .SetInitialFileSize(512 * 1024)) { }

    public override ISessionAppendContext<ILevel3Quote, DailyToHourlyLevel3QuoteSubBuckets<ILevel3Quote>> CreateAppendContext() =>
        new PQQuoteAppendContext<ILevel3Quote, DailyToHourlyLevel3QuoteSubBuckets<ILevel3Quote>, PQLevel3Quote>();

    public static WeeklyLevel3QuoteTimeSeriesFile OpenExistingTimeSeriesFile(FileInfo file) => OpenExistingTimeSeriesFile(file.FullName);
}
