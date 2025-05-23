// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.AsyncProcessing;
using FortitudeCommon.Chronometry;
using FortitudeCommon.Monitoring.Logging.Diagnostics.Performance;
using FortitudeCommon.Serdes;
using FortitudeCommon.Serdes.Binary;
using FortitudeCommon.Types;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Protocols.Serdes.Binary.Sockets;
using FortitudeIO.Transports.Network.Logging;
using FortitudeMarkets.Pricing;
using FortitudeMarkets.Pricing.FeedEvents;
using FortitudeMarkets.Pricing.FeedEvents.LastTraded;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.LastTraded;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.PQ.Serdes;
using FortitudeMarkets.Pricing.PQ.Serdes.Deserialization;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;
using FortitudeTests.FortitudeMarkets.Pricing.FeedEvents.Quotes;
using Moq;
using static FortitudeMarkets.Configuration.ClientServerConfig.MarketClassificationExtensions;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Serdes.Deserialization;

[TestClass]
public class PQMessageDeserializerBaseTests
{
    private const int MessageHeaderByteSize = PQQuoteMessageHeader.HeaderSize;
    private const int BufferReadWriteOffset = 6;

    private DummyPQMessageDeserializerBase<IPQPublishableLevel1Quote> dummyLevel1MessageDeserializer = null!;
    private DummyPQMessageDeserializerBase<IPQPublishableLevel2Quote> dummyLevel2MessageDeserializer = null!;
    private DummyPQMessageDeserializerBase<IPQPublishableLevel3Quote> dummyLevel3MessageDeserializer = null!;

    private DummyPQMessageDeserializerBase<IPQPublishableTickInstant> dummyTickInstantDeserializer = null!;

    private bool haveCalledAcquire;

    private Mock<IObserver<IPQPublishableLevel1Quote>> moqL1QObserver = null!;
    private Mock<IObserver<IPQPublishableLevel2Quote>> moqL2QObserver = null!;
    private Mock<IObserver<IPQPublishableLevel3Quote>> moqL3QObserver = null!;

    private Mock<IList<IObserver<IPQPublishableLevel1Quote>>> moqLevel1Subscribers = null!;
    private Mock<IList<IObserver<IPQPublishableLevel2Quote>>> moqLevel2Subscribers = null!;
    private Mock<IList<IObserver<IPQPublishableLevel3Quote>>> moqLevel3Subscribers = null!;

    private Mock<IPerfLogger>          moqPerfLogger        = null!;
    private Mock<IPerfLoggerPool>      moqPerfLoggerPool    = null!;
    private Mock<IPQMessageDeserializer> moqQuoteDeserializer = null!;
    private Mock<ISyncLock>            moqSyncLock          = null!;

    private Mock<IObserver<IPQPublishableTickInstant>> moqTickInstantObserver = null!;

    private Mock<IList<IObserver<IPQPublishableTickInstant>>> moqTickInstantSubscribers = null!;
    private Mock<ISourceTickerInfo>                moqUniqueSrcTkrId         = null!;

    private CircularReadWriteBuffer readWriteBuffer         = null!;
    private SocketBufferReadContext socketBufferReadContext = null!;
    private SourceTickerInfo        sourceTickerInfo        = null!;

    private IDisposable subscribedL1Observer = null!;
    private IDisposable subscribedL2Observer = null!;
    private IDisposable subscribedL3Observer = null!;

    private IDisposable subscribedTickInstantObserver = null!;

    [TestInitialize]
    public void SetUp()
    {
        moqUniqueSrcTkrId = new Mock<ISourceTickerInfo>();
        moqUniqueSrcTkrId.SetupGet(stqi => stqi.FilledAttributes).Returns([]);
        dummyTickInstantDeserializer = new DummyPQMessageDeserializerBase<IPQPublishableTickInstant>(moqUniqueSrcTkrId.Object);
        dummyLevel1MessageDeserializer = new DummyPQMessageDeserializerBase<IPQPublishableLevel1Quote>(moqUniqueSrcTkrId.Object);
        dummyLevel2MessageDeserializer = new DummyPQMessageDeserializerBase<IPQPublishableLevel2Quote>(moqUniqueSrcTkrId.Object);
        dummyLevel3MessageDeserializer = new DummyPQMessageDeserializerBase<IPQPublishableLevel3Quote>(moqUniqueSrcTkrId.Object);
        moqQuoteDeserializer         = new Mock<IPQMessageDeserializer>();

        readWriteBuffer = new CircularReadWriteBuffer(new byte[30_000]);
        socketBufferReadContext = new SocketBufferReadContext
        {
            DetectTimestamp    = new DateTime(2017, 07, 01, 18, 59, 22)
          , ReceivingTimestamp = new DateTime(2017, 07, 01, 19, 03, 22)
          , DeserializerTime   = new DateTime(2017, 07, 01, 19, 03, 52), EncodedBuffer = readWriteBuffer
          , MessageHeader      = new MessageHeader(1, 0, 0, 1)
        };
        readWriteBuffer.ReadCursor  = BufferReadWriteOffset;
        readWriteBuffer.WriteCursor = BufferReadWriteOffset;

        sourceTickerInfo =
            new SourceTickerInfo
                (ushort.MaxValue, "TestSource", ushort.MaxValue, "TestTicker", TickerQuoteDetailLevel.Level3Quote, Unknown
               , 20, 0.000001m, 30000m, 50000000m, 1000m
               , layerFlags: LayerFlags.Volume | LayerFlags.Price
               , lastTradedFlags: LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName | LastTradedFlags.LastTradedVolume |
                                  LastTradedFlags.LastTradedTime);

        moqPerfLoggerPool = new Mock<IPerfLoggerPool>();

        NonPublicInvocator.SetStaticField
            (dummyTickInstantDeserializer, "PublishPQQuoteDeserializerLatencyTraceLoggerPool", moqPerfLoggerPool.Object);
        NonPublicInvocator.SetStaticField
            (dummyLevel1MessageDeserializer, "PublishPQQuoteDeserializerLatencyTraceLoggerPool", moqPerfLoggerPool.Object);
        NonPublicInvocator.SetStaticField
            (dummyLevel2MessageDeserializer, "PublishPQQuoteDeserializerLatencyTraceLoggerPool", moqPerfLoggerPool.Object);
        NonPublicInvocator.SetStaticField
            (dummyLevel3MessageDeserializer, "PublishPQQuoteDeserializerLatencyTraceLoggerPool", moqPerfLoggerPool.Object);

        moqPerfLogger = new Mock<IPerfLogger>();
        moqPerfLoggerPool.Setup(plp => plp.StartNewTrace())
                         .Returns(moqPerfLogger.Object).Verifiable();
        moqPerfLoggerPool.Setup(plp => plp.StopTrace(moqPerfLogger.Object))
                         .Verifiable();
        moqPerfLogger.SetupGet(pl => pl.Enabled).Returns(true);
    }

