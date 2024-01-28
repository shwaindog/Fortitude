#region

using FortitudeMarketsCore.Pricing.PQ.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization.SyncState;
using FortitudeMarketsCore.Pricing.PQ.Subscription;
using Moq;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization.SyncState;

[TestClass]
public class StaleStateTests : InSyncStateTests
{
    protected override QuoteSyncState ExpectedQuoteState => QuoteSyncState.Stale;

    protected override void BuildSyncState()
    {
        syncState = new StaleState<PQLevel0Quote>(pqQuoteStreamDeserializer);
    }

    [TestMethod]
    public override void NewSyncState_ProcessInStateProcessNextExpectedUpdate_CallsExpectedBehaviour()
    {
        var deserializeInputList = QuoteSequencedTestDataBuilder.BuildSerializeContextForQuotes(ExpectedQuotes,
            PQFeedType.Update, uint.MaxValue);
        var dispatchContext = deserializeInputList.First();

        SetupQuoteStreamDeserializerExpectations();

        MoqFlogger.Setup(fl => fl.Info("Stream {0} recovered after timeout, RecvSeqID={1}",
            It.IsAny<object[]>())).Callback<string, object[]>((strTemplt, strParams) =>
        {
            Assert.AreEqual(2, strParams.Length);
            Assert.AreEqual(SourceTickerQuoteInfo, strParams[0]);
            Assert.AreEqual(0u, strParams[1]);
        }).Verifiable();

        syncState.ProcessInState(dispatchContext);

        MoqFlogger.Verify();
    }

    [TestMethod]
    public override void NewSyncState_HasJustGoneStale_CallsExpectedBehaviour()
    {
        var clientReceivedTime = new DateTime(2017, 09, 24, 23, 23, 05);
        DesersializerPqLevel0Quote.ClientReceivedTime = clientReceivedTime;

        Assert.IsFalse(syncState.HasJustGoneStale(clientReceivedTime));
        Assert.IsFalse(syncState.HasJustGoneStale(clientReceivedTime.AddSeconds(1)));
        Assert.IsFalse(syncState.HasJustGoneStale(clientReceivedTime.AddSeconds(2)));
        Assert.IsFalse(syncState.HasJustGoneStale(clientReceivedTime.AddMilliseconds(2001)));
        Assert.IsFalse(syncState.HasJustGoneStale(DateTime.MaxValue));
    }
}
