#region

using System.Net;
using System.Net.Sockets;
using System.Reactive.Subjects;
using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.AsyncWrappers;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeCommon.Serdes.Binary;
using FortitudeCommon.Types;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Protocols.Serdes.Binary.Sockets;
using FortitudeIO.Transports.Sockets;
using FortitudeIO.Transports.Sockets.Dispatcher;
using FortitudeIO.Transports.Sockets.Publishing;
using FortitudeIO.Transports.Sockets.SessionConnection;
using FortitudeMarketsApi.Pricing.Quotes.SourceTickerInfo;
using FortitudeMarketsCore.Pricing.PQ;
using FortitudeMarketsCore.Pricing.PQ.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Serialization;
using FortitudeMarketsCore.Pricing.PQ.Subscription;
using FortitudeMarketsCore.Pricing.Quotes.SourceTickerInfo;
using FortitudeTests.FortitudeCommon.Chronometry;
using Moq;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription;

[TestClass]
public class PQSnapshotClientTests
{
    private ISubject<IConnectionUpdate> configUpdateSubject = null!;
    private string expectedHost = null!;
    private byte[] expectedIpAddress = null!;
    private int expectedPort;
    private Mock<IMessageDeserializer> moqDecoderDeserializer = null!;
    private Mock<ISocketDispatcher> moqDispatcher = null!;
    private Mock<IFLogger> moqFlogger = null!;
    private Mock<IFLoggerFactory> moqFloggerFactory = null!;
    private Mock<IIntraOSThreadSignal> moqIntraOsThreadSignal = null!;
    private Mock<IOSNetworkingController> moqNetworkingController = null!;
    private Mock<IOSSocket> moqOsSocket = null!;
    private Mock<IOSParallelController> moqParallelControler = null!;
    private Mock<IOSParallelControllerFactory> moqParallelControllerFactory = null!;
    private Mock<IPQQuoteSerializerRepository> moqPQQuoteSerializationRepo = null!;
    private Mock<IMap<uint, IMessageDeserializer>> moqSerializerCache = null!;
    private Mock<IConnectionConfig> moqServerConnectionConfig = null!;
    private Mock<ICallbackMessageDeserializer<PQLevel0Quote>> moqSocketBinaryDeserializer = null!;
    private Mock<IBinaryStreamPublisher> moqStreamToPublisher = null!;
    private Mock<ITimerCallbackSubscription> moqTimerCallbackSubscription = null!;
    private PQSnapshotClient pqSnapshotClient = null!;

    private IList<IUniqueSourceTickerIdentifier> sendSrcTkrIds = null!;
    private string sessionDescription = null!;
    private TimeContextTests.StubTimeContext stubContext = null!;