    [TestCleanup]
    public void TearDown()
    {
        var realInstance =
            PerfLoggingPoolFactory.Instance.GetLatencyTracingLoggerPool
                ("clientCallback", TimeSpan.FromMilliseconds(10), typeof(UserCallback));

        NonPublicInvocator.SetStaticField
            (dummyTickInstantDeserializer, "PublishPQQuoteDeserializerLatencyTraceLoggerPool", realInstance);
        NonPublicInvocator.SetStaticField
            (dummyLevel1MessageDeserializer, "PublishPQQuoteDeserializerLatencyTraceLoggerPool", realInstance);
        NonPublicInvocator.SetStaticField
            (dummyLevel2MessageDeserializer, "PublishPQQuoteDeserializerLatencyTraceLoggerPool", realInstance);
        NonPublicInvocator.SetStaticField
            (dummyLevel3MessageDeserializer, "PublishPQQuoteDeserializerLatencyTraceLoggerPool", realInstance);
    }

    [TestMethod]
    public void NewPQQuoteDeserializer_New_SetsSourceTickerIdentifier()
    {
        Assert.IsNotNull(dummyTickInstantDeserializer.PublishedQuote);
        Assert.IsNotNull(dummyLevel1MessageDeserializer.PublishedQuote);
        Assert.IsNotNull(dummyLevel2MessageDeserializer.PublishedQuote);
        Assert.IsNotNull(dummyLevel3MessageDeserializer.PublishedQuote);
    }

    [TestMethod]
    public void RegisteredCallback_InvokeOnReceivedUpdate_CallsCallback()
    {
        var haveCalledPQTickInstantCallback = false;
        var haveCalledPQLevel1QuoteCallback = false;
        var haveCalledPQLevel2QuoteCallback = false;
        var haveCalledPQLevel3QuoteCallback = false;

        dummyTickInstantDeserializer.ReceivedUpdate += _ => { haveCalledPQTickInstantCallback = true; };
        dummyLevel1MessageDeserializer.ReceivedUpdate += _ => { haveCalledPQLevel1QuoteCallback = true; };
        dummyLevel2MessageDeserializer.ReceivedUpdate += _ => { haveCalledPQLevel2QuoteCallback = true; };
        dummyLevel3MessageDeserializer.ReceivedUpdate += _ => { haveCalledPQLevel3QuoteCallback = true; };

        dummyTickInstantDeserializer.InvokeOnReceivedUpdate(moqQuoteDeserializer.Object);
        dummyLevel1MessageDeserializer.InvokeOnReceivedUpdate(moqQuoteDeserializer.Object);
        dummyLevel2MessageDeserializer.InvokeOnReceivedUpdate(moqQuoteDeserializer.Object);
        dummyLevel3MessageDeserializer.InvokeOnReceivedUpdate(moqQuoteDeserializer.Object);

        Assert.IsTrue(haveCalledPQTickInstantCallback);
        Assert.IsTrue(haveCalledPQLevel1QuoteCallback);
        Assert.IsTrue(haveCalledPQLevel2QuoteCallback);
        Assert.IsTrue(haveCalledPQLevel3QuoteCallback);
    }

