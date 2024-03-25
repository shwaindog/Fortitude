#region

using System.Net;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.AsyncWrappers;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeIO.Transports.NewSocketAPI.Config;
using FortitudeIO.Transports.NewSocketAPI.Controls;
using FortitudeIO.Transports.NewSocketAPI.Dispatcher;
using FortitudeIO.Transports.NewSocketAPI.Sockets;
using Moq;

#endregion

namespace FortitudeTests.FortitudeIO.Transports.NewSocketAPI.Controls;

[TestClass]
public class InitiateControlsTests
{
    private IPEndPoint connectedIpEndPoint = null!;
    private InitiateControls initiateControls = null!;
    private Mock<ISocketDispatcher> moqDispatcher = null!;
    private Mock<IFLogger> moqFlogger = null!;
    private Mock<IFLoggerFactory> moqFloggerFactory = null!;
    private Mock<IIntraOSThreadSignal> moqIntraOsThreadSignal = null!;
    private Mock<IOSSocket> moqOsSocket = null!;
    private Mock<IOSParallelController> moqParallelControler = null!;
    private Mock<IOSParallelControllerFactory> moqParallelControllerFactory = null!;
    private Mock<ISocketConnection> moqSocketConnection = null!;
    private Mock<ISocketConnectionConfig> moqSocketConnectionConfig = null!;
    private Mock<ISocketFactories> moqSocketFactories = null!;
    private Mock<ISocketFactory> moqSocketFactory = null!;
    private Mock<ISocketReconnectConfig> moqSocketReconnectConfig = null!;
    private Mock<ISocketSessionContext> moqSocketSessionContext = null!;
    private Mock<ISocketTopicConnectionConfig> moqSocketTopicConnectionConfig = null!;
    private string testHostName = null!;
    private ushort testHostPort;

    [TestInitialize]
    public void SetUp()
    {
        testHostName = "TestHostname";
        testHostPort = 1979;
        moqFlogger = new Mock<IFLogger>();
        moqFloggerFactory = new Mock<IFLoggerFactory>();
        moqDispatcher = new Mock<ISocketDispatcher>();
        moqParallelControler = new Mock<IOSParallelController>();
        moqParallelControllerFactory = new Mock<IOSParallelControllerFactory>();
        moqIntraOsThreadSignal = new Mock<IIntraOSThreadSignal>();
        moqSocketTopicConnectionConfig = new Mock<ISocketTopicConnectionConfig>();
        moqSocketConnectionConfig = new Mock<ISocketConnectionConfig>();
        moqSocketReconnectConfig = new Mock<ISocketReconnectConfig>();
        moqParallelControllerFactory.SetupGet(pcf => pcf.GetOSParallelController)
            .Returns(moqParallelControler.Object);

        moqOsSocket = new Mock<IOSSocket>();
        moqSocketSessionContext = new Mock<ISocketSessionContext>();
        moqSocketFactories = new Mock<ISocketFactories>();
        moqSocketFactory = new Mock<ISocketFactory>();
        connectedIpEndPoint = new IPEndPoint(new IPAddress(new byte[] { 127, 0, 0, 1 }), testHostPort);
        moqSocketConnection = new Mock<ISocketConnection>();

        moqSocketReconnectConfig.SetupGet(scc => scc.NextReconnectIntervalMs).Returns(5u);
        moqSocketConnectionConfig = new Mock<ISocketConnectionConfig>();
        moqSocketSessionContext.SetupGet(ssc => ssc.SocketFactories).Returns(moqSocketFactories.Object);
        moqSocketFactories.SetupGet(ssc => ssc.ParallelController).Returns(moqParallelControler.Object);
        moqSocketConnectionConfig.SetupGet(scc => scc.InstanceName).Returns("InitiateControlsTests");
        moqSocketConnectionConfig.SetupGet(scc => scc.Hostname).Returns(testHostName);
        moqSocketConnectionConfig.SetupGet(scc => scc.Port).Returns(testHostPort);
        moqSocketTopicConnectionConfig.Setup(stcc => stcc.GetEnumerator())
            .Returns(moqSocketTopicConnectionConfig.Object);
        moqSocketTopicConnectionConfig.SetupGet(stcc => stcc.Current).Returns(moqSocketConnectionConfig.Object);
        moqSocketTopicConnectionConfig.SetupSequence(stcc => stcc.MoveNext()).Returns(true).Returns(false);
        moqSocketTopicConnectionConfig.SetupGet(scc => scc.ReconnectConfig).Returns(moqSocketReconnectConfig.Object);
        moqSocketTopicConnectionConfig.SetupGet(scc => scc.TopicDescription).Returns("TestSessionDescription");

        moqSocketSessionContext.SetupGet(ssc => ssc.SocketTopicConnectionConfig)
            .Returns(moqSocketTopicConnectionConfig.Object);
        moqSocketSessionContext.SetupGet(ssc => ssc.SocketDispatcher).Returns(moqDispatcher.Object);

        moqSocketFactories.SetupGet(ssc => ssc.SocketFactory).Returns(moqSocketFactory.Object);
        moqFlogger.Setup(fl => fl.Info(It.IsAny<string>(), It.IsAny<object[]>()));
        moqOsSocket.SetupAllProperties();

        moqFloggerFactory.Setup(ff => ff.GetLogger(It.IsAny<Type>())).Returns(moqFlogger.Object);
        OSParallelControllerFactory.Instance = moqParallelControllerFactory.Object;
        FLoggerFactory.Instance = moqFloggerFactory.Object;

        initiateControls = new InitiateControls(moqSocketSessionContext.Object);
    }

