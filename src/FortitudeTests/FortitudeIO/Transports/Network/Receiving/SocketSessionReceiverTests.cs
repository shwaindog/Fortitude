#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Monitoring.Logging.Diagnostics;
using FortitudeCommon.Monitoring.Logging.Diagnostics.Performance;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeCommon.Serdes.Binary;
using FortitudeCommon.Types;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Protocols.Serdes.Binary.Sockets;
using FortitudeIO.Transports;
using FortitudeIO.Transports.Network.Config;
using FortitudeIO.Transports.Network.Construction;
using FortitudeIO.Transports.Network.Controls;
using FortitudeIO.Transports.Network.Receiving;
using FortitudeIO.Transports.Network.Sockets;
using FortitudeIO.Transports.Network.State;
using FortitudeTests.FortitudeCommon.OSWrapper.NetworkingWrappers;
using Moq;

#endregion

namespace FortitudeTests.FortitudeIO.Transports.Network.Receiving;

[TestClass]
public class SocketReceiverTests
{
    private const string SessionName = "Socket Receiver Tests";
    private const int NumberOfReceivesPerPoll = 2;
    private const int LargeSocketReceiveBufferSize = 1024 * 1024 * 2;
    private const int SmallSocketReceiveBufferSize = byte.MaxValue * 2;
    private readonly int socketHandle = 123456;
    private bool acceptorHandlerHasBeenCalled;
    private byte[] defaultSocketDataWritten = null!;
    private DirectOSNetworkingStub directOsNetworkingApiStub = null!;
    private Func<int> getLastErrorCallback = null!;
    private Mock<IFLogger> moqDataDumpFlogger = null!;
    private Mock<IPerfLogger> moqDispatchPerfLogger = null!;
    private Mock<IMessageStreamDecoder> moqFeedDecoder = null!;
    private Mock<IFLoggerFactory> moqFloggerFactory = null!;
    private Mock<IOSNetworkingController> moqNetworkingController = null!;
    private Mock<INetworkTopicConnectionConfig> moqNetworkTopicConConfig = null!;
    private Mock<IOSSocket> moqOsSocket = null!;
    private Mock<IPerfLoggerPool> moqPerfLoggerPool = null!;
    private Mock<IPerfLoggingPoolFactory> moqPerfPoolFac = null!;
    private Mock<IPerfLogger> moqReadSocketPerfLogger = null!;
    private Mock<ISocketConnection> moqSocketConnection = null!;
    private Mock<ISocketFactoryResolver> moqSocketFactoryResolver = null!;
    private Mock<ISocketSessionContext> moqSocketSessionContext = null!;
    private Mock<IStreamControls> moqStreamControls = null!;
    private Mock<IFLogger> moqTextFlogger = null!;
    private SocketBufferReadContext socketBufferReadContext = null!;
    private IntPtr socketHandlePtr;
    private SocketReceiver socketReceiver = null!;

