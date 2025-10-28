#region

using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeIO.Protocols.ORX.Serdes.Deserialization;
using FortitudeIO.Protocols.ORX.Serdes.Serialization;
using FortitudeIO.Protocols.Serdes.Binary;

#endregion

namespace FortitudeIO.Protocols.ORX.Serdes;

public interface IOrxSerdesRepositoryFactory : IMessageSerdesRepositoryFactory
{
    new IConversationDeserializationRepository MessageDeserializationRepository(string name);
}

public sealed class OrxSerdesRepositoryFactory : IOrxSerdesRepositoryFactory
{
    private readonly IRecycler deserializationRecycler;
    private readonly IMessageSerdesRepositoryFactory? fallbackMessageSerdesRepositoryFactory;
    private readonly IRecycler serializationRecycler;
    private IConversationDeserializationRepository? messageDeserializationRepository;
    private IMessageSerializationRepository? messageSerializationRepository;

    public OrxSerdesRepositoryFactory(IRecycler? serializationRecycler = null, IRecycler? deserializationRecycler = null,
        IMessageSerdesRepositoryFactory? fallbackMessageSerdesRepositoryFactory = null)
    {
        this.serializationRecycler = serializationRecycler ?? new Recycler();
        this.deserializationRecycler = deserializationRecycler ?? new Recycler();
        this.fallbackMessageSerdesRepositoryFactory = fallbackMessageSerdesRepositoryFactory;
    }

    public IConversationDeserializationRepository MessageDeserializationRepository(string name) =>
        messageDeserializationRepository
            ??= new OrxMessageDeserializationRepository(name, deserializationRecycler
                , fallbackMessageSerdesRepositoryFactory?.MessageDeserializationRepository(name));

    public IMessageStreamDecoderFactory MessageStreamDecoderFactory(string name) =>
        messageDeserializationRepository
            ??= new OrxMessageDeserializationRepository(name, deserializationRecycler
                , fallbackMessageSerdesRepositoryFactory?.MessageDeserializationRepository(name));

    public IMessageSerializationRepository MessageSerializationRepository =>
        messageSerializationRepository
            ??= new OrxMessageSerializationRepository(serializationRecycler, fallbackMessageSerdesRepositoryFactory?.MessageSerializationRepository);

    IMessageDeserializationRepository IMessageSerdesRepositoryFactory.MessageDeserializationRepository(string name) =>
        MessageDeserializationRepository(name);
}
