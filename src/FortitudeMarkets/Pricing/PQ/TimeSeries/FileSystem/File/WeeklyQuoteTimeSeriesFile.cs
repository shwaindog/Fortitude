// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory.UnmanagedMemory.MemoryMappedFiles;
using FortitudeIO.TimeSeries;
using FortitudeIO.TimeSeries.FileSystem.File;
using FortitudeIO.TimeSeries.FileSystem.File.Header;
using FortitudeIO.TimeSeries.FileSystem.File.Session;
using FortitudeMarkets.Pricing.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes;
using FortitudeMarkets.Pricing.PQ.TimeSeries.FileSystem.File.Buckets;

#endregion

namespace FortitudeMarkets.Pricing.PQ.TimeSeries.FileSystem.File;

public class WeeklyTickInstantTimeSeriesFile : PriceQuoteTimeSeriesFile<WeeklyTickInstantTimeSeriesFile,
    DailyToHourlyTickInstantSubBuckets<IPublishableTickInstant>, IPublishableTickInstant>
{
    public WeeklyTickInstantTimeSeriesFile(PagedMemoryMappedFile pagedMemoryMappedFile, IMutableTimeSeriesFileHeader header)
        : base(pagedMemoryMappedFile, header) { }

    public WeeklyTickInstantTimeSeriesFile(PriceTimeSeriesFileParameters sourceTickerTimeSeriesFileParams)
        : base(sourceTickerTimeSeriesFileParams.SetFilePeriod(TimeBoundaryPeriod.OneWeek)
                                               .AssertTimeSeriesEntryType(InstrumentType.Price)
                                               .SetFileFlags(FileFlags.HasSubFileHeader)
                                               .SetInternalIndexSize(7)
                                               .SetInitialFileSize(512 * 1024)) { }

    public override ISessionAppendContext<IPublishableTickInstant, DailyToHourlyTickInstantSubBuckets<IPublishableTickInstant>> CreateAppendContext() =>
        new PQQuoteAppendContext<IPublishableTickInstant, DailyToHourlyTickInstantSubBuckets<IPublishableTickInstant>, PQPublishableTickInstant>();

    public static WeeklyTickInstantTimeSeriesFile OpenExistingTimeSeriesFile(FileInfo file) => OpenExistingTimeSeriesFile(file.FullName);
}

public class WeeklyLevel1QuoteTimeSeriesFile : PriceQuoteTimeSeriesFile<WeeklyLevel1QuoteTimeSeriesFile,
    DailyToHourlyLevel1QuoteSubBuckets<IPublishableLevel1Quote>, IPublishableLevel1Quote>
{
    public WeeklyLevel1QuoteTimeSeriesFile(PagedMemoryMappedFile pagedMemoryMappedFile, IMutableTimeSeriesFileHeader header)
        : base(pagedMemoryMappedFile, header) { }

    public WeeklyLevel1QuoteTimeSeriesFile(PriceTimeSeriesFileParameters sourceTickerTimeSeriesFileParams)
        : base(sourceTickerTimeSeriesFileParams.SetFilePeriod(TimeBoundaryPeriod.OneWeek)
                                               .AssertTimeSeriesEntryType(InstrumentType.Price)
                                               .SetFileFlags(FileFlags.HasSubFileHeader)
                                               .SetInternalIndexSize(7)
                                               .SetInitialFileSize(512 * 1024)) { }

    public override ISessionAppendContext<IPublishableLevel1Quote, DailyToHourlyLevel1QuoteSubBuckets<IPublishableLevel1Quote>> CreateAppendContext() =>
        new PQQuoteAppendContext<IPublishableLevel1Quote, DailyToHourlyLevel1QuoteSubBuckets<IPublishableLevel1Quote>, PQPublishableLevel1Quote>();


    public static WeeklyLevel1QuoteTimeSeriesFile OpenExistingTimeSeriesFile(FileInfo file) => OpenExistingTimeSeriesFile(file.FullName);
}

public class WeeklyLevel2QuoteTimeSeriesFile : PriceQuoteTimeSeriesFile<WeeklyLevel2QuoteTimeSeriesFile,
    DailyToHourlyLevel2QuoteSubBuckets<IPublishableLevel2Quote>, IPublishableLevel2Quote>
{
    public WeeklyLevel2QuoteTimeSeriesFile(PagedMemoryMappedFile pagedMemoryMappedFile, IMutableTimeSeriesFileHeader header)
        : base(pagedMemoryMappedFile, header) { }

    public WeeklyLevel2QuoteTimeSeriesFile(PriceTimeSeriesFileParameters sourceTickerTimeSeriesFileParams)
        : base(sourceTickerTimeSeriesFileParams.SetFilePeriod(TimeBoundaryPeriod.OneWeek)
                                               .AssertTimeSeriesEntryType(InstrumentType.Price)
                                               .SetFileFlags(FileFlags.HasSubFileHeader)
                                               .SetInternalIndexSize(7)
                                               .SetInitialFileSize(512 * 1024)) { }

    public override ISessionAppendContext<IPublishableLevel2Quote, DailyToHourlyLevel2QuoteSubBuckets<IPublishableLevel2Quote>> CreateAppendContext() =>
        new PQQuoteAppendContext<IPublishableLevel2Quote, DailyToHourlyLevel2QuoteSubBuckets<IPublishableLevel2Quote>, PQPublishableLevel2Quote>();

    public static WeeklyLevel2QuoteTimeSeriesFile OpenExistingTimeSeriesFile(FileInfo file) => OpenExistingTimeSeriesFile(file.FullName);
}

public class WeeklyLevel3QuoteTimeSeriesFile : PriceQuoteTimeSeriesFile<WeeklyLevel3QuoteTimeSeriesFile,
    DailyToHourlyLevel3QuoteSubBuckets<IPublishableLevel3Quote>, IPublishableLevel3Quote>
{
    public WeeklyLevel3QuoteTimeSeriesFile(PagedMemoryMappedFile pagedMemoryMappedFile, IMutableTimeSeriesFileHeader header)
        : base(pagedMemoryMappedFile, header) { }

    public WeeklyLevel3QuoteTimeSeriesFile(PriceTimeSeriesFileParameters sourceTickerTimeSeriesFileParams)
        : base(sourceTickerTimeSeriesFileParams.SetFilePeriod(TimeBoundaryPeriod.OneWeek)
                                               .AssertTimeSeriesEntryType(InstrumentType.Price)
                                               .SetFileFlags(FileFlags.HasSubFileHeader)
                                               .SetInternalIndexSize(7)
                                               .SetInitialFileSize(512 * 1024)) { }

    public override ISessionAppendContext<IPublishableLevel3Quote, DailyToHourlyLevel3QuoteSubBuckets<IPublishableLevel3Quote>> CreateAppendContext() =>
        new PQQuoteAppendContext<IPublishableLevel3Quote, DailyToHourlyLevel3QuoteSubBuckets<IPublishableLevel3Quote>, PQPublishableLevel3Quote>();

    public static WeeklyLevel3QuoteTimeSeriesFile OpenExistingTimeSeriesFile(FileInfo file) => OpenExistingTimeSeriesFile(file.FullName);
}