    [TestMethod]
    public void RegisteredCallback_InvokeOnSyncOk_CallsCallback()
    {
        var haveCalledPQTickInstantCallback = false;
        var haveCalledPQLevel1QuoteCallback = false;
        var haveCalledPQLevel2QuoteCallback = false;
        var haveCalledPQLevel3QuoteCallback = false;

        dummyTickInstantDeserializer.SyncOk += _ => { haveCalledPQTickInstantCallback = true; };
        dummyLevel1MessageDeserializer.SyncOk += _ => { haveCalledPQLevel1QuoteCallback = true; };
        dummyLevel2MessageDeserializer.SyncOk += _ => { haveCalledPQLevel2QuoteCallback = true; };
        dummyLevel3MessageDeserializer.SyncOk += _ => { haveCalledPQLevel3QuoteCallback = true; };

        dummyTickInstantDeserializer.InvokeOnSyncOk(moqQuoteDeserializer.Object);
        dummyLevel1MessageDeserializer.InvokeOnSyncOk(moqQuoteDeserializer.Object);
        dummyLevel2MessageDeserializer.InvokeOnSyncOk(moqQuoteDeserializer.Object);
        dummyLevel3MessageDeserializer.InvokeOnSyncOk(moqQuoteDeserializer.Object);

        Assert.IsTrue(haveCalledPQTickInstantCallback);
        Assert.IsTrue(haveCalledPQLevel1QuoteCallback);
        Assert.IsTrue(haveCalledPQLevel2QuoteCallback);
        Assert.IsTrue(haveCalledPQLevel3QuoteCallback);
    }

    [TestMethod]
    public void RegisteredCallback_InvokeOnOutOfSync_CallsCallback()
    {
        var haveCalledPQTickInstantCallback = false;
        var haveCalledPQLevel1QuoteCallback = false;
        var haveCalledPQLevel2QuoteCallback = false;
        var haveCalledPQLevel3QuoteCallback = false;

        dummyTickInstantDeserializer.OutOfSync += _ => { haveCalledPQTickInstantCallback = true; };
        dummyLevel1MessageDeserializer.OutOfSync += _ => { haveCalledPQLevel1QuoteCallback = true; };
        dummyLevel2MessageDeserializer.OutOfSync += _ => { haveCalledPQLevel2QuoteCallback = true; };
        dummyLevel3MessageDeserializer.OutOfSync += _ => { haveCalledPQLevel3QuoteCallback = true; };

        dummyTickInstantDeserializer.InvokeOnOutOfSync(moqQuoteDeserializer.Object);
        dummyLevel1MessageDeserializer.InvokeOnOutOfSync(moqQuoteDeserializer.Object);
        dummyLevel2MessageDeserializer.InvokeOnOutOfSync(moqQuoteDeserializer.Object);
        dummyLevel3MessageDeserializer.InvokeOnOutOfSync(moqQuoteDeserializer.Object);

        Assert.IsTrue(haveCalledPQTickInstantCallback);
        Assert.IsTrue(haveCalledPQLevel1QuoteCallback);
        Assert.IsTrue(haveCalledPQLevel2QuoteCallback);
        Assert.IsTrue(haveCalledPQLevel3QuoteCallback);
    }

    [TestMethod]
    public void NewPQQuoteDeserializer_Subscribe_SyncLockProtectsAddingObserver()
    {
        SetupObserversAndSyncLock();

        moqTickInstantSubscribers = new Mock<IList<IObserver<IPQPublishableTickInstant>>>();
        moqTickInstantSubscribers.Setup(lo => lo.Add(moqTickInstantObserver.Object)).Callback(() => { Assert.IsTrue(haveCalledAcquire); })
                                 .Verifiable();
        moqLevel1Subscribers = new Mock<IList<IObserver<IPQPublishableLevel1Quote>>>();
        moqLevel1Subscribers.Setup(lo => lo.Add(moqL1QObserver.Object)).Callback(() => { Assert.IsTrue(haveCalledAcquire); }).Verifiable();
        moqLevel2Subscribers = new Mock<IList<IObserver<IPQPublishableLevel2Quote>>>();
        moqLevel2Subscribers.Setup(lo => lo.Add(moqL2QObserver.Object)).Callback(() => { Assert.IsTrue(haveCalledAcquire); }).Verifiable();
        moqLevel3Subscribers = new Mock<IList<IObserver<IPQPublishableLevel3Quote>>>();
        moqLevel3Subscribers.Setup(lo => lo.Add(moqL3QObserver.Object)).Callback(() => { Assert.IsTrue(haveCalledAcquire); }).Verifiable();


        NonPublicInvocator.SetInstanceField
            (dummyTickInstantDeserializer, "Subscribers", moqTickInstantSubscribers.Object);
        NonPublicInvocator.SetInstanceField
            (dummyLevel1MessageDeserializer, "Subscribers", moqLevel1Subscribers.Object);
        NonPublicInvocator.SetInstanceField
            (dummyLevel2MessageDeserializer, "Subscribers", moqLevel2Subscribers.Object);
        NonPublicInvocator.SetInstanceField
            (dummyLevel3MessageDeserializer, "Subscribers", moqLevel3Subscribers.Object);

        subscribedTickInstantObserver = dummyTickInstantDeserializer.Subscribe(moqTickInstantObserver.Object);

        subscribedL1Observer = dummyLevel1MessageDeserializer.Subscribe(moqL1QObserver.Object);
        subscribedL2Observer = dummyLevel2MessageDeserializer.Subscribe(moqL2QObserver.Object);
        subscribedL3Observer = dummyLevel3MessageDeserializer.Subscribe(moqL3QObserver.Object);

        moqTickInstantSubscribers.Verify();
        moqLevel1Subscribers.Verify();
        moqLevel2Subscribers.Verify();
        moqLevel3Subscribers.Verify();
    }

