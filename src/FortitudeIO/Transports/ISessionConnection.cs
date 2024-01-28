#region

using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Protocols.Serdes.Binary.Sockets;

#endregion

namespace FortitudeIO.Transports;

public interface ISessionConnection : IDoublyLinkedListNode<ISessionConnection>, ISession
{
    ISessionReceiver? SessionReceiver { get; }
    ISessionSender? SessionSender { get; }
}

public interface ISessionSender
{
    void Enqueue(IVersionedMessage message, IMessageSerializer serializer);
    bool SendData();
}

public interface ISessionReceiver
{
    void SetFeedDecoder(IMessageStreamDecoder addThisMessageStreamDecoder);
    bool ReceiveData(ReadSocketBufferContext readSocketBufferContext);
}
