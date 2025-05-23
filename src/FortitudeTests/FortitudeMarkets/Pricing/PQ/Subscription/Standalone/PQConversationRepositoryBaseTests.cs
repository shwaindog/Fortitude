﻿// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.AsyncWrappers;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeCommon.Types;
using FortitudeIO.Conversations;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Transports.Network.Config;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.PQ.Subscription.Standalone;
using Moq;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Subscription.Standalone;

[TestClass]
public class PQConversationRepositoryBaseTests
{
    private DummySktSubRegFctryBse<DummyConversationSubscriber>
        dummySktSubRegFctryBs = null!;

    private DummyConversationSubscriber dummySocketSubscriber = null!;

    private Mock<IMessageDeserializationRepository> moqBinaryDeserializationFactory = null!;

    private Mock<IFLogger>  moqFlogger  = null!;
    private Mock<IOSSocket> moqOsSocket = null!;

    private Mock<IOSParallelController> moqParallelControler = null!;

    private Mock<IOSParallelControllerFactory> moqParallelControllerFactory = null!;

    private Mock<INotifyingMessageDeserializer<PQPublishableTickInstant>> moqSocketBinaryDeserializer = null!;

    private Mock<INetworkTopicConnectionConfig> moqSocketTopicConnectionConfig = null!;

    private string testHostName = null!;

    [TestInitialize]
    public void SetUp()
    {
        moqFlogger           = new Mock<IFLogger>();
        moqParallelControler = new Mock<IOSParallelController>();

        moqParallelControllerFactory = new Mock<IOSParallelControllerFactory>();
        moqParallelControllerFactory.SetupGet(pcf => pcf.GetOSParallelController)
                                    .Returns(moqParallelControler.Object);
        OSParallelControllerFactory.Instance = moqParallelControllerFactory.Object;

        moqSocketTopicConnectionConfig  = new Mock<INetworkTopicConnectionConfig>();
        moqBinaryDeserializationFactory = new Mock<IMessageDeserializationRepository>();
        moqSocketBinaryDeserializer     = new Mock<INotifyingMessageDeserializer<PQPublishableTickInstant>>();

        moqOsSocket = new Mock<IOSSocket>();

        testHostName = "TestHostname";
        moqSocketTopicConnectionConfig.SetupGet(scc => scc.TopicName).Returns(testHostName);
        moqFlogger.Setup(fl => fl.Info(It.IsAny<string>(), It.IsAny<object[]>()));
        moqOsSocket.SetupAllProperties();

        moqSocketBinaryDeserializer.SetupAllProperties();

        moqBinaryDeserializationFactory
            .Setup(bdf => bdf
                       .GetDeserializer<PQPublishableTickInstant>(uint.MaxValue))
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
        var socketClient = dummySktSubRegFctryBs.RetrieveOrCreateConversation(moqSocketTopicConnectionConfig.Object);

        Assert.IsNotNull(socketClient);
        Assert.AreSame(dummySocketSubscriber, socketClient);

        var foundSubscription = dummySktSubRegFctryBs.RetrieveConversation(moqSocketTopicConnectionConfig.Object);
        Assert.AreSame(socketClient, foundSubscription);
    }

    [TestMethod]
    public void EmptySocketSubRegFactory_FindSocketSubscription_ReturnsNull()
    {
        var foundSubscription = dummySktSubRegFctryBs.RetrieveConversation(moqSocketTopicConnectionConfig.Object);
        Assert.IsNull(foundSubscription);
    }

