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
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsApi.Pricing.Summaries;
using FortitudeMarketsCore.Indicators;
using FortitudeMarketsCore.Indicators.Pricing.PeriodSummaries;
using FortitudeMarketsCore.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.Summaries;
using FortitudeTests.FortitudeBusRules.BusMessaging;
using FortitudeTests.FortitudeBusRules.Rules.Common.TimeSeries;
using FortitudeTests.FortitudeCommon.Chronometry;
using FortitudeTests.FortitudeMarketsCore.Indicators.Config;
using FortitudeTests.FortitudeMarketsCore.Pricing.PQ.TimeSeries.BusRules;
using FortitudeTests.FortitudeMarketsCore.Pricing.Quotes;
using static FortitudeIO.TimeSeries.TimeSeriesPeriod;
using static FortitudeMarketsApi.Configuration.ClientServerConfig.MarketClassificationExtensions;
using static FortitudeMarketsApi.Pricing.Quotes.QuoteLevel;
using static FortitudeTests.FortitudeMarketsCore.Pricing.Summaries.PricePeriodSummaryTests;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Indicators.Pricing.PeriodSummaries;

[TestClass]
public class HistoricalPeriodSummariesResolverRuleTests : OneOfEachMessageQueueTypeTestSetup
{
    private readonly DateTime seedDateTime = new(2024, 6, 19);

    private List<PricePeriodSummary> fifteenSecondPeriodSummaries = null!;
    private HistoricalPeriodParams   fifteenSecondsHistoricalPeriodParams;

    private decimal  highLowSpread;
    private DateTime historical15sSummariesLatestTime;
    private DateTime historical30sSummariesLatestTime;
    private DateTime historicalQuotesStartTime;

    private IndicatorServiceRegistryStubRule indicatorRegistryStubRule = null!;

    private List<InstrumentFileEntryInfo> lastFileEntryInfoRetrieved = new();
    private List<InstrumentFileInfo>      lastFileInfoRetrieved      = new();
    private List<ILevel1Quote>            lastQuotesRetrieved        = new();
    private List<PricePeriodSummary>      lastSummariesRetrieved     = new();

    private decimal mid1;
    private decimal mid2;

    private List<ILevel1Quote> oneSecondLevel1Quotes = null!;
    private BoundedTimeRange   restrictedRetrievalRange;

    private decimal spread;

    private StubTimeContext        stubTimeContext = null!;
    private HistoricalPeriodParams thirtySecondsHistoricalPeriodParams;

    private ISourceTickerQuoteInfo tickerId15sPeriod = new SourceTickerQuoteInfo
        (2, "SourceName", 2, "TickerName2", Level1, Unknown
       , 1, 0.001m, 10m, 100m, 10m);

    private ISourceTickerQuoteInfo tickerId30sPeriod = new SourceTickerQuoteInfo
        (3, "SourceName", 3, "TickerName3", Level1, Unknown
       , 1, 0.001m, 10m, 100m, 10m);


    private IList<IAsyncDisposable> undeploy = null!;

