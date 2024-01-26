#region

using FortitudeIO.Protocols.Serdes.Binary;

#endregion

namespace FortitudeIO.Protocols.Serialization;

public interface IMessageStreamDecoder
{
    bool AddMessageDecoder(uint msgId, IMessageDeserializer deserializer);
    int Process(DispatchContext dispatchContext);
}
