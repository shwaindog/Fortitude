#region

using System.Net;
using System.Net.Sockets;
using FortitudeCommon.AsyncProcessing;
using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeCommon.Types;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.ORX.Authentication;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Transports.Sockets.Dispatcher;
using FortitudeIO.Transports.Sockets.Publishing;
using FortitudeIO.Transports.Sockets.SessionConnection;
using FortitudeIO.Transports.Sockets.Subscription;
using Moq;

#endregion

namespace FortitudeTests.FortitudeIO.Transports.Sockets.Publishing;

[TestClass]
public class TcpSocketPublisherTests
{
    private DummyTcpSocketPublisher dummyTcpSocketPublisher = null!;
    private Mock<IMessageIdSerializationRepository> moqBinSerialFac = null!;
    private Mock<IMessageSerializer> moqBinSerializer = null!;
    private Mock<IBinaryStreamSubscriber> moqBinStreamSubscriber = null!;
    private Mock<ISyncLock> moqClientsLock = null!;
    private Mock<ISyncLock> moqConnLock = null!;
    private Mock<IFLogger> moqFLogger = null!;
    private Mock<IOSNetworkingController> moqNetworkingController = null!;
    private Mock<IOSSocket> moqSocket = null!;
    private Mock<ISocketDispatcher> moqSocketDispatcher = null!;
    private Mock<ISocketDispatcherListener> moqSocketDispatcherListener = null!;
    private Mock<ISocketDispatcherSender> moqSocketDispatcherSender = null!;
    private Mock<ISocketSubscriber> moqSocketSubscriber = null!;
    private int port;
    private int sendBufferSize;
    private string sessionDescription = null!;

    [TestInitialize]
    public void SetUp()
    {
        moqFLogger = new Mock<IFLogger>();
        moqSocketDispatcherListener = new Mock<ISocketDispatcherListener>();
        moqSocketDispatcherSender = new Mock<ISocketDispatcherSender>();
        moqSocketDispatcher = new Mock<ISocketDispatcher>();
        moqSocketDispatcher.SetupProperty(sd => sd.Listener, moqSocketDispatcherListener.Object);
        moqSocketDispatcher.SetupProperty(sd => sd.Sender, moqSocketDispatcherSender.Object);
        sessionDescription = "testSessionDescription";
        moqBinStreamSubscriber = new Mock<IBinaryStreamSubscriber>();
        moqSocketSubscriber = moqBinStreamSubscriber.As<ISocketSubscriber>();
        moqBinSerialFac = new Mock<IMessageIdSerializationRepository>();
        moqSocket = new Mock<IOSSocket>();
        moqSocket.SetupAllProperties();
        moqNetworkingController = new Mock<IOSNetworkingController>();

        moqConnLock = new Mock<ISyncLock>();
        moqClientsLock = new Mock<ISyncLock>();

        sendBufferSize = 1234567;
        port = 34567;
        dummyTcpSocketPublisher = new DummyTcpSocketPublisher(moqFLogger.Object,
            moqSocketDispatcher.Object, moqNetworkingController.Object, port, sessionDescription,
            moqBinSerialFac.Object, sendBufferSize, moqBinStreamSubscriber.Object);

        NonPublicInvocator.SetInstanceField(dummyTcpSocketPublisher, "connSync", moqConnLock.Object);
        NonPublicInvocator.SetInstanceField(dummyTcpSocketPublisher, "clientsSync", moqClientsLock.Object);

        moqBinSerializer = new Mock<IMessageSerializer>();
        moqNetworkingController.Setup(nc => nc.CreateOSSocket(AddressFamily.InterNetwork,
            SocketType.Stream, ProtocolType.Tcp)).Returns(moqSocket.Object).Verifiable();
    }

