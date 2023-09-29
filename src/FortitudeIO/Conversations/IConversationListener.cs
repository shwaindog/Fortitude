using FortitudeIO.Protocols.Serialization;

namespace FortitudeIO.Transports
{
    public interface IConversationListener
    {
        IStreamDecoderFactory DecoderFactory { get; set; }
        IStreamDecoder Decoder { get; set; }
    }
}