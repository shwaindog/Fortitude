// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeIO.Transports.Network.Config;
using FortitudeMarkets.Configuration;
using FortitudeMarkets.Configuration.PricingConfig;
using FortitudeMarkets.Pricing.FeedEvents.LastTraded;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.PQ.Serdes.Deserialization;
using static FortitudeIO.Transports.Network.Config.CountryCityCodes;
using static FortitudeMarkets.Configuration.MarketClassificationExtensions;
using static FortitudeMarkets.Pricing.FeedEvents.TickerInfo.TickerQuoteDetailLevel;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Serdes.Deserialization;

[TestClass]
public class PQClientQuoteDeserializerRepositoryTests
{
    private const ushort ExpectedSourceId = ushort.MaxValue;
    private const ushort ExpectedTickerd  = ushort.MaxValue;
    private const uint   ExpectedStreamId = ((uint)ExpectedSourceId << 16) | ExpectedTickerd;

    private PQClientQuoteDeserializerRepository pqClientQuoteDeserializerRepository   = null!;
    private ISourceTickerInfo                   sourceTickerInfo                      = null!;
    private ITickerPricingSubscriptionConfig    sourceTickerPricingSubscriptionConfig = null!;

    [TestInitialize]
    public void SetUp()
    {
        sourceTickerInfo =
            new SourceTickerInfo
                (ExpectedSourceId, "TestSource", ExpectedTickerd, "TestTicker", Level3Quote, MarketClassification.Unknown
               , AUinMEL, AUinMEL, AUinMEL
               , 20, 0.00001m, 30000m, 50000000m, 1000m, 1
               , layerFlags: LayerFlags.Volume | LayerFlags.Price
               , lastTradedFlags: LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName | LastTradedFlags.LastTradedVolume |
                                  LastTradedFlags.LastTradedTime);
        sourceTickerPricingSubscriptionConfig =
            new TickerPricingSubscriptionConfig
                (sourceTickerInfo
               , new PricingServerConfig
                     (new NetworkTopicConnectionConfig
                          ("ConnectionName", SocketConversationProtocol.TcpClient
                         , new[]
                           {
                               new EndpointConfig("ConnectionName", 9090, AUinMEL)
                           }),
                      new NetworkTopicConnectionConfig
                          ("ConnectionName", SocketConversationProtocol.TcpClient
                         , new[]
                           {
                               new EndpointConfig("ConnectionName", 9090, AUinMEL)
                           })));

        pqClientQuoteDeserializerRepository = new PQClientQuoteDeserializerRepository("PQClientTest", new Recycler());
    }

    [TestMethod]
    public void NewSerializerFactory_CreateQuoteDeserializer_CreatesNewPQQuoteDeserializer()
    {
        var quoteDeserializer = pqClientQuoteDeserializerRepository
            .CreateQuoteDeserializer<PQPublishableTickInstant>(sourceTickerPricingSubscriptionConfig);
        Assert.IsNotNull(quoteDeserializer);
        Assert.AreEqual(quoteDeserializer.Identifier, sourceTickerInfo);
        pqClientQuoteDeserializerRepository = new PQClientQuoteDeserializerRepository("PQClientTest1", new Recycler());
        var level1Deserializer = pqClientQuoteDeserializerRepository
            .CreateQuoteDeserializer<PQPublishableLevel1Quote>(sourceTickerPricingSubscriptionConfig);
        Assert.IsNotNull(level1Deserializer);
        Assert.AreEqual(level1Deserializer.Identifier, sourceTickerInfo);
        pqClientQuoteDeserializerRepository = new PQClientQuoteDeserializerRepository("PQClientTest2", new Recycler());
        var level2Deserializer = pqClientQuoteDeserializerRepository
            .CreateQuoteDeserializer<PQPublishableLevel2Quote>(sourceTickerPricingSubscriptionConfig);
        Assert.IsNotNull(level2Deserializer);
        Assert.AreEqual(level2Deserializer.Identifier, sourceTickerInfo);
        pqClientQuoteDeserializerRepository = new PQClientQuoteDeserializerRepository("PQClientTest3", new Recycler());
        var level3Deserializer = pqClientQuoteDeserializerRepository
            .CreateQuoteDeserializer<PQPublishableLevel3Quote>(sourceTickerPricingSubscriptionConfig);
        Assert.IsNotNull(level3Deserializer);
        Assert.AreEqual(level3Deserializer.Identifier, sourceTickerInfo);
    }

    [TestMethod]
    public void CreateDeserializer_GetQuoteDeserializer_ReturnsPreviouslyCreatedDeserializer()
    {
        var quoteDeserializer = pqClientQuoteDeserializerRepository
            .CreateQuoteDeserializer<PQPublishableTickInstant>(sourceTickerPricingSubscriptionConfig);
        Assert.IsNotNull(quoteDeserializer);

        var requestedDeserializer = pqClientQuoteDeserializerRepository.GetDeserializer(sourceTickerInfo);

        Assert.AreSame(quoteDeserializer, requestedDeserializer);
    }

    [TestMethod]
    public void NoEnteredDeserializer_GetQuoteDeserializer_ReturnsNullDeserializer()
    {
        Assert.IsNull(pqClientQuoteDeserializerRepository.GetDeserializer(sourceTickerInfo));
    }

    [TestMethod]
    public void CreateDeserializer_RemoveQuoteDeserializer_RemovesDeserializerAndGetDeserializerReturnsNull()
    {
        var quoteDeserializer = pqClientQuoteDeserializerRepository
            .CreateQuoteDeserializer<PQPublishableTickInstant>(sourceTickerPricingSubscriptionConfig);
        Assert.IsNotNull(quoteDeserializer);

        pqClientQuoteDeserializerRepository.UnregisterDeserializer(sourceTickerInfo);

        Assert.IsNull(pqClientQuoteDeserializerRepository.GetDeserializer(sourceTickerInfo));
    }

    [TestMethod]
    public void CreateDeserializer_GetDeserializer_ReturnsDeserializerThatMatchesId()
    {
        var quoteDeserializer = pqClientQuoteDeserializerRepository
            .CreateQuoteDeserializer<PQPublishableTickInstant>(sourceTickerPricingSubscriptionConfig);
        Assert.IsNotNull(quoteDeserializer);

        var getQuoteDeserializer = pqClientQuoteDeserializerRepository
            .GetDeserializer<PQPublishableTickInstant>(sourceTickerInfo.SourceInstrumentId);
        Assert.IsNotNull(getQuoteDeserializer);

        Assert.AreSame(getQuoteDeserializer, quoteDeserializer);
    }

    [TestMethod]
    public void NoEnteredDeserializer_GetDeserializer_ReturnsNull()
    {
        var result = pqClientQuoteDeserializerRepository.GetDeserializer<PQPublishableTickInstant>(sourceTickerInfo.SourceInstrumentId);
        Assert.IsNull(result);
    }
}
