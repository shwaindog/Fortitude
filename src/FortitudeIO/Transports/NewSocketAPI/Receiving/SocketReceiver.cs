#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Monitoring.Logging.Diagnostics.Performance;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Conversations;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Protocols.Serialization;
using FortitudeIO.Transports.NewSocketAPI.Sockets;
using FortitudeIO.Transports.Sockets;
using FortitudeIO.Transports.Sockets.Logging;

#endregion

namespace FortitudeIO.Transports.NewSocketAPI.Receiving;

public interface ISocketReceiver : IConversationListener
{
    bool IsAcceptor { get; }
    bool ListenActive { get; set; }
    IntPtr SocketHandle { get; }
    bool ZeroBytesReadIsDisconnection { get; set; }
    bool Poll(DispatchContext dispatchContext);
    event Action? Accept;
    void HandleReceiveError(string message, Exception exception);
    IOSSocket AcceptClientSocketRequest();
    void NewClientSocketRequest();
}

public sealed class SocketReceiver : ISocketReceiver
{
    private const int MaxUdpPacketSize = 65507;
    private const int LargeBufferSize = MaxUdpPacketSize / 4;
    private const double ReportFullThreshold = 0.7;
    private const int ReportEveryNthFullBufferBreach = 1000000;
    private static readonly IDictionary<string, bool> HasBouncedFeedAlready = new Dictionary<string, bool>();
    private readonly int bufferSize;
    private readonly IFLogger byteStreamLogger;
    private readonly IDirectOSNetworkingApi directOSNetworkingApi;
    private readonly int numberOfReceivesPerPoll;
    private readonly ReadWriteBuffer receiveBuffer;

    private readonly IPerfLoggerPool receiveSocketCxLatencyTraceLoggerPool;
    private readonly IOSSocket socket;
    private readonly ISocketSessionContext socketSessionContext;
    private int bufferFullCounter;
    private DateTime firstReadTime;
    private DateTime lastReportOfHighDataOutburts = DateTime.MinValue;
    private long numberOfMessages;
    private long totalMessageSize;

    public SocketReceiver(ISocketSessionContext socketSessionContext, int numberOfReceivesPerPoll = 1,
        bool zeroBytesReadIsDisconnection = true)
    {
        socket = socketSessionContext.SocketConnection!.OSSocket;
        directOSNetworkingApi = socketSessionContext.SocketFactories.NetworkingController!.DirectOSNetworkingApi;
        this.socketSessionContext = socketSessionContext;
        this.numberOfReceivesPerPoll = numberOfReceivesPerPoll;

        var socketUseDescriptionNoWhiteSpaces = this.socketSessionContext.ConversationDescription.Replace(" ", "");
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

    public bool ListenActive { get; set; }
    public IntPtr SocketHandle => socket.Handle;
    public bool ZeroBytesReadIsDisconnection { get; set; } = true;

    public event Action? Accept;
    public bool IsAcceptor => Accept != null;

    public IStreamDecoderFactory DecoderFactory
    {
        get => socketSessionContext.SerdesFactory.StreamDecoderFactory;
        set
        {
            socketSessionContext.SerdesFactory.StreamDecoderFactory = value;
            if (value != null && Decoder != null) Decoder = value.Supply();
        }
    }

    public IMessageStreamDecoder? Decoder { get; set; }

    public bool Poll(DispatchContext dispatchContext)
    {
        if (Decoder == null) return true;
        var receivingTs = TimeContext.UtcNow;
        dispatchContext.DispatchLatencyLogger?.Indent();
        var recvLen = PrepareBufferAndReceiveData(dispatchContext.DispatchLatencyLogger);
        if (recvLen == 0)
            return !ZeroBytesReadIsDisconnection;
        if (receiveBuffer.UnreadBytesRemaining > LargeBufferSize
            && dispatchContext.DetectTimestamp > lastReportOfHighDataOutburts.AddMinutes(1))
        {
            lastReportOfHighDataOutburts = dispatchContext.DetectTimestamp;
            dispatchContext.DispatchLatencyLogger?.Add("High outburst of incoming data received read ",
                receiveBuffer.UnreadBytesRemaining);
            if (dispatchContext.DispatchLatencyLogger != null) dispatchContext.DispatchLatencyLogger.WriteTrace = true;
        }

        dispatchContext.ReceivingTimestamp = receivingTs;
        dispatchContext.Conversation = socketSessionContext;
        dispatchContext.EncodedBuffer = receiveBuffer;
        if (Decoder.Process(dispatchContext) <= 0)
        {
            dispatchContext.DispatchLatencyLogger?.Add("Data detected but not decoded");
            if (dispatchContext.DispatchLatencyLogger != null) dispatchContext.DispatchLatencyLogger.WriteTrace = true;
        }

        dispatchContext.DispatchLatencyLogger?.Dedent();
        dispatchContext.DispatchLatencyLogger?.Dedent();
        return true;
    }

    public IOSSocket AcceptClientSocketRequest() => socket.Accept();

    public void NewClientSocketRequest()
    {
        Accept?.Invoke();
    }

    public void HandleReceiveError(string message, Exception exception) { }

    private int PrepareBufferAndReceiveData(IPerfLogger? detectionToPublishLatencyTraceLogger)
    {
        if (receiveBuffer.AllRead) receiveBuffer.Reset();
        if (!receiveBuffer.HasStorageForBytes(400)) receiveBuffer.MoveUnreadToBufferStart();

        var messageRecvLen = 0;
        detectionToPublishLatencyTraceLogger?.Add(SocketDataLatencyLogger.BeforeSocketRead
            , socketSessionContext.ConversationDescription);
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
            throw new Exception("Win32 error " + directOSNetworkingApi.GetLastCallError() + " on recv call");
        receiveBuffer.WrittenCursor += messageRecvLen;
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
            if (directOSNetworkingApi.IoCtlSocket(socket.Handle, ref bufferRecvLen) != 0)
                throw new Exception("Win32 error " + directOSNetworkingApi.GetLastCallError()
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
                if (!HasBouncedFeedAlready.TryGetValue(socketSessionContext.ConversationDescription
                        , out haveBouncedFeedOnceAlready))
                {
                    HasBouncedFeedAlready.Add(socketSessionContext.ConversationDescription, true);
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
                byteStreamLogger.Debug(BitConverter.ToString(receiveBuffer.Buffer, receiveBuffer.WrittenCursor,
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
            var currentMessageLength = directOSNetworkingApi.Recv(socket.Handle,
                ptr + receiveBuffer.WrittenCursor + messageRecvLen,
                Math.Min(availableLocalBuffer, remainingDataInSocketBuffer),
                ref lastReadWasPartial);
            if (currentMessageLength <= 0)
            {
                socketTraceLogger.Add("WSARecvEx return unexpected readsize", currentMessageLength);
                socketTraceLogger.Add("ToReadCursor + messageRecvLen",
                    receiveBuffer.WrittenCursor + messageRecvLen);
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
        } while (i < numberOfReceivesPerPoll &&
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
