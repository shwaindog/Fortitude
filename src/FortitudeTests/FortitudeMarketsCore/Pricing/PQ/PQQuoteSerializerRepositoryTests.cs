#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeIO.Transports.Network.Config;
using FortitudeMarketsApi.Configuration.ClientServerConfig;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.LastTraded;
using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsCore.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsCore.Pricing.PQ.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Serialization;
using FortitudeMarketsCore.Pricing.PQ.Subscription;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ;

[TestClass]
public class PQClientQuoteSerializerRepositoryTests
{
    private const uint ExpectedStreamId = uint.MaxValue;
    private PQClientQuoteSerializerRepository pqClientQuoteSerializerRepository = null!;
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

        pqClientQuoteSerializerRepository = new PQClientQuoteSerializerRepository(new Recycler(), null);
    }

    [TestMethod]
    public void EmptyQuoteSerializationFactory_GetSerializerUintArray_ReturnsPQRequestSerializer()
    {
        pqClientQuoteSerializerRepository.RegisterSerializer<PQSnapshotIdsRequest>();
        var uintArraySerializer
            = pqClientQuoteSerializerRepository.GetSerializer<PQSnapshotIdsRequest>(0);
        Assert.IsNotNull(uintArraySerializer);
        Assert.IsInstanceOfType(uintArraySerializer, typeof(PQSnapshotIdsRequestSerializer));
    }

    [TestMethod]
    public void NoEnteredDeserializer_GetSerializerNonSupportedType_ReturnsNull()
    {
        var result = pqClientQuoteSerializerRepository.GetSerializer<PQLevel0Quote>(sourceTickerClientAndPublicationConfig.Id);
        Assert.IsNull(result);
    }
}
