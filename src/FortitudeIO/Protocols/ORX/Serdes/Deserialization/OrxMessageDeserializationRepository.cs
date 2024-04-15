#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeIO.Protocols.Serdes.Binary;

#endregion

namespace FortitudeIO.Protocols.ORX.Serdes.Deserialization;

public interface IOrxDeserializationRepository : IConversationDeserializationRepository, IMessageStreamDecoderFactory { }

internal class OrxMessageDeserializationRepository : ConversationDeserializationRepository, IOrxDeserializationRepository
{
    public OrxMessageDeserializationRepository(IRecycler recycler, IMessageDeserializationRepository? cascadingFallbackDeserializationRepo = null) :
        base(recycler, cascadingFallbackDeserializationRepo) { }

    public override IMessageStreamDecoder Supply() => new OrxMessageStreamDecoder(new OrxMessageDeserializationRepository(Recycler, this));

    protected override INotifyingMessageDeserializer<TM>? SourceMessageDeserializer<TM>(uint msgId) => new OrxDeserializer<TM>(Recycler, msgId);
}
