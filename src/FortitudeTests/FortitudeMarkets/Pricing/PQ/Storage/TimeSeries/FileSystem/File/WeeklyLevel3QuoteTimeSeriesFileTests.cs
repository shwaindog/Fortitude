﻿// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.DataStructures.Memory.UnmanagedMemory.MemoryMappedFiles;
using FortitudeCommon.Extensions;
using FortitudeCommon.Monitoring.Logging;
using FortitudeIO.Storage.TimeSeries;
using FortitudeIO.Storage.TimeSeries.FileSystem.DirectoryStructure;
using FortitudeIO.Storage.TimeSeries.FileSystem.File;
using FortitudeIO.Storage.TimeSeries.FileSystem.File.Buckets;
using FortitudeIO.Storage.TimeSeries.FileSystem.Session;
using FortitudeIO.Storage.TimeSeries.FileSystem.Session.Retrieval;
using FortitudeMarkets.Config;
using FortitudeMarkets.Pricing.FeedEvents.Generators.Quotes;
using FortitudeMarkets.Pricing.FeedEvents.LastTraded;
using FortitudeMarkets.Pricing.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.Generators.Quotes;
using FortitudeMarkets.Pricing.Generators.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.PQ.Generators.Quotes;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.PQ.Storage.TimeSeries.FileSystem.File;
using FortitudeMarkets.Pricing.PQ.Storage.TimeSeries.FileSystem.File.Buckets;
using static FortitudeIO.Transports.Network.Config.CountryCityCodes;
using static FortitudeMarkets.Pricing.FeedEvents.TickerInfo.TickerQuoteDetailLevel;
using static FortitudeTests.FortitudeMarkets.Pricing.PQ.Storage.TimeSeries.FileSystem.File.TestWeeklyDataGeneratorFixture;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Storage.TimeSeries.FileSystem.File;

