// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

#region

using System.Net.Sockets;
using FortitudeCommon.AsyncProcessing;
using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.DataStructures.MemoryPools.Buffers.ByteBuffers;
using FortitudeCommon.EventProcessing.Disruption.Rings;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeCommon.Serdes;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Conversations;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Transports.Network.Config;
using FortitudeIO.Transports.Network.State;

#endregion

namespace FortitudeIO.Transports.Network.Publishing;

public interface ISocketSender : IStreamPublisher
{
    IMessageSerializationRepository MessageSerializationRepository { get; }

    bool AttemptCloseOnSendComplete { get; set; }

    int       Id      { get; }
    string    Name    { get; }
    bool      CanSend { get; }
    IOSSocket Socket  { get; set; }

    void SetCloseReason(CloseReason closeReason, string? reasonText);
    void SendExpectSessionCloseMessageAndClose();
}

public sealed class SocketSender : ISocketSender
{
    private const ushort MaxUdpDatagramPayloadSize = 65_507;

    private readonly IDirectOSNetworkingApi directOSNetworkingApi;

    private readonly StaticRing<SocketEncoder> encoders =
        new(1024, () => new SocketEncoder(), false);

    private readonly IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(SocketSender));

    private readonly ISyncLock sendLock = new SpinLockLight();

    private readonly ISocketSessionContext socketSessionContext;

    private readonly MessageBufferContext writeBufferContext;

    private volatile bool sendActive;

    private CloseReason closeReason;
    private string?     closeReasonText;
    private bool        haveClosedSocket;
    private bool        haveSentExpectSessionCloseMessage;

    public SocketSender(ISocketSessionContext socketSessionContext, IMessageSerializationRepository messageSerializationRepository)
    {
        Socket                         = socketSessionContext.SocketConnection!.OSSocket;
        this.socketSessionContext      = socketSessionContext;
        MessageSerializationRepository = messageSerializationRepository;
        directOSNetworkingApi = socketSessionContext.SocketFactoryResolver.NetworkingController!
                                                    .DirectOSNetworkingApi;
        var circularReadWriteBuffer = new CircularReadWriteBuffer(new byte[Socket.SendBufferSize]);
        if (this.socketSessionContext.SocketConversationProtocol == SocketConversationProtocol.UdpPublisher)
        {
            circularReadWriteBuffer.EnforceCappedMessageSize = true;
            circularReadWriteBuffer.MaximumMessageSize       = MaxUdpDatagramPayloadSize;
        }
        writeBufferContext = new MessageBufferContext(circularReadWriteBuffer)
            { Direction = ContextDirection.Write };
    }

    public bool SendActive
    {
        get => sendActive;
        private set => sendActive = value;
    }

    public IOSSocket Socket { get; set; }

    public int Id => socketSessionContext.Id;

    public string Name => socketSessionContext.Name;

    public IMessageSerializationRepository MessageSerializationRepository { get; }

    public bool AttemptCloseOnSendComplete { get; set; }

    public bool CanSend => socketSessionContext.SocketConnection?.IsConnected == true;

    public void SetCloseReason(CloseReason closeReason, string? reasonText)
    {
        this.closeReason = closeReason;
        closeReasonText  = reasonText;
    }

    public void SendExpectSessionCloseMessageAndClose()
    {
        var canSendExpectClose = closeReason != CloseReason.RemoteDisconnecting && CanSend;
        if (canSendExpectClose && !haveSentExpectSessionCloseMessage)
        {
            AttemptCloseOnSendComplete        = true;
            haveSentExpectSessionCloseMessage = true;
            Send(new ExpectSessionCloseMessage(closeReason, closeReasonText));
        }
        else if (!haveClosedSocket)
        {
            haveClosedSocket = true;
            socketSessionContext.SetDisconnected();
        }
    }

    public void Send(IVersionedMessage message)
    {
        Enqueue(message);
        sendLock.Acquire();
        if (!CanSend)
        {
            logger.Warn("Can not send message on {0} as the session is not currently connected. {1}", socketSessionContext, message);
            return;
        }

        try
        {
            if (!SendActive)
            {
                SendActive = true;
                socketSessionContext.SocketDispatcher.Sender!.EnqueueSocketSender(this);
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
        var checkSerializer = MessageSerializationRepository.GetSerializer(message.MessageId)!;
        if (checkSerializer == null) throw new KeyNotFoundException($"Can not find MessageSerializer with id {message.MessageId}");
        sendLock.Acquire();
        try
        {
            var encoder = encoders.Claim();
            encoder.Message      = message;
            encoder.Serializer   = checkSerializer;
            encoder.AttemptCount = 1;
        }
        finally
        {
            sendLock.Release();
        }
    }

    public bool SendQueued()
    {
        var foundExpectSessionClose = false;
        if (!CanSend)
        {
            logger.Warn("Can not send message on {0} as the session is not currently connected.", socketSessionContext);
            return true;
        }

        try
        {
            while (encoders.Count > 0 || !writeBufferContext.EncodedBuffer!.AllRead)
            {
                while (encoders.Count > 0)
                {
                    SocketEncoder encoder;
                    sendLock.Acquire();
                    try
                    {
                        encoder = encoders.Peek()!;
                    }
                    finally
                    {
                        sendLock.Release();
                    }

                    var message = encoder.Message;
                    // logger.Info("Received {0}", message);
                    foundExpectSessionClose |= message is ExpectSessionCloseMessage;
                    encoder.Serializer!.Serialize(encoder.Message, writeBufferContext);
                    var writtenSize = writeBufferContext.LastWriteLength;

                    if (writtenSize <= 0)
                    {
                        if (writeBufferContext.EncodedBuffer!.AllRead || encoder.AttemptCount > 100)
                        {
                            logger.Error(
                                         "Message could not be serialized or was too big for the buffer or there was no bytes to serialize. Message {0}"
                                       , encoder.Message);
                            MoveNextEncoder(encoder);
                        }
                        else
                        {
                            encoder.AttemptCount++;
                            break;
                        }
                    }

                    if (message is IRecyclableObject recyclableObject) recyclableObject.DecrementRefCount();

                    MoveNextEncoder(encoder);
                }

                if (!WriteToSocketSucceeded()) return false;
            }
        }
        catch (SocketSendException sse)
        {
            if (!foundExpectSessionClose)
            {
                logger.Warn("Caught exception attempting to send messages to {0}. Got {1}", this, sse);
                throw;
            }

            return foundExpectSessionClose;
        }

        return true;
    }

    public void HandleSendError(string message, Exception exception)
    {
        logger.Warn(
                    $"Error trying to send for {socketSessionContext.Name} got {message} and {exception}");
    }

    private void MoveNextEncoder(SocketEncoder encoder)
    {
        encoder.Message    = null!;
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

    private unsafe bool WriteToSocketSucceeded()
    {
        using var fixedBuffer = writeBufferContext.EncodedBuffer!;
        if (fixedBuffer.AllRead) return false;
        int sentSize = 0;
        if (fixedBuffer is { EnforceCappedMessageSize: true, HasAnotherMessage: true })
        {
            while (fixedBuffer.HasAnotherMessage)
            {
                var nextMessage = fixedBuffer.PopNextMessage();

                var readStartPtr = fixedBuffer.ReadBuffer + nextMessage.BufferAdjustedMessageStart;
                var amtDataSent  = nextMessage.MessageSize;
                sentSize += directOSNetworkingApi.Send(Socket.Handle, readStartPtr, amtDataSent, SocketFlags.None);
            }
        }
        else
        {
            var readStartPtr = fixedBuffer.ReadBuffer + fixedBuffer.BufferRelativeReadCursor;
            var amtDataSent  = (int)fixedBuffer.UnreadBytesRemaining;
            sentSize = directOSNetworkingApi.Send(Socket.Handle, readStartPtr, amtDataSent, SocketFlags.None);
        }
        // logger.Info("Socket Sender has sent {0} bytes on {1}", sentSize, Thread.CurrentThread.Name);
        if (sentSize < 0)
            throw new SocketSendException("Win32 error " +
                                          directOSNetworkingApi.GetLastCallError() + " on send call", socketSessionContext);
        fixedBuffer.ReadCursor += sentSize;
        if (fixedBuffer.AllRead) fixedBuffer.SetAllRead();
        if (!fixedBuffer.HasStorageForBytes(512 * 1024)) fixedBuffer.TryHandleRemainingWriteBufferRunningLow();

        return sentSize > 0;
    }

    public override string ToString() =>
        $"SocketSender({nameof(socketSessionContext)}: {socketSessionContext}, " +
        $"{nameof(SendActive)}: {SendActive}, {nameof(Id)}: {Id})";

    internal sealed class SocketEncoder
    {
        public int                AttemptCount = 0;
        public IVersionedMessage  Message      = null!;
        public IMessageSerializer Serializer   = null!;
    }
}

public class SocketSendException : Exception
{
    public SocketSendException(string? message, ISocketSessionContext socketSessionContext) : base(message) =>
        SocketSessionContext = socketSessionContext;

    public ISocketSessionContext SocketSessionContext { get; }

    public override string ToString() => $"SocketSendException({base.ToString()}, {nameof(SocketSessionContext)}: {SocketSessionContext})";
}
