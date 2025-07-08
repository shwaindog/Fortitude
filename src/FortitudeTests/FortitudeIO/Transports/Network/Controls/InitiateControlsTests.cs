#region

using System.Net;
using FortitudeCommon.Config;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.AsyncWrappers;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeIO.Config;
using FortitudeIO.Protocols;
using FortitudeIO.Transports.Network.Config;
using FortitudeIO.Transports.Network.Construction;
using FortitudeIO.Transports.Network.Controls;
using FortitudeIO.Transports.Network.Dispatcher;
using FortitudeIO.Transports.Network.Sockets;
using FortitudeIO.Transports.Network.State;
using Moq;

#endregion

namespace FortitudeTests.FortitudeIO.Transports.Network.Controls;

[TestClass]
public class InitiateControlsTests
{
    private IPEndPoint connectedIpEndPoint = null!;
    private InitiateControls initiateControls = null!;
    private Mock<ISocketDispatcher> moqDispatcher = null!;
    private Mock<IEnumerator<IEndpointConfig>> moqEndpointEnumerator = null!;
    private Mock<IFLogger> moqFlogger = null!;
    private Mock<IFLoggerFactory> moqFloggerFactory = null!;
    private Mock<IIntraOSThreadSignal> moqIntraOsThreadSignal = null!;
    private Mock<IOSSocket> moqOsSocket = null!;
    private Mock<IOSParallelController> moqParallelControler = null!;
    private Mock<IOSParallelControllerFactory> moqParallelControllerFactory = null!;
    private Mock<ISocketConnection> moqSocketConnection = null!;
    private Mock<IEndpointConfig> moqSocketConnectionConfig = null!;
    private Mock<ISocketFactoryResolver> moqSocketFactories = null!;
    private Mock<ISocketFactory> moqSocketFactory = null!;
    private Mock<IRetryConfig> moqSocketReconnectConfig = null!;
    private Mock<ISocketSessionContext> moqSocketSessionContext = null!;
    private Mock<INetworkTopicConnectionConfig> moqSocketTopicConnectionConfig = null!;
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
        moqSocketTopicConnectionConfig = new Mock<INetworkTopicConnectionConfig>();
        moqEndpointEnumerator = new Mock<IEnumerator<IEndpointConfig>>();
        moqSocketConnectionConfig = new Mock<IEndpointConfig>();
        moqSocketReconnectConfig = new Mock<IRetryConfig>();
        moqParallelControllerFactory.SetupGet(pcf => pcf.GetOSParallelController)
            .Returns(moqParallelControler.Object);

        moqOsSocket = new Mock<IOSSocket>();
        moqSocketSessionContext = new Mock<ISocketSessionContext>();
        moqSocketFactories = new Mock<ISocketFactoryResolver>();
        moqSocketFactory = new Mock<ISocketFactory>();
        connectedIpEndPoint = new IPEndPoint(new IPAddress(new byte[] { 127, 0, 0, 1 }), testHostPort);
        moqSocketConnection = new Mock<ISocketConnection>();

        moqSocketReconnectConfig.Setup(scc => scc.GetIntervalForAttempt(It.IsAny<int>())).Returns(TimeSpan.FromMilliseconds(5));
        moqSocketConnectionConfig = new Mock<IEndpointConfig>();
        moqSocketSessionContext.SetupGet(ssc => ssc.SocketFactoryResolver).Returns(moqSocketFactories.Object);
        moqSocketFactories.SetupGet(ssc => ssc.ParallelController).Returns(moqParallelControler.Object);
        moqSocketConnectionConfig.SetupGet(scc => scc.InstanceName).Returns("InitiateControlsTests");
        moqSocketConnectionConfig.SetupGet(scc => scc.Hostname).Returns(testHostName);
        moqSocketConnectionConfig.SetupGet(scc => scc.Port).Returns(testHostPort);
        moqSocketTopicConnectionConfig.Setup(stcc => stcc.GetEnumerator())
            .Returns(moqEndpointEnumerator.Object);
        moqEndpointEnumerator.SetupGet(stcc => stcc.Current).Returns(moqSocketConnectionConfig.Object);
        moqEndpointEnumerator.SetupSequence(stcc => stcc.MoveNext()).Returns(true).Returns(false);
        moqSocketTopicConnectionConfig.SetupGet(scc => scc.ReconnectConfig).Returns(moqSocketReconnectConfig.Object);
        moqSocketTopicConnectionConfig.SetupGet(scc => scc.TopicDescription).Returns("TestSessionDescription");

