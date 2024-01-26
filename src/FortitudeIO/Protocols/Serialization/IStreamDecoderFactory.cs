namespace FortitudeIO.Protocols.Serialization;

public interface IStreamDecoderFactory
{
    IMessageStreamDecoder Supply();
}
