#region

using FortitudeCommon.Chronometry.Timers;
using FortitudeCommon.EventProcessing.Disruption.Rings.PollingRings;
using FortitudeCommon.Monitoring.Logging.Diagnostics.Performance;
using FortitudeCommon.OSWrapper.AsyncWrappers;
using FortitudeCommon.Types;
using FortitudeIO.Protocols.Serdes.Binary.Sockets;
using FortitudeIO.Transports.Network;
using FortitudeIO.Transports.Network.Construction;
using FortitudeIO.Transports.Network.Dispatcher;
using FortitudeIO.Transports.Network.Logging;
using FortitudeIO.Transports.Network.Receiving;
using Moq;

#endregion

namespace FortitudeTests.FortitudeIO.Transports.Network.Dispatcher;

[TestClass]
public class SimpleSocketBatchEnumerableRingPollerListenerTests
{
    private const uint NoDataPauseTimeout = 100U;
    private List<SimpleSocketReceiverPayload> emptyEnumerable = new();
    private List<ISocketReceiver> firstListOfSocketsWithUpdate = new();
    private SimpleSocketReceiverPayload firstReceiverPayload = null!;
    private Mock<IIntraOSThreadSignal> intraOsThreadSignal = null!;
    private Mock<IOSThread> moqOsThread = null!;
    private Mock<IOSParallelController> moqParallelController = null!;
    private Mock<IOSParallelControllerFactory> moqParallelControllerFactory = null!;
    private Mock<IPerfLoggingPoolFactory> moqPerfFac = null!;
    private Mock<IPerfLogger> moqPerfLogger = null!;
    private Mock<IPerfLoggerPool> moqPerfPool = null!;
    private Mock<IEnumerableBatchPollingRing<SimpleSocketReceiverPayload>> moqPollingRing = null!;
    private Mock<ISocketDataLatencyLoggerFactory> moqSocketDataLatencyFactory = null!;
    private Mock<ISocketDataLatencyLogger> moqSocketDataLatencyLogger = null!;
    private Mock<ISocketReceiver> moqSocketReceiver = null!;
    private Mock<ISocketSelector> moqSocketSelector = null!;
    private Mock<IUpdateableTimer> moqUpdateableTimer = null!;
    private SimpleSocketReceiverPayload secondReceiverPayload = null!;
    private List<SimpleSocketReceiverPayload> singleItemEnumerable = null!;
    private SimpleSocketBatchEnumerableRingPollerListener socketBatchEnumerableRingPollerListener = null!;
    private SocketBufferReadContext socketBufferReadContext = null!;
    private SimpleSocketReceiverPayload thirdRecieverPayload = null!;
    private DateTime wakeTime;
    private ThreadStart workerThreadMethod = null!;

