﻿// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Reactive.Disposables;
using FortitudeCommon.AsyncProcessing;
using FortitudeCommon.Types;
using FortitudeIO.Transports.Network.Config;
using FortitudeMarkets.Config;
using FortitudeMarkets.Config.PricingConfig;
using FortitudeMarkets.Pricing.FeedEvents.LastTraded;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.PQ.Subscription.Standalone;
using Moq;
using static FortitudeIO.Transports.Network.Config.CountryCityCodes;
using static FortitudeMarkets.Pricing.FeedEvents.TickerInfo.TickerQuoteDetailLevel;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Subscription.Standalone;

[TestClass]
public class PQTickerFeedSubscriptionQuoteStreamTests
{
    private IPricingServerConfig feedConfig               = null!;
    private Mock<IPQPublishableTickInstant> initializingQuote        = null!;
    private Mock<ISyncLock>      initializingQuoteSyncLock = null!;
    private List<IPQPublishableTickInstant> list1ReceivedQuotes      = null!;
    private List<IPQPublishableTickInstant> list2ReceivedQuotes      = null!;

    private PQTickerFeedSubscriptionQuoteStream<IPQPublishableTickInstant> pqTickerFeedSubscription = null!;

    private IPQPublishableTickInstant    publishingQuote  = null!;
    private ISourceTickerInfo sourceTickerInfo = null!;

    [TestInitialize]
    public void SetUp()
    {
        feedConfig =
            new PricingServerConfig
                (new NetworkTopicConnectionConfig
                     ("testConnectionName", SocketConversationProtocol.TcpAcceptor
                    , new List<IEndpointConfig>
                      {
                          new EndpointConfig("testHost", 9090, AUinMEL)
                      })
               , new NetworkTopicConnectionConfig
                     ("testConnectionName", SocketConversationProtocol.TcpAcceptor
                    , new List<IEndpointConfig>
                      {
                          new EndpointConfig("testHost", 9090, AUinMEL)
                      }));

        sourceTickerInfo = new SourceTickerInfo
            (ushort.MaxValue, "TestSource", ushort.MaxValue, "TestTicker", Level3Quote, MarketClassification.Unknown
          ,  AUinMEL, AUinMEL, AUinMEL
           , 20, 0.00001m, 30000m, 50000000m, 1000m
           , layerFlags: LayerFlags.Volume | LayerFlags.Price | LayerFlags.OrderTraderName | LayerFlags.OrderSize | LayerFlags.OrdersCount
           , lastTradedFlags: LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName |
                              LastTradedFlags.LastTradedVolume | LastTradedFlags.LastTradedTime);

        publishingQuote = new PQPublishableTickInstant(sourceTickerInfo);

        initializingQuote        = new Mock<IPQPublishableTickInstant>();
        initializingQuoteSyncLock = new Mock<ISyncLock>();
        initializingQuote.SetupGet(pqPti => pqPti.Lock).Returns(initializingQuoteSyncLock.Object);

        pqTickerFeedSubscription = new PQTickerFeedSubscriptionQuoteStream<IPQPublishableTickInstant>
            (feedConfig, sourceTickerInfo, initializingQuote.Object);

        list1ReceivedQuotes = new List<IPQPublishableTickInstant>();
        list2ReceivedQuotes = new List<IPQPublishableTickInstant>();
    }

    [TestMethod]
    public void NewTickerFeedSubscription_MultipleSubscribe_AddsObserverToSubscription()
    {
        pqTickerFeedSubscription.Subscribe(q0 => { list1ReceivedQuotes.Add(q0.Clone()); });

        publishingQuote.SingleTickValue = 1.234567m;
        pqTickerFeedSubscription.OnNext(publishingQuote);

        Assert.AreEqual(1, list1ReceivedQuotes.Count);
        Assert.AreEqual(1.234567m, list1ReceivedQuotes[0].SingleTickValue);


        pqTickerFeedSubscription.Subscribe(q0 => { list2ReceivedQuotes.Add(q0.Clone()); });

        publishingQuote.SingleTickValue = 9.876543m;
        pqTickerFeedSubscription.OnNext(publishingQuote);

        Assert.AreEqual(2, list1ReceivedQuotes.Count);
        Assert.AreEqual(9.876543m, list1ReceivedQuotes[1].SingleTickValue);
        Assert.AreEqual(1, list2ReceivedQuotes.Count);
        Assert.AreEqual(9.876543m, list2ReceivedQuotes[0].SingleTickValue);
    }

