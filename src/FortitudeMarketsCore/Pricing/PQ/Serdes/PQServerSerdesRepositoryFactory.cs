#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Serdes.Deserialization;
using FortitudeMarketsCore.Pricing.PQ.Serdes.Serialization;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Serdes;

public class PQServerSerdesRepositoryFactory(PQMessageFlags feedType, IRecycler? serializationRecycler = null,
        IRecycler? deserializationRecycler = null, IMessageSerializationRepository? coalescingFallbackPQQuoteSerializerRepository = null,
        IPQClientQuoteDeserializerRepository? coalescingFallbackPQQuoteDeserializerRepository = null)
    : IMessageSerdesRepositoryFactory
{
    private readonly IRecycler deserializationRecycler = deserializationRecycler ?? new Recycler();
    private readonly IRecycler serializationRecycler = serializationRecycler ?? new Recycler();

    private IPQServerDeserializationRepository? singleInstanceDeserializationRepo;

    private IMessageSerializationRepository? singleInstanceSerializationRepo;

    public IPQServerDeserializationRepository MessageDeserializationRepository =>
        singleInstanceDeserializationRepo ??=
            new PQServerDeserializationRepository(deserializationRecycler, coalescingFallbackPQQuoteDeserializerRepository);

    public IMessageStreamDecoderFactory MessageStreamDecoderFactory =>
        singleInstanceDeserializationRepo ??=
            new PQServerDeserializationRepository(deserializationRecycler, coalescingFallbackPQQuoteDeserializerRepository);

    public IMessageSerializationRepository MessageSerializationRepository =>
        singleInstanceSerializationRepo ??=
            new PQServerSerializationRepository(feedType, serializationRecycler, coalescingFallbackPQQuoteSerializerRepository);

    IMessageDeserializationRepository IMessageSerdesRepositoryFactory.MessageDeserializationRepository => MessageDeserializationRepository;
}
