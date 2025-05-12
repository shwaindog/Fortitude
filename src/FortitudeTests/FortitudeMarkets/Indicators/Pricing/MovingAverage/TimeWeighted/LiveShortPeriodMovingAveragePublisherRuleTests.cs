// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.BusMessaging.Messages.ListeningSubscriptions;
using FortitudeBusRules.BusMessaging.Routing.Response;
using FortitudeBusRules.Rules;
using FortitudeCommon.Chronometry;
using FortitudeCommon.Chronometry.Timers;
using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.Extensions;
using FortitudeCommon.Monitoring.Logging;
using FortitudeMarkets.Indicators;
using FortitudeMarkets.Indicators.Pricing;
using FortitudeMarkets.Indicators.Pricing.MovingAverage;
using FortitudeMarkets.Indicators.Pricing.MovingAverage.TimeWeighted;
using FortitudeMarkets.Indicators.Pricing.Parameters;
using FortitudeMarkets.Pricing;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Converters;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes;
using FortitudeMarkets.Pricing.PQ.Subscription.BusRules;
using FortitudeMarkets.Pricing.PQ.TimeSeries.BusRules;
using FortitudeTests.FortitudeBusRules.BusMessaging;
using FortitudeTests.FortitudeCommon.Chronometry;
using FortitudeTests.FortitudeCommon.Chronometry.Timers;
using FortitudeTests.FortitudeMarkets.Indicators.Config;
using FortitudeTests.FortitudeMarkets.Pricing.FeedEvents.Quotes;
using FortitudeTests.FortitudeMarkets.Pricing.Quotes;
using MathNet.Numerics;
using static FortitudeCommon.Chronometry.TimeBoundaryPeriod;
using static FortitudeMarkets.Configuration.ClientServerConfig.MarketClassificationExtensions;
using static FortitudeMarkets.Pricing.FeedEvents.TickerInfo.TickerQuoteDetailLevel;

#endregion

namespace FortitudeTests.FortitudeMarkets.Indicators.Pricing.MovingAverage.TimeWeighted;

