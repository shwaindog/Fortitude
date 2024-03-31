#region

using System.Net;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.AsyncWrappers;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeCommon.Types;
using FortitudeIO.Conversations;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Transports.Network.Config;
using FortitudeIO.Transports.Network.Construction;
using FortitudeIO.Transports.Network.Controls;
using FortitudeIO.Transports.Network.Dispatcher;
using FortitudeIO.Transports.Network.Receiving;
using FortitudeIO.Transports.Network.Sockets;
using FortitudeIO.Transports.Network.State;
using Moq;

#endregion

namespace FortitudeTests.FortitudeIO.Transports.Network.Controls;

[TestClass]
public class TcpAcceptorControlsTests
{
    private Action? capturedOnCxAcceptCallback;
    private IPEndPoint connectedIpEndPoint = null!;
    private IConversationRequester lastCapturedClientConversation = null!;
    private Mock<IOSSocket> moqAcceptorOsSocket = null!;
    private Mock<Action<SocketSessionState>> moqCapturedClientSessionStateChanged = null!;
    private Mock<INetworkTopicConnectionConfig> moqClientNetworkTopicConnectionConfig = null!;
    private Mock<IOSSocket> moqClientOsSocket = null!;
    private Mock<ISocketDispatcher> moqDispatcher = null!;
    private Mock<IFLogger> moqFlogger = null!;
    private Mock<IFLoggerFactory> moqFloggerFactory = null!;
    private Mock<INetworkTopicConnectionConfig> moqNetworkTopicConnectionConfig = null!;
    private Mock<IOSParallelController> moqParallelControler = null!;
    private Mock<IOSParallelControllerFactory> moqParallelControllerFactory = null!;
    private Mock<ISerdesFactory> moqSerdesFactory = null!;
    private Mock<ISocketConnection> moqSocketConnection = null!;
    private Mock<IEndpointConfig> moqSocketConnectionConfig = null!;
    private Mock<ISocketConnectivityChanged> moqSocketConnectivityChanged = null!;
    private Mock<ISocketDispatcherResolver> moqSocketDispatcherResolver = null!;
    private Mock<ISocketDispatcherListener> moqSocketDispatchListener = null!;
    private Mock<ISocketFactoryResolver> moqSocketFactories = null!;
    private Mock<ISocketFactory> moqSocketFactory = null!;
    private Mock<ISocketReceiver> moqSocketReceiver = null!;
    private Mock<ISocketReconnectConfig> moqSocketReconnectConfig = null!;
    private Mock<ISocketSessionContext> moqSocketSessionContext = null!;
    private Mock<IVersionedMessage> moqVersionedMessage = null!;
    private TcpAcceptorControls tcpAcceptorControls = null!;
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
        moqNetworkTopicConnectionConfig = new Mock<INetworkTopicConnectionConfig>();
        moqClientNetworkTopicConnectionConfig = new Mock<INetworkTopicConnectionConfig>();
        moqSocketConnectionConfig = new Mock<IEndpointConfig>();
        moqSocketReconnectConfig = new Mock<ISocketReconnectConfig>();
        moqParallelControllerFactory.SetupGet(pcf => pcf.GetOSParallelController)
            .Returns(moqParallelControler.Object);

        moqAcceptorOsSocket = new Mock<IOSSocket>();
        moqClientOsSocket = new Mock<IOSSocket>();
        moqSocketSessionContext = new Mock<ISocketSessionContext>();
        moqSocketFactories = new Mock<ISocketFactoryResolver>();
        moqSocketFactory = new Mock<ISocketFactory>();
        moqSerdesFactory = new Mock<ISerdesFactory>();
        connectedIpEndPoint = new IPEndPoint(new IPAddress(new byte[] { 127, 0, 0, 1 }), testHostPort);
        moqSocketConnection = new Mock<ISocketConnection>();
        moqSocketReceiver = new Mock<ISocketReceiver>();
        moqSocketDispatchListener = new Mock<ISocketDispatcherListener>();
        moqSocketDispatcherResolver = new Mock<ISocketDispatcherResolver>();
        moqSocketConnectivityChanged = new Mock<ISocketConnectivityChanged>();
        moqCapturedClientSessionStateChanged = new Mock<Action<SocketSessionState>>();
        moqVersionedMessage = new Mock<IVersionedMessage>();

