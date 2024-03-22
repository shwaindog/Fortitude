#region

using FortitudeCommon.AsyncProcessing;
using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeCommon.Types;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.ORX.Serialization;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Transports;
using FortitudeIO.Transports.Sockets.Dispatcher;
using FortitudeIO.Transports.Sockets.Publishing;
using FortitudeIO.Transports.Sockets.SessionConnection;
using FortitudeIO.Transports.Sockets.Subscription;
using FortitudeMarketsCore.Pricing.PQ.Quotes;
using Moq;

#endregion

namespace FortitudeTests.FortitudeIO.Transports.Sockets.Subscription;

[TestClass]
public class SocketStreamSubscriberTests
{
    private readonly int wholeMessagesPerReceive = 20;
    private DummySocketStreamSubscriber dummySocketSubscriber = null!;
    private Mock<IBinaryStreamPublisher> moqBinStreamPublisher = null!;
    private Mock<IMessageIdDeserializationRepository> moqBinUnserialFac = null!;
    private Mock<ISocketDispatcher> moqDispatcher = null!;
    private Mock<IFLogger> moqFLogger = null!;
    private Mock<ISyncLock> moqSerializerLock = null!;
    private Mock<IMap<uint, IMessageDeserializer>> moqSerialzierCache = null!;
    private Mock<IOSSocket> moqSocket = null!;
    private int recvBufferSize;
    private string sessionDescription = null!;

    [TestInitialize]
    public void SetUp()
    {
        moqFLogger = new Mock<IFLogger>();
        moqDispatcher = new Mock<ISocketDispatcher>();
        sessionDescription = "testSessionDescription";
        moqBinStreamPublisher = new Mock<IBinaryStreamPublisher>();
        moqBinUnserialFac = new Mock<IMessageIdDeserializationRepository>();
        moqSocket = new Mock<IOSSocket>();
        moqSocket.SetupAllProperties();
        moqSerialzierCache = new Mock<IMap<uint, IMessageDeserializer>>();

        moqSerializerLock = new Mock<ISyncLock>();

        recvBufferSize = 1234567;

        dummySocketSubscriber = new DummySocketStreamSubscriber(moqFLogger.Object,
            moqDispatcher.Object, sessionDescription, wholeMessagesPerReceive, moqSerialzierCache.Object,
            moqBinUnserialFac.Object, recvBufferSize, moqBinStreamPublisher.Object);

        NonPublicInvocator.SetInstanceField(dummySocketSubscriber, "serializerLock", moqSerializerLock.Object);
    }

    [TestMethod]
    public void NewSocketStreamSubr_StartMessaging_StartsDispatcher()
    {
        moqDispatcher.Setup(d => d.Start()).Verifiable();

        dummySocketSubscriber.StartMessaging();

        moqDispatcher.Verify();
    }

    [TestMethod]
    public void StartedSocketStreamSubr_StopMessaging_StopsDispatcher()
    {
        moqDispatcher.Setup(d => d.Stop()).Verifiable();

        dummySocketSubscriber.StopMessaging();

        moqDispatcher.Verify();
    }

    [TestMethod]
    public void RegisteredSocketStreamSubr_Unregister_RemovesSocketSessionConnectionFromDispatcher()
    {
        var moqSocketSessionConnection = new Mock<ISocketSessionConnection>();
        moqDispatcher.Setup(d => d.Listener.UnregisterForListen(moqSocketSessionConnection.Object)).Verifiable();

        dummySocketSubscriber.Unregister(moqSocketSessionConnection.Object);

        moqDispatcher.Verify();
    }

    [TestMethod]
    public void NoDeserializersRegistered_RegisterDeserializer_AddsSerializerForMessageAndSyncProtectsCount()
    {
        var inSerializerLock = false;
        moqSerializerLock.Setup(sl => sl.Acquire()).Callback(() => { inSerializerLock = true; }).Verifiable();
        moqSerializerLock.Setup(sl => sl.Release()).Callback(() => { inSerializerLock = false; }).Verifiable();

        var moqSerializerCount = new Mock<IDictionary<uint, uint>>();
        moqSerializerCount.SetupGet(d => d[0])
            .Callback(() => { Assert.IsTrue(inSerializerLock); })
            .Returns(0)
            .Verifiable();
        moqSerializerCount.SetupSet(d => d[0] = 1).Callback(() => { Assert.IsTrue(inSerializerLock); })
            .Verifiable();
        NonPublicInvocator.SetInstanceField(dummySocketSubscriber, "deserializersCallbackCount",
            moqSerializerCount.Object);

        var moqBinPQ0Deserializer = new Mock<ICallbackMessageDeserializer<DummyMessage>>();
        moqBinUnserialFac.Setup(buf => buf.GetDeserializer<DummyMessage>(0))
            .Returns(moqBinPQ0Deserializer.Object).Verifiable();

        Action<DummyMessage, object?, ISession?> msgHandler = (msgId, msg, connSession) => { };
        dummySocketSubscriber.RegisterDeserializer(0u, msgHandler);

        Assert.IsFalse(inSerializerLock);
        moqSerializerLock.Verify();
        moqBinUnserialFac.Verify();
        moqSerializerCount.Verify();
    }

