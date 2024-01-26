#region

using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.OSWrapper.AsyncWrappers;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeCommon.Types;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Transports.Sockets;
using FortitudeIO.Transports.Sockets.Dispatcher;
using FortitudeIO.Transports.Sockets.Subscription;
using FortitudeMarketsCore.Pricing.PQ.Publication;
using FortitudeMarketsCore.Pricing.PQ.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Serialization;
using Moq;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Publication;

[TestClass]
public class PQUpdatePublisherTests
{
    private Mock<ISocketDispatcher> moqDispatcher = null!;
    private Mock<IOSNetworkingController> moqNetworkingController = null!;
    private Mock<IOSParallelController> moqParallelControler = null!;
    private Mock<IOSParallelControllerFactory> moqParallelControllerFactory = null!;
    private Mock<IConnectionConfig> moqServerConnectionConfig = null!;
    private Mock<IOSSocket> moqSocket = null!;
    private string networkAddress = null!;

    private PQUpdatePublisher pqUpdatePublisher = null!;
    private string sessionDescription = null!;

    [TestInitialize]
    public void SetUp()
    {
        moqDispatcher = new Mock<ISocketDispatcher>();
        sessionDescription = "testSessionDescription";
        networkAddress = "testNetworkAddress";
        moqServerConnectionConfig = new Mock<IConnectionConfig>();
        moqSocket = new Mock<IOSSocket>();
        moqSocket.SetupAllProperties();
        moqNetworkingController = new Mock<IOSNetworkingController>();
        moqParallelControllerFactory = new Mock<IOSParallelControllerFactory>();
        moqParallelControler = new Mock<IOSParallelController>();
        moqParallelControllerFactory.SetupGet(pcf => pcf.GetOSParallelController)
            .Returns(moqParallelControler.Object);
        OSParallelControllerFactory.Instance = moqParallelControllerFactory.Object;

        pqUpdatePublisher = new PQUpdatePublisher(moqDispatcher.Object, moqNetworkingController.Object,
            moqServerConnectionConfig.Object, sessionDescription, networkAddress);
    }

    [TestCleanup]
    public void TearDown()
    {
        OSParallelControllerFactory.Instance = new OSParallelControllerFactory();
    }


    [TestMethod]
    public void NewPQSnapShotServer_RegisterSerializer_ForPQFullSnapshot()
    {
        var registeredSerializers = NonPublicInvocator
            .GetInstanceField<IMap<uint, IMessageSerializer>>(pqUpdatePublisher, "serializers");

        IMessageSerializer? pqSnapshotSerializer;
        Assert.IsTrue(registeredSerializers.TryGetValue(0, out pqSnapshotSerializer));
        Assert.IsInstanceOfType(pqSnapshotSerializer, typeof(PQQuoteSerializer));


        Assert.IsTrue(registeredSerializers.TryGetValue(1u, out pqSnapshotSerializer));
        Assert.IsInstanceOfType(pqSnapshotSerializer, typeof(PQHeartbeatSerializer));
    }

    [TestMethod]
    public void NewPQUpdateServer_GetFactory_ReturnsSerializationFactory()
    {
        var serializeFac = pqUpdatePublisher.GetFactory();

        Assert.IsNotNull(serializeFac);
        var pqMessageSerializer = serializeFac.GetSerializer<PQLevel0Quote>(0u);

        Assert.IsNotNull(pqMessageSerializer);
        var pqHeartBeatSerializer = serializeFac.GetSerializer<PQHeartBeatQuotesMessage>(1u);

        Assert.IsNotNull(pqHeartBeatSerializer);
    }

    [TestMethod]
    public void NewPQSnapshotServer_SendBufferSize_ReturnsExpectedSize()
    {
        Assert.AreEqual(2097152, pqUpdatePublisher.SendBufferSize);
    }

    [TestMethod]
    public void StreamFromSubscriber_OnRecvZeroBytes_DoesNotCloseSocket()
    {
        var streamFromSubscriber = (SocketSubscriber)pqUpdatePublisher.StreamFromSubscriber;

        Assert.IsFalse(streamFromSubscriber.ZeroBytesReadIsDisconnection);
    }
}
