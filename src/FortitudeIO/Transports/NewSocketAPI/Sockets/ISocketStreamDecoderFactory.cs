#region

using FortitudeIO.Protocols.Serialization;

#endregion

namespace FortitudeIO.Transports.NewSocketAPI.Sockets;

public interface ISocketStreamDecoderFactory : IStreamDecoderFactory
{
    new IMessageStreamDecoder Supply();
}