    [TestMethod]
    [ExpectedException(typeof(NullReferenceException))]
    public void NoDeserializersRegistered_RegisterDeserializerWithNoCallBackMethod_ThrowsException()
    {
        dummySocketSubscriber.RegisterDeserializer(0u, (DummyMessage _, object? _, ISession? _) => { });
    }

    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public void AlreadyRegisteredMsg_RegisterDeserializerWithDiffTypeCallback_ThrowsException()
    {
        var moqBinPQ0Deserializer = new Mock<ICallbackMessageDeserializer<DummyMessage>>();
        moqBinUnserialFac.Setup(buf => buf.GetDeserializer<DummyMessage>(0))
            .Returns(moqBinPQ0Deserializer.Object).Verifiable();
        var dummySafeMap = new LinkedListCache<uint, IMessageDeserializer>();

        NonPublicInvocator.SetInstanceField(dummySocketSubscriber, "deserializers", dummySafeMap);

        Action<DummyMessage, object?, ISession?> msgHandler = (msgId, msg, connSession) => { };
        dummySocketSubscriber.RegisterDeserializer(0u, msgHandler);
        Action<PQLevel0Quote, object?, ISession?> diffTypeMsgHandler = (msgId, msg, connSession) => { };
        dummySocketSubscriber.RegisterDeserializer(0u, diffTypeMsgHandler);
    }

    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public void AlreadyRegisteredMsg_RegisterDeserializerTwice_ThrowsException()
    {
        Action<DummyMessage, object?, ISession?> msgHandler = (msgId, msg, connSession) => { };
        var moqBinPQ0Deserializer = new Mock<ICallbackMessageDeserializer<DummyMessage>>();
        moqBinUnserialFac.Setup(buf => buf.GetDeserializer<DummyMessage>(0))
            .Returns(moqBinPQ0Deserializer.Object).Verifiable();
        moqBinPQ0Deserializer.Setup(bu => bu.IsRegistered(msgHandler))
            .Returns(true).Verifiable();
        var dummySafeMap = new LinkedListCache<uint, IMessageDeserializer>();

        NonPublicInvocator.SetInstanceField(dummySocketSubscriber, "deserializers", dummySafeMap);

        dummySocketSubscriber.RegisterDeserializer(0u, msgHandler);
        dummySocketSubscriber.RegisterDeserializer(0u, msgHandler);
    }