    [TestMethod]
    public void NewSocketStream_RegisterAcceptor_ReturnsConfiguredSocketSessionConnection()
    {
        moqSocketDispatcher.Setup(d => d.Listener.RegisterForListen(
            It.IsAny<ISocketSessionConnection>())).Verifiable();

        moqSocket.SetupAllProperties();

        var socketSessConnection = dummyTcpSocketPublisher.RegisterAcceptor(moqSocket.Object, connection => { });

        Assert.IsNotNull(socketSessConnection);
        Assert.AreEqual(dummyTcpSocketPublisher.SendBufferSize, moqSocket.Object.SendBufferSize);
        Assert.AreEqual(sessionDescription, socketSessConnection.SessionDescription);
        Assert.AreEqual(moqSocket.Object, socketSessConnection.Socket);
        moqSocketDispatcher.Verify();
    }

    [TestMethod]
    public void UnconnectedSocketPublisher_Connect_SyncProtectsCreateSocketAndConnectionCallsOnConnect()
    {
        var inConnSyncLock = false;

        moqSocket.Setup(s => s.Bind(new IPEndPoint(IPAddress.Any, port))).Verifiable();

        moqConnLock.Setup(cl => cl.Acquire()).Callback(() => { inConnSyncLock = true; }).Verifiable();
        moqConnLock.Setup(cl => cl.Release()).Callback(() => { inConnSyncLock = false; }).Verifiable();

        moqSocketDispatcher.Setup(d => d.Start()).Callback(() => { Assert.IsTrue(inConnSyncLock); }).Verifiable();
        moqFLogger.Setup(d => d.Info(It.IsAny<string>(), It.IsAny<object[]>()))
            .Callback(() => { Assert.IsTrue(inConnSyncLock); })
            .Verifiable();

        Assert.IsFalse(inConnSyncLock);
        var calledOnConnected = false;
        dummyTcpSocketPublisher.OnConnected += () => { calledOnConnected = true; };

        dummyTcpSocketPublisher.Connect();

        Assert.IsTrue(calledOnConnected);
        Assert.IsFalse(inConnSyncLock);
        Assert.IsNotNull(dummyTcpSocketPublisher.AcceptorSession!.SessionReceiver!.Socket);
        Assert.AreEqual(true, dummyTcpSocketPublisher.AcceptorSession.SessionReceiver.Socket.NoDelay);
        Assert.IsNotNull(dummyTcpSocketPublisher.AcceptorSession);
        moqNetworkingController.Verify();
        moqSocket.Verify();
        moqConnLock.Verify();
        moqSocketDispatcher.Verify();
        moqFLogger.Verify();
    }

    [TestMethod]
    public void ConnectedSocketPublisher_Connect_DoesNothing()
    {
        moqConnLock.Setup(cl => cl.Acquire()).Verifiable();
        moqConnLock.Setup(cl => cl.Release()).Verifiable();
        moqSocketDispatcher.Setup(d => d.Start()).Verifiable();
        moqFLogger.Setup(d => d.Info(It.IsAny<string>(), It.IsAny<object[]>())).Verifiable();
        var calledOnConnected = false;
        dummyTcpSocketPublisher.OnConnected += () => { calledOnConnected = true; };
        dummyTcpSocketPublisher.Connect();
        Assert.IsTrue(calledOnConnected);
        calledOnConnected = false;
        moqConnLock.Reset();
        moqSocketDispatcher.Reset();
        moqFLogger.Reset();

        dummyTcpSocketPublisher.Connect();

        Assert.IsFalse(calledOnConnected);
        moqSocketDispatcher.Verify(d => d.Start(), Times.Never);
        moqConnLock.Verify();
        moqFLogger.Verify(d => d.Info(It.IsAny<string>(), It.IsAny<object[]>()), Times.Never);
    }

