// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory.UnmanagedMemory.MemoryMappedFiles;
using FortitudeCommon.Extensions;
using FortitudeCommon.Monitoring.Logging;
using FortitudeIO.TimeSeries;
using FortitudeIO.TimeSeries.FileSystem.DirectoryStructure;
using FortitudeIO.TimeSeries.FileSystem.File;
using FortitudeIO.TimeSeries.FileSystem.File.Buckets;
using FortitudeIO.TimeSeries.FileSystem.Session;
using FortitudeIO.TimeSeries.FileSystem.Session.Retrieval;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsApi.Pricing.Quotes.LastTraded;
using FortitudeMarketsApi.Pricing.Quotes.LayeredBook;
using FortitudeMarketsCore.Pricing.Generators.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Generators.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;
using FortitudeMarketsCore.Pricing.PQ.TimeSeries.FileSystem.File;
using FortitudeMarketsCore.Pricing.PQ.TimeSeries.FileSystem.File.Buckets;
using FortitudeMarketsCore.Pricing.Quotes;
using static FortitudeMarketsApi.Configuration.ClientServerConfig.MarketClassificationExtensions;
using static FortitudeMarketsApi.Pricing.Quotes.QuoteLevel;
using static FortitudeTests.FortitudeMarketsCore.Pricing.PQ.TimeSeries.FileSystem.File.TestWeeklyDataGeneratorFixture;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.TimeSeries.FileSystem.File;

