#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeMarketsCore.Pricing.PQ;
using FortitudeMarketsCore.Pricing.PQ.Messages;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;
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
        var quoteSerializer = snapshotServerSerializationRepository.GetSerializer((uint)PQMessageIds.Quote);
        Assert.IsInstanceOfType(quoteSerializer, typeof(PQQuoteSerializer));

        snapshotServerSerializationRepository.RegisterSerializer<PQHeartBeatQuotesMessage>();
        var heartBeatSerializer = snapshotServerSerializationRepository.GetSerializer<PQHeartBeatQuotesMessage>((uint)PQMessageIds.HeartBeat);
        Assert.IsInstanceOfType(heartBeatSerializer, typeof(PQHeartbeatSerializer));

        snapshotServerSerializationRepository.RegisterSerializer<PQSourceTickerInfoResponse>();
        var sourceTickerInfoResponseSerializer
            = snapshotServerSerializationRepository.GetSerializer<PQSourceTickerInfoResponse>((uint)PQMessageIds.SourceTickerInfoResponse);
        Assert.IsInstanceOfType(sourceTickerInfoResponseSerializer, typeof(PQSourceTickerInfoResponseSerializer));
    }
}
