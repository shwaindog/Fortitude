#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Monitoring.Logging.Diagnostics;
using FortitudeCommon.Monitoring.Logging.Diagnostics.Performance;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeCommon.Types;
using FortitudeIO.Protocols.Serialization;
using FortitudeIO.Transports;
using FortitudeIO.Transports.Sockets;
using FortitudeIO.Transports.Sockets.SessionConnection;
using FortitudeTests.FortitudeCommon.OSWrapper.NetworkingWrappers;
using Moq;

#endregion

namespace FortitudeTests.FortitudeIO.Transports.Sockets.SessionConnection;

[TestClass]
public class SocketSessionReceiverTests
{
    private readonly int socketHandle = 123456;
    private bool acceptorHandlerHasBeenCalled;
    private byte[] defaultSocketDataWritten = null!;
    private DirectOSNetworkingStub directOsNetworkingApiStub = null!;
    private DispatchContext dispatchContext = null!;
    private Func<int> getLastErrorCallback = null!;
    private Mock<IPerfLogger> moqDispatchPerfLogger = null!;
    private Mock<IStreamDecoder> moqFeedDecoder = null!;
    private Mock<IOSSocket> moqOsSocket = null!;
    private Mock<IPerfLoggerPool> moqPerfLoggerPool = null!;
    private Mock<IPerfLoggingPoolFactory> moqPerfPoolFac = null!;
    private Mock<IPerfLogger> moqReadSocketPerfLogger = null!;
    private Mock<ISocketSessionConnection> moqSocketSessionConnection = null!;
    private IntPtr socketHandlePtr;
    private SocketSessionReceiver socketSessionReceiverAsDataReceiver = null!;
    private string testSessionDescription = null!;

    [TestInitialize]
    public void SetUp()
    {
        HierarchicalConfigurationUpdater.Instance.DiagnosticsUpdate("flogger",
            new KeyValuePair<string, string?>("SocketByteDump", "on"), UpdateType.Removed);
        defaultSocketDataWritten = new byte[]
            { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 };
        moqOsSocket = new Mock<IOSSocket>();
        socketHandlePtr = new IntPtr(socketHandle);
        moqOsSocket.SetupGet(oss => oss.Handle).Returns(socketHandlePtr).Verifiable();
        moqOsSocket.SetupGet(oss => oss.ReceiveBufferSize).Returns(1024 * 1024 * 2).Verifiable();
        getLastErrorCallback = () => 20;

        directOsNetworkingApiStub = new DirectOSNetworkingStub(getLastErrorCallback, null);
        moqPerfPoolFac = new Mock<IPerfLoggingPoolFactory>();
        moqPerfLoggerPool = new Mock<IPerfLoggerPool>();
        moqReadSocketPerfLogger = new Mock<IPerfLogger>();
        moqDispatchPerfLogger = new Mock<IPerfLogger>();

        moqPerfPoolFac.Setup(ltcspf => ltcspf.GetLatencyTracingLoggerPool(
                "Receive.TestSessionDescription", TimeSpan.FromMilliseconds(1), typeof(ISessionConnection)))
            .Returns(moqPerfLoggerPool.Object).Verifiable();

        PerfLoggingPoolFactory.Instance = moqPerfPoolFac.Object;

        testSessionDescription = "Test Session Description";
        moqFeedDecoder = new Mock<IStreamDecoder>();
        socketSessionReceiverAsDataReceiver = new SocketSessionReceiver(moqOsSocket.Object,
            directOsNetworkingApiStub, moqFeedDecoder.Object, testSessionDescription);

        moqSocketSessionConnection = new Mock<ISocketSessionConnection>();
        socketSessionReceiverAsDataReceiver.Parent = moqSocketSessionConnection.Object;

        moqReadSocketPerfLogger.Setup(ltcsl => ltcsl.Add(It.IsAny<string>(), It.IsAny<int>()))
            .Verifiable();
        dispatchContext = new DispatchContext
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
    }

