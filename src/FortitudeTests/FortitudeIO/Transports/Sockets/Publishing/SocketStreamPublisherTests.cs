using System;
using FortitudeCommon.DataStructures.LinkedLists;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.ORX.Authentication;
using FortitudeIO.Protocols.Serialization;
using FortitudeIO.Transports.Sockets.Dispatcher;
using FortitudeIO.Transports.Sockets.Publishing;
using FortitudeIO.Transports.Sockets.SessionConnection;
using FortitudeIO.Transports.Sockets.Subscription;
using FortitudeMarketsCore.Pricing.PQ.Quotes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FortitudeTests.FortitudeIO.Transports.Sockets.Publishing
{
    [TestClass]
    public class SocketStreamPublisherTests
    {
        private Action<ISocketSessionConnection, string, int> dummyOnCxError;
        private DummySocketStreamPublisher dummySocketStreamPublisher;
        private Mock<IBinarySerializationFactory> moqBinSerialFac;
        private Mock<IBinarySerializer> moqBinSerializer;
        private Mock<IBinaryStreamSubscriber> moqBinStreamSubscriber;
        private Mock<ISocketDispatcher> moqDispatcher;
        private Mock<IFLogger> moqFLogger;
        private Mock<ISocketSessionConnection> moqSocketSessionConnection;
        private Mock<ISocketSessionSender> moqSocketSessionSender;
        private readonly string sessionDescription = "testSession";
        private Mock<IOSSocket> moqSocket;

        [TestInitialize]
        public void Setup()
        {
            moqFLogger = new Mock<IFLogger>();
            moqDispatcher = new Mock<ISocketDispatcher>();
            moqBinSerialFac = new Mock<IBinarySerializationFactory>();
            moqBinStreamSubscriber = new Mock<IBinaryStreamSubscriber>();
            moqSocket = new Mock<IOSSocket>();

            dummyOnCxError = (connection, s, arg3) => { };
            dummySocketStreamPublisher = new DummySocketStreamPublisher(moqFLogger.Object, moqDispatcher.Object,
                sessionDescription, moqBinSerialFac.Object,
                dummyOnCxError, 1234567, moqBinStreamSubscriber.Object);

            moqSocketSessionConnection = new Mock<ISocketSessionConnection>();
            moqSocketSessionSender = new Mock<ISocketSessionSender>();
            moqSocketSessionConnection.SetupGet(ssc => ssc.SessionSender).Returns(moqSocketSessionSender.Object);
            moqBinSerializer = new Mock<IBinarySerializer>();
        }

        [TestMethod]
        public void ListeningSocketStreamPublisher_Unregister_InformsDispatcherToUnregister()
        {
            moqDispatcher.Setup(d => d.Listener.UnregisterForListen(moqSocketSessionConnection.Object)).Verifiable();
            dummySocketStreamPublisher.Unregister(moqSocketSessionConnection.Object);
            moqDispatcher.Verify();
        }

        [TestMethod]
        public void ListeningSocketStreamPublisher_StartMessaging_StartsDispatcher()
        {
            moqDispatcher.Setup(d => d.Start()).Verifiable();
            dummySocketStreamPublisher.StartMessaging();
            moqDispatcher.Verify();
        }

        [TestMethod]
        public void ListeningSocketStreamPublisher_StopMessaging_InformsDispatcherToUnregister()
        {
            moqDispatcher.Setup(d => d.Stop()).Verifiable();
            dummySocketStreamPublisher.StopMessaging();
            moqDispatcher.Verify();
        }

        [TestMethod]
        public void NewSocketStream_RegisterSerializer_FindsSerializerAddsItToLookup()
        {
            moqBinSerialFac.Setup(d => d.GetSerializer<IPQLevel0Quote>(0)).Returns(moqBinSerializer.Object).Verifiable();

            dummySocketStreamPublisher.RegisterSerializer<IPQLevel0Quote>(0);

            Assert.AreEqual(1, dummySocketStreamPublisher.RegisteredSerializersCount);
            moqBinSerialFac.Verify();
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void AlreadyRegisteredId_RegisterSerializer_ThrowsException()
        {
            moqBinSerialFac.Setup(d => d.GetSerializer<IPQLevel0Quote>(0)).Returns(moqBinSerializer.Object).Verifiable();

            dummySocketStreamPublisher.RegisterSerializer<IPQLevel0Quote>(0);

            Assert.AreEqual(1, dummySocketStreamPublisher.RegisteredSerializersCount);
            moqBinSerialFac.Verify();

            dummySocketStreamPublisher.RegisterSerializer<IPQLevel0Quote>(0);
        }

        [TestMethod]
        public void NewSocketStream_UnregisterSerializer_FindsSerializerAddsItToLookup()
        {
            moqBinSerialFac.Setup(d => d.GetSerializer<IPQLevel0Quote>(0)).Returns(moqBinSerializer.Object).Verifiable();
            dummySocketStreamPublisher.RegisterSerializer<IPQLevel0Quote>(0);
            Assert.AreEqual(1, dummySocketStreamPublisher.RegisteredSerializersCount);
            moqBinSerialFac.Verify();

            dummySocketStreamPublisher.UnregisterSerializer(0);

            Assert.AreEqual(0, dummySocketStreamPublisher.RegisteredSerializersCount);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void UnregisteredMessageId_UnregisterSerializer_ThrowsException()
        {
            dummySocketStreamPublisher.UnregisterSerializer(0);
        }

        [TestMethod]
        public void RegisteredSerializer_EnqueSingleCx_AlertsCxWithMessageSerializerAndDispatcher()
        {
            IVersionedMessage toBeSentToCx = new OrxLogonResponse();
            moqBinSerialFac.Setup(d => d.GetSerializer<IPQLevel0Quote>(toBeSentToCx.MessageId)).Returns(moqBinSerializer.Object);
            dummySocketStreamPublisher.RegisterSerializer<IPQLevel0Quote>(toBeSentToCx.MessageId);
            var socketSessCx = new SocketSessionConnection(null, moqSocketSessionSender.Object, null);
            moqSocketSessionSender.Setup(ssc => ssc.Enqueue(toBeSentToCx, moqBinSerializer.Object)).Verifiable();
            moqDispatcher.Setup(d => d.Sender.AddToSendQueue(socketSessCx)).Verifiable();


            dummySocketStreamPublisher.Enqueue(socketSessCx, toBeSentToCx);

            moqSocketSessionSender.Verify();
            moqDispatcher.Verify();
        }

        [TestMethod, ExpectedException(typeof(Exception))]
        public void UnregisteredSerializer_EnqueSingleCx_ThrowsException()
        {
            IVersionedMessage toBeSentToCx = new OrxLogonResponse();

            dummySocketStreamPublisher.Enqueue(moqSocketSessionConnection.Object, toBeSentToCx);
        }

        [TestMethod]
        public void RegisteredSerializer_EnqueMultipleCx_AlertsEachCxWithMessageSerializerAndDispatcher()
        {
            IVersionedMessage toBeSentToCx = new OrxLogonResponse();
            moqBinSerialFac.Setup(d => d.GetSerializer<IPQLevel0Quote>(toBeSentToCx.MessageId)).Returns(moqBinSerializer.Object);
            dummySocketStreamPublisher.RegisterSerializer<IPQLevel0Quote>(toBeSentToCx.MessageId);
            var firstSocketCx = new SocketSessionConnection(null, moqSocketSessionSender.Object, null);

            var moqSecondCxSender = new Mock<ISocketSessionSender>();
            var secondCx = new SocketSessionConnection(null, moqSecondCxSender.Object, null);
            moqSecondCxSender.Setup(ssc => ssc.Enqueue(toBeSentToCx, moqBinSerializer.Object)).Verifiable();
            moqSocketSessionSender.Setup(ssc => ssc.Enqueue(toBeSentToCx, moqBinSerializer.Object)).Verifiable();
            moqDispatcher.Setup(d => d.Sender.AddToSendQueue(firstSocketCx)).Verifiable();
            moqDispatcher.Setup(d => d.Sender.AddToSendQueue(secondCx)).Verifiable();

            var linkedListOfCx = new DoublyLinkedList<ISocketSessionConnection>();
            linkedListOfCx.AddLast(secondCx);
            linkedListOfCx.AddFirst(firstSocketCx);

            dummySocketStreamPublisher.Enqueue(linkedListOfCx, toBeSentToCx);

            moqSocketSessionConnection.Verify();
            moqSecondCxSender.Verify();
            moqSocketSessionSender.Verify();
            moqDispatcher.Verify();
        }

        internal class DummySocketStreamPublisher : SocketStreamPublisher
        {
            private readonly Action<ISocketSessionConnection, string, int> dummyOnCxError;
            private readonly IBinarySerializationFactory dummySerializationFactory;
            
            public DummySocketStreamPublisher(IFLogger logger, ISocketDispatcher dispatcher, string sessionDescription,
                IBinarySerializationFactory dummySerializationFactory,
                Action<ISocketSessionConnection, string, int> dummyOnCxError, int sendBufferSize,
                IBinaryStreamSubscriber streamFromSubscriber)
                : base(logger, dispatcher, sessionDescription)
            {
                this.dummySerializationFactory = dummySerializationFactory;
                this.dummyOnCxError = dummyOnCxError;
                SendBufferSize = sendBufferSize;
                StreamFromSubscriber = streamFromSubscriber;
            }

            public override int SendBufferSize { get; }

            public override IBinaryStreamSubscriber StreamFromSubscriber { get; }

            public override IBinarySerializationFactory GetFactory()
            {
                return dummySerializationFactory;
            }
        }
    }
}