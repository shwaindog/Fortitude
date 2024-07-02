// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
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
using FortitudeMarketsApi.Pricing.Quotes.LayeredBook;
using FortitudeMarketsApi.Pricing.Summaries;
using FortitudeMarketsCore.Pricing.Generators.Summaries;
using FortitudeMarketsCore.Pricing.PQ.Generators.Summaries;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.SourceTickerInfo;
using FortitudeMarketsCore.Pricing.PQ.Summaries;
using FortitudeMarketsCore.Pricing.PQ.TimeSeries.FileSystem.File;
using FortitudeMarketsCore.Pricing.Summaries;
using static FortitudeMarketsApi.Configuration.ClientServerConfig.MarketClassificationExtensions;
using static FortitudeMarketsApi.Pricing.Quotes.QuoteLevel;
using static FortitudeTests.FortitudeMarketsCore.Pricing.PQ.TimeSeries.FileSystem.File.TestWeeklyDataGeneratorFixture;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.TimeSeries.FileSystem.File;

[TestClass]
public class PriceSummaryTimeSeriesFileTests
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(PriceSummaryTimeSeriesFileTests));

    private readonly Func<IPricePeriodSummary> asPQPricePeriodSummaryFactory = () => new PQPricePeriodSummary();
    private readonly Func<IPricePeriodSummary> asPricePeriodSummaryFactory   = () => new PricePeriodSummary();

    private Func<IPricePeriodSummary> asPQPriceStoragePeriodSummaryFactory = null!;

    private PQPriceStoragePeriodSummaryGenerator pqPriceStorageSummaryGenerator = null!;
    private PQPricePeriodSummaryGenerator        pqPriceSummaryGenerator        = null!;

    private ITimeSeriesEntryFile<IPricePeriodSummary> priceSummaryFile      = null!;
    private PricePeriodSummaryGenerator               priceSummaryGenerator = null!;

    private IReaderSession<IPricePeriodSummary>? sessionReader;
    private IWriterSession<IPricePeriodSummary>  sessionWriter = null!;

    private SourceTickerQuoteInfo srcTkrQtInfo = null!;

    private DateTime startOfWeek;


    [TestInitialize]
    public void Setup()
    {
        PagedMemoryMappedFile.LogMappingMessages = true;
        srcTkrQtInfo =
            new SourceTickerQuoteInfo
                (19, "PriceSummaryTimeSeriesFileTests", 79, "PriceSummaryTests", Level0, Unknown
               , 1, 0.1m, 10_000m, 100_000_000m, 10_000m, 1, LayerFlags.None);


        asPQPriceStoragePeriodSummaryFactory = () => new PQPriceStoragePeriodSummary(new PQSourceTickerQuoteInfo(srcTkrQtInfo));
        var dateToGenerate   = DateTime.UtcNow.Date.TruncToMonthBoundary().AddDays(15);
        var currentDayOfWeek = dateToGenerate.DayOfWeek;
        var dayDiff          = DayOfWeek.Sunday - currentDayOfWeek;
        startOfWeek = dateToGenerate.AddDays(dayDiff);

        var generateQuoteInfo = new GeneratePriceSummariesInfo(srcTkrQtInfo);
        generateQuoteInfo.MidPriceGenerator!.StartTime            = startOfWeek;
        generateQuoteInfo.MidPriceGenerator!.StartPrice           = 200_042m;
        generateQuoteInfo.MidPriceGenerator!.RoundAtDecimalPlaces = 1;
        generateQuoteInfo.MinimumVolume                           = 10_000;

        priceSummaryGenerator          = new PricePeriodSummaryGenerator(generateQuoteInfo);
        pqPriceSummaryGenerator        = new PQPricePeriodSummaryGenerator(generateQuoteInfo);
        pqPriceStorageSummaryGenerator = new PQPriceStoragePeriodSummaryGenerator(generateQuoteInfo);
    }

    private PriceTimeSeriesFileParameters CreatePriceSummaryFileParameters
    (FileFlags fileFlags = FileFlags.WriterOpened | FileFlags.HasInternalIndexInHeader, TimeSeriesPeriod filePeriod = TimeSeriesPeriod.OneWeek,
        TimeSeriesPeriod entryPeriod = TimeSeriesPeriod.OneSecond, uint internalIndexSize = 7)
    {
        fileFlags |= FileFlags.WriterOpened | FileFlags.HasInternalIndexInHeader;

        var testTimeSeriesFilePath = Path.Combine(Environment.CurrentDirectory, GenerateUniqueFileNameOffDateTime());
        var timeSeriesFile         = new FileInfo(testTimeSeriesFilePath);
        if (timeSeriesFile.Exists) timeSeriesFile.Delete();
        var fileStartTime =
            filePeriod switch
            {
                TimeSeriesPeriod.OneMonth  => startOfWeek.TruncToFirstSundayInMonth()
              , TimeSeriesPeriod.OneYear   => startOfWeek.TruncToFirstSundayInYear()
              , TimeSeriesPeriod.OneDecade => startOfWeek.TruncToFirstSundayInDecade()
              , TimeSeriesPeriod.None      => DateTimeConstants.UnixEpoch
              , _                          => startOfWeek
            };

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
               , new Instrument("TestInstrumentName", InstrumentType.PriceSummaryPeriod, entryPeriod, instrumentFields, optionalInstrumentFields)
               , filePeriod, fileStartTime, internalIndexSize, fileFlags);
        return new PriceTimeSeriesFileParameters(srcTkrQtInfo, createTestCreateFileParameters);
    }

    [TestCleanup]
    public void TearDown()
    {
        try
        {
            sessionReader?.Close();
            sessionWriter.Close();
            priceSummaryFile.Close();
        }
        catch (Exception ex)
        {
            Console.Out.WriteLine("Could not close all sessions. Got {0}", ex);
        }
        var dirInfo = new DirectoryInfo(Environment.CurrentDirectory);
        DeleteTestFiles(dirInfo);
    }

    [TestMethod]
    public void WeeklyDailyHourlyFile15sSummaryPeriods_TwoSmallPeriods_OriginalValuesAreReturned()
    {
        var createParams = CreatePriceSummaryFileParameters();
        priceSummaryFile = new WeeklyDailyHourlyPriceSummaryTimeSeriesFile(createParams);
        CreateNewTyped_TwoSmallPeriods_OriginalValuesAreReturned
            (startOfWeek.AddDays(1), startOfWeek.AddDays(3), TimeSeriesPeriod.FifteenSeconds
           , priceSummaryGenerator, asPricePeriodSummaryFactory);
    }

    [TestMethod]
    public void WeeklyFourHourlyFile30sPQSummaryPeriods_TwoSmallPeriods_OriginalValuesAreReturned()
    {
        var createParams = CreatePriceSummaryFileParameters(entryPeriod: TimeSeriesPeriod.ThirtySeconds, internalIndexSize: 42);
        priceSummaryFile = new WeeklyFourHourlyPriceSummaryTimeSeriesFile(createParams);
        CreateNewTyped_TwoSmallPeriods_OriginalValuesAreReturned
            (startOfWeek.AddDays(1), startOfWeek.AddDays(3), TimeSeriesPeriod.ThirtySeconds
           , pqPriceSummaryGenerator, asPQPricePeriodSummaryFactory);
    }

    [TestMethod]
    public void MonthlyFourHourlyFile1mPQStorageSummaryPeriods_TwoSmallPeriods_OriginalValuesAreReturned()
    {
        var createParams = CreatePriceSummaryFileParameters
            (entryPeriod: TimeSeriesPeriod.OneMinute, internalIndexSize: 186, filePeriod: TimeSeriesPeriod.OneMonth);
        priceSummaryFile = new MonthlyFourHourlyPriceSummaryTimeSeriesFile(createParams);
        CreateNewTyped_TwoSmallPeriods_OriginalValuesAreReturned
            (startOfWeek.AddDays(1), startOfWeek.AddDays(3), TimeSeriesPeriod.OneMinute
           , pqPriceStorageSummaryGenerator, asPQPriceStoragePeriodSummaryFactory);
    }

    [TestMethod]
    public void MonthlyDailyHourlyFile5mSummaryPeriods_TwoSmallPeriods_OriginalValuesAreReturned()
    {
        var createParams = CreatePriceSummaryFileParameters
            (entryPeriod: TimeSeriesPeriod.FiveMinutes, internalIndexSize: 31, filePeriod: TimeSeriesPeriod.OneMonth);
        priceSummaryFile = new MonthlyDailyHourlyPriceSummaryTimeSeriesFile(createParams);
        CreateNewTyped_TwoSmallPeriods_OriginalValuesAreReturned
            (startOfWeek.AddDays(1), startOfWeek.AddDays(3), TimeSeriesPeriod.FiveMinutes
           , priceSummaryGenerator, asPricePeriodSummaryFactory);
    }

    [TestMethod]
    public void MonthlyWeeklyFourHourlyFile10mPQSummaryPeriods_TwoSmallPeriods_OriginalValuesAreReturned()
    {
        var createParams = CreatePriceSummaryFileParameters
            (entryPeriod: TimeSeriesPeriod.TenMinutes, internalIndexSize: 4, filePeriod: TimeSeriesPeriod.OneMonth);
        priceSummaryFile = new MonthlyWeeklyFourHourlyPriceSummaryTimeSeriesFile(createParams);
        CreateNewTyped_TwoSmallPeriods_OriginalValuesAreReturned
            (startOfWeek.TruncToMonthBoundary().AddDays(10), startOfWeek.TruncToMonthBoundary().AddDays(20), TimeSeriesPeriod.TenMinutes
           , pqPriceSummaryGenerator, asPQPricePeriodSummaryFactory);
    }

    [TestMethod]
    public void MonthlyWeeklyDailyFile15mPQStorageSummaryPeriods_TwoSmallPeriods_OriginalValuesAreReturned()
    {
        var createParams = CreatePriceSummaryFileParameters
            (entryPeriod: TimeSeriesPeriod.FifteenMinutes, internalIndexSize: 4, filePeriod: TimeSeriesPeriod.OneMonth);
        priceSummaryFile = new MonthlyWeeklyDailyPriceSummaryTimeSeriesFile(createParams);
        CreateNewTyped_TwoSmallPeriods_OriginalValuesAreReturned
            (startOfWeek.TruncToMonthBoundary().AddDays(10), startOfWeek.TruncToMonthBoundary().AddDays(20)
           , TimeSeriesPeriod.FifteenMinutes, pqPriceStorageSummaryGenerator, asPQPriceStoragePeriodSummaryFactory);
    }

    [TestMethod]
    public void YearlyMonthlyFourHourlyFile10mSummaryPeriods_TwoSmallPeriods_OriginalValuesAreReturned()
    {
        var createParams = CreatePriceSummaryFileParameters
            (entryPeriod: TimeSeriesPeriod.TenMinutes, internalIndexSize: 12, filePeriod: TimeSeriesPeriod.OneYear);
        priceSummaryFile = new YearlyMonthlyFourHourlyPriceSummaryTimeSeriesFile(createParams);
        CreateNewTyped_TwoSmallPeriods_OriginalValuesAreReturned
            (startOfWeek.TruncToFirstSundayInYear().AddDays(15), startOfWeek.TruncToFirstSundayInYear().AddDays(45)
           , TimeSeriesPeriod.TenMinutes, priceSummaryGenerator, asPricePeriodSummaryFactory);
    }

    [TestMethod]
    public void YearlyWeeklyDailyFile1hSummaryPeriods_TwoSmallPeriods_OriginalValuesAreReturned()
    {
        var createParams = CreatePriceSummaryFileParameters
            (entryPeriod: TimeSeriesPeriod.OneHour, internalIndexSize: 53, filePeriod: TimeSeriesPeriod.OneYear);
        priceSummaryFile = new YearlyWeeklyDailyPriceSummaryTimeSeriesFile(createParams);
        CreateNewTyped_TwoSmallPeriods_OriginalValuesAreReturned
            (startOfWeek.TruncToFirstSundayInYear().AddDays(15), startOfWeek.TruncToFirstSundayInYear().AddDays(45)
           , TimeSeriesPeriod.OneHour, pqPriceSummaryGenerator, asPQPricePeriodSummaryFactory);
    }

    [TestMethod]
    public void YearlyMonthlyWeeklyFile4hSummaryPeriods_TwoSmallPeriods_OriginalValuesAreReturned()
    {
        var createParams = CreatePriceSummaryFileParameters
            (entryPeriod: TimeSeriesPeriod.FourHours, internalIndexSize: 12, filePeriod: TimeSeriesPeriod.OneYear);
        priceSummaryFile = new YearlyMonthlyWeeklyPriceSummaryTimeSeriesFile(createParams);
        CreateNewTyped_TwoSmallPeriods_OriginalValuesAreReturned
            (startOfWeek.TruncToFirstSundayInYear().AddDays(15), startOfWeek.TruncToFirstSundayInYear().AddDays(45)
           , TimeSeriesPeriod.FourHours, pqPriceStorageSummaryGenerator, asPQPriceStoragePeriodSummaryFactory);
    }

    [TestMethod]
    public void YearlyMonthlyWeeklyFile30mSummaryPeriods_TwoSmallPeriods_OriginalValuesAreReturned()
    {
        var createParams = CreatePriceSummaryFileParameters
            (entryPeriod: TimeSeriesPeriod.ThirtyMinutes, internalIndexSize: 12, filePeriod: TimeSeriesPeriod.OneYear);
        priceSummaryFile = new DecenniallyMonthlyWeeklyPriceSummaryTimeSeriesFile(createParams);
        CreateNewTyped_TwoSmallPeriods_OriginalValuesAreReturned
            (startOfWeek.TruncToFirstSundayInYear().AddDays(15), startOfWeek.TruncToFirstSundayInYear().AddDays(45)
           , TimeSeriesPeriod.ThirtyMinutes, priceSummaryGenerator, asPricePeriodSummaryFactory);
    }

    // [TestMethod]
    public void LongRunningDecenniallyYearlyMonthlyFile1dSummaryPeriods_TwoSmallPeriods_OriginalValuesAreReturned()
    {
        var createParams = CreatePriceSummaryFileParameters
            (entryPeriod: TimeSeriesPeriod.OneDay, internalIndexSize: 10, filePeriod: TimeSeriesPeriod.OneYear);
        priceSummaryFile = new DecenniallyYearlyMonthlyPriceSummaryTimeSeriesFile(createParams);
        CreateNewTyped_TwoSmallPeriods_OriginalValuesAreReturned
            (startOfWeek.TruncToFirstSundayInYear().AddDays(15), startOfWeek.TruncToFirstSundayInYear().AddDays(45)
           , TimeSeriesPeriod.OneDay, pqPriceSummaryGenerator, asPQPricePeriodSummaryFactory);
    }

    // [TestMethod]
    public void LongRunningUnlimitedDecenniallyYearlyFile1WSummaryPeriods_TwoSmallPeriods_OriginalValuesAreReturned()
    {
        var createParams = CreatePriceSummaryFileParameters
            (entryPeriod: TimeSeriesPeriod.OneWeek, internalIndexSize: 10, filePeriod: TimeSeriesPeriod.None);
        priceSummaryFile = new UnlimitedDecenniallyYearlyPriceSummaryTimeSeriesFile(createParams);
        CreateNewTyped_TwoSmallPeriods_OriginalValuesAreReturned
            (startOfWeek.AddDays(15), startOfWeek.AddDays(400).AddYears(5)
           , TimeSeriesPeriod.OneWeek, pqPriceStorageSummaryGenerator, asPQPriceStoragePeriodSummaryFactory);
    }

    // [TestMethod]
    public void LongRunningUnlimitedDecenniallyFile1MSummaryPeriods_TwoSmallPeriods_OriginalValuesAreReturned()
    {
        var createParams = CreatePriceSummaryFileParameters
            (entryPeriod: TimeSeriesPeriod.OneMonth, internalIndexSize: 50, filePeriod: TimeSeriesPeriod.None);
        priceSummaryFile = new UnlimitedDecenniallyPriceSummaryTimeSeriesFile(createParams);
        CreateNewTyped_TwoSmallPeriods_OriginalValuesAreReturned
            (startOfWeek.AddDays(15), startOfWeek.AddDays(400).AddYears(5)
           , TimeSeriesPeriod.OneMonth, priceSummaryGenerator, asPricePeriodSummaryFactory);
    }

    // [TestMethod]
    public void LongRunningUnlimitedFile1DecadeSummaryPeriods_TwoSmallPeriods_OriginalValuesAreReturned()
    {
        var createParams = CreatePriceSummaryFileParameters
            (entryPeriod: TimeSeriesPeriod.OneDecade, internalIndexSize: 1, filePeriod: TimeSeriesPeriod.None);
        priceSummaryFile = new UnlimitedPriceSummaryTimeSeriesFile(createParams);
        CreateNewTyped_TwoSmallPeriods_OriginalValuesAreReturned
            (startOfWeek.AddDays(15), startOfWeek.AddDays(3600).AddYears(5)
           , TimeSeriesPeriod.OneDecade, priceSummaryGenerator, asPricePeriodSummaryFactory);
    }

    public void CreateNewTyped_TwoSmallPeriods_OriginalValuesAreReturned<TEntry>
    (DateTime firstPeriodSummaryTime, DateTime secondPeriodSummaryTime, TimeSeriesPeriod summaryPeriod
      , IPricePeriodSummaryGenerator<TEntry> quoteGenerator, Func<IPricePeriodSummary> retrievalFactory)
        where TEntry : class, IMutablePricePeriodSummary
    {
        var toPersistAndCheck = GenerateRepeatablePriceSummaries
            (1, 10, 1, DayOfWeek.Wednesday, summaryPeriod, quoteGenerator, firstPeriodSummaryTime).ToList();
        toPersistAndCheck.AddRange
            (GenerateRepeatablePriceSummaries
                (1, 10, 1, DayOfWeek.Thursday, summaryPeriod, quoteGenerator, secondPeriodSummaryTime));


        sessionWriter = priceSummaryFile.GetWriterSession()!;
        foreach (var firstPeriod in toPersistAndCheck)
        {
            var result = sessionWriter.AppendEntry(firstPeriod);
            Assert.AreEqual(StorageAttemptResult.PeriodRangeMatched, result.StorageAttemptResult);
        }
        priceSummaryFile.AutoCloseOnZeroSessions = false;
        sessionWriter.Close();

        Assert.AreEqual((uint)toPersistAndCheck.Count, priceSummaryFile.Header.TotalEntries);
        sessionReader = priceSummaryFile.GetReaderSession();
        var allEntriesReader = sessionReader.GetAllEntriesReader(EntryResultSourcing.FromFactoryFuncUnlimited, retrievalFactory);
        var storedItems      = allEntriesReader.ResultEnumerable.ToList();
        Assert.AreEqual(toPersistAndCheck.Count, allEntriesReader.CountMatch);
        Assert.AreEqual(allEntriesReader.CountMatch, allEntriesReader.CountProcessed);
        Assert.AreEqual(toPersistAndCheck.Count, storedItems.Count);
        CompareExpectedToExtracted(toPersistAndCheck, storedItems);
    }

    private void CompareExpectedToExtracted(List<IPricePeriodSummary> originalList, List<IPricePeriodSummary> toCompareList)
    {
        for (var i = 0; i < originalList.Count; i++)
        {
            var originalEntry = originalList[i];
            ((IMutablePricePeriodSummary)originalEntry).PeriodSummaryFlags |= PricePeriodSummaryFlags.FromStorage;
            var compareEntry = toCompareList[i];
            if (!originalEntry.AreEquivalent(compareEntry))
            {
                Logger.Warn("Entries at {0} differ test failed \noriginal {1}\nretrieved {2} ", i, originalEntry, compareEntry);
                FLoggerFactory.WaitUntilDrained();
                Assert.Fail($"Entries at {i} differ test failed \noriginal {originalEntry}\nretrieved {compareEntry}.");
            }
        }
    }
}