    [TestInitialize]
    public void SetUp()
    {
        moqParallelControllerFactory = new Mock<IOSParallelControllerFactory>();
        moqPollingRing = new Mock<IEnumerableBatchPollingRing<SimpleSocketReceiverPayload>>();
        moqParallelController = new Mock<IOSParallelController>();
        moqParallelControllerFactory.SetupGet(pcf => pcf.GetOSParallelController)
            .Returns(moqParallelController.Object);
        moqOsThread = new Mock<IOSThread>();
        moqSocketSelector = new Mock<ISocketSelector>();
        moqUpdateableTimer = new Mock<IUpdateableTimer>();
        intraOsThreadSignal = new Mock<IIntraOSThreadSignal>();
        intraOsThreadSignal.Setup(iots => iots.WaitOne()).Verifiable();
        moqParallelController.Setup(pc => pc.CreateNewOSThread(It.IsAny<ThreadStart>()))
            .Callback<ThreadStart>(threadRootMethod => { workerThreadMethod = threadRootMethod; })
            .Returns(moqOsThread.Object).Verifiable();
        moqParallelController.Setup(pc => pc.AllWaitingOSThreadActivateSignal(It.IsAny<bool>()))
            .Returns(intraOsThreadSignal.Object).Verifiable();
        moqPerfFac = new Mock<IPerfLoggingPoolFactory>();
        moqPerfPool = new Mock<IPerfLoggerPool>();
        moqPerfLogger = new Mock<IPerfLogger>();
        moqPerfFac.Setup(fac => fac.GetLatencyTracingLoggerPool(It.IsAny<string>(), It.IsAny<TimeSpan>(),
            It.IsAny<Type>())).Returns(moqPerfPool.Object).Verifiable();
        moqSocketDataLatencyLogger = new Mock<ISocketDataLatencyLogger>();
        moqSocketDataLatencyFactory = new Mock<ISocketDataLatencyLoggerFactory>();
        moqSocketDataLatencyFactory
            .Setup(sdlf => sdlf.GetSocketDataLatencyLogger(It.IsAny<string>()))
            .Returns(moqSocketDataLatencyLogger.Object);
        singleItemEnumerable = new List<SimpleSocketReceiverPayload>();

        firstReceiverPayload = new SimpleSocketReceiverPayload();
        secondReceiverPayload = new SimpleSocketReceiverPayload();
        thirdRecieverPayload = new SimpleSocketReceiverPayload();

        SocketDataLatencyLoggerFactory.Instance = moqSocketDataLatencyFactory.Object;
        PerfLoggingPoolFactory.Instance = moqPerfFac.Object;
        OSParallelControllerFactory.Instance = moqParallelControllerFactory.Object;

        moqPollingRing.Setup(pr => pr.Name).Returns("SimpleSocketRingPollerSenderTests");
        moqPollingRing.Setup(pr => pr[0]).Returns(firstReceiverPayload);
        moqPollingRing.Setup(pr => pr[1]).Returns(secondReceiverPayload);
        moqPollingRing.Setup(pr => pr[2]).Returns(thirdRecieverPayload);
        moqPollingRing.SetupSequence(pr => pr.Claim()).Returns(0).Returns(1).Returns(2);
        moqPollingRing.SetupGet(pr => pr.CurrentBatchSize).Returns(3);
        moqPollingRing.SetupSequence(pr => pr.StartOfBatch).Returns(true).Returns(false).Returns(false);
        moqPollingRing.SetupSequence(pr => pr.EndOfBatch).Returns(false).Returns(false).Returns(true);
        moqPollingRing.SetupSequence(pr => pr.CurrentSequence).Returns(0).Returns(1).Returns(2);

        moqSocketReceiver = new Mock<ISocketReceiver>();

        moqPollingRing.Setup(pr => pr.GetEnumerator()).Callback(() =>
        {
            moqPollingRing.Setup(pr => pr.GetEnumerator()).Returns(emptyEnumerable.GetEnumerator());
            socketBatchEnumerableRingPollerListener.Stop();
        }).Returns(emptyEnumerable.GetEnumerator());

        socketBatchEnumerableRingPollerListener = new SimpleSocketBatchEnumerableRingPollerListener(moqPollingRing.Object, NoDataPauseTimeout
            , moqSocketSelector.Object, moqUpdateableTimer.Object
            , null, moqParallelController.Object);


        var pollerAndDecoding
            = NonPublicInvocator.GetInstanceField<SocketsPollerAndDecoding>(socketBatchEnumerableRingPollerListener, "socketsPollerAndDecoding");

        socketBufferReadContext = NonPublicInvocator.GetInstanceField<SocketBufferReadContext>(pollerAndDecoding, "socketBufferReadContext");
    }

    [TestCleanup]
    public void TearDown()
    {
        OSParallelControllerFactory.ClearOSParallelControllerFactory();
        SocketDataLatencyLoggerFactory.ClearSocketDataLatencyLoggerFactory();
        PerfLoggingPoolFactory.ClearPerfLoggingPoolFactory();
    }

    [TestMethod]
    public void NewSocketRingPollerListener_New_ReceivesLatencyTraceCallsLoggerPool()
    {
        moqPerfFac.Verify();
    }

