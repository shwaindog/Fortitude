#region

using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Protocols.Serialization;
using FortitudeMarketsCore.Pricing.PQ.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Subscription;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Serialization;

internal sealed class PQServerSerializationFactory : IBinarySerializationFactory
{
    private readonly PQHeartbeatSerializer hbSerializer;
    private readonly PQQuoteSerializer pqQuoteSerializer;

    public PQServerSerializationFactory(PQFeedType feed)
    {
        pqQuoteSerializer
            = new PQQuoteSerializer(feed == PQFeedType.Snapshot ? UpdateStyle.FullSnapshot : UpdateStyle.Updates);
        hbSerializer = new PQHeartbeatSerializer();
    }

    public IMessageSerializer GetSerializer<Tm>(uint msgId) where Tm : class, IVersionedMessage, new()
    {
        if (typeof(IPQLevel0Quote).IsAssignableFrom(typeof(Tm)) && msgId == 0) return pqQuoteSerializer;
        if (typeof(IEnumerable<IPQLevel0Quote>).IsAssignableFrom(typeof(Tm)) && msgId == 1) return hbSerializer;
        throw new NotSupportedException();
    }
}
