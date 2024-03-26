#region

using FortitudeIO.Protocols.Serdes.Binary;

#endregion

namespace FortitudeIO.Conversations;

public interface IStreamListener
{
    IMessageStreamDecoder? Decoder { get; set; }
}
