#region

using FortitudeCommon.Serdes;
using FortitudeCommon.Serdes.Binary;

#endregion

namespace FortitudeIO.Protocols.Serdes.Binary;

public interface IMessageSerializer
{
    void Serialize(IVersionedMessage message, IBufferContext writeContext);
}

public interface IMessageSerializer<T> : IMessageSerializer, ISerializer<T>
    where T : class, IVersionedMessage { }
