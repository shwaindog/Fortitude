// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Types;
using FortitudeIO.TimeSeries;
using FortitudeIO.TimeSeries.FileSystem;
using FortitudeIO.TimeSeries.FileSystem.DirectoryStructure;
using FortitudeIO.TimeSeries.FileSystem.File;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.PQ.TimeSeries.FileSystem.File;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.TimeSeries.FileSystem;

public class PQOneWeekQuoteRepoFileFactory<TEntry> : TimeSeriesRepositoryFileFactory<TEntry>
    where TEntry : ITimeSeriesEntry<TEntry>, ILevel0Quote

{
    protected override TimeSeriesFileParameters CreateTimeSeriesFileParameters
        (FileInfo fileInfo, IInstrument instrument, TimeSeriesPeriod filePeriod, DateTime filePeriodTime)
    {
        var fileStart = filePeriod.ContainingPeriodBoundaryStart(filePeriodTime);
        return new TimeSeriesFileParameters(fileInfo, instrument, filePeriod, fileStart, 7, FileFlags.WriterOpened);
    }

    protected virtual PriceTimeSeriesFileParameters CreatePriceQuoteTimeSeriesFileParameters
        (FileInfo fileInfo, IInstrument instrument, TimeSeriesPeriod filePeriod, DateTime filePeriodTime)
    {
        if (filePeriod != TimeSeriesPeriod.OneWeek) throw new Exception("Expected file period to be one week");
        var sourceTickerQuoteInfo = instrument as ISourceTickerQuoteInfo;
        if (sourceTickerQuoteInfo == null) throw new Exception("Expected instrument to be of ISourceTickerQuoteInfo");

        var timeSeriesFileParams = CreateTimeSeriesFileParameters(fileInfo, instrument, filePeriod, filePeriodTime);

        return new PriceTimeSeriesFileParameters(sourceTickerQuoteInfo, timeSeriesFileParams);
    }

    public override ITimeSeriesEntryFile<TEntry>? OpenExistingEntryFile(FileInfo fileInfo)
    {
        return EntryType switch
               {
                   _ when EntryType == typeof(ILevel0Quote) => (ITimeSeriesEntryFile<TEntry>)WeeklyLevel0QuoteTimeSeriesFile
                       .OpenExistingTimeSeriesFile(fileInfo)
                 , _ when EntryType == typeof(ILevel1Quote) => (ITimeSeriesEntryFile<TEntry>)WeeklyLevel1QuoteTimeSeriesFile
                       .OpenExistingTimeSeriesFile(fileInfo)
                 , _ when EntryType == typeof(ILevel2Quote) => (ITimeSeriesEntryFile<TEntry>)WeeklyLevel2QuoteTimeSeriesFile
                       .OpenExistingTimeSeriesFile(fileInfo)
                 , _ when EntryType == typeof(ILevel3Quote) => (ITimeSeriesEntryFile<TEntry>)WeeklyLevel3QuoteTimeSeriesFile
                       .OpenExistingTimeSeriesFile(fileInfo)
                 , _ => throw new Exception("Expected entry type to be ILevel#Quote type")
               };
    }

    public override ITimeSeriesFile? OpenExisting(FileInfo fileInfo) => OpenExistingEntryFile(fileInfo);

    public override bool IsBestFactoryFor(IInstrument instrument)
    {
        var srcTickerInfo = instrument as ISourceTickerQuoteInfo;
        var category      = srcTickerInfo?.PublishedQuoteLevel.ToString() ?? instrument[nameof(RepositoryPathName.Category)];
        var entryType     = typeof(TEntry);
        return instrument.Type == InstrumentType.Price
            && category switch
               {
                   nameof(QuoteLevel.Level0) => !entryType.ImplementsInterface<ILevel1Quote>()
                 , nameof(QuoteLevel.Level1) => !entryType.ImplementsInterface<ILevel2Quote>()
                 , nameof(QuoteLevel.Level2) => !entryType.ImplementsInterface<ILevel3Quote>()
                 , _                         => true
               };
    }

    public override ITimeSeriesEntryFile<TEntry> OpenOrCreate
        (FileInfo fileInfo, IInstrument instrument, TimeSeriesPeriod filePeriod, DateTime filePeriodTime)
    {
        var priceQuoteFileParams = CreatePriceQuoteTimeSeriesFileParameters(fileInfo, instrument, filePeriod, filePeriodTime);
        return EntryType switch
               {
                   _ when EntryType == typeof(ILevel0Quote) => (ITimeSeriesEntryFile<TEntry>)new WeeklyLevel0QuoteTimeSeriesFile(priceQuoteFileParams)
                 , _ when EntryType == typeof(ILevel1Quote) => (ITimeSeriesEntryFile<TEntry>)new WeeklyLevel1QuoteTimeSeriesFile(priceQuoteFileParams)
                 , _ when EntryType == typeof(ILevel2Quote) => (ITimeSeriesEntryFile<TEntry>)new WeeklyLevel2QuoteTimeSeriesFile(priceQuoteFileParams)
                 , _ when EntryType == typeof(ILevel3Quote) => (ITimeSeriesEntryFile<TEntry>)new WeeklyLevel3QuoteTimeSeriesFile(priceQuoteFileParams)
                 , _                                        => throw new Exception("Expected entry type to be ILevel#Quote type")
               };
    }
}
