using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.Monitoring.Logging.Diagnostics.Performance;
using FortitudeCommon.OSWrapper.AsyncWrappers;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeCommon.Types;
using FortitudeIO.Protocols.Serialization;
using FortitudeIO.Sockets;
using FortitudeIO.Transports.Sockets;
using FortitudeIO.Transports.Sockets.Dispatcher;
using FortitudeIO.Transports.Sockets.SessionConnection;
using FortitudeIO.Transports.Sockets.Subscription;
using FortitudeMarketsCore.Pricing.PQ.Publication;
using FortitudeMarketsCore.Pricing.PQ.Quotes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Publication
{
    [TestClass]
    public class PQSnapshotServerTests
    {
        private PQSnapshotServer pqSnapshotServer;
        private Mock<IOSParallelControllerFactory> moqParallelControllerFactory;
        private Mock<IOSParallelController> moqParallelControler;
        private Mock<ISocketDispatcher> moqDispatcher;
        private Mock<IOSNetworkingController> moqNetworkingController;
        private Mock<IConnectionConfig> moqServerConnectionConfig;
        private Mock<IOSSocket> moqSocket;
        private Mock<ISocketSessionConnection> moqSocketSessionConnection;
        private Mock<IPerfLogger> moqLatencyTraceLogger;
        private string sessionDescription;

        [TestInitialize]
        public void SetUp()
        {
            moqDispatcher = new Mock<ISocketDispatcher>();
            sessionDescription = "testSessionDescription";
            moqServerConnectionConfig = new Mock<IConnectionConfig>();
            moqSocket = new Mock<IOSSocket>();
            moqSocket.SetupAllProperties();
            moqNetworkingController = new Mock<IOSNetworkingController>();
            moqParallelControllerFactory = new Mock<IOSParallelControllerFactory>();
            moqParallelControler = new Mock<IOSParallelController>();
            moqParallelControllerFactory.SetupGet(pcf => pcf.GetOSParallelController)
                .Returns(moqParallelControler.Object);
            OSParallelControllerFactory.Instance = moqParallelControllerFactory.Object;

            pqSnapshotServer = new PQSnapshotServer(moqDispatcher.Object, moqNetworkingController.Object, 
                moqServerConnectionConfig.Object, sessionDescription);

            moqSocketSessionConnection = new Mock<ISocketSessionConnection>();
            moqLatencyTraceLogger = new Mock<IPerfLogger>();
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
                .GetInstanceField<IMap<uint, IBinarySerializer>>(pqSnapshotServer, "serializers");

            IBinarySerializer pqSnapshotSerializer;
            Assert.IsTrue(registeredSerializers.TryGetValue(0, out pqSnapshotSerializer));
            Assert.IsInstanceOfType(pqSnapshotSerializer, typeof(IBinarySerializer<IPQLevel0Quote>));
        }

        
        [TestMethod]
        public void NewPQSnapshotServer_GetFactory_ReturnsSerializationFactory()
        {
            var serializeFac = pqSnapshotServer.GetFactory();

            Assert.IsNotNull(serializeFac);
            var pqMessageSerializer = serializeFac.GetSerializer<IPQLevel0Quote>(0u);

            Assert.IsNotNull(pqMessageSerializer);
        }
        
        [TestMethod]
        public void NewPQSnapshotServer_SendBufferSize_ReturnsExpectedSize()
        {
            Assert.AreEqual(131072, pqSnapshotServer.SendBufferSize);
        }
        
        [TestMethod]
        public void NewPQSnapshotServer_SnapshotClientStreamFromSubscriber_CanListenForSnapshotRequests()
        {
            var snapShotSubscriberStream = pqSnapshotServer.SnapshotClientStreamFromSubscriber;

            var decoder = snapShotSubscriberStream.GetDecoder(null);

            var byteBuff = new byte[]{1,0,0,6,0,1,0,1,0,0};
            var readWriteBuffer = new ReadWriteBuffer(byteBuff) {WrittenCursor = byteBuff.Length};

            bool onSnapshotRequestCalled = false;

            snapShotSubscriberStream.OnSnapshotRequest += (connection, uints) =>
            {
                Assert.AreSame(moqSocketSessionConnection.Object, connection);
                Assert.IsNotNull(uints);
                Assert.AreEqual(1, uints.Length);
                Assert.AreEqual(0x10000u, uints[0]);
                onSnapshotRequestCalled = true;
            };

            var encodedBuffer = new DispatchContext
            {
                DetectTimestamp = TimeContext.UtcNow,
                ReceivingTimestamp = TimeContext.UtcNow,
                EncodedBuffer = readWriteBuffer,
                Session = moqSocketSessionConnection.Object,
                DispatchLatencyLogger = moqLatencyTraceLogger.Object
            };

            decoder.Process(encodedBuffer);
            Assert.IsTrue(onSnapshotRequestCalled);
        }

        [TestMethod]
        public void NewPQSnapshotServer_SnapshotClientStreamFromSubscriber_RecvBufferIsExpectedSize()
        {
            var snapShotSubscriberStream = pqSnapshotServer.SnapshotClientStreamFromSubscriber;

            Assert.AreEqual(131072, snapShotSubscriberStream.RecvBufferSize);
        }

        [TestMethod]
        public void StreamFromSubscriber_StreamToPublisher_RefersBackToReferencingPublisher()
        {
            var streamFromSubscriber = pqSnapshotServer.SnapshotClientStreamFromSubscriber;

            Assert.AreSame(streamFromSubscriber.StreamToPublisher, pqSnapshotServer); 
        }

        [TestMethod]
        public void StreamFromSubscriber_OnRecvZeroBytes_DoesNotCloseSocket()
        {
            var streamFromSubscriber = (SocketSubscriber)pqSnapshotServer.SnapshotClientStreamFromSubscriber;

            Assert.IsFalse(streamFromSubscriber.ZeroBytesReadIsDisconnection);
        }
    }
}