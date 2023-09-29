using System;
using System.Linq;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization.SyncState;
using FortitudeMarketsCore.Pricing.PQ.Subscription;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization.SyncState
{
    [TestClass]
    public class InSyncStateTests : SyncStateBaseTests
    {

        protected override void BuildSyncState()
        {
            syncState = new InSyncState<IPQLevel0Quote>(MoqPqQuoteStreamDeserializer.Object);
        }

        protected override QuoteSyncState ExpectedQuoteState => QuoteSyncState.InSync;

        [TestMethod]
        public override void NewSyncState_ProcessInStateProcessNextExpectedUpdate_CallsExpectedBehaviour()
        {
            //can received the same update twice.
            SendUpdateSequenceId(1, PQFeedType.Update);
            SendUpdateSequenceId(1, PQFeedType.Update);

            InSyncUpdate_ProcessUnsyncedUpdateMessageInPast_LogsAnomaly();
        }

        [TestMethod]
        public override void NewSyncState_ProcessUnsyncedUpdateMessage_CallsExpectedBehaviour()
        {
            var deserializeInputList = QuoteSequencedTestDataBuilder.BuildSerializeContextForQuotes(ExpectedQuotes,
                PQFeedType.Update, 2);
            var dispatchContext = deserializeInputList.First();

            DesersializerPqLevel0Quote.PQSequenceId = 4;

            MoqPqQuoteStreamDeserializer.Setup(qsd => qsd.ClearSyncRing()).Verifiable();
            MoqPqQuoteStreamDeserializer.Setup(qsd => qsd.ClaimSyncSlotEntry()).Returns(SyncSlotPqLevel0Quote)
                .Verifiable();
            MoqPqQuoteStreamDeserializer.Setup(qsd => qsd.UpdateQuote(dispatchContext, SyncSlotPqLevel0Quote, 2))
                .Verifiable();

            var dispatchContextDeserializerTimestamp = new DateTime(2017, 09, 23, 19, 47, 32);
            dispatchContext.DeserializerTimestamp = dispatchContextDeserializerTimestamp;

            MoqFlogger.Setup(fl => fl.Info("Unexpected sequence Id (#{0}) on stream {1}, PrevSeqID={2}, " +
                                           "RecvSeqID={3}, WakeUpTs={4}, DeserializeTs={5}, ReceivingTimestamp={6}", 
                It.IsAny<object[]>())) .Callback<string, object[]>( (strTemplt, strParams) =>
                {
                    Assert.AreEqual("Unexpected sequence Id (#{0}) on stream {1}, PrevSeqID={2}, RecvSeqID={3}, " +
                                    "WakeUpTs={4}, DeserializeTs={5}, ReceivingTimestamp={6}", strTemplt);
                    Assert.AreEqual(7, strParams.Length);
                    Assert.AreEqual(0, strParams[0]);
                    Assert.AreEqual(SourceTickerQuoteInfo, strParams[1]);
                    Assert.AreEqual(4u, strParams[2]);
                    Assert.AreEqual(2u, strParams[3]);
                    Assert.AreEqual(PQQuoteDeserializationSequencedTestDataBuilder.ClientReceivedTimestamp(
                            PQQuoteDeserializationSequencedTestDataBuilder.TimeOffsetForSequenceId(2))
                            .ToString(ExpectedDateFormat), strParams[4]);
                    Assert.AreEqual(dispatchContextDeserializerTimestamp.ToString(ExpectedDateFormat), strParams[5]);
                    Assert.AreEqual(PQQuoteDeserializationSequencedTestDataBuilder.RecevingTimestampBaseTime(
                            PQQuoteDeserializationSequencedTestDataBuilder.TimeOffsetForSequenceId(2))
                            .ToString(ExpectedDateFormat), strParams[6]);
                }).Verifiable();

            MoqFlogger.Setup(fl => fl.Info("Sequence anomaly detected on stream {0}, PrevSeqID={1}, RecvSeqID={2}, " +
                                    "WakeUpTs={3}, DeserializeTs={4}, ReceivingTimestamp={5}", It.IsAny<object[]>()))
                .Callback<string, object[]>((strTemplt, strParams) =>
                {
                    Assert.AreEqual(6, strParams.Length);
                    Assert.AreEqual(SourceTickerQuoteInfo, strParams[0]);
                    Assert.AreEqual(4u, strParams[1]);
                    Assert.AreEqual(2u, strParams[2]);
                    Assert.AreEqual(PQQuoteDeserializationSequencedTestDataBuilder.ClientReceivedTimestamp(
                        PQQuoteDeserializationSequencedTestDataBuilder.TimeOffsetForSequenceId(2)).ToString(ExpectedDateFormat),
                        strParams[3]);
                    Assert.AreEqual(dispatchContextDeserializerTimestamp.ToString(ExpectedDateFormat), strParams[4]);
                    Assert.AreEqual(PQQuoteDeserializationSequencedTestDataBuilder.RecevingTimestampBaseTime(
                        PQQuoteDeserializationSequencedTestDataBuilder.TimeOffsetForSequenceId(2)).ToString(ExpectedDateFormat),
                        strParams[5]);
                }).Verifiable();

            MoqPqQuoteStreamDeserializer.Setup(qsd => qsd.SwitchSyncState(QuoteSyncState.Synchronising)).Verifiable();
            MoqPqQuoteStreamDeserializer.Setup(qsd => qsd.OnOutOfSync(MoqPqQuoteStreamDeserializer.Object))
                .Verifiable();
            MoqPqQuoteStreamDeserializer.Setup(qsd => qsd.PushQuoteToSubscribers(PQSyncStatus.OutOfSync,
                MoqDispatchPerfLogger.Object)).Verifiable();
            
            syncState.ProcessInState(dispatchContext);
            MoqFlogger.Verify();
            MoqPqQuoteStreamDeserializer.Verify();
        }

        [TestMethod]
        public virtual void NewSyncState_HasJustGoneStale_CalssExpectedBehaviour()
        {
            var clientReceivedTime = new DateTime(2017, 09, 24, 23, 23, 05);
            DesersializerPqLevel0Quote.ClientReceivedTime = clientReceivedTime;
            
            MoqPqQuoteStreamDeserializer.Setup(qsd => qsd.SwitchSyncState(QuoteSyncState.Stale)).Verifiable();
            MoqPqQuoteStreamDeserializer.Setup(qsd => qsd.PushQuoteToSubscribers(PQSyncStatus.Stale, null))
                .Verifiable();

            MoqFlogger.Setup(fl => fl.Info("Stale detected on stream {0}, {1}ms elapsed with no update",
                It.IsAny<object[]>())).Callback<string, object[]>((strTemplt, strParams) =>
                {
                    Assert.AreEqual(2, strParams.Length);
                    Assert.AreEqual(SourceTickerQuoteInfo, strParams[0]);
                    Assert.AreEqual(2001, strParams[1]);
                }).Verifiable();

            Assert.IsFalse(syncState.HasJustGoneStale(clientReceivedTime));
            Assert.IsFalse(syncState.HasJustGoneStale(clientReceivedTime.AddSeconds(1)));
            Assert.IsFalse(syncState.HasJustGoneStale(clientReceivedTime.AddSeconds(2)));
            Assert.IsTrue(syncState.HasJustGoneStale(clientReceivedTime.AddMilliseconds(2001)));

            MoqFlogger.Verify();
            MoqPqQuoteStreamDeserializer.Verify();
        }

        private void SendUpdateSequenceId(uint sequenceId, PQFeedType feedType)
        {
            var deserializeInputList = QuoteSequencedTestDataBuilder.BuildSerializeContextForQuotes(ExpectedQuotes,
                feedType, sequenceId);
            var dispatchContext = deserializeInputList.First();

            MoqPqQuoteStreamDeserializer.Reset();
            SetupQuoteStreamDeserializerExpectations();
            
            MoqPqQuoteStreamDeserializer.Setup(qsd => qsd.UpdateQuote(dispatchContext, DesersializerPqLevel0Quote, sequenceId))
                .Verifiable();
            MoqPqQuoteStreamDeserializer.Setup(qsd => qsd.OnReceivedUpdate(MoqPqQuoteStreamDeserializer.Object))
                .Verifiable();
            MoqPqQuoteStreamDeserializer.Setup(qsd => qsd.PushQuoteToSubscribers(PQSyncStatus.Good,
                MoqDispatchPerfLogger.Object)).Verifiable();
            
            syncState.ProcessInState(dispatchContext);
            
            MoqPqQuoteStreamDeserializer.Verify();
        }

        private void InSyncUpdate_ProcessUnsyncedUpdateMessageInPast_LogsAnomaly()
        {
            var deserializeInputList = QuoteSequencedTestDataBuilder.BuildSerializeContextForQuotes(ExpectedQuotes,
                PQFeedType.Update, 2);
            var dispatchContext = deserializeInputList.First();
            DesersializerPqLevel0Quote.PQSequenceId = 4;
            MoqPqQuoteStreamDeserializer.SetupGet(qsd => qsd.AllowUpdatesCatchup).Returns(true).Verifiable();

            MoqFlogger.Setup(fl => fl.Info("Sequence anomaly ignored on stream {0}, PrevSeqID={1}, RecvSeqID={2}", 
                It.IsAny<object[]>())) .Callback<string, object[]>((strTemplt, strParams) =>
            {
                Assert.AreEqual(3, strParams.Length);
                Assert.AreEqual(SourceTickerQuoteInfo, strParams[0]);
                Assert.AreEqual(4u, strParams[1]);
                Assert.AreEqual(2u, strParams[2]);
            }).Verifiable();
            
            syncState.ProcessInState(dispatchContext);

            MoqFlogger.Verify();
            MoqPqQuoteStreamDeserializer.Verify();
        }
    }
}