// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using Castle.Components.DictionaryAdapter;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Monitoring.Logging.Diagnostics.Performance;
using FortitudeCommon.Types;
using FortitudeMarkets.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarkets.Pricing.FeedEvents;
using FortitudeMarkets.Pricing.FeedEvents.LastTraded;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes;
using FortitudeMarkets.Pricing.PQ.Serdes.Deserialization;
using FortitudeMarkets.Pricing.PQ.Serdes.Deserialization.SyncState;
using FortitudeTests.FortitudeIO.Transports.Network.Config;
using Moq;
using static FortitudeMarkets.Configuration.ClientServerConfig.MarketClassificationExtensions;
using static FortitudeMarkets.Pricing.FeedEvents.TickerInfo.TickerQuoteDetailLevel;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Serdes.Deserialization.SyncState;

[TestClass]
public class SyncStateBaseTests
{
    protected const  string ExpectedDateFormat = "yyyy-MM-dd HH:mm:ss.ffffff";
    private readonly bool   allowCatchup       = true;
    private readonly uint   retryWaitMs        = 2000;

    protected PQPublishableTickInstant        DesersializerPqTickInstant = null!;
    protected List<IPQPublishableTickInstant> ExpectedQuotes             = null!;
    protected Mock<IPerfLogger>    MoqDispatchPerfLogger      = null!;
    protected Mock<IFLogger>       MoqFlogger                 = null!;

    protected PQQuoteDeserializer<PQPublishableTickInstant> pqQuoteStreamDeserializer = null!;

    protected PQQuoteDeserializationSequencedTestDataBuilder QuoteSequencedTestDataBuilder = null!;

    protected PQPublishableTickInstant     SendPqTickInstant     = null!;
    protected ISourceTickerInfo SourceTickerInfo      = null!;
    protected PQPublishableTickInstant     SyncSlotPQTickInstant = null!;

    protected SyncStateBase<PQPublishableTickInstant> syncState = null!;

    protected virtual QuoteSyncState ExpectedQuoteState => QuoteSyncState.InitializationState;

    [TestInitialize]
    public void SetUp()
    {
        InitializeStateInputs();

        SetupPostStateMoqs();

        BuildSyncState();
    }

    [TestCleanup]
    public void TearDown()
    {
        NonPublicInvocator.SetStaticField
            (typeof(SyncStateBase<PQPublishableTickInstant>), "Logger"
           , FLoggerFactory.Instance.GetLogger(typeof(PQQuoteDeserializer<PQPublishableTickInstant>)));
    }

    protected virtual void BuildSyncState()
    {
        syncState = new DummySyncStateBase<PQPublishableTickInstant>(pqQuoteStreamDeserializer, QuoteSyncState.InitializationState);
    }

    private void SetupPostStateMoqs()
    {
        SetLogger();
        BuildPQQuote();
        InitializeSequenceBuilder();
    }

    private void InitializeSequenceBuilder()
    {
        ExpectedQuotes = new EditableList<IPQPublishableTickInstant> { SendPqTickInstant };

        MoqDispatchPerfLogger = new Mock<IPerfLogger>();
        QuoteSequencedTestDataBuilder =
            new PQQuoteDeserializationSequencedTestDataBuilder
                (ExpectedQuotes, MoqDispatchPerfLogger.Object);
    }

    private void BuildPQQuote()
    {
        SourceTickerInfo =
            new SourceTickerInfo
                (ushort.MaxValue, "TestSource", ushort.MaxValue, "TestTicker", Level3Quote, Unknown
               , 20, 0.00001m, 30000m, 50000000m, 1000m, 1
               , layerFlags: LayerFlags.Volume | LayerFlags.Price | LayerFlags.OrderTraderName | LayerFlags.OrderSize | LayerFlags.OrdersCount
               , lastTradedFlags: LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName | LastTradedFlags.LastTradedVolume |
                                  LastTradedFlags.LastTradedTime);
        pqQuoteStreamDeserializer
            = new PQQuoteDeserializer<PQPublishableTickInstant>
                (new TickerPricingSubscriptionConfig
                    (SourceTickerInfo,
                     new PricingServerConfig
                         (NetworkTopicConnectionConfigTests.DummyTopicConnectionConfig,
                          NetworkTopicConnectionConfigTests.DummyTopicConnectionConfig,
                          syncRetryIntervalMs: retryWaitMs, allowUpdatesCatchup: allowCatchup)));
        SendPqTickInstant = new PQPublishableTickInstant(SourceTickerInfo)
            { FeedSyncStatus = FeedSyncStatus.Good, PQSequenceId = 2 };
        DesersializerPqTickInstant = pqQuoteStreamDeserializer.PublishedQuote;
        SyncSlotPQTickInstant = new PQPublishableTickInstant(SourceTickerInfo)
            { FeedSyncStatus = FeedSyncStatus.Good };
    }

    private void SetLogger()
    {
        MoqFlogger = new Mock<IFLogger>();
        NonPublicInvocator.SetStaticField(typeof(SyncStateBase<PQPublishableTickInstant>), "Logger", MoqFlogger.Object);
    }


    protected void InitializeStateInputs() { }

    [TestMethod]
    public void NewSyncState_LinkedDeserializerAndState_SetAsExpected()
    {
        Assert.AreEqual(pqQuoteStreamDeserializer, syncState.LinkedDeserializer);
        Assert.AreEqual(ExpectedQuoteState, syncState.State);
    }

