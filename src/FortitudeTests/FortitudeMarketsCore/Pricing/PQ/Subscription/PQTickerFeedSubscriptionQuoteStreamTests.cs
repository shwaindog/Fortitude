#region

using System.Reactive.Disposables;
using FortitudeCommon.AsyncProcessing;
using FortitudeCommon.Types;
using FortitudeIO.Transports.NewSocketAPI.Config;
using FortitudeIO.Transports.NewSocketAPI.Sockets;
using FortitudeMarketsApi.Configuration.ClientServerConfig;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.LastTraded;
using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsApi.Pricing.Quotes.SourceTickerInfo;
using FortitudeMarketsCore.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsCore.Pricing.PQ.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Subscription;
using Moq;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription;

[TestClass]
public class PQTickerFeedSubscriptionQuoteStreamTests
{
    private ISnapshotUpdatePricingServerConfig feedConfig = null!;
    private Mock<IPQLevel0Quote> initializingQuote = null!;
    private Mock<ISyncLock> intializingQuoteSyncLock = null!;

    private List<IPQLevel0Quote> list1ReceivedQuotes = null!;
    private List<IPQLevel0Quote> list2ReceivedQuotes = null!;
    private PQTickerFeedSubscriptionQuoteStream<IPQLevel0Quote> pqTickerFeedSubscription = null!;
    private IPQLevel0Quote publishingQuote = null!;
    private ISourceTickerQuoteInfo sourceTickerQuoteInfo = null!;

    [TestInitialize]
    public void SetUp()
    {
        feedConfig = new SnapshotUpdatePricingServerConfig("TestServerConfig", MarketServerType.MarketData,
            new[]
            {
                new SocketTopicConnectionConfig("testConnectionName", SocketConversationProtocol.TcpAcceptor
                    , new List<ISocketConnectionConfig>
                    {
                        new SocketConnectionConfig("testhost", 9090)
                    })
            }, null, 1234,
            new[] { new SourceTickerPublicationConfig(0, "", "") },
            true, true);

        sourceTickerQuoteInfo = new SourceTickerClientAndPublicationConfig(
            uint.MaxValue, "TestSource", "TestTicker", 20, 0.00001m, 30000m, 50000000m, 1000m, 1,
            LayerFlags.Volume | LayerFlags.Price | LayerFlags.TraderName | LayerFlags.TraderSize
            | LayerFlags.TraderCount, LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName
                                                                  | LastTradedFlags.LastTradedVolume |
                                                                  LastTradedFlags.LastTradedTime, null,
            3000);

        publishingQuote = new PQLevel0Quote(sourceTickerQuoteInfo);

        initializingQuote = new Mock<IPQLevel0Quote>();
        intializingQuoteSyncLock = new Mock<ISyncLock>();
        initializingQuote.SetupGet(pql0Q => pql0Q.Lock).Returns(intializingQuoteSyncLock.Object);

        pqTickerFeedSubscription = new PQTickerFeedSubscriptionQuoteStream<IPQLevel0Quote>(feedConfig,
            sourceTickerQuoteInfo, initializingQuote.Object);

        list1ReceivedQuotes = new List<IPQLevel0Quote>();
        list2ReceivedQuotes = new List<IPQLevel0Quote>();
    }

    [TestMethod]
    public void NewTickerFeedSubscription_MultiipleSubscribe_AddsObserverToSubscription()
    {
        pqTickerFeedSubscription.Subscribe(q0 => { list1ReceivedQuotes.Add(q0.Clone()); });

        publishingQuote.SinglePrice = 1.234567m;
        pqTickerFeedSubscription.OnNext(publishingQuote);

        Assert.AreEqual(1, list1ReceivedQuotes.Count);
        Assert.AreEqual(1.234567m, list1ReceivedQuotes[0].SinglePrice);


        pqTickerFeedSubscription.Subscribe(q0 => { list2ReceivedQuotes.Add(q0.Clone()); });

        publishingQuote.SinglePrice = 9.876543m;
        pqTickerFeedSubscription.OnNext(publishingQuote);

        Assert.AreEqual(2, list1ReceivedQuotes.Count);
        Assert.AreEqual(9.876543m, list1ReceivedQuotes[1].SinglePrice);
        Assert.AreEqual(1, list2ReceivedQuotes.Count);
        Assert.AreEqual(9.876543m, list2ReceivedQuotes[0].SinglePrice);
    }

    [TestMethod]
    public void NewTickerFeedSubscription_MultipleSubscribeThenOneUnsubscribe_AddsObserverToSubscription()
    {
        var list1Sub = pqTickerFeedSubscription.Subscribe(q0 => { list1ReceivedQuotes.Add(q0.Clone()); });
        pqTickerFeedSubscription.Subscribe(q0 => { list2ReceivedQuotes.Add(q0.Clone()); });

        publishingQuote.SinglePrice = 1.234567m;
        pqTickerFeedSubscription.OnNext(publishingQuote);

        Assert.AreEqual(1, list1ReceivedQuotes.Count);
        Assert.AreEqual(1.234567m, list1ReceivedQuotes[0].SinglePrice);
        Assert.AreEqual(1, list2ReceivedQuotes.Count);
        Assert.AreEqual(1.234567m, list2ReceivedQuotes[0].SinglePrice);

        list1Sub.Dispose();

        publishingQuote.SinglePrice = 9.876543m;
        pqTickerFeedSubscription.OnNext(publishingQuote);

        Assert.AreEqual(1, list1ReceivedQuotes.Count);
        Assert.AreEqual(1.234567m, list1ReceivedQuotes[0].SinglePrice);
        Assert.AreEqual(2, list2ReceivedQuotes.Count);
        Assert.AreEqual(9.876543m, list2ReceivedQuotes[1].SinglePrice);
    }

