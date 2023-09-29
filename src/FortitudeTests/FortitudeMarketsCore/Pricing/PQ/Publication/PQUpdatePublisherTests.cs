using System.Collections.Generic;
using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.OSWrapper.AsyncWrappers;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeCommon.Types;
using FortitudeIO.Protocols.Serialization;
using FortitudeIO.Sockets;
using FortitudeIO.Transports.Sockets;
using FortitudeIO.Transports.Sockets.Dispatcher;
using FortitudeIO.Transports.Sockets.Subscription;
using FortitudeMarketsCore.Pricing.PQ.Publication;
using FortitudeMarketsCore.Pricing.PQ.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Publication
{
    [TestClass]
    public class PQUpdatePublisherTests
    {

        private PQUpdatePublisher pqUpdatePublisher;
        private Mock<IOSParallelControllerFactory> moqParallelControllerFactory;
        private Mock<IOSParallelController> moqParallelControler;
        private Mock<ISocketDispatcher> moqDispatcher;
        private Mock<IOSNetworkingController> moqNetworkingController;
        private Mock<IConnectionConfig> moqServerConnectionConfig;
        private Mock<IOSSocket> moqSocket;
        private string sessionDescription;
        private string networkAddress;

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
                .GetInstanceField<IMap<uint, IBinarySerializer>>(pqUpdatePublisher, "serializers");

            IBinarySerializer pqSnapshotSerializer;
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
            var pqMessageSerializer = serializeFac.GetSerializer<IPQLevel0Quote>(0u);

            Assert.IsNotNull(pqMessageSerializer);
            var pqHeartBeatSerializer = serializeFac.GetSerializer<IEnumerable<IPQLevel0Quote>>(1u);

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
}