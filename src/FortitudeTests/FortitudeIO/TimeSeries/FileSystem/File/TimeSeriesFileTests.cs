// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory.UnmanagedMemory.MemoryMappedFiles;
using FortitudeCommon.Extensions;
using FortitudeCommon.Monitoring.Logging;
using FortitudeIO.TimeSeries;
using FortitudeIO.TimeSeries.FileSystem.File;
using FortitudeIO.TimeSeries.FileSystem.File.Buckets;
using FortitudeIO.TimeSeries.FileSystem.File.Reading;
using FortitudeMarketsApi.Pricing.Quotes;

#endregion

namespace FortitudeTests.FortitudeIO.TimeSeries.FileSystem.File;

[TestClass]
public class TimeSeriesFileTests
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(TimeSeriesFileTests));
    private static          int      newTestCount;

    private Func<Level1QuoteStruct>? createNewEntryFactory;
    private CreateFileParameters     createTestCreateFileParameters;

    private TimeSeriesFile<TestLevel1DailyQuoteStructBucket, Level1QuoteStruct>      oneWeekFile = null!;
    private IReaderFileSession<TestLevel1DailyQuoteStructBucket, Level1QuoteStruct>? readerSession;
    private TestDirectoryFileNameResolver                                            testFileNameResolver = null!;

    private string   testTimeSeriesFilePath = null!;
    private FileInfo timeSeriesFile         = null!;

    private IWriterFileSession<TestLevel1DailyQuoteStructBucket, Level1QuoteStruct> writerSession = null!;

    [TestInitialize]
    public void Setup()
    {
        PagedMemoryMappedFile.LogMappingMessages = true;
        CreateTimeSeriesFile();
    }

    private void CreateTimeSeriesFile(FileFlags fileFlags = FileFlags.WriterOpened | FileFlags.HasInternalIndexInHeader)
    {
        fileFlags |= FileFlags.WriterOpened | FileFlags.HasInternalIndexInHeader;

        testTimeSeriesFilePath = Path.Combine(Environment.CurrentDirectory, GenerateUniqueFileNameOffDateTime());
        timeSeriesFile         = new FileInfo(testTimeSeriesFilePath);
        createNewEntryFactory  = () => new Level1QuoteStruct(DateTimeConstants.UnixEpoch, 0m, DateTimeConstants.UnixEpoch, 0m, 0m, false);
        if (timeSeriesFile.Exists) timeSeriesFile.Delete();
        testFileNameResolver = new TestDirectoryFileNameResolver
        {
            TestTimeSeriesFile = timeSeriesFile
        };
        createTestCreateFileParameters =
            new CreateFileParameters(testFileNameResolver,
                                     "TestInstrumentName", "TestSourceName",
                                     TimeSeriesPeriod.OneWeek, DateTime.UtcNow.Date, TimeSeriesEntryType.Price, 7,
                                     fileFlags, 6,
                                     category: "TestInstrumentCategory");
        oneWeekFile   = new TimeSeriesFile<TestLevel1DailyQuoteStructBucket, Level1QuoteStruct>(createTestCreateFileParameters);
        writerSession = oneWeekFile.GetWriterSession()!;
    }

    [TestCleanup]
    public void TearDown()
    {
        try
        {
            readerSession?.Close();
            writerSession.Close();
            oneWeekFile.Close();
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
    public void CreateNew_TwoSmallCompressedPeriods_OriginalValuesAreReturned()
    {
        var toPersistAndCheck = GenerateRepeatableL1QuoteStructs(1, 10, 1, DayOfWeek.Wednesday).ToList();
        toPersistAndCheck.AddRange(GenerateRepeatableL1QuoteStructs(1, 10, 1, DayOfWeek.Thursday));

        writerSession.Close();
        oneWeekFile.Close();

        CreateTimeSeriesFile(FileFlags.WriteDataCompressed);

        foreach (var firstPeriod in toPersistAndCheck)
        {
            var result = writerSession.AppendEntry(firstPeriod);
            Assert.AreEqual(StorageAttemptResult.PeriodRangeMatched, result);
        }
        oneWeekFile.AutoCloseOnZeroSessions = false;
        writerSession.Close();

        Assert.AreEqual((uint)toPersistAndCheck.Count, oneWeekFile.Header.TotalEntries);
        readerSession = oneWeekFile.GetReaderSession();
        var allEntriesReader = readerSession.GetAllEntriesReader(createNewEntryFactory);
        var storedItems      = allEntriesReader.ResultEnumerable.ToList();
        Assert.AreEqual(toPersistAndCheck.Count, allEntriesReader.CountMatch);
        Assert.AreEqual(allEntriesReader.CountMatch, allEntriesReader.CountProcessed);
        Assert.AreEqual(toPersistAndCheck.Count, storedItems.Count);
        CompareExpectedToExtracted(toPersistAndCheck, storedItems);
    }

    // [TestMethod]
    public void LongRunningCreateNew_TwoLargeCompressedPeriods_OriginalValuesAreReturned()
    {
        var toPersistAndCheck = GenerateRepeatableL1QuoteStructs(1, 8000, 1, DayOfWeek.Wednesday).ToList();
        toPersistAndCheck.AddRange(GenerateRepeatableL1QuoteStructs(1, 8000, 1, DayOfWeek.Thursday));

        writerSession.Close();
        oneWeekFile.Close();

        CreateTimeSeriesFile(FileFlags.WriteDataCompressed);

        foreach (var firstPeriod in toPersistAndCheck)
        {
            var result = writerSession.AppendEntry(firstPeriod);
            Assert.AreEqual(StorageAttemptResult.PeriodRangeMatched, result);
        }
        oneWeekFile.AutoCloseOnZeroSessions = false;
        writerSession.Close();

        Assert.AreEqual((uint)toPersistAndCheck.Count, oneWeekFile.Header.TotalEntries);
        readerSession = oneWeekFile.GetReaderSession();
        var allEntriesReader = readerSession.GetAllEntriesReader(createNewEntryFactory);
        var storedItems      = allEntriesReader.ResultEnumerable.ToList();
        Assert.AreEqual(toPersistAndCheck.Count, allEntriesReader.CountMatch);
        Assert.AreEqual(allEntriesReader.CountMatch, allEntriesReader.CountProcessed);
        Assert.AreEqual(toPersistAndCheck.Count, storedItems.Count);
        CompareExpectedToExtracted(toPersistAndCheck, storedItems);
    }

    // [TestMethod]
    public void LongRunningCreateNewCompressedDataFile_SavesEntriesCloseAndReopen_OriginalValuesAreReturned()
    {
        writerSession.Close();
        oneWeekFile.Close();

        CreateTimeSeriesFile(FileFlags.WriteDataCompressed);

        NewFile_SavesEntriesCloseAndReopen_OriginalValuesAreReturned(46916);
    }

    [TestMethod]
    public void CreateNewFile_SavesEntriesCloseAndReopen_OriginalValuesAreReturned()
    {
        NewFile_SavesEntriesCloseAndReopen_OriginalValuesAreReturned();
    }

    public unsafe void NewFile_SavesEntriesCloseAndReopen_OriginalValuesAreReturned(ulong? overrideExpectedFileSize = null, ulong tolerance = 30)
    {
        var toPersistAndCheck = GenerateForEachDayAndHourOfCurrentWeek(0, 10).ToList();

        ulong expectedDataSize = 0;
        foreach (var level1QuoteStruct in toPersistAndCheck)
        {
            var result = writerSession.AppendEntry(level1QuoteStruct);
            expectedDataSize += (ulong)sizeof(Level1QuoteStruct);
            Assert.AreEqual(StorageAttemptResult.PeriodRangeMatched, result);
        }
        oneWeekFile.AutoCloseOnZeroSessions = false;
        writerSession.Close();
        if (overrideExpectedFileSize != null)
        {
            var lowerBound = overrideExpectedFileSize - tolerance;
            var upperBound = overrideExpectedFileSize + tolerance;
            Assert.IsTrue(oneWeekFile.Header.TotalFileDataSizeBytes > lowerBound && oneWeekFile.Header.TotalFileDataSizeBytes < upperBound,
                          $"oneWeekFile.Header.TotalFileDataSizeBytes was {oneWeekFile.Header.TotalFileDataSizeBytes} " +
                          $"and was expected to be between {lowerBound} and {upperBound}");
        }
        else
        {
            Assert.AreEqual(expectedDataSize, oneWeekFile.Header.TotalFileDataSizeBytes);
        }

        readerSession = oneWeekFile.GetReaderSession();
        var allEntriesReader = readerSession.GetAllEntriesReader(createNewEntryFactory);
        var storedItems      = allEntriesReader.ResultEnumerable.ToList();
        Assert.AreEqual(toPersistAndCheck.Count, allEntriesReader.CountMatch);
        Assert.AreEqual(allEntriesReader.CountMatch, allEntriesReader.CountProcessed);
        Assert.AreEqual(toPersistAndCheck.Count, storedItems.Count);
        CompareExpectedToExtracted(toPersistAndCheck, storedItems);
        var newReaderSession = oneWeekFile.GetReaderSession();
        Assert.AreNotSame(readerSession, newReaderSession);
        var newEntriesReader = readerSession.GetAllEntriesReader(createNewEntryFactory);
        newEntriesReader.ResultPublishFlags = ResultFlags.CopyToList;
        newEntriesReader.RunReader();
        var listResults = newEntriesReader.ResultList;
        Assert.AreEqual(toPersistAndCheck.Count, newEntriesReader.CountMatch);
        Assert.AreEqual(allEntriesReader.CountMatch, newEntriesReader.CountProcessed);
        Assert.AreEqual(toPersistAndCheck.Count, listResults.Count);
        Assert.IsTrue(toPersistAndCheck.SequenceEqual(listResults));
        newReaderSession.Close();
    }

    private void CompareExpectedToExtracted(List<Level1QuoteStruct> originalList, List<Level1QuoteStruct> toCompareList)
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
        var header      = oneWeekFile.Header;
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
        Assert.AreEqual(typeof(TestLevel1DailyQuoteStructBucket), header.BucketType);
        Assert.AreEqual(typeof(Level1QuoteStruct), header.EntryType);
        Assert.AreEqual(typeof(TimeSeriesFile<TestLevel1DailyQuoteStructBucket, Level1QuoteStruct>), header.TimeSeriesFileType);
        oneWeekFile.Close();
        oneWeekFile = (TimeSeriesFile<TestLevel1DailyQuoteStructBucket, Level1QuoteStruct>)
            TimeSeriesFile<TestLevel1DailyQuoteStructBucket, Level1QuoteStruct>.OpenExistingTimeSeriesFile(testTimeSeriesFilePath);
        header = oneWeekFile.Header;
        Assert.AreEqual(truncated, header.AnnotationFileRelativePath);
        Assert.AreEqual(truncated, header.ExternalIndexFileRelativePath);
        Assert.AreEqual(truncated, header.OriginSourceText);
        Assert.AreEqual(truncated, header.SourceName);
        Assert.AreEqual(truncated, header.Category);
        Assert.AreEqual(truncated, header.InstrumentName);
        Assert.AreEqual(TimeSeriesPeriod.OneWeek, header.FilePeriod);
        Assert.AreEqual(TimeSeriesPeriod.OneWeek.ContainingPeriodBoundaryStart(DateTime.UtcNow.Date), header.FileStartPeriod);
        Assert.AreEqual(TimeSeriesEntryType.Price, header.TimeSeriesEntryType);
        Assert.AreEqual(typeof(TestLevel1DailyQuoteStructBucket), header.BucketType);
        Assert.AreEqual(typeof(Level1QuoteStruct), header.EntryType);
        Assert.AreEqual(typeof(TimeSeriesFile<TestLevel1DailyQuoteStructBucket, Level1QuoteStruct>), header.TimeSeriesFileType);
    }

    [TestMethod]
    public void CreateNewFile_BeyondFileTime_ReturnsFileRangeNotSupported()
    {
        Assert.AreEqual(TimeSeriesEntryType.Price, oneWeekFile.TimeSeriesEntryType);
        var singleQuoteMiddleOfWeek = GenerateRepeatableL1QuoteStructs(1, 1, 12, DayOfWeek.Wednesday);
        var nextWeekQuote           = singleQuoteMiddleOfWeek.First();
        nextWeekQuote.SourceTime = nextWeekQuote.SourceTime.AddDays(7);
        var result = writerSession.AppendEntry(nextWeekQuote);
        Assert.AreEqual(StorageAttemptResult.NextFilePeriod, result);
    }

    [TestMethod]
    public void CreateNewFile_AfterBucketClosesOnNextEntry_TryingToAddExistingEntryReturnsBucketClosedForAppend()
    {
        var wednesdayQuotes = GenerateRepeatableL1QuoteStructs(1, 1, 12, DayOfWeek.Wednesday);
        var thursdayQuotes  = GenerateRepeatableL1QuoteStructs(1, 1, 12, DayOfWeek.Thursday);
        var wednesdayQuote  = wednesdayQuotes.First();
        var thursdayQuote   = thursdayQuotes.First();
        var result          = writerSession.AppendEntry(wednesdayQuote);
        Assert.AreEqual(StorageAttemptResult.PeriodRangeMatched, result);
        result = writerSession.AppendEntry(thursdayQuote);
        Assert.AreEqual(StorageAttemptResult.PeriodRangeMatched, result);
        result = writerSession.AppendEntry(wednesdayQuote);
        Assert.AreEqual(StorageAttemptResult.BucketClosedForAppend, result);
    }

    public IEnumerable<Level1QuoteStruct> GenerateForEachDayAndHourOfCurrentWeek(int start, int amount)
    {
        var dateToGenerate   = DateTime.UtcNow.Date;
        var currentDayOfWeek = dateToGenerate.DayOfWeek;
        var dayDiff          = DayOfWeek.Sunday - currentDayOfWeek;
        var startOfWeek      = dateToGenerate.AddDays(dayDiff);
        var currentDay       = startOfWeek;
        for (var i = 0; i < 7; i++)
        {
            for (var j = 0; j < 24; j++)
                foreach (var l1QuoteStruct in GenerateRepeatableL1QuoteStructs(start, amount, j, currentDay.DayOfWeek))
                    yield return l1QuoteStruct;
            currentDay = currentDay.AddDays(1);
        }
    }

    private IEnumerable<Level1QuoteStruct> GenerateRepeatableL1QuoteStructs(int start, int amount, int hour, DayOfWeek forDayOfWeek)
    {
        var dateToGenerate   = DateTime.UtcNow.Date;
        var currentDayOfWeek = dateToGenerate.DayOfWeek;
        var dayDiff          = forDayOfWeek - currentDayOfWeek;
        var generateDate     = dateToGenerate.AddDays(dayDiff);
        var hourToGenerate   = TimeSpan.FromHours(hour);
        var generateDateHour = generateDate.Add(hourToGenerate);

        for (var i = start; i < start + amount; i++)
            yield return new Level1QuoteStruct(generateDateHour + TimeSpan.FromMilliseconds(i), 1.2345m + i * 0.0001m,
                                               generateDateHour + TimeSpan.FromMilliseconds(i) + TimeSpan.FromMilliseconds(i),
                                               1.2345m - i * 0.0002m, 1.2345m + i * 0.0002m, true);
    }

    private string GenerateUniqueFileNameOffDateTime()
    {
        var now = DateTime.Now;
        Interlocked.Increment(ref newTestCount);
        var nowString = now.ToString("yyyy-MM-dd_HH-mm-ss_fff");
        return "TimeSeriesFileTests_" + nowString + "_" + newTestCount + ".bin";
    }
}
