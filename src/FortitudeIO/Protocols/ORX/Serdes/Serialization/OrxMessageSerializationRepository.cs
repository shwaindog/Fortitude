#region

using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeIO.Protocols.Serdes.Binary;

#endregion

namespace FortitudeIO.Protocols.ORX.Serdes.Serialization;

public class OrxMessageSerializationRepository(IRecycler recycler
        , IMessageSerializationRepository? cascadingFallbackMessageSerializationRepository = null)
    : FactorySerializationRepository(recycler, cascadingFallbackMessageSerializationRepository)
{
    protected override IMessageSerializer<TM>? SourceMessageSerializer<TM>(uint msgId) => new OrxSerializer<TM>((ushort)msgId);
}