    [TestMethod]
    public void RegisteredSocketSubscriber_UnregisterSocketSubscriber_FindSocketSubscriptionReturnsSameInstance()
    {
        var socketClient = dummySktSubRegFctryBs.RetrieveOrCreateConversation(moqSocketTopicConnectionConfig.Object);

        var socketSubscriptions = NonPublicInvocator.GetInstanceField<ConcurrentMap<INetworkTopicConnectionConfig,
            DummyConversationSubscriber>>(dummySktSubRegFctryBs, "socketSubscriptions");

        Assert.IsNotNull(socketClient);
        Assert.AreEqual(1, socketSubscriptions.Count);
        var kvp = socketSubscriptions.First();
        Assert.AreSame(dummySocketSubscriber, socketClient);
        Assert.AreSame(moqSocketTopicConnectionConfig.Object, kvp.Key);
        Assert.AreSame(socketClient, kvp.Value);

        dummySktSubRegFctryBs.RemoveConversation(moqSocketTopicConnectionConfig.Object);
        Assert.AreEqual(0, socketSubscriptions.Count);
        moqSocketBinaryDeserializer.Verify();
    }

    [TestMethod]
    public void MultiRegisteredFactory_UnregisterSocketSubscriber_RemoveSubscriptionEvenIfContainsSubscriptions()
    {
        moqBinaryDeserializationFactory
            .Setup(bdf => bdf
                       .GetDeserializer<PQPublishableTickInstant>(1u))
            .Returns(moqSocketBinaryDeserializer.Object)
            .Verifiable();

        var socketClient  = dummySktSubRegFctryBs.RetrieveOrCreateConversation(moqSocketTopicConnectionConfig.Object);
        var socketClient2 = dummySktSubRegFctryBs.RetrieveOrCreateConversation(moqSocketTopicConnectionConfig.Object);

        var socketSubscriptions = NonPublicInvocator.GetInstanceField<ConcurrentMap<INetworkTopicConnectionConfig,
            DummyConversationSubscriber>>(dummySktSubRegFctryBs, "socketSubscriptions");

        Assert.IsNotNull(socketClient);
        Assert.IsNotNull(socketClient2);
        Assert.AreEqual(1, socketSubscriptions.Count);
        var kvp = socketSubscriptions.First();
        Assert.AreSame(dummySocketSubscriber, socketClient);
        Assert.AreSame(moqSocketTopicConnectionConfig.Object, kvp.Key);
        Assert.AreSame(socketClient, kvp.Value);

        moqSocketBinaryDeserializer
            .Setup(sbd =>
                       sbd.IsRegistered(It.IsAny<ConversationMessageReceivedHandler<PQPublishableTickInstant>>())).Returns(true)
            .Verifiable();

        dummySktSubRegFctryBs.RemoveConversation(moqSocketTopicConnectionConfig.Object);

        Assert.IsNotNull(socketClient);
        Assert.IsNotNull(socketClient2);
        Assert.AreEqual(0, socketSubscriptions.Count);
    }

    internal class DummySktSubRegFctryBse<T> :
        PQConversationRepositoryBase<T> where T : class, IConversation
    {
        public DummySktSubRegFctryBse(T returnedSocketSubscriber) => ReturnedSocketSubscriber = returnedSocketSubscriber;

        public T ReturnedSocketSubscriber { get; }

        protected override T CreateNewSocketSubscriptionType(INetworkTopicConnectionConfig connectionConfig) => ReturnedSocketSubscriber;
    }

    public class DummyConversationSubscriber : IConversationSubscriber
    {
        public DummyConversationSubscriber()
        {
            Error   += (_, _) => { };
            Started += () => { };
            Stopped += () => { };
            Error?.Invoke("To hide complier warnings", 0);
            Started?.Invoke();
            Stopped?.Invoke();
        }

        public ConversationType  ConversationType  { get; set; } = ConversationType.Subscriber;
        public ConversationState ConversationState { get; set; } = ConversationState.New;


        public int Id { get; } = 0;

        public IConversationSession Session { get; } = null!;

        public string Name { get; set; } = "";

        public bool IsStarted { get; } = false;

        public IStreamListener? StreamListener { get; set; }

        public event Action<string, int>? Error;

        public event Action? Started;
        public event Action? Stopped;

        public void Start() { }

        public void Stop(CloseReason closeReason = CloseReason.Completed, string? reason = null) { }

        public void OnSessionFailure(string reason) { }
    }
}
