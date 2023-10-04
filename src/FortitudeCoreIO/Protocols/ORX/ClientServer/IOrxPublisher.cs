using FortitudeIO.Protocols.ORX.Serialization.ObjectRecycling;
using FortitudeIO.Transports.Sockets.Publishing;

namespace FortitudeIO.Protocols.ORX.ClientServer
{
    public interface IOrxPublisher : ITcpSocketPublisher
    {
        OrxRecyclingFactory RecyclingFactory { get; }
        void RegisterSerializer<T>() where T : class, IVersionedMessage, new();
        IOrxSubscriber StreamFromSubscriber { get; }
    }
}