    [TestMethod]
    public void NewSocketRingPollerListener_RegisterForLister_RegistersSocketReceiverWithSelector()
    {
        moqSocketSelector.Setup(ss => ss.Register(moqSocketReceiver.Object)).Verifiable();
        moqSocketReceiver.SetupSet(ssc => ssc.ListenActive = true).Verifiable();
        singleItemEnumerable.Add(firstReceiverPayload);
        moqPollingRing.Setup(pr => pr.GetEnumerator()).Callback(() =>
        {
            moqPollingRing.Setup(pr => pr.GetEnumerator()).Callback(() =>
                {
                    moqPollingRing.Setup(pr => pr.GetEnumerator()).Returns(emptyEnumerable.GetEnumerator());
                    socketBatchEnumerableRingPollerListener.Stop();
                })
                .Returns(emptyEnumerable.GetEnumerator());
        }).Returns(singleItemEnumerable.GetEnumerator());

        socketBatchEnumerableRingPollerListener.Start();
        socketBatchEnumerableRingPollerListener.RegisterForListen(moqSocketReceiver.Object);
        workerThreadMethod();

        moqSocketReceiver.Verify();
        moqSocketSelector.Verify();
    }

    [TestMethod]
    public void NewSocketRingPollerListener_UnregisterForListen_UnregistersSocketReceiverWithSelector()
    {
        moqSocketSelector.Setup(ss => ss.Unregister(moqSocketReceiver.Object)).Verifiable();
        moqSocketReceiver.SetupSet(ssc => ssc.ListenActive = false).Verifiable();
        singleItemEnumerable.Add(firstReceiverPayload);
        moqPollingRing.Setup(pr => pr.GetEnumerator()).Callback(() =>
        {
            moqPollingRing.Setup(pr => pr.GetEnumerator()).Callback(() =>
                {
                    moqPollingRing.Setup(pr => pr.GetEnumerator()).Returns(emptyEnumerable.GetEnumerator());
                    socketBatchEnumerableRingPollerListener.Stop();
                })
                .Returns(emptyEnumerable.GetEnumerator());
        }).Returns(singleItemEnumerable.GetEnumerator());

        socketBatchEnumerableRingPollerListener.Start();
        socketBatchEnumerableRingPollerListener.UnregisterForListen(moqSocketReceiver.Object);
        workerThreadMethod();

        moqSocketReceiver.Verify();
        moqSocketSelector.Verify();
    }

    [TestMethod]
    public void SocketWithUpdate_ReceiveSuccessfullyCallsPoll_NormalProcessingOccurs()
    {
        PrepareOneSuccessfulReceive();
        socketBatchEnumerableRingPollerListener.Start();
        workerThreadMethod();

        moqParallelController.Verify();
        moqOsThread.Verify();
        moqPerfPool.Verify();
        moqSocketSelector.Verify();
        moqPerfLogger.Verify();
        moqSocketDataLatencyLogger.Verify();
        moqSocketReceiver.Verify();
    }

    [TestMethod]
    public void SocketWithUpdate_ReceiveDataReturnsFalse_ErrorIsLoggedAndConnectionErrorIsRaised()
    {
        PrepareOneSuccessfulReceive();
        moqSocketReceiver.Setup(ssc => ssc.Poll(socketBufferReadContext))
            .Callback(() =>
            {
                Assert.AreEqual(wakeTime, socketBufferReadContext.DetectTimestamp);
                Assert.AreEqual(moqPerfLogger.Object, socketBufferReadContext.DispatchLatencyLogger);
                NonPublicInvocator.SetInstanceField(socketBatchEnumerableRingPollerListener, "isRunning", false);
            })
            .Returns(false).Verifiable();
        moqSocketReceiver.Setup(scc =>
                scc.HandleReceiveError(It.IsRegex("Connection lost on SocketRingPoller .+"), It.IsAny<Exception>()))
            .Verifiable();
        moqPerfLogger.Reset();
        moqPerfLogger.Setup(ltcsl => ltcsl.Indent()).Verifiable();
        moqPerfLogger.Setup(ltcsl => ltcsl.Add("Connection lost on SocketRingPoller."))
            .Verifiable();
        moqPerfLogger.Setup(ltcsl => ltcsl.Dedent()).Verifiable();
        moqPerfLogger.Setup(ltcsl => ltcsl.AddContextMeasurement(1)).Verifiable();
        moqPerfLogger.Setup(ltcsl => ltcsl.Add("End Processing Socket Data")).Verifiable();
        socketBatchEnumerableRingPollerListener.Start();
        workerThreadMethod();

        moqParallelController.Verify();
        moqOsThread.Verify();
        moqPerfPool.Verify();
        moqSocketSelector.Verify();
        moqSocketReceiver.Verify();
        moqPerfLogger.Verify();
        moqSocketDataLatencyLogger.Verify();
    }

