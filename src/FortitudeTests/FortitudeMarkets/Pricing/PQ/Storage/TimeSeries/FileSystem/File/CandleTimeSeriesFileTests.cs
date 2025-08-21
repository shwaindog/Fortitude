// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.DataStructures.Memory.Buffers.ByteBuffers.UnmanagedMemory.MemoryMappedFiles;
using FortitudeCommon.Extensions;
using FortitudeCommon.Monitoring.Logging;
using FortitudeIO.Storage.TimeSeries;
using FortitudeIO.Storage.TimeSeries.FileSystem.DirectoryStructure;
using FortitudeIO.Storage.TimeSeries.FileSystem.File;
using FortitudeIO.Storage.TimeSeries.FileSystem.File.Buckets;
using FortitudeIO.Storage.TimeSeries.FileSystem.Session.Retrieval;
using FortitudeMarkets.Config;
using FortitudeMarkets.Pricing.FeedEvents.Candles;
using FortitudeMarkets.Pricing.FeedEvents.Generators.Candles;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Generators.Candles;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Candles;
using FortitudeMarkets.Pricing.PQ.Storage.TimeSeries.FileSystem.File;
using static FortitudeIO.Transports.Network.Config.CountryCityCodes;
using static FortitudeMarkets.Pricing.FeedEvents.TickerInfo.TickerQuoteDetailLevel;
using static FortitudeTests.FortitudeMarkets.Pricing.PQ.Storage.TimeSeries.FileSystem.File.TestWeeklyDataGeneratorFixture;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Storage.TimeSeries.FileSystem.File;

