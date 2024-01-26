#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeIO.Protocols.ORX.Serialization.Deserialization;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Protocols.Serialization;

#endregion

namespace FortitudeIO.Protocols.ORX.Serialization;

public sealed class OrxSerializationFactory : IBinarySerializationFactory,
    IBinaryDeserializationFactory
{
    private readonly IRecycler recyclingFactory;

    public OrxSerializationFactory(IRecycler recyclingFactory) => this.recyclingFactory = recyclingFactory;


    public ICallbackMessageDeserializer<TM> GetDeserializer<TM>(uint msgId)
        where TM : class, IVersionedMessage, new() =>
        new OrxDeserializer<TM>(recyclingFactory);

    public IMessageSerializer GetSerializer<TM>(uint msgId) where TM : class, IVersionedMessage, new() =>
        new OrxSerializer<TM>((ushort)msgId);
}
