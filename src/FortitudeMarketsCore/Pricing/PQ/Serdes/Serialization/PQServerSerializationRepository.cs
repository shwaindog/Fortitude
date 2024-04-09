#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeMarketsCore.Pricing.PQ.Messages;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DeltaUpdates;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Serdes.Serialization;

public sealed class PQServerSerializationRepository : FactorySerializationRepository
{
    private readonly PQFeedType feedType;

    public PQServerSerializationRepository(PQFeedType feedType, IRecycler recycler
        , IMessageSerializationRepository? coalescingMessageSerializationRepository = null)
        : base(recycler, coalescingMessageSerializationRepository)
    {
        this.feedType = feedType;
        RegisterSerializer<PQLevel0Quote>();
        RegisterSerializer<PQHeartBeatQuotesMessage>();
        RegisterSerializer<PQSourceTickerInfoResponse>();
    }

    protected override IMessageSerializer? SourceMessageSerializer<TM>(uint msgId)
    {
        switch (msgId)
        {
            case (uint)PQMessageIds.Quote:
                return new PQQuoteSerializer(feedType == PQFeedType.Snapshot ? UpdateStyle.FullSnapshot : UpdateStyle.Updates);
            case (uint)PQMessageIds.HeartBeat: return new PQHeartbeatSerializer();
            case (uint)PQMessageIds.SourceTickerInfoResponse: return new PQSourceTickerInfoResponseSerializer();
            default: return null;
        }
    }
}
