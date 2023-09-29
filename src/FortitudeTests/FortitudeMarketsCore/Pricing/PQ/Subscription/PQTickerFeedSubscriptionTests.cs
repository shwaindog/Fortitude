using FortitudeIO.Sockets;
using FortitudeIO.Transports.Sockets;
using FortitudeMarketsApi.Configuration.ClientServerConfig;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.LastTraded;
using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsApi.Pricing.Quotes.SourceTickerInfo;
using FortitudeMarketsCore.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsCore.Pricing.PQ.Subscription;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription
{
    [TestClass]
    public class PQTickerFeedSubscriptionTests
    {
        private ISnapshotUpdatePricingServerConfig feedConfig;
        private ISourceTickerQuoteInfo sourceTickerQuoteInfo;
        private DummyPQTickerFeedSubscription pqTickerFeedSubscription;


        [TestInitialize]
        public void SetUp()
        {
            feedConfig = new SnapshotUpdatePricingServerConfig("TestServerConfig", MarketServerType.MarketData, 
                new []{new ConnectionConfig("testConnectionName", "testhost", 9090, 
                ConnectionDirectionType.Both, null, 4000, null), }, null, 1234, null, true, true);

            sourceTickerQuoteInfo = new SourceTickerClientAndPublicationConfig(
                uint.MaxValue, "TestSource", "TestTicker", 20, 0.00001m, 30000m, 50000000m, 1000m, 1, 
                LayerFlags.Volume | LayerFlags.Price | LayerFlags.TraderName | LayerFlags.TraderSize
                | LayerFlags.TraderCount, LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName
                                          | LastTradedFlags.LastTradedVolume | LastTradedFlags.LastTradedTime, null,
                3000, true);
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
                ISourceTickerQuoteInfo sourceTickerQuoteInfo) : base(feedServerConfig, sourceTickerQuoteInfo)
            {
            }

            public bool UnsubscribeHasBeenCalled { get; set; }

            public override void Unsubscribe()
            {
                UnsubscribeHasBeenCalled = true;
            }
        }
    }
}