    [TestMethod]
    public void SessionReceiverAsAcceptor_New_PropertiesSetupAsExpected()
    {
        Action<ISocketSessionConnection> acceptorHandler = ssc => { };
        var socketSessionReceiverAsAcceptor = new SocketSessionReceiver(moqOsSocket.Object,
            directOsNetworkingApiStub, acceptorHandler, testSessionDescription)
        {
            Parent = moqSocketSessionConnection.Object
        };
        Assert.IsTrue(socketSessionReceiverAsAcceptor.IsAcceptor);
        Assert.AreEqual(moqOsSocket.Object, socketSessionReceiverAsAcceptor.Socket);
        Assert.AreEqual(testSessionDescription, socketSessionReceiverAsAcceptor.SessionDescription);
        Assert.IsTrue(socketSessionReceiverAsAcceptor.ZeroBytesReadIsDisconnection);
        Assert.AreEqual(moqSocketSessionConnection.Object, socketSessionReceiverAsAcceptor.Parent);
        Assert.AreEqual(socketHandlePtr, socketSessionReceiverAsAcceptor.Handle);
        moqPerfPoolFac.Verify();
    }

    [TestMethod]
    public void SessionReceiverAsReceiver_New_PropertiesSetupAsExpected()
    {
        Assert.IsFalse(socketSessionReceiverAsDataReceiver.IsAcceptor);
        Assert.AreEqual(moqOsSocket.Object, socketSessionReceiverAsDataReceiver.Socket);
        Assert.AreEqual(testSessionDescription, socketSessionReceiverAsDataReceiver.SessionDescription);
        Assert.IsTrue(socketSessionReceiverAsDataReceiver.ZeroBytesReadIsDisconnection);
        Assert.AreEqual(moqSocketSessionConnection.Object, socketSessionReceiverAsDataReceiver.Parent);
        Assert.AreEqual(socketHandlePtr, socketSessionReceiverAsDataReceiver.Handle);
        moqPerfPoolFac.Verify();
    }

    [TestMethod]
    public void AsAcceptorSocket_OnAccept_InvokesHandler()
    {
        acceptorHandlerHasBeenCalled = false;
        Action<ISocketSessionConnection> acceptorHandler = ssc => { acceptorHandlerHasBeenCalled = true; };
        var socketSessionReceiverAsAcceptor = new SocketSessionReceiver(moqOsSocket.Object,
            directOsNetworkingApiStub, acceptorHandler, testSessionDescription);

        socketSessionReceiverAsAcceptor.OnAccept();

        Assert.IsTrue(acceptorHandlerHasBeenCalled);
    }

    [TestMethod]
    public void AsReceiverSocketWithDataAndAvailableBuffer_ReceiveData_CallsDecoderToReadBuffer()
    {
        PrepareSocketFullRead();
        moqFeedDecoder.Setup(fd => fd.Process(dispatchContext)).Returns(defaultSocketDataWritten.Length)
            .Verifiable();
        var dataReceived = socketSessionReceiverAsDataReceiver.ReceiveData(dispatchContext);
        Assert.IsTrue(dataReceived);
        moqDispatchPerfLogger.Verify();
        moqPerfLoggerPool.Verify();
        moqReadSocketPerfLogger.Verify();
        moqFeedDecoder.Verify();
    }

    [TestMethod]
    public void AsReceiverSocketWithNoDataAndZeroBytesReadIsNotDisconnection_ReceiveData_ReturnsTrue()
    {
        socketSessionReceiverAsDataReceiver = new SocketSessionReceiver(moqOsSocket.Object,
            directOsNetworkingApiStub, moqFeedDecoder.Object, testSessionDescription, 1, false);
        PrepareSocketFullRead();
        directOsNetworkingApiStub.ClearQueue();
        directOsNetworkingApiStub.QueueResponseBytes(new byte[0], true);
        var dataReceived = socketSessionReceiverAsDataReceiver.ReceiveData(dispatchContext);
        Assert.IsTrue(dataReceived);
    }

