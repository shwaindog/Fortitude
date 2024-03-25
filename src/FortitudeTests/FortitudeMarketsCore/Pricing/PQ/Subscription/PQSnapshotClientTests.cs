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
using FortitudeIO.Transports.NewSocketAPI.Config;
using FortitudeIO.Transports.NewSocketAPI.Controls;
using FortitudeIO.Transports.NewSocketAPI.Dispatcher;
using FortitudeIO.Transports.NewSocketAPI.Publishing;
using FortitudeIO.Transports.NewSocketAPI.Sockets;
using FortitudeMarketsApi.Pricing.Quotes.SourceTickerInfo;
using FortitudeMarketsCore.Pricing.PQ;
using FortitudeMarketsCore.Pricing.PQ.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Subscription;
using FortitudeMarketsCore.Pricing.Quotes.SourceTickerInfo;
using FortitudeTests.FortitudeCommon.Chronometry;
using Moq;
using ISocketDispatcher = FortitudeIO.Transports.Sockets.Dispatcher.ISocketDispatcher;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription;

[TestClass]
public class PQSnapshotClientTests
{
    private uint connectionTimeoutMs = 10_000;
    private string expectedHost = null!;
    private byte[] expectedIpAddress = null!;
    private ushort expectedPort;
    private Mock<ISocketDispatcher> moqDispatcher = null!;
    private Mock<ISocketDispatcherResolver> moqDispatcherResolver = null!;
    private Mock<IFLogger> moqFlogger = null!;
    private Mock<IFLoggerFactory> moqFloggerFactory = null!;
    private Mock<IIntraOSThreadSignal> moqIIntraOSThreadSignal = null!;
    private Mock<IInitiateControls> moqInitiateControls = null!;
    private Mock<IIntraOSThreadSignal> moqIntraOsThreadSignal = null!;
    private Mock<IOSNetworkingController> moqNetworkingController = null!;
    private Mock<IOSSocket> moqOsSocket = null!;
    private Mock<IOSParallelController> moqParallelController = null!;
    private Mock<IOSParallelControllerFactory> moqParallelControllerFactory = null!;
    private Mock<IPQQuoteSerializerRepository> moqPQQuoteSerializationRepo = null!;
    private Mock<ISerdesFactory> moqSerdesFactory = null!;
    private Mock<IMap<uint, IMessageDeserializer>> moqSerializerCache = null!;
    private Mock<ICallbackMessageDeserializer<PQLevel0Quote>> moqSocketBinaryDeserializer = null!;
    private Mock<ISocketConnection> moqSocketConnection = null!;
    private Mock<ISocketConnectionConfig> moqSocketConnectionConfig = null!;
    private Mock<ISocketFactories> moqSocketFactories = null!;
    private Mock<ISocketFactory> moqSocketFactory = null!;
    private Mock<ISocketSender> moqSocketSender = null!;
    private Mock<ISocketSessionContext> moqSocketSessionContext = null!;
    private Mock<ISocketTopicConnectionConfig> moqSocketTopicConnectionConfig = null!;
    private Mock<ITimerCallbackSubscription> moqTimerCallbackSubscription = null!;
    private PQSnapshotClient pqSnapshotClient = null!;

    private IList<IUniqueSourceTickerIdentifier> sendSrcTkrIds = null!;
    private string sessionDescription = null!;
    private TimeContextTests.StubTimeContext stubContext = null!;