    [TestMethod]
    public void SubscribedObserver_DisposeSubscription_SyncLockProtectsRemovingObserver()
    {
        NewPQQuoteDeserializer_Subscribe_SyncLockProtectsAddingObserver();
        moqTickInstantSubscribers
            .Setup(lo => lo.Remove(moqTickInstantObserver.Object))
            .Callback(() => { Assert.IsTrue(haveCalledAcquire); })
            .Returns(true).Verifiable();
        moqLevel1Subscribers
            .Setup(lo => lo.Remove(moqL1QObserver.Object))
            .Callback(() => { Assert.IsTrue(haveCalledAcquire); })
            .Returns(true).Verifiable();
        moqLevel2Subscribers
            .Setup(lo => lo.Remove(moqL2QObserver.Object))
            .Callback(() => { Assert.IsTrue(haveCalledAcquire); })
            .Returns(true).Verifiable();
        moqLevel3Subscribers
            .Setup(lo => lo.Remove(moqL3QObserver.Object))
            .Callback(() => { Assert.IsTrue(haveCalledAcquire); })
            .Returns(true).Verifiable();

        subscribedTickInstantObserver.Dispose();
        subscribedL1Observer.Dispose();
        subscribedL2Observer.Dispose();
        subscribedL3Observer.Dispose();

        moqTickInstantSubscribers.Verify();
        moqLevel1Subscribers.Verify();
        moqLevel2Subscribers.Verify();
        moqLevel3Subscribers.Verify();
    }

    [TestMethod]
    public void EmptyTickInstant_UpdateInstant_SetsDispatcherContextDetails()
    {
        var expectedSequenceId = 101u;
        var expectedTickInstant = new PQPublishableTickInstant(sourceTickerInfo)
        {
            SingleTickValue              = 0.78568m, SourceTime = new DateTime(2017, 07, 01, 19, 35, 00)
          , FeedMarketConnectivityStatus = FeedConnectivityStatusFlags.IsAdapterReplay
          , PQSequenceId                 = expectedSequenceId
        };

        var quoteSerializer = new PQQuoteSerializer(PQMessageFlags.Snapshot);
        var amountWritten   = quoteSerializer.Serialize(readWriteBuffer, expectedTickInstant);
        socketBufferReadContext.MessageHeader              = new MessageHeader(1, 0, 0, (uint)amountWritten);
        socketBufferReadContext.EncodedBuffer!.WriteCursor = BufferReadWriteOffset + amountWritten;
        socketBufferReadContext.LastWriteLength            = amountWritten;

        var actualTickInstant = new PQPublishableTickInstant(sourceTickerInfo);

        socketBufferReadContext.EncodedBuffer.ReadCursor = BufferReadWriteOffset + MessageHeaderByteSize;
        dummyTickInstantDeserializer.InvokeUpdateQuote(socketBufferReadContext, actualTickInstant, expectedSequenceId);

        Assert.AreEqual(socketBufferReadContext.DetectTimestamp, actualTickInstant.ClientReceivedTime);
        Assert.AreEqual(socketBufferReadContext.ReceivingTimestamp, actualTickInstant.InboundSocketReceivingTime);
        Assert.AreEqual(socketBufferReadContext.DeserializerTime, actualTickInstant.InboundProcessedTime);
        Assert.AreEqual(expectedSequenceId, actualTickInstant.PQSequenceId);
        Assert.AreEqual(expectedTickInstant.SingleTickValue, actualTickInstant.SingleTickValue);
        Assert.AreEqual(expectedTickInstant.SourceTime, actualTickInstant.SourceTime);
        Assert.AreEqual(expectedTickInstant.FeedMarketConnectivityStatus, actualTickInstant.FeedMarketConnectivityStatus);
    }

    [TestMethod]
    public void EmptyQuoteLvl1Quote_UpdateQuote_SetsDispatcherContextDetails()
    {
        var expectedL1Quote = new PQPublishableLevel1Quote(sourceTickerInfo)
        {
            FeedMarketConnectivityStatus = FeedConnectivityStatusFlags.IsAdapterReplay
          , Executable                   = true
          , SourceAskTime                = new DateTime(2017, 07, 01, 21, 25, 30)
          , SourceBidTime                = new DateTime(2017, 07, 01, 19, 27, 00)
          , AdapterReceivedTime          = new DateTime(2017, 07, 01, 19, 27, 30)
          , AdapterSentTime              = new DateTime(2017, 07, 01, 19, 27, 39), BidPriceTop = 0.79324m, AskPriceTop = 0.79326m
        };

        var quoteSerializer = new PQQuoteSerializer(PQMessageFlags.Snapshot);
        var amountWritten   = quoteSerializer.Serialize(readWriteBuffer, expectedL1Quote);
        socketBufferReadContext.MessageHeader              = new MessageHeader(1, 0, 0, (uint)amountWritten);
        socketBufferReadContext.EncodedBuffer!.WriteCursor = BufferReadWriteOffset + amountWritten;
        socketBufferReadContext.LastWriteLength            = amountWritten;

        var actualL1Quote = new PQPublishableLevel1Quote(sourceTickerInfo);

        var expectedSequenceId = 102u;
        socketBufferReadContext.EncodedBuffer.ReadCursor = BufferReadWriteOffset + PQQuoteMessageHeader.HeaderSize;
        dummyTickInstantDeserializer.InvokeUpdateQuote(socketBufferReadContext, actualL1Quote, expectedSequenceId);

        Assert.AreEqual(expectedL1Quote.Executable, actualL1Quote.Executable);
        Assert.AreEqual(expectedL1Quote.SourceBidTime, actualL1Quote.SourceBidTime);
        Assert.AreEqual(expectedL1Quote.SourceAskTime, actualL1Quote.SourceAskTime);
        Assert.AreEqual(expectedL1Quote.AdapterReceivedTime, actualL1Quote.AdapterReceivedTime);
        Assert.AreEqual(expectedL1Quote.AdapterSentTime, actualL1Quote.AdapterSentTime);
        Assert.AreEqual(expectedL1Quote.BidPriceTop, actualL1Quote.BidPriceTop);
        Assert.AreEqual(expectedL1Quote.AskPriceTop, actualL1Quote.AskPriceTop);
    }

