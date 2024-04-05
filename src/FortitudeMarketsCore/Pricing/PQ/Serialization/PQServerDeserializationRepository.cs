#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeMarketsCore.Pricing.PQ.Publication;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Serialization;

public interface IPQServerDeserializationRepository : IMessageDeserializationRepository
{
    new IPQServerMessageStreamDecoder Supply();
}

public sealed class PQServerDeserializationRepository(IRecycler recycler
        , IMessageDeserializationRepository? cascadingFallbackDeserializationRepo = null)
    : FactoryDeserializationRepository(recycler, cascadingFallbackDeserializationRepo), IPQServerDeserializationRepository
{
    public override IPQServerMessageStreamDecoder Supply() => new PQServerMessageStreamDecoder(this);

    protected override INotifyingMessageDeserializer<TM>? SourceMessageDeserializer<TM>(uint msgId) =>
        throw new NotImplementedException("No Server Deserializers are required");
}