    [TestMethod]
    public virtual void NewSyncState_EligibleForResync_ReturnsExpected()
    {
        Assert.IsFalse(syncState.EligibleForResync(new DateTime()));
        Assert.IsFalse(syncState.EligibleForResync(DateTime.MaxValue));
    }

    [TestMethod]
    public virtual void NewSyncState_HasJustTimedOut_ReturnsExpected()
    {
        Assert.IsFalse(syncState.HasJustGoneStale(new DateTime()));
        Assert.IsFalse(syncState.HasJustGoneStale(DateTime.MaxValue));
    }

    [TestMethod]
    public virtual void NewSyncState_ProcessInStateProcessNextExpectedUpdate_CallsExpectedBehaviour()
    {
        var deserializeInputList = QuoteSequencedTestDataBuilder.BuildSerializeContextForQuotes
            (ExpectedQuotes, PQMessageFlags.Update, 0);
        var sockBuffContext = deserializeInputList.First();

        syncState.ProcessInState(sockBuffContext);

        NewSyncState_ProcessUnsyncedUpdateMessage_CallsExpectedBehaviour();
    }


    //Not making a test as is inherited and only applies to test base class method.
    // called from NewSyncState_ProcessInStateProcessNextExpectedUpdate_CallsExpectedBehaviour
    public virtual void NewSyncState_ProcessUnsyncedUpdateMessage_CallsExpectedBehaviour()
    {
        var deserializeInputList = QuoteSequencedTestDataBuilder.BuildSerializeContextForQuotes
            (ExpectedQuotes, PQMessageFlags.Update, 1);
        var sockBuffContext = deserializeInputList.First();
        syncState.ProcessInState(sockBuffContext);

        SendPqTickInstant.HasUpdates = true;
        deserializeInputList = QuoteSequencedTestDataBuilder.BuildSerializeContextForQuotes
            (ExpectedQuotes, PQMessageFlags.Update, 0);
        sockBuffContext = deserializeInputList.First();

        var sockBuffContextDeserializerTimestamp = new DateTime(2017, 09, 23, 19, 47, 32);
        sockBuffContext.DeserializerTime        = sockBuffContextDeserializerTimestamp;
        DesersializerPqTickInstant.PQSequenceId = 4;

        MoqFlogger.Setup(fl => fl.Info(It.IsAny<string>(), It.IsAny<object[]>())).Callback<string, object[]>(
         (strTemplt, strParams) =>
         {
             Assert.AreEqual("Unexpected sequence Id (#{0}) on stream {1}, PrevSeqID={2}, RecvSeqID={3}, " +
                             "WakeUpTs={4}, DeserializeTs={5}, ReceivingTimestamp={6}", strTemplt);
             Assert.AreEqual(7, strParams.Length);
             Assert.AreEqual(0, strParams[0]);
             Assert.AreEqual(SourceTickerInfo, strParams[1]);
             Assert.AreEqual(4u, strParams[2]);
             Assert.AreEqual(0u, strParams[3]);
             Assert.AreEqual(PQQuoteDeserializationSequencedTestDataBuilder.ClientReceivedTimestamp
                                                                               (PQQuoteDeserializationSequencedTestDataBuilder
                                                                                   .TimeOffsetForSequenceId(0))
                                                                           .ToString(ExpectedDateFormat), strParams[4]);
             Assert.AreEqual(sockBuffContextDeserializerTimestamp.ToString(ExpectedDateFormat), strParams[5]);
             Assert.AreEqual(PQQuoteDeserializationSequencedTestDataBuilder
                             .RecevingTimestampBaseTime(PQQuoteDeserializationSequencedTestDataBuilder.TimeOffsetForSequenceId(0))
                             .ToString(ExpectedDateFormat), strParams[6]);
         }).Verifiable();

        syncState.ProcessInState(sockBuffContext);

        MoqFlogger.Verify();
    }

    [TestMethod]
    public virtual void NewSyncState_ProcessSnapshot_CallsExpectedBehaviour()
    {
        pqQuoteStreamDeserializer.PublishedQuote.PQSequenceId = 3;
        var deserializeInputList = QuoteSequencedTestDataBuilder.BuildSerializeContextForQuotes
            (ExpectedQuotes, PQMessageFlags.Snapshot, 2);
        var sockBuffContext = deserializeInputList.First();

        var hitCallback = false;

        MoqFlogger.Setup(fl => fl.Info(It.IsAny<string>(), It.IsAny<object[]>())).Callback<string, object[]>(
         (strTemplt, strParams) =>
         {
             hitCallback = true;
             Assert.AreEqual("Received unexpected or no longer required snapshot for stream {0}", strTemplt);
             Assert.AreEqual(1, strParams.Length);
             Assert.AreEqual(SourceTickerInfo, strParams[0]);
         });

        syncState.ProcessInState(sockBuffContext);

        Assert.IsTrue(hitCallback);
    }

    public class DummySyncStateBase<T> : SyncStateBase<T> where T : PQPublishableTickInstant, new()
    {
        public DummySyncStateBase(IPQQuotePublishingDeserializer<T> linkedDeserializer, QuoteSyncState state)
            : base(linkedDeserializer, state) { }
    }
}
