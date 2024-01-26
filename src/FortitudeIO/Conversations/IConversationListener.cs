#region

using FortitudeIO.Protocols.Serialization;

#endregion

namespace FortitudeIO.Conversations;

public interface IConversationListener
{
    IStreamDecoderFactory DecoderFactory { get; set; }
    IMessageStreamDecoder? Decoder { get; set; }
}