    [TestInitialize]
    public void SetUp()
    {
        HierarchicalConfigurationUpdater.Instance.DiagnosticsUpdate("flogger",
            new KeyValuePair<string, string?>("SocketByteDump", "on"), UpdateType.Removed);
        defaultSocketDataWritten = new byte[]
            { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 };
        moqOsSocket = new Mock<IOSSocket>();
        socketHandlePtr = new IntPtr(socketHandle);
        getLastErrorCallback = () => 20;

        directOsNetworkingApiStub = new DirectOSNetworkingStub(getLastErrorCallback, null);
        moqPerfPoolFac = new Mock<IPerfLoggingPoolFactory>();
        moqPerfLoggerPool = new Mock<IPerfLoggerPool>();
        moqSocketConnection = new Mock<ISocketConnection>();
        moqSocketFactoryResolver = new Mock<ISocketFactoryResolver>();
        moqReadSocketPerfLogger = new Mock<IPerfLogger>();
        moqDispatchPerfLogger = new Mock<IPerfLogger>();
        moqSocketSessionContext = new Mock<ISocketSessionContext>();
        moqStreamControls = new Mock<IStreamControls>();
        moqNetworkingController = new Mock<IOSNetworkingController>();
        moqNetworkTopicConConfig = new Mock<INetworkTopicConnectionConfig>();
        moqFloggerFactory = new Mock<IFLoggerFactory>();
        moqTextFlogger = new Mock<IFLogger>();
        moqDataDumpFlogger = new Mock<IFLogger>();

        moqOsSocket.SetupGet(oss => oss.Handle).Returns(socketHandlePtr).Verifiable();
        moqSocketConnection.SetupGet(sc => sc.OSSocket).Returns(moqOsSocket.Object);
        moqOsSocket.SetupGet(sc => sc.ReceiveBufferSize).Returns(SmallSocketReceiveBufferSize);
        moqSocketFactoryResolver.SetupGet(sfr => sfr.NetworkingController).Returns(moqNetworkingController.Object);
        moqNetworkingController.SetupGet(nc => nc.DirectOSNetworkingApi).Returns(directOsNetworkingApiStub);

        moqNetworkTopicConConfig.SetupGet(nc => nc.NumberOfReceivesPerPoll).Returns(NumberOfReceivesPerPoll);

        moqSocketSessionContext.SetupGet(ssc => ssc.SocketConnection).Returns(moqSocketConnection.Object);
        moqSocketSessionContext.SetupGet(ssc => ssc.SocketFactoryResolver).Returns(moqSocketFactoryResolver.Object);
        moqSocketSessionContext.SetupGet(ssc => ssc.Name).Returns(SessionName);
        moqSocketSessionContext.SetupGet(ssc => ssc.NetworkTopicConnectionConfig).Returns(moqNetworkTopicConConfig.Object);
        moqSocketSessionContext.SetupGet(ssc => ssc.StreamControls).Returns(moqStreamControls.Object);

        moqPerfPoolFac.Setup(ltcspf => ltcspf.GetLatencyTracingLoggerPool(
                "Receive.SocketReceiverTests", TimeSpan.FromMilliseconds(1), typeof(ISession)))
            .Returns(moqPerfLoggerPool.Object).Verifiable();

        moqFloggerFactory.Setup(ff => ff.GetLogger(It.IsAny<Type>())).Returns(moqTextFlogger.Object);
        moqFloggerFactory.Setup(ff => ff.GetLogger(It.IsAny<string>())).Returns(moqDataDumpFlogger.Object);

        PerfLoggingPoolFactory.Instance = moqPerfPoolFac.Object;
        FLoggerFactory.Instance = moqFloggerFactory.Object;

        moqFeedDecoder = new Mock<IMessageStreamDecoder>();
        socketReceiver = new SocketReceiver(moqSocketSessionContext.Object)
        {
            Decoder = moqFeedDecoder.Object, ListenActive = true
        };

        moqReadSocketPerfLogger.Setup(ltcsl => ltcsl.Add(It.IsAny<string>(), It.IsAny<int>()))
            .Verifiable();
        socketBufferReadContext = new SocketBufferReadContext
        {
            DetectTimestamp = new DateTime(2017, 05, 15, 20, 33, 55)
            , DispatchLatencyLogger = moqDispatchPerfLogger.Object
        };
    }

    [TestCleanup]
    public void TearDown()
    {
        PerfLoggingPoolFactory.Instance = new PerfLoggingPoolFactory();
        TimeContext.Provider = new HighPrecisionTimeContext();
        FLoggerFactory.Instance = new FLoggerFactory();
    }

    [TestMethod]
    public void SocketReceiverAsAcceptor_New_PropertiesSetupAsExpected()
    {
        var acceptorHandler = () => { };
        socketReceiver.Accept += acceptorHandler;
        socketReceiver.ZeroBytesReadIsDisconnection = false; //Set by TCPAcceptorControls

        Assert.IsTrue(socketReceiver.IsAcceptor);
        Assert.AreEqual(moqOsSocket.Object.Handle, socketReceiver.SocketHandle);
        Assert.IsFalse(socketReceiver.ZeroBytesReadIsDisconnection);
        moqPerfPoolFac.Verify();
    }

