// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.BusMessaging.Messages.ListeningSubscriptions;
using FortitudeBusRules.BusMessaging.Pipelines;
using FortitudeBusRules.BusMessaging.Routing.Response;
using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;
using FortitudeCommon.Chronometry;
using FortitudeCommon.Chronometry.Timers;
using FortitudeCommon.Extensions;
using FortitudeCommon.Logging.Core;
using FortitudeCommon.Logging.Core.LoggerViews;
using FortitudeIO.Storage.TimeSeries;
using FortitudeIO.Storage.TimeSeries.FileSystem;
using FortitudeMarkets.Config;
using FortitudeMarkets.Indicators;
using FortitudeMarkets.Indicators.Pricing;
using FortitudeMarkets.Indicators.Pricing.Candles;
using FortitudeMarkets.Indicators.Pricing.Candles.Construction;
using FortitudeMarkets.Pricing;
using FortitudeMarkets.Pricing.FeedEvents.Candles;
using FortitudeMarkets.Pricing.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Subscription.BusRules;
using FortitudeTests.FortitudeBusRules.BusMessaging;
using FortitudeTests.FortitudeBusRules.Rules.Common.TimeSeries;
using FortitudeTests.FortitudeCommon.Chronometry;
using FortitudeTests.FortitudeCommon.Chronometry.Timers;
using FortitudeTests.FortitudeMarkets.Indicators.Config;
using FortitudeTests.FortitudeMarkets.Pricing.FeedEvents.Quotes;
using static FortitudeCommon.Chronometry.TimeBoundaryPeriod;
using static FortitudeIO.Transports.Network.Config.CountryCityCodes;
using static FortitudeTests.FortitudeMarkets.Pricing.FeedEvents.Candles.CandleTests;
using static FortitudeMarkets.Pricing.FeedEvents.TickerInfo.TickerQuoteDetailLevel;

// ReSharper disable FormatStringProblem

#endregion

namespace FortitudeTests.FortitudeMarkets.Indicators.Pricing.Candles;

[TestClass]
public class LiveCandlePublisherRuleTests : OneOfEachMessageQueueTypeTestSetup
{
    private static readonly IVersatileFLogger Logger = FLog.FLoggerForType.As<IVersatileFLogger>();


    private readonly List<InstrumentFileEntryInfo> lastFileEntryInfoRetrieved = new();
    private readonly List<InstrumentFileInfo>      lastFileInfoRetrieved      = new();

    private readonly DateTime testEpochTime = new(2024, 7, 11);

    private readonly SourceTickerInfo tickerId15SPeriod = new
        (2, "SourceName", 2, "TickerName2", Level1Quote, MarketClassification.Unknown
       , AUinMEL, AUinMEL, AUinMEL
       , 1, 0.001m, 10m, 100m, 10m);

    private readonly SourceTickerInfo tickerId1MPeriod = new
        (2, "SourceName", 2, "TickerName4", Level1Quote, MarketClassification.Unknown
       , AUinMEL, AUinMEL, AUinMEL
       , 1, 0.001m, 10m, 100m, 10m);

    private readonly SourceTickerInfo tickerId30SPeriod = new
        (3, "SourceName", 3, "TickerName3", Level1Quote, MarketClassification.Unknown
       , AUinMEL, AUinMEL, AUinMEL
       , 1, 0.001m, 10m, 100m, 10m);

    private readonly SourceTickerInfo tickerId5SPeriod = new
        (1, "SourceName", 1, "TickerName1", Level1Quote, MarketClassification.Unknown
       , AUinMEL, AUinMEL, AUinMEL
       , 1, 0.001m, 10m, 100m, 10m);

    private List<Candle> fifteenSecondPeriodSummaries = null!;

    private LivePublishCandleParams fifteenSecondsLivePeriodParams;

    private LivePublishCandleParams fiveSecondsLivePeriodParams;

    private decimal highLowSpread;

    private IndicatorServiceRegistryStubRule indicatorRegistryStubRule = null!;

    private decimal mid1;
    private decimal mid2;

    private LivePublishCandleParams oneMinuteLivePeriodParams;

    private List<PublishableLevel1PriceQuote> oneSecondLevel1Quotes = null!;

    private decimal spread;

    private StubTimeContext stubTimeContext = null!;

    private List<Candle> thirtySecondPeriodSummaries = null!;

    private LivePublishCandleParams thirtySecondsLivePeriodParams;

    private IList<IAsyncDisposable> undeploy = null!;

