// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.Messages;
using FortitudeCommon.Monitoring.Logging;
using FortitudeIO.Transports.Network.Construction;
using FortitudeMarkets.Configuration.ClientServerConfig;
using FortitudeMarkets.Pricing.PQ.Publication.BusRules;
using FortitudeMarkets.Pricing.PQ.Publication.BusRules.BusMessages;
using FortitudeMarkets.Pricing.PQ.Subscription.BusRules;
using FortitudeMarkets.Pricing.Quotes.LastTraded;
using FortitudeMarkets.Pricing.Quotes.LayeredBook;
using FortitudeTests.FortitudeBusRules.BusMessaging;
using FortitudeTests.FortitudeMarkets.Pricing.PQ.Subscription.BusRules;
using FortitudeTests.FortitudeMarkets.Pricing.Quotes;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Publication.BusRules;

[TestClass]
public class PQPricingServerFeedRuleTests : OneOfEachMessageQueueTypeTestSetup
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(PQPricingServerFeedRuleTests));

    private IMarketConnectionConfig    clientMarketConfig              = null!;
    private string                     feedName                        = null!;
    private ManualResetEvent           haveReceivedPriceAutoResetEvent = null!;
    private PQPricingClientFeedRule    pqPricingClientFeedRule         = null!;
    private PQPricingServerFeedRule    pricingServerFeedRule           = null!;
    private PublishQuoteEvent          publishQuoteEvent               = null!;
    private LocalHostPQTestSetupCommon serverConfig                    = null!;
    private TestSubscribeToTickerRule  testSubscribeToTickerRule       = null!;


    [TestInitialize]
    public void Setup()
    {
        // otherwise other tests using PQPricingClientFeedRule will break
        PQPricingClientSnapshotConversationRequester.SocketFactories = SocketFactoryResolver.GetRealSocketFactories();
        PQPricingClientUpdatesConversationSubscriber.SocketFactories = SocketFactoryResolver.GetRealSocketFactories();

        serverConfig = new LocalHostPQTestSetupCommon
        {
            LayerDetails = LayerFlags.Price | LayerFlags.Volume | LayerFlags.TraderName | LayerFlags.TraderSize
          , LastTradedFlags = LastTradedFlags.TraderName | LastTradedFlags.LastTradedPrice | LastTradedFlags.PaidOrGiven |
                              LastTradedFlags.LastTradedTime
        };
        serverConfig.InitializeCommonConfig();
        haveReceivedPriceAutoResetEvent = new ManualResetEvent(false);

        feedName = serverConfig.DefaultServerMarketConnectionConfig.SourceName;

        var serverConfigMarketConnectionConfig = serverConfig.DefaultServerMarketConnectionConfig.ShiftPortsBy(8);
        ;
        pricingServerFeedRule         = new PQPricingServerFeedRule(serverConfigMarketConnectionConfig);
        clientMarketConfig            = serverConfigMarketConnectionConfig.ToggleProtocolDirection("PQPricingServerFeedRuleTests");
        clientMarketConfig.SourceName = "PQClientSourceFeedRuleTests";
        pqPricingClientFeedRule       = new PQPricingClientFeedRule(clientMarketConfig);
        testSubscribeToTickerRule     = new TestSubscribeToTickerRule(clientMarketConfig.SourceName, "EUR/USD", haveReceivedPriceAutoResetEvent);
        publishQuoteEvent = new PublishQuoteEvent
        {
            AutoRecycleAtRefCountZero = false
        };
    }

    [TestCleanup]
    public void TearDown()
    {
        TearDownMessageBus();
        // FLoggerFactory.GracefullyTerminateProcessLogging();
        // SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
    }


    [TestMethod]
    [Timeout(20_000)]
    public async Task StartPQPricingServerFeedRule_PublishPrices_ClientReceivesPrices()
    {
        await using var serverDeploy = await EventQueue1.LaunchRuleAsync(pricingServerFeedRule, pricingServerFeedRule, EventQueue1SelectionResult);
        Logger.Info("Deployed pricing server");
        publishQuoteEvent.PublishQuote = Level3PriceQuoteTests.GenerateL3QuoteWithTraderLayerAndLastTrade(serverConfig.FirstTickerInfo);
        await MessageBus.PublishAsync(pricingServerFeedRule, feedName.FeedTickerPublishAddress(), publishQuoteEvent, new DispatchOptions());

        await using var clientDeploy
            = await EventQueue1.LaunchRuleAsync(pqPricingClientFeedRule, pqPricingClientFeedRule, EventQueue1SelectionResult);
        Logger.Info("Deployed pricing client");

        await Task.Delay(50);

        await using var testSubscribe
            = await CustomQueue1.LaunchRuleAsync(testSubscribeToTickerRule, testSubscribeToTickerRule, CustomQueue1SelectionResult);
        Logger.Info("Deployed client listening rule");
        await Task.Delay(1); // NEED this to allow tasks above to dispatch any callbacks

        var  counter = 0;
        bool receivedSnapshotTick;
        do
        {
            receivedSnapshotTick = haveReceivedPriceAutoResetEvent.WaitOne(50);
            await Task.Delay(10);
        } while (receivedSnapshotTick == false && counter++ < 300);

        Assert.IsTrue(receivedSnapshotTick, "Did not receive snapshot response tick from the client before timeout was reached");
        haveReceivedPriceAutoResetEvent.Reset();
        Logger.Info("Received snapshot await update");
        publishQuoteEvent.PublishQuote = Level3PriceQuoteTests.GenerateL3QuoteWithTraderLayerAndLastTrade(serverConfig.FirstTickerInfo, 2);
        await MessageBus.PublishAsync(pricingServerFeedRule, feedName.FeedTickerPublishAddress(), publishQuoteEvent, new DispatchOptions());
        counter = 0;
        bool receivedUpdateTick;
        do
        {
            receivedUpdateTick = haveReceivedPriceAutoResetEvent.WaitOne(50);
            await Task.Delay(10);
        } while (receivedSnapshotTick == false && counter++ < 300);

        Logger.Info("Received update ");
        Assert.IsTrue(receivedUpdateTick, "Did not receive update tick from the client before timeout was reached");
    }
}