    [TestMethod]
    public void NewTickerFeedSubscription_OnComplete_CallUncompleteOnObserver()
    {
        var hasCalledOnComplete = false;
        pqTickerFeedSubscription.Subscribe(q0 => { }, () => hasCalledOnComplete = true);

        pqTickerFeedSubscription.OnCompleted();

        Assert.IsTrue(hasCalledOnComplete);

        hasCalledOnComplete = false;
        pqTickerFeedSubscription.Subscribe(q0 => { }, () => hasCalledOnComplete = true);
        Assert.IsTrue(hasCalledOnComplete);
    }

    [TestMethod]
    public void NewTickerFeedSubscription_OnError_CallUncompleteOnObserver()
    {
        var hasCalledOnErrror = false;
        pqTickerFeedSubscription.Subscribe(q0 => { }, exception => { hasCalledOnErrror = true; });
        pqTickerFeedSubscription.OnError(new ArgumentException());
        Assert.IsTrue(hasCalledOnErrror);

        hasCalledOnErrror = false;
        pqTickerFeedSubscription.Subscribe(q0 => { }, exception => { hasCalledOnErrror = true; });
        Assert.IsTrue(hasCalledOnErrror);
    }

    [TestMethod]
    public void AddedCleanupActions_Unsubscribe_RunsCleanupActions()
    {
        var hasCalledCleanupAction = false;
        pqTickerFeedSubscription.AddCleanupAction(Disposable.Create(() => hasCalledCleanupAction = true));

        Assert.IsFalse(hasCalledCleanupAction);
        pqTickerFeedSubscription.Unsubscribe();
        Assert.IsTrue(hasCalledCleanupAction);
    }

    [TestMethod]
    public void AddedCleanupActionsBeforeAnySubscriptions_LastSubscriptionDisposed_RunsCleanupActions()
    {
        var hasCalledCleanupAction = false;
        pqTickerFeedSubscription.AddCleanupAction(Disposable.Create(() => hasCalledCleanupAction = true));

        var subscription = pqTickerFeedSubscription.Subscribe(q0 => { });

        Assert.IsFalse(hasCalledCleanupAction);
        subscription.Dispose();
        Assert.IsTrue(hasCalledCleanupAction);
    }

    [TestMethod]
    public void AddedCleanupActionsAfterAnySubscriptions_LastSubscriptionDisposed_RunsCleanupActions()
    {
        var subscription = pqTickerFeedSubscription.Subscribe(q0 => { });

        var hasCalledCleanupAction = false;
        pqTickerFeedSubscription.AddCleanupAction(Disposable.Create(() => hasCalledCleanupAction = true));

        Assert.IsFalse(hasCalledCleanupAction);
        subscription.Dispose();
        Assert.IsTrue(hasCalledCleanupAction);
    }

    [TestMethod]
    public void AddingNewObserver_Subscribe_ProtectsObserverCollectionInQuoteSyncLock()
    {
        var insideSyncLock = false;
        intializingQuoteSyncLock.Setup(sl => sl.Acquire()).Callback(() => { insideSyncLock = true; });
        intializingQuoteSyncLock.Setup(sl => sl.Release()).Callback(() => { insideSyncLock = false; });

        var hasRunCallback = false;
        var subscribedObserverMock = new Mock<IObserver<IPQLevel0Quote>>();
        var observerCollectionMock = new Mock<IList<IObserver<IPQLevel0Quote>>>();

        NonPublicInvocator.SetInstanceField(pqTickerFeedSubscription, "observers", observerCollectionMock.Object);

        observerCollectionMock.Setup(l => l.Add(subscribedObserverMock.Object)).Callback<IObserver<IPQLevel0Quote>>(
            sub =>
            {
                Assert.IsTrue(insideSyncLock);
                hasRunCallback = true;
            });

        pqTickerFeedSubscription.Subscribe(subscribedObserverMock.Object);

        Assert.IsTrue(hasRunCallback);
        Assert.IsFalse(insideSyncLock);
    }

    [TestMethod]
    public void RemovingSubscriber_SubscriberDispose_ProtectsObserverCollectionInQuoteSyncLock()
    {
        var insideSyncLock = false;
        intializingQuoteSyncLock.Setup(sl => sl.Acquire()).Callback(() => { insideSyncLock = true; });
        intializingQuoteSyncLock.Setup(sl => sl.Release()).Callback(() => { insideSyncLock = false; });

        var hasRunCallback = false;
        var subscribedObserverMock = new Mock<IObserver<IPQLevel0Quote>>();
        var observerCollectionMock = new Mock<IList<IObserver<IPQLevel0Quote>>>();

        NonPublicInvocator.SetInstanceField(pqTickerFeedSubscription, "observers", observerCollectionMock.Object);

        observerCollectionMock.Setup(l => l.Remove(subscribedObserverMock.Object)).Callback<IObserver<IPQLevel0Quote>>(
            sub =>
            {
                Assert.IsTrue(insideSyncLock);
                hasRunCallback = true;
            });

        var subscription = pqTickerFeedSubscription.Subscribe(subscribedObserverMock.Object);

        Assert.IsFalse(hasRunCallback);
        Assert.IsFalse(insideSyncLock);

        subscription.Dispose();

        Assert.IsTrue(hasRunCallback);
        Assert.IsFalse(insideSyncLock);
    }
}
