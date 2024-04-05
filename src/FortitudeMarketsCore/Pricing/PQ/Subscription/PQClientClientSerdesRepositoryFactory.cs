#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeIO.Protocols.Serdes.Binary;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Subscription;

public interface IPQClientSerdesRepositoryFactory : IMessageSerdesRepositoryFactory
{
    new IPQClientQuoteDeserializerRepository MessageDeserializationRepository { get; }
}

public class PQClientClientSerdesRepositoryFactory(PQFeedType feedType, IRecycler? serializationRecycler = null,
        IRecycler? deserializationRecycler = null, IMessageSerializationRepository? coalescingFallbackPQQuoteSerializerRepository = null,
        IPQClientQuoteDeserializerRepository? coalescingFallbackPQQuoteDeserializerRepository = null)
    : IPQClientSerdesRepositoryFactory
{
    private readonly IRecycler deserializationRecycler = deserializationRecycler ?? new Recycler();
    private readonly IRecycler serializationRecycler = serializationRecycler ?? new Recycler();
    private IPQClientQuoteDeserializerRepository? singleInstanceDeserializationRepo;

    private IMessageSerializationRepository? singleInstanceSerializationRepo;

    public IMessageSerializationRepository MessageSerializationRepository =>
        singleInstanceSerializationRepo ??=
            new PQClientQuoteSerializerRepository(serializationRecycler, coalescingFallbackPQQuoteSerializerRepository);

    IMessageDeserializationRepository IMessageSerdesRepositoryFactory.MessageDeserializationRepository => MessageDeserializationRepository;

    public IPQClientQuoteDeserializerRepository MessageDeserializationRepository =>
        singleInstanceDeserializationRepo ??=
            new PQClientQuoteDeserializerRepository(deserializationRecycler, feedType, coalescingFallbackPQQuoteDeserializerRepository);
}
