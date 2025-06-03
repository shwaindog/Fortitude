// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeMarkets.Pricing.PQ.Messages;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;
using PQMessageFlags = FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.PQMessageFlags;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Serdes.Serialization;

[TestClass]
public class PQServerSerializationRepositoryTests
{
    private PQServerSerializationRepository snapshotServerSerializationRepository = null!;

    [TestInitialize]
    public void SetUp()
    {
        snapshotServerSerializationRepository = new PQServerSerializationRepository(PQMessageFlags.Snapshot, new Recycler());
    }

    [TestMethod]
    public void NewSerializationFactory_GetSerializer_ReturnsAppropriateSerializerForMessageType()
    {
        snapshotServerSerializationRepository.RegisterSerializer<PQPublishableTickInstant>();
        var quoteSerializer = snapshotServerSerializationRepository.GetSerializer((uint)PQMessageIds.Quote);
        Assert.IsInstanceOfType(quoteSerializer, typeof(PQMessageSerializer));

        snapshotServerSerializationRepository.RegisterSerializer<PQHeartBeatQuotesMessage>();
        var heartBeatSerializer = snapshotServerSerializationRepository.GetSerializer<PQHeartBeatQuotesMessage>((uint)PQMessageIds.HeartBeat);
        Assert.IsInstanceOfType(heartBeatSerializer, typeof(PQHeartbeatSerializer));

        snapshotServerSerializationRepository.RegisterSerializer<PQSourceTickerInfoResponse>();
        var sourceTickerInfoResponseSerializer
            = snapshotServerSerializationRepository.GetSerializer<PQSourceTickerInfoResponse>((uint)PQMessageIds.SourceTickerInfoResponse);
        Assert.IsInstanceOfType(sourceTickerInfoResponseSerializer, typeof(PQSourceTickerInfoResponseSerializer));
    }
}
