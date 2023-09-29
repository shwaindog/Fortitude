using System;
using FortitudeIO.Protocols.ORX.Serialization.ObjectRecycling;
using FortitudeIO.Sockets;
using FortitudeIO.Transports.Sockets.Subscription;

namespace FortitudeIO.Protocols.ORX.ClientServer
{
    public interface IOrxSubscriber : ISocketSubscriber
    {
        OrxRecyclingFactory RecyclingFactory { get; }
        void RegisterDeserializer<T>(Action<T, object, ISession> msgHandler) where T : class, IVersionedMessage, new();
        new IOrxPublisher StreamToPublisher { get; }
    }
}