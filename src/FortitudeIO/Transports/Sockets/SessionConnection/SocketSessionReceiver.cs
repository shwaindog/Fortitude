#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Monitoring.Logging.Diagnostics.Performance;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Protocols.Serdes.Binary.Sockets;
using FortitudeIO.Transports.Sockets.Logging;

#endregion

namespace FortitudeIO.Transports.Sockets.SessionConnection;

public class SocketSessionReceiver : SocketSessionConnectionBase, ISocketSessionReceiver
{
    private const int MaxUdpPacketSize = 65507;
    private const int LargeBufferSize = MaxUdpPacketSize / 4;
    private const double ReportFullThreshold = 0.7;
    private const int ReportEveryNthFullBufferBreach = 1000000;

    // ReSharper disable once FieldCanBeMadeReadOnly.Local
    private static IDictionary<string, bool> HasBouncedFeedAlready = new Dictionary<string, bool>();
    private readonly int bufferSize;
    private readonly IFLogger byteStreamLogger;
    private readonly ReadWriteBuffer receiveBuffer;

    private readonly IPerfLoggerPool receiveSocketCxLatencyTraceLoggerPool;
    private readonly int wholeMessagesPerReceive;
    private int bufferFullCounter;
    private IMessageStreamDecoder? decoder;
    private DateTime firstReadTime;
    private DateTime lastReportOfHighDataOutburts = DateTime.MinValue;
    private long numberOfMessages;
    private long totalMessageSize;

    internal SocketSessionReceiver(IOSSocket socket, IDirectOSNetworkingApi directOSNetworkingApi,
        Action<ISocketSessionConnection> acceptHandler, string sessionDescription)
        : this(socket, directOSNetworkingApi, (IMessageStreamDecoder?)null, sessionDescription) =>
        Accept = acceptHandler;

    public SocketSessionReceiver(IOSSocket socket, IDirectOSNetworkingApi directOSNetworkingApi,
        IMessageStreamDecoder? decoder, string sessionDescription, int wholeMessagesPerReceive = 1,
        bool zeroBytesReadIsDisconnection = true)
        : base(socket, directOSNetworkingApi, sessionDescription)
    {
        this.decoder = decoder;
        this.wholeMessagesPerReceive = wholeMessagesPerReceive > 0 ? wholeMessagesPerReceive : 1;

        var socketUseDescriptionNoWhiteSpaces = sessionDescription?.Replace(" ", "");
        receiveSocketCxLatencyTraceLoggerPool = PerfLoggingPoolFactory.Instance
            .GetLatencyTracingLoggerPool("Receive." + socketUseDescriptionNoWhiteSpaces,
                TimeSpan.FromMilliseconds(1), typeof(ISessionConnection));
        byteStreamLogger = FLoggerFactory.Instance.GetLogger("SocketByteDump." + socketUseDescriptionNoWhiteSpaces);
        byteStreamLogger.DefaultEnabled = false;

        socket.Blocking = false;

        receiveBuffer = new ReadWriteBuffer(new byte[socket.ReceiveBufferSize]);
        bufferSize = receiveBuffer.Buffer.Length;

        ZeroBytesReadIsDisconnection = zeroBytesReadIsDisconnection;
    }

    public bool ZeroBytesReadIsDisconnection { get; set; }

    public event Action<ISocketSessionConnection>? Accept;
    public bool IsAcceptor => Accept != null;


    public void SetFeedDecoder(IMessageStreamDecoder addThisMessageStreamDecoder)
    {
        decoder = addThisMessageStreamDecoder;
    }

    public bool ReceiveData(ReadSocketBufferContext readSocketBufferContext)
    {
        if (decoder == null) return true;
        var receivingTs = TimeContext.UtcNow;
        readSocketBufferContext.DispatchLatencyLogger?.Indent();
        var recvLen = PrepareBufferAndReceiveData(readSocketBufferContext.DispatchLatencyLogger);
        if (recvLen == 0)
            return !ZeroBytesReadIsDisconnection;
        if (receiveBuffer.UnreadBytesRemaining > LargeBufferSize
            && readSocketBufferContext.DetectTimestamp > lastReportOfHighDataOutburts.AddMinutes(1))
        {
            lastReportOfHighDataOutburts = readSocketBufferContext.DetectTimestamp;
            readSocketBufferContext.DispatchLatencyLogger?.Add("High outburst of incoming data received read ",
                receiveBuffer.UnreadBytesRemaining);
            if (readSocketBufferContext.DispatchLatencyLogger != null)
                readSocketBufferContext.DispatchLatencyLogger.WriteTrace = true;
        }

        readSocketBufferContext.ReceivingTimestamp = receivingTs;
        readSocketBufferContext.LegacySession = Parent;
        readSocketBufferContext.EncodedBuffer = receiveBuffer;
        if (decoder.Process(readSocketBufferContext) <= 0)
        {
            readSocketBufferContext.DispatchLatencyLogger?.Add("Data detected but not decoded");
            if (readSocketBufferContext.DispatchLatencyLogger != null)
                readSocketBufferContext.DispatchLatencyLogger.WriteTrace = true;
        }

        readSocketBufferContext.DispatchLatencyLogger?.Dedent();
        readSocketBufferContext.DispatchLatencyLogger?.Dedent();
        return true;
    }

    public IOSSocket AcceptClientSocketRequest() => Socket.Accept();

    public void OnAccept()
    {
        Accept?.Invoke(Parent!);
    }

