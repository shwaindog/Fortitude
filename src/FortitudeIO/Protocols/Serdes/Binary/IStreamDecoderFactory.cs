#region


#endregion

namespace FortitudeIO.Protocols.Serdes.Binary;

public interface IStreamDecoderFactory
{
    IMessageStreamDecoder Supply();
}
