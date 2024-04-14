#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeMarketsCore.Pricing.PQ.Messages;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Serdes.Deserialization;

public interface IPQServerDeserializationRepository : IConversationDeserializationRepository, IMessageStreamDecoderFactory { }

public sealed class PQServerDeserializationRepository : ConversationDeserializationRepository, IPQServerDeserializationRepository
{
    public PQServerDeserializationRepository(IRecycler recycler, IMessageDeserializationRepository? cascadingFallbackDeserializationRepo = null) :
        base(recycler, cascadingFallbackDeserializationRepo) { }

    public override IPQServerMessageStreamDecoder Supply() => new PQServerMessageStreamDecoder(new PQServerDeserializationRepository(Recycler, this));

    protected override IMessageDeserializer? SourceMessageDeserializer<TM>(uint msgId)
    {
        switch (msgId)
        {
            case (uint)PQMessageIds.SnapshotIdsRequest: return new PQSnapshotIdsRequestDeserializer(Recycler);
            case (uint)PQMessageIds.SourceTickerInfoRequest: return new PQSourceTickerInfoRequestDeserializer(Recycler);
        }

        return null;
    }
}
