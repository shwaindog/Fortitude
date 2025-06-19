// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.Types;
using FortitudeIO.Storage.TimeSeries;
using FortitudeIO.Storage.TimeSeries.FileSystem;
using FortitudeIO.Storage.TimeSeries.FileSystem.DirectoryStructure;
using FortitudeIO.Storage.TimeSeries.FileSystem.File;
using FortitudeMarkets.Pricing.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Storage.TimeSeries.FileSystem.File;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Storage.TimeSeries.FileSystem;

public class PQOneWeekQuoteRepoFileFactory<TEntry> : TimeSeriesRepositoryFileFactory<TEntry>
    where TEntry : ITimeSeriesEntry, ITickInstant

{
    protected override TimeSeriesFileParameters CreateTimeSeriesFileParameters
        (FileInfo fileInfo, IInstrument instrument, TimeBoundaryPeriod filePeriod, DateTime filePeriodTime)
    {
        var fileStart = filePeriod.ContainingPeriodBoundaryStart(filePeriodTime);
        return new TimeSeriesFileParameters(fileInfo, instrument, filePeriod, fileStart, 7, FileFlags.WriterOpened);
    }

    protected virtual PriceTimeSeriesFileParameters CreatePriceQuoteTimeSeriesFileParameters
        (FileInfo fileInfo, IInstrument instrument, TimeBoundaryPeriod filePeriod, DateTime filePeriodTime)
    {
        if (filePeriod != TimeBoundaryPeriod.OneWeek) throw new Exception("Expected file period to be one week");
        var sourceTickerInfo = instrument as ISourceTickerInfo;
        if (sourceTickerInfo == null) throw new Exception("Expected instrument to be of ISourceTickerInfo");

        var timeSeriesFileParams = CreateTimeSeriesFileParameters(fileInfo, instrument, filePeriod, filePeriodTime);

        return new PriceTimeSeriesFileParameters(sourceTickerInfo, timeSeriesFileParams);
    }

    public override ITimeSeriesEntryFile<TEntry>? OpenExistingEntryFile(FileInfo fileInfo)
    {
        return EntryType switch
               {
                   _ when EntryType == typeof(IPublishableTickInstant) => (ITimeSeriesEntryFile<TEntry>)WeeklyTickInstantTimeSeriesFile
                       .OpenExistingTimeSeriesFile(fileInfo)
                 , _ when EntryType == typeof(IPublishableLevel1Quote) => (ITimeSeriesEntryFile<TEntry>)WeeklyLevel1QuoteTimeSeriesFile
                       .OpenExistingTimeSeriesFile(fileInfo)
                 , _ when EntryType == typeof(IPublishableLevel2Quote) => (ITimeSeriesEntryFile<TEntry>)WeeklyLevel2QuoteTimeSeriesFile
                       .OpenExistingTimeSeriesFile(fileInfo)
                 , _ when EntryType == typeof(IPublishableLevel3Quote) => (ITimeSeriesEntryFile<TEntry>)WeeklyLevel3QuoteTimeSeriesFile
                       .OpenExistingTimeSeriesFile(fileInfo)
                 , _ => throw new Exception("Expected entry type to be ILevel#Quote type")
               };
    }

    public override ITimeSeriesFile? OpenExisting(FileInfo fileInfo) => OpenExistingEntryFile(fileInfo);

    public override bool IsBestFactoryFor(IInstrument instrument)
    {
        var srcTickerInfo = instrument as ISourceTickerInfo;
        var category      = srcTickerInfo?.PublishedTickerQuoteDetailLevel.ToString() ?? instrument[nameof(RepositoryPathName.Category)];
        var entryType     = typeof(TEntry);
        return instrument.InstrumentType == InstrumentType.Price
            && category switch
               {
                   nameof(TickerQuoteDetailLevel.SingleValue) => !entryType.ImplementsInterface<IPublishableLevel1Quote>()
                 , nameof(TickerQuoteDetailLevel.Level1Quote) => !entryType.ImplementsInterface<IPublishableLevel2Quote>()
                 , nameof(TickerQuoteDetailLevel.Level2Quote) => !entryType.ImplementsInterface<IPublishableLevel3Quote>()
                 , _                                     => true
               };
    }

    public override ITimeSeriesEntryFile<TEntry> OpenOrCreate
        (FileInfo fileInfo, IInstrument instrument, TimeBoundaryPeriod filePeriod, DateTime filePeriodTime)
    {
        var priceQuoteFileParams = CreatePriceQuoteTimeSeriesFileParameters(fileInfo, instrument, filePeriod, filePeriodTime);
        return EntryType switch
               {
                   _ when EntryType == typeof(IPublishableTickInstant) =>
                       (ITimeSeriesEntryFile<TEntry>)new WeeklyTickInstantTimeSeriesFile(priceQuoteFileParams)
                 , _ when EntryType == typeof(IPublishableLevel1Quote) => (ITimeSeriesEntryFile<TEntry>)new WeeklyLevel1QuoteTimeSeriesFile(priceQuoteFileParams)
                 , _ when EntryType == typeof(IPublishableLevel2Quote) => (ITimeSeriesEntryFile<TEntry>)new WeeklyLevel2QuoteTimeSeriesFile(priceQuoteFileParams)
                 , _ when EntryType == typeof(IPublishableLevel3Quote) => (ITimeSeriesEntryFile<TEntry>)new WeeklyLevel3QuoteTimeSeriesFile(priceQuoteFileParams)
                 , _                                        => throw new Exception("Expected entry type to be ILevel#Quote type")
               };
    }
}