[TestClass]
public class WeeklyLevel3QuoteTimeSeriesFileTests
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(WeeklyLevel3QuoteTimeSeriesFileTests));

    private readonly Func<IPublishableLevel3Quote> asLevel3PriceQuoteFactory = () => new PublishableLevel3PriceQuote();
    private readonly Func<IPublishableLevel3Quote> asPQLevel3QuoteFactory    = () => new PQPublishableLevel3Quote();

    private WeeklyLevel3QuoteTimeSeriesFile level3OneWeekFile = null!;

    private PublishableLevel3QuoteGenerator level3QuoteGenerator = null!;

    private IReaderSession<IPublishableLevel3Quote>? level3SessionReader;
    private IWriterSession<IPublishableLevel3Quote>  level3SessionWriter = null!;

    private PQPublishableLevel3QuoteGenerator pqLevel3QuoteGenerator = null!;

    private DateTime         startOfWeek;
    private SourceTickerInfo level3SrcTkrInfo = null!;

    [TestInitialize]
    public void Setup()
    {
        PagedMemoryMappedFile.LogMappingMessages = true;

        var dateToGenerate   = DateTime.UtcNow.Date.TruncToMonthBoundary().AddDays(15);
        var currentDayOfWeek = dateToGenerate.DayOfWeek;
        var dayDiff          = DayOfWeek.Sunday - currentDayOfWeek;
        startOfWeek = dateToGenerate.AddDays(dayDiff);
    }

    private void CreateLevel3File
    (FileFlags fileFlags = FileFlags.WriterOpened | FileFlags.HasInternalIndexInHeader,
        LayerType layerType = LayerType.PriceVolume, byte numberOfLayers = 5, LastTradeType lastTradeType = LastTradeType.Price)
    {
        level3SrcTkrInfo =
            new SourceTickerInfo
                (19, "WeeklyLevel3QuoteTimeSeriesFileTests", 79, "PersistTest", Level3Quote, MarketClassification.Unknown
               , AUinMEL, AUinMEL, AUinMEL
               , numberOfLayers, layerFlags: layerType.SupportedLayerFlags(), lastTradedFlags: lastTradeType.SupportedLastTradedFlags()
               , roundingPrecision: 0.000001m, minSubmitSize: 0.01m, incrementSize: 0.01m);

        var generateQuoteInfo = new GenerateQuoteInfo(level3SrcTkrInfo)
        {
            MidPriceGenerator =
            {
                StartTime = startOfWeek, StartPrice = 1.332211m
            }
          , BookGenerationInfo = new BookGenerationInfo()
            {
                NumberOfBookLayers = 5, GenerateBookLayerInfo = new GenerateBookLayerInfo()
                {
                    AverageOrdersPerLayer = 5
                }
            }
        };

        level3QuoteGenerator   = new PublishableLevel3QuoteGenerator(new CurrentQuoteInstantValueGenerator(generateQuoteInfo));
        pqLevel3QuoteGenerator = new PQPublishableLevel3QuoteGenerator(new CurrentQuoteInstantValueGenerator(generateQuoteInfo));

        fileFlags |= FileFlags.WriterOpened | FileFlags.HasInternalIndexInHeader;

        var timeSeriesFile = GenerateUniqueFileNameOffDateTime();
        if (timeSeriesFile.Exists) timeSeriesFile.Delete();
        var instrumentFields = new Dictionary<string, string>
        {
            { nameof(RepositoryPathName.AssetType), "Unknown" }
          , { nameof(RepositoryPathName.MarketProductType), "Unknown" }
          , { nameof(RepositoryPathName.MarketRegion), "Unknown" }
        };
        var optionalInstrumentFields = new Dictionary<string, string>
        {
            { nameof(RepositoryPathName.Category), "TestInstrumentCategory" }
        };
        var createTestCreateFileParameters =
            new TimeSeriesFileParameters
                (timeSeriesFile
               , new Instrument("TestInstrumentName", "TestSourceName", InstrumentType.Price, new DiscreetTimePeriod(TimeBoundaryPeriod.Tick)
                              , instrumentFields
                              , optionalInstrumentFields),
                 TimeBoundaryPeriod.OneWeek, DateTime.UtcNow.Date, 7,
                 fileFlags, 6);
        var createPriceQuoteFile = new PriceTimeSeriesFileParameters(level3SrcTkrInfo, createTestCreateFileParameters);
        level3OneWeekFile   = new WeeklyLevel3QuoteTimeSeriesFile(createPriceQuoteFile);
        level3SessionWriter = level3OneWeekFile.GetWriterSession()!;
    }

    [TestCleanup]
    public void TearDown()
    {
        try
        {
            level3SessionReader?.Close();
            level3SessionWriter.Close();
            level3OneWeekFile.Close();
        }
        catch (Exception ex)
        {
            Console.Out.WriteLine("Could not close all sessions. Got {0}", ex);
        }
        DeleteTestFiles();
    }

    [TestMethod]
    public void CreateNewPriceLastTradeQuote_TwoSmallPeriods_OriginalValuesAreReturned()
    {
        CreateLevel3File(layerType: LayerType.PriceVolume, lastTradeType: LastTradeType.Price);
        CreateNewTyped_TwoSmallPeriods_OriginalValuesAreReturned(level3QuoteGenerator, asLevel3PriceQuoteFactory);
    }

    [TestMethod]
    public void CreateNewPQPriceLastTradeQuote_TwoSmallPeriods_OriginalValuesAreReturned()
    {
        CreateLevel3File(layerType: LayerType.PriceVolume, lastTradeType: LastTradeType.Price);
        CreateNewTyped_TwoSmallPeriods_OriginalValuesAreReturned(pqLevel3QuoteGenerator, asPQLevel3QuoteFactory);
    }

    [TestMethod]
    public void CreateNewPaidGivenLastTradeQuote_TwoSmallCompressedPeriods_OriginalValuesAreReturned()
    {
        CreateLevel3File(FileFlags.WriteDataCompressed, lastTradeType: LastTradeType.PricePaidOrGivenVolume);
        CreateNewTyped_TwoSmallPeriods_OriginalValuesAreReturned(level3QuoteGenerator, asLevel3PriceQuoteFactory);
    }

    [TestMethod]
    public void CreateNewPQPaidGivenLastTradeQuote_TwoSmallCompressedPeriods_OriginalValuesAreReturned()
    {
        CreateLevel3File(FileFlags.WriteDataCompressed, lastTradeType: LastTradeType.PricePaidOrGivenVolume);
        CreateNewTyped_TwoSmallPeriods_OriginalValuesAreReturned(pqLevel3QuoteGenerator, asPQLevel3QuoteFactory);
    }

    [TestMethod]
    public void CreateNewTraderPaidGivenLastTradeQuote_TwoSmallPeriods_OriginalValuesAreReturned()
    {
        CreateLevel3File(layerType: LayerType.SourcePriceVolume, lastTradeType: LastTradeType.PriceLastTraderPaidOrGivenVolume);
        CreateNewTyped_TwoSmallPeriods_OriginalValuesAreReturned(level3QuoteGenerator, asLevel3PriceQuoteFactory);
    }

    [TestMethod]
    public void CreateNewPQTraderPaidGivenLastTradeQuote_TwoSmallPeriods_OriginalValuesAreReturned()
    {
        CreateLevel3File(layerType: LayerType.SourcePriceVolume, lastTradeType: LastTradeType.PriceLastTraderPaidOrGivenVolume);
        CreateNewTyped_TwoSmallPeriods_OriginalValuesAreReturned(pqLevel3QuoteGenerator, asPQLevel3QuoteFactory);
    }

    public void CreateNewTyped_TwoSmallPeriods_OriginalValuesAreReturned<TEntry>
        (ITickGenerator<TEntry> tickGenerator, Func<IPublishableLevel3Quote> retrievalFactory)
        where TEntry : class, IMutablePublishableLevel3Quote, IPublishableLevel3Quote
    {
        var toPersistAndCheck
            = GenerateRepeatableQuotes<IPublishableLevel3Quote, TEntry>
                (1, 10, 1, DayOfWeek.Wednesday, tickGenerator).ToList();
        toPersistAndCheck.AddRange
            (GenerateRepeatableQuotes<IPublishableLevel3Quote, TEntry>
                (1, 10, 1, DayOfWeek.Thursday, tickGenerator));

        foreach (var firstPeriod in toPersistAndCheck)
        {
            var result = level3SessionWriter.AppendEntry(firstPeriod);
            Assert.AreEqual(StorageAttemptResult.PeriodRangeMatched, result.StorageAttemptResult);
        }
        level3OneWeekFile.AutoCloseOnZeroSessions = false;
        level3SessionWriter.Close();

        Assert.AreEqual((uint)toPersistAndCheck.Count, level3OneWeekFile.Header.TotalEntries);
        level3SessionReader = level3OneWeekFile.GetReaderSession();
        var allEntriesReader = level3SessionReader.AllChronologicalEntriesReader
            (new Recycler(), EntryResultSourcing.FromFactoryFuncUnlimited, ReaderOptions.ReadFastAsPossible, retrievalFactory);
        var storedItems = allEntriesReader.ResultEnumerable.ToList();
        Assert.AreEqual(toPersistAndCheck.Count, allEntriesReader.CountMatch);
        Assert.AreEqual(allEntriesReader.CountMatch, allEntriesReader.CountProcessed);
        Assert.AreEqual(toPersistAndCheck.Count, storedItems.Count);
        CompareExpectedToExtracted(toPersistAndCheck, storedItems);
    }

    //[TestMethod]
    public void LongRunningCreateNewPriceQuote_TwoLargeCompressedPeriods_OriginalValuesAreReturned()
    {
        CreateLevel3File(FileFlags.WriteDataCompressed, LayerType.PriceVolume, 2, LastTradeType.PriceLastTraderPaidOrGivenVolume);
        CreateNewTyped_TwoLargeCompressedPeriods_OriginalValuesAreReturned(level3QuoteGenerator, asLevel3PriceQuoteFactory);
    }

    //[TestMethod]
    public void LongRunningCreateNewPQPriceQuote_TwoLargeCompressedPeriods_OriginalValuesAreReturned()
    {
        CreateLevel3File(FileFlags.WriteDataCompressed, LayerType.PriceVolume, 2, LastTradeType.PriceLastTraderPaidOrGivenVolume);
        CreateNewTyped_TwoLargeCompressedPeriods_OriginalValuesAreReturned(pqLevel3QuoteGenerator, asPQLevel3QuoteFactory);
    }

    public void CreateNewTyped_TwoLargeCompressedPeriods_OriginalValuesAreReturned<TEntry>
        (ITickGenerator<TEntry> tickGenerator, Func<IPublishableLevel3Quote> retrievalFactory)
        where TEntry : class, IMutablePublishableLevel3Quote, IPublishableLevel3Quote
    {
        var toPersistAndCheck
            = GenerateRepeatableQuotes<IPublishableLevel3Quote, TEntry>
                (1, 8000, 1, DayOfWeek.Wednesday, tickGenerator).ToList();
        toPersistAndCheck.AddRange
            (GenerateRepeatableQuotes<IPublishableLevel3Quote, TEntry>
                (1, 8000, 1, DayOfWeek.Thursday, tickGenerator));

        foreach (var firstPeriod in toPersistAndCheck)
        {
            var result = level3SessionWriter.AppendEntry(firstPeriod);
            Assert.AreEqual(StorageAttemptResult.PeriodRangeMatched, result.StorageAttemptResult);
        }
        level3OneWeekFile.AutoCloseOnZeroSessions = false;
        level3SessionWriter.Close();

        Assert.AreEqual((uint)toPersistAndCheck.Count, level3OneWeekFile.Header.TotalEntries);
        level3SessionReader = level3OneWeekFile.GetReaderSession();
        var allEntriesReader = level3SessionReader.AllChronologicalEntriesReader
            (new Recycler(), EntryResultSourcing.FromFactoryFuncUnlimited, ReaderOptions.ReadFastAsPossible, retrievalFactory);
        var storedItems = allEntriesReader.ResultEnumerable.ToList();
        Assert.AreEqual(toPersistAndCheck.Count, allEntriesReader.CountMatch);
        Assert.AreEqual(allEntriesReader.CountMatch, allEntriesReader.CountProcessed);
        Assert.AreEqual(toPersistAndCheck.Count, storedItems.Count);
        CompareExpectedToExtracted(toPersistAndCheck, storedItems);
    }

    //[TestMethod]
    public void LongRunningCreateNewPriceQuoteCompressedDataFile_SavesEntriesCloseAndReopen_OriginalValuesAreReturned()
    {
        CreateLevel3File(FileFlags.WriteDataCompressed);
        NewFile_SavesEntriesCloseAndReopen_OriginalValuesAreReturned(level3QuoteGenerator, asLevel3PriceQuoteFactory);
    }

    [TestMethod]
    public void CreateNewPriceQuoteFile_SavesEntriesCloseAndReopen_OriginalValuesAreReturned()
    {
        CreateLevel3File(layerType: LayerType.OrdersAnonymousPriceVolume, numberOfLayers: 5, lastTradeType: LastTradeType.PricePaidOrGivenVolume);
        NewFile_SavesEntriesCloseAndReopen_ReadReverseOriginalValuesAreReturned(level3QuoteGenerator);
    }

    //[TestMethod]
    public void LongRunningCreateNewPQQuoteCompressedDataFile_SavesEntriesCloseAndReopen_OriginalValuesAreReturned()
    {
        CreateLevel3File(FileFlags.WriteDataCompressed, LayerType.SourcePriceVolume, 2, LastTradeType.PriceLastTraderName);
        NewFile_SavesEntriesCloseAndReopen_OriginalValuesAreReturned(pqLevel3QuoteGenerator, asPQLevel3QuoteFactory);
    }

    [TestMethod]
    public void CreateNewPQQuoteFile_SavesEntriesCloseAndReopen_OriginalValuesAreReturned()
    {
        CreateLevel3File(layerType: LayerType.SourceQuoteRefPriceVolume, numberOfLayers: 10);
        NewFile_SavesEntriesCloseAndReopen_OriginalValuesAreReturned(pqLevel3QuoteGenerator, asPQLevel3QuoteFactory);
    }

    public void NewFile_SavesEntriesCloseAndReopen_OriginalValuesAreReturned<TEntry>
        (ITickGenerator<TEntry> tickGenerator, Func<IPublishableLevel3Quote> retrievalFactory)
        where TEntry : class, IMutablePublishableLevel3Quote, IPublishableLevel3Quote
    {
        var toPersistAndCheck =
            GenerateQuotesForEachDayAndHourOfCurrentWeek<IPublishableLevel3Quote, TEntry>
                (0, 10, tickGenerator).ToList();

        foreach (var level3QuoteStruct in toPersistAndCheck)
        {
            var result = level3SessionWriter.AppendEntry(level3QuoteStruct);
            Assert.AreEqual(StorageAttemptResult.PeriodRangeMatched, result.StorageAttemptResult);
        }
        level3OneWeekFile.AutoCloseOnZeroSessions = false;
        level3SessionWriter.Close();
        Assert.AreEqual((uint)toPersistAndCheck.Count, level3OneWeekFile.Header.TotalEntries);

        level3SessionReader = level3OneWeekFile.GetReaderSession();
        var allEntriesReader = level3SessionReader.AllChronologicalEntriesReader
            (new Recycler(), EntryResultSourcing.FromFactoryFuncUnlimited, ReaderOptions.ReadFastAsPossible, retrievalFactory);
        var storedItems = allEntriesReader.ResultEnumerable.ToList();
        Assert.AreEqual(toPersistAndCheck.Count, allEntriesReader.CountMatch);
        Assert.AreEqual(allEntriesReader.CountMatch, allEntriesReader.CountProcessed);
        Assert.AreEqual(toPersistAndCheck.Count, storedItems.Count);
        CompareExpectedToExtracted(toPersistAndCheck, storedItems);
        var newReaderSession = level3OneWeekFile.GetReaderSession();
        Assert.AreNotSame(level3SessionReader, newReaderSession);
        var newEntriesReader = level3SessionReader.AllChronologicalEntriesReader
            (new Recycler(), EntryResultSourcing.FromFactoryFuncUnlimited, ReaderOptions.ReadFastAsPossible, retrievalFactory);
        newEntriesReader.ResultPublishFlags = ResultFlags.CopyToList;
        newEntriesReader.RunReader();
        var listResults = newEntriesReader.ResultList;
        Assert.AreEqual(toPersistAndCheck.Count, newEntriesReader.CountMatch);
        Assert.AreEqual(allEntriesReader.CountMatch, newEntriesReader.CountProcessed);
        Assert.AreEqual(toPersistAndCheck.Count, listResults.Count);
        CompareExpectedToExtracted(toPersistAndCheck, storedItems);
        newReaderSession.Close();
    }

    public void NewFile_SavesEntriesCloseAndReopen_ReadReverseOriginalValuesAreReturned<TEntry>(ITickGenerator<TEntry> tickGenerator)
        where TEntry : class, IMutablePublishableLevel3Quote, IPublishableLevel3Quote
    {
        var toPersistAndCheck =
            GenerateQuotesForEachDayAndHourOfCurrentWeek<IPublishableLevel3Quote, TEntry>
                (0, 10, tickGenerator).ToList();

        foreach (var level3QuoteStruct in toPersistAndCheck)
        {
            var result = level3SessionWriter.AppendEntry(level3QuoteStruct);
            Assert.AreEqual(StorageAttemptResult.PeriodRangeMatched, result.StorageAttemptResult);
        }
        level3OneWeekFile.AutoCloseOnZeroSessions = false;
        level3SessionWriter.Close();
        Assert.AreEqual((uint)toPersistAndCheck.Count, level3OneWeekFile.Header.TotalEntries);

        level3SessionReader = level3OneWeekFile.GetReaderSession();
        var allEntriesReader = level3SessionReader.AllReverseChronologicalEntriesReader<PublishableLevel3PriceQuote>(new Recycler());
        var storedItems      = allEntriesReader.ResultEnumerable.ToList();
        Assert.AreEqual(toPersistAndCheck.Count, allEntriesReader.CountMatch);
        Assert.AreEqual(allEntriesReader.CountMatch, allEntriesReader.CountProcessed);
        Assert.AreEqual(toPersistAndCheck.Count, storedItems.Count);
        toPersistAndCheck.Reverse();
        CompareExpectedToExtracted(toPersistAndCheck, storedItems);
        var newReaderSession = level3OneWeekFile.GetReaderSession();
        Assert.AreNotSame(level3SessionReader, newReaderSession);
        var newEntriesReader = level3SessionReader.AllReverseChronologicalEntriesReader<PublishableLevel3PriceQuote>(new Recycler());
        newEntriesReader.ResultPublishFlags = ResultFlags.CopyToList;
        newEntriesReader.RunReader();
        var listResults = newEntriesReader.ResultList;
        Assert.AreEqual(toPersistAndCheck.Count, newEntriesReader.CountMatch);
        Assert.AreEqual(allEntriesReader.CountMatch, newEntriesReader.CountProcessed);
        Assert.AreEqual(toPersistAndCheck.Count, listResults.Count);
        CompareExpectedToExtracted(toPersistAndCheck, storedItems);
        newReaderSession.Close();
    }

    private void CompareExpectedToExtracted(List<IPublishableLevel3Quote> originalList, List<IPublishableLevel3Quote> toCompareList)
    {
        for (var i = 0; i < originalList.Count; i++)
        {
            var originalEntry = originalList[i];
            var compareEntry  = toCompareList[i];
            if (!originalEntry.AreEquivalent(compareEntry))
            {
                Logger.Warn("Entries at {0} differ test failed \ndiff {1}", i, originalEntry.DiffQuotes(compareEntry));
                FLoggerFactory.WaitUntilDrained();
                Assert.Fail($"Entries at {i} differ test failed \ndiff {originalEntry.DiffQuotes(compareEntry)}.");
            }
        }
    }

    [TestMethod]
    public void CreateNewFile_SetLargeStringsWhichAreTruncated_CloseReopenSafelyReturnsTruncatedStrings()
    {
        CreateLevel3File();
        var largeString = 999.CharIndexPosListedSizeString();
        var truncated   = largeString.Substring(0, 254); // byte.MaxValue -1 (largest storable value)  (-1 null terminator)
        var header      = level3OneWeekFile.Header;
        header.AnnotationFileRelativePath = largeString;
        Assert.AreEqual(truncated, header.AnnotationFileRelativePath);
        header.ExternalIndexFileRelativePath = largeString;
        Assert.AreEqual(truncated, header.ExternalIndexFileRelativePath);
        header.OriginSourceText = largeString;
        Assert.AreEqual(truncated, header.OriginSourceText);
        header.SourceName = largeString;
        Assert.AreEqual(truncated, header.SourceName);
        header.Category = largeString;
        Assert.AreEqual(truncated, header.Category);
        header.InstrumentName = largeString;
        Assert.AreEqual(truncated, header.InstrumentName);
        Assert.AreEqual(TimeBoundaryPeriod.OneWeek, header.FilePeriod);
        Assert.AreEqual(TimeBoundaryPeriod.OneWeek.ContainingPeriodBoundaryStart(DateTime.UtcNow.Date), header.FileStartPeriod);
        Assert.AreEqual(InstrumentType.Price, header.InstrumentType);
        Assert.AreEqual(typeof(DailyToHourlyLevel3QuoteSubBuckets<IPublishableLevel3Quote>), header.BucketType);
        Assert.AreEqual(typeof(IPublishableLevel3Quote), header.EntryType);
        Assert.AreEqual(typeof(WeeklyLevel3QuoteTimeSeriesFile), header.TimeSeriesFileType);
        level3OneWeekFile.Close();
        level3OneWeekFile = WeeklyLevel3QuoteTimeSeriesFile
            .OpenExistingTimeSeriesFile(level3OneWeekFile.FileName);
        header = level3OneWeekFile.Header;
        Assert.AreEqual(truncated, header.AnnotationFileRelativePath);
        Assert.AreEqual(truncated, header.ExternalIndexFileRelativePath);
        Assert.AreEqual(truncated, header.OriginSourceText);
        Assert.AreEqual(truncated, header.SourceName);
        Assert.AreEqual(truncated, header.Category);
        Assert.AreEqual(truncated, header.InstrumentName);
        Assert.AreEqual(TimeBoundaryPeriod.OneWeek, header.FilePeriod);
        Assert.AreEqual(TimeBoundaryPeriod.OneWeek.ContainingPeriodBoundaryStart(DateTime.UtcNow.Date), header.FileStartPeriod);
        Assert.AreEqual(InstrumentType.Price, header.InstrumentType);
        Assert.AreEqual(typeof(DailyToHourlyLevel3QuoteSubBuckets<IPublishableLevel3Quote>), header.BucketType);
        Assert.AreEqual(typeof(IPublishableLevel3Quote), header.EntryType);
        Assert.AreEqual(typeof(WeeklyLevel3QuoteTimeSeriesFile), header.TimeSeriesFileType);
    }

    [TestMethod]
    public void CreateNewFile_BeyondFileTime_ReturnsFileRangeNotSupported()
    {
        CreateLevel3File();
        Assert.AreEqual(InstrumentType.Price, level3OneWeekFile.InstrumentType);
        var singleQuoteMiddleOfWeek
            = GenerateRepeatableQuotes<IPublishableLevel3Quote, PublishableLevel3PriceQuote>
                (1, 1, 12, DayOfWeek.Wednesday, level3QuoteGenerator);
        var nextWeekQuote = (IMutablePublishableLevel3Quote)singleQuoteMiddleOfWeek.First();
        nextWeekQuote.SourceTime = nextWeekQuote.SourceTime.AddDays(7);
        var result = level3SessionWriter.AppendEntry(nextWeekQuote);
        Assert.AreEqual(StorageAttemptResult.NextFilePeriod, result.StorageAttemptResult);
    }

    [TestMethod]
    public void CreateNewFile_AfterBucketClosesOnNextEntry_TryingToAddExistingEntryReturnsBucketClosedForAppend()
    {
        CreateLevel3File();
        var wednesdayQuotes =
            GenerateRepeatableQuotes<IPublishableLevel3Quote, PublishableLevel3PriceQuote>
                (1, 1, 12, DayOfWeek.Wednesday, level3QuoteGenerator);
        var thursdayQuotes =
            GenerateRepeatableQuotes<IPublishableLevel3Quote, PublishableLevel3PriceQuote>
                (1, 1, 12, DayOfWeek.Thursday, level3QuoteGenerator);
        var wednesdayQuote = wednesdayQuotes.First();
        var thursdayQuote  = thursdayQuotes.First();
        var result         = level3SessionWriter.AppendEntry(wednesdayQuote);
        Assert.AreEqual(StorageAttemptResult.PeriodRangeMatched, result.StorageAttemptResult);
        result = level3SessionWriter.AppendEntry(thursdayQuote);
        Assert.AreEqual(StorageAttemptResult.PeriodRangeMatched, result.StorageAttemptResult);
        result = level3SessionWriter.AppendEntry(wednesdayQuote);
        Assert.AreEqual(StorageAttemptResult.BucketClosedForAppend, result.StorageAttemptResult);
    }
}