    [TestMethod]
    public void SocketReceiverAsListener_New_PropertiesSetupAsExpected()
    {
        Assert.IsFalse(socketReceiver.IsAcceptor);
        Assert.AreEqual(moqOsSocket.Object.Handle, socketReceiver.SocketHandle);
        Assert.IsTrue(socketReceiver.ZeroBytesReadIsDisconnection);
        moqPerfPoolFac.Verify();
    }

    [TestMethod]
    public void SocketReceiverAsAcceptor_NewClientSocketRequest_InvokesAccept()
    {
        acceptorHandlerHasBeenCalled = false;
        var acceptorHandler = () => { acceptorHandlerHasBeenCalled = true; };
        socketReceiver.Accept += acceptorHandler;
        socketReceiver.ZeroBytesReadIsDisconnection = false; //Set by TCPAcceptorControls

        socketReceiver.NewClientSocketRequest();

        Assert.IsTrue(acceptorHandlerHasBeenCalled);
    }

    [TestMethod]
    public void SocketReceiver_PollWithDataAndAvailableBuffer_CallsDecoderToReadBuffer()
    {
        PrepareSocketFullRead();
        moqFeedDecoder.Setup(fd => fd.Process(socketBufferReadContext)).Returns(defaultSocketDataWritten.Length)
            .Verifiable();
        var connected = socketReceiver.Poll(socketBufferReadContext);
        Assert.IsTrue(connected);
        moqDispatchPerfLogger.Verify();
        moqPerfLoggerPool.Verify();
        moqReadSocketPerfLogger.Verify();
        moqFeedDecoder.Verify();
    }

    [TestMethod]
    public void SocketReceiverZeroBytesReadIsDisconnectionFalse_PollNoData_ReturnsTrue()
    {
        PrepareSocketFullRead();
        socketReceiver.ZeroBytesReadIsDisconnection = false;
        directOsNetworkingApiStub.ClearQueue();
        directOsNetworkingApiStub.QueueResponseBytes(new byte[0], true);
        var connected = socketReceiver.Poll(socketBufferReadContext);
        Assert.IsTrue(connected);
    }

    [TestMethod]
    public void SocketReceiverNoDataAndZeroBytesReadIsDisconnectionTrue_Poll_ReturnsConnectedFalse()
    {
        PrepareSocketFullRead();
        directOsNetworkingApiStub.ClearQueue();
        directOsNetworkingApiStub.QueueResponseBytes(new byte[0], true);

        var dataReceived = socketReceiver.Poll(socketBufferReadContext);
        Assert.IsFalse(dataReceived);
    }

    [TestMethod]
    public void SocketReceiver_PollWithTooMuchData_OnlyReportsDataBurstEveryMinute()
    {
        const int unreadBytes = 16377;
        moqOsSocket.SetupGet(sc => sc.ReceiveBufferSize).Returns(LargeSocketReceiveBufferSize);
        PrepareSocketFullRead();
        moqDispatchPerfLogger.Setup(ltcsl => ltcsl.Add(It.IsAny<string>(), unreadBytes))
            .Verifiable();
        moqDispatchPerfLogger.SetupSet(ltcsl => ltcsl.WriteTrace = true).Verifiable();
        directOsNetworkingApiStub.ClearQueue();
        socketReceiver = new SocketReceiver(moqSocketSessionContext.Object)
        {
            Decoder = moqFeedDecoder.Object, ListenActive = true
        };

        CallReceiveDataAssertDataBurstAlertTimes(unreadBytes, 1, new DateTime(2017, 05, 14, 17, 40, 01));

        CallReceiveDataAssertDataBurstAlertTimes(unreadBytes, 1, new DateTime(2017, 05, 14, 17, 41, 00));

        CallReceiveDataAssertDataBurstAlertTimes(unreadBytes, 2, new DateTime(2017, 05, 14, 17, 41, 02));
    }

