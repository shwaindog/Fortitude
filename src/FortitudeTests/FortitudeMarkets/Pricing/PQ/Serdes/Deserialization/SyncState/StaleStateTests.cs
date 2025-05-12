// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes;
using FortitudeMarkets.Pricing.PQ.Serdes.Deserialization.SyncState;
using Moq;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Serdes.Deserialization.SyncState;

[TestClass]
public class StaleStateTests : InSyncStateTests
{
    protected override QuoteSyncState ExpectedQuoteState => QuoteSyncState.Stale;

    protected override void BuildSyncState()
    {
        syncState = new StaleState<PQPublishableTickInstant>(pqQuoteStreamDeserializer);
    }

    [TestMethod]
    public override void NewSyncState_ProcessInStateProcessNextExpectedUpdate_CallsExpectedBehaviour()
    {
        var deserializeInputList = QuoteSequencedTestDataBuilder.BuildSerializeContextForQuotes
            (ExpectedQuotes, PQMessageFlags.Update, 0);
        var sockBuffContext = deserializeInputList.First();

        MoqFlogger.Setup(fl => fl.Info("Stream {0} recovered after timeout, RecvSeqID={1}",
                                       It.IsAny<object[]>())).Callback<string, object[]>((strTemplt, strParams) =>
        {
            Assert.AreEqual(2, strParams.Length);
            Assert.AreEqual(SourceTickerInfo, strParams[0]);
            Assert.AreEqual(0u, strParams[1]);
        }).Verifiable();

        syncState.ProcessInState(sockBuffContext);

        MoqFlogger.Verify();
    }

    [TestMethod]
    public override void NewSyncState_HasJustGoneStale_CallsExpectedBehaviour()
    {
        var clientReceivedTime = new DateTime(2017, 09, 24, 23, 23, 05);
        DesersializerPqTickInstant.ClientReceivedTime = clientReceivedTime;

        Assert.IsFalse(syncState.HasJustGoneStale(clientReceivedTime));
        Assert.IsFalse(syncState.HasJustGoneStale(clientReceivedTime.AddSeconds(1)));
        Assert.IsFalse(syncState.HasJustGoneStale(clientReceivedTime.AddSeconds(2)));
        Assert.IsFalse(syncState.HasJustGoneStale(clientReceivedTime.AddMilliseconds(2001)));
        Assert.IsFalse(syncState.HasJustGoneStale(DateTime.MaxValue));
    }
}