    [TestMethod]
    public void ConnectedSocketPublisher_Disconnect_SyncProtectsStopsDispatcherAlertsClientsCallsOnDisconnect()
    {
        dummyTcpSocketPublisher.Connect();

        var inConnSyncLock = false;
        moqConnLock.Setup(cl => cl.Acquire()).Callback(() => { inConnSyncLock = true; }).Verifiable();
        moqConnLock.Setup(cl => cl.Release()).Callback(() => { inConnSyncLock = false; }).Verifiable();

        var inClientSyncLock = false;
        moqClientsLock.Setup(cl => cl.Acquire()).Callback(() => { inClientSyncLock = true; }).Verifiable();
        moqClientsLock.Setup(cl => cl.Release()).Callback(() => { inClientSyncLock = false; }).Verifiable();

        moqSocketDispatcher.Setup(d => d.Stop()).Callback(() => { Assert.IsTrue(inConnSyncLock); }).Verifiable();
        moqFLogger.Setup(d => d.Info(It.IsAny<string>(), It.IsAny<object[]>()))
            .Callback(() => { Assert.IsTrue(inConnSyncLock); })
            .Verifiable();

        var calledOnDisconnecting = false;
        dummyTcpSocketPublisher.OnDisconnecting += () => { calledOnDisconnecting = true; };
        var calledOnDisconnected = false;
        dummyTcpSocketPublisher.OnDisconnected += () => { calledOnDisconnected = true; };
        Assert.IsFalse(inConnSyncLock);

        var moqFirstClient = new Mock<ISocketSessionConnection>();
        var moqSecondClient = new Mock<ISocketSessionConnection>();
        moqFirstClient.SetupAllProperties();
        moqFirstClient.SetupGet(ssc => ssc.Socket)
            .Returns(moqSocket.Object)
            .Callback(() => { Assert.IsTrue(inClientSyncLock); })
            .Verifiable();
        moqSecondClient.SetupAllProperties();
        moqSecondClient.SetupGet(ssc => ssc.Socket)
            .Returns(moqSocket.Object)
            .Callback(() => { Assert.IsTrue(inClientSyncLock); })
            .Verifiable();
        moqSocketDispatcher.Setup(d => d.Listener.UnregisterForListen(moqFirstClient.Object)).Callback(() =>
        {
            Assert.IsTrue(inClientSyncLock);
            Assert.IsTrue(inConnSyncLock);
        }).Verifiable();
        moqSocketDispatcher.Setup(d => d.Listener.UnregisterForListen(moqSecondClient.Object)).Callback(() =>
        {
            Assert.IsTrue(inClientSyncLock);
            Assert.IsTrue(inConnSyncLock);
        }).Verifiable();

        var clientsList =
            NonPublicInvocator.GetInstanceField<IDoublyLinkedList<ISocketSessionConnection>>(dummyTcpSocketPublisher,
                "clients");
        clientsList.AddLast(moqSecondClient.Object);
        clientsList.AddFirst(moqFirstClient.Object);

        dummyTcpSocketPublisher.Disconnect();

        Assert.IsTrue(calledOnDisconnecting);
        Assert.IsTrue(calledOnDisconnected);
        Assert.IsFalse(inConnSyncLock);
        Assert.IsFalse(inClientSyncLock);
        Assert.IsNull(dummyTcpSocketPublisher.AcceptorSession);
        moqFirstClient.Verify();
        moqSecondClient.Verify();
        moqConnLock.Verify();
        moqClientsLock.Verify();
        moqSocketDispatcher.Verify();
        moqFLogger.Verify();
    }

    [TestMethod]
    public void UnconnectedSocketPublisher_Disconnect_DoesNothing()
    {
        moqConnLock.Setup(cl => cl.Acquire()).Verifiable();
        moqConnLock.Setup(cl => cl.Release()).Verifiable();

        var calledOnDisconnecting = false;
        dummyTcpSocketPublisher.OnDisconnecting += () => { calledOnDisconnecting = true; };
        var calledOnDisconnected = false;
        dummyTcpSocketPublisher.OnDisconnected += () => { calledOnDisconnected = true; };

        dummyTcpSocketPublisher.Disconnect();

        Assert.IsFalse(calledOnDisconnecting);
        Assert.IsFalse(calledOnDisconnected);
        Assert.IsNull(dummyTcpSocketPublisher.AcceptorSession);
        moqConnLock.Verify();
        moqFLogger.Verify(d => d.Info(It.IsAny<string>(), It.IsAny<object[]>()), Times.Never);
        moqSocketDispatcher.Verify(d => d.Stop(), Times.Never);
    }

