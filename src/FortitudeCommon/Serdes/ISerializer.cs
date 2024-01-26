namespace FortitudeCommon.Serdes;

public interface ISerializer<in T>
{
    MarshalType MarshalType { get; }
    void Serialize(T obj, ISerdeContext writeContext);
}
