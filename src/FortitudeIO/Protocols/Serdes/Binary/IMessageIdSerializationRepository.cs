namespace FortitudeIO.Protocols.Serdes.Binary;

public interface IMessageIdSerializationRepository
{
    IMessageSerializer? GetSerializer<TM>(uint msgId) where TM : class, IVersionedMessage, new();
}

public interface IMessageIdDeserializationRepository
{
    INotifyingMessageDeserializer<TM>? GetDeserializer<TM>(uint msgId) where TM : class, IVersionedMessage, new();
}