[TestClass]
public class LiveShortPeriodMovingAveragePublisherRuleTests : OneOfEachMessageQueueTypeTestSetup
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(LiveShortPeriodMovingAveragePublisherRuleTests));

    private readonly DateTime quotesStart = new(2024, 7, 11);
    private readonly DateTime testEpoch   = new(2024, 7, 11, 0, 10, 0);

    private readonly SourceTickerInfo tickerInfo = new
        (2, "SourceName", 2, "TickerName2", Level1Quote, Unknown
       , 1, 0.001m, 10m, 100m, 10m);

    private decimal highLowSpread;

    private int incrementEvery = 8;

    private IndicatorServiceRegistryStubRule indicatorRegistryStubRule = null!;

    private PricingIndicatorId indicatorTickerIdentifier;

    private DateTime lastGeneratedQuotesEndTime;
    private decimal  midIncrement;

    private decimal midStart;

    private decimal spread;

    private StubTimeContext stubTimeContext = null!;

    private List<PQPublishableLevel1Quote> tenMinBeforeEpoch = null!;

    private TestLiveShortPeriodMovingAverageClient testClient           = null!;
    private IRuleDeploymentLifeTime                testClientDeployment = null!;

    private IList<IAsyncDisposable> undeploy = null!;

    [TestInitialize]
    public async Task Setup()
    {
        tickerInfo.Register();
        indicatorTickerIdentifier = new PricingIndicatorId(IndicatorConstants.MovingAverageTimeWeightedBidAskId, (IPricingInstrumentId)tickerInfo);
        indicatorTickerIdentifier.Register();

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

        tickerInfo.Register();
        testClient           = new TestLiveShortPeriodMovingAverageClient(indicatorTickerIdentifier);
        testClientDeployment = await WorkerQueue1.LaunchRuleAsync(indicatorRegistryStubRule, testClient, WorkerQueue1SelectionResult);

        undeploy.Add(preqDeploy);
    }

    public override ITimerProvider ResolverTimerProvider()
    {
        TimeContext.Provider = stubTimeContext = new StubTimeContext(testEpoch);
        return new StubTimerContextProvider();
    }

    private List<PQPublishableLevel1Quote> GenerateQuotes
    (DateTime? startAt = null, TimeBoundaryPeriod quoteGap = TenSeconds, decimal? startAtMid = null,
        int numberToGenerate = 61, int? incrementQuoteEvery = null)
    {
        var quoteTime  = quoteGap.PreviousPeriodStart(startAt ?? quotesStart);
        var currentMid = startAtMid ?? midStart;
        var incAt      = incrementQuoteEvery ?? incrementEvery;
        var quotes     = new List<PQPublishableLevel1Quote>(numberToGenerate);
        for (var i = 1; i <= numberToGenerate; i++)
        {
            if (i % incAt == 0) currentMid += midIncrement;
            quotes.Add(tickerInfo.CreatePublishableLevel1Quote(quoteTime = quoteGap.PeriodEnd(quoteTime), currentMid, spread).ToPublishableL1PQQuote());
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

    [TestMethod]
    [Timeout(20_000)]
    public async Task New8SMovingAverageRuleEvery1S_RequestsHistoricalQuotes_CalculatesExpectedMovingAverageFromHistorical()
    {
        var live8SMovingAverage1SPubParams = new LiveShortPeriodMovingAveragePublishParams
            (indicatorTickerIdentifier
           , new MovingAveragePublisherParams
                 (new IndicatorPublishInterval(OneSecond), new ResponsePublishParams()
                , new CalculateMovingAverageOptions(TimeLengthFlags.ValidPeriodTime)
                , new MovingAverageOffset(new DiscreetTimePeriod(TimeSpan.FromSeconds(8)))));

        var live8SMovingAverage1SPubRule = new LiveShortPeriodMovingAveragePublisherRule(live8SMovingAverage1SPubParams);

        testClient.HistoricalRepositoryQuotesToReturn = tenMinBeforeEpoch;

        await indicatorRegistryStubRule.DeployChildRuleAsync(live8SMovingAverage1SPubRule);

        await stubTimeContext.AddSecondsAsync(1);

        var generatedMovingAverage = await testClient.GetPublished8SMovingAveragePeriods(1);

        Assert.IsNotNull(generatedMovingAverage);
        Assert.AreEqual(1, generatedMovingAverage.Count);
        var movingAverage = generatedMovingAverage[0];
        var expectedMovingAverage = CalculatedMovingAverage
            (tenMinBeforeEpoch, new BoundedTimeRange(testEpoch.AddSeconds(-8), testEpoch));
        Assert.AreEqual(stubTimeContext.UtcNow, movingAverage.AtTime);
        Assert.AreEqual(expectedMovingAverage.BidPrice, movingAverage.BidPrice);
        Assert.AreEqual(expectedMovingAverage.AskPrice, movingAverage.AskPrice);

        var threeOneSecondIncrementingQuotes = GenerateQuotes(stubTimeContext.UtcNow, OneSecond, midStart, 3, 1);
        await testClient.SendPricesToLivePeriodRule(threeOneSecondIncrementingQuotes, stubTimeContext);
        testClient.CreateNewWait(3);
        var threeMoreSecondsMovingAverages = await testClient.GetPublished8SMovingAveragePeriods(4);
        Assert.AreEqual(4, threeMoreSecondsMovingAverages.Count);
        tenMinBeforeEpoch.AddRange(threeOneSecondIncrementingQuotes);
        expectedMovingAverage = CalculatedMovingAverage
            (tenMinBeforeEpoch, new BoundedTimeRange(testEpoch.AddSeconds(-6), testEpoch.AddSeconds(2)));
        movingAverage = generatedMovingAverage[1];
        Assert.AreEqual(expectedMovingAverage.BidPrice, movingAverage.BidPrice);
        Assert.AreEqual(expectedMovingAverage.AskPrice, movingAverage.AskPrice);
        expectedMovingAverage = CalculatedMovingAverage
            (tenMinBeforeEpoch, new BoundedTimeRange(testEpoch.AddSeconds(-5), testEpoch.AddSeconds(3)));
        movingAverage = generatedMovingAverage[2];
        Assert.AreEqual(expectedMovingAverage.BidPrice, movingAverage.BidPrice);
        Assert.AreEqual(expectedMovingAverage.AskPrice, movingAverage.AskPrice);
        expectedMovingAverage = CalculatedMovingAverage
            (tenMinBeforeEpoch, new BoundedTimeRange(testEpoch.AddSeconds(-4), testEpoch.AddSeconds(4)));
        movingAverage = generatedMovingAverage[3];
        Assert.AreEqual(expectedMovingAverage.BidPrice, movingAverage.BidPrice);
        Assert.AreEqual(expectedMovingAverage.AskPrice, movingAverage.AskPrice);
    }

    public BidAskPair CalculatedMovingAverage(List<PQPublishableLevel1Quote> quotes, BoundedTimeRange matchingRange)
    {
        var timeWeightedBidAskPair = CalculateValidTimeWeightedBidAsk(quotes, matchingRange);
        var average                = TimeWeightBidAskToAverage(timeWeightedBidAskPair, matchingRange.TimeSpan());

        Logger.Info("Verify Calculated Average Bid: {0}, Ask: {1} for {2}s  at {3}", average.BidPrice, average.AskPrice, matchingRange.TimeSpan()
                  , matchingRange.ToTime);
        return average;
    }

    public BidAskPair CalculateValidTimeWeightedBidAsk(List<PQPublishableLevel1Quote> quotes, BoundedTimeRange matchingRange)
    {
        var timeWeightedBidMs = 0m;
        var timeWeightedAskMs = 0m;
        var i                 = 0;
        for (; i < quotes.Count - 1; i++)
        {
            var current = quotes[i];
            var next    = quotes[i + 1];

            if (!(matchingRange.Contains(current.SourceTime) || matchingRange.Contains(next.SourceTime))) continue;
            var validStart    = current.ValidFrom.Max(matchingRange.FromTime);
            var validValidEnd = current.ValidTo.Min(matchingRange.ToTime).Min(next.SourceTime);
            if (validStart >= validValidEnd) continue;

            var validMs = (decimal)(validValidEnd - validStart).TotalMilliseconds;

            timeWeightedBidMs += current.BidPriceTop * validMs;
            timeWeightedAskMs += current.AskPriceTop * validMs;

            Logger.Info("Verify Bid: {0}, Ask: {1} for {2}ms at {3}", current.BidPriceTop, current.AskPriceTop, validMs.Round(0), current.SourceTime);
        }
        if (i > 0 && i == quotes.Count - 1)
        {
            var lastQuote = quotes[^1];
            if (matchingRange.Contains(lastQuote.SourceTime))
            {
                var validStart    = lastQuote.ValidFrom.Max(matchingRange.FromTime);
                var validValidEnd = lastQuote.ValidTo.Min(matchingRange.ToTime);
                var validMs       = (decimal)(validValidEnd - validStart).TotalMilliseconds;
                timeWeightedBidMs += lastQuote.BidPriceTop * validMs;
                timeWeightedAskMs += lastQuote.AskPriceTop * validMs;
                Logger.Info("Verify Bid: {0}, Ask: {1} for {2}ms  at {3}", lastQuote.BidPriceTop, lastQuote.AskPriceTop, validMs.Round(0)
                          , lastQuote.SourceTime);
            }
        }
        return new BidAskPair(timeWeightedBidMs, timeWeightedAskMs);
    }

    public BidAskPair TimeWeightBidAskToAverage(BidAskPair timeWeightedBidAskMs, TimeSpan period)
    {
        var periodMs = (decimal)period.TotalMilliseconds;

        return new BidAskPair(timeWeightedBidAskMs.BidPrice / periodMs, timeWeightedBidAskMs.AskPrice / periodMs);
    }

    private struct PublishQuotesWithTimeProgress
    {
        public PublishQuotesWithTimeProgress(List<PQPublishableLevel1Quote> toPublish, IUpdateTime timeUpdater)
        {
            TimeUpdater = timeUpdater;
            ToPublish   = toPublish;
        }

        public List<PQPublishableLevel1Quote> ToPublish { get; }

        public IUpdateTime TimeUpdater { get; }
    }

    private class TestLiveShortPeriodMovingAverageClient
        (PricingInstrumentId instrumentSourceTickerIdentifier, int waitNumberForPublish = 1) : Rule
    {
        private const string LivePeriodTestClientPublishPricesAddress
            = "TestClient.LiveShortPeriodMovingAverage.Publish.Quotes";

        private const string LivePeriodTestClientReturnHistoricalQuotesAddress
            = "TestClient.LiveShortPeriodMovingAverage.Return.HistoricalQuoates";

        private TaskCompletionSource<int> awaitPublishSource = new();

        private ISubscription? historicalQuoteRepoRequestSubscription;
        private ISubscription? historicalQuoteReturnResultsSubscription;

        private List<PQPublishableLevel1Quote> historicalRepositoryQuotesToReturn = new();
        private ISubscription?      listenForPublishPricesSubscription;
        private ISubscription?      live15SMovingAveragePublishSubscription;

        private ISubscription? live30SMovingAveragePublishSubscription;

        private string quoteListenAddress = null!;

        private List<IndicatorValidRangeBidAskPeriodValue> ReceivedLive8SMovingAverageEvents { get; } = new();

        private List<IndicatorValidRangeBidAskPeriodValue> ReceivedLive30SMovingAverageEvents { get; } = new();

        public List<HistoricalQuotesRequest<PQPublishableLevel1Quote>> ReceivedQuoteHistoricalRequests { get; } = new();

        public List<PQPublishableLevel1Quote> HistoricalRepositoryQuotesToReturn
        {
            get => historicalRepositoryQuotesToReturn;
            set
            {
                historicalRepositoryQuotesToReturn = new List<PQPublishableLevel1Quote>(value);
                historicalRepositoryQuotesToReturn.Reverse();
            }
        }

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
            historicalQuoteRepoRequestSubscription = await this.RegisterRequestListenerAsync<HistoricalQuotesRequest<PQPublishableLevel1Quote>, bool>
                (HistoricalQuoteTimeSeriesRepositoryConstants.PricingRepoRetrievePqL1QuoteRequest, HistoricalQuoteRepositoryRequest);
            historicalQuoteReturnResultsSubscription = await this.RegisterListenerAsync<HistoricalQuotesRequest<PQPublishableLevel1Quote>>
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

        private bool HistoricalQuoteRepositoryRequest(HistoricalQuotesRequest<PQPublishableLevel1Quote> historicalQuoteReq)
        {
            if (HistoricalRepositoryQuotesToReturn.Any())
            {
                this.Publish(LivePeriodTestClientReturnHistoricalQuotesAddress, historicalQuoteReq);
                return true;
            }

            return false;
        }

        private async ValueTask ReturnHistoricalQuotes(HistoricalQuotesRequest<PQPublishableLevel1Quote> historicalQuoteReq)
        {
            ReceivedQuoteHistoricalRequests.Add(historicalQuoteReq);

            var channel  = historicalQuoteReq.ChannelRequest.PublishChannel;
            var sendMore = true;
            if (historicalQuoteReq.ChannelRequest.BatchSize > 1)
                for (var i = 0; i < HistoricalRepositoryQuotesToReturn.Count; i += historicalQuoteReq.ChannelRequest.BatchSize)
                {
                    var reusableList = Context.PooledRecycler.Borrow<ReusableList<PQPublishableLevel1Quote>>();
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

        public async ValueTask SendPricesToLivePeriodRule(List<PQPublishableLevel1Quote> publishPrices, IUpdateTime progressTime)
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
                await this.PublishAsync(quoteListenAddress, level1PriceQuote);
                await timeUpdater.UpdateTime(level1PriceQuote.SourceTime);
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
