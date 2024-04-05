#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeMarketsCore.Pricing.PQ;
using FortitudeMarketsCore.Pricing.PQ.Publication;
using FortitudeMarketsCore.Pricing.PQ.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Serdes;
using FortitudeMarketsCore.Pricing.PQ.Serdes.Serialization;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serdes.Serialization;

[TestClass]
public class PQServerSerializationRepositoryTests
{
    private PQServerSerializationRepository snapshotServerSerializationRepository = null!;

    [TestInitialize]
    public void SetUp()
    {
        snapshotServerSerializationRepository = new PQServerSerializationRepository(PQFeedType.Snapshot, new Recycler());
    }

    [TestMethod]
    public void NewSerializationFactory_GetSerializer_ReturnsAppropriateSerializerForMessageType()
    {
        snapshotServerSerializationRepository.RegisterSerializer<PQLevel0Quote>();
        var quoteSerializer = snapshotServerSerializationRepository.GetSerializer(0);
        Assert.IsInstanceOfType(quoteSerializer, typeof(PQQuoteSerializer));

        snapshotServerSerializationRepository.RegisterSerializer<PQHeartBeatQuotesMessage>();
        var heartBeatSerializer = snapshotServerSerializationRepository.GetSerializer<PQHeartBeatQuotesMessage>(1);
        Assert.IsInstanceOfType(heartBeatSerializer, typeof(PQHeartbeatSerializer));
    }
}
