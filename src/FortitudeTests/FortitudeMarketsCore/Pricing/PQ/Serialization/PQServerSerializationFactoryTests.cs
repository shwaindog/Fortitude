#region

using FortitudeMarketsCore.Pricing.PQ.Publication;
using FortitudeMarketsCore.Pricing.PQ.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Serialization;
using FortitudeMarketsCore.Pricing.PQ.Subscription;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serialization;

[TestClass]
public class PQServerSerializationFactoryTests
{
    private PQServerSerializationFactory snapshotServerSerializationFactory = null!;

    [TestInitialize]
    public void SetUp()
    {
        snapshotServerSerializationFactory = new PQServerSerializationFactory(PQFeedType.Snapshot);
    }

    [TestMethod]
    public void NewSerializationFactory_GetSerializer_ReturnsAppropriateSerializerForMessageType()
    {
        var quoteSerializer = snapshotServerSerializationFactory.GetSerializer<PQLevel0Quote>(0);
        Assert.IsInstanceOfType(quoteSerializer, typeof(PQQuoteSerializer));

        var heartBeatSerializer = snapshotServerSerializationFactory.GetSerializer<PQHeartBeatQuotesMessage>(1);
        Assert.IsInstanceOfType(heartBeatSerializer, typeof(PQHeartbeatSerializer));
    }

    [TestMethod]
    [ExpectedException(typeof(NotSupportedException))]
    public void QuoteSerializerWrongId_GetSerializerWrongIdsForType_ThrowsNotSupportedException()
    {
        snapshotServerSerializationFactory.GetSerializer<PQLevel0Quote>(1);
    }

    [TestMethod]
    [ExpectedException(typeof(NotSupportedException))]
    public void HeartBeatSerializerWrongId_GetSerializerWrongIdsForType_ThrowsNotSupportedException()
    {
        snapshotServerSerializationFactory.GetSerializer<PQHeartBeatQuotesMessage>(0);
    }
}
