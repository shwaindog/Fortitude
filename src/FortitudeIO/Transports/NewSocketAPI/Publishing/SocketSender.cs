#region

using System.Net.Sockets;
using FortitudeCommon.AsyncProcessing;
using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.EventProcessing.Disruption.Rings;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeCommon.Serdes;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Conversations;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Transports.NewSocketAPI.Sockets;

#endregion

namespace FortitudeIO.Transports.NewSocketAPI.Publishing;

public interface ISocketSender : IStreamPublisher
{
    int Id { get; }
    bool SendActive { get; }
    IEnumerable<KeyValuePair<uint, IMessageSerializer>> RegisteredSerializers { get; }
    void HandleSendError(string message, Exception exception);
}

public sealed class SocketSender : ISocketSender
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(SocketSender));
    private readonly IDirectOSNetworkingApi directOSNetworkingApi;

    private readonly StaticRing<SocketEncoder> encoders =
        new(1024, () => new SocketEncoder(), false);

    private readonly ISyncLock sendLock = new SpinLockLight();
    private readonly IMap<uint, IMessageSerializer> serializers = new LinkedListCache<uint, IMessageSerializer>();

    private readonly ISocketSessionContext socketSocketSessionContext;

    private readonly BufferContext writeBufferContext;
    private volatile bool sendActive;
    private int sentCursor;

    public SocketSender(ISocketSessionContext socketSocketSessionContext)
    {
        Socket = socketSocketSessionContext.SocketConnection!.OSSocket;
        this.socketSocketSessionContext = socketSocketSessionContext;
        directOSNetworkingApi = socketSocketSessionContext.SocketFactories.NetworkingController!.DirectOSNetworkingApi;
        writeBufferContext = new BufferContext(new ReadWriteBuffer(new byte[Socket.SendBufferSize]))
            { Direction = ContextDirection.Write };
        sentCursor = 0;
    }

    public IOSSocket Socket { get; set; }

    public int Id => socketSocketSessionContext.Id;

    public bool SendActive
    {
        get => sendActive;
        private set => sendActive = value;
    }

    public void RegisterSerializer(uint messageId, IMessageSerializer serializer)
    {
        if (!serializers.TryGetValue(messageId, out var binSerializer) || binSerializer == null)
            serializers.Add(messageId, serializer);
        else
            throw new Exception("Two different message types cannot be registered to the same Id");
    }

    public IEnumerable<KeyValuePair<uint, IMessageSerializer>> RegisteredSerializers => serializers;

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
        while (encoders.Count > 0 || sentCursor < writeBufferContext.EncodedBuffer!.WrittenCursor)
        {
            while (encoders.Count > 0)
            {
                SocketEncoder encoder;
                sendLock.Acquire();
                try
                {
                    encoder = encoders.Peek();
                }
                finally
                {
                    sendLock.Release();
                }

                encoder.Serializer!.Serialize(encoder.Message, writeBufferContext);
                var writtenSize = writeBufferContext.LastWriteLength;
                if (writtenSize < 0)
                {
                    if (writeBufferContext.EncodedBuffer!.WrittenCursor == 0)
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
            }

            if (!Send()) return false;
        }

        return true;
    }

    public void HandleSendError(string message, Exception exception)
    {
        Logger.Warn(
            $"Error trying to send for {socketSocketSessionContext.Name} got {message} and {exception}");
    }

    public void UnregisterSerializer(uint msgId)
    {
        if (!serializers.TryGetValue(msgId, out var binSerializer) || binSerializer == null)
            throw new Exception("Message Type could not be matched with the provided Id");
        serializers.Remove(msgId);
    }

    private unsafe bool Send()
    {
        if (sentCursor == writeBufferContext.EncodedBuffer!.WrittenCursor)
        {
            sentCursor = writeBufferContext.EncodedBuffer!.WrittenCursor = 0;
            return true;
        }

        int sentSize;
        fixed (byte* ptr = writeBufferContext.EncodedBuffer.Buffer)
        {
            var amtDataSent = writeBufferContext.EncodedBuffer!.WrittenCursor - sentCursor;
            sentSize = directOSNetworkingApi.Send(Socket.Handle, ptr + sentCursor,
                amtDataSent, SocketFlags.None);
        }

        if (sentSize < 0)
            throw new Exception("Win32 error " +
                                directOSNetworkingApi.GetLastCallError() + " on send call");
        sentCursor += sentSize;
        if (sentCursor == writeBufferContext.EncodedBuffer!.WrittenCursor)
        {
            sentCursor = writeBufferContext.EncodedBuffer!.WrittenCursor = 0;
            return true;
        }

        return false;
    }

    public override string ToString() =>
        $"SocketSender({nameof(socketSocketSessionContext)}: {socketSocketSessionContext}, " +
        $"{nameof(SendActive)}: {SendActive}, {nameof(Id)}: {Id})";

    internal sealed class SocketEncoder
    {
        public IVersionedMessage Message = null!;
        public IMessageSerializer Serializer = null!;
    }
}
