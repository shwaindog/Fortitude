namespace FortitudeCommon.Serdes;

public interface IDeserializer<out T>
{
    MarshalType MarshalType { get; }
    T? Deserialize(ISerdeContext readContext);
}
