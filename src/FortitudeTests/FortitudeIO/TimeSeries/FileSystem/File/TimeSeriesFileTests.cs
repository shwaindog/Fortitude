#region

using FortitudeCommon.Monitoring.Logging;
using FortitudeIO.TimeSeries;
using FortitudeIO.TimeSeries.FileSystem.File;
using FortitudeIO.TimeSeries.FileSystem.File.Buckets;
using FortitudeMarketsApi.Pricing.Quotes;

#endregion

namespace FortitudeTests.FortitudeIO.TimeSeries.FileSystem.File;

[TestClass]
public class TimeSeriesFileTests
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(TimeSeriesFileTests));
    private static int newTestCount;
    private FileInfo memoryMappedFile = null!;
    private string newMemoryMappedFilePath = null!;

    private TimeSeriesFile<TestLevel1DailyQuoteStructBucket, Level1QuoteStruct> oneWeekFile = null!;
    private IReaderFileSession<TestLevel1DailyQuoteStructBucket, Level1QuoteStruct>? readerSession;
    private IWriterFileSession<TestLevel1DailyQuoteStructBucket, Level1QuoteStruct> writerSession = null!;

    [TestInitialize]
    public void Setup()
    {
        newMemoryMappedFilePath = Path.Combine(Environment.CurrentDirectory, GenerateUniqueFileNameOffDateTime());
        memoryMappedFile = new FileInfo(newMemoryMappedFilePath);
        if (memoryMappedFile.Exists) memoryMappedFile.Delete();
        oneWeekFile = new TimeSeriesFile<TestLevel1DailyQuoteStructBucket, Level1QuoteStruct>(newMemoryMappedFilePath,
            TimeSeriesPeriod.OneWeek, DateTime.UtcNow.Date, 6,
            FileFlags.WriterOpened | FileFlags.HasInternalIndexInHeader, 7);
        writerSession = oneWeekFile.GetWriterSession()!;
    }

    [TestCleanup]
    public void TearDown()
    {
        readerSession?.Close();
        writerSession.Close();
        oneWeekFile.Close();
        var dirInfo = new DirectoryInfo(Environment.CurrentDirectory);
        foreach (var existingTimeSeriesFile in dirInfo.GetFiles("TimeSeriesFileTests_*"))
            try
            {
                existingTimeSeriesFile.Delete();
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine("Could not delete file {0}", existingTimeSeriesFile);
                FLoggerFactory.WaitUntilDrained();
            }
    }


    [TestMethod]
    public unsafe void CreateNewFile_SavesEntriesCloseAndReopen_OriginalValuesAreReturned()
    {
        var toPersistAndCheck = GenerateForEachDayAndHourOfCurrentWeek(0, 10).ToList();

        ulong expectedDataSize = 0;
        foreach (var level1QuoteStruct in toPersistAndCheck)
        {
            var result = writerSession.AppendEntry(level1QuoteStruct);
            expectedDataSize += (ulong)sizeof(Level1QuoteStruct);
            Assert.AreEqual(StorageAttemptResult.PeriodRangeMatched, result);
        }

        Assert.AreEqual(expectedDataSize, oneWeekFile.Header.TotalDataSizeBytes);

        writerSession.Close();
        readerSession = oneWeekFile.GetReaderSession();
        var storedItems = readerSession.AllEntries.ToList();
        Assert.AreEqual(toPersistAndCheck.Count, storedItems.Count);
        Assert.IsTrue(toPersistAndCheck.SequenceEqual(storedItems));
    }

    [TestMethod]
    public void CreateNewFile_BeyondFileTime_ReturnsFileRangeNotSupported()
    {
        var singleQuoteMiddleOfWeek = GenerateRepeatableL1QuoteStructs(1, 1, 12, DayOfWeek.Wednesday);
        var nextWeekQuote = singleQuoteMiddleOfWeek.First();
        nextWeekQuote.SourceTime = nextWeekQuote.SourceTime.AddDays(7);
        var result = writerSession.AppendEntry(nextWeekQuote);
        Assert.AreEqual(StorageAttemptResult.NextFilePeriod, result);
    }

    [TestMethod]
    public void CreateNewFile_AfterBucketClosesOnNextEntry_TryingToAddExistingEntryReturnsBucketClosedForAppend()
    {
        var wednesdayQuotes = GenerateRepeatableL1QuoteStructs(1, 1, 12, DayOfWeek.Wednesday);
        var thursdayQuotes = GenerateRepeatableL1QuoteStructs(1, 1, 12, DayOfWeek.Thursday);
        var wednesdayQuote = wednesdayQuotes.First();
        var thursdayQuote = thursdayQuotes.First();
        var result = writerSession.AppendEntry(wednesdayQuote);
        Assert.AreEqual(StorageAttemptResult.PeriodRangeMatched, result);
        result = writerSession.AppendEntry(thursdayQuote);
        Assert.AreEqual(StorageAttemptResult.PeriodRangeMatched, result);
        result = writerSession.AppendEntry(wednesdayQuote);
        Assert.AreEqual(StorageAttemptResult.BucketClosedForAppend, result);
    }

    public IEnumerable<Level1QuoteStruct> GenerateForEachDayAndHourOfCurrentWeek(int start, int amount)
    {
        var dateToGenerate = DateTime.UtcNow.Date;
        var currentDayOfWeek = dateToGenerate.DayOfWeek;
        var dayDiff = DayOfWeek.Sunday - currentDayOfWeek;
        var startOfWeek = dateToGenerate.AddDays(dayDiff);
        var currentDay = startOfWeek;
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
        var dateToGenerate = DateTime.UtcNow.Date;
        var currentDayOfWeek = dateToGenerate.DayOfWeek;
        var dayDiff = forDayOfWeek - currentDayOfWeek;
        var generateDate = dateToGenerate.AddDays(dayDiff);
        var hourToGenerate = TimeSpan.FromHours(hour);
        var generateDateHour = generateDate.Add(hourToGenerate);

        for (var i = start; i < start + amount; i++)
            yield return new Level1QuoteStruct(generateDateHour + TimeSpan.FromSeconds(i), 1.2345m + i * 0.0001m,
                generateDateHour + TimeSpan.FromSeconds(i) + TimeSpan.FromMilliseconds(i),
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
