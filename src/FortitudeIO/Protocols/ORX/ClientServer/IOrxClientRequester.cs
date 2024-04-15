#region

using FortitudeIO.Conversations;
using FortitudeIO.Protocols.ORX.Serdes.Deserialization;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Transports.Network.Conversations;

#endregion

namespace FortitudeIO.Protocols.ORX.ClientServer;

public interface IOrxClientRequester : IConversationRequester, ISocketConversation
{
    IOrxDeserializationRepository DeserializationRepository { get; }
    IMessageSerializationRepository SerializationRepository { get; }
}