    [TestMethod]
    public void ConnectedSocket_IsConnect_ReturnsTrue()
    {
        dummyTcpSocketPublisher.Connect();

        moqSocket.SetupGet(s => s.Connected).Returns(true);

        Assert.IsTrue(dummyTcpSocketPublisher.IsConnected);
    }

    [TestMethod]
    public void BoundSocket_IsConnect_ReturnsTrue()
    {
        dummyTcpSocketPublisher.Connect();

        moqSocket.SetupGet(s => s.IsBound).Returns(true);

        Assert.IsTrue(dummyTcpSocketPublisher.IsConnected);
    }

    [TestMethod]
    public void ConnectedRegisteredMessage_Send_EnquesMessageWithSerializer()
    {
        var toBeSentToCx = new OrxLogonResponse();
        moqBinSerialFac.Setup(d => d.GetSerializer<DummyMessage>(toBeSentToCx.MessageId))
            .Returns(moqBinSerializer.Object);
        dummyTcpSocketPublisher.RegisterSerializer<DummyMessage>(toBeSentToCx.MessageId);
        var moqSocketSessionSender = new Mock<ISocketSessionSender>();
        moqSocketSessionSender.SetupAllProperties();
        var socketSessionConnection = new SocketSessionConnection(null, moqSocketSessionSender.Object, null);

        moqSocketSessionSender.Setup(ssc => ssc.Enqueue(toBeSentToCx, moqBinSerializer.Object)).Verifiable();
        moqSocketDispatcher.Setup(d => d.Sender.AddToSendQueue(socketSessionConnection)).Verifiable();

        dummyTcpSocketPublisher.Send(socketSessionConnection, toBeSentToCx);
        moqSocketSessionSender.Verify();
        moqSocketDispatcher.Verify();

        moqSocketDispatcher.Reset();
        moqSocketSessionSender.Setup(ssc => ssc.Enqueue(toBeSentToCx, moqBinSerializer.Object)).Verifiable();
        moqSocketDispatcher.Setup(d => d.Sender.AddToSendQueue(socketSessionConnection)).Verifiable();

        dummyTcpSocketPublisher.Send(socketSessionConnection, toBeSentToCx);

        moqSocketDispatcher.Verify();
        moqSocketSessionSender.Verify();
    }

    [TestMethod]
    public void ConnectedClient_OnCxError_UnregistersAndRemovesClient()
    {
        moqClientsLock.Setup(cl => cl.Acquire()).Verifiable();
        moqClientsLock.Setup(cl => cl.Release()).Verifiable();

        var moqFirstClient = new Mock<ISocketSessionConnection>();
        var moqSecondClient = new Mock<ISocketSessionConnection>();
        moqFirstClient.SetupAllProperties();
        moqSecondClient.SetupAllProperties();
        moqSecondClient.SetupGet(ssc => ssc.Socket).Returns(moqSocket.Object).Verifiable();
        moqSocketDispatcher.Setup(d => d.Listener.UnregisterForListen(moqSecondClient.Object)).Verifiable();

        var onClientRemovedCalled = false;
        dummyTcpSocketPublisher.OnClientRemoved += connection => { onClientRemovedCalled = true; };

        var clientsList =
            NonPublicInvocator.GetInstanceField<IDoublyLinkedList<ISocketSessionConnection>>(dummyTcpSocketPublisher,
                "clients");
        clientsList.AddLast(moqSecondClient.Object);
        clientsList.AddFirst(moqFirstClient.Object);
        moqFLogger.Setup(d => d.Info(It.IsAny<string>(), It.IsAny<object[]>())).Verifiable();

        dummyTcpSocketPublisher.OnConnectionError!(moqSecondClient.Object, "Error Message", 1234);

        Assert.IsTrue(onClientRemovedCalled);
        moqFLogger.Verify();
        moqSocketDispatcher.Verify();
        Assert.AreEqual(moqFirstClient.Object, clientsList.Head);
        Assert.AreEqual(moqFirstClient.Object, clientsList.Tail);
    }

