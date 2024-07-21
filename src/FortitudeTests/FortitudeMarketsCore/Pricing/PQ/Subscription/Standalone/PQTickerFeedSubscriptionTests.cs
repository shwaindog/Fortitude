// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeIO.Transports.Network.Config;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsApi.Pricing.Quotes.LastTraded;
using FortitudeMarketsApi.Pricing.Quotes.LayeredBook;
using FortitudeMarketsCore.Pricing.PQ.Subscription.Standalone;
using static FortitudeMarketsApi.Configuration.ClientServerConfig.MarketClassificationExtensions;
using static FortitudeMarketsApi.Pricing.Quotes.TickerDetailLevel;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription.Standalone;

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
           , layerFlags: LayerFlags.Volume | LayerFlags.Price | LayerFlags.TraderName | LayerFlags.TraderSize | LayerFlags.TraderCount
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