    [TestInitialize]
    public void SetUp()
    {
        moqDispatcher = new Mock<ISocketDispatcher>();
        moqDispatcherResolver = new Mock<ISocketDispatcherResolver>();
        moqInitiateControls = new Mock<IInitiateControls>();
        moqSocketSessionContext = new Mock<ISocketSessionContext>();
        moqSocketFactories = new Mock<ISocketFactories>();
        moqSocketFactory = new Mock<ISocketFactory>();
        moqParallelController = new Mock<IOSParallelController>();
        moqIntraOsThreadSignal = new Mock<IIntraOSThreadSignal>();
        moqSocketSender = new Mock<ISocketSender>();
        moqParallelControllerFactory = new Mock<IOSParallelControllerFactory>();
        moqIIntraOSThreadSignal = new Mock<IIntraOSThreadSignal>();
        moqSocketConnection = new Mock<ISocketConnection>();
        moqSerdesFactory = new Mock<ISerdesFactory>();
        moqParallelControllerFactory.SetupGet(pcf => pcf.GetOSParallelController)
            .Returns(moqParallelController.Object);

        moqParallelController.Setup(pcf => pcf.SingleOSThreadActivateSignal(It.IsAny<bool>()))
            .Returns(moqIIntraOSThreadSignal.Object);
        OSParallelControllerFactory.Instance = moqParallelControllerFactory.Object;
        moqTimerCallbackSubscription = new Mock<ITimerCallbackSubscription>();
        moqNetworkingController = new Mock<IOSNetworkingController>();
        moqSocketConnectionConfig = new Mock<ISocketConnectionConfig>();
        moqSocketTopicConnectionConfig = new Mock<ISocketTopicConnectionConfig>();
        sessionDescription = "TestSocketDescription PQSnapshotClient";
        moqPQQuoteSerializationRepo = new Mock<IPQQuoteSerializerRepository>();
        moqSocketBinaryDeserializer = new Mock<ICallbackMessageDeserializer<PQLevel0Quote>>();
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
        moqSocketSessionContext.SetupGet(ssc => ssc.SocketFactories).Returns(moqSocketFactories.Object);
        moqSocketSessionContext.SetupGet(ssc => ssc.SocketTopicConnectionConfig)
            .Returns(moqSocketTopicConnectionConfig.Object);
        moqSocketSessionContext.SetupGet(ssc => ssc.SerdesFactory).Returns(moqSerdesFactory.Object);
        moqSocketSessionContext.SetupGet(ssc => ssc.SocketSender).Returns(moqSocketSender.Object);
        moqSocketSessionContext.SetupAdd(ssc => ssc.SocketConnected += null);
        expectedHost = "TestHostname";
        expectedPort = 1979;
        expectedIpAddress = new byte[] { 61, 26, 5, 6 };
        moqSocketConnectionConfig.SetupGet(scc => scc.InstanceName).Returns("PQSnapshotClientTests");
        moqSocketConnectionConfig.SetupGet(scc => scc.Hostname).Returns(expectedHost);
        moqSocketConnectionConfig.SetupGet(scc => scc.Port).Returns(expectedPort);
        moqSocketTopicConnectionConfig.SetupGet(stcc => stcc.Current).Returns(moqSocketConnectionConfig.Object);
        moqSocketTopicConnectionConfig.SetupGet(scc => scc.TopicDescription).Returns("PQSnapshotClientTests");
        moqSocketTopicConnectionConfig.SetupGet(scc => scc.ConnectionTimeoutMs).Returns(connectionTimeoutMs);
        moqSocketFactories.SetupGet(pcf => pcf.SocketFactory).Returns(moqSocketFactory.Object);
        moqSocketFactories.SetupGet(pcf => pcf.NetworkingController).Returns(moqNetworkingController.Object);
        moqSocketFactories.SetupGet(pcf => pcf.ConnectionChangedHandlerResolver).Returns(moqCallback);
        moqSocketFactories.SetupGet(pcf => pcf.SocketDispatcherResolver).Returns(moqDispatcherResolver.Object);
        moqSocketFactories.SetupGet(pcf => pcf.ParallelController).Returns(moqParallelController.Object);
        moqSerdesFactory.SetupProperty(sf => sf.StreamDecoderFactory);
        moqSerdesFactory.SetupProperty(sf => sf.StreamEncoderFactory);
        moqFlogger.Setup(fl => fl.Info(It.IsAny<string>(), It.IsAny<object[]>()));
        moqSerializerCache = new Mock<IMap<uint, IMessageDeserializer>>();
        moqOsSocket.SetupAllProperties();

        sendSrcTkrIds = new List<IUniqueSourceTickerIdentifier>
        {
            new UniqueSourceTickerIdentifier(07, "FirstSource", "FirstTicker")
            , new UniqueSourceTickerIdentifier(77, "FirstSource", "SecondTicker")
            , new UniqueSourceTickerIdentifier(15, "FirstSource", "ThirdTicker")
            , new UniqueSourceTickerIdentifier(19, "FirstSource", "FourthTicker")
            , new UniqueSourceTickerIdentifier(798, "FirstSource", "FifthTicker")
        };

        moqSocketBinaryDeserializer.SetupAllProperties();

        moqPQQuoteSerializationRepo.Setup(pqqsf => pqqsf.GetDeserializer<PQLevel0Quote>(uint.MaxValue))
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

        moqSocketSessionContext.Raise(sc => sc.SocketConnected += null, moqSocketConnection.Object);

        moqFlogger.Verify();
    }

    [TestMethod]
    public void ConnectedPQSnapshotClient_GetDecoderOnResponse_ResetsDisconnectionTimer()
    {
        ConnectMoqSetup();
        pqSnapshotClient.Connect();

        var decoder = pqSnapshotClient.MessageStreamDecoder;
        moqTimerCallbackSubscription.Setup(tcs => tcs.Unregister(moqIntraOsThreadSignal.Object)).Verifiable();

        decoder.Process(new ReadSocketBufferContext
        {
            EncodedBuffer = new ReadWriteBuffer(new byte[]
            {
                1, (byte)PQBinaryMessageFlags.IsHeartBeat, 0, 0, 0, 14, 0, 0, 0x4C, 0x7B, 0, 0, 0, 1
            })
            {
                WrittenCursor = 14
            }
        });
        Assert.IsInstanceOfType(decoder, typeof(PQClientMessageStreamDecoder));
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
            .Callback((IIntraOSThreadSignal iosts, WaitOrTimerCallback wotc, uint period, bool repeat) =>
            {
                callback = wotc;
            })
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