    [TestMethod]
    public void EmptyQuoteLvl2Quote_UpdateQuote_SetsDispatcherContextDetails()
    {
        var expectedL2Quote = new PQPublishableLevel2Quote(sourceTickerInfo);

        var numLayers = expectedL2Quote.BidBook.Capacity;
        Assert.IsTrue(numLayers >= 20);
        for (var i = 0; i < numLayers; i++)
        {
            var bidBookLayer = expectedL2Quote.BidBook[i]!;
            bidBookLayer.Price  = 0.791905m - 0.00001m * i;
            bidBookLayer.Volume = 30000 + 10000 * i;
            var askBookLayer = expectedL2Quote.AskBook[i]!;
            askBookLayer.Price  = 0.791906m + 0.00001m * i;
            askBookLayer.Volume = 30000 + 10000 * i;
        }

        var quoteSerializer = new PQQuoteSerializer(PQMessageFlags.Snapshot);
        var amountWritten   = quoteSerializer.Serialize(readWriteBuffer, expectedL2Quote);
        socketBufferReadContext.MessageHeader              = new MessageHeader(1, 0, 0, (uint)amountWritten);
        socketBufferReadContext.EncodedBuffer!.WriteCursor = BufferReadWriteOffset + amountWritten;
        socketBufferReadContext.LastWriteLength            = amountWritten;

        var actualL2Quote = new PQPublishableLevel2Quote(sourceTickerInfo);

        var expectedSequenceId = 102u;
        socketBufferReadContext.EncodedBuffer.ReadCursor = BufferReadWriteOffset + PQQuoteMessageHeader.HeaderSize;
        dummyTickInstantDeserializer.InvokeUpdateQuote(socketBufferReadContext, actualL2Quote, expectedSequenceId);

        for (var i = 0; i < numLayers; i++)
        {
            var expectedBidBookLayer = expectedL2Quote.BidBook[i]!;
            var actualBidBookLayer   = actualL2Quote.BidBook[i]!;
            var expectedAskBookLayer = expectedL2Quote.AskBook[i]!;
            var actualAskBookLayer   = actualL2Quote.AskBook[i]!;

            Assert.AreEqual(expectedBidBookLayer.Price, actualBidBookLayer.Price);
            Assert.AreEqual(expectedBidBookLayer.Volume, actualBidBookLayer.Volume);
            Assert.AreEqual(expectedAskBookLayer.Price, actualAskBookLayer.Price);
            Assert.AreEqual(expectedAskBookLayer.Volume, actualAskBookLayer.Volume);
        }
    }

