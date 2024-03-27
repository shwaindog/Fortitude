#region

using FortitudeIO.Transports.NewSocketAPI.Config;
using FortitudeMarketsApi.Configuration.ClientServerConfig;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.LastTraded;
using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsApi.Pricing.Quotes.SourceTickerInfo;
using FortitudeMarketsCore.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsCore.Pricing.PQ.Subscription;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription;

[TestClass]
public class PQTickerFeedSubscriptionTests
{
    private ISnapshotUpdatePricingServerConfig feedConfig = null!;
    private DummyPQTickerFeedSubscription pqTickerFeedSubscription = null!;
    private ISourceTickerQuoteInfo sourceTickerQuoteInfo = null!;


    [TestInitialize]
    public void SetUp()
    {
        feedConfig = new SnapshotUpdatePricingServerConfig("TestServerConfig", MarketServerType.MarketData,
            new[]
            {
                new NetworkTopicConnectionConfig("testConnectionName", SocketConversationProtocol.TcpClient, new[]
                {
                    new EndpointConfig("testhost", 9090)
                }, "testConnectionName")
            },
            null, 1234,
            new[] { new SourceTickerPublicationConfig(0, "", "") },
            true, true);

        sourceTickerQuoteInfo = new SourceTickerClientAndPublicationConfig(
            uint.MaxValue, "TestSource", "TestTicker", 20, 0.00001m, 30000m, 50000000m, 1000m, 1,
            LayerFlags.Volume | LayerFlags.Price | LayerFlags.TraderName | LayerFlags.TraderSize
            | LayerFlags.TraderCount, LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName
                                                                  | LastTradedFlags.LastTradedVolume |
                                                                  LastTradedFlags.LastTradedTime, null,
            3000);
        pqTickerFeedSubscription = new DummyPQTickerFeedSubscription(feedConfig, sourceTickerQuoteInfo);
    }

    [TestMethod]
    public void NewTickerFeedSubscription_Properties_InitializedAsExpected()
    {
        Assert.AreEqual("TestSource", pqTickerFeedSubscription.Source);
        Assert.AreEqual("TestTicker", pqTickerFeedSubscription.Ticker);
        Assert.AreSame(feedConfig, pqTickerFeedSubscription.Feed);

        Assert.AreNotEqual(0, pqTickerFeedSubscription.GetHashCode());
    }


    public class DummyPQTickerFeedSubscription : PQTickerFeedSubscription
    {
        public DummyPQTickerFeedSubscription(ISnapshotUpdatePricingServerConfig feedServerConfig,
            ISourceTickerQuoteInfo sourceTickerQuoteInfo) : base(feedServerConfig, sourceTickerQuoteInfo) { }

        public bool UnsubscribeHasBeenCalled { get; set; }

        public override void Unsubscribe()
        {
            UnsubscribeHasBeenCalled = true;
        }
    }
}
