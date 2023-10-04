#region

using FortitudeCommon.AsyncProcessing;
using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.OSWrapper.AsyncWrappers;
using FortitudeCommon.Types;
using FortitudeIO.Transports.Sockets.Dispatcher;
using FortitudeIO.Transports.Sockets.SessionConnection;
using Moq;

#endregion

namespace FortitudeTests.FortitudeIO.Transports.Sockets.Dispatcher;

[TestClass]
public class SocketDispatcherSenderTests
{
    private const long WellKnownSocketId = 12345;
    private bool inSyncLock;
    private Mock<IIntraOSThreadSignal> moqInterOSThreadSignal = null!;
    private Mock<IOSThread> moqOsThread = null!;
    private Mock<IOSParallelController> moqParallelController = null!;
    private Mock<IOSParallelControllerFactory> moqParallelControllerFactory = null!;
    private Mock<ISocketSessionConnection> moqSocketSessionConnection = null!;
    private Mock<ISocketSessionSender> moqSocketSessionSender = null!;
    private Mock<ISyncLock> moqSyncLock = null!;
    private Mock<IDictionary<long, ISocketSessionConnection>> moqToWrite = null!;
    private IDictionary<long, ISocketSessionConnection> originalToWrite = null!;
    private ThreadStart rootMethod = null!;
    private SocketDispatcherSender socketDispatcherSender = null!;

    [TestInitialize]
    public void SetUp()
    {
        moqParallelControllerFactory = new Mock<IOSParallelControllerFactory>();
        moqParallelController = new Mock<IOSParallelController>();
        moqParallelControllerFactory.SetupGet(pcf => pcf.GetOSParallelController)
            .Returns(moqParallelController.Object);
        OSParallelControllerFactory.Instance = moqParallelControllerFactory.Object;
        moqOsThread = new Mock<IOSThread>();
        moqParallelController.Setup(pc => pc.CreateNewOSThread(It.IsAny<ThreadStart>()))
            .Callback<ThreadStart>(threadRootMethod => { rootMethod = threadRootMethod; })
            .Returns(moqOsThread.Object).Verifiable();

        socketDispatcherSender = new SocketDispatcherSender("TestDescription");

        inSyncLock = false;
        PostDispatcherSenderConstructionTestInitialization();
    }

    [TestCleanup]
    public void TearDown()
    {
        OSParallelControllerFactory.Instance = new OSParallelControllerFactory();
    }

    private void PostDispatcherSenderConstructionTestInitialization()
    {
        moqSyncLock = new Mock<ISyncLock>();
        NonPublicInvocator.SetInstanceField(socketDispatcherSender!, "sendLock", moqSyncLock.Object);
        moqSyncLock.Setup(sl => sl.Acquire()).Callback(() => { inSyncLock = true; }).Verifiable();
        moqSyncLock.Setup(sl => sl.Release()).Callback(() => { inSyncLock = false; }).Verifiable();

        moqInterOSThreadSignal = new Mock<IIntraOSThreadSignal>();
        NonPublicInvocator.SetInstanceField(socketDispatcherSender!, "canSend", moqInterOSThreadSignal.Object);


        moqSocketSessionConnection = new Mock<ISocketSessionConnection>();
        moqSocketSessionSender = new Mock<ISocketSessionSender>();
        moqSocketSessionConnection.SetupAllProperties();
        moqSocketSessionConnection.SetupGet(ssc => ssc.SessionSender).Returns(moqSocketSessionSender.Object);
        moqSocketSessionConnection.SetupGet(ssc => ssc.Id).Returns(WellKnownSocketId).Verifiable();
    }

    [TestMethod]
    public void NewDispatcherSender_New_CreatesOSThreadSignalSetsApplicationType()
    {
        var canSendOSInterThreadSignal = NonPublicInvocator.GetInstanceField<IIntraOSThreadSignal>(
            socketDispatcherSender!, "canSend");
        Assert.IsNotNull(canSendOSInterThreadSignal);
    }

