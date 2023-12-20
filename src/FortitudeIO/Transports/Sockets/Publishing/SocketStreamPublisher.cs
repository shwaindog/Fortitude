#region

using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.Monitoring.Logging;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Serialization;
using FortitudeIO.Transports.Sockets.Dispatcher;
using FortitudeIO.Transports.Sockets.SessionConnection;
using FortitudeIO.Transports.Sockets.Subscription;

#endregion

namespace FortitudeIO.Transports.Sockets.Publishing;

public abstract class SocketStreamPublisher : ISocketLinkListener
{
    protected readonly ISocketDispatcher Dispatcher;
    protected readonly IFLogger Logger;

    private readonly IMap<uint, IBinarySerializer> serializers = new LinkedListCache<uint, IBinarySerializer>();

    protected SocketStreamPublisher(IFLogger logger, ISocketDispatcher dispatcher, string sessionDescription)
    {
        Logger = logger;
        Dispatcher = dispatcher;
        SessionDescription = sessionDescription;
    }

    public abstract int SendBufferSize { get; }

    public abstract IBinaryStreamSubscriber StreamFromSubscriber { get; }

    public void Unregister(ISocketSessionConnection sessionConnection)
    {
        Dispatcher.Listener.UnregisterForListen(sessionConnection);
    }

    public void StartMessaging()
    {
        Dispatcher.Start();
    }

    public void StopMessaging()
    {
        Dispatcher.Stop();
    }

    public string SessionDescription { get; }

    public void Enqueue(ISessionConnection cx, IVersionedMessage message)
    {
        if (serializers.TryGetValue(message.MessageId, out var binarySerializer))
        {
            cx.SessionSender!.Enqueue(message, binarySerializer!);
            Dispatcher.Sender.AddToSendQueue((ISocketSessionConnection)cx);
        }
        else
        {
            throw new Exception("Message serializer was not registered with the provided Id");
        }
    }

    public void Enqueue(IDoublyLinkedList<ISocketSessionConnection> cxs, IVersionedMessage message)
    {
        foreach (var cx in cxs) Enqueue(cx, message);
    }

    public void RegisterSerializer<Tm>(uint msgId) where Tm : class, new()
    {
        if (!serializers.TryGetValue(msgId, out var binSerializer) || binSerializer == null)
            serializers.Add(msgId, GetFactory().GetSerializer<Tm>(msgId)!);
        else
            throw new Exception("Two different message types cannot be registered to the same Id");
    }

    public void UnregisterSerializer(uint msgId)
    {
        if (!serializers.TryGetValue(msgId, out var binSerializer) || binSerializer == null)
            throw new Exception("Message Type could not be matched with the provided Id");
        serializers.Remove(msgId);
    }

    public int RegisteredSerializersCount => serializers.Count;
    public abstract IBinarySerializationFactory GetFactory();

    protected virtual IMap<uint, IBinaryDeserializer> CreateEmptySerialisesCache() =>
        new LinkedListCache<uint, IBinaryDeserializer>();
}