    [TestInitialize]
    public async Task Setup()
    {
        undeploy = new List<IAsyncDisposable>();

        mid1 = 0.5000m;
        mid2 = 1.0000m;

        spread        = 0.2000m;
        highLowSpread = 0.8000m;

        Generate15SSummaries();
        Generate1SQuotes();
        Generate30SSummaries();

        fiveSecondsLivePeriodParams = new LivePublishCandleParams
            ((SourceTickerIdentifier)tickerId5SPeriod, FiveSeconds, new IndicatorPublishInterval(TimeSpan.FromSeconds(1))
           , new ResponsePublishParams());
        fifteenSecondsLivePeriodParams
            = new LivePublishCandleParams(tickerId15SPeriod, FifteenSeconds, new IndicatorPublishInterval(TimeSpan.FromSeconds(1))
                                        , new ResponsePublishParams());
        thirtySecondsLivePeriodParams = new LivePublishCandleParams
            (tickerId30SPeriod, ThirtySeconds, new IndicatorPublishInterval(TimeSpan.FromSeconds(1)), new ResponsePublishParams());
        oneMinuteLivePeriodParams = new LivePublishCandleParams
            (tickerId1MPeriod, OneMinute, new IndicatorPublishInterval(TimeSpan.FromSeconds(1)), new ResponsePublishParams());
        var unitTestNoRepositoryConfig = IndicatorServicesConfigTests.UnitTestNoRepositoryConfig();
        unitTestNoRepositoryConfig.PersistenceConfig.PersistPriceSummaries = true;
        var repoInfoStubRule = new TimeSeriesRepositoryInfoStubRule(GetStubRepoFileInfo, GetStubRepoFileEntryInfo);
        indicatorRegistryStubRule
            = new IndicatorServiceRegistryStubRule
                (new IndicatorServiceRegistryParams(unitTestNoRepositoryConfig));

        var preqDeploy
            = await EventQueue1.LaunchRuleAsync
                (indicatorRegistryStubRule, indicatorRegistryStubRule, EventQueue1SelectionResult);
        indicatorRegistryStubRule.RegisterGlobalServiceStatus(ServiceType.CandleFilePersister, ServiceRunStatus.ServiceStarted);
        indicatorRegistryStubRule.RegisterTickerPeriodServiceStatus
            (tickerId1MPeriod, new DiscreetTimePeriod(FifteenSeconds), ServiceType.LiveCandle, ServiceRunStatus.ServiceStarted);
        indicatorRegistryStubRule.RegisterTickerPeriodServiceStatus
            (tickerId1MPeriod, new DiscreetTimePeriod(ThirtySeconds), ServiceType.LiveCandle, ServiceRunStatus.ServiceStarted);
        indicatorRegistryStubRule.RegisterTickerPeriodServiceStatus
            (tickerId30SPeriod, new DiscreetTimePeriod(FifteenSeconds), ServiceType.HistoricalCandlesResolver
           , ServiceRunStatus.ServiceStarted);

        await indicatorRegistryStubRule.RegisterAndDeployGlobalService(ServiceType.TimeSeriesFileRepositoryInfo, repoInfoStubRule);
        indicatorRegistryStubRule.RegisterTickerPeriodServiceStatus
            (tickerId1MPeriod, new DiscreetTimePeriod(ThirtySeconds), ServiceType.HistoricalCandlesResolver
           , ServiceRunStatus.ServiceStarted);
        undeploy.Add(preqDeploy);
    }

    private List<InstrumentFileInfo> GetStubRepoFileInfo
        (string instrumentName, InstrumentType? instrumentType = null, DiscreetTimePeriod? period = null)
    {
        lastFileInfoRetrieved.Clear();
        if (period?.Period == FiveSeconds)
        {
            var lastHistoricalCandleStartTime = FiveSeconds.ContainingPeriodBoundaryStart(testEpochTime);
            var lastHistoricalEntryTime = instrumentType == InstrumentType.Candle
                ? lastHistoricalCandleStartTime
                : testEpochTime;
            var earliestHistoricalEntryTime = instrumentType == InstrumentType.Candle
                ? FiveSeconds.PreviousPeriodStart(lastHistoricalEntryTime)
                : testEpochTime;
            var fileStartDate = lastHistoricalEntryTime.TruncToWeekBoundary();
            lastFileInfoRetrieved
                .Add(new InstrumentFileInfo(tickerId5SPeriod, OneWeek, earliestHistoricalEntryTime, lastHistoricalEntryTime
                                          , [fileStartDate]));
            return lastFileInfoRetrieved;
        }
        if (period?.Period == FifteenSeconds)
        {
            var lastHistoricalCandleStartTime = FifteenSeconds.ContainingPeriodBoundaryStart(testEpochTime);
            var lastHistoricalEntryTime = instrumentType == InstrumentType.Candle
                ? lastHistoricalCandleStartTime
                : testEpochTime;
            var earliestHistoricalEntryTime = instrumentType == InstrumentType.Candle
                ? FifteenSeconds.PreviousPeriodStart(lastHistoricalEntryTime)
                : testEpochTime;
            var fileStartDate = lastHistoricalEntryTime.TruncToWeekBoundary();
            lastFileInfoRetrieved
                .Add(new InstrumentFileInfo(tickerId15SPeriod, OneWeek, earliestHistoricalEntryTime, lastHistoricalEntryTime
                                          , [fileStartDate]));
            return lastFileInfoRetrieved;
        }
        var latestCandleStartTime = ThirtySeconds.ContainingPeriodBoundaryStart(testEpochTime);
        var lastEntryTime = instrumentType == InstrumentType.Candle
            ? latestCandleStartTime
            : testEpochTime;
        var earliestEntryTime = instrumentType == InstrumentType.Candle
            ? ThirtySeconds.PreviousPeriodStart(latestCandleStartTime)
            : testEpochTime;
        var fileStartTime = lastEntryTime.TruncToWeekBoundary();
        lastFileInfoRetrieved
            .Add(new InstrumentFileInfo(tickerId30SPeriod, OneWeek, earliestEntryTime, lastEntryTime, [fileStartTime]));
        return lastFileInfoRetrieved;
    }

