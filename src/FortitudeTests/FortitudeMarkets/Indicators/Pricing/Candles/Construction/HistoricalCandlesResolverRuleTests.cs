// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.BusMessaging.Messages.ListeningSubscriptions;
using FortitudeBusRules.BusMessaging.Routing.Response;
using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;
using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Extensions;
using FortitudeIO.TimeSeries;
using FortitudeIO.TimeSeries.FileSystem;
using FortitudeMarkets.Indicators;
using FortitudeMarkets.Indicators.Persistence;
using FortitudeMarkets.Indicators.Pricing.Candles;
using FortitudeMarkets.Indicators.Pricing.Candles.Construction;
using FortitudeMarkets.Pricing;
using FortitudeMarkets.Pricing.FeedEvents.Candles;
using FortitudeMarkets.Pricing.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeTests.FortitudeBusRules.BusMessaging;
using FortitudeTests.FortitudeBusRules.Rules.Common.TimeSeries;
using FortitudeTests.FortitudeCommon.Chronometry;
using FortitudeTests.FortitudeMarkets.Indicators.Config;
using FortitudeTests.FortitudeMarkets.Pricing.FeedEvents.Quotes;
using FortitudeTests.FortitudeMarkets.Pricing.PQ.TimeSeries.BusRules;
using FortitudeTests.FortitudeMarkets.Pricing.Quotes;
using static FortitudeCommon.Chronometry.TimeBoundaryPeriod;
using static FortitudeMarkets.Configuration.ClientServerConfig.MarketClassificationExtensions;
using static FortitudeTests.FortitudeMarkets.Pricing.FeedEvents.Candles.CandleTests;
using static FortitudeMarkets.Pricing.FeedEvents.TickerInfo.TickerQuoteDetailLevel;

#endregion

namespace FortitudeTests.FortitudeMarkets.Indicators.Pricing.Candles.Construction;

[TestClass]
public class HistoricalCandlesResolverRuleTests : OneOfEachMessageQueueTypeTestSetup
{
    private readonly List<InstrumentFileEntryInfo> lastFileEntryInfoRetrieved = new();
    private readonly List<InstrumentFileInfo>      lastFileInfoRetrieved      = new();
    private readonly DateTime                      testEpochTime              = new(2024, 6, 19);

    private readonly SourceTickerInfo tickerId15SPeriod = new
        (2, "SourceName", 2, "TickerName2", Level1Quote, Unknown
       , 1, 0.001m, 10m, 100m, 10m);

    private readonly SourceTickerInfo tickerId30SPeriod =
        new(3, "SourceName", 3, "TickerName3", Level1Quote, Unknown
          , 1, 0.001m, 10m, 100m, 10m);

    private readonly SourceTickerInfo tickerId5SPeriod =
        new(1, "SourceName", 1, "TickerName1", Level1Quote, Unknown
          , 1, 0.001m, 10m, 100m, 10m);

    private List<Candle>           fifteenSecondPeriodSummaries = null!;
    private HistoricalCandleParams fifteenSecondsHistoricalCandleParams;

    private HistoricalCandleParams fiveSecondsHistoricalCandleParams;

    private decimal  highLowSpread;
    private DateTime historical15SSummariesLatestTime;
    private DateTime historical30SSummariesLatestTime;
    private DateTime historicalQuotesLatestTime;
    private DateTime historicalQuotesStartTime;

    private IndicatorServiceRegistryStubRule indicatorRegistryStubRule = null!;

    private List<IPublishableLevel1Quote> lastQuotesRetrieved    = new();
    private List<Candle>                  lastSummariesRetrieved = new();

    private decimal mid1;
    private decimal mid2;

    private List<IPublishableLevel1Quote> oneSecondLevel1Quotes = null!;
    private BoundedTimeRange   restrictedRetrievalRange;

    private decimal spread;

    private StubTimeContext        stubTimeContext             = null!;
    private List<Candle>           thirtySecondPeriodSummaries = null!;
    private HistoricalCandleParams thirtySecondsHistoricalCandleParams;


    private IList<IAsyncDisposable> undeploy = null!;

