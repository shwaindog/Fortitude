#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeIO.Transports.Network.Config;
using FortitudeMarketsApi.Configuration.ClientServerConfig;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.LastTraded;
using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsCore.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsCore.Pricing.PQ.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Subscription;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription;

[TestClass]
public class PQClientQuoteDeserializerRepositoryTests
{
    private const uint ExpectedStreamId = uint.MaxValue;
    private PQClientQuoteDeserializerRepository pqClientQuoteDeserializerRepository = null!;
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
                        new NetworkTopicConnectionConfig("ConnectionName", SocketConversationProtocol.TcpClient, new[]
                        {
                            new EndpointConfig("ConnectionName", 9090)
                        })
                    }, null, 0,
                    new List<ISourceTickerPublicationConfig>(), true, true)));

        pqClientQuoteDeserializerRepository = new PQClientQuoteDeserializerRepository(new Recycler(), PQFeedType.Snapshot);
    }

    [TestMethod]
    public void NewSerializerFactory_CreateQuoteDeserializer_CreatesNewPQQuoteDeserializer()
    {
        var level0Deserializer = pqClientQuoteDeserializerRepository
            .CreateQuoteDeserializer<PQLevel0Quote>(sourceTickerClientAndPublicationConfig);
        Assert.IsNotNull(level0Deserializer);
        Assert.AreEqual(level0Deserializer.Identifier, sourceTickerClientAndPublicationConfig);
        pqClientQuoteDeserializerRepository = new PQClientQuoteDeserializerRepository(new Recycler(), PQFeedType.Snapshot);
        var level1Deserializer = pqClientQuoteDeserializerRepository
            .CreateQuoteDeserializer<PQLevel1Quote>(sourceTickerClientAndPublicationConfig);
        Assert.IsNotNull(level1Deserializer);
        Assert.AreEqual(level1Deserializer.Identifier, sourceTickerClientAndPublicationConfig);
        pqClientQuoteDeserializerRepository = new PQClientQuoteDeserializerRepository(new Recycler(), PQFeedType.Snapshot);
        var level2Deserializer = pqClientQuoteDeserializerRepository
            .CreateQuoteDeserializer<PQLevel2Quote>(sourceTickerClientAndPublicationConfig);
        Assert.IsNotNull(level2Deserializer);
        Assert.AreEqual(level2Deserializer.Identifier, sourceTickerClientAndPublicationConfig);
        pqClientQuoteDeserializerRepository = new PQClientQuoteDeserializerRepository(new Recycler(), PQFeedType.Snapshot);
        var level3Deserializer = pqClientQuoteDeserializerRepository
            .CreateQuoteDeserializer<PQLevel3Quote>(sourceTickerClientAndPublicationConfig);
        Assert.IsNotNull(level3Deserializer);
        Assert.AreEqual(level3Deserializer.Identifier, sourceTickerClientAndPublicationConfig);
    }

    [TestMethod]
    public void CreateDeserializer_GetQuoteDeserializer_ReturnsPreviouslyCreatedDeserializer()
    {
        var level0Deserializer = pqClientQuoteDeserializerRepository
            .CreateQuoteDeserializer<PQLevel0Quote>(sourceTickerClientAndPublicationConfig);
        Assert.IsNotNull(level0Deserializer);

        var requestedDeserializer = pqClientQuoteDeserializerRepository.GetDeserializer(sourceTickerClientAndPublicationConfig);

        Assert.AreSame(level0Deserializer, requestedDeserializer);
    }

    [TestMethod]
    public void NoEnteredDeserializer_GetQuoteDeserializer_ReturnsNullDeserializer()
    {
        Assert.IsNull(pqClientQuoteDeserializerRepository.GetDeserializer(sourceTickerClientAndPublicationConfig));
    }

    [TestMethod]
    public void CreateDeserializer_RemoveQuoteDeserializer_RemovesDeserializerAndGetDeserializerReturnsNull()
    {
        var level0Deserializer = pqClientQuoteDeserializerRepository
            .CreateQuoteDeserializer<PQLevel0Quote>(sourceTickerClientAndPublicationConfig);
        Assert.IsNotNull(level0Deserializer);

        pqClientQuoteDeserializerRepository.UnregisterDeserializer(sourceTickerClientAndPublicationConfig);

        Assert.IsNull(pqClientQuoteDeserializerRepository.GetDeserializer(sourceTickerClientAndPublicationConfig));
    }

    [TestMethod]
    public void CreateDeserializer_GetDeserializer_ReturnsDeserializerThatMatchesId()
    {
        var level0Deserializer = pqClientQuoteDeserializerRepository
            .CreateQuoteDeserializer<PQLevel0Quote>(sourceTickerClientAndPublicationConfig);
        Assert.IsNotNull(level0Deserializer);

        var getLevel0Deserializer = pqClientQuoteDeserializerRepository
            .GetDeserializer<PQLevel0Quote>(sourceTickerClientAndPublicationConfig.Id);
        Assert.IsNotNull(getLevel0Deserializer);

        Assert.AreSame(getLevel0Deserializer, level0Deserializer);
    }

    [TestMethod]
    public void NoEnteredDeserializer_GetDeserializer_ReturnsNull()
    {
        var result = pqClientQuoteDeserializerRepository.GetDeserializer<PQLevel0Quote>(sourceTickerClientAndPublicationConfig.Id);
        Assert.IsNull(result);
    }
}
