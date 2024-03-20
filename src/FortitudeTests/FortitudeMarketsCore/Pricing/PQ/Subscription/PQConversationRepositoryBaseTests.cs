#region

using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.AsyncWrappers;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeCommon.Types;
using FortitudeIO.Conversations;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Transports;
using FortitudeIO.Transports.NewSocketAPI.Config;
using FortitudeMarketsCore.Pricing.PQ.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Subscription;
using Moq;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription;

[TestClass]
public class PQConversationRepositoryBaseTests
{
    private DummySktSubRegFctryBse<DummyConversationSubscriber>
        dummySktSubRegFctryBs = null!;

    private DummyConversationSubscriber dummySocketSubscriber = null!;
    private Mock<IMessageIdDeserializationRepository> moqBinaryDeserializationFactory = null!;
    private Mock<IFLogger> moqFlogger = null!;
    private Mock<IOSSocket> moqOsSocket = null!;
    private Mock<IOSParallelController> moqParallelControler = null!;
    private Mock<IOSParallelControllerFactory> moqParallelControllerFactory = null!;
    private Mock<ISocketConnectionConfig> moqServerConnectionConfig = null!;
    private Mock<ICallbackMessageDeserializer<PQLevel0Quote>> moqSocketBinaryDeserializer = null!;
    private string testHostName = null!;
    private ushort testHostPort;

    [TestInitialize]
    public void SetUp()
    {
        moqFlogger = new Mock<IFLogger>();
        moqParallelControler = new Mock<IOSParallelController>();
        moqParallelControllerFactory = new Mock<IOSParallelControllerFactory>();
        moqParallelControllerFactory.SetupGet(pcf => pcf.GetOSParallelController)
            .Returns(moqParallelControler.Object);
        OSParallelControllerFactory.Instance = moqParallelControllerFactory.Object;
        moqServerConnectionConfig = new Mock<ISocketConnectionConfig>();
        moqBinaryDeserializationFactory = new Mock<IMessageIdDeserializationRepository>();
        moqSocketBinaryDeserializer = new Mock<ICallbackMessageDeserializer<PQLevel0Quote>>();
        moqOsSocket = new Mock<IOSSocket>();

        testHostName = "TestHostname";
        moqServerConnectionConfig.SetupGet(scc => scc.Hostname).Returns(testHostName);
        moqServerConnectionConfig.SetupGet(scc => scc.PortStartRange).Returns(testHostPort);
        testHostPort = 1979;
        moqFlogger.Setup(fl => fl.Info(It.IsAny<string>(), It.IsAny<object[]>()));
        moqOsSocket.SetupAllProperties();

        moqSocketBinaryDeserializer.SetupAllProperties();

        moqBinaryDeserializationFactory.Setup(bdf => bdf
                .GetDeserializer<PQLevel0Quote>(uint.MaxValue))
            .Returns(moqSocketBinaryDeserializer.Object)
            .Verifiable();

        dummySocketSubscriber = new DummyConversationSubscriber();

        dummySktSubRegFctryBs = new DummySktSubRegFctryBse<DummyConversationSubscriber>(dummySocketSubscriber);
    }

    [TestCleanup]
    public void TearDown()
    {
        OSParallelControllerFactory.Instance = new OSParallelControllerFactory();
    }

    [TestMethod]
    public void EmptySocketSubRegFactory_RegisterSocketSubscriber_FindSocketSubscriptionReturnsSameInstance()
    {
        var socketClient = dummySktSubRegFctryBs.RetrieveOrCreateConversation(moqServerConnectionConfig.Object);

        Assert.IsNotNull(socketClient);
        Assert.AreSame(dummySocketSubscriber, socketClient);

        var foundSubscription = dummySktSubRegFctryBs.RetrieveConversation(moqServerConnectionConfig.Object);
        Assert.AreSame(socketClient, foundSubscription);
    }

    [TestMethod]
    public void EmptySocketSubRegFactory_FindSocketSubscription_ReturnsNull()
    {
        var foundSubscription = dummySktSubRegFctryBs.RetrieveConversation(moqServerConnectionConfig.Object);
        Assert.IsNull(foundSubscription);
    }

