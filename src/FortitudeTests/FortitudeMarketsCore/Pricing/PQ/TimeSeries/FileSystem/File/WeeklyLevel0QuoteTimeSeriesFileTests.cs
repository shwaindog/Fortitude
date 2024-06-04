// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory.UnmanagedMemory.MemoryMappedFiles;
using FortitudeCommon.Extensions;
using FortitudeCommon.Monitoring.Logging;
using FortitudeIO.TimeSeries;
using FortitudeIO.TimeSeries.FileSystem.File;
using FortitudeIO.TimeSeries.FileSystem.File.Buckets;
using FortitudeIO.TimeSeries.FileSystem.File.Reading;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.LastTraded;
using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;
using FortitudeMarketsCore.Pricing.PQ.TimeSeries.FileSystem.File;
using FortitudeMarketsCore.Pricing.PQ.TimeSeries.FileSystem.File.Buckets;
using FortitudeMarketsCore.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.Quotes.Generators;
using FortitudeTests.FortitudeIO.TimeSeries.FileSystem.File;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.TimeSeries.FileSystem.File;

[TestClass]
public class WeeklyLevel0QuoteTimeSeriesFileTests
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(WeeklyLevel0QuoteTimeSeriesFileTests));

    private static int newTestCount;

    private Func<ILevel0Quote>? asLevel0PriceQuoteFactory = () => new Level0PriceQuote();
    private Func<ILevel0Quote>? asPQLevel0QuoteFactory    = () => new PQLevel0Quote();

    private WeeklyLevel0QuoteTimeSeriesFile level0OneWeekFile = null!;

    private Level0QuoteGenerator level0QuoteGenerator;

    private IReaderFileSession<DailyToOneHourPQLevel0QuoteSubBuckets<ILevel0Quote>, ILevel0Quote>? level0SessionReader;
    private IWriterFileSession<DailyToOneHourPQLevel0QuoteSubBuckets<ILevel0Quote>, ILevel0Quote>  level0SessionWriter = null!;

    private SourceTickerQuoteInfo level0SrcTkrQtInfo;
    // private Level1QuoteGenerator  level1QuoteGenerator;
    // private SourceTickerQuoteInfo level1SrcTkrQtInfo;
    // private Level2QuoteGenerator  level2QuoteGenerator;
    // private SourceTickerQuoteInfo level2SrcTkrQtInfo;
    // private Level3QuoteGenerator  level3QuoteGenerator;
    // private SourceTickerQuoteInfo level3SrcTkrQtInfo;

    [TestInitialize]
    public void Setup()
    {
        PagedMemoryMappedFile.LogMappingMessages = true;
        level0SrcTkrQtInfo =
            new SourceTickerQuoteInfo
                (19, "OneWeekDailyHourlyPriceQuoteTimeSeriesFileTests", 79, "PersistTest",
                 QuoteLevel.Level0, 1, layerFlags: LayerFlags.None, lastTradedFlags: LastTradedFlags.None);
        // level1SrcTkrQtInfo =
        //     new SourceTickerQuoteInfo
        //         (19, "OneWeekDailyHourlyPriceQuoteTimeSeriesFileTests", 79, "PersistTest",
        //          QuoteLevel.Level1, 1, layerFlags: LayerFlags.None, lastTradedFlags: LastTradedFlags.None);
        // level2SrcTkrQtInfo =
        //     new SourceTickerQuoteInfo
        //         (19, "OneWeekDailyHourlyPriceQuoteTimeSeriesFileTests", 79, "PersistTest",
        //          QuoteLevel.Level2, 17, layerFlags: LayerFlags.TraderName | LayerFlags.SourceQuoteReference,
        //          lastTradedFlags: LastTradedFlags.None);
        // level3SrcTkrQtInfo =
        //     new SourceTickerQuoteInfo
        //         (19, "OneWeekDailyHourlyPriceQuoteTimeSeriesFileTests", 79, "PersistTest",
        //          QuoteLevel.Level3, 17, layerFlags: LayerFlags.TraderName | LayerFlags.SourceQuoteReference,
        //          lastTradedFlags: LastTradedFlags.TraderName | LastTradedFlags.PaidOrGiven);


        var dateToGenerate   = DateTime.UtcNow.Date;
        var currentDayOfWeek = dateToGenerate.DayOfWeek;
        var dayDiff          = DayOfWeek.Sunday - currentDayOfWeek;
        var startOfWeek      = dateToGenerate.AddDays(dayDiff);

        var generateQuoteInfo = new GenerateQuoteInfo(level0SrcTkrQtInfo);
        generateQuoteInfo.MidPriceGenerator!.StartTime  = startOfWeek;
        generateQuoteInfo.MidPriceGenerator!.StartPrice = 1.332211m;

        level0QuoteGenerator = new Level0QuoteGenerator(generateQuoteInfo);
        // level1QuoteGenerator = new Level1QuoteGenerator(generateQuoteInfo);
        // level2QuoteGenerator = new Level2QuoteGenerator(generateQuoteInfo);
        // level3QuoteGenerator = new Level3QuoteGenerator(generateQuoteInfo);
        CreateLevel0File();
    }

    private void CreateLevel0File(FileFlags fileFlags = FileFlags.WriterOpened | FileFlags.HasInternalIndexInHeader)
    {
        fileFlags |= FileFlags.WriterOpened | FileFlags.HasInternalIndexInHeader;

        var testTimeSeriesFilePath = Path.Combine(Environment.CurrentDirectory, GenerateUniqueFileNameOffDateTime());
        var timeSeriesFile         = new FileInfo(testTimeSeriesFilePath);
        if (timeSeriesFile.Exists) timeSeriesFile.Delete();
        var testFileNameResolver = new TestDirectoryFileNameResolver
        {
            TestTimeSeriesFile = timeSeriesFile
        };
        var createTestCreateFileParameters =
            new CreateFileParameters(testFileNameResolver,
                                     "TestInstrumentName", "TestSourceName",
                                     TimeSeriesPeriod.OneWeek, DateTime.UtcNow.Date, TimeSeriesEntryType.Price, 7,
                                     fileFlags, 6,
                                     category: "TestInstrumentCategory");
        var createPriceQuoteFile = new PriceQuoteCreateFileParameters(level0SrcTkrQtInfo, createTestCreateFileParameters);
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
    public void CreateNew_TwoLargeCompressedPeriods_OriginalValuesAreReturned()
    {
        var toPersistAndCheck
            = GenerateRepeatableL1QuoteStructs<ILevel0Quote, Level0PriceQuote>(1, 8000, 1, DayOfWeek.Wednesday, level0QuoteGenerator).ToList();
        toPersistAndCheck.AddRange(GenerateRepeatableL1QuoteStructs<ILevel0Quote, Level0PriceQuote>(1, 8000, 1, DayOfWeek.Thursday
                                                                                                  , level0QuoteGenerator));

        level0SessionWriter.Close();
        level0OneWeekFile.Close();

        CreateLevel0File(FileFlags.WriteDataCompressed);

        foreach (var firstPeriod in toPersistAndCheck)
        {
            var result = level0SessionWriter.AppendEntry(firstPeriod);
            Assert.AreEqual(StorageAttemptResult.PeriodRangeMatched, result);
        }
        level0OneWeekFile.AutoCloseOnZeroSessions = false;
        level0SessionWriter.Close();

        Assert.AreEqual((uint)toPersistAndCheck.Count, level0OneWeekFile.Header.TotalEntries);
        level0SessionReader = level0OneWeekFile.GetReaderSession();
        var allEntriesReader = level0SessionReader.GetAllEntriesReader(EntryResultSourcing.FromFactoryFuncUnlimited, asLevel0PriceQuoteFactory);
        var storedItems      = allEntriesReader.ResultEnumerable.ToList();
        Assert.AreEqual(toPersistAndCheck.Count, allEntriesReader.CountMatch);
        Assert.AreEqual(allEntriesReader.CountMatch, allEntriesReader.CountProcessed);
        Assert.AreEqual(toPersistAndCheck.Count, storedItems.Count);
        CompareExpectedToExtracted(toPersistAndCheck, storedItems);
    }

    [TestMethod]
    public void CreateNewCompressedDataFile_SavesEntriesCloseAndReopen_OriginalValuesAreReturned()
    {
        level0SessionWriter.Close();
        level0OneWeekFile.Close();

        CreateLevel0File(FileFlags.WriteDataCompressed);

        NewFile_SavesEntriesCloseAndReopen_OriginalValuesAreReturned(46916);
    }

    [TestMethod]
    public void CreateNewFile_SavesEntriesCloseAndReopen_OriginalValuesAreReturned()
    {
        NewFile_SavesEntriesCloseAndReopen_OriginalValuesAreReturned();
    }

    public void NewFile_SavesEntriesCloseAndReopen_OriginalValuesAreReturned(ulong? overrideExpectedFileSize = null, ulong tolerance = 30)
    {
        var toPersistAndCheck = GenerateForEachDayAndHourOfCurrentWeek<ILevel0Quote, Level0PriceQuote>(0, 10, level0QuoteGenerator).ToList();

        foreach (var level1QuoteStruct in toPersistAndCheck)
        {
            var result = level0SessionWriter.AppendEntry(level1QuoteStruct);
            Assert.AreEqual(StorageAttemptResult.PeriodRangeMatched, result);
        }
        level0OneWeekFile.AutoCloseOnZeroSessions = false;
        level0SessionWriter.Close();
        Assert.AreEqual((uint)toPersistAndCheck.Count, level0OneWeekFile.Header.TotalEntries);

        level0SessionReader = level0OneWeekFile.GetReaderSession();
        var allEntriesReader = level0SessionReader.GetAllEntriesReader(EntryResultSourcing.FromFactoryFuncUnlimited, asLevel0PriceQuoteFactory);
        var storedItems      = allEntriesReader.ResultEnumerable.ToList();
        Assert.AreEqual(toPersistAndCheck.Count, allEntriesReader.CountMatch);
        Assert.AreEqual(allEntriesReader.CountMatch, allEntriesReader.CountProcessed);
        Assert.AreEqual(toPersistAndCheck.Count, storedItems.Count);
        CompareExpectedToExtracted(toPersistAndCheck, storedItems);
        var newReaderSession = level0OneWeekFile.GetReaderSession();
        Assert.AreNotSame(level0SessionReader, newReaderSession);
        var newEntriesReader = level0SessionReader.GetAllEntriesReader(EntryResultSourcing.FromFactoryFuncUnlimited, asLevel0PriceQuoteFactory);
        newEntriesReader.ResultPublishFlags = ResultFlags.CopyToList;
        newEntriesReader.RunReader();
        var listResults = newEntriesReader.ResultList;
        Assert.AreEqual(toPersistAndCheck.Count, newEntriesReader.CountMatch);
        Assert.AreEqual(allEntriesReader.CountMatch, newEntriesReader.CountProcessed);
        Assert.AreEqual(toPersistAndCheck.Count, listResults.Count);
        Assert.IsTrue(toPersistAndCheck.SequenceEqual(listResults));
        newReaderSession.Close();
    }

    private void CompareExpectedToExtracted(List<ILevel0Quote> originalList, List<ILevel0Quote> toCompareList)
    {
        for (var i = 0; i < originalList.Count; i++)
        {
            var originalEntry = originalList[i];
            var compareEntry  = toCompareList[i];
            if (!Equals(originalEntry, compareEntry))
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
        Assert.AreEqual(TimeSeriesEntryType.Price, header.TimeSeriesEntryType);
        Assert.AreEqual(typeof(DailyToOneHourPQLevel0QuoteSubBuckets<ILevel0Quote>), header.BucketType);
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
        Assert.AreEqual(TimeSeriesEntryType.Price, header.TimeSeriesEntryType);
        Assert.AreEqual(typeof(DailyToOneHourPQLevel0QuoteSubBuckets<ILevel0Quote>), header.BucketType);
        Assert.AreEqual(typeof(ILevel0Quote), header.EntryType);
        Assert.AreEqual(typeof(WeeklyLevel0QuoteTimeSeriesFile), header.TimeSeriesFileType);
    }

    [TestMethod]
    public void CreateNewFile_BeyondFileTime_ReturnsFileRangeNotSupported()
    {
        Assert.AreEqual(TimeSeriesEntryType.Price, level0OneWeekFile.TimeSeriesEntryType);
        var singleQuoteMiddleOfWeek
            = GenerateRepeatableL1QuoteStructs<ILevel0Quote, Level0PriceQuote>(1, 1, 12, DayOfWeek.Wednesday, level0QuoteGenerator);
        var nextWeekQuote = (IMutableLevel0Quote)singleQuoteMiddleOfWeek.First();
        nextWeekQuote.SourceTime = nextWeekQuote.SourceTime.AddDays(7);
        var result = level0SessionWriter.AppendEntry(nextWeekQuote);
        Assert.AreEqual(StorageAttemptResult.NextFilePeriod, result);
    }

    [TestMethod]
    public void CreateNewFile_AfterBucketClosesOnNextEntry_TryingToAddExistingEntryReturnsBucketClosedForAppend()
    {
        var wednesdayQuotes = GenerateRepeatableL1QuoteStructs<ILevel0Quote, Level0PriceQuote>(1, 1, 12, DayOfWeek.Wednesday, level0QuoteGenerator);
        var thursdayQuotes  = GenerateRepeatableL1QuoteStructs<ILevel0Quote, Level0PriceQuote>(1, 1, 12, DayOfWeek.Thursday, level0QuoteGenerator);
        var wednesdayQuote  = wednesdayQuotes.First();
        var thursdayQuote   = thursdayQuotes.First();
        var result          = level0SessionWriter.AppendEntry(wednesdayQuote);
        Assert.AreEqual(StorageAttemptResult.PeriodRangeMatched, result);
        result = level0SessionWriter.AppendEntry(thursdayQuote);
        Assert.AreEqual(StorageAttemptResult.PeriodRangeMatched, result);
        result = level0SessionWriter.AppendEntry(wednesdayQuote);
        Assert.AreEqual(StorageAttemptResult.BucketClosedForAppend, result);
    }

    public IEnumerable<TQuoteLevel> GenerateForEachDayAndHourOfCurrentWeek<TQuoteLevel, TEntry>(int start, int amount
      , IQuoteGenerator<TEntry> quoteGenerator)
        where TQuoteLevel : ILevel0Quote where TEntry : class, IMutableLevel0Quote, TQuoteLevel
    {
        var dateToGenerate   = DateTime.UtcNow.Date;
        var currentDayOfWeek = dateToGenerate.DayOfWeek;
        var dayDiff          = DayOfWeek.Sunday - currentDayOfWeek;
        var startOfWeek      = dateToGenerate.AddDays(dayDiff);
        var currentDay       = startOfWeek;
        for (var i = 0; i < 7; i++)
        {
            for (var j = 0; j < 24; j++)
                foreach (var l1QuoteStruct in GenerateRepeatableL1QuoteStructs<TQuoteLevel, TEntry>(start, amount, j, currentDay.DayOfWeek
                                                                                                  , quoteGenerator))
                    yield return l1QuoteStruct;
            currentDay = currentDay.AddDays(1);
        }
    }

    private IEnumerable<TQuoteLevel> GenerateRepeatableL1QuoteStructs<TQuoteLevel, TEntry>(int start, int amount,
        int hour, DayOfWeek forDayOfWeek, IQuoteGenerator<TEntry> quoteGenerator)
        where TQuoteLevel : ILevel0Quote where TEntry : class, IMutableLevel0Quote, TQuoteLevel
    {
        var dateToGenerate   = DateTime.UtcNow.Date;
        var currentDayOfWeek = dateToGenerate.DayOfWeek;
        var dayDiff          = forDayOfWeek - currentDayOfWeek;
        var generateDate     = dateToGenerate.AddDays(dayDiff);
        var hourToGenerate   = TimeSpan.FromHours(hour);
        var generateDateHour = generateDate.Add(hourToGenerate);

        for (var i = start; i < start + amount; i++)
        {
            var startTime = generateDateHour + TimeSpan.FromMilliseconds(i);
            yield return quoteGenerator.Quotes(startTime, TimeSpan.FromMilliseconds(1), 1, i).First();
        }
    }

    private string GenerateUniqueFileNameOffDateTime()
    {
        var now = DateTime.Now;
        Interlocked.Increment(ref newTestCount);
        var nowString = now.ToString("yyyy-MM-dd_HH-mm-ss_fff");
        return "TimeSeriesFileTests_" + nowString + "_" + newTestCount + ".bin";
    }
}
