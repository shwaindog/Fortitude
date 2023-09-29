using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.AsyncWrappers;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeCommon.Types;
using FortitudeIO.Protocols.Serialization;
using FortitudeIO.Sockets;
using FortitudeIO.Transports.Sockets;
using FortitudeIO.Transports.Sockets.Dispatcher;
using FortitudeIO.Transports.Sockets.Publishing;
using FortitudeIO.Transports.Sockets.Subscription;
using FortitudeMarketsCore.Pricing.PQ;
using FortitudeMarketsCore.Pricing.PQ.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Subscription;
using FortitudeTests.FortitudeIO.Transports.Sockets.Subscription;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription
{
    [TestClass]
    public class PQSocketSubscriptionRegistrationFactoryBaseTests
    {
        private DummySktSubRegFctryBse<SocketSubscriberTests.DummySocketSubscriber>
            dummySktSubRegFctryBs;
        private SocketSubscriberTests.DummySocketSubscriber dummySocketSubscriber;
        private string testSessionDescription;
        private int wholeMessagesPerReceive;
        private int recvBufferSize;
        private Mock<IConnectionConfig> moqServerConnectionConfig;
        private Mock<IOSParallelControllerFactory> moqParallelControllerFactory;
        private Mock<IOSParallelController> moqParallelControler;
        private Mock<IOSNetworkingController> moqNetworkingController;
        private Mock<ISocketDispatcher> moqDispatcher;
        private Mock<IFLogger> moqFlogger;
        private Mock<IBinaryStreamPublisher> moqBinaryStreamPublisher;
        private Mock<IStreamDecoder> moqFeedDecoder;
        private Mock<IBinaryDeserializationFactory> moqBinaryDeserializationFactory;
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
            testSessionDescription = "TestSessionDescription";
            wholeMessagesPerReceive = 23;
            recvBufferSize = 1234567;
            moqBinaryStreamPublisher = new Mock<IBinaryStreamPublisher>();
            moqPQQuoteSerializationFactory = new Mock<IPQQuoteSerializerFactory>();
            moqFeedDecoder = new Mock<IStreamDecoder>();
            moqBinaryDeserializationFactory = new Mock<IBinaryDeserializationFactory>();
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

            moqBinaryDeserializationFactory.Setup(bdf => bdf
            .GetDeserializer<IPQLevel0Quote>(uint.MaxValue))
            .Returns(moqSocketBinaryDeserializer.Object)
            .Verifiable();

            dummySocketSubscriber = new SocketSubscriberTests.DummySocketSubscriber(moqFlogger.Object, 
                moqDispatcher.Object, moqNetworkingController.Object, moqServerConnectionConfig.Object, 
                testSessionDescription, wholeMessagesPerReceive, null, recvBufferSize, 
                moqBinaryStreamPublisher.Object, moqFeedDecoder.Object, moqBinaryDeserializationFactory.Object, 
                moqOsSocket.Object);

            dummySktSubRegFctryBs = new DummySktSubRegFctryBse<SocketSubscriberTests.DummySocketSubscriber>(
                moqNetworkingController.Object, dummySocketSubscriber);
        }

        [TestCleanup]
        public void TearDown()
        {
            OSParallelControllerFactory.Instance = new OSParallelControllerFactory();
        }

        [TestMethod]
        public void EmptySocketSubRegFactory_RegisterSocketSubscriber_FindSocketSubscriptionReturnsSameInstance()
        {
            var socketClient = dummySktSubRegFctryBs.RegisterSocketSubscriber("TestSocketDescription",
                moqServerConnectionConfig.Object, uint.MaxValue, moqDispatcher.Object, 50,
                moqPQQuoteSerializationFactory.Object, "multicastInterfaceIP");

            Assert.IsNotNull(socketClient);
            Assert.AreSame(dummySocketSubscriber, socketClient);

            var foundSubscription = dummySktSubRegFctryBs.FindSocketSubscription(moqServerConnectionConfig.Object);
            Assert.AreSame(socketClient, foundSubscription);
        }

        [TestMethod]
        public void EmptySocketSubRegFactory_FindSocketSubscription_ReturnsNull()
        {
            var foundSubscription = dummySktSubRegFctryBs.FindSocketSubscription(moqServerConnectionConfig.Object);
            Assert.IsNull(foundSubscription);
        }

        [TestMethod]
        public void RegisteredSocketSubscriber_UnregisterSocketSubscriber_FindSocketSubscriptionReturnsSameInstance()
        {
            var socketClient = dummySktSubRegFctryBs.RegisterSocketSubscriber("TestSocketDescription",
                moqServerConnectionConfig.Object, uint.MaxValue, moqDispatcher.Object, 50,
                moqPQQuoteSerializationFactory.Object, "multicastInterfaceIP");

            var socketSubscriptions = NonPublicInvocator.GetInstanceField<IDictionary<IConnectionConfig,
                SocketSubscriberTests.DummySocketSubscriber>>(dummySktSubRegFctryBs, "socketSubscriptions");

            Assert.IsNotNull(socketClient);
            Assert.AreEqual(1, socketSubscriptions.Count);
            var kvp = socketSubscriptions.First();
            Assert.AreSame(dummySocketSubscriber, socketClient);
            Assert.AreSame(moqServerConnectionConfig.Object, kvp.Key);
            Assert.AreSame(socketClient, kvp.Value);

            moqSocketBinaryDeserializer.Setup(sbd =>
                sbd.IsRegistered(It.IsAny<Action<IPQLevel0Quote, object, ISession>>())).Returns(true).Verifiable();

            dummySktSubRegFctryBs.UnregisterSocketSubscriber(moqServerConnectionConfig.Object, uint.MaxValue);
            Assert.AreEqual(0, socketSubscriptions.Count);
            moqSocketBinaryDeserializer.Verify();
        }

        [TestMethod]
        public void MultiRegisteredFactory_UnregisterSocketSubscriber_DoesntRemoveSubscriptionUntilAllUnsubscribed()
        {
            moqBinaryDeserializationFactory.Setup(bdf => bdf
                    .GetDeserializer<IPQLevel0Quote>(1u))
                .Returns(moqSocketBinaryDeserializer.Object)
                .Verifiable();

            var socketClient = dummySktSubRegFctryBs.RegisterSocketSubscriber("TestSocketDescription",
                moqServerConnectionConfig.Object, uint.MaxValue, moqDispatcher.Object, 50,
                moqPQQuoteSerializationFactory.Object, "multicastInterfaceIP");
            var socketClient2 = dummySktSubRegFctryBs.RegisterSocketSubscriber("TestSocketDescription",
                moqServerConnectionConfig.Object, 1, moqDispatcher.Object, 50,
                moqPQQuoteSerializationFactory.Object, "multicastInterfaceIP");

            var socketSubscriptions = NonPublicInvocator.GetInstanceField<IDictionary<IConnectionConfig,
                SocketSubscriberTests.DummySocketSubscriber>>(dummySktSubRegFctryBs, "socketSubscriptions");

            Assert.IsNotNull(socketClient);
            Assert.IsNotNull(socketClient2);
            Assert.AreEqual(1, socketSubscriptions.Count);
            var kvp = socketSubscriptions.First();
            Assert.AreSame(dummySocketSubscriber, socketClient);
            Assert.AreSame(moqServerConnectionConfig.Object, kvp.Key);
            Assert.AreSame(socketClient, kvp.Value);

            moqSocketBinaryDeserializer.Setup(sbd =>
                sbd.IsRegistered(It.IsAny<Action<IPQLevel0Quote, object, ISession>>())).Returns(true).Verifiable();

            dummySktSubRegFctryBs.UnregisterSocketSubscriber(moqServerConnectionConfig.Object, uint.MaxValue);

            Assert.IsNotNull(socketClient);
            Assert.IsNotNull(socketClient2);
            Assert.AreEqual(1, socketSubscriptions.Count);
            kvp = socketSubscriptions.First();
            Assert.AreSame(dummySocketSubscriber, socketClient);
            Assert.AreSame(moqServerConnectionConfig.Object, kvp.Key);
            Assert.AreSame(socketClient, kvp.Value);

            dummySktSubRegFctryBs.UnregisterSocketSubscriber(moqServerConnectionConfig.Object, 1u);

            Assert.AreEqual(0, socketSubscriptions.Count);
            moqSocketBinaryDeserializer.Verify();

        }

        internal class DummySktSubRegFctryBse<T> :
            PQSocketSubscriptionRegistrationFactoryBase<T> where T : SocketSubscriber
        {
            private readonly T returnedSocketSubscriber;

            public DummySktSubRegFctryBse(IOSNetworkingController networkingController,
                T returnedSocketSubscriber) 
                : base(networkingController)
            {
                this.returnedSocketSubscriber = returnedSocketSubscriber;
            }

            public T ReturnedSocketSubscriber => returnedSocketSubscriber;

            protected override T CreateNewSocketSubscriptionType(ISocketDispatcher dispatcher, 
                IOSNetworkingController networkingController,
                IConnectionConfig connectionConfig, string socketUseDescription, uint cxTimeoutS,
                int wholeMessagesPerReceive, IPQQuoteSerializerFactory pqQuoteSerializerFactory, 
                string multicastInterface)
            {
                return returnedSocketSubscriber;
            }
        }
    }
}