    [TestInitialize]
    public async Task Setup()
    {
        TimeContext.Provider = stubTimeContext = new StubTimeContext(testEpochTime);

        undeploy = new List<IAsyncDisposable>();

        historical15SSummariesLatestTime = testEpochTime;
        historical30SSummariesLatestTime = testEpochTime;
        historicalQuotesLatestTime       = testEpochTime;

        mid1 = 0.5000m;
        mid2 = 1.0000m;

        spread        = 0.2000m;
        highLowSpread = 0.8000m;

        Generate15SSummaries();
        Generate1SQuotes();
        Generate30SSummaries();

        fiveSecondsHistoricalCandleParams = new HistoricalCandleParams(tickerId5SPeriod, FiveSeconds, new TimeLength(TimeSpan.FromMinutes(30)));
        fifteenSecondsHistoricalCandleParams
            = new HistoricalCandleParams(tickerId15SPeriod, FifteenSeconds, new TimeLength(TimeSpan.FromMinutes(30)));
        thirtySecondsHistoricalCandleParams = new HistoricalCandleParams(tickerId30SPeriod, ThirtySeconds, new TimeLength(TimeSpan.FromMinutes(30)));
        var unitTestNoRepositoryConfig = IndicatorServicesConfigTests.UnitTestNoRepositoryConfig();
        unitTestNoRepositoryConfig.PersistenceConfig.PersistPriceSummaries = true;
        indicatorRegistryStubRule
            = new IndicatorServiceRegistryStubRule
                (new IndicatorServiceRegistryParams(unitTestNoRepositoryConfig));

        restrictedRetrievalRange = new BoundedTimeRange(testEpochTime, FifteenSeconds.PeriodEnd(testEpochTime));

        var repoInfoStubRule                      = new TimeSeriesRepositoryInfoStubRule(GetStubRepoFileInfo, GetStubRepoFileEntryInfo);
        var quotesRetrievalStubRule               = new HistoricalQuotesRetrievalStubRule(GetStubQuotes);
        var candleRetrievalStubRule = new HistoricalCandleRetrievalStubRule(GetStubSummaries);

        var preqDeploy
            = await EventQueue1.Context.RegisteredOn.LaunchRuleAsync
                (indicatorRegistryStubRule, indicatorRegistryStubRule, EventQueue1SelectionResult);
        undeploy.Add(preqDeploy);
        indicatorRegistryStubRule.RegisterGlobalServiceStatus(ServiceType.CandleFilePersister, ServiceRunStatus.ServiceStarted);
        indicatorRegistryStubRule.RegisterTickerPeriodServiceStatus
            (tickerId30SPeriod, new DiscreetTimePeriod(FifteenSeconds), ServiceType.LiveCandle, ServiceRunStatus.ServiceStarted);
        indicatorRegistryStubRule.RegisterTickerPeriodServiceStatus
            (tickerId30SPeriod, new DiscreetTimePeriod(ThirtySeconds), ServiceType.LiveCandle, ServiceRunStatus.ServiceStarted);
        await indicatorRegistryStubRule.RegisterAndDeployGlobalService(ServiceType.TimeSeriesFileRepositoryInfo, repoInfoStubRule);
        await indicatorRegistryStubRule.RegisterAndDeployGlobalService(ServiceType.HistoricalQuotesRetriever, quotesRetrievalStubRule);
        await indicatorRegistryStubRule.RegisterAndDeployGlobalService
            (ServiceType.HistoricalCandlesRetriever, candleRetrievalStubRule);
    }

    private void Generate1SQuotes(int numberToGenerate = 90)
    {
        var startOffsetTimeSpan = TimeSpan.FromSeconds(30) - TimeSpan.FromSeconds(numberToGenerate);
        historicalQuotesStartTime = testEpochTime + startOffsetTimeSpan;
        var entriesStartTime = historicalQuotesStartTime;

        oneSecondLevel1Quotes = new List<IPublishableLevel1Quote>(numberToGenerate);
        for (var i = 0; i < numberToGenerate; i++)
            oneSecondLevel1Quotes.Add
                (tickerId15SPeriod.CreatePublishableLevel1Quote(entriesStartTime = OneSecond.PeriodEnd(entriesStartTime), i % 2 == 0 ? mid1 : mid2, spread));
    }

    private void Generate15SSummaries(int numberToGenerate = 5)
    {
        var startOffsetTimeSpan = TimeSpan.FromSeconds(numberToGenerate * 15);
        var entriesStartTime    = testEpochTime - startOffsetTimeSpan;

        fifteenSecondPeriodSummaries = new List<Candle>(numberToGenerate);
        for (var i = 0; i < numberToGenerate; i++)
            fifteenSecondPeriodSummaries.Add
                (CreateCandle
                    (FifteenSeconds, entriesStartTime = FifteenSeconds.PeriodEnd(entriesStartTime), i % 2 == 0 ? mid1 : mid2, spread, highLowSpread));
    }

    private void Generate30SSummaries(int numberToGenerate = 2)
    {
        var startOffsetTimeSpan = TimeSpan.FromSeconds(numberToGenerate * 30);
        var entriesStartTime    = testEpochTime - startOffsetTimeSpan;

        thirtySecondPeriodSummaries = new List<Candle>(numberToGenerate);
        for (var i = 0; i < numberToGenerate; i++)
            thirtySecondPeriodSummaries.Add
                (CreateCandle
                    (ThirtySeconds, entriesStartTime = ThirtySeconds.PeriodEnd(entriesStartTime), i % 2 == 0 ? mid1 : mid2, spread, highLowSpread));
    }

