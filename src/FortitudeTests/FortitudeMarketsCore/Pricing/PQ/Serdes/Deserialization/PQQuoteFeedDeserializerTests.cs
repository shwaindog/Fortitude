// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Monitoring.Logging.Diagnostics.Performance;
using FortitudeIO.Protocols.Serdes.Binary.Sockets;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsApi.Pricing.Quotes.LastTraded;
using FortitudeMarketsApi.Pricing.Quotes.LayeredBook;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Serdes.Deserialization;
using FortitudeTests.FortitudeMarketsCore.Pricing.Quotes;
using Moq;
using static FortitudeMarketsApi.Configuration.ClientServerConfig.MarketClassificationExtensions;
using static FortitudeMarketsApi.Pricing.Quotes.QuoteLevel;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serdes.Deserialization;

[TestClass]
public class PQQuoteFeedDeserializerTests
{
    private bool compareQuoteWithExpected;
    private int  countLevel0SerializerPublishes;
    private int  countLevel0SerializerUpdates;
    private int  countLevel1SerializerPublishes;
    private int  countLevel1SerializerUpdates;
    private int  countLevel2SerializerPublishes;
    private int  countLevel2SerializerUpdates;
    private int  countLevel3SerializerPublishes;
    private int  countLevel3SerializerUpdates;

    private IList<IPQQuoteDeserializer> deserializers = null!;

    private PQLevel0Quote expectedL0FullyInitializedQuote = null!;
    private PQLevel1Quote expectedL1FullyInitializedQuote = null!;
    private PQLevel2Quote expectedL2FullyInitializedQuote = null!;
    private PQLevel3Quote expectedL3FullyInitializedQuote = null!;

    private IList<IPQLevel0Quote> expectedQuotes = null!;

    private bool l0PublicationStateWasExpected;
    private bool l0QuoteIsInSync;
    private bool l1PublicationStateWasExpected;
    private bool l1QuoteIsInSync;
    private bool l2PublicationStateWasExpected;
    private bool l2QuoteIsInSync;
    private bool l3PublicationStateWasExpected;
    private bool l3QuoteIsInSync;

    private Mock<IPerfLogger> moqDispatchPerfLogger = null!;

    private Mock<IObserver<IPQLevel0Quote>> moqL0QObserver = null!;
    private Mock<IObserver<IPQLevel1Quote>> moqL1QObserver = null!;
    private Mock<IObserver<IPQLevel2Quote>> moqL2QObserver = null!;
    private Mock<IObserver<IPQLevel3Quote>> moqL3QObserver = null!;

    private PQQuoteFeedDeserializer<IPQLevel0Quote> pqLevel0QuoteDeserializer = null!;
    private PQQuoteFeedDeserializer<IPQLevel1Quote> pqLevel1QuoteDeserializer = null!;
    private PQQuoteFeedDeserializer<IPQLevel2Quote> pqLevel2QuoteDeserializer = null!;
    private PQQuoteFeedDeserializer<IPQLevel3Quote> pqLevel3QuoteDeserializer = null!;

    private PQQuoteDeserializationSequencedTestDataBuilder quoteDeserializerSequencedTestDataBuilder = null!;

    private QuoteSequencedTestDataBuilder quoteSequencedTestDataBuilder = null!;
    private SourceTickerQuoteInfo         sourceTickerQuoteInfo         = null!;

