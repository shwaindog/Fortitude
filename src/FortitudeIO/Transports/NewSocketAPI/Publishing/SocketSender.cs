#region

using System.Net.Sockets;
using FortitudeCommon.AsyncProcessing;
using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.EventProcessing.Disruption.Rings;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Serialization;
using FortitudeIO.Transports.NewSocketAPI.Sockets;
using FortitudeIO.Transports.Sockets.SessionConnection;

#endregion

namespace FortitudeIO.Transports.NewSocketAPI.Publishing;

public interface ISocketSender : IConversationPublisher
{
    int Id { get; }
    bool SendActive { get; }
    void HandleSendError(string message, Exception exception);
}

public sealed class SocketSender : ISocketSender
{
    private static readonly IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(SocketSender));
    private readonly IDirectOSNetworkingApi directOSNetworkingApi;

    private readonly StaticRing<SocketSessionSender.SocketEncoder> encoders =
        new(1024,
            () => new SocketSessionSender.SocketEncoder(), false);

    private readonly byte[] sendBuffer;
    private readonly ISyncLock sendLock = new SpinLockLight();
    private readonly IMap<uint, IBinarySerializer> serializers = new LinkedListCache<uint, IBinarySerializer>();

    private readonly ISocketSessionContext socketSocketSessionContext;
    private volatile bool sendActive;
    private int sentCursor;
    private int toSendCursor;

    internal SocketSender(ISocketSessionContext socketSocketSessionContext)
    {
        Socket = socketSocketSessionContext.SocketConnection!.OSSocket;
        this.socketSocketSessionContext = socketSocketSessionContext;
        directOSNetworkingApi = socketSocketSessionContext.SocketFactories.NetworkingController!.DirectOSNetworkingApi;
        sendBuffer = new byte[Socket.SendBufferSize];
        sentCursor = toSendCursor = 0;
    }

    public IOSSocket Socket { get; set; }

    public int Id => socketSocketSessionContext.Id;

    public bool SendActive
    {
        get => sendActive;
        private set => sendActive = value;
    }

    public void RegisterSerializer(uint messageId, IBinarySerializer serializer)
    {
        if (!serializers.TryGetValue(messageId, out var binSerializer) || binSerializer == null)
            serializers.Add(messageId, serializer);
        else
            throw new Exception("Two different message types cannot be registered to the same Id");
    }

    public void Send(IVersionedMessage message)
    {
        Enqueue(message);
        sendLock.Acquire();
        try
        {
            if (!SendActive)
            {
                SendActive = true;
                socketSocketSessionContext.SocketDispatcher.Sender.AddToSendQueue(this);
            }
        }
        finally
        {
            sendLock.Release();
        }
    }

    public void Enqueue(IVersionedMessage message)
    {
        message.IncrementRefCount();
        sendLock.Acquire();
        try
        {
            var encoder = encoders.Claim();
            encoder.Message = message;
            encoder.Serializer = serializers[message.MessageId]!;
        }
        finally
        {
            sendLock.Release();
        }
    }

    public bool SendEnqueued()
    {
        while (encoders.Count > 0 || sentCursor < toSendCursor)
        {
            while (encoders.Count > 0)
            {
                SocketSessionSender.SocketEncoder encoder;
                sendLock.Acquire();
                try
                {
                    encoder = encoders.Peek();
                }
                finally
                {
                    sendLock.Release();
                }

                var writtenSize = encoder.Serializer!.Serialize(sendBuffer, toSendCursor, encoder.Message!);
                if (writtenSize < 0)
                {
                    if (toSendCursor == 0)
                        throw new Exception("Message could not be serialized or was too big for the buffer");
                    break;
                }

                encoder.Message = null!;
                encoder.Serializer = null!;
                sendLock.Acquire();
                try
                {
                    encoders.Clear(1);
                    SendActive = false;
                }
                finally
                {
                    sendLock.Release();
                }

                toSendCursor += writtenSize;
            }

            if (!Send()) return false;
        }

        return true;
    }

    public void HandleSendError(string message, Exception exception)
    {
        logger.Warn(
            $"Error trying to send for {socketSocketSessionContext.ConversationDescription} got {message} and {exception}");
    }

    public void UnregisterSerializer(uint msgId)
    {
        if (!serializers.TryGetValue(msgId, out var binSerializer) || binSerializer == null)
            throw new Exception("Message Type could not be matched with the provided Id");
        serializers.Remove(msgId);
    }

    private unsafe bool Send()
    {
        if (sentCursor == toSendCursor)
        {
            sentCursor = toSendCursor = 0;
            return true;
        }

        int sentSize;
        fixed (byte* ptr = sendBuffer)
        {
            var amtDataSent = toSendCursor - sentCursor;
            sentSize = directOSNetworkingApi.Send(Socket.Handle, ptr + sentCursor,
                amtDataSent, SocketFlags.None);
        }

        if (sentSize < 0)
            throw new Exception("Win32 error " +
                                directOSNetworkingApi.GetLastCallError() + " on send call");
        sentCursor += sentSize;
        if (sentCursor == toSendCursor)
        {
            sentCursor = toSendCursor = 0;
            return true;
        }

        return false;
    }
}
