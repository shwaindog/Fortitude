// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Monitoring.Logging.Diagnostics.Performance;
using FortitudeIO.Protocols.Serdes.Binary.Sockets;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes;
using FortitudeMarkets.Pricing.PQ.Serdes.Deserialization;
using FortitudeMarkets.Pricing.Quotes;
using FortitudeMarkets.Pricing.Quotes.LastTraded;
using FortitudeMarkets.Pricing.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.Quotes.TickerInfo;
using FortitudeTests.FortitudeMarkets.Pricing.Quotes;
using Moq;
using static FortitudeMarkets.Configuration.ClientServerConfig.MarketClassificationExtensions;
using static FortitudeMarkets.Pricing.Quotes.TickerInfo.TickerQuoteDetailLevel;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Serdes.Deserialization;

[TestClass]
public class PQQuoteFeedDeserializerTests
{
    private bool compareQuoteWithExpected;
    private int  countLevel1SerializerPublishes;
    private int  countLevel1SerializerUpdates;
    private int  countLevel2SerializerPublishes;
    private int  countLevel2SerializerUpdates;
    private int  countLevel3SerializerPublishes;
    private int  countLevel3SerializerUpdates;
    private int  countTickInstantSerializerPublishes;
    private int  countTickInstantSerializerUpdates;

    private IList<IPQQuoteDeserializer> deserializers = null!;

    private PQPublishableTickInstant expectedFullyInitializedTickInstant = null!;

    private PQPublishableLevel1Quote expectedL1FullyInitializedQuote = null!;
    private PQPublishableLevel2Quote expectedL2FullyInitializedQuote = null!;
    private PQPublishableLevel3Quote expectedL3FullyInitializedQuote = null!;

    private IList<IPQPublishableTickInstant> expectedQuotes = null!;

    private bool l1PublicationStateWasExpected;
    private bool l1QuoteIsInSync;
    private bool l2PublicationStateWasExpected;
    private bool l2QuoteIsInSync;
    private bool l3PublicationStateWasExpected;
    private bool l3QuoteIsInSync;

    private Mock<IPerfLogger> moqDispatchPerfLogger = null!;

    private Mock<IObserver<IPQPublishableLevel1Quote>> moqL1QObserver = null!;
    private Mock<IObserver<IPQPublishableLevel2Quote>> moqL2QObserver = null!;
    private Mock<IObserver<IPQPublishableLevel3Quote>> moqL3QObserver = null!;

    private Mock<IObserver<IPQPublishableTickInstant>> moqTickInstantObserver = null!;

    private PQQuoteFeedDeserializer<IPQPublishableLevel1Quote> pqLevel1QuoteDeserializer = null!;
    private PQQuoteFeedDeserializer<IPQPublishableLevel2Quote> pqLevel2QuoteDeserializer = null!;
    private PQQuoteFeedDeserializer<IPQPublishableLevel3Quote> pqLevel3QuoteDeserializer = null!;

    private PQQuoteFeedDeserializer<IPQPublishableTickInstant> pqTickInstantDeserializer = null!;

    private PQQuoteDeserializationSequencedTestDataBuilder quoteDeserializerSequencedTestDataBuilder = null!;

    private QuoteSequencedTestDataBuilder quoteSequencedTestDataBuilder = null!;

    private SourceTickerInfo sourceTickerInfo = null!;

    private bool tickInstantPublicationStateWasExpected;
    private bool tickInstantQuoteIsInSync;

    [TestInitialize]
    public void SetUp()
    {
        sourceTickerInfo = BuildSourceTickerInfo(ushort.MaxValue, 0, "");

        pqTickInstantDeserializer = new PQQuoteFeedDeserializer<IPQPublishableTickInstant>(sourceTickerInfo);
        pqLevel1QuoteDeserializer = new PQQuoteFeedDeserializer<IPQPublishableLevel1Quote>(sourceTickerInfo);
        pqLevel2QuoteDeserializer = new PQQuoteFeedDeserializer<IPQPublishableLevel2Quote>(sourceTickerInfo);
        pqLevel3QuoteDeserializer = new PQQuoteFeedDeserializer<IPQPublishableLevel3Quote>(sourceTickerInfo);

        SetupDefaultState();

        SetupEventListeners();

        deserializers = new List<IPQQuoteDeserializer>
        {
            pqTickInstantDeserializer, pqLevel1QuoteDeserializer, pqLevel2QuoteDeserializer, pqLevel3QuoteDeserializer
        };

        SetupPQLevelQuotes(BuildSourceTickerInfo(ushort.MaxValue, (ushort)1, "TestTicker1"), FeedSyncStatus.Good);

        SetupQuoteListeners();

        SetupMockPublishQuoteIsExpected();

        moqDispatchPerfLogger         = new Mock<IPerfLogger>();
        quoteSequencedTestDataBuilder = new QuoteSequencedTestDataBuilder();
        quoteDeserializerSequencedTestDataBuilder = new PQQuoteDeserializationSequencedTestDataBuilder(expectedQuotes,
         moqDispatchPerfLogger.Object);
    }

