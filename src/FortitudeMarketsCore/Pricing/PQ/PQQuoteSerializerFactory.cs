#region

using System.Collections;
using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.Quotes.SourceTickerInfo;
using FortitudeMarketsCore.Pricing.PQ.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Serialization;
using FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization;
using FortitudeMarketsCore.Pricing.PQ.Subscription;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ;

public sealed class PQQuoteSerializerRepository : IPQQuoteSerializerRepository
{
    private readonly IMap<uint, PQDeserializerBase> deserializers =
        new LinkedListCache<uint, PQDeserializerBase>();

    private readonly Dictionary<uint, IMessageSerializer> registeredSerializers = new()
    {
        { 0, new PQSnapshotIdsRequestSerializer() }
    };

    public bool HasPictureDeserializers => deserializers.Count > 0;

    public IPQDeserializer CreateQuoteDeserializer<T>(ISourceTickerClientAndPublicationConfig streamPubConfig)
        where T : PQLevel0Quote, new()
    {
        PQDeserializerBase quoteDeserializer = new PQQuoteDeserializer<T>(streamPubConfig);
        deserializers.Add(streamPubConfig.Id, quoteDeserializer);
        return quoteDeserializer;
    }

    public IPQDeserializer? GetQuoteDeserializer(IUniqueSourceTickerIdentifier identifier) =>
        deserializers.TryGetValue(identifier.Id, out var quoteDeserializer) ? quoteDeserializer : null;

    public void RemoveQuoteDeserializer(IUniqueSourceTickerIdentifier identifier)
    {
        deserializers.Remove(identifier.Id);
    }

    public ICallbackMessageDeserializer<T>? GetDeserializer<T>(uint msgId) where T : class, IVersionedMessage, new()
    {
        if (typeof(T) == typeof(PQLevel0Quote))
            if (deserializers.TryGetValue(msgId, out var quoteDeserializer))
                return quoteDeserializer as ICallbackMessageDeserializer<T>;
        throw new NotSupportedException();
    }

    public IMessageSerializer? GetSerializer<T>(uint msgId) where T : class, IVersionedMessage, new()
    {
        if (typeof(T) == typeof(PQSnapshotClient.SnapShotStreamPublisher)) return new PQSnapshotIdsRequestSerializer();
        throw new NotSupportedException();
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<KeyValuePair<uint, IMessageSerializer>> GetEnumerator() => registeredSerializers.GetEnumerator();

    public int RegisteredSerializerCount => registeredSerializers.Count;

    public IMessageSerializer<T> MessageEncoder<T>(IBufferContext bufferContext, T message)
        where T : class, IVersionedMessage =>
        (IMessageSerializer<T>)registeredSerializers[message.MessageId];

    public IMessageSerializer<T> MessageEncoder<T>(T message) where T : class, IVersionedMessage =>
        (IMessageSerializer<T>)registeredSerializers[message.MessageId];

    public IMessageSerializer<T> MessageEncoder<T>(uint id) where T : class, IVersionedMessage =>
        (IMessageSerializer<T>)registeredSerializers[id];

    public IMessageSerializer MessageEncoder(IBufferContext bufferContext, uint id) => registeredSerializers[id];

    public IMessageSerializer MessageEncoder(uint id) => registeredSerializers[id];

    public void RegisterMessageSerializer(uint id, IMessageSerializer messageSerializer)
    {
        registeredSerializers[id] = messageSerializer;
    }

    public void UnregisterMessageSerializer(uint id)
    {
        registeredSerializers.Remove(id);
    }
}