    [TestMethod]
    public void AlreadyRegisteredMsg_UnregisterDeserializer_RemovesSerializer()
    {
        var inSerializerLock = false;
        moqSerializerLock.Setup(sl => sl.Acquire()).Callback(() => { inSerializerLock = true; }).Verifiable();
        moqSerializerLock.Setup(sl => sl.Release()).Callback(() => { inSerializerLock = false; }).Verifiable();

        var moqSerializerCount = new Mock<IDictionary<uint, uint>>();
        moqSerializerCount.SetupGet(d => d[0])
            .Callback(() => { Assert.IsTrue(inSerializerLock); })
            .Returns(1)
            .Verifiable();
        moqSerializerCount.SetupSet(d => d[0] = 0).Callback(() => Assert.IsTrue(inSerializerLock)).Verifiable();
        NonPublicInvocator.SetInstanceField(dummySocketSubscriber,
            "deserializersCallbackCount", moqSerializerCount.Object);

        var hasCalledCallback = false;
        Action<DummyMessage, object, ISession> msgHandler =
            (msgId, msg, connSession) => { hasCalledCallback = true; };
        var moqBinPQ0Deserializer = new Mock<ICallbackMessageDeserializer<DummyMessage>>();
        moqBinPQ0Deserializer.Object.Deserialized += msgHandler;
        moqBinPQ0Deserializer.Setup(bu => bu.IsRegistered(msgHandler))
            .Returns(true).Verifiable();
        // ReSharper disable once RedundantAssignment
        IMessageDeserializer? returnDeserializer = moqBinPQ0Deserializer.Object;
        moqSerialzierCache.Setup(sm => sm.TryGetValue(0, out returnDeserializer)).Returns(true).Verifiable();
        moqSerialzierCache.Setup(sm => sm.Remove(0))
            .Callback(() => { Assert.IsTrue(inSerializerLock); })
            .Returns(true)
            .Verifiable();

        dummySocketSubscriber.UnregisterDeserializer(0, msgHandler);

        moqBinPQ0Deserializer.Raise(us => us.Deserialized += null, new object(), new object(), new object());
        Assert.IsFalse(hasCalledCallback);
        moqSerializerLock.Verify();
        moqSerializerCount.Verify();
        moqBinPQ0Deserializer.Verify();
        moqSerialzierCache.Verify();
    }

    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public void NonRegisteredMsg_UnregisterDeserializer_ThrowsException()
    {
        var dummySafeMap = new LinkedListCache<uint, IMessageDeserializer>();
        NonPublicInvocator.SetInstanceField(dummySocketSubscriber, "deserializers", dummySafeMap);
        Action<DummyMessage, object, ISession> msgHandler = (msgId, msg, connSession) => { };

        dummySocketSubscriber.UnregisterDeserializer(0u, msgHandler);
    }

    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public void RegisteredDifferentCallbackTypeMsg_UnregisterDeserializer_ThrowsException()
    {
        var dummySafeMap = new LinkedListCache<uint, IMessageDeserializer>();
        NonPublicInvocator.SetInstanceField(dummySocketSubscriber, "deserializers", dummySafeMap);
        Action<DummyMessage, object, ISession> msgHandler = (msgId, msg, connSession) => { };
        var moqBinPQ0Deserializer = new Mock<ICallbackMessageDeserializer<DummyMessage>>();
        dummySafeMap.Add(0, moqBinPQ0Deserializer.Object);

        dummySocketSubscriber.UnregisterDeserializer(0u, msgHandler);
    }

    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public void NonRegisteredCallbackTypeMsg_UnregisterDeserializer_ThrowsException()
    {
        var dummySafeMap = new LinkedListCache<uint, IMessageDeserializer>();
        NonPublicInvocator.SetInstanceField(dummySocketSubscriber, "deserializers", dummySafeMap);
        Action<DummyMessage, object, ISession> msgHandler = (msgId, msg, connSession) => { };
        var moqBinPQ0Deserializer = new Mock<ICallbackMessageDeserializer<DummyMessage>>();
        moqBinPQ0Deserializer.Setup(sbu => sbu.IsRegistered(msgHandler)).Returns(false);
        dummySafeMap.Add(0, moqBinPQ0Deserializer.Object);

        dummySocketSubscriber.UnregisterDeserializer(0u, msgHandler);
    }

    public class DummyMessage : VersionedMessage
    {
        public override uint MessageId => 99999;
        public override IVersionedMessage Clone() => this;
    }

    internal class DummySocketStreamSubscriber : SocketStreamSubscriber
    {
        private readonly IMessageIdDeserializationRepository messageIdDeserializationRepository;

        public DummySocketStreamSubscriber() :
            base(FLoggerFactory.Instance.GetLogger(typeof(DummySocketStreamSubscriber)),
                new Mock<ISocketDispatcher>().Object, "", 1,
                new ConcurrentMap<uint, IMessageDeserializer>())
        {
            messageIdDeserializationRepository = new OrxSerdesFactory(new Recycler());
            StreamToPublisher = new Mock<IBinaryStreamPublisher>().Object;
        }

        public DummySocketStreamSubscriber(IFLogger logger, ISocketDispatcher dispatcher, string sessionDescription,
            int wholeMessagesPerReceive, IMap<uint, IMessageDeserializer> serializerCache,
            IMessageIdDeserializationRepository messageIdDeserializationRepository, int recvBuffrSize,
            IBinaryStreamPublisher streamToPublisher)
            : base(logger, dispatcher, sessionDescription, wholeMessagesPerReceive, serializerCache)
        {
            RecvBufferSize = recvBuffrSize;
            StreamToPublisher = streamToPublisher;
            this.messageIdDeserializationRepository = messageIdDeserializationRepository;
        }

        public override int RecvBufferSize { get; }

        public override IBinaryStreamPublisher StreamToPublisher { get; }

        protected override IMessageIdDeserializationRepository GetFactory() => messageIdDeserializationRepository;

        public override void OnCxError(ISocketSessionConnection cx, string errorMsg, int proposedReconnect) { }

        public override IMessageStreamDecoder?
            GetDecoder(IMap<uint, IMessageDeserializer> decoderDeserializers) =>
            null;
    }
}
