// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.BusMessaging.Messages.ListeningSubscriptions;
using FortitudeBusRules.BusMessaging.Routing.Response;
using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;
using FortitudeCommon.Chronometry;
using FortitudeCommon.Chronometry.Timers;
using FortitudeIO.TimeSeries;
using FortitudeMarketsApi.Pricing;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsApi.Pricing.Summaries;
using FortitudeMarketsCore.Indicators;
using FortitudeMarketsCore.Indicators.Pricing;
using FortitudeMarketsCore.Indicators.Pricing.PeriodSummaries;
using FortitudeMarketsCore.Indicators.Pricing.PeriodSummaries.Construction;
using FortitudeMarketsCore.Pricing.PQ.Subscription.BusRules;
using FortitudeMarketsCore.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.Summaries;
using FortitudeTests.FortitudeBusRules.BusMessaging;
using FortitudeTests.FortitudeCommon.Chronometry;
using FortitudeTests.FortitudeCommon.Chronometry.Timers;
using FortitudeTests.FortitudeMarketsCore.Indicators.Config;
using FortitudeTests.FortitudeMarketsCore.Pricing.Quotes;
using static FortitudeIO.TimeSeries.TimeSeriesPeriod;
using static FortitudeMarketsApi.Configuration.ClientServerConfig.MarketClassificationExtensions;
using static FortitudeTests.FortitudeMarketsCore.Pricing.Summaries.PricePeriodSummaryTests;
using static FortitudeMarketsApi.Pricing.Quotes.QuoteLevel;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Indicators.Pricing.PeriodSummaries;

[TestClass]
public class LivePricePeriodSummaryPublisherRuleTests : OneOfEachMessageQueueTypeTestSetup
{
    private readonly DateTime testEpochTime = new(2024, 7, 11);

    private readonly SourceTickerQuoteInfo tickerId15SPeriod = new
        (2, "SourceName", 2, "TickerName2", Level1, Unknown
       , 1, 0.001m, 10m, 100m, 10m);

    private readonly SourceTickerQuoteInfo tickerId1MPeriod = new
        (2, "SourceName", 2, "TickerName4", Level1, Unknown
       , 1, 0.001m, 10m, 100m, 10m);

    private readonly SourceTickerQuoteInfo tickerId30SPeriod = new
        (3, "SourceName", 3, "TickerName3", Level1, Unknown
       , 1, 0.001m, 10m, 100m, 10m);

    private readonly SourceTickerQuoteInfo tickerId5SPeriod = new
        (1, "SourceName", 1, "TickerName1", Level1, Unknown
       , 1, 0.001m, 10m, 100m, 10m);

    private List<PricePeriodSummary>            fifteenSecondPeriodSummaries = null!;
    private LivePublishPricePeriodSummaryParams fifteenSecondsLivePeriodParams;

    private LivePublishPricePeriodSummaryParams fiveSecondsLivePeriodParams;

    private decimal highLowSpread;

    private IndicatorServiceRegistryStubRule indicatorRegistryStubRule = null!;

    private decimal mid1;
    private decimal mid2;

    private LivePublishPricePeriodSummaryParams oneMinuteLivePeriodParams;

    private List<Level1PriceQuote> oneSecondLevel1Quotes = null!;

    private decimal spread;

    private StubTimeContext                     stubTimeContext             = null!;
    private List<PricePeriodSummary>            thirtySecondPeriodSummaries = null!;
    private LivePublishPricePeriodSummaryParams thirtySecondsLivePeriodParams;

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

        fiveSecondsLivePeriodParams = new LivePublishPricePeriodSummaryParams
            ((SourceTickerIdentifier)tickerId5SPeriod, FiveSeconds, new IndicatorPublishInterval(TimeSpan.FromSeconds(1))
           , new ResponsePublishParams());
        fifteenSecondsLivePeriodParams
            = new LivePublishPricePeriodSummaryParams(tickerId15SPeriod, FifteenSeconds, new IndicatorPublishInterval(TimeSpan.FromSeconds(1))
                                                    , new ResponsePublishParams());
        thirtySecondsLivePeriodParams = new LivePublishPricePeriodSummaryParams
            (tickerId30SPeriod, ThirtySeconds, new IndicatorPublishInterval(TimeSpan.FromSeconds(1)), new ResponsePublishParams());
        oneMinuteLivePeriodParams = new LivePublishPricePeriodSummaryParams
            (tickerId1MPeriod, OneMinute, new IndicatorPublishInterval(TimeSpan.FromSeconds(1)), new ResponsePublishParams());
        var unitTestNoRepositoryConfig = IndicatorServicesConfigTests.UnitTestNoRepositoryConfig();
        unitTestNoRepositoryConfig.PersistenceConfig.PersistPriceSummaries = true;
        indicatorRegistryStubRule
            = new IndicatorServiceRegistryStubRule
                (new IndicatorServiceRegistryParams(unitTestNoRepositoryConfig));

