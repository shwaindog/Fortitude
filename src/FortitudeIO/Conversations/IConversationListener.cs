#region

using FortitudeIO.Protocols.Serialization;

#endregion

namespace FortitudeIO.Conversations;

public interface IConversationListener
{
    IStreamDecoderFactory DecoderFactory { get; set; }
    IStreamDecoder? Decoder { get; set; }
}
