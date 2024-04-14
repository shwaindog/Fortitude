#region

using System.Net;
using System.Net.Sockets;
using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.AsyncWrappers;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Protocols.Serdes.Binary.Sockets;
using FortitudeIO.Transports.Network.Config;
using FortitudeIO.Transports.Network.Construction;
using FortitudeIO.Transports.Network.Controls;
using FortitudeIO.Transports.Network.Dispatcher;
using FortitudeIO.Transports.Network.Publishing;
using FortitudeIO.Transports.Network.Receiving;
using FortitudeIO.Transports.Network.Sockets;
using FortitudeIO.Transports.Network.State;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsCore.Pricing.PQ.Messages;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Serdes;
using FortitudeMarketsCore.Pricing.PQ.Serdes.Deserialization;
using FortitudeMarketsCore.Pricing.PQ.Subscription;
using FortitudeTests.FortitudeCommon.Chronometry;
using Moq;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription;

[TestClass]
public class PQSnapshotClientTests
{
    private uint connectionTimeoutMs = 10_000;
    private string expectedHost = null!;
    private byte[] expectedIpAddress = null!;
    private ushort expectedPort;
    private Mock<IPQClientMessageStreamDecoder> moqClientMessageStreamDecoder = null!;
    private Mock<ISocketDispatcher> moqDispatcher = null!;
    private Mock<ISocketDispatcherResolver> moqDispatcherResolver = null!;
    private Mock<IEnumerator<IEndpointConfig>> moqEndpointEnumerator = null!;
    private Mock<IFLogger> moqFlogger = null!;
    private Mock<IFLoggerFactory> moqFloggerFactory = null!;
    private Mock<IIntraOSThreadSignal> moqIIntraOSThreadSignal = null!;
    private Mock<IInitiateControls> moqInitiateControls = null!;
    private Mock<IIntraOSThreadSignal> moqIntraOsThreadSignal = null!;
    private Mock<IOSNetworkingController> moqNetworkingController = null!;
    private Mock<IOSSocket> moqOsSocket = null!;
    private Mock<IOSParallelController> moqParallelController = null!;
    private Mock<IOSParallelControllerFactory> moqParallelControllerFactory = null!;
    private Mock<IPQClientSerdesRepositoryFactory> moqPqClientSerdesRepoFactory = null!;
    private Mock<IPQClientQuoteDeserializerRepository> moqPQQuoteDeserializationRepo = null!;
    private Mock<IMap<uint, IMessageDeserializer>> moqSerializerCache = null!;
    private Mock<INotifyingMessageDeserializer<PQLevel0Quote>> moqSocketBinaryDeserializer = null!;
    private Mock<ISocketConnection> moqSocketConnection = null!;
    private Mock<IEndpointConfig> moqSocketConnectionConfig = null!;
    private Mock<ISocketFactoryResolver> moqSocketFactories = null!;
    private Mock<ISocketFactory> moqSocketFactory = null!;
    private Mock<ISocketSender> moqSocketSender = null!;
    private Mock<ISocketSessionContext> moqSocketSessionContext = null!;
    private Mock<INetworkTopicConnectionConfig> moqSocketTopicConnectionConfig = null!;
    private Mock<ITimerCallbackSubscription> moqTimerCallbackSubscription = null!;
    private PQSnapshotClient pqSnapshotClient = null!;

    private IList<ISourceTickerQuoteInfo> sendSrcTkrIds = null!;
    private string sessionDescription = null!;
    private TimeContextTests.StubTimeContext stubContext = null!;

