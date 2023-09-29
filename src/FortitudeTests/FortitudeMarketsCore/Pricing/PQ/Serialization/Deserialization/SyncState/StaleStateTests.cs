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
    public class StaleStateTests : InSyncStateTests
    {
        protected override void BuildSyncState()
        {
            syncState = new StaleState<IPQLevel0Quote>(MoqPqQuoteStreamDeserializer.Object);
        }

        protected override QuoteSyncState ExpectedQuoteState => QuoteSyncState.Stale;

        [TestMethod]
        public override void NewSyncState_ProcessInStateProcessNextExpectedUpdate_CallsExpectedBehaviour()
        {
            var deserializeInputList = QuoteSequencedTestDataBuilder.BuildSerializeContextForQuotes(ExpectedQuotes,
                PQFeedType.Update, 1);
            var dispatchContext = deserializeInputList.First();

            MoqPqQuoteStreamDeserializer.Reset();
            SetupQuoteStreamDeserializerExpectations();

            MoqPqQuoteStreamDeserializer.Setup(qsd => qsd.SwitchSyncState(QuoteSyncState.InSync)).Verifiable();
            MoqPqQuoteStreamDeserializer.Setup(qsd => qsd.UpdateQuote(dispatchContext, DesersializerPqLevel0Quote, 1))
                .Verifiable();
            MoqPqQuoteStreamDeserializer.Setup(qsd => qsd.OnSyncOk(MoqPqQuoteStreamDeserializer.Object))
                .Verifiable();
            MoqPqQuoteStreamDeserializer.Setup(qsd => qsd.PushQuoteToSubscribers(PQSyncStatus.Good,
                MoqDispatchPerfLogger.Object)).Verifiable();

            MoqFlogger.Setup(fl => fl.Info("Stream {0} recovered after timeout, RecvSeqID={1}",
                It.IsAny<object[]>())).Callback<string, object[]>((strTemplt, strParams) =>
                {
                    Assert.AreEqual(2, strParams.Length);
                    Assert.AreEqual(SourceTickerQuoteInfo, strParams[0]);
                    Assert.AreEqual(1u, strParams[1]);
                }).Verifiable();
            
            syncState.ProcessInState(dispatchContext);

            MoqFlogger.Verify();
            MoqPqQuoteStreamDeserializer.Verify();
        }

        [TestMethod]
        public override void NewSyncState_HasJustGoneStale_CalssExpectedBehaviour()
        {
            var clientReceivedTime = new DateTime(2017, 09, 24, 23, 23, 05);
            DesersializerPqLevel0Quote.ClientReceivedTime = clientReceivedTime;
            
            Assert.IsFalse(syncState.HasJustGoneStale(clientReceivedTime));
            Assert.IsFalse(syncState.HasJustGoneStale(clientReceivedTime.AddSeconds(1)));
            Assert.IsFalse(syncState.HasJustGoneStale(clientReceivedTime.AddSeconds(2)));
            Assert.IsFalse(syncState.HasJustGoneStale(clientReceivedTime.AddMilliseconds(2001)));
            Assert.IsFalse(syncState.HasJustGoneStale(DateTime.MaxValue));
            
            MoqPqQuoteStreamDeserializer.Verify();
        }
    }
}