        moqSocketReconnectConfig.SetupGet(scc => scc.NextReconnectIntervalMs).Returns(5u);
        moqSocketConnectionConfig = new Mock<IEndpointConfig>();
        moqSocketConnectionConfig.SetupGet(scc => scc.InstanceName).Returns("InitiateControlsTests");
        moqSocketConnectionConfig.SetupGet(scc => scc.Hostname).Returns(testHostName);
        moqSocketConnectionConfig.SetupGet(scc => scc.Port).Returns(testHostPort);
        moqNetworkTopicConnectionConfig.Setup(stcc => stcc.GetEnumerator())
            .Returns(moqNetworkTopicConnectionConfig.Object);
        moqNetworkTopicConnectionConfig.SetupGet(stcc => stcc.Current).Returns(moqSocketConnectionConfig.Object);
        moqNetworkTopicConnectionConfig.SetupSequence(stcc => stcc.MoveNext()).Returns(true).Returns(false);
        moqNetworkTopicConnectionConfig.SetupGet(scc => scc.ReconnectConfig).Returns(moqSocketReconnectConfig.Object);
        moqNetworkTopicConnectionConfig.SetupGet(scc => scc.TopicDescription).Returns("TestSessionDescription");
        moqNetworkTopicConnectionConfig.Setup(scc => scc.ToggleProtocolDirection()).Returns(moqClientNetworkTopicConnectionConfig.Object);

        moqSocketSessionContext.SetupGet(ssc => ssc.NetworkTopicConnectionConfig)
            .Returns(moqNetworkTopicConnectionConfig.Object);
        moqSocketSessionContext.SetupGet(ssc => ssc.SocketDispatcher).Returns(moqDispatcher.Object);
        moqSocketSessionContext.SetupGet(ssc => ssc.Name).Returns("TcpAcceptorControlsTests");
        moqSocketSessionContext.SetupGet(ssc => ssc.SerdesFactory).Returns(moqSerdesFactory.Object);

        moqSocketSessionContext.SetupGet(ssc => ssc.SocketFactoryResolver).Returns(moqSocketFactories.Object);
        moqSocketFactories.SetupGet(ssc => ssc.ParallelController).Returns(moqParallelControler.Object);
        moqSocketConnectivityChanged.Setup(scc => scc.GetOnConnectionChangedHandler())
            .Returns(moqCapturedClientSessionStateChanged.Object);
        ISocketConnectivityChanged MoqCallback(ISocketSessionContext context) => moqSocketConnectivityChanged.Object;
        moqSocketFactories.SetupGet(pcf => pcf.ConnectionChangedHandlerResolver).Returns(MoqCallback);
        moqSocketFactories.SetupGet(ssc => ssc.SocketFactory).Returns(moqSocketFactory.Object);
        moqSocketFactories.SetupGet(ssc => ssc.SocketDispatcherResolver).Returns(moqSocketDispatcherResolver.Object);
        //moqFlogger.Setup(fl => fl.Info(It.IsAny<string>(), It.IsAny<object[]>()));
        moqAcceptorOsSocket.SetupAllProperties();
        moqSocketDispatcherResolver.Setup(dr => dr.Resolve(It.IsAny<INetworkTopicConnectionConfig>()))
            .Returns(moqDispatcher.Object);

        moqFloggerFactory.Setup(ff => ff.GetLogger(It.IsAny<Type>())).Returns(moqFlogger.Object);
        FLoggerFactory.Instance = moqFloggerFactory.Object;
        OSParallelControllerFactory.Instance = moqParallelControllerFactory.Object;

