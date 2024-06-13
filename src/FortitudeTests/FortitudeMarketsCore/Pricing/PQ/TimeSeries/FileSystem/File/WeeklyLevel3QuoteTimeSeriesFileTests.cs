// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory.UnmanagedMemory.MemoryMappedFiles;
using FortitudeCommon.Extensions;
using FortitudeCommon.Monitoring.Logging;
using FortitudeIO.TimeSeries;
using FortitudeIO.TimeSeries.FileSystem.File;
using FortitudeIO.TimeSeries.FileSystem.File.Buckets;
using FortitudeIO.TimeSeries.FileSystem.Session;
using FortitudeIO.TimeSeries.FileSystem.Session.Retrieval;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing;
using FortitudeMarketsApi.Pricing.LastTraded;
using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.Generators;
using FortitudeMarketsCore.Pricing.PQ.TimeSeries.FileSystem.File;
using FortitudeMarketsCore.Pricing.PQ.TimeSeries.FileSystem.File.Buckets;
using FortitudeMarketsCore.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.Quotes.Generators;
using static FortitudeTests.FortitudeMarketsCore.Pricing.PQ.TimeSeries.FileSystem.File.TestWeeklyDataGeneratorFixture;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.TimeSeries.FileSystem.File;

[TestClass]
public class WeeklyLevel3QuoteTimeSeriesFileTests
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(WeeklyLevel3QuoteTimeSeriesFileTests));

    private readonly Func<ILevel3Quote> asLevel3PriceQuoteFactory = () => new Level3PriceQuote();
    private readonly Func<ILevel3Quote> asPQLevel3QuoteFactory    = () => new PQLevel3Quote();

    private WeeklyLevel3QuoteTimeSeriesFile level3OneWeekFile = null!;

    private Level3QuoteGenerator level3QuoteGenerator = null!;

    private IReaderSession<ILevel3Quote>? level3SessionReader;
    private IWriterSession<ILevel3Quote>  level3SessionWriter = null!;

    private SourceTickerQuoteInfo  level3SrcTkrQtInfo     = null!;
    private PQLevel3QuoteGenerator pqLevel3QuoteGenerator = null!;
    private DateTime               startOfWeek;

    [TestInitialize]
    public void Setup()
    {
        PagedMemoryMappedFile.LogMappingMessages = true;

        var dateToGenerate   = DateTime.UtcNow.Date;
        var currentDayOfWeek = dateToGenerate.DayOfWeek;
        var dayDiff          = DayOfWeek.Sunday - currentDayOfWeek;
        startOfWeek = dateToGenerate.AddDays(dayDiff);
    }

    private void CreateLevel3File(FileFlags fileFlags = FileFlags.WriterOpened | FileFlags.HasInternalIndexInHeader,
        LayerType layerType = LayerType.PriceVolume, byte numberOfLayers = 20, LastTradeType lastTradeType = LastTradeType.Price)
    {
        level3SrcTkrQtInfo =
            new SourceTickerQuoteInfo
                (19, "WeeklyLevel3QuoteTimeSeriesFileTests", 79, "PersistTest",
                 QuoteLevel.Level3, numberOfLayers, layerFlags: layerType.SupportedLayerFlags(),
                 lastTradedFlags: lastTradeType.SupportedLastTradedFlags());

        var generateQuoteInfo = new GenerateQuoteInfo(level3SrcTkrQtInfo);
        generateQuoteInfo.MidPriceGenerator!.StartTime  = startOfWeek;
        generateQuoteInfo.MidPriceGenerator!.StartPrice = 1.332211m;

        level3QuoteGenerator   = new Level3QuoteGenerator(generateQuoteInfo);
        pqLevel3QuoteGenerator = new PQLevel3QuoteGenerator(generateQuoteInfo);

        fileFlags |= FileFlags.WriterOpened | FileFlags.HasInternalIndexInHeader;

        var testTimeSeriesFilePath = Path.Combine(Environment.CurrentDirectory, GenerateUniqueFileNameOffDateTime());
        var timeSeriesFile         = new FileInfo(testTimeSeriesFilePath);
        if (timeSeriesFile.Exists) timeSeriesFile.Delete();
        var createTestCreateFileParameters =
            new TimeSeriesFileParameters
                (timeSeriesFile
               , new Instrument("TestInstrumentName", "TestSourceName", InstrumentType.Price, TimeSeriesPeriod.Tick, "TestInstrumentCategory"),
                 TimeSeriesPeriod.OneWeek, DateTime.UtcNow.Date, 7,
                 fileFlags, 6);
        var createPriceQuoteFile = new PriceQuoteTimeSeriesFileParameters(level3SrcTkrQtInfo, createTestCreateFileParameters);
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
        var dirInfo = new DirectoryInfo(Environment.CurrentDirectory);
        foreach (var existingTimeSeriesFile in dirInfo.GetFiles("TimeSeriesFileTests_*"))
            try
            {
                existingTimeSeriesFile.Delete();
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine("Could not delete file {0}. Got {1}", existingTimeSeriesFile, ex);
                FLoggerFactory.WaitUntilDrained();
            }
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
        CreateLevel3File(FileFlags.WriteDataCompressed, LayerType.PriceVolume, lastTradeType: LastTradeType.PricePaidOrGivenVolume);
        CreateNewTyped_TwoSmallPeriods_OriginalValuesAreReturned(level3QuoteGenerator, asLevel3PriceQuoteFactory);
    }

    [TestMethod]
    public void CreateNewPQPaidGivenLastTradeQuote_TwoSmallCompressedPeriods_OriginalValuesAreReturned()
    {
        CreateLevel3File(FileFlags.WriteDataCompressed, LayerType.PriceVolume, lastTradeType: LastTradeType.PricePaidOrGivenVolume);
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

    public void CreateNewTyped_TwoSmallPeriods_OriginalValuesAreReturned<TEntry>(
        IQuoteGenerator<TEntry> quoteGenerator, Func<ILevel3Quote> retrievalFactory)
        where TEntry : class, IMutableLevel3Quote, ILevel3Quote
    {
        var toPersistAndCheck
            = GenerateRepeatableL1QuoteStructs<ILevel3Quote, TEntry>
                (1, 10, 1, DayOfWeek.Wednesday, quoteGenerator).ToList();
        toPersistAndCheck.AddRange
            (GenerateRepeatableL1QuoteStructs<ILevel3Quote, TEntry>
                (1, 10, 1, DayOfWeek.Thursday, quoteGenerator));

        foreach (var firstPeriod in toPersistAndCheck)
        {
            var result = level3SessionWriter.AppendEntry(firstPeriod);
            Assert.AreEqual(StorageAttemptResult.PeriodRangeMatched, result.StorageAttemptResult);
        }
        level3OneWeekFile.AutoCloseOnZeroSessions = false;
        level3SessionWriter.Close();

        Assert.AreEqual((uint)toPersistAndCheck.Count, level3OneWeekFile.Header.TotalEntries);
        level3SessionReader = level3OneWeekFile.GetReaderSession();
        var allEntriesReader = level3SessionReader.GetAllEntriesReader(EntryResultSourcing.FromFactoryFuncUnlimited, retrievalFactory);
        var storedItems      = allEntriesReader.ResultEnumerable.ToList();
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

    public void CreateNewTyped_TwoLargeCompressedPeriods_OriginalValuesAreReturned<TEntry>(
        IQuoteGenerator<TEntry> quoteGenerator, Func<ILevel3Quote> retrievalFactory)
        where TEntry : class, IMutableLevel3Quote, ILevel3Quote
    {
        var toPersistAndCheck
            = GenerateRepeatableL1QuoteStructs<ILevel3Quote, TEntry>
                (1, 8000, 1, DayOfWeek.Wednesday, quoteGenerator).ToList();
        toPersistAndCheck.AddRange
            (GenerateRepeatableL1QuoteStructs<ILevel3Quote, TEntry>
                (1, 8000, 1, DayOfWeek.Thursday, quoteGenerator));

        foreach (var firstPeriod in toPersistAndCheck)
        {
            var result = level3SessionWriter.AppendEntry(firstPeriod);
            Assert.AreEqual(StorageAttemptResult.PeriodRangeMatched, result.StorageAttemptResult);
        }
        level3OneWeekFile.AutoCloseOnZeroSessions = false;
        level3SessionWriter.Close();

        Assert.AreEqual((uint)toPersistAndCheck.Count, level3OneWeekFile.Header.TotalEntries);
        level3SessionReader = level3OneWeekFile.GetReaderSession();
        var allEntriesReader = level3SessionReader.GetAllEntriesReader(EntryResultSourcing.FromFactoryFuncUnlimited, retrievalFactory);
        var storedItems      = allEntriesReader.ResultEnumerable.ToList();
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
        CreateLevel3File(layerType: LayerType.TraderPriceVolume, numberOfLayers: 5, lastTradeType: LastTradeType.PricePaidOrGivenVolume);
        NewFile_SavesEntriesCloseAndReopen_OriginalValuesAreReturned(level3QuoteGenerator, asLevel3PriceQuoteFactory);
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

    public void NewFile_SavesEntriesCloseAndReopen_OriginalValuesAreReturned<TEntry>(
        IQuoteGenerator<TEntry> quoteGenerator, Func<ILevel3Quote> retrievalFactory)
        where TEntry : class, IMutableLevel3Quote, ILevel3Quote
    {
        var toPersistAndCheck =
            GenerateForEachDayAndHourOfCurrentWeek<ILevel3Quote, TEntry>
                (0, 10, quoteGenerator).ToList();

        foreach (var Level3QuoteStruct in toPersistAndCheck)
        {
            var result = level3SessionWriter.AppendEntry(Level3QuoteStruct);
            Assert.AreEqual(StorageAttemptResult.PeriodRangeMatched, result.StorageAttemptResult);
        }
        level3OneWeekFile.AutoCloseOnZeroSessions = false;
        level3SessionWriter.Close();
        Assert.AreEqual((uint)toPersistAndCheck.Count, level3OneWeekFile.Header.TotalEntries);

        level3SessionReader = level3OneWeekFile.GetReaderSession();
        var allEntriesReader = level3SessionReader.GetAllEntriesReader(EntryResultSourcing.FromFactoryFuncUnlimited, retrievalFactory);
        var storedItems      = allEntriesReader.ResultEnumerable.ToList();
        Assert.AreEqual(toPersistAndCheck.Count, allEntriesReader.CountMatch);
        Assert.AreEqual(allEntriesReader.CountMatch, allEntriesReader.CountProcessed);
        Assert.AreEqual(toPersistAndCheck.Count, storedItems.Count);
        CompareExpectedToExtracted(toPersistAndCheck, storedItems);
        var newReaderSession = level3OneWeekFile.GetReaderSession();
        Assert.AreNotSame(level3SessionReader, newReaderSession);
        var newEntriesReader = level3SessionReader.GetAllEntriesReader(EntryResultSourcing.FromFactoryFuncUnlimited, retrievalFactory);
        newEntriesReader.ResultPublishFlags = ResultFlags.CopyToList;
        newEntriesReader.RunReader();
        var listResults = newEntriesReader.ResultList;
        Assert.AreEqual(toPersistAndCheck.Count, newEntriesReader.CountMatch);
        Assert.AreEqual(allEntriesReader.CountMatch, newEntriesReader.CountProcessed);
        Assert.AreEqual(toPersistAndCheck.Count, listResults.Count);
        CompareExpectedToExtracted(toPersistAndCheck, storedItems);
        newReaderSession.Close();
    }

    private void CompareExpectedToExtracted(List<ILevel3Quote> originalList, List<ILevel3Quote> toCompareList)
    {
        for (var i = 0; i < originalList.Count; i++)
        {
            var originalEntry = originalList[i];
            var compareEntry  = toCompareList[i];
            if (!originalEntry.AreEquivalent(compareEntry))
            {
                Logger.Warn("Entries at {0} differ test failed \ndiff {1}", i, originalEntry.DiffQuotes(compareEntry, false));
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
        Assert.AreEqual(TimeSeriesPeriod.OneWeek, header.FilePeriod);
        Assert.AreEqual(TimeSeriesPeriod.OneWeek.ContainingPeriodBoundaryStart(DateTime.UtcNow.Date), header.FileStartPeriod);
        Assert.AreEqual(InstrumentType.Price, header.InstrumentType);
        Assert.AreEqual(typeof(DailyToOneHourPQLevel3QuoteSubBuckets<ILevel3Quote>), header.BucketType);
        Assert.AreEqual(typeof(ILevel3Quote), header.EntryType);
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
        Assert.AreEqual(TimeSeriesPeriod.OneWeek, header.FilePeriod);
        Assert.AreEqual(TimeSeriesPeriod.OneWeek.ContainingPeriodBoundaryStart(DateTime.UtcNow.Date), header.FileStartPeriod);
        Assert.AreEqual(InstrumentType.Price, header.InstrumentType);
        Assert.AreEqual(typeof(DailyToOneHourPQLevel3QuoteSubBuckets<ILevel3Quote>), header.BucketType);
        Assert.AreEqual(typeof(ILevel3Quote), header.EntryType);
        Assert.AreEqual(typeof(WeeklyLevel3QuoteTimeSeriesFile), header.TimeSeriesFileType);
    }

    [TestMethod]
    public void CreateNewFile_BeyondFileTime_ReturnsFileRangeNotSupported()
    {
        CreateLevel3File();
        Assert.AreEqual(InstrumentType.Price, level3OneWeekFile.InstrumentType);
        var singleQuoteMiddleOfWeek
            = GenerateRepeatableL1QuoteStructs<ILevel3Quote, Level3PriceQuote>
                (1, 1, 12, DayOfWeek.Wednesday, level3QuoteGenerator);
        var nextWeekQuote = (IMutableLevel3Quote)singleQuoteMiddleOfWeek.First();
        nextWeekQuote.SourceTime = nextWeekQuote.SourceTime.AddDays(7);
        var result = level3SessionWriter.AppendEntry(nextWeekQuote);
        Assert.AreEqual(StorageAttemptResult.NextFilePeriod, result.StorageAttemptResult);
    }

    [TestMethod]
    public void CreateNewFile_AfterBucketClosesOnNextEntry_TryingToAddExistingEntryReturnsBucketClosedForAppend()
    {
        CreateLevel3File();
        var wednesdayQuotes =
            GenerateRepeatableL1QuoteStructs<ILevel3Quote, Level3PriceQuote>
                (1, 1, 12, DayOfWeek.Wednesday, level3QuoteGenerator);
        var thursdayQuotes =
            GenerateRepeatableL1QuoteStructs<ILevel3Quote, Level3PriceQuote>
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