    [TestInitialize]
    public void SetUp()
    {
        moqDispatcher = new Mock<ISocketDispatcher>();
        moqParallelControler = new Mock<IOSParallelController>();
        moqIntraOsThreadSignal = new Mock<IIntraOSThreadSignal>();
        moqParallelControllerFactory = new Mock<IOSParallelControllerFactory>();
        moqParallelControllerFactory.SetupGet(pcf => pcf.GetOSParallelController)
            .Returns(moqParallelControler.Object);
        OSParallelControllerFactory.Instance = moqParallelControllerFactory.Object;
        moqTimerCallbackSubscription = new Mock<ITimerCallbackSubscription>();
        moqNetworkingController = new Mock<IOSNetworkingController>();
        moqServerConnectionConfig = new Mock<IConnectionConfig>();
        sessionDescription = "TestSocketDescription PQSnapshotClient";
        moqPQQuoteSerializationRepo = new Mock<IPQQuoteSerializerRepository>();
        moqSocketBinaryDeserializer = new Mock<ICallbackMessageDeserializer<PQLevel0Quote>>();
        moqOsSocket = new Mock<IOSSocket>();
        configUpdateSubject = new Subject<IConnectionUpdate>();
        moqStreamToPublisher = new Mock<IBinaryStreamPublisher>();
        stubContext = new TimeContextTests.StubTimeContext();
        TimeContext.Provider = stubContext;
        stubContext.UtcNow = new DateTime(2018, 01, 29, 19, 54, 12);
        moqFlogger = new Mock<IFLogger>();
        moqFloggerFactory = new Mock<IFLoggerFactory>();
        moqFloggerFactory.Setup(flf => flf.GetLogger(It.IsAny<Type>())).Returns(moqFlogger.Object);
        moqFloggerFactory.Setup(flf => flf.GetLogger(It.IsAny<string>())).Returns(new Mock<IFLogger>().Object);
        FLoggerFactory.Instance = moqFloggerFactory.Object;

        moqParallelControler.Setup(pc => pc.SingleOSThreadActivateSignal(false))
            .Returns(moqIntraOsThreadSignal.Object).Verifiable();
        expectedHost = "TestHostname";
        expectedPort = 1979;
        expectedIpAddress = new byte[] { 61, 26, 5, 6 };
        moqServerConnectionConfig.SetupGet(scc => scc.Hostname).Returns(expectedHost);
        moqServerConnectionConfig.SetupGet(scc => scc.Port).Returns(expectedPort);
        moqServerConnectionConfig.SetupProperty(scc => scc.Updates, configUpdateSubject);
        moqFlogger.Setup(fl => fl.Info(It.IsAny<string>(), It.IsAny<object[]>()));
        moqSerializerCache = new Mock<IMap<uint, IMessageDeserializer>>();
        moqDecoderDeserializer = new Mock<IMessageDeserializer>();
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

        pqSnapshotClient = new PQSnapshotClient(moqDispatcher.Object, moqNetworkingController.Object,
            moqServerConnectionConfig.Object, "TestSocketDescription", 5, 50,
            moqPQQuoteSerializationRepo.Object);

        moqFlogger.Setup(fl => fl.Info("Attempting TCP connection to {0} on {1}:{2}",
            sessionDescription, expectedHost, expectedPort)).Verifiable();
        moqNetworkingController.Setup(nc => nc.CreateOSSocket(AddressFamily.InterNetwork, SocketType.Stream,
            ProtocolType.Tcp)).Returns(moqOsSocket.Object).Verifiable();
        moqNetworkingController.Setup(nc => nc.GetIpAddress(expectedHost)).Returns(
            new IPAddress(expectedIpAddress)).Verifiable();
        moqOsSocket.SetupSet(oss => oss.NoDelay = true).Verifiable();
        moqOsSocket.Setup(oss => oss.Connect(It.IsAny<IPEndPoint>())).Verifiable();
    }

    [TestCleanup]
    public void TearDown()
    {
        OSParallelControllerFactory.Instance = new OSParallelControllerFactory();
        FLoggerFactory.Instance = new FLoggerFactory();
        TimeContext.Provider = new HighPrecisionTimeContext();
    }

    [TestMethod]
    public void MissingSerializationFactory_New_UsesDefaultSerializationFactory()
    {
        pqSnapshotClient = new PQSnapshotClient(moqDispatcher.Object, moqNetworkingController.Object,
            moqServerConnectionConfig.Object, "TestSocketDescription", 5, 50,
            new PQQuoteSerializerRepository());

        var binaryDeserializationFactory = NonPublicInvocator.RunInstanceMethod<IMessageIdDeserializationRepository>(
            pqSnapshotClient, "GetFactory");
        Assert.IsInstanceOfType(binaryDeserializationFactory, typeof(PQQuoteSerializerRepository));
    }

    [TestMethod]
    public void PQSnapshotClient_RequestSnapshots_ConnectsStartsConnectionTimeoutSendRequestIds()
    {
        moqFlogger.Setup(fl => fl.Info("Sending snapshot request for streams {0}", "7,77,15,19,798"))
            .Verifiable();

        MockStreamToPublisher();
        moqStreamToPublisher.Setup(stp => stp.Enqueue(It.IsAny<ISocketSessionConnection>(),
            new PQSnapshotIdsRequest(new uint[] { 7, 77, 15, 19, 798 }))).Verifiable();


        ConnectMoqSetup();

        pqSnapshotClient.RequestSnapshots(sendSrcTkrIds);

        VerifyFullConnectedCalled();
        moqFlogger.Verify();
        moqStreamToPublisher.Verify();
    }

