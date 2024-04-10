#region

using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.AsyncWrappers;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
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
public class TcpAcceptedClientControlsTests
{
    private Mock<ISocketDispatcher> moqDispatcher = null!;
    private Mock<IEnumerator<IEndpointConfig>> moqEndpointEnumerator = null!;
    private Mock<IFLogger> moqFlogger = null!;
    private Mock<IFLoggerFactory> moqFloggerFactory = null!;
    private Mock<IOSSocket> moqOsSocket = null!;
    private Mock<IOSParallelController> moqParallelControler = null!;
    private Mock<IOSParallelControllerFactory> moqParallelControllerFactory = null!;
    private Mock<ISocketConnection> moqSocketConnection = null!;
    private Mock<IEndpointConfig> moqSocketConnectionConfig = null!;
    private Mock<ISocketFactoryResolver> moqSocketFactories = null!;
    private Mock<ISocketFactory> moqSocketFactory = null!;
    private Mock<ISocketReconnectConfig> moqSocketReconnectConfig = null!;
    private Mock<ISocketSessionContext> moqSocketSessionContext = null!;
    private Mock<INetworkTopicConnectionConfig> moqSocketTopicConnectionConfig = null!;
    private TcpAcceptedClientControls tcpAcceptedClientControls = null!;
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
        moqSocketTopicConnectionConfig = new Mock<INetworkTopicConnectionConfig>();
        moqEndpointEnumerator = new Mock<IEnumerator<IEndpointConfig>>();
        moqSocketConnectionConfig = new Mock<IEndpointConfig>();
        moqSocketReconnectConfig = new Mock<ISocketReconnectConfig>();
        moqParallelControllerFactory.SetupGet(pcf => pcf.GetOSParallelController)
            .Returns(moqParallelControler.Object);

        moqOsSocket = new Mock<IOSSocket>();
        moqSocketSessionContext = new Mock<ISocketSessionContext>();
        moqSocketFactories = new Mock<ISocketFactoryResolver>();
        moqSocketFactory = new Mock<ISocketFactory>();
        moqSocketConnection = new Mock<ISocketConnection>();

        moqSocketReconnectConfig.SetupGet(scc => scc.NextReconnectIntervalMs).Returns(5u);
        moqSocketConnectionConfig = new Mock<IEndpointConfig>();
        moqSocketSessionContext.SetupGet(ssc => ssc.SocketFactoryResolver).Returns(moqSocketFactories.Object);
        moqSocketFactories.SetupGet(ssc => ssc.ParallelController).Returns(moqParallelControler.Object);
        moqSocketConnectionConfig.SetupGet(scc => scc.InstanceName).Returns("TcpAcceptedClientControlsTests");
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
        FLoggerFactory.Instance = moqFloggerFactory.Object;
        OSParallelControllerFactory.Instance = moqParallelControllerFactory.Object;

        tcpAcceptedClientControls = new TcpAcceptedClientControls(moqSocketSessionContext.Object);
    }

    [TestCleanup]
    public void TearDown()
    {
        OSParallelControllerFactory.Instance = new OSParallelControllerFactory();
        FLoggerFactory.Instance = new FLoggerFactory();
    }

    [TestMethod]
    public void Unconnected_CallsConnect_LogsWillNotConnect()
    {
        moqFlogger.Setup(fl => fl.Warn("Can NOT connect to client accepted session {0} will not attempt connect.", It.IsAny<object[]>()))
            .Verifiable();

        tcpAcceptedClientControls.Connect();

        moqFlogger.Verify();
    }

    [TestMethod]
    public void Unconnected_CallsStartAsync_LogsWillNotConnect()
    {
        moqFlogger.Setup(fl => fl.Warn("Can NOT connect to client accepted session {0} will not attempt connect.", It.IsAny<object[]>()))
            .Verifiable();

        tcpAcceptedClientControls.StartAsync();

        moqFlogger.Verify();
    }

    [TestMethod]
    public void Connected_CallsDisconnect_CallsShutdown()
    {
        DisconnectMoqSetup();

        tcpAcceptedClientControls.Disconnect();

        moqSocketSessionContext.Verify();
        moqSocketConnection.Verify();
        moqDispatcher.Verify();
        moqFlogger.Verify();
        moqOsSocket.Verify();
    }

    [TestMethod]
    public void Connected_OnSessionFailure_LogsNoReconnectAndDisconnects()
    {
        moqFlogger.Setup(fl => fl.Warn("Problem communicating with client session {0} will not attempt reconnect. Reason {1}", It.IsAny<object[]>()))
            .Verifiable();

        DisconnectMoqSetup();

        tcpAcceptedClientControls.OnSessionFailure("Lost connect will attempting to send");

        moqSocketSessionContext.Verify();
        moqSocketConnection.Verify();
        moqDispatcher.Verify();
        moqFlogger.Verify();
        moqOsSocket.Verify();
    }


    [TestMethod]
    public void Unconnected_CallsDisconnect_DoesNotDoAnything()
    {
        moqSocketSessionContext.SetupGet(ssc => ssc.SocketSessionState)
            .Returns(SocketSessionState.Disconnected);
        moqSocketSessionContext.SetupGet(ssc => ssc.SocketConnection).Returns(moqSocketConnection.Object).Verifiable();
        moqSocketConnection.SetupGet(sc => sc.IsConnected).Returns(false).Verifiable();
        moqSocketSessionContext.Setup(ssc => ssc.OnDisconnected()).Verifiable();

        tcpAcceptedClientControls.Disconnect();

        moqSocketSessionContext.Verify();
        moqFlogger.Verify();
        moqSocketConnection.Verify();
    }


    private void DisconnectMoqSetup()
    {
        moqSocketSessionContext.SetupGet(ssc => ssc.SocketSessionState)
            .Returns(SocketSessionState.Connected);
        moqSocketSessionContext.Setup(ssc => ssc.OnDisconnecting()).Verifiable();
        moqSocketSessionContext.SetupGet(ssc => ssc.SocketConnection).Returns(moqSocketConnection.Object).Verifiable();
        moqSocketConnection.SetupGet(sc => sc.IsConnected).Returns(true).Verifiable();
        moqDispatcher.Setup(sd => sd.Stop()).Verifiable();
        moqFlogger.Setup(fl => fl.Info("Connection client accepted session to {0} {1} id {2}:{3} closed", It.IsAny<object[]>())).Verifiable();
        moqSocketConnection.Setup(sf => sf.OSSocket).Returns(moqOsSocket.Object).Verifiable();
        moqOsSocket.Setup(sf => sf.Close()).Verifiable();
        moqSocketSessionContext.Setup(ssc => ssc.OnDisconnected()).Verifiable();
    }
}
