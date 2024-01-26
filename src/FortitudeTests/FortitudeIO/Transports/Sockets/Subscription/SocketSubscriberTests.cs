#region

using System.Reactive.Subjects;
using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.EventProcessing;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.AsyncWrappers;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Protocols.Serialization;
using FortitudeIO.Transports.Sockets;
using FortitudeIO.Transports.Sockets.Dispatcher;
using FortitudeIO.Transports.Sockets.Publishing;
using FortitudeIO.Transports.Sockets.SessionConnection;
using FortitudeIO.Transports.Sockets.Subscription;
using Moq;

#endregion

namespace FortitudeTests.FortitudeIO.Transports.Sockets.Subscription;

[TestClass]
public class SocketSubscriberTests
{
    private readonly string testCxFailureReason = "Test Failure Reason";
    private ISubject<IConnectionUpdate> configUpdateSubject = null!;
    private DummySocketSubscriber dummySocketSubscriber = null!;
    private Mock<IBinaryDeserializationFactory> moqBinaryDeserializationFactory = null!;
    private Mock<IBinaryStreamPublisher> moqBinaryStreamPublisher = null!;
    private Mock<ISocketDispatcher> moqDispatcher = null!;
    private Mock<IMessageStreamDecoder> moqFeedDecoder = null!;
    private Mock<IFLogger> moqFlogger = null!;
    private Mock<IOSNetworkingController> moqNetworkingController = null!;
    private Mock<IOSSocket> moqOsSocket = null!;
    private Mock<IOSParallelController> moqParallelControler = null!;
    private Mock<IOSParallelControllerFactory> moqParallelControllerFactory = null!;
    private Mock<IMap<uint, IMessageDeserializer>> moqSerializerCache = null!;
    private Mock<IConnectionConfig> moqServerConnectionConfig = null!;
    private bool onConnectedCalled;
    private bool onDisconnectedCalled;
    private bool onDisconnectingCalled;
    private int recvBufferSize;
    private string testHostName = null!;
    private int testHostPort;
    private string testSessionDescription = null!;
    private int wholeMessagesPerReceive;

    [TestInitialize]
    public void SetUp()
    {
        moqFlogger = new Mock<IFLogger>();
        moqDispatcher = new Mock<ISocketDispatcher>();
        moqParallelControler = new Mock<IOSParallelController>();
        moqParallelControllerFactory = new Mock<IOSParallelControllerFactory>();
        moqParallelControllerFactory.SetupGet(pcf => pcf.GetOSParallelController)
            .Returns(moqParallelControler.Object);
        OSParallelControllerFactory.Instance = moqParallelControllerFactory.Object;
        moqNetworkingController = new Mock<IOSNetworkingController>();
        moqServerConnectionConfig = new Mock<IConnectionConfig>();
        moqSerializerCache = new Mock<IMap<uint, IMessageDeserializer>>();
        testSessionDescription = "TestSessionDescription";
        wholeMessagesPerReceive = 23;
        recvBufferSize = 1234567;
        moqBinaryStreamPublisher = new Mock<IBinaryStreamPublisher>();
        moqFeedDecoder = new Mock<IMessageStreamDecoder>();
        moqBinaryDeserializationFactory = new Mock<IBinaryDeserializationFactory>();
        moqOsSocket = new Mock<IOSSocket>();
        configUpdateSubject = new Subject<IConnectionUpdate>();

        testHostName = "TestHostname";
        moqServerConnectionConfig.SetupGet(scc => scc.Hostname).Returns(testHostName);
        testHostPort = 1979;
        moqServerConnectionConfig.SetupGet(scc => scc.Port).Returns(testHostPort);
        moqServerConnectionConfig.SetupProperty(scc => scc.Updates, configUpdateSubject);
        moqFlogger.Setup(fl => fl.Info(It.IsAny<string>(), It.IsAny<object[]>()));
        moqOsSocket.SetupAllProperties();

        dummySocketSubscriber = new DummySocketSubscriber(moqFlogger.Object, moqDispatcher.Object,
            moqNetworkingController.Object, moqServerConnectionConfig.Object, testSessionDescription,
            wholeMessagesPerReceive, moqSerializerCache.Object, recvBufferSize, moqBinaryStreamPublisher.Object,
            moqFeedDecoder.Object, moqBinaryDeserializationFactory.Object, moqOsSocket.Object);
    }

