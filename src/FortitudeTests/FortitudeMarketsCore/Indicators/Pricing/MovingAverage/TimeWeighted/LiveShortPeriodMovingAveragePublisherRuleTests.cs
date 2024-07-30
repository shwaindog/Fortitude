// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.Chronometry.Timers;
using FortitudeIO.TimeSeries;
using FortitudeMarketsApi.Indicators;
using FortitudeMarketsApi.Indicators.Pricing;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Indicators;
using FortitudeMarketsCore.Indicators.Pricing.Parameters;
using FortitudeMarketsCore.Pricing.Quotes;
using FortitudeTests.FortitudeBusRules.BusMessaging;
using FortitudeTests.FortitudeCommon.Chronometry;
using FortitudeTests.FortitudeCommon.Chronometry.Timers;
using FortitudeTests.FortitudeMarketsCore.Indicators.Config;
using FortitudeTests.FortitudeMarketsCore.Pricing.Quotes;
using static FortitudeIO.TimeSeries.TimeSeriesPeriod;
using static FortitudeMarketsApi.Configuration.ClientServerConfig.MarketClassificationExtensions;
using static FortitudeMarketsApi.Pricing.Quotes.TickerDetailLevel;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Indicators.Pricing.MovingAverage.TimeWeighted;

[TestClass]
public class LiveShortPeriodMovingAveragePublisherRuleTests : OneOfEachMessageQueueTypeTestSetup
{
    private readonly DateTime testEpochTime = new(2024, 7, 11);

    private readonly SourceTickerInfo tickerId15SPeriod = new
        (2, "SourceName", 2, "TickerName2", Level1Quote, Unknown
       , 1, 0.001m, 10m, 100m, 10m);


    private LiveShortPeriodMovingAveragePublishParams fifteenSecondsLivePeriodParams;

    private decimal highLowSpread;

    private IndicatorServiceRegistryStubRule indicatorRegistryStubRule = null!;

    private decimal mid1;
    private decimal mid2;

    private List<Level1PriceQuote> oneSecondLevel1Quotes = null!;

    private decimal spread;

    private StubTimeContext stubTimeContext = null!;

    private IList<IAsyncDisposable> undeploy = null!;

