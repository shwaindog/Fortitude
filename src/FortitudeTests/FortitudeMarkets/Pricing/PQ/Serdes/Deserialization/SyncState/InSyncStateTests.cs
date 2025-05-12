// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Types;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes;
using FortitudeMarkets.Pricing.PQ.Serdes.Deserialization.SyncState;
using Moq;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Serdes.Deserialization.SyncState;

[TestClass]
public class InSyncStateTests : SyncStateBaseTests
{
    protected override QuoteSyncState ExpectedQuoteState => QuoteSyncState.InSync;

    protected override void BuildSyncState()
    {
        syncState = new InSyncState<PQPublishableTickInstant>(pqQuoteStreamDeserializer);
    }

    [TestMethod]
    public override void NewSyncState_ProcessInStateProcessNextExpectedUpdate_CallsExpectedBehaviour()
    {
        //can received the same update twice.
        SendUpdateSequenceId(0, PQMessageFlags.Update);
        foreach (var expectedQuote in ExpectedQuotes) expectedQuote.SourceTime = expectedQuote.SourceTime.AddSeconds(2);
        SendUpdateSequenceId(1, PQMessageFlags.Update);
        SendUpdateSequenceId(1, PQMessageFlags.Update);

        InSyncUpdate_ProcessUnsyncedUpdateMessageInPast_LogsAnomaly();
    }

    [TestMethod]
    public override void NewSyncState_ProcessUnsyncedUpdateMessage_CallsExpectedBehaviour()
    {
        var deserializeInputList = QuoteSequencedTestDataBuilder.BuildSerializeContextForQuotes(ExpectedQuotes,
                                                                                                PQMessageFlags.Update, 2);
        var sockBuffContext = deserializeInputList.First();

        DesersializerPqTickInstant.PQSequenceId = 4;

        var sockBuffContextDeserializerTimestamp = new DateTime(2017, 09, 23, 19, 47, 32);
        sockBuffContext.DeserializerTime = sockBuffContextDeserializerTimestamp;

        MoqFlogger.Setup
            (fl => fl.Info("Unexpected sequence Id (#{0}) on stream {1}, PrevSeqID={2}, " +
                           "RecvSeqID={3}, WakeUpTs={4}, DeserializeTs={5}, ReceivingTimestamp={6}"
                         , It.IsAny<object[]>())).Callback<string, object[]>((strTemplt, strParams) =>
        {
            Assert.AreEqual("Unexpected sequence Id (#{0}) on stream {1}, PrevSeqID={2}, RecvSeqID={3}, " +
                            "WakeUpTs={4}, DeserializeTs={5}, ReceivingTimestamp={6}", strTemplt);
            Assert.AreEqual(7, strParams.Length);
            Assert.AreEqual(0, strParams[0]);
            Assert.AreEqual(SourceTickerInfo, strParams[1]);
            Assert.AreEqual(4u, strParams[2]);
            Assert.AreEqual(2u, strParams[3]);
            Assert.AreEqual
                (PQQuoteDeserializationSequencedTestDataBuilder
                 .ClientReceivedTimestamp
                     (PQQuoteDeserializationSequencedTestDataBuilder.TimeOffsetForSequenceId(2))
                 .ToString(ExpectedDateFormat), strParams[4]);
            Assert.AreEqual(sockBuffContextDeserializerTimestamp.ToString(ExpectedDateFormat), strParams[5]);
            Assert.AreEqual(PQQuoteDeserializationSequencedTestDataBuilder
                            .RecevingTimestampBaseTime
                                (PQQuoteDeserializationSequencedTestDataBuilder.TimeOffsetForSequenceId(2))
                            .ToString(ExpectedDateFormat), strParams[6]);
        }).Verifiable();

        MoqFlogger.Setup(fl => fl.Info("Sequence anomaly detected on stream {0}, PrevSeqID={1}, RecvSeqID={2}, " +
                                       "WakeUpTs={3}, DeserializeTs={4}, ReceivingTimestamp={5}", It.IsAny<object[]>()))
                  .Callback<string, object[]>((strTemplt, strParams) =>
                  {
                      Assert.AreEqual(6, strParams.Length);
                      Assert.AreEqual(SourceTickerInfo, strParams[0]);
                      Assert.AreEqual(4u, strParams[1]);
                      Assert.AreEqual(2u, strParams[2]);
                      Assert.AreEqual(PQQuoteDeserializationSequencedTestDataBuilder
                                      .ClientReceivedTimestamp
                                          (PQQuoteDeserializationSequencedTestDataBuilder.TimeOffsetForSequenceId(2))
                                      .ToString(ExpectedDateFormat), strParams[3]);
                      Assert.AreEqual(sockBuffContextDeserializerTimestamp.ToString(ExpectedDateFormat), strParams[4]);
                      Assert.AreEqual(PQQuoteDeserializationSequencedTestDataBuilder
                                      .RecevingTimestampBaseTime
                                          (PQQuoteDeserializationSequencedTestDataBuilder.TimeOffsetForSequenceId(2))
                                      .ToString(ExpectedDateFormat), strParams[5]);
                  }).Verifiable();

        syncState.ProcessInState(sockBuffContext);
        MoqFlogger.Verify();
    }

