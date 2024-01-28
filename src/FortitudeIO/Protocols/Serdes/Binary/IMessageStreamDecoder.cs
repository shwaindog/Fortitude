#region

using FortitudeIO.Protocols.Serdes.Binary.Sockets;

#endregion

namespace FortitudeIO.Protocols.Serdes.Binary;

public interface IMessageStreamDecoder
{
    bool AddMessageDecoder(uint msgId, IMessageDeserializer deserializer);
    int Process(ReadSocketBufferContext readSocketBufferContext);
}