    [TestMethod]
    public void SocketReceiver_PollWithData_LogsTraceIfDecoderProcessesNothing()
    {
        PrepareSocketFullRead();
        moqFeedDecoder.Setup(fd => fd.Process(socketBufferReadContext)).Returns(0).Verifiable();

        socketReceiver.Poll(socketBufferReadContext);

        moqDispatchPerfLogger.Verify(ltcsl => ltcsl.Add(It.IsAny<string>()),
            Times.Once);
        moqDispatchPerfLogger.VerifySet(ltcsl => ltcsl.WriteTrace = true, Times.Once);
        moqFeedDecoder.Verify(fd => fd.Process(socketBufferReadContext), Times.Once);
    }

    [TestMethod]
    public void SocketReceiverUnreadBuffer_Poll_DoesNotResetBufferWhileUnreadRemains()
    {
        PrepareSocketFullRead();

        moqFeedDecoder.Setup(fd => fd.Process(socketBufferReadContext)).Returns(0).Verifiable();

        var receiveBuffer = NonPublicInvocator.GetInstanceField<ReadWriteBuffer>(
            socketReceiver, "receiveBuffer");
        AssertReceiveBufferIsExpected(receiveBuffer, 8, 25);
        AssertReceiveBufferIsExpected(receiveBuffer, 16, 26);
        AssertReceiveBufferIsExpected(receiveBuffer, 27, 27);
        AssertReceiveBufferIsExpected(receiveBuffer, 1, 1);
        AssertReceiveBufferIsExpected(receiveBuffer, 1, 1);
    }


    [TestMethod]
    public void SocketReceiverRemainingBufferLessThan400_Poll_ShiftsUnreadDataToBufferStart()
    {
        PrepareSocketFullRead();
        directOsNetworkingApiStub.ClearQueue();
        directOsNetworkingApiStub.QueueResponseBytes(new byte[401], true);
        var receiveBuffer = NonPublicInvocator.GetInstanceField<ReadWriteBuffer>(
            socketReceiver, "receiveBuffer");

        for (var i = 0; i < receiveBuffer.Buffer.Length; i++) receiveBuffer.Buffer[i] = (byte)(i % byte.MaxValue);
        receiveBuffer.ReadCursor = byte.MaxValue / 4;
        receiveBuffer.WriteCursor = 113;

        moqFeedDecoder.SetupSequence(fd => fd.Process(socketBufferReadContext)).Returns(0);

        var receiveData = socketReceiver.Poll(socketBufferReadContext);

        Assert.IsTrue(receiveData);
        for (var i = 0; i < 48; i++) Assert.AreEqual(i + byte.MaxValue / 4, receiveBuffer.Buffer[i]);
    }

    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public void SocketReceiver_PollWithIOCTLError_ThrowsExceptionWithLasCallError()
    {
        PrepareSocketFullRead();
        directOsNetworkingApiStub.ClearQueue();
        directOsNetworkingApiStub.QueueResponseBytes(null, true);
        socketReceiver.Poll(socketBufferReadContext);
        Assert.Fail("Should not reach here");
    }

    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public void SocketReceiver_PollWithRecvError_ThrowsExceptionWithLastCallError()
    {
        PrepareSocketFullRead();
        directOsNetworkingApiStub.ClearQueue();
        directOsNetworkingApiStub.QueueResponseBytes(null, false);
        try
        {
            socketReceiver.Poll(socketBufferReadContext);
        }
        catch (Exception)
        {
            moqReadSocketPerfLogger.Verify(ltcsl => ltcsl.Add(It.IsAny<string>(), It.IsAny<int>()),
                Times.Exactly(5));
            moqReadSocketPerfLogger.VerifySet(ltcsl => ltcsl.WriteTrace = true, Times.Once);
            throw;
        }

        Assert.Fail("Should not reach here");
    }

