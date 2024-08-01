// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.BusMessaging.Messages.ListeningSubscriptions;
using FortitudeBusRules.BusMessaging.Routing.Response;
using FortitudeBusRules.Rules;
using FortitudeCommon.Chronometry;
using FortitudeCommon.Chronometry.Timers;
using FortitudeCommon.DataStructures.Lists;
using FortitudeMarketsApi.Indicators;
using FortitudeMarketsApi.Indicators.Pricing;
using FortitudeMarketsApi.Pricing;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Indicators;
using FortitudeMarketsCore.Indicators.Pricing;
using FortitudeMarketsCore.Indicators.Pricing.MovingAverage;
using FortitudeMarketsCore.Indicators.Pricing.MovingAverage.TimeWeighted;
using FortitudeMarketsCore.Indicators.Pricing.Parameters;
using FortitudeMarketsCore.Pricing.PQ.Converters;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Subscription.BusRules;
using FortitudeMarketsCore.Pricing.PQ.TimeSeries.BusRules;
using FortitudeMarketsCore.Pricing.Quotes;
using FortitudeTests.FortitudeBusRules.BusMessaging;
using FortitudeTests.FortitudeCommon.Chronometry;
using FortitudeTests.FortitudeCommon.Chronometry.Timers;
using FortitudeTests.FortitudeMarketsCore.Indicators.Config;
using FortitudeTests.FortitudeMarketsCore.Pricing.Quotes;
using static FortitudeCommon.Chronometry.TimeBoundaryPeriod;
using static FortitudeMarketsApi.Configuration.ClientServerConfig.MarketClassificationExtensions;
using static FortitudeMarketsApi.Pricing.Quotes.TickerDetailLevel;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Indicators.Pricing.MovingAverage.TimeWeighted;

[TestClass]
public class LiveShortPeriodMovingAveragePublisherRuleTests : OneOfEachMessageQueueTypeTestSetup
{
    private readonly DateTime quotesStart = new(2024, 7, 11);
    private readonly DateTime testEpoch   = new(2024, 7, 11, 0, 10, 0);

    private readonly SourceTickerInfo tickerId15SPeriod = new
        (2, "SourceName", 2, "TickerName2", Level1Quote, Unknown
       , 1, 0.001m, 10m, 100m, 10m);

    private decimal highLowSpread;

    private int incrementEvery = 8;

    private IndicatorServiceRegistryStubRule indicatorRegistryStubRule = null!;

    private IndicatorSourceTickerIdentifier indicatorTickerIdentifier;

    private DateTime lastGeneratedQuotesEndTime;
    private decimal  midIncrement;

    private decimal midStart;

    private decimal spread;

    private StubTimeContext stubTimeContext = null!;

    private List<PQLevel1Quote> tenMinBeforeEpoch = null!;

    private TestLiveShortPeriodMovingAverageClient testClient;
    private IRuleDeploymentLifeTime                testClientDeployment;

    private IList<IAsyncDisposable> undeploy = null!;

    [TestInitialize]
    public async Task Setup()
    {
        indicatorTickerIdentifier = new IndicatorSourceTickerIdentifier(IndicatorConstants.MovingAverageTimeWeightedBidAskId, tickerId15SPeriod);

        undeploy = new List<IAsyncDisposable>();

        midIncrement = 8.0000m;
        midStart     = 100.0000m;

        spread = 2.000m;

        tenMinBeforeEpoch = GenerateQuotes();

        var unitTestNoRepositoryConfig = IndicatorServicesConfigTests.UnitTestNoRepositoryConfig();
        unitTestNoRepositoryConfig.PersistenceConfig.PersistPriceSummaries = true;
        indicatorRegistryStubRule
            = new IndicatorServiceRegistryStubRule
                (new IndicatorServiceRegistryParams(unitTestNoRepositoryConfig));

        var preqDeploy
            = await EventQueue1.LaunchRuleAsync
                (indicatorRegistryStubRule, indicatorRegistryStubRule, EventQueue1SelectionResult);

        tickerId15SPeriod.Register();
        testClient           = new TestLiveShortPeriodMovingAverageClient(indicatorTickerIdentifier);
        testClientDeployment = await WorkerQueue1.LaunchRuleAsync(indicatorRegistryStubRule, testClient, WorkerQueue1SelectionResult);


        undeploy.Add(preqDeploy);
    }

    public override ITimerProvider ResolverTimerProvider()
    {
        TimeContext.Provider = stubTimeContext = new StubTimeContext(testEpoch);
        return new StubTimerContextProvider();
    }