    private List<InstrumentFileInfo> GetStubRepoFileInfo
        (string instrumentName, InstrumentType? instrumentType = null, DiscreetTimePeriod? period = null)
    {
        lastFileInfoRetrieved.Clear();
        if (period?.Period == FiveSeconds)
        {
            var lastHistoricalCandleStartTime = FiveSeconds.ContainingPeriodBoundaryStart(historical15SSummariesLatestTime);
            var lastHistoricalEntryTime = instrumentType == InstrumentType.Candle
                ? lastHistoricalCandleStartTime
                : historicalQuotesLatestTime;
            var earliestHistoricalEntryTime = instrumentType == InstrumentType.Candle
                ? FiveSeconds.PreviousPeriodStart(lastHistoricalEntryTime)
                : historicalQuotesStartTime;
            var fileStartDate = lastHistoricalEntryTime.TruncToWeekBoundary();
            lastFileInfoRetrieved
                .Add(new InstrumentFileInfo(tickerId5SPeriod, OneWeek, earliestHistoricalEntryTime, lastHistoricalEntryTime
                                          , new List<DateTime> { fileStartDate }));
            return lastFileInfoRetrieved;
        }
        if (period?.Period == FifteenSeconds)
        {
            var lastHistoricalCandleStartTime = FifteenSeconds.ContainingPeriodBoundaryStart(historical15SSummariesLatestTime);
            var lastHistoricalEntryTime = instrumentType == InstrumentType.Candle
                ? lastHistoricalCandleStartTime
                : historicalQuotesLatestTime;
            var earliestHistoricalEntryTime = instrumentType == InstrumentType.Candle
                ? FifteenSeconds.PreviousPeriodStart(lastHistoricalEntryTime)
                : historicalQuotesStartTime;
            var fileStartDate = lastHistoricalEntryTime.TruncToWeekBoundary();
            lastFileInfoRetrieved
                .Add(new InstrumentFileInfo(tickerId15SPeriod, OneWeek, earliestHistoricalEntryTime, lastHistoricalEntryTime
                                          , new List<DateTime> { fileStartDate }));
            return lastFileInfoRetrieved;
        }
        var latestCandleStartTIme = ThirtySeconds.ContainingPeriodBoundaryStart(historical30SSummariesLatestTime);
        var lastEntryTime = instrumentType == InstrumentType.Candle
            ? latestCandleStartTIme
            : historicalQuotesLatestTime;
        var earliestEntryTime = instrumentType == InstrumentType.Candle
            ? ThirtySeconds.PreviousPeriodStart(latestCandleStartTIme)
            : historicalQuotesStartTime;
        var fileStartTime = lastEntryTime.TruncToWeekBoundary();
        lastFileInfoRetrieved
            .Add(new InstrumentFileInfo(tickerId30SPeriod, OneWeek, earliestEntryTime, lastEntryTime, new List<DateTime> { fileStartTime }));
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

    private IEnumerable<IPublishableTickInstant> GetStubQuotes(SourceTickerIdentifier srcTickerIdentifier, UnboundedTimeRange? requestTimeRange)
    {
        return lastQuotesRetrieved =
            oneSecondLevel1Quotes
                .Where(q => q.SourceTime >= restrictedRetrievalRange.FromTime
                         && q.SourceTime < restrictedRetrievalRange.ToTime
                         && q.SourceTime >= (requestTimeRange?.FromTime ?? DateTime.MinValue)
                         && q.SourceTime < (requestTimeRange?.ToTime ?? DateTime.MaxValue))
                .ToList();
    }

    private IEnumerable<Candle> GetStubSummaries
        (SourceTickerIdentifier srcTickerIdentifier, TimeBoundaryPeriod requestPeriod, UnboundedTimeRange? requestTimeRange)
    {
        if (srcTickerIdentifier.TickerId == tickerId5SPeriod.InstrumentId) return Enumerable.Empty<Candle>();
        if (srcTickerIdentifier.TickerId == tickerId30SPeriod.InstrumentId && requestPeriod == FifteenSeconds)
            return lastSummariesRetrieved =
                fifteenSecondPeriodSummaries
                    .Where(q => q.PeriodEndTime >= restrictedRetrievalRange.FromTime
                             && q.PeriodEndTime <= restrictedRetrievalRange.ToTime
                             && q.PeriodEndTime >= (requestTimeRange?.FromTime ?? DateTime.MinValue)
                             && q.PeriodEndTime <= (requestTimeRange?.ToTime ?? DateTime.MaxValue)).ToList();
        if (srcTickerIdentifier.TickerId == tickerId30SPeriod.InstrumentId && requestPeriod == ThirtySeconds)
            return lastSummariesRetrieved =
                thirtySecondPeriodSummaries
                    .Where(q => q.PeriodEndTime >= restrictedRetrievalRange.FromTime
                             && q.PeriodEndTime <= restrictedRetrievalRange.ToTime
                             && q.PeriodEndTime >= (requestTimeRange?.FromTime ?? DateTime.MinValue)
                             && q.PeriodEndTime <= (requestTimeRange?.ToTime ?? DateTime.MaxValue)).ToList();
        return lastSummariesRetrieved =
            fifteenSecondPeriodSummaries
                .Where(q => q.PeriodEndTime >= restrictedRetrievalRange.FromTime
                         && q.PeriodEndTime <= restrictedRetrievalRange.ToTime
                         && q.PeriodEndTime >= (requestTimeRange?.FromTime ?? DateTime.MinValue)
                         && q.PeriodEndTime <= (requestTimeRange?.ToTime ?? DateTime.MaxValue)).ToList();
    }

    [TestCleanup]
    public async Task TearDown()
    {
        await stubTimeContext.DisposeAsync();
        foreach (var asyncDisposable in undeploy) await asyncDisposable.DisposeAsync();
    }

    [TestMethod]
    [Timeout(20_000)]
    public async Task AllTicksSummary4QuotesOn15sResolver_SendEntriesToPersister_CanRetrieveEntriesFromRepository()
    {
        stubTimeContext.AddSeconds(5);
        historicalQuotesLatestTime       = stubTimeContext.UtcNow;
        historical15SSummariesLatestTime = FifteenSeconds.PreviousPeriodStart(stubTimeContext.UtcNow);
        restrictedRetrievalRange         = new BoundedTimeRange(historicalQuotesStartTime, stubTimeContext.UtcNow);

        var test15SHistoricalPeriodClient = new TestHistoricalPeriodClient(FifteenSeconds, tickerId15SPeriod);
        await indicatorRegistryStubRule.DeployChildRuleAsync(test15SHistoricalPeriodClient);

        var histResolver15SRule = new HistoricalCandlesResolverRule<PublishableLevel1PriceQuote>(fifteenSecondsHistoricalCandleParams);

        await using var histResolverDeploy = await indicatorRegistryStubRule.DeployChildRuleAsync(histResolver15SRule);

        Assert.AreEqual(0, lastSummariesRetrieved.Count);
        Assert.AreEqual(14, lastQuotesRetrieved.Count);

        await test15SHistoricalPeriodClient.BlockUntilToPersistReaches(1);
        var hasResult = await test15SHistoricalPeriodClient.InvokeResponseRequestOnTestClientQueue
            (new HistoricalCandleResponseRequest(new BoundedTimeRange(testEpochTime - TimeSpan.FromSeconds(15), testEpochTime)));
        Assert.AreEqual(1, test15SHistoricalPeriodClient.ReceivedToPersistEvents.Count);
        Assert.AreEqual(1, hasResult.Count);
    }

    [TestMethod]
    [Timeout(20_000)]
    public async Task MissingOneHistoricalSummary_OnStartupResolverStartsPublishingAvailablePeriods_ReceiveOneHistoricalPeriodToPersist()
    {
        stubTimeContext.AddSeconds(24);
        historicalQuotesLatestTime = stubTimeContext.UtcNow;
        restrictedRetrievalRange   = new BoundedTimeRange(historicalQuotesStartTime, stubTimeContext.UtcNow);

        var test30SHistoricalPeriodClient = new TestHistoricalPeriodClient(ThirtySeconds, tickerId30SPeriod);

        await using var testClientDeployment = await indicatorRegistryStubRule.DeployChildRuleAsync(test30SHistoricalPeriodClient);

        historical15SSummariesLatestTime = FifteenSeconds.ContainingPeriodBoundaryStart(stubTimeContext.UtcNow);
        var thirtySeconds15SPeriodHistoricalPeriodParams = new HistoricalCandleParams
            (thirtySecondsHistoricalCandleParams.SourceTickerIdentifier, FifteenSeconds, new TimeLength(TimeSpan.FromMinutes(1)));
        var histResolver15SRule = new HistoricalCandlesResolverRule<PublishableLevel1PriceQuote>(thirtySeconds15SPeriodHistoricalPeriodParams);

        await indicatorRegistryStubRule.RegisterAndDeployTickerPeriodService
            (thirtySecondsHistoricalCandleParams.SourceTickerIdentifier, new DiscreetTimePeriod(FifteenSeconds)
           , ServiceType.HistoricalCandlesResolver, histResolver15SRule);

        historical30SSummariesLatestTime = ThirtySeconds.PreviousPeriodStart(testEpochTime);
        var histResolver30SRule = new HistoricalCandlesResolverRule<PublishableLevel1PriceQuote>(thirtySecondsHistoricalCandleParams);

        await using var histResolverDeploy = await indicatorRegistryStubRule.DeployChildRuleAsync(histResolver30SRule);
        await test30SHistoricalPeriodClient.BlockUntilToPersistReaches(1);

        Assert.AreEqual(5, lastSummariesRetrieved.Count);
        Assert.AreEqual(0, lastQuotesRetrieved.Count);
        Assert.AreEqual(1, test30SHistoricalPeriodClient.ReceivedToPersistEvents.Count);
    }

    [TestMethod]
    [Timeout(20_000)]
    public async Task SummariesUpToDate_StreamRequestForHistoricalGoesToRepoForUncached_ReceivesAllRequestedHistoricalSummaries()
    {
        Generate30SSummaries(121);
        historicalQuotesStartTime  = thirtySecondPeriodSummaries.First().PeriodStartTime;
        historicalQuotesLatestTime = stubTimeContext.UtcNow;
        restrictedRetrievalRange   = new BoundedTimeRange(historicalQuotesStartTime, stubTimeContext.UtcNow);

        var test30SHistoricalPeriodClient = new TestHistoricalPeriodClient(ThirtySeconds, tickerId30SPeriod);

        await using var testClientDeployment = await indicatorRegistryStubRule.DeployChildRuleAsync(test30SHistoricalPeriodClient);

        historical30SSummariesLatestTime = testEpochTime;
        historical15SSummariesLatestTime = testEpochTime;
        var histResolver30SRule = new HistoricalCandlesResolverRule<PublishableLevel1PriceQuote>(thirtySecondsHistoricalCandleParams);

        await using var histResolverDeploy = await indicatorRegistryStubRule.DeployChildRuleAsync(histResolver30SRule);

        Assert.AreEqual(0, lastSummariesRetrieved.Count);
        Assert.AreEqual(0, lastQuotesRetrieved.Count);

        var hasResult = await test30SHistoricalPeriodClient.InvokeStreamRequestOnTestClientQueue
            (new HistoricalCandleStreamRequest
                (new BoundedTimeRange(historicalQuotesStartTime, historicalQuotesStartTime + TimeSpan.FromHours(1)), new ResponsePublishParams()));
        Assert.AreEqual(60, lastSummariesRetrieved.Count); // most recent first is loaded when cache is loaded
        Assert.AreEqual(0, lastQuotesRetrieved.Count);
        Assert.AreEqual(120, hasResult.Count);
    }

    [TestMethod]
    [Timeout(20_000)]
    public async Task SummariesUpToDate_ResponseRequestForHistoricalGoesToRepoForUncached_ReceivesAllRequestedHistoricalSummaries()
    {
        Generate30SSummaries(121);
        historicalQuotesStartTime  = thirtySecondPeriodSummaries.First().PeriodStartTime;
        historicalQuotesLatestTime = stubTimeContext.UtcNow;
        restrictedRetrievalRange   = new BoundedTimeRange(historicalQuotesStartTime, stubTimeContext.UtcNow);

        var test30SHistoricalPeriodClient = new TestHistoricalPeriodClient(ThirtySeconds, tickerId30SPeriod);

        await using var testClientDeployment = await indicatorRegistryStubRule.DeployChildRuleAsync(test30SHistoricalPeriodClient);

        historical30SSummariesLatestTime = testEpochTime;
        historical15SSummariesLatestTime = testEpochTime;
        var histResolver30SRule = new HistoricalCandlesResolverRule<PublishableLevel1PriceQuote>(thirtySecondsHistoricalCandleParams);

        await using var histResolverDeploy = await indicatorRegistryStubRule.DeployChildRuleAsync(histResolver30SRule);

        Assert.AreEqual(0, lastSummariesRetrieved.Count);
        Assert.AreEqual(0, lastQuotesRetrieved.Count);

        var hasResult = await test30SHistoricalPeriodClient.InvokeResponseRequestOnTestClientQueue
            (new HistoricalCandleResponseRequest(new BoundedTimeRange(historicalQuotesStartTime, historicalQuotesStartTime + TimeSpan.FromHours(1))));
        Assert.AreEqual(59, lastSummariesRetrieved.Count); // most recent first is loaded when cache is loaded
        Assert.AreEqual(0, lastQuotesRetrieved.Count);
        Assert.AreEqual(120, hasResult.Count);
    }

    [TestMethod]
    [Timeout(20_000)]
    public async Task FromTicksSummaryLargeHistory_FileUpToDate_StreamRequestGeneratesResultsFromQuotes()
    {
        Generate1SQuotes(3631);

        var test15SHistoricalPeriodClient = new TestHistoricalPeriodClient(FifteenSeconds, tickerId15SPeriod);

        await using var testClientDeployment = await indicatorRegistryStubRule.DeployChildRuleAsync(test15SHistoricalPeriodClient);

        var histResolver15SRule = new HistoricalCandlesResolverRule<PublishableLevel1PriceQuote>(fifteenSecondsHistoricalCandleParams);
        historical15SSummariesLatestTime = oneSecondLevel1Quotes.First().SourceTime.AddHours(-1);
        historicalQuotesStartTime        = historical15SSummariesLatestTime.AddHours(-1);
        historicalQuotesLatestTime       = historical15SSummariesLatestTime;
        restrictedRetrievalRange         = new BoundedTimeRange(historicalQuotesLatestTime, historicalQuotesLatestTime);
        await using var histResolverDeploy = await indicatorRegistryStubRule.DeployChildRuleAsync(histResolver15SRule);

        Assert.AreEqual(0, lastSummariesRetrieved.Count);
        Assert.AreEqual(0, lastQuotesRetrieved.Count);
        historicalQuotesStartTime  = oneSecondLevel1Quotes.First().SourceTime;
        historicalQuotesLatestTime = testEpochTime.AddSeconds(2);
        restrictedRetrievalRange   = new BoundedTimeRange(historicalQuotesStartTime, historicalQuotesLatestTime);

        var hasResult = await test15SHistoricalPeriodClient.InvokeStreamRequestOnTestClientQueue
            (new HistoricalCandleStreamRequest
                (new BoundedTimeRange(historicalQuotesStartTime, historicalQuotesLatestTime), new ResponsePublishParams()));
        Assert.AreEqual(0, test15SHistoricalPeriodClient.ReceivedToPersistEvents.Count);
        Assert.AreEqual(240, hasResult.Count);
    }

    [TestMethod]
    [Timeout(20_000)]
    public async Task FromSubSummaryLargeHistory_FileUpToDate_StreamRequestGeneratesResultsFromQuotes()
    {
        Generate15SSummaries(241);

        var testClient = new TestHistoricalPeriodClient(ThirtySeconds, tickerId30SPeriod);
        TimeContext.Provider = new StubTimeContext(testEpochTime.AddHours(4));

        await using var testClientDeployment = await indicatorRegistryStubRule.DeployChildRuleAsync(testClient);
        restrictedRetrievalRange         = new BoundedTimeRange(testEpochTime.AddMinutes(-30), stubTimeContext.UtcNow);
        historicalQuotesStartTime        = testEpochTime.AddHours(-1).AddSeconds(-1);
        historicalQuotesLatestTime       = testEpochTime;
        historical15SSummariesLatestTime = testEpochTime;
        var thirtySeconds15SPeriodHistoricalPeriodParams = new HistoricalCandleParams
            (thirtySecondsHistoricalCandleParams.SourceTickerIdentifier, FifteenSeconds, new TimeLength(TimeSpan.FromMinutes(30)));
        var histResolver15SRule = new HistoricalCandlesResolverRule<PublishableLevel1PriceQuote>(thirtySeconds15SPeriodHistoricalPeriodParams);
        await indicatorRegistryStubRule.RegisterAndDeployTickerPeriodService
            (thirtySecondsHistoricalCandleParams.SourceTickerIdentifier, new DiscreetTimePeriod(FifteenSeconds)
           , ServiceType.HistoricalCandlesResolver, histResolver15SRule);

        var histResolver30SRule = new HistoricalCandlesResolverRule<PublishableLevel1PriceQuote>(thirtySecondsHistoricalCandleParams);

        historical15SSummariesLatestTime = testEpochTime.AddHours(-1);
        historical30SSummariesLatestTime = historical15SSummariesLatestTime.AddSeconds(-1);
        TimeContext.Provider             = new StubTimeContext(historical30SSummariesLatestTime);
        historicalQuotesStartTime        = historical15SSummariesLatestTime.AddHours(-1);
        historicalQuotesLatestTime       = historical15SSummariesLatestTime;
        restrictedRetrievalRange         = new BoundedTimeRange(historicalQuotesStartTime, historicalQuotesStartTime);
        await using var histResolverDeploy = await indicatorRegistryStubRule.DeployChildRuleAsync(histResolver30SRule);

        Assert.AreEqual(0, lastSummariesRetrieved.Count);
        Assert.AreEqual(0, lastQuotesRetrieved.Count);

        TimeContext.Provider       = new StubTimeContext(testEpochTime);
        historicalQuotesStartTime  = testEpochTime.AddHours(-1).AddSeconds(-1);
        historicalQuotesLatestTime = testEpochTime;
        restrictedRetrievalRange   = new BoundedTimeRange(historicalQuotesStartTime, historicalQuotesLatestTime);

        var hasResult = await testClient.InvokeStreamRequestOnTestClientQueue
            (new HistoricalCandleStreamRequest
                (new BoundedTimeRange(historicalQuotesStartTime, historicalQuotesLatestTime), new ResponsePublishParams()));
        Assert.AreEqual(0, testClient.ReceivedToPersistEvents.Count);
        Assert.AreEqual(120, hasResult.Count);
    }

    [TestMethod]
    [Timeout(20_000)]
    public async Task FromSubSummaryLargeHistory_FileUpToDate_ResponseRequestGeneratesResultsFromRepoSubSummaries()
    {
        Generate15SSummaries(241);

        var testClient = new TestHistoricalPeriodClient(ThirtySeconds, tickerId30SPeriod);
        TimeContext.Provider = new StubTimeContext(testEpochTime.AddHours(4));

        await using var testClientDeployment = await indicatorRegistryStubRule.DeployChildRuleAsync(testClient);
        restrictedRetrievalRange         = new BoundedTimeRange(testEpochTime.AddMinutes(-30), stubTimeContext.UtcNow);
        historicalQuotesStartTime        = testEpochTime.AddHours(-1).AddSeconds(-1);
        historicalQuotesLatestTime       = testEpochTime;
        historical15SSummariesLatestTime = testEpochTime;
        var thirtySeconds15SPeriodHistoricalPeriodParams = new HistoricalCandleParams
            (thirtySecondsHistoricalCandleParams.SourceTickerIdentifier, FifteenSeconds, new TimeLength(TimeSpan.FromMinutes(30)));
        var histResolver15SRule = new HistoricalCandlesResolverRule<PublishableLevel1PriceQuote>(thirtySeconds15SPeriodHistoricalPeriodParams);
        await indicatorRegistryStubRule.RegisterAndDeployTickerPeriodService
            (thirtySecondsHistoricalCandleParams.SourceTickerIdentifier, new DiscreetTimePeriod(FifteenSeconds)
           , ServiceType.HistoricalCandlesResolver, histResolver15SRule);

        var histResolver30SRule = new HistoricalCandlesResolverRule<PublishableLevel1PriceQuote>(thirtySecondsHistoricalCandleParams);

        historical15SSummariesLatestTime = testEpochTime.AddHours(-1);
        historical30SSummariesLatestTime = historical15SSummariesLatestTime.AddSeconds(-1);
        TimeContext.Provider             = new StubTimeContext(historical30SSummariesLatestTime);
        historicalQuotesStartTime        = historical15SSummariesLatestTime.AddHours(-1);
        historicalQuotesLatestTime       = historical15SSummariesLatestTime;
        restrictedRetrievalRange         = new BoundedTimeRange(historicalQuotesStartTime, historicalQuotesStartTime);
        await using var histResolverDeploy = await indicatorRegistryStubRule.DeployChildRuleAsync(histResolver30SRule);

        Assert.AreEqual(0, lastSummariesRetrieved.Count);
        Assert.AreEqual(0, lastQuotesRetrieved.Count);

        TimeContext.Provider       = new StubTimeContext(testEpochTime);
        historicalQuotesStartTime  = testEpochTime.AddHours(-1).AddSeconds(-15);
        historicalQuotesLatestTime = testEpochTime;
        restrictedRetrievalRange   = new BoundedTimeRange(historicalQuotesStartTime, historicalQuotesLatestTime);

        var hasResult = await testClient.InvokeResponseRequestOnTestClientQueue
            (new HistoricalCandleResponseRequest(new BoundedTimeRange(historicalQuotesStartTime, historicalQuotesLatestTime)));
        Assert.AreEqual(0, testClient.ReceivedToPersistEvents.Count);
        Assert.AreEqual(120, hasResult.Count);
    }

    [TestMethod]
    [Timeout(20_000)]
    public async Task AllTicksSummaryLargeHistory_FileUpToDate_ResponseRequestGeneratesResultsFromQuotes()
    {
        Generate1SQuotes(3631);
        historical15SSummariesLatestTime = testEpochTime;
        restrictedRetrievalRange         = new BoundedTimeRange(historicalQuotesStartTime, stubTimeContext.UtcNow);

        var test5SHistoricalPeriodClient = new TestHistoricalPeriodClient(FiveSeconds, tickerId5SPeriod);

        await using var testClientDeployment = await indicatorRegistryStubRule.DeployChildRuleAsync(test5SHistoricalPeriodClient);

        var histResolver5SRule = new HistoricalCandlesResolverRule<PublishableLevel1PriceQuote>(fiveSecondsHistoricalCandleParams);

        await using var histResolverDeploy = await indicatorRegistryStubRule.DeployChildRuleAsync(histResolver5SRule);

        Assert.AreEqual(0, lastSummariesRetrieved.Count);
        Assert.AreEqual(0, lastQuotesRetrieved.Count);


        var hasResult = await test5SHistoricalPeriodClient.InvokeResponseRequestOnTestClientQueue
            (new HistoricalCandleResponseRequest(new BoundedTimeRange(historicalQuotesStartTime, historicalQuotesStartTime + TimeSpan.FromHours(1))));
        Assert.AreEqual(0, test5SHistoricalPeriodClient.ReceivedToPersistEvents.Count);
        Assert.AreEqual(720, hasResult.Count);
    }

    private class TestHistoricalPeriodClient(TimeBoundaryPeriod period, SourceTickerIdentifier sourceTickerIdentifier) : Rule
    {
        private const string HistoricalPeriodTestClientInvokeResponseRequestAddress
            = "TestClient.HistoricalCandle.Invoke.RequestResponse";
        private const string HistoricalPeriodTestClientInvokeStreamRequestAddress = "TestClient.HistoricalCandle.Invoke.StreamRequest";

        private ISubscription? invokeHistoricalPeriodResponseRequest;
        private ISubscription? invokeHistoricalPeriodStreamRequest;
        private ISubscription? toPersistSubscription;

        public List<ICandle> ReceivedToPersistEvents { get; } = new();

        public async Task BlockUntilToPersistReaches(int expectedNumber, int timeoutMs = 20)
        {
            var timeoutTime = DateTime.UtcNow.AddMilliseconds(timeoutMs);
            while (ReceivedToPersistEvents.Count < expectedNumber && DateTime.UtcNow < timeoutTime) await Task.Delay(timeoutMs);
        }

        public override async ValueTask StartAsync()
        {
            invokeHistoricalPeriodResponseRequest
                = await this.RegisterRequestListenerAsync<HistoricalCandleResponseRequest, List<Candle>>
                    (HistoricalPeriodTestClientInvokeResponseRequestAddress, TestRequestResponseHandler);
            invokeHistoricalPeriodStreamRequest = await this.RegisterRequestListenerAsync<HistoricalCandleStreamRequest, List<Candle>>
                (HistoricalPeriodTestClientInvokeStreamRequestAddress, TestStreamRequestHandler);
            toPersistSubscription = await this.RegisterListenerAsync<ChainableInstrumentPayload<Candle>>
                (CandleConstants.PersistAppendCandlePublish(), SaveToPersistPeriodSummariesHandler);
            await base.StartAsync();
        }

        public async ValueTask<List<Candle>> InvokeResponseRequestOnTestClientQueue
            (HistoricalCandleResponseRequest request) =>
            await this.RequestAsync<HistoricalCandleResponseRequest, List<Candle>>
                (HistoricalPeriodTestClientInvokeResponseRequestAddress, request);

        public async ValueTask<List<Candle>> InvokeStreamRequestOnTestClientQueue
            (HistoricalCandleStreamRequest request) =>
            await this.RequestAsync<HistoricalCandleStreamRequest, List<Candle>>
                (HistoricalPeriodTestClientInvokeStreamRequestAddress, request);

        private void SaveToPersistPeriodSummariesHandler(IBusMessage<ChainableInstrumentPayload<Candle>> toPersistMsg)
        {
            var candle = toPersistMsg.Payload.Body();
            ReceivedToPersistEvents.Add(candle.Entry);
        }

        private async ValueTask<List<Candle>> TestRequestResponseHandler
            (IBusRespondingMessage<HistoricalCandleResponseRequest, List<Candle>> requestResponseMsg)
        {
            var historicalRequest = requestResponseMsg.Payload.Body();

            return await this.RequestAsync<HistoricalCandleResponseRequest, List<Candle>>
                (sourceTickerIdentifier.HistoricalCandleResponseRequest(period), historicalRequest);
        }

        private async ValueTask<List<Candle>> TestStreamRequestHandler
            (IBusRespondingMessage<HistoricalCandleStreamRequest, List<Candle>> streamRequestMsg)
        {
            var historicalRequest = streamRequestMsg.Payload.Body();

            var limitedRecycler = Context.PooledRecycler.Borrow<LimitedBlockingRecycler>();
            limitedRecycler.MaxTypeBorrowLimit = 200;
            var remainingPeriods      = new List<Candle>();
            var rebuildCompleteSource = new TaskCompletionSource<bool>();
            var rebuildCompleteTask   = rebuildCompleteSource.Task;
            var summaryPopulateChannel = this.CreateChannelFactory(ce =>
            {
                if (ce.IsLastEvent)
                    rebuildCompleteSource.SetResult(true);
                else
                    remainingPeriods.Add(ce.Event);
                return !ce.IsLastEvent;
            }, limitedRecycler);
            var channelStreamRequest = summaryPopulateChannel.ToChannelPublishRequest();
            historicalRequest
                = new HistoricalCandleStreamRequest(historicalRequest.RequestTimeRange, new ResponsePublishParams(channelStreamRequest));

            var expectResults = await this.RequestAsync<HistoricalCandleStreamRequest, bool>
                (sourceTickerIdentifier.HistoricalCandleStreamRequest(period), historicalRequest);

            if (expectResults) await rebuildCompleteTask;
            return remainingPeriods;
        }

        public override async ValueTask StopAsync()
        {
            await toPersistSubscription.NullSafeUnsubscribe();
            await invokeHistoricalPeriodResponseRequest.NullSafeUnsubscribe();
            await invokeHistoricalPeriodStreamRequest.NullSafeUnsubscribe();
            await base.StopAsync();
        }
    }
}
