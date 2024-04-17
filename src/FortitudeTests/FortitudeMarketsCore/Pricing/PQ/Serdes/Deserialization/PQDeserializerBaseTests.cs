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
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.LastTraded;
using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.LastTraded;
using FortitudeMarketsCore.Pricing.PQ.Serdes;
using FortitudeMarketsCore.Pricing.PQ.Serdes.Deserialization;
using FortitudeMarketsCore.Pricing.PQ.Serdes.Serialization;
using Moq;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serdes.Deserialization;

[TestClass]
public class PQDeserializerBaseTests
{
    private const int MessageHeaderByteSize = PQQuoteMessageHeader.HeaderSize;
    private const int BufferReadWriteOffset = 6;
    private DummyPQQuoateDeserializerBase<IPQLevel0Quote> dummyLevel0QuoteDeserializer = null!;
    private DummyPQQuoateDeserializerBase<IPQLevel1Quote> dummyLevel1QuoteDeserializer = null!;
    private DummyPQQuoateDeserializerBase<IPQLevel2Quote> dummyLevel2QuoteDeserializer = null!;
    private DummyPQQuoateDeserializerBase<IPQLevel3Quote> dummyLevel3QuoteDeserializer = null!;
    private bool haveCalledAcquire;
    private Mock<IObserver<IPQLevel0Quote>> moqL0QObserver = null!;
    private Mock<IObserver<IPQLevel1Quote>> moqL1QObserver = null!;
    private Mock<IObserver<IPQLevel2Quote>> moqL2QObserver = null!;
    private Mock<IObserver<IPQLevel3Quote>> moqL3QObserver = null!;
    private Mock<IList<IObserver<IPQLevel0Quote>>> moqLevel0Subscribers = null!;
    private Mock<IList<IObserver<IPQLevel1Quote>>> moqLevel1Subscribers = null!;
    private Mock<IList<IObserver<IPQLevel2Quote>>> moqLevel2Subscribers = null!;
    private Mock<IList<IObserver<IPQLevel3Quote>>> moqLevel3Subscribers = null!;
    private Mock<IPerfLogger> moqPerfLogger = null!;
    private Mock<IPerfLoggerPool> moqPerfLoggerPool = null!;
    private Mock<IPQDeserializer> moqQuoteDeserializer = null!;
    private Mock<ISyncLock> moqSyncLock = null!;
    private Mock<ISourceTickerQuoteInfo> moqUniqueSrcTkrId = null!;
    private ReadWriteBuffer readWriteBuffer = null!;
    private SocketBufferReadContext socketBufferReadContext = null!;
    private SourceTickerQuoteInfo sourceTickerQuoteInfo = null!;
    private IDisposable subscribedL0Observer = null!;
    private IDisposable subscribedL1Observer = null!;
    private IDisposable subscribedL2Observer = null!;
    private IDisposable subscribedL3Observer = null!;

    [TestInitialize]
    public void SetUp()
    {
        moqUniqueSrcTkrId = new Mock<ISourceTickerQuoteInfo>();
        dummyLevel0QuoteDeserializer = new DummyPQQuoateDeserializerBase<IPQLevel0Quote>(moqUniqueSrcTkrId.Object);
        dummyLevel1QuoteDeserializer = new DummyPQQuoateDeserializerBase<IPQLevel1Quote>(moqUniqueSrcTkrId.Object);
        dummyLevel2QuoteDeserializer = new DummyPQQuoateDeserializerBase<IPQLevel2Quote>(moqUniqueSrcTkrId.Object);
        dummyLevel3QuoteDeserializer = new DummyPQQuoateDeserializerBase<IPQLevel3Quote>(moqUniqueSrcTkrId.Object);
        moqQuoteDeserializer = new Mock<IPQDeserializer>();

        readWriteBuffer = new ReadWriteBuffer(new byte[9000]);
        socketBufferReadContext = new SocketBufferReadContext
        {
            DetectTimestamp = new DateTime(2017, 07, 01, 18, 59, 22)
            , ReceivingTimestamp = new DateTime(2017, 07, 01, 19, 03, 22)
            , DeserializerTime = new DateTime(2017, 07, 01, 19, 03, 52), EncodedBuffer = readWriteBuffer
            , MessageHeader = new MessageHeader(1, 0, 0, 1)
        };
        readWriteBuffer.ReadCursor = BufferReadWriteOffset;
        readWriteBuffer.WriteCursor = BufferReadWriteOffset;

        sourceTickerQuoteInfo = new SourceTickerQuoteInfo(ushort.MaxValue, "TestSource", ushort.MaxValue, "TestTicker",
            20, 0.00001m, 30000m, 50000000m, 1000m, 1, LayerFlags.Volume | LayerFlags.Price,
            LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName |
            LastTradedFlags.LastTradedVolume | LastTradedFlags.LastTradedTime);

        moqPerfLoggerPool = new Mock<IPerfLoggerPool>();

        NonPublicInvocator.SetStaticField(dummyLevel0QuoteDeserializer,
            "PublishPQQuoteDeserializerLatencyTraceLoggerPool", moqPerfLoggerPool.Object);
        NonPublicInvocator.SetStaticField(dummyLevel1QuoteDeserializer,
            "PublishPQQuoteDeserializerLatencyTraceLoggerPool", moqPerfLoggerPool.Object);
        NonPublicInvocator.SetStaticField(dummyLevel2QuoteDeserializer,
            "PublishPQQuoteDeserializerLatencyTraceLoggerPool", moqPerfLoggerPool.Object);
        NonPublicInvocator.SetStaticField(dummyLevel3QuoteDeserializer,
            "PublishPQQuoteDeserializerLatencyTraceLoggerPool", moqPerfLoggerPool.Object);

        moqPerfLogger = new Mock<IPerfLogger>();
        moqPerfLoggerPool.Setup(ltcslp => ltcslp.StartNewTrace())
            .Returns(moqPerfLogger.Object).Verifiable();
        moqPerfLoggerPool.Setup(ltcslp => ltcslp.StopTrace(moqPerfLogger.Object))
            .Verifiable();
        moqPerfLogger.SetupGet(ltcsl => ltcsl.Enabled).Returns(true);
    }

