#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeMarketsCore.Pricing.PQ.Serdes.Deserialization;
using FortitudeMarketsCore.Pricing.PQ.Serdes.Serialization;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Serdes;

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

    public IMessageStreamDecoderFactory MessageStreamDecoderFactory =>
        singleInstanceDeserializationRepo ??=
            new PQClientQuoteDeserializerRepository(deserializationRecycler, feedType, coalescingFallbackPQQuoteDeserializerRepository);

    IMessageDeserializationRepository IMessageSerdesRepositoryFactory.MessageDeserializationRepository => MessageDeserializationRepository;

    public IPQClientQuoteDeserializerRepository MessageDeserializationRepository =>
        singleInstanceDeserializationRepo ??=
            new PQClientQuoteDeserializerRepository(deserializationRecycler, feedType, coalescingFallbackPQQuoteDeserializerRepository);
}