    [TestInitialize]
    public void SetUp()
    {
        moqDispatcher = new Mock<ISocketDispatcher>();
        moqDispatcherResolver = new Mock<ISocketDispatcherResolver>();
        moqInitiateControls = new Mock<IInitiateControls>();
        moqSocketSessionContext = new Mock<ISocketSessionContext>();
        moqSocketFactories = new Mock<ISocketFactoryResolver>();
        moqSocketFactory = new Mock<ISocketFactory>();
        moqParallelController = new Mock<IOSParallelController>();
        moqIntraOsThreadSignal = new Mock<IIntraOSThreadSignal>();
        moqSocketSender = new Mock<ISocketSender>();
        moqParallelControllerFactory = new Mock<IOSParallelControllerFactory>();
        moqIIntraOSThreadSignal = new Mock<IIntraOSThreadSignal>();
        moqSocketConnection = new Mock<ISocketConnection>();
        moqPqClientSerdesRepoFactory = new Mock<IPQClientSerdesRepositoryFactory>();
        moqParallelControllerFactory.SetupGet(pcf => pcf.GetOSParallelController)
            .Returns(moqParallelController.Object);

        moqParallelController.Setup(pcf => pcf.SingleOSThreadActivateSignal(It.IsAny<bool>()))
            .Returns(moqIIntraOSThreadSignal.Object);
        OSParallelControllerFactory.Instance = moqParallelControllerFactory.Object;
        moqTimerCallbackSubscription = new Mock<ITimerCallbackSubscription>();
        moqNetworkingController = new Mock<IOSNetworkingController>();
        moqSocketConnectionConfig = new Mock<IEndpointConfig>();
        moqSocketTopicConnectionConfig = new Mock<INetworkTopicConnectionConfig>();
        moqEndpointEnumerator = new Mock<IEnumerator<IEndpointConfig>>();
        sessionDescription = "TestSocketDescription PQSnapshotClient";
        moqPQQuoteDeserializationRepo = new Mock<IPQClientQuoteDeserializerRepository>();
        moqClientMessageStreamDecoder = new Mock<IPQClientMessageStreamDecoder>();
        moqSocketBinaryDeserializer = new Mock<INotifyingMessageDeserializer<PQLevel0Quote>>();
        moqOsSocket = new Mock<IOSSocket>();
        stubContext = new TimeContextTests.StubTimeContext();
        TimeContext.Provider = stubContext;
        stubContext.UtcNow = new DateTime(2018, 01, 29, 19, 54, 12);
        moqFlogger = new Mock<IFLogger>();
        moqFloggerFactory = new Mock<IFLoggerFactory>();
        moqFloggerFactory.Setup(flf => flf.GetLogger(It.IsAny<Type>())).Returns(moqFlogger.Object);
        moqFloggerFactory.Setup(flf => flf.GetLogger(It.IsAny<string>())).Returns(new Mock<IFLogger>().Object);
        FLoggerFactory.Instance = moqFloggerFactory.Object;

        var moqSocketConnectivityChanged = new Mock<ISocketConnectivityChanged>();
        Func<ISocketSessionContext, ISocketConnectivityChanged> moqCallback = context =>
            moqSocketConnectivityChanged.Object;
        moqSocketConnection.SetupGet(sc => sc.IsConnected).Returns(false);
        moqParallelController.Setup(pc => pc.SingleOSThreadActivateSignal(false))
            .Returns(moqIntraOsThreadSignal.Object).Verifiable();
        moqSocketSessionContext.SetupGet(ssc => ssc.Name).Returns("New Client Connection");
        moqSocketSessionContext.SetupGet(ssc => ssc.SocketConnection).Returns(moqSocketConnection.Object);
        moqSocketSessionContext.SetupGet(ssc => ssc.SocketFactoryResolver).Returns(moqSocketFactories.Object);
        moqSocketSessionContext.SetupGet(ssc => ssc.NetworkTopicConnectionConfig)
            .Returns(moqSocketTopicConnectionConfig.Object);
        moqSocketSessionContext.SetupGet(ssc => ssc.SerdesFactory).Returns(moqPqClientSerdesRepoFactory.Object);
        moqSocketSessionContext.SetupGet(ssc => ssc.SocketSender).Returns(moqSocketSender.Object);
        moqSocketSessionContext.SetupAdd(ssc => ssc.SocketConnected += null);
        expectedHost = "TestHostname";
        expectedPort = 1979;
        expectedIpAddress = new byte[] { 61, 26, 5, 6 };
        moqSocketConnectionConfig.SetupGet(scc => scc.InstanceName).Returns("PQSnapshotClientTests");
        moqSocketConnectionConfig.SetupGet(scc => scc.Hostname).Returns(expectedHost);
        moqSocketConnectionConfig.SetupGet(scc => scc.Port).Returns(expectedPort);
        moqEndpointEnumerator.SetupGet(stcc => stcc.Current).Returns(moqSocketConnectionConfig.Object);
        moqSocketTopicConnectionConfig.SetupGet(scc => scc.TopicDescription).Returns("PQSnapshotClientTests");
        moqSocketTopicConnectionConfig.SetupGet(scc => scc.ConnectionTimeoutMs).Returns(connectionTimeoutMs);
        moqSocketFactories.SetupGet(pcf => pcf.SocketFactory).Returns(moqSocketFactory.Object);
        moqSocketFactories.SetupGet(pcf => pcf.NetworkingController).Returns(moqNetworkingController.Object);
        moqSocketFactories.SetupGet(pcf => pcf.ConnectionChangedHandlerResolver).Returns(moqCallback);
        moqSocketFactories.SetupGet(pcf => pcf.SocketDispatcherResolver).Returns(moqDispatcherResolver.Object);
        moqSocketFactories.SetupGet(pcf => pcf.ParallelController).Returns(moqParallelController.Object);
        moqPQQuoteDeserializationRepo.Setup(qdr => qdr.Supply()).Returns(moqClientMessageStreamDecoder.Object);
        moqPqClientSerdesRepoFactory.SetupGet(sf => sf.MessageDeserializationRepository).Returns(moqPQQuoteDeserializationRepo.Object);
        moqPqClientSerdesRepoFactory.SetupGet(sf => sf.MessageSerializationRepository).Returns(new Mock<IMessageSerializationRepository>().Object);

        moqSocketSessionContext.SetupAdd(sc => sc.Connected += null);
        moqFlogger.Setup(fl => fl.Info(It.IsAny<string>(), It.IsAny<object[]>()));
        moqSerializerCache = new Mock<IMap<uint, IMessageDeserializer>>();
        moqOsSocket.SetupAllProperties();

        sendSrcTkrIds = new List<ISourceTickerQuoteInfo>
        {
            new SourceTickerQuoteInfo(7, "FirstSource", 7, "FirstTicker")
            , new SourceTickerQuoteInfo(77, "FirstSource", 77, "SecondTicker")
            , new SourceTickerQuoteInfo(15, "FirstSource", 16, "ThirdTicker")
            , new SourceTickerQuoteInfo(19, "FirstSource", 19, "FourthTicker")
            , new SourceTickerQuoteInfo(798, "FirstSource", 798, "FifthTicker")
        };

        moqSocketBinaryDeserializer.SetupAllProperties();

        moqPQQuoteDeserializationRepo.Setup(pqqsf => pqqsf.GetDeserializer<PQLevel0Quote>(uint.MaxValue))
            .Returns(moqSocketBinaryDeserializer.Object).Verifiable();

        pqSnapshotClient = new PQSnapshotClient(moqSocketSessionContext.Object, moqInitiateControls.Object);

        moqFlogger.Setup(fl => fl.Info("Attempting TCP connection to {0} on {1}:{2}",
            sessionDescription, expectedHost, expectedPort)).Verifiable();
        moqNetworkingController.Setup(nc => nc.CreateOSSocket(AddressFamily.InterNetwork, SocketType.Stream,
            ProtocolType.Tcp)).Returns(moqOsSocket.Object).Verifiable();
        moqNetworkingController.Setup(nc => nc.GetIpAddress(expectedHost)).Returns(
            new IPAddress(expectedIpAddress)).Verifiable();
    }

