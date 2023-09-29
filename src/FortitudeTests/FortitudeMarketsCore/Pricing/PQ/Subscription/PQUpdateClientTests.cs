using System.Reactive.Subjects;
using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.AsyncWrappers;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeCommon.Types;
using FortitudeIO.Protocols.Serialization;
using FortitudeIO.Sockets;
using FortitudeIO.Transports.Sockets;
using FortitudeIO.Transports.Sockets.Dispatcher;
using FortitudeMarketsCore.Pricing.PQ;
using FortitudeMarketsCore.Pricing.PQ.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Subscription;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription
{
    [TestClass]
    public class PQUpdateClientTests
    {
        private PQUpdateClient pqUpdateClient;
        private Mock<IMap<uint, IBinaryDeserializer>> moqSerializerCache;
        private Mock<IConnectionConfig> moqServerConnectionConfig;
        private Mock<IOSParallelControllerFactory> moqParallelControllerFactory;
        private Mock<IOSParallelController> moqParallelControler;
        private Mock<IOSNetworkingController> moqNetworkingController;
        private Mock<ISocketDispatcher> moqDispatcher;
        private Mock<IFLogger> moqFlogger;
        private Mock<IPQQuoteSerializerFactory> moqPQQuoteSerializationFactory;
        private Mock<IOSSocket> moqOsSocket;
        private ISubject<IConnectionUpdate> configUpdateSubject;
        private string testHostName;
        private int testHostPort;
        private Mock<ICallbackBinaryDeserializer<IPQLevel0Quote>> moqSocketBinaryDeserializer;

        [TestInitialize]
        public void SetUp()
        {
            moqFlogger = new Mock<IFLogger>();
            moqDispatcher = new Mock<ISocketDispatcher>();
            moqParallelControler = new Mock<IOSParallelController>();
            moqParallelControllerFactory = new Mock<IOSParallelControllerFactory>();
            moqParallelControllerFactory.SetupGet(pcf => pcf.GetOSParallelController)
                .Returns(moqParallelControler.Object);
            OSParallelControllerFactory.Instance = moqParallelControllerFactory.Object;
            moqNetworkingController = new Mock<IOSNetworkingController>();
            moqServerConnectionConfig = new Mock<IConnectionConfig>();
            moqPQQuoteSerializationFactory = new Mock<IPQQuoteSerializerFactory>();
            moqSocketBinaryDeserializer = new Mock<ICallbackBinaryDeserializer<IPQLevel0Quote>>();
            moqOsSocket = new Mock<IOSSocket>();
            configUpdateSubject = new Subject<IConnectionUpdate>();

            testHostName = "TestHostname";
            moqServerConnectionConfig.SetupGet(scc => scc.Hostname).Returns(testHostName);
            testHostPort = 1979;
            moqServerConnectionConfig.SetupGet(scc => scc.Port).Returns(testHostPort);
            moqServerConnectionConfig.SetupProperty(scc => scc.Updates, configUpdateSubject);
            moqFlogger.Setup(fl => fl.Info(It.IsAny<string>(), It.IsAny<object[]>()));
            moqSerializerCache = new Mock<IMap<uint, IBinaryDeserializer>>();
            moqOsSocket.SetupAllProperties();

            moqSocketBinaryDeserializer.SetupAllProperties();

            moqPQQuoteSerializationFactory.Setup(pqqsf => pqqsf.GetDeserializer<IPQLevel0Quote>(uint.MaxValue))
                .Returns(moqSocketBinaryDeserializer.Object).Verifiable();

            pqUpdateClient = new PQUpdateClient(moqDispatcher.Object, moqNetworkingController.Object,
                moqServerConnectionConfig.Object, "TestSocketDescription", "multicastInterfaceIP", 50,
                moqPQQuoteSerializationFactory.Object);
        }

        [TestCleanup]
        public void TearDown()
        {
            OSParallelControllerFactory.Instance = new OSParallelControllerFactory();
        }

        [TestMethod]
        public void MissingSerializationFactory_New_UsesDefaultSerializationFactory()
        {
            pqUpdateClient = new PQUpdateClient(moqDispatcher.Object, moqNetworkingController.Object,
                moqServerConnectionConfig.Object, "TestSocketDescription", "multicastInterfaceIP", 50,
                null);

            var binaryDeserializationFactory = NonPublicInvocator.RunInstanceMethod<IBinaryDeserializationFactory>(
                pqUpdateClient, "GetFactory");
            Assert.IsInstanceOfType(binaryDeserializationFactory, typeof(PQQuoteSerializerFactory));
        }

        [TestMethod]
        public void UpdateClient_GetDecoder_ReturnsPQClientDecoder()
        {
            var decoder = pqUpdateClient.GetDecoder(moqSerializerCache.Object);
            
            Assert.IsInstanceOfType(decoder, typeof(PQClientDecoder));
        }

        [TestMethod]
        public void UpdateClient_RecvBufferSize_ReturnsPQClientDecoder()
        {
            var bufferSize = pqUpdateClient.RecvBufferSize;
            
            Assert.AreEqual(2097152, bufferSize);
        }

        [TestMethod]
        public void UpdateClient_HasNoStreamToPublisher()
        {
            Assert.IsNull(pqUpdateClient.StreamToPublisher);
        }
    }
}