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
using FortitudeIO.TimeSeries.FileSystem.Session.Retrieval;
using FortitudeMarkets.Pricing.Generators.Summaries;
using FortitudeMarkets.Pricing.PQ.Generators.Summaries;
using FortitudeMarkets.Pricing.PQ.Summaries;
using FortitudeMarkets.Pricing.PQ.TimeSeries.FileSystem.File;
using FortitudeMarkets.Pricing.Quotes;
using FortitudeMarkets.Pricing.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.Quotes.TickerInfo;
using FortitudeMarkets.Pricing.Summaries;
using static FortitudeMarkets.Configuration.ClientServerConfig.MarketClassificationExtensions;
using static FortitudeMarkets.Pricing.Quotes.TickerInfo.TickerDetailLevel;
using static FortitudeTests.FortitudeMarkets.Pricing.PQ.TimeSeries.FileSystem.File.TestWeeklyDataGeneratorFixture;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.TimeSeries.FileSystem.File;

[TestClass]
public class PriceSummaryTimeSeriesFileTests
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(PriceSummaryTimeSeriesFileTests));

    private readonly Func<PricePeriodSummary>   asDtoPricePeriodSummaryFactory = () => new PricePeriodSummary();
    private readonly Func<PQPricePeriodSummary> asPQPricePeriodSummaryFactory  = () => new PQPricePeriodSummary();

    private readonly Func<IPricePeriodSummary> asPricePeriodSummaryFactory = () => new PricePeriodSummary();

    private Func<PQPriceStoragePeriodSummary> asPQPriceStoragePeriodSummaryFactory = null!;

    private PQPriceStoragePeriodSummaryGenerator pqPriceStorageSummaryGenerator = null!;
    private PQPricePeriodSummaryGenerator        pqPriceSummaryGenerator        = null!;

    private PricePeriodSummaryGenerator priceSummaryGenerator = null!;

    private SourceTickerInfo srcTkrInfo = null!;

    private DateTime startOfWeek;


    [TestInitialize]
    public void Setup()
    {
        PagedMemoryMappedFile.LogMappingMessages = true;
        srcTkrInfo =
            new SourceTickerInfo
                (19, "PriceSummaryTimeSeriesFileTests", 79, "PriceSummaryTests", SingleValue, Unknown
               , 1, 0.1m, 1m, 10_000m, 100_000_000m, 10_000m, 1, 10_000
               , true, false, LayerFlags.None);


        asPQPriceStoragePeriodSummaryFactory = () => new PQPriceStoragePeriodSummary();
        var dateToGenerate   = DateTime.UtcNow.Date.TruncToMonthBoundary().AddDays(15);
        var currentDayOfWeek = dateToGenerate.DayOfWeek;
        var dayDiff          = DayOfWeek.Sunday - currentDayOfWeek;
        startOfWeek = dateToGenerate.AddDays(dayDiff);

        var generateQuoteInfo = new GeneratePriceSummariesInfo(srcTkrInfo);
        generateQuoteInfo.MidPriceGenerator!.StartTime            = startOfWeek;
        generateQuoteInfo.MidPriceGenerator!.StartPrice           = 200_042m;
        generateQuoteInfo.MidPriceGenerator!.RoundAtDecimalPlaces = 1;
        generateQuoteInfo.MinimumVolume                           = 10_000;

        priceSummaryGenerator          = new PricePeriodSummaryGenerator(generateQuoteInfo);
        pqPriceSummaryGenerator        = new PQPricePeriodSummaryGenerator(generateQuoteInfo);
        pqPriceStorageSummaryGenerator = new PQPriceStoragePeriodSummaryGenerator(generateQuoteInfo);
    }

    private PriceTimeSeriesFileParameters CreatePriceSummaryFileParameters
    (FileFlags fileFlags = FileFlags.WriterOpened | FileFlags.HasInternalIndexInHeader, TimeBoundaryPeriod filePeriod = TimeBoundaryPeriod.OneWeek,
        TimeBoundaryPeriod entryCoveringPeriod = TimeBoundaryPeriod.OneSecond, uint internalIndexSize = 7)
    {
        fileFlags |= FileFlags.WriterOpened | FileFlags.HasInternalIndexInHeader;

        var timeSeriesFile = GenerateUniqueFileNameOffDateTime();
        if (timeSeriesFile.Exists) timeSeriesFile.Delete();
        var fileStartTime =
            filePeriod switch
            {
                TimeBoundaryPeriod.OneWeek   => startOfWeek
              , TimeBoundaryPeriod.OneMonth  => startOfWeek.TruncToFirstSundayInMonth()
              , TimeBoundaryPeriod.OneYear   => startOfWeek.TruncToFirstSundayInYear()
              , TimeBoundaryPeriod.OneDecade => startOfWeek.TruncToFirstSundayInDecade()
              , _                            => startOfWeek
            };

        var instrumentFields = new Dictionary<string, string>
        {
            { nameof(RepositoryPathName.MarketType), "Unknown" }, { nameof(RepositoryPathName.MarketProductType), "Unknown" }
          , { nameof(RepositoryPathName.MarketRegion), "Unknown" }
        };
        var optionalInstrumentFields = new Dictionary<string, string>
        {
            { nameof(RepositoryPathName.Category), "TestInstrumentCategory" }
        };

        var createTestCreateFileParameters =
            new TimeSeriesFileParameters
                (timeSeriesFile
               , new Instrument("TestInstrumentName", "TestSourceName", InstrumentType.PriceSummaryPeriod, new DiscreetTimePeriod(entryCoveringPeriod)
                              , instrumentFields
                              , optionalInstrumentFields)
               , filePeriod, fileStartTime, internalIndexSize, fileFlags);
        return new PriceTimeSeriesFileParameters(srcTkrInfo, createTestCreateFileParameters);
    }

    [TestCleanup]
    public void TearDown()
    {
        DeleteTestFiles();
    }

    [TestMethod]
    public void WeeklyDailyHourlyFile15sSummaryPeriods_TwoSmallPeriods_OriginalValuesAreReturned()
    {
        var       createParams     = CreatePriceSummaryFileParameters();
        using var priceSummaryFile = new WeeklyDailyHourlyPriceSummaryTimeSeriesFile<IPricePeriodSummary>(createParams);
        CreateNewTyped_TwoSmallPeriods_OriginalValuesAreReturned
            (priceSummaryFile, startOfWeek.AddDays(1), startOfWeek.AddDays(3), TimeBoundaryPeriod.FifteenSeconds
           , priceSummaryGenerator, asPricePeriodSummaryFactory);
    }

    [TestMethod]
    public void WeeklyFourHourlyFile30sPQSummaryPeriods_TwoSmallPeriods_OriginalValuesAreReturned()
    {
        var       createParams       = CreatePriceSummaryFileParameters(entryCoveringPeriod: TimeBoundaryPeriod.ThirtySeconds, internalIndexSize: 42);
        using var pqPriceSummaryFile = new WeeklyFourHourlyPriceSummaryTimeSeriesFile<PQPricePeriodSummary>(createParams);
        CreateNewTyped_TwoSmallPeriods_OriginalValuesAreReturned
            (pqPriceSummaryFile, startOfWeek.AddDays(1), startOfWeek.AddDays(3), TimeBoundaryPeriod.ThirtySeconds
           , pqPriceSummaryGenerator, asPQPricePeriodSummaryFactory);
    }

    [TestMethod]
    public void MonthlyFourHourlyFile1mPQStorageSummaryPeriods_TwoSmallPeriods_OriginalValuesAreReturned()
    {
        var createParams = CreatePriceSummaryFileParameters
            (entryCoveringPeriod: TimeBoundaryPeriod.OneMinute, internalIndexSize: 186, filePeriod: TimeBoundaryPeriod.OneMonth);
        using var pqPriceStorageSummaryFile = new MonthlyFourHourlyPriceSummaryTimeSeriesFile<PQPriceStoragePeriodSummary>(createParams);
        CreateNewTyped_TwoSmallPeriods_OriginalValuesAreReturned
            (pqPriceStorageSummaryFile, startOfWeek.AddDays(1), startOfWeek.AddDays(3), TimeBoundaryPeriod.OneMinute
           , pqPriceStorageSummaryGenerator, asPQPriceStoragePeriodSummaryFactory);
    }

    [TestMethod]
    public void MonthlyDailyHourlyFile5mSummaryPeriods_TwoSmallPeriods_OriginalValuesAreReturned()
    {
        var createParams = CreatePriceSummaryFileParameters
            (entryCoveringPeriod: TimeBoundaryPeriod.FiveMinutes, internalIndexSize: 31, filePeriod: TimeBoundaryPeriod.OneMonth);
        using var dtoPriceSummaryFile = new MonthlyDailyHourlyPriceSummaryTimeSeriesFile<PricePeriodSummary>(createParams);
        CreateNewTyped_TwoSmallPeriods_OriginalValuesAreReturned
            (dtoPriceSummaryFile, startOfWeek.AddDays(1), startOfWeek.AddDays(3), TimeBoundaryPeriod.FiveMinutes
           , priceSummaryGenerator, asDtoPricePeriodSummaryFactory);
    }

    [TestMethod]
    public void MonthlyWeeklyFourHourlyFile10mPQSummaryPeriods_TwoSmallPeriods_OriginalValuesAreReturned()
    {
        var createParams = CreatePriceSummaryFileParameters
            (entryCoveringPeriod: TimeBoundaryPeriod.TenMinutes, internalIndexSize: 4, filePeriod: TimeBoundaryPeriod.OneMonth);
        using var pqPriceSummaryFile = new MonthlyWeeklyFourHourlyPriceSummaryTimeSeriesFile<PQPricePeriodSummary>(createParams);
        CreateNewTyped_TwoSmallPeriods_OriginalValuesAreReturned
            (pqPriceSummaryFile, startOfWeek.TruncToMonthBoundary().AddDays(10), startOfWeek.TruncToMonthBoundary().AddDays(20)
           , TimeBoundaryPeriod.TenMinutes
           , pqPriceSummaryGenerator, asPQPricePeriodSummaryFactory);
    }

    [TestMethod]
    public void MonthlyWeeklyDailyFile15mPQStorageSummaryPeriods_TwoSmallPeriods_OriginalValuesAreReturned()
    {
        var createParams = CreatePriceSummaryFileParameters
            (entryCoveringPeriod: TimeBoundaryPeriod.FifteenMinutes, internalIndexSize: 4, filePeriod: TimeBoundaryPeriod.OneMonth);
        using var pqPriceStorageSummaryFile = new MonthlyWeeklyDailyPriceSummaryTimeSeriesFile<PQPriceStoragePeriodSummary>(createParams);
        CreateNewTyped_TwoSmallPeriods_OriginalValuesAreReturned
            (pqPriceStorageSummaryFile, startOfWeek.TruncToMonthBoundary().AddDays(10), startOfWeek.TruncToMonthBoundary().AddDays(20)
           , TimeBoundaryPeriod.FifteenMinutes, pqPriceStorageSummaryGenerator, asPQPriceStoragePeriodSummaryFactory);
    }

    [TestMethod]
    public void YearlyMonthlyFourHourlyFile10mSummaryPeriods_TwoSmallPeriods_OriginalValuesAreReturned()
    {
        var createParams = CreatePriceSummaryFileParameters
            (entryCoveringPeriod: TimeBoundaryPeriod.TenMinutes, internalIndexSize: 12, filePeriod: TimeBoundaryPeriod.OneYear);
        using var priceSummaryFile = new YearlyMonthlyFourHourlyPriceSummaryTimeSeriesFile<IPricePeriodSummary>(createParams);
        CreateNewTyped_TwoSmallPeriods_OriginalValuesAreReturned
            (priceSummaryFile, startOfWeek.TruncToFirstSundayInYear().AddDays(15), startOfWeek.TruncToFirstSundayInYear().AddDays(45)
           , TimeBoundaryPeriod.TenMinutes, priceSummaryGenerator, asPricePeriodSummaryFactory);
    }

    [TestMethod]
    public void YearlyWeeklyDailyFile1hSummaryPeriods_TwoSmallPeriods_OriginalValuesAreReturned()
    {
        var createParams = CreatePriceSummaryFileParameters
            (entryCoveringPeriod: TimeBoundaryPeriod.OneHour, internalIndexSize: 53, filePeriod: TimeBoundaryPeriod.OneYear);
        using var pqPriceSummaryFile = new YearlyWeeklyDailyPriceSummaryTimeSeriesFile<PQPricePeriodSummary>(createParams);
        CreateNewTyped_TwoSmallPeriods_OriginalValuesAreReturned
            (pqPriceSummaryFile, startOfWeek.TruncToFirstSundayInYear().AddDays(15), startOfWeek.TruncToFirstSundayInYear().AddDays(45)
           , TimeBoundaryPeriod.OneHour, pqPriceSummaryGenerator, asPQPricePeriodSummaryFactory);
    }

    [TestMethod]
    public void YearlyMonthlyWeeklyFile4hSummaryPeriods_TwoSmallPeriods_OriginalValuesAreReturned()
    {
        var createParams = CreatePriceSummaryFileParameters
            (entryCoveringPeriod: TimeBoundaryPeriod.FourHours, internalIndexSize: 12, filePeriod: TimeBoundaryPeriod.OneYear);
        using var pqPriceStorageSummaryFile = new YearlyMonthlyWeeklyPriceSummaryTimeSeriesFile<PQPriceStoragePeriodSummary>(createParams);
        CreateNewTyped_TwoSmallPeriods_OriginalValuesAreReturned
            (pqPriceStorageSummaryFile, startOfWeek.TruncToFirstSundayInYear().AddDays(15), startOfWeek.TruncToFirstSundayInYear().AddDays(45)
           , TimeBoundaryPeriod.FourHours, pqPriceStorageSummaryGenerator, asPQPriceStoragePeriodSummaryFactory);
    }

    [TestMethod]
    public void YearlyMonthlyWeeklyFile30mSummaryPeriods_TwoSmallPeriods_OriginalValuesAreReturned()
    {
        var createParams = CreatePriceSummaryFileParameters
            (entryCoveringPeriod: TimeBoundaryPeriod.ThirtyMinutes, internalIndexSize: 12, filePeriod: TimeBoundaryPeriod.OneYear);
        using var dtoPriceSummaryFile = new DecenniallyMonthlyWeeklyPriceSummaryTimeSeriesFile<PricePeriodSummary>(createParams);
        CreateNewTyped_TwoSmallPeriods_OriginalValuesAreReturned
            (dtoPriceSummaryFile, startOfWeek.TruncToFirstSundayInYear().AddDays(15), startOfWeek.TruncToFirstSundayInYear().AddDays(45)
           , TimeBoundaryPeriod.ThirtyMinutes, priceSummaryGenerator, asDtoPricePeriodSummaryFactory);
    }

    // [TestMethod]
    public void LongRunningDecenniallyYearlyMonthlyFile1dSummaryPeriods_TwoSmallPeriods_OriginalValuesAreReturned()
    {
        var createParams = CreatePriceSummaryFileParameters
            (entryCoveringPeriod: TimeBoundaryPeriod.OneDay, internalIndexSize: 10, filePeriod: TimeBoundaryPeriod.OneYear);
        using var pqPriceSummaryFile = new DecenniallyYearlyMonthlyPriceSummaryTimeSeriesFile<PQPricePeriodSummary>(createParams);
        CreateNewTyped_TwoSmallPeriods_OriginalValuesAreReturned
            (pqPriceSummaryFile, startOfWeek.TruncToFirstSundayInYear().AddDays(15), startOfWeek.TruncToFirstSundayInYear().AddDays(45)
           , TimeBoundaryPeriod.OneDay, pqPriceSummaryGenerator, asPQPricePeriodSummaryFactory);
    }

    // [TestMethod]
    public void LongRunningUnlimitedDecenniallyYearlyFile1WSummaryPeriods_TwoSmallPeriods_OriginalValuesAreReturned()
    {
        var createParams = CreatePriceSummaryFileParameters
            (entryCoveringPeriod: TimeBoundaryPeriod.OneWeek, internalIndexSize: 10, filePeriod: TimeBoundaryPeriod.OneWeek);
        using var pqPriceStorageSummaryFile = new UnlimitedDecenniallyYearlyPriceSummaryTimeSeriesFile<PQPriceStoragePeriodSummary>(createParams);
        CreateNewTyped_TwoSmallPeriods_OriginalValuesAreReturned
            (pqPriceStorageSummaryFile, startOfWeek.AddDays(15), startOfWeek.AddDays(400).AddYears(5)
           , TimeBoundaryPeriod.OneWeek, pqPriceStorageSummaryGenerator, asPQPriceStoragePeriodSummaryFactory);
    }

    // [TestMethod]
    public void LongRunningUnlimitedDecenniallyFile1MSummaryPeriods_TwoSmallPeriods_OriginalValuesAreReturned()
    {
        var createParams = CreatePriceSummaryFileParameters
            (entryCoveringPeriod: TimeBoundaryPeriod.OneMonth, internalIndexSize: 50, filePeriod: TimeBoundaryPeriod.OneWeek);
        using var priceSummaryFile = new UnlimitedDecenniallyPriceSummaryTimeSeriesFile<IPricePeriodSummary>(createParams);
        CreateNewTyped_TwoSmallPeriods_OriginalValuesAreReturned
            (priceSummaryFile, startOfWeek.AddDays(15), startOfWeek.AddDays(400).AddYears(5)
           , TimeBoundaryPeriod.OneMonth, priceSummaryGenerator, asPricePeriodSummaryFactory);
    }

    // [TestMethod]
    public void LongRunningUnlimitedFile1DecadeSummaryPeriods_TwoSmallPeriods_OriginalValuesAreReturned()
    {
        var createParams = CreatePriceSummaryFileParameters
            (entryCoveringPeriod: TimeBoundaryPeriod.OneDecade, internalIndexSize: 1, filePeriod: TimeBoundaryPeriod.OneWeek);
        using var dtoPriceSummaryFile = new UnlimitedPriceSummaryTimeSeriesFile<PricePeriodSummary>(createParams);
        CreateNewTyped_TwoSmallPeriods_OriginalValuesAreReturned
            (dtoPriceSummaryFile, startOfWeek.AddDays(15), startOfWeek.AddDays(3600).AddYears(5)
           , TimeBoundaryPeriod.OneDecade, priceSummaryGenerator, asDtoPricePeriodSummaryFactory);
    }

    public void CreateNewTyped_TwoSmallPeriods_OriginalValuesAreReturned<TEntry>
    (ITimeSeriesEntryFile<TEntry> tsf, DateTime firstPeriodSummaryTime, DateTime secondPeriodSummaryTime, TimeBoundaryPeriod summaryPeriod
      , IPricePeriodSummaryGenerator<IMutablePricePeriodSummary> quoteGenerator, Func<TEntry> retrievalFactory)
        where TEntry : class, ITimeSeriesEntry, IPricePeriodSummary
    {
        var generated = GenerateRepeatablePriceSummaries
            (1, 10, 1, DayOfWeek.Wednesday, summaryPeriod, quoteGenerator, firstPeriodSummaryTime).ToList();
        generated.AddRange
            (GenerateRepeatablePriceSummaries
                (1, 10, 1, DayOfWeek.Thursday, summaryPeriod, quoteGenerator, secondPeriodSummaryTime));

        var toPersistAndCheck = generated.Select(pps => retrievalFactory().CopyFrom(pps)).OfType<TEntry>().ToList();

        using var sessionWriter = tsf.GetWriterSession()!;
        foreach (var firstPeriod in toPersistAndCheck)
        {
            var result = sessionWriter.AppendEntry(firstPeriod);
            Assert.AreEqual(StorageAttemptResult.PeriodRangeMatched, result.StorageAttemptResult);
        }
        tsf.AutoCloseOnZeroSessions = false;
        sessionWriter.Close();

        Assert.AreEqual((uint)toPersistAndCheck.Count, tsf.Header.TotalEntries);
        using var sessionReader = tsf.GetReaderSession();
        var allEntriesReader = sessionReader.AllChronologicalEntriesReader
            (new Recycler(), EntryResultSourcing.FromFactoryFuncUnlimited, ReaderOptions.ReadFastAsPossible, retrievalFactory);
        var storedItems = allEntriesReader.ResultEnumerable.ToList();
        Assert.AreEqual(toPersistAndCheck.Count, allEntriesReader.CountMatch);
        Assert.AreEqual(allEntriesReader.CountMatch, allEntriesReader.CountProcessed);
        Assert.AreEqual(toPersistAndCheck.Count, storedItems.Count);
        CompareExpectedToExtracted(toPersistAndCheck, storedItems);
    }

    private void CompareExpectedToExtracted<TEntry>(List<TEntry> originalList, List<TEntry> toCompareList)
        where TEntry : class, ITimeSeriesEntry, IPricePeriodSummary
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