    private List<InstrumentFileEntryInfo> GetStubRepoFileEntryInfo
        (string instrumentName, InstrumentType? instrumentType = null, DiscreetTimePeriod? period = null)
    {
        lastFileInfoRetrieved.Clear();
        if (instrumentName == tickerId15SPeriod.InstrumentName)
        {
            lastFileEntryInfoRetrieved.Add(new InstrumentFileEntryInfo(tickerId15SPeriod, OneWeek, new List<FileEntryInfo>(), 1));
            return lastFileEntryInfoRetrieved;
        }
        lastFileEntryInfoRetrieved.Add(new InstrumentFileEntryInfo(tickerId30SPeriod, OneWeek, new List<FileEntryInfo>(), 1));
        return lastFileEntryInfoRetrieved;
    }

    public override ITimerProvider ResolverTimerProvider()
    {
        TimeContext.Provider = stubTimeContext = new StubTimeContext(testEpochTime);
        return new StubTimerContextProvider();
    }

    private void Generate1SQuotes(int numberToGenerate = 120)
    {
        var entriesStartTime = OneSecond.PreviousPeriodStart(testEpochTime);

        oneSecondLevel1Quotes = new List<PublishableLevel1PriceQuote>(numberToGenerate);
        for (var i = 0; i < numberToGenerate; i++)
        {
            var l1Quote = tickerId15SPeriod.CreatePublishableLevel1Quote(entriesStartTime = OneSecond.PeriodEnd(entriesStartTime), i % 2 == 0 ? mid1 : mid2
                                                                       , spread);

            // logger.TrcApnd("Generated 1s quote {0}")?.Args(l1Quote);
            oneSecondLevel1Quotes.Add(l1Quote);
        }
    }

    private void Generate15SSummaries(int numberToGenerate = 8)
    {
        var entriesStartTime = FifteenSeconds.PreviousPeriodStart(testEpochTime);

        fifteenSecondPeriodSummaries = new List<Candle>(numberToGenerate);
        for (var i = 0; i < numberToGenerate; i++)
        {
            var candle = CreateCandle
                (FifteenSeconds, entriesStartTime = FifteenSeconds.PeriodEnd(entriesStartTime), i % 2 == 0 ? mid1 : mid2, spread, highLowSpread);

            // logger.TrcApnd("Generated 15S Candle {0}")?.Args(candle);
            fifteenSecondPeriodSummaries.Add(candle);
        }
    }

    private void Generate30SSummaries(int numberToGenerate = 4)
    {
        var entriesStartTime = ThirtySeconds.PreviousPeriodStart(testEpochTime);

        thirtySecondPeriodSummaries = new List<Candle>(numberToGenerate);
        for (var i = 0; i < numberToGenerate; i++)
        {
            var candle = CreateCandle
                (ThirtySeconds, entriesStartTime = ThirtySeconds.PeriodEnd(entriesStartTime), i % 2 == 0 ? mid1 : mid2, spread, highLowSpread);
            // logger.TrcApnd("Generated 30S Candle {0}")?.Args(candle);
            thirtySecondPeriodSummaries.Add(candle);
        }
    }


    [TestCleanup]
    public async Task TearDown()
    {
        await stubTimeContext.DisposeAsync();
        foreach (var asyncDisposable in undeploy) await asyncDisposable.DisposeAsync();
    }

    [TestMethod]
    [Timeout(20_000)]
    public async Task NewLiveCandle_SendTwoPeriodsOfQuotes_PublishingNextTickSendsCompleteOfFirstPeriod()
    {
        var test5SLivePeriodClient = new TestLivePeriodClient
            (new PricingInstrumentIdValue
                 ((SourceTickerIdentifier)tickerId5SPeriod
                , new PeriodInstrumentTypePair(InstrumentType.Candle, new DiscreetTimePeriod(FiveSeconds))));
        await indicatorRegistryStubRule.DeployChildRuleAsync(test5SLivePeriodClient);

        var liveResolver5SRule = new LiveCandlePublisherRule<PublishableLevel1PriceQuote>(fiveSecondsLivePeriodParams);

        await using var histResolverDeploy = await indicatorRegistryStubRule.DeployChildRuleAsync(liveResolver5SRule);

        var progressTime = stubTimeContext.ProgressTimeWithoutEvents;
        await test5SLivePeriodClient.SendPricesToLivePeriodRule(oneSecondLevel1Quotes.Take(12).ToList(), progressTime);

        var receivedCompletePeriods = await test5SLivePeriodClient.GetPopulatedCompleteResults();
        Assert.AreEqual(1, receivedCompletePeriods.Count);
    }

    [TestMethod]
    [Timeout(20_000)]
    public async Task NewLiveCandle_SendOnePeriodsOfQuotes_IncrementingTimeTriggersOnCompleteTimer()
    {
        var test5SLivePeriodClient = new TestLivePeriodClient
            (new PricingInstrumentIdValue
                 ((SourceTickerIdentifier)tickerId5SPeriod
                , new PeriodInstrumentTypePair(InstrumentType.Candle, new DiscreetTimePeriod(FiveSeconds))));
        await indicatorRegistryStubRule.DeployChildRuleAsync(test5SLivePeriodClient);

        var liveResolver5SRule = new LiveCandlePublisherRule<PublishableLevel1PriceQuote>(fiveSecondsLivePeriodParams);

        await using var live5SDeploy = await indicatorRegistryStubRule.DeployChildRuleAsync(liveResolver5SRule);

        stubTimeContext.IncrementedTimeCallback = async (_, _, _) =>
        {
            // Between each event fire and await the queue to run an empty callback to give rule time to update next timer;
            await liveResolver5SRule.Context.RegisteredOn.RunOnAndWait(liveResolver5SRule, () => ValueTask.CompletedTask);
        };
        await test5SLivePeriodClient.SendPricesToLivePeriodRule(oneSecondLevel1Quotes.Take(9).ToList(), stubTimeContext);

        var receivedCompletePeriods = await test5SLivePeriodClient.GetPopulatedCompleteResults();
        Assert.AreEqual(1, receivedCompletePeriods.Count);
        test5SLivePeriodClient.CreateNewWait(1, 8);
        var receivedLivePeriods = await test5SLivePeriodClient.GetPopulatedLiveResults(8);
        Assert.AreEqual(8, receivedLivePeriods.Count);
    }

