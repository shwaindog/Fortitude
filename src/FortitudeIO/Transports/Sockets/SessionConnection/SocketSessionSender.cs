#region

using System.Net.Sockets;
using FortitudeCommon.AsyncProcessing;
using FortitudeCommon.EventProcessing.Disruption.Rings;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeCommon.Serdes;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Serdes.Binary;

#endregion

namespace FortitudeIO.Transports.Sockets.SessionConnection;

public sealed class SocketSessionSender : SocketSessionConnectionBase, ISocketSessionSender
{
    private readonly StaticRing<SocketEncoder> encoders = new(1024,
        () => new SocketEncoder(), false);

    private readonly ISyncLock sendLock = new SpinLockLight();

    private readonly BufferContext writeBufferContext;
    private IFLogger logger;
    private int sentCursor;

    internal SocketSessionSender(IOSSocket socket, IDirectOSNetworkingApi directOSNetworkingApi,
        string sessionDescription)
        : base(socket, directOSNetworkingApi, sessionDescription)
    {
        writeBufferContext = new BufferContext(new ReadWriteBuffer(new byte[Socket.SendBufferSize]))
            { Direction = ContextDirection.Write };
        sentCursor = 0;
        logger = FLoggerFactory.Instance.GetLogger(typeof(SocketSessionSender).FullName + "-" + sessionDescription);
    }

    public void Enqueue(IVersionedMessage message, IMessageSerializer serializer)
    {
        message.IncrementRefCount();
        sendLock.Acquire();
        try
        {
            var encoder = encoders.Claim();
            encoder.Message = message;
            encoder.Serializer = serializer;
        }
        finally
        {
            sendLock.Release();
        }
    }

    public bool SendData()
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

                // logger.Debug("Sent {0}", encoder.Message.ToString());
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

    private unsafe bool Send()
    {
        if (sentCursor == writeBufferContext.EncodedBuffer!.WrittenCursor)
        {
            sentCursor = writeBufferContext.EncodedBuffer!.WrittenCursor = 0;
            return true;
        }

        int sentSize;
        fixed (byte* ptr = writeBufferContext.EncodedBuffer!.Buffer)
        {
            sentSize = DirectOSNetworkingApi.Send(Handle, ptr + sentCursor,
                writeBufferContext.EncodedBuffer!.WrittenCursor - sentCursor, SocketFlags.None);
        }

        if (sentSize < 0)
            throw new Exception("Win32 error " +
                                DirectOSNetworkingApi.GetLastCallError() + " on send call");
        sentCursor += sentSize;
        if (sentCursor == writeBufferContext.EncodedBuffer!.WrittenCursor)
        {
            sentCursor = writeBufferContext.EncodedBuffer!.WrittenCursor = 0;
            return true;
        }

        return false;
    }

    internal sealed class SocketEncoder
    {
        public IVersionedMessage Message = null!;
        public IMessageSerializer Serializer = null!;
    }
}