    [TestMethod]
    public virtual void NewSyncState_HasJustGoneStale_CallsExpectedBehaviour()
    {
        var clientReceivedTime = new DateTime(2017, 09, 24, 23, 23, 05);
        DesersializerPqTickInstant.ClientReceivedTime = clientReceivedTime;

        MoqFlogger.Setup(fl => fl.Info("Stale detected on stream {0}, {1}ms elapsed with no update",
                                       It.IsAny<object[]>())).Callback<string, object[]>((strTemplt, strParams) =>
        {
            Assert.AreEqual(2, strParams.Length);
            Assert.AreEqual(SourceTickerInfo, strParams[0]);
            Assert.AreEqual(2001, strParams[1]);
        }).Verifiable();

        Assert.IsFalse(syncState.HasJustGoneStale(clientReceivedTime));
        Assert.IsFalse(syncState.HasJustGoneStale(clientReceivedTime.AddSeconds(1)));
        Assert.IsFalse(syncState.HasJustGoneStale(clientReceivedTime.AddSeconds(2)));
        Assert.IsTrue(syncState.HasJustGoneStale(clientReceivedTime.AddMilliseconds(2001)));

        MoqFlogger.Verify();
    }

    private void SendUpdateSequenceId(uint sequenceId, PQMessageFlags feedType)
    {
        var deserializeInputList = QuoteSequencedTestDataBuilder.BuildSerializeContextForQuotes
            (ExpectedQuotes, feedType, sequenceId);
        var sockBuffContext = deserializeInputList.First();

        syncState.ProcessInState(sockBuffContext);
    }

    private void InSyncUpdate_ProcessUnsyncedUpdateMessageInPast_LogsAnomaly()
    {
        var deserializeInputList = QuoteSequencedTestDataBuilder.BuildSerializeContextForQuotes
            (ExpectedQuotes, PQMessageFlags.Snapshot, 8);
        var sockBuffContext = deserializeInputList.First();
        syncState.ProcessInState(sockBuffContext);

        SendPqTickInstant.HasUpdates = true;
        deserializeInputList = QuoteSequencedTestDataBuilder.BuildSerializeContextForQuotes
            (ExpectedQuotes, PQMessageFlags.Update, 4);
        sockBuffContext = deserializeInputList.First();

        MoqFlogger.Setup(fl => fl.Info("Sequence anomaly ignored on stream {0}, PrevSeqID={1}, RecvSeqID={2}",
                                       It.IsAny<object[]>())).Callback<string, object[]>((strTemplt, strParams) =>
        {
            Assert.AreEqual(3, strParams.Length);
            Assert.AreEqual(SourceTickerInfo, strParams[0]);
            Assert.AreEqual(8u, strParams[1]);
            Assert.AreEqual(4u, strParams[2]);
        }).Verifiable();

        NonPublicInvocator.SetInstanceField(syncState, "LogCounter", 0);
        syncState.ProcessInState(sockBuffContext);

        MoqFlogger.Verify();
    }
}
