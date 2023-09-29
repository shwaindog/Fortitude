namespace FortitudeIO.Protocols.Serialization
{
    public interface IBinaryDeserializer
    {
        object Deserialize(DispatchContext dispatchContext);
    }
}