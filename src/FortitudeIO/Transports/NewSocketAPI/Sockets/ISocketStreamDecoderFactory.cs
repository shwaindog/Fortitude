#region

using FortitudeIO.Protocols.Serdes;
using FortitudeIO.Protocols.Serdes.Binary;

#endregion

namespace FortitudeIO.Transports.NewSocketAPI.Sockets;

public interface ISocketStreamDecoderFactory : IStreamDecoderFactory
{
    new IMessageStreamDecoder Supply();
}
