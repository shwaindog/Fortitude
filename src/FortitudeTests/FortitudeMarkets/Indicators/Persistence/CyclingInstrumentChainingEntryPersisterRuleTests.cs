// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.Rules.Common.TimeSeries;
using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Extensions;
using FortitudeCommon.Monitoring.Logging;
using FortitudeIO.Storage.TimeSeries;
using FortitudeIO.Storage.TimeSeries.FileSystem;
using FortitudeIO.Storage.TimeSeries.FileSystem.Session.Retrieval;
using FortitudeMarkets.Config;
using FortitudeMarkets.Indicators.Persistence;
using FortitudeMarkets.Pricing;
using FortitudeMarkets.Pricing.FeedEvents.Candles;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeTests.FortitudeBusRules.BusMessaging;
using FortitudeTests.FortitudeMarkets.Indicators.Config;
using static FortitudeCommon.Chronometry.TimeBoundaryPeriod;
using static FortitudeIO.Transports.Network.Config.CountryCityCodes;
using static FortitudeTests.FortitudeCommon.Extensions.DirectoryInfoExtensionsTests;
using static FortitudeTests.FortitudeMarkets.Pricing.FeedEvents.Candles.CandleTests;
using static FortitudeMarkets.Pricing.FeedEvents.TickerInfo.TickerQuoteDetailLevel;

#endregion

namespace FortitudeTests.FortitudeMarkets.Indicators.Persistence;

