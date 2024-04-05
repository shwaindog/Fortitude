#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeIO.Protocols.ORX.Serdes.Deserialization;
using FortitudeIO.Protocols.Serdes.Binary;

#endregion

namespace FortitudeIO.Protocols.ORX.Serdes;

public interface IOrxSerdesRepositoryFactory : IMessageSerdesRepositoryFactory
{
    new IOrxDeserializationRepository MessageDeserializationRepository { get; }
}

public sealed class OrxSerdesRepositoryFactory : IOrxSerdesRepositoryFactory
{
    private readonly IRecycler deserializationRecycler;
    private readonly IMessageSerdesRepositoryFactory? fallbackMessageSerdesRepositoryFactory;
    private readonly IRecycler serializationRecycler;
    private IOrxDeserializationRepository? messageDeserializationRepository;
    private IMessageSerializationRepository? messageSerializationRepository;

    public OrxSerdesRepositoryFactory(IRecycler? serializationRecycler = null, IRecycler? deserializationRecycler = null,
        IMessageSerdesRepositoryFactory? fallbackMessageSerdesRepositoryFactory = null)
    {
        this.serializationRecycler = serializationRecycler ?? new Recycler();
        this.deserializationRecycler = deserializationRecycler ?? new Recycler();
        this.fallbackMessageSerdesRepositoryFactory = fallbackMessageSerdesRepositoryFactory;
    }

    public IOrxDeserializationRepository MessageDeserializationRepository =>
        messageDeserializationRepository
            ??= new OrxMessageDeserializationRepository(deserializationRecycler
                , fallbackMessageSerdesRepositoryFactory?.MessageDeserializationRepository);

    public IMessageSerializationRepository MessageSerializationRepository =>
        messageSerializationRepository
            ??= new OrxMessageSerializationRepository(serializationRecycler, fallbackMessageSerdesRepositoryFactory?.MessageSerializationRepository);

    IMessageDeserializationRepository IMessageSerdesRepositoryFactory.MessageDeserializationRepository => MessageDeserializationRepository;
}