[TestClass]
public class CandleTimeSeriesFileTests
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(CandleTimeSeriesFileTests));

    private readonly Func<Candle>   asDtoCandleFactory = () => new Candle();
    private readonly Func<PQCandle> asPQCandleFactory  = () => new PQCandle();

    private readonly Func<ICandle> asCandleFactory = () => new Candle();

    private Func<PQStorageCandle> asPQStorageCandleFactory = null!;

    private PQStorageCandleGenerator pqStorageCandleGenerator = null!;
    private PQCandleGenerator        pqCandleGenerator        = null!;

    private CandleGenerator candleGenerator = null!;

    private SourceTickerInfo srcTkrInfo = null!;

    private DateTime startOfWeek;


    [TestInitialize]
    public void Setup()
    {
        PagedMemoryMappedFile.LogMappingMessages = true;
        srcTkrInfo =
            new SourceTickerInfo
                (19, nameof(CandleTimeSeriesFileTests), 79, "CandleTests", SingleValue, MarketClassification.Unknown
               , AUinMEL, AUinMEL, AUinMEL
               , 1, 0.1m, 1m, 10_000m, 100_000_000m, 10_000m, 1, 10_000
               , true, false, LayerFlags.None);


        asPQStorageCandleFactory = () => new PQStorageCandle();
        var dateToGenerate   = DateTime.UtcNow.Date.TruncToMonthBoundary().AddDays(15);
        var currentDayOfWeek = dateToGenerate.DayOfWeek;
        var dayDiff          = DayOfWeek.Sunday - currentDayOfWeek;
        startOfWeek = dateToGenerate.AddDays(dayDiff);

        var generateQuoteInfo = new GenerateCandlesInfo(srcTkrInfo);
        generateQuoteInfo.MidPriceGenerator!.StartTime            = startOfWeek;
        generateQuoteInfo.MidPriceGenerator!.StartPrice           = 200_042m;
        generateQuoteInfo.MidPriceGenerator!.RoundAtDecimalPlaces = 1;
        generateQuoteInfo.MinimumVolume                           = 10_000;

        candleGenerator          = new CandleGenerator(generateQuoteInfo);
        pqCandleGenerator        = new PQCandleGenerator(generateQuoteInfo);
        pqStorageCandleGenerator = new PQStorageCandleGenerator(generateQuoteInfo);
    }

    private PriceTimeSeriesFileParameters CreateCandleFileParameters
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
            { nameof(RepositoryPathName.AssetType), "Unknown" }, { nameof(RepositoryPathName.MarketProductType), "Unknown" }
          , { nameof(RepositoryPathName.MarketRegion), "Unknown" }
        };
        var optionalInstrumentFields = new Dictionary<string, string>
        {
            { nameof(RepositoryPathName.Category), "TestInstrumentCategory" }
        };

        var createTestCreateFileParameters =
            new TimeSeriesFileParameters
                (timeSeriesFile
               , new Instrument("TestInstrumentName", "TestSourceName", InstrumentType.Candle, new DiscreetTimePeriod(entryCoveringPeriod)
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
    public void WeeklyDailyHourlyFile15sCandles_TwoSmallPeriods_OriginalValuesAreReturned()
    {
        var       createParams     = CreateCandleFileParameters();
        using var candleFile = new WeeklyDailyHourlyCandleTimeSeriesFile<ICandle>(createParams);
        CreateNewTyped_TwoSmallPeriods_OriginalValuesAreReturned
            (candleFile, startOfWeek.AddDays(1), startOfWeek.AddDays(3), TimeBoundaryPeriod.FifteenSeconds
           , candleGenerator, asCandleFactory);
    }

    [TestMethod]
    public void WeeklyFourHourlyFile30sPQCandles_TwoSmallPeriods_OriginalValuesAreReturned()
    {
        var       createParams       = CreateCandleFileParameters(entryCoveringPeriod: TimeBoundaryPeriod.ThirtySeconds, internalIndexSize: 42);
        using var pqCandleFile = new WeeklyFourHourlyCandleTimeSeriesFile<PQCandle>(createParams);
        CreateNewTyped_TwoSmallPeriods_OriginalValuesAreReturned
            (pqCandleFile, startOfWeek.AddDays(1), startOfWeek.AddDays(3), TimeBoundaryPeriod.ThirtySeconds
           , pqCandleGenerator, asPQCandleFactory);
    }

    [TestMethod]
    public void MonthlyFourHourlyFile1mPQStorageCandles_TwoSmallPeriods_OriginalValuesAreReturned()
    {
        var createParams = CreateCandleFileParameters
            (entryCoveringPeriod: TimeBoundaryPeriod.OneMinute, internalIndexSize: 186, filePeriod: TimeBoundaryPeriod.OneMonth);
        using var pqStorageCandleFile = new MonthlyFourHourlyCandleTimeSeriesFile<PQStorageCandle>(createParams);
        CreateNewTyped_TwoSmallPeriods_OriginalValuesAreReturned
            (pqStorageCandleFile, startOfWeek.AddDays(1), startOfWeek.AddDays(3), TimeBoundaryPeriod.OneMinute
           , pqStorageCandleGenerator, asPQStorageCandleFactory);
    }

    [TestMethod]
    public void MonthlyDailyHourlyFile5mCandles_TwoSmallPeriods_OriginalValuesAreReturned()
    {
        var createParams = CreateCandleFileParameters
            (entryCoveringPeriod: TimeBoundaryPeriod.FiveMinutes, internalIndexSize: 31, filePeriod: TimeBoundaryPeriod.OneMonth);
        using var dtoCandleFile = new MonthlyDailyHourlyCandleTimeSeriesFile<Candle>(createParams);
        CreateNewTyped_TwoSmallPeriods_OriginalValuesAreReturned
            (dtoCandleFile, startOfWeek.AddDays(1), startOfWeek.AddDays(3), TimeBoundaryPeriod.FiveMinutes
           , candleGenerator, asDtoCandleFactory);
    }

    [TestMethod]
    public void MonthlyWeeklyFourHourlyFile10mPQCandles_TwoSmallPeriods_OriginalValuesAreReturned()
    {
        var createParams = CreateCandleFileParameters
            (entryCoveringPeriod: TimeBoundaryPeriod.TenMinutes, internalIndexSize: 4, filePeriod: TimeBoundaryPeriod.OneMonth);
        using var pqCandleFile = new MonthlyWeeklyFourHourlyCandleTimeSeriesFile<PQCandle>(createParams);
        CreateNewTyped_TwoSmallPeriods_OriginalValuesAreReturned
            (pqCandleFile, startOfWeek.TruncToMonthBoundary().AddDays(10), startOfWeek.TruncToMonthBoundary().AddDays(20)
           , TimeBoundaryPeriod.TenMinutes
           , pqCandleGenerator, asPQCandleFactory);
    }

    [TestMethod]
    public void MonthlyWeeklyDailyFile15mPQStorageCandles_TwoSmallPeriods_OriginalValuesAreReturned()
    {
        var createParams = CreateCandleFileParameters
            (entryCoveringPeriod: TimeBoundaryPeriod.FifteenMinutes, internalIndexSize: 4, filePeriod: TimeBoundaryPeriod.OneMonth);
        using var pqStorageCandleFile = new MonthlyWeeklyDailyCandleTimeSeriesFile<PQStorageCandle>(createParams);
        CreateNewTyped_TwoSmallPeriods_OriginalValuesAreReturned
            (pqStorageCandleFile, startOfWeek.TruncToMonthBoundary().AddDays(10), startOfWeek.TruncToMonthBoundary().AddDays(20)
           , TimeBoundaryPeriod.FifteenMinutes, pqStorageCandleGenerator, asPQStorageCandleFactory);
    }

    [TestMethod]
    public void YearlyMonthlyFourHourlyFile10mCandles_TwoSmallPeriods_OriginalValuesAreReturned()
    {
        var createParams = CreateCandleFileParameters
            (entryCoveringPeriod: TimeBoundaryPeriod.TenMinutes, internalIndexSize: 12, filePeriod: TimeBoundaryPeriod.OneYear);
        using var candleFile = new YearlyMonthlyFourHourlyCandleTimeSeriesFile<ICandle>(createParams);
        CreateNewTyped_TwoSmallPeriods_OriginalValuesAreReturned
            (candleFile, startOfWeek.TruncToFirstSundayInYear().AddDays(15), startOfWeek.TruncToFirstSundayInYear().AddDays(45)
           , TimeBoundaryPeriod.TenMinutes, candleGenerator, asCandleFactory);
    }

    [TestMethod]
    public void YearlyWeeklyDailyFile1hCandles_TwoSmallPeriods_OriginalValuesAreReturned()
    {
        var createParams = CreateCandleFileParameters
            (entryCoveringPeriod: TimeBoundaryPeriod.OneHour, internalIndexSize: 53, filePeriod: TimeBoundaryPeriod.OneYear);
        using var pqCandleFile = new YearlyWeeklyDailyCandleTimeSeriesFile<PQCandle>(createParams);
        CreateNewTyped_TwoSmallPeriods_OriginalValuesAreReturned
            (pqCandleFile, startOfWeek.TruncToFirstSundayInYear().AddDays(15), startOfWeek.TruncToFirstSundayInYear().AddDays(45)
           , TimeBoundaryPeriod.OneHour, pqCandleGenerator, asPQCandleFactory);
    }

    [TestMethod]
    public void YearlyMonthlyWeeklyFile4hCandles_TwoSmallPeriods_OriginalValuesAreReturned()
    {
        var createParams = CreateCandleFileParameters
            (entryCoveringPeriod: TimeBoundaryPeriod.FourHours, internalIndexSize: 12, filePeriod: TimeBoundaryPeriod.OneYear);
        using var pqStorageCandleFile = new YearlyMonthlyWeeklyCandleTimeSeriesFile<PQStorageCandle>(createParams);
        CreateNewTyped_TwoSmallPeriods_OriginalValuesAreReturned
            (pqStorageCandleFile, startOfWeek.TruncToFirstSundayInYear().AddDays(15), startOfWeek.TruncToFirstSundayInYear().AddDays(45)
           , TimeBoundaryPeriod.FourHours, pqStorageCandleGenerator, asPQStorageCandleFactory);
    }

    [TestMethod]
    public void YearlyMonthlyWeeklyFile30mCandles_TwoSmallPeriods_OriginalValuesAreReturned()
    {
        var createParams = CreateCandleFileParameters
            (entryCoveringPeriod: TimeBoundaryPeriod.ThirtyMinutes, internalIndexSize: 12, filePeriod: TimeBoundaryPeriod.OneYear);
        using var dtoCandleFile = new DecenniallyMonthlyWeeklyCandleTimeSeriesFile<Candle>(createParams);
        CreateNewTyped_TwoSmallPeriods_OriginalValuesAreReturned
            (dtoCandleFile, startOfWeek.TruncToFirstSundayInYear().AddDays(15), startOfWeek.TruncToFirstSundayInYear().AddDays(45)
           , TimeBoundaryPeriod.ThirtyMinutes, candleGenerator, asDtoCandleFactory);
    }

    // [TestMethod]
    public void LongRunningDecenniallyYearlyMonthlyFile1dCandles_TwoSmallPeriods_OriginalValuesAreReturned()
    {
        var createParams = CreateCandleFileParameters
            (entryCoveringPeriod: TimeBoundaryPeriod.OneDay, internalIndexSize: 10, filePeriod: TimeBoundaryPeriod.OneYear);
        using var pqCandleFile = new DecenniallyYearlyMonthlyCandleTimeSeriesFile<PQCandle>(createParams);
        CreateNewTyped_TwoSmallPeriods_OriginalValuesAreReturned
            (pqCandleFile, startOfWeek.TruncToFirstSundayInYear().AddDays(15), startOfWeek.TruncToFirstSundayInYear().AddDays(45)
           , TimeBoundaryPeriod.OneDay, pqCandleGenerator, asPQCandleFactory);
    }

    // [TestMethod]
    public void LongRunningUnlimitedDecenniallyYearlyFile1WCandles_TwoSmallPeriods_OriginalValuesAreReturned()
    {
        var createParams = CreateCandleFileParameters
            (entryCoveringPeriod: TimeBoundaryPeriod.OneWeek, internalIndexSize: 10, filePeriod: TimeBoundaryPeriod.OneWeek);
        using var pqStorageCandleFile = new UnlimitedDecenniallyYearlyCandleTimeSeriesFile<PQStorageCandle>(createParams);
        CreateNewTyped_TwoSmallPeriods_OriginalValuesAreReturned
            (pqStorageCandleFile, startOfWeek.AddDays(15), startOfWeek.AddDays(400).AddYears(5)
           , TimeBoundaryPeriod.OneWeek, pqStorageCandleGenerator, asPQStorageCandleFactory);
    }

    // [TestMethod]
    public void LongRunningUnlimitedDecenniallyFile1MCandles_TwoSmallPeriods_OriginalValuesAreReturned()
    {
        var createParams = CreateCandleFileParameters
            (entryCoveringPeriod: TimeBoundaryPeriod.OneMonth, internalIndexSize: 50, filePeriod: TimeBoundaryPeriod.OneWeek);
        using var candleFile = new UnlimitedDecenniallyCandleTimeSeriesFile<ICandle>(createParams);
        CreateNewTyped_TwoSmallPeriods_OriginalValuesAreReturned
            (candleFile, startOfWeek.AddDays(15), startOfWeek.AddDays(400).AddYears(5)
           , TimeBoundaryPeriod.OneMonth, candleGenerator, asCandleFactory);
    }

    // [TestMethod]
    public void LongRunningUnlimitedFile1DecadeCandles_TwoSmallPeriods_OriginalValuesAreReturned()
    {
        var createParams = CreateCandleFileParameters
            (entryCoveringPeriod: TimeBoundaryPeriod.OneDecade, internalIndexSize: 1, filePeriod: TimeBoundaryPeriod.OneWeek);
        using var dtoCandleFile = new UnlimitedCandleTimeSeriesFile<Candle>(createParams);
        CreateNewTyped_TwoSmallPeriods_OriginalValuesAreReturned
            (dtoCandleFile, startOfWeek.AddDays(15), startOfWeek.AddDays(3600).AddYears(5)
           , TimeBoundaryPeriod.OneDecade, candleGenerator, asDtoCandleFactory);
    }

    public void CreateNewTyped_TwoSmallPeriods_OriginalValuesAreReturned<TEntry>
    (ITimeSeriesEntryFile<TEntry> tsf, DateTime firstCandleTime, DateTime secondCandleTime, TimeBoundaryPeriod candlePeriod
      , ICandleGenerator<IMutableCandle> quoteGenerator, Func<TEntry> retrievalFactory)
        where TEntry : class, ITimeSeriesEntry, ICandle
    {
        var generated = GenerateRepeatablePriceSummaries
            (1, 10, 1, DayOfWeek.Wednesday, candlePeriod, quoteGenerator, firstCandleTime).ToList();
        generated.AddRange
            (GenerateRepeatablePriceSummaries
                (1, 10, 1, DayOfWeek.Thursday, candlePeriod, quoteGenerator, secondCandleTime));

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
        where TEntry : class, ITimeSeriesEntry, ICandle
    {
        for (var i = 0; i < originalList.Count; i++)
        {
            var originalEntry = originalList[i];
            ((IMutableCandle)originalEntry).CandleFlags |= CandleFlags.FromStorage;
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