    [TestMethod]
    public void AsReceiverSocketWithNoDataAndZeroBytesReadIsDisconnection_ReceiveData_ReturnsFalse()
    {
        PrepareSocketFullRead();
        directOsNetworkingApiStub.ClearQueue();
        directOsNetworkingApiStub.QueueResponseBytes(new byte[0], true);
        var dataReceived = socketSessionReceiverAsDataReceiver.ReceiveData(dispatchContext);
        Assert.IsFalse(dataReceived);
    }

    [TestMethod]
    public void AsReceiverSocketWithTooMuchData_ReceiveData_OnlyReportsOutburstEveryMinute()
    {
        const int unreadBytes = 16377;
        PrepareSocketFullRead();
        moqDispatchPerfLogger.Setup(ltcsl => ltcsl.Add(It.IsAny<string>(), unreadBytes))
            .Verifiable();
        moqDispatchPerfLogger.SetupSet(ltcsl => ltcsl.WriteTrace = true).Verifiable();
        directOsNetworkingApiStub.ClearQueue();
        CallReceiveDataAssertDataBurstAlertTimes(unreadBytes, 1, new DateTime(2017, 05, 14, 17, 40, 01));

        CallReceiveDataAssertDataBurstAlertTimes(unreadBytes, 1, new DateTime(2017, 05, 14, 17, 41, 00));

        CallReceiveDataAssertDataBurstAlertTimes(unreadBytes, 2, new DateTime(2017, 05, 14, 17, 41, 02));
    }

    [TestMethod]
    public void AsReceiverSocketWithData_ReceiveData_LogsTraceIfDecoderProcessesNothing()
    {
        PrepareSocketFullRead();
        moqFeedDecoder.Setup(fd => fd.Process(dispatchContext)).Returns(0).Verifiable();

        socketSessionReceiverAsDataReceiver.ReceiveData(dispatchContext);

        moqDispatchPerfLogger.Verify(ltcsl => ltcsl.Add(It.IsAny<string>()),
            Times.Once);
        moqDispatchPerfLogger.VerifySet(ltcsl => ltcsl.WriteTrace = true, Times.Once);
        moqFeedDecoder.Verify(fd => fd.Process(dispatchContext), Times.Once);
    }

    [TestMethod]
    public void AsReceiverSocketUnreadBuffer_ReceiveData_DoesNotResetBufferWhileUnreadRemains()
    {
        PrepareSocketFullRead();

        moqFeedDecoder.Setup(fd => fd.Process(dispatchContext)).Returns(0).Verifiable();

        var receiveBuffer = NonPublicInvocator.GetInstanceField<ReadWriteBuffer>(
            socketSessionReceiverAsDataReceiver, "receiveBuffer");
        AssertReceiveBufferIsExpected(receiveBuffer, 8, 24);
        AssertReceiveBufferIsExpected(receiveBuffer, 16, 25);
        AssertReceiveBufferIsExpected(receiveBuffer, 26, 26);
        AssertReceiveBufferIsExpected(receiveBuffer, 1, 1);
    }


