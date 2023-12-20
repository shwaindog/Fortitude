#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeIO.Protocols.ORX.Serialization.Deserialization;
using FortitudeIO.Protocols.Serialization;

#endregion

namespace FortitudeIO.Protocols.ORX.Serialization;

public sealed class OrxSerializationFactory : IBinarySerializationFactory,
    IBinaryDeserializationFactory
{
    private readonly IRecycler recyclingFactory;

    public OrxSerializationFactory(IRecycler recyclingFactory) => this.recyclingFactory = recyclingFactory;


    public ICallbackBinaryDeserializer<TM> GetDeserializer<TM>(uint msgId) where TM : class, new() =>
        new OrxDeserializer<TM>(recyclingFactory);

    public IBinarySerializer GetSerializer<TM>(uint msgId) where TM : class, new() =>
        new OrxSerializer<TM>((ushort)msgId);
}