    [TestCleanup]
    public void TearDown()
    {
        FLoggerFactory.Instance = new FLoggerFactory();
        OSParallelControllerFactory.Instance = new OSParallelControllerFactory();
    }

    [TestMethod]
    public void Unconnected_CallsConnect_ConnectsLogsAccepted()
    {
        ConnectMoqSetup();
        initiateControls.Connect();

        moqSocketSessionContext.Verify();
        moqSocketReconnectConfig.Verify();
        moqFlogger.Verify();
        moqDispatcher.Verify();
        moqOsSocket.Verify();
    }

    [TestMethod]
    public void Unconnected_CallConnectThrowsExceptionTwice_CallsDisconnectAndTriggersConnectNowSignal()
    {
        ConnectMoqSetup();
        SetupConnectionExceptionDisconnect();

        initiateControls.Connect();
        moqFlogger.Verify();
        moqParallelControler.Verify();
    }

    [TestMethod]
    public void Connected_CallsDisconnect_CallsShutdown()
    {
        moqSocketSessionContext.SetupGet(ssc => ssc.SocketSessionState)
            .Returns(SocketSessionState.Connected);
        moqSocketSessionContext.Setup(ssc => ssc.OnDisconnecting()).Verifiable();
        moqSocketSessionContext.SetupGet(ssc => ssc.SocketConnection).Returns(moqSocketConnection.Object).Verifiable();
        moqSocketConnection.SetupGet(sc => sc.IsConnected).Returns(true).Verifiable();
        moqDispatcher.Setup(sd => sd.Stop()).Verifiable();
        moqFlogger.Setup(fl => fl.Info("Connection to {0} {1} id {2}:{3} closed", It.IsAny<object[]>())).Verifiable();
        moqSocketConnection.Setup(sf => sf.OSSocket).Returns(moqOsSocket.Object).Verifiable();
        moqOsSocket.Setup(sf => sf.Close()).Verifiable();
        moqSocketSessionContext.Setup(ssc => ssc.OnDisconnected()).Verifiable();

        initiateControls.Disconnect();

        moqSocketSessionContext.Verify();
        moqSocketConnection.Verify();
        moqDispatcher.Verify();
        moqFlogger.Verify();
        moqOsSocket.Verify();
    }

    [TestMethod]
    public void Unconnected_FirstConnectionUnavailable_SecondConnectionConnects()
    {
        ConnectMoqSetup();

        moqSocketSessionContext.SetupSequence(ssc => ssc.SocketSessionState).Returns(SocketSessionState.New)
            .Returns(SocketSessionState.Disconnected);
        moqSocketSessionContext.SetupSequence(ssc => ssc.SocketConnection).Returns(null as ISocketConnection)
            .Returns(moqSocketConnection.Object).Returns(moqSocketConnection.Object);
        moqSocketConnection.SetupSequence(ssc => ssc.IsConnected).Returns(false)
            .Returns(true);
        moqSocketFactory.SetupSequence(sf => sf.Create(It.IsAny<ISocketTopicConnectionConfig>(),
                It.IsAny<ISocketConnectionConfig>())).Throws(new Exception("Connection failure"))
            .Returns(moqOsSocket.Object);
        moqSocketTopicConnectionConfig.SetupSequence(stcc => stcc.MoveNext()).Returns(true).Returns(true)
            .Returns(false);
        moqFlogger.Setup(fl => fl.Info("Connection to {0} {1}:{2} rejected: {3}", It.IsAny<object[]>())).Verifiable();


        initiateControls.Connect();

        moqSocketSessionContext.Verify();
        moqSocketReconnectConfig.Verify();
        moqFlogger.Verify();
        moqDispatcher.Verify();
        moqOsSocket.Verify();
    }