    [TestMethod]
    public void SocketSessionWithMessageToSend_AddToSendQueue_AddsSessionToWriteDictionary()
    {
        moqToWrite = new Mock<IDictionary<long, ISocketSessionConnection>>();
        NonPublicInvocator.SetInstanceField(socketDispatcherSender!, "toWrite", moqToWrite.Object);
        moqToWrite.SetupSet(ssd => ssd[WellKnownSocketId] = moqSocketSessionConnection.Object)
            .Callback(() => { Assert.IsTrue(inSyncLock); })
            .Verifiable();
        moqInterOSThreadSignal.Setup(iosts => iosts.Set()).Verifiable();

        socketDispatcherSender.AddToSendQueue(moqSocketSessionConnection!.Object);
        Assert.IsFalse(inSyncLock);
        moqSyncLock!.Verify();
        moqInterOSThreadSignal.Verify();
        moqToWrite.Verify();
    }

    [TestMethod]
    public void TwoSocketSessionWithMessageToSend_AddToSendQueue_AddsSessionsToWriteDictionary()
    {
        moqToWrite = new Mock<IDictionary<long, ISocketSessionConnection>>();
        NonPublicInvocator.SetInstanceField(socketDispatcherSender!, "toWrite", moqToWrite.Object);
        var moqSecondSocketSessionConnection = new Mock<ISocketSessionConnection>();
        moqSecondSocketSessionConnection.SetupAllProperties();
        const long secondSocketId = 5678;
        moqSecondSocketSessionConnection.SetupGet(ssc => ssc.Id).Returns(secondSocketId).Verifiable();

        moqToWrite.SetupSet(ssd => ssd[WellKnownSocketId] = moqSocketSessionConnection.Object)
            .Callback(() => { Assert.IsTrue(inSyncLock); })
            .Verifiable();
        moqToWrite.SetupSet(ssd => ssd[secondSocketId] = moqSecondSocketSessionConnection.Object)
            .Callback(() => { Assert.IsTrue(inSyncLock); })
            .Verifiable();
        moqInterOSThreadSignal.Setup(iosts => iosts.Set()).Verifiable();
        var socketSessionConnections = new DoublyLinkedList<ISocketSessionConnection>();
        socketSessionConnections.AddFirst(moqSecondSocketSessionConnection.Object);
        socketSessionConnections.AddFirst(moqSocketSessionConnection!.Object);

        socketDispatcherSender.AddToSendQueue(socketSessionConnections);
        Assert.IsFalse(inSyncLock);
        moqSyncLock.Verify();
        moqInterOSThreadSignal.Verify();
        moqToWrite.Verify();
    }

    [TestMethod]
    public void ToWriteHasSocketSessionConnection_Send_CallsSendDataAndClearsWriting()
    {
        PrepareMoqsForOneSuccessfulSend();

        socketDispatcherSender!.Start();
        rootMethod!();

        moqParallelController!.Verify();
        moqOsThread!.Verify();
        moqInterOSThreadSignal!.Verify();
        moqSocketSessionConnection!.Verify();
        moqSocketSessionSender!.Verify();
        var writingEndOfSend = NonPublicInvocator
            .GetInstanceField<IDictionary<long, ISocketSessionConnection>?>(socketDispatcherSender, "writing");
        Assert.IsTrue(!(writingEndOfSend?.Any() ?? false));
    }

    [TestMethod]
    public void ToWriteHasSocketSessionConnection_SendDataReturnsFalse_RequeuesToWrite()
    {
        PrepareMoqsForOneSuccessfulSend();
        moqSocketSessionConnection!.Reset();
        moqSocketSessionConnection!.SetupAllProperties();
        moqSocketSessionConnection!.SetupGet(ssc => ssc.SessionSender).Returns(moqSocketSessionSender!.Object);
        moqSocketSessionConnection!.SetupGet(ssc => ssc.Id).Returns(WellKnownSocketId).Verifiable();
        moqSocketSessionConnection!.SetupGet(ssc => ssc.Active).Callback(() =>
            {
                NonPublicInvocator.SetInstanceField(socketDispatcherSender!, "Running", false);
            })
            .Returns(true).Verifiable();
        moqSocketSessionSender.Setup(ssc => ssc.SendData()).Returns(false).Verifiable();

        socketDispatcherSender!.Start();
        rootMethod!();

        var newToWrite = NonPublicInvocator
            .GetInstanceField<IDictionary<long, ISocketSessionConnection>>(socketDispatcherSender, "toWrite");
        Assert.IsTrue(newToWrite.ContainsKey(WellKnownSocketId));
        moqParallelController!.Verify();
        moqOsThread!.Verify();
        moqInterOSThreadSignal!.Verify();
        moqSocketSessionConnection.Verify();
        moqSocketSessionSender.Verify();
    }

