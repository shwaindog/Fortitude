#region

using FortitudeIO.Conversations;
using FortitudeIO.Protocols.ORX.Serdes.Deserialization;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Transports;
using FortitudeIO.Transports.Network.State;

#endregion

namespace FortitudeIO.Protocols.ORX.ClientServer;

public interface IOrxMessageResponder : IConversationResponder
{
    IOrxDeserializationRepository DeserializationRepository { get; }
    IMessageSerializationRepository SerializationRepository { get; }

    event Action Disconnecting;
    void Send(ISession client, IVersionedMessage message);
    void Send(IConversation client, IVersionedMessage message);
    void Send(ISocketSessionContext client, IVersionedMessage message);
}