    [TestMethod]
    public void PQSnapshotClientNotYetConnected_RequestSnapshots_SchedulesConnectQueuesIdsForSend()
    {
        MockStreamToPublisher();

        ConnectMoqSetup();
        moqFlogger.Reset();
        moqFlogger.Setup(fl => fl.Info("Queuing snapshot request for ticker ids {0}", "7,77,15,19,798"))
            .Verifiable();
        moqParallelControler.Setup(pc => pc.CallFromThreadPool(It.IsAny<WaitCallback>())).Verifiable();

        pqSnapshotClient.RequestSnapshots(sendSrcTkrIds);

        moqFlogger.Verify();
    }

    [TestMethod]
    public void AlreadyQueuedIds_RequestSnapshots_LogsIdsAlreadyQueuedForSendOnConnect()
    {
        ConnectMoqSetup();
        moqParallelControler.Setup(pc => pc.CallFromThreadPool(It.IsAny<WaitCallback>())).Verifiable();

        pqSnapshotClient.RequestSnapshots(sendSrcTkrIds);
        moqFlogger.Reset();
        moqFlogger.Setup(fl => fl.Info("Snapshot request already queued for ticker ids {0}, last snapshot sent at {1}",
            "7,77,15,19,798", new DateTime().ToString("O"))).Verifiable();
        pqSnapshotClient.RequestSnapshots(sendSrcTkrIds);

        moqFlogger.Verify();
    }

    [TestMethod]
    public void QueuedTickerIdsForRequest_OnConnect_SendsTickerIdsWhenConnected()
    {
        MockStreamToPublisher();

        MockStreamToPublisher();
        moqStreamToPublisher.Setup(stp => stp.Enqueue(It.IsAny<ISocketSessionConnection>(),
            new PQSnapshotIdsRequest(new uint[] { 7, 77, 15, 19, 798 }))).Verifiable();

        ConnectMoqSetup();

        moqParallelControler.Setup(pc => pc.CallFromThreadPool(It.IsAny<WaitCallback>())).Verifiable();
        pqSnapshotClient.RequestSnapshots(sendSrcTkrIds);
        moqParallelControler.Setup(pc => pc.CallFromThreadPool(It.IsAny<WaitCallback>()))
            .Callback<WaitCallback>(wc => wc.Invoke(new object())).Verifiable();

        moqFlogger.Reset();
        moqFlogger.Setup(fl => fl.Info("Sending queued snapshot requests for streams: {0}", "7,77,15,19,798"))
            .Verifiable();

        NonPublicInvocator.SetInstanceField(pqSnapshotClient, "connecting", false);
        pqSnapshotClient.Connect();

        moqFlogger.Verify();
        moqStreamToPublisher.Verify();
    }

    [TestMethod]
    public void ConnectedPQSnapshotClient_GetDecoderOnResponse_ResetsDisconnectionTimer()
    {
        ConnectMoqSetup();
        pqSnapshotClient.Connect();
        moqDecoderDeserializer.Setup(dd => dd.Deserialize(It.IsAny<ReadSocketBufferContext>())).Verifiable();
        var result = moqDecoderDeserializer.Object;
        moqSerializerCache.Setup(m => m.TryGetValue(19579, out result)).Returns(true).Verifiable();

        NonPublicInvocator.SetInstanceField(pqSnapshotClient, "deserializers", moqSerializerCache.Object);

        var decoder = pqSnapshotClient.GetDecoder(moqSerializerCache.Object);
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
        moqParallelControler.Verify();
        moqTimerCallbackSubscription.Verify();
        moqDecoderDeserializer.Verify();
        moqSerializerCache.Verify();
    }

    [TestMethod]
    public void UpdateClient_RecvBufferSize_ReturnsPQClientDecoder()
    {
        var bufferSize = pqSnapshotClient.RecvBufferSize;

        Assert.AreEqual(131072, bufferSize);
    }

