#region

using System.Collections;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeMarketsCore.Pricing.PQ.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.Subscription;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Serialization;

internal sealed class PQServerSerializationRepository : IMessageIdSerializationRepository, IStreamEncoderFactory
{
    private readonly PQHeartbeatSerializer hbSerializer;
    private readonly PQQuoteSerializer pqQuoteSerializer;

    private readonly Dictionary<uint, IMessageSerializer> pqSerializers;

    public PQServerSerializationRepository(PQFeedType feed)
    {
        pqQuoteSerializer
            = new PQQuoteSerializer(feed == PQFeedType.Snapshot ? UpdateStyle.FullSnapshot : UpdateStyle.Updates);
        hbSerializer = new PQHeartbeatSerializer();
        pqSerializers = new Dictionary<uint, IMessageSerializer>()
        {
            { 0u, pqQuoteSerializer }, { 1u, hbSerializer }
        };
    }

    public IMessageSerializer GetSerializer<Tm>(uint msgId) where Tm : class, IVersionedMessage, new() =>
        pqSerializers[msgId];

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<KeyValuePair<uint, IMessageSerializer>> GetEnumerator() => pqSerializers.GetEnumerator();

    public int RegisteredSerializerCount => pqSerializers.Count;

    public IMessageSerializer<T> MessageEncoder<T>(IBufferContext bufferContext, T message)
        where T : class, IVersionedMessage =>
        (IMessageSerializer<T>)pqSerializers[message.MessageId];

    public IMessageSerializer<T> MessageEncoder<T>(T message) where T : class, IVersionedMessage =>
        (IMessageSerializer<T>)pqSerializers[message.MessageId];

    public IMessageSerializer<T> MessageEncoder<T>(uint id) where T : class, IVersionedMessage =>
        (IMessageSerializer<T>)pqSerializers[id];

    public IMessageSerializer MessageEncoder(IBufferContext bufferContext, uint id) => pqSerializers[id];

    public IMessageSerializer MessageEncoder(uint id) => pqSerializers[id];

    public void RegisterMessageSerializer(uint id, IMessageSerializer messageSerializer)
    {
        pqSerializers[id] = messageSerializer;
    }

    public void UnregisterMessageSerializer(uint id)
    {
        pqSerializers.Remove(id);
    }

    public IMessageSerializer GetSerializer(uint msgId) => pqSerializers[msgId];
}
