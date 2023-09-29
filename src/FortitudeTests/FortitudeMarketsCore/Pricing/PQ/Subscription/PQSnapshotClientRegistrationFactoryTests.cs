using System.Reactive.Subjects;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.AsyncWrappers;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
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
    public class PQSnapshotClientRegistrationFactoryTests
    {
        private PQSnapshotClientRegistrationFactory pqSnapshotClientRegFactory;
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
            moqOsSocket.SetupAllProperties();

            moqSocketBinaryDeserializer.SetupAllProperties();
            
            moqPQQuoteSerializationFactory.Setup(pqqsf => pqqsf.GetDeserializer<IPQLevel0Quote>(uint.MaxValue))
                .Returns(moqSocketBinaryDeserializer.Object).Verifiable();

            pqSnapshotClientRegFactory = new PQSnapshotClientRegistrationFactory(moqNetworkingController.Object);
        }

        [TestMethod]
        public void EmptySocketSubRegFactory_RegisterSocketSubscriber_FindSocketSubscriptionReturnsSameInstance()
        {
            var socketClient = pqSnapshotClientRegFactory.RegisterSocketSubscriber("TestSocketDescription",
                moqServerConnectionConfig.Object, uint.MaxValue, moqDispatcher.Object, 50,
                moqPQQuoteSerializationFactory.Object, "multicastInterfaceIP");

            Assert.IsNotNull(socketClient);
            Assert.IsInstanceOfType(socketClient, typeof(PQSnapshotClient));

            var foundSubscription = pqSnapshotClientRegFactory.FindSocketSubscription(moqServerConnectionConfig.Object);
            Assert.AreSame(socketClient, foundSubscription);
        }
    }
}