    [TestCleanup]
    public void TearDown()
    {
        OSParallelControllerFactory.Instance = new OSParallelControllerFactory();
        FLoggerFactory.Instance = new FLoggerFactory();
        TimeContext.Provider = new HighPrecisionTimeContext();
    }


    [TestMethod]
    public void PQSnapshotClient_RequestSnapshots_ConnectsStartsConnectionTimeoutSendRequestIds()
    {
        moqSocketConnection.SetupGet(sc => sc.IsConnected).Returns(true);
        moqFlogger.Reset();
        moqFlogger.Setup(fl => fl.Info("Sending snapshot request for streams {0}", It.IsAny<object[]>()))
            .Verifiable();

        ConnectMoqSetup();

        pqSnapshotClient.RequestSnapshots(sendSrcTkrIds);

        VerifyFullConnectedCalled();
        moqFlogger.Verify();
    }

    [TestMethod]
    public void PQSnapshotClientNotYetConnected_RequestSnapshots_SchedulesConnectQueuesIdsForSend()
    {
        ConnectMoqSetup();
        moqFlogger.Reset();
        moqFlogger.Setup(fl => fl.Info("Queuing snapshot request for ticker ids {0}", It.IsAny<object[]>()))
            .Verifiable();
        moqParallelController.Setup(pc => pc.CallFromThreadPool(It.IsAny<WaitCallback>())).Verifiable();

        pqSnapshotClient.RequestSnapshots(sendSrcTkrIds);

        moqFlogger.Verify();
    }

    [TestMethod]
    public void AlreadyQueuedIds_RequestSnapshots_LogsIdsAlreadyQueuedForSendOnConnect()
    {
        ConnectMoqSetup();
        moqParallelController.Setup(pc => pc.CallFromThreadPool(It.IsAny<WaitCallback>())).Verifiable();

        pqSnapshotClient.RequestSnapshots(sendSrcTkrIds);
        moqFlogger.Reset();
        moqFlogger.Setup(fl => fl.Info(It.IsAny<string>(), It.IsAny<object[]>()))
            .Callback((string s1, object[] s2) =>
                Console.Out.WriteLine($"Logger Received ({s1}, [{string.Join(", ", s2)}])"));
        moqFlogger.Setup(fl => fl.Info(It.IsAny<string>(), It.IsAny<string>()))
            .Callback((string s1, object[] s2) =>
                Console.Out.WriteLine($"Logger Received ({s1}, [{string.Join(", ", s2)}])"));
        moqFlogger.Setup(fl => fl.Info("Snapshot request already queued for ticker ids {0}, last snapshot sent at {1}",
                It.IsAny<object[]>()))
            .Verifiable();
        pqSnapshotClient.RequestSnapshots(sendSrcTkrIds);

        moqFlogger.Verify();
    }

