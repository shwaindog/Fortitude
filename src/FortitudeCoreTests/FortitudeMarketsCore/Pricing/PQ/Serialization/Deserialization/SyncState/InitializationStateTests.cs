#region

using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization.SyncState;
using FortitudeMarketsCore.Pricing.PQ.Subscription;
using Moq;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization.SyncState;

[TestClass]
public class InitializationStateTests : SynchronisingStateTests
{
    protected override QuoteSyncState ExpectedQuoteState => QuoteSyncState.InitializationState;

    protected override void BuildSyncState()
    {
        syncState = new InitializationState<PQLevel0Quote>(MoqPqQuoteStreamDeserializer.Object);
    }

    [TestMethod]
    public override void NewSyncState_ProcessInStateProcessNextExpectedUpdate_CallsExpectedBehaviour()
    {
        var deserializeInputList = QuoteSequencedTestDataBuilder.BuildSerializeContextForQuotes(ExpectedQuotes,
            PQFeedType.Update, 0);
        var dispatchContext = deserializeInputList.First();
        uint mistmatchedSeqId;

        MoqPqQuoteStreamDeserializer.Setup(qsd => qsd.Synchronize(out mistmatchedSeqId))
            .Returns(true).Verifiable();
        MoqPqQuoteStreamDeserializer.Setup(qsd => qsd.UpdateQuote(dispatchContext, DesersializerPqLevel0Quote, 0))
            .Verifiable();

        MoqPqQuoteStreamDeserializer.Setup(qsd => qsd.OnSyncOk(MoqPqQuoteStreamDeserializer.Object)).Verifiable();
        MoqPqQuoteStreamDeserializer.Setup(qsd => qsd.SwitchSyncState(QuoteSyncState.InSync)).Verifiable();
        MoqPqQuoteStreamDeserializer.Setup(qsd => qsd.PushQuoteToSubscribers(PQSyncStatus.Good,
            MoqDispatchPerfLogger.Object)).Verifiable();


        MoqFlogger.Setup(fl => fl.Info(It.IsAny<string>(), It.IsAny<object[]>())).Callback<string, object[]>(
            (strTemplt, strParams) =>
            {
                Assert.AreEqual("Stream {0} started from first message, RecvSeqID={1}",
                    strTemplt);
                Assert.AreEqual(2, strParams.Length);
                Assert.AreEqual(SourceTickerQuoteInfo, strParams[0]);
                Assert.AreEqual(0u, strParams[1]);
            }).Verifiable();

        syncState.ProcessInState(dispatchContext);

        MoqFlogger.Verify();
        MoqPqQuoteStreamDeserializer.Verify();
    }

    [TestMethod]
    public override void NewSyncState_ProcessInStateProcessNextExpectedUpdateCantSync_LogsProblem()
    {
        var deserializeInputList = QuoteSequencedTestDataBuilder.BuildSerializeContextForQuotes(ExpectedQuotes,
            PQFeedType.Update, 0);
        var dispatchContext = deserializeInputList.First();

        uint mistmatchedSeqId;

        MoqPqQuoteStreamDeserializer.Setup(qsd => qsd.Synchronize(out mistmatchedSeqId))
            .Returns(false).Verifiable();
        MoqPqQuoteStreamDeserializer.Setup(qsd => qsd.UpdateQuote(dispatchContext, DesersializerPqLevel0Quote, 0))
            .Verifiable();

        MoqFlogger.Setup(fl => fl.Info(It.IsAny<string>(), It.IsAny<object[]>())).Callback<string, object[]>(
            (strTemplt, strParams) =>
            {
                Assert.AreEqual("Stream {0} could not recover after sequence anomaly, " +
                                "PrevSeqId={1}, RecvSeqID={2}, MismatchedId={3}", strTemplt);
                Assert.AreEqual(4, strParams.Length);
                Assert.AreEqual(SourceTickerQuoteInfo, strParams[0]);
                Assert.AreEqual(0u, strParams[1]);
                Assert.AreEqual(0u, strParams[2]);
                Assert.AreEqual(0u, strParams[3]);
            }).Verifiable();

        syncState.ProcessInState(dispatchContext);

        MoqFlogger.Verify();
        MoqPqQuoteStreamDeserializer.Verify();
    }

