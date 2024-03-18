#region

using FortitudeIO.Protocols.Serdes.Binary;

#endregion

namespace FortitudeIO.Conversations;

public interface IConversationListener
{
    IMessageStreamDecoder? Decoder { get; set; }
}
