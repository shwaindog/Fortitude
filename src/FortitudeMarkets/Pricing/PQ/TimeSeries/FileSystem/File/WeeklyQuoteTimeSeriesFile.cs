﻿// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory.UnmanagedMemory.MemoryMappedFiles;
using FortitudeIO.TimeSeries;
using FortitudeIO.TimeSeries.FileSystem.File;
using FortitudeIO.TimeSeries.FileSystem.File.Header;
using FortitudeIO.TimeSeries.FileSystem.File.Session;
using FortitudeMarkets.Pricing.Quotes;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes;
using FortitudeMarkets.Pricing.PQ.TimeSeries.FileSystem.File.Buckets;

#endregion

namespace FortitudeMarkets.Pricing.PQ.TimeSeries.FileSystem.File;

public class WeeklyTickInstantTimeSeriesFile : PriceQuoteTimeSeriesFile<WeeklyTickInstantTimeSeriesFile,
    DailyToHourlyTickInstantSubBuckets<ITickInstant>, ITickInstant>
{
    public WeeklyTickInstantTimeSeriesFile(PagedMemoryMappedFile pagedMemoryMappedFile, IMutableTimeSeriesFileHeader header)
        : base(pagedMemoryMappedFile, header) { }

    public WeeklyTickInstantTimeSeriesFile(PriceTimeSeriesFileParameters sourceTickerTimeSeriesFileParams)
        : base(sourceTickerTimeSeriesFileParams.SetFilePeriod(TimeBoundaryPeriod.OneWeek)
                                               .AssertTimeSeriesEntryType(InstrumentType.Price)
                                               .SetFileFlags(FileFlags.HasSubFileHeader)
                                               .SetInternalIndexSize(7)
                                               .SetInitialFileSize(512 * 1024)) { }

    public override ISessionAppendContext<ITickInstant, DailyToHourlyTickInstantSubBuckets<ITickInstant>> CreateAppendContext() =>
        new PQQuoteAppendContext<ITickInstant, DailyToHourlyTickInstantSubBuckets<ITickInstant>, PQTickInstant>();

    public static WeeklyTickInstantTimeSeriesFile OpenExistingTimeSeriesFile(FileInfo file) => OpenExistingTimeSeriesFile(file.FullName);
}

public class WeeklyLevel1QuoteTimeSeriesFile : PriceQuoteTimeSeriesFile<WeeklyLevel1QuoteTimeSeriesFile,
    DailyToHourlyLevel1QuoteSubBuckets<ILevel1Quote>, ILevel1Quote>
{
    public WeeklyLevel1QuoteTimeSeriesFile(PagedMemoryMappedFile pagedMemoryMappedFile, IMutableTimeSeriesFileHeader header)
        : base(pagedMemoryMappedFile, header) { }

    public WeeklyLevel1QuoteTimeSeriesFile(PriceTimeSeriesFileParameters sourceTickerTimeSeriesFileParams)
        : base(sourceTickerTimeSeriesFileParams.SetFilePeriod(TimeBoundaryPeriod.OneWeek)
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
        : base(sourceTickerTimeSeriesFileParams.SetFilePeriod(TimeBoundaryPeriod.OneWeek)
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
        : base(sourceTickerTimeSeriesFileParams.SetFilePeriod(TimeBoundaryPeriod.OneWeek)
                                               .AssertTimeSeriesEntryType(InstrumentType.Price)
                                               .SetFileFlags(FileFlags.HasSubFileHeader)
                                               .SetInternalIndexSize(7)
                                               .SetInitialFileSize(512 * 1024)) { }

    public override ISessionAppendContext<ILevel3Quote, DailyToHourlyLevel3QuoteSubBuckets<ILevel3Quote>> CreateAppendContext() =>
        new PQQuoteAppendContext<ILevel3Quote, DailyToHourlyLevel3QuoteSubBuckets<ILevel3Quote>, PQLevel3Quote>();

    public static WeeklyLevel3QuoteTimeSeriesFile OpenExistingTimeSeriesFile(FileInfo file) => OpenExistingTimeSeriesFile(file.FullName);
}
