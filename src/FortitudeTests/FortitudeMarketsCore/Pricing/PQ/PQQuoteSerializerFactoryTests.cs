﻿#region

using FortitudeIO.Transports.Sockets;
using FortitudeMarketsApi.Configuration.ClientServerConfig;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.LastTraded;
using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsCore.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsCore.Pricing.PQ;
using FortitudeMarketsCore.Pricing.PQ.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Serialization;
using FortitudeMarketsCore.Pricing.PQ.Subscription;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ;

[TestClass]
public class PQQuoteSerializerFactoryTests
{
    private const uint ExpectedStreamId = uint.MaxValue;
    private PQQuoteSerializerFactory pqQuoteSerializerFactory = null!;
    private ISourceTickerClientAndPublicationConfig sourceTickerClientAndPublicationConfig = null!;

    [TestInitialize]
    public void SetUp()
    {
        sourceTickerClientAndPublicationConfig = new SourceTickerClientAndPublicationConfig(
            new SourceTickerPublicationConfig(ExpectedStreamId, "TestSource", "TestTicker", 20,
                0.00001m, 30000m, 50000000m, 1000m, 1, LayerFlags.Volume | LayerFlags.Price,
                LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName |
                LastTradedFlags.LastTradedVolume | LastTradedFlags.LastTradedTime,
                new SnapshotUpdatePricingServerConfig("SnapshortServerName", MarketServerType.MarketData,
                    new[]
                    {
                        new ConnectionConfig("ConnectionName", "connectionHostName", 9090,
                            ConnectionDirectionType.Both, "127.0.0.0", 4000)
                    }, null, 0,
                    new List<ISourceTickerPublicationConfig>(), true, true)));

        pqQuoteSerializerFactory = new PQQuoteSerializerFactory();
    }

    [TestMethod]
    public void NewSerializerFactory_CreateQuoteDeserializer_CreatesNewPQQuoteDeserializer()
    {
        var level0Deserializer = pqQuoteSerializerFactory
            .CreateQuoteDeserializer<PQLevel0Quote>(sourceTickerClientAndPublicationConfig);
        Assert.IsNotNull(level0Deserializer);
        Assert.AreEqual(level0Deserializer.Identifier, sourceTickerClientAndPublicationConfig);
        pqQuoteSerializerFactory = new PQQuoteSerializerFactory();
        var level1Deserializer = pqQuoteSerializerFactory
            .CreateQuoteDeserializer<PQLevel1Quote>(sourceTickerClientAndPublicationConfig);
        Assert.IsNotNull(level1Deserializer);
        Assert.AreEqual(level1Deserializer.Identifier, sourceTickerClientAndPublicationConfig);
        pqQuoteSerializerFactory = new PQQuoteSerializerFactory();
        var level2Deserializer = pqQuoteSerializerFactory
            .CreateQuoteDeserializer<PQLevel2Quote>(sourceTickerClientAndPublicationConfig);
        Assert.IsNotNull(level2Deserializer);
        Assert.AreEqual(level2Deserializer.Identifier, sourceTickerClientAndPublicationConfig);
        pqQuoteSerializerFactory = new PQQuoteSerializerFactory();
        var level3Deserializer = pqQuoteSerializerFactory
            .CreateQuoteDeserializer<PQLevel3Quote>(sourceTickerClientAndPublicationConfig);
        Assert.IsNotNull(level3Deserializer);
        Assert.AreEqual(level3Deserializer.Identifier, sourceTickerClientAndPublicationConfig);
    }

    [TestMethod]
    public void CreateDeserializer_GetQuoteDeserializer_ReturnsPreviouslyCreatedDeserializer()
    {
        var level0Deserializer = pqQuoteSerializerFactory
            .CreateQuoteDeserializer<PQLevel0Quote>(sourceTickerClientAndPublicationConfig);
        Assert.IsNotNull(level0Deserializer);

        var requestedDeserializer = pqQuoteSerializerFactory
            .GetQuoteDeserializer(sourceTickerClientAndPublicationConfig);

        Assert.AreSame(level0Deserializer, requestedDeserializer);
    }

    [TestMethod]
    public void NoEnteredDeserializer_GetQuoteDeserializer_ReturnsNullDeserializer()
    {
        Assert.IsNull(pqQuoteSerializerFactory.GetQuoteDeserializer(sourceTickerClientAndPublicationConfig));
    }

    [TestMethod]
    public void CreateDeserializer_RemoveQuoteDeserializer_RemovesDeserializerAndGetDeserializerReturnsNull()
    {
        var level0Deserializer = pqQuoteSerializerFactory
            .CreateQuoteDeserializer<PQLevel0Quote>(sourceTickerClientAndPublicationConfig);
        Assert.IsNotNull(level0Deserializer);

        pqQuoteSerializerFactory.RemoveQuoteDeserializer(sourceTickerClientAndPublicationConfig);

        Assert.IsNull(pqQuoteSerializerFactory.GetQuoteDeserializer(sourceTickerClientAndPublicationConfig));
    }

    [TestMethod]
    public void CreateDeserializer_GetDeserializer_ReturnsDeserializerThatMatchesId()
    {
        var level0Deserializer = pqQuoteSerializerFactory
            .CreateQuoteDeserializer<PQLevel0Quote>(sourceTickerClientAndPublicationConfig);
        Assert.IsNotNull(level0Deserializer);

        var getLevel0Deserializer = pqQuoteSerializerFactory
            .GetDeserializer<PQLevel0Quote>(sourceTickerClientAndPublicationConfig.Id);
        Assert.IsNotNull(getLevel0Deserializer);

        Assert.AreSame(getLevel0Deserializer, level0Deserializer);
    }

    [TestMethod]
    [ExpectedException(typeof(NotSupportedException))]
    public void NoEnteredDeserializer_GetDeserializer_ReturnsDeserializerThatMatchesId()
    {
        pqQuoteSerializerFactory.GetDeserializer<PQLevel0Quote>(sourceTickerClientAndPublicationConfig.Id);
    }

    [TestMethod]
    public void EmptyQuoteSerializationFactory_GetSerializerUintArray_ReturnsPQRequestSerializer()
    {
        var uintArraySerializer = pqQuoteSerializerFactory.GetSerializer<PQSnapshotClient.SnapShotStreamPublisher>(0);
        Assert.IsNotNull(uintArraySerializer);
        Assert.IsInstanceOfType(uintArraySerializer, typeof(PQSnapshotIdsRequestSerializer));
    }

    [TestMethod]
    [ExpectedException(typeof(NotSupportedException))]
    public void NoEnteredDeserializer_GetSerializerNonSupportedType_ReturnsDeserializerThatMatchesId()
    {
        pqQuoteSerializerFactory.GetSerializer<PQLevel0Quote>(sourceTickerClientAndPublicationConfig.Id);
    }
}
