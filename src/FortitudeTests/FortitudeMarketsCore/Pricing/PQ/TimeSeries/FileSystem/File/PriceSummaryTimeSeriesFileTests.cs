// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory.UnmanagedMemory.MemoryMappedFiles;
using FortitudeCommon.Monitoring.Logging;
using FortitudeIO.TimeSeries;
using FortitudeIO.TimeSeries.FileSystem.File;
using FortitudeIO.TimeSeries.FileSystem.File.Buckets;
using FortitudeIO.TimeSeries.FileSystem.Session;
using FortitudeIO.TimeSeries.FileSystem.Session.Retrieval;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsApi.Pricing.TimeSeries;
using FortitudeMarketsCore.Pricing.Generators.Summaries;
using FortitudeMarketsCore.Pricing.PQ.Generators.Summaries;
using FortitudeMarketsCore.Pricing.PQ.Summaries;
using FortitudeMarketsCore.Pricing.PQ.TimeSeries.FileSystem.File;
using FortitudeMarketsCore.Pricing.Summaries;
using static FortitudeIO.TimeSeries.MarketClassificationExtensions;
using static FortitudeMarketsApi.Pricing.Quotes.QuoteLevel;
using static FortitudeTests.FortitudeMarketsCore.Pricing.PQ.TimeSeries.FileSystem.File.TestWeeklyDataGeneratorFixture;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.TimeSeries.FileSystem.File;

[TestClass]
public class PriceSummaryTimeSeriesFileTests
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(PriceSummaryTimeSeriesFileTests));

    private readonly Func<IPricePeriodSummary> asPQPricePeriodSummaryFactory        = () => new PQPricePeriodSummary();
    private readonly Func<IPricePeriodSummary> asPQPriceStoragePeriodSummaryFactory = () => new PQPriceStoragePeriodSummary();
    private readonly Func<IPricePeriodSummary> asPricePeriodSummaryFactory          = () => new PricePeriodSummary();

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

        var dateToGenerate   = DateTime.UtcNow.Date;
        var currentDayOfWeek = dateToGenerate.DayOfWeek;
        var dayDiff          = DayOfWeek.Sunday - currentDayOfWeek;
        startOfWeek = dateToGenerate.AddDays(dayDiff);

        var generateQuoteInfo = new GeneratePriceSummariesInfo(srcTkrQtInfo);
        generateQuoteInfo.MidPriceGenerator!.StartTime  = startOfWeek;
        generateQuoteInfo.MidPriceGenerator!.StartPrice = 200_042m;
        generateQuoteInfo.MinimumVolume                 = 10_000;

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
        var createTestCreateFileParameters =
            new TimeSeriesFileParameters
                (timeSeriesFile
               , new Instrument("TestInstrumentName", "TestSourceName", InstrumentType.PriceSummaryPeriod, Unknown
                              , entryPeriod, "TestInstrumentCategory"),
                 filePeriod, DateTime.UtcNow.Date, internalIndexSize,
                 fileFlags, 6);
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

    public void CreateNewTyped_TwoSmallPeriods_OriginalValuesAreReturned<TEntry>
    (DateTime firstPeriodSummaryTime, DateTime secondPeriodSummaryTime, TimeSeriesPeriod summaryPeriod
      , IPricePeriodSummaryGenerator<TEntry> quoteGenerator, Func<IPricePeriodSummary> retrievalFactory)
        where TEntry : class, IMutablePricePeriodSummary
    {
        var toPersistAndCheck = GenerateRepeatablePriceSummaries
            (1, 10, 1, DayOfWeek.Wednesday, summaryPeriod, quoteGenerator, firstPeriodSummaryTime).ToList();
        toPersistAndCheck.AddRange(GenerateRepeatablePriceSummaries
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
            var compareEntry  = toCompareList[i];
            if (!originalEntry.AreEquivalent(compareEntry))
            {
                Logger.Warn("Entries at {0} differ test failed \noriginal {1}\nretrieved {2} ", i, originalEntry, compareEntry);
                FLoggerFactory.WaitUntilDrained();
                Assert.Fail($"Entries at {i} differ test failed \noriginal {originalEntry}\nretrieved {compareEntry}.");
            }
        }
    }
}