    [TestCleanup]
    public void TearDown()
    {
        var realInstance =
            PerfLoggingPoolFactory.Instance.GetLatencyTracingLoggerPool("clientCallback",
                TimeSpan.FromMilliseconds(10), typeof(UserCallback));

        NonPublicInvocator.SetStaticField(dummyLevel0QuoteDeserializer,
            "PublishPQQuoteDeserializerLatencyTraceLoggerPool", realInstance);
        NonPublicInvocator.SetStaticField(dummyLevel1QuoteDeserializer,
            "PublishPQQuoteDeserializerLatencyTraceLoggerPool", realInstance);
        NonPublicInvocator.SetStaticField(dummyLevel2QuoteDeserializer,
            "PublishPQQuoteDeserializerLatencyTraceLoggerPool", realInstance);
        NonPublicInvocator.SetStaticField(dummyLevel3QuoteDeserializer,
            "PublishPQQuoteDeserializerLatencyTraceLoggerPool", realInstance);
    }

    [TestMethod]
    public void NewPQQuoteDeserializer_New_SetsSourceTickerIdentifer()
    {
        Assert.IsNotNull(dummyLevel0QuoteDeserializer.PublishedQuote);
        Assert.IsNotNull(dummyLevel1QuoteDeserializer.PublishedQuote);
        Assert.IsNotNull(dummyLevel2QuoteDeserializer.PublishedQuote);
        Assert.IsNotNull(dummyLevel3QuoteDeserializer.PublishedQuote);
    }

    [TestMethod]
    public void RegisteredCallback_InvokeOnReceivedUpdate_CallsCallback()
    {
        var haveCalledPQLevel0QuoteCallback = false;
        var haveCalledPQLevel1QuoteCallback = false;
        var haveCalledPQLevel2QuoteCallback = false;
        var haveCalledPQLevel3QuoteCallback = false;
        dummyLevel0QuoteDeserializer.ReceivedUpdate += deserializer => { haveCalledPQLevel0QuoteCallback = true; };
        dummyLevel1QuoteDeserializer.ReceivedUpdate += deserializer => { haveCalledPQLevel1QuoteCallback = true; };
        dummyLevel2QuoteDeserializer.ReceivedUpdate += deserializer => { haveCalledPQLevel2QuoteCallback = true; };
        dummyLevel3QuoteDeserializer.ReceivedUpdate += deserializer => { haveCalledPQLevel3QuoteCallback = true; };

        dummyLevel0QuoteDeserializer.InvokeOnReceivedUpdate(moqQuoteDeserializer.Object);
        dummyLevel1QuoteDeserializer.InvokeOnReceivedUpdate(moqQuoteDeserializer.Object);
        dummyLevel2QuoteDeserializer.InvokeOnReceivedUpdate(moqQuoteDeserializer.Object);
        dummyLevel3QuoteDeserializer.InvokeOnReceivedUpdate(moqQuoteDeserializer.Object);

        Assert.IsTrue(haveCalledPQLevel0QuoteCallback);
        Assert.IsTrue(haveCalledPQLevel1QuoteCallback);
        Assert.IsTrue(haveCalledPQLevel2QuoteCallback);
        Assert.IsTrue(haveCalledPQLevel3QuoteCallback);
    }

