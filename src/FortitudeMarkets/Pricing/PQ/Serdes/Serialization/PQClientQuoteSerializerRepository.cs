#region

using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeMarkets.Pricing.PQ.Messages;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Serdes.Serialization;

public sealed class PQClientQuoteSerializerRepository :
    FactorySerializationRepository
{
    public PQClientQuoteSerializerRepository(IRecycler recycler, IMessageSerializationRepository? fallbackCoalasingDeserializer = null) : base(
        recycler, fallbackCoalasingDeserializer)
    {
        RegisterSerializer<PQSnapshotIdsRequest>();
        RegisterSerializer<PQSourceTickerInfoRequest>();
    }

    protected override IMessageSerializer? SourceMessageSerializer<TM>(uint msgId)
    {
        switch (msgId)
        {
            case (uint)PQMessageIds.SnapshotIdsRequest: return new PQSnapshotIdsRequestSerializer();
            case (uint)PQMessageIds.SourceTickerInfoRequest: return new PQSourceTickerInfoRequestSerializer();
        }

        throw new NotSupportedException();
    }
}