    [TestInitialize]
    public void SetUp()
    {
        sourceTickerQuoteInfo     = BuildSourceTickerQuoteInfo(ushort.MaxValue, 0, "");
        pqLevel0QuoteDeserializer = new PQQuoteFeedDeserializer<IPQLevel0Quote>(sourceTickerQuoteInfo);
        pqLevel1QuoteDeserializer = new PQQuoteFeedDeserializer<IPQLevel1Quote>(sourceTickerQuoteInfo);
        pqLevel2QuoteDeserializer = new PQQuoteFeedDeserializer<IPQLevel2Quote>(sourceTickerQuoteInfo);
        pqLevel3QuoteDeserializer = new PQQuoteFeedDeserializer<IPQLevel3Quote>(sourceTickerQuoteInfo);

        SetupDefaultState();

        SetupEventListeners();

        deserializers = new List<IPQQuoteDeserializer>
        {
            pqLevel0QuoteDeserializer, pqLevel1QuoteDeserializer, pqLevel2QuoteDeserializer, pqLevel3QuoteDeserializer
        };

        SetupPQLevelQuotes(BuildSourceTickerQuoteInfo(ushort.MaxValue, (ushort)1, "TestTicker1"), PriceSyncStatus.Good);

        SetupQuoteListeners();

        SetupMockPublishQuoteIsExpected();

        moqDispatchPerfLogger         = new Mock<IPerfLogger>();
        quoteSequencedTestDataBuilder = new QuoteSequencedTestDataBuilder();
        quoteDeserializerSequencedTestDataBuilder = new PQQuoteDeserializationSequencedTestDataBuilder(expectedQuotes,
         moqDispatchPerfLogger.Object);
    }

    private SourceTickerQuoteInfo BuildSourceTickerQuoteInfo(ushort sourceId, ushort tickerId, string ticker) =>
        new(sourceId, "TestSource", tickerId, ticker, Level3, Unknown
          , 20, 0.00001m, 30000m, 50000000m, 1000m, 1
          , LayerFlags.Volume | LayerFlags.Price | LayerFlags.TraderName | LayerFlags.TraderSize | LayerFlags.TraderCount
          , LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName | LastTradedFlags.LastTradedVolume | LastTradedFlags.LastTradedTime);

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

        SetupPQLevelQuotes(BuildSourceTickerQuoteInfo(ushort.MaxValue, 2, "TestTicker1"), PriceSyncStatus.Stale);
        AssertDeserializerHasTimedOutAndNeedsSnapshotIs
            (PQQuoteDeserializationSequencedTestDataBuilder
                .ClientReceivedTimestamp(PQQuoteDeserializationSequencedTestDataBuilder.TimeOffsetForSequenceId(21)), true);

        SetupPQLevelQuotes(BuildSourceTickerQuoteInfo(ushort.MaxValue, 2, "TestTicker2"), PriceSyncStatus.Good);
        SendsSequenceIdFromTo(2, 2, true);
        AssertDeserializerHasTimedOutAndNeedsSnapshotIs
            (PQQuoteDeserializationSequencedTestDataBuilder
                .ClientReceivedTimestamp(PQQuoteDeserializationSequencedTestDataBuilder.TimeOffsetForSequenceId(21)), false);
        AssertDeserializerHasTimedOutAndNeedsSnapshotIs
            (PQQuoteDeserializationSequencedTestDataBuilder
                .ClientReceivedTimestamp(PQQuoteDeserializationSequencedTestDataBuilder.TimeOffsetForSequenceId(23)), false);

