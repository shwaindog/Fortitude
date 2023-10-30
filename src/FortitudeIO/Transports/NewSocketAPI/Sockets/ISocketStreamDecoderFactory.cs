using FortitudeIO.Protocols.Serialization;

namespace FortitudeIO.Transports.NewSocketAPI.Sockets
{
    public interface ISocketStreamDecoderFactory : IStreamDecoderFactory
    {
        new IStreamDecoder Supply();
    }
}