    [TestMethod]
    public void RegisteredCallback_InvokeOnSyncOk_CallsCallback()
    {
        var haveCalledPQLevel0QuoteCallback = false;
        var haveCalledPQLevel1QuoteCallback = false;
        var haveCalledPQLevel2QuoteCallback = false;
        var haveCalledPQLevel3QuoteCallback = false;
        dummyLevel0QuoteDeserializer.SyncOk += deserializer => { haveCalledPQLevel0QuoteCallback = true; };
        dummyLevel1QuoteDeserializer.SyncOk += deserializer => { haveCalledPQLevel1QuoteCallback = true; };
        dummyLevel2QuoteDeserializer.SyncOk += deserializer => { haveCalledPQLevel2QuoteCallback = true; };
        dummyLevel3QuoteDeserializer.SyncOk += deserializer => { haveCalledPQLevel3QuoteCallback = true; };

        dummyLevel0QuoteDeserializer.InvokeOnSyncOk(moqQuoteDeserializer.Object);
        dummyLevel1QuoteDeserializer.InvokeOnSyncOk(moqQuoteDeserializer.Object);
        dummyLevel2QuoteDeserializer.InvokeOnSyncOk(moqQuoteDeserializer.Object);
        dummyLevel3QuoteDeserializer.InvokeOnSyncOk(moqQuoteDeserializer.Object);

        Assert.IsTrue(haveCalledPQLevel0QuoteCallback);
        Assert.IsTrue(haveCalledPQLevel1QuoteCallback);
        Assert.IsTrue(haveCalledPQLevel2QuoteCallback);
        Assert.IsTrue(haveCalledPQLevel3QuoteCallback);
    }

    [TestMethod]
    public void RegisteredCallback_InvokeOnOutOfSync_CallsCallback()
    {
        var haveCalledPQLevel0QuoteCallback = false;
        var haveCalledPQLevel1QuoteCallback = false;
        var haveCalledPQLevel2QuoteCallback = false;
        var haveCalledPQLevel3QuoteCallback = false;
        dummyLevel0QuoteDeserializer.OutOfSync += deserializer => { haveCalledPQLevel0QuoteCallback = true; };
        dummyLevel1QuoteDeserializer.OutOfSync += deserializer => { haveCalledPQLevel1QuoteCallback = true; };
        dummyLevel2QuoteDeserializer.OutOfSync += deserializer => { haveCalledPQLevel2QuoteCallback = true; };
        dummyLevel3QuoteDeserializer.OutOfSync += deserializer => { haveCalledPQLevel3QuoteCallback = true; };

        dummyLevel0QuoteDeserializer.InvokeOnOutOfSync(moqQuoteDeserializer.Object);
        dummyLevel1QuoteDeserializer.InvokeOnOutOfSync(moqQuoteDeserializer.Object);
        dummyLevel2QuoteDeserializer.InvokeOnOutOfSync(moqQuoteDeserializer.Object);
        dummyLevel3QuoteDeserializer.InvokeOnOutOfSync(moqQuoteDeserializer.Object);

        Assert.IsTrue(haveCalledPQLevel0QuoteCallback);
        Assert.IsTrue(haveCalledPQLevel1QuoteCallback);
        Assert.IsTrue(haveCalledPQLevel2QuoteCallback);
        Assert.IsTrue(haveCalledPQLevel3QuoteCallback);
    }

    [TestMethod]
    public void NewPQQuoteDeserializer_Subscribe_SyncLockProtectsAddingObserver()
    {
        SetupObserversAndSyncLock();

        moqLevel0Subscribers = new Mock<IList<IObserver<IPQLevel0Quote>>>();
        moqLevel0Subscribers.Setup(lo => lo.Add(moqL0QObserver.Object)).Callback(() => { Assert.IsTrue(haveCalledAcquire); }).Verifiable();
        moqLevel1Subscribers = new Mock<IList<IObserver<IPQLevel1Quote>>>();
        moqLevel1Subscribers.Setup(lo => lo.Add(moqL1QObserver.Object)).Callback(() => { Assert.IsTrue(haveCalledAcquire); }).Verifiable();
        moqLevel2Subscribers = new Mock<IList<IObserver<IPQLevel2Quote>>>();
        moqLevel2Subscribers.Setup(lo => lo.Add(moqL2QObserver.Object)).Callback(() => { Assert.IsTrue(haveCalledAcquire); }).Verifiable();
        moqLevel3Subscribers = new Mock<IList<IObserver<IPQLevel3Quote>>>();
        moqLevel3Subscribers.Setup(lo => lo.Add(moqL3QObserver.Object)).Callback(() => { Assert.IsTrue(haveCalledAcquire); }).Verifiable();


        NonPublicInvocator.SetInstanceField(dummyLevel0QuoteDeserializer,
            "Subscribers", moqLevel0Subscribers.Object);
        NonPublicInvocator.SetInstanceField(dummyLevel1QuoteDeserializer,
            "Subscribers", moqLevel1Subscribers.Object);
        NonPublicInvocator.SetInstanceField(dummyLevel2QuoteDeserializer,
            "Subscribers", moqLevel2Subscribers.Object);
        NonPublicInvocator.SetInstanceField(dummyLevel3QuoteDeserializer,
            "Subscribers", moqLevel3Subscribers.Object);

        subscribedL0Observer = dummyLevel0QuoteDeserializer.Subscribe(moqL0QObserver.Object);
        subscribedL1Observer = dummyLevel1QuoteDeserializer.Subscribe(moqL1QObserver.Object);
        subscribedL2Observer = dummyLevel2QuoteDeserializer.Subscribe(moqL2QObserver.Object);
        subscribedL3Observer = dummyLevel3QuoteDeserializer.Subscribe(moqL3QObserver.Object);

        moqLevel0Subscribers.Verify();
        moqLevel1Subscribers.Verify();
        moqLevel2Subscribers.Verify();
        moqLevel3Subscribers.Verify();
    }

