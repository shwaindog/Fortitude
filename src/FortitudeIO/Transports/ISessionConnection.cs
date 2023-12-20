#region

using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Serialization;

#endregion

namespace FortitudeIO.Transports;

public interface ISessionConnection : IDoublyLinkedListNode<ISessionConnection>, ISession
{
    ISessionReceiver? SessionReceiver { get; }
    ISessionSender? SessionSender { get; }
}

public interface ISessionSender
{
    void Enqueue(IVersionedMessage message, IBinarySerializer serializer);
    bool SendData();
}

public interface ISessionReceiver
{
    void SetFeedDecoder(IStreamDecoder addThisDecoder);
    bool ReceiveData(DispatchContext dispatchContext);
}
