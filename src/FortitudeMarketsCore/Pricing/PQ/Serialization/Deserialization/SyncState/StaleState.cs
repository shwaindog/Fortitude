#region

using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Quotes;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization.SyncState;

public class StaleState<T> : InSyncState<T> where T : PQLevel0Quote, new()
{
    public StaleState(IPQQuoteDeserializer<T> linkedDeserializer)
        : base(linkedDeserializer, QuoteSyncState.Stale) { }

    protected override void ProcessNextExpectedUpdate(DispatchContext dispatchContext, uint sequenceId)
    {
        LinkedDeserializer.UpdateQuote(dispatchContext, LinkedDeserializer.PublishedQuote, sequenceId);
        Logger.Info("Stream {0} recovered after timeout, RecvSeqID={1}", LinkedDeserializer.Identifier,
            sequenceId);
        SwitchState(QuoteSyncState.InSync);
        PublishQuoteRunAction(PQSyncStatus.Good, dispatchContext.DispatchLatencyLogger,
            LinkedDeserializer.OnSyncOk);
    }

    public override bool HasJustGoneStale(DateTime utcNow) => false;
}