    [TestMethod]
    public void SubscribedObserver_DisposeSubscription_SyncLockProtectsRemovingObserver()
    {
        NewPQQuoteDeserializer_Subscribe_SyncLockProtectsAddingObserver();
        moqLevel0Subscribers.Setup(lo => lo.Remove(moqL0QObserver.Object)).Callback(() => { Assert.IsTrue(haveCalledAcquire); }).Returns(true)
            .Verifiable();
        moqLevel1Subscribers.Setup(lo => lo.Remove(moqL1QObserver.Object)).Callback(() => { Assert.IsTrue(haveCalledAcquire); }).Returns(true)
            .Verifiable();
        moqLevel2Subscribers.Setup(lo => lo.Remove(moqL2QObserver.Object)).Callback(() => { Assert.IsTrue(haveCalledAcquire); }).Returns(true)
            .Verifiable();
        moqLevel3Subscribers.Setup(lo => lo.Remove(moqL3QObserver.Object)).Callback(() => { Assert.IsTrue(haveCalledAcquire); }).Returns(true)
            .Verifiable();

        subscribedL0Observer.Dispose();
        subscribedL1Observer.Dispose();
        subscribedL2Observer.Dispose();
        subscribedL3Observer.Dispose();

        moqLevel0Subscribers.Verify();
        moqLevel1Subscribers.Verify();
        moqLevel2Subscribers.Verify();
        moqLevel3Subscribers.Verify();
    }

    [TestMethod]
    public void EmptyQuoteLvl0Quote_UpdateQuote_SetsDispatcherContextDetails()
    {
        var expectedL0Quote = new PQLevel0Quote(sourceTickerQuoteInfo)
        {
            SinglePrice = 0.78568m, SourceTime = new DateTime(2017, 07, 01, 19, 35, 00), IsReplay = true
        };

        var quoteSerializer = new PQQuoteSerializer(PQMessageFlags.Snapshot);
        var amountWritten = quoteSerializer.Serialize(readWriteBuffer.Buffer, BufferReadWriteOffset, expectedL0Quote);
        socketBufferReadContext.MessageHeader = new MessageHeader(1, 0, 0, (uint)amountWritten);
        socketBufferReadContext.EncodedBuffer!.WriteCursor = BufferReadWriteOffset + amountWritten;
        socketBufferReadContext.LastWriteLength = amountWritten;

        var actualL0Quote = new PQLevel0Quote(sourceTickerQuoteInfo);

        var expectedSequenceId = 101u;
        socketBufferReadContext.EncodedBuffer.ReadCursor = BufferReadWriteOffset + MessageHeaderByteSize;
        dummyLevel0QuoteDeserializer.InvokeUpdateQuote(socketBufferReadContext, actualL0Quote, expectedSequenceId);

        Assert.AreEqual(socketBufferReadContext.DetectTimestamp, actualL0Quote.ClientReceivedTime);
        Assert.AreEqual(socketBufferReadContext.ReceivingTimestamp, actualL0Quote.SocketReceivingTime);
        Assert.AreEqual(socketBufferReadContext.DeserializerTime, actualL0Quote.ProcessedTime);
        Assert.AreEqual(expectedSequenceId, actualL0Quote.PQSequenceId);
        Assert.AreEqual(expectedL0Quote.SinglePrice, actualL0Quote.SinglePrice);
        Assert.AreEqual(expectedL0Quote.SourceTime, actualL0Quote.SourceTime);
        Assert.AreEqual(expectedL0Quote.IsReplay, actualL0Quote.IsReplay);
    }

