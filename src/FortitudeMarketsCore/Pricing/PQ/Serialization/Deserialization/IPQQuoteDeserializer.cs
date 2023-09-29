using FortitudeCommon.Monitoring.Logging.Diagnostics.Performance;
using FortitudeIO.Protocols.Serialization;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization.SyncState;

namespace FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization
{
    public interface IPQQuoteDeserializer<T> : IPQDeserializer<T> where T : IPQLevel0Quote
    {
        bool AllowUpdatesCatchup { get; }

        void UpdateQuote(DispatchContext dispatchContext, T ent, uint sequenceId);

        void PushQuoteToSubscribers(PQSyncStatus syncStatus,
            IPerfLogger detectionToPublishLatencyTraceLogger = null);
        void ClearSyncRing();
        T ClaimSyncSlotEntry();
        uint SyncRetryMs { get; }
        void SwitchSyncState(QuoteSyncState newState);
        bool Synchronize(out uint misMatchedSeqId);
    }
}