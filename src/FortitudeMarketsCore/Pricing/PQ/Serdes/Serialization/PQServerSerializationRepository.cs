#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeMarketsCore.Pricing.PQ.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.Publication;
using FortitudeMarketsCore.Pricing.PQ.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Serdes.Serialization;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Serdes;

public sealed class PQServerSerializationRepository : FactorySerializationRepository
{
    private readonly PQHeartbeatSerializer hbSerializer;
    private readonly PQQuoteSerializer pqQuoteSerializer;

    public PQServerSerializationRepository(PQFeedType feed, IRecycler recycler
        , IMessageSerializationRepository? coalescingMessageSerializationRepository = null)
        : base(recycler, coalescingMessageSerializationRepository)
    {
        pqQuoteSerializer = new PQQuoteSerializer(feed == PQFeedType.Snapshot ? UpdateStyle.FullSnapshot : UpdateStyle.Updates);
        hbSerializer = new PQHeartbeatSerializer();
        RegisterSerializer<PQLevel0Quote>();
        RegisterSerializer<PQHeartBeatQuotesMessage>();
    }

    protected override IMessageSerializer? SourceMessageSerializer<TM>(uint msgId)
    {
        switch (msgId)
        {
            case 0: return pqQuoteSerializer;
            case 1: return hbSerializer;
            default: return null;
        }
    }
}