        tcpAcceptorControls = new TcpAcceptorControls(moqSocketSessionContext.Object);
    }

    [TestCleanup]
    public void TearDown()
    {
        OSParallelControllerFactory.Instance = new OSParallelControllerFactory();
        FLoggerFactory.Instance = new FLoggerFactory();
    }

    [TestMethod]
    public void Unconnected_CallsConnect_ConnectsLogsAccepted()
    {
        ConnectMoqSetupCaptureOnCxAcceptCallback();
        tcpAcceptorControls.Connect();

        moqSocketSessionContext.Verify();
        moqFlogger.Verify();
        moqDispatcher.Verify();
        moqAcceptorOsSocket.Verify();
        moqSocketReceiver.Verify();
    }

    [TestMethod]
    public void Unconnected_CallConnectThrowsExceptionTwice_LogsExceptionThenReturns()
    {
        ConnectMoqSetupCaptureOnCxAcceptCallback();
        SetupConnectionException();
        moqNetworkTopicConnectionConfig.SetupSequence(stcc => stcc.MoveNext()).Returns(true).Returns(true)
            .Returns(false);

        tcpAcceptorControls.Connect();

        moqFlogger.Verify();
        moqSocketConnection.Verify();
    }

    [TestMethod]
    public void Connected_CallsDisconnect_CallsShutdownClosesAllClients()
    {
        moqSocketSessionContext.SetupGet(ssc => ssc.SocketSessionState).Returns(SocketSessionState.Connected)
            .Verifiable();
        moqSocketSessionContext.SetupGet(ssc => ssc.SocketReceiver).Returns(moqSocketReceiver.Object).Verifiable();
        moqSocketReceiver.SetupGet(ssc => ssc.IsAcceptor).Returns(true).Verifiable();
        moqSocketSessionContext.Setup(ssc => ssc.OnDisconnecting()).Verifiable();
        moqSocketSessionContext.Setup(ssc => ssc.SocketConnection).Returns(moqSocketConnection.Object).Verifiable();
        moqDispatcher.Setup(ssc => ssc.Stop()).Verifiable();
        moqFlogger.Setup(fl => fl.Info("Stopping publisher {0} @{1}", It.IsAny<object[]>())).Verifiable();

        var moqFirstClientListener = new Mock<IStreamListener>();
        var moqFirstClient = new Mock<IConversationRequester>();
        moqFirstClient.Setup(cr => cr.StreamListener).Returns(moqFirstClientListener.Object).Verifiable();
        moqFirstClient.SetupGet(cr => cr.Id).Returns(1).Verifiable();
        moqFirstClient.Setup(cr => cr.Stop()).Verifiable();
        var moqSecondClientListener = new Mock<IStreamListener>();
        var moqSecondClient = new Mock<IConversationRequester>();
        moqSecondClient.Setup(cr => cr.StreamListener).Returns(moqSecondClientListener.Object).Verifiable();
        moqSecondClient.Setup(cr => cr.Id).Returns(2).Verifiable();
        moqSecondClient.Setup(cr => cr.Stop()).Verifiable();
        var clientMap = new Dictionary<int, IConversationRequester>
        {
            { moqFirstClient.Object.Id, moqFirstClient.Object }, { moqSecondClient.Object.Id, moqSecondClient.Object }
        };
        NonPublicInvocator.SetInstanceField(tcpAcceptorControls, "clients", clientMap);
        moqSocketReceiver.SetupRemove(sr => sr.Accept -= It.IsAny<Action>()).Verifiable();
        moqSocketSessionContext.Setup(sdl => sdl.SocketDispatcher).Returns(moqDispatcher.Object).Verifiable();
        moqDispatcher.Setup(sdl => sdl.Listener).Returns(moqSocketDispatchListener.Object).Verifiable();
        moqSocketDispatchListener.Setup(sdl => sdl.UnregisterForListen(It.IsAny<IStreamListener>())).Verifiable();
        moqSocketConnection.SetupGet(sr => sr.OSSocket).Returns(moqAcceptorOsSocket.Object).Verifiable();
        moqAcceptorOsSocket.Setup(sdl => sdl.Close()).Verifiable();
        moqFlogger.Setup(fl => fl.Info("Publisher {0} @{0} stopped", It.IsAny<object[]>())).Verifiable();
        moqSocketSessionContext.Setup(ssc => ssc.OnDisconnected()).Verifiable();

        tcpAcceptorControls.Disconnect();

        moqSocketSessionContext.Verify();
        moqDispatcher.Verify();
        moqFlogger.Verify();
        moqFlogger.Verify();
        moqFirstClient.VerifyAll();
        moqSecondClient.VerifyAll();
        moqSocketReceiver.Verify();
        moqSocketDispatchListener.Verify();
        moqSocketConnection.Verify();
        moqAcceptorOsSocket.Verify();
    }

    [TestMethod]
    public void Unconnected_FirstConnectionUnavailable_SecondConnectionConnects()
    {
        ConnectMoqSetupCaptureOnCxAcceptCallback();

        moqSocketSessionContext.SetupGet(ssc => ssc.SocketSessionState).Returns(SocketSessionState.New);
        moqSocketFactory.SetupSequence(sf => sf.Create(It.IsAny<INetworkTopicConnectionConfig>(),
                It.IsAny<IEndpointConfig>())).Throws(new Exception("Connection failure"))
            .Returns(moqAcceptorOsSocket.Object);
        moqNetworkTopicConnectionConfig.SetupSequence(stcc => stcc.MoveNext()).Returns(true).Returns(true)
            .Returns(false);
        moqFlogger.Setup(fl => fl.Error("Failed to open socket for {0}. Got {1}", It.IsAny<object[]>())).Verifiable();

        tcpAcceptorControls.Connect();

        moqSocketSessionContext.Verify();
        moqSocketReconnectConfig.Verify();
        moqFlogger.Verify();
        moqDispatcher.Verify();
        moqAcceptorOsSocket.Verify();
    }

    private void ConnectMoqSetupCaptureOnCxAcceptCallback()
    {
        moqSocketSessionContext.SetupGet(ssc => ssc.SocketSessionState).Returns(SocketSessionState.New).Verifiable();
        moqSocketSessionContext.SetupSequence(ssc => ssc.SocketSessionState)
            .Returns(SocketSessionState.Connecting);
        moqSocketSessionContext.SetupGet(ssc => ssc.SocketConnection).Returns(moqSocketConnection.Object).Verifiable();
        moqSocketSessionContext.Setup(ssc => ssc.OnSocketStateChanged(SocketSessionState.Connecting)).Verifiable();
        moqSocketFactory.Setup(sf => sf.Create(It.IsAny<INetworkTopicConnectionConfig>(),
            It.IsAny<IEndpointConfig>())).Returns(moqAcceptorOsSocket.Object).Verifiable();
        moqAcceptorOsSocket.Setup(os => os.RemoteEndPoint).Returns(connectedIpEndPoint);
        moqAcceptorOsSocket.Setup(os => os.LocalEndPoint).Returns(connectedIpEndPoint);
        moqSocketConnection.SetupGet(sc => sc.IsConnected).Returns(true).Verifiable();

        moqFlogger.Setup(fl => fl.Info("Starting publisher {0} @{1}", It.IsAny<object[]>())).Verifiable();
        moqFlogger.Setup(fl => fl.Info("Publisher {0} {1}@{2} started", It.IsAny<object[]>())).Verifiable();

        moqSocketReceiver.SetupSet(ssc => ssc.ZeroBytesReadIsDisconnection = false).Verifiable();
        moqSocketReceiver.SetupAdd(sr => sr.Accept += It.IsAny<Action?>()).Callback<Action>(callback => { capturedOnCxAcceptCallback = callback; })
            .Verifiable();
        moqSocketSessionContext.Setup(ssc => ssc.OnConnected(It.IsAny<SocketConnection>())).Verifiable();
        moqSocketSessionContext.SetupGet(ssc => ssc.SocketReceiver).Returns(moqSocketReceiver.Object).Verifiable();
        moqDispatcher.Setup(sd => sd.Start(It.IsAny<Action>())).Verifiable();
    }

    private void SetupConnectionException()
    {
        moqFlogger.Reset();
        moqFlogger.Setup(fl => fl.Info("Starting publisher {0} @{1}", It.IsAny<object[]>())).Verifiable();
        moqSocketFactory.Setup(sf => sf.Create(It.IsAny<INetworkTopicConnectionConfig>(),
                It.IsAny<IEndpointConfig>())).Throws(new Exception("Connection failure"))
            .Verifiable();
        moqFlogger.Setup(fl => fl.Error("Failed to open socket for {0}. Got {1}", It.IsAny<object[]>())).Verifiable();
        moqSocketConnection.SetupGet(sc => sc.IsConnected).Returns(false).Verifiable();
    }

    [TestMethod]
    public void NewClientConnection_RegisterAcceptor_RaisesNewClient()
    {
        ConnectMoqSetupCaptureOnCxAcceptCallback();
        tcpAcceptorControls.Connect();
        //Setup OnCxAccept
        moqSocketReceiver.Setup(sr => sr.AcceptClientSocketRequest()).Returns(moqClientOsSocket.Object).Verifiable();
        moqClientNetworkTopicConnectionConfig.SetupGet(sr => sr.SendBufferSize).Returns(2000).Verifiable();
        moqClientNetworkTopicConnectionConfig.SetupGet(sr => sr.ReceiveBufferSize).Returns(2000).Verifiable();
        moqSocketSessionContext.SetupGet(ssc => ssc.SocketFactoryResolver).Returns(moqSocketFactories.Object).Verifiable();
        moqSocketFactories.SetupGet(ssc => ssc.SocketDispatcherResolver).Returns(moqSocketDispatcherResolver.Object).Verifiable();
        moqSocketDispatcherResolver.Setup(sdr => sdr.Resolve(It.IsAny<INetworkTopicConnectionConfig>()))
            .Returns(moqDispatcher.Object);
        moqClientOsSocket.SetupGet(os => os.Connected).Returns(true).Verifiable();
        moqClientOsSocket.Setup(os => os.RemoteEndPoint).Returns(connectedIpEndPoint).Verifiable();
        moqCapturedClientSessionStateChanged.Setup(os => os.Invoke(SocketSessionState.New)).Verifiable();
        moqFlogger.Setup(os => os.Info("Client {0} ({1}) connected to server {2} @{3}", It.IsAny<object[]>()))
            .Verifiable();

        var moqNewClientCallback = new Mock<Action<IConversationRequester>>();
        moqNewClientCallback.Setup(stcc => stcc.Invoke(It.IsAny<IConversationRequester>()))
            .Callback<IConversationRequester>(newClientRequester => lastCapturedClientConversation = newClientRequester)
            .Verifiable();
        tcpAcceptorControls.NewClient += moqNewClientCallback.Object;
        var moqRemovedClientCallback = new Mock<Action<IConversationRequester>>();
        moqRemovedClientCallback.Setup(stcc => stcc.Invoke(It.IsAny<IConversationRequester>()))
            .Verifiable();
        tcpAcceptorControls.ClientRemoved += moqRemovedClientCallback.Object;

        capturedOnCxAcceptCallback?.Invoke();

        moqSocketReceiver.Verify();
        moqClientNetworkTopicConnectionConfig.Verify();
        moqSocketSessionContext.Verify();
        moqSocketDispatcherResolver.Verify();
        moqClientOsSocket.Verify();
        moqCapturedClientSessionStateChanged.Verify();
        moqFlogger.Verify();
        moqNewClientCallback.Verify();
        Assert.IsNotNull(lastCapturedClientConversation);

        // Setup client disconnect checks
        moqDispatcher.Setup(sdl => sdl.Listener).Returns(moqSocketDispatchListener.Object).Verifiable();
        moqSocketDispatchListener.Setup(sdl => sdl.UnregisterForListen(It.IsAny<IStreamListener>())).Verifiable();
        moqFlogger.Setup(os => os.Info("Client {0} ({1}) connected to server {2} @{3}", It.IsAny<object[]>()))
            .Verifiable();
        lastCapturedClientConversation.Stop();
        moqRemovedClientCallback.Verify();
    }

    [TestMethod]
    public void NewClientConnection_RegisterAcceptor_RaisesErrorAndLogs()
    {
        ConnectMoqSetupCaptureOnCxAcceptCallback();
        tcpAcceptorControls.Connect();
        //Setup OnCxAccept
        moqSocketReceiver.Setup(sr => sr.AcceptClientSocketRequest()).Throws(new Exception("Client Accept Error"))
            .Verifiable();
        moqFlogger.Setup(
                os => os.Error("Error while connecting client from server {0} @{1}: {2}", It.IsAny<object[]>()))
            .Verifiable();


        capturedOnCxAcceptCallback?.Invoke();

        moqSocketReceiver.Verify();
        moqFlogger.Verify();
    }

    [TestMethod]
    public void TwoConnectedClientsServer_Broadcast_SendsMessageForEachClient()
    {
        var moqFirstClientSender = new Mock<IStreamPublisher>();
        var moqFirstClient = new Mock<IConversationRequester>();
        moqFirstClient.Setup(cr => cr.StreamPublisher).Returns(moqFirstClientSender.Object).Verifiable();
        moqFirstClient.SetupGet(cr => cr.Id).Returns(1);
        moqFirstClientSender.Setup(cr => cr.Send(moqVersionedMessage.Object)).Verifiable();
        var moqSecondClientSender = new Mock<IStreamPublisher>();
        var moqSecondClient = new Mock<IConversationRequester>();
        moqSecondClient.Setup(cr => cr.StreamPublisher).Returns(moqSecondClientSender.Object).Verifiable();
        moqSecondClient.Setup(cr => cr.Id).Returns(2);
        moqSecondClientSender.Setup(cr => cr.Send(moqVersionedMessage.Object)).Verifiable();
        var clientMap = new Dictionary<int, IConversationRequester>
        {
            { moqFirstClient.Object.Id, moqFirstClient.Object }, { moqSecondClient.Object.Id, moqSecondClient.Object }
        };
        NonPublicInvocator.SetInstanceField(tcpAcceptorControls, "clients", clientMap);

        tcpAcceptorControls.Broadcast(moqVersionedMessage.Object);

        moqFirstClient.Verify();
        moqSecondClient.Verify();
        moqFirstClientSender.Verify();
        moqSecondClientSender.Verify();
    }
}