    [TestMethod]
    public void EmptyQuoteLvl3Quote_UpdateQuote_SetsDispatcherContextDetails()
    {
        var expectedL3Quote = new PQPublishableLevel3Quote(sourceTickerInfo);

        var deepestPossibleLayerIndex = expectedL3Quote.BidBook.Capacity;
        Assert.IsTrue(deepestPossibleLayerIndex >= 20);
        var toggleGivenBool = false;
        var togglePaidBool  = true;
        for (var i = 0; i < deepestPossibleLayerIndex; i++)
            if (i < QuoteSequencedTestDataBuilder.GeneratedNumberOfLastTrades &&
                expectedL3Quote.OnTickLastTraded![i] is PQLastExternalCounterPartyTrade lastTradeInfo)
            {
                lastTradeInfo.TradePrice  = 0.76591m;
                lastTradeInfo.TradeTime   = new DateTime(2017, 07, 02, 13, 40, 11);
                lastTradeInfo.TradeVolume = 2000000;
                lastTradeInfo.WasGiven    = toggleGivenBool = !toggleGivenBool;
                lastTradeInfo.WasPaid     = togglePaidBool  = !togglePaidBool;
                lastTradeInfo.ExternalTraderName  = "NewTraderName " + i;
            }

        var quoteSerializer = new PQQuoteSerializer(PQMessageFlags.Snapshot);
        var amountWritten   = quoteSerializer.Serialize(readWriteBuffer, expectedL3Quote);
        socketBufferReadContext.MessageHeader              = new MessageHeader(1, 0, 0, (uint)amountWritten);
        socketBufferReadContext.EncodedBuffer!.WriteCursor = BufferReadWriteOffset + amountWritten;
        socketBufferReadContext.LastWriteLength            = amountWritten;

        var actualL3Quote = new PQPublishableLevel3Quote(sourceTickerInfo);

        var expectedSequenceId = 102u;
        socketBufferReadContext.EncodedBuffer.ReadCursor = BufferReadWriteOffset + PQQuoteMessageHeader.HeaderSize;
        dummyTickInstantDeserializer.InvokeUpdateQuote(socketBufferReadContext, actualL3Quote, expectedSequenceId);

        for (var i = 0; i < deepestPossibleLayerIndex && i < QuoteSequencedTestDataBuilder.GeneratedNumberOfLastTrades; i++)
        {
            var expectedLastTradeInfo = expectedL3Quote.OnTickLastTraded![i] as PQLastExternalCounterPartyTrade;
            Assert.IsNotNull(expectedLastTradeInfo);
            var actualLastTradeInfo = actualL3Quote.OnTickLastTraded![i] as PQLastExternalCounterPartyTrade;
            Assert.IsNotNull(actualLastTradeInfo);
            Assert.AreEqual(expectedLastTradeInfo.TradePrice, actualLastTradeInfo.TradePrice);
            Assert.AreEqual(expectedLastTradeInfo.TradeTime, actualLastTradeInfo.TradeTime);
            Assert.AreEqual(expectedLastTradeInfo.TradeVolume, actualLastTradeInfo.TradeVolume);
            Assert.AreEqual(expectedLastTradeInfo.WasGiven, actualLastTradeInfo.WasGiven);
            Assert.AreEqual(expectedLastTradeInfo.WasPaid, actualLastTradeInfo.WasPaid);
            Assert.AreEqual(expectedLastTradeInfo.ExternalTraderName, actualLastTradeInfo.ExternalTraderName);
        }
    }

