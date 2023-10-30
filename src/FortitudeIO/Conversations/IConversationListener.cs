#region

using FortitudeIO.Protocols.Serialization;

#endregion

namespace FortitudeIO.Transports;

public interface IConversationListener
{
    IStreamDecoderFactory DecoderFactory { get; set; }
    IStreamDecoder? Decoder { get; set; }
}
