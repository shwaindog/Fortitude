﻿#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Monitoring.Logging.Diagnostics.Performance;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Conversations;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Protocols.Serdes.Binary.Sockets;
using FortitudeIO.Transports.NewSocketAPI.Logging;
using FortitudeIO.Transports.NewSocketAPI.State;

#endregion

namespace FortitudeIO.Transports.NewSocketAPI.Receiving;

public interface ISocketReceiver : IStreamListener
{
    bool IsAcceptor { get; }
    bool ListenActive { get; set; }
    IntPtr SocketHandle { get; }
    bool ZeroBytesReadIsDisconnection { get; set; }
    bool Poll(ReadSocketBufferContext readSocketBufferContext);
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
    private DateTime lastReportOfHighDataOutbursts = DateTime.MinValue;
    private long numberOfMessages;
    private long totalMessageSize;

    public SocketReceiver(ISocketSessionContext socketSessionContext)
    {
        socket = socketSessionContext.SocketConnection!.OSSocket;
        directOSNetworkingApi = socketSessionContext.SocketFactoryResolver.NetworkingController!.DirectOSNetworkingApi;
        this.socketSessionContext = socketSessionContext;
        numberOfReceivesPerPoll = socketSessionContext.NetworkTopicConnectionConfig.NumberOfReceivesPerPoll;

        var socketUseDescriptionNoWhiteSpaces = this.socketSessionContext.Name.Replace(" ", "");
        receiveSocketCxLatencyTraceLoggerPool = PerfLoggingPoolFactory.Instance
            .GetLatencyTracingLoggerPool("Receive." + socketUseDescriptionNoWhiteSpaces,
                TimeSpan.FromMilliseconds(1), typeof(ISessionConnection));
        byteStreamLogger = FLoggerFactory.Instance.GetLogger("SocketByteDump." + socketUseDescriptionNoWhiteSpaces);
        byteStreamLogger.DefaultEnabled = false;

        socket.Blocking = false;

        receiveBuffer = new ReadWriteBuffer(new byte[socket.ReceiveBufferSize]);
        bufferSize = receiveBuffer.Buffer.Length;

        ZeroBytesReadIsDisconnection = true;
    }

    public bool ListenActive { get; set; }
    public IntPtr SocketHandle => socket.Handle;
    public bool ZeroBytesReadIsDisconnection { get; set; }

    public event Action? Accept;
    public bool IsAcceptor => Accept != null;

    public IMessageStreamDecoder? Decoder { get; set; }

    public bool Poll(ReadSocketBufferContext readSocketBufferContext)
    {
        if (Decoder == null) return true;
        var receivingTs = TimeContext.UtcNow;
        readSocketBufferContext.DispatchLatencyLogger?.Indent();
        var recvLen = PrepareBufferAndReceiveData(readSocketBufferContext.DispatchLatencyLogger);
        if (recvLen == 0)
            return !ZeroBytesReadIsDisconnection;
        if (receiveBuffer.UnreadBytesRemaining > LargeBufferSize
            && readSocketBufferContext.DetectTimestamp > lastReportOfHighDataOutbursts.AddMinutes(1))
        {
            lastReportOfHighDataOutbursts = readSocketBufferContext.DetectTimestamp;
            readSocketBufferContext.DispatchLatencyLogger?.Add("High outburst of incoming data received read ",
                receiveBuffer.UnreadBytesRemaining);
            if (readSocketBufferContext.DispatchLatencyLogger != null)
                readSocketBufferContext.DispatchLatencyLogger.WriteTrace = true;
        }

        readSocketBufferContext.ReceivingTimestamp = receivingTs;
        readSocketBufferContext.SessionContext = socketSessionContext;
        readSocketBufferContext.EncodedBuffer = receiveBuffer;
        if (Decoder.Process(readSocketBufferContext) <= 0)
        {
            readSocketBufferContext.DispatchLatencyLogger?.Add("Data detected but not decoded");
            if (readSocketBufferContext.DispatchLatencyLogger != null)
                readSocketBufferContext.DispatchLatencyLogger.WriteTrace = true;
        }

        readSocketBufferContext.DispatchLatencyLogger?.Dedent();
        readSocketBufferContext.DispatchLatencyLogger?.Dedent();
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
            , socketSessionContext.Name);
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
                if (!HasBouncedFeedAlready.TryGetValue(socketSessionContext.Name
                        , out _))
                {
                    HasBouncedFeedAlready.Add(socketSessionContext.Name, true);
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
                socketTraceLogger.Add("Recv return unexpected readsize", currentMessageLength);
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

    public override string ToString() =>
        $"SocketReceiver({nameof(socketSessionContext)}: {socketSessionContext}, {nameof(SocketHandle)}: {SocketHandle}, " +
        $"{nameof(ListenActive)}: {ListenActive}, {nameof(ZeroBytesReadIsDisconnection)}: {ZeroBytesReadIsDisconnection}, " +
        $"{nameof(IsAcceptor)}: {IsAcceptor}, {nameof(Decoder)}: {Decoder})";
}
