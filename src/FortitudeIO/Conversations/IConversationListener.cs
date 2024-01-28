#region

using FortitudeIO.Protocols.Serdes;
using FortitudeIO.Protocols.Serdes.Binary;

#endregion

namespace FortitudeIO.Conversations;

public interface IConversationListener
{
    IStreamDecoderFactory DecoderFactory { get; set; }
    IMessageStreamDecoder? Decoder { get; set; }
}