    [TestMethod]
    [Timeout(20_000)]
    public async Task NewLiveCandle_SendSingleQuotes_LivePeriodUpdateReceivedBetweenEach()
    {
        var test5SLivePeriodClient = new TestLivePeriodClient
            (new PricingInstrumentIdValue
                 ((SourceTickerIdentifier)tickerId5SPeriod
                , new PeriodInstrumentTypePair(InstrumentType.Candle, new DiscreetTimePeriod(FiveSeconds))));
        await indicatorRegistryStubRule.DeployChildRuleAsync(test5SLivePeriodClient);

        var liveResolver5SRule = new LiveCandlePublisherRule<PublishableLevel1PriceQuote>(fiveSecondsLivePeriodParams);

        await using var live5SDeploy = await indicatorRegistryStubRule.DeployChildRuleAsync(liveResolver5SRule);

        stubTimeContext.IncrementedTimeCallback = async (_, _, _) =>
        {
            // Between each event fire and await the queue to run an empty callback to give rule time to update next timer;
            await liveResolver5SRule.Context.RegisteredOn.RunOnAndWait(liveResolver5SRule, () => ValueTask.CompletedTask);
        };
        var quotesToSend = oneSecondLevel1Quotes.Skip(1).Take(6).ToList();
        await test5SLivePeriodClient.SendPricesToLivePeriodRule
            ([quotesToSend.First()], stubTimeContext.ProgressTimeWithoutEvents);
        for (var i = 0; i < quotesToSend.Count; i++)
        {
            var quote = quotesToSend[i];
            test5SLivePeriodClient.CreateNewWait();
            await test5SLivePeriodClient.SendPricesToLivePeriodRule([quote], stubTimeContext);
            var receivedLivePeriods = await test5SLivePeriodClient.GetPopulatedLiveResults(i + 1);
            Assert.AreEqual(i + 1, receivedLivePeriods.Count, $"For Loop {i} ");
        }
        await stubTimeContext.AddSecondsAsync(2);
        var receivedCompletePeriods = await test5SLivePeriodClient.GetPopulatedCompleteResults();
        Assert.AreEqual(1, receivedCompletePeriods.Count);
    }

    [TestMethod]
    [Timeout(20_000)]
    public async Task NewLiveCandle_RequestsSubPeriodReturnsWithinSamePeriod_PublishesQuotesToNextPeriod()
    {
        await stubTimeContext.AddSecondsAsync(45);
        var test1MLivePeriodClient = new TestLivePeriodClient
            (new PricingInstrumentIdValue
                 ((SourceTickerIdentifier)tickerId1MPeriod
                , new PeriodInstrumentTypePair(InstrumentType.Candle, new DiscreetTimePeriod(OneMinute))));
        test1MLivePeriodClient.RegisterSubPeriodResponse
            (FifteenSeconds
           , () => new ValueTask<List<Candle>>(fifteenSecondPeriodSummaries.Skip(2).Take(1).ToList()));
        test1MLivePeriodClient.RegisterSubPeriodResponse
            (ThirtySeconds
           , () => new ValueTask<List<Candle>>(thirtySecondPeriodSummaries.Take(1).ToList()));
        await indicatorRegistryStubRule.DeployChildRuleAsync(test1MLivePeriodClient);

        var live1MRule = new LiveCandlePublisherRule<PublishableLevel1PriceQuote>(oneMinuteLivePeriodParams);

        await using var live1MDeploy = await indicatorRegistryStubRule.DeployChildRuleAsync(live1MRule);

        stubTimeContext.IncrementedTimeCallback = async (_, _, _) =>
        {
            // Between each event fire and await the queue to run an empty callback to give rule time to update next timer;
            await live1MRule.Context.RegisteredOn.RunOnAndWait(live1MRule, () => ValueTask.CompletedTask);
        };

        var quotesToSend = oneSecondLevel1Quotes.Skip(46).Take(14).ToList();
        await test1MLivePeriodClient.SendPricesToLivePeriodRule
            ([quotesToSend.First()], stubTimeContext.ProgressTimeWithoutEvents);
        for (var i = 0; i < quotesToSend.Count; i++)
        {
            var quote = quotesToSend[i];
            test1MLivePeriodClient.CreateNewWait();
            await test1MLivePeriodClient.SendPricesToLivePeriodRule([quote], stubTimeContext);
            var receivedLivePeriods = await test1MLivePeriodClient.GetPopulatedLiveResults(i + 1);
            Assert.AreEqual(i + 1, receivedLivePeriods.Count);
        }
        await stubTimeContext.AddSecondsAsync(6);
        var receivedCompletePeriods = await test1MLivePeriodClient.GetPopulatedCompleteResults();
        Assert.AreEqual(1, receivedCompletePeriods.Count);
        var summary = receivedCompletePeriods.First();
        Assert.AreEqual((ushort)0, (ushort)((uint)summary.CandleFlags >> 16));
    }

