#region

using FortitudeMarketsCore.Pricing.PQ.Publication;
using FortitudeMarketsCore.Pricing.PQ.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Serialization;
using FortitudeMarketsCore.Pricing.PQ.Subscription;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serialization;

[TestClass]
public class PQServerSerializationRepositoryTests
{
    private PQServerSerializationRepository snapshotServerSerializationRepository = null!;

    [TestInitialize]
    public void SetUp()
    {
        snapshotServerSerializationRepository = new PQServerSerializationRepository(PQFeedType.Snapshot);
    }

    [TestMethod]
    public void NewSerializationFactory_GetSerializer_ReturnsAppropriateSerializerForMessageType()
    {
        var quoteSerializer = snapshotServerSerializationRepository.GetSerializer<PQLevel0Quote>(0);
        Assert.IsInstanceOfType(quoteSerializer, typeof(PQQuoteSerializer));

        var heartBeatSerializer = snapshotServerSerializationRepository.GetSerializer<PQHeartBeatQuotesMessage>(1);
        Assert.IsInstanceOfType(heartBeatSerializer, typeof(PQHeartbeatSerializer));
    }

    [TestMethod]
    [ExpectedException(typeof(NotSupportedException))]
    public void QuoteSerializerWrongId_GetSerializerWrongIdsForType_ThrowsNotSupportedException()
    {
        snapshotServerSerializationRepository.GetSerializer<PQLevel0Quote>(1);
    }

    [TestMethod]
    [ExpectedException(typeof(NotSupportedException))]
    public void HeartBeatSerializerWrongId_GetSerializerWrongIdsForType_ThrowsNotSupportedException()
    {
        snapshotServerSerializationRepository.GetSerializer<PQHeartBeatQuotesMessage>(0);
    }
}