    private List<PQLevel1Quote> GenerateQuotes
    (DateTime? startAt = null, TimeBoundaryPeriod quoteGap = TenSeconds, decimal? startAtMid = null, int numberToGenerate = 60
      , int? incrementQuoteEvery = null)
    {
        var quoteTime  = startAt ?? quotesStart;
        var currentMid = startAtMid ?? midStart;
        var incAt      = incrementQuoteEvery ?? incrementEvery;
        var quotes     = new List<PQLevel1Quote>(numberToGenerate);
        for (var i = 1; i <= numberToGenerate; i++)
        {
            if (i % incAt == 0) currentMid += midIncrement;
            quotes.Add(tickerId15SPeriod.CreateLevel1Quote(quoteTime = quoteGap.PeriodEnd(quoteTime), currentMid, spread).ToL1PQQuote());
        }
        lastGeneratedQuotesEndTime = quoteTime;
        return quotes;
    }

    [TestCleanup]
    public async Task TearDown()
    {
        await stubTimeContext.DisposeAsync();
        await testClientDeployment.DisposeAsync();
        foreach (var asyncDisposable in undeploy) await asyncDisposable.DisposeAsync();
    }

    // [TestMethod]
    public async Task New8SMovingAverageRuleEvery1S_RequestsHistoricalQuotes_CalculatesExpectedMovingAverageFromHistorical()
    {
        var live8SMovingAverage1SPubParams = new LiveShortPeriodMovingAveragePublishParams
            (indicatorTickerIdentifier
           , new MovingAveragePublisherParams
                 (new IndicatorPublishInterval(OneSecond), new ResponsePublishParams()
                , new CalculateMovingAverageOptions(TimeLengthFlags.ValidPeriodTime)
                , new MovingAverageOffset(new DiscreetTimePeriod(TimeSpan.FromSeconds(8)))));

        var live8SMovingAverage1SPubRule = new LiveShortPeriodMovingAveragePublisherRule(live8SMovingAverage1SPubParams);

        var fromRepository = new List<PQLevel1Quote>(tenMinBeforeEpoch);
        fromRepository.Reverse();
        testClient.HistoricalRepositoryQuotesToReturn = fromRepository;

        await indicatorRegistryStubRule.DeployRuleAsync(live8SMovingAverage1SPubRule);

        await stubTimeContext.AddSecondsAsync(1);

        var generatedMovingAverage = await testClient.GetPublished8SMovingAveragePeriods(1);

        Assert.IsNotNull(generatedMovingAverage);
        Assert.AreEqual(1, generatedMovingAverage.Count);
    }

    private struct PublishQuotesWithTimeProgress
    {
        public PublishQuotesWithTimeProgress(List<Level1PriceQuote> toPublish, IUpdateTime timeUpdater)
        {
            TimeUpdater = timeUpdater;
            ToPublish   = toPublish;
        }

        public List<Level1PriceQuote> ToPublish { get; }

        public IUpdateTime TimeUpdater { get; }
    }

