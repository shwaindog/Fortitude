// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Monitoring.Logging.Diagnostics.Performance;
using FortitudeIO.Protocols.Serdes.Binary.Sockets;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing;
using FortitudeMarketsApi.Pricing.LastTraded;
using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Serdes.Deserialization;
using FortitudeTests.FortitudeIO.Transports.Network.Config;
using FortitudeTests.FortitudeMarketsCore.Pricing.Quotes;
using Moq;
using static FortitudeIO.TimeSeries.MarketClassificationExtensions;
using static FortitudeMarketsApi.Pricing.Quotes.QuoteLevel;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serdes.Deserialization;

[TestClass]
public class PQQuoteDeserializerTests
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

    private bool l0QuoteIsInSync;
    private bool l0QuoteSame;
    private bool l1QuoteIsInSync;
    private bool l1QuoteSame;
    private bool l2QuoteIsInSync;
    private bool l2QuoteSame;
    private bool l3QuoteIsInSync;
    private bool l3QuoteSame;

    private Mock<IPerfLogger> moqDispatchPerfLogger = null!;

    private Mock<IObserver<IPQLevel0Quote>> moqL0QObserver = null!;
    private Mock<IObserver<IPQLevel1Quote>> moqL1QObserver = null!;
    private Mock<IObserver<IPQLevel2Quote>> moqL2QObserver = null!;
    private Mock<IObserver<IPQLevel3Quote>> moqL3QObserver = null!;

    private PQQuoteDeserializer<PQLevel0Quote> pqLevel0QuoteDeserializer = null!;
    private PQQuoteDeserializer<PQLevel1Quote> pqLevel1QuoteDeserializer = null!;
    private PQQuoteDeserializer<PQLevel2Quote> pqLevel2QuoteDeserializer = null!;
    private PQQuoteDeserializer<PQLevel3Quote> pqLevel3QuoteDeserializer = null!;

    private PQQuoteDeserializationSequencedTestDataBuilder quoteDeserializerSequencedTestDataBuilder = null!;
    private QuoteSequencedTestDataBuilder                  quoteSequencedTestDataBuilder             = null!;

    private ISourceTickerQuoteInfo           sourceTickerQuoteInfo           = null!;
    private ITickerPricingSubscriptionConfig tickerPricingSubscriptionConfig = null!;

    [TestInitialize]
    public void SetUp()
    {
        sourceTickerQuoteInfo = new SourceTickerQuoteInfo
            (ushort.MaxValue, "TestSource", ushort.MaxValue, "TestTicker", Level3, Unknown
           , 20, 0.00001m, 30000m, 50000000m, 1000m, 1
           , LayerFlags.Volume | LayerFlags.Price | LayerFlags.TraderName | LayerFlags.TraderSize | LayerFlags.TraderCount
           , LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName
                                         | LastTradedFlags.LastTradedVolume | LastTradedFlags.LastTradedTime);
        tickerPricingSubscriptionConfig = new TickerPricingSubscriptionConfig
            (sourceTickerQuoteInfo,
             new PricingServerConfig(
                                     NetworkTopicConnectionConfigTests.DummyTopicConnectionConfig,
                                     NetworkTopicConnectionConfigTests.DummyTopicConnectionConfig));
        pqLevel0QuoteDeserializer = new PQQuoteDeserializer<PQLevel0Quote>(tickerPricingSubscriptionConfig);
        pqLevel1QuoteDeserializer = new PQQuoteDeserializer<PQLevel1Quote>(tickerPricingSubscriptionConfig);
        pqLevel2QuoteDeserializer = new PQQuoteDeserializer<PQLevel2Quote>(tickerPricingSubscriptionConfig);
        pqLevel3QuoteDeserializer = new PQQuoteDeserializer<PQLevel3Quote>(tickerPricingSubscriptionConfig);

        SetupDefaultState();

        SetupEventListeners();

        deserializers = new List<IPQQuoteDeserializer>
        {
            pqLevel0QuoteDeserializer, pqLevel1QuoteDeserializer, pqLevel2QuoteDeserializer, pqLevel3QuoteDeserializer
        };

        SetupPQLevelQuotes();

        SetupQuoteListeners();

        SetupMockPublishQuoteIsExpected();

        moqDispatchPerfLogger         = new Mock<IPerfLogger>();
        quoteSequencedTestDataBuilder = new QuoteSequencedTestDataBuilder();
        quoteDeserializerSequencedTestDataBuilder = new PQQuoteDeserializationSequencedTestDataBuilder(expectedQuotes,
         moqDispatchPerfLogger.Object);
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
        SendsSequenceIdFromTo(1, 3, false);
        AssertQuotesAreInSync(false);
        AssertPublishCountIs(1);

        var missingUpdateSequence = quoteDeserializerSequencedTestDataBuilder
                                    .BuildQuotesStartingAt(0, 1,
                                                           new List<uint>()).First();

        quoteSequencedTestDataBuilder.InitializeQuotes(expectedQuotes
                                                     , 3); // gets PQSequenceId is incremented on serialization to 2
        quoteDeserializerSequencedTestDataBuilder
            .BuildSerializeContextForQuotes(expectedQuotes, PQMessageFlags.Update
                                          , 3);

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

        var snapshotSequence2 = quoteDeserializerSequencedTestDataBuilder
                                .BuildQuotesStartingAt(8, 1,
                                                       new List<uint> { 8 }).First();
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
        AssertDeserializerHasTimedOutAndNeedsSnapshotIs(PQQuoteDeserializationSequencedTestDataBuilder
                                                            .ClientReceivedTimestamp(PQQuoteDeserializationSequencedTestDataBuilder
                                                                                         .TimeOffsetForSequenceId(0)), false);
        AssertDeserializerHasTimedOutAndNeedsSnapshotIs(PQQuoteDeserializationSequencedTestDataBuilder
                                                            .ClientReceivedTimestamp(PQQuoteDeserializationSequencedTestDataBuilder
                                                                                         .TimeOffsetForSequenceId(20))
                                                      , false);


        ResetObserverMockExpectations();
        moqL0QObserver.Setup(o => o.OnNext(pqLevel0QuoteDeserializer.PublishedQuote))
                      .Callback<IPQLevel0Quote>(
                                                pq =>
                                                {
                                                    countLevel0SerializerPublishes++;
                                                    Assert.AreEqual(PQSyncStatus.Stale, pq.PQSyncStatus);
                                                }
                                               ).Verifiable();
        moqL1QObserver.Setup(o => o.OnNext(pqLevel1QuoteDeserializer.PublishedQuote))
                      .Callback<IPQLevel1Quote>(
                                                pq =>
                                                {
                                                    countLevel1SerializerPublishes++;
                                                    Assert.AreEqual(PQSyncStatus.Stale, pq.PQSyncStatus);
                                                }
                                               ).Verifiable();
        moqL2QObserver.Setup(o => o.OnNext(pqLevel2QuoteDeserializer.PublishedQuote))
                      .Callback<IPQLevel2Quote>(
                                                pq =>
                                                {
                                                    countLevel2SerializerPublishes++;
                                                    Assert.AreEqual(PQSyncStatus.Stale, pq.PQSyncStatus);
                                                }
                                               ).Verifiable();
        moqL3QObserver.Setup(o => o.OnNext(pqLevel3QuoteDeserializer.PublishedQuote))
                      .Callback<IPQLevel3Quote>(
                                                pq =>
                                                {
                                                    countLevel3SerializerPublishes++;
                                                    Assert.AreEqual(PQSyncStatus.Stale, pq.PQSyncStatus);
                                                }
                                               ).Verifiable();

        AssertDeserializerHasTimedOutAndNeedsSnapshotIs(PQQuoteDeserializationSequencedTestDataBuilder
                                                            .ClientReceivedTimestamp(PQQuoteDeserializationSequencedTestDataBuilder
                                                                                         .TimeOffsetForSequenceId(21)), true);
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

        moqL0QObserver.Setup(o => o.OnNext(pqLevel0QuoteDeserializer.PublishedQuote))
                      .Callback<IPQLevel0Quote>(
                                                pq =>
                                                {
                                                    countLevel0SerializerPublishes++;
                                                    Assert.AreEqual(PQSyncStatus.Good, pq.PQSyncStatus);
                                                }
                                               ).Verifiable();
        moqL1QObserver.Setup(o => o.OnNext(pqLevel1QuoteDeserializer.PublishedQuote))
                      .Callback<IPQLevel1Quote>(
                                                pq =>
                                                {
                                                    countLevel1SerializerPublishes++;
                                                    Assert.AreEqual(PQSyncStatus.Good, pq.PQSyncStatus);
                                                }
                                               ).Verifiable();
        moqL2QObserver.Setup(o => o.OnNext(pqLevel2QuoteDeserializer.PublishedQuote))
                      .Callback<IPQLevel2Quote>(
                                                pq =>
                                                {
                                                    countLevel2SerializerPublishes++;
                                                    Assert.AreEqual(PQSyncStatus.Good, pq.PQSyncStatus);
                                                }
                                               ).Verifiable();
        moqL3QObserver.Setup(o => o.OnNext(pqLevel3QuoteDeserializer.PublishedQuote))
                      .Callback<IPQLevel3Quote>(
                                                pq =>
                                                {
                                                    countLevel3SerializerPublishes++;
                                                    Assert.AreEqual(PQSyncStatus.Good, pq.PQSyncStatus);
                                                }
                                               ).Verifiable();

        CallDeserializer(nextUpdateMessage);
        AssertPublishCountIs(3);
        AssertObserversWereHit();
        AssertDeserializerHasTimedOutAndNeedsSnapshotIs(PQQuoteDeserializationSequencedTestDataBuilder
                                                            .ClientReceivedTimestamp(PQQuoteDeserializationSequencedTestDataBuilder
                                                                                         .TimeOffsetForSequenceId(21))
                                                      , false);
    }

    [TestMethod]
    public void TimedOutDeserializer_HasTimedOutAndNeedsSnapshot_ReturnsFalseNeedsSnapshot()
    {
        InSyncDeserializer_TimesOut_PublishesTimeoutState();
        AssertDeserializerHasTimedOutAndNeedsSnapshotIs(PQQuoteDeserializationSequencedTestDataBuilder
                                                            .ClientReceivedTimestamp(PQQuoteDeserializationSequencedTestDataBuilder
                                                                                         .TimeOffsetForSequenceId(21))
                                                      , false);
        AssertDeserializerHasTimedOutAndNeedsSnapshotIs(PQQuoteDeserializationSequencedTestDataBuilder
                                                            .ClientReceivedTimestamp(PQQuoteDeserializationSequencedTestDataBuilder
                                                                                         .TimeOffsetForSequenceId(99))
                                                      , false);
    }

    [TestMethod]
    [Timeout(30_000)]
    public void OutOfSyncDeserializer_RequestSnapshotReceivesUpdatesUpToBuffer_GoesInSyncPublishesLatestUpdate()
    {
        FreshSerializerDeserializeGetsExpected(PQMessageFlags.Snapshot);

        SendsSequenceIdFromTo(1, PQQuoteDeserializer<PQLevel0Quote>.MaxBufferedUpdates, false);

        AssertQuotesAreInSync(false);
        AssertPublishCountIs(1);

        var snapshotSequence2 = quoteDeserializerSequencedTestDataBuilder
                                .BuildQuotesStartingAt(1, 1, new List<uint> { 1 })
                                .First();
        SetupMockPublishQuoteIsExpected();
        //
        // quoteDeserializerSequencedTestDataBuilder.BuildQuotesStartingAt(PQQuoteDeserializer<PQLevel0Quote>
        //     .MaxBufferedUpdates + 1, 1, new List<int>());

        quoteSequencedTestDataBuilder.InitializeQuotes(expectedQuotes
                                                     , PQQuoteDeserializer<PQLevel0Quote>.MaxBufferedUpdates);


        CallDeserializer(snapshotSequence2);
        AssertQuotesAreInSync();
        AssertExpectedQuoteReceivedAndIsSameAsExpected();
        AssertPublishCountIs(2);
    }

    [TestMethod]
    public void OutOfSyncDeserializer_RequestSnapshotReceivesUpdatesMoreThanBuffer_PublishesNothing()
    {
        FreshSerializerDeserializeGetsExpected(PQMessageFlags.Snapshot);

        SendsSequenceIdFromTo(2, PQQuoteDeserializer<PQLevel0Quote>.MaxBufferedUpdates + 1, false);

        AssertQuotesAreInSync(false);
        AssertPublishCountIs(1);

        var snapshotSequence2 =
            quoteDeserializerSequencedTestDataBuilder
                .BuildQuotesStartingAt(1, 1, new List<uint>()).First();


        moqL0QObserver.Setup(o => o.OnNext(pqLevel0QuoteDeserializer.PublishedQuote))
                      .Callback<IPQLevel0Quote>(
                                                _ => { Assert.Fail("Should not publish anything"); }
                                               ).Verifiable();
        moqL1QObserver.Setup(o => o.OnNext(pqLevel1QuoteDeserializer.PublishedQuote))
                      .Callback<IPQLevel1Quote>(
                                                _ => { Assert.Fail("Should not publish anything"); }
                                               ).Verifiable();
        moqL2QObserver.Setup(o => o.OnNext(pqLevel2QuoteDeserializer.PublishedQuote))
                      .Callback<IPQLevel2Quote>(
                                                _ => { Assert.Fail("Should not publish anything"); }
                                               ).Verifiable();
        moqL3QObserver.Setup(o => o.OnNext(pqLevel3QuoteDeserializer.PublishedQuote))
                      .Callback<IPQLevel3Quote>(
                                                _ => { Assert.Fail("Should not publish anything"); }
                                               ).Verifiable();

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

        Assert.IsTrue(pqLevel3QuoteDeserializer.CheckResync(new DateTime(2017, 09, 23, 15, 25, 30)));
        Assert.IsFalse(pqLevel3QuoteDeserializer.CheckResync(new DateTime(2017, 09, 23, 15, 25, 31)));
        Assert.IsFalse(pqLevel3QuoteDeserializer.CheckResync(new DateTime(2017, 09, 23, 15, 25, 31)));
        Assert.IsTrue(pqLevel3QuoteDeserializer.CheckResync(new DateTime(2017, 09, 23, 15, 25, 32)));
    }

    private void SendsSequenceIdFromTo(uint startId, int batchSize, bool expected)
    {
        var quoteBatches
            = quoteDeserializerSequencedTestDataBuilder.BuildQuotesStartingAt(startId, batchSize, new List<uint>());

        ResetObserverMockExpectations();
        l0QuoteSame = false;
        l1QuoteSame = false;
        l2QuoteSame = false;
        l3QuoteSame = false;

        moqL0QObserver.Setup(o => o.OnNext(pqLevel0QuoteDeserializer.PublishedQuote))
                      .Callback<IPQLevel0Quote>(pq =>
                      {
                          if (pq.PQSyncStatus == PQSyncStatus.Good) l0QuoteSame = true;
                      });
        moqL1QObserver.Setup(o => o.OnNext(pqLevel1QuoteDeserializer.PublishedQuote))
                      .Callback<IPQLevel1Quote>(pq =>
                      {
                          if (pq.PQSyncStatus == PQSyncStatus.Good) l1QuoteSame = true;
                      });
        moqL2QObserver.Setup(o => o.OnNext(pqLevel2QuoteDeserializer.PublishedQuote))
                      .Callback<IPQLevel2Quote>(pq =>
                      {
                          if (pq.PQSyncStatus == PQSyncStatus.Good) l2QuoteSame = true;
                      });
        moqL3QObserver.Setup(o => o.OnNext(pqLevel3QuoteDeserializer.PublishedQuote))
                      .Callback<IPQLevel3Quote>(pq =>
                      {
                          if (pq.PQSyncStatus == PQSyncStatus.Good) l3QuoteSame = true;
                      });

        foreach (var quoteBatch in quoteBatches) CallDeserializer(quoteBatch);
        Assert.AreEqual(expected, l0QuoteSame);
        Assert.AreEqual(expected, l1QuoteSame);
        Assert.AreEqual(expected, l2QuoteSame);
        Assert.AreEqual(expected, l3QuoteSame);
    }

    private void FreshSerializerDeserializeGetsExpected(PQMessageFlags feedType)
    {
        AssertQuotesAreInSync(false);
        var batchId = feedType == PQMessageFlags.Update ? uint.MaxValue : 0u;
        quoteSequencedTestDataBuilder.InitializeQuotes(expectedQuotes, 0);
        var deserializeInputList = quoteDeserializerSequencedTestDataBuilder
            .BuildSerializeContextForQuotes(expectedQuotes, feedType, batchId);

        l0QuoteSame = false;
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
        Assert.IsTrue(l0QuoteSame);
        Assert.IsTrue(l1QuoteSame);
        Assert.IsTrue(l2QuoteSame);
        Assert.IsTrue(l3QuoteSame);
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
        for (var i = 0; i < deserializers.Count; i++)
            Assert.AreEqual(expectedValue, deserializers[i].HasTimedOutAndNeedsSnapshot(currentTime));
    }

    private void CallDeserializer(IList<SocketBufferReadContext> deserializeInputList)
    {
        for (var i = 0; i < deserializers.Count; i++) deserializers[i].Deserialize(deserializeInputList[i]);
        // deserializers[3].Deserialize(deserializeInputList[3]);
    }

    private void SetupMockPublishQuoteIsExpected()
    {
        l0QuoteSame = false;
        l1QuoteSame = false;
        l2QuoteSame = false;
        l3QuoteSame = false;
        moqL0QObserver
            .Setup(o => o.OnNext(pqLevel0QuoteDeserializer.PublishedQuote))
            .Callback<IPQLevel0Quote>(
                                      pq =>
                                      {
                                          countLevel0SerializerPublishes++;
                                          pq.HasUpdates = false;
                                          if (!compareQuoteWithExpected) return;
                                          Console.Out.WriteLine("Level0Quote differences are \n '"
                                                              + expectedL0FullyInitializedQuote.DiffQuotes(pq) + "'");
                                          l0QuoteSame = expectedL0FullyInitializedQuote.AreEquivalent(pq);
                                      }
                                     ).Verifiable();
        moqL1QObserver
            .Setup(o => o.OnNext(pqLevel1QuoteDeserializer.PublishedQuote))
            .Callback<IPQLevel1Quote>(
                                      pq =>
                                      {
                                          countLevel1SerializerPublishes++;
                                          pq.HasUpdates = false;
                                          if (!compareQuoteWithExpected) return;
                                          Console.Out.WriteLine("Level1Quote differences are \n '"
                                                              + expectedL1FullyInitializedQuote.DiffQuotes(pq) + "'");
                                          l1QuoteSame = expectedL1FullyInitializedQuote.AreEquivalent(pq);
                                      }
                                     ).Verifiable();
        moqL2QObserver
            .Setup(o => o.OnNext(pqLevel2QuoteDeserializer.PublishedQuote))
            .Callback<IPQLevel2Quote>(
                                      pq =>
                                      {
                                          countLevel2SerializerPublishes++;
                                          pq.HasUpdates = false;
                                          if (!compareQuoteWithExpected) return;
                                          Console.Out.WriteLine("Level2Quote differences are \n '"
                                                              + expectedL2FullyInitializedQuote.DiffQuotes(pq) + "'");
                                          l2QuoteSame = expectedL2FullyInitializedQuote.AreEquivalent(pq);
                                      }
                                     ).Verifiable();
        moqL3QObserver
            .Setup(o => o.OnNext(pqLevel3QuoteDeserializer.PublishedQuote))
            .Callback<IPQLevel3Quote>(
                                      pq =>
                                      {
                                          countLevel3SerializerPublishes++;
                                          pq.HasUpdates = false;
                                          if (!compareQuoteWithExpected) return;
                                          Console.Out.WriteLine("Level3Quote differences are \n '"
                                                              + expectedL3FullyInitializedQuote.DiffQuotes(pq) + "'");
                                          l3QuoteSame = expectedL3FullyInitializedQuote.AreEquivalent(pq);
                                      }
                                     ).Verifiable();
    }

    private void SetupPQLevelQuotes()
    {
        expectedL0FullyInitializedQuote = new PQLevel0Quote(sourceTickerQuoteInfo)
            { PQSyncStatus = PQSyncStatus.Good };
        expectedL1FullyInitializedQuote = new PQLevel1Quote(sourceTickerQuoteInfo)
            { PQSyncStatus = PQSyncStatus.Good };
        expectedL2FullyInitializedQuote = new PQLevel2Quote(sourceTickerQuoteInfo)
            { PQSyncStatus = PQSyncStatus.Good };
        expectedL3FullyInitializedQuote = new PQLevel3Quote(sourceTickerQuoteInfo)
            { PQSyncStatus = PQSyncStatus.Good };

        expectedQuotes = new List<IPQLevel0Quote>
        {
            expectedL0FullyInitializedQuote, expectedL1FullyInitializedQuote, expectedL2FullyInitializedQuote
          , expectedL3FullyInitializedQuote
        };
    }

    private void SetupDefaultState()
    {
        compareQuoteWithExpected = true;

        l0QuoteSame = false;
        l1QuoteSame = false;
        l2QuoteSame = false;
        l3QuoteSame = false;

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
        pqLevel0QuoteDeserializer.OutOfSync += _ => l0QuoteIsInSync = false;
        pqLevel1QuoteDeserializer.OutOfSync += _ => l1QuoteIsInSync = false;
        pqLevel2QuoteDeserializer.OutOfSync += _ => l2QuoteIsInSync = false;
        pqLevel3QuoteDeserializer.OutOfSync += _ => l3QuoteIsInSync = false;

        pqLevel0QuoteDeserializer.SyncOk += _ => l0QuoteIsInSync = true;
        pqLevel1QuoteDeserializer.SyncOk += _ => l1QuoteIsInSync = true;
        pqLevel2QuoteDeserializer.SyncOk += _ => l2QuoteIsInSync = true;
        pqLevel3QuoteDeserializer.SyncOk += _ => l3QuoteIsInSync = true;

        pqLevel0QuoteDeserializer.ReceivedUpdate += _ => countLevel0SerializerUpdates++;
        pqLevel1QuoteDeserializer.ReceivedUpdate += _ => countLevel1SerializerUpdates++;
        pqLevel2QuoteDeserializer.ReceivedUpdate += _ => countLevel2SerializerUpdates++;
        pqLevel3QuoteDeserializer.ReceivedUpdate += _ => countLevel3SerializerUpdates++;
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
