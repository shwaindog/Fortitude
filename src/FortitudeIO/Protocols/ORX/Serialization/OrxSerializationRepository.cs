#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeIO.Protocols.ORX.Serialization.Deserialization;
using FortitudeIO.Protocols.Serdes.Binary;

#endregion

namespace FortitudeIO.Protocols.ORX.Serialization;

public sealed class OrxSerializationRepository : IMessageIdSerializationRepository,
    IMessageIdDeserializationRepository
{
    private readonly IRecycler recyclingFactory;

    public OrxSerializationRepository(IRecycler recyclingFactory) => this.recyclingFactory = recyclingFactory;


    public ICallbackMessageDeserializer<TM> GetDeserializer<TM>(uint msgId)
        where TM : class, IVersionedMessage, new() =>
        new OrxDeserializer<TM>(recyclingFactory);

    public IMessageSerializer GetSerializer<TM>(uint msgId) where TM : class, IVersionedMessage, new() =>
        new OrxSerializer<TM>((ushort)msgId);
}