    [TestMethod]
    public void QueuedTickerIdsForRequest_OnConnect_SendsTickerIdsWhenConnected()
    {
        ConnectMoqSetup();

        moqFlogger.Reset();
        moqFlogger.Setup(fl => fl.Info("Sending queued snapshot requests for streams: {0}", It.IsAny<object[]>()))
            .Verifiable();

        pqSnapshotClient.RequestSnapshots(sendSrcTkrIds);

        moqSocketSessionContext.Raise(sc => sc.Connected += null);

        moqFlogger.Verify();
    }

    [TestMethod]
    public void ConnectedPQSnapshotClient_GetDecoderOnResponse_ResetsDisconnectionTimer()
    {
        ConnectMoqSetup();

        var moqSocketReceiver = new Mock<ISocketReceiver>();
        var moqSourceTickerResponseDeserializer = new Mock<INotifyingMessageDeserializer<PQSourceTickerInfoResponse>>();
        moqSocketSessionContext.Setup(ssc => ssc.SocketReceiver).Returns(moqSocketReceiver.Object);
        var decoder = new PQClientMessageStreamDecoder(moqPQQuoteDeserializationRepo.Object);
        moqSocketReceiver.SetupGet(sr => sr.Decoder).Returns(decoder);
        moqSocketSessionContext.SetupAdd(ssc => ssc.SocketReceiverUpdated += It.IsAny<Action>());
        moqPQQuoteDeserializationRepo.Setup(qdr => qdr.Supply())
            .Returns(decoder);
        moqPQQuoteDeserializationRepo.Setup(qdr => qdr.RegisterDeserializer<PQSourceTickerInfoResponse>())
            .Returns(moqSourceTickerResponseDeserializer.Object);
        pqSnapshotClient = new PQSnapshotClient(moqSocketSessionContext.Object, moqInitiateControls.Object);
        pqSnapshotClient.Connect();
        moqSocketSessionContext.Raise(ssc => ssc.SocketReceiverUpdated += null);

        moqTimerCallbackSubscription.Setup(tcs => tcs.Unregister(moqIntraOsThreadSignal.Object)).Verifiable();

        decoder.Process(new SocketBufferReadContext
        {
            EncodedBuffer = new ReadWriteBuffer(new byte[]
            {
                1, (byte)PQMessageFlags.None, 0, 0, 0x4C, 0x7B, 0, 0, 0, 14, 0, 0, 0, 1
            })
            {
                WriteCursor = 14
            }
        });
        Assert.IsNotNull(decoder);
        moqParallelController.Verify();
        moqTimerCallbackSubscription.Verify();
        moqSerializerCache.Verify();
    }

    [TestMethod]
    public void ConnectingPQSnapshotClient_TimeoutConnection_CallsDisconnect()
    {
        ConnectMoqSetup();
        WaitOrTimerCallback? callback = null;
        moqParallelController.Setup(pc => pc.ScheduleWithEarlyTrigger(It.IsAny<IIntraOSThreadSignal>(),
                It.IsAny<WaitOrTimerCallback>(), connectionTimeoutMs, false))
            .Callback((IIntraOSThreadSignal iosts, WaitOrTimerCallback wotc, uint period, bool repeat) => { callback = wotc; })
            .Returns(moqTimerCallbackSubscription.Object).Verifiable();
        moqFlogger.Reset();
        DisconnectMoqSetup();

        pqSnapshotClient.Connect();
        callback!(moqIntraOsThreadSignal.Object, true);

        moqFlogger.Verify();
        moqDispatcher.Verify();
        moqOsSocket.Verify();
        moqTimerCallbackSubscription.Verify();
    }

    private void ConnectMoqSetup()
    {
        moqParallelController.Setup(pc => pc.ScheduleWithEarlyTrigger(It.IsAny<IIntraOSThreadSignal>(),
                It.IsAny<WaitOrTimerCallback>(), connectionTimeoutMs, false))
            .Returns(moqTimerCallbackSubscription.Object)
            .Verifiable();
    }

    private void VerifyFullConnectedCalled()
    {
        moqParallelController.Verify();
    }

    private void DisconnectMoqSetup()
    {
        moqInitiateControls.Setup(tcs => tcs.Disconnect()).Verifiable();
    }
}