    private SourceTickerInfo BuildSourceTickerInfo(ushort sourceId, ushort tickerId, string ticker) =>
        new(sourceId, "TestSource", tickerId, ticker, Level3Quote, Unknown
          , 20, 0.00001m, 0.0001m, 30000m, 50000000m, 1000m, 1
          , layerFlags: LayerFlags.Volume | LayerFlags.Price | LayerFlags.OrderTraderName | LayerFlags.OrderSize | LayerFlags.OrdersCount
          , lastTradedFlags: LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName | LastTradedFlags.LastTradedVolume |
                             LastTradedFlags.LastTradedTime);

    [TestMethod]
    public void FreshSerializer_DeserializeSnapshot_SyncClientQuoteWithExpected()
    {
        FreshSerializerDeserializeGetsExpected(PQMessageFlags.Update);
        AssertDeserializerHasTimedOutAndNeedsSnapshotIs
            (PQQuoteDeserializationSequencedTestDataBuilder
                .ClientReceivedTimestamp(PQQuoteDeserializationSequencedTestDataBuilder.TimeOffsetForSequenceId(0)), false);
        AssertDeserializerHasTimedOutAndNeedsSnapshotIs
            (PQQuoteDeserializationSequencedTestDataBuilder
                .ClientReceivedTimestamp(PQQuoteDeserializationSequencedTestDataBuilder.TimeOffsetForSequenceId(20)), false);

        SetupPQLevelQuotes(BuildSourceTickerInfo(ushort.MaxValue, 2, "TestTicker1"), FeedSyncStatus.Stale);
        AssertDeserializerHasTimedOutAndNeedsSnapshotIs
            (PQQuoteDeserializationSequencedTestDataBuilder
                .ClientReceivedTimestamp(PQQuoteDeserializationSequencedTestDataBuilder.TimeOffsetForSequenceId(21)), true);

        SetupPQLevelQuotes(BuildSourceTickerInfo(ushort.MaxValue, 2, "TestTicker2"), FeedSyncStatus.Good);
        SendsSequenceIdFromTo(2, 2, true);
        AssertDeserializerHasTimedOutAndNeedsSnapshotIs
            (PQQuoteDeserializationSequencedTestDataBuilder
                .ClientReceivedTimestamp(PQQuoteDeserializationSequencedTestDataBuilder.TimeOffsetForSequenceId(21)), false);
        AssertDeserializerHasTimedOutAndNeedsSnapshotIs
            (PQQuoteDeserializationSequencedTestDataBuilder
                .ClientReceivedTimestamp(PQQuoteDeserializationSequencedTestDataBuilder.TimeOffsetForSequenceId(23)), false);

        SetupPQLevelQuotes(BuildSourceTickerInfo(ushort.MaxValue, 2, "TestTicker2"), FeedSyncStatus.Stale);
        AssertDeserializerHasTimedOutAndNeedsSnapshotIs
            (PQQuoteDeserializationSequencedTestDataBuilder
                .ClientReceivedTimestamp(PQQuoteDeserializationSequencedTestDataBuilder.TimeOffsetForSequenceId(24)), true);
    }

    private void SendsSequenceIdFromTo(uint startId, int batchSize, bool expected)
    {
        var quoteBatches
            = quoteDeserializerSequencedTestDataBuilder.BuildQuotesStartingAt(startId, batchSize, new List<uint>());

        foreach (var quoteBatch in quoteBatches) CallDeserializer(quoteBatch);
        Assert.AreEqual(expected, tickInstantPublicationStateWasExpected);
        Assert.AreEqual(expected, l1PublicationStateWasExpected);
        Assert.AreEqual(expected, l2PublicationStateWasExpected);
        Assert.AreEqual(expected, l3PublicationStateWasExpected);
    }

    private void FreshSerializerDeserializeGetsExpected(PQMessageFlags feedType)
    {
        AssertQuotesAreInSync(false);
        quoteSequencedTestDataBuilder.InitializeQuotes(expectedQuotes, 0);
        var deserializeInputList
            = quoteDeserializerSequencedTestDataBuilder.BuildSerializeContextForQuotes(expectedQuotes, feedType, 0);

        tickInstantPublicationStateWasExpected = false;

        l1PublicationStateWasExpected = false;
        l2PublicationStateWasExpected = false;
        l3PublicationStateWasExpected = false;

        CallDeserializer(deserializeInputList);

        AssertExpectedQuoteReceivedAndIsSameAsExpected();
        AssertPublishCountIs(1);
    }

