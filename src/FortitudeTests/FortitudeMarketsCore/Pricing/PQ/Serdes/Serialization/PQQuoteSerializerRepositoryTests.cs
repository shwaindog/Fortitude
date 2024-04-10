#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.LastTraded;
using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsCore.Pricing.PQ.Messages;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Serdes.Serialization;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serdes.Serialization;

[TestClass]
public class PQClientQuoteSerializerRepositoryTests
{
    private const ushort ExpectedSourceId = ushort.MaxValue;
    private const ushort ExpectedTickerd = ushort.MaxValue;
    private PQClientQuoteSerializerRepository pqClientQuoteSerializerRepository = null!;
    private ISourceTickerQuoteInfo sourceTickerQuoteInfo = null!;

    [TestInitialize]
    public void SetUp()
    {
        sourceTickerQuoteInfo = new SourceTickerQuoteInfo(ExpectedSourceId, "TestSource", ExpectedTickerd, "TestTicker", 20,
            0.00001m, 30000m, 50000000m, 1000m, 1, LayerFlags.Volume | LayerFlags.Price,
            LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName |
            LastTradedFlags.LastTradedVolume | LastTradedFlags.LastTradedTime);

        pqClientQuoteSerializerRepository = new PQClientQuoteSerializerRepository(new Recycler(), null);
    }

    [TestMethod]
    public void EmptyQuoteSerializationFactory_GetSerializerUintArray_ReturnsPQRequestSerializer()
    {
        pqClientQuoteSerializerRepository.RegisterSerializer<PQSnapshotIdsRequest>();
        var uintArraySerializer
            = pqClientQuoteSerializerRepository.GetSerializer<PQSnapshotIdsRequest>((uint)PQMessageIds.SnapshotIdsRequest);
        Assert.IsNotNull(uintArraySerializer);
        Assert.IsInstanceOfType(uintArraySerializer, typeof(PQSnapshotIdsRequestSerializer));
    }

    [TestMethod]
    public void NoEnteredDeserializer_GetSerializerNonSupportedType_ReturnsNull()
    {
        var result = pqClientQuoteSerializerRepository.GetSerializer<PQLevel0Quote>(sourceTickerQuoteInfo.Id);
        Assert.IsNull(result);
    }
}
