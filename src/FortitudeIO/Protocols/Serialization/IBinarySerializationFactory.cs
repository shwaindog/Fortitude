#region

using FortitudeIO.Protocols.Serdes.Binary;

#endregion

namespace FortitudeIO.Protocols.Serialization;

public interface IBinarySerializationFactory
{
    IMessageSerializer? GetSerializer<TM>(uint msgId) where TM : class, IVersionedMessage, new();
}

public interface IBinaryDeserializationFactory
{
    ICallbackMessageDeserializer<TM>? GetDeserializer<TM>(uint msgId) where TM : class, IVersionedMessage, new();
}