    [TestMethod]
    public void RegisteredSocketSubscriber_UnregisterSocketSubscriber_FindSocketSubscriptionReturnsSameInstance()
    {
        var socketClient = dummySktSubRegFctryBs.RetrieveOrCreateConversation(moqServerConnectionConfig.Object);

        var socketSubscriptions = NonPublicInvocator.GetInstanceField<ConcurrentMap<ISocketConnectionConfig,
            DummyConversationSubscriber>>(dummySktSubRegFctryBs, "socketSubscriptions");

        Assert.IsNotNull(socketClient);
        Assert.AreEqual(1, socketSubscriptions.Count);
        var kvp = socketSubscriptions.First();
        Assert.AreSame(dummySocketSubscriber, socketClient);
        Assert.AreSame(moqServerConnectionConfig.Object, kvp.Key);
        Assert.AreSame(socketClient, kvp.Value);

        dummySktSubRegFctryBs.RemoveConversation(moqServerConnectionConfig.Object);
        Assert.AreEqual(0, socketSubscriptions.Count);
        moqSocketBinaryDeserializer.Verify();
    }

    [TestMethod]
    public void MultiRegisteredFactory_UnregisterSocketSubscriber_RemoveSubscriptionEvenIfContainsSubscriptions()
    {
        moqBinaryDeserializationFactory.Setup(bdf => bdf
                .GetDeserializer<PQLevel0Quote>(1u))
            .Returns(moqSocketBinaryDeserializer.Object)
            .Verifiable();

        var socketClient = dummySktSubRegFctryBs.RetrieveOrCreateConversation(moqServerConnectionConfig.Object);
        var socketClient2 = dummySktSubRegFctryBs.RetrieveOrCreateConversation(moqServerConnectionConfig.Object);

        var socketSubscriptions = NonPublicInvocator.GetInstanceField<ConcurrentMap<ISocketConnectionConfig,
            DummyConversationSubscriber>>(dummySktSubRegFctryBs, "socketSubscriptions");

        Assert.IsNotNull(socketClient);
        Assert.IsNotNull(socketClient2);
        Assert.AreEqual(1, socketSubscriptions.Count);
        var kvp = socketSubscriptions.First();
        Assert.AreSame(dummySocketSubscriber, socketClient);
        Assert.AreSame(moqServerConnectionConfig.Object, kvp.Key);
        Assert.AreSame(socketClient, kvp.Value);

        moqSocketBinaryDeserializer.Setup(sbd =>
            sbd.IsRegistered(It.IsAny<Action<PQLevel0Quote, object, ISession>>())).Returns(true).Verifiable();

        dummySktSubRegFctryBs.RemoveConversation(moqServerConnectionConfig.Object);

        Assert.IsNotNull(socketClient);
        Assert.IsNotNull(socketClient2);
        Assert.AreEqual(0, socketSubscriptions.Count);
    }

    internal class DummySktSubRegFctryBse<T> :
        PQConversationRepositoryBase<T> where T : class, IConversation
    {
        public DummySktSubRegFctryBse(T returnedSocketSubscriber) =>
            ReturnedSocketSubscriber = returnedSocketSubscriber;

        public T ReturnedSocketSubscriber { get; }

        protected override T CreateNewSocketSubscriptionType(ISocketConnectionConfig connectionConfig) =>
            ReturnedSocketSubscriber;
    }

    public class DummyConversationSubscriber : IConversationSubscriber
    {
        public ConversationType ConversationType { get; set; } = ConversationType.Subscriber;
        public ConversationState ConversationState { get; set; } = ConversationState.New;
        public string Name { get; set; } = "";
        public event Action<string, int>? Error;
        public event Action? Started;
        public event Action? Stopped;

        public void Start() { }

        public void Stop() { }

        public bool IsStarted { get; } = false;
        public IConversationListener? ConversationListener { get; set; }
    }
}
