#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeIO.Conversations;
using FortitudeIO.Protocols.ORX.Serialization.Deserialization;
using FortitudeIO.Transports;
using FortitudeIO.Transports.NewSocketAPI.Sockets;
using FortitudeIO.Transports.Sockets.Publishing;

#endregion

namespace FortitudeIO.Protocols.ORX.ClientServer;

public interface IOrxPublisher : IConversationResponder
{
    IRecycler RecyclingFactory { get; }
    IOrxDeserializationRepository DeserializationRepository { get; }
    event Action Disconnecting;
    void RegisterSerializer<T>() where T : class, IVersionedMessage, new();
    void Send(ISession client, IVersionedMessage message);
    void Send(IConversation client, IVersionedMessage message);
    void Send(ISocketSessionContext client, IVersionedMessage message);
}

public interface IClientOrxPublisher : ITcpSocketPublisher, IBinaryStreamPublisher
{
    IRecycler RecyclingFactory { get; }
    IOrxSubscriber StreamFromSubscriber { get; }
    void RegisterSerializer<T>() where T : class, IVersionedMessage, new();
    void Send(ISession client, IVersionedMessage message);
    void Send(ISocketSessionContext client, IVersionedMessage message);
}
