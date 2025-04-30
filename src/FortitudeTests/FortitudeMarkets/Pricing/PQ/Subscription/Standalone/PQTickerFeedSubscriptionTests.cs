// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeIO.Transports.Network.Config;
using FortitudeMarkets.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarkets.Pricing.PQ.Subscription.Standalone;
using FortitudeMarkets.Pricing.Quotes;
using FortitudeMarkets.Pricing.Quotes.LastTraded;
using FortitudeMarkets.Pricing.Quotes.LayeredBook;
using static FortitudeMarkets.Configuration.ClientServerConfig.MarketClassificationExtensions;
using static FortitudeMarkets.Pricing.Quotes.TickerDetailLevel;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Subscription.Standalone;

[TestClass]
public class PQTickerFeedSubscriptionTests
{
    private IPricingServerConfig          feedConfig               = null!;
    private DummyPQTickerFeedSubscription pqTickerFeedSubscription = null!;
    private ISourceTickerInfo             sourceTickerInfo         = null!;


    [TestInitialize]
    public void SetUp()
    {
        feedConfig = new PricingServerConfig
            (new NetworkTopicConnectionConfig
                 ("testConnectionName", SocketConversationProtocol.TcpClient
                , new[]
                  {
                      new EndpointConfig("testhost", 9090)
                  }, "testConnectionName")
           , new NetworkTopicConnectionConfig
                 ("testConnectionName", SocketConversationProtocol.TcpClient
                , new[]
                  {
                      new EndpointConfig("testhost", 9090)
                  }, "testConnectionName"));

        sourceTickerInfo = new SourceTickerInfo
            (ushort.MaxValue, "TestSource", ushort.MaxValue, "TestTicker", Level3Quote, Unknown
           , 20, 0.00001m, 30000m, 50000000m, 1000m, 1
           , layerFlags: LayerFlags.Volume | LayerFlags.Price | LayerFlags.OrderTraderName | LayerFlags.OrderSize | LayerFlags.OrdersCount
           , lastTradedFlags: LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName |
                              LastTradedFlags.LastTradedVolume | LastTradedFlags.LastTradedTime);
        pqTickerFeedSubscription = new DummyPQTickerFeedSubscription(feedConfig, sourceTickerInfo);
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
        public DummyPQTickerFeedSubscription
        (IPricingServerConfig feedServerConfig,
            ISourceTickerInfo sourceTickerInfo) : base(feedServerConfig, sourceTickerInfo) { }

        public bool UnsubscribeHasBeenCalled { get; set; }

        public override void Unsubscribe()
        {
            UnsubscribeHasBeenCalled = true;
        }
    }
}