    [TestMethod]
    public void SocketReceiverWithSocketTraceEnabledNearFullBuffer_Poll_SetsTraceAndMessageSize()
    {
        moqReadSocketPerfLogger.SetupGet(ltcsl => ltcsl.Enabled).Returns(true).Verifiable();
        moqReadSocketPerfLogger.SetupSet(ltcsl => ltcsl.WriteTrace = true).Verifiable();
        moqReadSocketPerfLogger.Setup(ltcsl => ltcsl.Add(It.IsAny<string>(), It.IsAny<long>()))
            .Verifiable();
        PrepareSocketFullRead();
        directOsNetworkingApiStub.ClearQueue();
        directOsNetworkingApiStub.QueueResponseBytes(new byte[358], true);

        socketReceiver.Poll(socketBufferReadContext);

        moqReadSocketPerfLogger.Verify();
    }

    [TestMethod]
    public void SocketReceiverWithSocketTraceEnabledFullBuffer_PollAMillionTimes_SetsTraceAndMessageSize()
    {
        moqReadSocketPerfLogger.SetupGet(ltcsl => ltcsl.Enabled).Returns(true).Verifiable();
        moqReadSocketPerfLogger.SetupSet(ltcsl => ltcsl.WriteTrace = true).Verifiable();
        moqReadSocketPerfLogger.Setup(ltcsl => ltcsl.Add(It.IsAny<string>(), It.IsAny<long>()))
            .Verifiable();
        PrepareSocketFullRead();
        directOsNetworkingApiStub.ClearQueue();
        directOsNetworkingApiStub.QueueResponseBytes(new byte[358], true);

        NonPublicInvocator.SetInstanceField(socketReceiver, "bufferFullCounter", 999999);

        socketReceiver.Poll(socketBufferReadContext);

        moqReadSocketPerfLogger.Verify(ltcsl => ltcsl.Add(It.IsAny<string>(),
            It.IsAny<long>()), Times.Never);
        moqReadSocketPerfLogger.VerifySet(ltcsl => ltcsl.WriteTrace = true, Times.Never);


        directOsNetworkingApiStub.QueueResponseBytes(new byte[358], true);
        socketReceiver.Poll(socketBufferReadContext);
        moqReadSocketPerfLogger.Verify();
    }

    [TestMethod]
    public void SocketReceiverWithSocketTraceEnabledEmptyBuffer_ReceiveData_SetsTraceAndMessageSize()
    {
        moqReadSocketPerfLogger.SetupGet(ltcsl => ltcsl.Enabled).Returns(true).Verifiable();
        PrepareSocketFullRead();
        directOsNetworkingApiStub.ClearQueue();
        directOsNetworkingApiStub.QueueResponseBytes(new byte[356], true);

        socketReceiver.Poll(socketBufferReadContext);

        moqReadSocketPerfLogger.Verify(ltcsl => ltcsl.Add(It.IsAny<string>(),
            It.IsAny<long>()), Times.Never);
        moqReadSocketPerfLogger.VerifySet(ltcsl => ltcsl.WriteTrace = true, Times.Never);
    }

    [TestMethod]
    public void SocketReceiverSocketDumpEnable_ReceiveData_WriteBytesToLogger()
    {
        var moqFloggerFactory = new Mock<IFLoggerFactory>();
        var moqSocketByteDump = new Mock<IFLogger>();
        moqFloggerFactory.Setup(ff => ff.GetLogger("SocketByteDump.SocketReceiverTests"))
            .Returns(moqSocketByteDump.Object).Verifiable();
        try
        {
            FLoggerFactory.Instance = moqFloggerFactory.Object;
            moqSocketByteDump.SetupGet(fl => fl.Enabled).Returns(true).Verifiable();
            moqSocketByteDump.Setup(fl => fl.Debug(It.IsAny<string>())).Verifiable();
            PrepareSocketFullRead();
            directOsNetworkingApiStub.ClearQueue();
            directOsNetworkingApiStub.QueueResponseBytes(new byte[10], true);
            socketReceiver = new SocketReceiver(moqSocketSessionContext.Object)
            {
                Decoder = moqFeedDecoder.Object, ListenActive = true
            };

            var receiveData = socketReceiver.Poll(socketBufferReadContext);
            Assert.IsTrue(receiveData);

            moqSocketByteDump.Verify();
        }
        finally
        {
            FLoggerFactory.Instance = new FLoggerFactory();
        }
    }