    private void AssertExpectedQuoteReceivedAndIsSameAsExpected()
    {
        AssertObserversWereHit();
        Assert.IsTrue(tickInstantPublicationStateWasExpected);
        Assert.IsTrue(l1PublicationStateWasExpected);
        Assert.IsTrue(l2PublicationStateWasExpected);
        Assert.IsTrue(l3PublicationStateWasExpected);
        AssertQuotesAreInSync();
    }

    private void AssertObserversWereHit()
    {
        moqTickInstantObserver.Verify();
        moqL1QObserver.Verify();
        moqL2QObserver.Verify();
        moqL3QObserver.Verify();
    }

    private void ResetObserverMockExpectations()
    {
        moqTickInstantObserver.Reset();
        moqL1QObserver.Reset();
        moqL2QObserver.Reset();
        moqL3QObserver.Reset();
    }

    private void AssertQuotesAreInSync(bool isInSync = true)
    {
        Assert.IsTrue(isInSync ? tickInstantQuoteIsInSync : !tickInstantQuoteIsInSync);
        Assert.IsTrue(isInSync ? l1QuoteIsInSync : !l1QuoteIsInSync);
        Assert.IsTrue(isInSync ? l2QuoteIsInSync : !l2QuoteIsInSync);
        Assert.IsTrue(isInSync ? l3QuoteIsInSync : !l3QuoteIsInSync);
    }

    private void AssertReceivedUpdateCountIs(int assertValue)
    {
        Assert.AreEqual(assertValue, countTickInstantSerializerUpdates);
        Assert.AreEqual(assertValue, countLevel1SerializerUpdates);
        Assert.AreEqual(assertValue, countLevel2SerializerUpdates);
        Assert.AreEqual(assertValue, countLevel3SerializerUpdates);
    }

    private void AssertPublishCountIs(int assertValue)
    {
        Assert.AreEqual(assertValue, countTickInstantSerializerPublishes);
        Assert.AreEqual(assertValue, countLevel1SerializerPublishes);
        Assert.AreEqual(assertValue, countLevel2SerializerPublishes);
        Assert.AreEqual(assertValue, countLevel3SerializerPublishes);
    }

    private void AssertDeserializerHasTimedOutAndNeedsSnapshotIs(DateTime currentTime, bool expectedValue)
    {
        for (var i = 0; i < deserializers.Count; i++) Assert.AreEqual(expectedValue, deserializers[i].HasTimedOutAndNeedsSnapshot(currentTime));
    }

    private void CallDeserializer(IList<SocketBufferReadContext> deserializeInputList)
    {
        for (var i = 0; i < deserializers.Count; i++) deserializers[i].Deserialize(deserializeInputList[i]);
    }

    private void SetupMockPublishQuoteIsExpected()
    {
        tickInstantPublicationStateWasExpected = false;

        l1PublicationStateWasExpected = false;
        l2PublicationStateWasExpected = false;
        l3PublicationStateWasExpected = false;
        moqTickInstantObserver
            .Setup(o => o.OnNext(pqTickInstantDeserializer.PublishedQuote))
            .Callback<IPQPublishableTickInstant>
                (pq =>
                 {
                     countTickInstantSerializerPublishes++;
                     pq.HasUpdates = false;
                     if (!compareQuoteWithExpected) return;
                     Console.Out.WriteLine("TickInstant publication status is '" + pq.FeedSyncStatus + "'");
                     tickInstantPublicationStateWasExpected = expectedFullyInitializedTickInstant.FeedSyncStatus == pq.FeedSyncStatus;
                 }
                ).Verifiable();
        moqL1QObserver
            .Setup(o => o.OnNext(pqLevel1QuoteDeserializer.PublishedQuote))
            .Callback<IPQPublishableLevel1Quote>
                (pq =>
                 {
                     countLevel1SerializerPublishes++;
                     pq.HasUpdates = false;
                     if (!compareQuoteWithExpected) return;
                     Console.Out.WriteLine("Level1Quote publication status is '" + pq.FeedSyncStatus + "'");
                     l1PublicationStateWasExpected = expectedL1FullyInitializedQuote.FeedSyncStatus == pq.FeedSyncStatus;
                 }
                ).Verifiable();
        moqL2QObserver
            .Setup(o => o.OnNext(pqLevel2QuoteDeserializer.PublishedQuote))
            .Callback<IPQPublishableLevel2Quote>
                (pq =>
                 {
                     countLevel2SerializerPublishes++;
                     pq.HasUpdates = false;
                     if (!compareQuoteWithExpected) return;
                     Console.Out.WriteLine("Level2Quote publication status is '" + pq.FeedSyncStatus + "'");
                     l2PublicationStateWasExpected = expectedL2FullyInitializedQuote.FeedSyncStatus == pq.FeedSyncStatus;
                 }
                ).Verifiable();
        moqL3QObserver
            .Setup(o => o.OnNext(pqLevel3QuoteDeserializer.PublishedQuote))
            .Callback<IPQPublishableLevel3Quote>
                (pq =>
                 {
                     countLevel3SerializerPublishes++;
                     pq.HasUpdates = false;
                     if (!compareQuoteWithExpected) return;
                     Console.Out.WriteLine("Level3Quote publication status is '" + pq.FeedSyncStatus + "'");
                     l3PublicationStateWasExpected = expectedL3FullyInitializedQuote.FeedSyncStatus == pq.FeedSyncStatus;
                 }
                ).Verifiable();
    }