    [TestInitialize]
    public async Task Setup()
    {
        undeploy = new List<IAsyncDisposable>();

        mid1 = 0.5000m;
        mid2 = 1.0000m;

        spread        = 0.2000m;
        highLowSpread = 0.8000m;

        Generate1SQuotes();

        fifteenSecondsLivePeriodParams
            = new LiveShortPeriodMovingAveragePublishParams
                (new IndicatorSourceTickerIdentifier(IndicatorConstants.BidAskMovingAverageId, tickerId15SPeriod));
        var unitTestNoRepositoryConfig = IndicatorServicesConfigTests.UnitTestNoRepositoryConfig();
        unitTestNoRepositoryConfig.PersistenceConfig.PersistPriceSummaries = true;
        indicatorRegistryStubRule
            = new IndicatorServiceRegistryStubRule
                (new IndicatorServiceRegistryParams(unitTestNoRepositoryConfig));

        var preqDeploy
            = await EventQueue1.LaunchRuleAsync
                (indicatorRegistryStubRule, indicatorRegistryStubRule, EventQueue1SelectionResult);
        undeploy.Add(preqDeploy);
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

    [TestCleanup]
    public async Task TearDown()
    {
        await stubTimeContext.DisposeAsync();
        foreach (var asyncDisposable in undeploy) await asyncDisposable.DisposeAsync();
    }

    // private class TestLiveMovingAverageClient
    //     (IndicatorSourceTickerIdentifier instrumentSourceTickerIdentifier, int waitNumberForCompleted = 1, int waitNumberForLive = 1) : Rule
    // {
    //     private const string LivePeriodTestClientPublishPricesAddress
    //         = "TestClient.LivePricePeriodSummary.Publish.Quotes";
    //
    //     private readonly Dictionary<string, TimeSeriesPeriod>        historicalSubPeriodAddressToSubPeriodLookup = new();
    //     private readonly Dictionary<TimeSeriesPeriod, ISubscription> historicalSubPeriodRequestSubscriptions     = new();
    //
    //     private readonly Dictionary<TimeSeriesPeriod, Func<ValueTask<List<PricePeriodSummary>>>> historicalSubPeriodResponseCallbacks = new();
    //
    //     private readonly Dictionary<string, TimeSeriesPeriod> liveSubPeriodAddressToSubReceivedHistorical = new();
    //
    //     private readonly Dictionary<TimeSeriesPeriod, ISubscription> liveSubPeriodCompletePublisherSubscriptions = new();
    //
    //     private readonly Dictionary<TimeSeriesPeriod, List<PricePeriodSummary>> receivedCompleteSubPeriods = new();
    //
    //     private TaskCompletionSource<int> awaitCompleteSource = new();
    //     private TaskCompletionSource<int> awaitLiveSource     = new();
    //
    //     private ISubscription? completePublishSubscription;
    //     private ISubscription? listenForPublishPricesSubscription;
    //     private ISubscription? livePublishSubscription;
    //
    //     private string quoteListenAddress = null!;
    //
    //     private List<BidAskInstantPair> ReceivedLive15SMovingAverageEvents { get; } = new();
    //
    //     private List<BidAskInstantPair> ReceivedLive30SMovingAverageEvents { get; } = new();
    //
    //     public List<HistoricalPeriodResponseRequest> ReceivedSubPeriodHistoricalRequests { get; } = new();
    //
    //     public override async ValueTask StartAsync()
    //     {
    //         quoteListenAddress = pricingInstrumentId.Source.SubscribeToTickerQuotes(pricingInstrumentId.Ticker);
    //         listenForPublishPricesSubscription = await this.RegisterRequestListenerAsync<PublishQuotesWithTimeProgress, ValueTask>
    //             (LivePeriodTestClientPublishPricesAddress, PublishPriceQuotesHandler);
    //         livePublishSubscription = await this.RegisterListenerAsync<PricePeriodSummary>
    //             (pricingInstrumentId.LivePeriodSummaryAddress(), ReceivedLivePricePeriodSummaries);
    //         completePublishSubscription = await this.RegisterListenerAsync<PricePeriodSummary>
    //             (pricingInstrumentId.CompletePeriodSummaryAddress(), ReceivedCompletePricePeriodSummaries);
    //         foreach (var subPeriod in pricingInstrumentId.EntryPeriod.ConstructingDivisiblePeriods()
    //                                                      .Where(tsp => tsp >= PricePeriodSummaryConstants.PersistPeriodsFrom))
    //         {
    //             historicalSubPeriodRequestSubscriptions.Add
    //                 (subPeriod, await this.RegisterRequestListenerAsync<HistoricalPeriodResponseRequest, List<PricePeriodSummary>>
    //                     (((SourceTickerId)pricingInstrumentId).HistoricalPeriodSummaryResponseRequest(subPeriod)
    //                    , ReceivedHistoricalSubPeriodResponseRequest));
    //             historicalSubPeriodAddressToSubPeriodLookup
    //                 .Add(((SourceTickerId)pricingInstrumentId).HistoricalPeriodSummaryResponseRequest(subPeriod), subPeriod);
    //             liveSubPeriodCompletePublisherSubscriptions.Add
    //                 (subPeriod, await this.RegisterListenerAsync<PricePeriodSummary>
    //                     (((SourceTickerId)pricingInstrumentId).CompletePeriodSummaryAddress(subPeriod), ReceivedCompleteSubPricePeriodSummaries));
    //             liveSubPeriodAddressToSubReceivedHistorical.Add(((SourceTickerId)pricingInstrumentId).CompletePeriodSummaryAddress(subPeriod)
    //                                                           , subPeriod);
    //         }
    //         await base.StartAsync();
    //     }
    //
    //     public void CreateNewWait(int waitForComplete = 1, int waitForLive = 1)
    //     {
    //         waitNumberForCompleted = waitForComplete;
    //         waitNumberForLive      = waitForLive;
    //         awaitCompleteSource    = new TaskCompletionSource<int>();
    //         awaitLiveSource        = new TaskCompletionSource<int>();
    //     }
    //
    //     public async ValueTask<List<IPricePeriodSummary>> GetPopulatedLiveResults(int waitNumber = 1)
    //     {
    //         waitNumberForLive = waitNumber;
    //         if (ReceivedLivePublishEvents.Count < waitNumber) await Task.WhenAny(awaitLiveSource.Task, Task.Delay(2_000));
    //         return ReceivedLivePublishEvents;
    //     }
    //
    //     public async ValueTask<List<IPricePeriodSummary>> GetPopulatedCompleteResults(int waitNumber = 1)
    //     {
    //         waitNumberForCompleted = waitNumber;
    //         if (ReceivedCompletePublishEvents.Count < waitNumber) await Task.WhenAny(awaitCompleteSource.Task, Task.Delay(2_000));
    //         return ReceivedCompletePublishEvents;
    //     }
    //
    //     private void ReceivedLivePricePeriodSummaries(IBusMessage<PricePeriodSummary> livePublishMsg)
    //     {
    //         var pricePeriodSummary = livePublishMsg.Payload.Body();
    //         ReceivedLivePublishEvents.Add(pricePeriodSummary);
    //         if (ReceivedLivePublishEvents.Count >= waitNumberForLive) awaitLiveSource.TrySetResult(0);
    //     }
    //
    //     private void ReceivedCompletePricePeriodSummaries(IBusMessage<PricePeriodSummary> completePublishMsg)
    //     {
    //         var pricePeriodSummary = completePublishMsg.Payload.Body();
    //         ReceivedCompletePublishEvents.Add(pricePeriodSummary);
    //         if (ReceivedCompletePublishEvents.Count >= waitNumberForCompleted) awaitCompleteSource.TrySetResult(0);
    //     }
    //
    //     private async ValueTask<List<PricePeriodSummary>> ReceivedHistoricalSubPeriodResponseRequest
    //         (IBusRespondingMessage<HistoricalPeriodResponseRequest, List<PricePeriodSummary>> completePublishMsg)
    //     {
    //         var historicalSubPeriodRequest = completePublishMsg.Payload.Body();
    //         ReceivedSubPeriodHistoricalRequests.Add(historicalSubPeriodRequest);
    //         var subPeriod = historicalSubPeriodAddressToSubPeriodLookup[completePublishMsg.DestinationAddress!];
    //         if (!historicalSubPeriodResponseCallbacks.TryGetValue(subPeriod, out var resultCallback))
    //             resultCallback = () => new ValueTask<List<PricePeriodSummary>>(new List<PricePeriodSummary>());
    //         return await resultCallback();
    //     }
    //
    //     private void ReceivedCompleteSubPricePeriodSummaries(IBusMessage<PricePeriodSummary> livePublishMsg)
    //     {
    //         var pricePeriodSummary = livePublishMsg.Payload.Body();
    //         ReceivedLivePublishEvents.Add(pricePeriodSummary);
    //         var subPeriod = liveSubPeriodAddressToSubReceivedHistorical[livePublishMsg.DestinationAddress!];
    //         if (!receivedCompleteSubPeriods.TryGetValue(subPeriod, out var resultCallback))
    //         {
    //             resultCallback = new List<PricePeriodSummary>();
    //             receivedCompleteSubPeriods.Add(subPeriod, resultCallback);
    //         }
    //         resultCallback.Add(pricePeriodSummary);
    //     }
    //
    //     public void RegisterSubPeriodResponse(TimeSeriesPeriod subPeriod, Func<ValueTask<List<PricePeriodSummary>>> returnedResults)
    //     {
    //         historicalSubPeriodResponseCallbacks.Add(subPeriod, returnedResults);
    //     }
    //
    //     public async ValueTask SendPricesToLivePeriodRule(List<Level1PriceQuote> publishPrices, IUpdateTime progressTime)
    //     {
    //         await this.RequestAsync<PublishQuotesWithTimeProgress, ValueTask>
    //             (LivePeriodTestClientPublishPricesAddress, new PublishQuotesWithTimeProgress(publishPrices, progressTime));
    //     }
    //
    //     private async ValueTask PublishPriceQuotesHandler
    //         (IBusRespondingMessage<PublishQuotesWithTimeProgress, ValueTask> requestResponseMsg)
    //     {
    //         var toPublishListAndTimeUpdater = requestResponseMsg.Payload.Body();
    //         var toPublishList               = toPublishListAndTimeUpdater.ToPublish;
    //         var timeUpdater                 = toPublishListAndTimeUpdater.TimeUpdater;
    //         foreach (var level1PriceQuote in toPublishList)
    //         {
    //             await timeUpdater.UpdateTime(level1PriceQuote.SourceTime);
    //             await this.PublishAsync(quoteListenAddress, level1PriceQuote);
    //         }
    //     }
    //
    //     public override async ValueTask StopAsync()
    //     {
    //         await livePublishSubscription.NullSafeUnsubscribe();
    //         await listenForPublishPricesSubscription.NullSafeUnsubscribe();
    //         await completePublishSubscription.NullSafeUnsubscribe();
    //         foreach (var listenHistoricalSub in historicalSubPeriodRequestSubscriptions.Values) await listenHistoricalSub.NullSafeUnsubscribe();
    //         foreach (var listenLiveSub in liveSubPeriodCompletePublisherSubscriptions.Values) await listenLiveSub.NullSafeUnsubscribe();
    //         await base.StopAsync();
    //     }
    // }
}
