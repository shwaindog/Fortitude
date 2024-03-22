#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeIO.Transports;
using FortitudeIO.Transports.Sockets.Subscription;

#endregion

namespace FortitudeIO.Protocols.ORX.ClientServer;

public interface IOrxSubscriber : ISocketSubscriber
{
    IRecycler RecyclingFactory { get; }
    new IClientOrxPublisher StreamToPublisher { get; }
    void RegisterDeserializer<T>(Action<T, object?, ISession?>? msgHandler) where T : class, IVersionedMessage, new();
}
