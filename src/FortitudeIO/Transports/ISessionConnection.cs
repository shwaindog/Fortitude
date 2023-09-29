using System.Collections.Generic;
using FortitudeCommon.DataStructures.LinkedLists;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Serialization;

namespace FortitudeIO.Sockets
{
    public interface ISessionConnection : IDoublyLinkedListNode<ISessionConnection>, ISession
    {
        ISessionReceiver SessionReceiver { get; }
        ISessionSender SessionSender { get; }
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
}