    [TestMethod]
    public void SocketReceiver2NumberOfReceivesPerPollLimitBufferSize_Poll_RemovesExpectedNumMessages()
    {
        PrepareSocketFullRead();
        directOsNetworkingApiStub.ClearQueue();
        moqFeedDecoder.Setup(fd => fd.Process(socketBufferReadContext)).Returns(510)
            .Verifiable();
        directOsNetworkingApiStub.QueueResponseBytes(new byte[510], true);
        directOsNetworkingApiStub.QueueResponseBytes(new byte[510], true);
        directOsNetworkingApiStub.QueueResponseBytes(new byte[510], true);
        directOsNetworkingApiStub.QueueResponseBytes(new byte[510], true);
        Assert.AreEqual(4, directOsNetworkingApiStub.NumberQueuedMessages);
        var receiveData = socketReceiver.Poll(socketBufferReadContext);
        Assert.IsTrue(receiveData);

        Assert.AreEqual(3, directOsNetworkingApiStub.NumberQueuedMessages);
    }

    [TestMethod]
    public void SocketReceiver2NumberOfReceivesPerPoll_ReceiveData_RemovesExpectedNumMessages()
    {
        PrepareSocketFullRead();
        directOsNetworkingApiStub.QueueResponseBytes(new byte[10], true);
        directOsNetworkingApiStub.QueueResponseBytes(new byte[10], true);
        directOsNetworkingApiStub.QueueResponseBytes(new byte[10], true);
        Assert.AreEqual(4, directOsNetworkingApiStub.NumberQueuedMessages);
        var receiveData = socketReceiver.Poll(socketBufferReadContext);
        Assert.IsTrue(receiveData);

        Assert.AreEqual(2, directOsNetworkingApiStub.NumberQueuedMessages);
    }

    [TestMethod]
    public void SocketReceiverMultiWholeMsgsLmtRemainingBufferSz_ReceiveData_RemovesExpectedNumMessages()
    {
        PrepareSocketFullRead();
        moqOsSocket.SetupGet(oss => oss.ReceiveBufferSize).Returns(400).Verifiable();
        var moqTimeContext = new Mock<ITimeContext>();
        moqTimeContext.SetupGet(tc => tc.UtcNow).Returns(new DateTime(2017, 05, 15, 20, 33, 56));
        TimeContext.Provider = moqTimeContext.Object;
        socketReceiver.Poll(socketBufferReadContext);
        directOsNetworkingApiStub.QueueResponseBytes(new byte[100], true);
        directOsNetworkingApiStub.QueueResponseBytes(new byte[100], true);
        directOsNetworkingApiStub.QueueResponseBytes(new byte[100], true);
        directOsNetworkingApiStub.QueueResponseBytes(new byte[100], true);
        Assert.AreEqual(4, directOsNetworkingApiStub.NumberQueuedMessages);
        moqTimeContext.SetupGet(tc => tc.UtcNow).Returns(new DateTime(2017, 05, 15, 20, 33, 58));
        var receiveData = socketReceiver.Poll(socketBufferReadContext);
        Assert.IsTrue(receiveData);

        Assert.AreEqual(2, directOsNetworkingApiStub.NumberQueuedMessages);
    }

    [TestMethod]
    public void SocketReceiver4NumberOfReceivesPerPoll_ReceiveData_RemovesAllMessages()
    {
        PrepareSocketFullRead();
        directOsNetworkingApiStub.QueueResponseBytes(new byte[10], true);
        directOsNetworkingApiStub.QueueResponseBytes(new byte[10], true);
        directOsNetworkingApiStub.QueueResponseBytes(new byte[10], true);
        Assert.AreEqual(4, directOsNetworkingApiStub.NumberQueuedMessages);
        moqNetworkTopicConConfig.Setup(ntcc => ntcc.NumberOfReceivesPerPoll).Returns(4);
        socketReceiver = new SocketReceiver(moqSocketSessionContext.Object)
        {
            Decoder = moqFeedDecoder.Object, ListenActive = true
        };
        var receiveData = socketReceiver.Poll(socketBufferReadContext);
        Assert.IsTrue(receiveData);

        Assert.AreEqual(0, directOsNetworkingApiStub.NumberQueuedMessages);
    }

