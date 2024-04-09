#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeMarketsCore.Pricing.PQ.Messages;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Serdes.Serialization;

public sealed class PQClientQuoteSerializerRepository :
    FactorySerializationRepository
{
    public PQClientQuoteSerializerRepository(IRecycler recycler, IMessageSerializationRepository? fallbackCoalasingDeserializer = null) : base(
        recycler, fallbackCoalasingDeserializer)
    {
        RegisterSerializer<PQSnapshotIdsRequest>();
        RegisterSerializer<PQSourceTickerInfoResponse>();
    }

    protected override IMessageSerializer? SourceMessageSerializer<TM>(uint msgId)
    {
        switch (msgId)
        {
            case (uint)PQMessageIds.SnapshotIdsRequest: return new PQSnapshotIdsRequestSerializer();
            case (uint)PQMessageIds.SourceTickerInfoResponse: return new PQSourceTickerInfoResponseSerializer();
        }

        throw new NotSupportedException();
    }
}