    private class TestLiveShortPeriodMovingAverageClient
        (IndicatorSourceTickerIdentifier instrumentSourceTickerIdentifier, int waitNumberForPublish = 1) : Rule
    {
        private const string LivePeriodTestClientPublishPricesAddress
            = "TestClient.LiveShortPeriodMovingAverage.Publish.Quotes";

        private const string LivePeriodTestClientReturnHistoricalQuotesAddress
            = "TestClient.LiveShortPeriodMovingAverage.Return.HistoricalQuoates";

        private TaskCompletionSource<int> awaitPublishSource = new();

        private ISubscription? historicalQuoteRepoRequestSubscription;
        private ISubscription? historicalQuoteReturnResultsSubscription;
        private ISubscription? listenForPublishPricesSubscription;
        private ISubscription? live15SMovingAveragePublishSubscription;

        private ISubscription? live30SMovingAveragePublishSubscription;

        private string quoteListenAddress = null!;

        private List<IndicatorValidRangeBidAskPeriodValue> ReceivedLive8SMovingAverageEvents { get; } = new();

        private List<IndicatorValidRangeBidAskPeriodValue> ReceivedLive30SMovingAverageEvents { get; } = new();

        public List<HistoricalQuotesRequest<PQLevel1Quote>> ReceivedQuoteHistoricalRequests { get; } = new();

        public List<PQLevel1Quote> HistoricalRepositoryQuotesToReturn { get; set; } = new();

        public override async ValueTask StartAsync()
        {
            quoteListenAddress = instrumentSourceTickerIdentifier.Source.SubscribeToTickerQuotes(instrumentSourceTickerIdentifier.Ticker);
            listenForPublishPricesSubscription = await this.RegisterRequestListenerAsync<PublishQuotesWithTimeProgress, ValueTask>
                (LivePeriodTestClientPublishPricesAddress, PublishPriceQuotesHandler);
            live15SMovingAveragePublishSubscription = await this.RegisterListenerAsync<IndicatorValidRangeBidAskPeriodValue>
                (((SourceTickerIdentifier)instrumentSourceTickerIdentifier)
                 .MovingAverageTimeWeightedLiveShortPeriodPublish(new DiscreetTimePeriod(TimeSpan.FromSeconds(8))), Received8SMovingAveragePeriod);
            live30SMovingAveragePublishSubscription = await this.RegisterListenerAsync<IndicatorValidRangeBidAskPeriodValue>
                (((SourceTickerIdentifier)instrumentSourceTickerIdentifier)
                 .MovingAverageTimeWeightedLiveShortPeriodPublish(new DiscreetTimePeriod(ThirtySeconds)), Received30SMovingAveragePeriod);
            historicalQuoteRepoRequestSubscription = await this.RegisterRequestListenerAsync<HistoricalQuotesRequest<PQLevel1Quote>, bool>
                (HistoricalQuoteTimeSeriesRepositoryConstants.PricingRepoRetrievePqL1QuoteRequest, HistoricalQuoteRepositoryRequest);
            historicalQuoteReturnResultsSubscription = await this.RegisterListenerAsync<HistoricalQuotesRequest<PQLevel1Quote>>
                (LivePeriodTestClientReturnHistoricalQuotesAddress, ReturnHistoricalQuotes);
            await base.StartAsync();
        }

        public void CreateNewWait(int waitForLive = 1)
        {
            waitNumberForPublish = waitForLive;
            awaitPublishSource   = new TaskCompletionSource<int>();
        }

        public async ValueTask<List<IndicatorValidRangeBidAskPeriodValue>> GetPublished8SMovingAveragePeriods(int waitNumber = 1)
        {
            waitNumberForPublish = waitNumber;
            if (ReceivedLive8SMovingAverageEvents.Count < waitNumber) await Task.WhenAny(awaitPublishSource.Task, Task.Delay(2_000));
            return ReceivedLive8SMovingAverageEvents;
        }

        private void Received8SMovingAveragePeriod(IndicatorValidRangeBidAskPeriodValue movingAverage)
        {
            ReceivedLive8SMovingAverageEvents.Add(movingAverage);
            if (ReceivedLive8SMovingAverageEvents.Count >= waitNumberForPublish) awaitPublishSource.TrySetResult(0);
        }

        private void Received30SMovingAveragePeriod(IndicatorValidRangeBidAskPeriodValue movingAverage)
        {
            ReceivedLive30SMovingAverageEvents.Add(movingAverage);
            if (ReceivedLive30SMovingAverageEvents.Count >= waitNumberForPublish) awaitPublishSource.TrySetResult(0);
        }

        private bool HistoricalQuoteRepositoryRequest(HistoricalQuotesRequest<PQLevel1Quote> historicalQuoteReq)
        {
            if (HistoricalRepositoryQuotesToReturn.Any())
            {
                this.Publish(LivePeriodTestClientReturnHistoricalQuotesAddress, historicalQuoteReq);
                return true;
            }

            return false;
        }

        private async ValueTask ReturnHistoricalQuotes(HistoricalQuotesRequest<PQLevel1Quote> historicalQuoteReq)
        {
            ReceivedQuoteHistoricalRequests.Add(historicalQuoteReq);

            var channel  = historicalQuoteReq.ChannelRequest.PublishChannel;
            var sendMore = true;
            if (historicalQuoteReq.ChannelRequest.BatchSize > 1)
                for (var i = 0; i < HistoricalRepositoryQuotesToReturn.Count; i += historicalQuoteReq.ChannelRequest.BatchSize)
                {
                    var reusableList = Context.PooledRecycler.Borrow<ReusableList<PQLevel1Quote>>();
                    for (var j = i; j < i + historicalQuoteReq.ChannelRequest.BatchSize && j < HistoricalRepositoryQuotesToReturn.Count; j++)
                        reusableList.Add(HistoricalRepositoryQuotesToReturn[j]);
                    sendMore = await channel.Publish(this, reusableList);
                    if (!sendMore) break;
                }
            else
                foreach (var pqLevel1Quote in HistoricalRepositoryQuotesToReturn)
                {
                    sendMore = await channel.Publish(this, pqLevel1Quote);
                    if (!sendMore) break;
                }
            await channel.PublishComplete(this);
        }

        public async ValueTask SendPricesToLivePeriodRule(List<Level1PriceQuote> publishPrices, IUpdateTime progressTime)
        {
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
            await live15SMovingAveragePublishSubscription.NullSafeUnsubscribe();
            await listenForPublishPricesSubscription.NullSafeUnsubscribe();
            await live30SMovingAveragePublishSubscription.NullSafeUnsubscribe();
            await historicalQuoteRepoRequestSubscription.NullSafeUnsubscribe();
            await base.StopAsync();
        }
    }
}
