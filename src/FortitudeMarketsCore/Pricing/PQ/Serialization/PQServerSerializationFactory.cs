using System;
using System.Collections.Generic;
using FortitudeIO.Protocols.Serialization;
using FortitudeMarketsCore.Pricing.PQ.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Subscription;

namespace FortitudeMarketsCore.Pricing.PQ.Serialization 
{
    internal sealed class PQServerSerializationFactory : IBinarySerializationFactory
    {
        private readonly PQQuoteSerializer pqQuoteSerializer; 
        private readonly PQHeartbeatSerializer hbSerializer;
        public PQServerSerializationFactory(PQFeedType feed) 
        {
            pqQuoteSerializer = new PQQuoteSerializer(feed == PQFeedType.Snapshot 
                                                            ? UpdateStyle.FullSnapshot : UpdateStyle.Updates);
            hbSerializer = new PQHeartbeatSerializer();
        }

        public IBinarySerializer GetSerializer<Tm>(uint msgId) where Tm : class
        {
            if (typeof(IPQLevel0Quote).IsAssignableFrom(typeof(Tm)) && msgId == 0)
            {
                return pqQuoteSerializer;
            }
            if (typeof(IEnumerable<IPQLevel0Quote>).IsAssignableFrom(typeof(Tm)) && msgId == 1)
            {
                return hbSerializer;
            }
            throw new NotSupportedException();
        }
    }
}