    [TestMethod]
    public void ConnectedServer_OnClientConnect_SyncProtectsCreatesClientSocketAddsToList()
    {
        dummyTcpSocketPublisher.Connect();

        var inClientSyncLock = false;

        moqClientsLock.Setup(cl => cl.Acquire()).Callback(() => { inClientSyncLock = true; }).Verifiable();
        moqClientsLock.Setup(cl => cl.Release()).Callback(() => { inClientSyncLock = false; }).Verifiable();

        var moqClientSession = new Mock<ISocketSessionConnection>();
        moqClientSession.SetupAllProperties();

        moqFLogger.Setup(fl => fl.Info(It.IsAny<string>(), It.IsAny<object[]>())).Verifiable();


        var moqSecondSocket = new Mock<IOSSocket>();

        moqSocket.Setup(oss => oss.Accept()).Returns(moqSecondSocket.Object).Verifiable();
        moqSocketSubscriber.Setup(sli => sli.RegisterConnector(moqSecondSocket.Object))
            .Returns(moqClientSession.Object)
            .Verifiable();
        moqClientSession.Setup(ssc => ssc.Socket).Returns(moqSecondSocket.Object).Verifiable();
        moqSecondSocket.SetupGet(coss => coss.RemoteEndPoint)
            .Returns(new IPEndPoint(new IPAddress(new byte[] { 0, 0, 0, 0 }), 12345));
        moqClientSession.SetupGet(cssc => cssc.Socket).Returns(moqSecondSocket.Object).Verifiable();

        var onNewClientCalled = false;
        dummyTcpSocketPublisher.OnNewClient += connection => { onNewClientCalled = true; };

        var moqDoublyLinkedList = new Mock<IDoublyLinkedList<ISocketSessionConnection>>();
        moqDoublyLinkedList.SetupAllProperties();
        moqDoublyLinkedList.Setup(dll => dll.AddLast(moqClientSession.Object))
            .Callback(() => Assert.IsTrue(inClientSyncLock)).Returns(moqClientSession.Object)
            .Verifiable();

        NonPublicInvocator.SetInstanceField(dummyTcpSocketPublisher, "clients", moqDoublyLinkedList.Object);

        dummyTcpSocketPublisher.AcceptorSession!.SessionReceiver!.OnAccept();

        Assert.IsTrue(onNewClientCalled);
        moqClientsLock.Verify();
        moqSocketSubscriber.Verify();
        moqDoublyLinkedList.Verify();
        moqSocket.Verify();
    }


    [TestMethod]
    public void ConnectedClientServer_RemoveClient_SyncProtectsRemoveClient()
    {
        dummyTcpSocketPublisher.Connect();

        var inClientSyncLock = false;

        moqClientsLock.Setup(cl => cl.Acquire()).Callback(() => { inClientSyncLock = true; }).Verifiable();
        moqClientsLock.Setup(cl => cl.Release()).Callback(() => { inClientSyncLock = false; }).Verifiable();

        var moqClientSession = new Mock<ISocketSessionConnection>();
        moqClientSession.SetupAllProperties();

        moqFLogger.Setup(fl => fl.Info(It.IsAny<string>(), It.IsAny<object[]>())).Verifiable();

        var moqSecondSocket = new Mock<IOSSocket>();
        moqSecondSocket.SetupGet(coss => coss.RemoteEndPoint)
            .Returns(new IPEndPoint(new IPAddress(new byte[] { 0, 0, 0, 0 }), 12345));
        moqClientSession.SetupGet(cssc => cssc.Socket).Returns(moqSecondSocket.Object).Verifiable();

        var onClientRemovedCalled = false;
        dummyTcpSocketPublisher.OnClientRemoved += connection => { onClientRemovedCalled = true; };

        var moqDoublyLinkedList = new Mock<IDoublyLinkedList<ISocketSessionConnection>>();
        moqDoublyLinkedList.SetupAllProperties();
        moqDoublyLinkedList.Setup(dll => dll.Remove(moqClientSession.Object))
            .Callback(() => Assert.IsTrue(inClientSyncLock)).Returns(moqClientSession.Object)
            .Verifiable();

        NonPublicInvocator.SetInstanceField(dummyTcpSocketPublisher, "clients", moqDoublyLinkedList.Object);

        dummyTcpSocketPublisher.RemoveClient(moqClientSession.Object);

        Assert.IsTrue(onClientRemovedCalled);
        moqClientsLock.Verify();
        moqSocketSubscriber.Verify();
        moqDoublyLinkedList.Verify();
    }