    [TestCleanup]
    public void TearDown()
    {
        OSParallelControllerFactory.Instance = new OSParallelControllerFactory();
    }

    [TestMethod]
    public void NeverStarted_ConfigurationUpdate_CallsConnect()
    {
        ConnectMoqSetup();

        configUpdateSubject.OnNext(new ConnectionUpdate(moqServerConnectionConfig.Object,
            EventType.Updated));

        VerifyFullConnectedCalled();
    }

    [TestMethod]
    public void AlreadyConnected_ConfigurationUpdate_CallsDisconnectThenConnect()
    {
        ConnectMoqSetup();
        DisconnectMoqSetup();

        dummySocketSubscriber.Connect();

        onConnectedCalled = false;

        configUpdateSubject.OnNext(new ConnectionUpdate(moqServerConnectionConfig.Object,
            EventType.Updated));

        Assert.IsTrue(onDisconnectedCalled);
        VerifyFullConnectedCalled();
    }

    [TestMethod]
    public void NewSocketSubscriber_IsConnected_WhenSocketIsConnectedOrBound()
    {
        ConnectMoqSetup();
        dummySocketSubscriber.Connect();

        Assert.IsTrue(dummySocketSubscriber.IsConnected);
        moqOsSocket.SetupGet(oss => oss.Connected).Returns(false).Verifiable();
        Assert.IsFalse(dummySocketSubscriber.IsConnected);
        moqOsSocket.SetupGet(oss => oss.IsBound).Returns(true).Verifiable();
        Assert.IsTrue(dummySocketSubscriber.IsConnected);
        moqOsSocket.SetupGet(oss => oss.IsBound).Returns(false).Verifiable();
        Assert.IsFalse(dummySocketSubscriber.IsConnected);
    }


    [TestMethod]
    public void NewSocketStreamSubr_RegisterConnector_ReturnsSocketSessionConnection()
    {
        moqDispatcher.Setup(d => d.Listener.RegisterForListen(It.IsAny<ISocketSessionConnection>())).Verifiable();


        var socketSessionConnection = dummySocketSubscriber.RegisterConnector(moqOsSocket.Object);

        Assert.IsNotNull(socketSessionConnection);
        Assert.AreEqual(recvBufferSize, moqOsSocket.Object.ReceiveBufferSize);
        Assert.AreEqual(moqOsSocket.Object, socketSessionConnection.Socket);
        Assert.AreEqual(dummySocketSubscriber.ZeroBytesReadIsDisconnection,
            socketSessionConnection.SessionReceiver!.ZeroBytesReadIsDisconnection);
        Assert.AreEqual(dummySocketSubscriber.SessionDescription, socketSessionConnection.SessionDescription);
        moqDispatcher.Verify();
    }

    [TestMethod]
    public void UnconnectedSocketSubscriber_BlockedUntilConnected_Connects()
    {
        ConnectMoqSetup();
        dummySocketSubscriber.BlockUntilConnected();

        Assert.IsTrue(onConnectedCalled);
        Assert.AreEqual(dummySocketSubscriber.RecvBufferSize, moqOsSocket.Object.ReceiveBufferSize);
        moqFlogger.Verify();
        moqDispatcher.Verify();
        moqOsSocket.Verify();
    }

    [TestMethod]
    public void Connected_OnCxErrorWith0ms_CallDisconnectWithNoDisconnectingEventThenImmediatelyConnect()
    {
        ConnectMoqSetup();
        dummySocketSubscriber.BlockUntilConnected();

        ResetMoqs();
        ConnectMoqSetup();
        DisconnectMoqSetup();
        moqFlogger.Setup(fl => fl.Info("Closing connection {0}-{1} to {2}:{3} [{4}]", testSessionDescription,
            It.IsAny<long>(), testHostName, testHostPort, testCxFailureReason)).Verifiable();


        var moqSocketSessionConnection = new Mock<ISocketSessionConnection>();
        dummySocketSubscriber.CxErrorCallback(moqSocketSessionConnection.Object, testCxFailureReason, 0);

        Assert.IsTrue(onDisconnectedCalled);
        Assert.IsFalse(onDisconnectingCalled);
        VerifyFullConnectedCalled();
    }