    [TestMethod]
    [Timeout(20_000)]
    public async Task NewLiveCandle_RequestsSubPeriodReturnsWithinNextPeriod_PublishesQuotesToNextPeriod()
    {
        await stubTimeContext.AddSecondsAsync(15);
        var test30SLivePeriodClient = new TestLivePeriodClient
            (new PricingInstrumentIdValue
                 ((SourceTickerIdentifier)tickerId30SPeriod
                , new PeriodInstrumentTypePair(InstrumentType.Candle, new DiscreetTimePeriod(ThirtySeconds))));
        var taskCompletionsSource = new TaskCompletionSource<int>();
        test30SLivePeriodClient.RegisterSubPeriodResponse
            (FifteenSeconds
           , async () =>
             {
                 await taskCompletionsSource.Task;
                 return fifteenSecondPeriodSummaries.Take(1).ToList();
             });
        await indicatorRegistryStubRule.DeployChildRuleAsync(test30SLivePeriodClient);

        var live30SRule = new LiveCandlePublisherRule<PublishableLevel1PriceQuote>(thirtySecondsLivePeriodParams);

        await using var live30SDeploy = await indicatorRegistryStubRule.DeployChildRuleAsync(live30SRule);

        stubTimeContext.IncrementedTimeCallback = async (_, _, _) =>
        {
            // Between each event fire and await the queue to run an empty callback to give rule time to update next timer;
            await live30SRule.Context.RegisteredOn.RunOnAndWait(live30SRule, () => ValueTask.CompletedTask);
        };

        var quotesToSend = oneSecondLevel1Quotes.Skip(16).Take(17).ToList();
        await test30SLivePeriodClient.SendPricesToLivePeriodRule
            ([quotesToSend.First()], stubTimeContext.ProgressTimeWithoutEvents);
        for (var i = 0; i < quotesToSend.Count; i++)
        {
            var quote = quotesToSend[i];
            test30SLivePeriodClient.CreateNewWait();
            await test30SLivePeriodClient.SendPricesToLivePeriodRule([quote], stubTimeContext);
            var receivedLivePeriods = await test30SLivePeriodClient.GetPopulatedLiveResults(i + 1);
            Assert.AreEqual(i + 1, receivedLivePeriods.Count);
        }
        taskCompletionsSource.SetResult(0);
        await stubTimeContext.AddSecondsAsync(4);
        var receivedCompletePeriods = await test30SLivePeriodClient.GetPopulatedCompleteResults();
        Assert.AreEqual(1, receivedCompletePeriods.Count);
        var summary = receivedCompletePeriods.First();
        Assert.AreEqual((ushort)0, (ushort)((uint)summary.CandleFlags >> 16));
    }

    [TestMethod]
    [Timeout(20_000)]
    public async Task NewLiveCandle_FirstSubPeriodReturnsNoSubPeriodData_PublishesQuotesWithMissingData()
    {
        await stubTimeContext.AddSecondsAsync(15);
        var test30SLivePeriodClient = new TestLivePeriodClient
            (new PricingInstrumentIdValue
                 ((SourceTickerIdentifier)tickerId30SPeriod
                , new PeriodInstrumentTypePair(InstrumentType.Candle, new DiscreetTimePeriod(ThirtySeconds))));
        test30SLivePeriodClient.RegisterSubPeriodResponse
            (FifteenSeconds, () => new ValueTask<List<Candle>>(new List<Candle>()));
        await indicatorRegistryStubRule.DeployChildRuleAsync(test30SLivePeriodClient);

        var live30SRule = new LiveCandlePublisherRule<PublishableLevel1PriceQuote>(thirtySecondsLivePeriodParams);

        await using var live30SDeploy = await indicatorRegistryStubRule.DeployChildRuleAsync(live30SRule);

        stubTimeContext.IncrementedTimeCallback = async (_, _, _) =>
        {
            // Between each event fire and await the queue to run an empty callback to give rule time to update next timer;
            await live30SRule.Context.RegisteredOn.RunOnAndWait(live30SRule, () => ValueTask.CompletedTask);
        };

        var quotesToSend = oneSecondLevel1Quotes.Skip(16).Take(17).ToList();
        await test30SLivePeriodClient.SendPricesToLivePeriodRule
            ([quotesToSend.First()], stubTimeContext.ProgressTimeWithoutEvents);
        for (var i = 0; i < quotesToSend.Count; i++)
        {
            var quote = quotesToSend[i];
            test30SLivePeriodClient.CreateNewWait();
            await test30SLivePeriodClient.SendPricesToLivePeriodRule([quote], stubTimeContext);
            var receivedLivePeriods = await test30SLivePeriodClient.GetPopulatedLiveResults(i + 1);
            Assert.AreEqual(i + 1, receivedLivePeriods.Count);
        }
        await stubTimeContext.AddSecondsAsync(4);
        var receivedCompletePeriods = await test30SLivePeriodClient.GetPopulatedCompleteResults();
        Assert.AreEqual(1, receivedCompletePeriods.Count);
        var summary = receivedCompletePeriods.First();
        Assert.AreEqual((ushort)0x01FF, (ushort)((uint)summary.CandleFlags >> 16));
    }