[TestClass]
public class CyclingInstrumentChainingEntryPersisterRuleTests : OneOfEachMessageQueueTypeTestSetup
{
    private const string TestEntriesToPersistAddress = "TestCyclingEntriesToPersistAddress";

    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(CyclingInstrumentChainingEntryPersisterRuleTests));

    private static readonly string FullDrainRequestAddress = CyclingInstrumentChainingEntryPersisterRule<Candle>.FullDrainRequestAddress;

    private static DateTime testEpoch = new(2024, 7, 12);

    private readonly Random random = new();

    private readonly SourceTickerInfo ticker1Id = new
        (1, "SourceName1", 1, "TickerName1", Level1Quote, MarketClassification.Unknown
         , AUinMEL, AUinMEL, AUinMEL
       , 1, 0.001m, 10m, 100m, 10m);

    private readonly SourceTickerInfo ticker2Id = new
        (1, "SourceName1", 2, "TickerName2", Level1Quote, MarketClassification.Unknown
       , AUinMEL, AUinMEL, AUinMEL
       , 1, 0.001m, 10m, 100m, 10m);

    private readonly SourceTickerInfo ticker3Id = new
        (2, "SourceName2", 1, "TickerName1", Level1Quote, MarketClassification.Unknown
       , AUinMEL, AUinMEL, AUinMEL
       , 1, 0.001m, 10m, 100m, 10m);

    private readonly SourceTickerInfo ticker4Id = new
        (2, "SourceName2", 1, "TickerName2", Level1Quote, MarketClassification.Unknown
       , AUinMEL, AUinMEL, AUinMEL
       , 1, 0.001m, 10m, 100m, 10m);

    private CyclingInstrumentChainingEntryPersisterParams                    cyclingPersisterParams;
    private CyclingInstrumentChainingEntryPersisterRule<Candle>? cyclingPersisterRule;

    private decimal highLowSpread;

    private SourceTickerInfo[] instruments = null!;

    private decimal mid1;
    private decimal mid2;

    private TimeBoundaryPeriod[] periods = null!;

    private DateTime persistStartTime;

    private IRecycler     recycler    = null!;
    private DirectoryInfo repoRootDir = null!;

    private DateTime runningTime = testEpoch;
    private decimal  spread;

    private ITimeSeriesRepository timeSeriesRepository = null!;

    protected override int RingPollerSize => 100_000;

    [TestInitialize]
    public void Setup()
    {
        recycler  = new Recycler();
        testEpoch = new DateTime(2024, 7, 12);

        persistStartTime = testEpoch;

        mid1 = 0.5000m;
        mid2 = 1.0000m;

        spread        = 0.2000m;
        highLowSpread = 0.8000m;

        repoRootDir = UniqueNewTestDirectory("CyclingPersisterRuleTests");
        var serviceConfig = IndicatorServicesConfigTests.UnitTestConfig(repoRootDir);

        timeSeriesRepository = serviceConfig.TimeSeriesFileRepositoryConfig!.BuildRepository();
        cyclingPersisterParams = new CyclingInstrumentChainingEntryPersisterParams
            (new TimeSeriesRepositoryParams(timeSeriesRepository), TestEntriesToPersistAddress);

        instruments = new[] { ticker1Id, ticker2Id, ticker3Id, ticker4Id };
        foreach (var sourceTickerInfo in instruments)
        {
            sourceTickerInfo.Register();
            sourceTickerInfo.InstrumentType = InstrumentType.Candle;
        }

        periods = new[] { FifteenSeconds, ThirtySeconds, OneMinute, FiveMinutes, FifteenMinutes, ThirtyMinutes };
    }

    private IEnumerable<ChainableInstrumentPayload<Candle>> GenerateAlternatingInstrumentSummaries(int numberToGenerate = 200)
    {
        for (var i = 0; i < numberToGenerate; i++)
        {
            var selectTickerIndex = random.Next(instruments.Length);
            var ticker            = instruments[selectTickerIndex];
            var remainingItems    = numberToGenerate - i;
            var selectPeriodIndex = random.Next(periods.Length);
            var period            = periods[selectPeriodIndex];
            var maxTickerCluster  = Math.Max(1, Math.Min(remainingItems, Math.Min(numberToGenerate / 20, random.Next(remainingItems))));
            foreach (var candle in GenerateCandles(period, maxTickerCluster))
            {
                var instrument = ticker.Clone();
                instrument.CoveringPeriod = new DiscreetTimePeriod(candle.TimeBoundaryPeriod);
                instrument.InstrumentType = InstrumentType.Candle;
                yield return new ChainableInstrumentPayload<Candle>(instrument, candle);
            }
            i += maxTickerCluster - 1;
        }
    }

    private IEnumerable<Candle> GenerateCandles(TimeBoundaryPeriod timeBoundaryPeriod, int numberToGenerate = 4)
    {
        runningTime = timeBoundaryPeriod.ContainingPeriodEnd(runningTime);

        for (var i = 0; i < numberToGenerate; i++)
            yield return CreateCandle
                (timeBoundaryPeriod, runningTime = timeBoundaryPeriod.PeriodEnd(runningTime), i % 2 == 0 ? mid1 : mid2, spread, highLowSpread);
    }

    [TestCleanup]
    public async Task TearDown()
    {
        if (cyclingPersisterRule != null) await CustomQueue1.StopRuleAsync(cyclingPersisterRule, cyclingPersisterRule);
        timeSeriesRepository.CloseAllFilesAndSessions();
        repoRootDir.RecursiveDelete();
    }

    [TestMethod]
    [Timeout(60_000)]
    public async Task NewRepository_SendEntriesToPersister_CanRetrieveEntriesFromRepository()
    {
        cyclingPersisterRule = new CyclingInstrumentChainingEntryPersisterRule<Candle>(cyclingPersisterParams);
        await using var ruleDeployment = await CustomQueue1.LaunchRuleAsync(cyclingPersisterRule, cyclingPersisterRule, CustomQueue1SelectionResult);

        var toCheck = GenerateAlternatingInstrumentSummaries().ToList();

        foreach (var pricingPersist in toCheck) MessageBus.Publish(cyclingPersisterRule, TestEntriesToPersistAddress, pricingPersist);

        var numberPersisted = await MessageBus.RequestAsync<string, int>(cyclingPersisterRule, FullDrainRequestAddress, "CyclingPersisterTest");

        await Console.Out.WriteLineAsync($"Finished persisting last round persisted {numberPersisted}");

        var distinctInstruments = toCheck.Select(cip => cip.PricingInstrumentId).Distinct().ToList();

        foreach (var pricingInstrumentId in distinctInstruments)
        {
            var entriesForInstrument = toCheck
                                       .Where(cip => Equals(cip.PricingInstrumentId, pricingInstrumentId))
                                       .Select(cip =>
                                       {
                                           cip.Entry.CandleFlags |= CandleFlags.FromStorage;
                                           return cip.Entry;
                                       })
                                       .ToList();

            Console.Out.WriteLine($"Persisted {pricingInstrumentId} it has {entriesForInstrument.Count} instruments");
            var instrument = new PricingInstrumentId(pricingInstrumentId);

            var fileInfo = timeSeriesRepository.GetInstrumentFileEntryInfo(instrument);
            Assert.IsTrue(fileInfo.HasInstrument);
            using var reader = timeSeriesRepository.GetReaderSession<Candle>(instrument);

            var entryReader = reader!.AllChronologicalEntriesReader
                (new Recycler(), EntryResultSourcing.NewEachEntryUnlimited, ReaderOptions.ReadFastAsPossible, () => new Candle());
            var readEntries = entryReader.ResultEnumerable.ToList();

            Console.Out.WriteLine($"Read for  {pricingInstrumentId} it has {readEntries.Count} instruments");
            Assert.AreEqual(entriesForInstrument.Count, readEntries.Count);

            foreach (var candle in readEntries) entriesForInstrument.Remove(candle);
            if (entriesForInstrument.Any())
            {
                foreach (var candle in readEntries) Console.Out.WriteLine($"candle {candle} returned");
                Console.Out.WriteLine("");
                Console.Out.WriteLine("");
                foreach (var candle in entriesForInstrument) Console.Out.WriteLine($"candle {candle} not found");
            }
            Assert.AreEqual(0, entriesForInstrument.Count);
        }
        cyclingPersisterRule = null;
    }
}