    [TestInitialize]
    public async Task Setup()
    {
        TimeContext.Provider = stubTimeContext = new StubTimeContext(seedDateTime);
        undeploy             = new List<IAsyncDisposable>();

        historical15sSummariesLatestTime = seedDateTime;
        historical30sSummariesLatestTime = seedDateTime;

        mid1 = 0.5000m;
        mid2 = 1.0000m;

        spread        = 0.2000m;
        highLowSpread = 0.8000m;

        var entriesStartTime = OneMinute.PreviousPeriodStart(seedDateTime);

        fifteenSecondPeriodSummaries = new List<PricePeriodSummary>
        {
            CreatePricePeriodSummary(FifteenSeconds, entriesStartTime, mid1, spread, highLowSpread)
          , CreatePricePeriodSummary(FifteenSeconds, entriesStartTime = FifteenSeconds.PeriodEnd(entriesStartTime), mid2, highLowSpread)
          , CreatePricePeriodSummary(FifteenSeconds, entriesStartTime = FifteenSeconds.PeriodEnd(entriesStartTime), mid1, highLowSpread)
          , CreatePricePeriodSummary(FifteenSeconds, entriesStartTime = FifteenSeconds.PeriodEnd(entriesStartTime), mid2, highLowSpread)
          , CreatePricePeriodSummary(FifteenSeconds, FifteenSeconds.PeriodEnd(entriesStartTime), mid1, spread, highLowSpread)
        };


        historicalQuotesStartTime = OneMinute.PreviousPeriodStart(seedDateTime);
        entriesStartTime          = historicalQuotesStartTime;

        oneSecondLevel1Quotes = new List<ILevel1Quote>();
        for (var i = 0; i < 90; i++)
            oneSecondLevel1Quotes.Add
                (tickerId15sPeriod.CreateLevel1Quote(entriesStartTime = OneSecond.PeriodEnd(entriesStartTime), i % 2 == 0 ? mid1 : mid2, spread));

        fifteenSecondsHistoricalPeriodParams = new HistoricalPeriodParams(tickerId15sPeriod, FifteenSeconds, new TimeLength(TimeSpan.FromMinutes(1)));
        thirtySecondsHistoricalPeriodParams  = new HistoricalPeriodParams(tickerId30sPeriod, ThirtySeconds, new TimeLength(TimeSpan.FromMinutes(1)));
        indicatorRegistryStubRule
            = new IndicatorServiceRegistryStubRule
                (new IndicatorServiceRegistryParams(IndicatorServicesConfigTests.UnitTestNoRepositoryConfig()));

        restrictedRetrievalRange = new BoundedTimeRange(seedDateTime, FifteenSeconds.PeriodEnd(seedDateTime));

        var repoInfoStubRule                      = new TimeSeriesRepositoryInfoStubRule(GetStubRepoFileInfo, GetStubRepoFileEntryInfo);
        var quotesRetrievalStubRule               = new HistoricalQuotesRetrievalStubRule(GetStubQuotes);
        var pricePeriodSummariesRetrievalStubRule = new HistoricalPricePeriodSummaryRetrievalStubRule(GetStubSummaries);

        var prequDeploy
            = await EventQueue1.Context.RegisteredOn.LaunchRuleAsync
                (indicatorRegistryStubRule, indicatorRegistryStubRule, EventQueue1SelectionResult);
        undeploy.Add(prequDeploy);
        await indicatorRegistryStubRule.RegisterAndDeployGlobalService(ServiceType.TimeSeriesFileRepositoryInfo, repoInfoStubRule);
        await indicatorRegistryStubRule.RegisterAndDeployGlobalService(ServiceType.HistoricalQuotesRetriever, quotesRetrievalStubRule);
        await indicatorRegistryStubRule.RegisterAndDeployGlobalService(ServiceType.HistoricalPricePeriodSummaryRetriever
                                                                     , pricePeriodSummariesRetrievalStubRule);
    }

    private List<InstrumentFileInfo> GetStubRepoFileInfo
        (string instrumentName, InstrumentType? instrumentType = null, TimeSeriesPeriod? period = null)
    {
        lastFileInfoRetrieved.Clear();
        if (period == FifteenSeconds)
        {
            var lastHistoricalPeriodSummaryStartTime = FifteenSeconds.ContainingPeriodBoundaryStart(historical15sSummariesLatestTime);
            var lastHistoricalEntryTime = instrumentType == InstrumentType.PriceSummaryPeriod
                ? lastHistoricalPeriodSummaryStartTime
                : TimeContext.UtcNow;
            var earliestHistoricalEntryTime = instrumentType == InstrumentType.PriceSummaryPeriod
                ? FifteenSeconds.PreviousPeriodStart(lastHistoricalEntryTime)
                : historicalQuotesStartTime;
            var fileStartDate = lastHistoricalEntryTime.TruncToWeekBoundary();
            lastFileInfoRetrieved
                .Add(new InstrumentFileInfo(tickerId15sPeriod, OneWeek, earliestHistoricalEntryTime, lastHistoricalEntryTime
                                          , new List<DateTime> { fileStartDate }));
            return lastFileInfoRetrieved;
        }
        var latestPeriodSummaryStartTIme = ThirtySeconds.ContainingPeriodBoundaryStart(historical30sSummariesLatestTime);
        var lastEntryTime = instrumentType == InstrumentType.PriceSummaryPeriod
            ? latestPeriodSummaryStartTIme
            : TimeContext.UtcNow;
        var earliestEntryTime = instrumentType == InstrumentType.PriceSummaryPeriod
            ? ThirtySeconds.PreviousPeriodStart(latestPeriodSummaryStartTIme)
            : historicalQuotesStartTime;
        var fileStartTime = lastEntryTime.TruncToWeekBoundary();
        lastFileInfoRetrieved
            .Add(new InstrumentFileInfo(tickerId30sPeriod, OneWeek, earliestEntryTime, lastEntryTime, new List<DateTime> { fileStartTime }));
        return lastFileInfoRetrieved;
    }

