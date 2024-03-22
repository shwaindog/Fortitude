namespace FortitudeIO.Protocols.Serdes.Binary;

public interface IStreamDecoderFactory
{
    int RegisteredDeserializerCount { get; }
    IEnumerable<KeyValuePair<uint, IMessageDeserializer>> RegisteredDeserializers { get; }
    IMessageStreamDecoder Supply();
    void RegisterMessageDeserializer(uint id, IMessageDeserializer messageSerializer);
    void UnregisterMessageDeserializer(uint id);
}