    private void SetupPQLevelQuotes
    (ISourceTickerInfo publication,
        FeedSyncStatus expectedSyncStatus)
    {
        expectedFullyInitializedTickInstant = new PQPublishableTickInstant(publication)
            { FeedSyncStatus = expectedSyncStatus };
        expectedL1FullyInitializedQuote = new PQPublishableLevel1Quote(publication)
            { FeedSyncStatus = expectedSyncStatus };
        expectedL2FullyInitializedQuote = new PQPublishableLevel2Quote(publication)
            { FeedSyncStatus = expectedSyncStatus };
        expectedL3FullyInitializedQuote = new PQPublishableLevel3Quote(publication)
            { FeedSyncStatus = expectedSyncStatus };

        expectedQuotes = new List<IPQPublishableTickInstant>
        {
            expectedFullyInitializedTickInstant, expectedL1FullyInitializedQuote, expectedL2FullyInitializedQuote
          , expectedL3FullyInitializedQuote
        };
    }

    private void SetupDefaultState()
    {
        compareQuoteWithExpected = true;

        tickInstantPublicationStateWasExpected = false;

        l1PublicationStateWasExpected = false;
        l2PublicationStateWasExpected = false;
        l3PublicationStateWasExpected = false;

        countTickInstantSerializerPublishes = 0;

        countLevel1SerializerPublishes = 0;
        countLevel2SerializerPublishes = 0;
        countLevel3SerializerPublishes = 0;

        tickInstantQuoteIsInSync = false;

        l1QuoteIsInSync = false;
        l2QuoteIsInSync = false;
        l3QuoteIsInSync = false;

        countTickInstantSerializerUpdates = 0;

        countLevel1SerializerUpdates = 0;
        countLevel2SerializerUpdates = 0;
        countLevel3SerializerUpdates = 0;
    }

    private void SetupEventListeners()
    {
        pqTickInstantDeserializer.OutOfSync += deserializer => tickInstantQuoteIsInSync = false;
        pqLevel1QuoteDeserializer.OutOfSync += deserializer => l1QuoteIsInSync          = false;
        pqLevel2QuoteDeserializer.OutOfSync += deserializer => l2QuoteIsInSync          = false;
        pqLevel3QuoteDeserializer.OutOfSync += deserializer => l3QuoteIsInSync          = false;

        pqTickInstantDeserializer.SyncOk += deserializer => tickInstantQuoteIsInSync = true;
        pqLevel1QuoteDeserializer.SyncOk += deserializer => l1QuoteIsInSync          = true;
        pqLevel2QuoteDeserializer.SyncOk += deserializer => l2QuoteIsInSync          = true;
        pqLevel3QuoteDeserializer.SyncOk += deserializer => l3QuoteIsInSync          = true;

        pqTickInstantDeserializer.ReceivedUpdate += deserializer => countTickInstantSerializerUpdates++;
        pqLevel1QuoteDeserializer.ReceivedUpdate += deserializer => countLevel1SerializerUpdates++;
        pqLevel2QuoteDeserializer.ReceivedUpdate += deserializer => countLevel2SerializerUpdates++;
        pqLevel3QuoteDeserializer.ReceivedUpdate += deserializer => countLevel3SerializerUpdates++;
    }

    private void SetupQuoteListeners()
    {
        moqTickInstantObserver = new Mock<IObserver<IPQPublishableTickInstant>>();

        moqL1QObserver = new Mock<IObserver<IPQPublishableLevel1Quote>>();
        moqL2QObserver = new Mock<IObserver<IPQPublishableLevel2Quote>>();
        moqL3QObserver = new Mock<IObserver<IPQPublishableLevel3Quote>>();

        pqTickInstantDeserializer.Subscribe(moqTickInstantObserver.Object);
        pqLevel1QuoteDeserializer.Subscribe(moqL1QObserver.Object);
        pqLevel2QuoteDeserializer.Subscribe(moqL2QObserver.Object);
        pqLevel3QuoteDeserializer.Subscribe(moqL3QObserver.Object);
    }
}