        var preqDeploy
            = await EventQueue1.LaunchRuleAsync
                (indicatorRegistryStubRule, indicatorRegistryStubRule, EventQueue1SelectionResult);
        undeploy.Add(preqDeploy);
        indicatorRegistryStubRule.RegisterGlobalServiceStatus(ServiceType.PricePeriodSummaryFilePersister, ServiceRunStatus.ServiceStarted);
        indicatorRegistryStubRule.RegisterTickerPeriodServiceStatus
            (tickerId30SPeriod, FifteenSeconds, ServiceType.LivePricePeriodSummary, ServiceRunStatus.ServiceStarted);
        indicatorRegistryStubRule.RegisterTickerPeriodServiceStatus
            (tickerId1MPeriod, FifteenSeconds, ServiceType.LivePricePeriodSummary, ServiceRunStatus.ServiceStarted);
        indicatorRegistryStubRule.RegisterTickerPeriodServiceStatus
            (tickerId1MPeriod, ThirtySeconds, ServiceType.LivePricePeriodSummary, ServiceRunStatus.ServiceStarted);
        indicatorRegistryStubRule.RegisterTickerPeriodServiceStatus
            (tickerId30SPeriod, FifteenSeconds, ServiceType.HistoricalPricePeriodSummaryResolver, ServiceRunStatus.ServiceStarted);
        indicatorRegistryStubRule.RegisterTickerPeriodServiceStatus
            (tickerId1MPeriod, FifteenSeconds, ServiceType.HistoricalPricePeriodSummaryResolver, ServiceRunStatus.ServiceStarted);
        indicatorRegistryStubRule.RegisterTickerPeriodServiceStatus
            (tickerId1MPeriod, ThirtySeconds, ServiceType.HistoricalPricePeriodSummaryResolver, ServiceRunStatus.ServiceStarted);
    }

    public override ITimerProvider ResolverTimerProvider()
    {
        TimeContext.Provider = stubTimeContext = new StubTimeContext(testEpochTime);
        return new StubTimerContextProvider();
    }

    private void Generate1SQuotes(int numberToGenerate = 120)
    {
        var entriesStartTime = OneSecond.PreviousPeriodStart(testEpochTime);

        oneSecondLevel1Quotes = new List<Level1PriceQuote>(numberToGenerate);
        for (var i = 0; i < numberToGenerate; i++)
            oneSecondLevel1Quotes.Add
                (tickerId15SPeriod.CreateLevel1Quote(entriesStartTime = OneSecond.PeriodEnd(entriesStartTime), i % 2 == 0 ? mid1 : mid2, spread));
    }

    private void Generate15SSummaries(int numberToGenerate = 8)
    {
        var entriesStartTime = FifteenSeconds.PreviousPeriodStart(testEpochTime);

        fifteenSecondPeriodSummaries = new List<PricePeriodSummary>(numberToGenerate);
        for (var i = 0; i < numberToGenerate; i++)
            fifteenSecondPeriodSummaries.Add
                (CreatePricePeriodSummary
                    (FifteenSeconds, entriesStartTime = FifteenSeconds.PeriodEnd(entriesStartTime), i % 2 == 0 ? mid1 : mid2, spread, highLowSpread));
    }

    private void Generate30SSummaries(int numberToGenerate = 4)
    {
        var entriesStartTime = ThirtySeconds.PreviousPeriodStart(testEpochTime);

        thirtySecondPeriodSummaries = new List<PricePeriodSummary>(numberToGenerate);
        for (var i = 0; i < numberToGenerate; i++)
            thirtySecondPeriodSummaries.Add
                (CreatePricePeriodSummary
                    (ThirtySeconds, entriesStartTime = ThirtySeconds.PeriodEnd(entriesStartTime), i % 2 == 0 ? mid1 : mid2, spread, highLowSpread));
    }


    [TestCleanup]
    public async Task TearDown()
    {
        await stubTimeContext.DisposeAsync();
        foreach (var asyncDisposable in undeploy) await asyncDisposable.DisposeAsync();
    }

    [TestMethod]
    public async Task NewLivePeriodSummary_SendTwoPeriodsOfQuotes_PublishingNextTickSendsCompleteOfFirstPeriod()
    {
        var test5SLivePeriodClient = new TestLivePeriodClient
            (new PricingInstrumentId
                ((SourceTickerIdentifier)tickerId5SPeriod
               , new PeriodInstrumentTypePair(InstrumentType.PriceSummaryPeriod, FiveSeconds)));
        await indicatorRegistryStubRule.DeployRuleAsync(test5SLivePeriodClient);

        var liveResolver5SRule = new LivePricePeriodSummaryPublisherRule<Level1PriceQuote>(fiveSecondsLivePeriodParams);

        await using var histResolverDeploy = await indicatorRegistryStubRule.DeployRuleAsync(liveResolver5SRule);

        var progressTime = stubTimeContext.ProgressTimeWithoutEvents;
        await test5SLivePeriodClient.SendPricesToLivePeriodRule(oneSecondLevel1Quotes.Take(12).ToList(), progressTime);

        var receivedCompletePeriods = await test5SLivePeriodClient.GetPopulatedCompleteResults();
        Assert.AreEqual(1, receivedCompletePeriods.Count);
    }

    [TestMethod]
    public async Task NewLivePeriodSummary_SendOnePeriodsOfQuotes_IncrementingTimeTriggersOnCompleteTimer()
    {
        var test5SLivePeriodClient = new TestLivePeriodClient
            (new PricingInstrumentId
                ((SourceTickerIdentifier)tickerId5SPeriod
               , new PeriodInstrumentTypePair(InstrumentType.PriceSummaryPeriod, FiveSeconds)));
        await indicatorRegistryStubRule.DeployRuleAsync(test5SLivePeriodClient);

        var liveResolver5SRule = new LivePricePeriodSummaryPublisherRule<Level1PriceQuote>(fiveSecondsLivePeriodParams);

        await using var live5SDeploy = await indicatorRegistryStubRule.DeployRuleAsync(liveResolver5SRule);

        stubTimeContext.IncrementedTimeCallback = async (_, _, _) =>
        {
            // Between each event fire and await the queue to run an empty callback to give rule time to update next timer;
            await liveResolver5SRule.Context.RegisteredOn.RunOnAndWait(liveResolver5SRule, () => ValueTask.CompletedTask);
        };
        await test5SLivePeriodClient.SendPricesToLivePeriodRule(oneSecondLevel1Quotes.Take(9).ToList(), stubTimeContext);

        var receivedCompletePeriods = await test5SLivePeriodClient.GetPopulatedCompleteResults(1);
        Assert.AreEqual(1, receivedCompletePeriods.Count);
        var receivedLivePeriods = await test5SLivePeriodClient.GetPopulatedLiveResults(9);
        Assert.AreEqual(8, receivedLivePeriods.Count);
    }

    [TestMethod]
    public async Task NewLivePeriodSummary_SendSingleQuotes_LivePeriodUpdateReceivedBetweenEach()
    {
        var test5SLivePeriodClient = new TestLivePeriodClient
            (new PricingInstrumentId
                ((SourceTickerIdentifier)tickerId5SPeriod
               , new PeriodInstrumentTypePair(InstrumentType.PriceSummaryPeriod, FiveSeconds)));
        await indicatorRegistryStubRule.DeployRuleAsync(test5SLivePeriodClient);

        var liveResolver5SRule = new LivePricePeriodSummaryPublisherRule<Level1PriceQuote>(fiveSecondsLivePeriodParams);

        await using var live5SDeploy = await indicatorRegistryStubRule.DeployRuleAsync(liveResolver5SRule);

        stubTimeContext.IncrementedTimeCallback = async (_, _, _) =>
        {
            // Between each event fire and await the queue to run an empty callback to give rule time to update next timer;
            await liveResolver5SRule.Context.RegisteredOn.RunOnAndWait(liveResolver5SRule, () => ValueTask.CompletedTask);
        };
        var quotesToSend = oneSecondLevel1Quotes.Skip(1).Take(6).ToList();
        await test5SLivePeriodClient.SendPricesToLivePeriodRule
            (new List<Level1PriceQuote> { quotesToSend.First() }, stubTimeContext.ProgressTimeWithoutEvents);
        for (var i = 0; i < quotesToSend.Count; i++)
        {
            var quote = quotesToSend[i];
            test5SLivePeriodClient.CreateNewWait();
            await test5SLivePeriodClient.SendPricesToLivePeriodRule(new List<Level1PriceQuote> { quote }, stubTimeContext);
            var receivedLivePeriods = await test5SLivePeriodClient.GetPopulatedLiveResults(i + 1);
            Assert.AreEqual(i + 1, receivedLivePeriods.Count, $"For Loop {i} ");
        }
        await stubTimeContext.AddSecondsAsync(2);
        var receivedCompletePeriods = await test5SLivePeriodClient.GetPopulatedCompleteResults(1);
        Assert.AreEqual(1, receivedCompletePeriods.Count);
    }

    [TestMethod]
    public async Task NewLivePeriodSummary_RequestsSubPeriodReturnsWithinSamePeriod_PublishesQuotesToNextPeriod()
    {
        await stubTimeContext.AddSecondsAsync(45);
        var test1MLivePeriodClient = new TestLivePeriodClient
            (new PricingInstrumentId
                ((SourceTickerIdentifier)tickerId1MPeriod
               , new PeriodInstrumentTypePair(InstrumentType.PriceSummaryPeriod, OneMinute)));
        test1MLivePeriodClient.RegisterSubPeriodResponse
            (FifteenSeconds
           , () => new ValueTask<List<PricePeriodSummary>>(fifteenSecondPeriodSummaries.Skip(2).Take(1).ToList()));
        test1MLivePeriodClient.RegisterSubPeriodResponse
            (ThirtySeconds
           , () => new ValueTask<List<PricePeriodSummary>>(thirtySecondPeriodSummaries.Take(1).ToList()));
        await indicatorRegistryStubRule.DeployRuleAsync(test1MLivePeriodClient);

        var live1MRule = new LivePricePeriodSummaryPublisherRule<Level1PriceQuote>(oneMinuteLivePeriodParams);

        await using var live1MDeploy = await indicatorRegistryStubRule.DeployRuleAsync(live1MRule);

        stubTimeContext.IncrementedTimeCallback = async (_, _, _) =>
        {
            // Between each event fire and await the queue to run an empty callback to give rule time to update next timer;
            await live1MRule.Context.RegisteredOn.RunOnAndWait(live1MRule, () => ValueTask.CompletedTask);
        };

        var quotesToSend = oneSecondLevel1Quotes.Skip(46).Take(14).ToList();
        await test1MLivePeriodClient.SendPricesToLivePeriodRule
            (new List<Level1PriceQuote> { quotesToSend.First() }, stubTimeContext.ProgressTimeWithoutEvents);
        for (var i = 0; i < quotesToSend.Count; i++)
        {
            var quote = quotesToSend[i];
            test1MLivePeriodClient.CreateNewWait();
            await test1MLivePeriodClient.SendPricesToLivePeriodRule(new List<Level1PriceQuote> { quote }, stubTimeContext);
            var receivedLivePeriods = await test1MLivePeriodClient.GetPopulatedLiveResults(i + 1);
            Assert.AreEqual(i + 1, receivedLivePeriods.Count);
        }
        await stubTimeContext.AddSecondsAsync(6);
        var receivedCompletePeriods = await test1MLivePeriodClient.GetPopulatedCompleteResults(1);
        Assert.AreEqual(1, receivedCompletePeriods.Count);
        var summary = receivedCompletePeriods.First();
        Assert.AreEqual((ushort)0, (ushort)((uint)summary.PeriodSummaryFlags >> 16));
    }

    [TestMethod]
    public async Task NewLivePeriodSummary_RequestsSubPeriodReturnsWithinNextPeriod_PublishesQuotesToNextPeriod()
    {
        await stubTimeContext.AddSecondsAsync(15);
        var test30SLivePeriodClient = new TestLivePeriodClient
            (new PricingInstrumentId
                ((SourceTickerIdentifier)tickerId30SPeriod
               , new PeriodInstrumentTypePair(InstrumentType.PriceSummaryPeriod, ThirtySeconds)));
        var taskCompletionsSource = new TaskCompletionSource<int>();
        test30SLivePeriodClient.RegisterSubPeriodResponse
            (FifteenSeconds
           , async () =>
             {
                 await taskCompletionsSource.Task;
                 return fifteenSecondPeriodSummaries.Take(1).ToList();
             });
        await indicatorRegistryStubRule.DeployRuleAsync(test30SLivePeriodClient);

        var live30SRule = new LivePricePeriodSummaryPublisherRule<Level1PriceQuote>(thirtySecondsLivePeriodParams);

        await using var live30SDeploy = await indicatorRegistryStubRule.DeployRuleAsync(live30SRule);

        stubTimeContext.IncrementedTimeCallback = async (_, _, _) =>
        {
            // Between each event fire and await the queue to run an empty callback to give rule time to update next timer;
            await live30SRule.Context.RegisteredOn.RunOnAndWait(live30SRule, () => ValueTask.CompletedTask);
        };

        var quotesToSend = oneSecondLevel1Quotes.Skip(16).Take(17).ToList();
        await test30SLivePeriodClient.SendPricesToLivePeriodRule
            (new List<Level1PriceQuote> { quotesToSend.First() }, stubTimeContext.ProgressTimeWithoutEvents);
        for (var i = 0; i < quotesToSend.Count; i++)
        {
            var quote = quotesToSend[i];
            test30SLivePeriodClient.CreateNewWait();
            await test30SLivePeriodClient.SendPricesToLivePeriodRule(new List<Level1PriceQuote> { quote }, stubTimeContext);
            var receivedLivePeriods = await test30SLivePeriodClient.GetPopulatedLiveResults(i + 1);
            Assert.AreEqual(i + 1, receivedLivePeriods.Count);
        }
        taskCompletionsSource.SetResult(0);
        await stubTimeContext.AddSecondsAsync(4);
        var receivedCompletePeriods = await test30SLivePeriodClient.GetPopulatedCompleteResults(1);
        Assert.AreEqual(1, receivedCompletePeriods.Count);
        var summary = receivedCompletePeriods.First();
        Assert.AreEqual((ushort)0, (ushort)((uint)summary.PeriodSummaryFlags >> 16));
    }

    [TestMethod]
    public async Task NewLivePeriodSummary_FirstSubPeriodReturnsNoSubPeriodData_PublishesQuotesWithMissingData()
    {
        await stubTimeContext.AddSecondsAsync(15);
        var test30SLivePeriodClient = new TestLivePeriodClient
            (new PricingInstrumentId
                ((SourceTickerIdentifier)tickerId30SPeriod
               , new PeriodInstrumentTypePair(InstrumentType.PriceSummaryPeriod, ThirtySeconds)));
        test30SLivePeriodClient.RegisterSubPeriodResponse
            (FifteenSeconds, () => new ValueTask<List<PricePeriodSummary>>(new List<PricePeriodSummary>()));
        await indicatorRegistryStubRule.DeployRuleAsync(test30SLivePeriodClient);

        var live30SRule = new LivePricePeriodSummaryPublisherRule<Level1PriceQuote>(thirtySecondsLivePeriodParams);

        await using var live30SDeploy = await indicatorRegistryStubRule.DeployRuleAsync(live30SRule);

        stubTimeContext.IncrementedTimeCallback = async (_, _, _) =>
        {
            // Between each event fire and await the queue to run an empty callback to give rule time to update next timer;
            await live30SRule.Context.RegisteredOn.RunOnAndWait(live30SRule, () => ValueTask.CompletedTask);
        };

        var quotesToSend = oneSecondLevel1Quotes.Skip(16).Take(17).ToList();
        await test30SLivePeriodClient.SendPricesToLivePeriodRule
            (new List<Level1PriceQuote> { quotesToSend.First() }, stubTimeContext.ProgressTimeWithoutEvents);
        for (var i = 0; i < quotesToSend.Count; i++)
        {
            var quote = quotesToSend[i];
            test30SLivePeriodClient.CreateNewWait();
            await test30SLivePeriodClient.SendPricesToLivePeriodRule(new List<Level1PriceQuote> { quote }, stubTimeContext);
            var receivedLivePeriods = await test30SLivePeriodClient.GetPopulatedLiveResults(i + 1);
            Assert.AreEqual(i + 1, receivedLivePeriods.Count);
        }
        await stubTimeContext.AddSecondsAsync(4);
        var receivedCompletePeriods = await test30SLivePeriodClient.GetPopulatedCompleteResults(1);
        Assert.AreEqual(1, receivedCompletePeriods.Count);
        var summary = receivedCompletePeriods.First();
        Assert.AreEqual((ushort)0x01FF, (ushort)((uint)summary.PeriodSummaryFlags >> 16));
    }

    [TestMethod]
    public async Task NewLivePeriodSummaryOnStartup_ReceivesNoQuotesOrSubPeriods_PublishesNothing()
    {
        var test5SLivePeriodClient = new TestLivePeriodClient
            (new PricingInstrumentId
                ((SourceTickerIdentifier)tickerId5SPeriod
               , new PeriodInstrumentTypePair(InstrumentType.PriceSummaryPeriod, FiveSeconds)));
        await indicatorRegistryStubRule.DeployRuleAsync(test5SLivePeriodClient);

        var liveResolver5SRule = new LivePricePeriodSummaryPublisherRule<Level1PriceQuote>(fiveSecondsLivePeriodParams);

        await using var live5SDeploy = await indicatorRegistryStubRule.DeployRuleAsync(liveResolver5SRule);

        await stubTimeContext.UpdateTime(stubTimeContext.UtcNow.AddSeconds(20));

        var receivedCompletePeriods = await test5SLivePeriodClient.GetPopulatedCompleteResults(0);
        Assert.AreEqual(0, receivedCompletePeriods.Count);
        var receivedLivePeriods = await test5SLivePeriodClient.GetPopulatedLiveResults(0);
        Assert.AreEqual(0, receivedLivePeriods.Count);
    }

    [TestMethod]
    public async Task NewLivePeriodSummaryOnStartup_ReceivesNoQuotesOrSubPeriods_StartReceivingAndPublishing()
    {
        var test5SLivePeriodClient = new TestLivePeriodClient
            (new PricingInstrumentId
                ((SourceTickerIdentifier)tickerId5SPeriod
               , new PeriodInstrumentTypePair(InstrumentType.PriceSummaryPeriod, FiveSeconds)));
        await indicatorRegistryStubRule.DeployRuleAsync(test5SLivePeriodClient);

        var liveResolver5SRule = new LivePricePeriodSummaryPublisherRule<Level1PriceQuote>(fiveSecondsLivePeriodParams);

        await using var live5SDeploy = await indicatorRegistryStubRule.DeployRuleAsync(liveResolver5SRule);

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
            (new List<Level1PriceQuote> { quotesToSend.First() }, stubTimeContext.ProgressTimeWithoutEvents);
        for (var i = 0; i < quotesToSend.Count; i++)
        {
            var quote = quotesToSend[i];
            test5SLivePeriodClient.CreateNewWait();
            await test5SLivePeriodClient.SendPricesToLivePeriodRule(new List<Level1PriceQuote> { quote }, stubTimeContext);
            receivedLivePeriods = await test5SLivePeriodClient.GetPopulatedLiveResults(i + 1);
            Assert.AreEqual(i + 1, receivedLivePeriods.Count);
        }

        receivedCompletePeriods = await test5SLivePeriodClient.GetPopulatedCompleteResults(1);
        Assert.AreEqual(1, receivedCompletePeriods.Count);
        receivedLivePeriods = await test5SLivePeriodClient.GetPopulatedLiveResults(7);
        Assert.AreEqual(7, receivedLivePeriods.Count);
    }

    private struct PublishQuotesWithTimeProgress
    {
        public PublishQuotesWithTimeProgress(List<Level1PriceQuote> toPublish, IUpdateTime timeUpdater)
        {
            TimeUpdater = timeUpdater;
            ToPublish   = toPublish;
        }

        public List<Level1PriceQuote> ToPublish   { get; }
        public IUpdateTime            TimeUpdater { get; }
    }

    private class TestLivePeriodClient
        (PricingInstrumentId pricingInstrumentId, int waitNumberForCompleted = 1, int waitNumberForLive = 1) : Rule
    {
        private const string LivePeriodTestClientPublishPricesAddress
            = "TestClient.LivePricePeriodSummary.Publish.Quotes";

        private readonly Dictionary<string, TimeSeriesPeriod>        historicalSubPeriodAddressToSubPeriodLookup = new();
        private readonly Dictionary<TimeSeriesPeriod, ISubscription> historicalSubPeriodRequestSubscriptions     = new();

        private readonly Dictionary<TimeSeriesPeriod, Func<ValueTask<List<PricePeriodSummary>>>> historicalSubPeriodResponseCallbacks        = new();
        private readonly Dictionary<string, TimeSeriesPeriod>                                    liveSubPeriodAddressToSubReceivedHistorical = new();

        private readonly Dictionary<TimeSeriesPeriod, ISubscription>            liveSubPeriodCompletePublisherSubscriptions = new();
        private readonly Dictionary<TimeSeriesPeriod, List<PricePeriodSummary>> receivedCompleteSubPeriods                  = new();

        private TaskCompletionSource<int> awaitCompleteSource = new();
        private TaskCompletionSource<int> awaitLiveSource     = new();

        private ISubscription? completePublishSubscription;
        private ISubscription? listenForPublishPricesSubscription;
        private ISubscription? livePublishSubscription;

        private string quoteListenAddress = null!;

        private List<IPricePeriodSummary> ReceivedLivePublishEvents { get; } = new();

        private List<IPricePeriodSummary> ReceivedCompletePublishEvents { get; } = new();

        public List<HistoricalPeriodResponseRequest> ReceivedSubPeriodHistoricalRequests { get; } = new();

        public override async ValueTask StartAsync()
        {
            quoteListenAddress = pricingInstrumentId.Source.SubscribeToTickerQuotes(pricingInstrumentId.Ticker);
            listenForPublishPricesSubscription = await this.RegisterRequestListenerAsync<PublishQuotesWithTimeProgress, ValueTask>
                (LivePeriodTestClientPublishPricesAddress, PublishPriceQuotesHandler);
            livePublishSubscription = await this.RegisterListenerAsync<PricePeriodSummary>
                (pricingInstrumentId.LivePeriodSummaryAddress(), ReceivedLivePricePeriodSummaries);
            completePublishSubscription = await this.RegisterListenerAsync<PricePeriodSummary>
                (pricingInstrumentId.CompletePeriodSummaryAddress(), ReceivedCompletePricePeriodSummaries);
            foreach (var subPeriod in pricingInstrumentId.EntryPeriod.ConstructingDivisiblePeriods()
                                                         .Where(tsp => tsp >= PricePeriodSummaryConstants.PersistPeriodsFrom))
            {
                historicalSubPeriodRequestSubscriptions.Add
                    (subPeriod, await this.RegisterRequestListenerAsync<HistoricalPeriodResponseRequest, List<PricePeriodSummary>>
                        (((SourceTickerIdentifier)pricingInstrumentId).HistoricalPeriodSummaryResponseRequest(subPeriod)
                       , ReceivedHistoricalSubPeriodResponseRequest));
                historicalSubPeriodAddressToSubPeriodLookup
                    .Add(((SourceTickerIdentifier)pricingInstrumentId).HistoricalPeriodSummaryResponseRequest(subPeriod), subPeriod);
                liveSubPeriodCompletePublisherSubscriptions.Add
                    (subPeriod, await this.RegisterListenerAsync<PricePeriodSummary>
                        (((SourceTickerIdentifier)pricingInstrumentId).CompletePeriodSummaryAddress(subPeriod)
                       , ReceivedCompleteSubPricePeriodSummaries));
                liveSubPeriodAddressToSubReceivedHistorical.Add(((SourceTickerIdentifier)pricingInstrumentId).CompletePeriodSummaryAddress(subPeriod)
                                                              , subPeriod);
            }
            await base.StartAsync();
        }

        public void CreateNewWait(int waitForComplete = 1, int waitForLive = 1)
        {
            waitNumberForCompleted = waitForComplete;
            waitNumberForLive      = waitForLive;
            awaitCompleteSource    = new TaskCompletionSource<int>();
            awaitLiveSource        = new TaskCompletionSource<int>();
        }

        public async ValueTask<List<IPricePeriodSummary>> GetPopulatedLiveResults(int waitNumber = 1)
        {
            waitNumberForLive = waitNumber;
            if (ReceivedLivePublishEvents.Count < waitNumber) await Task.WhenAny(awaitLiveSource.Task, Task.Delay(2_000));
            return ReceivedLivePublishEvents;
        }

        public async ValueTask<List<IPricePeriodSummary>> GetPopulatedCompleteResults(int waitNumber = 1)
        {
            waitNumberForCompleted = waitNumber;
            if (ReceivedCompletePublishEvents.Count < waitNumber) await Task.WhenAny(awaitCompleteSource.Task, Task.Delay(2_000));
            return ReceivedCompletePublishEvents;
        }

        private void ReceivedLivePricePeriodSummaries(IBusMessage<PricePeriodSummary> livePublishMsg)
        {
            var pricePeriodSummary = livePublishMsg.Payload.Body();
            ReceivedLivePublishEvents.Add(pricePeriodSummary);
            if (ReceivedLivePublishEvents.Count >= waitNumberForLive) awaitLiveSource.TrySetResult(0);
        }

        private void ReceivedCompletePricePeriodSummaries(IBusMessage<PricePeriodSummary> completePublishMsg)
        {
            var pricePeriodSummary = completePublishMsg.Payload.Body();
            ReceivedCompletePublishEvents.Add(pricePeriodSummary);
            if (ReceivedCompletePublishEvents.Count >= waitNumberForCompleted) awaitCompleteSource.TrySetResult(0);
        }

        private async ValueTask<List<PricePeriodSummary>> ReceivedHistoricalSubPeriodResponseRequest
            (IBusRespondingMessage<HistoricalPeriodResponseRequest, List<PricePeriodSummary>> completePublishMsg)
        {
            var historicalSubPeriodRequest = completePublishMsg.Payload.Body();
            ReceivedSubPeriodHistoricalRequests.Add(historicalSubPeriodRequest);
            var subPeriod = historicalSubPeriodAddressToSubPeriodLookup[completePublishMsg.DestinationAddress!];
            if (!historicalSubPeriodResponseCallbacks.TryGetValue(subPeriod, out var resultCallback))
                resultCallback = () => new ValueTask<List<PricePeriodSummary>>(new List<PricePeriodSummary>());
            return await resultCallback();
        }

        private void ReceivedCompleteSubPricePeriodSummaries(IBusMessage<PricePeriodSummary> livePublishMsg)
        {
            var pricePeriodSummary = livePublishMsg.Payload.Body();
            ReceivedLivePublishEvents.Add(pricePeriodSummary);
            var subPeriod = liveSubPeriodAddressToSubReceivedHistorical[livePublishMsg.DestinationAddress!];
            if (!receivedCompleteSubPeriods.TryGetValue(subPeriod, out var resultCallback))
            {
                resultCallback = new List<PricePeriodSummary>();
                receivedCompleteSubPeriods.Add(subPeriod, resultCallback);
            }
            resultCallback.Add(pricePeriodSummary);
        }

        public void RegisterSubPeriodResponse(TimeSeriesPeriod subPeriod, Func<ValueTask<List<PricePeriodSummary>>> returnedResults)
        {
            historicalSubPeriodResponseCallbacks.Add(subPeriod, returnedResults);
        }

        public async ValueTask SendPricesToLivePeriodRule(List<Level1PriceQuote> publishPrices, IUpdateTime progressTime)
        {
            await this.RequestAsync<PublishQuotesWithTimeProgress, ValueTask>
                (LivePeriodTestClientPublishPricesAddress, new PublishQuotesWithTimeProgress(publishPrices, progressTime));
        }

        private async ValueTask PublishPriceQuotesHandler
            (IBusRespondingMessage<PublishQuotesWithTimeProgress, ValueTask> requestResponseMsg)
        {
            var toPublishListAndTimeUpdater = requestResponseMsg.Payload.Body();
            var toPublishList               = toPublishListAndTimeUpdater.ToPublish;
            var timeUpdater                 = toPublishListAndTimeUpdater.TimeUpdater;
            foreach (var level1PriceQuote in toPublishList)
            {
                await timeUpdater.UpdateTime(level1PriceQuote.SourceTime);
                await this.PublishAsync(quoteListenAddress, level1PriceQuote);
            }
        }

        public override async ValueTask StopAsync()
        {
            await livePublishSubscription.NullSafeUnsubscribe();
            await listenForPublishPricesSubscription.NullSafeUnsubscribe();
            await completePublishSubscription.NullSafeUnsubscribe();
            foreach (var listenHistoricalSub in historicalSubPeriodRequestSubscriptions.Values) await listenHistoricalSub.NullSafeUnsubscribe();
            foreach (var listenLiveSub in liveSubPeriodCompletePublisherSubscriptions.Values) await listenLiveSub.NullSafeUnsubscribe();
            await base.StopAsync();
        }
    }
}