    [TestMethod]
    public void SocketReceiver_HandleReceiveError_CallsOnSessionFailure()
    {
        moqTextFlogger.Setup(fl => fl.Warn("Receive Error - {0} got on {1}", It.IsAny<object[]>())).Verifiable();
        moqStreamControls.Setup(sc => sc.OnSessionFailure(It.IsAny<string>())).Verifiable();

        socketReceiver.HandleReceiveError("read error.", new Exception("Something bad happened!"));

        moqTextFlogger.Verify();
        moqStreamControls.Verify();
    }

    private void AssertReceiveBufferIsExpected(ReadWriteBuffer receiveBuffer, int expectedRead, int expectedWritten)
    {
        directOsNetworkingApiStub.QueueResponseBytes(new byte[1], true);
        moqFeedDecoder.Setup(fd => fd.Process(socketBufferReadContext)).Callback(() => { receiveBuffer.ReadCursor = expectedRead; });

        socketReceiver.Poll(socketBufferReadContext);
        Assert.AreEqual(expectedRead, receiveBuffer.ReadCursor);
        Assert.AreEqual(expectedWritten, receiveBuffer.WriteCursor);
    }

    private void CallReceiveDataAssertDataBurstAlertTimes(int unreadBytes, int numTimesExpected,
        DateTime socketWakeTime)
    {
        socketBufferReadContext.DetectTimestamp = socketWakeTime;
        moqFeedDecoder.Setup(fd => fd.Process(socketBufferReadContext))
            .Callback(() => { socketBufferReadContext.EncodedBuffer!.ReadCursor = unreadBytes; }).Returns(unreadBytes)
            .Verifiable();
        directOsNetworkingApiStub.QueueResponseBytes(new byte[unreadBytes], true);
        socketReceiver.Poll(socketBufferReadContext);
        moqDispatchPerfLogger.Verify(ltcsl => ltcsl.Add(It.IsAny<string>(), unreadBytes),
            Times.Exactly(numTimesExpected));
        moqDispatchPerfLogger.VerifySet(ltcsl => ltcsl.WriteTrace = true,
            Times.Exactly(numTimesExpected));
    }

    private void PrepareSocketFullRead()
    {
        PrepareReceiveDataMocks();
        PrepareReceiveMocks();
        PrepareReadDataMocks();
    }

    private void PrepareReceiveDataMocks()
    {
        moqDispatchPerfLogger.Setup(ltcsl => ltcsl.Indent()).Verifiable();
        moqDispatchPerfLogger.Setup(ltcsl => ltcsl.Dedent()).Verifiable();
        moqFeedDecoder.Setup(fd => fd.Process(socketBufferReadContext)).Returns(defaultSocketDataWritten.Length)
            .Verifiable();
    }

    private void PrepareReceiveMocks()
    {
        moqPerfLoggerPool.Setup(ltcslp => ltcslp.StartNewTrace())
            .Returns(moqReadSocketPerfLogger.Object).Verifiable();
        moqPerfLoggerPool.Setup(ltcslp => ltcslp.StopTrace(moqReadSocketPerfLogger.Object))
            .Verifiable();
    }

    private void PrepareReadDataMocks()
    {
        moqReadSocketPerfLogger.Setup(ltcsl => ltcsl.Add(It.IsAny<string>())).Verifiable();
        directOsNetworkingApiStub.QueueResponseBytes(defaultSocketDataWritten, true);
        moqReadSocketPerfLogger.Setup(ltcsl => ltcsl.AddContextMeasurement(It.IsAny<double>()))
            .Verifiable();
        moqReadSocketPerfLogger.Setup(ltcsl => ltcsl.Add(It.IsAny<string>(), It.IsAny<int>()))
            .Verifiable();
    }
}