[TestClass]
public class WeeklyLevel1QuoteTimeSeriesFileTests
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(WeeklyLevel1QuoteTimeSeriesFileTests));

    private readonly Func<ILevel1Quote> asLevel1PriceQuoteFactory = () => new Level1PriceQuote();
    private readonly Func<ILevel1Quote> asPQLevel1QuoteFactory    = () => new PQLevel1Quote();

    private WeeklyLevel1QuoteTimeSeriesFile level1OneWeekFile = null!;

    private Level1QuoteGenerator level1QuoteGenerator = null!;

    private IReaderSession<ILevel1Quote>? level1SessionReader;
    private IWriterSession<ILevel1Quote>  level1SessionWriter = null!;

    private SourceTickerQuoteInfo  level1SrcTkrQtInfo     = null!;
    private PQLevel1QuoteGenerator pqLevel1QuoteGenerator = null!;

    [TestInitialize]
    public void Setup()
    {
        PagedMemoryMappedFile.LogMappingMessages = true;
        level1SrcTkrQtInfo =
            new SourceTickerQuoteInfo
                (19, "WeeklyLevel1QuoteTimeSeriesFileTests", 79, "PersistTest", Level1, Unknown
               , 1, layerFlags: LayerFlags.None, lastTradedFlags: LastTradedFlags.None, roundingPrecision: 0.000001m);


        var dateToGenerate = DateTime.UtcNow.Date.TruncToMonthBoundary().AddDays(15);
        ;
        var currentDayOfWeek = dateToGenerate.DayOfWeek;
        var dayDiff          = DayOfWeek.Sunday - currentDayOfWeek;
        var startOfWeek      = dateToGenerate.AddDays(dayDiff);

        var generateQuoteInfo = new GenerateQuoteInfo(level1SrcTkrQtInfo);
        generateQuoteInfo.MidPriceGenerator!.StartTime  = startOfWeek;
        generateQuoteInfo.MidPriceGenerator!.StartPrice = 1.332211m;

        level1QuoteGenerator   = new Level1QuoteGenerator(generateQuoteInfo);
        pqLevel1QuoteGenerator = new PQLevel1QuoteGenerator(generateQuoteInfo);
    }

    private void CreateLevel1File(FileFlags fileFlags = FileFlags.WriterOpened | FileFlags.HasInternalIndexInHeader)
    {
        fileFlags |= FileFlags.WriterOpened | FileFlags.HasInternalIndexInHeader;

        var testTimeSeriesFilePath = Path.Combine(Environment.CurrentDirectory, GenerateUniqueFileNameOffDateTime());
        var timeSeriesFile         = new FileInfo(testTimeSeriesFilePath);
        if (timeSeriesFile.Exists) timeSeriesFile.Delete();
        var instrumentFields = new Dictionary<string, string>
        {
            { nameof(RepositoryPathName.SourceName), "TestSourceName" }, { nameof(RepositoryPathName.MarketType), "Unknown" }
          , { nameof(RepositoryPathName.MarketProductType), "Unknown" }, { nameof(RepositoryPathName.MarketRegion), "Unknown" }
        };
        var optionalInstrumentFields = new Dictionary<string, string>
        {
            { nameof(RepositoryPathName.Category), "TestInstrumentCategory" }
        };
        var createTestCreateFileParameters =
            new TimeSeriesFileParameters
                (timeSeriesFile
               , new Instrument("TestInstrumentName", InstrumentType.Price, TimeSeriesPeriod.Tick, instrumentFields, optionalInstrumentFields),
                 TimeSeriesPeriod.OneWeek, DateTime.UtcNow.Date, 7, fileFlags, 6);
        var createPriceQuoteFile = new PriceTimeSeriesFileParameters(level1SrcTkrQtInfo, createTestCreateFileParameters);
        level1OneWeekFile   = new WeeklyLevel1QuoteTimeSeriesFile(createPriceQuoteFile);
        level1SessionWriter = level1OneWeekFile.GetWriterSession()!;
    }

    [TestCleanup]
    public void TearDown()
    {
        try
        {
            level1SessionReader?.Close();
            level1SessionWriter.Close();
            level1OneWeekFile.Close();
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
        CreateLevel1File(FileFlags.WriteDataCompressed);
        CreateNewTyped_TwoLargeCompressedPeriods_OriginalValuesAreReturned(level1QuoteGenerator, asLevel1PriceQuoteFactory);
    }

    [TestMethod]
    public void CreateNewPQPriceQuote_TwoLargeCompressedPeriods_OriginalValuesAreReturned()
    {
        CreateLevel1File(FileFlags.WriteDataCompressed);
        CreateNewTyped_TwoLargeCompressedPeriods_OriginalValuesAreReturned(pqLevel1QuoteGenerator, asPQLevel1QuoteFactory);
    }

    public void CreateNewTyped_TwoLargeCompressedPeriods_OriginalValuesAreReturned<TEntry>
        (IQuoteGenerator<TEntry> quoteGenerator, Func<ILevel1Quote> retrievalFactory)
        where TEntry : class, IMutableLevel1Quote, ILevel1Quote
    {
        var toPersistAndCheck
            = GenerateRepeatableQuotes<ILevel1Quote, TEntry>
                (1, 8000, 1, DayOfWeek.Wednesday, quoteGenerator).ToList();
        toPersistAndCheck.AddRange
            (GenerateRepeatableQuotes<ILevel1Quote, TEntry>
                (1, 8000, 1, DayOfWeek.Thursday, quoteGenerator));

        foreach (var firstPeriod in toPersistAndCheck)
        {
            var result = level1SessionWriter.AppendEntry(firstPeriod);
            Assert.AreEqual(StorageAttemptResult.PeriodRangeMatched, result.StorageAttemptResult);
        }
        level1OneWeekFile.AutoCloseOnZeroSessions = false;
        level1SessionWriter.Close();

        Assert.AreEqual((uint)toPersistAndCheck.Count, level1OneWeekFile.Header.TotalEntries);
        level1SessionReader = level1OneWeekFile.GetReaderSession();
        var allEntriesReader = level1SessionReader.GetAllEntriesReader(EntryResultSourcing.FromFactoryFuncUnlimited, retrievalFactory);
        var storedItems      = allEntriesReader.ResultEnumerable.ToList();
        Assert.AreEqual(toPersistAndCheck.Count, allEntriesReader.CountMatch);
        Assert.AreEqual(allEntriesReader.CountMatch, allEntriesReader.CountProcessed);
        Assert.AreEqual(toPersistAndCheck.Count, storedItems.Count);
        CompareExpectedToExtracted(toPersistAndCheck, storedItems);
    }

    [TestMethod]
    public void CreateNewPriceQuoteCompressedDataFile_SavesEntriesCloseAndReopen_OriginalValuesAreReturned()
    {
        CreateLevel1File(FileFlags.WriteDataCompressed);
        NewFile_SavesEntriesCloseAndReopen_OriginalValuesAreReturned(level1QuoteGenerator, asLevel1PriceQuoteFactory);
    }

    [TestMethod]
    public void CreateNewPriceQuoteFile_SavesEntriesCloseAndReopen_OriginalValuesAreReturned()
    {
        CreateLevel1File();
        NewFile_SavesEntriesCloseAndReopen_OriginalValuesAreReturned(level1QuoteGenerator, asLevel1PriceQuoteFactory);
    }

    [TestMethod]
    public void CreateNewPQQuoteCompressedDataFile_SavesEntriesCloseAndReopen_OriginalValuesAreReturned()
    {
        CreateLevel1File(FileFlags.WriteDataCompressed);
        NewFile_SavesEntriesCloseAndReopen_OriginalValuesAreReturned(pqLevel1QuoteGenerator, asPQLevel1QuoteFactory);
    }

    [TestMethod]
    public void CreateNewPQQuoteFile_SavesEntriesCloseAndReopen_OriginalValuesAreReturned()
    {
        CreateLevel1File();
        NewFile_SavesEntriesCloseAndReopen_OriginalValuesAreReturned(pqLevel1QuoteGenerator, asPQLevel1QuoteFactory);
    }

    public void NewFile_SavesEntriesCloseAndReopen_OriginalValuesAreReturned<TEntry>
        (IQuoteGenerator<TEntry> quoteGenerator, Func<ILevel1Quote> retrievalFactory)
        where TEntry : class, IMutableLevel1Quote, ILevel1Quote
    {
        var toPersistAndCheck =
            GenerateQuotesForEachDayAndHourOfCurrentWeek<ILevel1Quote, TEntry>
                (0, 10, quoteGenerator).ToList();

        foreach (var level1QuoteStruct in toPersistAndCheck)
        {
            var result = level1SessionWriter.AppendEntry(level1QuoteStruct);
            Assert.AreEqual(StorageAttemptResult.PeriodRangeMatched, result.StorageAttemptResult);
        }
        level1OneWeekFile.AutoCloseOnZeroSessions = false;
        level1SessionWriter.Close();
        Assert.AreEqual((uint)toPersistAndCheck.Count, level1OneWeekFile.Header.TotalEntries);

        level1SessionReader = level1OneWeekFile.GetReaderSession();
        var allEntriesReader = level1SessionReader.GetAllEntriesReader(EntryResultSourcing.FromFactoryFuncUnlimited, retrievalFactory);
        var storedItems      = allEntriesReader.ResultEnumerable.ToList();
        Assert.AreEqual(toPersistAndCheck.Count, allEntriesReader.CountMatch);
        Assert.AreEqual(allEntriesReader.CountMatch, allEntriesReader.CountProcessed);
        Assert.AreEqual(toPersistAndCheck.Count, storedItems.Count);
        CompareExpectedToExtracted(toPersistAndCheck, storedItems);
        var newReaderSession = level1OneWeekFile.GetReaderSession();
        Assert.AreNotSame(level1SessionReader, newReaderSession);
        var newEntriesReader = level1SessionReader.GetAllEntriesReader(EntryResultSourcing.FromFactoryFuncUnlimited, retrievalFactory);
        newEntriesReader.ResultPublishFlags = ResultFlags.CopyToList;
        newEntriesReader.RunReader();
        var listResults = newEntriesReader.ResultList;
        Assert.AreEqual(toPersistAndCheck.Count, newEntriesReader.CountMatch);
        Assert.AreEqual(allEntriesReader.CountMatch, newEntriesReader.CountProcessed);
        Assert.AreEqual(toPersistAndCheck.Count, listResults.Count);
        CompareExpectedToExtracted(toPersistAndCheck, storedItems);
        newReaderSession.Close();
    }

    private void CompareExpectedToExtracted(List<ILevel1Quote> originalList, List<ILevel1Quote> toCompareList)
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
        CreateLevel1File();
        var largeString = 999.CharIndexPosListedSizeString();
        var truncated   = largeString.Substring(0, 254); // byte.MaxValue -1 (largest storable value)  (-1 null terminator)
        var header      = level1OneWeekFile.Header;
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
        Assert.AreEqual(typeof(DailyToHourlyLevel1QuoteSubBuckets<ILevel1Quote>), header.BucketType);
        Assert.AreEqual(typeof(ILevel1Quote), header.EntryType);
        Assert.AreEqual(typeof(WeeklyLevel1QuoteTimeSeriesFile), header.TimeSeriesFileType);
        level1OneWeekFile.Close();
        level1OneWeekFile = WeeklyLevel1QuoteTimeSeriesFile
            .OpenExistingTimeSeriesFile(level1OneWeekFile.FileName);
        header = level1OneWeekFile.Header;
        Assert.AreEqual(truncated, header.AnnotationFileRelativePath);
        Assert.AreEqual(truncated, header.ExternalIndexFileRelativePath);
        Assert.AreEqual(truncated, header.OriginSourceText);
        Assert.AreEqual(truncated, header.SourceName);
        Assert.AreEqual(truncated, header.Category);
        Assert.AreEqual(truncated, header.InstrumentName);
        Assert.AreEqual(TimeSeriesPeriod.OneWeek, header.FilePeriod);
        Assert.AreEqual(TimeSeriesPeriod.OneWeek.ContainingPeriodBoundaryStart(DateTime.UtcNow.Date), header.FileStartPeriod);
        Assert.AreEqual(InstrumentType.Price, header.InstrumentType);
        Assert.AreEqual(typeof(DailyToHourlyLevel1QuoteSubBuckets<ILevel1Quote>), header.BucketType);
        Assert.AreEqual(typeof(ILevel1Quote), header.EntryType);
        Assert.AreEqual(typeof(WeeklyLevel1QuoteTimeSeriesFile), header.TimeSeriesFileType);
    }

    [TestMethod]
    public void CreateNewFile_BeyondFileTime_ReturnsFileRangeNotSupported()
    {
        CreateLevel1File();
        Assert.AreEqual(InstrumentType.Price, level1OneWeekFile.InstrumentType);
        var singleQuoteMiddleOfWeek
            = GenerateRepeatableQuotes<ILevel1Quote, Level1PriceQuote>
                (1, 1, 12, DayOfWeek.Wednesday, level1QuoteGenerator);
        var nextWeekQuote = (IMutableLevel1Quote)singleQuoteMiddleOfWeek.First();
        nextWeekQuote.SourceTime = nextWeekQuote.SourceTime.AddDays(7);
        var result = level1SessionWriter.AppendEntry(nextWeekQuote);
        Assert.AreEqual(StorageAttemptResult.NextFilePeriod, result.StorageAttemptResult);
    }

    [TestMethod]
    public void CreateNewFile_AfterBucketClosesOnNextEntry_TryingToAddExistingEntryReturnsBucketClosedForAppend()
    {
        CreateLevel1File();
        var wednesdayQuotes =
            GenerateRepeatableQuotes<ILevel1Quote, Level1PriceQuote>
                (1, 1, 12, DayOfWeek.Wednesday, level1QuoteGenerator);
        var thursdayQuotes =
            GenerateRepeatableQuotes<ILevel1Quote, Level1PriceQuote>
                (1, 1, 12, DayOfWeek.Thursday, level1QuoteGenerator);
        var wednesdayQuote = wednesdayQuotes.First();
        var thursdayQuote  = thursdayQuotes.First();
        var result         = level1SessionWriter.AppendEntry(wednesdayQuote);
        Assert.AreEqual(StorageAttemptResult.PeriodRangeMatched, result.StorageAttemptResult);
        result = level1SessionWriter.AppendEntry(thursdayQuote);
        Assert.AreEqual(StorageAttemptResult.PeriodRangeMatched, result.StorageAttemptResult);
        result = level1SessionWriter.AppendEntry(wednesdayQuote);
        Assert.AreEqual(StorageAttemptResult.BucketClosedForAppend, result.StorageAttemptResult);
    }
}