    [TestMethod]
    [Timeout(20_000)]
    public async Task NewLiveCandleOnStartup_ReceivesNoQuotesOrSubPeriods_PublishesNothing()
    {
        var test5SLivePeriodClient = new TestLivePeriodClient
            (new PricingInstrumentIdValue
                 ((SourceTickerIdentifier)tickerId5SPeriod
                , new PeriodInstrumentTypePair(InstrumentType.Candle, new DiscreetTimePeriod(FiveSeconds))));
        await indicatorRegistryStubRule.DeployChildRuleAsync(test5SLivePeriodClient);

        var liveResolver5SRule = new LiveCandlePublisherRule<PublishableLevel1PriceQuote>(fiveSecondsLivePeriodParams);

        await using var live5SDeploy = await indicatorRegistryStubRule.DeployChildRuleAsync(liveResolver5SRule);

        await stubTimeContext.UpdateTime(stubTimeContext.UtcNow.AddSeconds(20));

        var receivedCompletePeriods = await test5SLivePeriodClient.GetPopulatedCompleteResults(0);
        Assert.AreEqual(0, receivedCompletePeriods.Count);
        var receivedLivePeriods = await test5SLivePeriodClient.GetPopulatedLiveResults(0);
        Assert.AreEqual(0, receivedLivePeriods.Count);
    }

    [TestMethod]
    [Timeout(200_000)]
    public async Task NewLiveCandleOnStartup_ReceivesNoQuotesOrSubPeriods_StartReceivingAndPublishing()
    {
        var test5SLivePeriodClient = new TestLivePeriodClient
            (new PricingInstrumentIdValue
                 ((SourceTickerIdentifier)tickerId5SPeriod
                , new PeriodInstrumentTypePair(InstrumentType.Candle, new DiscreetTimePeriod(FiveSeconds))));
        await indicatorRegistryStubRule.DeployChildRuleAsync(test5SLivePeriodClient);

        var liveResolver5SRule = new LiveCandlePublisherRule<PublishableLevel1PriceQuote>(fiveSecondsLivePeriodParams);

        await using var live5SDeploy = await indicatorRegistryStubRule.DeployChildRuleAsync(liveResolver5SRule);

        stubTimeContext.IncrementedTimeCallback = async (_, _, _) =>
        {
            // Between each event fire and await the queue to run an empty callback to give rule time to update next timer;
            await liveResolver5SRule.Context.RegisteredOn.RunOnAndWait(liveResolver5SRule, () => ValueTask.CompletedTask);
        };
        await stubTimeContext.UpdateTime(stubTimeContext.UtcNow.AddSeconds(20));

        var receivedCompletePeriods = await test5SLivePeriodClient.GetPopulatedCompleteResults(0);
        Assert.AreEqual(0, receivedCompletePeriods.Count);
        var receivedLivePeriods = await test5SLivePeriodClient.GetPopulatedLiveResults(0);
        Assert.AreEqual(0, receivedLivePeriods.Count);

        var quotesToSend = oneSecondLevel1Quotes.Skip(22).Take(7).ToList();
        await test5SLivePeriodClient.SendPricesToLivePeriodRule
            ([quotesToSend.First()], stubTimeContext.ProgressTimeWithoutEvents);
        for (var i = 0; i < quotesToSend.Count; i++)
        {
            var quote = quotesToSend[i];
            test5SLivePeriodClient.CreateNewWait();
            Logger.DebugFormat("Sending quote {0}")?.WithOnlyParam(quote);
            await test5SLivePeriodClient.SendPricesToLivePeriodRule([quote], stubTimeContext);
            receivedLivePeriods = await test5SLivePeriodClient.GetPopulatedLiveResults(i + 1);
            Assert.AreEqual(i + 1, receivedLivePeriods.Count);
        }

        receivedCompletePeriods = await test5SLivePeriodClient.GetPopulatedCompleteResults();
        Logger.DebugFormat("Received {0} complete periods")?.WithOnlyParamCollection.Add(receivedCompletePeriods);
        Assert.AreEqual(1, receivedCompletePeriods.Count);
        receivedLivePeriods = await test5SLivePeriodClient.GetPopulatedLiveResults(7);
        Assert.AreEqual(7, receivedLivePeriods.Count);
    }

    private readonly struct PublishQuotesWithTimeProgress(List<PublishableLevel1PriceQuote> toPublish, IUpdateTime timeUpdater)
    {
        public List<PublishableLevel1PriceQuote> ToPublish { get; } = toPublish;

        public IUpdateTime TimeUpdater { get; } = timeUpdater;
    }