    [TestMethod]
    public void ToWriteHasTwoSocketSessionConnections_SendDataThrowsException_SecondSocketSendsSuccessfully()
    {
        PrepareMoqsForOneSuccessfulSend();
        moqSocketSessionConnection!.Reset();
        moqSocketSessionConnection!.SetupAllProperties();
        moqSocketSessionConnection!.SetupGet(ssc => ssc.Active).Callback(() =>
        {
            NonPublicInvocator.SetInstanceField(socketDispatcherSender!, "Running", false);
        }).Returns(true).Verifiable();
        moqSocketSessionConnection!.SetupGet(ssc => ssc.SessionSender).Returns(moqSocketSessionSender.Object);
        moqSocketSessionSender.Setup(ssc => ssc.SendData()).Throws(new Exception()).Verifiable();
        var moqSecondSocketSessionConnection = new Mock<ISocketSessionConnection>();
        var moqSecondSocketSessionSender = new Mock<ISocketSessionSender>();
        moqSecondSocketSessionConnection.SetupAllProperties();
        moqSecondSocketSessionConnection.SetupGet(ssc => ssc.SessionSender)
            .Returns(moqSecondSocketSessionSender.Object);
        const long secondSocketId = 5678;
        moqSecondSocketSessionConnection.SetupGet(ssc => ssc.Id).Returns(secondSocketId).Verifiable();
        moqSecondSocketSessionConnection.SetupGet(ssc => ssc.Active).Returns(true).Verifiable();
        moqSecondSocketSessionSender.Setup(ssc => ssc.SendData()).Returns(true).Verifiable();
        originalToWrite![moqSecondSocketSessionConnection.Object.Id] = moqSecondSocketSessionConnection.Object;

        socketDispatcherSender!.Start();
        rootMethod!();

        moqParallelController!.Verify();
        moqOsThread!.Verify();
        moqInterOSThreadSignal!.Verify();
        moqSocketSessionConnection!.Verify();
        moqSocketSessionSender.Verify();
        moqSecondSocketSessionConnection.Verify();
        moqSecondSocketSessionSender.Verify();
    }

    [TestMethod]
    public void ToWriteSocketSessionConnection_SocketNoLongerActive_SkipsCallToSendData()
    {
        PrepareMoqsForOneSuccessfulSend();
        moqSocketSessionConnection!.Reset();
        moqSocketSessionConnection!.SetupAllProperties();
        moqSocketSessionConnection!.SetupGet(ssc => ssc.Active).Callback(() =>
        {
            NonPublicInvocator.SetInstanceField(socketDispatcherSender!, "Running", false);
        }).Returns(false).Verifiable();

        socketDispatcherSender!.Start();
        rootMethod!();

        moqParallelController!.Verify();
        moqOsThread!.Verify();
        moqInterOSThreadSignal!.Verify();
        moqSocketSessionConnection.Verify();
        moqSocketSessionSender!.Verify(ssc => ssc.SendData(), Times.Never);
    }

    private void PrepareMoqsForOneSuccessfulSend()
    {
        moqOsThread!.SetupSet(ost => ost.Name = It.IsRegex(@"SocketSendingThread\d+")).Verifiable();
        moqOsThread.SetupSet(ost => ost.IsBackground = true).Verifiable();
        moqOsThread.Setup(ost => ost.Start()).Verifiable();
        moqInterOSThreadSignal!.Setup(iosts => iosts.WaitOne()).Verifiable();
        originalToWrite = NonPublicInvocator
            .GetInstanceField<IDictionary<long, ISocketSessionConnection>>(socketDispatcherSender!, "toWrite");
        originalToWrite[moqSocketSessionConnection!.Object.Id] = moqSocketSessionConnection.Object;
        moqSocketSessionConnection.SetupGet(ssc => ssc.Active).Callback(() =>
            {
                NonPublicInvocator.SetInstanceField(socketDispatcherSender!, "Running", false);
            })
            .Returns(true).Verifiable();
        moqSocketSessionSender!.Setup(ssc => ssc.SendData()).Returns(true).Verifiable();
    }
}
