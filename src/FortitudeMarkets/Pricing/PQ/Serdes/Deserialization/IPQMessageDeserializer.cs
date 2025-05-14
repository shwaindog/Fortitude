// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.Monitoring.Logging.Diagnostics.Performance;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeMarkets.Pricing.FeedEvents;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Messages;
using FortitudeMarkets.Pricing.PQ.Serdes.Deserialization.SyncState;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Serdes.Deserialization;

public interface IPQMessageDeserializer : INotifyingMessageDeserializer,
    IDoublyLinkedListNode<IPQMessageDeserializer>
{
    ISourceTickerInfo Identifier { get; }

    event Action<IPQMessageDeserializer> ReceivedUpdate;
    event Action<IPQMessageDeserializer> SyncOk;
    event Action<IPQMessageDeserializer> OutOfSync;

    void OnReceivedUpdate(IPQMessageDeserializer quoteDeserializer);
    void OnSyncOk(IPQMessageDeserializer quoteDeserializer);
    void OnOutOfSync(IPQMessageDeserializer quoteDeserializer);
    bool HasTimedOutAndNeedsSnapshot(DateTime utcNow);
    bool CheckResync(DateTime utcNow);
}

public interface IPQMessageDeserializer<T> : IPQMessageDeserializer, INotifyingMessageDeserializer<T>, IObservable<T>
    where T : IPQMutableMessage
{
    T   PublishedQuote { get; }
    int UpdateEntity(IMessageBufferContext readContext, T ent, uint sequenceId);
}

public interface IPQMessagePublishingDeserializer<T> : IPQMessageDeserializer<T>
    where T : IPQMutableMessage
{
    bool AllowUpdatesCatchup { get; }

    uint SyncRetryMs { get; }

    void PushQuoteToSubscribers(FeedSyncStatus syncStatus, IPerfLogger? detectionToPublishLatencyTraceLogger = null);

    void ClearSyncRing();
    T    ClaimSyncSlotEntry();
    void SwitchSyncState(QuoteSyncState newState);
    bool Synchronize(out uint misMatchedSeqId);
}
