#region

using FortitudeBusRules.BusMessaging.Messages.ListeningSubscriptions;
using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;
using FortitudeCommon.Monitoring.Logging;
using FortitudeMarketsCore.Pricing.PQ.Converters;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Publication;
using FortitudeMarketsCore.Pricing.PQ.Subscription.BusRules;
using FortitudeTests.FortitudeBusRules.BusMessaging;
using FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Publication;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription.BusRules;

[TestClass]
public class PQPricingClientFeedRuleTests : OneOfEachMessageQueueTypeTestSetup
{
    private readonly IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(PQPricingClientFeedRuleTests));
    private PQPricingClientFeedRule pqPricingClientFeedRule = null!;
    private PQPublisher<PQLevel3Quote> pqPublisher = null!;
    private LocalHostPQServerLevel3QuoteTestSetup pqServerL3QuoteServerSetup = null!;
    private TestSubscribeToTickerRule testSubscribeToTickerRule = null!;

    [TestInitialize]
    public void Setup()
    {
        pqServerL3QuoteServerSetup = new LocalHostPQServerLevel3QuoteTestSetup();
        pqPublisher = pqServerL3QuoteServerSetup.CreatePQPublisher();
        var clientMarketConfig
            = pqServerL3QuoteServerSetup.DefaultServerMarketConnectionConfig.ToggleProtocolDirection("PQClientSourceFeedRuleTestsClient");
        clientMarketConfig.Name = "PQClientSourceFeedRuleTests";
        pqPricingClientFeedRule = new PQPricingClientFeedRule(clientMarketConfig);
        testSubscribeToTickerRule = new TestSubscribeToTickerRule(clientMarketConfig.Name, "EUR/USD");
    }

    [TestCleanup]
    public void TearDown()
    {
        logger.Info("Test complete starting services shutdown");
        pqServerL3QuoteServerSetup.TearDown();
        TearDownMessageBus();
        FLoggerFactory.GracefullyTerminateProcessLogging();
        SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
    }

    [TestMethod]
    [Timeout(10_000)]
    public async Task StartedPQServer_DeployPQClientFeedRule_ClientSubscribingToDefaultAddressReceivesPrice()
    {
        await EventQueue1.LaunchRuleAsync(pqPricingClientFeedRule, pqPricingClientFeedRule, EventQueue1SelectionResult);
        logger.Info("Deployed pricing client");
        await CustomQueue1.LaunchRuleAsync(pqPricingClientFeedRule, testSubscribeToTickerRule, CustomQueue1SelectionResult);
        logger.Info("Deployed client listening rule");
        var sourcePriceQuote = pqServerL3QuoteServerSetup.GenerateL3QuoteWithTraderLayerAndLastTrade(1);
        var pqLevel3Quote = sourcePriceQuote.ToL3PQQuote();
        pqLevel3Quote.OverrideSerializationFlags = PQMessageFlags.Snapshot;
        pqPublisher.PublishQuoteUpdate(pqLevel3Quote);
        await EventQueue1.StopRuleAsync(pqPricingClientFeedRule, testSubscribeToTickerRule);
        await EventQueue1.StopRuleAsync(pqPricingClientFeedRule, pqPricingClientFeedRule);
    }
}

public class TestSubscribeToTickerRule : Rule
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(TestSubscribeToTickerRule));
    private readonly string feedName;
    private readonly string feedTickerListenAddress;
    private readonly string tickerToSubscribeTo;

    private ISubscription tickerListenSubscription = null!;

    public TestSubscribeToTickerRule(string feedName, string tickerToSubscribeTo) : base("TestSubscribeToTickerRule_" + feedName + "_" +
                                                                                         tickerToSubscribeTo)
    {
        this.feedName = feedName;
        this.tickerToSubscribeTo = tickerToSubscribeTo;
        feedTickerListenAddress = $"Markets.Pricing.Subscription.Feed.{feedName}.Ticker.{tickerToSubscribeTo}";
    }

    public override async ValueTask StartAsync()
    {
        tickerListenSubscription = await Context.MessageBus.RegisterListenerAsync<PQLevel3Quote>(this
            , feedTickerListenAddress, Handler);
        Logger.Info("Rule {0} has subscribed to address {1}", FriendlyName, feedTickerListenAddress);
    }

    public override async ValueTask StopAsync()
    {
        await tickerListenSubscription.UnsubscribeAsync();
    }

    private void Handler(IBusMessage<PQLevel3Quote> priceQuote)
    {
        var pqL3Quote = priceQuote.Payload.Body()!;
        Logger.Info("Rule {0} listening on {1} received {2} with Sequence Number {3}", FriendlyName, feedTickerListenAddress
            , pqL3Quote.GetType().Name, pqL3Quote.PQSequenceId);
    }
}
