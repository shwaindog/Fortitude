// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Monitoring.Logging.Diagnostics.Performance;
using FortitudeIO.Protocols.Serdes.Binary.Sockets;
using FortitudeMarkets.Configuration;
using FortitudeMarkets.Configuration.PricingConfig;
using FortitudeMarkets.Pricing.FeedEvents;
using FortitudeMarkets.Pricing.FeedEvents.LastTraded;
using FortitudeMarkets.Pricing.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.PQ.Serdes.Deserialization;
using FortitudeTests.FortitudeIO.Transports.Network.Config;
using FortitudeTests.FortitudeMarkets.Pricing.FeedEvents.Quotes;
using Moq;
using static FortitudeIO.Transports.Network.Config.CountryCityCodes;
using static FortitudeMarkets.Pricing.FeedEvents.TickerInfo.TickerQuoteDetailLevel;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Serdes.Deserialization;

[TestClass]
public class PQMessageDeserializerTests
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

    private IList<IPQMessageDeserializer> deserializers = null!;

    private PQPublishableTickInstant expectedFullyInitializedTickInstant = null!;

    private PQPublishableLevel1Quote expectedL1FullyInitializedQuote = null!;
    private PQPublishableLevel2Quote expectedL2FullyInitializedQuote = null!;
    private PQPublishableLevel3Quote expectedL3FullyInitializedQuote = null!;

    private IList<IPQPublishableTickInstant> expectedQuotes = null!;

    private bool l1QuoteIsInSync;
    private bool l1QuoteSame;
    private bool l2QuoteIsInSync;
    private bool l2QuoteSame;
    private bool l3QuoteIsInSync;
    private bool l3QuoteSame;

    private Mock<IPerfLogger> moqDispatchPerfLogger = null!;

    private Mock<IObserver<IPQPublishableLevel1Quote>> moqL1QObserver = null!;
    private Mock<IObserver<IPQPublishableLevel2Quote>> moqL2QObserver = null!;
    private Mock<IObserver<IPQPublishableLevel3Quote>> moqL3QObserver = null!;

    private Mock<IObserver<IPQPublishableTickInstant>> moqTickInstantObserver = null!;

    private PQMessageDeserializer<PQPublishableLevel1Quote> pqLevel1MessageDeserializer = null!;
    private PQMessageDeserializer<PQPublishableLevel2Quote> pqLevel2MessageDeserializer = null!;
    private PQMessageDeserializer<PQPublishableLevel3Quote> pqLevel3MessageDeserializer = null!;

    private PQMessageDeserializer<PQPublishableTickInstant> pqTickInstantDeserializer = null!;

    private PQQuoteDeserializationSequencedTestDataBuilder quoteDeserializerSequencedTestDataBuilder = null!;

    private QuoteSequencedTestDataBuilder quoteSequencedTestDataBuilder = null!;

    private ISourceTickerInfo sourceTickerInfo = null!;

    private ITickerPricingSubscriptionConfig tickerPricingSubscriptionConfig = null!;

    private bool tickInstantIsInSync;
    private bool tickInstantSame;

    [TestInitialize]
    public void SetUp()
    {
        sourceTickerInfo = new SourceTickerInfo
            (ushort.MaxValue, "TestSource", ushort.MaxValue, "TestTicker", Level3Quote, MarketClassification.Unknown
          ,  AUinMEL, AUinMEL, AUinMEL
           , 20, 0.000001m, 0.0001m, 1m, 50000000m, 1m, 1
           , layerFlags: LayerFlags.Volume | LayerFlags.Price | LayerFlags.OrderTraderName | LayerFlags.OrderSize | LayerFlags.OrdersCount
           , lastTradedFlags: LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName | LastTradedFlags.LastTradedVolume |
                              LastTradedFlags.LastTradedTime);
        tickerPricingSubscriptionConfig = new TickerPricingSubscriptionConfig
            (sourceTickerInfo,
             new PricingServerConfig
                 (NetworkTopicConnectionConfigTests.DummyTopicConnectionConfig
                , NetworkTopicConnectionConfigTests.DummyTopicConnectionConfig));
        pqTickInstantDeserializer = new PQMessageDeserializer<PQPublishableTickInstant>(tickerPricingSubscriptionConfig);
        pqLevel1MessageDeserializer = new PQMessageDeserializer<PQPublishableLevel1Quote>(tickerPricingSubscriptionConfig);
        pqLevel2MessageDeserializer = new PQMessageDeserializer<PQPublishableLevel2Quote>(tickerPricingSubscriptionConfig);
        pqLevel3MessageDeserializer = new PQMessageDeserializer<PQPublishableLevel3Quote>(tickerPricingSubscriptionConfig);

        SetupDefaultState();

        SetupEventListeners();

        deserializers = new List<IPQMessageDeserializer>
        {
            pqTickInstantDeserializer, pqLevel1MessageDeserializer, pqLevel2MessageDeserializer, pqLevel3MessageDeserializer
        };

        SetupPQLevelQuotes();

        SetupQuoteListeners();

        SetupMockPublishQuoteIsExpected();

        moqDispatchPerfLogger                     = new Mock<IPerfLogger>();
        quoteSequencedTestDataBuilder             = new QuoteSequencedTestDataBuilder();
        quoteDeserializerSequencedTestDataBuilder = new PQQuoteDeserializationSequencedTestDataBuilder(expectedQuotes, moqDispatchPerfLogger.Object);
    }

    [TestMethod]
    public void FreshSerializer_DeserializeSnapshot_SyncClientQuoteWithExpected()
    {
        FreshSerializerDeserializeGetsExpected(PQMessageFlags.Snapshot);
    }

    [TestMethod]
    public void FreshSerializer_DeserializeFullUpdateSequenceId0_SyncClientQuoteWithExpected()
    {
        FreshSerializerDeserializeGetsExpected(PQMessageFlags.Update);
    }

    [TestMethod]
    public void FreshSerializerMissedUpdates_DeserializeManyUnexpectedSeqId_StaysUnsynced()
    {
        AssertReceivedUpdateCountIs(0);
        AssertQuotesAreInSync(false);

        SendsSequenceIdFromTo(2, 3, false);

        AssertReceivedUpdateCountIs(0);
        AssertQuotesAreInSync(false);
        AssertPublishCountIs(0);
    }

    [TestMethod]
    public void FreshSerializerMissedUpdates_DeserializeManyUnexpectedSeqIdThenSyncSnapshot_PublishesLatestQuote()
    {
        FreshSerializerMissedUpdates_DeserializeManyUnexpectedSeqId_StaysUnsynced();

        var snapshotSequence1 =
            quoteDeserializerSequencedTestDataBuilder
                .BuildQuotesStartingAt(2, 1, new List<uint> { 2 }).First();

        quoteSequencedTestDataBuilder.InitializeQuotes(expectedQuotes, 4);
        quoteDeserializerSequencedTestDataBuilder.BuildSerializeContextForQuotes(expectedQuotes, PQMessageFlags.Update, 4);
        SetupMockPublishQuoteIsExpected();

        CallDeserializer(snapshotSequence1);

        AssertExpectedQuoteReceivedAndIsSameAsExpected();
        AssertQuotesAreInSync();
        AssertPublishCountIs(1);
    }

    [TestMethod]
    [Timeout(30_000)]
    public void InSyncDeserializer_DeserializeOutOfOrderUpdateThenMissingIdUpdate_GoesOutOfSyncThenInSyncAgain()
    {
        AssertQuotesAreInSync(false);
        FreshSerializerDeserializeGetsExpected(PQMessageFlags.Snapshot);
        AssertQuotesAreInSync();
        AssertPublishCountIs(1);
        SendsSequenceIdFromTo(2, 3, false);
        AssertQuotesAreInSync(false);
        AssertPublishCountIs(1);

        var missingUpdateSequence =
            quoteDeserializerSequencedTestDataBuilder
                .BuildQuotesStartingAt(1, 1, new List<uint>()).First();

        quoteSequencedTestDataBuilder.InitializeQuotes(expectedQuotes, 4); // gets PQSequenceId is incremented on serialization to 2
        quoteDeserializerSequencedTestDataBuilder
            .BuildSerializeContextForQuotes(expectedQuotes, PQMessageFlags.Update, 4);

        SetupMockPublishQuoteIsExpected();

        CallDeserializer(missingUpdateSequence);

        AssertQuotesAreInSync();
        AssertExpectedQuoteReceivedAndIsSameAsExpected();
        AssertPublishCountIs(2);
    }

    [TestMethod]
    public void InSyncDeserializer_DeserializeOutOfOrderUpdateThenHigherSnapshotId_GoesOutOfSyncThenInSyncAgain()
    {
        AssertQuotesAreInSync(false);
        FreshSerializerDeserializeGetsExpected(PQMessageFlags.Snapshot);
        AssertQuotesAreInSync();
        AssertPublishCountIs(1);
        SendsSequenceIdFromTo(3, 3, false);
        AssertQuotesAreInSync(false);
        AssertPublishCountIs(1);

        var snapshotSequence2 =
            quoteDeserializerSequencedTestDataBuilder
                .BuildQuotesStartingAt(8, 1, new List<uint> { 8 }).First();
        SetupMockPublishQuoteIsExpected();
        CallDeserializer(snapshotSequence2);

        AssertQuotesAreInSync();
        AssertExpectedQuoteReceivedAndIsSameAsExpected();
        AssertPublishCountIs(2);
    }

    [TestMethod]
    public void InSyncDeserializer_TimesOut_PublishesTimeoutState()
    {
        AssertQuotesAreInSync(false);
        FreshSerializerDeserializeGetsExpected(PQMessageFlags.Snapshot);
        AssertDeserializerHasTimedOutAndNeedsSnapshotIs
            (PQQuoteDeserializationSequencedTestDataBuilder.ClientReceivedTimestamp
                (PQQuoteDeserializationSequencedTestDataBuilder.TimeOffsetForSequenceId(0)), false);
        AssertDeserializerHasTimedOutAndNeedsSnapshotIs
            (PQQuoteDeserializationSequencedTestDataBuilder.ClientReceivedTimestamp
                (PQQuoteDeserializationSequencedTestDataBuilder.TimeOffsetForSequenceId(20)), false);


        ResetObserverMockExpectations();
        moqTickInstantObserver
            .Setup(o => o.OnNext(pqTickInstantDeserializer.PublishedQuote))
            .Callback<IPQPublishableTickInstant>
                (pq =>
                 {
                     countTickInstantSerializerPublishes++;
                     Assert.AreEqual(FeedSyncStatus.Stale, pq.FeedSyncStatus);
                 }
                )
            .Verifiable();
        moqL1QObserver
            .Setup(o => o.OnNext(pqLevel1MessageDeserializer.PublishedQuote))
            .Callback<IPQPublishableLevel1Quote>
                (
                 pq =>
                 {
                     countLevel1SerializerPublishes++;
                     Assert.AreEqual(FeedSyncStatus.Stale, pq.FeedSyncStatus);
                 }
                )
            .Verifiable();
        moqL2QObserver
            .Setup(o => o.OnNext(pqLevel2MessageDeserializer.PublishedQuote))
            .Callback<IPQPublishableLevel2Quote>
                (
                 pq =>
                 {
                     countLevel2SerializerPublishes++;
                     Assert.AreEqual(FeedSyncStatus.Stale, pq.FeedSyncStatus);
                 }
                )
            .Verifiable();
        moqL3QObserver
            .Setup(o => o.OnNext(pqLevel3MessageDeserializer.PublishedQuote))
            .Callback<IPQPublishableLevel3Quote>
                (pq =>
                 {
                     countLevel3SerializerPublishes++;
                     Assert.AreEqual(FeedSyncStatus.Stale, pq.FeedSyncStatus);
                 }
                )
            .Verifiable();

        AssertDeserializerHasTimedOutAndNeedsSnapshotIs
            (PQQuoteDeserializationSequencedTestDataBuilder.ClientReceivedTimestamp
                (PQQuoteDeserializationSequencedTestDataBuilder.TimeOffsetForSequenceId(21)), true);
        AssertPublishCountIs(2);
        AssertObserversWereHit();
    }

    [TestMethod]
    public void TimedOutDeserializer_ReceivesNextUpdate_GoesBackToInSync()
    {
        InSyncDeserializer_TimesOut_PublishesTimeoutState();
        var nextUpdateMessage =
            quoteDeserializerSequencedTestDataBuilder
                .BuildQuotesStartingAt(1, 1, new List<uint>()).First();

        ResetObserverMockExpectations();

        moqTickInstantObserver
            .Setup(o => o.OnNext(pqTickInstantDeserializer.PublishedQuote))
            .Callback<IPQPublishableTickInstant>
                (
                 pq =>
                 {
                     countTickInstantSerializerPublishes++;
                     Assert.AreEqual(FeedSyncStatus.Good, pq.FeedSyncStatus);
                 }
                )
            .Verifiable();
        moqL1QObserver
            .Setup(o => o.OnNext(pqLevel1MessageDeserializer.PublishedQuote))
            .Callback<IPQPublishableLevel1Quote>
                (pq =>
                 {
                     countLevel1SerializerPublishes++;
                     Assert.AreEqual(FeedSyncStatus.Good, pq.FeedSyncStatus);
                 }
                )
            .Verifiable();
        moqL2QObserver
            .Setup(o => o.OnNext(pqLevel2MessageDeserializer.PublishedQuote))
            .Callback<IPQPublishableLevel2Quote>
                (pq =>
                 {
                     countLevel2SerializerPublishes++;
                     Assert.AreEqual(FeedSyncStatus.Good, pq.FeedSyncStatus);
                 }
                )
            .Verifiable();
        moqL3QObserver
            .Setup(o => o.OnNext(pqLevel3MessageDeserializer.PublishedQuote))
            .Callback<IPQPublishableLevel3Quote>
                (
                 pq =>
                 {
                     countLevel3SerializerPublishes++;
                     Assert.AreEqual(FeedSyncStatus.Good, pq.FeedSyncStatus);
                 }
                )
            .Verifiable();

        CallDeserializer(nextUpdateMessage);
        AssertPublishCountIs(3);
        AssertObserversWereHit();
        AssertDeserializerHasTimedOutAndNeedsSnapshotIs
            (PQQuoteDeserializationSequencedTestDataBuilder.ClientReceivedTimestamp
                (PQQuoteDeserializationSequencedTestDataBuilder.TimeOffsetForSequenceId(21)), false);
    }

    [TestMethod]
    public void TimedOutDeserializer_HasTimedOutAndNeedsSnapshot_ReturnsFalseNeedsSnapshot()
    {
        InSyncDeserializer_TimesOut_PublishesTimeoutState();
        AssertDeserializerHasTimedOutAndNeedsSnapshotIs
            (PQQuoteDeserializationSequencedTestDataBuilder.ClientReceivedTimestamp
                (PQQuoteDeserializationSequencedTestDataBuilder.TimeOffsetForSequenceId(21)), false);
        AssertDeserializerHasTimedOutAndNeedsSnapshotIs
            (PQQuoteDeserializationSequencedTestDataBuilder.ClientReceivedTimestamp
                (PQQuoteDeserializationSequencedTestDataBuilder.TimeOffsetForSequenceId(99)), false);
    }

    [TestMethod]
    [Timeout(90_000)]
    public void OutOfSyncDeserializer_RequestSnapshotReceivesUpdatesUpToBuffer_GoesInSyncPublishesLatestUpdate()
    {
        FreshSerializerDeserializeGetsExpected(PQMessageFlags.Snapshot);

        SendsSequenceIdFromTo(2, PQMessageDeserializer<PQPublishableTickInstant>.MaxBufferedUpdates, false);

        AssertQuotesAreInSync(false);
        AssertPublishCountIs(1);

        var snapshotSequence2 =
            quoteDeserializerSequencedTestDataBuilder
                .BuildQuotesStartingAt(1, 1, new List<uint> { 1 })
                .First();
        SetupMockPublishQuoteIsExpected();
        //
        // quoteDeserializerSequencedTestDataBuilder.BuildQuotesStartingAt(PQQuoteDeserializer<PQTickInstant>
        //     .MaxBufferedUpdates + 1, 1, new List<int>());

        quoteSequencedTestDataBuilder.InitializeQuotes(expectedQuotes, PQMessageDeserializer<PQPublishableTickInstant>.MaxBufferedUpdates + 1);

        CallDeserializer(snapshotSequence2);
        AssertQuotesAreInSync();
        AssertPublishCountIs(2);
        AssertExpectedQuoteReceivedAndIsSameAsExpected();
    }

    [TestMethod]
    public void OutOfSyncDeserializer_RequestSnapshotReceivesUpdatesMoreThanBuffer_PublishesNothing()
    {
        FreshSerializerDeserializeGetsExpected(PQMessageFlags.Snapshot);

        SendsSequenceIdFromTo(2, PQMessageDeserializer<PQPublishableTickInstant>.MaxBufferedUpdates + 1, false);

        AssertQuotesAreInSync(false);
        AssertPublishCountIs(1);

        var snapshotSequence2 =
            quoteDeserializerSequencedTestDataBuilder
                .BuildQuotesStartingAt(1, 1, new List<uint>()).First();

        moqTickInstantObserver
            .Setup(o => o.OnNext(pqTickInstantDeserializer.PublishedQuote))
            .Callback<IPQPublishableTickInstant>(_ => { Assert.Fail("Should not publish anything"); })
            .Verifiable();
        moqL1QObserver
            .Setup(o => o.OnNext(pqLevel1MessageDeserializer.PublishedQuote))
            .Callback<IPQPublishableLevel1Quote>(_ => { Assert.Fail("Should not publish anything"); })
            .Verifiable();
        moqL2QObserver
            .Setup(o => o.OnNext(pqLevel2MessageDeserializer.PublishedQuote))
            .Callback<IPQPublishableLevel2Quote>(_ => { Assert.Fail("Should not publish anything"); })
            .Verifiable();
        moqL3QObserver
            .Setup(o => o.OnNext(pqLevel3MessageDeserializer.PublishedQuote))
            .Callback<IPQPublishableLevel3Quote>(_ => { Assert.Fail("Should not publish anything"); })
            .Verifiable();

        CallDeserializer(snapshotSequence2);
        AssertQuotesAreInSync(false);
        AssertPublishCountIs(1);
    }

    [TestMethod]
    public void SynchronisingDeserializer_CheckResyncAfterWaitTimeout_ReturnsTrueToStartSynchronisationRequest()
    {
        AssertReceivedUpdateCountIs(0);
        AssertQuotesAreInSync(false);

        SendsSequenceIdFromTo(1, 1, false);
        AssertQuotesAreInSync(false);

        Assert.IsTrue(pqLevel3MessageDeserializer.CheckResync(new DateTime(2017, 09, 23, 15, 25, 30)));
        Assert.IsFalse(pqLevel3MessageDeserializer.CheckResync(new DateTime(2017, 09, 23, 15, 25, 31)));
        Assert.IsFalse(pqLevel3MessageDeserializer.CheckResync(new DateTime(2017, 09, 23, 15, 25, 31)));
        Assert.IsTrue(pqLevel3MessageDeserializer.CheckResync(new DateTime(2017, 09, 23, 15, 25, 32)));
    }

    private void SendsSequenceIdFromTo(uint startId, int batchSize, bool expected)
    {
        var quoteBatches
            = quoteDeserializerSequencedTestDataBuilder.BuildQuotesStartingAt(startId, batchSize, new List<uint>());

        ResetObserverMockExpectations();
        tickInstantSame = false;

        l1QuoteSame = false;
        l2QuoteSame = false;
        l3QuoteSame = false;

        moqTickInstantObserver
            .Setup(o => o.OnNext(pqTickInstantDeserializer.PublishedQuote))
            .Callback<IPQPublishableTickInstant>(pq =>
            {
                if (pq.FeedSyncStatus == FeedSyncStatus.Good) tickInstantSame = true;
            });
        moqL1QObserver
            .Setup(o => o.OnNext(pqLevel1MessageDeserializer.PublishedQuote))
            .Callback<IPQPublishableLevel1Quote>(pq =>
            {
                if (pq.FeedSyncStatus == FeedSyncStatus.Good) l1QuoteSame = true;
            });
        moqL2QObserver
            .Setup(o => o.OnNext(pqLevel2MessageDeserializer.PublishedQuote))
            .Callback<IPQPublishableLevel2Quote>(pq =>
            {
                if (pq.FeedSyncStatus == FeedSyncStatus.Good) l2QuoteSame = true;
            });
        moqL3QObserver
            .Setup(o => o.OnNext(pqLevel3MessageDeserializer.PublishedQuote))
            .Callback<IPQPublishableLevel3Quote>(pq =>
            {
                if (pq.FeedSyncStatus == FeedSyncStatus.Good) l3QuoteSame = true;
            });

        foreach (var quoteBatch in quoteBatches) CallDeserializer(quoteBatch);
        Assert.AreEqual(expected, tickInstantSame);
        Assert.AreEqual(expected, l1QuoteSame);
        Assert.AreEqual(expected, l2QuoteSame);
        Assert.AreEqual(expected, l3QuoteSame);
    }

    private void FreshSerializerDeserializeGetsExpected(PQMessageFlags feedType)
    {
        AssertQuotesAreInSync(false);
        var batchId = 0u;
        quoteSequencedTestDataBuilder.InitializeQuotes(expectedQuotes, 0);
        var deserializeInputList = quoteDeserializerSequencedTestDataBuilder
            .BuildSerializeContextForQuotes(expectedQuotes, feedType, batchId);

        tickInstantSame = false;

        l1QuoteSame = false;
        l2QuoteSame = false;
        l3QuoteSame = false;

        CallDeserializer(deserializeInputList);

        AssertExpectedQuoteReceivedAndIsSameAsExpected();
        AssertPublishCountIs(1);
    }

    private void AssertExpectedQuoteReceivedAndIsSameAsExpected()
    {
        AssertObserversWereHit();
        Assert.IsTrue(tickInstantSame);
        Assert.IsTrue(l1QuoteSame);
        Assert.IsTrue(l2QuoteSame);
        Assert.IsTrue(l3QuoteSame);
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
        Assert.IsTrue(isInSync ? tickInstantIsInSync : !tickInstantIsInSync);
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
        // deserializers[3].Deserialize(deserializeInputList[3]);
    }

    private void SetupMockPublishQuoteIsExpected()
    {
        tickInstantSame = false;

        l1QuoteSame = false;
        l2QuoteSame = false;
        l3QuoteSame = false;
        moqTickInstantObserver
            .Setup(o => o.OnNext(pqTickInstantDeserializer.PublishedQuote))
            .Callback<IPQPublishableTickInstant>
                (pq =>
                 {
                     countTickInstantSerializerPublishes++;
                     pq.HasUpdates = false;

                     expectedFullyInitializedTickInstant.HasUpdates = false;
                     if (!compareQuoteWithExpected) return;
                     Console.Out.WriteLine("TickInstant differences are \n '"
                                         + expectedFullyInitializedTickInstant.DiffQuotes(pq) + "'");
                     tickInstantSame = expectedFullyInitializedTickInstant.AreEquivalent(pq);
                 }
                )
            .Verifiable();
        moqL1QObserver
            .Setup(o => o.OnNext(pqLevel1MessageDeserializer.PublishedQuote))
            .Callback<IPQPublishableLevel1Quote>
                (pq =>
                 {
                     countLevel1SerializerPublishes++;

                     pq.HasUpdates                              = false;
                     expectedL1FullyInitializedQuote.HasUpdates = false;
                     if (!compareQuoteWithExpected) return;
                     Console.Out.WriteLine("Level1Quote differences are \n '"
                                         + expectedL1FullyInitializedQuote.DiffQuotes(pq) + "'");
                     l1QuoteSame = expectedL1FullyInitializedQuote.AreEquivalent(pq);
                 }
                )
            .Verifiable();
        moqL2QObserver
            .Setup(o => o.OnNext(pqLevel2MessageDeserializer.PublishedQuote))
            .Callback<IPQPublishableLevel2Quote>
                (pq =>
                 {
                     countLevel2SerializerPublishes++;
                     pq.HasUpdates                              = false;
                     expectedL2FullyInitializedQuote.HasUpdates = false;
                     if (!compareQuoteWithExpected) return;
                     Console.Out.WriteLine("Level2Quote differences are \n '"
                                         + expectedL2FullyInitializedQuote.DiffQuotes(pq) + "'");
                     l2QuoteSame = expectedL2FullyInitializedQuote.AreEquivalent(pq);
                 }
                )
            .Verifiable();
        moqL3QObserver
            .Setup(o => o.OnNext(pqLevel3MessageDeserializer.PublishedQuote))
            .Callback<IPQPublishableLevel3Quote>
                (pq =>
                 {
                     countLevel3SerializerPublishes++;
                     pq.HasUpdates                              = false;
                     expectedL3FullyInitializedQuote.HasUpdates = false;
                     if (!compareQuoteWithExpected) return;
                     Console.Out.WriteLine("Level3Quote differences are \n '"
                                         + expectedL3FullyInitializedQuote.DiffQuotes(pq) + "'");
                     l3QuoteSame = expectedL3FullyInitializedQuote.AreEquivalent(pq);
                 }
                )
            .Verifiable();
    }

    private void SetupPQLevelQuotes()
    {
        expectedFullyInitializedTickInstant = new PQPublishableTickInstant(sourceTickerInfo)
            { FeedSyncStatus = FeedSyncStatus.Good };
        expectedL1FullyInitializedQuote = new PQPublishableLevel1Quote(sourceTickerInfo)
            { FeedSyncStatus = FeedSyncStatus.Good };
        expectedL2FullyInitializedQuote = new PQPublishableLevel2Quote(sourceTickerInfo)
            { FeedSyncStatus = FeedSyncStatus.Good };
        expectedL3FullyInitializedQuote = new PQPublishableLevel3Quote(sourceTickerInfo)
            { FeedSyncStatus = FeedSyncStatus.Good };

        expectedQuotes = new List<IPQPublishableTickInstant>
        {
            expectedFullyInitializedTickInstant, expectedL1FullyInitializedQuote, expectedL2FullyInitializedQuote
          , expectedL3FullyInitializedQuote
        };
    }

    private void SetupDefaultState()
    {
        compareQuoteWithExpected = true;

        tickInstantSame = false;

        l1QuoteSame = false;
        l2QuoteSame = false;
        l3QuoteSame = false;

        countTickInstantSerializerPublishes = 0;

        countLevel1SerializerPublishes = 0;
        countLevel2SerializerPublishes = 0;
        countLevel3SerializerPublishes = 0;

        tickInstantIsInSync = false;

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
        pqTickInstantDeserializer.OutOfSync += _ => tickInstantIsInSync = false;

        pqLevel1MessageDeserializer.OutOfSync += _ => l1QuoteIsInSync = false;
        pqLevel2MessageDeserializer.OutOfSync += _ => l2QuoteIsInSync = false;
        pqLevel3MessageDeserializer.OutOfSync += _ => l3QuoteIsInSync = false;

        pqTickInstantDeserializer.SyncOk += _ => tickInstantIsInSync = true;

        pqLevel1MessageDeserializer.SyncOk += _ => l1QuoteIsInSync = true;
        pqLevel2MessageDeserializer.SyncOk += _ => l2QuoteIsInSync = true;
        pqLevel3MessageDeserializer.SyncOk += _ => l3QuoteIsInSync = true;

        pqTickInstantDeserializer.ReceivedUpdate += _ => countTickInstantSerializerUpdates++;
        pqLevel1MessageDeserializer.ReceivedUpdate += _ => countLevel1SerializerUpdates++;
        pqLevel2MessageDeserializer.ReceivedUpdate += _ => countLevel2SerializerUpdates++;
        pqLevel3MessageDeserializer.ReceivedUpdate += _ => countLevel3SerializerUpdates++;
    }

    private void SetupQuoteListeners()
    {
        moqTickInstantObserver = new Mock<IObserver<IPQPublishableTickInstant>>();

        moqL1QObserver = new Mock<IObserver<IPQPublishableLevel1Quote>>();
        moqL2QObserver = new Mock<IObserver<IPQPublishableLevel2Quote>>();
        moqL3QObserver = new Mock<IObserver<IPQPublishableLevel3Quote>>();

        pqTickInstantDeserializer.Subscribe(moqTickInstantObserver.Object);
        pqLevel1MessageDeserializer.Subscribe(moqL1QObserver.Object);
        pqLevel2MessageDeserializer.Subscribe(moqL2QObserver.Object);
        pqLevel3MessageDeserializer.Subscribe(moqL3QObserver.Object);
    }
}
