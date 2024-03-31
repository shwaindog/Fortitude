#region

using FortitudeCommon.EventProcessing.Disruption.Rings.Batching;
using FortitudeCommon.Monitoring.Logging.Diagnostics.Performance;
using FortitudeCommon.OSWrapper.AsyncWrappers;
using FortitudeCommon.Types;
using FortitudeIO.Protocols.Serdes.Binary.Sockets;
using FortitudeIO.Transports.NewSocketAPI;
using FortitudeIO.Transports.NewSocketAPI.Construction;
using FortitudeIO.Transports.NewSocketAPI.Dispatcher;
using FortitudeIO.Transports.NewSocketAPI.Logging;
using FortitudeIO.Transports.NewSocketAPI.Receiving;
using Moq;

#endregion

namespace FortitudeTests.FortitudeIO.Transports.NewSocketAPI.Dispatcher;

[TestClass]
public class SimpleSocketRingPollerListenerTests
{
    private const uint NoDataPauseTimeout = 100U;
    private List<SocketReceiverUpdate> emptyEnumerable = new();
    private List<ISocketReceiver> firstListOfSocketsWithUpdate = new();
    private SocketReceiverUpdate firstReceiverUpdate = null!;
    private Mock<IIntraOSThreadSignal> intraOsThreadSignal = null!;
    private Mock<IOSThread> moqOsThread = null!;
    private Mock<IOSParallelController> moqParallelController = null!;
    private Mock<IOSParallelControllerFactory> moqParallelControllerFactory = null!;
    private Mock<IPerfLoggingPoolFactory> moqPerfFac = null!;
    private Mock<IPerfLogger> moqPerfLogger = null!;
    private Mock<IPerfLoggerPool> moqPerfPool = null!;
    private Mock<IPollingRing<SocketReceiverUpdate>> moqPollingRing = null!;
    private Mock<ISocketDataLatencyLoggerFactory> moqSocketDataLatencyFactory = null!;
    private Mock<ISocketDataLatencyLogger> moqSocketDataLatencyLogger = null!;
    private Mock<ISocketReceiver> moqSocketReceiver = null!;
    private Mock<ISocketSelector> moqSocketSelector = null!;
    private ReadSocketBufferContext readSocketBufferContext = null!;
    private SocketReceiverUpdate secondReceiverUpdate = null!;
    private List<SocketReceiverUpdate> singleItemEnumerable = null!;
    private SimpleSocketRingPollerListener socketRingPollerListener = null!;
    private SocketReceiverUpdate thirdRecieverUpdate = null!;
    private DateTime wakeTime;
    private ThreadStart workerThreadMethod = null!;