    [TestMethod]
    public void ConnectedSocketSubscriber_OnCxErrorWith1000ms_CallDisconnectThenSchedulesConnect()
    {
        ConnectMoqSetup();
        dummySocketSubscriber.BlockUntilConnected();

        ResetMoqs();
        DisconnectMoqSetup();
        var osThreadSignal = new Mock<IIntraOSThreadSignal>();
        moqParallelControler.Setup(pc => pc.ScheduleWithEarlyTrigger(
            It.IsAny<WaitOrTimerCallback>(), It.IsAny<uint>(), true)).Returns(osThreadSignal.Object).Verifiable();
        onConnectedCalled = false;

        var moqSocketSessionConnection = new Mock<ISocketSessionConnection>();
        dummySocketSubscriber.CxErrorCallback(moqSocketSessionConnection.Object, testCxFailureReason, 1000);

        Assert.IsFalse(onConnectedCalled);
        Assert.IsTrue(onDisconnectedCalled);
        VerifyMoqsCalled();
    }

    [TestMethod]
    public void Connected_OnCxErrorTwiceWith1000ms_CallDisconnectThenSchedulesConnectOnlyOnce()
    {
        //Connect
        ConnectMoqSetup();
        dummySocketSubscriber.BlockUntilConnected();

        // Prepare mocks for first Cx error
        ResetMoqs();
        DisconnectMoqSetup();
        var osThreadSignal = new Mock<IIntraOSThreadSignal>();
        moqParallelControler.Setup(pc => pc.ScheduleWithEarlyTrigger(
            It.IsAny<WaitOrTimerCallback>(), It.IsAny<uint>(), true)).Returns(osThreadSignal.Object).Verifiable();
        onConnectedCalled = false;
        var moqSocketSessionConnection = new Mock<ISocketSessionConnection>();

        // First Cx Error
        dummySocketSubscriber.CxErrorCallback(moqSocketSessionConnection.Object, testCxFailureReason, 1000);
        // Assert first error performs as expected
        Assert.IsFalse(onConnectedCalled);
        Assert.IsTrue(onDisconnectedCalled);
        ResetMoqs();
        moqDispatcher.Setup(d => d.Listener.UnregisterForListen(It.IsAny<ISocketSessionConnection>())).Verifiable();
        onDisconnectedCalled = false;

        //second Cx error
        dummySocketSubscriber.CxErrorCallback(moqSocketSessionConnection.Object, testCxFailureReason, 1000);

        //Assert none of the first Cx values are called again
        moqParallelControler.Verify(pc => pc.ScheduleWithEarlyTrigger(
            It.IsAny<WaitOrTimerCallback>(), It.IsAny<uint>(), true), Times.Never);
        moqParallelControler.Verify(pc => pc.CallFromThreadPool(It.IsAny<WaitCallback>()), Times.Never);
        Assert.IsFalse(onConnectedCalled);
        Assert.IsTrue(onDisconnectedCalled);
    }

    [TestMethod]
    public void NewSocketSubscriber_Connect_DispatchesConnectOnBackgroundThreadImmediately()
    {
        ConnectMoqSetup();
        dummySocketSubscriber.Connect();
        VerifyFullConnectedCalled();
    }

    [TestMethod]
    public void ConnectingSubscriber_Connect2ndTime_IgnoresSecondConnectionRequest()
    {
        NewSocketSubscriber_Connect_DispatchesConnectOnBackgroundThreadImmediately();

        ResetMoqs();
        onConnectedCalled = false;
        dummySocketSubscriber.Connect();

        Assert.IsFalse(onConnectedCalled);
        moqDispatcher.Verify(d => d.Start(), Times.Never);
    }

    [TestMethod]
    public void ConnectedSocketSubscriber_Connect_SchedulesConnectOnlyOnce()
    {
        ConnectMoqSetup();
        dummySocketSubscriber.Connect();

        VerifyFullConnectedCalled();
    }