    [TestMethod]
    public void EmptyQuoteLvl1Quote_UpdateQuote_SetsDispatcherContextDetails()
    {
        var expectedL1Quote = new PQLevel1Quote(sourceTickerQuoteInfo)
        {
            IsReplay = true, Executable = true, SourceAskTime = new DateTime(2017, 07, 01, 21, 25, 30)
            , SourceBidTime = new DateTime(2017, 07, 01, 19, 27, 00)
            , AdapterReceivedTime = new DateTime(2017, 07, 01, 19, 27, 30)
            , AdapterSentTime = new DateTime(2017, 07, 01, 19, 27, 39), BidPriceTop = 0.79324m, AskPriceTop = 0.79326m
        };

        var quoteSerializer = new PQQuoteSerializer(PQMessageFlags.Snapshot);
        var amountWritten = quoteSerializer.Serialize(readWriteBuffer.Buffer, BufferReadWriteOffset, expectedL1Quote);
        socketBufferReadContext.MessageHeader = new MessageHeader(1, 0, 0, (uint)amountWritten);
        socketBufferReadContext.EncodedBuffer!.WriteCursor = BufferReadWriteOffset + amountWritten;
        socketBufferReadContext.LastWriteLength = amountWritten;

        var actualL1Quote = new PQLevel1Quote(sourceTickerQuoteInfo);

        var expectedSequenceId = 102u;
        socketBufferReadContext.EncodedBuffer.ReadCursor = BufferReadWriteOffset + PQQuoteMessageHeader.HeaderSize;
        dummyLevel0QuoteDeserializer.InvokeUpdateQuote(socketBufferReadContext, actualL1Quote, expectedSequenceId);

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
        var expectedL2Quote = new PQLevel2Quote(sourceTickerQuoteInfo);
        var numLayers = expectedL2Quote.BidBook.Capacity;
        Assert.IsTrue(numLayers >= 20);
        for (var i = 0; i < numLayers; i++)
        {
            var bidBooki = expectedL2Quote.BidBook[i]!;
            bidBooki.Price = 0.791905m - 0.00001m * i;
            bidBooki.Volume = 30000 + 10000 * i;
            var askBooki = expectedL2Quote.AskBook[i]!;
            askBooki.Price = 0.791906m + 0.00001m * i;
            askBooki.Volume = 30000 + 10000 * i;
        }

        var quoteSerializer = new PQQuoteSerializer(PQMessageFlags.Snapshot);
        var amountWritten = quoteSerializer.Serialize(readWriteBuffer.Buffer, BufferReadWriteOffset, expectedL2Quote);
        socketBufferReadContext.MessageHeader = new MessageHeader(1, 0, 0, (uint)amountWritten);
        socketBufferReadContext.EncodedBuffer!.WriteCursor = BufferReadWriteOffset + amountWritten;
        socketBufferReadContext.LastWriteLength = amountWritten;

        var actualL2Quote = new PQLevel2Quote(sourceTickerQuoteInfo);

        var expectedSequenceId = 102u;
        socketBufferReadContext.EncodedBuffer.ReadCursor = BufferReadWriteOffset + PQQuoteMessageHeader.HeaderSize;
        dummyLevel0QuoteDeserializer.InvokeUpdateQuote(socketBufferReadContext, actualL2Quote, expectedSequenceId);

        for (var i = 0; i < numLayers; i++)
        {
            var expectedBidBooki = expectedL2Quote.BidBook[i]!;
            var actualBidBooki = actualL2Quote.BidBook[i]!;
            var expectedAskBooki = expectedL2Quote.AskBook[i]!;
            var actualAskBooki = actualL2Quote.AskBook[i]!;
            Assert.AreEqual(expectedBidBooki.Price, actualBidBooki.Price);
            Assert.AreEqual(expectedBidBooki.Volume, actualBidBooki.Volume);
            Assert.AreEqual(expectedAskBooki.Price, actualAskBooki.Price);
            Assert.AreEqual(expectedAskBooki.Volume, actualAskBooki.Volume);
        }
    }

