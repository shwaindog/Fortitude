#region

using FortitudeCommon.DataStructures.Memory;

#endregion

namespace FortitudeCommon.Serdes;

internal class ReusableObjectSerdes<T> : ISerdes<T, T> where T : class, IReusableObject, new()
{
    private readonly ReusableObjectItemSerde<T> previousSerializedSerde = new();
    public Type SerializesType => typeof(T);
    public Type DeserializesType => typeof(T);
    public MarshalType MarshalType => MarshalType.Object | MarshalType.Cached;
    public ISerializer<T> Serializer => previousSerializedSerde;
    public IDeserializer<T> Deserializer => previousSerializedSerde;
}