    [TestMethod]
    public void NewTickerFeedSubscription_MultipleSubscribeThenOneUnsubscribe_AddsObserverToSubscription()
    {
        var list1Sub = pqTickerFeedSubscription.Subscribe(q0 => { list1ReceivedQuotes.Add(q0.Clone()); });
        pqTickerFeedSubscription.Subscribe(q0 => { list2ReceivedQuotes.Add(q0.Clone()); });

        publishingQuote.SingleTickValue = 1.234567m;
        pqTickerFeedSubscription.OnNext(publishingQuote);

        Assert.AreEqual(1, list1ReceivedQuotes.Count);
        Assert.AreEqual(1.234567m, list1ReceivedQuotes[0].SingleTickValue);
        Assert.AreEqual(1, list2ReceivedQuotes.Count);
        Assert.AreEqual(1.234567m, list2ReceivedQuotes[0].SingleTickValue);

        list1Sub.Dispose();

        publishingQuote.SingleTickValue = 9.876543m;
        pqTickerFeedSubscription.OnNext(publishingQuote);

        Assert.AreEqual(1, list1ReceivedQuotes.Count);
        Assert.AreEqual(1.234567m, list1ReceivedQuotes[0].SingleTickValue);
        Assert.AreEqual(2, list2ReceivedQuotes.Count);
        Assert.AreEqual(9.876543m, list2ReceivedQuotes[1].SingleTickValue);
    }

    [TestMethod]
    public void NewTickerFeedSubscription_OnComplete_CallIncompleteOnObserver()
    {
        var hasCalledOnComplete = false;
        pqTickerFeedSubscription.Subscribe(_ => { }, () => hasCalledOnComplete = true);

        pqTickerFeedSubscription.OnCompleted();

        Assert.IsTrue(hasCalledOnComplete);

        hasCalledOnComplete = false;
        pqTickerFeedSubscription.Subscribe(_ => { }, () => hasCalledOnComplete = true);
        Assert.IsTrue(hasCalledOnComplete);
    }

    [TestMethod]
    public void NewTickerFeedSubscription_OnError_CallIncompleteOnObserver()
    {
        var hasCalledOnError = false;
        pqTickerFeedSubscription.Subscribe(_ => { }, _ => { hasCalledOnError = true; });
        pqTickerFeedSubscription.OnError(new ArgumentException());
        Assert.IsTrue(hasCalledOnError);

        hasCalledOnError = false;
        pqTickerFeedSubscription.Subscribe(_ => { }, _ => { hasCalledOnError = true; });
        Assert.IsTrue(hasCalledOnError);
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

        var subscription = pqTickerFeedSubscription.Subscribe(_=> { });

        Assert.IsFalse(hasCalledCleanupAction);
        subscription.Dispose();
        Assert.IsTrue(hasCalledCleanupAction);
    }

    [TestMethod]
    public void AddedCleanupActionsAfterAnySubscriptions_LastSubscriptionDisposed_RunsCleanupActions()
    {
        var subscription = pqTickerFeedSubscription.Subscribe(_ => { });

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
        initializingQuoteSyncLock.Setup(sl => sl.Acquire()).Callback(() => { insideSyncLock = true; });
        initializingQuoteSyncLock.Setup(sl => sl.Release()).Callback(() => { insideSyncLock = false; });

        var hasRunCallback         = false;
        var subscribedObserverMock = new Mock<IObserver<IPQPublishableTickInstant>>();
        var observerCollectionMock = new Mock<IList<IObserver<IPQPublishableTickInstant>>>();

        NonPublicInvocator.SetInstanceField(pqTickerFeedSubscription, "observers", observerCollectionMock.Object);

        observerCollectionMock.Setup(l => l.Add(subscribedObserverMock.Object)).Callback<IObserver<IPQPublishableTickInstant>>(
         _ =>
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
        initializingQuoteSyncLock.Setup(sl => sl.Acquire()).Callback(() => { insideSyncLock = true; });
        initializingQuoteSyncLock.Setup(sl => sl.Release()).Callback(() => { insideSyncLock = false; });

        var hasRunCallback         = false;
        var subscribedObserverMock = new Mock<IObserver<IPQPublishableTickInstant>>();
        var observerCollectionMock = new Mock<IList<IObserver<IPQPublishableTickInstant>>>();

        NonPublicInvocator.SetInstanceField(pqTickerFeedSubscription, "observers", observerCollectionMock.Object);

        observerCollectionMock.Setup(l => l.Remove(subscribedObserverMock.Object)).Callback<IObserver<IPQPublishableTickInstant>>(
         _ =>
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