    [TestMethod]
    public override void NewSyncState_ProcessSnapshot_CallsExpectedBehaviour()
    {
        var deserializeInputList = QuoteSequencedTestDataBuilder.BuildSerializeContextForQuotes(ExpectedQuotes,
            PQFeedType.Snapshot, 0);
        var dispatchContext = deserializeInputList.First();

        uint mistmatchedSeqId;

        MoqPqQuoteStreamDeserializer.Setup(qsd => qsd.Synchronize(out mistmatchedSeqId))
            .Returns(true).Verifiable();
        MoqPqQuoteStreamDeserializer.Setup(qsd => qsd.UpdateQuote(dispatchContext, DesersializerPqLevel0Quote, 0))
            .Verifiable();

        MoqPqQuoteStreamDeserializer.Setup(qsd => qsd.OnSyncOk(MoqPqQuoteStreamDeserializer.Object)).Verifiable();
        MoqPqQuoteStreamDeserializer.Setup(qsd => qsd.SwitchSyncState(QuoteSyncState.InSync)).Verifiable();
        MoqPqQuoteStreamDeserializer.Setup(qsd => qsd.PushQuoteToSubscribers(PQSyncStatus.Good,
            MoqDispatchPerfLogger.Object)).Verifiable();


        MoqFlogger.Setup(fl => fl.Info(It.IsAny<string>(), It.IsAny<object[]>())).Callback<string, object[]>(
            (strTemplt, strParams) =>
            {
                Assert.AreEqual("Stream {0} recovered after snapshot, PrevSeqId={1}, SnapshotSeqId={2}, " +
                                "LastUpdateSeqId={3}", strTemplt);
                Assert.AreEqual(4, strParams.Length);
                Assert.AreEqual(SourceTickerQuoteInfo, strParams[0]);
                Assert.AreEqual(0u, strParams[1]);
                Assert.AreEqual(0u, strParams[2]);
                Assert.AreEqual(0u, strParams[3]);
            }).Verifiable();

        syncState.ProcessInState(dispatchContext);

        MoqFlogger.Verify();
        MoqPqQuoteStreamDeserializer.Verify();
    }


    [TestMethod]
    public override void NewSyncState_ProcessInStateProcessSnapshotCantSync_LogsProblem()
    {
        var deserializeInputList = QuoteSequencedTestDataBuilder.BuildSerializeContextForQuotes(ExpectedQuotes,
            PQFeedType.Snapshot, 0);
        var dispatchContext = deserializeInputList.First();
        uint mistmatchedSeqId;

        MoqPqQuoteStreamDeserializer.Setup(qsd => qsd.Synchronize(out mistmatchedSeqId))
            .Returns(false).Verifiable();
        MoqPqQuoteStreamDeserializer.Setup(qsd => qsd.UpdateQuote(dispatchContext, DesersializerPqLevel0Quote, 0))
            .Verifiable();

        MoqFlogger.Setup(fl => fl.Info(It.IsAny<string>(), It.IsAny<object[]>())).Callback<string, object[]>(
            (strTemplt, strParams) =>
            {
                Assert.AreEqual("Stream {0} could not recover after snapshot, " +
                                "PrevSeqId={1}, SnapshotSeqId={2}, MismatchedId={3}", strTemplt);
                Assert.AreEqual(4, strParams.Length);
                Assert.AreEqual(SourceTickerQuoteInfo, strParams[0]);
                Assert.AreEqual(0u, strParams[1]);
                Assert.AreEqual(0u, strParams[2]);
                Assert.AreEqual(0u, strParams[3]);
            }).Verifiable();

        syncState.ProcessInState(dispatchContext);

        MoqFlogger.Verify();
        MoqPqQuoteStreamDeserializer.Verify();
    }
}
