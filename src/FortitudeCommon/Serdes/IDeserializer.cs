namespace FortitudeCommon.Serdes;

public interface IDeserializedObjectSink<in T>
{
    void Sink(T deserializedObj, ISerdeContext socketConversation, object? header = null);
}

public interface IDeserializer<out T>
{
    MarshalType MarshalType { get; }
    T? Deserialize(ISerdeContext readContext);
}

public interface IDeserializerPublisher<T> : IDeserializer<T>
{
    IList<IDeserializedObjectSink<T>> PublishSinks { get; }
    void DeserializeAndPublish(ISerdeContext readContext);
}