    private List<InstrumentFileEntryInfo> GetStubRepoFileEntryInfo
        (string instrumentName, InstrumentType? instrumentType = null, TimeSeriesPeriod? period = null)
    {
        lastFileInfoRetrieved.Clear();
        if (instrumentName == tickerId15sPeriod.Ticker)
        {
            lastFileEntryInfoRetrieved.Add(new InstrumentFileEntryInfo(tickerId15sPeriod, OneWeek, new List<FileEntryInfo>(), 1));
            return lastFileEntryInfoRetrieved;
        }
        lastFileEntryInfoRetrieved.Add(new InstrumentFileEntryInfo(tickerId30sPeriod, OneWeek, new List<FileEntryInfo>(), 1));
        return lastFileEntryInfoRetrieved;
    }

    private IEnumerable<ILevel0Quote> GetStubQuotes(ISourceTickerIdentifier srcTickerId, UnboundedTimeRange? requestTimeRange)
    {
        return lastQuotesRetrieved =
            oneSecondLevel1Quotes
                .Where(q => q.SourceTime >= restrictedRetrievalRange.FromTime
                         && q.SourceTime < restrictedRetrievalRange.ToTime
                         && q.SourceTime >= (requestTimeRange?.FromTime ?? DateTime.MinValue)
                         && q.SourceTime < (requestTimeRange?.ToTime ?? DateTime.MaxValue))
                .ToList();
    }

