// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeMarkets.Pricing.PQ.Messages;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Serdes.Serialization;

public sealed class PQServerSerializationRepository : FactorySerializationRepository
{
    private readonly PQMessageFlags feedType;

    public PQServerSerializationRepository
    (PQMessageFlags feedType, IRecycler recycler
      , IMessageSerializationRepository? coalescingMessageSerializationRepository = null)
        : base(recycler, coalescingMessageSerializationRepository)
    {
        this.feedType = feedType;
        RegisterSerializer<PQPublishableTickInstant>();
        RegisterSerializer<PQHeartBeatQuotesMessage>();
        RegisterSerializer<PQSourceTickerInfoResponse>();
    }

    protected override IMessageSerializer? SourceMessageSerializer<TM>(uint msgId)
    {
        switch (msgId)
        {
            case (uint)PQMessageIds.Quote:     return new PQQuoteSerializer(feedType);
            case (uint)PQMessageIds.HeartBeat: return new PQHeartbeatSerializer();

            case (uint)PQMessageIds.SourceTickerInfoResponse: return new PQSourceTickerInfoResponseSerializer();

            default: return null;
        }
    }
}