    [TestMethod]
    public void Unconnected_CallsStartAsync_DispatchesConnectOnBackgroundThreadImmediately()
    {
        ConnectMoqSetup();
        moqParallelControler.Setup(pc => pc.CallFromThreadPool(It.IsAny<WaitCallback>()))
            .Callback<WaitCallback>(waitCallback => waitCallback(new object())).Verifiable();
        moqSocketSessionContext.SetupSequence(ssc => ssc.SocketSessionState)
            .Returns(SocketSessionState.New)
            .Returns(SocketSessionState.New)
            .Returns(SocketSessionState.New)
            .Returns(SocketSessionState.Connecting);
        moqSocketSessionContext.SetupSequence(ssc => ssc.SocketConnection)
            .Returns(null as ISocketConnection)
            .Returns(null as ISocketConnection)
            .Returns(moqSocketConnection.Object);

        initiateControls.StartAsync();

        moqParallelControler.Verify();
        moqSocketSessionContext.Verify();
        moqSocketReconnectConfig.Verify();
        moqFlogger.Verify();
        moqDispatcher.Verify();
        moqOsSocket.Verify();
    }

    private void ConnectMoqSetup()
    {
        moqSocketSessionContext.SetupSequence(ssc => ssc.SocketSessionState).Returns(SocketSessionState.New)
            .Returns(SocketSessionState.Connecting);
        moqSocketSessionContext.SetupSequence(ssc => ssc.SocketConnection).Returns(null as ISocketConnection)
            .Returns(moqSocketConnection.Object);
        moqSocketSessionContext.Setup(ssc => ssc.OnSocketStateChanged(SocketSessionState.Connecting)).Verifiable();
        moqSocketFactory.Setup(sf => sf.Create(It.IsAny<ISocketTopicConnectionConfig>(),
            It.IsAny<ISocketConnectionConfig>())).Returns(moqOsSocket.Object).Verifiable();
        moqOsSocket.Setup(os => os.RemoteEndPoint).Returns(connectedIpEndPoint);
        moqOsSocket.Setup(os => os.LocalEndPoint).Returns(connectedIpEndPoint);
        moqSocketConnection.SetupGet(sc => sc.IsConnected).Returns(true).Verifiable();
        moqSocketReconnectConfig.SetupSet(src => src.NextReconnectIntervalMs = It.IsAny<uint>()).Verifiable();

        moqFlogger.Setup(fl => fl.Info("Connection to id:{0} {1}:{2} accepted", It.IsAny<object[]>())).Verifiable();
        moqSocketSessionContext.Setup(ssc => ssc.OnConnected(It.IsAny<SocketConnection>())).Verifiable();
        moqDispatcher.Setup(sd => sd.Start(It.IsAny<Action>())).Verifiable();
    }

    private void SetupConnectionExceptionDisconnect()
    {
        moqFlogger.Reset();
        moqSocketSessionContext.SetupSequence(ssc => ssc.SocketSessionState).Returns(SocketSessionState.New)
            .Returns(SocketSessionState.Disconnected);
        moqSocketFactory.Setup(sf => sf.Create(It.IsAny<ISocketTopicConnectionConfig>(),
                It.IsAny<ISocketConnectionConfig>())).Throws(new Exception("Connection failure"))
            .Verifiable();
        moqFlogger.Setup(fl => fl.Info("Connection to {0} {1}:{2} rejected: {3}", It.IsAny<object[]>())).Verifiable();
        moqSocketConnection.SetupGet(sc => sc.IsConnected).Returns(false).Verifiable();
        moqParallelControler
            .Setup(pc =>
                pc.ScheduleWithEarlyTrigger(It.IsAny<WaitOrTimerCallback>(), It.IsAny<uint>(), It.IsAny<bool>()))
            .Returns(moqIntraOsThreadSignal.Object).Verifiable();
    }
}
