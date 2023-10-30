namespace FortitudeIO.Protocols.Serialization;

public interface IBinarySerializationFactory
{
    IBinarySerializer? GetSerializer<TM>(uint msgId) where TM : class, new();
}

public interface IBinaryDeserializationFactory
{
    ICallbackBinaryDeserializer<TM>? GetDeserializer<TM>(uint msgId) where TM : class, new();
}
