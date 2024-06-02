// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory.UnmanagedMemory.MemoryMappedFiles;
using FortitudeIO.TimeSeries;
using FortitudeIO.TimeSeries.FileSystem.File.Header;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.PQ.TimeSeries.FileSystem.File.Buckets;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.TimeSeries.FileSystem.File;

public class OneWeekDailyHourlyLevel0QuoteTimeSeriesFile : PriceQuoteTimeSeriesFile<DailyToOneHourPQLevel0QuoteSubBuckets<ILevel0Quote>, ILevel0Quote>
{
    public OneWeekDailyHourlyLevel0QuoteTimeSeriesFile(PagedMemoryMappedFile pagedMemoryMappedFile, IMutableTimeSeriesFileHeader header)
        : base(pagedMemoryMappedFile, header) { }

    public OneWeekDailyHourlyLevel0QuoteTimeSeriesFile(PriceQuoteCreateFileParameters sourceTickerTimeSeriesFileParams)
        : base(sourceTickerTimeSeriesFileParams.SetFilePeriod(TimeSeriesPeriod.OneWeek)
                                               .SetTimeSeriesEntryType(TimeSeriesEntryType.Price)
                                               .SetInternalIndexSize(7)
                                               .SetInitialFileSize(512 * 1024)) { }
}

public class OneWeekDailyHourlyLevel1QuoteTimeSeriesFile : PriceQuoteTimeSeriesFile<DailyToOneHourPQLevel1QuoteSubBuckets<ILevel1Quote>, ILevel1Quote>
{
    public OneWeekDailyHourlyLevel1QuoteTimeSeriesFile(PagedMemoryMappedFile pagedMemoryMappedFile, IMutableTimeSeriesFileHeader header)
        : base(pagedMemoryMappedFile, header) { }

    public OneWeekDailyHourlyLevel1QuoteTimeSeriesFile(PriceQuoteCreateFileParameters sourceTickerTimeSeriesFileParams)
        : base(sourceTickerTimeSeriesFileParams.SetFilePeriod(TimeSeriesPeriod.OneWeek)
                                               .SetTimeSeriesEntryType(TimeSeriesEntryType.Price)
                                               .SetInternalIndexSize(7)
                                               .SetInitialFileSize(512 * 1024)) { }
}

public class OneWeekDailyHourlyLevel2QuoteTimeSeriesFile : PriceQuoteTimeSeriesFile<DailyToOneHourPQLevel2QuoteSubBuckets<ILevel2Quote>, ILevel2Quote>
{
    public OneWeekDailyHourlyLevel2QuoteTimeSeriesFile(PagedMemoryMappedFile pagedMemoryMappedFile, IMutableTimeSeriesFileHeader header)
        : base(pagedMemoryMappedFile, header) { }

    public OneWeekDailyHourlyLevel2QuoteTimeSeriesFile(PriceQuoteCreateFileParameters sourceTickerTimeSeriesFileParams)
        : base(sourceTickerTimeSeriesFileParams.SetFilePeriod(TimeSeriesPeriod.OneWeek)
                                               .SetTimeSeriesEntryType(TimeSeriesEntryType.Price)
                                               .SetInternalIndexSize(7)
                                               .SetInitialFileSize(512 * 1024)) { }
}

public class OneWeekDailyHourlyLevel3QuoteTimeSeriesFile : PriceQuoteTimeSeriesFile<DailyToOneHourPQLevel3QuoteSubBuckets<ILevel3Quote>, ILevel3Quote>
{
    public OneWeekDailyHourlyLevel3QuoteTimeSeriesFile(PagedMemoryMappedFile pagedMemoryMappedFile, IMutableTimeSeriesFileHeader header)
        : base(pagedMemoryMappedFile, header) { }

    public OneWeekDailyHourlyLevel3QuoteTimeSeriesFile(PriceQuoteCreateFileParameters sourceTickerTimeSeriesFileParams)
        : base(sourceTickerTimeSeriesFileParams.SetFilePeriod(TimeSeriesPeriod.OneWeek)
                                               .SetTimeSeriesEntryType(TimeSeriesEntryType.Price)
                                               .SetInternalIndexSize(7)
                                               .SetInitialFileSize(512 * 1024)) { }
}
