#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeMarkets.Pricing.PQ.Serdes.Deserialization;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Serdes;

public interface IPQClientSerdesRepositoryFactory : IMessageSerdesRepositoryFactory
{
    new IPQClientQuoteDeserializerRepository MessageDeserializationRepository(string name);
}

public class PQClientClientSerdesRepositoryFactory(IRecycler? serializationRecycler = null,
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

    public IMessageStreamDecoderFactory MessageStreamDecoderFactory(string name) =>
        singleInstanceDeserializationRepo ??=
            new PQClientQuoteDeserializerRepository(name, deserializationRecycler, coalescingFallbackPQQuoteDeserializerRepository);

    IMessageDeserializationRepository IMessageSerdesRepositoryFactory.MessageDeserializationRepository(string name) =>
        MessageDeserializationRepository(name);

    public IPQClientQuoteDeserializerRepository MessageDeserializationRepository(string name) =>
        singleInstanceDeserializationRepo ??=
            new PQClientQuoteDeserializerRepository(name, deserializationRecycler, coalescingFallbackPQQuoteDeserializerRepository);
}