    [TestMethod]
    public void UnconnectedSocketSubscriber_ConnectFails_SchedulesBackgroundReconnectAtReconnectInterval()
    {
        ConnectMoqSetup();
        moqParallelControler.Reset();
        moqOsSocket.SetupGet(oss => oss.Connected).Returns(false).Verifiable();
        moqDispatcher.Setup(d => d.Listener.RegisterForListen(It.IsAny<ISocketSessionConnection>())).Verifiable();
        var osThreadSignal = new Mock<IIntraOSThreadSignal>();

        WaitOrTimerCallback? scheduledConnect = null;
        moqParallelControler.Setup(pc => pc.ScheduleWithEarlyTrigger(
                It.IsAny<WaitOrTimerCallback>(), It.IsAny<uint>(), true))
            .Callback<WaitOrTimerCallback, uint, bool>((callback, time, callOnce) => { scheduledConnect = callback; })
            .Returns(osThreadSignal.Object).Verifiable();
        moqServerConnectionConfig.SetupGet(scc => scc.ReconnectIntervalMs).Returns(250u);

        dummySocketSubscriber.BlockUntilConnected();

        moqParallelControler.Verify();

        ResetMoqs();
        ConnectMoqSetup();
        moqOsSocket.SetupSequence(oss => oss.Connected).Returns(false).Returns(true).Returns(true);
        moqParallelControler.Reset();

        scheduledConnect!.Invoke(new object(), false);

        VerifyFullConnectedCalled();
    }


    [TestMethod]
    public void UnconnectedSocketSubscriber_ConnectFailsFirstConfig_FallbackConfigConnects()
    {
        ConnectMoqSetup();
        moqOsSocket.SetupGet(oss => oss.Connected).Returns(false).Verifiable();
        moqDispatcher.Setup(d => d.Listener.RegisterForListen(It.IsAny<ISocketSessionConnection>())).Verifiable();
        var fallbackConfig = new Mock<IConnectionConfig>();
        const string testfallbackhostname = "TestFallbackHostName";
        fallbackConfig.SetupGet(scc => scc.Hostname).Returns(testfallbackhostname);
        const int fallbackPort = 1981;
        fallbackConfig.SetupGet(scc => scc.Port).Returns(fallbackPort);
        moqServerConnectionConfig.SetupGet(scc => scc.FallBackConnectionConfig)
            .Callback(() => { moqOsSocket.SetupGet(oss => oss.Connected).Returns(true).Verifiable(); })
            .Returns(fallbackConfig.Object);

        moqFlogger.Reset();
        moqFlogger.Setup(fl => fl.Info("Connection to id:{0} {1}:{2} accepted", testSessionDescription,
            It.IsAny<long>(), testfallbackhostname, fallbackPort)).Verifiable();

        dummySocketSubscriber.BlockUntilConnected();

        moqParallelControler.Reset();
        VerifyFullConnectedCalled();
    }


    [TestMethod]
    public void UnconnectedSocketSubscriber_ConnectThrowsExceptionFirstConfig_FallbackConfigConnects()
    {
        ConnectMoqSetup();
        moqOsSocket.SetupGet(oss => oss.Connected).Returns(false).Verifiable();
        var expectedException = new Exception("During connection");
        moqDispatcher.Setup(d => d.Listener.RegisterForListen(It.IsAny<ISocketSessionConnection>()))
            .Throws(expectedException)
            .Verifiable();
        var fallbackConfig = new Mock<IConnectionConfig>();
        const string testfallbackhostname = "TestFallbackHostName";
        fallbackConfig.SetupGet(scc => scc.Hostname).Returns(testfallbackhostname);
        const int fallbackPort = 1981;
        fallbackConfig.SetupGet(scc => scc.Port).Returns(fallbackPort);
        moqServerConnectionConfig.SetupGet(scc => scc.FallBackConnectionConfig)
            .Callback(() =>
            {
                moqOsSocket.SetupGet(oss => oss.Connected).Returns(true).Verifiable();
                moqDispatcher.Setup(d => d.Listener.RegisterForListen(It.IsAny<ISocketSessionConnection>()))
                    .Verifiable();
            })
            .Returns(fallbackConfig.Object);

        moqFlogger.Reset();
        moqFlogger.Setup(fl => fl.Info("Connection to {0} {1}:{2} rejected: {3}", testSessionDescription,
            testHostName, testHostPort, expectedException)).Verifiable();
        moqFlogger.Setup(fl => fl.Info("Connection to id:{0} {1}:{2} accepted", testSessionDescription,
            It.IsAny<long>(), testfallbackhostname, fallbackPort)).Verifiable();

        dummySocketSubscriber.BlockUntilConnected();

        moqParallelControler.Reset();
        VerifyFullConnectedCalled();
    }

