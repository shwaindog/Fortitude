// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.BusMessaging.Messages.ListeningSubscriptions;
using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;
using FortitudeCommon.Monitoring.Logging;
using FortitudeIO.Transports.Network.Construction;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.PQ.Publication;
using FortitudeMarkets.Pricing.PQ.Subscription.BusRules;
using FortitudeTests.FortitudeBusRules.BusMessaging;
using FortitudeTests.FortitudeMarkets.Pricing.FeedEvents.Quotes;
using FortitudeTests.FortitudeMarkets.Pricing.PQ.Publication;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Subscription.BusRules;

[TestClass]
public class PQPricingClientFeedRuleTests : OneOfEachMessageQueueTypeTestSetup
{
    private readonly IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(PQPricingClientFeedRuleTests));

    private ManualResetEvent haveReceivedPriceAutoResetEvent = null!;

    private PQPricingClientFeedRule pqPricingClientFeedRule = null!;

    private PQPublisher<PQPublishableLevel3Quote> pqPublisher = null!;

    private LocalHostPQServerLevel3QuoteTestSetup pqServerL3QuoteServerSetup = null!;

    private TestSubscribeToTickerRule testSubscribeToTickerRule = null!;

    [TestInitialize]
    public void Setup()
    {
        // otherwise other tests using PQPricingClientFeedRule will break
        PQPricingClientSnapshotConversationRequester.SocketFactories = SocketFactoryResolver.GetRealSocketFactories();
        PQPricingClientUpdatesConversationSubscriber.SocketFactories = SocketFactoryResolver.GetRealSocketFactories();

        pqServerL3QuoteServerSetup = new LocalHostPQServerLevel3QuoteTestSetup();
        pqPublisher                = pqServerL3QuoteServerSetup.CreatePQPublisher();
        var clientMarketConfig
            = pqServerL3QuoteServerSetup.DefaultServerMarketConnectionConfig.ToggleProtocolDirection("PQClientSourceFeedRuleTestsClient");
        clientMarketConfig.SourceName   = "PQClientSourceFeedRuleTests";
        pqPricingClientFeedRule         = new PQPricingClientFeedRule(clientMarketConfig);
        haveReceivedPriceAutoResetEvent = new ManualResetEvent(false);
        testSubscribeToTickerRule       = new TestSubscribeToTickerRule(clientMarketConfig.SourceName, "EUR/USD", haveReceivedPriceAutoResetEvent);
    }

    [TestCleanup]
    public void TearDown()
    {
        logger.Info("Test complete starting services shutdown");
        pqServerL3QuoteServerSetup.TearDown();
        TearDownMessageBus();
        // FLoggerFactory.GracefullyTerminateProcessLogging();
        // SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
    }

    [TestMethod]
    [Timeout(20_000)]
    public async Task StartedPQServer_DeployPQClientFeedRule_ClientSubscribingToDefaultAddressReceivesPrice()
    {
        await using var clientDeploy
            = await EventQueue1.LaunchRuleAsync(pqPricingClientFeedRule, pqPricingClientFeedRule, EventQueue1SelectionResult);
        logger.Info("Deployed pricing client");
        await using var testSubscribeDeploy
            = await CustomQueue1.LaunchRuleAsync(pqPricingClientFeedRule, testSubscribeToTickerRule, CustomQueue1SelectionResult);
        logger.Info("Deployed client listening rule");
        await Task.Delay(1); // NEED this to allow tasks above to dispatch any callbacks
        var receivedSnapshotTick = haveReceivedPriceAutoResetEvent.WaitOne(8_000);
        Assert.IsTrue(receivedSnapshotTick, "Did not receive snapshot response tick from the client before timeout was reached");
        haveReceivedPriceAutoResetEvent.Reset();
        logger.Info("Received snapshot await update");
        var sourcePriceQuote = Level3PriceQuoteTests.GenerateL3QuoteWithTraderLayerAndLastTrade(pqServerL3QuoteServerSetup.FirstTickerInfo, 3);
        pqPublisher.PublishQuoteUpdate(sourcePriceQuote);
        var receivedUpdateTick = haveReceivedPriceAutoResetEvent.WaitOne(8_000);
        logger.Info("Received update ");
        Assert.IsTrue(receivedUpdateTick, "Did not receive update tick from the client before timeout was reached");
    }
}

public class TestSubscribeToTickerRule : Rule
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(TestSubscribeToTickerRule));

    private readonly string feedName;
    private readonly string feedTickerListenAddress;

    private readonly ManualResetEvent haveReceivedTick;

    private readonly string tickerToSubscribeTo;

    private ISubscription? tickerListenSubscription;

    public TestSubscribeToTickerRule(string feedName, string tickerToSubscribeTo, ManualResetEvent haveReceivedTick) : base(
     "TestSubscribeToTickerRule_" + feedName + "_" +
     tickerToSubscribeTo)
    {
        this.feedName            = feedName;
        this.tickerToSubscribeTo = tickerToSubscribeTo;
        this.haveReceivedTick    = haveReceivedTick;
        feedTickerListenAddress  = $"Markets.Pricing.Subscription.Feed.{feedName}.Ticker.{tickerToSubscribeTo}";
    }

    public override async ValueTask StartAsync()
    {
        tickerListenSubscription = await Context.MessageBus.RegisterListenerAsync<PQPublishableLevel3Quote>
            (this, feedTickerListenAddress, Handler);
        Logger.Info("Rule {0} has subscribed to address {1} on Thread Name {2}", FriendlyName, feedTickerListenAddress, Thread.CurrentThread.Name);
    }

    public override async ValueTask StopAsync()
    {
        await tickerListenSubscription.NullSafeUnsubscribe();
    }

    private void Handler(IBusMessage<PQPublishableLevel3Quote> priceQuote)
    {
        var pqL3Quote = priceQuote.Payload.Body();
        Logger.Info("Rule {0} listening on {1} received {2} with Sequence Number {3} on Thread Name {4}", FriendlyName, feedTickerListenAddress
                  , pqL3Quote.GetType().Name, pqL3Quote.PQSequenceId, Thread.CurrentThread.Name);
        haveReceivedTick.Set();
    }
}
