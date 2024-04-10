#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeIO.Transports.Network.Config;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.LastTraded;
using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsCore.Pricing.PQ;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Serdes.Deserialization;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serdes.Deserialization;

[TestClass]
public class PQClientQuoteDeserializerRepositoryTests
{
    private const ushort ExpectedSourceId = ushort.MaxValue;
    private const ushort ExpectedTickerd = ushort.MaxValue;
    private const uint ExpectedStreamId = ((uint)ExpectedSourceId << 16) | ExpectedTickerd;
    private PQClientQuoteDeserializerRepository pqClientQuoteDeserializerRepository = null!;
    private ITickerPricingSubscriptionConfig sourceTickerPricingSubscriptionConfig = null!;
    private ISourceTickerQuoteInfo sourceTickerQuoteInfo = null!;

    [TestInitialize]
    public void SetUp()
    {
        sourceTickerQuoteInfo = new SourceTickerQuoteInfo(ExpectedSourceId, "TestSource", ExpectedTickerd, "TestTicker", 20,
            0.00001m, 30000m, 50000000m, 1000m, 1, LayerFlags.Volume | LayerFlags.Price,
            LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName |
            LastTradedFlags.LastTradedVolume | LastTradedFlags.LastTradedTime);
        sourceTickerPricingSubscriptionConfig = new TickerPricingSubscriptionConfig(
            sourceTickerQuoteInfo,
            new PricingServerConfig(
                new NetworkTopicConnectionConfig("ConnectionName", SocketConversationProtocol.TcpClient, new[]
                {
                    new EndpointConfig("ConnectionName", 9090)
                }),
                new NetworkTopicConnectionConfig("ConnectionName", SocketConversationProtocol.TcpClient, new[]
                {
                    new EndpointConfig("ConnectionName", 9090)
                })));

        pqClientQuoteDeserializerRepository = new PQClientQuoteDeserializerRepository(new Recycler(), PQFeedType.Snapshot);
    }

    [TestMethod]
    public void NewSerializerFactory_CreateQuoteDeserializer_CreatesNewPQQuoteDeserializer()
    {
        var level0Deserializer = pqClientQuoteDeserializerRepository
            .CreateQuoteDeserializer<PQLevel0Quote>(sourceTickerPricingSubscriptionConfig);
        Assert.IsNotNull(level0Deserializer);
        Assert.AreEqual(level0Deserializer.Identifier, sourceTickerQuoteInfo);
        pqClientQuoteDeserializerRepository = new PQClientQuoteDeserializerRepository(new Recycler(), PQFeedType.Snapshot);
        var level1Deserializer = pqClientQuoteDeserializerRepository
            .CreateQuoteDeserializer<PQLevel1Quote>(sourceTickerPricingSubscriptionConfig);
        Assert.IsNotNull(level1Deserializer);
        Assert.AreEqual(level1Deserializer.Identifier, sourceTickerQuoteInfo);
        pqClientQuoteDeserializerRepository = new PQClientQuoteDeserializerRepository(new Recycler(), PQFeedType.Snapshot);
        var level2Deserializer = pqClientQuoteDeserializerRepository
            .CreateQuoteDeserializer<PQLevel2Quote>(sourceTickerPricingSubscriptionConfig);
        Assert.IsNotNull(level2Deserializer);
        Assert.AreEqual(level2Deserializer.Identifier, sourceTickerQuoteInfo);
        pqClientQuoteDeserializerRepository = new PQClientQuoteDeserializerRepository(new Recycler(), PQFeedType.Snapshot);
        var level3Deserializer = pqClientQuoteDeserializerRepository
            .CreateQuoteDeserializer<PQLevel3Quote>(sourceTickerPricingSubscriptionConfig);
        Assert.IsNotNull(level3Deserializer);
        Assert.AreEqual(level3Deserializer.Identifier, sourceTickerQuoteInfo);
    }

    [TestMethod]
    public void CreateDeserializer_GetQuoteDeserializer_ReturnsPreviouslyCreatedDeserializer()
    {
        var level0Deserializer = pqClientQuoteDeserializerRepository
            .CreateQuoteDeserializer<PQLevel0Quote>(sourceTickerPricingSubscriptionConfig);
        Assert.IsNotNull(level0Deserializer);

        var requestedDeserializer = pqClientQuoteDeserializerRepository.GetDeserializer(sourceTickerQuoteInfo);

        Assert.AreSame(level0Deserializer, requestedDeserializer);
    }

    [TestMethod]
    public void NoEnteredDeserializer_GetQuoteDeserializer_ReturnsNullDeserializer()
    {
        Assert.IsNull(pqClientQuoteDeserializerRepository.GetDeserializer(sourceTickerQuoteInfo));
    }

    [TestMethod]
    public void CreateDeserializer_RemoveQuoteDeserializer_RemovesDeserializerAndGetDeserializerReturnsNull()
    {
        var level0Deserializer = pqClientQuoteDeserializerRepository
            .CreateQuoteDeserializer<PQLevel0Quote>(sourceTickerPricingSubscriptionConfig);
        Assert.IsNotNull(level0Deserializer);

        pqClientQuoteDeserializerRepository.UnregisterDeserializer(sourceTickerQuoteInfo);

        Assert.IsNull(pqClientQuoteDeserializerRepository.GetDeserializer(sourceTickerQuoteInfo));
    }

    [TestMethod]
    public void CreateDeserializer_GetDeserializer_ReturnsDeserializerThatMatchesId()
    {
        var level0Deserializer = pqClientQuoteDeserializerRepository
            .CreateQuoteDeserializer<PQLevel0Quote>(sourceTickerPricingSubscriptionConfig);
        Assert.IsNotNull(level0Deserializer);

        var getLevel0Deserializer = pqClientQuoteDeserializerRepository
            .GetDeserializer<PQLevel0Quote>(sourceTickerQuoteInfo.Id);
        Assert.IsNotNull(getLevel0Deserializer);

        Assert.AreSame(getLevel0Deserializer, level0Deserializer);
    }

    [TestMethod]
    public void NoEnteredDeserializer_GetDeserializer_ReturnsNull()
    {
        var result = pqClientQuoteDeserializerRepository.GetDeserializer<PQLevel0Quote>(sourceTickerQuoteInfo.Id);
        Assert.IsNull(result);
    }
}
