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
using FortitudeMarketsApi.Pricing.LastTraded;
using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.Generators.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Generators.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;
using FortitudeMarketsCore.Pricing.PQ.TimeSeries.FileSystem.File;
using FortitudeMarketsCore.Pricing.PQ.TimeSeries.FileSystem.File.Buckets;
using FortitudeMarketsCore.Pricing.Quotes;
using static FortitudeIO.TimeSeries.MarketClassificationExtensions;
using static FortitudeMarketsApi.Pricing.Quotes.QuoteLevel;
using static FortitudeTests.FortitudeMarketsCore.Pricing.PQ.TimeSeries.FileSystem.File.TestWeeklyDataGeneratorFixture;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.TimeSeries.FileSystem.File;

[TestClass]
public class WeeklyLevel0QuoteTimeSeriesFileTests
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(WeeklyLevel0QuoteTimeSeriesFileTests));

    private readonly Func<ILevel0Quote> asLevel0PriceQuoteFactory = () => new Level0PriceQuote();
    private readonly Func<ILevel0Quote> asPQLevel0QuoteFactory    = () => new PQLevel0Quote();

    private WeeklyLevel0QuoteTimeSeriesFile level0OneWeekFile = null!;

    private Level0QuoteGenerator level0QuoteGenerator = null!;

    private IReaderSession<ILevel0Quote>? level0SessionReader;
    private IWriterSession<ILevel0Quote>  level0SessionWriter = null!;

    private SourceTickerQuoteInfo  level0SrcTkrQtInfo     = null!;
    private PQLevel0QuoteGenerator pqLevel0QuoteGenerator = null!;

    [TestInitialize]
    public void Setup()
    {
        PagedMemoryMappedFile.LogMappingMessages = true;
        level0SrcTkrQtInfo =
            new SourceTickerQuoteInfo
                (19, "WeeklyLevel0QuoteTimeSeriesFileTests", 79, "PersistTest", Level0, Unknown
               , 1, layerFlags: LayerFlags.None, lastTradedFlags: LastTradedFlags.None);

        var dateToGenerate   = DateTime.UtcNow.Date;
        var currentDayOfWeek = dateToGenerate.DayOfWeek;
        var dayDiff          = DayOfWeek.Sunday - currentDayOfWeek;
        var startOfWeek      = dateToGenerate.AddDays(dayDiff);

        var generateQuoteInfo = new GenerateQuoteInfo(level0SrcTkrQtInfo);
        generateQuoteInfo.MidPriceGenerator!.StartTime  = startOfWeek;
        generateQuoteInfo.MidPriceGenerator!.StartPrice = 1.332211m;

        level0QuoteGenerator   = new Level0QuoteGenerator(generateQuoteInfo);
        pqLevel0QuoteGenerator = new PQLevel0QuoteGenerator(generateQuoteInfo);
    }

    private void CreateLevel0File(FileFlags fileFlags = FileFlags.WriterOpened | FileFlags.HasInternalIndexInHeader)
    {
        fileFlags |= FileFlags.WriterOpened | FileFlags.HasInternalIndexInHeader;

        var testTimeSeriesFilePath = Path.Combine(Environment.CurrentDirectory, GenerateUniqueFileNameOffDateTime());
        var timeSeriesFile         = new FileInfo(testTimeSeriesFilePath);
        if (timeSeriesFile.Exists) timeSeriesFile.Delete();
        var createTestCreateFileParameters =
            new TimeSeriesFileParameters
                (timeSeriesFile
               , new Instrument("TestInstrumentName", "TestSourceName", InstrumentType.Price, Unknown
                              , TimeSeriesPeriod.Tick, "TestInstrumentCategory"),
                 TimeSeriesPeriod.OneWeek, DateTime.UtcNow.Date, 7, fileFlags, 6);
        var createPriceQuoteFile = new PriceTimeSeriesFileParameters(level0SrcTkrQtInfo, createTestCreateFileParameters);
        level0OneWeekFile   = new WeeklyLevel0QuoteTimeSeriesFile(createPriceQuoteFile);
        level0SessionWriter = level0OneWeekFile.GetWriterSession()!;
    }

    [TestCleanup]
    public void TearDown()
    {
        try
        {
            level0SessionReader?.Close();
            level0SessionWriter.Close();
            level0OneWeekFile.Close();
        }
        catch (Exception ex)
        {
            Console.Out.WriteLine("Could not close all sessions. Got {0}", ex);
        }
        var dirInfo = new DirectoryInfo(Environment.CurrentDirectory);
        DeleteTestFiles(dirInfo);
    }

    [TestMethod]
    public void CreateNewPriceQuote_TwoLargeCompressedPeriods_OriginalValuesAreReturned()
    {
        CreateLevel0File(FileFlags.WriteDataCompressed);
        CreateNewTyped_TwoLargeCompressedPeriods_OriginalValuesAreReturned(level0QuoteGenerator, asLevel0PriceQuoteFactory);
    }

    [TestMethod]
    public void CreateNewPQPriceQuote_TwoLargeCompressedPeriods_OriginalValuesAreReturned()
    {
        CreateLevel0File(FileFlags.WriteDataCompressed);
        CreateNewTyped_TwoLargeCompressedPeriods_OriginalValuesAreReturned(pqLevel0QuoteGenerator, asPQLevel0QuoteFactory);
    }

    public void CreateNewTyped_TwoLargeCompressedPeriods_OriginalValuesAreReturned<TEntry>
        (IQuoteGenerator<TEntry> quoteGenerator, Func<ILevel0Quote> retrievalFactory)
        where TEntry : class, IMutableLevel0Quote, ILevel0Quote
    {
        var toPersistAndCheck
            = GenerateRepeatableQuotes<ILevel0Quote, TEntry>
                (1, 8000, 1, DayOfWeek.Wednesday, quoteGenerator).ToList();
        toPersistAndCheck.AddRange
            (GenerateRepeatableQuotes<ILevel0Quote, TEntry>
                (1, 8000, 1, DayOfWeek.Thursday, quoteGenerator));

        foreach (var firstPeriod in toPersistAndCheck)
        {
            var result = level0SessionWriter.AppendEntry(firstPeriod);
            Assert.AreEqual(StorageAttemptResult.PeriodRangeMatched, result.StorageAttemptResult);
        }
        level0OneWeekFile.AutoCloseOnZeroSessions = false;
        level0SessionWriter.Close();

        Assert.AreEqual((uint)toPersistAndCheck.Count, level0OneWeekFile.Header.TotalEntries);
        level0SessionReader = level0OneWeekFile.GetReaderSession();
        var allEntriesReader = level0SessionReader.GetAllEntriesReader(EntryResultSourcing.FromFactoryFuncUnlimited, retrievalFactory);
        var storedItems      = allEntriesReader.ResultEnumerable.ToList();
        Assert.AreEqual(toPersistAndCheck.Count, allEntriesReader.CountMatch);
        Assert.AreEqual(allEntriesReader.CountMatch, allEntriesReader.CountProcessed);
        Assert.AreEqual(toPersistAndCheck.Count, storedItems.Count);
        CompareExpectedToExtracted(toPersistAndCheck, storedItems);
    }

    [TestMethod]
    public void CreateNewPriceQuoteCompressedDataFile_SavesEntriesCloseAndReopen_OriginalValuesAreReturned()
    {
        CreateLevel0File(FileFlags.WriteDataCompressed);
        NewFile_SavesEntriesCloseAndReopen_OriginalValuesAreReturned(level0QuoteGenerator, asLevel0PriceQuoteFactory);
    }

    [TestMethod]
    public void CreateNewPriceQuoteFile_SavesEntriesCloseAndReopen_OriginalValuesAreReturned()
    {
        CreateLevel0File();
        NewFile_SavesEntriesCloseAndReopen_OriginalValuesAreReturned(level0QuoteGenerator, asLevel0PriceQuoteFactory);
    }

    [TestMethod]
    public void CreateNewPQQuoteCompressedDataFile_SavesEntriesCloseAndReopen_OriginalValuesAreReturned()
    {
        CreateLevel0File(FileFlags.WriteDataCompressed);
        NewFile_SavesEntriesCloseAndReopen_OriginalValuesAreReturned(pqLevel0QuoteGenerator, asPQLevel0QuoteFactory);
    }

    [TestMethod]
    public void CreateNewPQQuoteFile_SavesEntriesCloseAndReopen_OriginalValuesAreReturned()
    {
        CreateLevel0File();
        NewFile_SavesEntriesCloseAndReopen_OriginalValuesAreReturned(pqLevel0QuoteGenerator, asPQLevel0QuoteFactory);
    }

    public void NewFile_SavesEntriesCloseAndReopen_OriginalValuesAreReturned<TEntry>
        (IQuoteGenerator<TEntry> quoteGenerator, Func<ILevel0Quote> retrievalFactory)
        where TEntry : class, IMutableLevel0Quote, ILevel0Quote
    {
        var toPersistAndCheck =
            GenerateQuotesForEachDayAndHourOfCurrentWeek<ILevel0Quote, TEntry>
                (0, 10, quoteGenerator).ToList();

        foreach (var level1QuoteStruct in toPersistAndCheck)
        {
            var result = level0SessionWriter.AppendEntry(level1QuoteStruct);
            Assert.AreEqual(StorageAttemptResult.PeriodRangeMatched, result.StorageAttemptResult);
        }
        level0OneWeekFile.AutoCloseOnZeroSessions = false;
        level0SessionWriter.Close();
        Assert.AreEqual((uint)toPersistAndCheck.Count, level0OneWeekFile.Header.TotalEntries);

        level0SessionReader = level0OneWeekFile.GetReaderSession();
        var allEntriesReader = level0SessionReader.GetAllEntriesReader(EntryResultSourcing.FromFactoryFuncUnlimited, retrievalFactory);
        var storedItems      = allEntriesReader.ResultEnumerable.ToList();
        Assert.AreEqual(toPersistAndCheck.Count, allEntriesReader.CountMatch);
        Assert.AreEqual(allEntriesReader.CountMatch, allEntriesReader.CountProcessed);
        Assert.AreEqual(toPersistAndCheck.Count, storedItems.Count);
        CompareExpectedToExtracted(toPersistAndCheck, storedItems);
        var newReaderSession = level0OneWeekFile.GetReaderSession();
        Assert.AreNotSame(level0SessionReader, newReaderSession);
        var newEntriesReader = level0SessionReader.GetAllEntriesReader(EntryResultSourcing.FromFactoryFuncUnlimited, retrievalFactory);
        newEntriesReader.ResultPublishFlags = ResultFlags.CopyToList;
        newEntriesReader.RunReader();
        var listResults = newEntriesReader.ResultList;
        Assert.AreEqual(toPersistAndCheck.Count, newEntriesReader.CountMatch);
        Assert.AreEqual(allEntriesReader.CountMatch, newEntriesReader.CountProcessed);
        Assert.AreEqual(toPersistAndCheck.Count, listResults.Count);
        CompareExpectedToExtracted(toPersistAndCheck, storedItems);
        newReaderSession.Close();
    }

    private void CompareExpectedToExtracted(List<ILevel0Quote> originalList, List<ILevel0Quote> toCompareList)
    {
        for (var i = 0; i < originalList.Count; i++)
        {
            var originalEntry = originalList[i];
            var compareEntry  = toCompareList[i];
            if (!originalEntry.AreEquivalent(compareEntry))
            {
                Logger.Warn("Entries at {0} differ test failed \noriginal {1}\n returned {2}", i, originalEntry, compareEntry);
                FLoggerFactory.WaitUntilDrained();
                Assert.Fail($"Entries at {i} differ test failed \noriginal {originalEntry}\n returned {compareEntry}");
            }
        }
    }

    [TestMethod]
    public void CreateNewFile_SetLargeStringsWhichAreTruncated_CloseReopenSafelyReturnsTruncatedStrings()
    {
        CreateLevel0File();
        var largeString = 999.CharIndexPosListedSizeString();
        var truncated   = largeString.Substring(0, 254); // byte.MaxValue -1 (largest storable value)  (-1 null terminator)
        var header      = level0OneWeekFile.Header;
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
        Assert.AreEqual(typeof(DailyToHourlyLevel0QuoteSubBuckets<ILevel0Quote>), header.BucketType);
        Assert.AreEqual(typeof(ILevel0Quote), header.EntryType);
        Assert.AreEqual(typeof(WeeklyLevel0QuoteTimeSeriesFile), header.TimeSeriesFileType);
        level0OneWeekFile.Close();
        level0OneWeekFile = WeeklyLevel0QuoteTimeSeriesFile
            .OpenExistingTimeSeriesFile(level0OneWeekFile.FileName);
        header = level0OneWeekFile.Header;
        Assert.AreEqual(truncated, header.AnnotationFileRelativePath);
        Assert.AreEqual(truncated, header.ExternalIndexFileRelativePath);
        Assert.AreEqual(truncated, header.OriginSourceText);
        Assert.AreEqual(truncated, header.SourceName);
        Assert.AreEqual(truncated, header.Category);
        Assert.AreEqual(truncated, header.InstrumentName);
        Assert.AreEqual(TimeSeriesPeriod.OneWeek, header.FilePeriod);
        Assert.AreEqual(TimeSeriesPeriod.OneWeek.ContainingPeriodBoundaryStart(DateTime.UtcNow.Date), header.FileStartPeriod);
        Assert.AreEqual(InstrumentType.Price, header.InstrumentType);
        Assert.AreEqual(typeof(DailyToHourlyLevel0QuoteSubBuckets<ILevel0Quote>), header.BucketType);
        Assert.AreEqual(typeof(ILevel0Quote), header.EntryType);
        Assert.AreEqual(typeof(WeeklyLevel0QuoteTimeSeriesFile), header.TimeSeriesFileType);
    }

    [TestMethod]
    public void CreateNewFile_BeyondFileTime_ReturnsFileRangeNotSupported()
    {
        CreateLevel0File();
        Assert.AreEqual(InstrumentType.Price, level0OneWeekFile.InstrumentType);
        var singleQuoteMiddleOfWeek
            = GenerateRepeatableQuotes<ILevel0Quote, Level0PriceQuote>
                (1, 1, 12, DayOfWeek.Wednesday, level0QuoteGenerator);
        var nextWeekQuote = (IMutableLevel0Quote)singleQuoteMiddleOfWeek.First();
        nextWeekQuote.SourceTime = nextWeekQuote.SourceTime.AddDays(7);
        var result = level0SessionWriter.AppendEntry(nextWeekQuote);
        Assert.AreEqual(StorageAttemptResult.NextFilePeriod, result.StorageAttemptResult);
    }

    [TestMethod]
    public void CreateNewFile_AfterBucketClosesOnNextEntry_TryingToAddExistingEntryReturnsBucketClosedForAppend()
    {
        CreateLevel0File();
        var wednesdayQuotes =
            GenerateRepeatableQuotes<ILevel0Quote, Level0PriceQuote>
                (1, 1, 12, DayOfWeek.Wednesday, level0QuoteGenerator);
        var thursdayQuotes =
            GenerateRepeatableQuotes<ILevel0Quote, Level0PriceQuote>
                (1, 1, 12, DayOfWeek.Thursday, level0QuoteGenerator);
        var wednesdayQuote = wednesdayQuotes.First();
        var thursdayQuote  = thursdayQuotes.First();
        var result         = level0SessionWriter.AppendEntry(wednesdayQuote);
        Assert.AreEqual(StorageAttemptResult.PeriodRangeMatched, result.StorageAttemptResult);
        result = level0SessionWriter.AppendEntry(thursdayQuote);
        Assert.AreEqual(StorageAttemptResult.PeriodRangeMatched, result.StorageAttemptResult);
        result = level0SessionWriter.AppendEntry(wednesdayQuote);
        Assert.AreEqual(StorageAttemptResult.BucketClosedForAppend, result.StorageAttemptResult);
    }
}
