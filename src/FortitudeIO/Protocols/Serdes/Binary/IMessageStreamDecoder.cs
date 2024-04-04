#region

using FortitudeIO.Protocols.Serdes.Binary.Sockets;

#endregion

namespace FortitudeIO.Protocols.Serdes.Binary;

public interface IMessageStreamDecoder
{
    IMessageDeserializationRepository MessageDeserializationRepository { get; }
    int Process(SocketBufferReadContext socketBufferReadContext);
}
