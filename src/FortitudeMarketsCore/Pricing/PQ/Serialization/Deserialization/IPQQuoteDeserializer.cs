#region

using FortitudeCommon.Monitoring.Logging.Diagnostics.Performance;
using FortitudeCommon.Serdes.Binary;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization.SyncState;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization;

public interface IPQQuoteDeserializer<T> : IPQDeserializer<T> where T : PQLevel0Quote
{
    bool AllowUpdatesCatchup { get; }
    uint SyncRetryMs { get; }

    void UpdateQuote(IBufferContext readContext, T ent, uint sequenceId);

    void PushQuoteToSubscribers(PQSyncStatus syncStatus,
        IPerfLogger? detectionToPublishLatencyTraceLogger = null);

    void ClearSyncRing();
    T ClaimSyncSlotEntry();
    void SwitchSyncState(QuoteSyncState newState);
    bool Synchronize(out uint misMatchedSeqId);
}
