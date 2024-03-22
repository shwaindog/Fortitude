#region

using FortitudeIO.Protocols.Serdes.Binary;

#endregion

namespace FortitudeIO.Conversations;

public interface IListener
{
    IMessageStreamDecoder? Decoder { get; set; }
}