    private class TestLivePeriodClient
        (PricingInstrumentIdValue pricingInstrumentId, int waitNumberForCompleted = 1, int waitNumberForLive = 1) : Rule
    {
        private static readonly IVersatileFLogger TestClientLogger = FLog.FLoggerForType.As<IVersatileFLogger>();

        private const string LivePeriodTestClientPublishPricesAddress
            = "TestClient.LiveCandle.Publish.Quotes";

        private readonly Dictionary<string, TimeBoundaryPeriod> historicalSubPeriodAddressToSubPeriodLookup = new();

        private readonly Dictionary<TimeBoundaryPeriod, ISubscription> historicalSubPeriodRequestSubscriptions = new();

        private readonly Dictionary<TimeBoundaryPeriod, Func<ValueTask<List<Candle>>>> historicalSubPeriodResponseCallbacks = new();

        private readonly Dictionary<string, TimeBoundaryPeriod> liveSubPeriodAddressToSubReceivedHistorical = new();

        private readonly Dictionary<TimeBoundaryPeriod, ISubscription> liveSubPeriodCompletePublisherSubscriptions = new();

        private readonly Dictionary<TimeBoundaryPeriod, List<Candle>> receivedCompleteSubPeriods = new();

        private TaskCompletionSource<int> awaitCompleteSource = new();
        private TaskCompletionSource<int> awaitLiveSource     = new();

        private ISubscription? completePublishSubscription;
        private ISubscription? listenForPublishPricesSubscription;
        private ISubscription? livePublishSubscription;

        private string quoteListenAddress     = null!;
        
        private int    waitNumberForCompleted = waitNumberForCompleted;
        private int    waitNumberForLive      = waitNumberForLive;

        private List<ICandle> ReceivedLivePublishEvents { get; } = new();

        private List<ICandle> ReceivedCompletePublishEvents { get; } = new();

        public List<HistoricalCandleResponseRequest> ReceivedSubPeriodHistoricalRequests { get; } = new();

        public override async ValueTask StartAsync()
        {
            TestClientLogger.DebugFormat("Starting TestLivePeriodClient with {0}")?.WithOnlyParamCollection.Add(ReceivedCompletePublishEvents);
            quoteListenAddress = pricingInstrumentId.SourceName.SubscribeToTickerQuotes(pricingInstrumentId.InstrumentName);
            listenForPublishPricesSubscription = await this.RegisterRequestListenerAsync<PublishQuotesWithTimeProgress, ValueTask>
                (LivePeriodTestClientPublishPricesAddress, PublishPriceQuotesHandler);
            livePublishSubscription = await this.RegisterListenerAsync<Candle>
                (pricingInstrumentId.LiveCandleAddress(), ReceivedLiveCandles);
            completePublishSubscription = await this.RegisterListenerAsync<Candle>
                (pricingInstrumentId.CompleteCandleAddress(), ReceivedCompleteCandles);
            foreach (var subPeriod in pricingInstrumentId.CoveringPeriod.Period.WholeSecondConstructingDivisiblePeriods()
                                                         .Where(tsp => tsp >= CandleConstants.PersistPeriodsFrom))
            {
                TestClientLogger.Dbg(((SourceTickerIdentifier)pricingInstrumentId).HistoricalCandleResponseRequest(subPeriod));
                TestClientLogger.DbgFmt("Subscribing to {0}")?.Args(((SourceTickerIdentifier)pricingInstrumentId).HistoricalCandleResponseRequest(subPeriod));
                historicalSubPeriodRequestSubscriptions.Add
                    (subPeriod, await this.RegisterRequestListenerAsync<HistoricalCandleResponseRequest, List<Candle>>
                         (((SourceTickerIdentifier)pricingInstrumentId).HistoricalCandleResponseRequest(subPeriod)
                        , ReceivedHistoricalSubPeriodResponseRequest));
                historicalSubPeriodAddressToSubPeriodLookup
                    .Add(((SourceTickerIdentifier)pricingInstrumentId).HistoricalCandleResponseRequest(subPeriod), subPeriod);
                TestClientLogger.Dbg(((SourceTickerIdentifier)pricingInstrumentId).CompleteCandleAddress(subPeriod));
                TestClientLogger.DbgFmt("Subscribing to {0}")?.Args(((SourceTickerIdentifier)pricingInstrumentId).CompleteCandleAddress(subPeriod));
                liveSubPeriodCompletePublisherSubscriptions.Add
                    (subPeriod, await this.RegisterListenerAsync<Candle>
                         (((SourceTickerIdentifier)pricingInstrumentId).CompleteCandleAddress(subPeriod)
                        , ReceivedCompleteSubCandles));
                liveSubPeriodAddressToSubReceivedHistorical.Add
                    (((SourceTickerIdentifier)pricingInstrumentId).CompleteCandleAddress(subPeriod), subPeriod);
            }
            await base.StartAsync();
            TestClientLogger.DebugFormat("Started TestLivePeriodClient with {0}")?.WithOnlyParamCollection.Add(ReceivedCompletePublishEvents);
        }

        public void CreateNewWait(int waitForComplete = 1, int waitForLive = 1)
        {
            waitNumberForCompleted = waitForComplete;
            waitNumberForLive      = waitForLive;
            awaitCompleteSource    = new TaskCompletionSource<int>();
            awaitLiveSource        = new TaskCompletionSource<int>();
        }

        public async ValueTask<List<ICandle>> GetPopulatedLiveResults(int waitNumber = 1)
        {
            TestClientLogger.DebugFormat("Starting GetPopulatedLiveResults {0} complete periods waiting for number {1}")?.WithParamsCollection
                            .Add(ReceivedLivePublishEvents).AndFinalParam(waitNumber);
            waitNumberForLive = waitNumber;
            if (ReceivedLivePublishEvents.Count < waitNumber) await Task.WhenAny(awaitLiveSource.Task, Task.Delay(2_000));
            TestClientLogger.DebugFormat("On Event or timeout GetPopulatedLiveResults {0} complete periods for number {1}")?.WithParamsCollection
                            .Add(ReceivedLivePublishEvents).AndFinalParam(waitNumber);
            return ReceivedLivePublishEvents;
        }

        public async ValueTask<List<ICandle>> GetPopulatedCompleteResults(int waitNumber = 1)
        {
            TestClientLogger.DebugFormat("Starting GetPopulatedCompleteResults {0} complete periods waiting for number {1}")?.WithParamsCollection
                            .Add(ReceivedCompletePublishEvents).AndFinalParam(waitNumber);
            waitNumberForCompleted = waitNumber;
            if (ReceivedCompletePublishEvents.Count < waitNumber) await Task.WhenAny(awaitCompleteSource.Task, Task.Delay(2_000));
            TestClientLogger.DebugFormat("On Event or timeout GetPopulatedCompleteResults {0} complete periods for number {1}")?.WithParamsCollection
                            .Add(ReceivedCompletePublishEvents).AndFinalParam(waitNumber);
            return ReceivedCompletePublishEvents;
        }

        private void ReceivedLiveCandles(Candle candle)
        {
            ReceivedLivePublishEvents.Add(candle);
            if (ReceivedLivePublishEvents.Count >= waitNumberForLive) awaitLiveSource.TrySetResult(0);
            TestClientLogger.DebugFormat("TestLivePeriodClient Received Live {0} current count {1}")?.WithParams(candle)?.AndFinalParam(ReceivedLivePublishEvents.Count);
        }

        private void ReceivedCompleteCandles(Candle candle)
        {
            ReceivedCompletePublishEvents.Add(candle);
            if (ReceivedCompletePublishEvents.Count >= waitNumberForCompleted) awaitCompleteSource.TrySetResult(0);
            TestClientLogger.DebugFormat("TestLivePeriodClient Complete Live {0} current count {1}")?.WithParams(candle)?.AndFinalParam(ReceivedCompletePublishEvents.Count);
        }

        private async ValueTask<List<Candle>> ReceivedHistoricalSubPeriodResponseRequest
            (IBusRespondingMessage<HistoricalCandleResponseRequest, List<Candle>> completePublishMsg)
        {
            var historicalSubPeriodRequest = completePublishMsg.Payload.Body();
            TestClientLogger.DebugAppend("TestLivePeriodClient Received Sub Period Response for Request: ")
                  ?.FinalAppend(historicalSubPeriodRequest, HistoricalCandleResponseRequest.Styler);
            ReceivedSubPeriodHistoricalRequests.Add(historicalSubPeriodRequest);
            var subPeriod = historicalSubPeriodAddressToSubPeriodLookup[completePublishMsg.DestinationAddress!];
            if (!historicalSubPeriodResponseCallbacks.TryGetValue(subPeriod, out var resultCallback))
                resultCallback = () => new ValueTask<List<Candle>>(new List<Candle>());
            return await resultCallback();
        }

        private void ReceivedCompleteSubCandles(IBusMessage<Candle> livePublishMsg)
        {
            var candle = livePublishMsg.Payload.Body();
            TestClientLogger.DebugAppend("TestLivePeriodClient Received Completed Sub : ")?.FinalAppend(candle);
            ReceivedLivePublishEvents.Add(candle);
            var subPeriod = liveSubPeriodAddressToSubReceivedHistorical[livePublishMsg.DestinationAddress!];
            if (!receivedCompleteSubPeriods.TryGetValue(subPeriod, out var resultCallback))
            {
                resultCallback = new List<Candle>();
                receivedCompleteSubPeriods.Add(subPeriod, resultCallback);
            }
            resultCallback.Add(candle);
        }

        public void RegisterSubPeriodResponse(TimeBoundaryPeriod subPeriod, Func<ValueTask<List<Candle>>> returnedResults)
        {
            TestClientLogger.DebugAppend("TestLivePeriodClient Received Sub Period Response for Request: ")?.FinalAppend(subPeriod);
            historicalSubPeriodResponseCallbacks.Add(subPeriod, returnedResults);
        }

        public async ValueTask SendPricesToLivePeriodRule(List<PublishableLevel1PriceQuote> publishPrices, IUpdateTime progressTime)
        {
            TestClientLogger.DebugAppend("TestLivePeriodClient SendPricesToLivePeriodRule: ")?.FinalAppendCollection.Add(publishPrices);
            await this.RequestAsync<PublishQuotesWithTimeProgress, ValueTask>
                (LivePeriodTestClientPublishPricesAddress, new PublishQuotesWithTimeProgress(publishPrices, progressTime));
        }

        private async ValueTask PublishPriceQuotesHandler(PublishQuotesWithTimeProgress toPublishListAndTimeUpdater)
        {
            var toPublishList = toPublishListAndTimeUpdater.ToPublish;
            var timeUpdater   = toPublishListAndTimeUpdater.TimeUpdater;
            foreach (var level1PriceQuote in toPublishList)
            {
                await timeUpdater.UpdateTime(level1PriceQuote.SourceTime);
                await this.PublishAsync(quoteListenAddress, level1PriceQuote);
            }
        }

        public override async ValueTask StopAsync()
        {
            TestClientLogger.Debug("Stopping TestLivePeriodClient ");
            await livePublishSubscription.NullSafeUnsubscribe();
            await listenForPublishPricesSubscription.NullSafeUnsubscribe();
            await completePublishSubscription.NullSafeUnsubscribe();
            foreach (var listenHistoricalSub in historicalSubPeriodRequestSubscriptions.Values) await listenHistoricalSub.NullSafeUnsubscribe();
            foreach (var listenLiveSub in liveSubPeriodCompletePublisherSubscriptions.Values) await listenLiveSub.NullSafeUnsubscribe();
            await base.StopAsync();
            TestClientLogger.Debug("Stopped TestLivePeriodClient ");
        }
    }
}