        moqSocketSessionContext.SetupGet(ssc => ssc.NetworkTopicConnectionConfig)
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
    public void Connected_CallsStop_CallsShutdown()
    {
        moqSocketSessionContext.SetupGet(ssc => ssc.SocketSessionState)
            .Returns(SocketSessionState.Connected);
        moqSocketSessionContext.Setup(ssc => ssc.OnDisconnecting()).Verifiable();
        moqSocketSessionContext.SetupGet(ssc => ssc.SocketConnection).Returns(moqSocketConnection.Object).Verifiable();
        moqSocketConnection.SetupGet(sc => sc.IsConnected).Returns(true).Verifiable();
        moqFlogger.Setup(fl => fl.Info("Connection to {0} closed. {1}", It.IsAny<object[]>())).Verifiable();
        moqSocketSessionContext.Setup(ssc => ssc.OnDisconnected(It.IsAny<CloseReason>(), It.IsAny<string?>())).Verifiable();

        initiateControls.Stop(CloseReason.Completed);

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
        moqSocketFactory.SetupSequence(sf => sf.Create(It.IsAny<INetworkTopicConnectionConfig>(),
                It.IsAny<IEndpointConfig>())).Throws(new Exception("Connection failure"))
            .Returns(moqOsSocket.Object);
        moqEndpointEnumerator.SetupSequence(stcc => stcc.MoveNext()).Returns(true).Returns(true)
            .Returns(false);
        moqFlogger.Setup(fl => fl.Info("Connection attempt to {0} to host {1}:{2} was rejected: {3}", It.IsAny<object[]>())).Verifiable();


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
        moqSocketReconnectConfig.Reset();
        SetupConnectionExceptionDisconnect();
        moqParallelControler.Setup(pc => pc.CallFromThreadPool(It.IsAny<WaitCallback>()))
            .Callback<WaitCallback>(waitCallback =>
            {
                moqEndpointEnumerator.SetupSequence(stcc => stcc.MoveNext()).Returns(true).Returns(false);
                waitCallback(new object());
            }).Verifiable();

        moqParallelControler
            .Setup(pc =>
                pc.ScheduleWithEarlyTrigger(It.IsAny<WaitOrTimerCallback>(), It.IsAny<uint>(), It.IsAny<bool>()))
            .Callback<WaitOrTimerCallback, uint, bool>((waitCallback, waitTime, _) =>
            {
                moqEndpointEnumerator.SetupSequence(stcc => stcc.MoveNext()).Returns(true).Returns(false);
                waitCallback(new object(), false);
            })
            .Returns(moqIntraOsThreadSignal.Object).Verifiable();
        moqSocketConnection.SetupGet(sc => sc.IsConnected).Returns(true).Verifiable();
        moqSocketFactory.SetupSequence(sf => sf.Create(It.IsAny<INetworkTopicConnectionConfig>(),
                It.IsAny<IEndpointConfig>()))
            .Returns(() => throw new Exception("Connection failure"))
            .Returns(() => throw new Exception("Connection failure"))
            .Returns(moqOsSocket.Object);
        moqSocketSessionContext.SetupSequence(ssc => ssc.SocketSessionState)
            .Returns(SocketSessionState.New) // connect check is connected
            .Returns(SocketSessionState.New) // schedule reconnect check
            .Returns(SocketSessionState.Reconnecting) // connect check is connected
            .Returns(SocketSessionState.Reconnecting) // schedule reconnect check
            .Returns(SocketSessionState.Reconnecting) // connect check is connected
            .Returns(SocketSessionState.Connected);
        moqSocketSessionContext.SetupSequence(ssc => ssc.SocketConnection)
            .Returns(null as ISocketConnection) // connect check is connected
            .Returns(null as ISocketConnection) // post connect check
            .Returns(null as ISocketConnection) // disconnect
            .Returns(null as ISocketConnection) // schedule reconnect
            .Returns(null as ISocketConnection) // connect check is connected
            .Returns(null as ISocketConnection) // post connect check
            .Returns(null as ISocketConnection) // disconnect
            .Returns(null as ISocketConnection) // schedule reconnect
            .Returns(null as ISocketConnection) // connect check is connected
            .Returns(moqSocketConnection.Object);
        moqSocketReconnectConfig.SetupSequence(src => src.GetIntervalForAttempt(It.IsAny<int>())).Returns(TimeSpan.Zero).Returns(TimeSpan.FromMilliseconds(1));

        initiateControls.Connect();

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
        moqSocketFactory.Setup(sf => sf.Create(It.IsAny<INetworkTopicConnectionConfig>(),
            It.IsAny<IEndpointConfig>())).Returns(moqOsSocket.Object).Verifiable();
        moqOsSocket.Setup(os => os.RemoteEndPoint).Returns(connectedIpEndPoint);
        moqOsSocket.Setup(os => os.LocalEndPoint).Returns(connectedIpEndPoint);
        moqSocketConnection.SetupGet(sc => sc.IsConnected).Returns(true).Verifiable();

        moqFlogger.Setup(fl => fl.Info("Connection {0} was accepted by host {1}:{2}", It.IsAny<object[]>())).Verifiable();
        moqSocketSessionContext.Setup(ssc => ssc.OnConnected(It.IsAny<SocketConnection>())).Verifiable();
    }

    private void SetupConnectionExceptionDisconnect()
    {
        moqFlogger.Reset();
        moqSocketSessionContext.SetupSequence(ssc => ssc.SocketSessionState).Returns(SocketSessionState.New)
            .Returns(SocketSessionState.Disconnected);
        moqSocketFactory.Setup(sf => sf.Create(It.IsAny<INetworkTopicConnectionConfig>(),
                It.IsAny<IEndpointConfig>())).Throws(new Exception("Connection failure"))
            .Verifiable();
        moqFlogger.Setup(fl => fl.Info("Connection attempt to {0} to host {1}:{2} was rejected: {3}", It.IsAny<object[]>())).Verifiable();
        moqSocketConnection.SetupGet(sc => sc.IsConnected).Returns(false).Verifiable();
        moqParallelControler
            .Setup(pc =>
                pc.ScheduleWithEarlyTrigger(It.IsAny<WaitOrTimerCallback>(), It.IsAny<uint>(), It.IsAny<bool>()))
            .Returns(moqIntraOsThreadSignal.Object).Verifiable();
    }
}