    [TestInitialize]
    public void SetUp()
    {
        moqParallelControllerFactory = new Mock<IOSParallelControllerFactory>();
        moqPollingRing = new Mock<IPollingRing<SocketReceiverUpdate>>();
        moqParallelController = new Mock<IOSParallelController>();
        moqParallelControllerFactory.SetupGet(pcf => pcf.GetOSParallelController)
            .Returns(moqParallelController.Object);
        moqOsThread = new Mock<IOSThread>();
        moqSocketSelector = new Mock<ISocketSelector>();
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
        singleItemEnumerable = new List<SocketReceiverUpdate>();

        firstReceiverUpdate = new SocketReceiverUpdate();
        secondReceiverUpdate = new SocketReceiverUpdate();
        thirdRecieverUpdate = new SocketReceiverUpdate();

        OSParallelControllerFactory.Instance = moqParallelControllerFactory.Object;
        SocketDataLatencyLoggerFactory.Instance = moqSocketDataLatencyFactory.Object;
        PerfLoggingPoolFactory.Instance = moqPerfFac.Object;

        moqPollingRing.Setup(pr => pr.Name).Returns("SimpleSocketRingPollerSenderTests");
        moqPollingRing.Setup(pr => pr[0L]).Returns(firstReceiverUpdate);
        moqPollingRing.Setup(pr => pr[1L]).Returns(secondReceiverUpdate);
        moqPollingRing.Setup(pr => pr[2L]).Returns(thirdRecieverUpdate);
        moqPollingRing.SetupSequence(pr => pr.Claim()).Returns(0).Returns(1).Returns(2);
        moqPollingRing.SetupGet(pr => pr.CurrentBatchSize).Returns(3);
        moqPollingRing.SetupSequence(pr => pr.StartOfBatch).Returns(true).Returns(false).Returns(false);
        moqPollingRing.SetupSequence(pr => pr.EndOfBatch).Returns(false).Returns(false).Returns(true);
        moqPollingRing.SetupSequence(pr => pr.CurrentSequence).Returns(0).Returns(1).Returns(2);

        moqSocketReceiver = new Mock<ISocketReceiver>();

        moqPollingRing.Setup(pr => pr.GetEnumerator()).Callback(() =>
        {
            moqPollingRing.Setup(pr => pr.GetEnumerator()).Returns(emptyEnumerable.GetEnumerator());
            socketRingPollerListener.Stop();
        }).Returns(emptyEnumerable.GetEnumerator());

        socketRingPollerListener = new SimpleSocketRingPollerListener(moqPollingRing.Object, NoDataPauseTimeout, moqSocketSelector.Object
            , null, moqParallelController.Object);

        readSocketBufferContext = NonPublicInvocator.GetInstanceField<ReadSocketBufferContext>(
            socketRingPollerListener, "readSocketBufferContext");
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
        singleItemEnumerable.Add(firstReceiverUpdate);
        moqPollingRing.Setup(pr => pr.GetEnumerator()).Callback(() =>
        {
            moqPollingRing.Setup(pr => pr.GetEnumerator()).Callback(() =>
                {
                    moqPollingRing.Setup(pr => pr.GetEnumerator()).Returns(emptyEnumerable.GetEnumerator());
                    socketRingPollerListener.Stop();
                })
                .Returns(emptyEnumerable.GetEnumerator());
        }).Returns(singleItemEnumerable.GetEnumerator());

        socketRingPollerListener.Start();
        socketRingPollerListener.RegisterForListen(moqSocketReceiver.Object);
        workerThreadMethod();

        moqSocketReceiver.Verify();
        moqSocketSelector.Verify();
    }

    [TestMethod]
    public void NewSocketRingPollerListener_UnregisterForListen_UnregistersSocketReceiverWithSelector()
    {
        moqSocketSelector.Setup(ss => ss.Unregister(moqSocketReceiver.Object)).Verifiable();
        moqSocketReceiver.SetupSet(ssc => ssc.ListenActive = false).Verifiable();
        singleItemEnumerable.Add(firstReceiverUpdate);
        moqPollingRing.Setup(pr => pr.GetEnumerator()).Callback(() =>
        {
            moqPollingRing.Setup(pr => pr.GetEnumerator()).Callback(() =>
                {
                    moqPollingRing.Setup(pr => pr.GetEnumerator()).Returns(emptyEnumerable.GetEnumerator());
                    socketRingPollerListener.Stop();
                })
                .Returns(emptyEnumerable.GetEnumerator());
        }).Returns(singleItemEnumerable.GetEnumerator());

        socketRingPollerListener.Start();
        socketRingPollerListener.UnregisterForListen(moqSocketReceiver.Object);
        workerThreadMethod();

        moqSocketReceiver.Verify();
        moqSocketSelector.Verify();
    }

