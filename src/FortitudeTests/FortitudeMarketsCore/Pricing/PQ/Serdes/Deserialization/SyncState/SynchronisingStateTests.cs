#region

using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Serdes.Deserialization.SyncState;
using Moq;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serdes.Deserialization.SyncState;

[TestClass]
public class SynchronisingStateTests : SyncStateBaseTests
{
    protected override QuoteSyncState ExpectedQuoteState => QuoteSyncState.Synchronising;

    protected override void BuildSyncState()
    {
        syncState = new SynchronisingState<PQLevel0Quote>(pqQuoteStreamDeserializer);
    }

    [TestMethod]
    public override void NewSyncState_EligibleForResync_ReturnsExpected()
    {
        Assert.IsFalse(syncState.EligibleForResync(new DateTime()));
        Assert.IsTrue(syncState.EligibleForResync(new DateTime(2017, 09, 23, 22, 45, 51)));
        Assert.IsFalse(syncState.EligibleForResync(new DateTime(2017, 09, 23, 22, 45, 52)));
        Assert.IsTrue(syncState.EligibleForResync(new DateTime(2017, 09, 23, 22, 45, 53)));
        Assert.IsTrue(syncState.EligibleForResync(DateTime.MaxValue));
    }

    [TestMethod]
    public override void NewSyncState_ProcessInStateProcessNextExpectedUpdate_CallsExpectedBehaviour()
    {
        var deserializeInputList = QuoteSequencedTestDataBuilder.BuildSerializeContextForQuotes(ExpectedQuotes,
            PQMessageFlags.Update, 1);
        var sockBuffContext = deserializeInputList.First();

        MoqFlogger.Setup(fl => fl.Info(It.IsAny<string>(), It.IsAny<object[]>())).Callback<string, object[]>(
            (strTemplt, strParams) =>
            {
                Assert.AreEqual("Stream {0} recovered after message sequence out of order, RecvSeqID={1}",
                    strTemplt);
                Assert.AreEqual(2, strParams.Length);
                Assert.AreEqual(SourceTickerQuoteInfo, strParams[0]);
                Assert.AreEqual(1u, strParams[1]);
            });

        syncState.ProcessInState(sockBuffContext);
        MoqFlogger.Verify();
    }

    [TestMethod]
    public virtual void NewSyncState_ProcessInStateProcessNextExpectedUpdateCantSync_LogsProblem()
    {
        var deserializeInputList = QuoteSequencedTestDataBuilder.BuildSerializeContextForQuotes(ExpectedQuotes,
            PQMessageFlags.Update, 2);
        var sockBuffContext = deserializeInputList.First();
        syncState.ProcessInState(sockBuffContext);

        SendPqLevel0Quote.HasUpdates = true;
        deserializeInputList = QuoteSequencedTestDataBuilder.BuildSerializeContextForQuotes(ExpectedQuotes,
            PQMessageFlags.Update, uint.MaxValue);
        sockBuffContext = deserializeInputList.First();

        MoqFlogger.Setup(fl => fl.Info(It.IsAny<string>(), It.IsAny<object[]>())).Callback<string, object[]>(
            (strTemplt, strParams) =>
            {
                Assert.AreEqual("Stream {0} could not recover after sequence anomaly, " +
                                "PrevSeqId={1}, RecvSeqID={2}, MismatchedId={3}", strTemplt);
                Assert.AreEqual(4, strParams.Length);
                Assert.AreEqual(SourceTickerQuoteInfo, strParams[0]);
                Assert.AreEqual(0u, strParams[1]);
                Assert.AreEqual(0u, strParams[2]);
                Assert.AreEqual(3u, strParams[3]);
            }).Verifiable();

        syncState.ProcessInState(sockBuffContext);

        MoqFlogger.Verify();
    }

    public override void NewSyncState_ProcessUnsyncedUpdateMessage_CallsExpectedBehaviour()
    {
        base.NewSyncState_ProcessUnsyncedUpdateMessage_CallsExpectedBehaviour();

        var deserializeInputList = QuoteSequencedTestDataBuilder.BuildSerializeContextForQuotes(ExpectedQuotes,
            PQMessageFlags.Update, 1);
        var sockBuffContext = deserializeInputList.First();

        syncState.ProcessInState(sockBuffContext);
    }

    [TestMethod]
    public override void NewSyncState_ProcessSnapshot_CallsExpectedBehaviour()
    {
        pqQuoteStreamDeserializer.PublishedQuote.PQSequenceId = 0;
        var deserializeInputList = QuoteSequencedTestDataBuilder.BuildSerializeContextForQuotes(ExpectedQuotes,
            PQMessageFlags.Snapshot, 2);
        var sockBuffContext = deserializeInputList.First();

        MoqFlogger.Setup(fl => fl.Info(It.IsAny<string>(), It.IsAny<object[]>())).Callback<string, object[]>(
            (strTemplt, strParams) =>
            {
                Assert.AreEqual("Stream {0} recovered after snapshot, PrevSeqId={1}, SnapshotSeqId={2}, " +
                                "LastUpdateSeqId={3}", strTemplt);
                Assert.AreEqual(4, strParams.Length);
                Assert.AreEqual(SourceTickerQuoteInfo, strParams[0]);
                Assert.AreEqual(0u, strParams[1]);
                Assert.AreEqual(2u, strParams[2]);
                Assert.AreEqual(0u, strParams[3]);
            }).Verifiable();

        syncState.ProcessInState(sockBuffContext);

        MoqFlogger.Verify();
    }

    [TestMethod]
    public virtual void NewSyncState_ProcessInStateProcessSnapshotMovesToSnapshot_LogsRecovery()
    {
        var deserializeInputList = QuoteSequencedTestDataBuilder.BuildSerializeContextForQuotes(ExpectedQuotes,
            PQMessageFlags.Snapshot, 2);
        var sockBuffContext = deserializeInputList.First();

        MoqFlogger.Setup(fl => fl.Info(It.IsAny<string>(), It.IsAny<object[]>())).Callback<string, object[]>(
            (strTemplt, strParams) =>
            {
                Assert.AreEqual("Stream {0} recovered after snapshot, PrevSeqId={1}," +
                                " SnapshotSeqId={2}, LastUpdateSeqId={3}", strTemplt);
                Assert.AreEqual(4, strParams.Length);
                Assert.AreEqual(SourceTickerQuoteInfo, strParams[0]);
                Assert.AreEqual(0u, strParams[1]);
                Assert.AreEqual(2u, strParams[2]);
                Assert.AreEqual(0u, strParams[3]);
            }).Verifiable();

        syncState.ProcessInState(sockBuffContext);

        MoqFlogger.Verify();
    }
}
