namespace FortitudeCommon.Serdes;

internal class InOutCacheSerdes<T> : ISerdes<T, T>
{
    private readonly CachedItemSerde<T> previousSerializedSerde = new();
    public Type SerializesType => typeof(T);
    public Type DeserializesType => typeof(T);
    public MarshalType MarshalType => MarshalType.Object | MarshalType.Cached;
    public ISerializer<T> Serializer => previousSerializedSerde;
    public IDeserializer<T> Deserializer => previousSerializedSerde;
}
