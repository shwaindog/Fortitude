// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.Monitoring.Logging.Diagnostics.Performance;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeMarkets.Pricing.FeedEvents;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes;
using FortitudeMarkets.Pricing.PQ.Serdes.Deserialization.SyncState;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Serdes.Deserialization;

public interface IPQQuoteDeserializer : INotifyingMessageDeserializer,
    IDoublyLinkedListNode<IPQQuoteDeserializer>
{
    ISourceTickerInfo Identifier { get; }

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
    where T : class, IPQPublishableTickInstant
{
    T   PublishedQuote { get; }
    int UpdateQuote(IMessageBufferContext readContext, T ent, uint sequenceId);
}

public interface IPQQuotePublishingDeserializer<T> : IPQQuoteDeserializer<T>
    where T : class, IPQPublishableTickInstant
{
    bool AllowUpdatesCatchup { get; }

    uint SyncRetryMs { get; }

    void PushQuoteToSubscribers(FeedSyncStatus syncStatus, IPerfLogger? detectionToPublishLatencyTraceLogger = null);

    void ClearSyncRing();
    T    ClaimSyncSlotEntry();
    void SwitchSyncState(QuoteSyncState newState);
    bool Synchronize(out uint misMatchedSeqId);
}
