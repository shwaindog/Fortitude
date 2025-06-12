// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeIO.Transports.Network.Config;
using FortitudeMarkets.Configuration;
using FortitudeMarkets.Configuration.PricingConfig;
using FortitudeMarkets.Pricing.FeedEvents.LastTraded;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Subscription.Standalone;
using static FortitudeIO.Transports.Network.Config.CountryCityCodes;
using static FortitudeMarkets.Pricing.FeedEvents.TickerInfo.TickerQuoteDetailLevel;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Subscription.Standalone;

[TestClass]
public class PQTickerFeedSubscriptionTests
{
    private DummyPQTickerFeedSubscription pqTickerFeedSubscription = null!;

    private IPricingServerConfig feedConfig       = null!;
    private ISourceTickerInfo    sourceTickerInfo = null!;


    [TestInitialize]
    public void SetUp()
    {
        feedConfig = new PricingServerConfig
            (new NetworkTopicConnectionConfig
                 ("testConnectionName", SocketConversationProtocol.TcpClient
                , [new EndpointConfig("testHost", 9090, AUinMEL)]
                , "testConnectionName")
           , new NetworkTopicConnectionConfig
                 ("testConnectionName", SocketConversationProtocol.TcpClient
                , [new EndpointConfig("testHost", 9090, AUinMEL)]
                , "testConnectionName"));

        sourceTickerInfo = new SourceTickerInfo
            (ushort.MaxValue, "TestSource", ushort.MaxValue, "TestTicker", Level3Quote, MarketClassification.Unknown
           , AUinMEL, AUinMEL, AUinMEL
           , 20, 0.00001m, 30000m, 50000000m, 1000m
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


    public class DummyPQTickerFeedSubscription(IPricingServerConfig feedServerConfig, ISourceTickerInfo sourceTickerInfo)
        : PQTickerFeedSubscription(feedServerConfig, sourceTickerInfo)
    {
        public bool UnsubscribeHasBeenCalled { get; set; }

        public override void Unsubscribe()
        {
            UnsubscribeHasBeenCalled = true;
        }
    }
}
