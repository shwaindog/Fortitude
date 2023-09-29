using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Components.DictionaryAdapter;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Monitoring.Logging.Diagnostics.Performance;
using FortitudeCommon.Types;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.LastTraded;
using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsCore.Pricing.PQ.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization;
using FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization.SyncState;
using FortitudeMarketsCore.Pricing.PQ.Subscription;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization.SyncState
{
    [TestClass]
    public class SyncStateBaseTests
    {
        protected Mock<IPQQuoteDeserializer<IPQLevel0Quote>> MoqPqQuoteStreamDeserializer;
        protected SyncStateBase<IPQLevel0Quote> syncState;
        protected Mock<IFLogger> MoqFlogger;
        protected IPQLevel0Quote DesersializerPqLevel0Quote;
        protected IPQLevel0Quote SyncSlotPqLevel0Quote;
        protected ISourceTickerClientAndPublicationConfig SourceTickerQuoteInfo;
        private uint retryWaitMs = 2000;
        private bool allowCatchup = true;
        protected List<IPQLevel0Quote> ExpectedQuotes;
        protected PQQuoteDeserializationSequencedTestDataBuilder QuoteSequencedTestDataBuilder;
        protected Mock<IPerfLogger> MoqDispatchPerfLogger;
        protected const string ExpectedDateFormat = "yyyy-MM-dd HH:mm:ss.ffffff";

        [TestInitialize]
        public void SetUp()
        {
            InitializeStateInputs();

            BuildSyncState();

            SetupPostStateMoqs();
        }

        [TestCleanup]
        public void TearDown()
        {
            NonPublicInvocator.SetStaticField(typeof(SyncStateBase<IPQLevel0Quote>), "Logger", 
                FLoggerFactory.Instance.GetLogger(typeof(PQQuoteDeserializer<IPQLevel0Quote>)));
        }

        protected virtual void BuildSyncState()
        {
            syncState = new DummySyncStateBase<IPQLevel0Quote>(MoqPqQuoteStreamDeserializer.Object,
                QuoteSyncState.InitializationState);
        }

        protected virtual QuoteSyncState ExpectedQuoteState => QuoteSyncState.InitializationState;
        private void SetupPostStateMoqs()
        {
            SetLogger();
            BuildPQQuote();
            InitializeSequenceBuilder();
        }

        private void InitializeSequenceBuilder()
        {
            ExpectedQuotes = new EditableList<IPQLevel0Quote> {DesersializerPqLevel0Quote};
            MoqDispatchPerfLogger = new Mock<IPerfLogger>();
            QuoteSequencedTestDataBuilder = new PQQuoteDeserializationSequencedTestDataBuilder(ExpectedQuotes,
                MoqDispatchPerfLogger.Object);
        }

        private void BuildPQQuote()
        {
            SourceTickerQuoteInfo = new SourceTickerClientAndPublicationConfig(uint.MaxValue, "TestSource",
                "TestTicker", 20, 0.00001m, 30000m, 50000000m, 1000m, 1,
                LayerFlags.Volume | LayerFlags.Price | LayerFlags.TraderName | LayerFlags.TraderSize
                | LayerFlags.TraderCount, LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName
                                          | LastTradedFlags.LastTradedVolume | LastTradedFlags.LastTradedTime, 
                null, retryWaitMs, allowCatchup);

            DesersializerPqLevel0Quote = new PQLevel0Quote(SourceTickerQuoteInfo)
                {PQSyncStatus = PQSyncStatus.Good};
            SyncSlotPqLevel0Quote = new PQLevel0Quote(SourceTickerQuoteInfo)
            { PQSyncStatus = PQSyncStatus.Good };

            SetupQuoteStreamDeserializerExpectations();
        }

        protected void SetupQuoteStreamDeserializerExpectations()
        {
            MoqPqQuoteStreamDeserializer.SetupGet(qsd => qsd.PublishedQuote).Returns(DesersializerPqLevel0Quote);
            MoqPqQuoteStreamDeserializer.SetupGet(qsd => qsd.Identifier).Returns(SourceTickerQuoteInfo);
            MoqPqQuoteStreamDeserializer.SetupGet(qsd => qsd.SyncRetryMs).Returns(2000);
        }

        private void SetLogger()
        {
            MoqFlogger = new Mock<IFLogger>();
            NonPublicInvocator.SetStaticField(typeof(SyncStateBase<IPQLevel0Quote>), "Logger", MoqFlogger.Object);
        }


        protected void InitializeStateInputs()
        {
            MoqPqQuoteStreamDeserializer = new Mock<IPQQuoteDeserializer<IPQLevel0Quote>>();
        }

        [TestMethod]
        public void NewSyncState_LinkedDeserializerAndState_SetAsExpected()
        {
            Assert.AreEqual(MoqPqQuoteStreamDeserializer.Object, syncState.LinkedDeserializer);
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
            var deserializeInputList = QuoteSequencedTestDataBuilder.BuildSerializeContextForQuotes(ExpectedQuotes, 
                PQFeedType.Update, 1);
            var dispatchContext = deserializeInputList.First();

            MoqPqQuoteStreamDeserializer.Setup(qsd => qsd.UpdateQuote(dispatchContext, DesersializerPqLevel0Quote, 1))
                .Verifiable();
            
            syncState.ProcessInState(dispatchContext);

            MoqPqQuoteStreamDeserializer.Verify();
            
            NewSyncState_ProcessUnsyncedUpdateMessage_CallsExpectedBehaviour();
        }


        //Not making a test as is inherited and only applies to test base class method.
        // called from NewSyncState_ProcessInStateProcessNextExpectedUpdate_CallsExpectedBehaviour
        public virtual void NewSyncState_ProcessUnsyncedUpdateMessage_CallsExpectedBehaviour()
        {
            var deserializeInputList = QuoteSequencedTestDataBuilder.BuildSerializeContextForQuotes(ExpectedQuotes,
                PQFeedType.Update, 0);
            var dispatchContext = deserializeInputList.First();

            var dispatchContextDeserializerTimestamp = new DateTime(2017, 09, 23, 19, 47, 32);
            dispatchContext.DeserializerTimestamp = dispatchContextDeserializerTimestamp;
            DesersializerPqLevel0Quote.PQSequenceId = 4;
            
            MoqFlogger.Setup(fl => fl.Info(It.IsAny<string>(), It.IsAny<object[]>())).Callback<string, object[]>(
                (strTemplt, strParams) =>
                {
                    Assert.AreEqual("Unexpected sequence Id (#{0}) on stream {1}, PrevSeqID={2}, RecvSeqID={3}, " +
                                    "WakeUpTs={4}, DeserializeTs={5}, ReceivingTimestamp={6}", strTemplt);
                    Assert.AreEqual(7, strParams.Length);
                    Assert.AreEqual(0, strParams[0]);
                    Assert.AreEqual(SourceTickerQuoteInfo, strParams[1]);
                    Assert.AreEqual(4u, strParams[2]);
                    Assert.AreEqual(0u, strParams[3]);
                    Assert.AreEqual(PQQuoteDeserializationSequencedTestDataBuilder.ClientReceivedTimestamp(
                        PQQuoteDeserializationSequencedTestDataBuilder.TimeOffsetForSequenceId(0)).ToString(ExpectedDateFormat), 
                        strParams[4]);
                    Assert.AreEqual(dispatchContextDeserializerTimestamp.ToString(ExpectedDateFormat), strParams[5]);
                    Assert.AreEqual(PQQuoteDeserializationSequencedTestDataBuilder.RecevingTimestampBaseTime(
                        PQQuoteDeserializationSequencedTestDataBuilder.TimeOffsetForSequenceId(0)).ToString(ExpectedDateFormat),
                        strParams[6]);
                }).Verifiable();
            
            syncState.ProcessInState(dispatchContext);

            MoqFlogger.Verify(); 
            MoqPqQuoteStreamDeserializer.Verify();
        }

        [TestMethod]
        public virtual void NewSyncState_ProcessSnapshot_CallsExpectedBehaviour()
        {
            var deserializeInputList = QuoteSequencedTestDataBuilder.BuildSerializeContextForQuotes(ExpectedQuotes,
                PQFeedType.Snapshot, 1);
            var dispatchContext = deserializeInputList.First();

            bool hitCallback = false;

            MoqFlogger.Setup(fl => fl.Info(It.IsAny<string>(), It.IsAny<object[]>())).Callback<string, object[]>(
                (strTemplt, strParams) =>
                {
                    hitCallback = true;
                    Assert.AreEqual("Received unexpected or no longer required snapshot for stream {0}", strTemplt);
                    Assert.AreEqual(1, strParams.Length);
                    Assert.AreEqual(SourceTickerQuoteInfo, strParams[0]);
                });

            syncState.ProcessInState(dispatchContext);

            Assert.IsTrue(hitCallback);
            MoqPqQuoteStreamDeserializer.Verify();
        }

        public class DummySyncStateBase<T> : SyncStateBase<T> where T : class, IPQLevel0Quote
        {
            public DummySyncStateBase(IPQQuoteDeserializer<T> linkedDeserializer, QuoteSyncState state) 
                : base(linkedDeserializer, state)
            {
            }
        }
    }
}