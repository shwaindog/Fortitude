#region

using FortitudeIO.Protocols.Serdes.Binary.Sockets;

#endregion

namespace FortitudeIO.Protocols.Serdes.Binary;

public interface IMessageStreamDecoder
{
    IEnumerable<KeyValuePair<uint, IMessageDeserializer>> RegisteredDeserializers { get; }
    bool AddMessageDeserializer(uint msgId, IMessageDeserializer deserializer);
    int Process(ReadSocketBufferContext readSocketBufferContext);
}