    [TestMethod]
    public void AsReceiverSocketRemainingBufferLessThan400_ReceiveData_ShiftsUnreadDataToBufferStart()
    {
        PrepareSocketFullRead();
        directOsNetworkingApiStub.ClearQueue();
        directOsNetworkingApiStub.QueueResponseBytes(new byte[401], true);
        moqOsSocket.SetupGet(oss => oss.ReceiveBufferSize).Returns(byte.MaxValue * 2).Verifiable();
        socketSessionReceiverAsDataReceiver = new SocketSessionReceiver(moqOsSocket.Object,
            directOsNetworkingApiStub, moqFeedDecoder.Object, testSessionDescription);
        var receiveBuffer = NonPublicInvocator.GetInstanceField<ReadWriteBuffer>(
            socketSessionReceiverAsDataReceiver, "receiveBuffer");

        for (var i = 0; i < receiveBuffer.Buffer.Length; i++) receiveBuffer.Buffer[i] = (byte)(i % byte.MaxValue);
        receiveBuffer.ReadCursor = byte.MaxValue / 4;
        receiveBuffer.WrittenCursor = 113;

        moqFeedDecoder.SetupSequence(fd => fd.Process(dispatchContext)).Returns(0);

        var receiveData = socketSessionReceiverAsDataReceiver.ReceiveData(dispatchContext);

        Assert.IsTrue(receiveData);
        for (var i = 0; i < 48; i++) Assert.AreEqual(i + byte.MaxValue / 4, receiveBuffer.Buffer[i]);
    }

    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public void AsReceiverSocketWithIOCTLError_ReceiveData_ThrowsExceptionWithLasCallError()
    {
        PrepareSocketFullRead();
        directOsNetworkingApiStub.ClearQueue();
        directOsNetworkingApiStub.QueueResponseBytes(null, true);
        socketSessionReceiverAsDataReceiver.ReceiveData(dispatchContext);
        Assert.Fail("Should not reach here");
    }

    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public void AsReceiverSocketWithRecvError_ReceiveData_ThrowsExceptionWithLastCallError()
    {
        PrepareSocketFullRead();
        directOsNetworkingApiStub.ClearQueue();
        directOsNetworkingApiStub.QueueResponseBytes(null, false);
        try
        {
            socketSessionReceiverAsDataReceiver.ReceiveData(dispatchContext);
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
    public void AsReceiverSocketWithSocketTraceEnabledNearFullBuffer_ReceiveData_SetsTraceAndMessageSize()
    {
        moqReadSocketPerfLogger.SetupGet(ltcsl => ltcsl.Enabled).Returns(true).Verifiable();
        moqReadSocketPerfLogger.SetupSet(ltcsl => ltcsl.WriteTrace = true).Verifiable();
        moqReadSocketPerfLogger.Setup(ltcsl => ltcsl.Add(It.IsAny<string>(), It.IsAny<long>()))
            .Verifiable();
        PrepareSocketFullRead();
        directOsNetworkingApiStub.ClearQueue();
        directOsNetworkingApiStub.QueueResponseBytes(new byte[358], true);
        moqOsSocket.SetupGet(oss => oss.ReceiveBufferSize).Returns(byte.MaxValue * 2).Verifiable();
        socketSessionReceiverAsDataReceiver = new SocketSessionReceiver(moqOsSocket.Object,
            directOsNetworkingApiStub, moqFeedDecoder.Object, testSessionDescription);

        socketSessionReceiverAsDataReceiver.ReceiveData(dispatchContext);

        moqReadSocketPerfLogger.Verify();
    }

    [TestMethod]
    public void AsReceiverSocketWithSocketTraceEnabledFullBuffer_ReceiveDataAMillionTimes_SetsTraceAndMessageSize()
    {
        moqReadSocketPerfLogger.SetupGet(ltcsl => ltcsl.Enabled).Returns(true).Verifiable();
        moqReadSocketPerfLogger.SetupSet(ltcsl => ltcsl.WriteTrace = true).Verifiable();
        moqReadSocketPerfLogger.Setup(ltcsl => ltcsl.Add(It.IsAny<string>(), It.IsAny<long>()))
            .Verifiable();
        PrepareSocketFullRead();
        directOsNetworkingApiStub.ClearQueue();
        directOsNetworkingApiStub.QueueResponseBytes(new byte[358], true);
        moqOsSocket.SetupGet(oss => oss.ReceiveBufferSize).Returns(byte.MaxValue * 2).Verifiable();
        socketSessionReceiverAsDataReceiver = new SocketSessionReceiver(moqOsSocket.Object,
            directOsNetworkingApiStub, moqFeedDecoder.Object, testSessionDescription);

        NonPublicInvocator.SetInstanceField(socketSessionReceiverAsDataReceiver, "bufferFullCounter", 999999);

        socketSessionReceiverAsDataReceiver.ReceiveData(dispatchContext);

        moqReadSocketPerfLogger.Verify(ltcsl => ltcsl.Add(It.IsAny<string>(),
            It.IsAny<long>()), Times.Never);
        moqReadSocketPerfLogger.VerifySet(ltcsl => ltcsl.WriteTrace = true, Times.Never);


        directOsNetworkingApiStub.QueueResponseBytes(new byte[358], true);
        socketSessionReceiverAsDataReceiver.ReceiveData(dispatchContext);
        moqReadSocketPerfLogger.Verify();
    }

    [TestMethod]
    public void AsReceiverSocketWithSocketTraceEnabledEmptyBuffer_ReceiveData_SetsTraceAndMessageSize()
    {
        moqReadSocketPerfLogger.SetupGet(ltcsl => ltcsl.Enabled).Returns(true).Verifiable();
        PrepareSocketFullRead();
        directOsNetworkingApiStub.ClearQueue();
        directOsNetworkingApiStub.QueueResponseBytes(new byte[356], true);
        moqOsSocket.SetupGet(oss => oss.ReceiveBufferSize).Returns(byte.MaxValue * 2).Verifiable();
        socketSessionReceiverAsDataReceiver = new SocketSessionReceiver(moqOsSocket.Object,
            directOsNetworkingApiStub, moqFeedDecoder.Object, testSessionDescription);

        socketSessionReceiverAsDataReceiver.ReceiveData(dispatchContext);

        moqReadSocketPerfLogger.Verify(ltcsl => ltcsl.Add(It.IsAny<string>(),
            It.IsAny<long>()), Times.Never);
        moqReadSocketPerfLogger.VerifySet(ltcsl => ltcsl.WriteTrace = true, Times.Never);
    }

    [TestMethod]
    [ExpectedException(typeof(SocketBufferTooFullException))]
    public void AsReceiverSocketWithNearFullBuffer_FirstReceiveData_ThrowsExceptionToCloseReopenSocketEmpty()
    {
        NonPublicInvocator.SetStaticField(typeof(SocketSessionReceiver),
            "HasBouncedFeedAlready", new Dictionary<string, bool>());
        PrepareSocketFullRead();
        directOsNetworkingApiStub.ClearQueue();
        directOsNetworkingApiStub.QueueResponseBytes(new byte[460], true);
        moqOsSocket.SetupGet(oss => oss.ReceiveBufferSize).Returns(byte.MaxValue * 2).Verifiable();
        socketSessionReceiverAsDataReceiver = new SocketSessionReceiver(moqOsSocket.Object,
            directOsNetworkingApiStub, moqFeedDecoder.Object, testSessionDescription);

        socketSessionReceiverAsDataReceiver.ReceiveData(dispatchContext);

        Assert.Fail("Should Not Get Here");
    }

    [TestMethod]
    public void AsReceiverSocketWithNearFullBuffer_SecondReceiveData_DoesNotThrowsException()
    {
        NonPublicInvocator.SetStaticField(typeof(SocketSessionReceiver),
            "HasBouncedFeedAlready", new Dictionary<string, bool>());

        PrepareSocketFullRead();
        directOsNetworkingApiStub.ClearQueue();
        directOsNetworkingApiStub.QueueResponseBytes(new byte[460], true);
        moqOsSocket.SetupGet(oss => oss.ReceiveBufferSize).Returns(byte.MaxValue * 2).Verifiable();
        socketSessionReceiverAsDataReceiver = new SocketSessionReceiver(moqOsSocket.Object,
            directOsNetworkingApiStub, moqFeedDecoder.Object, testSessionDescription);

        var caughtException = false;
        try
        {
            socketSessionReceiverAsDataReceiver.ReceiveData(dispatchContext);
        }
        catch (SocketBufferTooFullException)
        {
            caughtException = true;
        }

        Assert.IsTrue(caughtException);
        socketSessionReceiverAsDataReceiver = new SocketSessionReceiver(moqOsSocket.Object,
            directOsNetworkingApiStub, moqFeedDecoder.Object, testSessionDescription);
        var receiveData = socketSessionReceiverAsDataReceiver.ReceiveData(dispatchContext);
        Assert.IsTrue(receiveData);
    }

    [TestMethod]
    public void AsReceiverSocketSocketDumpEnable_ReceiveData_WriteBytesToLogger()
    {
        var moqFloggerFactory = new Mock<IFLoggerFactory>();
        var moqSocketByteDump = new Mock<IFLogger>();
        moqFloggerFactory.Setup(ff => ff.GetLogger("SocketByteDump.TestSessionDescription"))
            .Returns(moqSocketByteDump.Object).Verifiable();
        try
        {
            FLoggerFactory.Instance = moqFloggerFactory.Object;
            moqSocketByteDump.SetupGet(fl => fl.Enabled).Returns(true).Verifiable();
            moqSocketByteDump.Setup(fl => fl.Debug(It.IsAny<string>())).Verifiable();
            PrepareSocketFullRead();
            directOsNetworkingApiStub.ClearQueue();
            directOsNetworkingApiStub.QueueResponseBytes(new byte[10], true);
            moqOsSocket.SetupGet(oss => oss.ReceiveBufferSize).Returns(byte.MaxValue * 2).Verifiable();
            socketSessionReceiverAsDataReceiver = new SocketSessionReceiver(moqOsSocket.Object,
                directOsNetworkingApiStub, moqFeedDecoder.Object, testSessionDescription);
            var receiveData = socketSessionReceiverAsDataReceiver.ReceiveData(dispatchContext);
            Assert.IsTrue(receiveData);

            moqSocketByteDump.Verify();
        }
        finally
        {
            FLoggerFactory.Instance = new FLoggerFactory();
        }
    }

    [TestMethod]
    public void AsReceiverSocketMultipleWholeMessaagesLimitOnSessionMax_ReceiveData_RemovesExpectedNumMessages()
    {
        PrepareSocketFullRead();
        directOsNetworkingApiStub.QueueResponseBytes(new byte[10], true);
        directOsNetworkingApiStub.QueueResponseBytes(new byte[10], true);
        directOsNetworkingApiStub.QueueResponseBytes(new byte[10], true);
        Assert.AreEqual(4, directOsNetworkingApiStub.NumberQueuedMessages);
        socketSessionReceiverAsDataReceiver = new SocketSessionReceiver(moqOsSocket.Object,
            directOsNetworkingApiStub, moqFeedDecoder.Object, testSessionDescription, 3);
        var receiveData = socketSessionReceiverAsDataReceiver.ReceiveData(dispatchContext);
        Assert.IsTrue(receiveData);

        Assert.AreEqual(1, directOsNetworkingApiStub.NumberQueuedMessages);
    }


    [TestMethod]
    public void AsReceiverSocketMultipleWholeMessaagesLimitOnDecoderMax_ReceiveData_RemovesExpectedNumMessages()
    {
        PrepareSocketFullRead();
        directOsNetworkingApiStub.QueueResponseBytes(new byte[10], true);
        directOsNetworkingApiStub.QueueResponseBytes(new byte[10], true);
        directOsNetworkingApiStub.QueueResponseBytes(new byte[10], true);
        Assert.AreEqual(4, directOsNetworkingApiStub.NumberQueuedMessages);
        socketSessionReceiverAsDataReceiver = new SocketSessionReceiver(moqOsSocket.Object,
            directOsNetworkingApiStub, moqFeedDecoder.Object, testSessionDescription, 2);
        var receiveData = socketSessionReceiverAsDataReceiver.ReceiveData(dispatchContext);
        Assert.IsTrue(receiveData);

        Assert.AreEqual(2, directOsNetworkingApiStub.NumberQueuedMessages);
    }

    [TestMethod]
    public void AsReceiverSocketMultiWholeMsgsLmtRemainingBufferSz_ReceiveData_RemovesExpectedNumMessages()
    {
        PrepareSocketFullRead();
        moqOsSocket.SetupGet(oss => oss.ReceiveBufferSize).Returns(400).Verifiable();
        socketSessionReceiverAsDataReceiver = new SocketSessionReceiver(moqOsSocket.Object,
            directOsNetworkingApiStub, moqFeedDecoder.Object, testSessionDescription, 2);
        var moqTimeContext = new Mock<ITimeContext>();
        moqTimeContext.SetupGet(tc => tc.UtcNow).Returns(new DateTime(2017, 05, 15, 20, 33, 56));
        TimeContext.Provider = moqTimeContext.Object;
        socketSessionReceiverAsDataReceiver.ReceiveData(dispatchContext);
        directOsNetworkingApiStub.QueueResponseBytes(new byte[100], true);
        directOsNetworkingApiStub.QueueResponseBytes(new byte[100], true);
        directOsNetworkingApiStub.QueueResponseBytes(new byte[100], true);
        directOsNetworkingApiStub.QueueResponseBytes(new byte[100], true);
        Assert.AreEqual(4, directOsNetworkingApiStub.NumberQueuedMessages);
        moqTimeContext.SetupGet(tc => tc.UtcNow).Returns(new DateTime(2017, 05, 15, 20, 33, 58));
        var receiveData = socketSessionReceiverAsDataReceiver.ReceiveData(dispatchContext);
        Assert.IsTrue(receiveData);

        Assert.AreEqual(2, directOsNetworkingApiStub.NumberQueuedMessages);
    }

    [TestMethod]
    public void AsReceiverSocketMultipleWholeMessaagesLimitSocketOutOfData_ReceiveData_RemovesAllMessages()
    {
        PrepareSocketFullRead();
        directOsNetworkingApiStub.QueueResponseBytes(new byte[10], true);
        directOsNetworkingApiStub.QueueResponseBytes(new byte[10], true);
        directOsNetworkingApiStub.QueueResponseBytes(new byte[10], true);
        Assert.AreEqual(4, directOsNetworkingApiStub.NumberQueuedMessages);
        socketSessionReceiverAsDataReceiver = new SocketSessionReceiver(moqOsSocket.Object,
            directOsNetworkingApiStub, moqFeedDecoder.Object, testSessionDescription, 50);
        var receiveData = socketSessionReceiverAsDataReceiver.ReceiveData(dispatchContext);
        Assert.IsTrue(receiveData);

        Assert.AreEqual(0, directOsNetworkingApiStub.NumberQueuedMessages);
    }

    private void AssertReceiveBufferIsExpected(ReadWriteBuffer receiveBuffer, int expectedRead, int expectedWritten)
    {
        directOsNetworkingApiStub.QueueResponseBytes(new byte[1], true);
        moqFeedDecoder.Setup(fd => fd.Process(dispatchContext)).Callback(() =>
        {
            dispatchContext.EncodedBuffer!.ReadCursor = expectedRead;
        });

        socketSessionReceiverAsDataReceiver.ReceiveData(dispatchContext);
        Assert.AreEqual(expectedRead, receiveBuffer.ReadCursor);
        Assert.AreEqual(expectedWritten, receiveBuffer.WrittenCursor);
    }

    private void CallReceiveDataAssertDataBurstAlertTimes(int unreadBytes, int numTimesExpected,
        DateTime socketWakeTime)
    {
        dispatchContext.DetectTimestamp = socketWakeTime;
        moqFeedDecoder.Setup(fd => fd.Process(dispatchContext))
            .Callback(() => { dispatchContext.EncodedBuffer!.ReadCursor = unreadBytes; }).Returns(unreadBytes)
            .Verifiable();
        directOsNetworkingApiStub.QueueResponseBytes(new byte[unreadBytes], true);
        socketSessionReceiverAsDataReceiver.ReceiveData(dispatchContext);
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
        moqFeedDecoder.Setup(fd => fd.Process(dispatchContext)).Returns(defaultSocketDataWritten.Length)
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
