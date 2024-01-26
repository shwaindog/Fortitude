namespace FortitudeCommon.Serdes;

internal class InOutCacheSerdes<T> : ISerdes<T, T>
{
    private readonly CachedItemSerde<T> previousSerializedSerde = new();
    public MarshalType MarshalType => MarshalType.Object | MarshalType.Cached;
    public ISerializer<T> Serializer => previousSerializedSerde;
    public IDeserializer<T> Deserializer => previousSerializedSerde;
}