    [TestMethod]
    public void SubscribedTickerQuoteHasChanges_PushQuoteToSubscribers_LatencyTraceSyncLocksUpdatedQuote()
    {
        SetupObserversAndSyncLock();

        SetupQuoteChanges(new DateTime(2017, 07, 15, 22, 06, 25));

        var preTestTimeContext = TimeContext.Provider;
        try
        {
            var moqTimeContext = new Mock<ITimeContext>();
            TimeContext.Provider = moqTimeContext.Object;

            var expectedDateTime          = new DateTime(2017, 07, 15, 21, 35, 14);
            var expectedPublicationStatus = FeedSyncStatus.Good;

            moqTimeContext.SetupGet(tc => tc.UtcNow).Returns(expectedDateTime);

            moqTickInstantObserver
                .Setup(o => o.OnNext(dummyTickInstantDeserializer.PublishedQuote))
                .Callback<IPQPublishableTickInstant>
                    (pq =>
                     {
                         Assert.IsTrue(haveCalledAcquire);
                         Assert.IsTrue(pq.HasUpdates);
                         Assert.AreEqual(expectedDateTime, pq.SubscriberDispatchedTime);
                         Assert.AreEqual(expectedPublicationStatus, pq.FeedSyncStatus);
                     }
                    ).Verifiable();
            moqL1QObserver
                .Setup(o => o.OnNext(dummyLevel1MessageDeserializer.PublishedQuote))
                .Callback<IPQPublishableLevel1Quote>
                    (pq =>
                     {
                         Assert.IsTrue(haveCalledAcquire);
                         Assert.IsTrue(pq.HasUpdates);
                         Assert.AreEqual(expectedDateTime, pq.SubscriberDispatchedTime);
                         Assert.AreEqual(expectedPublicationStatus, pq.FeedSyncStatus);
                     }
                    ).Verifiable();
            moqL2QObserver
                .Setup(o => o.OnNext(dummyLevel2MessageDeserializer.PublishedQuote))
                .Callback<IPQPublishableLevel2Quote>
                    (pq =>
                     {
                         Assert.IsTrue(haveCalledAcquire);
                         Assert.IsTrue(pq.HasUpdates);
                         Assert.AreEqual(expectedDateTime, pq.SubscriberDispatchedTime);
                         Assert.AreEqual(expectedPublicationStatus, pq.FeedSyncStatus);
                     }
                    ).Verifiable();
            moqL3QObserver
                .Setup(o => o.OnNext(dummyLevel3MessageDeserializer.PublishedQuote))
                .Callback<IPQPublishableLevel3Quote>
                    (pq =>
                     {
                         Assert.IsTrue(haveCalledAcquire);
                         Assert.IsTrue(pq.HasUpdates);
                         Assert.AreEqual(expectedDateTime, pq.SubscriberDispatchedTime);
                         Assert.AreEqual(expectedPublicationStatus, pq.FeedSyncStatus);
                     }
                    ).Verifiable();

            const string expectedTicker = "TestTicker";
            const string expectedSource = "TestSource";

            moqUniqueSrcTkrId.As<ISourceTickerId>().Verify(sti => sti.InstrumentName, Times.AtLeast(4));
            moqUniqueSrcTkrId.As<ISourceTickerId>().Verify(sti => sti.SourceName, Times.AtLeast(4));
            moqUniqueSrcTkrId.As<ISourceTickerInfo>().SetupGet(sti => sti.InstrumentName).Returns(expectedTicker);
            moqUniqueSrcTkrId.As<ISourceTickerInfo>().SetupGet(sti => sti.SourceName).Returns(expectedSource);

            subscribedTickInstantObserver = dummyTickInstantDeserializer.Subscribe(moqTickInstantObserver.Object);

            subscribedL1Observer = dummyLevel1MessageDeserializer.Subscribe(moqL1QObserver.Object);
            subscribedL2Observer = dummyLevel2MessageDeserializer.Subscribe(moqL2QObserver.Object);
            subscribedL3Observer = dummyLevel3MessageDeserializer.Subscribe(moqL3QObserver.Object);

            dummyTickInstantDeserializer.InvokePushQuoteToSubscribers(expectedPublicationStatus);
            dummyLevel1MessageDeserializer.InvokePushQuoteToSubscribers(expectedPublicationStatus);
            dummyLevel2MessageDeserializer.InvokePushQuoteToSubscribers(expectedPublicationStatus);
            dummyLevel3MessageDeserializer.InvokePushQuoteToSubscribers(expectedPublicationStatus);


            moqPerfLogger.Verify(pl => pl.Enabled, Times.AtLeast(8));
            moqUniqueSrcTkrId.As<ISourceTickerInfo>().Verify(sti => sti.InstrumentName, Times.AtLeast(4));
            moqUniqueSrcTkrId.As<ISourceTickerInfo>().Verify(sti => sti.SourceName, Times.AtLeast(4));
            moqUniqueSrcTkrId.As<ISourceTickerId>().Verify(sti => sti.InstrumentName, Times.AtLeast(4));
            moqUniqueSrcTkrId.As<ISourceTickerId>().Verify(sti => sti.SourceName, Times.AtLeast(4));
            moqPerfLogger.Verify(pl => pl.Add("Ticker", expectedTicker), Times.AtLeast(4));
            moqPerfLogger.Verify(pl => pl.Add("Source", expectedSource), Times.AtLeast(4));
            moqPerfLoggerPool.Verify(plp => plp.StartNewTrace(), Times.AtLeast(4));
            moqPerfLoggerPool.Verify(plp => plp.StartNewTrace(), Times.AtLeast(4));
            moqTickInstantObserver.Verify();
            moqL1QObserver.Verify();
            moqL2QObserver.Verify();
            moqL3QObserver.Verify();

            var moqDispatchPerfLogger = new Mock<IPerfLogger>();
            moqDispatchPerfLogger.Setup(pl => pl.Add(SocketDataLatencyLogger.BeforePublish)).Verifiable();
            SetupQuoteChanges(new DateTime(2017, 07, 15, 23, 39, 26));

            dummyTickInstantDeserializer.InvokePushQuoteToSubscribers
                (expectedPublicationStatus, moqDispatchPerfLogger.Object);
            dummyLevel1MessageDeserializer.InvokePushQuoteToSubscribers
                (expectedPublicationStatus, moqDispatchPerfLogger.Object);
            dummyLevel2MessageDeserializer.InvokePushQuoteToSubscribers
                (expectedPublicationStatus, moqDispatchPerfLogger.Object);
            dummyLevel3MessageDeserializer.InvokePushQuoteToSubscribers
                (expectedPublicationStatus, moqDispatchPerfLogger.Object);

            moqDispatchPerfLogger.Verify(pl => pl.Add(SocketDataLatencyLogger.BeforePublish), Times.Exactly(4));
        }
        finally
        {
            TimeContext.Provider = preTestTimeContext;
        }
    }

    [TestMethod]
    public void NoSubscribedObservers_PushQuoteToSubscribers_ReturnsDoesNothing()
    {
        SetupObserversAndSyncLock();

        SetupQuoteChanges(new DateTime(2017, 07, 15, 22, 06, 25));

        var expectedPublicationStatus = FeedSyncStatus.Good;

        dummyTickInstantDeserializer.InvokePushQuoteToSubscribers(expectedPublicationStatus);
        dummyLevel1MessageDeserializer.InvokePushQuoteToSubscribers(expectedPublicationStatus);
        dummyLevel2MessageDeserializer.InvokePushQuoteToSubscribers(expectedPublicationStatus);
        dummyLevel3MessageDeserializer.InvokePushQuoteToSubscribers(expectedPublicationStatus);
    }

