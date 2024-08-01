// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.DataStructures.Memory.UnmanagedMemory.MemoryMappedFiles;
using FortitudeCommon.Extensions;
using FortitudeCommon.Monitoring.Logging;
using FortitudeIO.TimeSeries;
using FortitudeIO.TimeSeries.FileSystem.DirectoryStructure;
using FortitudeIO.TimeSeries.FileSystem.File;
using FortitudeIO.TimeSeries.FileSystem.File.Buckets;
using FortitudeIO.TimeSeries.FileSystem.Session;
using FortitudeIO.TimeSeries.FileSystem.Session.Retrieval;
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
using static FortitudeMarketsApi.Pricing.Quotes.TickerDetailLevel;
using static FortitudeTests.FortitudeMarketsCore.Pricing.PQ.TimeSeries.FileSystem.File.TestWeeklyDataGeneratorFixture;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.TimeSeries.FileSystem.File;

[TestClass]
public class WeeklyTickInstantTimeSeriesFileTests
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(WeeklyTickInstantTimeSeriesFileTests));

    private readonly Func<ITickInstant> asPQTickInstantFactory = () => new PQTickInstant();
    private readonly Func<ITickInstant> asTickInstantFactory   = () => new TickInstant();

    private PQTickInstantGenerator pqTickInstantGenerator = null!;

    private TickInstantGenerator tickInstantGenerator = null!;

    private WeeklyTickInstantTimeSeriesFile tickInstantOneWeekFile = null!;

    private IReaderSession<ITickInstant>? tickInstantSessionReader;
    private IWriterSession<ITickInstant>  tickInstantSessionWriter = null!;

    private SourceTickerInfo tickInstantSrcTkrInfo = null!;

    [TestInitialize]
    public void Setup()
    {
        PagedMemoryMappedFile.LogMappingMessages = true;
        tickInstantSrcTkrInfo =
            new SourceTickerInfo
                (19, "WeeklyTickInstantTimeSeriesFileTests", 79, "PersistTest", SingleValue, Unknown
               , 1, layerFlags: LayerFlags.None, lastTradedFlags: LastTradedFlags.None, roundingPrecision: 0.000001m);

        var dateToGenerate = DateTime.UtcNow.Date.TruncToMonthBoundary().AddDays(15);
        ;
        var currentDayOfWeek = dateToGenerate.DayOfWeek;
        var dayDiff          = DayOfWeek.Sunday - currentDayOfWeek;
        var startOfWeek      = dateToGenerate.AddDays(dayDiff);

        var generateQuoteInfo = new GenerateQuoteInfo(tickInstantSrcTkrInfo);
        generateQuoteInfo.MidPriceGenerator!.StartTime  = startOfWeek;
        generateQuoteInfo.MidPriceGenerator!.StartPrice = 1.332211m;

        tickInstantGenerator   = new TickInstantGenerator(generateQuoteInfo);
        pqTickInstantGenerator = new PQTickInstantGenerator(generateQuoteInfo);
    }

    private void CreateTickInstantFile(FileFlags fileFlags = FileFlags.WriterOpened | FileFlags.HasInternalIndexInHeader)
    {
        fileFlags |= FileFlags.WriterOpened | FileFlags.HasInternalIndexInHeader;

        var timeSeriesFile = GenerateUniqueFileNameOffDateTime();
        if (timeSeriesFile.Exists) timeSeriesFile.Delete();
        var instrumentFields = new Dictionary<string, string>
        {
            { nameof(RepositoryPathName.MarketType), "Unknown" }
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
                              , instrumentFields, optionalInstrumentFields), TimeBoundaryPeriod.OneWeek, DateTime.UtcNow.Date, 7, fileFlags, 6);
        var createPriceQuoteFile = new PriceTimeSeriesFileParameters(tickInstantSrcTkrInfo, createTestCreateFileParameters);
        tickInstantOneWeekFile   = new WeeklyTickInstantTimeSeriesFile(createPriceQuoteFile);
        tickInstantSessionWriter = tickInstantOneWeekFile.GetWriterSession()!;
    }

    [TestCleanup]
    public void TearDown()
    {
        try
        {
            tickInstantSessionReader?.Close();
            tickInstantSessionWriter.Close();
            tickInstantOneWeekFile.Close();
        }
        catch (Exception ex)
        {
            Console.Out.WriteLine("Could not close all sessions. Got {0}", ex);
        }
        DeleteTestFiles();
    }

    [TestMethod]
    public void CreateNewPriceQuote_TwoLargeCompressedPeriods_OriginalValuesAreReturned()
    {
        CreateTickInstantFile(FileFlags.WriteDataCompressed);
        CreateNewTyped_TwoLargeCompressedPeriods_OriginalValuesAreReturned(tickInstantGenerator, asTickInstantFactory);
    }

    [TestMethod]
    public void CreateNewPQPriceQuote_TwoLargeCompressedPeriods_OriginalValuesAreReturned()
    {
        CreateTickInstantFile(FileFlags.WriteDataCompressed);
        CreateNewTyped_TwoLargeCompressedPeriods_OriginalValuesAreReturned(pqTickInstantGenerator, asPQTickInstantFactory);
    }

    public void CreateNewTyped_TwoLargeCompressedPeriods_OriginalValuesAreReturned<TEntry>
        (ITickGenerator<TEntry> tickGenerator, Func<ITickInstant> retrievalFactory)
        where TEntry : class, IMutableTickInstant, ITickInstant
    {
        var toPersistAndCheck
            = GenerateRepeatableQuotes<ITickInstant, TEntry>
                (1, 8000, 1, DayOfWeek.Wednesday, tickGenerator).ToList();
        toPersistAndCheck.AddRange
            (GenerateRepeatableQuotes<ITickInstant, TEntry>
                (1, 8000, 1, DayOfWeek.Thursday, tickGenerator));

        foreach (var firstPeriod in toPersistAndCheck)
        {
            var result = tickInstantSessionWriter.AppendEntry(firstPeriod);
            Assert.AreEqual(StorageAttemptResult.PeriodRangeMatched, result.StorageAttemptResult);
        }
        tickInstantOneWeekFile.AutoCloseOnZeroSessions = false;
        tickInstantSessionWriter.Close();

        Assert.AreEqual((uint)toPersistAndCheck.Count, tickInstantOneWeekFile.Header.TotalEntries);
        tickInstantSessionReader = tickInstantOneWeekFile.GetReaderSession();
        var allEntriesReader = tickInstantSessionReader.AllChronologicalEntriesReader
            (new Recycler(), EntryResultSourcing.FromFactoryFuncUnlimited, ReaderOptions.ReadFastAsPossible, retrievalFactory);
        var storedItems = allEntriesReader.ResultEnumerable.ToList();
        Assert.AreEqual(toPersistAndCheck.Count, allEntriesReader.CountMatch);
        Assert.AreEqual(allEntriesReader.CountMatch, allEntriesReader.CountProcessed);
        Assert.AreEqual(toPersistAndCheck.Count, storedItems.Count);
        CompareExpectedToExtracted(toPersistAndCheck, storedItems);
    }

    [TestMethod]
    public void CreateNewPriceQuoteCompressedDataFile_SavesEntriesCloseAndReopen_ReadInReverseOriginalValuesAreReturnedInReverseOrder()
    {
        CreateTickInstantFile(FileFlags.WriteDataCompressed);
        NewFile_SavesEntriesCloseAndReopen_ReadInReverseOriginalValuesAreReturned(tickInstantGenerator);
    }

    [TestMethod]
    public void CreateNewPriceQuoteFile_SavesEntriesCloseAndReopen_ReadInReverseOriginalValuesAreReturnedInReverseOrder()
    {
        CreateTickInstantFile();
        NewFile_SavesEntriesCloseAndReopen_ReadInReverseOriginalValuesAreReturned(tickInstantGenerator);
    }

    [TestMethod]
    public void CreateNewPQQuoteCompressedDataFile_SavesEntriesCloseAndReopen_OriginalValuesAreReturned()
    {
        CreateTickInstantFile(FileFlags.WriteDataCompressed);
        NewFile_SavesEntriesCloseAndReopen_OriginalValuesAreReturned(pqTickInstantGenerator, asPQTickInstantFactory);
    }

    [TestMethod]
    public void CreateNewPQQuoteFile_SavesEntriesCloseAndReopen_OriginalValuesAreReturned()
    {
        CreateTickInstantFile();
        NewFile_SavesEntriesCloseAndReopen_OriginalValuesAreReturned(pqTickInstantGenerator, asPQTickInstantFactory);
    }

    public void NewFile_SavesEntriesCloseAndReopen_OriginalValuesAreReturned<TEntry>
        (ITickGenerator<TEntry> tickGenerator, Func<ITickInstant> retrievalFactory)
        where TEntry : class, IMutableTickInstant, ITickInstant
    {
        var toPersistAndCheck =
            GenerateQuotesForEachDayAndHourOfCurrentWeek<ITickInstant, TEntry>
                (0, 10, tickGenerator).ToList();

        foreach (var level1QuoteStruct in toPersistAndCheck)
        {
            var result = tickInstantSessionWriter.AppendEntry(level1QuoteStruct);
            Assert.AreEqual(StorageAttemptResult.PeriodRangeMatched, result.StorageAttemptResult);
        }
        tickInstantOneWeekFile.AutoCloseOnZeroSessions = false;
        tickInstantSessionWriter.Close();
        Assert.AreEqual((uint)toPersistAndCheck.Count, tickInstantOneWeekFile.Header.TotalEntries);

        tickInstantSessionReader = tickInstantOneWeekFile.GetReaderSession();
        var allEntriesReader = tickInstantSessionReader.AllChronologicalEntriesReader
            (new Recycler(), EntryResultSourcing.FromFactoryFuncUnlimited, ReaderOptions.ReadFastAsPossible, retrievalFactory);
        var storedItems = allEntriesReader.ResultEnumerable.ToList();
        Assert.AreEqual(toPersistAndCheck.Count, allEntriesReader.CountMatch);
        Assert.AreEqual(allEntriesReader.CountMatch, allEntriesReader.CountProcessed);
        Assert.AreEqual(toPersistAndCheck.Count, storedItems.Count);
        CompareExpectedToExtracted(toPersistAndCheck, storedItems);
        var newReaderSession = tickInstantOneWeekFile.GetReaderSession();
        Assert.AreNotSame(tickInstantSessionReader, newReaderSession);
        var newEntriesReader = tickInstantSessionReader.AllChronologicalEntriesReader
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

    public void NewFile_SavesEntriesCloseAndReopen_ReadInReverseOriginalValuesAreReturned<TEntry>
        (ITickGenerator<TEntry> tickGenerator)
        where TEntry : class, IMutableTickInstant, ITickInstant
    {
        var toPersistAndCheck =
            GenerateQuotesForEachDayAndHourOfCurrentWeek<ITickInstant, TEntry>
                (0, 10, tickGenerator).ToList();

        foreach (var level1QuoteStruct in toPersistAndCheck)
        {
            var result = tickInstantSessionWriter.AppendEntry(level1QuoteStruct);
            Assert.AreEqual(StorageAttemptResult.PeriodRangeMatched, result.StorageAttemptResult);
        }
        tickInstantOneWeekFile.AutoCloseOnZeroSessions = false;
        tickInstantSessionWriter.Close();
        Assert.AreEqual((uint)toPersistAndCheck.Count, tickInstantOneWeekFile.Header.TotalEntries);

        tickInstantSessionReader = tickInstantOneWeekFile.GetReaderSession();
        var allEntriesReader = tickInstantSessionReader.AllReverseChronologicalEntriesReader<TickInstant>(new Recycler());
        var storedItems      = allEntriesReader.ResultEnumerable.ToList();
        Assert.AreEqual(toPersistAndCheck.Count, allEntriesReader.CountMatch);
        Assert.AreEqual(allEntriesReader.CountMatch, allEntriesReader.CountProcessed);
        Assert.AreEqual(toPersistAndCheck.Count, storedItems.Count);
        toPersistAndCheck.Reverse();
        CompareExpectedToExtracted(toPersistAndCheck, storedItems);
        var newReaderSession = tickInstantOneWeekFile.GetReaderSession();
        Assert.AreNotSame(tickInstantSessionReader, newReaderSession);
        var newEntriesReader = tickInstantSessionReader.AllReverseChronologicalEntriesReader<TickInstant>(new Recycler());
        newEntriesReader.ResultPublishFlags = ResultFlags.CopyToList;
        newEntriesReader.RunReader();
        var listResults = newEntriesReader.ResultList;
        Assert.AreEqual(toPersistAndCheck.Count, newEntriesReader.CountMatch);
        Assert.AreEqual(allEntriesReader.CountMatch, newEntriesReader.CountProcessed);
        Assert.AreEqual(toPersistAndCheck.Count, listResults.Count);
        CompareExpectedToExtracted(toPersistAndCheck, storedItems);
        newReaderSession.Close();
    }

    private void CompareExpectedToExtracted(List<ITickInstant> originalList, List<ITickInstant> toCompareList)
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
        CreateTickInstantFile();
        var largeString = 999.CharIndexPosListedSizeString();
        var truncated   = largeString.Substring(0, 254); // byte.MaxValue -1 (largest storable value)  (-1 null terminator)
        var header      = tickInstantOneWeekFile.Header;
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
        Assert.AreEqual(typeof(DailyToHourlyTickInstantSubBuckets<ITickInstant>), header.BucketType);
        Assert.AreEqual(typeof(ITickInstant), header.EntryType);
        Assert.AreEqual(typeof(WeeklyTickInstantTimeSeriesFile), header.TimeSeriesFileType);
        tickInstantOneWeekFile.Close();
        tickInstantOneWeekFile = WeeklyTickInstantTimeSeriesFile
            .OpenExistingTimeSeriesFile(tickInstantOneWeekFile.FileName);
        header = tickInstantOneWeekFile.Header;
        Assert.AreEqual(truncated, header.AnnotationFileRelativePath);
        Assert.AreEqual(truncated, header.ExternalIndexFileRelativePath);
        Assert.AreEqual(truncated, header.OriginSourceText);
        Assert.AreEqual(truncated, header.SourceName);
        Assert.AreEqual(truncated, header.Category);
        Assert.AreEqual(truncated, header.InstrumentName);
        Assert.AreEqual(TimeBoundaryPeriod.OneWeek, header.FilePeriod);
        Assert.AreEqual(TimeBoundaryPeriod.OneWeek.ContainingPeriodBoundaryStart(DateTime.UtcNow.Date), header.FileStartPeriod);
        Assert.AreEqual(InstrumentType.Price, header.InstrumentType);
        Assert.AreEqual(typeof(DailyToHourlyTickInstantSubBuckets<ITickInstant>), header.BucketType);
        Assert.AreEqual(typeof(ITickInstant), header.EntryType);
        Assert.AreEqual(typeof(WeeklyTickInstantTimeSeriesFile), header.TimeSeriesFileType);
    }

    [TestMethod]
    public void CreateNewFile_BeyondFileTime_ReturnsFileRangeNotSupported()
    {
        CreateTickInstantFile();
        Assert.AreEqual(InstrumentType.Price, tickInstantOneWeekFile.InstrumentType);
        var singleQuoteMiddleOfWeek
            = GenerateRepeatableQuotes<ITickInstant, TickInstant>
                (1, 1, 12, DayOfWeek.Wednesday, tickInstantGenerator);
        var nextWeekQuote = (IMutableTickInstant)singleQuoteMiddleOfWeek.First();
        nextWeekQuote.SourceTime = nextWeekQuote.SourceTime.AddDays(7);
        var result = tickInstantSessionWriter.AppendEntry(nextWeekQuote);
        Assert.AreEqual(StorageAttemptResult.NextFilePeriod, result.StorageAttemptResult);
    }

    [TestMethod]
    public void CreateNewFile_AfterBucketClosesOnNextEntry_TryingToAddExistingEntryReturnsBucketClosedForAppend()
    {
        CreateTickInstantFile();
        var wednesdayQuotes =
            GenerateRepeatableQuotes<ITickInstant, TickInstant>
                (1, 1, 12, DayOfWeek.Wednesday, tickInstantGenerator);
        var thursdayQuotes =
            GenerateRepeatableQuotes<ITickInstant, TickInstant>
                (1, 1, 12, DayOfWeek.Thursday, tickInstantGenerator);
        var wednesdayQuote = wednesdayQuotes.First();
        var thursdayQuote  = thursdayQuotes.First();
        var result         = tickInstantSessionWriter.AppendEntry(wednesdayQuote);
        Assert.AreEqual(StorageAttemptResult.PeriodRangeMatched, result.StorageAttemptResult);
        result = tickInstantSessionWriter.AppendEntry(thursdayQuote);
        Assert.AreEqual(StorageAttemptResult.PeriodRangeMatched, result.StorageAttemptResult);
        result = tickInstantSessionWriter.AppendEntry(wednesdayQuote);
        Assert.AreEqual(StorageAttemptResult.BucketClosedForAppend, result.StorageAttemptResult);
    }
}