        SetupPQLevelQuotes(BuildSourceTickerQuoteInfo(ushort.MaxValue, 2, "TestTicker2"), PriceSyncStatus.Stale);
        AssertDeserializerHasTimedOutAndNeedsSnapshotIs
            (PQQuoteDeserializationSequencedTestDataBuilder
                .ClientReceivedTimestamp(PQQuoteDeserializationSequencedTestDataBuilder.TimeOffsetForSequenceId(24)), true);
    }

    private void SendsSequenceIdFromTo(uint startId, int batchSize, bool expected)
    {
        var quoteBatches
            = quoteDeserializerSequencedTestDataBuilder.BuildQuotesStartingAt(startId, batchSize, new List<uint>());

        foreach (var quoteBatch in quoteBatches) CallDeserializer(quoteBatch);
        Assert.AreEqual(expected, l0PublicationStateWasExpected);
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

        l0PublicationStateWasExpected = false;
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
        Assert.IsTrue(l0PublicationStateWasExpected);
        Assert.IsTrue(l1PublicationStateWasExpected);
        Assert.IsTrue(l2PublicationStateWasExpected);
        Assert.IsTrue(l3PublicationStateWasExpected);
        AssertQuotesAreInSync();
    }

    private void AssertObserversWereHit()
    {
        moqL0QObserver.Verify();
        moqL1QObserver.Verify();
        moqL2QObserver.Verify();
        moqL3QObserver.Verify();
    }

    private void ResetObserverMockExpectations()
    {
        moqL0QObserver.Reset();
        moqL1QObserver.Reset();
        moqL2QObserver.Reset();
        moqL3QObserver.Reset();
    }

    private void AssertQuotesAreInSync(bool isInSync = true)
    {
        Assert.IsTrue(isInSync ? l0QuoteIsInSync : !l0QuoteIsInSync);
        Assert.IsTrue(isInSync ? l1QuoteIsInSync : !l1QuoteIsInSync);
        Assert.IsTrue(isInSync ? l2QuoteIsInSync : !l2QuoteIsInSync);
        Assert.IsTrue(isInSync ? l3QuoteIsInSync : !l3QuoteIsInSync);
    }

    private void AssertReceivedUpdateCountIs(int assertValue)
    {
        Assert.AreEqual(assertValue, countLevel0SerializerUpdates);
        Assert.AreEqual(assertValue, countLevel1SerializerUpdates);
        Assert.AreEqual(assertValue, countLevel2SerializerUpdates);
        Assert.AreEqual(assertValue, countLevel3SerializerUpdates);
    }

    private void AssertPublishCountIs(int assertValue)
    {
        Assert.AreEqual(assertValue, countLevel0SerializerPublishes);
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
        l0PublicationStateWasExpected = false;
        l1PublicationStateWasExpected = false;
        l2PublicationStateWasExpected = false;
        l3PublicationStateWasExpected = false;
        moqL0QObserver
            .Setup(o => o.OnNext(pqLevel0QuoteDeserializer.PublishedQuote))
            .Callback<IPQLevel0Quote>
                (pq =>
                 {
                     countLevel0SerializerPublishes++;
                     pq.HasUpdates = false;
                     if (!compareQuoteWithExpected) return;
                     Console.Out.WriteLine("Level0Quote publication status is '" + pq.PQPriceSyncStatus + "'");
                     l0PublicationStateWasExpected = expectedL0FullyInitializedQuote.PQPriceSyncStatus == pq.PQPriceSyncStatus;
                 }
                ).Verifiable();
        moqL1QObserver
            .Setup(o => o.OnNext(pqLevel1QuoteDeserializer.PublishedQuote))
            .Callback<IPQLevel1Quote>
                (pq =>
                 {
                     countLevel1SerializerPublishes++;
                     pq.HasUpdates = false;
                     if (!compareQuoteWithExpected) return;
                     Console.Out.WriteLine("Level1Quote publication status is '" + pq.PQPriceSyncStatus + "'");
                     l1PublicationStateWasExpected = expectedL1FullyInitializedQuote.PQPriceSyncStatus == pq.PQPriceSyncStatus;
                 }
                ).Verifiable();
        moqL2QObserver
            .Setup(o => o.OnNext(pqLevel2QuoteDeserializer.PublishedQuote))
            .Callback<IPQLevel2Quote>
                (pq =>
                 {
                     countLevel2SerializerPublishes++;
                     pq.HasUpdates = false;
                     if (!compareQuoteWithExpected) return;
                     Console.Out.WriteLine("Level2Quote publication status is '" + pq.PQPriceSyncStatus + "'");
                     l2PublicationStateWasExpected = expectedL2FullyInitializedQuote.PQPriceSyncStatus == pq.PQPriceSyncStatus;
                 }
                ).Verifiable();
        moqL3QObserver
            .Setup(o => o.OnNext(pqLevel3QuoteDeserializer.PublishedQuote))
            .Callback<IPQLevel3Quote>
                (pq =>
                 {
                     countLevel3SerializerPublishes++;
                     pq.HasUpdates = false;
                     if (!compareQuoteWithExpected) return;
                     Console.Out.WriteLine("Level3Quote publication status is '" + pq.PQPriceSyncStatus + "'");
                     l3PublicationStateWasExpected = expectedL3FullyInitializedQuote.PQPriceSyncStatus == pq.PQPriceSyncStatus;
                 }
                ).Verifiable();
    }

    private void SetupPQLevelQuotes
    (ISourceTickerQuoteInfo publicationQuotes,
        PriceSyncStatus expectedSyncStatus)
    {
        expectedL0FullyInitializedQuote = new PQLevel0Quote(publicationQuotes)
            { PQPriceSyncStatus = expectedSyncStatus };
        expectedL1FullyInitializedQuote = new PQLevel1Quote(publicationQuotes)
            { PQPriceSyncStatus = expectedSyncStatus };
        expectedL2FullyInitializedQuote = new PQLevel2Quote(publicationQuotes)
            { PQPriceSyncStatus = expectedSyncStatus };
        expectedL3FullyInitializedQuote = new PQLevel3Quote(publicationQuotes)
            { PQPriceSyncStatus = expectedSyncStatus };

        expectedQuotes = new List<IPQLevel0Quote>
        {
            expectedL0FullyInitializedQuote, expectedL1FullyInitializedQuote, expectedL2FullyInitializedQuote
          , expectedL3FullyInitializedQuote
        };
    }

    private void SetupDefaultState()
    {
        compareQuoteWithExpected = true;

        l0PublicationStateWasExpected = false;
        l1PublicationStateWasExpected = false;
        l2PublicationStateWasExpected = false;
        l3PublicationStateWasExpected = false;

        countLevel0SerializerPublishes = 0;
        countLevel1SerializerPublishes = 0;
        countLevel2SerializerPublishes = 0;
        countLevel3SerializerPublishes = 0;

        l0QuoteIsInSync = false;
        l1QuoteIsInSync = false;
        l2QuoteIsInSync = false;
        l3QuoteIsInSync = false;

        countLevel0SerializerUpdates = 0;
        countLevel1SerializerUpdates = 0;
        countLevel2SerializerUpdates = 0;
        countLevel3SerializerUpdates = 0;
    }

    private void SetupEventListeners()
    {
        pqLevel0QuoteDeserializer.OutOfSync += deserializer => l0QuoteIsInSync = false;
        pqLevel1QuoteDeserializer.OutOfSync += deserializer => l1QuoteIsInSync = false;
        pqLevel2QuoteDeserializer.OutOfSync += deserializer => l2QuoteIsInSync = false;
        pqLevel3QuoteDeserializer.OutOfSync += deserializer => l3QuoteIsInSync = false;

        pqLevel0QuoteDeserializer.SyncOk += deserializer => l0QuoteIsInSync = true;
        pqLevel1QuoteDeserializer.SyncOk += deserializer => l1QuoteIsInSync = true;
        pqLevel2QuoteDeserializer.SyncOk += deserializer => l2QuoteIsInSync = true;
        pqLevel3QuoteDeserializer.SyncOk += deserializer => l3QuoteIsInSync = true;

        pqLevel0QuoteDeserializer.ReceivedUpdate += deserializer => countLevel0SerializerUpdates++;
        pqLevel1QuoteDeserializer.ReceivedUpdate += deserializer => countLevel1SerializerUpdates++;
        pqLevel2QuoteDeserializer.ReceivedUpdate += deserializer => countLevel2SerializerUpdates++;
        pqLevel3QuoteDeserializer.ReceivedUpdate += deserializer => countLevel3SerializerUpdates++;
    }

    private void SetupQuoteListeners()
    {
        moqL0QObserver = new Mock<IObserver<IPQLevel0Quote>>();
        moqL1QObserver = new Mock<IObserver<IPQLevel1Quote>>();
        moqL2QObserver = new Mock<IObserver<IPQLevel2Quote>>();
        moqL3QObserver = new Mock<IObserver<IPQLevel3Quote>>();

        pqLevel0QuoteDeserializer.Subscribe(moqL0QObserver.Object);
        pqLevel1QuoteDeserializer.Subscribe(moqL1QObserver.Object);
        pqLevel2QuoteDeserializer.Subscribe(moqL2QObserver.Object);
        pqLevel3QuoteDeserializer.Subscribe(moqL3QObserver.Object);
    }
}
