// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Monitoring.Logging.Diagnostics.Performance;
using FortitudeCommon.Serdes.Binary;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Serdes.Deserialization.SyncState;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Serdes.Deserialization;

public interface IPQQuoteDeserializer<T> : IPQDeserializer<T> where T : class, IPQLevel0Quote, new()
{
    bool AllowUpdatesCatchup { get; }
    uint SyncRetryMs         { get; }

    int UpdateQuote(IMessageBufferContext readContext, T ent, uint sequenceId);

    void PushQuoteToSubscribers(PQSyncStatus syncStatus,
        IPerfLogger? detectionToPublishLatencyTraceLogger = null);

    void ClearSyncRing();
    T    ClaimSyncSlotEntry();
    void SwitchSyncState(QuoteSyncState newState);
    bool Synchronize(out uint misMatchedSeqId);
}