    [TestMethod]
    public void EmptyQuoteLvl3Quote_UpdateQuote_SetsDispatcherContextDetails()
    {
        var expectedL3Quote = new PQLevel3Quote(sourceTickerQuoteInfo);
        var deepestPossibleLayerIndex = expectedL3Quote.BidBook.Capacity;
        Assert.IsTrue(deepestPossibleLayerIndex >= 20);
        var toggleGivenBool = false;
        var togglePaidBool = true;
        for (var i = 0; i < deepestPossibleLayerIndex; i++)
            if (i < PQFieldKeys.SingleByteFieldIdMaxPossibleLastTrades &&
                expectedL3Quote.RecentlyTraded![i] is PQLastTraderPaidGivenTrade lastTradeInfo)
            {
                lastTradeInfo.TradePrice = 0.76591m;
                lastTradeInfo.TradeTime = new DateTime(2017, 07, 02, 13, 40, 11);
                lastTradeInfo.TradeVolume = 2000000;
                lastTradeInfo.WasGiven = toggleGivenBool = !toggleGivenBool;
                lastTradeInfo.WasPaid = togglePaidBool = !togglePaidBool;
                lastTradeInfo.TraderName = "NewTraderName " + i;
            }

        var quoteSerializer = new PQQuoteSerializer(PQMessageFlags.Snapshot);
        var amountWritten = quoteSerializer.Serialize(readWriteBuffer.Buffer, BufferReadWriteOffset, expectedL3Quote);
        socketBufferReadContext.MessageHeader = new MessageHeader(1, 0, 0, (uint)amountWritten);
        socketBufferReadContext.EncodedBuffer!.WriteCursor = BufferReadWriteOffset + amountWritten;
        socketBufferReadContext.LastWriteLength = amountWritten;

        var actualL3Quote = new PQLevel3Quote(sourceTickerQuoteInfo);

        var expectedSequenceId = 102u;
        socketBufferReadContext.EncodedBuffer.ReadCursor = BufferReadWriteOffset + PQQuoteMessageHeader.HeaderSize;
        dummyLevel0QuoteDeserializer.InvokeUpdateQuote(socketBufferReadContext, actualL3Quote, expectedSequenceId);

        for (var i = 0; i < deepestPossibleLayerIndex && i < PQFieldKeys.SingleByteFieldIdMaxPossibleLastTrades; i++)
        {
            var expectedLastTradeInfo = expectedL3Quote.RecentlyTraded![i] as PQLastTraderPaidGivenTrade;
            Assert.IsNotNull(expectedLastTradeInfo);
            var actualLastTradeInfo = actualL3Quote.RecentlyTraded![i] as PQLastTraderPaidGivenTrade;
            Assert.IsNotNull(actualLastTradeInfo);
            Assert.AreEqual(expectedLastTradeInfo.TradePrice, actualLastTradeInfo.TradePrice);
            Assert.AreEqual(expectedLastTradeInfo.TradeTime, actualLastTradeInfo.TradeTime);
            Assert.AreEqual(expectedLastTradeInfo.TradeVolume, actualLastTradeInfo.TradeVolume);
            Assert.AreEqual(expectedLastTradeInfo.WasGiven, actualLastTradeInfo.WasGiven);
            Assert.AreEqual(expectedLastTradeInfo.WasPaid, actualLastTradeInfo.WasPaid);
            Assert.AreEqual(expectedLastTradeInfo.TraderName, actualLastTradeInfo.TraderName);
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

            var expectedDateTime = new DateTime(2017, 07, 15, 21, 35, 14);
            var expectedPublicationStatus = PQSyncStatus.Good;

            moqTimeContext.SetupGet(tc => tc.UtcNow).Returns(expectedDateTime);

            moqL0QObserver.Setup(o => o.OnNext(dummyLevel0QuoteDeserializer.PublishedQuote))
                .Callback<IPQLevel0Quote>(
                    pq =>
                    {
                        Assert.IsTrue(haveCalledAcquire);
                        Assert.IsTrue(pq.HasUpdates);
                        Assert.AreEqual(expectedDateTime, pq.DispatchedTime);
                        Assert.AreEqual(expectedPublicationStatus, pq.PQSyncStatus);
                    }
                ).Verifiable();
            moqL1QObserver.Setup(o => o.OnNext(dummyLevel1QuoteDeserializer.PublishedQuote))
                .Callback<IPQLevel1Quote>(
                    pq =>
                    {
                        Assert.IsTrue(haveCalledAcquire);
                        Assert.IsTrue(pq.HasUpdates);
                        Assert.AreEqual(expectedDateTime, pq.DispatchedTime);
                        Assert.AreEqual(expectedPublicationStatus, pq.PQSyncStatus);
                    }
                ).Verifiable();
            moqL2QObserver.Setup(o => o.OnNext(dummyLevel2QuoteDeserializer.PublishedQuote))
                .Callback<IPQLevel2Quote>(
                    pq =>
                    {
                        Assert.IsTrue(haveCalledAcquire);
                        Assert.IsTrue(pq.HasUpdates);
                        Assert.AreEqual(expectedDateTime, pq.DispatchedTime);
                        Assert.AreEqual(expectedPublicationStatus, pq.PQSyncStatus);
                    }
                ).Verifiable();
            moqL3QObserver.Setup(o => o.OnNext(dummyLevel3QuoteDeserializer.PublishedQuote))
                .Callback<IPQLevel3Quote>(
                    pq =>
                    {
                        Assert.IsTrue(haveCalledAcquire);
                        Assert.IsTrue(pq.HasUpdates);
                        Assert.AreEqual(expectedDateTime, pq.DispatchedTime);
                        Assert.AreEqual(expectedPublicationStatus, pq.PQSyncStatus);
                    }
                ).Verifiable();

            const string expectedTicker = "TestTicker";
            const string expectedSource = "TestSource";

            moqUniqueSrcTkrId.As<ISourceTickerQuoteInfo>().Verify(usti => usti.Ticker, Times.AtLeast(4));
            moqUniqueSrcTkrId.As<ISourceTickerQuoteInfo>().Verify(usti => usti.Source, Times.AtLeast(4));
            moqUniqueSrcTkrId.As<ISourceTickerQuoteInfo>().SetupGet(usti => usti.Ticker).Returns(expectedTicker);
            moqUniqueSrcTkrId.As<ISourceTickerQuoteInfo>().SetupGet(usti => usti.Source).Returns(expectedSource);

            subscribedL0Observer = dummyLevel0QuoteDeserializer.Subscribe(moqL0QObserver.Object);
            subscribedL1Observer = dummyLevel1QuoteDeserializer.Subscribe(moqL1QObserver.Object);
            subscribedL2Observer = dummyLevel2QuoteDeserializer.Subscribe(moqL2QObserver.Object);
            subscribedL3Observer = dummyLevel3QuoteDeserializer.Subscribe(moqL3QObserver.Object);

            dummyLevel0QuoteDeserializer.InvokePushQuoteToSubscribers(expectedPublicationStatus);
            dummyLevel1QuoteDeserializer.InvokePushQuoteToSubscribers(expectedPublicationStatus);
            dummyLevel2QuoteDeserializer.InvokePushQuoteToSubscribers(expectedPublicationStatus);
            dummyLevel3QuoteDeserializer.InvokePushQuoteToSubscribers(expectedPublicationStatus);


            moqPerfLogger.Verify(ltcsl => ltcsl.Enabled, Times.AtLeast(8));
            moqUniqueSrcTkrId.As<ISourceTickerQuoteInfo>().Verify(usti => usti.Ticker, Times.AtLeast(8));
            moqUniqueSrcTkrId.As<ISourceTickerQuoteInfo>().Verify(usti => usti.Source, Times.AtLeast(8));
            moqPerfLogger.Verify(ltcsl => ltcsl.Add("Ticker", expectedTicker), Times.AtLeast(4));
            moqPerfLogger.Verify(ltcsl => ltcsl.Add("Source", expectedSource), Times.AtLeast(4));
            moqPerfLoggerPool.Verify(ltcslp => ltcslp.StartNewTrace(), Times.AtLeast(4));
            moqPerfLoggerPool.Verify(ltcslp => ltcslp.StartNewTrace(), Times.AtLeast(4));
            moqL0QObserver.Verify();
            moqL1QObserver.Verify();
            moqL2QObserver.Verify();
            moqL3QObserver.Verify();

            var moqDisptachPerfLogger = new Mock<IPerfLogger>();
            moqDisptachPerfLogger.Setup(pl => pl.Add(SocketDataLatencyLogger.BeforePublish)).Verifiable();
            SetupQuoteChanges(new DateTime(2017, 07, 15, 23, 39, 26));

            dummyLevel0QuoteDeserializer.InvokePushQuoteToSubscribers(expectedPublicationStatus,
                moqDisptachPerfLogger.Object);
            dummyLevel1QuoteDeserializer.InvokePushQuoteToSubscribers(expectedPublicationStatus,
                moqDisptachPerfLogger.Object);
            dummyLevel2QuoteDeserializer.InvokePushQuoteToSubscribers(expectedPublicationStatus,
                moqDisptachPerfLogger.Object);
            dummyLevel3QuoteDeserializer.InvokePushQuoteToSubscribers(expectedPublicationStatus,
                moqDisptachPerfLogger.Object);

            moqDisptachPerfLogger.Verify(pl => pl.Add(SocketDataLatencyLogger.BeforePublish), Times.Exactly(4));
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

        var expectedPublicationStatus = PQSyncStatus.Good;

        dummyLevel0QuoteDeserializer.InvokePushQuoteToSubscribers(expectedPublicationStatus);
        dummyLevel1QuoteDeserializer.InvokePushQuoteToSubscribers(expectedPublicationStatus);
        dummyLevel2QuoteDeserializer.InvokePushQuoteToSubscribers(expectedPublicationStatus);
        dummyLevel3QuoteDeserializer.InvokePushQuoteToSubscribers(expectedPublicationStatus);
    }

    [TestMethod]
    public void SubscribersNoQuoteupdates_PushQuoteToSubscribers_SetsPubStatusDoesntPush()
    {
        SetupObserversAndSyncLock();

        var expectedPublicationStatus = PQSyncStatus.Good;

        moqL0QObserver.Setup(o => o.OnNext(dummyLevel0QuoteDeserializer.PublishedQuote))
            .Callback(() => { Assert.Fail("Should Never Get Here"); });
        moqL1QObserver.Setup(o => o.OnNext(dummyLevel1QuoteDeserializer.PublishedQuote))
            .Callback(() => { Assert.Fail("Should Never Get Here"); });
        moqL2QObserver.Setup(o => o.OnNext(dummyLevel2QuoteDeserializer.PublishedQuote))
            .Callback(() => { Assert.Fail("Should Never Get Here"); });
        moqL3QObserver.Setup(o => o.OnNext(dummyLevel3QuoteDeserializer.PublishedQuote))
            .Callback(() => { Assert.Fail("Should Never Get Here"); });

        const string expectedTicker = "TestTicker";
        const string expectedSource = "TestSource";

        moqUniqueSrcTkrId.SetupGet(usti => usti.Ticker).Returns(expectedTicker);
        moqUniqueSrcTkrId.SetupGet(usti => usti.Source).Returns(expectedSource);

        subscribedL0Observer = dummyLevel0QuoteDeserializer.Subscribe(moqL0QObserver.Object);
        subscribedL1Observer = dummyLevel1QuoteDeserializer.Subscribe(moqL1QObserver.Object);
        subscribedL2Observer = dummyLevel2QuoteDeserializer.Subscribe(moqL2QObserver.Object);
        subscribedL3Observer = dummyLevel3QuoteDeserializer.Subscribe(moqL3QObserver.Object);

        dummyLevel0QuoteDeserializer.InvokePushQuoteToSubscribers(expectedPublicationStatus);
        dummyLevel1QuoteDeserializer.InvokePushQuoteToSubscribers(expectedPublicationStatus);
        dummyLevel2QuoteDeserializer.InvokePushQuoteToSubscribers(expectedPublicationStatus);
        dummyLevel3QuoteDeserializer.InvokePushQuoteToSubscribers(expectedPublicationStatus);

        moqPerfLoggerPool.Verify(ltcslp => ltcslp.StartNewTrace(), Times.Exactly(4));
        moqPerfLoggerPool.Verify(ltcslp => ltcslp.StartNewTrace(), Times.Exactly(4));
        Assert.AreEqual(expectedPublicationStatus, dummyLevel0QuoteDeserializer.PublishedQuote.PQSyncStatus);
        Assert.AreEqual(expectedPublicationStatus, dummyLevel1QuoteDeserializer.PublishedQuote.PQSyncStatus);
        Assert.AreEqual(expectedPublicationStatus, dummyLevel2QuoteDeserializer.PublishedQuote.PQSyncStatus);
        Assert.AreEqual(expectedPublicationStatus, dummyLevel3QuoteDeserializer.PublishedQuote.PQSyncStatus);
    }

    private void SetupObserversAndSyncLock()
    {
        moqSyncLock = new Mock<ISyncLock>();
        haveCalledAcquire = false;
        moqSyncLock.Setup(sl => sl.Acquire()).Callback(() => { haveCalledAcquire = true; });
        moqSyncLock.Setup(sl => sl.Release()).Callback(() => { haveCalledAcquire = false; });

        NonPublicInvocator.SetInstanceField(dummyLevel0QuoteDeserializer.PublishedQuote,
            "SyncLock", moqSyncLock.Object);
        NonPublicInvocator.SetInstanceField(dummyLevel1QuoteDeserializer.PublishedQuote,
            "SyncLock", moqSyncLock.Object);
        NonPublicInvocator.SetInstanceField(dummyLevel2QuoteDeserializer.PublishedQuote,
            "SyncLock", moqSyncLock.Object);
        NonPublicInvocator.SetInstanceField(dummyLevel3QuoteDeserializer.PublishedQuote,
            "SyncLock", moqSyncLock.Object);

        moqL0QObserver = new Mock<IObserver<IPQLevel0Quote>>();
        moqL1QObserver = new Mock<IObserver<IPQLevel1Quote>>();
        moqL2QObserver = new Mock<IObserver<IPQLevel2Quote>>();
        moqL3QObserver = new Mock<IObserver<IPQLevel3Quote>>();
    }

    private void SetupQuoteChanges(DateTime newDateTime)
    {
        dummyLevel0QuoteDeserializer.PublishedQuote.SourceTime = newDateTime;
        dummyLevel1QuoteDeserializer.PublishedQuote.SourceTime = newDateTime;
        dummyLevel2QuoteDeserializer.PublishedQuote.SourceTime = newDateTime;
        dummyLevel3QuoteDeserializer.PublishedQuote.SourceTime = newDateTime;
    }

    private class DummyPQQuoateDeserializerBase<T> : PQDeserializerBase<T> where T : class, IPQLevel0Quote
    {
        public DummyPQQuoateDeserializerBase(ISourceTickerQuoteInfo identifier) : base(identifier) { }


        public override PQLevel0Quote? Deserialize(ISerdeContext readContext) => throw new NotImplementedException();

        public void InvokeOnReceivedUpdate(IPQDeserializer quoteDeserializer)
        {
            OnReceivedUpdate(quoteDeserializer);
        }

        public void InvokeOnSyncOk(IPQDeserializer quoteDeserializer)
        {
            OnSyncOk(quoteDeserializer);
        }

        public void InvokeOnOutOfSync(IPQDeserializer quoteDeserializer)
        {
            OnOutOfSync(quoteDeserializer);
        }

        public void InvokeUpdateQuote(SocketBufferReadContext socketBufferReadContext, T ent, uint sequenceId)
        {
            UpdateQuote(socketBufferReadContext, ent, sequenceId);
        }

        public void InvokePushQuoteToSubscribers(PQSyncStatus syncStatus,
            IPerfLogger? detectionToPublishLatencyTraceLogger = null)
        {
            PushQuoteToSubscribers(syncStatus, detectionToPublishLatencyTraceLogger);
        }

        public override IMessageDeserializer Clone() => this;
    }
}
