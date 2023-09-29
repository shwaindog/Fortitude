using System;
using System.Collections.Generic;
using System.Threading;
using FortitudeCommon.Monitoring.Logging.Diagnostics.Performance;
using FortitudeCommon.OSWrapper.AsyncWrappers;
using FortitudeCommon.Types;
using FortitudeIO.Protocols.Serialization;
using FortitudeIO.Transports.Sockets;
using FortitudeIO.Transports.Sockets.Dispatcher;
using FortitudeIO.Transports.Sockets.Logging;
using FortitudeIO.Transports.Sockets.SessionConnection;
using FortitudeIO.Transports.Sockets.Subscription;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FortitudeTests.FortitudeIO.Transports.Sockets.Dispatcher
{
    [TestClass]
    public class SocketDispatcherListenerTests
    {
        private Mock<IOSThread> moqOsThread;
        private Mock<IOSParallelControllerFactory> moqParallelControllerFactory;
        private Mock<IOSParallelController> moqParallelController;
        private Mock<ISocketSelector> moqSocketSelector;
        private Mock<ISocketSessionConnection> moqSocketSessionConnection;
        private Mock<ISocketSessionReceiver> moqSocketSessionReceiver;
        private Mock<IPerfLoggingPoolFactory> moqPerfFac;
        private Mock<IPerfLoggerPool> moqPerfPool;
        private Mock<IPerfLogger> moqPerfLogger;
        private Mock<ISocketDataLatencyLogger> moqSocketDataLatencyLogger;
        private Mock<ISocketDataLatencyLoggerFactory> moqSocketDataLatencyFactory;
        private SocketDispatcherListener socketDispatcherListener;
        private ThreadStart rootMethod;
        private List<ISocketSessionConnection> firstListOfSocketsWithUpdate;
        private DateTime wakeTime;
        private DispatchContext dispatchContext;

        [TestInitialize]
        public void SetUp()
        {
            moqParallelControllerFactory = new Mock<IOSParallelControllerFactory>();
            moqParallelController = new Mock<IOSParallelController>();
            moqParallelControllerFactory.SetupGet(pcf => pcf.GetOSParallelController)
                .Returns(moqParallelController.Object);
            OSParallelControllerFactory.Instance = moqParallelControllerFactory.Object;
            moqOsThread = new Mock<IOSThread>();
            moqSocketSelector = new Mock<ISocketSelector>();
            moqParallelController.Setup(pc => pc.CreateNewOSThread(It.IsAny<ThreadStart>()))
                .Callback<ThreadStart>(threadRootMethod => { rootMethod = threadRootMethod; })
                .Returns(moqOsThread.Object).Verifiable();
            moqPerfFac = new Mock<IPerfLoggingPoolFactory>();
            moqPerfPool = new Mock<IPerfLoggerPool>();
            moqPerfFac.Setup(fac => fac.GetLatencyTracingLoggerPool(It.IsAny<string>(), It.IsAny<TimeSpan>(),
                It.IsAny<Type>())).Returns(moqPerfPool.Object).Verifiable();
            moqSocketDataLatencyLogger = new Mock<ISocketDataLatencyLogger>();
            moqSocketDataLatencyFactory = new Mock<ISocketDataLatencyLoggerFactory>();
            moqSocketDataLatencyFactory.Setup(sdlf => sdlf.GetSocketDataLatencyLogger("TestDescription")).Returns(
                moqSocketDataLatencyLogger.Object);

            PerfLoggingPoolFactory.Instance = moqPerfFac.Object;
            SocketDataLatencyLoggerFactory.Instance = moqSocketDataLatencyFactory.Object;

            socketDispatcherListener = new SocketDispatcherListener(moqSocketSelector.Object, "TestDescription");

            moqSocketSessionConnection = new Mock<ISocketSessionConnection>();
            moqSocketSessionReceiver = new Mock<ISocketSessionReceiver>();
            moqSocketSessionConnection.SetupGet(ssc => ssc.SessionReceiver).Returns(moqSocketSessionReceiver.Object);
            dispatchContext = NonPublicInvocator.GetInstanceField<DispatchContext>(
                socketDispatcherListener, "dispatchContext");
        }

        [TestCleanup]
        public void TearDown()
        {
            OSParallelControllerFactory.Instance = null;
            SocketDataLatencyLoggerFactory.Instance = null;
            PerfLoggingPoolFactory.Instance = null;
        }

        [TestMethod]
        public void NewDispatcherListenerWithDescription_New_ReceivesLatencyTraceCallsLoggerPool()
        {
            moqPerfFac.Verify();
        }

        [TestMethod]
        public void NewDispatcherListener_RegisterForLister_RegistersSocketSessionConnectionWithSelector()
        {
            moqSocketSelector.Setup(ss => ss.Register(moqSocketSessionConnection.Object)).Verifiable();
            moqSocketSessionConnection.SetupSet(ssc => ssc.Active = true).Verifiable();

            socketDispatcherListener.RegisterForListen(moqSocketSessionConnection.Object);

            moqSocketSessionConnection.Verify();
            moqSocketSelector.Verify();
        }

        [TestMethod]
        public void NewDispatcherListener_UnregisterForListen_UnregistersSocketSessionConnectionWithSelector()
        {
            moqSocketSelector.Setup(ss => ss.Unregister(moqSocketSessionConnection.Object)).Verifiable();
            moqSocketSessionConnection.SetupSet(ssc => ssc.Active = false).Verifiable();

            socketDispatcherListener.UnregisterForListen(moqSocketSessionConnection.Object);

            moqSocketSessionConnection.Verify();
            moqSocketSelector.Verify();
        }

        [TestMethod]
        public void SocketWithUpdate_ReceiveSuccessfullyCallsReceiveData_NormalProcessingOccurs()
        {
            PrepareOneSuccessfulReceive();
            socketDispatcherListener.Start();
            rootMethod();

            moqParallelController.Verify();
            moqOsThread.Verify();
            moqPerfPool.Verify();
            moqSocketSelector.Verify();
            moqSocketSessionConnection.Verify();
            moqPerfLogger.Verify();
            moqSocketDataLatencyLogger.Verify();
            moqSocketSessionReceiver.Verify();
        }

        [TestMethod]
        public void SocketWithUpdate_ReceiveDataReturnsFalse_ErrorIsLoggedAndConnectionErrorIsRaised()
        {
            PrepareOneSuccessfulReceive();
            moqSocketSessionReceiver.Setup(ssc => ssc.ReceiveData(dispatchContext))
                .Callback(() =>
                {
                    Assert.AreEqual(wakeTime, dispatchContext.DetectTimestamp);
                    Assert.AreEqual(moqPerfLogger.Object, dispatchContext.DispatchLatencyLogger);
                    NonPublicInvocator.SetInstanceField(socketDispatcherListener, "Running", false);
                })
                .Returns(false).Verifiable();
            moqSocketSessionConnection.Setup(scc =>
                scc.OnError(moqSocketSessionConnection.Object, It.IsRegex("Connection lost on dispatcher .+"), -1))
                .Verifiable();
            moqPerfLogger.Reset();
            moqPerfLogger.Setup(ltcsl => ltcsl.Indent()).Verifiable();
            moqPerfLogger.Setup(ltcsl => ltcsl.Add(It.IsRegex("Connection lost on dispatcher .+")))
                .Verifiable();
            moqPerfLogger.Setup(ltcsl => ltcsl.Dedent()).Verifiable();
            moqPerfLogger.Setup(ltcsl => ltcsl.AddContextMeasurement(1)).Verifiable();
            moqPerfLogger.Setup(ltcsl => ltcsl.Add("End Processing Socket Data")).Verifiable();
            socketDispatcherListener.Start();
            rootMethod();

            moqParallelController.Verify();
            moqOsThread.Verify();
            moqPerfPool.Verify();
            moqSocketSelector.Verify();
            moqSocketSessionConnection.Verify();
            moqSocketSessionReceiver.Verify();
            moqPerfLogger.Verify();
            moqSocketDataLatencyLogger.Verify();
        }

        [TestMethod]
        public void TwoSocketsWithUpdate_ReceiveFirstSocketThrowsException_SecondSocketIsProcessedNormally()
        {
            PrepareOneSuccessfulReceive();

            Mock<ISocketSessionConnection> moqSocketSessionThrowsException = new Mock<ISocketSessionConnection>();
            Mock<ISocketSessionReceiver> moqSocketSessionReceiverThrowsException = new Mock<ISocketSessionReceiver>();
            moqSocketSessionThrowsException.SetupGet(ssc => ssc.SessionReceiver)
                .Returns(moqSocketSessionReceiverThrowsException.Object);
            moqSocketSessionReceiverThrowsException.SetupGet(ssc => ssc.IsAcceptor).Returns(false).Verifiable();
            moqSocketSessionReceiverThrowsException.Setup(ssc => 
                ssc.ReceiveData(dispatchContext))
                .Throws(new SocketBufferTooFullException("Test Socket Buffer full")).Verifiable();
            moqSocketSessionThrowsException.Setup(scc => 
                scc.OnError(moqSocketSessionThrowsException.Object, It.IsRegex("Read error:.+"), 0)).Verifiable();
            firstListOfSocketsWithUpdate.Insert(0, moqSocketSessionThrowsException.Object);
            moqPerfLogger.Reset();
            moqPerfLogger.Setup(ltcsl => ltcsl.Indent()).Verifiable();
            moqPerfLogger.Setup(ltcsl => ltcsl.Add("Read error: ", It.IsAny<Exception>())).Verifiable();
            moqPerfLogger.Setup(ltcsl => ltcsl.Dedent()).Verifiable();
            moqPerfLogger.Setup(ltcsl => ltcsl.AddContextMeasurement(2)).Verifiable();
            moqPerfLogger.Setup(ltcsl => ltcsl.Add("End Processing Socket Data")).Verifiable();
            socketDispatcherListener.Start();
            rootMethod();

            moqParallelController.Verify();
            moqOsThread.Verify();
            moqPerfPool.Verify();
            moqSocketSelector.Verify();
            moqSocketSessionConnection.Verify();
            moqPerfLogger.Verify();
            moqSocketDataLatencyLogger.Verify();
            moqSocketSessionThrowsException.Verify();
            moqSocketSessionReceiverThrowsException.Verify();
        }

        [TestMethod]
        public void SocketWithUpdate_SelectRecvThrowsException_RecoversAndCallsAgain()
        {
            PrepareOneSuccessfulReceive();
            moqSocketSelector.Reset();
            moqSocketSelector.SetupSequence(ss => ss.WatchSocketsForRecv(moqPerfLogger.Object))
                .Throws<Exception>().Returns(firstListOfSocketsWithUpdate);
            moqSocketSelector.SetupGet(ss => ss.WakeTs).Returns(wakeTime).Verifiable();
            socketDispatcherListener.Start();
            rootMethod();

            moqParallelController.Verify();
            moqOsThread.Verify();
            moqPerfPool.Verify();
            moqSocketSelector.Verify();
            moqSocketSessionConnection.Verify();
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
            socketDispatcherListener.Start();
            rootMethod();

            moqParallelController.Verify();
            moqOsThread.Verify();
            moqPerfPool.Verify();
            moqSocketSelector.Verify();
            moqSocketSessionConnection.Verify();
            moqPerfLogger.Verify();
            moqSocketDataLatencyLogger.Verify();
        }

        [TestMethod]
        public void AcceptorSocket_Receive_SocketSelectorReturnsSocketReceiveDataIsCalled()
        {
            PrepareOneSuccessfulReceive();
            moqSocketSessionReceiver.Reset();
            moqSocketSessionReceiver.SetupGet(ssc => ssc.IsAcceptor).Returns(true).Verifiable();
            moqSocketSessionReceiver.Setup(ssc => ssc.OnAccept())
                .Callback(() =>
                {
                    NonPublicInvocator.SetInstanceField(socketDispatcherListener, "Running", false);
                }).Verifiable();

            socketDispatcherListener.Start();
            rootMethod();

            moqParallelController.Verify();
            moqOsThread.Verify();
            moqPerfPool.Verify();
            moqSocketSessionConnection.Verify();
            moqSocketSessionReceiver.Verify();
            moqPerfLogger.Verify();
            moqSocketDataLatencyLogger.Verify();
        }

        private void PrepareOneSuccessfulReceive()
        {
            firstListOfSocketsWithUpdate = new List<ISocketSessionConnection> {moqSocketSessionConnection.Object};

            moqOsThread.SetupSet(ost => ost.Name = It.IsRegex(@"SocketReceivingThread\d+")).Verifiable();
            moqOsThread.SetupSet(ost => ost.IsBackground = true).Verifiable();
            moqOsThread.Setup(ost => ost.Start()).Verifiable();
            moqPerfLogger = new Mock<IPerfLogger>();
            moqPerfPool.Setup(pool => pool.StartNewTrace())
                .Returns(moqPerfLogger.Object).Verifiable();
            moqSocketSelector.Setup(ss => ss.WatchSocketsForRecv(moqPerfLogger.Object))
                .Returns(firstListOfSocketsWithUpdate).Verifiable();
            moqSocketSessionReceiver.SetupGet(ssc => ssc.IsAcceptor).Returns(false).Verifiable();
            wakeTime = new DateTime(2017, 04, 21, 22, 33, 23);
            moqSocketSelector.SetupGet(ss => ss.WakeTs).Returns(wakeTime).Verifiable();
            moqSocketSessionReceiver.Setup(ssc => ssc.ReceiveData(dispatchContext))
                .Callback(() =>
                {
                    Assert.AreEqual(wakeTime, dispatchContext.DetectTimestamp);
                    Assert.AreEqual(moqPerfLogger.Object, dispatchContext.DispatchLatencyLogger);
                    NonPublicInvocator.SetInstanceField(socketDispatcherListener, "Running", false);
                })
                .Returns(true).Verifiable();
            moqPerfLogger.Setup(ltcsl => ltcsl.AddContextMeasurement(1)).Verifiable();
            moqPerfLogger.Setup(ltcsl => ltcsl.Add("End Processing Socket Data")).Verifiable();
            moqSocketDataLatencyLogger.Setup(ltcsl => ltcsl.ParseTraceLog(moqPerfLogger.Object))
                .Verifiable();
            moqPerfPool.Setup(ltcsp => ltcsp.StopTrace(moqPerfLogger.Object)).Verifiable();
        }
    }
}