    [TestMethod]
    public void UpdateClient_HasNoStreamToPublisher()
    {
        Assert.IsInstanceOfType(pqSnapshotClient.StreamToPublisher,
            typeof(PQSnapshotClient.PQSnapshotStreamPublisher));
    }

    [TestMethod]
    public void ConnectingPQSnapshotClient_TimeoutConnection_CallsDisconnect()
    {
        ConnectMoqSetup();
        WaitOrTimerCallback? callback = null;
        moqParallelControler.Setup(pc => pc.ScheduleWithEarlyTrigger(It.IsAny<IIntraOSThreadSignal>(),
                It.IsAny<WaitOrTimerCallback>(), 5000u, false))
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

    [TestMethod]
    public void DefaultFactoryPQSnapshotClientStreamToPublisher_New_RegistersPublisherForMessageId0()
    {
        pqSnapshotClient = new PQSnapshotClient(moqDispatcher.Object, moqNetworkingController.Object,
            moqServerConnectionConfig.Object, "TestSocketDescription", 5, 50,
            new PQQuoteSerializerRepository());

        var streamToPublisher = pqSnapshotClient.StreamToPublisher;
        var registeredSerializers = NonPublicInvocator.GetInstanceField<IMap<uint, IMessageSerializer>>(
            streamToPublisher, "serializers");

        Assert.AreEqual(1, registeredSerializers.Count);
        Assert.IsInstanceOfType(registeredSerializers[0], typeof(PQSnapshotIdsRequestSerializer));
    }

    [TestMethod]
    public void PQSnapshotClientStreamToPublisher_SendBufferSize_StreamFromSubscriber_AreExpected()
    {
        var streamToPublisher = (ISocketLinkListener)pqSnapshotClient.StreamToPublisher;
        Assert.AreEqual(131_072, streamToPublisher.SendBufferSize);
        Assert.AreSame(pqSnapshotClient, streamToPublisher.StreamFromSubscriber);
    }

    private void ConnectMoqSetup()
    {
        moqParallelControler.Setup(pc => pc.CallFromThreadPool(It.IsAny<WaitCallback>()))
            .Callback<WaitCallback>(wc => wc.Invoke(new object())).Verifiable();
        moqFlogger.Setup(fl => fl.Info("Connection to id:{0} {1}:{2} accepted", sessionDescription,
            It.IsAny<long>(), expectedHost, expectedPort)).Verifiable();
        moqDispatcher.Setup(d => d.Start()).Verifiable();
        moqDispatcher.Setup(d => d.Listener.RegisterForListen(It.IsAny<ISocketSessionConnection>())).Callback(() =>
        {
            moqOsSocket.SetupGet(oss => oss.Connected).Returns(true).Verifiable();
        }).Verifiable();
        moqOsSocket.SetupGet(oss => oss.Connected).Returns(true).Verifiable();

        moqParallelControler.Setup(pc => pc.ScheduleWithEarlyTrigger(It.IsAny<IIntraOSThreadSignal>(),
            It.IsAny<WaitOrTimerCallback>(), 5000u, false)).Returns(moqTimerCallbackSubscription.Object).Verifiable();
    }

    private void MockStreamToPublisher()
    {
        NonPublicInvocator.SetInstanceField(pqSnapshotClient, "streamToPublisher", moqStreamToPublisher.Object);
    }

    private void VerifyFullConnectedCalled()
    {
        moqParallelControler.Verify();
    }

    private void DisconnectMoqSetup()
    {
        moqFlogger.Setup(fl => fl.Info("Connection to {0} {1} id {2}:{3} closed", sessionDescription,
            It.IsAny<long>(), expectedHost, expectedPort)).Verifiable();
        moqDispatcher.Setup(d => d.Stop()).Verifiable();
        moqDispatcher.Setup(d => d.Listener.UnregisterForListen(It.IsAny<ISocketSessionConnection>())).Verifiable();

        moqTimerCallbackSubscription.Setup(tcs => tcs.Unregister(It.IsAny<IIntraOSThreadSignal>())).Verifiable();
    }
}
