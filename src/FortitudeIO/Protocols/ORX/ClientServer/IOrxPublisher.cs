#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeIO.Transports.Sockets.Publishing;

#endregion

namespace FortitudeIO.Protocols.ORX.ClientServer;

public interface IOrxPublisher : ITcpSocketPublisher, IBinaryStreamPublisher
{
    IRecycler RecyclingFactory { get; }
    IOrxSubscriber StreamFromSubscriber { get; }
    void RegisterSerializer<T>() where T : class, IVersionedMessage, new();
}
