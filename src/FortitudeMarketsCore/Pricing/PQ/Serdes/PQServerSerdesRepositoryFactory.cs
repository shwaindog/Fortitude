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

    public IMessageStreamDecoderFactory MessageStreamDecoderFactory(string name) =>
        new PQServerDeserializationRepository(name, deserializationRecycler, coalescingFallbackPQQuoteDeserializerRepository);

    public IMessageSerializationRepository MessageSerializationRepository =>
        new PQServerSerializationRepository(feedType, serializationRecycler, coalescingFallbackPQQuoteSerializerRepository);

    IMessageDeserializationRepository IMessageSerdesRepositoryFactory.MessageDeserializationRepository(string name) =>
        MessageDeserializationRepository(name);

    public IConversationDeserializationRepository MessageDeserializationRepository(string name) =>
        new PQServerDeserializationRepository(name, deserializationRecycler, coalescingFallbackPQQuoteDeserializerRepository);
}