    [TestMethod]
    public void SocketWithUpdate_ReceiveSuccessfullyCallsPoll_NormalProcessingOccurs()
    {
        PrepareOneSuccessfulReceive();
        socketRingPollerListener.Start();
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
        moqSocketReceiver.Setup(ssc => ssc.Poll(readSocketBufferContext))
            .Callback(() =>
            {
                Assert.AreEqual(wakeTime, readSocketBufferContext.DetectTimestamp);
                Assert.AreEqual(moqPerfLogger.Object, readSocketBufferContext.DispatchLatencyLogger);
            })
            .Returns(false).Verifiable();
        moqSocketReceiver.Setup(scc =>
                scc.HandleReceiveError(It.IsRegex("Connection lost on SocketRingPoller .+"), It.IsAny<Exception>()))
            .Verifiable();
        moqPerfLogger.Reset();
        moqPerfLogger.Setup(ltcsl => ltcsl.Indent()).Verifiable();
        moqPerfLogger.Setup(ltcsl => ltcsl.Add(It.IsRegex("Connection lost on SocketRingPoller .+")))
            .Verifiable();
        moqPerfLogger.Setup(ltcsl => ltcsl.Dedent()).Verifiable();
        moqPerfLogger.Setup(ltcsl => ltcsl.AddContextMeasurement(1)).Verifiable();
        moqPerfLogger.Setup(ltcsl => ltcsl.Add("End Processing Socket Data")).Verifiable();
        socketRingPollerListener.Start();
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
        moqSocketReceiverThrowsException.Setup(ssc =>
                ssc.Poll(readSocketBufferContext))
            .Throws(new SocketBufferTooFullException("Test Socket Buffer full")).Verifiable();
        moqSocketReceiverThrowsException.Setup(scc =>
            scc.HandleReceiveError(It.IsRegex("Read error:.+"), It.IsAny<Exception>())).Verifiable();
        var twoSocketsWithUpdate = new List<ISocketReceiver> { moqSocketReceiverThrowsException.Object, moqSocketReceiver.Object };
        moqSocketSelector.Setup(ss => ss.WatchSocketsForRecv(moqPerfLogger.Object))
            .Returns(twoSocketsWithUpdate).Verifiable();

        moqPerfLogger.Reset();
        moqPerfLogger.Setup(ltcsl => ltcsl.Indent()).Verifiable();
        moqPerfLogger.Setup(ltcsl => ltcsl.Add("Read error: ", It.IsAny<Exception>())).Verifiable();
        moqPerfLogger.Setup(ltcsl => ltcsl.Dedent()).Verifiable();
        moqPerfLogger.Setup(ltcsl => ltcsl.AddContextMeasurement(2)).Verifiable();
        moqPerfLogger.Setup(ltcsl => ltcsl.Add("End Processing Socket Data")).Verifiable();
        socketRingPollerListener.Start();
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
                    socketRingPollerListener.Stop();
                })
                .Returns(emptyEnumerable.GetEnumerator());
        }).Returns(emptyEnumerable.GetEnumerator());
        socketRingPollerListener.Start();
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
                    socketRingPollerListener.Stop();
                })
                .Returns(emptyEnumerable.GetEnumerator());
        }).Returns(emptyEnumerable.GetEnumerator());
        socketRingPollerListener.Start();
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
            .Callback(() => { NonPublicInvocator.SetInstanceField(socketRingPollerListener, "Running", false); })
            .Verifiable();

        socketRingPollerListener.Start();
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
        moqSocketReceiver.Setup(ssc => ssc.Poll(readSocketBufferContext))
            .Callback(() =>
            {
                Assert.AreEqual(wakeTime, readSocketBufferContext.DetectTimestamp);
                Assert.AreEqual(moqPerfLogger.Object, readSocketBufferContext.DispatchLatencyLogger);
                NonPublicInvocator.SetInstanceField(socketRingPollerListener, "Running", false);
            })
            .Returns(true).Verifiable();
        moqPerfLogger.Setup(ltcsl => ltcsl.AddContextMeasurement(1)).Verifiable();
        moqPerfLogger.Setup(ltcsl => ltcsl.Add("End Processing Socket Data")).Verifiable();
        moqSocketDataLatencyLogger.Setup(ltcsl => ltcsl.ParseTraceLog(moqPerfLogger.Object))
            .Verifiable();
        moqPerfPool.Setup(ltcsp => ltcsp.StopTrace(moqPerfLogger.Object)).Verifiable();
    }
}
