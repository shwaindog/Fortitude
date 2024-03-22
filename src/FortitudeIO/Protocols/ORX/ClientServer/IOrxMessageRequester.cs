#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeIO.Conversations;
using FortitudeIO.Protocols.ORX.Serialization;
using FortitudeIO.Protocols.ORX.Serialization.Deserialization;
using FortitudeIO.Transports.NewSocketAPI.Sockets;

#endregion

namespace FortitudeIO.Protocols.ORX.ClientServer;

public interface IOrxMessageRequester : IConversationRequester, ISocketConversation
{
    IRecycler RecyclingFactory { get; }
    IOrxDeserializationRepository DeserializationRepository { get; }
    IOrxSerializationRepository SerializationRepository { get; }
}