    [TestMethod]
    public void TwoConnectedClientsServer_Broadcast_SyncTwoEnqueingForBroadcast()
    {
        dummyTcpSocketPublisher.Connect();


        var message = new OrxLogonResponse();
        moqBinSerialFac.Setup(d => d.GetSerializer<DummyMessage>(message.MessageId))
            .Returns(moqBinSerializer.Object);
        dummyTcpSocketPublisher.RegisterSerializer<DummyMessage>(message.MessageId);

        var inClientSyncLock = false;

        moqClientsLock.Setup(cl => cl.Acquire()).Callback(() => { inClientSyncLock = true; }).Verifiable();
        moqClientsLock.Setup(cl => cl.Release()).Callback(() => { inClientSyncLock = false; }).Verifiable();

        var moqSocketSession1Sender = new Mock<ISocketSessionSender>();
        var cientSession1 = new SocketSessionConnection(null, moqSocketSession1Sender.Object, null);

        var moqSocketSession2Sender = new Mock<ISocketSessionSender>();
        var clientSession2 = new SocketSessionConnection(null, moqSocketSession2Sender.Object, null);

        var clientConns = NonPublicInvocator.GetInstanceField<IDoublyLinkedList<ISocketSessionConnection>>
            (dummyTcpSocketPublisher, "clients");

        clientConns.AddLast(clientSession2);
        clientConns.AddFirst(cientSession1);

        moqSocketDispatcher.Setup(d => d.Sender.AddToSendQueue(cientSession1))
            .Callback(() => Assert.IsTrue(inClientSyncLock)).Verifiable();
        moqSocketDispatcher.Setup(d => d.Sender.AddToSendQueue(clientSession2))
            .Callback(() => Assert.IsTrue(inClientSyncLock)).Verifiable();

        moqSocketSession1Sender.Setup(ssc => ssc.Enqueue(message, moqBinSerializer.Object))
            .Callback(() => Assert.IsTrue(inClientSyncLock)).Verifiable();
        moqSocketSession2Sender.Setup(ssc => ssc.Enqueue(message, moqBinSerializer.Object))
            .Callback(() => Assert.IsTrue(inClientSyncLock)).Verifiable();

        dummyTcpSocketPublisher.Broadcast(message);

        moqClientsLock.Verify();
        moqSocketDispatcher.Verify();
        moqSocketSession1Sender.Verify();
        moqSocketSession2Sender.Verify();
    }

    private class DummyMessage : VersionedMessage
    {
        public override uint MessageId => 66666;
        public override IVersionedMessage Clone() => this;
    }

    public class DummyTcpSocketPublisher : TcpSocketPublisher
    {
        private readonly IMessageIdSerializationRepository dummySerializationRepository;

        public DummyTcpSocketPublisher(IFLogger logger, ISocketDispatcher dispatcher,
            IOSNetworkingController networkingController, int port, string sessionDescription,
            IMessageIdSerializationRepository dummySerializationRepository, int sendBufferSize,
            IBinaryStreamSubscriber streamFromSubscriber)
            : base(logger, dispatcher, networkingController, port, sessionDescription)
        {
            this.dummySerializationRepository = dummySerializationRepository;
            SendBufferSize = sendBufferSize;
            StreamFromSubscriber = streamFromSubscriber;
        }

        public override int SendBufferSize { get; }

        public override IBinaryStreamSubscriber StreamFromSubscriber { get; }
        public ISocketSessionConnection? AcceptorSession => Acceptor;

        public Action<ISocketSessionConnection, string, int>? OnConnectionError => OnCxError;

        public override IMessageIdSerializationRepository GetFactory() => dummySerializationRepository;
    }
}
