// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.Monitoring.Logging.Diagnostics.Performance;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Serdes.Deserialization.SyncState;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Serdes.Deserialization;

public interface IPQQuoteDeserializer : INotifyingMessageDeserializer,
    IDoublyLinkedListNode<IPQQuoteDeserializer>
{
    ISourceTickerQuoteInfo             Identifier { get; }
    event Action<IPQQuoteDeserializer> ReceivedUpdate;
    event Action<IPQQuoteDeserializer> SyncOk;
    event Action<IPQQuoteDeserializer> OutOfSync;

    void OnReceivedUpdate(IPQQuoteDeserializer quoteDeserializer);
    void OnSyncOk(IPQQuoteDeserializer quoteDeserializer);
    void OnOutOfSync(IPQQuoteDeserializer quoteDeserializer);
    bool HasTimedOutAndNeedsSnapshot(DateTime utcNow);
    bool CheckResync(DateTime utcNow);
}

public interface IPQQuoteDeserializer<T> : IPQQuoteDeserializer, INotifyingMessageDeserializer<T>, IObservable<T>
    where T : class, IPQLevel0Quote
{
    T   PublishedQuote { get; }
    int UpdateQuote(IMessageBufferContext readContext, T ent, uint sequenceId);
}

public interface IPQQuotePublishingDeserializer<T> : IPQQuoteDeserializer<T>
    where T : class, IPQLevel0Quote
{
    bool AllowUpdatesCatchup { get; }
    uint SyncRetryMs         { get; }

    void PushQuoteToSubscribers(PQSyncStatus syncStatus, IPerfLogger? detectionToPublishLatencyTraceLogger = null);

    void ClearSyncRing();
    T    ClaimSyncSlotEntry();
    void SwitchSyncState(QuoteSyncState newState);
    bool Synchronize(out uint misMatchedSeqId);
}