    [TestMethod]
    public void TwoSocketsWithUpdate_ReceiveFirstSocketThrowsException_SecondSocketIsProcessedNormally()
    {
        PrepareOneSuccessfulReceive();

        var moqSocketReceiverThrowsException = new Mock<ISocketReceiver>();
        moqSocketReceiverThrowsException.SetupGet(ssc => ssc.IsAcceptor).Returns(false).Verifiable();
        moqSocketReceiverThrowsException.Setup(sr =>
                sr.Poll(socketBufferReadContext))
            .Throws(new SocketBufferTooFullException("Test Socket Buffer full")).Verifiable();
        moqSocketReceiverThrowsException.Setup(scc =>
            scc.HandleReceiveError(It.IsRegex("Connection lost on SocketRingPoller .+"), It.IsAny<Exception>())).Verifiable();
        var twoSocketsWithUpdate = new List<ISocketReceiver> { moqSocketReceiverThrowsException.Object, moqSocketReceiver.Object };
        moqSocketSelector.Setup(ss => ss.WatchSocketsForRecv(moqPerfLogger.Object))
            .Returns(twoSocketsWithUpdate).Verifiable();

        moqPerfLogger.Reset();
        moqPerfLogger.Setup(ltcsl => ltcsl.Indent()).Verifiable();
        moqPerfLogger.Setup(ltcsl => ltcsl.Add(
            It.Is<string>(check => check.Contains("Connection lost on SocketRingPoller.")))).Verifiable();
        moqPerfLogger.Setup(ltcsl => ltcsl.Dedent()).Verifiable();
        moqPerfLogger.Setup(ltcsl => ltcsl.AddContextMeasurement(2)).Verifiable();
        moqPerfLogger.Setup(ltcsl => ltcsl.Add("End Processing Socket Data")).Verifiable();
        socketBatchEnumerableRingPollerListener.Start();
        workerThreadMethod();

        moqParallelController.Verify();
        moqOsThread.Verify();
        moqPerfPool.Verify();
        moqSocketSelector.Verify();
        moqSocketReceiver.Verify();
        moqPerfLogger.Verify();
        moqSocketDataLatencyLogger.Verify();
        moqSocketReceiverThrowsException.Verify();
        moqSocketReceiver.Verify();
    }

    [TestMethod]
    public void SocketWithUpdate_SelectRecvThrowsException_RecoversAndCallsAgain()
    {
        PrepareOneSuccessfulReceive();
        moqSocketSelector.Reset();
        moqSocketSelector.SetupGet(ss => ss.CountRegisteredReceivers).Returns(1);
        moqSocketSelector.SetupSequence(ss => ss.WatchSocketsForRecv(moqPerfLogger.Object))
            .Throws<Exception>().Returns(firstListOfSocketsWithUpdate);
        moqSocketSelector.SetupGet(ss => ss.WakeTs).Returns(wakeTime).Verifiable();

        moqPollingRing.Setup(pr => pr.GetEnumerator()).Callback(() =>
        {
            moqPollingRing.Setup(pr => pr.GetEnumerator()).Callback(() =>
                {
                    moqPollingRing.Setup(pr => pr.GetEnumerator()).Returns(emptyEnumerable.GetEnumerator());
                    socketBatchEnumerableRingPollerListener.Stop();
                })
                .Returns(emptyEnumerable.GetEnumerator());
        }).Returns(emptyEnumerable.GetEnumerator());
        socketBatchEnumerableRingPollerListener.Start();
        workerThreadMethod();

        moqParallelController.Verify();
        moqOsThread.Verify();
        moqPerfPool.Verify();
        moqSocketSelector.Verify();
        moqSocketReceiver.Verify();
        moqPerfLogger.Verify();
        moqSocketDataLatencyLogger.Verify();
    }

