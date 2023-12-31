﻿#region

using System.Net.Sockets;
using FortitudeCommon.AsyncProcessing;
using FortitudeCommon.EventProcessing.Disruption.Rings;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Serialization;

#endregion

namespace FortitudeIO.Transports.Sockets.SessionConnection;

public sealed class SocketSessionSender : SocketSessionConnectionBase, ISocketSessionSender
{
    private readonly StaticRing<SocketEncoder> encoders = new(1024,
        () => new SocketEncoder(), false);

    private readonly byte[] sendBuffer;
    private readonly ISyncLock sendLock = new SpinLockLight();
    private IFLogger logger;
    private int sentCursor;
    private int toSendCursor;

    internal SocketSessionSender(IOSSocket socket, IDirectOSNetworkingApi directOSNetworkingApi,
        string sessionDescription)
        : base(socket, directOSNetworkingApi, sessionDescription)
    {
        sendBuffer = new byte[socket.SendBufferSize];
        sentCursor = toSendCursor = 0;
        logger = FLoggerFactory.Instance.GetLogger(typeof(SocketSessionSender).FullName + "-" + sessionDescription);
    }

    public void Enqueue(IVersionedMessage message, IBinarySerializer serializer)
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
        while (encoders.Count > 0 || sentCursor < toSendCursor)
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
                var writtenSize = encoder.Serializer.Serialize(sendBuffer, toSendCursor, encoder.Message);
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
            sentSize = DirectOSNetworkingApi.Send(Handle, ptr + sentCursor,
                toSendCursor - sentCursor, SocketFlags.None);
        }

        if (sentSize < 0)
            throw new Exception("Win32 error " +
                                DirectOSNetworkingApi.GetLastCallError() + " on send call");
        sentCursor += sentSize;
        if (sentCursor == toSendCursor)
        {
            sentCursor = toSendCursor = 0;
            return true;
        }

        return false;
    }

    internal sealed class SocketEncoder
    {
        public IVersionedMessage Message = null!;
        public IBinarySerializer Serializer = null!;
    }
}
