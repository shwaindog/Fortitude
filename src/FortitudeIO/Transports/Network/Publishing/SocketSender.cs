﻿#region

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
using FortitudeIO.Transports.Network.State;

#endregion

namespace FortitudeIO.Transports.Network.Publishing;

public interface ISocketSender : IStreamPublisher
{
    int Id { get; }
    bool SendActive { get; }
    IEnumerable<KeyValuePair<uint, IMessageSerializer>> RegisteredSerializers { get; }
    IOSSocket Socket { get; set; }
    void HandleSendError(string message, Exception exception);
}

public sealed class SocketSender : ISocketSender
{
    private readonly IDirectOSNetworkingApi directOSNetworkingApi;

    private readonly StaticRing<SocketEncoder> encoders =
        new(1024, () => new SocketEncoder(), false);

    private readonly IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(SocketSender));

    private readonly ISyncLock sendLock = new SpinLockLight();
    private readonly IMap<uint, IMessageSerializer> serializers = new LinkedListCache<uint, IMessageSerializer>();

    private readonly ISocketSessionContext socketSocketSessionContext;

    private readonly BufferContext writeBufferContext;
    private volatile bool sendActive;

    public SocketSender(ISocketSessionContext socketSocketSessionContext)
    {
        Socket = socketSocketSessionContext.SocketConnection!.OSSocket;
        this.socketSocketSessionContext = socketSocketSessionContext;
        directOSNetworkingApi = socketSocketSessionContext.SocketFactoryResolver.NetworkingController!
            .DirectOSNetworkingApi;
        writeBufferContext = new BufferContext(new ReadWriteBuffer(new byte[Socket.SendBufferSize]))
            { Direction = ContextDirection.Write };
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
            encoder.AttemptCount = 1;
        }
        finally
        {
            sendLock.Release();
        }
    }

    public bool SendQueued()
    {
        while (encoders.Count > 0 || !writeBufferContext.EncodedBuffer!.AllRead)
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

                if (writtenSize <= 0)
                {
                    if (writeBufferContext.EncodedBuffer!.AllRead || encoder.AttemptCount > 100)
                        throw new SocketSendException("Message could not be serialized or was too big for the buffer", socketSocketSessionContext);
                    encoder.AttemptCount++;
                    break;
                }

                MoveNextEncoder(encoder);
            }

            if (!WriteToSocketSucceeded()) return false;
        }

        return true;
    }

    public void HandleSendError(string message, Exception exception)
    {
        logger.Warn(
            $"Error trying to send for {socketSocketSessionContext.Name} got {message} and {exception}");
    }

    private void MoveNextEncoder(SocketEncoder encoder)
    {
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

    public void UnregisterSerializer(uint msgId)
    {
        if (!serializers.TryGetValue(msgId, out var binSerializer) || binSerializer == null)
            throw new Exception("Message Type could not be matched with the provided Id");
        serializers.Remove(msgId);
    }

    private unsafe bool WriteToSocketSucceeded()
    {
        if (writeBufferContext.EncodedBuffer!.AllRead) return false;
        int sentSize;
        fixed (byte* ptr = writeBufferContext.EncodedBuffer!.Buffer)
        {
            var amtDataSent = writeBufferContext.EncodedBuffer!.UnreadBytesRemaining;
            sentSize = directOSNetworkingApi.Send(Socket.Handle, ptr + writeBufferContext.EncodedBuffer!.ReadCursor,
                amtDataSent, SocketFlags.None);
        }

        if (sentSize < 0)
            throw new SocketSendException("Win32 error " +
                                          directOSNetworkingApi.GetLastCallError() + " on send call", socketSocketSessionContext);
        writeBufferContext.EncodedBuffer!.ReadCursor += sentSize;
        if (writeBufferContext.EncodedBuffer!.AllRead) writeBufferContext.EncodedBuffer.Reset();
        if (!writeBufferContext.EncodedBuffer.HasStorageForBytes(400)) writeBufferContext.EncodedBuffer.MoveUnreadToBufferStart();

        return sentSize > 0;
    }

    public override string ToString() =>
        $"SocketSender({nameof(socketSocketSessionContext)}: {socketSocketSessionContext}, " +
        $"{nameof(SendActive)}: {SendActive}, {nameof(Id)}: {Id})";

    internal sealed class SocketEncoder
    {
        public int AttemptCount = 0;
        public IVersionedMessage Message = null!;
        public IMessageSerializer Serializer = null!;
    }
}

public class SocketSendException : Exception
{
    public SocketSendException(string? message, ISocketSessionContext socketSessionContext) : base(message) =>
        SocketSessionContext = socketSessionContext;

    public ISocketSessionContext SocketSessionContext { get; }

    public override string ToString() => $"SocketSendException({base.ToString()}, {nameof(SocketSessionContext)}: {SocketSessionContext})";
}