    private int PrepareBufferAndReceiveData(IPerfLogger? detectionToPublishLatencyTraceLogger)
    {
        if (receiveBuffer.AllRead) receiveBuffer.Reset();
        if (!receiveBuffer.HasStorageForBytes(400)) receiveBuffer.MoveUnreadToBufferStart();

        var messageRecvLen = 0;
        detectionToPublishLatencyTraceLogger?.Add(SocketDataLatencyLogger.BeforeSocketRead, SessionDescription);
        var socketTraceLogger = receiveSocketCxLatencyTraceLoggerPool.StartNewTrace();
        try
        {
            messageRecvLen = GatherSocketDataSizeMetricsAndReceiveData(socketTraceLogger);
        }
        finally
        {
            detectionToPublishLatencyTraceLogger?.Add(SocketDataLatencyLogger.AfterSocketRead,
                (double)messageRecvLen);
            detectionToPublishLatencyTraceLogger?.Indent();
            receiveSocketCxLatencyTraceLoggerPool.StopTrace(socketTraceLogger);
        }

        if (messageRecvLen < 0)
            throw new Exception("Win32 error " + DirectOSNetworkingApi.GetLastCallError() + " on recv call");
        receiveBuffer.WriteCursor += messageRecvLen;
        return messageRecvLen;
    }

    private unsafe int GatherSocketDataSizeMetricsAndReceiveData(IPerfLogger socketTraceLogger)
    {
        int messageRecvLen;
        var bufferRecvLen = 0;
        var availableLocalBuffer = receiveBuffer.RemainingStorage;
        fixed (byte* ptr = receiveBuffer.Buffer)
        {
            socketTraceLogger.Add("before ioctlsocket");
            if (DirectOSNetworkingApi.IoCtlSocket(Handle, ref bufferRecvLen) != 0)
                throw new Exception("Win32 error " + DirectOSNetworkingApi.GetLastCallError()
                                                   + " on ioctlsocket call");

            socketTraceLogger.AddContextMeasurement(bufferRecvLen);
            if (socketTraceLogger.Enabled)
                if (bufferRecvLen > bufferSize * ReportFullThreshold
                    && bufferFullCounter++ % ReportEveryNthFullBufferBreach == 0)
                    TraceSocketDataStats(socketTraceLogger);

            socketTraceLogger.Add("end recv bufferRecvLen", bufferRecvLen);
            var remainingDataInSocketBuffer = bufferRecvLen;

            if ((firstReadTime == default || firstReadTime.AddSeconds(2) > TimeContext.UtcNow) &&
                bufferRecvLen > bufferSize * 9.0D / 10.0D)
            {
                bool haveBouncedFeedOnceAlready;
                if (!HasBouncedFeedAlready.TryGetValue(SessionDescription, out haveBouncedFeedOnceAlready))
                {
                    HasBouncedFeedAlready.Add(SessionDescription, true);
                    throw new SocketBufferTooFullException(
                        "Socket too full to read efficiently on startup.  Closing socket so that it may be" +
                        " reopened whilst socket dispatch is running.");
                }
            }
            else if (firstReadTime == default)
            {
                firstReadTime = TimeContext.UtcNow;
            }

            messageRecvLen = MultipleReceiveData(ptr, availableLocalBuffer, remainingDataInSocketBuffer,
                socketTraceLogger);
            if (byteStreamLogger.Enabled)
                byteStreamLogger.Debug(BitConverter.ToString(receiveBuffer.Buffer, receiveBuffer.WriteCursor,
                    messageRecvLen));
            socketTraceLogger.Add("end recv messageRecvLen", messageRecvLen);
        }

        return messageRecvLen;
    }

    private unsafe int MultipleReceiveData(byte* ptr, int availableLocalBuffer, int remainingDataInSocketBuffer,
        IPerfLogger socketTraceLogger)
    {
        var messageRecvLen = 0;
        var i = 0;
        var lastReadWasPartial = false;
        do
        {
            var currentMessageLength = DirectOSNetworkingApi.Recv(Handle,
                ptr + receiveBuffer.WriteCursor + messageRecvLen,
                Math.Min(availableLocalBuffer, remainingDataInSocketBuffer),
                ref lastReadWasPartial);
            if (currentMessageLength <= 0)
            {
                socketTraceLogger.Add("WSARecvEx return unexpected readsize", currentMessageLength);
                socketTraceLogger.Add("ToReadCursor + messageRecvLen",
                    receiveBuffer.WriteCursor + messageRecvLen);
                socketTraceLogger.Add("availableBuffer", availableLocalBuffer);
                socketTraceLogger.WriteTrace = true;
                messageRecvLen = currentMessageLength;
                break;
            }

            messageRecvLen += currentMessageLength;
            remainingDataInSocketBuffer -= currentMessageLength;
            availableLocalBuffer -= currentMessageLength;
            totalMessageSize += currentMessageLength;
            numberOfMessages++;
            i++;
        } while (i < wholeMessagesPerReceive &&
                 availableLocalBuffer > 200 && remainingDataInSocketBuffer > 0);

        return messageRecvLen;
    }

    private void TraceSocketDataStats(IPerfLogger socketTraceLogger)
    {
        socketTraceLogger.WriteTrace = true;
        socketTraceLogger.Add("Number of times breaching threshold", bufferFullCounter - 1);
        try
        {
            numberOfMessages = numberOfMessages > 0 ? numberOfMessages : 1;
            socketTraceLogger.Add("Average message size is ", totalMessageSize / numberOfMessages);
        } // ReSharper disable once EmptyGeneralCatchClause
        catch { }
        finally
        {
            totalMessageSize = 0;
            numberOfMessages = 0;
        }
    }
}
