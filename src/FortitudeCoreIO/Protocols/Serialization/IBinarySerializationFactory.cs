namespace FortitudeIO.Protocols.Serialization
{
    public interface IBinarySerializationFactory
    {
        IBinarySerializer GetSerializer<Tm>(uint msgId) where Tm : class;
    }

    public interface IBinaryDeserializationFactory
    {
        ICallbackBinaryDeserializer<Tm> GetDeserializer<Tm>(uint msgId) where Tm : class;
    }
}
