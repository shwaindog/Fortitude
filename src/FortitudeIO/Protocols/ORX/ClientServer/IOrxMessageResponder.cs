#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeIO.Conversations;
using FortitudeIO.Protocols.ORX.Serialization;
using FortitudeIO.Protocols.ORX.Serialization.Deserialization;
using FortitudeIO.Transports;
using FortitudeIO.Transports.Network.State;

#endregion

namespace FortitudeIO.Protocols.ORX.ClientServer;

public interface IOrxMessageResponder : IConversationResponder
{
    IRecycler RecyclingFactory { get; }
    IOrxDeserializationRepository DeserializationRepository { get; }
    IOrxSerializationRepository SerializationRepository { get; }

    event Action Disconnecting;
    void Send(ISession client, IVersionedMessage message);
    void Send(IConversation client, IVersionedMessage message);
    void Send(ISocketSessionContext client, IVersionedMessage message);
}