    private void ConnectMoqSetup()
    {
        moqParallelControler.Setup(pc => pc.CallFromThreadPool(It.IsAny<WaitCallback>()))
            .Callback<WaitCallback>(wc => wc.Invoke(new object())).Verifiable();
        moqFlogger.Setup(fl => fl.Info("Connection to id:{0} {1}:{2} accepted", testSessionDescription,
            It.IsAny<long>(), testHostName, testHostPort)).Verifiable();
        moqDispatcher.Setup(d => d.Start()).Verifiable();
        moqDispatcher.Setup(d => d.Listener.RegisterForListen(It.IsAny<ISocketSessionConnection>())).Callback(() =>
        {
            moqOsSocket.SetupGet(oss => oss.Connected).Returns(true).Verifiable();
        }).Verifiable();
        moqOsSocket.SetupGet(oss => oss.Connected).Returns(true).Verifiable();

        onConnectedCalled = false;
        dummySocketSubscriber.OnConnected += () => { onConnectedCalled = true; };
    }

    private void VerifyFullConnectedCalled()
    {
        Assert.IsTrue(onConnectedCalled);
        Assert.AreEqual(dummySocketSubscriber.RecvBufferSize, moqOsSocket.Object.ReceiveBufferSize);
        VerifyMoqsCalled();
    }

    private void VerifyMoqsCalled()
    {
        moqFlogger.Verify();
        moqDispatcher.Verify();
        moqOsSocket.Verify();
        moqParallelControler.Verify();
    }

    private void DisconnectMoqSetup()
    {
        moqFlogger.Setup(fl => fl.Info("Connection to {0} {1} id {2}:{3} closed", testSessionDescription,
            It.IsAny<long>(), testHostName, testHostPort)).Verifiable();
        moqDispatcher.Setup(d => d.Stop()).Verifiable();
        moqDispatcher.Setup(d => d.Listener.UnregisterForListen(It.IsAny<ISocketSessionConnection>())).Verifiable();
        moqOsSocket.Setup(oss => oss.Close()).Callback(() =>
            {
                moqOsSocket.SetupGet(oss => oss.Connected).Returns(false).Verifiable();
            })
            .Verifiable();

        onDisconnectedCalled = false;
        dummySocketSubscriber.OnDisconnected += () => { onDisconnectedCalled = true; };
        onDisconnectingCalled = false;
        dummySocketSubscriber.OnDisconnecting += () => { onDisconnectingCalled = true; };
    }

    private void ResetMoqs()
    {
        moqParallelControler.Reset();
        moqFlogger.Reset();
        moqDispatcher.Reset();
        moqOsSocket.Reset();
        moqOsSocket.SetupAllProperties();
    }

    public class DummySocketSubscriber : SocketSubscriber
    {
        private readonly IBinaryDeserializationFactory binaryDeserializationFactory;
        private readonly IOSSocket socket;
        private readonly IMessageStreamDecoder streamMessageStreamDecoder;

        public DummySocketSubscriber(IFLogger logger, ISocketDispatcher dispatcher,
            IOSNetworkingController networkingController, IConnectionConfig connectionConfig,
            string sessionDescription, int wholeMessagesPerReceive,
            IMap<uint, IMessageDeserializer> serializerCache, int recvBufferSize,
            IBinaryStreamPublisher streamToPublisher, IMessageStreamDecoder streamMessageStreamDecoder,
            IBinaryDeserializationFactory factory, IOSSocket socket)
            : base(logger, dispatcher, networkingController, connectionConfig,
                sessionDescription, wholeMessagesPerReceive, serializerCache)
        {
            this.streamMessageStreamDecoder = streamMessageStreamDecoder;
            RecvBufferSize = recvBufferSize;
            StreamToPublisher = streamToPublisher;
            binaryDeserializationFactory = factory;
            this.socket = socket;
        }

        public override int RecvBufferSize { get; }

        public override IBinaryStreamPublisher StreamToPublisher { get; }

        public Action<ISocketSessionConnection, string, int> CxErrorCallback => OnCxError;

        public override IMessageStreamDecoder GetDecoder(IMap<uint, IMessageDeserializer> decoderDeserializers) =>
            streamMessageStreamDecoder;

        protected override IBinaryDeserializationFactory GetFactory() => binaryDeserializationFactory;

        protected override IOSSocket CreateAndConnect(string host, int port) => socket;
    }
}