    private IEnumerable<PricePeriodSummary> GetStubSummaries(ISourceTickerIdentifier srcTickerId, UnboundedTimeRange? requestTimeRange)
    {
        if (srcTickerId.SourceId == tickerId30sPeriod.SourceId)
            return lastSummariesRetrieved =
                fifteenSecondPeriodSummaries
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
    public async Task AllTicksSummary4QuotesOn15sResolver_SendEntriesToPersister_CanRetrieveEntriesFromRepository()
    {
        // set time to 9 seconds
        stubTimeContext.AddSeconds(5);
        historical15sSummariesLatestTime = FifteenSeconds.PreviousPeriodStart(stubTimeContext.UtcNow);
        restrictedRetrievalRange         = new BoundedTimeRange(historicalQuotesStartTime, stubTimeContext.UtcNow);

        var test15sHistoricalPeriodClient = new TestHistoricalPeriodClient(FifteenSeconds, tickerId15sPeriod);
        await indicatorRegistryStubRule.DeployRuleAsync(test15sHistoricalPeriodClient);

        var histResolver15sRule = new HistoricalPeriodSummariesResolverRule<Level1PriceQuote>(fifteenSecondsHistoricalPeriodParams);

        await using var histResolverDeploy = await indicatorRegistryStubRule.DeployRuleAsync(histResolver15sRule);

        Assert.AreEqual(0, lastSummariesRetrieved.Count);
        Assert.AreEqual(15, lastQuotesRetrieved.Count);

        var hasResult = await test15sHistoricalPeriodClient.InvokeResponseRequestOnTestClientQueue
            (new HistoricalPeriodResponseRequest(new BoundedTimeRange(historical15sSummariesLatestTime, seedDateTime)));
        Assert.AreEqual(1, test15sHistoricalPeriodClient.ReceivedToPersistEvents.Count);
        Assert.AreEqual(1, hasResult.Count);
    }

    [TestMethod]
    public async Task MissingOneHistoricalSummary_OnStartupResolverStartsPublishingAvailablePeriods_ReceiveOneHistoricalPeriodToPersist()
    {
        // set time to 24 seconds past seedTime
        stubTimeContext.AddSeconds(24);
        restrictedRetrievalRange = new BoundedTimeRange(historicalQuotesStartTime, stubTimeContext.UtcNow);

        var test30sHistoricalPeriodClient = new TestHistoricalPeriodClient(ThirtySeconds, tickerId30sPeriod);
        await indicatorRegistryStubRule.DeployRuleAsync(test30sHistoricalPeriodClient);

        historical15sSummariesLatestTime = FifteenSeconds.ContainingPeriodBoundaryStart(stubTimeContext.UtcNow);
        var thirtySeconds15sPeriodHistoricalPeriodParams = new HistoricalPeriodParams
            (thirtySecondsHistoricalPeriodParams.TickerId, FifteenSeconds, new TimeLength(TimeSpan.FromMinutes(1)));
        var histResolver15sRule = new HistoricalPeriodSummariesResolverRule<Level1PriceQuote>(thirtySeconds15sPeriodHistoricalPeriodParams);

        await indicatorRegistryStubRule.RegisterAndDeployTickerPeriodService
            (thirtySecondsHistoricalPeriodParams.TickerId, FifteenSeconds
           , ServiceType.HistoricalPricePeriodSummaryResolver, histResolver15sRule);

        historical30sSummariesLatestTime = ThirtySeconds.PreviousPeriodStart(seedDateTime);
        var histResolver30sRule = new HistoricalPeriodSummariesResolverRule<Level1PriceQuote>(thirtySecondsHistoricalPeriodParams);

        await using var histResolverDeploy = await indicatorRegistryStubRule.DeployRuleAsync(histResolver30sRule);

        Assert.AreEqual(5, lastSummariesRetrieved.Count);
        Assert.AreEqual(0, lastQuotesRetrieved.Count);
        Assert.AreEqual(1, test30sHistoricalPeriodClient.ReceivedToPersistEvents.Count);
    }

    private class TestHistoricalPeriodClient : Rule
    {
        private const string HistoricalPeriodTestClientInvokeResponseRequestAddress
            = "TestClient.HistoricalPricePeriodSummary.Invoke.RequestResponse";
        private const string HistoricalPeriodTestClientInvokeStreamRequestAddress = "TestClient.HistoricalPricePeriodSummary.Invoke.StreamRequest";

        private readonly TimeSeriesPeriod        period;
        private readonly ISourceTickerIdentifier tickerId;

        private ISubscription? invokeHistoricalPeriodResponseRequest;
        private ISubscription? invokeHistoricalPeriodStreamRequest;
        private ISubscription? toPersistSubscription;

        public TestHistoricalPeriodClient(TimeSeriesPeriod period, ISourceTickerIdentifier tickerId)
        {
            this.period   = period;
            this.tickerId = tickerId;
        }

        public List<IPricePeriodSummary> ReceivedToPersistEvents { get; } = new();

        public override async ValueTask StartAsync()
        {
            invokeHistoricalPeriodResponseRequest
                = await this.RegisterRequestListenerAsync<HistoricalPeriodResponseRequest, List<IPricePeriodSummary>>
                    (HistoricalPeriodTestClientInvokeResponseRequestAddress, TestRequestResponseHandler);
            invokeHistoricalPeriodStreamRequest = await this.RegisterRequestListenerAsync<HistoricalPeriodStreamRequest, List<IPricePeriodSummary>>
                (HistoricalPeriodTestClientInvokeStreamRequestAddress, TestStreamRequestHandler);
            toPersistSubscription = await this.RegisterListenerAsync<IPricePeriodSummary>
                (tickerId.PersistAppendPeriodSummaryPublish(period), SaveToPersistPeriodSummariesHandler);
            await base.StartAsync();
        }

        public async ValueTask<List<IPricePeriodSummary>> InvokeResponseRequestOnTestClientQueue
            (HistoricalPeriodResponseRequest request) =>
            await this.RequestAsync<HistoricalPeriodResponseRequest, List<IPricePeriodSummary>>
                (HistoricalPeriodTestClientInvokeResponseRequestAddress, request);

        public async ValueTask<List<IPricePeriodSummary>> InvokeStreamRequestOnTestClientQueue
            (HistoricalPeriodStreamRequest request) =>
            await this.RequestAsync<HistoricalPeriodStreamRequest, List<IPricePeriodSummary>>
                (HistoricalPeriodTestClientInvokeStreamRequestAddress, request);

        private void SaveToPersistPeriodSummariesHandler(IBusMessage<IPricePeriodSummary> toPersistMsg)
        {
            var pricePeriodSummary = toPersistMsg.Payload.Body()!;
            ReceivedToPersistEvents.Add(pricePeriodSummary);
        }

        private async ValueTask<List<IPricePeriodSummary>> TestRequestResponseHandler
            (IBusRespondingMessage<HistoricalPeriodResponseRequest, List<IPricePeriodSummary>> requestResponseMsg)
        {
            var historicalRequest = requestResponseMsg.Payload.Body()!;

            return await this.RequestAsync<HistoricalPeriodResponseRequest, List<IPricePeriodSummary>>
                (tickerId.HistoricalPeriodSummaryResponseRequest(period), historicalRequest);
        }

        private async ValueTask<List<IPricePeriodSummary>> TestStreamRequestHandler
            (IBusRespondingMessage<HistoricalPeriodStreamRequest, List<IPricePeriodSummary>> streamRequestMsg)
        {
            var historicalRequest = streamRequestMsg.Payload.Body()!;

            var limitedRecycler = Context.PooledRecycler.Borrow<LimitedBlockingRecycler>();
            limitedRecycler.MaxTypeBorrowLimit = 200;
            var remainingPeriods      = new List<IPricePeriodSummary>();
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
                = new HistoricalPeriodStreamRequest(historicalRequest.RequestTimeRange, new ResponsePublishParams(channelStreamRequest));

            var expectResults = await this.RequestAsync<HistoricalPeriodStreamRequest, bool>
                (tickerId.HistoricalPeriodSummaryStreamRequest(period), historicalRequest);

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