    [TestMethod]
    public void SocketWithUpdate_GetPerfLoggerThrowsException_RecoversAndCallsAgain()
    {
        PrepareOneSuccessfulReceive();
        moqPerfPool.Reset();
        moqPerfPool.SetupSequence(pool => pool.StartNewTrace())
            .Throws<Exception>().Returns(moqPerfLogger.Object);

        moqPollingRing.Setup(pr => pr.GetEnumerator()).Callback(() =>
        {
            moqPollingRing.Setup(pr => pr.GetEnumerator()).Callback(() =>
                {
                    moqPollingRing.Setup(pr => pr.GetEnumerator()).Returns(emptyEnumerable.GetEnumerator());
                    socketBatchEnumerableRingPollerListener.Stop();
                })
                .Returns(emptyEnumerable.GetEnumerator());
        }).Returns(emptyEnumerable.GetEnumerator());
        socketBatchEnumerableRingPollerListener.Start();
        workerThreadMethod();

        moqParallelController.Verify();
        moqOsThread.Verify();
        moqPerfPool.Verify();
        moqPerfLogger.Verify();
        moqSocketDataLatencyLogger.Verify();
    }

    [TestMethod]
    public void AcceptorSocket_Receive_SocketSelectorReturnsSocketReceiveDataIsCalled()
    {
        PrepareOneSuccessfulReceive();
        moqSocketReceiver.Reset();
        moqSocketReceiver.SetupGet(ssc => ssc.IsAcceptor).Returns(true).Verifiable();
        moqSocketReceiver.Setup(ssc => ssc.NewClientSocketRequest())
            .Callback(() => { NonPublicInvocator.SetInstanceField(socketBatchEnumerableRingPollerListener, "isRunning", false); })
            .Verifiable();

        socketBatchEnumerableRingPollerListener.Start();
        workerThreadMethod();

        moqParallelController.Verify();
        moqOsThread.Verify();
        moqPerfPool.Verify();
        moqSocketReceiver.Verify();
        moqPerfLogger.Verify();
        moqSocketDataLatencyLogger.Verify();
    }

    private void PrepareOneSuccessfulReceive()
    {
        firstListOfSocketsWithUpdate = new List<ISocketReceiver> { moqSocketReceiver.Object };
        moqOsThread.SetupSet(ost => ost.IsBackground = true).Verifiable();
        moqOsThread.Setup(ost => ost.Start()).Verifiable();
        moqPerfPool.Setup(pool => pool.StartNewTrace())
            .Returns(moqPerfLogger.Object).Verifiable();
        moqSocketSelector.SetupGet(ss => ss.CountRegisteredReceivers).Returns(1);
        moqSocketSelector.Setup(ss => ss.WatchSocketsForRecv(moqPerfLogger.Object))
            .Returns(firstListOfSocketsWithUpdate).Verifiable();
        moqSocketReceiver.SetupGet(ssc => ssc.IsAcceptor).Returns(false).Verifiable();
        wakeTime = new DateTime(2017, 04, 21, 22, 33, 23);
        moqSocketSelector.SetupGet(ss => ss.WakeTs).Returns(wakeTime).Verifiable();
        moqSocketReceiver.Setup(ssc => ssc.Poll(socketBufferReadContext))
            .Callback(() =>
            {
                Assert.AreEqual(wakeTime, socketBufferReadContext.DetectTimestamp);
                Assert.AreEqual(moqPerfLogger.Object, socketBufferReadContext.DispatchLatencyLogger);
                NonPublicInvocator.SetInstanceField(socketBatchEnumerableRingPollerListener, "isRunning", false);
            })
            .Returns(true).Verifiable();
        moqPerfLogger.Setup(ltcsl => ltcsl.AddContextMeasurement(1)).Verifiable();
        moqPerfLogger.Setup(ltcsl => ltcsl.Add("End Processing Socket Data")).Verifiable();
        moqSocketDataLatencyLogger.Setup(ltcsl => ltcsl.ParseTraceLog(moqPerfLogger.Object))
            .Verifiable();
        moqPerfPool.Setup(ltcsp => ltcsp.StopTrace(moqPerfLogger.Object)).Verifiable();
    }
}
