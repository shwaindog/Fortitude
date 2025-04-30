// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarkets.Pricing.PQ.Messages.Quotes;
using FortitudeMarkets.Pricing.PQ.Serdes.Deserialization.SyncState;
using Moq;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Serdes.Deserialization.SyncState;

[TestClass]
public class InitializationStateTests : SynchronisingStateTests
{
    protected override QuoteSyncState ExpectedQuoteState => QuoteSyncState.InitializationState;

    protected override void BuildSyncState()
    {
        syncState = new InitializationState<PQTickInstant>(pqQuoteStreamDeserializer);
    }

    [TestMethod]
    public override void NewSyncState_ProcessInStateProcessNextExpectedUpdate_CallsExpectedBehaviour()
    {
        var deserializeInputList = QuoteSequencedTestDataBuilder.BuildSerializeContextForQuotes
            (ExpectedQuotes, PQMessageFlags.Update, 0); // sequenceId will roll over to 0 as is incremented during serialization;
        var sockBuffContext = deserializeInputList.First();
        MoqFlogger.Setup(fl => fl.Info(It.IsAny<string>(), It.IsAny<object[]>())).Callback<string, object[]>(
         (strTemplt, strParams) =>
         {
             Assert.AreEqual("Stream {0} started from first message, RecvSeqID={1}",
                             strTemplt);
             Assert.AreEqual(2, strParams.Length);
             Assert.AreEqual(SourceTickerInfo, strParams[0]);
             Assert.AreEqual(0u, strParams[1]);
         }).Verifiable();

        syncState.ProcessInState(sockBuffContext);

        MoqFlogger.Verify();
    }

    [TestMethod]
    public override void NewSyncState_ProcessInStateProcessNextExpectedUpdateCantSync_LogsProblem()
    {
        var deserializeInputList = QuoteSequencedTestDataBuilder.BuildSerializeContextForQuotes
            (ExpectedQuotes, PQMessageFlags.Update, 10);
        var sockBuffContext = deserializeInputList.First();
        syncState.ProcessInState(sockBuffContext);

        MoqFlogger.Setup(fl => fl.Info(It.IsAny<string>(), It.IsAny<object[]>())).Callback<string, object[]>(
         (strTemplt, strParams) =>
         {
             Assert.AreEqual("Stream {0} could not recover after sequence anomaly, " +
                             "PrevSeqId={1}, RecvSeqID={2}, MismatchedId={3}", strTemplt);
             Assert.AreEqual(4, strParams.Length);
             Assert.AreEqual(SourceTickerInfo, strParams[0]);
             Assert.AreEqual(0u, strParams[1]);
             Assert.AreEqual(0u, strParams[2]);
             Assert.AreEqual(10u, strParams[3]);
         }).Verifiable();
        deserializeInputList = QuoteSequencedTestDataBuilder.BuildSerializeContextForQuotes
            (ExpectedQuotes, PQMessageFlags.Update, uint.MaxValue); // will roll over to SequenceId = 0 on update generation.
        sockBuffContext = deserializeInputList.First();
        syncState.ProcessInState(sockBuffContext);

        MoqFlogger.Verify();
    }

    [TestMethod]
    public override void NewSyncState_ProcessSnapshot_CallsExpectedBehaviour()
    {
        var deserializeInputList = QuoteSequencedTestDataBuilder.BuildSerializeContextForQuotes
            (ExpectedQuotes, PQMessageFlags.Snapshot, 0);
        var sockBuffContext = deserializeInputList.First();

        MoqFlogger.Setup(fl => fl.Info(It.IsAny<string>(), It.IsAny<object[]>())).Callback<string, object[]>(
         (strTemplt, strParams) =>
         {
             Assert.AreEqual("Stream {0} recovered after snapshot, PrevSeqId={1}, SnapshotSeqId={2}, " +
                             "LastUpdateSeqId={3}", strTemplt);
             Assert.AreEqual(4, strParams.Length);
             Assert.AreEqual(SourceTickerInfo, strParams[0]);
             Assert.AreEqual(0u, strParams[1]);
             Assert.AreEqual(0u, strParams[2]);
             Assert.AreEqual(0u, strParams[3]);
         }).Verifiable();

        syncState.ProcessInState(sockBuffContext);

        MoqFlogger.Verify();
    }

    [TestMethod]
    public void NewSyncState_ProcessInStateProcessSnapshotCantSync_LogsProblem()
    {
        var deserializeInputList = QuoteSequencedTestDataBuilder.BuildSerializeContextForQuotes
            (ExpectedQuotes, PQMessageFlags.Update, 10);
        var sockBuffContext = deserializeInputList.First();
        syncState.ProcessInState(sockBuffContext);

        MoqFlogger.Setup(fl => fl.Info(It.IsAny<string>(), It.IsAny<object[]>())).Callback<string, object[]>(
         (strTemplt, strParams) =>
         {
             Assert.AreEqual("Stream {0} could not recover after snapshot, " +
                             "PrevSeqId={1}, SnapshotSeqId={2}, MismatchedId={3}", strTemplt);
             Assert.AreEqual(4, strParams.Length);
             Assert.AreEqual(SourceTickerInfo, strParams[0]);
             Assert.AreEqual(0u, strParams[1]);
             Assert.AreEqual(0u, strParams[2]);
             Assert.AreEqual(10u, strParams[3]);
         }).Verifiable();

        deserializeInputList = QuoteSequencedTestDataBuilder.BuildSerializeContextForQuotes
            (ExpectedQuotes, PQMessageFlags.Snapshot, 0);
        sockBuffContext = deserializeInputList.First();

        syncState.ProcessInState(sockBuffContext);

        MoqFlogger.Verify();
    }
}