    [TestMethod]
    public void SubscribersNoQuoteUpdates_PushQuoteToSubscribers_SetsPubStatusDoesNotPush()
    {
        SetupObserversAndSyncLock();

        var expectedPublicationStatus = FeedSyncStatus.Good;

        moqTickInstantObserver.Setup(o => o.OnNext(dummyTickInstantDeserializer.PublishedQuote))
                              .Callback(() => { Assert.Fail("Should Never Get Here"); });
        moqL1QObserver.Setup(o => o.OnNext(dummyLevel1MessageDeserializer.PublishedQuote))
                      .Callback(() => { Assert.Fail("Should Never Get Here"); });
        moqL2QObserver.Setup(o => o.OnNext(dummyLevel2MessageDeserializer.PublishedQuote))
                      .Callback(() => { Assert.Fail("Should Never Get Here"); });
        moqL3QObserver.Setup(o => o.OnNext(dummyLevel3MessageDeserializer.PublishedQuote))
                      .Callback(() => { Assert.Fail("Should Never Get Here"); });

        const string expectedTicker = "TestTicker";
        const string expectedSource = "TestSource";

        moqUniqueSrcTkrId.SetupGet(sti => sti.InstrumentName).Returns(expectedTicker);
        moqUniqueSrcTkrId.SetupGet(sti => sti.SourceName).Returns(expectedSource);

        subscribedTickInstantObserver = dummyTickInstantDeserializer.Subscribe(moqTickInstantObserver.Object);

        subscribedL1Observer = dummyLevel1MessageDeserializer.Subscribe(moqL1QObserver.Object);
        subscribedL2Observer = dummyLevel2MessageDeserializer.Subscribe(moqL2QObserver.Object);
        subscribedL3Observer = dummyLevel3MessageDeserializer.Subscribe(moqL3QObserver.Object);

        dummyTickInstantDeserializer.InvokePushQuoteToSubscribers(expectedPublicationStatus);
        dummyLevel1MessageDeserializer.InvokePushQuoteToSubscribers(expectedPublicationStatus);
        dummyLevel2MessageDeserializer.InvokePushQuoteToSubscribers(expectedPublicationStatus);
        dummyLevel3MessageDeserializer.InvokePushQuoteToSubscribers(expectedPublicationStatus);

        moqPerfLoggerPool.Verify(plp => plp.StartNewTrace(), Times.Exactly(4));
        moqPerfLoggerPool.Verify(plp => plp.StartNewTrace(), Times.Exactly(4));
        Assert.AreEqual(expectedPublicationStatus, dummyTickInstantDeserializer.PublishedQuote.FeedSyncStatus);
        Assert.AreEqual(expectedPublicationStatus, dummyLevel1MessageDeserializer.PublishedQuote.FeedSyncStatus);
        Assert.AreEqual(expectedPublicationStatus, dummyLevel2MessageDeserializer.PublishedQuote.FeedSyncStatus);
        Assert.AreEqual(expectedPublicationStatus, dummyLevel3MessageDeserializer.PublishedQuote.FeedSyncStatus);
    }

    private void SetupObserversAndSyncLock()
    {
        moqSyncLock       = new Mock<ISyncLock>();
        haveCalledAcquire = false;
        moqSyncLock.Setup(sl => sl.Acquire()).Callback(() => { haveCalledAcquire = true; });
        moqSyncLock.Setup(sl => sl.Release()).Callback(() => { haveCalledAcquire = false; });

        NonPublicInvocator.SetInstanceField
            (dummyTickInstantDeserializer.PublishedQuote, "SyncLock", moqSyncLock.Object);
        NonPublicInvocator.SetInstanceField
            (dummyLevel1MessageDeserializer.PublishedQuote, "SyncLock", moqSyncLock.Object);
        NonPublicInvocator.SetInstanceField
            (dummyLevel2MessageDeserializer.PublishedQuote, "SyncLock", moqSyncLock.Object);
        NonPublicInvocator.SetInstanceField
            (dummyLevel3MessageDeserializer.PublishedQuote, "SyncLock", moqSyncLock.Object);

        moqTickInstantObserver = new Mock<IObserver<IPQPublishableTickInstant>>();

        moqL1QObserver = new Mock<IObserver<IPQPublishableLevel1Quote>>();
        moqL2QObserver = new Mock<IObserver<IPQPublishableLevel2Quote>>();
        moqL3QObserver = new Mock<IObserver<IPQPublishableLevel3Quote>>();
    }

    private void SetupQuoteChanges(DateTime newDateTime)
    {
        dummyTickInstantDeserializer.PublishedQuote.SourceTime = newDateTime;
        dummyLevel1MessageDeserializer.PublishedQuote.SourceTime = newDateTime;
        dummyLevel2MessageDeserializer.PublishedQuote.SourceTime = newDateTime;
        dummyLevel3MessageDeserializer.PublishedQuote.SourceTime = newDateTime;
    }

    private class DummyPQMessageDeserializerBase<T>(ISourceTickerInfo identifier) : PQMessageDeserializerBase<T>(identifier)
        where T : class, IPQPublishableTickInstant
    {
        public override T Deserialize(ISerdeContext readContext) => throw new NotImplementedException();

        public void InvokeOnReceivedUpdate(IPQMessageDeserializer quoteDeserializer)
        {
            OnReceivedUpdate(quoteDeserializer);
        }

        public void InvokeOnSyncOk(IPQMessageDeserializer quoteDeserializer)
        {
            OnSyncOk(quoteDeserializer);
        }

        public void InvokeOnOutOfSync(IPQMessageDeserializer quoteDeserializer)
        {
            OnOutOfSync(quoteDeserializer);
        }

        public void InvokeUpdateQuote(SocketBufferReadContext socketBufferReadContext, T ent, uint sequenceId)
        {
            UpdateEntity(socketBufferReadContext, ent, sequenceId);
        }

        public void InvokePushQuoteToSubscribers(FeedSyncStatus syncStatus, IPerfLogger? detectionToPublishLatencyTraceLogger = null)
        {
            PushQuoteToSubscribers(syncStatus, detectionToPublishLatencyTraceLogger);
        }

        public override IMessageDeserializer Clone() => this;
    }
}
